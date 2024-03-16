package main

import (
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"time"

	"github.com/confluentinc/confluent-kafka-go/v2/kafka"
	_ "github.com/go-sql-driver/mysql"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/database"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/usecase/create_client"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/web"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/web/webserver"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/uow"
)
type KafkaMessage struct {
	Name string
	Payload BalanceUpdatedOutputDTO
}

type BalanceUpdatedOutputDTO struct {
	AccountIDFrom string `json:"account_id_from"`
	AccountIDTo string `json:"account_id_to"`
	BalanceAccountIdFrom float64 `json:"balance_account_id_from"`
	BalanceAccountIdTo float64 `json:"balance_account_id_to"`
}

func main() {
	fmt.Println("Starting...")
	db, err := sql.Open("mysql", fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?charset=utf8&parseTime=True&loc=Local", "root", "root", "mysql-reflected", "3307", "wallet-reflected"))
	if err != nil {
		panic(err)
	}
	defer db.Close()

	clientDb := database.NewClientDB(db)
	// accountDb := database.NewAccountDB(db)

	ctx := context.Background()
	uow := uow.NewUow(ctx, db)

	uow.Register("AccountDB", func(tx *sql.Tx) interface{} {
		return database.NewAccountDB(db)
	})

	uow.Register("TransactionDB", func(tx *sql.Tx) interface{} {
		return database.NewTransactionDB(db)
	})
	// createTransactionUseCase := create_transaction.NewCreateTransactionUseCase(uow, eventDispatcher, transactionCreatedEvent, balanceUpdatedEvent)
	createClientUseCase := create_client.NewCreateClientUseCase(clientDb)
	// createAccountUseCase := create_account.NewCreateAccountUseCase(accountDb, clientDb)

	webserver := webserver.NewWebServer(":3003")

	clientHandler := web.NewWebClientHandler(*createClientUseCase)
	// accountHandler := web.NewWebAccountHandler(*createAccountUseCase)
	// transactionHandler := web.NewWebTransactionHandler(*createTransactionUseCase)

	c, err := kafka.NewConsumer(&kafka.ConfigMap{
		"bootstrap.servers": "localhost:9092", //"kafka:29092", // 
		"group.id":          "wallet-reflected",
	})

	if err != nil {
		panic(err)
	}

	fmt.Println("Subscribing kafka topics...")
	c.SubscribeTopics([]string{"balances"}, nil)

	// A signal handler or similar could be used to set this to false to break the loop.
	run := true

	for run {
		fmt.Println("Reading a message...")
		msg, err := c.ReadMessage(time.Second)
		fmt.Println("Message has been readed...")
		if err == nil {
			fmt.Printf("Message on %s: %s\n", msg.TopicPartition, string(msg.Value))
			kafkaMessage := &KafkaMessage{}
			errJson := json.Unmarshal(msg.Value, kafkaMessage)
			if errJson != nil {
				panic(errJson)
			}

			fmt.Printf("AccountIDFrom: %s; AccountIDTo: %s\n", kafkaMessage.Payload.AccountIDFrom, kafkaMessage.Payload.AccountIDTo)

		} else if err.(kafka.Error).IsTimeout() {
			fmt.Println("Timeout...")
		} else {
			fmt.Printf("Consumer error: %v (%v)\n", err, msg)
		}
		fmt.Printf("-------------")
	}
	fmt.Println("Before close")
	
	c.Close()
	fmt.Println("After close")

	webserver.AddHandler("/balances/{account_id}", clientHandler.CreateClient)

	fmt.Println("Server is running at 3003 port")
	webserver.Start()
}
