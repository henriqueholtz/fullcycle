package gateway

import "github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"

type AccountGateway interface {
	Save(account *entity.Account) error
	FindByID(id string) (*entity.Account, error)
}