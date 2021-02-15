import * as gr from '../../../Base/cypress/Actions/GenerateRandoms';
import { AccountingSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { AccountingURLs } from '../constants/URLs';
import { PayableDetails } from '../../../Shipment/cypress/models/PayableDetails';
import { ARInvoiceDetails } from 'cypress/models/ARInvoiceDetails';
import { APPaymentDetails } from 'cypress/models/APPaymentDetails';
import { ARPaymentDetails } from 'cypress/models/ARPaymentDetails';
import { BaseURLs } from '../../../Base/cypress/constants/URLs';
import { QuickSearchDetails } from '../../../Base/cypress/models/QuickSearchDetails';

//#region CustomInvoices
export function NewCustomsCreditNoteARInvoice() {
    cy.Click(BaseSelectors.ToggleButtonClass, AccountingSelectors.ContainsCustoms, true);
    cy.Click(AccountingSelectors.CreateCustomsCreditNote, null, false);
}
export function NewCustomsARInvoice() {
    cy.Click(BaseSelectors.ToggleButtonClass, AccountingSelectors.ContainsCustoms, true);
    cy.Click(AccountingSelectors.CreateCustomsARInvoice, null, false);
}
//#endregion
export function NavigatesToAccountsPayablesWorkspace() {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(AccountingSelectors.PayableAccountingTab, null)
}
export function NavigatesToAccountsReceivableWorkspace(){
    cy.Click(BaseSelectors.AccountingMenu,null)
    cy.Click(AccountingSelectors.ReceivableAccounting,null)
}
//#region APInvoice
export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails, multiple = false,havePayableVendor?:string) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.FillLogLov(AccountingSelectors.APInvoiceVendor, aPInvoiceDetails.Vendor, false);
    cy.FillLogTextBox(AccountingSelectors.APInvoiceInvoiceNumber, generatedInvoiceNumber)
    cy.FillLogTextBox(AccountingSelectors.APInvoiceAmountInInvoice, aPInvoiceDetails.InvoiceAmount.toString());
    cy.FillLogLov(AccountingSelectors.APInvoiceInvoiceCurrency, aPInvoiceDetails.InvoiceCurrency, true);
    cy.FillLogTextBox(AccountingSelectors.APInvoiceInvoiceExchangeRate, aPInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(AccountingSelectors.APInvoiceInvoiceDate, aPInvoiceDetails.InvoiceDate)
    cy.FillLogLov(AccountingSelectors.APInvoicePaymentTerm, aPInvoiceDetails.PaymentTerms, true);
    cy.FillDate(AccountingSelectors.APInvoiceDueDate, aPInvoiceDetails.DueDate)
    cy.FillLogTextBox(AccountingSelectors.APInvoiceVATNumber, aPInvoiceDetails.VatNo.toString())
    cy.Click(AccountingSelectors.OkCreateAPInvoiceButton, null);
    if (!multiple) {
        if(!havePayableVendor)
        {
        cy.Click(BaseSelectors.CheckBoxLine, null)
        }
        cy.FillLogLov(AccountingSelectors.APInvoiceVatType, aPInvoiceDetails.VATType, true)
        cy.Click(AccountingSelectors.VatTypeApplyToAll, null)
    }
}
export function ReceiveAPInvoice() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(AccountingSelectors.APInvoiceSaveButton, null)
}

export function SaveAPInvoice() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.InvoiceDomain, RequestAliases.InvoiceDomain)
    cy.Click(AccountingSelectors.APInvoiceSaveButton, null)
}

export function APApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(AccountingSelectors.APInvoiceApproveButton, null)
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

