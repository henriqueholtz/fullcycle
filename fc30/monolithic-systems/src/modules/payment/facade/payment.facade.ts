import IUseCase from '../../@shared/domain/usecase/use-case.interface';
import IPaymentFacade, {
  IPaymentFacadeInputDto,
  IPaymentFacadeOutputDto,
} from './facade.interface';

export default class PaymentFacade implements IPaymentFacade {
  constructor(private processPaymentUseCase: IUseCase) {}
  process(input: IPaymentFacadeInputDto): Promise<IPaymentFacadeOutputDto> {
    return this.processPaymentUseCase.execute(input);
  }
}
