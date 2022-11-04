import Address from './address';

export default class Customer {
  private _id: string;
  private _name: string;
  private _address: Address | undefined;
  private _active: boolean = false;
  private _rewardPoints: number = 0;

  constructor(id: string, name: string, address: Address | undefined) {
    this._id = id;
    this._name = name;
    this._address = address;
    this.validate();
  }

  validate() {
    if (this._id.length === 0) throw new Error('Id is required!');
    if (this._name.length === 0) throw new Error('Name is required!');
  }

  get name(): string {
    return this._name;
  }

  get id(): string {
    return this._id;
  }

  get rewardPoints(): number {
    return this._rewardPoints;
  }

  get Address(): Address {
    return this._address;
  }

  changeName(name: string) {
    this._name = name;
    this.validate();
  }

  isActive(): boolean {
    return this._active;
  }

  activate() {
    if (this._address === undefined)
      throw new Error('Address is required to activate the customer!');

    this._active = true;
  }

  deactivate() {
    this._active = false;
  }

  addRewardPoints(points: number) {
    this._rewardPoints += points;
  }
}
