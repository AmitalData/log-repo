"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var PartnersTabComponent = /** @class */ (function () {
    function PartnersTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    PartnersTabComponent.prototype.PartnersTab = function (shipmentType) {
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
            this.AddPartner('CONSL', 'Shipment_ConsolidatorId', 'TestConsolidatorExport1');
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
    };
    PartnersTabComponent.prototype.AddPartner = function (partnerType, partnerID, PartnerText) {
        protractor_1.element.all(protractor_1.by.cssContainingText('.ToggleButton', 'Add Partners')).get(0).click();
        this.Helper.WaitByIdAndClick(partnerType);
        this.Helper.WaitByIdAndFill(partnerID, PartnerText);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndClick('PartnerOKbtn');
    };
    PartnersTabComponent.prototype.EditPartner = function (partnerTypeID, partnerNameID) {
        this.Helper.WaitByIdAndClick(partnerTypeID);
        protractor_1.element(protractor_1.by.id(partnerNameID)).clear();
        this.Helper.WaitByIdAndFill(partnerNameID, 'age');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndClick('PartnerOKbtn');
    };
    PartnersTabComponent.prototype.DeletePartner = function (partnerTypeID) {
        this.Helper.WaitByIdAndClick(partnerTypeID);
        this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
    };
    return PartnersTabComponent;
}());
exports.PartnersTabComponent = PartnersTabComponent;
//# sourceMappingURL=PartnersTab.js.map