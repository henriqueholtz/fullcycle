import IProductGateway from '../../gateway/product.gateway';
import {
  IFindProductInputDto,
  IFindProductOutputDto,
} from './find-product.dto';

export default class FindProductUseCase {
  constructor(private readonly productRepository: IProductGateway) {}

  async execute(input: IFindProductInputDto): Promise<IFindProductOutputDto> {
    const product = await this.productRepository.find(input.id);

    return {
      id: product.id.id,
      name: product.name,
      description: product.description,
      salesPrice: product.salesPrice,
    };
  }
}
