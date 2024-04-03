import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as APPaymentActions from '../../actions/APPaymentActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { APPaymentDetails } from '../../models/APPaymentDetails';
import * as Actions from '../../actions/Actions';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { AccountingSelectors } from '../../selectors/Selectors';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';

//#region Create new AP Payment
Given("the user logged in and navigates to Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesToAccountingMenu()
});

Given("an AP Payment with the following details", (dataTable) => {
    APPaymentActions.NavigatesAPPaymentWorkspace()
    let aPPaymentDetails = Assists.CreateInstance<APPaymentDetails>(dataTable, true);
    APPaymentActions.FillAPPayment(aPPaymentDetails)
});

When("save the AP Payment", () => {
    APPaymentActions.SaveAPPayment()
});

Then("the AP Payment should save successfully", () => {
    APPaymentActions.AssertSaveAPPayment()
});

Then("the status value should be {string}", (statusValue) => {
    BaseAssertion.AssertElementContain(AccountingSelectors.PaymentHeaderStatus, statusValue)
});

Then("the details fields should {string}", (condition) => {
    APPaymentActions.AssertAPPaymentDetailsFieldsDisabled(condition)
});
//#endregion

//#region Approve the AP Invoice
When("approve the AP Payment", () => {
    APPaymentActions.ApproveAPPayment()
});

Then("the AP Payment should approve successfully", () => {
    APPaymentActions.AssertUpdateAPPayment()
});
//#endregion