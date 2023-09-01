import Product from '../domain/product.entity';

export default interface IProductGateway {
  findAll(): Promise<Product[]>;
  find(id: string): Promise<Product>;
}
