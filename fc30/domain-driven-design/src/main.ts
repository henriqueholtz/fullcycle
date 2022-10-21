import Customer from './entity/customer';
import Address from './entity/address';
import OrderItem from './entity/order_item';
import Order from './entity/order';

let address = new Address('Street Abc', '10A', 'New York');
let customer = new Customer('123', 'Henrique Holtz', address);

const item1 = new OrderItem('1', 'Item 1', 11);
const item2 = new OrderItem('2', 'Item 2', 22);
const item3 = new OrderItem('3', 'Item 3', 33);

const order = new Order('1', '123', [item1, item2, item3]);
