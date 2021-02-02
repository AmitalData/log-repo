import * as gr from "../../../Base/cypress/Actions/GenerateRandoms"
import { Selectors } from "../selectors/Selectors"
import { ShipmentDetails } from "../models/ShipmentDetails";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { PayableDetails } from "cypress/models/PayableDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails"
import { ARInvoiceDetails } from "cypress/models/ARInvoiceDetails"
import * as BaseActions from "../../../Base/cypress/actions/Actions"
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ShipmentMapping } from "../mapping/ShipmentMapping"

export function OpenNewShipmentWizard(shipmentLevel: string){
    cy.Click("#HelperNotesButton_0_0", null)
    cy.Click(".LogitudeToggleButtonItem", shipmentLevel)
}

export function FillShipmentWizardsFields(shipmentDetails: ShipmentDetails){
    if(shipmentDetails.ShipmentLevel !== "Master"){
        FillDirectAndHouseFields(shipmentDetails);
    }else{
        FillMasterFields(shipmentDetails);
    }
}

export function CreateShipment(shipmentLevel: string){
    let createPreSelector = shipmentLevel === "Master" ? "#Master" : "#Shipment";
    cy.DefineRequestWait("POST", "**/shipment", "WaitPostShipmentRequest")
    cy.Click(createPreSelector + "Createbtn", null)
}


export function UpdateShipment(saveButtonSelector: string) {
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
    cy.Click(saveButtonSelector, null)
}

export function OpenShipment(ShipmentNumber: string) {
    cy.SelectQuickSearchFirstElement(Selectors.ShipmentSearchBar, ShipmentNumber)
}

export function CancelShipment() {
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.CancelShipmentButton, null);
    UpdateCanceledShipment();
}

export function UpdateCanceledShipment() {
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
    cy.Click(Selectors.ConfirmActionButton, null);
}

export function DisconnectShipment() {
    // cy.Click(BaseSelectors.Button,"Disconnect All");
    UpdateDisconnectedShipment()
}

export function UpdateDisconnectedShipment() {
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
    cy.Click(BaseSelectors.RedButton, "Yes");
}

export function FillGeneralTab(MoveType: string) {
    cy.FillRandomNumber(Selectors.ShipmentGrossWeight, 100, 1000)
    cy.SelectLogLovElement(Selectors.ShipmentMoveType, true, MoveType)
}

export function FillOrdersTab(shipmentType?:string, PackageType?: string){
    AddPackagesOrContainersForOrdersTab(shipmentType, PackageType)
}

export function FillPartnersTab(direction:string, transportMode:string, partnersDetails: PartnersDetails){
    if (direction === "Import") {
        AddPartner("SHIPR", "Shipment_ShipperId")
    }
    if((direction === "Export") || (direction === "Drop") || (direction === "Domestic" && transportMode !== "Inland")){
        AddPartner("CONSI", "Shipment_ConsigneeId")
    }
    AddPartner("AGENT", "Shipment_AgentId", partnersDetails.Agent)
    if((direction === "Import" && transportMode === "Air") || (direction === "Domestic" && transportMode === "Air")){
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

export function FillPackagesTab(transportMode:string, shipmentType?:string, packageType?:string){
    AddPackagesOrContainersForPackagesTab(transportMode, shipmentType, packageType)
}

export function FillHouseInShipmentsTab(Shipper: string) {
    cy.SelectLogLovElement("#Shipment_CustomerId", true, Shipper)
}



export function FillReceivablesTab(receivableDetails: ReceivableDetails) {
    cy.Click(Selectors.AddNewReceivableLine, null)
    cy.SelectLogLovElement(Selectors.ReceivableChargesType, true, receivableDetails.ChargesType)
    cy.SelectLogLovElement(Selectors.ReceivableMeasurement, true, receivableDetails.UOM)
    cy.get(Selectors.ReceivableQuantity).type(receivableDetails.Quantity);
    cy.get(Selectors.ReceivableUnitPrice).type(receivableDetails.UnitPrice);
    cy.SelectLogLovElement(Selectors.ReceivableCurrency, true, receivableDetails.Currency)
    cy.get(Selectors.ShipmentReceivableRate).clear().type(receivableDetails.ExchangeRate)
    cy.Click(Selectors.AddReceivableOkButton, null)
}

export function FillPickupRouting() {
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.PickUp, null)
    BaseAssertion.AssertStatusCode("WaitCardViewsRequest", 200)
    BaseAssertion.AssertStatusCode("WaitAddressViewsRequest", 200)
}

export function FillDeliveryRouting(partner: string) {
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.Delivery, null)
    BaseAssertion.AssertStatusCode("WaitCardViewsRequest", 200)
    BaseAssertion.AssertStatusCode("WaitAddressViewsRequest", 200)
    cy.get(Selectors.ShipmentPickUpDelivery_ToPartnerCardId).then((input) => {
        if (input.text() === "" || input.text() === null) {
            cy.SelectLogLovElement(Selectors.ShipmentPickUpDelivery_ToPartnerCardId, false, partner)
        }
    })
}

