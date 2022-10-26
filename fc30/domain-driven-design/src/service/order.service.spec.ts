import Order from '../entity/order';
import OrderItem from '../entity/order_item';
import OrderService from './order.service';

describe('Order service unit tests', () => {
  it('Should get total (price) of orders', () => {
    const orderItem1 = new OrderItem('1', 'name 1', 10, 'p1', 1);
    const orderItem2 = new OrderItem('2', 'name 2', 20, 'p2', 2);
    const order1 = new Order('p1', 'c1', [orderItem1]);
    const order2 = new Order('p2', 'c2', [orderItem2]);

    const total = OrderService.getTotal([order1, order2]);
    expect(total).toBe(50);
  });
});
