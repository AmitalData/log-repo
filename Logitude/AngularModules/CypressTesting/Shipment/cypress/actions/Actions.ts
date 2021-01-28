import * as gr from "../../../Base/cypress/Actions/GenerateRandoms"
import { Selectors } from "../selectors/Selectors"
import { ShipmentDetails } from "../models/ShipmentDetails";
import * as Assertions from "./Assertions";
import { PartnersDetails } from "cypress/models/PartnersDetails";

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

export function FillGeneralTab(MoveType: string){
    cy.FillRandomNumber(Selectors.ShipmentGrossWeight, 100, 1000)
    cy.SelectLogLovElement(Selectors.ShipmentMoveType, true, MoveType)
}

export function FillOrdersTab(shipmentTypeCode?:string, PackageType?: string){
    AddPackagesOrContainersForOrdersTab(shipmentTypeCode, PackageType)
}

export function FillPartnersTab(directionCode:string, transportModeCode:string, partnersDetails: PartnersDetails){
    if (directionCode === "I") {
        AddPartner("SHIPR", "Shipment_ShipperId")
    }
    if((directionCode === "E") || (directionCode === "R") || (directionCode === "D" && transportModeCode !== "I")){
        AddPartner("CONSI", "Shipment_ConsigneeId")
    }
    AddPartner("AGENT", "Shipment_AgentId", partnersDetails.Agent)
    if((directionCode === "I" && transportModeCode === "A") || (directionCode === "D" && transportModeCode === "A")){
        AddPartner("ISSAG", "Shipment_IssuingCarrierAgentId")
    }
    AddPartner("CSAEX", "Shipment_CustomAgentExportId", partnersDetails.CustomsAgentExport)
    AddPartner("CSAIM", "Shipment_CustomAgentImportId", partnersDetails.CustomsAgentImport)
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

export function FillHouseInShipmentsTab(Shipper: string){
    cy.SelectLogLovElement("#Shipment_CustomerId", true, Shipper)
}

export function FillPackagesTab(transportModeCode:string, shipmentTypeCode?:string, PackageType?:string){
    AddPackagesOrContainersForPackagesTab(transportModeCode, shipmentTypeCode, PackageType)
}

export function FillReceivablesTab(ChargesType: string) {
    cy.Click(Selectors.AddNewReceivableLine, null)
    cy.SelectLogLovElement(Selectors.ReceivableChargesType, true, ChargesType)
    cy.FillRandomNumber(Selectors.ReceivableUnitPrice, 100, 100)
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

export function FillDeliveryRouting(partner: string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.Delivery, null)
    cy.AssertResponseStatusCode("WaitCardViewsRequest", 200, null)
    cy.AssertResponseStatusCode("WaitAddressViewsRequest", 200, null)
    cy.get(Selectors.ShipmentPickUpDelivery_ToPartnerCardId).then((input) => {
        if(input.text() === "" || input.text() === null) {
            cy.SelectLogLovElement(Selectors.ShipmentPickUpDelivery_ToPartnerCardId, false, partner)
        }
    })
}

export function FillPreCarriageRouting(transportModeCode:string, fromPort: string, toPort: string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.PreCarriage).then((btn) => {
        if(!btn.is('[disabled]')) {
            cy.Click(Selectors.PreCarriage, null)
            cy.FillLogLov(Selectors.Shipment_PreCarriageTransportModeId, transportModeCode, true)
            cy.SelectLogLovElement(Selectors.Shipment_PreCarriageFromPortId, false, fromPort)
            cy.SelectLogLovElement(Selectors.Shipment_PreCarriageToPortId, false, toPort)
            cy.Click(Selectors.PreCarriageOKBtn, null)
        }else{
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportModeCode:string, fromPort: string, toPort: string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.OnCarriage).then((btn) => {
        if(!btn.is('[disabled]')) {
            cy.Click(Selectors.OnCarriage, null)
            cy.FillLogLov(Selectors.Shipment_OnCarriageTransportModeId, transportModeCode, true)
            cy.SelectLogLovElement(Selectors.Shipment_OnCarriageFromPortId, false, fromPort)
            cy.SelectLogLovElement(Selectors.Shipment_OnCarriageToPortId, false, toPort)
            cy.Click(Selectors.OnCarriageOKBtn, null)    
        }else{
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab(ChargesType: string, currency: string, measutment?: string) {
    cy.Click(Selectors.AddNewPayableLine , null)
    cy.SelectLogLovElement(Selectors.ShipmentPayable_ChargesTypeId, true, ChargesType)
    cy.SelectLogLovFirstElement(Selectors.ShipmentPayable_MeasurementId, true)
    cy.SelectLogLovElement(Selectors.ShipmentPayable_CurrencyId, true, currency)
    cy.Click(Selectors.AddPayableOkButton , null)
}

export function CreateAnewShipment(shipmentLevel: string) {
    var _ShipmentDetails = {
        ShipmentLevel: shipmentLevel.charAt(0),
        Direction: "E",
        TransportMode: "A",
        Shipper: "Shipper1",
        MainCarriageToPort: "LHR",
        MainCarriageFromPort: "MIA"
    } as ShipmentDetails;

    OpenNewShipmentWizard(_ShipmentDetails.ShipmentLevel);
    FillShipmentDefaultFields(_ShipmentDetails);
    CreateShipment(_ShipmentDetails.ShipmentLevel)

    let resultFile = "CreatedShipmentsData/" + _ShipmentDetails.ShipmentLevel + _ShipmentDetails.Direction +
    _ShipmentDetails.TransportMode +
    ((typeof _ShipmentDetails.ShipmentType) === "undefined" || _ShipmentDetails.ShipmentType === null ? "" : _ShipmentDetails.ShipmentType) + ".json";
  
    Assertions.ValidateCreatedShipment(resultFile);
}

function AddPackagesOrContainersForOrdersTab(shipmentTypeCode:string, PackageType: string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 3)
    for (let i = 0; i < numberOfPackages; i++){
        cy.Click(Selectors.OrdersAddPackage, null)
        cy.FillRandomNumber(Selectors.OrderPackageQuantity, 1, 10)
        if (shipmentTypeCode === "FCLD" || shipmentTypeCode === "FTL" || shipmentTypeCode === "LCLD" || shipmentTypeCode === "LTL"){
            cy.SelectLogLovElement(Selectors.OrderPackageType, true, PackageType)
            cy.FillRandomNumber(Selectors.OrderPackageGrossWeight, 1, 50)
        }
        cy.Click(Selectors.OrderOKButton, null)
    }
}

function AddPartner(partnerTypeId: string, partnerFieldId: string, partner?:string) {
    cy.Click("label", "Add Partners")
    
    cy.get("#" + partnerTypeId).then((btn) => {
        if(!btn.is('[disabled]')) {
            cy.Click(("#" + partnerTypeId), null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId + "']"
            if(partner) {
                cy.SelectLogLovElement(partnerFieldSelector, false, partner)
            }
            else {
                cy.SelectLogLovFirstElement(partnerFieldSelector, false)
            }
            cy.Click(Selectors.PartnerOKButton, null)
        }
    })
}

function AddPackagesOrContainersForPackagesTab(transportModeCode:string, shipmentTypeCode:string, PackageType:string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 3)
    for (let i = 0; i < numberOfPackages; i++){
        cy.Click(Selectors.AddPackage, null)
        if(transportModeCode !== "A"){
            cy.SelectLogLovElement(Selectors.PackageType, true, PackageType)
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