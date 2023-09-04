import { Sequelize } from 'sequelize-typescript';
import Id from '../../@shared/domain/value-object/id.value-object';
import Client from '../domain/client.entity';
import { ClientModel } from './client.model';
import ClientRepository from './client.repository';

describe('ClientRepository test', () => {
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
    const client = new Client({
      id: new Id('1'),
      name: 'Client 1',
      email: 'x@x.com',
      address: 'Address 1',
    });

    const repository = new ClientRepository();
    await repository.add(client);

    const clientDb = await ClientModel.findOne({
      where: { id: client.id.id },
    });

    const clientDbAsJson = clientDb.toJSON();

    expect(clientDbAsJson).toBeDefined();
    expect(clientDbAsJson.id).toBe(client.id.id);
    expect(clientDbAsJson.name).toBe(client.name);
    expect(clientDbAsJson.email).toBe(client.email);
    expect(clientDbAsJson.address).toBe(client.address);
  });

  it('Should find a client', async () => {
    const client = new Client({
      id: new Id('1'),
      name: 'Client 1',
      email: 'x@x.com',
      address: 'Address 1',
    });

    const repository = new ClientRepository();
    await repository.add(client);

    const result = await repository.find('1');

    expect(result).toBeDefined();
    expect(result.id.id).toBe(client.id.id);
    expect(result.name).toBe(client.name);
    expect(result.email).toBe(client.email);
    expect(result.address).toBe(client.address);
  });
});
