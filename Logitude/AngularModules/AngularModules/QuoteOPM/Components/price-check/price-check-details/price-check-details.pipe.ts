import { Pipe, PipeTransform } from '@angular/core';
import { ChargesTypeList } from 'Common/EntityLists/ChargesTypeList';
import { Offer, Service } from '../price-check.service';

@Pipe({
  name: 'priceCheckDetails'
})
export class PriceCheckDetailsPipe implements PipeTransform {

  transform(chrageType: string, offer:Offer, cahargesTypes: ChargesTypeList[]): Service[] {
    // if(chrageType === 'not include') {
    //   const allCodes: string[] = cahargesTypes.filter(r=>  ['Frighte Charges','Origin Charges','Destination Charges'].includes(r.QuoteGroupSectionID)).map(x=> x.Code)
    //   const servicesFilter =  offer.Result.Service.filter(service => !(allCodes.includes(service.ServiceCode)))
    // } else {
    //   const codes: string[] = cahargesTypes.filter(r=> r.QuoteGroupSectionID ).map(x=> x.Code)
    //   const servicesFilter: Service[] =  offer.Result.Service.filter(service => codes.includes(service.ServiceCode))
    // }


    //test
    const servicesFilter: Service[] =  offer.Result.Service.filter((value: Service, i: number) => ['Frighte Charges','Origin Charges','Destination Charges'].indexOf(chrageType) == i)
    if(chrageType === 'not include')
      servicesFilter.push(offer.Result.Service[0])

    return servicesFilter;
  }
}
