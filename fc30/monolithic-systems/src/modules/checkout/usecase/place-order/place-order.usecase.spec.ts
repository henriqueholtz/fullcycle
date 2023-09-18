import Id from '../../../@shared/domain/value-object/id.value-object';
import { IPlaceOrderInputDto } from './place-order.dto';
import PlaceOrderUseCase from './place-order.usecase';

describe('PlaceOrderUsecase unit test', () => {
  describe('validateProducts method', () => {
    //@ts-expect-error - no params in constructor
    const placeOrderUseCase = new PlaceOrderUseCase();

    it('Should throw error if no products are selected', async () => {
      const input: IPlaceOrderInputDto = {
        clientId: '0',
        products: [],
      };
      await expect(
        placeOrderUseCase['validateProducts'](input)
      ).rejects.toThrow(new Error('No products selected'));
    });

    it('Should throw error when the products it out of stock ', async () => {
      const mockProductFacade = {
        checkStock: jest.fn(({ productId }: { productId: string }) =>
          Promise.resolve({
            productId,
            stock: productId === '1' ? 0 : 1,
          })
        ),
      };

      //@ts-expect-error-force set product Facade
      placeOrderUseCase['_productFacade'] = mockProductFacade;

      let input: IPlaceOrderInputDto = {
        clientId: '0',
        products: [{ productId: '1' }],
      };

      await expect(
        placeOrderUseCase['validateProducts'](input)
      ).rejects.toThrow(new Error('Product 1 is not available in stock'));

      input = {
        clientId: '0',
        products: [{ productId: '0' }, { productId: '1' }],
      };
      await expect(
        placeOrderUseCase['validateProducts'](input)
      ).rejects.toThrow(new Error('Product 1 is not available in stock'));
      expect(mockProductFacade.checkStock).toHaveBeenCalledTimes(3);
    });
  });

  describe('Execute method', () => {
    it('Should throw an error when client is not found', async () => {
      const mockClientFacade = {
        find: jest.fn().mockResolvedValue(null),
      };
      //@ts-expect-error - no params in contructor
      const placeOrderUseCase = new PlaceOrderUseCase();
      //@ts-expect-error-force set clientFacade I
      placeOrderUseCase['_clientFacade'] = mockClientFacade;

      const input: IPlaceOrderInputDto = { clientId: '0', products: [] };

      await expect(placeOrderUseCase.execute(input)).rejects.toThrow(
        new Error('Client not found')
      );
    });

    it('Should throw an error when products are invalid', async () => {
      const mockClientFacade = {
        find: jest.fn().mockResolvedValue(true),
      };
      //@ts-expect-error- no params in constructor
      const placeOrderUseCase = new PlaceOrderUseCase();

      const mockValidateProducts = jest
        //@ts-expect-error - spy on private method
        .spyOn(placeOrderUseCase, 'validateProducts')
        //@ts-expect-error - not return never
        .mockRejectedValue(new Error('No products selected'));

      //@ts-expect-error-force set clientFacade
      placeOrderUseCase['_clientFacade'] = mockClientFacade;
      const input: IPlaceOrderInputDto = { clientId: '1', products: [] };
      await expect(placeOrderUseCase.execute(input)).rejects.toThrow(
        new Error('No products selected')
      );
      expect(mockValidateProducts).toHaveBeenCalledTimes(1);
    });
  });
});
