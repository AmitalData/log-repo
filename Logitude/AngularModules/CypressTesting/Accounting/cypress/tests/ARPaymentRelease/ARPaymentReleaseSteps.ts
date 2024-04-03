import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ARPaymentActions from '../../actions/ARPaymentActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ARPaymentDetails } from '../../models/ARPaymentDetails';
import * as Actions from '../../actions/Actions';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { AccountingSelectors } from '../../selectors/Selectors';

//#region Create new AR Payment
Given("the user logged in and navigates to Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesToAccountingMenu()
});

Given("an AR Payment with the following details", (dataTable) => {
    ARPaymentActions.NavigatesARPaymentWorkspace()
    let ARPaymentDetails = Assists.CreateInstance<ARPaymentDetails>(dataTable, true);
    ARPaymentActions.FillARPayment(ARPaymentDetails)
});

When("create AR Payment", () => {
    ARPaymentActions.CreateARPayment()
});

Then("the AR Payment should get successfully", () => {
    ARPaymentActions.AssertCreateARPayment()
});

Then("the status value should be {string}", (statusValue) => {
    BaseAssertion.AssertElementContain(AccountingSelectors.PaymentHeaderStatus, statusValue)
});

Then("the details fields should {string}", (condition) => {
    ARPaymentActions.AssertARPaymentDetailsFieldsDisabled(condition)
});
//#endregion

//#region Approve the AR Invoice
When("Approve the AR Payment", () => {
    ARPaymentActions.ApproveARPayment()
});

Then("the AR Payment should approve successfully", () => {
    ARPaymentActions.AssertApproveARPayment()
});
//#endregion