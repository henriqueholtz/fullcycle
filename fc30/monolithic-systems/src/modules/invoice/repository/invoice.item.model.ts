import {
  BelongsTo,
  Column,
  ForeignKey,
  Model,
  PrimaryKey,
  Table,
} from 'sequelize-typescript';
import { InvoiceModel } from './invoice.model';

@Table({
  tableName: 'invoiceitems',
  timestamps: false,
})
export class InvoiceItemModel extends Model {
  @PrimaryKey
  @Column({ allowNull: false })
  id: string;

  @ForeignKey(() => InvoiceModel)
  @Column({ allowNull: false })
  invoiceId: string;

  @BelongsTo(() => InvoiceModel)
  invoice: InvoiceModel;

  @Column({ allowNull: false })
  name: string;

  @Column({ allowNull: false })
  price: number;
}
