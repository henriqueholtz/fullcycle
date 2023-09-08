import Id from '../../../@shared/domain/value-object/id.value-object';
import Invoice from '../../domain/invoice';
import Address from '../../domain/invoice.address';
import InvoiceItems from '../../domain/invoice.items';
import {
  IGenerateInvoiceUseCaseInputDto,
  IGenerateInvoiceUseCaseOutputDto,
} from './generate-invoice.dto';
import GenerateInvoiceUseCase from './generate-invoice.usecase';

const invoice = new Invoice({
  id: new Id('1'),
  address: new Address({
    complement: 'Complement',
    number: '200A',
    city: 'Palotina',
    state: 'PR',
    street: 'Street A',
    zipCode: '85950-000',
  }),
  document: '12345',
  items: [
    new InvoiceItems({
      id: new Id('Item1'),
      name: 'Item 1',
      price: 15,
    }),
    new InvoiceItems({
      id: new Id('Item2'),
      name: 'Item 2',
      price: 35,
    }),
  ],
  name: 'Invoice 1',
});

const MockRepository = () => {
  return {
    save: jest.fn().mockReturnValue(Promise.resolve(invoice)),
  };
};

describe('Process generate-invoice usecase unit test', () => {
  it('Should save a invoice', async () => {
    const invoiceRepository = MockRepository();
    const usecase = new GenerateInvoiceUseCase(invoiceRepository);
    const input: IGenerateInvoiceUseCaseInputDto = {
      document: invoice.document,
      name: invoice.name,
      items: invoice.items.map((i) => {
        return {
          id: i.id.id,
          name: i.name,
          price: i.price,
        };
      }),
      number: invoice.address.number,
      city: invoice.address.city,
      complement: invoice.address.complement,
      state: invoice.address.state,
      street: invoice.address.street,
      zipCode: invoice.address.zipCode,
    };

    const result = await usecase.execute(input);

    expect(result.city).toBe(invoice.address.city);
    expect(result.street).toBe(invoice.address.street);
    expect(result.complement).toBe(invoice.address.complement);
    expect(result.street).toBe(invoice.address.street);
    expect(result.zipCode).toBe(invoice.address.zipCode);
    expect(result.name).toBe(invoice.name);

    expect(result.items.length).toBe(2);
    expect(result.total).toBe(50);
    expect(invoiceRepository.save).toHaveBeenCalled();
  });

  it("Shouldn't save the invoice without items", async () => {
    const invoiceRepository = MockRepository();
    const usecase = new GenerateInvoiceUseCase(invoiceRepository);
    const input: IGenerateInvoiceUseCaseInputDto = {
      document: invoice.document,
      name: invoice.name,
      items: [],
      number: invoice.address.number,
      city: invoice.address.city,
      complement: invoice.address.complement,
      state: invoice.address.state,
      street: invoice.address.street,
      zipCode: invoice.address.zipCode,
    };

    await expect(usecase.execute(input)).rejects.toThrow(
      'At least one item is required!'
    );
    expect(invoiceRepository.save).toHaveBeenCalledTimes(0);
  });
});
