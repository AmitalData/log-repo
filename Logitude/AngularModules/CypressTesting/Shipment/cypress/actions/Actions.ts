import * as gr from "../../../Base/cypress/Actions/GenerateRandoms";
import { ShipmentSelector } from "../selectors/Selectors";
import { ShipmentDetails } from "../models/ShipmentDetails";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { PayableDetails } from "cypress/models/PayableDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails";
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails";
import { ARInvoiceDetails } from "cypress/models/ARInvoiceDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { APPaymentDetails } from "cypress/models/APPaymentDetails";
import { ARPaymentDetails } from "cypress/models/ARPaymentDetails"
import { URLs } from "../constants/URLs";
import * as Conditions from "../actions/Conditions";

export function NavigatesToShipmentsWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelector.ShipmentTab, null)
}

export function OpenNewShipmentWizard(shipmentLevel: string) {
    cy.Click(ShipmentSelector.NewShipmentToggleButton, null)
    cy.Click(ShipmentSelector.NewShipmentToggleButtonItem, shipmentLevel)
}

export function FillShipmentWizardsFields(shipmentDetails: ShipmentDetails) {
    if (!Conditions.IsMaster(shipmentDetails.ShipmentLevel)) {
        FillDirectAndHouseFields(shipmentDetails);
    } else {
        FillMasterFields(shipmentDetails);
    }
}

export function CreateShipment(shipmentLevel: string) {
    let createSelector = Conditions.IsMaster(shipmentLevel) ? ShipmentSelector.CreateMasterShipmentButton : ShipmentSelector.CreateShipmentButton;
    cy.DefineRequestWait("POST", URLs.Shipment, "WaitPostShipmentRequest")
    cy.Click(createSelector, null)
}

export function UpdateShipment(saveButtonSelector: string, saveButtonSelectorContains?: string) {
    cy.DefineRequestWait("PUT", URLs.Shipment, "WaitPutShipmentRequest")
    cy.Click(saveButtonSelector, saveButtonSelectorContains)
}

export function OpenShipment(shipmentNumber: string) {
    cy.DefineRequestWait("GET", "**/ngMetaData/getmenubuttongrouppms?**", "WaitLoadShipmentMenuButtons");
    cy.SelectQuickSearchFirstElement(ShipmentSelector.ShipmentSearchBar, shipmentNumber)
}

export function CancelShipment() {
    cy.Click(ShipmentSelector.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelector.CancelShipmentButton, null);
    UpdateShipment(ShipmentSelector.ConfirmActionButton);
}

export function ConnectOrDisconnectShipment() {
    UpdateShipment(BaseSelectors.RedButton, "Yes");
}

export function FillGeneralTab(GrossWeight: string, MoveType: string) {
    cy.Click(ShipmentSelector.GeneralTab, null)
    cy.FillLogTextBox(ShipmentSelector.ShipmentGrossWeight, GrossWeight)
    cy.FillLogLov(ShipmentSelector.ShipmentMoveType, MoveType, true)
}

export function FillOrdersTab(packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(ShipmentSelector.OrdersTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelector.OrdersAddPackage, null)

        cy.FillLogTextBox(ShipmentSelector.OrderPackageQuantity, packagesDetails[i].Quantity.toString())
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelector.OrderPackageType, packagesDetails[i].PackageType, true)
        }
        if (!Conditions.IsFCL(shipmentType) && !Conditions.IsFTL(shipmentType)) {
            cy.FillLogTextBox(ShipmentSelector.OrderPackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(ShipmentSelector.OrderPackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(ShipmentSelector.OrderPackageHeight, packagesDetails[i].Height.toString())
        }
        cy.FillLogTextBox(ShipmentSelector.OrderPackageGrossWeight, packagesDetails[i].GrossWeight.toString())
        cy.Click(ShipmentSelector.OrderOKButton, null)
    }

}

