export default class Address {
  _street: string = '';
  _number: string = '';
  _city: string = '';

  constructor(street: string, number: string, city: string) {
    this._street = street;
    this._number = number;
    this._city = city;
    this.validate();
  }

  validate() {
    if (this._street.length === 0) throw new Error('Street is required.');
  }
}
