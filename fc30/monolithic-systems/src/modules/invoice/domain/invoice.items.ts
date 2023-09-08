import AggregateRoot from '../../@shared/domain/entity/aggregate-root.interface';
import BaseEntity from '../../@shared/domain/entity/base.entity';
import Id from '../../@shared/domain/value-object/id.value-object';

type InvoiceProps = {
  id?: Id;
  name: string;
  price: number;
};

export default class InvoiceItems extends BaseEntity implements AggregateRoot {
  private _name: string;
  private _price: number;

  constructor(props: InvoiceProps) {
    super(props.id);
    this._name = props.name;
    this._price = props.price;
    this.validate();
  }

  validate(): void {
    if (this._price <= 0) {
      throw new Error('Price must be greater than 0!');
    }
    if (this._name.length <= 0) {
      throw new Error('Name is required!');
    }
  }

  get name(): string {
    return this._name;
  }

  get price(): number {
    return this._price;
  }
}
