import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class OverviewTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  OverviewTab(ShipmentType: string) {

    this.Helper.WaitByIdAndClick('Quote.TH.Overview');
    this.Helper.WaitByIdAndFill('Quote_Notes','Test From Lana');
  }


}

