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

export function FillARPayment(aRPaymentDetails: ARPaymentDetails) {
    cy.FillLogLov(ARPaymentSelectors.Partner, aRPaymentDetails.Partner, true)
    cy.FillLogLov(ARPaymentSelectors.PaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ARPaymentSelectors.PaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.FillLogLov(ARPaymentSelectors.PaymentBranch, aRPaymentDetails.PaymentBranch, true)
    cy.FillLogLov(ARPaymentSelectors.PaymentCurrency, aRPaymentDetails.PaymentCurrency, true)
}

export function FillChequeDetails(aRPaymentDetails: ARPaymentDetails) {
    PressOnTitleAfterFillLogTextBox(ARPaymentSelectors.ChequeValueDate, aRPaymentDetails.ChequeValueDate)
    PressOnTitleAfterFillLogTextBox(ARPaymentSelectors.ChequeRef, aRPaymentDetails.ChequeRef)
    PressOnTitleAfterFillLogTextBox(ARPaymentSelectors.ChequeBank, aRPaymentDetails.ChequeBank)
    PressOnTitleAfterFillLogTextBox(ARPaymentSelectors.ChequeBankBranch, aRPaymentDetails.ChequeBankBranch)
    PressOnTitleAfterFillLogTextBox(ARPaymentSelectors.ChequeAccount, aRPaymentDetails.ChequeAccount)
}

function PressOnTitleAfterFillLogTextBox(selector, value) {
    cy.FillLogTextBox(selector, value)
    cy.Click(ARPaymentSelectors.ShortTitleControl, null)
}

export function CreateARPayment() {
    cy.DefineRequestWait(RestAPI.GET, URLs.InvoicesGetSingle, RequestAliases.ARPaymentView)
    cy.Click(ARPaymentSelectors.OkAddARPayment, null);
}

export function AssertCreateARPayment() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.ARPaymentView, 200)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, URLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(ARPaymentSelectors.ARPaymentBApprove, null)
}

export function AssertApproveARPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}

export function VoidARPayment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(BaseSelectors.MenuButtons, null)
    cy.Click(ARPaymentSelectors.VoidButton, null)
    cy.FillLogTextBox(ARPaymentSelectors.CancelationNotes, "Cancel")
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AssertVoidARPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}