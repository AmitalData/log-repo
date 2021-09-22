import { ARPaymentSelectors } from "../selectors/ARPaymentSelectors";
import { ARPaymentDetails } from "cypress/models/ARPaymentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesARPaymentWorkspace() {
    cy.Click(ARPaymentSelectors.NewARPaymentButton, null)
}

export function FillARPayment(ARPaymentDetails: ARPaymentDetails) {
    cy.FillLogLov(ARPaymentSelectors.Partner, ARPaymentDetails.Partner, false)
    cy.FillLogLov(ARPaymentSelectors.PaymentMethod, ARPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ARPaymentSelectors.PaymentAmount, ARPaymentDetails.PaymentAmount)
    cy.FillLogLov(ARPaymentSelectors.PaymentBranch, ARPaymentDetails.PaymentBranch, true)
    cy.FillLogLov(ARPaymentSelectors.PaymentCurrency, ARPaymentDetails.PaymentCurrency, true)
}

export function CreateARPayment() {
    cy.DefineRequestWait(RestAPI.GET, URLs.InvoicesGetSingle, RequestAliases.ARPaymentView)
    cy.Click(ARPaymentSelectors.OkAddARPayment, null);
}

export function AssertCreateARPayment() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.ARPaymentView, 200)
}

export function ARproveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, URLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(ARPaymentSelectors.ARPaymentBApprove, null)
}

export function AssertARproveARPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}