import * as gr from '../../../Base/cypress/Actions/GenerateRandoms';
import { AccountingSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { APInvoiceDetails } from "../../../Shipment/cypress/models/APInvoiceDetails";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { AccountingURLs } from '../constants/URLs';
import { PayableDetails } from '../../../Shipment/cypress/models/PayableDetails';
import { ARInvoiceDetails } from '../../../Shipment/cypress/models/ARInvoiceDetails';

export function NavigatesToAccountsPayablesWorkspace() {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(ShipmentSelectors.PayableAccountingTab, null)
}

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
        cy.Click(ShipmentSelectors.VatTypeApplyToAll, null)
    }
}
export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    // cy.get(".ComboBox").click();
    // cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    if(aRInvoiceDetails.Partner){
        cy.FillLogLov(ShipmentSelectors.ARInvoicePartner,aRInvoiceDetails.Partner,false)
    }
    cy.FillLogLov(ShipmentSelectors.ARInvoiceInvoiceCurrency, aRInvoiceDetails.InvoiceCurrency, true)
    cy.FillLogTextBox(ShipmentSelectors.ARInvoiceExchangeRate, aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(ShipmentSelectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillLogLov(ShipmentSelectors.ARInvoicePaymentTerm, aRInvoiceDetails.PaymentTerms, true)
    cy.FillDate(ShipmentSelectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.FillLogTextBox(ShipmentSelectors.ARInvoiceVatNumber, aRInvoiceDetails.VATNo)
    cy.FillLogLov(ShipmentSelectors.ARInvoiceBranch, aRInvoiceDetails.Branch, true)
    if(aRInvoiceDetails.Partner){
        cy.Click(BaseSelectors.RedButton,"OK")
        cy.Click(BaseSelectors.CheckBoxLine,null)
    }
    else{
    cy.Click(ShipmentSelectors.OkCreateARInvoiceButton, null);
    cy.FillLogLov(ShipmentSelectors.ARInvoiceVatType, aRInvoiceDetails.VATType, true)
    cy.Click(ShipmentSelectors.VatTypeApplyToAll, null)
    }
}

export function CreateARInvoice(Constituent?:boolean) {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    if(Constituent){
        cy.Click(ShipmentSelectors.GeneralSave, null)

    }
    else{
        cy.Click(ShipmentSelectors.ARInvoiceSaveButton, null)

    }
}
export function AddTwoShipmentLinesAndEditAmount(shipmentNumbers: string[], VATType: string, payableDetails: PayableDetails) {
    const amount = CalculateAmount(payableDetails.Quantity, payableDetails.UnitPrice);
    for (let i = 0; i < shipmentNumbers.length; i++) {
        cy.SelectQuickSearchFirstElement(ShipmentSelectors.ShipmentSearchBar, shipmentNumbers[i]);
        cy.get(ShipmentSelectors.EditShipmentLine).children().eq(i).click();
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

function CalculateAmount(Quantity: number, UnitPrice: number) : number {
    return Quantity * UnitPrice;
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
export function FillconsolidationInvoiceDetails(){
    cy.Click(BaseSelectors.AccountingMenu,null)
    cy.Click(AccountingSelectors.ReceivableAccounting,null)
    cy.get("label").contains("New").click()
    cy.Click("button","New consolidation invoice")


}