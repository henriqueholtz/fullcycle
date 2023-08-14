import { Sequelize } from 'sequelize-typescript';
import ProductModel from '../../../infraestructure/product/repository/sequelize/product.model';
import ProductRepository from '../../../infraestructure/product/repository/sequelize/product.repository';
import UpdateProductUseCase from './update.product.usecase';
import Product from '../../../domain/product/entity/product';
import {
  InputUpdateProductDto,
  OutputUpdateProductDto,
} from './update.product.dto';

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
    const usecase = new UpdateProductUseCase(productRepository);

    const product = new Product('12345', 'Laptop', 1728.5);
    await productRepository.create(product);

    const input: InputUpdateProductDto = {
      name: 'Laptop Updated',
      price: 1650.5,
      id: '12345',
    };

    const output: OutputUpdateProductDto = {
      id: input.id,
      name: input.name,
      price: input.price,
    };

    const result = await usecase.execute(input);

    expect(result.name).toEqual(output.name);
    expect(result.price).toEqual(output.price);
  });
});
