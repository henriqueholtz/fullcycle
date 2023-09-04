import Id from '../../../@shared/domain/value-object/id.value-object';
import Client from '../../domain/client.entity';
import IClientGateway from '../../gateway/client.gateway';
import { IAddClientInputDto, IAddClientOutputDto } from './add-client.dto';

export default class AddClientUseCase {
  private _clientRepository: IClientGateway;

  constructor(_ClientRepository: IClientGateway) {
    this._clientRepository = _ClientRepository;
  }

  async execute(input: IAddClientInputDto): Promise<IAddClientOutputDto> {
    const props = {
      id: new Id(input.id),
      name: input.name,
      email: input.email,
      address: input.address,
    };

    const client = new Client(props);
    this._clientRepository.add(client);

    return {
      id: client.id.id,
      name: client.name,
      email: client.email,
      address: client.address,
      createdAt: client.createdAt,
      updatedAt: client.updatedAt,
    };
  }
}
