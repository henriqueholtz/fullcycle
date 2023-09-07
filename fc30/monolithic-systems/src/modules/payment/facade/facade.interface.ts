export interface IPaymentFacadeInputDto {
  orderId: string;
  amount: number;
}

export interface IPaymentFacadeOutputDto {
  transactionId: string;
  orderId: string;
  amount: number;
  status: string;
  createdAt: Date;
  updatedAt: Date;
}

export default interface IPaymentFacade {
  process(input: IPaymentFacadeInputDto): Promise<IPaymentFacadeOutputDto>;
}
