export interface IFindProductInputDto {
  id: string;
}

export interface IFindProductOutputDto {
  id: string;
  name: string;
  description: string;
  salesPrice: number;
}
