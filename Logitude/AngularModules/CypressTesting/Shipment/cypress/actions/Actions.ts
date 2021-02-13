import { ShipmentSelectors } from '../selectors/Selectors';
import { ShipmentDetails } from '../models/ShipmentDetails';
import { PartnersDetails } from 'cypress/models/PartnersDetails';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { PayableDetails } from 'cypress/models/PayableDetails';
import { ReceivableDetails } from 'cypress/models/ReceivableDetails';
import { ARInvoiceDetails } from 'cypress/models/ARInvoiceDetails';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import { APPaymentDetails } from 'cypress/models/APPaymentDetails';
import { ARPaymentDetails } from 'cypress/models/ARPaymentDetails'
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as Conditions from "../actions/Conditions";
import { AccountingURLs } from '../../../Accounting/cypress/constants/URLs';
import { AccountingSelectors } from '../../../Accounting/cypress/selectors/Selectors';

export function NewCustomsCreditNoteARInvoice() {
    cy.Click(ShipmentSelectors.ToggleButtonClass, ShipmentSelectors.ContainsCustoms, true);
    cy.Click(ShipmentSelectors.CreateCustomsCreditNote, null, false);
}

export function NavigatesToShipmentsWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelectors.ShipmentTab, null)
}

export function OpenNewShipmentWizard(shipmentLevel: string) {
    cy.Click(ShipmentSelectors.NewShipmentToggleButton, null)
    cy.Click(ShipmentSelectors.NewShipmentToggleButtonItem, shipmentLevel)
}

export function FillShipmentWizardsFields(shipmentDetails: ShipmentDetails) {
    if (!Conditions.IsMaster(shipmentDetails.ShipmentLevel)) {
        FillDirectAndHouseFields(shipmentDetails);
    } else {
        FillMasterFields(shipmentDetails);
    }
}

export function CreateShipment(shipmentLevel: string) {
    let createSelector = Conditions.IsMaster(shipmentLevel) ? ShipmentSelectors.CreateMasterShipmentButton : ShipmentSelectors.CreateShipmentButton;
    cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(createSelector, null)
}

export function UpdateShipment(saveButtonSelector: string, saveButtonSelectorContains?: string) {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(saveButtonSelector, saveButtonSelectorContains)
}

export function OpenShipment(shipmentNumber: string) {
    cy.DefineRequestWait("GET", "**/ngMetaData/getmenubuttongrouppms?**", "WaitLoadShipmentMenuButtons");
    cy.SelectQuickSearchFirstElement(ShipmentSelectors.ShipmentSearchBar, shipmentNumber)
}

export function CancelShipment() {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.CancelShipmentButton, null);
    UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}

export function ConnectOrDisconnectShipment() {
    UpdateShipment(BaseSelectors.RedButton, "Yes");
}

export function FillGeneralTab(GrossWeight: string, MoveType: string) {
    cy.Click(ShipmentSelectors.GeneralTab, null)
    cy.FillLogTextBox(ShipmentSelectors.ShipmentGrossWeight, GrossWeight)
    cy.FillLogLov(ShipmentSelectors.ShipmentMoveType, MoveType, true)
}

export function FillOrdersTab(packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(ShipmentSelectors.OrdersTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.OrdersAddPackage, null)

        cy.FillLogTextBox(ShipmentSelectors.OrderPackageQuantity, packagesDetails[i].Quantity.toString())
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelectors.OrderPackageType, packagesDetails[i].PackageType, true)
        }
        if (!Conditions.IsFCL(shipmentType) && !Conditions.IsFTL(shipmentType)) {
            cy.FillLogTextBox(ShipmentSelectors.OrderPackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(ShipmentSelectors.OrderPackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(ShipmentSelectors.OrderPackageHeight, packagesDetails[i].Height.toString())
        }
        cy.FillLogTextBox(ShipmentSelectors.OrderPackageGrossWeight, packagesDetails[i].GrossWeight.toString())
        cy.Click(ShipmentSelectors.OrderOKButton, null)
    }
}

