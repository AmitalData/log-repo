import * as gr from "../../../Base/cypress/Actions/GenerateRandoms"
import { Selectors } from "../selectors/Selectors"
import { ShipmentDetails } from "../models/ShipmentDetails";

export function OpenNewShipmentWizard(levelCode: string){
    cy.Click("#HelperNotesButton_0_0", null)
    cy.Click(".LogitudeToggleButtonItem", levelCode)
}

export function FillShipmentDefaultFields(shipmentDetails: ShipmentDetails){
    cy.ClickRadio("#DirectionRadio_0" + shipmentDetails.Direction)
    cy.ClickRadio("#TransportModeRadio_0" + shipmentDetails.TransportMode)
    
    if(shipmentDetails.ShipmentType){
        if(shipmentDetails.ShipmentType === "Groupage"){
            cy.ClickRadio("#ShipmentTypeRadio_0MyG" + shipmentDetails.TransportMode);
        }else{
            cy.ClickRadio("#ShipmentTypeRadio_0" + shipmentDetails.ShipmentType)
        }
    }

    if(shipmentDetails.Direction === "D" && shipmentDetails.TransportMode === "I"){
        cy.FillLogLov("#Shipment_ShipperId", shipmentDetails.Shipper, false)
        cy.FillLogLov("#Shipment_ConsigneeId", shipmentDetails.Consignee, false)
    }else{
        if(shipmentDetails.ShipmentLevel === "Master"){
            cy.FillLogLov("#Master_AgentId", shipmentDetails.Agent, false)
        }else{
            if(shipmentDetails.Direction === "I"){
                cy.FillLogLov("#Shipment_ConsigneeId", shipmentDetails.Consignee, false)
            }else{
                cy.FillLogLov("#Shipment_ShipperId", shipmentDetails.Shipper, false)
            }
        }

        let portsPreSelector = shipmentDetails.ShipmentLevel === "Master" ? "#Master" : "#Shipment";

        cy.FillLogLov((portsPreSelector + "_MainCarriageFromPortId"), shipmentDetails.MainCarriageFromPort, false)
        cy.FillLogLov((portsPreSelector + "_MainCarriageToPortId"), shipmentDetails.MainCarriageToPort, false)
    }
}

export function CreateShipment(levelCode: string){
    let createPreSelector = levelCode === "Master" ? "#Master" : "#Shipment";
    cy.DefineRequestWait("POST", "**/shipment", "WaitPostShipmentRequest")
    cy.Click(createPreSelector + "Createbtn", null)
}

export function UpdateShipment(saveButtonSelector: string){
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
    cy.Click(saveButtonSelector, null)
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

export function FillReceivablesTab() {
    cy.Click(Selectors.AddNewReceivableLine, null)
    cy.get(Selectors.ReceivableChargesType).focus().clear().type("{downarrow}{downarrow}{enter}")
    cy.FillRandomNumber(Selectors.ReceivableUnitPrice, 100, 100)
    cy.get(Selectors.ReceivableTotalAmount).should('have.value', '100.00')
    cy.Click(Selectors.AddReceivableOkButton, null) 
}

export function FillPickupRouting(){
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.PickUp, null)
    cy.AssertResponseStatusCode("WaitCardViewsRequest", 200, null)
    cy.AssertResponseStatusCode("WaitAddressViewsRequest", 200, null)
}

export function FillDeliveryRouting(){
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.Delivery, null)
    cy.AssertResponseStatusCode("WaitCardViewsRequest", 200, null)
    cy.AssertResponseStatusCode("WaitAddressViewsRequest", 200, null)
    cy.get(Selectors.ShipmentPickUpDelivery_ToPartnerCardId).then((input) => {
        if(input.text() === "" || input.text() === null) {
            cy.SelectLogLovRandomElement(Selectors.ShipmentPickUpDelivery_ToPartnerCardId, false, 5)
        }
    })
}

export function FillPreCarriageRouting(transportModeCode:string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.PreCarriage).then((btn) => {
        if(!btn.is('[disabled]')) {
            cy.Click(Selectors.PreCarriage, null)
            cy.FillLogLov(Selectors.Shipment_PreCarriageTransportModeId, transportModeCode, true)
            cy.SelectLogLovFirstElement(Selectors.Shipment_PreCarriageFromPortId, false)
            cy.SelectLogLovFirstElement(Selectors.Shipment_PreCarriageToPortId, false)
            cy.Click(Selectors.PreCarriageOKBtn, null)
        }else{
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportModeCode:string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.OnCarriage).then((btn) => {
        if(!btn.is('[disabled]')) {
            cy.Click(Selectors.OnCarriage, null)
            cy.FillLogLov(Selectors.Shipment_OnCarriageTransportModeId, transportModeCode, true)
            cy.SelectLogLovFirstElement(Selectors.Shipment_OnCarriageFromPortId, false)
            cy.SelectLogLovFirstElement(Selectors.Shipment_OnCarriageToPortId, false)
            cy.Click(Selectors.OnCarriageOKBtn, null)    
        }else{
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab() {
    cy.Click(Selectors.AddNewPayableLine , null)
    cy.SelectLogLovFirstElement(Selectors.ShipmentPayable_ChargesTypeId, true)
    cy.SelectLogLovFirstElement(Selectors.ShipmentPayable_MeasurementId, true)
    cy.SelectLogLovFirstElement(Selectors.ShipmentPayable_CurrencyId, true)
    cy.Click(Selectors.AddPayableOkButton , null)
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