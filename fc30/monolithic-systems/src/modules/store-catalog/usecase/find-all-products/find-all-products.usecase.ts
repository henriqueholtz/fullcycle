import IProductGateway from '../../gateway/product.gateway';
import { IFindAllProductsOutputDto } from './find-all-products.dto';

export default class FindAllProductsUseCase {
  constructor(private readonly productRepository: IProductGateway) {}

  async execute(): Promise<IFindAllProductsOutputDto> {
    const product = await this.productRepository.findAll();

    return {
      products: product.map((product) => ({
        id: product.id.id,
        name: product.name,
        description: product.description,
        salesPrice: product.salesPrice,
      })),
    };
  }
}
