import { APPaymentSelectors } from "../selectors/APPaymentSelectors";
import { APPaymentDetails } from "../models/APPaymentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { AccountingURLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BaseURLs } from "../../../Base/cypress/constants/URLs";

export function NavigatesAPPaymentWorkspace() {
    cy.Click(APPaymentSelectors.PayableAccountingTab, null)
    cy.Click(APPaymentSelectors.NewAPPaymentHyperLink, null)
}

export function FillAPPayment(aPPaymentDetails: APPaymentDetails) {
    cy.FillLogLov(APPaymentSelectors.APPaymentVendor, aPPaymentDetails.Vendor, false)
    cy.FillLogLov(APPaymentSelectors.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(APPaymentSelectors.APPaymentAmount, aPPaymentDetails.PaymentAmount)
    cy.FillLogLov(APPaymentSelectors.APPaymentCurrency, aPPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(APPaymentSelectors.RegisterDate, aPPaymentDetails.RegisterDate)
}

export function SaveAPPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.APPayments, RequestAliases.PostAPPayments)
    cy.Click(APPaymentSelectors.APPaymentSaveButton, null);
}

export function AssertSaveAPPayment() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.PostAPPayments, 200)
}

export function ApproveAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APPayments, RequestAliases.PutAPPayments)
    cy.Click(APPaymentSelectors.APPaymentApproveButton, null)
}

export function UpdateAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APPayments, RequestAliases.PutAPPayments)
    cy.Click(APPaymentSelectors.APPaymentSaveButton, null);
}

export function AssertUpdateAPPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutAPPayments, 200)
}

export function DefineGetByFilterRequest() {
    cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetByFilters, RequestAliases.GetByFilter);
}

export function AssertGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200);
}

export function ConnectAPPaymentToInvoice(invoiceNumber) {
    DefineGetByFilterRequest()
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber)
    AssertGetByFilters()
    cy.wait(3000)
    cy.get(BaseSelectors.CheckBoxLine).eq(0).click()
}

export function DisConnectAPPaymentFromInvoice(invoiceNumber) {
    cy.Click(BaseSelectors.HyperlinkButtonControl, invoiceNumber, true)
    cy.Click(BaseSelectors.DefaultMenuItem, "Payments")
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(APPaymentSelectors.ShipmentPaymentDisconnectButton, null)
}