import Order from '../entity/order';

export default class OrderService {
  static getTotal(orders: Order[]) {
    // let currentTotal: number = 0;
    // orders.forEach((order) => {
    //   currentTotal += order.total();
    // });
    // return currentTotal;
    return orders?.reduce((acc, order) => acc + order.total(), 0);
  }
}
