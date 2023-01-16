import EventDispatcher from '../../@shared/event/event-dispatcher';
import CustomerAddressChangedEvent from './customer-address-changed.event';
import SendConsoleLogHandler from './handlers/send-console-log.handler';

const eventName: string = 'CustomerAddressChangedEvent';

describe('CustomerAddressChangedEvent tests', () => {
  it('should register the handler and notify it', () => {
    const eventDispatcher = new EventDispatcher();
    const sendConsoleLogHandler = new SendConsoleLogHandler();
    const spysendConsoleLogHandler = jest.spyOn(
      sendConsoleLogHandler,
      'handle'
    );

    eventDispatcher.register(eventName, sendConsoleLogHandler);
    expect(eventDispatcher.getEventHandlers[eventName]).toBeDefined();
    expect(eventDispatcher.getEventHandlers[eventName].length).toBe(1);
    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      sendConsoleLogHandler
    );

    const customerAddressChangedEvent = new CustomerAddressChangedEvent({
      id: '1',
      name: 'Henrique Holtz',
      address: {
        street: 'Street 2C',
        zipCode: '88888-888',
        city: 'Palotina',
      },
    });

    eventDispatcher.notify(customerAddressChangedEvent);

    expect(spysendConsoleLogHandler).toHaveBeenCalled();
  });
});
