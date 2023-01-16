import Address from './domain/customer/value-object/address';
import Customer from './domain/customer/entity/customer';
import OrderItem from './domain/checkout/entity/order_item';
import Order from './domain/checkout/entity/order';

let address = new Address('Street Abc', '10A', 'ZIP', 'New York');
let customer = new Customer('123', 'Henrique Holtz', address);

const item1 = new OrderItem('1', 'Item 1', 11, 'p1', 1);
const item2 = new OrderItem('2', 'Item 2', 22, 'p2', 2);
const item3 = new OrderItem('3', 'Item 3', 33, 'p3', 3);

const order = new Order('1', '123', [item1, item2, item3]);