export function FillPartnersTab(direction: string, transportMode: string, partnersDetails: PartnersDetails) {
    cy.Click(ShipmentSelectors.PartnersTab, null)

    if (Conditions.IsImport(direction)) {
        AddPartner(ShipmentSelectors.AddShipperButton, ShipmentSelectors.ShipmentShipper, partnersDetails.Shipper)
    }
    if (Conditions.IsExport(direction) || Conditions.IsDrop(direction) || (Conditions.IsDomestic(direction) && !Conditions.IsInland(transportMode))) {
        AddPartner(ShipmentSelectors.AddConsigneeButton, ShipmentSelectors.ShipmentConsignee, partnersDetails.Consignee)
    }
    AddPartner(ShipmentSelectors.AddAgentButton, ShipmentSelectors.ShipmentAgent, partnersDetails.Agent)
    if ((Conditions.IsImport(direction) && Conditions.IsAir(transportMode)) || (Conditions.IsDomestic(direction) && Conditions.IsAir(transportMode))) {
        AddPartner(ShipmentSelectors.AddIssuingCarrierAgentButton, ShipmentSelectors.ShipmentIssuingCarrierAgent)
    }
    AddPartner(ShipmentSelectors.AddCustomsAgentExportButton, ShipmentSelectors.ShipmentCustomAgentExport, partnersDetails.CustomsAgentExport)
    AddPartner(ShipmentSelectors.AddCustomsAgentImportButton, ShipmentSelectors.ShipmentCustomAgentImport, partnersDetails.CustomsAgentImport)
    AddPartner(ShipmentSelectors.AddNotify1Button, ShipmentSelectors.ShipmentNotify1, partnersDetails.Notify1)
    AddPartner(ShipmentSelectors.AddNotify2Button, ShipmentSelectors.ShipmentNotify2, partnersDetails.Notify2)
    AddPartner(ShipmentSelectors.AddShipperNotExporterButton, ShipmentSelectors.ShipmentShipperNotExporter, partnersDetails.ShipperNotExporter)
    AddPartner(ShipmentSelectors.AddConsigneeNotImporterButton, ShipmentSelectors.ShipmentConsigneeNotImporter, partnersDetails.ConsigneeNotImporter)
    AddPartner(ShipmentSelectors.AddFreightForwarderButton, ShipmentSelectors.ShipmentFreightForwarder, partnersDetails.FreightForwarder)
    AddPartner(ShipmentSelectors.AddColoaderButton, ShipmentSelectors.ShipmentColoader, partnersDetails.Coloader)
    AddPartner(ShipmentSelectors.AddCustomClearancePointButton, ShipmentSelectors.ShipmentCustomClearancePoint, partnersDetails.CustomClearancePoint)
    AddPartner(ShipmentSelectors.AddConsolidatorButton, ShipmentSelectors.ShipmentConsolidator, partnersDetails.Consolidator)
    AddPartner(ShipmentSelectors.AddReleasingAgentButton, ShipmentSelectors.ShipmentReleasingAgent, partnersDetails.ReleasingAgent)
}

export function FillPackageTab(transportMode: string, packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddPackage, null)
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelectors.PackageType, packagesDetails[i].PackageType, true)
        }
        if (!Conditions.IsFCL(shipmentType) && !Conditions.IsFTL(shipmentType)) {
            cy.FillLogTextBox(ShipmentSelectors.PackageQuantity, packagesDetails[i].Quantity.toString())
            cy.FillLogTextBox(ShipmentSelectors.PackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(ShipmentSelectors.PackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(ShipmentSelectors.PackageHeight, packagesDetails[i].Height.toString())
        }
        cy.get(ShipmentSelectors.PackageWeight).type(packagesDetails[i].GrossWeight.toString());

        if (Conditions.IsAir(transportMode)) {
            cy.Click(ShipmentSelectors.AirPackageOKButton, null)
        } else {
            cy.Click(ShipmentSelectors.OceanPackageOKButton, null)
        }
    }
}

export function FillHouseInShipmentsTab(Shipper: string) {
    cy.FillLogLov(ShipmentSelectors.ShipmentCustomer, Shipper, true)
}

