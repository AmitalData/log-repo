import { Pipe, PipeTransform } from '@angular/core';
import { ProductTypeList } from 'Common/EntityLists/ProductTypeList';

@Pipe({
  name: 'iconSrc'
})
export class IconSrcPipe implements PipeTransform {

  transform(productCode: string, productTypeList: ProductTypeList[]): string {
    if(!productCode || productTypeList.length === 0) return '';

    let transportType = '';
    const productType: ProductTypeList = productTypeList.find(x => x.Code === productCode)

    if (productType.Name.includes('Air'))
      transportType = 'airplane'
    else if (productType.Name.includes('Inland'))
      transportType = 'inland'
    else if (productType.Name.includes('Ocean'))
      transportType = 'ocean'

    return `assets/icons/${transportType}-blue.png`;
  }
}