export function FillPreCarriageRouting(transportMode:string, fromPort: string, toPort: string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.PreCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(Selectors.PreCarriage, null)
            cy.FillLogLov(Selectors.Shipment_PreCarriageTransportModeId, transportMode, true)
            cy.SelectLogLovElement(Selectors.Shipment_PreCarriageFromPortId, false, fromPort)
            cy.SelectLogLovElement(Selectors.Shipment_PreCarriageToPortId, false, toPort)
            cy.Click(Selectors.PreCarriageOKBtn, null)
        } else {
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportMode:string, fromPort: string, toPort: string){
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.OnCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(Selectors.OnCarriage, null)
            cy.FillLogLov(Selectors.Shipment_OnCarriageTransportModeId, transportMode, true)
            cy.SelectLogLovElement(Selectors.Shipment_OnCarriageFromPortId, false, fromPort)
            cy.SelectLogLovElement(Selectors.Shipment_OnCarriageToPortId, false, toPort)
            cy.Click(Selectors.OnCarriageOKBtn, null)
        } else {
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab(payableDetails: PayableDetails) {
    cy.Click(Selectors.AddNewPayableLine, null)
    cy.SelectLogLovElement(Selectors.ShipmentPayable_ChargesTypeId, true, payableDetails.ChargesType);
    cy.SelectLogLovElement(Selectors.ShipmentPayable_MeasurementId, true, payableDetails.UOM)
    if (payableDetails.Quantity) {
        cy.get(Selectors.ShipmentPayableQuantity).type(payableDetails.Quantity);
    }
    if (payableDetails.UnitPrice) {
        cy.get(Selectors.ShipmentPayableUnitPrice).type(payableDetails.UnitPrice);
    }
    cy.SelectLogLovElement(Selectors.ShipmentPayable_CurrencyId, true, payableDetails.Currency)
    cy.Click(Selectors.AddPayableOkButton, null)
}

export function CreateAnewShipment(shipmentLevel: string) {
    var _ShipmentDetails = {
        ShipmentLevel: shipmentLevel,
        Direction: "Export",
        TransportMode: "Air",
        Shipper: "Shipper1",
        MainCarriageToPort: "LHR",
        MainCarriageFromPort: "MIA"
    } as ShipmentDetails;

    OpenNewShipmentWizard(_ShipmentDetails.ShipmentLevel);
    FillShipmentWizardsFields(_ShipmentDetails);
    CreateShipment(_ShipmentDetails.ShipmentLevel)
}

export function CopyShipment(shipmentLevel: string) {
    cy.Click(Selectors.ShipmentMoreList, null)
    cy.Click(Selectors.CopyShipmentButton, null)
    CreateShipment(shipmentLevel);
}

export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.SelectLogLovElement(Selectors.APInvoiceVendor, false, aPInvoiceDetails.Vendor);
    cy.get(Selectors.APInvoiceInvoiceNumber).type(generatedInvoiceNumber)
    cy.get(Selectors.APInvoiceAmountInInvoice).type(aPInvoiceDetails.InvoiceAmount)
    cy.SelectLogLovElement(Selectors.APInvoiceInvoiceCurrency, true, aPInvoiceDetails.InvoiceCurrency);
    cy.get(Selectors.APInvoiceInvoiceExchangeRate).clear().type(aPInvoiceDetails.InvoiceExchangeRate);
    cy.FillDate(Selectors.APInvoiceInvoiceDate,aPInvoiceDetails.InvoiceDate)
    cy.SelectLogLovElement(Selectors.APInvoicePaymentTerm, true, aPInvoiceDetails.PaymentTerms);
    cy.FillDate(Selectors.APInvoiceDueDate,aPInvoiceDetails.DueDate)
    cy.Click(Selectors.OkCreateAPInvoiceButton, null);
    cy.Click(Selectors.APInvoiceLineCheckBox, null)
    cy.SelectLogLovElement(Selectors.APInvoiceVatType, true, aPInvoiceDetails.VATType)
    cy.Click(Selectors.VatTypeApplyToAll, null)
}

