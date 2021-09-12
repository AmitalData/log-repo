import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ARPaymentActions from '../../actions/ARPaymentActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ARPaymentDetails } from '../../models/ARPaymentDetails';
import * as Actions from '../../actions/Actions';

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

//#region ARprove the AR Invoice
When("ARprove the AR Payment", () => {
    ARPaymentActions.ARproveARPayment()
});

Then("the AR Payment should ARprove successfully", () => {
    ARPaymentActions.AssertARproveARPayment()
});
//#endregion