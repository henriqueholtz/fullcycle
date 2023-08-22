import IValidator from '../../../@shared/validator/validator.interface';
import productYupValidator from '../../../customer/validator/product.yup.validator';
import Product from '../product';

export default class ProductValidatorFactory {
  static create(): IValidator<Product> {
    return new productYupValidator();
  }
}
