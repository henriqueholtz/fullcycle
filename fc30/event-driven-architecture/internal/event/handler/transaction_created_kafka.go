package handler

import (
	"fmt"
	"sync"

	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/events"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/kafka"
)

type TransactionCreatedKafkaHandler struct {
	Kafka *kafka.Producer
}

func NewTransactionCreatedKafkaHandler(kafka *kafka.Producer) *TransactionCreatedKafkaHandler {
	return &TransactionCreatedKafkaHandler{
		Kafka: kafka,
	}
}

func (h *TransactionCreatedKafkaHandler) Handle(message events.IEvent, wg *sync.WaitGroup) {
	defer wg.Done()

	h.Kafka.Publish(message, nil, "transactions")
	fmt.Println("TransactionCreatedKafkaHandler: ", message.GetPayload())
}