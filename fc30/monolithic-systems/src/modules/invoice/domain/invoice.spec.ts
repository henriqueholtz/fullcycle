import Id from '../../@shared/domain/value-object/id.value-object';
import Invoice from './invoice';
import Address from './invoice.address';
import InvoiceItems from './invoice.items';

const GenerateAnInvoice = (items: InvoiceItems[]): Invoice => {
  return new Invoice({
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
    items: items,
    name: 'Invoice 1',
  });
};

describe('Unit tests for Invoice', () => {
  it('Should create an invoice', () => {
    const items = [
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
    ];
    const invoice = GenerateAnInvoice(items);
    expect(invoice.id.id.length > 0);
    expect(invoice.name).toBe('Invoice 1');
    expect(invoice.address).toBeDefined();
    expect(invoice.items.length).toBe(2);
    expect(invoice.total).toBe(50);
  });

  it("Shouldn't create an invoice without at least one item", () => {
    expect(() => GenerateAnInvoice([])).toThrow(
      'At least one item is required!'
    );
  });
});