export function FillPartnersTab(direction: string, transportMode: string, partnersDetails: PartnersDetails) {
    cy.Click(ShipmentSelector.PartnersTab, null)

    if (Conditions.IsImport(direction)) {
        AddPartner(ShipmentSelector.AddShipperButton, ShipmentSelector.ShipmentShipper, partnersDetails.Shipper)
    }
    if (Conditions.IsExport(direction) || Conditions.IsDrop(direction) || (Conditions.IsDomestic(direction) && !Conditions.IsInland(transportMode))) {
        AddPartner(ShipmentSelector.AddConsigneeButton, ShipmentSelector.ShipmentConsignee, partnersDetails.Consignee)
    }
    AddPartner(ShipmentSelector.AddAgentButton, ShipmentSelector.ShipmentAgent, partnersDetails.Agent)
    if ((Conditions.IsImport(direction) && Conditions.IsAir(transportMode)) || (Conditions.IsDomestic(direction) && Conditions.IsAir(transportMode))) {
        AddPartner(ShipmentSelector.AddIssuingCarrierAgentButton, ShipmentSelector.ShipmentIssuingCarrierAgent)
    }
    AddPartner(ShipmentSelector.AddCustomsAgentExportButton, ShipmentSelector.ShipmentCustomAgentExport, partnersDetails.CustomsAgentExport)
    AddPartner(ShipmentSelector.AddCustomsAgentImportButton, ShipmentSelector.ShipmentCustomAgentImport, partnersDetails.CustomsAgentImport)
    AddPartner(ShipmentSelector.AddNotify1Button, ShipmentSelector.ShipmentNotify1, partnersDetails.Notify1)
    AddPartner(ShipmentSelector.AddNotify2Button, ShipmentSelector.ShipmentNotify2, partnersDetails.Notify2)
    AddPartner(ShipmentSelector.AddShipperNotExporterButton, ShipmentSelector.ShipmentShipperNotExporter, partnersDetails.ShipperNotExporter)
    AddPartner(ShipmentSelector.AddConsigneeNotImporterButton, ShipmentSelector.ShipmentConsigneeNotImporter, partnersDetails.ConsigneeNotImporter)
    AddPartner(ShipmentSelector.AddFreightForwarderButton, ShipmentSelector.ShipmentFreightForwarder, partnersDetails.FreightForwarder)
    AddPartner(ShipmentSelector.AddColoaderButton, ShipmentSelector.ShipmentColoader, partnersDetails.Coloader)
    AddPartner(ShipmentSelector.AddCustomClearancePointButton, ShipmentSelector.ShipmentCustomClearancePoint, partnersDetails.CustomClearancePoint)
    AddPartner(ShipmentSelector.AddConsolidatorButton, ShipmentSelector.ShipmentConsolidator, partnersDetails.Consolidator)
    AddPartner(ShipmentSelector.AddReleasingAgentButton, ShipmentSelector.ShipmentReleasingAgent, partnersDetails.ReleasingAgent)
}

export function FillPackageTab(transportMode: string, packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(ShipmentSelector.PackagesTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelector.AddPackage, null)
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelector.PackageType, packagesDetails[i].PackageType, true)
        }
        if (!Conditions.IsFCL(shipmentType) && !Conditions.IsFTL(shipmentType)) {
            cy.FillLogTextBox(ShipmentSelector.PackageQuantity, packagesDetails[i].Quantity.toString())
            cy.FillLogTextBox(ShipmentSelector.PackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(ShipmentSelector.PackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(ShipmentSelector.PackageHeight, packagesDetails[i].Height.toString())
        }
        cy.get(ShipmentSelector.PackageWeight).type(packagesDetails[i].GrossWeight.toString());

        if (Conditions.IsAir(transportMode)) {
            cy.Click(ShipmentSelector.AirPackageOKButton, null)
        } else {
            cy.Click(ShipmentSelector.OceanPackageOKButton, null)
        }
    }
}

export function FillHouseInShipmentsTab(Shipper: string) {
    cy.FillLogLov(ShipmentSelector.ShipmentCustomer, Shipper, true)
}

