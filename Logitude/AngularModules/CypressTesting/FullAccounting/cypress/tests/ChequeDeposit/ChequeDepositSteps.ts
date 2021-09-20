import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as ARPaymentActions from '../../actions/ARPaymentActions';
import * as ChequeDepositActions from '../../actions/ChequeDepositActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ARPaymentDetails } from '../../models/ARPaymentDetails';
import { ChequeDepositDetails } from '../../models/ChequeDepositDetails';
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

//#region Approve the AR Invoice
Given("a cheque with the following details", (dataTable) => {
    let ARPaymentDetails = Assists.CreateInstance<ARPaymentDetails>(dataTable, true);
    ARPaymentActions.FillChequeDetails(ARPaymentDetails)
});

When("Approve the AR Payment", () => {
    ARPaymentActions.ApproveARPayment()
});

Then("the AR Payment should approve successfully", () => {
    ARPaymentActions.AssertApproveARPayment()
});
//#endregion

//#region Create new Cheque Deposit
Given("the user navigates to cheque deposit wizerd", () => {
    ChequeDepositActions.NavigatesChequeDepositWizerd()
});

Given("a cheque deposit with the following details", (dataTable) => {
    let chequeDepositDetails = Assists.CreateInstance<ChequeDepositDetails>(dataTable, true);
    ChequeDepositActions.FillChequeDepositDetails(chequeDepositDetails)
});

When("create cheque deposit", () => {
    ChequeDepositActions.CreateChequeDeposit()
});

Then("the cheque deposit should get successfully", () => {
    ChequeDepositActions.AssertCreateChequeDeposit()
});
//#endregion

//#region Approve the Cheque Deposit
Given("select the all cheques in the cheque deposit", () => {
    ChequeDepositActions.SelectAllCheques()
});

When("Approve the cheque deposit", () => {
    ChequeDepositActions.ApproveChequeDeposit()
});

Then("the cheque deposit should approve successfully", () => {
    ChequeDepositActions.AssertApproveChequeDeposit()
});
//#endregion