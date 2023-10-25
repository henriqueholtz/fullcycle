package gateway

import (
	"github.com/henriqueholtz/fullcycle/fc30/architecture-microservices-based/ms/walletcore/internal/entity"
)


type ClientGateway interface {
	Get(id string) (*entity.Client, error)
	Save(client *entity.Client) error
}