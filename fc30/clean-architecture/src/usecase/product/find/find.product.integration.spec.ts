import { Sequelize } from 'sequelize-typescript';
import Product from '../../../domain/product/entity/product';
import ProductModel from '../../../infraestructure/product/repository/sequelize/product.model';
import ProductRepository from '../../../infraestructure/product/repository/sequelize/product.repository';
import FindProductUseCase from './find.product.usecase';

describe('Test find product use case', () => {
  let sequelize: Sequelize;

  beforeEach(async () => {
    sequelize = new Sequelize({
      dialect: 'sqlite',
      storage: ':memory:',
      logging: false,
      sync: { force: true },
    });

    await sequelize.addModels([ProductModel]);
    await sequelize.sync();
  });

  afterEach(async () => {
    await sequelize.close();
  });

  it('Should find a product', async () => {
    const productRepository = new ProductRepository();
    const usecase = new FindProductUseCase(productRepository);

    const product = new Product('12345', 'Laptop', 1714.15);

    await productRepository.create(product);

    const input = {
      id: '12345',
    };

    const output = {
      id: '12345',
      name: 'Laptop',
      price: 1714.15,
    };

    const result = await usecase.execute(input);

    expect(result).toEqual(output);
  });
});
