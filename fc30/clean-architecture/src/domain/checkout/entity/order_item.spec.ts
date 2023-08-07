import OrderItem from './order_item';

describe('OrderItem unit tests', () => {
  it('Should throw error when id is empty', () => {
    expect(() => {
      let order = new OrderItem('', 'Name', 11, 'p1', 1);
    }).toThrowError('Id is required!');
  });

  it('Should throw error when quantity is less or equals to zero', () => {
    expect(() => {
      let order = new OrderItem('1', 'Name', 11, 'p1', 0);
    }).toThrowError('Quantity is required!');
    expect(() => {
      let order = new OrderItem('1', 'Name', 11, 'p1', -1);
    }).toThrowError('Quantity is required!');
  });
});
