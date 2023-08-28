import { Sequelize } from 'sequelize-typescript';
import Id from '../../@shared/domain/value-object/id.value-object';
import Product from '../domain/product.entity';
import { ProductModel } from './product.model';
import ProductRepository from './product.repository';

describe('ProductRepository test', () => {
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
    const productProps = {
      id: new Id('1'),
      name: 'Product 1',
      description: 'Product 1 description',
      purchasePrice: 100,
      stock: 10,
    };
    const product = new Product(productProps);
    const productRepository = new ProductRepository();
    await productRepository.add(product);

    const productDb = await ProductModel.findOne({
      where: { id: productProps.id.id },
    });

    var productDbJson = productDb.toJSON<ProductModel>();

    expect(productProps.id.id).toEqual(productDbJson.id);
    expect(productProps.id.id).toEqual(productDbJson.id);
    expect(productProps.name).toEqual(productDbJson.name);
    expect(productProps.description).toEqual(productDbJson.description);
    expect(productProps.purchasePrice).toEqual(productDbJson.purchasePrice);
    expect(productProps.stock).toEqual(productDbJson.stock);
  });

  it('Should find a product', async () => {
    const productRepository = new ProductRepository();

    ProductModel.create({
      id: '1',
      name: 'Product 1',
      description: 'Product 1 description',
      purchasePrice: 100,
      stock: 10,
      createdAt: new Date(),
      updatedAt: new Date(),
    });

    const product = await productRepository.find('1');

    expect(product.id.id).toEqual('1');
    expect(product.name).toEqual('Product 1');
    expect(product.description).toEqual('Product 1 description');
    expect(product.purchasePrice).toEqual(100);
    expect(product.stock).toEqual(10);
  });
});
