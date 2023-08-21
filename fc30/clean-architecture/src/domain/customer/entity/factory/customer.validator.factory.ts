import IValidator from '../../../@shared/validator/validator.interface';
import CustomerYupValidator from '../../validator/customer.yup.validator';
import Customer from '../customer';

export default class CustomerValidatorFactory {
  static create(): IValidator<Customer> {
    return new CustomerYupValidator();
  }
}
