import { Sequelize } from 'sequelize-typescript';
import Id from '../../@shared/domain/value-object/id.value-object';
import { InvoiceModel } from './invoice.model';
import { InvoiceAddressModel } from './invoice.address.model';
import Invoice from '../domain/invoice';
import Address from '../domain/invoice.address';
import InvoiceRepository from './invoice.repository';

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
      // InvoiceItemModel,
      InvoiceAddressModel,
    ]);
    await sequelize.sync();
  });

  afterEach(async () => {
    await sequelize.close();
  });

  it('Should create an invoice', async () => {
    const invoice = new Invoice({
      id: new Id('1'),
      name: 'Invoice 1',
      items: [],
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

    const repository = new InvoiceRepository();
    await repository.generate(invoice);

    const invoicetDb = await InvoiceModel.findOne({
      where: { id: invoice.id.id },
      include: [InvoiceAddressModel],
    });

    const invoicetDbAsJson = invoicetDb.toJSON();

    expect(invoicetDbAsJson).toBeDefined();
    expect(invoicetDbAsJson.id).toBe(invoice.id.id);
    expect(invoicetDbAsJson.name).toBe(invoice.name);
    // expect(invoicetDbAsJson.items.length).toBe(2);
    expect(invoicetDbAsJson.address.id).toBe(invoice.address.id.id);
  });
});
