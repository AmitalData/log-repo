import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PartnerTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  PartnerTab(ShipmentType: string) {

    this.Helper.WaitByIdAndClick('Quote.TH.Partners');
 
  }


}