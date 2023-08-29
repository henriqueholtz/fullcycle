import { Sequelize } from 'sequelize-typescript';
import { ProductModel } from '../repository/product.model';
import ProductAdmFacadeFactory from '../factory/facade.factory';

describe('ProductAdmFacade test', () => {
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
    // const productRepository = new ProductRepository();
    // const addProductUseCase = new AddProductUseCase(productRepository);
    // const productFacade = new ProductAdmFacade({
    //   addUseCase: addProductUseCase,
    //   stockUseCase: undefined,
    // });

    const productFacade = ProductAdmFacadeFactory.create();

    const input = {
      id: '1',
      name: 'Product 1',
      description: 'Product 1 description',
      purchasePrice: 10,
      stock: 10,
    };

    await productFacade.addProduct(input);

    const product = await ProductModel.findOne({ where: { id: '1' } });
    const productJson: ProductModel = product.toJSON();
    expect(productJson.id).toBe(input.id);
    expect(productJson.name).toBe(input.name);
    expect(productJson.description).toBe(input.description);
    expect(productJson.purchasePrice).toBe(input.purchasePrice);
    expect(productJson.stock).toBe(input.stock);
  });

  it('it should check the stock', async () => {
    const productFacade = ProductAdmFacadeFactory.create();
    const input = {
      id: '1',
      name: 'Product 1',
      description: 'Product 1 description',
      purchasePrice: 10,
      stock: 10,
    };

    await productFacade.addProduct(input);

    const result = await productFacade.checkStock({ productId: '1' });

    expect(result.productId).toBe(input.id);
    expect(result.stock).toBe(input.stock);
  });
});
