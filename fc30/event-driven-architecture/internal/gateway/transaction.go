package gateway

import "github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"

type TransactionGateway interface {
	Create(transaction *entity.Transaction) error
}