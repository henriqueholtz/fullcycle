package main

import (
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"sync"
	"time"

	"github.com/confluentinc/confluent-kafka-go/v2/kafka"
	_ "github.com/go-sql-driver/mysql"
	"github.com/gorilla/mux"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/database"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/uow"
)

type KafkaMessageTransaction struct {
	Name string
	Payload KafkaEventTransactionPayloadDTO
}

type KafkaEventTransactionPayloadDTO struct {
	ClientId string `json:"id"`
	AccountIDFrom string `json:"account_id_from"`
	AccountIDTo string `json:"account_id_to"`
	Amount float64 `json:"amount"`
}

type KafkaMessageBalance struct {
	Name string
	Payload KafkaEventBalancePayloadDTO
}

type KafkaEventBalancePayloadDTO struct {
	AccountIDFrom string `json:"account_id_from"`
	AccountIDTo string `json:"account_id_to"`
	BalanceAccountIdFrom float64 `json:"balance_account_id_from"`
	BalanceAccountIdTo float64 `json:"balance_account_id_to"`
}

func main() {
	fmt.Println("Starting...")
	db, err := sql.Open("mysql", fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?charset=utf8&parseTime=True&loc=Local", "root", "root", "mysql-reflected", "3306", "wallet-reflected"))
	if err != nil {
		panic(err)
	}
	defer db.Close()

	ctx := context.Background()
	uow := uow.NewUow(ctx, db)

	uow.Register("AccountDB", func(tx *sql.Tx) interface{} {
		return database.NewAccountDB(db)
	})

	uow.Register("TransactionDB", func(tx *sql.Tx) interface{} {
		return database.NewTransactionDB(db)
	})

	var wg sync.WaitGroup
	wg.Add(2)
	go webServer(db, &wg)
	go kafkaConsumer(db, &wg)
	wg.Wait()
}

type BalanceDependencies struct {
	Db *sql.DB
}

func (balanceDependencies *BalanceDependencies) returnBalance(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
    account_id := vars["account_id"]
    fmt.Printf("Endpoint Hit: %s\n", account_id)
	accountDb := database.NewAccountDB(balanceDependencies.Db)
	account, err := accountDb.FindByID(account_id)
	if err != nil {
		fmt.Println(err)
		json.NewEncoder(w).Encode(nil)
		http.NotFound(w, r)
	}
	
	json.NewEncoder(w).Encode(account)
}

func webServer(db *sql.DB, wg *sync.WaitGroup) {
	defer wg.Done()
    myRouter := mux.NewRouter().StrictSlash(true)
	balanceDependencies := &BalanceDependencies{
		Db: db,
	}
	myRouter.HandleFunc("/balances/{account_id}", balanceDependencies.returnBalance)

    log.Fatal(http.ListenAndServe(":3003", myRouter))
	fmt.Println("Server is running at 3003 port")
}

func kafkaConsumer(db *sql.DB, wg *sync.WaitGroup) {
	defer wg.Done()
	c, err := kafka.NewConsumer(&kafka.ConfigMap{
		"bootstrap.servers": "kafka:29092",
		"group.id":          "wallet-reflected",
	})

	if err != nil {
		panic(err)
	}

	fmt.Println("Subscribing kafka topics...")
	c.SubscribeTopics([]string{"transactions", "balances"}, nil)
	accountDb := database.NewAccountDB(db)
	transactionDb := database.NewTransactionDB(db)
	run := true

	for run {
		fmt.Println("Reading a message...")
		msg, err := c.ReadMessage(time.Second)

		if err == nil {
			fmt.Println("Message has been readed...")
			fmt.Printf("Message on %s: %s\n", *msg.TopicPartition.Topic, string(msg.Value))
			if (*msg.TopicPartition.Topic == "transactions") {
				kafkaMessage := &KafkaMessageTransaction{}
				errJson := json.Unmarshal(msg.Value, kafkaMessage)

				if errJson != nil {
					panic(errJson)
				}
				
				accountFrom, dbErr := accountDb.FindByID(kafkaMessage.Payload.AccountIDFrom)
				if dbErr != nil {
					panic(dbErr)
				}
				if accountFrom == nil {
					panic("AccountFrom does not exist!")
				}

				accountTo, dbErr := accountDb.FindByID(kafkaMessage.Payload.AccountIDTo)
				if dbErr != nil {
					panic(dbErr)
				}
				if accountTo == nil {
					panic("AccountTo does not exist!")
				}

				fmt.Printf("[transactions] Accounts from and to ok...")
				fmt.Printf("Transaction Amount: %f\n", kafkaMessage.Payload.Amount)
				transaction, transactionErr := entity.NewTransaction(accountFrom, accountTo, kafkaMessage.Payload.Amount)
				if transactionErr != nil {
					panic(transactionErr)
				}

				transactionErr = transactionDb.Create(transaction)
				if transactionErr != nil {
					panic(transactionErr)
				}
				fmt.Println("[transactions] Successfuly completed...")
				fmt.Println("[transactions] -------------")
			} else if (*msg.TopicPartition.Topic == "balances") {
				
				kafkaMessage := &KafkaMessageBalance{}
				errJson := json.Unmarshal(msg.Value, kafkaMessage)
				if errJson != nil {
					panic(errJson)
				}
				accountFrom, dbErr := accountDb.FindByID(kafkaMessage.Payload.AccountIDFrom)
				if dbErr != nil {
					panic(dbErr)
				}
				if accountFrom == nil {
					panic("AccountFrom does not exist!")
				}

				accountTo, dbErr := accountDb.FindByID(kafkaMessage.Payload.AccountIDTo)
				if dbErr != nil {
					panic(dbErr)
				}
				if accountTo == nil {
					panic("AccountTo does not exist!")
				}
				fmt.Printf("[balances] Accounts from and to ok...")

				accountFrom.Balance = kafkaMessage.Payload.BalanceAccountIdFrom
				accountDb.UpdateBalance(accountFrom)

				accountTo.Balance = kafkaMessage.Payload.BalanceAccountIdTo
				accountDb.UpdateBalance(accountTo)
				fmt.Println("[balances] Successfuly completed...")
				fmt.Println("[balances] -------------")
			}
		} else if !err.(kafka.Error).IsTimeout() {
			fmt.Printf("Consumer error: %v (%v)\n", err, msg)
			fmt.Println("-------------")
		}
	}
	fmt.Println("Before close")
	c.Close()
	fmt.Println("After close")
}