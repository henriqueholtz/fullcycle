import { Sequelize } from 'sequelize-typescript';
import ProductModel from '../db/sequelize/model/product';
import Product from '../../domain/entity/product';
import ProductRepository from './product.repository';

describe('Product Repository unit tests', () => {
  const id: string = '1';
  const name: string = 'p1';
  const price: number = 100;
  const product = new Product(id, name, price);
  let sequelize: Sequelize;

  beforeEach(async () => {
    sequelize = new Sequelize({
      dialect: 'sqlite',
      storage: ':memory:',
      logging: false,
      sync: { force: true },
    });

    sequelize.addModels([ProductModel]);
    await sequelize.sync();
  });

  afterEach(async () => {
    await sequelize.close();
  });

  it('Should create a product', async () => {
    const productRepository = new ProductRepository();
    await productRepository.create(product);

    const productModel = await ProductModel.findOne({ where: { id: id } });
    expect(productModel.toJSON()).toStrictEqual({
      id,
      name,
      price,
    });
  });

  it('Should update a product', async () => {
    const productRepository = new ProductRepository();
    await productRepository.create(product);
    const productModel = await ProductModel.findOne({ where: { id: id } });
    expect(productModel.toJSON()).toStrictEqual({
      id,
      name,
      price,
    });

    const newName: string = 'p1 updated';
    const newPrice: number = 120;
    product.changeName(newName);
    product.changePrice(newPrice);

    await productRepository.update(product);

    const productModel2 = await ProductModel.findOne({ where: { id: id } });
    expect(productModel2.toJSON()).toStrictEqual({
      id,
      name: newName,
      price: newPrice,
    });
  });

  it('Should find a product', async () => {
    const productRepository = new ProductRepository();
    await productRepository.create(product);
    const productModel = await ProductModel.findOne({ where: { id: id } });

    const foundProduct = await productRepository.find(id);

    expect(productModel.toJSON()).toStrictEqual({
      id: foundProduct.id,
      name: foundProduct.name,
      price: foundProduct.price,
    });
  });

  it('Should find all products', async () => {
    const productRepository = new ProductRepository();
    await productRepository.create(product);
    const product2 = new Product('2', 'p2', 120);
    const product3 = new Product('3', 'p3', 130);
    await productRepository.create(product2);
    await productRepository.create(product3);
    const products = [product, product2, product3];
    const foundProducts = await productRepository.findAll();

    expect(products).toEqual(foundProducts);
  });
});
