import Product from './product';

describe('Product unit tests', () => {
  it('Should throw error when id is empty', () => {
    expect(() => {
      let product = new Product('', 'Product 1', 100);
    }).toThrowError('Id is required!');
  });

  it('Should throw error when name is empty', () => {
    expect(() => {
      let product = new Product('1', '', 100);
    }).toThrowError('Name is required!');
  });

  it('Should throw error when price is less than zero', () => {
    expect(() => {
      let product = new Product('1', 'Product 1', -2);
    }).toThrowError('Price must be grater than zero!');
  });

  it('Should change name', () => {
    let newProductName: string = 'Product 1 Updated';
    let product = new Product('1', 'Product 1', 50);
    product.changeName(newProductName);
    expect(product.name).toBe(newProductName);
  });

  it('Should change price', () => {
    let newProductPrice: number = 75;
    let product = new Product('1', 'Product 1', 50);
    product.changePrice(newProductPrice);
    expect(product.price).toBe(newProductPrice);
  });
});
