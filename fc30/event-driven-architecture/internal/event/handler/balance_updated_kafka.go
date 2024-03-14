package handler

import (
	"fmt"
	"sync"

	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/events"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/kafka"
)

type UpdateBalanceKafkaHandler struct {
	Kafka *kafka.Producer
}

func NewUpdateBalanceKafkaHandler(kafka *kafka.Producer) *UpdateBalanceKafkaHandler {
	return &UpdateBalanceKafkaHandler{
		Kafka: kafka,
	}
}

func (h *UpdateBalanceKafkaHandler) Handle (message events.IEvent, wg *sync.WaitGroup) {
	defer wg.Done()
	h.Kafka.Publish(message, nil, "balances")
	fmt.Println("UpdateBalanceKafkaHandler called...")
}