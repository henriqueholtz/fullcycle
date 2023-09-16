import { Sequelize } from 'sequelize-typescript';
import { InvoiceModel } from '../repository/invoice.model';
import { InvoiceItemModel } from '../repository/invoice.item.model';
import { InvoiceAddressModel } from '../repository/invoice.address.model';
import InvoiceFacadeFactory from '../factory/facade.factory';
import { IGenerateInvoiceFacadeInputDto } from './invoice.facade.interface';

const input: IGenerateInvoiceFacadeInputDto = {
  city: 'Palotina',
  complement: 'Comp',
  document: '12345',
  street: 'Av 12',
  number: '10A',
  state: 'PR',
  zipCode: '85950-000',
  name: 'Name',
  items: [
    {
      id: 'Item1',
      name: 'Item 1',
      price: 15,
    },
    {
      id: 'Item2',
      name: 'Item 2',
      price: 40,
    },
  ],
};

describe('InvoiceFacade test', () => {
  let sequelize: Sequelize;

  beforeEach(async () => {
    sequelize = new Sequelize({
      dialect: 'sqlite',
      storage: ':memory:',
      logging: false,
      sync: { force: true },
    });

    await sequelize.addModels([
      InvoiceModel,
      InvoiceItemModel,
      InvoiceAddressModel,
    ]);
    await sequelize.sync();
  });

  afterEach(async () => {
    await sequelize.close();
  });

  it('Should create an invoice', async () => {
    const facade = InvoiceFacadeFactory.create();
    const result = await facade.generate(input);

    const invoiceDb = await InvoiceModel.findOne({
      where: { id: result.id },
      include: [InvoiceAddressModel, InvoiceItemModel],
    });
    const invoiceDbAsJson = invoiceDb.toJSON();

    expect(invoiceDbAsJson).toBeDefined();
    expect(invoiceDbAsJson!.name).toBe(input.name);
    expect(invoiceDbAsJson!.document).toBe(input.document);
    expect(invoiceDbAsJson!.items.length).toBe(input.items.length);
    expect(invoiceDbAsJson!.items[0].name).toBe(input.items[0].name);
    expect(invoiceDbAsJson!.items[0].price).toBe(input.items[0].price);
    expect(invoiceDbAsJson!.address.city).toBe(input.city);
    expect(invoiceDbAsJson!.address.street).toBe(input.street);
  });

  it('It should find an invoice', async () => {
    const facade = InvoiceFacadeFactory.create();

    const result = await facade.generate(input);

    const invoice = await facade.find({ id: result.id });

    expect(invoice).toBeDefined();
    expect(invoice.id).toBe(result.id);
    expect(invoice.address).toBeDefined();
    expect(invoice!.name).toBe(input.name);
    expect(invoice!.document).toBe(input.document);
    expect(invoice!.total).toBe(55);
    expect(invoice!.address.city).toBe(input.city);
    expect(invoice!.address.street).toBe(input.street);
  });

  it("It shouldn't find an invoice", async () => {
    const facade = InvoiceFacadeFactory.create();

    await expect(facade.find({ id: 'invalid-id' })).rejects.toThrow(
      `Invoice not found.`
    );
  });
});