export function FillReceivablesTab(receivableDetails: ReceivableDetails[]) {
    cy.Click(ShipmentSelector.ReceivablesTab, null)
    for (let i = 0; i < receivableDetails.length; i++) {
        cy.Click(ShipmentSelector.AddNewReceivableLine, null)
        cy.FillLogLov(ShipmentSelector.ReceivableChargesType, receivableDetails[i].ChargesType, true)
        cy.FillLogLov(ShipmentSelector.ReceivableMeasurement, receivableDetails[i].UOM, true)
        cy.get(ShipmentSelector.ReceivableQuantity).type(receivableDetails[i].Quantity.toString());
        cy.get(ShipmentSelector.ReceivableUnitPrice).type(receivableDetails[i].UnitPrice.toString());
        cy.FillLogLov(ShipmentSelector.ReceivableCurrency, receivableDetails[i].Currency, true)
        cy.get(ShipmentSelector.ShipmentReceivableRate).clear().type(receivableDetails[i].ExchangeRate.toString());
        cy.Click(ShipmentSelector.AddReceivableOkButton, null)
    }
}


export function FillPickupRouting() {
    cy.Click(ShipmentSelector.RoutingsTab, null, true)
    cy.Click(ShipmentSelector.RoutingToggle, null, true)
    cy.DefineRequestWait("GET", URLs.CardViews, "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", URLs.AddressViews, "WaitAddressViewsRequest")
    cy.Click(ShipmentSelector.PickUp, null)
    BaseAssertion.AssertStatusCode("WaitCardViewsRequest", 200)
    BaseAssertion.AssertStatusCode("WaitAddressViewsRequest", 200)
    cy.Click(ShipmentSelector.SaveClose, null)
}

export function EditMainCarriageLegs(Airline: string) {
    cy.Click(ShipmentSelector.EditRoutingMainCarriage, null)
    cy.FillLogLov(ShipmentSelector.ShipmentMainCarriageCarrierId, Airline, false)
    cy.FillRandomNumber(ShipmentSelector.ShipmentFlightNumber, 100, 999)
    cy.FillRandomNumber(ShipmentSelector.ShipmentMAWB, 10000000, 99999999)
    cy.Click(ShipmentSelector.ShipmentDateMaincarriageATD, null)
    cy.Click(BaseSelectors.Button, "Today")
    cy.Click(ShipmentSelector.MainCarriageOKBtn, null);
    cy.Click(ShipmentSelector.ShipmentSaveButton, null);
}

export function UpdateClosedShipment() {
    cy.DefineRequestWait("PUT", URLs.Shipment, "WaitPutShipmentRequest")
    cy.Click(BaseSelectors.RedButton, "Confirm");
}

export function FillDeliveryRouting(partner: string) {
    cy.Click(ShipmentSelector.RoutingsTab, null)
    cy.Click(ShipmentSelector.RoutingToggle, null, true)
    cy.DefineRequestWait("GET", URLs.CardViews, "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", URLs.AddressViews, "WaitAddressViewsRequest")
    cy.Click(ShipmentSelector.Delivery, null)
    BaseAssertion.AssertStatusCode("WaitCardViewsRequest", 200)
    BaseAssertion.AssertStatusCode("WaitAddressViewsRequest", 200)

    cy.FillLogLov(ShipmentSelector.ShipmentPickUpDeliveryToPartnerCard, partner, false)

    cy.Click(ShipmentSelector.SaveClose, null)
}

