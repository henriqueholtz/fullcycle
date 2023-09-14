import Id from '../../../@shared/domain/value-object/id.value-object';
import { IPlaceOrderInputDto } from './place-order.dto';
import PlaceOrderUseCase from './place-order.usecase';

// const transaction = new Transaction({
//   id: new Id('1'),
//   amount: 100,
//   orderId: '1',
//   status: 'approved',
// });

// const MockRepository = () => {
//   return {
//     save: jest.fn().mockReturnValue(Promise.resolve(transaction)),
//   };
// };

describe('PlaceOrderUsecase unit test', () => {
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
  });
});
