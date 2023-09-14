export interface IPlaceOrderInputDto {
  clientId: string;
  products: {
    productId: string;
  }[];
}

export interface IPlaceOrderOutputDto {
  id: string;
  total: number;
  products: {
    productId: string;
  }[];
}
