import { Sequelize } from 'sequelize-typescript';
import { ClientModel } from '../repository/client.model';
import ClientAdmFacadeFactory from '../factory/facade.factory';

describe('ClientAdmFacade test', () => {
  let sequelize: Sequelize;

  beforeEach(async () => {
    sequelize = new Sequelize({
      dialect: 'sqlite',
      storage: ':memory:',
      logging: false,
      sync: { force: true },
    });

    await sequelize.addModels([ClientModel]);
    await sequelize.sync();
  });

  afterEach(async () => {
    await sequelize.close();
  });

  it('Should create a client', async () => {
    const facade = ClientAdmFacadeFactory.create();

    const input = {
      id: '1',
      name: 'Client 1',
      email: 'x@x.com',
      address: 'Address 1',
    };

    await facade.add(input);

    const client = await ClientModel.findOne({ where: { id: '1' } });
    const clientAsJson = client.toJSON();

    expect(clientAsJson).toBeDefined();
    expect(clientAsJson!.name).toBe(input.name);
    expect(clientAsJson!.email).toBe(input.email);
    expect(clientAsJson!.address).toBe(input.address);
  });

  it('It should find a client', async () => {
    const facade = ClientAdmFacadeFactory.create();

    const input = {
      id: '1',
      name: 'Client 1',
      email: 'x@x.com',
      address: 'Address 1',
    };

    await facade.add(input);

    const client = await facade.find({ id: '1' });

    expect(client).toBeDefined();
    expect(client!.id).toBe(input.id);
    expect(client!.name).toBe(input.name);
    expect(client!.email).toBe(input.email);
    expect(client!.address).toBe(input.address);
  });
});
