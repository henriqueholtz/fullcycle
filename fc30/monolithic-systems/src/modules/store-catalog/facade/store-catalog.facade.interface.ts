export interface IFindStoreCatalogFacadeInputDto {
  id: string;
}

export interface IFindStoreCatalogFacadeOutputDto {
  id: string;
  name: string;
  description: string;
  salesPrice: number;
}

export interface IFindAllStoreCatalogFacadeOutputDto {
  products: {
    id: string;
    name: string;
    description: string;
    salesPrice: number;
  }[];
}

export default interface IStoreCatalogFacade {
  find(
    id: IFindStoreCatalogFacadeInputDto
  ): Promise<IFindStoreCatalogFacadeOutputDto>;
  findAll(): Promise<IFindAllStoreCatalogFacadeOutputDto>;
}
