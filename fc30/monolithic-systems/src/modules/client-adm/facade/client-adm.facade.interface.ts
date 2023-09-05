export interface IAddClientFacadeInputDto {
  id?: string;
  name: string;
  email: string;
  address: string;
}

export interface IFindClientFacadeInputDto {
  id: string;
}

export interface IFindClientFacadeOutputDto {
  id: string;
  name: string;
  email: string;
  address: string;
}

export default interface IClientAdmFacade {
  add(input: IAddClientFacadeInputDto): Promise<void>;
  find(input: IFindClientFacadeInputDto): Promise<IFindClientFacadeOutputDto>;
}
