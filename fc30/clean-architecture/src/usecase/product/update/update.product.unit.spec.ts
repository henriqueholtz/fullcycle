import productFactory from '../../../domain/product/entity/factory/product.factory';

import UpdateProductUseCase from './update.product.usecase';

const product = productFactory.create('a', 'Laptop', 1599.0);

const input = {
  id: product.id,
  name: 'Laptop Updated',
  price: 1701.55,
};

const MockRepository = () => {
  return {
    create: jest.fn(),
    findAll: jest.fn(),
    find: jest.fn().mockReturnValue(Promise.resolve(product)),
    update: jest.fn(),
  };
};

describe('Unit test for product update use case', () => {
  it('Should update a product', async () => {
    const productRepository = MockRepository();
    const productUpdateUseCase = new UpdateProductUseCase(productRepository);

    const output = await productUpdateUseCase.execute(input);

    expect(output).toEqual(input);
  });
});
