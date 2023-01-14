import EventDispatcher from '../@shared/event-dispatcher';
import CustomerCreatedEvent from './customer-created.event';
import SendConsoleLog1Handler from './handlers/send-console-log-1.handler';
import SendConsoleLog2Handler from './handlers/send-console-log-2.handler';

const eventName: string = 'CustomerCreatedEvent';

describe('CustomerCreatedEvent tests', () => {
  it('should register 2 handlers and notify all handlers', () => {
    const eventDispatcher = new EventDispatcher();
    const sendConsoleLog1Handler = new SendConsoleLog1Handler();
    const sendConsoleLog2Handler = new SendConsoleLog2Handler();
    const spySendConsoleLog1Handler = jest.spyOn(
      sendConsoleLog1Handler,
      'handle'
    );
    const spySendConsoleLog2Handler = jest.spyOn(
      sendConsoleLog2Handler,
      'handle'
    );

    eventDispatcher.register(eventName, sendConsoleLog1Handler);
    expect(eventDispatcher.getEventHandlers[eventName]).toBeDefined();
    expect(eventDispatcher.getEventHandlers[eventName].length).toBe(1);
    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      sendConsoleLog1Handler
    );

    eventDispatcher.register(eventName, sendConsoleLog2Handler);
    expect(eventDispatcher.getEventHandlers[eventName]).toBeDefined();
    expect(eventDispatcher.getEventHandlers[eventName].length).toBe(2);
    expect(eventDispatcher.getEventHandlers[eventName][1]).toMatchObject(
      sendConsoleLog2Handler
    );

    const customerCreatedEvent = new CustomerCreatedEvent({
      id: '1',
      name: 'Henrique Holtz',
    });

    eventDispatcher.notify(customerCreatedEvent);

    expect(spySendConsoleLog1Handler).toHaveBeenCalled();
    expect(spySendConsoleLog2Handler).toHaveBeenCalled();
  });
});
