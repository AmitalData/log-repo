import * as Actions from "../../actions/Actions";
import * as AccountingPaymentMethodActions from "../../actions/AccountingPaymentMethodActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { AccountingPaymentMethodSelectors } from "../../selectors/AccountingPaymentMethodSelectors";
import { AccountingPaymentMethodDetails } from "cypress/models/AccountingPaymentMethodDetails";

//#region Create new accounting payment
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, AccountingPaymentMethodSelectors.MaintenanceItem)
});

Given("an accounting payment with the following details", (dataTable) => {
    let accountingPaymentMethodDetails = Assists.CreateInstance<AccountingPaymentMethodDetails>(dataTable, true);
    Actions.OpenNewWizard("AccountingPaymentMethod");
    AccountingPaymentMethodActions.FillAccountingPaymentMethodDetails(accountingPaymentMethodDetails, 2);
});

When("create accounting payment", () => {
    AccountingPaymentMethodActions.CreateAccountingPaymentMethod();
});

Then("the accounting payment should create successfully", () => {
    AccountingPaymentMethodActions.AssertCreateAccountingPaymentMethod();
});
//#endregion

//#region Search for the accounting payment
When("search accounting payment", () => {
    AccountingPaymentMethodActions.SearchAccountingPaymentMethod()
});

Then("the accounting payment should appear successfully", () => {
    AccountingPaymentMethodActions.AssertSearchAccountingPaymentMethod();
});
//#endregion

//#region Open the accounting payment
When("open accounting payment", () => {
    AccountingPaymentMethodActions.OpenAccountingPaymentMethod();
});

Then("the accounting payment should open successfully", () => {
    AccountingPaymentMethodActions.AssertOpenAccountingPaymentMethod();
});
//#endregion

//#region Edit the accounting payment
Given("the user edit the following accounting payment details", (dataTable) => {
    let accountingPaymentMethodDetails = Assists.CreateInstance<AccountingPaymentMethodDetails>(dataTable, true);
    AccountingPaymentMethodActions.EditAccountingPaymentMethodGeneralTab(accountingPaymentMethodDetails)
});

Given("fill the following Accounting tab details", (dataTable) => {
    let accountingPaymentMethodDetails = Assists.CreateInstance<AccountingPaymentMethodDetails>(dataTable, true);
    cy.Navigate(AccountingPaymentMethodSelectors.AccountingTab);
    AccountingPaymentMethodActions.FillAccountingPaymentMethodAccountingTab(accountingPaymentMethodDetails)
});

When("save accounting payment", () => {
    AccountingPaymentMethodActions.UpdateAccountingPaymentMethod()
});

Then("the accounting payment should update successfully", () => {
    AccountingPaymentMethodActions.AssertUpdateAccountingPaymentMethod()
});
//#endregion

//#region Save and close the accounting payment
When("save and close accounting payment", () => {
    AccountingPaymentMethodActions.CloseSaveAccountingPaymentMethod();
});

Then("the accounting payment should close successfully", () => {
    AccountingPaymentMethodActions.AssertCloseSaveAccountingPaymentMethod();
});
 //#endregion