import Customer from '../../customer/entity/customer';
import Order from '../entity/order';
import OrderItem from '../entity/order_item';
import OrderService from './order.service';

describe('Order service unit tests', () => {
  const orderItem1 = new OrderItem('1', 'name 1', 10, 'p1', 1);
  const orderItem2 = new OrderItem('2', 'name 2', 20, 'p2', 2);

  it('Should throw error when no have items to place new Order with reward points', () => {
    expect(() => {
      const customer = new Customer('1', 'c1', undefined);
      const order = OrderService.placeOrderWithRewardPoints(customer, []);
    }).toThrowError('Should at least have one item!');
  });

  it('Should place an order with reward points', () => {
    const customer = new Customer('1', 'c1', undefined);
    const order = OrderService.placeOrderWithRewardPoints(customer, [
      orderItem1,
    ]);

    expect(customer.rewardPoints).toBe(5);
    expect(order.total()).toBe(10);
  });

  it('Should get total (price) of orders', () => {
    const order1 = new Order('p1', 'c1', [orderItem1]);
    const order2 = new Order('p2', 'c2', [orderItem2]);

    const total = OrderService.getTotal([order1, order2]);
    expect(total).toBe(50);
  });
});
