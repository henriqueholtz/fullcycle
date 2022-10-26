import Address from './address';
import Order from './order';
import OrderItem from './order_item';

describe('Order unit tests', () => {
  const item1: OrderItem = new OrderItem('1', 'Item 1', 11.0, 'p1', 1);
  const item2: OrderItem = new OrderItem('2', 'Item 2', 22.0, 'p2', 2);
  const items: OrderItem[] = [item1, item2];

  it('Should throw error when id is empty', () => {
    expect(() => {
      let order = new Order('', '1', []);
    }).toThrowError('Id is required!');
  });

  it('Should throw error when customerId is empty', () => {
    expect(() => {
      let order = new Order('1', '', []);
    }).toThrowError('customerId is required!');
  });

  it('Should calculate total correctly', () => {
    let order = new Order('1', '1', [item1]);
    expect(order.total()).toBe(11);

    order = new Order('1', '1', items);
    expect(order.total()).toBe(55);
  });
});
