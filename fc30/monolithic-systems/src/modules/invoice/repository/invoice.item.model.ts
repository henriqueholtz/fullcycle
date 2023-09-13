import { Model } from 'sequelize';
import { BelongsTo, Column, PrimaryKey, Table } from 'sequelize-typescript';
import { InvoiceModel } from './invoice.model';

@Table({
  tableName: 'invoiceitems',
  timestamps: false,
})
export class InvoiceItemModel extends Model {
  @PrimaryKey
  @Column({ allowNull: false })
  id: string;

  @Column({ allowNull: false })
  name: string;

  @Column({ allowNull: false })
  price: number;
}

// InvoiceItemModel.belongsTo(InvoiceModel);
