import Address from './address';
import Customer from './customer';

describe('Customer unit tests', () => {
  const address = new Address('street', '10A', 'New York');
  it('Should throw error when id is empty', () => {
    expect(() => {
      let customer = new Customer('', 'Jhon', address);
    }).toThrowError('Id is required!');
  });

  it('Should throw error when id is empty', () => {
    expect(() => {
      let customer = new Customer('123', '', address);
    }).toThrowError('Name is required!');
  });

  it('Should change name correctly', () => {
    const newName: string = 'Jane';
    let customer = new Customer('123', 'Jhon', address);
    customer.changeName(newName);
    expect(customer.name).toBe(newName);
  });

  it('Should activate customer', () => {
    let customer = new Customer('123', 'Jhon', address);
    customer.activate();
    expect(customer.isActive()).toBe(true);
  });

  it('Should deactivate customer', () => {
    let customer = new Customer('123', 'Jhon', address);
    customer.deactivate();
    expect(customer.isActive()).toBe(false);
  });

  it('Should throw error when Address is undefined when activating a customer', () => {
    expect(() => {
      let customer = new Customer('123', 'Jhon', undefined);
      customer.activate();
    }).toThrowError('Address is required to activate the customer!');
  });

  it('Should add reward points', () => {
    let customer = new Customer('123', 'Jhon', undefined);
    expect(customer.rewardPoints).toBe(0);

    const additionalRewardPoints: number = 10;
    customer.addRewardPoints(additionalRewardPoints);
    expect(customer.rewardPoints).toBe(additionalRewardPoints);

    customer.addRewardPoints(additionalRewardPoints);
    expect(customer.rewardPoints).toBe(additionalRewardPoints * 2);
  });
});
