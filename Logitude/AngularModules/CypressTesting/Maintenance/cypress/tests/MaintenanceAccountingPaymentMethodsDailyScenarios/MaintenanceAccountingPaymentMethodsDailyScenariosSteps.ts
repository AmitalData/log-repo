import * as Actions from "../../actions/Actions";
import * as GeneralActions from "../../actions/BaseActions";
import * as AccountingPaymentMethodActions from "../../actions/AccountingPaymentMethodActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { AccountingPaymentMethodSelectors } from "../../selectors/AccountingPaymentMethodSelectors";
import { AccountingPaymentMethodDetails } from "cypress/models/AccountingPaymentMethodDetails";
import { Urls } from "../../constants/Urls";

//#region Add accounting payment code with lenght more than 2
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, AccountingPaymentMethodSelectors.MaintenanceItem)
});

When("add {string} as accounting payment code", (accountingPaymentCode) => {
    Actions.OpenNewWizard("AccountingPaymentMethod");
    AccountingPaymentMethodActions.FillAccountingPaymentMethodCode(accountingPaymentCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Create new accounting payment
Given("an accounting payment with the following details", (dataTable) => {
    let accountingPaymentMethodDetails = Assists.CreateInstance<AccountingPaymentMethodDetails>(dataTable, true);
    AccountingPaymentMethodActions.FillAccountingPaymentMethodDetails(accountingPaymentMethodDetails, 2);
});

When("create accounting payment", () => {
    GeneralActions.MockCreate(Urls.AccountingPaymentMethods)
});

Then("the accounting payment should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the accounting payment
When("search for {string} accounting payment", (searchFieldValue) => {
    GeneralActions.Search(searchFieldValue)
});

Then("the {string} accounting payment should appear successfully", (searchFieldValue) => {
    GeneralActions.AssertSearch(searchFieldValue)
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

Given("fill the following Accounting tab details", () => {
    cy.Navigate(AccountingPaymentMethodSelectors.AccountingTab);
    AccountingPaymentMethodActions.FillAccountingPaymentMethodAccountingTab()
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