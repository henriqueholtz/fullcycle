import IUseCase from '../../../@shared/domain/usecase/use-case.interface';
import Id from '../../../@shared/domain/value-object/id.value-object';
import Invoice from '../../domain/invoice';
import Address from '../../domain/invoice.address';
import InvoiceItems from '../../domain/invoice.items';
import IInvoiceGateway from '../../gateway/invoice.gateway';
import {
  IGenerateInvoiceUseCaseInputDto,
  IGenerateInvoiceUseCaseOutputDto,
} from './generate-invoice.dto';

export default class GenerateInvoiceUseCase implements IUseCase {
  constructor(private invoiceRepository: IInvoiceGateway) {}

  async execute(
    input: IGenerateInvoiceUseCaseInputDto
  ): Promise<IGenerateInvoiceUseCaseOutputDto> {
    const invoice = new Invoice({
      name: input.name,
      document: input.document,
      items: input.items.map(
        (i) =>
          new InvoiceItems({ id: new Id(i.id), name: i.name, price: i.price })
      ),
      address: new Address({
        street: input.street,
        number: input.number,
        complement: input.complement,
        city: input.city,
        state: input.state,
        zipCode: input.zipCode,
      }),
    });

    const persistInvoice: Invoice = await this.invoiceRepository.generate(
      invoice
    );

    return {
      id: persistInvoice.id.id,
      document: persistInvoice.document,
      name: persistInvoice.name,
      items: persistInvoice.items.map((i) => {
        return {
          id: i.id.id,
          name: i.name,
          price: i.price,
        };
      }),
      street: persistInvoice.address.street,
      number: persistInvoice.address.number,
      complement: persistInvoice.address.complement,
      city: persistInvoice.address.city,
      state: persistInvoice.address.state,
      zipCode: persistInvoice.address.zipCode,
      total: persistInvoice.total,
    };
  }
}
