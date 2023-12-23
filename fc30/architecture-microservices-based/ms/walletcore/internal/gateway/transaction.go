package gateway

import "github.com/henriqueholtz/fullcycle/fc30/architecture-microservices-based/ms/walletcore/internal/entity"

type TransactionGateway interface {
	Create(transaction *entity.Transaction) error
}