export function FillReceivablesTab(receivableDetails: ReceivableDetails[]) {
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    for (let i = 0; i < receivableDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddNewReceivableLine, null)
        cy.FillLogLov(ShipmentSelectors.ReceivableChargesType, receivableDetails[i].ChargesType, true)
        cy.FillLogLov(ShipmentSelectors.ReceivableMeasurement, receivableDetails[i].UOM, true)
        cy.get(ShipmentSelectors.ReceivableQuantity).type(receivableDetails[i].Quantity.toString());
        cy.get(ShipmentSelectors.ReceivableUnitPrice).type(receivableDetails[i].UnitPrice.toString());
        cy.FillLogLov(ShipmentSelectors.ReceivableCurrency, receivableDetails[i].Currency, true)
        cy.get(ShipmentSelectors.ShipmentReceivableRate).clear().type(receivableDetails[i].ExchangeRate.toString());
        cy.Click(ShipmentSelectors.AddReceivableOkButton, null)
    }
}


export function FillPickupRouting() {
    cy.Click(ShipmentSelectors.RoutingsTab, null, true)
    cy.Click(ShipmentSelectors.RoutingToggle, null, true)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(ShipmentSelectors.PickUp, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)
    cy.Click(ShipmentSelectors.SaveClose, null)
}

export function EditMainCarriageLegs(Airline: string) {
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null)
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, Airline, false)
    cy.FillRandomNumber(ShipmentSelectors.ShipmentFlightNumber, 100, 999)
    cy.FillRandomNumber(ShipmentSelectors.ShipmentMAWB, 10000000, 99999999)
    cy.Click(ShipmentSelectors.ShipmentDateMaincarriageATD, null)
    cy.Click(BaseSelectors.Button, "Today")
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
    //cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
}

export function UpdateClosedShipment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(BaseSelectors.RedButton, "Confirm");
}

export function FillDeliveryRouting(partner: string) {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.RoutingToggle, null, true)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(ShipmentSelectors.Delivery, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)

    cy.FillLogLov(ShipmentSelectors.ShipmentPickUpDeliveryToPartnerCard, partner, false)

    cy.Click(ShipmentSelectors.SaveClose, null)
}

export function FillPreCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.RoutingToggle, null)
    cy.get(ShipmentSelectors.PreCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(ShipmentSelectors.PreCarriage, null)
            cy.FillLogLov(ShipmentSelectors.ShipmentPreCarriageTransportMode, transportMode, true)
            cy.FillLogLov(ShipmentSelectors.ShipmentPreCarriageFromPort, fromPort, false)
            cy.FillLogLov(ShipmentSelectors.ShipmentPreCarriageToPort, toPort, false)
            cy.Click(ShipmentSelectors.PreCarriageOKBtn, null)
        } else {
            cy.Click(ShipmentSelectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.RoutingToggle, null)
    cy.get(ShipmentSelectors.OnCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(ShipmentSelectors.OnCarriage, null)
            cy.FillLogLov(ShipmentSelectors.ShipmentOnCarriageTransportMode, transportMode, true)
            cy.FillLogLov(ShipmentSelectors.ShipmentOnCarriageFromPort, fromPort, false)
            cy.FillLogLov(ShipmentSelectors.ShipmentOnCarriageToPort, toPort, false)
            cy.Click(ShipmentSelectors.OnCarriageOKBtn, null)
        } else {
            cy.Click(ShipmentSelectors.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab(payableDetails: PayableDetails) {
    cy.Click(ShipmentSelectors.PayablesTab, null)
    cy.Click(ShipmentSelectors.AddNewPayableLine, null)
    cy.FillLogLov(ShipmentSelectors.ShipmentPayableChargesType, payableDetails.ChargesType, true);
    cy.FillLogLov(ShipmentSelectors.ShipmentPayableMeasurement, payableDetails.UOM, true)
    cy.FillLogTextBox(ShipmentSelectors.ShipmentPayableQuantity, payableDetails.Quantity.toString());
    cy.FillLogTextBox(ShipmentSelectors.ShipmentPayableUnitPrice, payableDetails.UnitPrice.toString());
    cy.FillLogLov(ShipmentSelectors.ShipmentPayableCurrency, payableDetails.Currency, true)
    if (payableDetails.Vendor)
        cy.FillLogLov(ShipmentSelectors.ShipmentPayableVendor, payableDetails.Vendor, true)

    cy.DefineRequestWait(RestAPI.GET, '**/cardviews/**', 'cardviews')

    cy.Click(ShipmentSelectors.AddPayableOkButton, null)

    BaseAssertion.AssertStatusCode('cardviews', 200)
}

export function CopyShipment(shipmentLevel: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true)
    cy.Click(ShipmentSelectors.CopyShipmentButton, null)
    CreateShipment(shipmentLevel);
}

export function APInvoiceCancelApproval() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(AccountingSelectors.APInvoiceCancelApprovalButton, null)
}

export function VoidAPInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(AccountingSelectors.APInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null);
}

