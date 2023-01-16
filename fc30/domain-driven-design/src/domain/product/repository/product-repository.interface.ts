import Product from '../entity/product';
import IRepository from '../../@shared/repository/repository.interface';

export default interface IProductRepository extends IRepository<Product> {}
