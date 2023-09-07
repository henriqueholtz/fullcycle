import IUseCase from '../../../@shared/domain/usecase/use-case.interface';
import Transaction from '../../domain/transaction';
import IPaymentGateway from '../../gateway/payment.gateway';
import {
  IProcessPaymentInputDto,
  IProcessPaymentOutputDto,
} from './process-payment.dto';

export default class ProcessPaymentUseCase implements IUseCase {
  constructor(private transactionRepository: IPaymentGateway) {}

  async execute(
    input: IProcessPaymentInputDto
  ): Promise<IProcessPaymentOutputDto> {
    const transaction = new Transaction({
      amount: input.amount,
      orderId: input.orderId,
    });
    transaction.process();
    const persistTransaction = await this.transactionRepository.save(
      transaction
    );

    return {
      transactionId: persistTransaction.id.id,
      orderId: persistTransaction.orderId,
      amount: persistTransaction.amount,
      status: persistTransaction.status,
      createdAt: persistTransaction.createdAt,
      updatedAt: persistTransaction.updatedAt,
    };
  }
}
