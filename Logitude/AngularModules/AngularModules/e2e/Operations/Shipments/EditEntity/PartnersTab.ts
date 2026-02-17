import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PartnersTabComponent {
  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();
  }


  PartnersTab(shipmentType: string) {
this.Helper.WaitBusyIndicator();
    this.Helper.WaitByIdAndClick('Shipment.TH.Partners');
    if (shipmentType == 'D' || shipmentType == 'H') {


      this.AddPartner('AGENT', 'Shipment_AgentId');
      this.AddPartner('NOTF1', 'Shipment_Notify1Id');
      this.AddPartner('NOTF2', 'Shipment_Notify2Id');
      this.AddPartner('SHPNT', 'Shipment_ShipperNotExporterId');
      this.AddPartner('CONNT', 'Shipment_ConsigneeNotImporterId');
      this.AddPartner('FRTFR', 'Shipment_FreightForwarderId');
      // browser.driver.sleep(1000);
      // this.EditPartner('Edit-Agent', 'Shipment_AgentId');
      this.DeletePartner('Delete-Agent');

    }
    else if (shipmentType == 'M') {

      this.AddPartner('NOTF1', 'Shipment_Notify1Id');
      this.AddPartner('NOTF2', 'Shipment_Notify2Id');
      this.AddPartner('SHPNT', 'Shipment_ShipperNotExporterId');
      this.AddPartner('CONNT', 'Shipment_ConsigneeNotImporterId');
    }

  }

  AddPartner(partnerTypeID: string, partnerNameID: string) {

    element.all(by.cssContainingText('.ToggleButton', 'Add Partners')).get(0).click();

    this.Helper.WaitByIdAndClick(partnerTypeID);
    this.Helper.WaitByIdAndFill(partnerNameID, 'c');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndClick('PartnerOKbtn');


  }
  EditPartner(partnerTypeID: string, partnerNameID: string) {
    this.Helper.WaitByIdAndClick(partnerTypeID);
    element(by.id(partnerNameID)).clear();
    this.Helper.WaitByIdAndFill(partnerNameID, 'age');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndClick('PartnerOKbtn');
  }
  DeletePartner(partnerTypeID: string) {
    this.Helper.WaitByIdAndClick(partnerTypeID);
    this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
  }

}

