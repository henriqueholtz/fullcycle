import Id from '../../../@shared/domain/value-object/id.value-object';
import Invoice from '../../domain/invoice';
import Address from '../../domain/invoice.address';
import InvoiceItems from '../../domain/invoice.items';
import FindInvoiceUseCase from './find-invoice.usecase';

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
    generate: jest.fn(),
    find: jest.fn().mockReturnValue(Promise.resolve(invoice)),
  };
};

describe('Find Invoice usecase unit test', () => {
  it('Should find the invoice', async () => {
    const repository = MockRepository();
    const usecase = new FindInvoiceUseCase(repository);

    const input = {
      id: '1',
    };

    const result = await usecase.execute(input);

    expect(repository.find).toHaveBeenCalled();
    expect(repository.generate).not.toHaveBeenCalled();
    expect(result.id).toBe(input.id);
  });

  it("Shouldn't find an invoice", async () => {
    const repository = MockRepository();
    repository.find = jest.fn().mockReturnValue(Promise.resolve());
    const usecase = new FindInvoiceUseCase(repository);

    const input = {
      id: '1',
    };

    await expect(usecase.execute(input)).rejects.toThrow('Invoice not found!');
  });
});
