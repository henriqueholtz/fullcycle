import InvoiceFacade from '../facade/invoice.facade';
import InvoiceRepository from '../repository/invoice.repository';
import FindInvoiceUseCase from '../usecase/find/find-invoice.usecase';
import GenerateInvoiceUseCase from '../usecase/generate/generate-invoice.usecase';

export default class InvoiceFacadeFactory {
  static create() {
    const repository = new InvoiceRepository();
    const findInvoiceUseCase = new FindInvoiceUseCase(repository);
    const generateInvoiceUseCase = new GenerateInvoiceUseCase(repository);

    return new InvoiceFacade({
      findUseCase: findInvoiceUseCase,
      generateUseCase: generateInvoiceUseCase,
    });
  }
}
