import { v4 as uuid } from 'uuid';
import Customer from '../customer';
import Address from '../../value-object/address';

export default class CustomerFactory {
  public static create(name: string): Customer {
    return new Customer(uuid(), name, undefined);
  }

  public static createWithAddress(name: string, address: Address): Customer {
    const customer = new Customer(uuid(), name, address);
    return customer;
  }
}
