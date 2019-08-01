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
    
  //  this.Helper.WaitByIdAndClick('Quote-Save');
  }


}
