package create_account

import (
	"github.com/henriqueholtz/fullcycle/fc30/architecture-microservices-based/ms/walletcore/internal/entity"
	"github.com/henriqueholtz/fullcycle/fc30/architecture-microservices-based/ms/walletcore/internal/gateway"
)

type CreateAccountInputDto struct {
	ClientID string
}

type CreateAccountOutputDto struct {
	ID string
}

type CreateAccountUseCase struct {
	AccountGateway gateway.AccountGateway
	ClientGateway gateway.ClientGateway
}

func NewCreateAccountUseCase(a gateway.AccountGateway, c gateway.ClientGateway) *CreateAccountUseCase {
	return &CreateAccountUseCase{
		AccountGateway: a,
		ClientGateway: c,
	}
}

func (uc *CreateAccountUseCase) Execute(input CreateAccountInputDto) (*CreateAccountOutputDto, error){
	client, err := uc.ClientGateway.Get(input.ClientID)
	if err != nil {
		return nil, err
	}

	account := entity.NewAccount(client)
	err = uc.AccountGateway.Save(account)
	if err != nil {
		return nil, err
	}

	return &CreateAccountOutputDto{
		ID: account.ID,
	}, nil
}