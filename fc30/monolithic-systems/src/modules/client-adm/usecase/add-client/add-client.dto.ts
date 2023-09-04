export interface IAddClientInputDto {
  id?: string;
  name: string;
  email: string;
  address: string;
}

export interface IAddClientOutputDto {
  id: string;
  name: string;
  email: string;
  address: string;
  createdAt: Date;
  updatedAt: Date;
}
