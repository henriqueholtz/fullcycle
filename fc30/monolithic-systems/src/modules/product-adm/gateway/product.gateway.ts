import Product from '../domain/product.entity';

export default interface IProductGateway {
  add(product: Product): Promise<void>;
  find(id: string): Promise<Product>;
}
