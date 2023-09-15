import { Sequelize } from 'sequelize-typescript';
import Id from '../../@shared/domain/value-object/id.value-object';
import { InvoiceModel } from './invoice.model';
import { InvoiceAddressModel } from './invoice.address.model';
import Invoice from '../domain/invoice';
import Address from '../domain/invoice.address';
import InvoiceRepository from './invoice.repository';
import { InvoiceItemModel } from './invoice.item.model';
import InvoiceItems from '../domain/invoice.items';

const invoice = new Invoice({
  id: new Id('1'),
  name: 'Invoice 1',
  items: [
    new InvoiceItems({
      id: new Id('1'),
      name: 'Item1',
      price: 15,
    }),
    new InvoiceItems({
      id: new Id('2'),
      name: 'Item2',
      price: 40,
    }),
  ],
  document: '12345',
  address: new Address({
    id: new Id('Add1'),
    city: 'Palotina',
    complement: '',
    number: '100A',
    state: 'PR',
    street: 'Street 1',
    zipCode: '85950-000',
  }),
});

describe('InvoiceRepository test', () => {
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
    const repository = new InvoiceRepository();
    await repository.generate(invoice);

    const invoicetDb = await InvoiceModel.findOne({
      where: { id: invoice.id.id },
      include: [InvoiceAddressModel, InvoiceItemModel],
    });

    const invoicetDbAsJson = invoicetDb.toJSON();

    expect(invoicetDbAsJson).toBeDefined();
    expect(invoicetDbAsJson.id).toBe(invoice.id.id);
    expect(invoicetDbAsJson.name).toBe(invoice.name);
    expect(invoicetDbAsJson.items.length).toBe(2);
    expect(invoicetDbAsJson.address.id).toBe(invoice.address.id.id);
  });

  it('Should find an invoice', async () => {
    const repository = new InvoiceRepository();
    await repository.generate(invoice);

    const result = await repository.find('1');

    expect(result).toBeDefined();
    expect(result.id.id).toBe(invoice.id.id);
    expect(result.name).toBe(invoice.name);
    expect(result.items.length).toBe(2);
    expect(result.address.id.id).toBe(invoice.address.id.id);
    expect(result.address.city).toBe(invoice.address.city);
    expect(result.address.street).toBe(invoice.address.street);
  });

  it('Should not find an invoice', async () => {
    const repository = new InvoiceRepository();
    await repository.generate(invoice);

    await expect(repository.find('2')).rejects.toThrow(`Invoice not found.`);
  });
});
