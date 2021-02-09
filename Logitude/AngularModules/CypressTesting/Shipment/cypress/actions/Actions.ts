import * as gr from '../../../Base/cypress/Actions/GenerateRandoms';
import { Selectors } from '../selectors/Selectors';
import { ShipmentDetails } from '../models/ShipmentDetails';
import { PartnersDetails } from 'cypress/models/PartnersDetails';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { PayableDetails } from 'cypress/models/PayableDetails';
import { ReceivableDetails } from 'cypress/models/ReceivableDetails';
import { APInvoiceDetails } from 'cypress/models/APInvoiceDetails';
import { ARInvoiceDetails } from 'cypress/models/ARInvoiceDetails';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import { APPaymentDetails } from 'cypress/models/APPaymentDetails';
import { ARPaymentDetails } from 'cypress/models/ARPaymentDetails'
import { URLs } from '../constants/URLs';
import { CustomerDetails } from 'cypress/models/CustomerDetails';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';

//#region Customer actions
export function NavigatesToCustomersWorkspace() {
    cy.Click(BaseSelectors.CustomersMenu, null);
}

export function AddNewCustomer(customerDetails: CustomerDetails) {
    cy.Click(Selectors.NewCustomer, null);
    cy.FillLogTextBox(Selectors.CustomerCompanyName, customerDetails.CompanyName);
    cy.FillLogTextBox(Selectors.CustomerCity, customerDetails.City);
    cy.FillLogLov(Selectors.CustomerCountry, customerDetails.Country, true);
    cy.FillLogLov(Selectors.CustomerState, customerDetails.State, true);
}

export function CreateCustomer() {
    cy.DefineRequestWait(RestAPI.POST, URLs.PartnersDomain, RequestAliases.PartnersDomainRequest)
    cy.Click(Selectors.AddCustomer, null);
}
//#endregion

export function NewCustomsCreditNoteARInvoice() {
    cy.Click(Selectors.ToggleButtonClass, Selectors.ContainsCustoms);
    cy.Click(Selectors.CreateCustomsCreditNote, null);
}

export function NavigatesToShipmentsWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
}

export function OpenNewShipmentWizard(shipmentLevel: string) {
    cy.Click(Selectors.NewShipmentToggleButton, null)
    cy.Click(Selectors.NewShipmentToggleButtonItem, shipmentLevel)
}

export function FillShipmentWizardsFields(shipmentDetails: ShipmentDetails) {
    if (!BaseAssertion.IsMaster(shipmentDetails.ShipmentLevel)) {
        FillDirectAndHouseFields(shipmentDetails);
    } else {
        FillMasterFields(shipmentDetails);
    }
}

export function CreateShipment(shipmentLevel: string) {
    let createSelector = BaseAssertion.IsMaster(shipmentLevel) ? Selectors.CreateMasterShipmentButton : Selectors.CreateShipmentButton;
    cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(createSelector, null)
}

export function UpdateShipment(saveButtonSelector: string, saveButtonSelectorContains?: string) {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(saveButtonSelector, saveButtonSelectorContains)
}

export function OpenShipment(ShipmentNumber: string) {
    cy.SelectQuickSearchFirstElement(Selectors.ShipmentSearchBar, ShipmentNumber)
}

export function CancelShipment() {
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.CancelShipmentButton, null);
    UpdateShipment(Selectors.ConfirmActionButton);
}

export function ConnectOrDisconnectShipment() {
    UpdateShipment(BaseSelectors.RedButton, "Yes");
}

export function FillGeneralTab(GrossWeight: string, MoveType: string) {
    cy.Click(Selectors.GeneralTab, null)
    cy.FillLogTextBox(Selectors.ShipmentGrossWeight, GrossWeight)
    cy.FillLogLov(Selectors.ShipmentMoveType, MoveType, true)
}

