import {
  BelongsTo,
  BelongsToMany,
  Column,
  ForeignKey,
  HasMany,
  HasOne,
  Model,
  PrimaryKey,
  Table,
} from 'sequelize-typescript';
import { InvoiceItemModel } from './invoice.item.model';
import { InvoiceAddressModel } from './invoice.address.model';

@Table({
  tableName: 'invoices',
  timestamps: false,
})
export class InvoiceModel extends Model {
  @PrimaryKey
  @Column({ allowNull: false })
  id: string;

  @Column({ allowNull: false })
  name: string;

  @Column({ allowNull: false })
  document: string;

  @Column({ allowNull: false })
  createdAt: Date;

  @Column({ allowNull: false })
  updatedAt: Date;

  @ForeignKey(() => InvoiceAddressModel)
  @Column({ allowNull: false })
  addressId: string;

  @HasOne(() => InvoiceAddressModel)
  address: InvoiceAddressModel;

  @HasMany(() => InvoiceItemModel)
  items: InvoiceItemModel[];
}
