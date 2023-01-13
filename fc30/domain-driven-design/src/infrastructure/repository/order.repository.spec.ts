import { Sequelize } from 'sequelize-typescript';
import CustomerModel from '../db/sequelize/model/customer.model';
import ProductModel from '../db/sequelize/model/product.model';
import OrderItemModel from '../db/sequelize/model/order_item.model';
import OrderModel from '../db/sequelize/model/order.model';
import CustomerRepository from './customer.repository';
import Customer from '../../domain/entity/customer';
import Address from '../../domain/entity/address';
import ProductRepository from './product.repository';
import Product from '../../domain/entity/product';
import OrderItem from '../../domain/entity/order_item';
import Order from '../../domain/entity/order';
import OrderRepository from './order.repository';

const address = new Address('Street 1', '1A', '85950000', 'Palotina');
const address2 = new Address('Street 2', '2A', '85950222', 'Palotina2');
const customer = new Customer('C1', 'Henrique', address);
const customer2 = new Customer('C2', 'enrique', address2);
const product = new Product('P1', 'Product 1', 15);
const product2 = new Product('P2', 'Product 2', 12);
const orderItem = new OrderItem(
  '1',
  product.name,
  product.price,
  product.id,
  2
);
const orderItem2 = new OrderItem(
  '2',
  product2.name,
  product2.price,
  product2.id,
  2
);
const order = new Order('O1', customer.id, [orderItem]);
const order2 = new Order('O2', customer2.id, [orderItem2]);

describe('Customer repository test', () => {
  let sequelize: Sequelize;

  beforeEach(async () => {
    sequelize = new Sequelize({
      dialect: 'sqlite',
      storage: ':memory:',
      logging: false,
      sync: { force: true },
    });

    await sequelize.addModels([
      CustomerModel,
      ProductModel,
      OrderItemModel,
      OrderModel,
    ]);
    await sequelize.sync();
  });

  afterEach(async () => {
    await sequelize.close();
  });

  it('Should create a new order', async () => {
    const customerRepository = new CustomerRepository();
    await customerRepository.create(customer);

    const productRepository = new ProductRepository();
    await productRepository.create(product);

    const orderRepository = new OrderRepository();
    await orderRepository.create(order);

    const orderModel = await OrderModel.findOne({
      where: { id: order.id },
      include: ['items'],
    });

    expect(orderModel.toJSON()).toStrictEqual({
      id: 'O1',
      customer_id: 'C1',
      total: order.total(),
      items: [
        {
          id: orderItem.id,
          name: orderItem.name,
          price: orderItem.price,
          quantity: orderItem.quantity,
          order_id: 'O1',
          product_id: 'P1',
        },
      ],
    });
  });

  it('should update an order', async () => {
    const customerRepository = new CustomerRepository();
    await customerRepository.create(customer);

    const productRepository = new ProductRepository();
    await productRepository.create(product);

    const orderRepository = new OrderRepository();
    await orderRepository.create(order);

    await orderRepository.update(order);
    var items = await OrderItemModel.findAll();
    const orderModel = await OrderModel.findOne({
      where: { id: order.id },
      include: [{ model: OrderItemModel }],
    });

    expect(orderModel.toJSON()).toStrictEqual({
      id: order.id,
      customer_id: order.customerId,
      total: order.total(),
      items: [
        {
          id: orderItem.id,
          name: orderItem.name,
          price: orderItem.price,
          quantity: orderItem.quantity,
          order_id: order.id,
          product_id: product.id,
        },
      ],
    });
  });

  it('should find a order', async () => {
    const customerRepository = new CustomerRepository();
    await customerRepository.create(customer);

    const productRepository = new ProductRepository();
    await productRepository.create(product);

    const orderRepository = new OrderRepository();
    await orderRepository.create(order);

    const orderResult = await orderRepository.find(order.id);

    // @equipe FullCycle algum proobleam em usar o próprios obj "order" ?
    expect(orderResult).toStrictEqual(order);
  });

  it('should throw an error when order is not found', async () => {
    const orderRepository = new OrderRepository();

    expect(async () => {
      await orderRepository.find('456ABC');
    }).rejects.toThrow('Order not found');
  });

  it('should find all customers', async () => {
    const customerRepository = new CustomerRepository();
    await customerRepository.create(customer);
    await customerRepository.create(customer2);

    const productRepository = new ProductRepository();
    await productRepository.create(product);
    await productRepository.create(product2);

    const orderRepository = new OrderRepository();
    await orderRepository.create(order);
    await orderRepository.create(order2);

    const orders = await orderRepository.findAll();

    expect(orders).toHaveLength(2);
    expect(orders).toContainEqual(order);
    expect(orders).toContainEqual(order2);
  });
});
