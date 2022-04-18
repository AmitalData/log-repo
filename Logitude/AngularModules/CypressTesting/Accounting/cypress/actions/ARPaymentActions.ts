import { ARPaymentSelectors } from "../selectors/ARPaymentSelectors";
import { ARPaymentDetails } from "cypress/models/ARPaymentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { AccountingURLs } from '../constants/URLs';
import { AccountingSelectors } from "../selectors/Selectors";

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
export function FillARPaymentWithSAT(aRPaymentDetails: ARPaymentDetails) {
    cy.FillLogLov(ARPaymentSelectors.Partner, aRPaymentDetails.Partner, true)
    cy.FillLogLov(ARPaymentSelectors.PaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(ARPaymentSelectors.PaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.FillLogLov(ARPaymentSelectors.PaymentCurrency, aRPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(ARPaymentSelectors.RegisterDate, aRPaymentDetails.RegisterDate)
    cy.FillLogLov(ARPaymentSelectors.MetodoPago, aRPaymentDetails.MetodoPago, true)
    cy.FillLogLov(ARPaymentSelectors.FormaPago, aRPaymentDetails.FormaPago, true)
    cy.FillLogLov(ARPaymentSelectors.Branch,aRPaymentDetails.Branch,true)

}

export function CreateARPayment() {
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.InvoicesGetSingle, RequestAliases.ARPaymentView)
    cy.Click(ARPaymentSelectors.OkAddARPayment, null);
}

export function AssertCreateARPayment() {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.ARPaymentView, 200)
}

export function AssertARPaymentDetailsFieldsDisabled(condition) {
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.Partner, condition)
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.ARPaymentBillToAddress, condition)
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.PaymentMethod, condition)
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.PaymentAmount, condition)
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.PaymentCurrency, condition)
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.dateARPaymentRegisterDate, condition)
    BaseAssertion.AssertElementDisabled(ARPaymentSelectors.ARPaymentBranch, condition)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(ARPaymentSelectors.ARPaymentBApprove, null)
}

export function AssertApproveARPayment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
}

export function NavigatesARPaymentWorkspaceSAT() {
    cy.get(ARPaymentSelectors.BackButton).click()
    cy.get(ARPaymentSelectors.EditBackbutton).click()
    cy.get(ARPaymentSelectors.GeneralMHAccounting).click()
    cy.get(ARPaymentSelectors.RecivableAccounting).click()
    cy.get(BaseSelectors.QueryLink).contains("New Payment").click()
}

export function  ChooseInvoiceNumber(){
cy.get(AccountingSelectors.InvoiceCheckBox).find('input').click({force:true})

}

export function SendARPaymentToSAT(){
cy.get(ARPaymentSelectors.SendToSAT).click()
cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSend);
}