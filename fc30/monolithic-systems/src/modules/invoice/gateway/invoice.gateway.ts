import Invoice from '../domain/invoice';

export default interface IInvoiceGateway {
  generate(input: Invoice): Promise<Invoice>;
  find(id: string): Promise<Invoice>;
}
