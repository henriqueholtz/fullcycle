import Product from '../entity/product';

export default class ProductService {
  static increasePrice(products: Product[], percentage: number): void {
    if (products.length === 0)
      throw new Error('Should have at least one product!');

    if (percentage === 0) throw new Error("Percentage shouldn't be zero!");

    products.forEach((product) => {
      product.changePrice((product.price * percentage) / 100 + product.price);
    });
  }
}
