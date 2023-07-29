package application_test

import (
	"testing"
	"github.com/stretchr/testify/require"
	"github.com/henriqueholtz/fullcycle/fc30/hexagonal-architecture/application"
)

func TestProduct_Enable(t *testing.T) {
	product := application.Product{}
	product.Name = "Laptop"
	product.Status = application.DISABLED
	product.Price = 3500

	// Can be enabled
	err := product.Enable()
	require.Nil(t, err)

	// Can't be enabled because the price is 0.
	product.Price = 0
	err = product.Enable()
	require.Equal(t, "The price must be greater than zero to enable the product.", err.Error())
}

func TestProduct_Disable(t *testing.T) {
	product := application.Product{}
	product.Name = "Laptop"
	product.Status = application.ENABLED
	product.Price = 0

	// Can be disabled
	err := product.Disable()
	require.Nil(t, err)

	// Can't be disabled
	// product.Price = 3500
	// err = product.Enable()
	// require.Equal(t, "The price must be zero in order to have the product disabled.", err.Error())

}