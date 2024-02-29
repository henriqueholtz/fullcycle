package create_transaction

import (
	"context"

	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/gateway"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/events"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/pkg/uow"
)

type CreateTransactionInputDTO struct {
	AccountIDFrom string `json:"account_id_from"`
	AccountIDTo string `json:"account_id_to"`
	Amount float64 `json:"amount"`
}

type CreateTransactionOutputDTO struct {
	ID string `json:"id"`
	AccountIDFrom string `json:"account_id_from"`
	AccountIDTo string `json:"account_id_to"`
	Amount float64 `json:"amount"`
}

type CreateTransactionUseCase struct {
	Uow uow.UowInterface
	EventDispatcher events.IEventDispatcher
	TransactionCreated events.IEvent
}

func NewCreateTransactionUseCase(
		uow uow.UowInterface,
		eventDispatcher events.IEventDispatcher,
		transactionCreated events.IEvent,
	) *CreateTransactionUseCase {
		return &CreateTransactionUseCase{
		Uow: uow,
		EventDispatcher: eventDispatcher,
		TransactionCreated: transactionCreated,
	}
}

func (uc *CreateTransactionUseCase) Execute(ctx context.Context, input CreateTransactionInputDTO) (*CreateTransactionOutputDTO, error) {
	output := &CreateTransactionOutputDTO{}
	err := uc.Uow.Do(ctx, func(_ *uow.Uow) error {
		accountRepository := uc.getAccountRepository(ctx)
		transactionRepository := uc.getTransactionRepository(ctx)

		accountFrom, err := accountRepository.FindByID(input.AccountIDFrom)
		if err != nil {
			return err
		}

		accountTo, err := accountRepository.FindByID(input.AccountIDTo)
		if err != nil {
			return err
		}
		
		transaction, err := entity.NewTransaction(accountFrom, accountTo, input.Amount)
		if err != nil {
			return err
		}

		err = accountRepository.UpdateBalance(accountFrom)
		if err != nil {
			return err
		}

		err = accountRepository.UpdateBalance(accountTo)
		if err != nil {
			return err
		}
		
		err = transactionRepository.Create(transaction)
		if err != nil {
			return err
		}

		output.ID = transaction.ID
		output.AccountIDFrom = transaction.AccountFrom.ID
		output.AccountIDTo = transaction.AccountTo.ID
		output.Amount = transaction.Amount
		return nil
	})

	if err != nil {
		return nil, err
		// uc.Uow.Rollback()
	}

	uc.TransactionCreated.SetPayload(output)
	uc.EventDispatcher.Dispatch(uc.TransactionCreated)

	return output, nil
}

func (uc *CreateTransactionUseCase) getAccountRepository(ctx context.Context) gateway.AccountGateway {
	repo, err := uc.Uow.GetRepository(ctx, "AccountDB")
	if err != nil {
		panic(err)
	}
	return repo.(gateway.AccountGateway)
}

func (uc *CreateTransactionUseCase) getTransactionRepository(ctx context.Context) gateway.TransactionGateway {
	repo, err := uc.Uow.GetRepository(ctx, "TransactionDB")
	if err != nil {
		panic(err)
	}
	return repo.(gateway.TransactionGateway)
}