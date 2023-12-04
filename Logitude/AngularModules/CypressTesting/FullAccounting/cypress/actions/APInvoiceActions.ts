import { APInvoiceSelectors } from "../selectors/APInvoiceSelectors";
import * as gr from '../../../Base/cypress/Actions/GenerateRandoms';
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails";
import { InvoiceLineDetails } from "cypress/models/InvoiceLineDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesAPInvoiceWizerd() {
    cy.Click(BaseSelectors.FullAccountingVendorsTab, null)
    cy.Click(APInvoiceSelectors.APInvoiceButton, null)
}

export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999);
    const now = new Date();
    cy.FillLogLov(APInvoiceSelectors.APInvoiceVendor, aPInvoiceDetails.Vendor, false);
    cy.FillLogTextBox(APInvoiceSelectors.APInvoiceInvoiceNumber, generatedInvoiceNumber)
    cy.wait(1000)
    cy.FillLogTextBox(APInvoiceSelectors.APInvoiceAmountInInvoice, aPInvoiceDetails.InvoiceAmount);
    //cy.FillDate(APInvoiceSelectors.APInvoiceInvoiceDate, aPInvoiceDetails.InvoiceDate)
    cy.FillDate(APInvoiceSelectors.APInvoiceInvoiceDate,now.toLocaleDateString('fr-FR') )
    //cy.FillDate(APInvoiceSelectors.APInvoiceAccountingDate, aPInvoiceDetails.AccountingDate)
    cy.FillDate(APInvoiceSelectors.APInvoiceAccountingDate,now.toLocaleDateString('fr-FR') )
    
}

export function CreateAPInvoice() {
    cy.DefineRequestWait(RestAPI.GET, URLs.InvoicesGetSingle, RequestAliases.APInvoiceView)
    cy.Click(APInvoiceSelectors.CreateAPInvoiceButton, null);
}

export function AssertCreateAPInvoice() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoiceView, 200)
}

export function NavigatesAPInvoiceLineWizerd() {
    cy.get(APInvoiceSelectors.AddInvoiceLine).click()
}

export function FillInvoiceLineDetails(invoiceLineDetails: InvoiceLineDetails) {
    cy.FillLogLov(APInvoiceSelectors.APInvoiceLineChargesType, invoiceLineDetails.ChargesType, true);
    cy.FillLogTextBox(APInvoiceSelectors.APInvoiceLineLocalDescription, invoiceLineDetails.LocalDescription)
    cy.FillLogLov(APInvoiceSelectors.APInvoiceLineVatType, invoiceLineDetails.VatType, true);
    cy.FillLogTextBox(APInvoiceSelectors.APInvoiceLineAmount, invoiceLineDetails.Amount)
}

export function AddAPInvoiceLine() {
    cy.Click(APInvoiceSelectors.AddAPInvoiceLineButton, null);
}

export function AssertAddAPInvoiceLine() {
    cy.get(APInvoiceSelectors.APInvoiceLineRows).should('have.length', 1)
}

export function ApproveAPInvoice() {
    cy.DefineRequestWait(RestAPI.POST, URLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(APInvoiceSelectors.APInvoiceApproveButton, null)
}

export function AssertApproveAPInvoice() {
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200)
}