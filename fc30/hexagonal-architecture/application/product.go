package application

import (
	"errors"
	"github.com/asaskevich/govalidator"
	uuid "github.com/satori/go.uuid"
)

func init() {
	govalidator.SetFieldsRequiredByDefault(true)
}

type IProduct interface {
	IsValid() (bool, error)
	Enable() error
	Disable() error
	GetId() string
	GetName() string
	GetStatus() string
	GetPrice() float64
}

type IProductService interface {
	Get(id string) (IProduct, error)
	Create(name string, price float64) (IProduct, error)
	Enable(product IProduct) (IProduct, error)
	Disable(product IProduct) (IProduct, error)
}

type ProductReader interface {
	Get(id string) (IProduct, error)
}

type ProductWriter interface {
	Save(product IProduct) (IProduct, error)
}

type IProductPersistence interface {
	ProductReader
	ProductWriter
}

const (
	DISABLED = "disabled"
	ENABLED = "enabled"
)

type Product struct {
	ID string `valid:"uuidv4"`
	Name string `valid:"required"`
	Status string `valid:"required"`
	Price float64 `valid:"float,optional"`
}

func NewProduct() *Product {
	product := Product{
		ID:     uuid.NewV4().String(),
		Status: DISABLED,
	}
	return &product
}

func (p *Product) IsValid() (bool, error) {
	if p.Status == "" {
		p.Status = DISABLED
	}

	if p.Status != ENABLED && p.Status != DISABLED {
		return false, errors.New("The status is invalid.")
	}
	
	if p.Price < 0 {
		return false, errors.New("The price is invalid.")
	}

	_, err := govalidator.ValidateStruct(p)
	if err != nil {
		return false, err
	}

	return true, nil
}

func (p *Product) Enable() error {
	if p.Price > 0 {
		p.Status = ENABLED
		return nil
	}

	return errors.New("The price must be greater than zero to enable the product.")
}

func (p *Product) Disable() error {
	if p.Price == 0 {
		p.Status = DISABLED
		return nil
	}

	return errors.New("The price must be zero in order to have the product disabled.")
}

func (p *Product) GetId() string {
	return p.ID
}

func (p *Product) GetName() string {
	return p.Name
}

func (p *Product) GetStatus() string {
	return p.Status
}

func (p *Product) GetPrice() float64 {
	return p.Price
}