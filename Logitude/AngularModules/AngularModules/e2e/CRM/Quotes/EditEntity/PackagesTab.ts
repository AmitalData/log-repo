import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PackagesTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  PackagesTab(ShipmentType: string) {
    this.Helper.WaitBusyIndicator();

    this.Helper.WaitByIdAndClick('Quote.TH.Packages');
    this.Helper.WaitByIdAndClick('AddPackage');
    this.Helper.WaitByIdAndFill('QuotePackage_Quantity','2');
    this.Helper.WaitByIdAndFill('QuotePackage_Length','100');
    this.Helper.WaitByIdAndFill('QuotePackage_Width','100');
    this.Helper.WaitByIdAndFill('QuotePackage_Height','100');
    this.Helper.WaitByIdAndFill('QuotePackage_GrossWeight','1000');
    this.Helper.WaitByIdAndClick('OkAddPackage');
    

  //  this.Helper.WaitByIdAndClick('Quote-Save');
  }


}
