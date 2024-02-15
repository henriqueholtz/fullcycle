package create_client

import (
	"github.com/henriqueholtz/fullcycle/fc30/architecture-microservices-based/ms/walletcore/internal/entity"
	"github.com/henriqueholtz/fullcycle/fc30/architecture-microservices-based/ms/walletcore/internal/gateway"
	"time"
)


type CreateClientInputDto struct {
	Name string
	Email string
}

type CreateClientOututDto struct {
	ID string
	Name string
	Email string
	CreatedAt time.Time
	UpdatedAt time.Time
}

type CreateClientUseCase struct {
	ClientGateway gateway.ClientGateway
}

func NewCreateClientUseCase(clientGateway gateway.ClientGateway) *CreateClientUseCase {
	return &CreateClientUseCase{
		ClientGateway: clientGateway,
	}
}

func (uc *CreateClientUseCase) Execute(input CreateClientInputDto) (*CreateClientOututDto, error) {
	client, err := entity.NewClient(input.Name, input.Email)
	if err != nil {
		return nil, err
	}
	
	err = uc.ClientGateway.Save(client)
	if err != nil {
		return nil, err
	}

	return &CreateClientOututDto{
		ID: client.ID,
		Name: client.Name,
		Email: client.Email,
		CreatedAt: client.CreatedAt,
		UpdatedAt: client.UpdatedAt,
	}, nil
}