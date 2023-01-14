import SendEmailWhenProductIsCreatedHandler from '../Product/handlers/send-email-when-product-is-created.handler';
import EventDispatcher from './event-dispatcher';
import ProductCreatedEvent from '../Product/product-created.event';

describe('Domain Event tests', () => {
  it('Should register an event handler', () => {
    const eventDispatcher = new EventDispatcher();
    const eventHandler = new SendEmailWhenProductIsCreatedHandler();
    const eventName: string = 'ProductCreatedEvent';
    eventDispatcher.register(eventName, eventHandler);

    expect(eventDispatcher.getEventHandlers[eventName]).toBeDefined();
    expect(eventDispatcher.getEventHandlers[eventName].length).toBe(1);
    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      eventHandler
    );
  });

  it('Should unregister an event handler', () => {
    const eventDispatcher = new EventDispatcher();
    const eventHandler = new SendEmailWhenProductIsCreatedHandler();
    const eventName: string = 'ProductCreatedEvent';
    eventDispatcher.register(eventName, eventHandler);

    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      eventHandler
    );

    eventDispatcher.unregister(eventName, eventHandler);

    expect(eventDispatcher.getEventHandlers[eventName]).toBeDefined();
    expect(eventDispatcher.getEventHandlers[eventName].length).toBe(0);
  });

  it('Should unregister all event handlers', () => {
    const eventDispatcher = new EventDispatcher();
    const eventHandler = new SendEmailWhenProductIsCreatedHandler();
    const eventName: string = 'ProductCreatedEvent';
    eventDispatcher.register(eventName, eventHandler);

    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      eventHandler
    );

    eventDispatcher.unregisterAll();

    expect(eventDispatcher.getEventHandlers[eventName]).toBeUndefined();
  });

  it('Should notify all event handlers', () => {
    const eventDispatcher = new EventDispatcher();
    const eventHandler = new SendEmailWhenProductIsCreatedHandler();
    const spyEventHandler = jest.spyOn(eventHandler, 'handle');

    const eventName: string = 'ProductCreatedEvent';
    eventDispatcher.register(eventName, eventHandler);

    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      eventHandler
    );

    const productCreatedEvent = new ProductCreatedEvent({
      name: 'Product Name',
      description: 'Description xyz',
      price: 10.0,
    });

    // When notify is executed, the SendEmailWhenProductIsCreatedHandler.handle() must be called
    eventDispatcher.notify(productCreatedEvent);

    expect(spyEventHandler).toHaveBeenCalled();
  });
});
