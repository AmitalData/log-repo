import { Resolvers } from "../../Resolvers/Resolvers";

export class PartnersTab {
    private direction: string;
    private transportMode: string;
    private isInlandDomestic: boolean = false;
    private levelCode: string;

    public RunPartnersTabScenarios(levelCode: string, direction: string, transportMode: string,) {
        this.direction = direction;
        this.transportMode = transportMode;
        this.levelCode = levelCode;

        if (this.direction == "D" && this.transportMode == "I") {
            this.isInlandDomestic = true;
        }
        this.GoToPartnerTab();
        if (this.levelCode == 'M') {
            this.AddMasterPartners();
        } else {
            this.AddPartners();
        }
        this.DeletePartner();
    }
    private GoToPartnerTab() {
        cy.get('#ShipmentTHPartners').click();
    } 
    private AddPartners() {
        if (this.direction == 'I') {
            this.FillPartner('#SHIPR', '#Shipment_ShipperId', 'TestShipper');
            this.FillPartner('#ISSAG', '#Shipment_IssuingCarrierAgentId', 'TestIssuingCarrierAgentExport1');
        } else if (!this.isInlandDomestic){
            this.FillPartner('#CONSI', '#Shipment_ConsigneeId', 'Testcons');
        }
        this.FillPartner('#AGENT', '#Shipment_AgentId', 'TestAgent');
        this.FillPartner('#CSAEX', '#Shipment_CustomAgentExportId', 'TestCustomsAgentExport1');
        this.FillPartner('#CSAIM', '#Shipment_CustomAgentImportId', 'TestCustomsAgentImport1');
        this.FillPartner('#NOTF1', '#Shipment_Notify1Id', 'TestNotify1IdExport1');
        this.FillPartner('#NOTF2', '#Shipment_Notify2Id', 'TestNotify2IdExport1');
        this.FillPartner('#SHPNT', '#Shipment_ShipperNotExporterId', 'TestShipperNotExporterExport1');
        this.FillPartner('#CONNT', '#Shipment_ConsigneeNotImporterId', 'TestConsigneeNotImporterExport1');
        this.FillPartner('#FRTFR', '#Shipment_FreightForwarderId', 'TestFreightForwarderExport1');
        this.FillPartner('#COLOD', '#Shipment_ColoaderId', 'TestColoaderExport1');
        this.FillPartner('#CLERN', '#Shipment_CustomClearancePointId', 'TestCustomClearancePointExport1');
        this.FillPartner('#CONSL', '#Shipment_ConsolidatorId', 'TestConsolidatorExport1');
    }
    private AddMasterPartners() {
        if (this.direction == 'I') {
            this.FillPartner('#ISSAG', '#Shipment_IssuingCarrierAgentId', 'TestIssuingCarrierAgentExport1');
        }
        this.FillPartner('#CSAEX', '#Shipment_CustomAgentExportId', 'TestCustomsAgentExport1');
        this.FillPartner('#CSAIM', '#Shipment_CustomAgentImportId', 'TestCustomsAgentImport1');
        this.FillPartner('#NOTF1', '#Shipment_Notify1Id', 'TestNotify1IdExport1');
        this.FillPartner('#NOTF2', '#Shipment_Notify2Id', 'TestNotify2IdExport1');
        this.FillPartner('#SHPNT', '#Shipment_ShipperNotExporterId', 'TestShipperNotExporterExport1');
        this.FillPartner('#CONNT', '#Shipment_ConsigneeNotImporterId', 'TestConsigneeNotImporterExport1');
        this.FillPartner('#FRTFR', '#Shipment_FreightForwarderId', 'TestFreightForwarderExport1');
        this.FillPartner('#COLOD', '#Shipment_ColoaderId', 'TestColoaderExport1');
        this.FillPartner('#CLERN', '#Shipment_CustomClearancePointId', 'TestCustomClearancePointExport1');
        this.FillPartner('#CONSL', '#Shipment_ConsolidatorId', 'TestConsolidatorExport1');

    }
    private DeletePartner() {
        Resolvers.ButtonResolver.Selector('#Delete-CustomAgentImport').ThenConfirmButtonText("Yes").Click();
    }
    private FillPartner(partnertypeId: string, partnerFieldId: string, name: string) {
        cy.contains('label', 'Add Partners').click({ force: true });
        Resolvers.ButtonResolver.Selector(partnertypeId).Click();
        Resolvers.LOVResolver.Selector(partnerFieldId).Type(name);
        Resolvers.ButtonResolver.Selector('#PartnerOKbtn').Click();
    }
}