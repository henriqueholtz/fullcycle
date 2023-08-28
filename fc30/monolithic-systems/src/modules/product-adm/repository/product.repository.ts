import Id from '../../@shared/domain/value-object/id.value-object';
import Product from '../domain/product.entity';
import IProductGateway from '../gateway/product.gateway';
import { ProductModel } from './product.model';

export default class ProductRepository implements IProductGateway {
  async add(product: Product): Promise<void> {
    await ProductModel.create({
      id: product.id.id,
      name: product.name,
      description: product.description,
      purchasePrice: product.purchasePrice,
      stock: product.stock,
      createdAt: new Date(),
      updatedAt: new Date(),
    });
  }
  async find(id: string): Promise<Product> {
    const product = await ProductModel.findOne({
      where: { id },
    });

    if (!product) {
      throw new Error(`Product with id ${id} not found`);
    }
    const productAsJson = product.toJSON<Product>();

    return new Product({
      id: new Id(productAsJson.id.toString()),
      name: productAsJson.name,
      description: productAsJson.description,
      purchasePrice: productAsJson.purchasePrice,
      stock: productAsJson.stock,
      createdAt: productAsJson.createdAt,
      updatedAt: productAsJson.updatedAt,
    });
  }
}
