export interface IAddProductFacadeInputDto {
  id?: string;
  name: string;
  description: string;
  purchasePrice: number;
  stock: number;
}

export interface ICheckStockFacadeInputDto {
  productId: string;
}

export interface ICheckStockFacadeOutputDto {
  productId: string;
  stock: number;
}

export default interface PIroductAdmFacade {
  addProduct(input: IAddProductFacadeInputDto): Promise<void>;
  checkStock(
    input: ICheckStockFacadeInputDto
  ): Promise<ICheckStockFacadeOutputDto>;
}
