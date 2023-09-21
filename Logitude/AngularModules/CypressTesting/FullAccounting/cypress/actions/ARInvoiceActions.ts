import { ARInvoiceSelectors } from "../selectors/ARInvoiceSelectors";
import { ARInvoiceDetails } from "cypress/models/ARInvoiceDetails";
import { InvoiceLineDetails } from "cypress/models/InvoiceLineDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesARInvoiceWizerd() {
    debugger

    cy.Click(ARInvoiceSelectors.NewInvoiceMenu, null, true)
    cy.Click(ARInvoiceSelectors.NewGeneralInvoice, null, true)
}

export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    debugger
    const now = new Date();
    const Current30 = new Date();
    Current30.setDate(Current30.getDate() + 30);
    cy.FillLogLov(ARInvoiceSelectors.BillTo, aRInvoiceDetails.BillTo, true)
    cy.FillLogLov(ARInvoiceSelectors.Currency, aRInvoiceDetails.InvoiceCurrency, true)
    //cy.FillDate(ARInvoiceSelectors.InvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillDate(ARInvoiceSelectors.InvoiceDate,now.toLocaleDateString('fr-FR') )
    cy.FillLogLov(ARInvoiceSelectors.PaymentTerm, aRInvoiceDetails.PaymentTerm, true)
    cy.FillDate(ARInvoiceSelectors.DueDate,Current30.toLocaleDateString('fr-FR') )
    cy.FillLogTextBox(ARInvoiceSelectors.VatNumber, aRInvoiceDetails.VATNo)
    cy.FillLogLov(ARInvoiceSelectors.Branch, aRInvoiceDetails.Branch, true)
    
}

export function CreateARInvoice() {
    cy.DefineRequestWait(RestAPI.GET, URLs.InvoicesGetSingle, RequestAliases.ARInvoiceView)
    
    cy.Click(ARInvoiceSelectors.CreateARInvoiceButton, null)
}

export function AssertCreateARInvoice() {
    debugger

    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceView, 200)
}

export function NavigatesARInvoiceLineWizerd() {
    cy.get(ARInvoiceSelectors.AddInvoiceLine).click()
}

export function FillInvoiceLineDetails(invoiceLineDetails: InvoiceLineDetails) {
    debugger

    cy.FillLogLov(ARInvoiceSelectors.InvoiceLineChargesType, invoiceLineDetails.ChargesType, true);
    cy.FillLogTextBox(ARInvoiceSelectors.InvoiceLineLocalDescription, invoiceLineDetails.LocalDescription)
    cy.FillLogLov(ARInvoiceSelectors.InvoiceLineVatType, invoiceLineDetails.VatType, true);
    cy.FillLogLov(ARInvoiceSelectors.InvoiceLineForiegnCurrency, invoiceLineDetails.ForiegnCurrency, true)
    cy.FillLogTextBox(ARInvoiceSelectors.Quantity, invoiceLineDetails.Quantity)
    cy.FillLogTextBox(ARInvoiceSelectors.UnitPrice, invoiceLineDetails.UnitPrice)
}

export function AddARInvoiceLine() {
    

    cy.Click(BaseSelectors.RedButton,"אישור")
}

export function AssertAddARInvoiceLine() {
    debugger

   // cy.get(BaseSelectors.GridViewCell).eq(1).contains("Air Freight")
   cy.get(ARInvoiceSelectors.AddInvoiceLine).should('have.length', 1)
}

export function ApproveARInvoice() {
   

    cy.DefineRequestWait(RestAPI.POST, URLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ARInvoiceSelectors.ARInvoiceApproveButton, null)
}

export function AssertApproveARInvoice() {
    

    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200)
}