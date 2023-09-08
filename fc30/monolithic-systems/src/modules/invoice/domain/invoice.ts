import AggregateRoot from '../../@shared/domain/entity/aggregate-root.interface';
import BaseEntity from '../../@shared/domain/entity/base.entity';
import Id from '../../@shared/domain/value-object/id.value-object';
import Address from './invoice.address';
import InvoiceItems from './invoice.items';

type InvoiceProps = {
  id?: Id;
  name: string;
  document: string;
  address: Address;
  items: InvoiceItems[];
  createdAt?: Date;
  updatedAt?: Date;
};

export default class Invoice extends BaseEntity implements AggregateRoot {
  private _name: string;
  private _document: string;
  private _address: Address;
  private _items: InvoiceItems[];

  constructor(props: InvoiceProps) {
    super(props.id);
    this._name = props.name;
    this._document = props.document;
    this._address = props.address;
    this._items = props.items;
    this.validate();
  }

  validate(): void {
    // if (this._amount <= 0) {
    //   throw new Error('Amount must be greater than 0');
    // }
  }

  process(): void {
    //   if (this._amount >= 100) {
    //     this.approve();
    //   } else {
    //     this.decline();
    //   }
  }

  get name(): string {
    return this._name;
  }

  get document(): string {
    return this._document;
  }

  get address(): Address {
    return this._address;
  }

  get items(): InvoiceItems[] {
    return this._items;
  }

  get total(): number {
    return this._items.reduce((partialSum, i) => partialSum + i.price, 0);
  }
}
