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
    debugger
    ChequeDepositActions.NavigatesChequeDepositWizerd()
    let ARPaymentDetails = Assists.CreateInstance<ChequeDepositDetails>(dataTable, true);
    ChequeDepositActions.FillChequeDepositDetails(ARPaymentDetails)
});

When("create AR Payment", () => {
    ChequeDepositActions.CreateChequeDeposit();
});

Given("a cheque with the following details", (dataTable) => {
    let ARPaymentDetails = Assists.CreateInstance<ChequeDepositDetails>(dataTable, true);
    ChequeDepositActions.FillChequeDepositAmount(ARPaymentDetails)
});

When("Approve the AR Payment", () => {
    ChequeDepositActions.ApproveChequeDeposit()
});

Then("the AR Payment should approve successfully", () => {
    ChequeDepositActions.AssertApproveChequeDeposit()
});
//#endregion

