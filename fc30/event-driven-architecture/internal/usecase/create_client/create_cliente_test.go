package create_client

import (
	"testing"

	"github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/internal/entity"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/mock"
)

type ClientGatewayMock struct {
	mock.Mock 
}

func (m *ClientGatewayMock) Get(id string) (*entity.Client, error) {
	args:= m.Called(id)
	return args.Get(0).(*entity.Client), args.Error(1)
}

func (m *ClientGatewayMock) Save(client *entity.Client) error {
	args:= m.Called(client)
	return args.Error(0)
}

func TestCreateClientUseCase_Execute(t *testing.T) {
	m := &ClientGatewayMock{}
	m.On("Save", mock.Anything).Return(nil)
	uc := NewCreateClientUseCase(m)

	output, err := uc.Execute(CreateClientInputDto{
		Name: "John Doe",
		Email: "j@j.com",
	})
	assert.Nil(t, err)
	assert.NotNil(t, output)
	assert.Equal(t, "John Doe", output.Name)
	assert.Equal(t, "j@j.com", output.Email)
	m.AssertExpectations(t)
	m.AssertNumberOfCalls(t, "Save", 1)
}