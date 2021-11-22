import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ARPaymentActions from '../../actions/ARPaymentActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ARPaymentDetails } from '../../models/ARPaymentDetails';
import * as Actions from '../../actions/Actions';
import { ARPaymentSelectors } from '../../selectors/ARPaymentSelectors';

//#region Create new AR Payment
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
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
//#endregion

//#region Edit print notes and Approve the AR Payment
Given("add {string} as print notes", (printNotes) => {
    cy.FillLogTextBox(ARPaymentSelectors.PrintNotes, printNotes)
});

When("Approve the AR Payment", () => {
    ARPaymentActions.ApproveARPayment()
});

Then("the AR Payment should approve successfully", () => {
    ARPaymentActions.AssertApproveARPayment()
});
//#endregion

//#region Void AR Payment
When("void AR Payment", () => {
    ARPaymentActions.VoidARPayment()
});

Then("the AR Payment should void successfully", () => {
    ARPaymentActions.AssertVoidARPayment()
});
//#endregion