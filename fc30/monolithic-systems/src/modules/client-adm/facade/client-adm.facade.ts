import IUseCase from '../../@shared/domain/usecase/use-case.interface';
import IClientAdmFacade, {
  IAddClientFacadeInputDto,
  IFindClientFacadeInputDto,
  IFindClientFacadeOutputDto,
} from './client-adm.facade.interface';

export interface UseCasesProps {
  findUseCase: IUseCase;
  addUseCase: IUseCase;
}

export default class ClientAdmFacade implements IClientAdmFacade {
  private _findUseCase: IUseCase;
  private _addUseCase: IUseCase;

  constructor(usecasesProps: UseCasesProps) {
    this._findUseCase = usecasesProps.findUseCase;
    this._addUseCase = usecasesProps.addUseCase;
  }
  async add(input: IAddClientFacadeInputDto): Promise<void> {
    await this._addUseCase.execute(input);
  }
  async find(
    input: IFindClientFacadeInputDto
  ): Promise<IFindClientFacadeOutputDto> {
    return await this._findUseCase.execute(input);
  }
}
