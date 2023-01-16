import Product from '../entity/product';
import ProductService from './product.service';

describe('Product service unit tests', () => {
  const product1 = new Product('p1', 'p1', 20);
  const product2 = new Product('p2', 'p2', 50);
  const product3 = new Product('p3', 'p3', 200);
  it('Should change the price in all products', () => {
    ProductService.increasePrice([product1, product2, product3], 100);
    expect(product1.price).toBe(40);
    expect(product2.price).toBe(100);
    expect(product3.price).toBe(400);
  });

  it("Should throw error when don't have any products", () => {
    expect(() => {
      ProductService.increasePrice([], 100);
    }).toThrowError('Should have at least one product!');
  });

  it('Should throw error when the price is zero', () => {
    expect(() => {
      ProductService.increasePrice([product1, product2], 0);
    }).toThrowError("Percentage shouldn't be zero!");
  });
});
