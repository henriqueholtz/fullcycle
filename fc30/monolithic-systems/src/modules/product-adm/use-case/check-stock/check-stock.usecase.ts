import IProductGateway from '../../gateway/product.gateway';
import { CheckStockInputDto, CheckStockOutputDto } from './check-stock.dto';

export default class CheckStockUseCase {
  constructor(private readonly checkStockRepository: IProductGateway) {}

  async execute(input: CheckStockInputDto): Promise<CheckStockOutputDto> {
    const product = await this.checkStockRepository.find(input.productId);

    return {
      productId: product.id.id,
      stock: product.stock,
    };
  }
}
