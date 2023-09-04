import ClientGateway from '../../gateway/client.gateway';
import { IFindClientInputDto, IFindClientOutputDto } from './find-client.dto';

export default class FindClientUseCase {
  constructor(private readonly repository: ClientGateway) {}

  async execute(input: IFindClientInputDto): Promise<IFindClientOutputDto> {
    const client = await this.repository.find(input.id);

    return {
      id: client.id.id,
      name: client.name,
      email: client.email,
      address: client.address,
    };
  }
}
