import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class ChargesTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  ChargesTab(ShipmentType: string) {
    this.Helper.WaitBusyIndicator();

    this.Helper.WaitByIdAndClick('Quote.TH.Charges');
    this.Helper.WaitByIdAndClick('AddCharges');
    this.Helper.WaitByIdAndFill('QuoteCharge_ChargesTypeId','coc');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndClick('CostPrice');
    
    this.Helper.WaitByIdAndFill('QuoteCharge_CostUnitPrice','10');
    this.Helper.WaitByIdAndFill('QuoteCharge_Notes','Test Note');
    this.Helper.WaitByIdAndClick('OKAddCharges');
    this.Helper.WaitByIdAndClick('Quote-Save');
    this.Helper.WaitBusyIndicator();
    
    

  }


}
