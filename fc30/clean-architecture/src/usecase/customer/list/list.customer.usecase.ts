import Customer from '../../../domain/customer/entity/customer';
import ICustomerRepository from '../../../domain/customer/repository/customer-repository.interface';
import {
  InputListCustomerDto,
  OutputListCustomerDto,
} from './list.customer.dto';

export default class ListCustomerUseCase {
  private customerRepository: ICustomerRepository;
  constructor(CustomerRepository: ICustomerRepository) {
    this.customerRepository = CustomerRepository;
  }

  async execute(input: InputListCustomerDto): Promise<OutputListCustomerDto> {
    const customers = await this.customerRepository.findAll();
    return OutputMapper.toOutput(customers);
  }
}

class OutputMapper {
  static toOutput(customers: Customer[]): OutputListCustomerDto {
    return {
      customers: customers.map((customer) => ({
        id: customer.id,
        name: customer.name,
        address: {
          street: customer.Address.street,
          number: customer.Address.number,
          zip: customer.Address.zip,
          city: customer.Address.city,
        },
      })),
    };
  }
}