export function FillPreCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(ShipmentSelector.RoutingsTab, null)
    cy.Click(ShipmentSelector.RoutingToggle, null)
    cy.get(ShipmentSelector.PreCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(ShipmentSelector.PreCarriage, null)
            cy.FillLogLov(ShipmentSelector.ShipmentPreCarriageTransportMode, transportMode, true)
            cy.FillLogLov(ShipmentSelector.ShipmentPreCarriageFromPort, fromPort, false)
            cy.FillLogLov(ShipmentSelector.ShipmentPreCarriageToPort, toPort, false)
            cy.Click(ShipmentSelector.PreCarriageOKBtn, null)
        } else {
            cy.Click(ShipmentSelector.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(ShipmentSelector.RoutingsTab, null)
    cy.Click(ShipmentSelector.RoutingToggle, null)
    cy.get(ShipmentSelector.OnCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(ShipmentSelector.OnCarriage, null)
            cy.FillLogLov(ShipmentSelector.ShipmentOnCarriageTransportMode, transportMode, true)
            cy.FillLogLov(ShipmentSelector.ShipmentOnCarriageFromPort, fromPort, false)
            cy.FillLogLov(ShipmentSelector.ShipmentOnCarriageToPort, toPort, false)
            cy.Click(ShipmentSelector.OnCarriageOKBtn, null)
        } else {
            cy.Click(ShipmentSelector.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab(payableDetails: PayableDetails) {
    cy.Click(ShipmentSelector.PayablesTab, null)
    cy.Click(ShipmentSelector.AddNewPayableLine, null)
    cy.FillLogLov(ShipmentSelector.ShipmentPayableChargesType, payableDetails.ChargesType, true);
    cy.FillLogLov(ShipmentSelector.ShipmentPayableMeasurement, payableDetails.UOM, true)
    cy.FillLogTextBox(ShipmentSelector.ShipmentPayableQuantity, payableDetails.Quantity.toString());
    cy.FillLogTextBox(ShipmentSelector.ShipmentPayableUnitPrice, payableDetails.UnitPrice.toString());
    cy.FillLogLov(ShipmentSelector.ShipmentPayableCurrency, payableDetails.Currency, true)
    cy.Click(ShipmentSelector.AddPayableOkButton, null)
}

export function CopyShipment(shipmentLevel: string) {
    cy.Click(ShipmentSelector.ShipmentMoreList, null, true)
    cy.Click(ShipmentSelector.CopyShipmentButton, null)
    CreateShipment(shipmentLevel);
}

export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.FillLogLov(ShipmentSelector.APInvoiceVendor, aPInvoiceDetails.Vendor, false);
    cy.FillLogTextBox(ShipmentSelector.APInvoiceInvoiceNumber, generatedInvoiceNumber)
    cy.FillLogTextBox(ShipmentSelector.APInvoiceAmountInInvoice, aPInvoiceDetails.InvoiceAmount.toString());
    cy.FillLogLov(ShipmentSelector.APInvoiceInvoiceCurrency, aPInvoiceDetails.InvoiceCurrency, true);
    cy.FillLogTextBox(ShipmentSelector.APInvoiceInvoiceExchangeRate, aPInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(ShipmentSelector.APInvoiceInvoiceDate, aPInvoiceDetails.InvoiceDate)
    cy.FillLogLov(ShipmentSelector.APInvoicePaymentTerm, aPInvoiceDetails.PaymentTerms, true);
    cy.FillDate(ShipmentSelector.APInvoiceDueDate, aPInvoiceDetails.DueDate)
    cy.FillLogTextBox(ShipmentSelector.APInvoiceVATNumber, aPInvoiceDetails.VatNo.toString())
    cy.Click(ShipmentSelector.OkCreateAPInvoiceButton, null);
    cy.Click(BaseSelectors.CheckBoxLine, null)
    cy.FillLogLov(ShipmentSelector.APInvoiceVatType, aPInvoiceDetails.VATType, true)
    cy.Click(ShipmentSelector.VatTypeApplyToAll, null)
}

export function ReceiveAPInvoice() {
    cy.DefineRequestWait("POST", URLs.APInvoices, "WaitPostAPInvoicesRequest")
    cy.Click(ShipmentSelector.APInvoiceSaveButton, null)
}

export function APApproveInvoice() {
    cy.DefineRequestWait("PUT", URLs.APInvoices, "WaitPutAPInvoicesRequest")
    cy.Click(ShipmentSelector.APInvoiceApproveButton, null)
}

export function APInvoiceCancelApproval() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait("PUT", URLs.APInvoices, "WaitPutAPInvoicesRequest")
    cy.Click(ShipmentSelector.APInvoiceCancelApprovalButton, null)
}

export function VoidAPInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(ShipmentSelector.APInvoiceVoidButton, null)
    cy.DefineRequestWait("PUT", URLs.APInvoices, "WaitPutAPInvoicesRequest")
    cy.Click(ShipmentSelector.ConfirmWindowYes, null);
}

export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    cy.get(".ComboBox").click();
    cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    cy.FillLogLov(ShipmentSelector.ARInvoiceInvoiceCurrency, aRInvoiceDetails.InvoiceCurrency, true)
    cy.get(ShipmentSelector.ARInvoiceExchangeRate).clear().type(aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(ShipmentSelector.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillLogLov(ShipmentSelector.ARInvoicePaymentTerm, aRInvoiceDetails.PaymentTerms, true)
    cy.FillDate(ShipmentSelector.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.get(ShipmentSelector.ARInvoiceVatNumber).clear().type(aRInvoiceDetails.VATNo)
    cy.FillLogLov(ShipmentSelector.ARInvoiceBranch, aRInvoiceDetails.Branch, true)
    cy.Click(ShipmentSelector.OkCreateARInvoiceButton, null);
    cy.FillLogLov(ShipmentSelector.ARInvoiceVatType, aRInvoiceDetails.VATType, true)
    cy.Click(ShipmentSelector.VatTypeApplyToAll, null)
}

export function CreateARInvoice() {
    cy.DefineRequestWait("POST", URLs.ARInvoices, "WaitPostARInvoicesRequest")
    cy.Click(ShipmentSelector.ARInvoiceSaveButton, null)
}

export function ARApproveInvoice() {
    cy.DefineRequestWait("PUT", URLs.ARInvoices, "WaitPutARInvoicesRequest")
    cy.Click(ShipmentSelector.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(ShipmentSelector.ARInvoiceSetAsSentButton, null)
    cy.DefineRequestWait("PUT", URLs.ARInvoices, "WaitPutARInvoicesRequest")
    cy.Click("button", "Confirm");
}

export function VoidARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(ShipmentSelector.ARInvoiceVoidButton, null)
    cy.DefineRequestWait("PUT", URLs.ARInvoices, "WaitPutARInvoicesRequest")
    cy.Click(ShipmentSelector.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait("PUT", URLs.ARInvoices, "WaitPutARInvoicesRequest")
    cy.Click(ShipmentSelector.ARInvoiceCancelDraftButton, null)
    cy.Click(ShipmentSelector.ConfirmWindowYes, null)
}

export function FillAPPayment(aPPaymentDetails: APPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(ShipmentSelector.PayableAccountingTab, null)
    cy.Click(ShipmentSelector.NewAPPayment, null)
    cy.FillLogLov(ShipmentSelector.APPaymentVendor, aPPaymentDetails.Vendor, false)
    cy.FillLogLov(ShipmentSelector.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ShipmentSelector.APPaymentAmount, aPPaymentDetails.PaymentAmount.toString())
    cy.FillLogLov(ShipmentSelector.APPaymentCurrency, aPPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(ShipmentSelector.APPaymentCurrencyExchangeRate, aPPaymentDetails.Rate.toString())
    cy.FillDate(ShipmentSelector.APPaymentRegisterDate, aPPaymentDetails.RegisterDate)
    cy.FillLogLov(ShipmentSelector.APPaymentBranch, aPPaymentDetails.Branch, true)
    cy.DefineRequestWait("GET", "**/apinvoiceviews/**", "WaitAPInvoiceView")
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode("WaitAPInvoiceView", 200)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}
export function SaveAPPayment() {
    cy.DefineRequestWait("POST", "**/appayments", "WaitPostAPPayments")
    cy.Click(ShipmentSelector.APPaymentSaveButton, null)
}
export function ApproveAPPayment() {
    cy.DefineRequestWait("PUT", "**/appayments", "WaitPutAPPayments")
    cy.Click(ShipmentSelector.APPaymentApproveButton, null)
}
export function PayAPInvoice() {
    SaveAPPayment()
    ApproveAPPayment()

}
export function FillARPaymentDetails(aRPaymentDetails: ARPaymentDetails) {
    if (aRPaymentDetails.Partner) {
        cy.FillLogLov(ShipmentSelector.ARPaymentPartner, aRPaymentDetails.Partner, false)
    }
    cy.FillLogLov(ShipmentSelector.ARPaymentPaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ShipmentSelector.ARPaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.Click(ShipmentSelector.OkAddARPayment, null)
}
export function SaveARPayment() {
    cy.DefineRequestWait("POST", "**/arpayments", "WaitPostARPayments")
    cy.Click(ShipmentSelector.ARPaymentSave, null)
    BaseAssertion.AssertStatusCode("WaitPostARPayments", 200)
}
export function ApproveARPayment() {
    cy.DefineRequestWait("PUT", "**/arpayments", "WaitPutARPayments")
    cy.Click(ShipmentSelector.ARPaymentBApprove, null)
}
export function PayARInvoice() {
    SaveARPayment()
    ApproveARPayment()
}
export function NewARPaymentFromAccounting(aRPaymentDetails: ARPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(ShipmentSelector.ReceivableAccounting, null)
    cy.Click(ShipmentSelector.QueryLink, "New Payment")
    FillARPaymentDetails(aRPaymentDetails)
    cy.DefineRequestWait("GET", "**/arinvoiceviews/**", "WaitARInvoiceviews")
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode("WaitARInvoiceviews", 200)
    cy.wait(10000)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}
export function SendDocs() {
    cy.Click(ShipmentSelector.DocsOutTab, null)
    cy.FillLogTextBox(BaseSelectors.SearchField, "Flight Update")
    cy.get("#FU-L-DocsOut").click()
    cy.get("#FU-S-DocsOut").click()
    cy.FillLogTextBox(ShipmentSelector.EmailSearchInput, "abd@logitudeworld.com{enter}")
    cy.DefineRequestWait("POST", "**/HtmlEditor/**", "WaitSendDocs")
    cy.Click(ShipmentSelector.SendMessageButton, null)
}
export function DeleteAttachment() {
    cy.DefineRequestWait("GET", "**/DocumentsFilingExtended/**", "WaitDelete")
    cy.contains("Delete Attachment").click()
}

export function FillMainCarriage(airline: string) {
    cy.FillLogLov(ShipmentSelector.ShipmentMainCarriageCarrierId, airline, false);
    cy.FillRandomNumber(ShipmentSelector.ShipmentFlightNumber, 100, 999);
    cy.FillRandomNumber(ShipmentSelector.ShipmentMAWB, 10000000, 99999999);
}

export function OpenAWBWizard(shipmentLevel: string){
    cy.wait("@WaitLoadShipmentMenuButtons");
    cy.Click(BaseSelectors.Button, shipmentLevel + " AWB Wizard");
}

export function FillAWBWizardPackagesTab(packagesDetails: PackagesDetails[]) {
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelector.AddPackageLineInAWBWizard, null);
        cy.FillLogTextBox(ShipmentSelector.PackageQuantityInAWBWizard, packagesDetails[i].Quantity.toString());
        cy.FillLogTextBox(ShipmentSelector.PackageLengthInAWBWizard, packagesDetails[i].Length.toString());
        cy.FillLogTextBox(ShipmentSelector.PackageWidthInAWBWizard, packagesDetails[i].Width.toString());
        cy.FillLogTextBox(ShipmentSelector.PackageHeightInAWBWizard, packagesDetails[i].Height.toString());
        cy.FillLogTextBox(ShipmentSelector.PackageWeightInAWBWizard, packagesDetails[i].GrossWeight.toString());
        cy.Click(BaseSelectors.OKBtn, null);
    }
}

function AddPartner(partnerTypeId: string, partnerFieldId: string, partner?: string) {
    cy.Click("label", "Add Partners")
    cy.get(partnerTypeId).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(partnerTypeId, null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId.replace("#", "") + "']"
            cy.FillLogLov(partnerFieldSelector, partner, false)
            cy.Click(ShipmentSelector.PartnerOKButton, null)
        }
    })
}

function FillDirectAndHouseFields(shipmentDetails: ShipmentDetails) {
    FillMainFields(shipmentDetails);
    FillShipperAndConsignee(shipmentDetails);
    FillMainCarriagePorts(shipmentDetails);
}

function FillMasterFields(shipmentDetails: ShipmentDetails) {
    FillMainFields(shipmentDetails);
    FillMasterAgent(shipmentDetails);
    FillMainCarriagePorts(shipmentDetails);
}

function FillMainFields(shipmentDetails: ShipmentDetails) {
    FillDirection(shipmentDetails);
    FillTransportMode(shipmentDetails);
    FillShipmentType(shipmentDetails);
}


function FillDirection(shipmentDetails: ShipmentDetails) {
    let directionRadioSelector = ShipmentSelector.DirectionRadio(shipmentDetails.Direction);
    cy.ClickRadio(directionRadioSelector);
}


function FillTransportMode(shipmentDetails: ShipmentDetails) {
    let transportModeRadioSelector = ShipmentSelector.TransportModeRadio(shipmentDetails.TransportMode);
    cy.ClickRadio(transportModeRadioSelector);
}

function FillShipmentType(shipmentDetails: ShipmentDetails) {
    if (shipmentDetails.ShipmentType) {
        let shipmentTypeRadioSelector: string;
        if (Conditions.IsGroupage(shipmentDetails.ShipmentType)) {
            shipmentTypeRadioSelector = ShipmentSelector.GroupageShipmentTypeRadio(shipmentDetails.TransportMode);
        } else {
            shipmentTypeRadioSelector = ShipmentSelector.ShipmentTypeRadio(shipmentDetails.ShipmentType);
        }
        cy.ClickRadio(shipmentTypeRadioSelector);
    }
}

function FillShipperAndConsignee(shipmentDetails: ShipmentDetails) {
    if (Conditions.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        cy.FillLogLov(ShipmentSelector.ShipmentShipper, shipmentDetails.Shipper, false)
        cy.FillLogLov(ShipmentSelector.ShipmentConsignee, shipmentDetails.Consignee, false)
    } else {
        if (Conditions.IsImport(shipmentDetails.Direction)) {
            cy.FillLogLov(ShipmentSelector.ShipmentConsignee, shipmentDetails.Consignee, false)
        } else {
            cy.FillLogLov(ShipmentSelector.ShipmentShipper, shipmentDetails.Shipper, false)
        }
    }
}

function FillMainCarriagePorts(shipmentDetails: ShipmentDetails) {
    if (!Conditions.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        let fromPortSelector = Conditions.IsMaster(shipmentDetails.ShipmentLevel) ? ShipmentSelector.MasterMainCarriageFromPort : ShipmentSelector.ShipmentMainCarriageFromPort;
        let toPortSelector = Conditions.IsMaster(shipmentDetails.ShipmentLevel) ? ShipmentSelector.MasterMainCarriageToPort : ShipmentSelector.ShipmentMainCarriageToPort;
        cy.FillLogLov(fromPortSelector, shipmentDetails.MainCarriageFromPort, false)
        cy.FillLogLov(toPortSelector, shipmentDetails.MainCarriageToPort, false)
    }
}

function FillMasterAgent(shipmentDetails: ShipmentDetails) {
    cy.FillLogLov(ShipmentSelector.MasterAgent, shipmentDetails.Agent, false)
}