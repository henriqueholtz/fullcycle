import IValueObject from './value-object.interface';
import { v4 as uuidv4 } from 'uuid';

export default class Id implements IValueObject {
  private _id: string;

  constructor(id?: string) {
    this._id = id || uuidv4();
  }

  get id(): string {
    return this._id;
  }
}
