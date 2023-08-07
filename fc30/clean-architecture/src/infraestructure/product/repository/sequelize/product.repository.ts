import Product from '../../../../domain/product/entity/product';
import IProductRepository from '../../../../domain/product/repository/product-repository.interface';
import ProductModel from './product.model';

export default class ProductRepository implements IProductRepository {
  async create(entity: Product): Promise<void> {
    await ProductModel.create({
      id: entity.id,
      name: entity.name,
      price: entity.price,
    });
  }
  async update(entity: Product): Promise<void> {
    await ProductModel.update(
      {
        name: entity.name,
        price: entity.price,
      },
      { where: { id: entity.id } }
    );
  }
  async find(id: string): Promise<Product> {
    const productModel = await ProductModel.findOne({ where: { id: id } });

    return new Product(productModel.id, productModel.name, productModel.price);
  }
  async findAll(): Promise<Product[]> {
    const products = await ProductModel.findAll();
    return products.map(
      (productModel) =>
        new Product(productModel.id, productModel.name, productModel.price)
    );
  }
}
