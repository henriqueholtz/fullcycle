import { Sequelize } from 'sequelize-typescript';
import ProductModel from '../../../infraestructure/product/repository/sequelize/product.model';
import ProductRepository from '../../../infraestructure/product/repository/sequelize/product.repository';
import ListProductUseCase from './list.product.usecase';
import { InputListProductDto, OutputListProductDto } from './list.product.dto';
import Product from '../../../domain/product/entity/product';

describe('Test list products use case', () => {
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

  it('Should list the products', async () => {
    const productRepository = new ProductRepository();
    const usecase = new ListProductUseCase(productRepository);

    const product1 = new Product('12345', 'Laptop 1', 1714.15);
    const product2 = new Product('54321', 'Laptop 2', 1728.5);

    await productRepository.create(product1);
    await productRepository.create(product2);

    const input: InputListProductDto = {};

    const output: OutputListProductDto = {
      products: [product1, product2],
    };

    const result = await usecase.execute(input);

    expect(result.products.length).toEqual(2);
    expect(result.products[0].name).toEqual(output.products[0].name);
    expect(result.products[0].price).toEqual(output.products[0].price);
    expect(result.products[1].name).toEqual(output.products[1].name);
    expect(result.products[1].price).toEqual(output.products[1].price);
  });
});
