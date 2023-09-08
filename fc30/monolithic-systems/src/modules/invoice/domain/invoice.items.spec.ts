import Id from '../../@shared/domain/value-object/id.value-object';
import InvoiceItems from './invoice.items';

describe('Unit tests for InvoiceItems', () => {
  it('Should create an item', () => {
    const item = new InvoiceItems({
      id: new Id('1'),
      name: 'Name 1',
      price: 50,
    });
    expect(item.id.id.length > 0);
    expect(item.name).toBe('Name 1');
    expect(item.price).toBe(50);
  });

  it("Shouldn't create an item without a name", () => {
    expect(
      () =>
        new InvoiceItems({
          id: new Id('1'),
          name: '',
          price: 50,
        })
    ).toThrow('Name is required!');
  });

  it("Shouldn't create an item without a valid price", () => {
    expect(
      () =>
        new InvoiceItems({
          id: new Id('1'),
          name: 'Item 1',
          price: 0,
        })
    ).toThrow('Price must be greater than 0');
  });
});
