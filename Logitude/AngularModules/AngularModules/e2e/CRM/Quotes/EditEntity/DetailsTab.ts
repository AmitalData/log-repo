import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class DetailsTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  DetailsTab(ShipmentType: string) {
    this.Helper.WaitBusyIndicator();

    this.Helper.WaitByIdAndClick('Quote.TH.Details');
    this.Helper.WaitByIdAndFill('Quote_TransitTime','11:30');
    this.Helper.WaitByIdAndFill('date_Quote_StartDate','10');
    this.Helper.WaitByIdAndFill('Quote_ExpirationDays','10');
  //  this.Helper.WaitByIdAndClick('Quote-Save');
  }


}
