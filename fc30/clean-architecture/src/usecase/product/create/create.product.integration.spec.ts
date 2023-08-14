import { Sequelize } from 'sequelize-typescript';
import ProductModel from '../../../infraestructure/product/repository/sequelize/product.model';
import ProductRepository from '../../../infraestructure/product/repository/sequelize/product.repository';
import CreateProductUseCase from './create.product.usecase';
import {
  InputCreateProductDto,
  OutputCreateProductDto,
} from './create.product.dto';

describe('Test create product use case', () => {
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

  it('Should create a product', async () => {
    const productRepository = new ProductRepository();
    const usecase = new CreateProductUseCase(productRepository);

    const input: InputCreateProductDto = {
      name: 'Laptop',
      price: 1650.5,
      type: 'a',
    };

    const output: OutputCreateProductDto = {
      id: 'uuid',
      name: 'Laptop',
      price: 1650.5,
    };

    const result = await usecase.execute(input);

    expect(result.name).toEqual(output.name);
    expect(result.price).toEqual(output.price);
  });
});
