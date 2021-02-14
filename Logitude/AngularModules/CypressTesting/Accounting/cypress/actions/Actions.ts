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

export function NavigatesToAccountsPayablesWorkspace() {
    cy.Click(BaseSelectors.AccountingMenu, null)
    cy.Click(ShipmentSelectors.PayableAccountingTab, null)
}

export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails, multiple = false) {
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
        cy.Click(BaseSelectors.CheckBoxLine, null)
        cy.FillLogLov(AccountingSelectors.APInvoiceVatType, aPInvoiceDetails.VATType, true)
        cy.Click(ShipmentSelectors.VatTypeApplyToAll, null)
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