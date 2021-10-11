import { APPaymentSelectors } from "../selectors/APPaymentSelectors";
import { APPaymentDetails } from "cypress/models/APPaymentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { AccountingURLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

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

export function AssertApproveAPPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutAPPayments, 200)
}