import Id from '../../@shared/domain/value-object/id.value-object';
import Product from '../domain/product.entity';
import IProductGateway from '../gateway/product.gateway';
import { ProductModel } from './product.model';

export default class ProductRepository implements IProductGateway {
  async findAll(): Promise<Product[]> {
    const products = await ProductModel.findAll();

    return products.map((product) => {
      const productAsJson = product.toJSON();
      return new Product({
        id: new Id(productAsJson.id),
        name: productAsJson.name,
        description: productAsJson.description,
        salesPrice: productAsJson.salesPrice,
      });
    });
  }

  async find(id: string): Promise<Product> {
    const product = await ProductModel.findOne({
      where: { id },
    });

    if (!product) {
      throw new Error(`Product with id ${id} not found`);
    }

    const productAsJson = product.toJSON();
    return new Product({
      id: new Id(productAsJson.id),
      name: productAsJson.name,
      description: productAsJson.description,
      salesPrice: productAsJson.salesPrice,
    });
  }
}
