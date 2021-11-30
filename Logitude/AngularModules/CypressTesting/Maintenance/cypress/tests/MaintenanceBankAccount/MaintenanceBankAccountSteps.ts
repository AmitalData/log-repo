import * as Actions from "../../actions/Actions";
import * as BankAccountActions from "../../actions/BankAccountActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { BankAccountDetails } from '../../models/BankAccountDetails'
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { BankAccountSelectors } from "../../selectors/BankAccountSelectors";
import { Constants } from '../../../cypress/constants/Constants'
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

let bankAccountDetails: BankAccountDetails

//#region Assert create bank account
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemBankAccount)
    Actions.OpenNewWizard(Constants.BankAccount);
});

When("create bank account", () => {
    BankAccountActions.CreateBankAccount();
});

Then("a validation error message with {string} should appear", (validationMessage) => {
    BaseAssertion.AssertElementContain(BaseSelectors.ValidationSummary, validationMessage)
});
//#endregion

//#region Create new bank account
Given("a bank account with the following details", (dataTable) => {
    bankAccountDetails = Assists.CreateInstance<BankAccountDetails>(dataTable, true);
    BankAccountActions.FillBankAccountDetails(bankAccountDetails)
});

Then("the bank account should create successfully", () => {
    BankAccountActions.AssertCreateBankAccount()
});
//#endregion

//#region Search for the bank account by code
When("search bank account", () => {
    BankAccountActions.SearchBankAccount()
});

Then("the bank account should appear successfully", () => {
    BankAccountActions.AssertSearchBankAccount()
});
//#endregion

//#region Open the bank account
When("open bank account", () => {
    BankAccountActions.OpenBankAccount();
});

Then("the bank account should open successfully", () => {
    BankAccountActions.AssertOpenBankAccount();
});
//#endregion

//#region Edit the bank account
Given("the user fill {string} as a local name value", (localName) => {
    BankAccountActions.FillBankAccountLocalName(localName)
});

Given("the user activate bank account", () => {
    Actions.ChangeInactiveCheckBoxValue(BankAccountSelectors.InActiveBankAccountCheckBox)
});

When("edit bank account", () => {
    BankAccountActions.EditBankAccount();
});

Then("the bank account should update successfully", () => {
    BankAccountActions.AssertEditBankAccount();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, BankAccountSelectors.BankAccountEventsTab);
});

When("save and close bank account", () => {
    BankAccountActions.CloseSaveBankAccount();
});

Then("the bank account should close successfully", () => {
    BankAccountActions.AssertCloseSaveBankAccount();
});
//#endregion