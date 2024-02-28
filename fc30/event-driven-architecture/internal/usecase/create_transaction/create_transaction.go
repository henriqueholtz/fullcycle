package create_transaction

import (
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/gateway"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/events"
)

type CreateTransactionInputDTO struct {
	AccountIDFrom string `json:"account_id_from"`
	AccountIDTo string `json:"account_id_to"`
	Amount float64 `json:"amount"`
}

type CreateTransactionOutputDTO struct {
	ID string `json:"id"`
}

type CreateTransactionUseCase struct {
	TransactionGateway gateway.TransactionGateway
	AccountGateway gateway.AccountGateway
	EventDispatcher events.IEventDispatcher
	TransactionCreated events.IEvent
}

func NewCreateTransactionUseCase(
		transactionGateway gateway.TransactionGateway,
		accountGateway gateway.AccountGateway,
		eventDispatcher events.IEventDispatcher,
		transactionCreated events.IEvent,
	) *CreateTransactionUseCase {
		return &CreateTransactionUseCase{
		TransactionGateway: transactionGateway,
		AccountGateway: accountGateway,
		EventDispatcher: eventDispatcher,
		TransactionCreated: transactionCreated,
	}
}

func (uc *CreateTransactionUseCase) Execute(input CreateTransactionInputDTO) (*CreateTransactionOutputDTO, error) {
	accountFrom, err := uc.AccountGateway.FindByID(input.AccountIDFrom)
	if err != nil {
		return nil, err
	}

	accountTo, err := uc.AccountGateway.FindByID(input.AccountIDTo)
	if err != nil {
		return nil, err
	}
	
	transaction, err := entity.NewTransaction(accountFrom, accountTo, input.Amount)
	if err != nil {
		return nil, err
	}
	
	err = uc.TransactionGateway.Create(transaction)
	if err != nil {
		return nil, err
	}

	output := &CreateTransactionOutputDTO{
		ID: transaction.ID,
	}

	uc.TransactionCreated.SetPayload(output)
	uc.EventDispatcher.Dispatch(uc.TransactionCreated)

	return output, nil
}