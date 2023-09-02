import IUseCase from '../../@shared/domain/usecase/use-case.interface';
import IStoreCatalogFacade, {
  IFindAllStoreCatalogFacadeOutputDto,
  IFindStoreCatalogFacadeInputDto,
  IFindStoreCatalogFacadeOutputDto,
} from './store-catalog.facade.interface';

export interface UseCasesProps {
  findUseCase: IUseCase;
  findAllUsecase: IUseCase;
}

export default class StoreCatalogFacade implements IStoreCatalogFacade {
  private _findUsecase: IUseCase;
  private _findAllUsecase: IUseCase;

  constructor(usecasesProps: UseCasesProps) {
    this._findUsecase = usecasesProps.findUseCase;
    this._findAllUsecase = usecasesProps.findAllUsecase;
  }
  async find(
    id: IFindStoreCatalogFacadeInputDto
  ): Promise<IFindStoreCatalogFacadeOutputDto> {
    const product = await this._findUsecase.execute(id);
    return product;
  }
  async findAll(): Promise<IFindAllStoreCatalogFacadeOutputDto> {
    const products = await this._findAllUsecase.execute();
    return products;
  }
}