export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    cy.get(".ComboBox").click();
    cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    cy.FillLogLov(ShipmentSelectors.ARInvoiceInvoiceCurrency, aRInvoiceDetails.InvoiceCurrency, true)
    cy.get(ShipmentSelectors.ARInvoiceExchangeRate).clear().type(aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(ShipmentSelectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillLogLov(ShipmentSelectors.ARInvoicePaymentTerm, aRInvoiceDetails.PaymentTerms, true)
    cy.FillDate(ShipmentSelectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.get(ShipmentSelectors.ARInvoiceVatNumber).clear().type(aRInvoiceDetails.VATNo)
    cy.FillLogLov(ShipmentSelectors.ARInvoiceBranch, aRInvoiceDetails.Branch, true)
    cy.Click(ShipmentSelectors.OkCreateARInvoiceButton, null);
    cy.FillLogLov(ShipmentSelectors.ARInvoiceVatType, aRInvoiceDetails.VATType, true)
    cy.Click(ShipmentSelectors.VatTypeApplyToAll, null)
}

export function CreateARInvoice() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ShipmentSelectors.ARInvoiceSaveButton, null)
}

export function ARApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ShipmentSelectors.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(ShipmentSelectors.ARInvoiceSetAsSentButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click("button", "Confirm");
}

export function VoidARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(ShipmentSelectors.ARInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ShipmentSelectors.ARInvoiceCancelDraftButton, null)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null)
}

export function FillAPPayment(aPPaymentDetails: APPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(ShipmentSelectors.PayableAccountingTab, null)
    cy.Click(ShipmentSelectors.NewAPPayment, null)
    cy.FillLogLov(ShipmentSelectors.APPaymentVendor, aPPaymentDetails.Vendor, false)
    cy.FillLogLov(ShipmentSelectors.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ShipmentSelectors.APPaymentAmount, aPPaymentDetails.PaymentAmount.toString())
    cy.FillLogLov(ShipmentSelectors.APPaymentCurrency, aPPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(ShipmentSelectors.APPaymentCurrencyExchangeRate, aPPaymentDetails.Rate.toString())
    cy.FillDate(ShipmentSelectors.APPaymentRegisterDate, aPPaymentDetails.RegisterDate)
    cy.FillLogLov(ShipmentSelectors.APPaymentBranch, aPPaymentDetails.Branch, true)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.APInvoiceViews, RequestAliases.APInvoiceView)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoiceView, 200)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}

export function SaveAPPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.APPayments, RequestAliases.APPayments)
    cy.Click(ShipmentSelectors.APPaymentSaveButton, null)
}

export function ApproveAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APPayments, RequestAliases.APPayments)
    cy.Click(ShipmentSelectors.APPaymentApproveButton, null)
}

export function PayAPInvoice() {
    SaveAPPayment()
    ApproveAPPayment()

}

export function FillARPaymentDetails(aRPaymentDetails: ARPaymentDetails) {
    if (aRPaymentDetails.Partner) {
        cy.FillLogLov(ShipmentSelectors.ARPaymentPartner, aRPaymentDetails.Partner, false)
    }
    cy.FillLogLov(ShipmentSelectors.ARPaymentPaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ShipmentSelectors.ARPaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.Click(ShipmentSelectors.OkAddARPayment, null)
}

export function SaveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(ShipmentSelectors.ARPaymentSave, null)
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(ShipmentSelectors.ARPaymentBApprove, null)
}

export function PayARInvoice() {
    SaveARPayment()
    ApproveARPayment()
}

export function NewARPaymentFromAccounting(aRPaymentDetails: ARPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(ShipmentSelectors.ReceivableAccounting, null)
    cy.Click(ShipmentSelectors.QueryLink, "New Payment")
    FillARPaymentDetails(aRPaymentDetails)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViews, RequestAliases.ARInvoiceviews)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200)
    cy.wait(10000)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}

