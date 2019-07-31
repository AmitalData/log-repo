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


      this.AddPartner('AGENT', 'Shipment_AgentId', 'TestAgent');
      this.AddPartner('CSAEX', 'Shipment_CustomAgentExportId', 'TestCustomsAgentExport1');
      this.AddPartner('CSAIM', 'Shipment_CustomAgentImportId', 'TestCustomsAgentImport1');

      this.AddPartner('NOTF1', 'Shipment_Notify1Id', 'TestNotify1IdExport1');
      this.AddPartner('NOTF2', 'Shipment_Notify2Id', 'TestNotify2IdExport1');

      this.AddPartner('SHPNT', 'Shipment_ShipperNotExporterId', 'TestShipperNotExporterExport1');
      this.AddPartner('CONNT', 'Shipment_ConsigneeNotImporterId', 'TestConsigneeNotImporterExport1');
      this.AddPartner('FRTFR', 'Shipment_FreightForwarderId', 'TestFreightForwarderExport1');

      this.AddPartner('COLOD', 'Shipment_ColoaderId', 'TestColoaderExport1');
      this.AddPartner('CLERN', 'Shipment_CustomClearancePointId', 'TestCustomClearancePointExport1');
      this.AddPartner('CONSL', 'Shipment_ConsolidatorId','TestConsolidatorExport1');


      // browser.driver.sleep(1000);
      // this.EditPartner('Edit-Agent', 'Shipment_AgentId');
      this.DeletePartner('Delete-Agent');

    }
    else if (shipmentType == 'M') {

      this.AddPartner('NOTF1', 'Shipment_Notify1Id', 'TestNotify1IdExport1');
      this.AddPartner('NOTF2', 'Shipment_Notify2Id', 'TestNotify2IdExport1');
      this.AddPartner('SHPNT', 'Shipment_ShipperNotExporterId', 'TestShipperNotExporterExport1');
      this.AddPartner('CONNT', 'Shipment_ConsigneeNotImporterId', 'TestConsigneeNotImporterExport1');
    }

  }

  AddPartner(partnerType: string, partnerID: string, PartnerText: string) {

    element.all(by.cssContainingText('.ToggleButton', 'Add Partners')).get(0).click();

    this.Helper.WaitByIdAndClick(partnerType);
    this.Helper.WaitByIdAndFill(partnerID, PartnerText);
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

