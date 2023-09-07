export interface IProcessPaymentInputDto {
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