export function SendDocs() {
    cy.Click(ShipmentSelectors.DocsOutTab, null)
    cy.FillLogTextBox(BaseSelectors.SearchField, "Flight Update")
    cy.get("#FU-L-DocsOut").click()
    cy.get("#FU-S-DocsOut").click()
    cy.FillLogTextBox(ShipmentSelectors.EmailSearchInput, "abd@logitudeworld.com{enter}")
    cy.DefineRequestWait(RestAPI.POST, URLs.HtmlEditor, "WaitSendDocs")
    cy.Click(ShipmentSelectors.SendMessageButton, null)
}

export function DeleteAttachment() {
    cy.DefineRequestWait(RestAPI.GET, URLs.DocumentsFilingExtended, "WaitDelete")
    cy.contains("Delete Attachment").click()
}

export function FillMainCarriage(airline: string) {
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, airline, false);
    cy.FillRandomNumber(ShipmentSelectors.ShipmentFlightNumber, 100, 999);
    cy.FillRandomNumber(ShipmentSelectors.ShipmentMAWB, 10000000, 99999999);
}

export function OpenAWBWizard(shipmentLevel: string) {
    cy.wait("@WaitLoadShipmentMenuButtons");
    cy.Click(BaseSelectors.Button, shipmentLevel + " AWB Wizard");
}

export function FillAWBWizardPackagesTab(packagesDetails: PackagesDetails[]) {
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddPackageLineInAWBWizard, null);
        cy.FillLogTextBox(ShipmentSelectors.PackageQuantityInAWBWizard, packagesDetails[i].Quantity.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageLengthInAWBWizard, packagesDetails[i].Length.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageWidthInAWBWizard, packagesDetails[i].Width.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageHeightInAWBWizard, packagesDetails[i].Height.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageWeightInAWBWizard, packagesDetails[i].GrossWeight.toString());
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
            cy.Click(ShipmentSelectors.PartnerOKButton, null)
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
    let directionRadioSelector = ShipmentSelectors.DirectionRadio(shipmentDetails.Direction);
    cy.ClickRadio(directionRadioSelector);
}


function FillTransportMode(shipmentDetails: ShipmentDetails) {
    let transportModeRadioSelector = ShipmentSelectors.TransportModeRadio(shipmentDetails.TransportMode);
    cy.ClickRadio(transportModeRadioSelector);
}

function FillShipmentType(shipmentDetails: ShipmentDetails) {
    if (shipmentDetails.ShipmentType) {
        let shipmentTypeRadioSelector: string;
        if (Conditions.IsGroupage(shipmentDetails.ShipmentType)) {
            shipmentTypeRadioSelector = ShipmentSelectors.GroupageShipmentTypeRadio(shipmentDetails.TransportMode);
        } else {
            shipmentTypeRadioSelector = ShipmentSelectors.ShipmentTypeRadio(shipmentDetails.ShipmentType);
        }
        cy.ClickRadio(shipmentTypeRadioSelector);
    }
}

function FillShipperAndConsignee(shipmentDetails: ShipmentDetails) {
    if (Conditions.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, false)
        cy.FillLogLov(ShipmentSelectors.ShipmentConsignee, shipmentDetails.Consignee, false)
    } else {
        if (Conditions.IsImport(shipmentDetails.Direction)) {
            cy.FillLogLov(ShipmentSelectors.ShipmentConsignee, shipmentDetails.Consignee, false)
        } else {
            cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, false)
        }
    }
}

function FillMainCarriagePorts(shipmentDetails: ShipmentDetails) {
    if (!Conditions.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        let fromPortSelector = Conditions.IsMaster(shipmentDetails.ShipmentLevel) ? ShipmentSelectors.MasterMainCarriageFromPort : ShipmentSelectors.ShipmentMainCarriageFromPort;
        let toPortSelector = Conditions.IsMaster(shipmentDetails.ShipmentLevel) ? ShipmentSelectors.MasterMainCarriageToPort : ShipmentSelectors.ShipmentMainCarriageToPort;
        cy.FillLogLov(fromPortSelector, shipmentDetails.MainCarriageFromPort, false)
        cy.FillLogLov(toPortSelector, shipmentDetails.MainCarriageToPort, false)
    }
}

function FillMasterAgent(shipmentDetails: ShipmentDetails) {
    cy.FillLogLov(ShipmentSelectors.MasterAgent, shipmentDetails.Agent, false)
}