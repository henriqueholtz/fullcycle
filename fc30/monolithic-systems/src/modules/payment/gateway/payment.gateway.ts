import Transaction from '../domain/transaction';

export default interface IPaymentGateway {
  save(input: Transaction): Promise<Transaction>;
}
