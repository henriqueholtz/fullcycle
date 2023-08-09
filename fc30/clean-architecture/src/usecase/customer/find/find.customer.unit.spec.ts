import Customer from '../../../domain/customer/entity/customer';
import Address from '../../../domain/customer/value-object/address';
import FindCustomerUseCase from './find.customer.usecase';

const address = new Address('Street', '200', 'Zip', 'City');
const customer = new Customer('123', 'John', address);

const MockRepository = () => {
  return {
    find: jest.fn().mockReturnValue(Promise.resolve(customer)),
    findAll: jest.fn(),
    create: jest.fn(),
    update: jest.fn(),
  };
};

describe('Unit Test find customer use case', () => {
  it('should find a customer', async () => {
    const customerRepository = MockRepository();
    const usecase = new FindCustomerUseCase(customerRepository);

    const input = {
      id: '123',
    };

    const output = {
      id: '123',
      name: 'John',
      address: {
        street: 'Street',
        city: 'City',
        number: '200',
        zip: 'Zip',
      },
    };

    const result = await usecase.execute(input);

    expect(result).toEqual(output);
  });

  it('should not find a customer', async () => {
    const customerRepository = MockRepository();
    const errorMessage = 'Customer not found';
    customerRepository.find.mockImplementation(() => {
      throw new Error(errorMessage);
    });
    const usecase = new FindCustomerUseCase(customerRepository);

    const input = {
      id: '123',
    };

    expect(() => {
      return usecase.execute(input);
    }).rejects.toThrow(errorMessage);
  });
});
