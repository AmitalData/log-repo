import * as gr from "../../../Base/cypress/Actions/GenerateRandoms"
import { Selectors } from "../selectors/Selectors"

export function OpenNewShipmentWizard(levelCode: string){
    cy.Click("#HelperNotesButton_0_0", null)
    cy.Click(".LogitudeToggleButtonItem", levelCode)
}

export function FillShipmentDefaultFields(directionCode: string, transportModCode: string, shipmentTypeCode: string = null){
    cy.ClickRadio("#DirectionRadio_0" + directionCode)
    cy.ClickRadio("#TransportModeRadio_0" + transportModCode)

    if(shipmentTypeCode){
        cy.ClickRadio("#ShipmentTypeRadio_0" + shipmentTypeCode)
    }

    if(directionCode === "D" && transportModCode === "I"){
        cy.SelectLogLovFirstElement("#Shipment_ShipperId", false)
        cy.SelectLogLovFirstElement("#Shipment_ConsigneeId", false)
    }else{
        if(directionCode === "I"){
            cy.SelectLogLovRandomElement("#Shipment_ConsigneeId", false, 5)
        }else{
            cy.SelectLogLovRandomElement("#Shipment_ShipperId", false, 5)
        }

        if(directionCode === "D"){
            cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId", false)
            cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId", false)
        }else{
            cy.SelectLogLovRandomElement("#Shipment_MainCarriageFromPortId", false, 5)
            cy.SelectLogLovRandomElement("#Shipment_MainCarriageToPortId", false, 5)
        }
    }
}

export function SaveShipment(resultFile: string) {
    cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", resultFile)
}

export function CreateShipment(){
    cy.DefineRequestWait("POST", "**/shipment", "WaitPostShipmentRequest")
    cy.Click("#ShipmentCreatebtn", null)
}

export function ValidateCreatedShipment(resultFile: string){
    cy.AssertResponseStatusCode("WaitPostShipmentRequest", 200, resultFile)
}

export function OpenShipment(dataFile: string){
    cy.fixture(dataFile).then((shipment) => {
        cy.SelectQuickSearchFirstElement(Selectors.ShipmentSearchBar, shipment.ShipmentNumber)
    })
}

export function FillGeneralTab(){
    cy.FillRandomNumber(Selectors.ShipmentGrossWeight, 100, 1000)
    cy.SelectLogLovRandomElement(Selectors.ShipmentMoveType, true, 5)
}

export function FillOrdersTab(shipmentTypeCode?:string){
    AddPackagesOrContainersForOrdersTab(shipmentTypeCode)
}

export function FillPartnersTab(directionCode:string, transportModeCode:string){
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

export function FillPackagesTab(transportModeCode:string, shipmentTypeCode?:string){
    AddPackagesOrContainersForPackagesTab(transportModeCode, shipmentTypeCode)
}


function AddPackagesOrContainersForOrdersTab(shipmentTypeCode:string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 3)
    for (let i = 0; i < numberOfPackages; i++){
        cy.Click(Selectors.OrdersAddPackage, null)
        cy.FillRandomNumber(Selectors.OrderPackageQuantity, 1, 10)
        if (shipmentTypeCode === "FCLD" || shipmentTypeCode === "FTL" || shipmentTypeCode === "LCLD" || shipmentTypeCode === "LTL"){
            cy.SelectLogLovRandomElement(Selectors.OrderPackageType, true, 5)
            cy.FillRandomNumber(Selectors.OrderPackageGrossWeight, 1, 50)
        }
        cy.Click(Selectors.OrderOKButton, null)
    }
}

function AddPartner(partnerTypeId: string, partnerFieldId: string) {
    cy.Click("label", "Add Partners")
    
    cy.get("#" + partnerTypeId).then((btn) => {
        if(!btn.is('[disabled]')) {
            cy.Click(("#" + partnerTypeId), null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId + "']"
            cy.SelectLogLovRandomElement(partnerFieldSelector, false, 10)
            cy.Click(Selectors.PartnerOKButton, null)
        }
    })
}

function AddPackagesOrContainersForPackagesTab(transportModeCode:string, shipmentTypeCode:string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 3)
    for (let i = 0; i < numberOfPackages; i++){
        cy.Click(Selectors.AddPackage, null)
        if(transportModeCode !== "A"){
            cy.SelectLogLovRandomElement(Selectors.PackageType, true, 5)
        }
        if(shipmentTypeCode !== "FCLD" && shipmentTypeCode !== "FTL"){
            cy.FillRandomNumber(Selectors.PackageQuantity, 1, 10)
        }
        if(transportModeCode !== "A"){
            cy.FillRandomNumber(Selectors.PackageWeight, 1, 10)
        }
        if(transportModeCode === "A"){
            cy.Click(Selectors.AirPackageOKButton, null)
        }else{
            cy.Click(Selectors.OceanPackageOKButton, null)
        }
    }
}