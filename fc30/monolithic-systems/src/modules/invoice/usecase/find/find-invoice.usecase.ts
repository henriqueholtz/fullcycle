import IUseCase from '../../../@shared/domain/usecase/use-case.interface';
import Id from '../../../@shared/domain/value-object/id.value-object';
import Invoice from '../../domain/invoice';
import Address from '../../domain/invoice.address';
import InvoiceItems from '../../domain/invoice.items';
import IInvoiceGateway from '../../gateway/invoice.gateway';
import {
  IFindInvoiceUseCaseInputDTO,
  IFindInvoiceUseCaseOutputDTO,
} from './find-invoice.dto';

export default class FindInvoiceUseCase implements IUseCase {
  constructor(private invoiceRepository: IInvoiceGateway) {}

  async execute(
    input: IFindInvoiceUseCaseInputDTO
  ): Promise<IFindInvoiceUseCaseOutputDTO> {
    const invoice: Invoice = await this.invoiceRepository.find(input.id);

    if (!invoice) throw new Error('Invoice not found!');

    return {
      createdAt: invoice.createdAt,
      name: invoice.name,
      document: invoice.document,
      id: invoice.id.id,
      total: invoice.total,
      items: invoice.items.map((i) => {
        return { id: i.id.id, name: i.name, price: i.price };
      }),
      address: new Address({
        street: invoice.address.street,
        number: invoice.address.number,
        complement: invoice.address.complement,
        city: invoice.address.city,
        state: invoice.address.state,
        zipCode: invoice.address.zipCode,
      }),
    };
  }
}
