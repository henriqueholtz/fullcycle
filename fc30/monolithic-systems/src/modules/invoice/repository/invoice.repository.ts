import Id from '../../@shared/domain/value-object/id.value-object';
import Invoice from '../domain/invoice';
import Address from '../domain/invoice.address';
import IInvoiceGateway from '../gateway/invoice.gateway';
import { InvoiceAddressModel } from './invoice.address.model';
import { InvoiceItemModel } from './invoice.item.model';
import { InvoiceModel } from './invoice.model';

export default class InvoiceRepository implements IInvoiceGateway {
  async generate(invoice: Invoice): Promise<Invoice> {
    const result: InvoiceModel = await InvoiceModel.create(
      {
        id: invoice.id?.id ?? new Id().id,
        name: invoice.name,
        document: invoice.document,
        addressId: invoice.address.id?.id ?? new Id().id,
        items: invoice.items.map((i) => {
          return {
            id: i.id.id,
            price: i.price,
            name: i.name,
          };
        }),
        address: {
          id: invoice.address.id?.id ?? new Id().id,
          city: invoice.address.city,
          street: invoice.address.street,
          complement: invoice.address.complement,
          number: invoice.address.number,
          state: invoice.address.state,
          zipCode: invoice.address.zipCode,
        },
        createdAt: new Date(),
        updatedAt: new Date(),
      },
      {
        include: [InvoiceAddressModel, InvoiceItemModel],
      }
    );

    const resultAsJson = result.toJSON();

    return new Invoice({
      address: new Address({
        id: new Id(resultAsJson.id),
        city: resultAsJson.address.city,
        complement: resultAsJson.address.complement,
        number: resultAsJson.address.number,
        state: resultAsJson.address.state,
        street: resultAsJson.address.street,
        zipCode: resultAsJson.address.zipCode,
      }),
      document: resultAsJson.document,
      items: resultAsJson.items.map((i: any) => {
        return {
          id: new Id(i.id),
          price: i.price,
          name: i.name,
        };
      }),
      name: resultAsJson.name,
      createdAt: resultAsJson.createdAt,
      updatedAt: resultAsJson.updatedAt,
      id: new Id(resultAsJson.id),
    });
  }
  async find(id: string): Promise<Invoice> {
    const invoice = await InvoiceModel.findOne({
      where: { id },
      include: [InvoiceAddressModel, InvoiceItemModel],
    });

    if (!invoice) {
      throw new Error(`Invoice not found.`);
    }

    const invoiceAsJson = invoice.toJSON();

    return new Invoice({
      id: new Id(invoiceAsJson.id),
      name: invoiceAsJson.name,
      document: invoiceAsJson.document,
      items: invoiceAsJson.items.map((i: any) => {
        return {
          id: new Id(i.id),
          price: i.price,
          name: i.name,
        };
      }),
      createdAt: invoiceAsJson.createdAt,
      updatedAt: invoiceAsJson.updatedAt,
      address: new Address({
        id: new Id(invoiceAsJson.addressId),
        city: invoiceAsJson.address.city,
        complement: invoiceAsJson.address.complement,
        number: invoiceAsJson.address.number,
        state: invoiceAsJson.address.state,
        street: invoiceAsJson.address.street,
        zipCode: invoiceAsJson.address.zipCode,
      }),
    });
  }
}
