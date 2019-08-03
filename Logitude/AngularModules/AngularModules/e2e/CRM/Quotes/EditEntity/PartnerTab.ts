import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PartnerTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  PartnerTab(ShipmentType: string) {
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndClick('Quote.TH.Partners');
    this.AddPartner('AGENT', 'Quote_AgentId', 'TestAgent');
    this.AddPartner('NOTFY', 'Quote_NotifyId', 'TestAgent');
  

  }

  AddPartner(partnerType: string, partnerID: string, PartnerText: string) {

    element.all(by.cssContainingText('.ToggleButton', 'Add Partners')).get(0).click();

    this.Helper.WaitByIdAndClick(partnerType);
    this.Helper.WaitByIdAndFill(partnerID, PartnerText);
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndClick('PartnerOKbtn');


  }


}