import { ARPaymentSelectors } from "../selectors/ARPaymentSelectors";
import { ARPaymentDetails } from "cypress/models/ARPaymentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { AccountingURLs } from '../constants/URLs';

export function NavigatesARPaymentWorkspace() {
    cy.get(BaseSelectors.QueryLink).contains("New Payment").click()
}

export function FillARPayment(aRPaymentDetails: ARPaymentDetails) {
    cy.FillLogLov(ARPaymentSelectors.Partner, aRPaymentDetails.Partner, false)
    cy.FillLogLov(ARPaymentSelectors.PaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ARPaymentSelectors.PaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.FillLogLov(ARPaymentSelectors.PaymentCurrency, aRPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(ARPaymentSelectors.RegisterDate, aRPaymentDetails.RegisterDate)
}

export function CreateARPayment() {
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.InvoicesGetSingle, RequestAliases.ARPaymentView)
    cy.Click(ARPaymentSelectors.OkAddARPayment, null);
}

export function AssertCreateARPayment() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.ARPaymentView, 200)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(ARPaymentSelectors.ARPaymentBApprove, null)
}

export function AssertApproveARPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}