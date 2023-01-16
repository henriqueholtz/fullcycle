import Order from '../../../../domain/checkout/entity/order';
import OrderItem from '../../../../domain/checkout/entity/order_item';
import IOrderRepository from '../../../../domain/checkout/repository/order-repository.interface';
import OrderModel from './order.model';
import OrderItemModel from './order_item.model';

export default class OrderRepository implements IOrderRepository {
  async create(entity: Order): Promise<void> {
    await OrderModel.create(
      {
        id: entity.id,
        customer_id: entity.customerId,
        total: entity.total(),
        items: entity.items.map((item) => ({
          id: item.id,
          name: item.name,
          price: item.price,
          product_id: item.productId,
          quantity: item.quantity,
        })),
      },
      {
        include: [{ model: OrderItemModel }],
      }
    );
  }

  async update(entity: Order): Promise<void> {
    await OrderModel.update(
      {
        id: entity.id,
        customer_id: entity.customerId,
        total: entity.total(),
        items: entity.items,
      },
      {
        where: {
          id: entity.id,
        },
      }
    );
  }

  async find(id: string): Promise<Order> {
    let orderModel;
    try {
      orderModel = await OrderModel.findOne({
        where: {
          id,
        },
        include: [{ model: OrderItemModel }],
        rejectOnEmpty: true,
      });
    } catch (error) {
      throw new Error('Order not found');
    }

    const order = new Order(
      orderModel.id,
      orderModel.customer_id,
      orderModel.items.map(
        (itemModel) =>
          new OrderItem(
            itemModel.id,
            itemModel.name,
            itemModel.price,
            itemModel.product_id,
            itemModel.quantity
          )
      )
    );
    return order;
  }

  async findAll(): Promise<Order[]> {
    const orderModels = await OrderModel.findAll({
      include: [{ model: OrderItemModel }],
    });

    const orders = orderModels.map((orderModel) => {
      let order = new Order(
        orderModel.id,
        orderModel.customer_id,
        orderModel.items.map(
          (itemModel) =>
            new OrderItem(
              itemModel.id,
              itemModel.name,
              itemModel.price,
              itemModel.product_id,
              itemModel.quantity
            )
        )
      );
      return order;
    });

    return orders;
  }
}
