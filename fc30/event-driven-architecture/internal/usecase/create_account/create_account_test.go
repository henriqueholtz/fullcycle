package create_account

import (
	"testing"

	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"
	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/usecase/mocks"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/mock"
)

func TestCreateAccountUseCase_Execute(t *testing.T) {
	client, _ := entity.NewClient("John Doe", "j@j")
	clientMock := &mocks.ClientGatewayMock{}
	clientMock.On("Get", client.ID).Return(client, nil)
	
	accountMock := &mocks.AccountGatewayMock{}
	accountMock.On("Save", mock.Anything).Return(nil)
	
	uc := NewCreateAccountUseCase(accountMock, clientMock)
	inputDto := CreateAccountInputDto{
		ClientID: client.ID,
	}
	output, err := uc.Execute(inputDto)
	assert.Nil(t, err)
	assert.NotNil(t, output.ID)
	clientMock.AssertExpectations(t)
	clientMock.AssertNotCalled(t, "Get", 1)
	accountMock.AssertExpectations(t)
	accountMock.AssertNumberOfCalls(t, "Save", 1)
}