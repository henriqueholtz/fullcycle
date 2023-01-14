import SendEmailWhenProductIsCreatedHandler from '../Product/handlers/send-email-when-product-is-created.handler';
import EventDispatcher from './event-dispatcher';

describe('Domain Event tests', () => {
  it('Should register an event handler', () => {
    const eventDispatcher = new EventDispatcher();
    const eventHandler = new SendEmailWhenProductIsCreatedHandler();
    const eventName: string = 'ProducCreatedEvent';
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
    const eventName: string = 'ProducCreatedEvent';
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
    const eventName: string = 'ProducCreatedEvent';
    eventDispatcher.register(eventName, eventHandler);

    expect(eventDispatcher.getEventHandlers[eventName][0]).toMatchObject(
      eventHandler
    );

    eventDispatcher.unregisterAll();

    expect(eventDispatcher.getEventHandlers[eventName]).toBeUndefined();
  });
});
