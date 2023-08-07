import EventHandlerInterface from '../../../@shared/event/event-handler.interface';
import CustomerAddressChangedEvent from '../customer-address-changed.event';

export default class SendConsoleLogHandler
  implements EventHandlerInterface<CustomerAddressChangedEvent>
{
  handle(event: CustomerAddressChangedEvent): void {
    const address = `${event.eventData?.address?.street}, ${event.eventData?.address?.zipCode} - ${event.eventData?.address?.city}`;
    console.log(
      `Endereço do cliente: ${event.eventData?.id} - ${event.eventData?.name} alterado para: ${address}`
    );
  }
}
