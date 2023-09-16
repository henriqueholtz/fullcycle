import IUseCase from '../../@shared/domain/usecase/use-case.interface';
import IGenerateInvoiceFacade, {
  IFindInvoiceFacadeInputDto,
  IFindInvoiceFacadeOutputDto,
  IGenerateInvoiceFacadeInputDto,
  IGenerateInvoiceFacadeOutputDto,
} from './invoice.facade.interface';

export interface UseCasesProps {
  findUseCase: IUseCase;
  generateUseCase: IUseCase;
}

export default class InvoiceFacade implements IGenerateInvoiceFacade {
  private _findUseCase: IUseCase;
  private _generateUseCase: IUseCase;

  constructor(usecasesProps: UseCasesProps) {
    this._findUseCase = usecasesProps.findUseCase;
    this._generateUseCase = usecasesProps.generateUseCase;
  }
  async generate(
    input: IGenerateInvoiceFacadeInputDto
  ): Promise<IGenerateInvoiceFacadeOutputDto> {
    return await this._generateUseCase.execute(input);
  }
  async find(
    input: IFindInvoiceFacadeInputDto
  ): Promise<IFindInvoiceFacadeOutputDto> {
    return await this._findUseCase.execute(input);
  }
}