export function FillOrdersTab(packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(Selectors.OrdersTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(Selectors.OrdersAddPackage, null)

        cy.FillLogTextBox(Selectors.OrderPackageQuantity, packagesDetails[i].Quantity.toString())
        if (BaseAssertion.hasPacakageType(shipmentType)) {
            cy.FillLogLov(Selectors.OrderPackageType, packagesDetails[i].PackageType, true)
        }
        if (!BaseAssertion.IsFCL(shipmentType) && !BaseAssertion.IsFTL(shipmentType)) {
            cy.FillLogTextBox(Selectors.OrderPackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(Selectors.OrderPackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(Selectors.OrderPackageHeight, packagesDetails[i].Height.toString())
        }
        cy.FillLogTextBox(Selectors.OrderPackageGrossWeight, packagesDetails[i].GrossWeight.toString())
        cy.Click(Selectors.OrderOKButton, null)
    }
}

export function FillPartnersTab(direction: string, transportMode: string, partnersDetails: PartnersDetails) {
    cy.Click(Selectors.PartnersTab, null)

    if (BaseAssertion.IsImport(direction)) {
        AddPartner(Selectors.AddShipperButton, Selectors.ShipmentShipper, partnersDetails.Shipper)
    }
    if (BaseAssertion.IsExport(direction) || BaseAssertion.IsDrop(direction) ||
        (BaseAssertion.IsDomestic(direction) && !BaseAssertion.IsInland(transportMode))) {
        AddPartner(Selectors.AddConsigneeButton, Selectors.ShipmentConsignee, partnersDetails.Consignee)
    }
    AddPartner(Selectors.AddAgentButton, Selectors.ShipmentAgent, partnersDetails.Agent)
    if ((BaseAssertion.IsImport(direction) && BaseAssertion.IsAir(transportMode)) ||
        (BaseAssertion.IsDomestic(direction) && BaseAssertion.IsAir(transportMode))) {
        AddPartner(Selectors.AddIssuingCarrierAgentButton, Selectors.ShipmentIssuingCarrierAgent)
    }
    AddPartner(Selectors.AddCustomsAgentExportButton, Selectors.ShipmentCustomAgentExport, partnersDetails.CustomsAgentExport)
    AddPartner(Selectors.AddCustomsAgentImportButton, Selectors.ShipmentCustomAgentImport, partnersDetails.CustomsAgentImport)
    AddPartner(Selectors.AddNotify1Button, Selectors.ShipmentNotify1, partnersDetails.Notify1)
    AddPartner(Selectors.AddNotify2Button, Selectors.ShipmentNotify2, partnersDetails.Notify2)
    AddPartner(Selectors.AddShipperNotExporterButton, Selectors.ShipmentShipperNotExporter, partnersDetails.ShipperNotExporter)
    AddPartner(Selectors.AddConsigneeNotImporterButton, Selectors.ShipmentConsigneeNotImporter, partnersDetails.ConsigneeNotImporter)
    AddPartner(Selectors.AddFreightForwarderButton, Selectors.ShipmentFreightForwarder, partnersDetails.FreightForwarder)
    AddPartner(Selectors.AddColoaderButton, Selectors.ShipmentColoader, partnersDetails.Coloader)
    AddPartner(Selectors.AddCustomClearancePointButton, Selectors.ShipmentCustomClearancePoint, partnersDetails.CustomClearancePoint)
    AddPartner(Selectors.AddConsolidatorButton, Selectors.ShipmentConsolidator, partnersDetails.Consolidator)
    AddPartner(Selectors.AddReleasingAgentButton, Selectors.ShipmentReleasingAgent, partnersDetails.ReleasingAgent)
}

export function FillPackageTab(transportMode: string, packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(Selectors.PackagesTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(Selectors.AddPackage, null)
        if (BaseAssertion.hasPacakageType(shipmentType)) {
            cy.FillLogLov(Selectors.PackageType, packagesDetails[i].PackageType, true)
        }
        if (!BaseAssertion.IsFCL(shipmentType) && !BaseAssertion.IsFTL(shipmentType)) {
            cy.FillLogTextBox(Selectors.PackageQuantity, packagesDetails[i].Quantity.toString())
            cy.FillLogTextBox(Selectors.PackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(Selectors.PackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(Selectors.PackageHeight, packagesDetails[i].Height.toString())
        }
        cy.get(Selectors.PackageWeight).type(packagesDetails[i].GrossWeight.toString());

        if (BaseAssertion.IsAir(transportMode)) {
            cy.Click(Selectors.AirPackageOKButton, null)
        } else {
            cy.Click(Selectors.OceanPackageOKButton, null)
        }
    }
}

export function FillHouseInShipmentsTab(Shipper: string) {
    cy.FillLogLov(Selectors.ShipmentCustomer, Shipper, true)
}

export function FillReceivablesTab(receivableDetails: ReceivableDetails[]) {
    cy.Click(Selectors.ReceivablesTab, null)
    for (let i = 0; i < receivableDetails.length; i++) {
        cy.Click(Selectors.AddNewReceivableLine, null)
        cy.FillLogLov(Selectors.ReceivableChargesType, receivableDetails[i].ChargesType, true)
        cy.FillLogLov(Selectors.ReceivableMeasurement, receivableDetails[i].UOM, true)
        cy.get(Selectors.ReceivableQuantity).type(receivableDetails[i].Quantity.toString());
        cy.get(Selectors.ReceivableUnitPrice).type(receivableDetails[i].UnitPrice.toString());
        cy.FillLogLov(Selectors.ReceivableCurrency, receivableDetails[i].Currency, true)
        cy.get(Selectors.ShipmentReceivableRate).clear().type(receivableDetails[i].ExchangeRate.toString());
        cy.Click(Selectors.AddReceivableOkButton, null)
    }
}


export function FillPickupRouting() {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(Selectors.PickUp, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)
    cy.Click(Selectors.SaveClose, null)
}

export function EditMainCarriageLegs(Airline: string) {
    cy.Click(Selectors.EditRoutingMainCarriage, null)
    cy.FillLogLov(Selectors.ShipmentMainCarriageCarrierId, Airline, false)
    cy.FillRandomNumber(Selectors.ShipmentFlightNumber, 100, 999)
    cy.FillRandomNumber(Selectors.ShipmentMAWB, 10000000, 99999999)
    cy.Click(Selectors.ShipmentDateMaincarriageATD, null)
    cy.Click(BaseSelectors.Button, "Today")
    cy.Click(Selectors.MainCarriageOKBtn, null);
    cy.Click(Selectors.ShipmentSaveButton, null);
}

export function UpdateClosedShipment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(BaseSelectors.RedButton, "Confirm");
}

export function FillDeliveryRouting(partner: string) {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(Selectors.Delivery, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)
    cy.get(Selectors.ShipmentPickUpDeliveryToPartnerCard).then((input) => {
        if (input.text() === "" || input.text() === null) {
            cy.FillLogLov(Selectors.ShipmentPickUpDeliveryToPartnerCard, partner, false)
        }
    })
    cy.Click(Selectors.SaveClose, null)
}

export function FillPreCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.PreCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(Selectors.PreCarriage, null)
            cy.FillLogLov(Selectors.ShipmentPreCarriageTransportMode, transportMode, true)
            cy.FillLogLov(Selectors.ShipmentPreCarriageFromPort, fromPort, false)
            cy.FillLogLov(Selectors.ShipmentPreCarriageToPort, toPort, false)
            cy.Click(Selectors.PreCarriageOKBtn, null)
        } else {
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.OnCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(Selectors.OnCarriage, null)
            cy.FillLogLov(Selectors.ShipmentOnCarriageTransportMode, transportMode, true)
            cy.FillLogLov(Selectors.ShipmentOnCarriageFromPort, fromPort, false)
            cy.FillLogLov(Selectors.ShipmentOnCarriageToPort, toPort, false)
            cy.Click(Selectors.OnCarriageOKBtn, null)
        } else {
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab(payableDetails: PayableDetails) {
    cy.Click(Selectors.PayablesTab, null)
    cy.Click(Selectors.AddNewPayableLine, null)
    cy.FillLogLov(Selectors.ShipmentPayableChargesType, payableDetails.ChargesType, true);
    cy.FillLogLov(Selectors.ShipmentPayableMeasurement, payableDetails.UOM, true)
    cy.FillLogTextBox(Selectors.ShipmentPayableQuantity, payableDetails.Quantity.toString());
    cy.FillLogTextBox(Selectors.ShipmentPayableUnitPrice, payableDetails.UnitPrice.toString());
    cy.FillLogLov(Selectors.ShipmentPayableCurrency, payableDetails.Currency, true)
    cy.Click(Selectors.AddPayableOkButton, null)
}

export function CopyShipment(shipmentLevel: string) {
    cy.Click(Selectors.ShipmentMoreList, null)
    cy.Click(Selectors.CopyShipmentButton, null)
    CreateShipment(shipmentLevel);
}

export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.FillLogLov(Selectors.APInvoiceVendor, aPInvoiceDetails.Vendor, false);
    cy.FillLogTextBox(Selectors.APInvoiceInvoiceNumber, generatedInvoiceNumber)
    cy.FillLogTextBox(Selectors.APInvoiceAmountInInvoice, aPInvoiceDetails.InvoiceAmount.toString());
    cy.FillLogLov(Selectors.APInvoiceInvoiceCurrency, aPInvoiceDetails.InvoiceCurrency, true);
    cy.FillLogTextBox(Selectors.APInvoiceInvoiceExchangeRate, aPInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(Selectors.APInvoiceInvoiceDate, aPInvoiceDetails.InvoiceDate)
    cy.FillLogLov(Selectors.APInvoicePaymentTerm, aPInvoiceDetails.PaymentTerms, true);
    cy.FillDate(Selectors.APInvoiceDueDate, aPInvoiceDetails.DueDate)
    cy.FillLogTextBox(Selectors.APInvoiceVATNumber, aPInvoiceDetails.VatNo.toString())
    cy.Click(Selectors.OkCreateAPInvoiceButton, null);
    cy.Click(BaseSelectors.CheckBoxLine, null)
    cy.FillLogLov(Selectors.APInvoiceVatType, aPInvoiceDetails.VATType, true)
    cy.Click(Selectors.VatTypeApplyToAll, null)
}

export function ReceiveAPInvoice() {
    cy.DefineRequestWait(RestAPI.POST, URLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(Selectors.APInvoiceSaveButton, null)
}

export function APApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(Selectors.APInvoiceApproveButton, null)
}

export function APInvoiceCancelApproval() {
    cy.Click(BaseSelectors.MoreList, null)
    cy.DefineRequestWait(RestAPI.PUT, URLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(Selectors.APInvoiceCancelApprovalButton, null)
}

export function VoidAPInvoice() {
    cy.Click(BaseSelectors.MoreList, null)
    cy.Click(Selectors.APInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, URLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(Selectors.ConfirmWindowYes, null);
}

export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    cy.get(".ComboBox").click();
    cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    cy.FillLogLov(Selectors.ARInvoiceInvoiceCurrency, aRInvoiceDetails.InvoiceCurrency, true)
    cy.get(Selectors.ARInvoiceExchangeRate).clear().type(aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(Selectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillLogLov(Selectors.ARInvoicePaymentTerm, aRInvoiceDetails.PaymentTerms, true)
    cy.FillDate(Selectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.get(Selectors.ARInvoiceVatNumber).clear().type(aRInvoiceDetails.VATNo)
    cy.FillLogLov(Selectors.ARInvoiceBranch, aRInvoiceDetails.Branch, true)
    cy.Click(Selectors.OkCreateARInvoiceButton, null);
    cy.FillLogLov(Selectors.ARInvoiceVatType, aRInvoiceDetails.VATType, true)
    cy.Click(Selectors.VatTypeApplyToAll, null)
}

export function CreateARInvoice() {
    cy.DefineRequestWait(RestAPI.POST, URLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(Selectors.ARInvoiceSaveButton, null)
}

export function ARApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(Selectors.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice() {
    cy.Click(BaseSelectors.MoreList, null)
    cy.Click(Selectors.ARInvoiceSetAsSentButton, null)
    cy.DefineRequestWait(RestAPI.PUT, URLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click("button", "Confirm");
}

export function VoidARInvoice() {
    cy.Click(BaseSelectors.MoreList, null)
    cy.Click(Selectors.ARInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, URLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(Selectors.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(BaseSelectors.MoreList, null)
    cy.DefineRequestWait(RestAPI.PUT, URLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(Selectors.ARInvoiceCancelDraftButton, null)
    cy.Click(Selectors.ConfirmWindowYes, null)
}

export function FillAPPayment(aPPaymentDetails: APPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(Selectors.PayableAccountingTab, null)
    cy.Click(Selectors.NewAPPayment, null)
    cy.FillLogLov(Selectors.APPaymentVendor, aPPaymentDetails.Vendor, false)
    cy.FillLogLov(Selectors.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(Selectors.APPaymentAmount, aPPaymentDetails.PaymentAmount.toString())
    cy.FillLogLov(Selectors.APPaymentCurrency, aPPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(Selectors.APPaymentCurrencyExchangeRate, aPPaymentDetails.Rate.toString())
    cy.FillDate(Selectors.APPaymentRegisterDate, aPPaymentDetails.RegisterDate)
    cy.FillLogLov(Selectors.APPaymentBranch, aPPaymentDetails.Branch, true)
    cy.DefineRequestWait(RestAPI.GET, URLs.APInvoiceViews, RequestAliases.APInvoiceView)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoiceView, 200)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}

export function SaveAPPayment() {
    cy.DefineRequestWait(RestAPI.POST, URLs.APPayments, RequestAliases.APPayments)
    cy.Click(Selectors.APPaymentSaveButton, null)
}

export function ApproveAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.APPayments, RequestAliases.APPayments)
    cy.Click(Selectors.APPaymentApproveButton, null)
}

export function PayAPInvoice() {
    SaveAPPayment()
    ApproveAPPayment()

}

export function FillARPaymentDetails(aRPaymentDetails: ARPaymentDetails) {
    cy.FillLogLov(Selectors.ARPaymentPartner, aRPaymentDetails.Partner, false)
    cy.FillLogLov(Selectors.ARPaymentPaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(Selectors.ARPaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.Click(Selectors.OkAddARPayment, null)
}

export function SaveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, URLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(Selectors.ARPaymentSave, null)
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(Selectors.ARPaymentBApprove, null)
}

export function PayARInvoice() {
    SaveARPayment()
    ApproveARPayment()
}

export function NewARPaymentFromAccounting(aRPaymentDetails: ARPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(Selectors.ReceivableAccounting, null)
    cy.Click(Selectors.QueryLink, "New Payment")
    FillARPaymentDetails(aRPaymentDetails)
    cy.DefineRequestWait(RestAPI.GET, URLs.ARInvoiceViews, RequestAliases.ARInvoiceviews)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200)
    cy.wait(10000)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}

export function SendDocs() {
    cy.Click(Selectors.DocsOutTab, null)
    cy.FillLogTextBox(BaseSelectors.SearchField, "Flight Update")
    cy.get("#FU-L-DocsOut").click()
    cy.get("#FU-S-DocsOut").click()
    cy.FillLogTextBox(Selectors.EmailSearchInput, "abd@logitudeworld.com{enter}")
    cy.DefineRequestWait(RestAPI.POST, URLs.HtmlEditor, "WaitSendDocs")
    cy.Click(Selectors.SendMessageButton, null)
}

export function DeleteAttachment() {
    cy.DefineRequestWait(RestAPI.GET, URLs.DocumentsFilingExtended, "WaitDelete")
    cy.contains("Delete Attachment").click()
}

function AddPartner(partnerTypeId: string, partnerFieldId: string, partner?: string) {
    cy.Click("label", "Add Partners")

    cy.get(partnerTypeId).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(partnerTypeId, null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId.replace("#", "") + "']"
            cy.FillLogLov(partnerFieldSelector, partner, false)
            cy.Click(Selectors.PartnerOKButton, null)
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
    let directionRadioSelector = Selectors.DirectionRadio(shipmentDetails.Direction);
    cy.ClickRadio(directionRadioSelector);
}


function FillTransportMode(shipmentDetails: ShipmentDetails) {
    let transportModeRadioSelector = Selectors.TransportModeRadio(shipmentDetails.TransportMode);
    cy.ClickRadio(transportModeRadioSelector);
}

function FillShipmentType(shipmentDetails: ShipmentDetails) {
    if (shipmentDetails.ShipmentType) {
        let shipmentTypeRadioSelector: string;
        if (BaseAssertion.IsGroupage(shipmentDetails.ShipmentType)) {
            shipmentTypeRadioSelector = Selectors.GroupageShipmentTypeRadio(shipmentDetails.TransportMode);
        } else {
            shipmentTypeRadioSelector = Selectors.ShipmentTypeRadio(shipmentDetails.ShipmentType);
        }
        cy.ClickRadio(shipmentTypeRadioSelector);
    }
}

function FillShipperAndConsignee(shipmentDetails: ShipmentDetails) {
    if (BaseAssertion.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        cy.FillLogLov(Selectors.ShipmentShipper, shipmentDetails.Shipper, false)
        cy.FillLogLov(Selectors.ShipmentConsignee, shipmentDetails.Consignee, false)
    } else {
        if (BaseAssertion.IsImport(shipmentDetails.Direction)) {
            cy.FillLogLov(Selectors.ShipmentConsignee, shipmentDetails.Consignee, false)
        } else {
            cy.FillLogLov(Selectors.ShipmentShipper, shipmentDetails.Shipper, false)
        }
    }
}

function FillMainCarriagePorts(shipmentDetails: ShipmentDetails) {
    if (!BaseAssertion.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        let fromPortSelector = BaseAssertion.IsMaster(shipmentDetails.ShipmentLevel) ? Selectors.MasterMainCarriageFromPort : Selectors.ShipmentMainCarriageFromPort;
        let toPortSelector = BaseAssertion.IsMaster(shipmentDetails.ShipmentLevel) ? Selectors.MasterMainCarriageToPort : Selectors.ShipmentMainCarriageToPort;
        cy.FillLogLov(fromPortSelector, shipmentDetails.MainCarriageFromPort, false)
        cy.FillLogLov(toPortSelector, shipmentDetails.MainCarriageToPort, false)
    }
}

function FillMasterAgent(shipmentDetails: ShipmentDetails) {
    cy.FillLogLov(Selectors.MasterAgent, shipmentDetails.Agent, false)
}