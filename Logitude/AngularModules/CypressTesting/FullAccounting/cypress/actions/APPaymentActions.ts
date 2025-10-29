import { APPaymentSelectors } from "../selectors/APPaymentSelectors";
import { APPaymentDetails } from "cypress/models/APPaymentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesAPPaymentWorkspace() {
    cy.Click(BaseSelectors.FullAccountingVendorsTab, null)
    cy.Click(APPaymentSelectors.NewAPPaymentButton, null)
}

export function FillAPPayment(aPPaymentDetails: APPaymentDetails) {
    cy.FillLogLov(APPaymentSelectors.APPaymentVendor, aPPaymentDetails.Vendor, true)
    cy.FillLogLov(APPaymentSelectors.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(APPaymentSelectors.APPaymentAmount, aPPaymentDetails.PaymentAmount)
    //cy.FillLogTextBox(APPaymentSelectors.APPaymentRegisterDate, (new Date()).toLocaleDateString('en-GB'))
    //cy.FillLogLov(APPaymentSelectors.APPaymentBranch,aPPaymentDetails.BranchId, true)
}

export function SaveAPPayment() {
    cy.DefineRequestWait(RestAPI.POST, URLs.APPayments, RequestAliases.PostAPPayments)
    cy.Click(APPaymentSelectors.APPaymentSaveButton, null);
}

export function AssertSaveAPPayment() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.PostAPPayments, 200)
}

export function ApproveAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.APPayments, RequestAliases.PutAPPayments)
    cy.Click(APPaymentSelectors.APPaymentApproveButton, null)
}

export function AssertApproveAPPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutAPPayments, 200)
    
}

export function VoidAPPayment() {

    cy.DefineRequestWait(RestAPI.PUT, URLs.APPayments, RequestAliases.APPayments)
    cy.Click(BaseSelectors.MenuButtons, null, true)
    cy.Click(APPaymentSelectors.VoidButton, null, true)
    cy.FillLogTextBox(APPaymentSelectors.CancelationNotes, BaseSelectors.ContainsCancel)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null, true)
    cy.Click(APPaymentSelectors.ConfirmWindow, null, true)
    
    
}

export function AssertVoidAPPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.APPayments, 200)
}