export function ReceiveAPInvoice() {
    cy.DefineRequestWait("POST", "**/apinvoices", "WaitPostAPInvoicesRequest")
    cy.Click(Selectors.APInvoiceSaveButton, null)
}

export function APApproveInvoice() {
    cy.DefineRequestWait("PUT", "**/apinvoices", "WaitPutAPInvoicesRequest")
    cy.Click(Selectors.APInvoiceApproveButton, null)
}

export function APInvoiceCancelApproval() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.DefineRequestWait("PUT", "**/apinvoices", "WaitPutAPInvoicesRequest")
    cy.Click(Selectors.APInvoiceCancelApprovalButton, null)
}

export function ConnectShipment() {
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest");
    cy.Click(BaseSelectors.RedButton, "Yes");
}

export function VoidAPInvoice(){
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.Click(Selectors.APInvoiceVoidButton,null)
    cy.DefineRequestWait("PUT", "**/apinvoices", "WaitPutAPInvoicesRequest")
    cy.Click(Selectors.ConfirmWindowYes, null);
}

export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    var generatedInvoiceNumber = "AR" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.get(".ComboBox").click();
    cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    cy.SelectLogLovElement(Selectors.ARInvoiceInvoiceCurrency, true, aRInvoiceDetails.InvoiceCurrency)
    cy.get(Selectors.ARInvoiceExchangeRate).clear().type(aRInvoiceDetails.InvoiceExchangeRate)
    cy.FillDate(Selectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.SelectLogLovElement(Selectors.ARInvoicePaymentTerm, true, aRInvoiceDetails.PaymentTerms)
    cy.FillDate(Selectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.get(Selectors.ARInvoiceVatNumber).clear().type(aRInvoiceDetails.VATNo)
    cy.SelectLogLovElement(Selectors.ARInvoiceBranch, true, aRInvoiceDetails.Branch)
    cy.Click(Selectors.OkCreateARInvoiceButton, null);
    cy.SelectLogLovElement(Selectors.ARInvoiceVatType, true, aRInvoiceDetails.VATType)
    cy.Click(Selectors.VatTypeApplyToAll, null)


}

export function CreateARInvoice() {
    cy.DefineRequestWait("POST", "**/arinvoices", "WaitPostARInvoicesRequest")
    cy.Click(Selectors.ARInvoiceSaveButton, null)
}

export function ARApproveInvoice() {
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click(Selectors.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice(){
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.Click(Selectors.ARInvoiceSetAsSentButton,null)
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click("button", "Confirm");
}

export function VoidARInvoice(){
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.Click(Selectors.ARInvoiceVoidButton,null)
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click(Selectors.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click(Selectors.ARInvoiceCancelDraftButton, null)
    cy.Click(Selectors.ConfirmWindowYes,null)
}

function AddPackagesOrContainersForOrdersTab(shipmentType:string, PackageType: string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 3)
    for (let i = 0; i < numberOfPackages; i++) {
        cy.Click(Selectors.OrdersAddPackage, null)
        cy.FillRandomNumber(Selectors.OrderPackageQuantity, 1, 10)
        if (shipmentType === "FCLD" || shipmentType === "FTL" || shipmentType === "LCLD" || shipmentType === "LTL"){
            cy.SelectLogLovElement(Selectors.OrderPackageType, true, PackageType)
            cy.FillRandomNumber(Selectors.OrderPackageGrossWeight, 1, 50)
        }
        cy.Click(Selectors.OrderOKButton, null)
    }
}

function AddPartner(partnerTypeId: string, partnerFieldId: string, partner?: string) {
    cy.Click("label", "Add Partners")

    cy.get("#" + partnerTypeId).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(("#" + partnerTypeId), null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId + "']"
            if (partner) {
                cy.SelectLogLovElement(partnerFieldSelector, false, partner)
            }
            else {
                cy.SelectLogLovFirstElement(partnerFieldSelector, false)
            }
            cy.Click(Selectors.PartnerOKButton, null)
        }
    })
}

function AddPackagesOrContainersForPackagesTab(transportMode:string, shipmentType:string, packageType:string){
    let numberOfPackages = gr.GenerateRandomNumber(1, 3)
    for (let i = 0; i < numberOfPackages; i++) {
        cy.Click(Selectors.AddPackage, null)
        if(transportMode !== "Air"){
            cy.SelectLogLovElement(Selectors.PackageType, true, packageType)
        }
        if(shipmentType !== "FCLD" && shipmentType !== "FTL"){
            cy.FillRandomNumber(Selectors.PackageQuantity, 1, 10)
        }
        if(transportMode !== "Air"){
            cy.FillRandomNumber(Selectors.PackageWeight, 1, 10)
        }
        if(transportMode === "Air"){
            cy.Click(Selectors.AirPackageOKButton, null)
        } else {
            cy.Click(Selectors.OceanPackageOKButton, null)
        }
    }
}

function FillDirectAndHouseFields(shipmentDetails: ShipmentDetails){
    FillMainFields(shipmentDetails.Direction, shipmentDetails.TransportMode, shipmentDetails.ShipmentType);
    FillShipperAndConsignee(shipmentDetails.Shipper, shipmentDetails.Consignee, shipmentDetails.Direction, shipmentDetails.TransportMode);
    FillMainCarriagePorts(shipmentDetails.MainCarriageFromPort, shipmentDetails.MainCarriageToPort, shipmentDetails.Direction, shipmentDetails.TransportMode);
}

function FillMasterFields(shipmentDetails: ShipmentDetails){
    FillMainFields(shipmentDetails.Direction, shipmentDetails.TransportMode, shipmentDetails.ShipmentType);
    FillMasterAgent(shipmentDetails.Agent);
    FillMainCarriagePorts(shipmentDetails.MainCarriageFromPort, shipmentDetails.MainCarriageToPort, shipmentDetails.Direction, shipmentDetails.TransportMode, true);
}

function FillMainFields(direction: string, transportMode: string, shipmentType: string){
    FillDirection(direction);
    FillTransportMode(transportMode);
    FillShipmentType(shipmentType, transportMode);
}

function FillDirection(direction: string){
    cy.ClickRadio("#DirectionRadio_0" + ShipmentMapping.GetDirectionCode(direction))
}

function FillTransportMode(transportMode: string){
    cy.ClickRadio("#TransportModeRadio_0" + ShipmentMapping.GetTransportModeCode(transportMode))
}

function FillShipmentType(shipmentType: string, transportMode: string){
    if(shipmentType){
        if(shipmentType === "Groupage"){
            cy.ClickRadio("#ShipmentTypeRadio_0MyG" + ShipmentMapping.GetTransportModeCode(transportMode));
        }else{
            cy.ClickRadio("#ShipmentTypeRadio_0" + shipmentType)
        }
    }
}

function FillShipperAndConsignee(shipper: string, consignee: string, direction: string, transportMode: string){
    if(IsInlandDomestic(direction, transportMode)){
        cy.FillLogLov("#Shipment_ShipperId", shipper, false)
        cy.FillLogLov("#Shipment_ConsigneeId", consignee, false)
    }else{
        if(direction === "Import"){
            cy.FillLogLov("#Shipment_ConsigneeId", consignee, false)
        }else{
            cy.FillLogLov("#Shipment_ShipperId", shipper, false)
        }
    }
}

function FillMainCarriagePorts(fromPort: string, toPort: string, direction: string, transportMode: string, isMaster: boolean = false){
    if(!IsInlandDomestic(direction, transportMode)){
        let portsPreSelector = isMaster ? "#Master" : "#Shipment";
        cy.FillLogLov((portsPreSelector + "_MainCarriageFromPortId"), fromPort, false)
        cy.FillLogLov((portsPreSelector + "_MainCarriageToPortId"), toPort, false)
    }
}

function FillMasterAgent(agent: string){
    cy.FillLogLov("#Master_AgentId", agent, false)
}

function IsInlandDomestic(direction: string, transportMode: string){
    return (direction === "Domestic" && transportMode === "Inland");
}