import IUseCase from '../../../@shared/domain/usecase/use-case.interface';
import Id from '../../../@shared/domain/value-object/id.value-object';
import IClientAdmFacade from '../../../client-adm/facade/client-adm.facade.interface';
import IProductAdmFacade from '../../../product-adm/facade/product-adm.facade.interface';
import StoreCatalogFacade from '../../../store-catalog/facade/store-catalog.facade';
import Client from '../../domain/client.entity';
import Order from '../../domain/order.entity';
import Product from '../../domain/product.entity';
import ICheckoutGateway from '../../gateway/checkout.gateway';
import { IPlaceOrderInputDto, IPlaceOrderOutputDto } from './place-order.dto';

export default class PlaceOrderUseCase implements IUseCase {
  private _clientFacade: IClientAdmFacade;
  private _productFacade: IProductAdmFacade;
  private _catalogFacade: StoreCatalogFacade;
  private _repository: ICheckoutGateway;

  constructor(
    clientFacade: IClientAdmFacade,
    productFacade: IProductAdmFacade,
    catalogFacade: StoreCatalogFacade,
    repository: ICheckoutGateway
  ) {
    this._clientFacade = clientFacade;
    this._productFacade = productFacade;
    this._catalogFacade = catalogFacade;
    this._repository = repository;
  }

  async execute(input: IPlaceOrderInputDto): Promise<IPlaceOrderOutputDto> {
    const client = await this._clientFacade.find({ id: input.clientId });
    if (!client) {
      throw new Error('Client not found');
    }

    await this.validateProducts(input);

    const products = await Promise.all(
      input.products.map((p) => {
        return this.getProduct(p.productId);
      })
    );

    const myClient = new Client({
      id: new Id(client.id),
      name: client.name,
      email: client.email,
      address: client.address,
    });

    const order = new Order({
      client: myClient,
      products: products,
    });
    this._repository.addOrder(order);

    return {
      id: order.id.id,
      total: order.total,
      products: order.products.map((p) => {
        return {
          productId: p.id.id,
        };
      }),
    };
  }

  private async validateProducts(input: IPlaceOrderInputDto) {
    if (input.products.length === 0) {
      throw new Error('No products selected');
    }

    for (const p of input.products) {
      const product = await this._productFacade.checkStock({
        productId: p.productId,
      });
      if (product.stock <= 0) {
        throw new Error(
          `Product ${product.productId} is not available in stock`
        );
      }
    }
  }

  private async getProduct(productId: string): Promise<Product> {
    const product = await this._catalogFacade.find({ id: productId });
    if (!product) {
      throw new Error('Product not found');
    }
    const productProps = {
      id: new Id(product.id),
      name: product.name,
      description: product.description,
      salesPrice: product.salesPrice,
    };
    const myProduct = new Product(productProps);
    return myProduct;
  }
}
