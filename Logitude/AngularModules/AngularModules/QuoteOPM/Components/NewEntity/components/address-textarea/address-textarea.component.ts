import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { AddressList } from 'Common/EntityLists/AddressList';

@Component({
  selector: 'app-address-textarea',
  templateUrl: './address-textarea.component.html',
  styleUrls: ['./address-textarea.component.scss']
})
export class AddressTextareaComponent {
  @Input() Address: AddressList = null as any;
  addressTextarea: string = '';

  constructor() { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes?.Address)
      this.initAddress();
  }

  initAddress() {
    const address: AddressList = this.Address;
    if(!address) return;
    
    const cityLine: string = this.GetCityLineText(address);
    const br = '\r\n';

    let addressFinal: string = '';
    if (address.Address1)
      addressFinal += address.Address1;
    if (address.Address2)
      addressFinal += br + address.Address2;
    if (cityLine)
      addressFinal += br + cityLine;
    if (address.CountryName)
      addressFinal += br + address.CountryName;

    this.addressTextarea = addressFinal;
  }

  GetCityLineText(address: AddressList): string {
    var myResult = "";

    if (address != null) {
      if (address.City)
        myResult = address.City;

      if (address.StateName)
        myResult += myResult ? ", " + address.StateName : address.StateName;

      if (address.ZipCode)
        myResult += myResult ? ", " + address.ZipCode : address.ZipCode;
    }

    return myResult;
  }

}
