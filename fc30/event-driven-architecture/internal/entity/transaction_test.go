package entity

import (
	"testing"

	"github.com/stretchr/testify/assert"
)


func TestCreateTransactionSuccessfully(t *testing.T) {
	client, _ := NewClient("John Doe", "j@j.com")
	account := NewAccount(client)

	client2, _ := NewClient("John Doe 2", "j2@j.com")
	account2 := NewAccount(client2)

	account.Credit(1000)
	account2.Credit(500)
	
	transaction, err := NewTransaction(account, account2, 150)

	assert.Nil(t, err)
	assert.NotNil(t, transaction)
	assert.Equal(t, account.Balance, 850.0)
	assert.Equal(t, account2.Balance, 650.0)
}


func TestCreateTransactionWithoutSufficientFunds(t *testing.T) {
	client, _ := NewClient("John Doe", "j@j.com")
	account := NewAccount(client)

	client2, _ := NewClient("John Doe 2", "j2@j.com")
	account2 := NewAccount(client2)

	account.Credit(1000)
	account2.Credit(500)
	
	transaction, err := NewTransaction(account, account2, 1500)

	assert.NotNil(t, err)
	assert.Error(t, err, "Insufficient funds!")
	assert.Nil(t, transaction)
	assert.Equal(t, account.Balance, 1000.0)
	assert.Equal(t, account2.Balance, 500.0)
}