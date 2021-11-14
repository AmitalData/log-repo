import { Pipe, PipeTransform } from '@angular/core';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { Offer } from '../price-check.service';

@Pipe({
  name: 'priceCheckDetails'
})
export class PriceCheckDetailsPipe implements PipeTransform {

  transform(chrageType: string, offer:Offer, cahargesTypes: ChargesTypeList[]): unknown {
    const servicesFilter =  offer.Result.Service.filter(service => service.ServiceCode == chrageType )
    return null;
  }

}