//#endregion
//#region ARInvoice
export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    // cy.get(".ComboBox").click();
    // cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    if(aRInvoiceDetails.Partner){
        cy.FillLogLov(AccountingSelectors.ARInvoicePartner,aRInvoiceDetails.Partner,false)
    }
    cy.FillLogLov(AccountingSelectors.ARInvoiceInvoiceCurrency, aRInvoiceDetails.InvoiceCurrency, true)
    cy.FillLogTextBox(AccountingSelectors.ARInvoiceExchangeRate, aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(AccountingSelectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillLogLov(AccountingSelectors.ARInvoicePaymentTerm, aRInvoiceDetails.PaymentTerms, true)
    cy.FillDate(AccountingSelectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.FillLogTextBox(AccountingSelectors.ARInvoiceVatNumber, aRInvoiceDetails.VATNo)
    cy.FillLogLov(AccountingSelectors.ARInvoiceBranch, aRInvoiceDetails.Branch, true)
    if(aRInvoiceDetails.Partner){
        cy.Click(BaseSelectors.RedButton,"OK")
        cy.Click(BaseSelectors.CheckBoxLine,null)
    }
    else{
    cy.Click(AccountingSelectors.OkCreateARInvoiceButton, null);
    cy.FillLogLov(AccountingSelectors.ARInvoiceVatType, aRInvoiceDetails.VATType, true)
    cy.Click(AccountingSelectors.VatTypeApplyToAll, null)
    }
}

export function CreateARInvoice(Constituent?:boolean) {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    if(Constituent){
        cy.Click(AccountingSelectors.GeneralSave, null)

    }
    else{
        cy.Click(AccountingSelectors.ARInvoiceSaveButton, null)

    }
}
export function ARApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(AccountingSelectors.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(AccountingSelectors.ARInvoiceSetAsSentButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click("button", "Confirm");
}

export function VoidARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(AccountingSelectors.ARInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(AccountingSelectors.ARInvoiceCancelDraftButton, null)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null)
}
//#endregion
//#region Add Two Shipment Lines And Edit Amount
export function AddTwoShipmentLinesAndEditAmount(shipmentNumbers: string[], VATType: string, payableDetails: PayableDetails) {
    const amount = CalculateAmount(payableDetails.Quantity, payableDetails.UnitPrice);
    for (let i = 0; i < shipmentNumbers.length; i++) {
        var quickSearchDetails = {
            Selector: ShipmentSelectors.ShipmentSearchBar,
            Parent: ShipmentSelectors.ShipmentSearchParent,
            ParentClass: ShipmentSelectors.ShipmentSearchParentClass,
            WaitURL: BaseURLs.GetQuickSearch,
            Value: shipmentNumbers[i]
        } as QuickSearchDetails;
        cy.SelectQuickSearchFirstElement(quickSearchDetails);
        
        cy.get(AccountingSelectors.EditShipmentLine).children().eq(i).click();
        cy.FillLogTextBox(AccountingSelectors.APInvoiceLineForiegnCurrencyAmount, amount.toString());
        cy.FillLogLov(AccountingSelectors.APInvoiceVatType, VATType, true);
        cy.Click(BaseSelectors.Button, BaseSelectors.ContainsApplytoall);
        cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.InvoiceDomain, RequestAliases.APInvoicesRequest);
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
        BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
    }
    const totalAmount = amount * shipmentNumbers.length;
    cy.FillLogTextBox(AccountingSelectors.APInvoiceAmountInInvoiceCurrency, totalAmount.toString());
}
//#endregion
//#region CalculateAmount
function CalculateAmount(Quantity: number, UnitPrice: number) : number {
    return Quantity * UnitPrice;
}
//#endregion
//#region ConsolidationInvoice
export function FillconsolidationInvoiceDetails(ARInvoiceData:ARInvoiceDetails){
    NavigatesToAccountsReceivableWorkspace()
    cy.Click(BaseSelectors.ToggleIcon,null)
    cy.Click(BaseSelectors.button,"New consolidation invoice")
    FillARInvoiceDetails(ARInvoiceData)

}
export function ApproveConsilidationInvoice(){
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ConsilidationInvoiceDomain, RequestAliases.ConsilidationInvoiceDomain)
    cy.Click(AccountingSelectors.ARInvoiceApproveButton, null)
}
//#endregion


//#region APPayment
export function FillAPPayment(aPPaymentDetails: APPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(AccountingSelectors.PayableAccountingTab, null)
    cy.Click(AccountingSelectors.NewAPPayment, null)
    cy.FillLogLov(AccountingSelectors.APPaymentVendor, aPPaymentDetails.Vendor, false)
    cy.FillLogLov(AccountingSelectors.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(AccountingSelectors.APPaymentAmount, aPPaymentDetails.PaymentAmount.toString())
    cy.FillLogLov(AccountingSelectors.APPaymentCurrency, aPPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(AccountingSelectors.APPaymentCurrencyExchangeRate, aPPaymentDetails.Rate.toString())
    cy.FillDate(AccountingSelectors.APPaymentRegisterDate, aPPaymentDetails.RegisterDate)
    cy.FillLogLov(AccountingSelectors.APPaymentBranch, aPPaymentDetails.Branch, true)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.APInvoiceViews, RequestAliases.APInvoiceView)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoiceView, 200)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}

export function SaveAPPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.APPayments, RequestAliases.APPayments)
    cy.Click(AccountingSelectors.APPaymentSaveButton, null)
}

export function ApproveAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APPayments, RequestAliases.APPayments)
    cy.Click(AccountingSelectors.APPaymentApproveButton, null)
}

export function PayAPInvoice() {
    SaveAPPayment()
    ApproveAPPayment()

}
//#endregion
//#region ARPayment
export function FillARPaymentDetails(aRPaymentDetails: ARPaymentDetails) {
    if (aRPaymentDetails.Partner) {
        cy.FillLogLov(AccountingSelectors.ARPaymentPartner, aRPaymentDetails.Partner, false)
    }
    cy.FillLogLov(AccountingSelectors.ARPaymentPaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(AccountingSelectors.ARPaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.Click(AccountingSelectors.OkAddARPayment, null)
}

export function SaveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(AccountingSelectors.ARPaymentSave, null)
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(AccountingSelectors.ARPaymentBApprove, null)
}

export function PayARInvoice() {
    SaveARPayment()
    ApproveARPayment()
}

export function NewARPaymentFromAccounting(aRPaymentDetails: ARPaymentDetails, invoiceNumber: string) {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(AccountingSelectors.ReceivableAccounting, null)
    cy.Click(AccountingSelectors.QueryLink, "New Payment")
    FillARPaymentDetails(aRPaymentDetails)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViews, RequestAliases.ARInvoiceviews)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200)
    cy.wait(10000)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}
//#endregion