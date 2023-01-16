import { v4 as uuidV4 } from 'uuid';
import Order from '../entity/order';
import OrderItem from '../entity/order_item';
import Customer from '../../customer/entity/customer';

export default class OrderService {
  static getTotal(orders: Order[]) {
    // let currentTotal: number = 0;
    // orders.forEach((order) => {
    //   currentTotal += order.total();
    // });
    // return currentTotal;
    return orders?.reduce((acc, order) => acc + order.total(), 0);
  }

  static placeOrderWithRewardPoints(
    customer: Customer,
    items: OrderItem[]
  ): Order {
    if (items.length === 0) throw new Error('Should at least have one item!');
    const order = new Order(uuidV4(), customer.id, items);
    customer.addRewardPoints(order.total() / 2);
    return order;
  }
}
