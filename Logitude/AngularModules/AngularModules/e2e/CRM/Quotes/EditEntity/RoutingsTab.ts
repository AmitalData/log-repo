import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class RoutingsTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  RoutingsTab(ShipmentType: string) {
    this.Helper.WaitBusyIndicator();

    this.Helper.WaitByIdAndClick('Quote.TH.Routings');
    this.Helper.WaitByIdAndFill('date_Quote_ETD','1');
    this.Helper.WaitByIdAndFill('date_Quote_ETA','2');
    this.Helper.WaitByIdAndClick('IncludePickUp');
    

  }


}
