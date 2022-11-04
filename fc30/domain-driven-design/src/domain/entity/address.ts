export default class Address {
  _street: string = '';
  _number: string = '';
  _city: string = '';
  _zip: string = '';

  constructor(street: string, number: string, zip: string, city: string) {
    this._street = street;
    this._number = number;
    this._zip = zip;
    this._city = city;
    this.validate();
  }

  get street(): string {
    return this._street;
  }

  get number(): string {
    return this._number;
  }
  get zip(): string {
    return this._zip;
  }
  get city(): string {
    return this._city;
  }

  validate() {
    if (this._street.length === 0) throw new Error('Street is required.');
  }
}
