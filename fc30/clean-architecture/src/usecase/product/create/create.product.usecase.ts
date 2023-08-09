import IProductRepository from '../../../domain/product/repository/product-repository.interface';
import {
  InputCreateProductDto,
  OutputCreateProductDto,
} from './create.product.dto';
import productFactory from '../../../domain/product/entity/factory/product.factory';
import Product from '../../../domain/product/entity/product';

export default class CreateProductUseCase {
  private productRepository: IProductRepository;

  constructor(productRepository: IProductRepository) {
    this.productRepository = productRepository;
  }

  async execute(input: InputCreateProductDto): Promise<OutputCreateProductDto> {
    const product = productFactory.create(input.type, input.name, input.price);

    await this.productRepository.create(
      new Product(product.id, product.name, product.price)
    );

    return {
      id: product.id,
      name: product.name,
      price: product.price,
    };
  }
}
