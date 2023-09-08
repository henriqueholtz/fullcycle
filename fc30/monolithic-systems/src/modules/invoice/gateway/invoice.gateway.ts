import Invoice from '../domain/invoice';

export default interface IInvoiceGateway {
  save(input: Invoice): Promise<Invoice>;
}
