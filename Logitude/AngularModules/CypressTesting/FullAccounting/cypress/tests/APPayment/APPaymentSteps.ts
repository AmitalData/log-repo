import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as APPaymentActions from '../../actions/APPaymentActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { APPaymentDetails } from '../../models/APPaymentDetails';
import * as Actions from '../../actions/Actions';

//#region Create new AP Payment
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
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
//#endregion

//#region Approve the AP Payment
When("approve the AP Payment", () => {
    APPaymentActions.ApproveAPPayment()
});

Then("the AP Payment should approve successfully", () => {
    APPaymentActions.AssertApproveAPPayment()
});
//#endregion

//#region Void AP Payment
When("void AP Payment", () => {
    APPaymentActions.VoidAPPayment()
});

Then("the AP Payment should void successfully", () => {
    APPaymentActions.AssertVoidAPPayment()
});
//#endregion