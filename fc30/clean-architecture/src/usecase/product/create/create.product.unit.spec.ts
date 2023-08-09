import CreateProductUseCase from './create.product.usecase';

const input = {
  name: 'Laptop',
  type: 'a',
  price: 1599.0,
};

const MockRepository = () => {
  return {
    find: jest.fn(),
    findAll: jest.fn(),
    create: jest.fn(),
    update: jest.fn(),
  };
};

describe('Unit test create product use case', () => {
  it('Should create a Product', async () => {
    const productRepository = MockRepository();
    const productCreateUseCase = new CreateProductUseCase(productRepository);

    const output = await productCreateUseCase.execute(input);

    expect(output).toEqual({
      id: expect.any(String),
      name: input.name,
      price: input.price,
    });
  });

  it("Should thrown an error when the product's type is invalid", async () => {
    const productRepository = MockRepository();
    const productCreateUseCase = new CreateProductUseCase(productRepository);

    input.type = 'invalid';

    await expect(productCreateUseCase.execute(input)).rejects.toThrow(
      'Product type not supported'
    );
  });
});
