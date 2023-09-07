import Id from '../../../@shared/domain/value-object/id.value-object';

export interface IProcessPaymentInputDto {
  id?: Id;
  orderId: string;
  amount: number;
}

export interface IProcessPaymentOutputDto {
  transactionId: string;
  orderId: string;
  amount: number;
  status: string;
  createdAt: Date;
  updatedAt: Date;
}
