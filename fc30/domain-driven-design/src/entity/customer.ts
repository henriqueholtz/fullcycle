import Address from './address';

export default class Customer {
  _id: string;
  _name: string;
  _address: Address;
  _active: boolean = true;

  constructor(id: string, name: string, address: Address) {
    this._id = id;
    this._name = name;
    this._address = address;
    this.validate();
  }

  validate() {
    if (this._name.length === 0) throw new Error('Name is required');
  }

  activate() {
    this._active = true;
  }

  deactivate() {
    this._active = false;
  }
}
