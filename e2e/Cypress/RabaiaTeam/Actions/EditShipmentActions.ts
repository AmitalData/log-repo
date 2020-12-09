import * as gr from "../General/Generator"
import { Resolvers } from "../../AymensTeam/Resolvers/Resolvers";

export function OpenShipment(dataFile: string){
    cy.fixture("ResponseData/" + dataFile + ".json").then((shipment) => {
        cy.SelectSearchBoxFirstElement("#Shipment_Search", shipment.ShipmentNumber)
    })
}

export function FillGeneralTab(){
    cy.Click("#ShipmentTHGeneral", null)
    cy.FillRandomNumber("#Shipment_GrossWeightInKG", 100, 1000, false)
    cy.SelectLogLovFirstElement("#Shipment_MoveTypeId")
}

export function FillOrdersTab(shipmentTypeCode?:string){
    cy.Click("#ShipmentTHOrders", null)
    AddPackagesOrContainers(shipmentTypeCode)
}

export function FillPartnersTab(directionCode:string, transportModeCode:string){
    cy.Click("#ShipmentTHPartners", null)
    if (directionCode === "I") {
        AddPartner("SHIPR", "Shipment_ShipperId")
    }
    if((directionCode === "E") || (directionCode === "R") || (directionCode === "D" && transportModeCode !== "I")){
        AddPartner("CONSI", "Shipment_ConsigneeId")
    }
    AddPartner("AGENT", "Shipment_AgentId")
    if((directionCode === "I" && transportModeCode === "A") || (directionCode === "D" && transportModeCode === "A")){
        AddPartner("ISSAG", "Shipment_IssuingCarrierAgentId")
    }
    AddPartner("CSAEX", "Shipment_CustomAgentExportId")
    AddPartner("CSAIM", "Shipment_CustomAgentImportId")
    AddPartner("NOTF1", "Shipment_Notify1Id")
    AddPartner("NOTF2", "Shipment_Notify2Id")
    AddPartner("SHPNT", "Shipment_ShipperNotExporterId")
    AddPartner("CONNT", "Shipment_ConsigneeNotImporterId")
    AddPartner("FRTFR", "Shipment_FreightForwarderId")
    AddPartner("COLOD", "Shipment_ColoaderId")
    AddPartner("CLERN", "Shipment_CustomClearancePointId")
    AddPartner("CONSL", "Shipment_ConsolidatorId")
    AddPartner("REAGT", "Shipment_ReleasingAgentId")
}


function AddPackagesOrContainers(shipmentTypeCode:string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 5)
    for (let i = 0; i < numberOfPackages; i++){
        cy.Click("#Orders-AddPackage", null)
        cy.FillRandomNumber("#ShipmentOrderPackage_Quantity", 1, 10, true)
        if (shipmentTypeCode === "FCLD" || shipmentTypeCode === "FTL" || shipmentTypeCode === "LCLD" || shipmentTypeCode === "LTL"){
            cy.SelectLogLovFirstElement("#ShipmentOrderPackage_PackageTypeId")
            cy.FillRandomNumber("#ShipmentOrderPackage_GrossWeight", 1, 50, true)
        }
        cy.Click("#OrderOKbtn", null)
    }
}

function AddPartner(partnerTypeId: string, partnerFieldId: string) {
    cy.Click("label", "Add Partners")
    cy.Click(("#" + partnerTypeId), null)
    cy.SelectLogLovFirstElement(("#" + partnerFieldId))
    cy.Click("#PartnerOKbtn", null)
}