import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ARPaymentActions from '../../actions/ARPaymentActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ARPaymentDetails } from '../../models/ARPaymentDetails';
import * as Actions from '../../actions/Actions';

//#region Create new AR Payment
Given("the user logged in and navigates to Accounting workspace", () => {
    cy.Login(true);
    Actions.NavigatesToAccountingMenu()
});

Given("an AR Payment with the following details", (dataTable) => {
    ARPaymentActions.NavigatesARPaymentWorkspace()
    let ARPaymentDetails = Assists.CreateInstance<ARPaymentDetails>(dataTable, true);
    ARPaymentActions.FillARPaymentWithSAT(ARPaymentDetails)
});

When("create AR Payment", () => {
    ARPaymentActions.CreateARPayment()
});

Then("the AR Payment should get successfully", () => {
    ARPaymentActions.AssertCreateARPayment()
});
//#endregion

//#region Connect Payment with Invoice 
Given("search for the specific invoice ", () => {
    Actions.SearchInvoice();
});


When("choose this invoice ", () => {

});

Then("the payment should be ready for sending to SAT", () => {

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

//#region Send AR Payment To SAT
When("Send AR Payment to SAT", () => {
    
});

Then("the AR Payment should Transferred successfully", () => {
    
});


//#endregion