import { toXML } from 'jstoxml';
import { OutputListProductDto } from '../../../usecase/product/list/list.product.dto';

export default class productPresenter {
  static listXML(data: OutputListProductDto): string {
    const xmlOption = {
      header: true,
      indent: '  ',
      newline: '\n',
      allowEmpty: true,
    };

    return toXML(
      {
        products: {
          product: data.products.map((product) => ({
            id: product.id,
            name: product.name,
            price: product.price,
          })),
        },
      },
      xmlOption
    );
  }
}
