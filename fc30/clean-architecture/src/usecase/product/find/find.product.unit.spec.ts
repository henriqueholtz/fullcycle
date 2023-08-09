import Product from '../../../domain/product/entity/product';
import FindproductUseCase from './find.product.usecase';

const product = new Product('12345', 'Laptop', 1712.25);

const MockRepository = () => {
  return {
    find: jest.fn().mockReturnValue(Promise.resolve(product)),
    findAll: jest.fn(),
    create: jest.fn(),
    update: jest.fn(),
  };
};

describe('Unit Test find product use case', () => {
  it('should find a product', async () => {
    const productRepository = MockRepository();
    const usecase = new FindproductUseCase(productRepository);

    const input = {
      id: '12345',
    };

    const output = {
      id: '12345',
      name: 'Laptop',
      price: 1712.25,
    };

    const result = await usecase.execute(input);

    expect(result).toEqual(output);
  });

  it('should not find a product', async () => {
    const productRepository = MockRepository();
    const errorMessage = 'Product not found';
    productRepository.find.mockImplementation(() => {
      throw new Error(errorMessage);
    });
    const usecase = new FindproductUseCase(productRepository);

    const input = {
      id: '123',
    };

    expect(() => {
      return usecase.execute(input);
    }).rejects.toThrow(errorMessage);
  });
});
