import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { AccountingSettingsDetails } from "../../models/AccountingSettingsDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let accountingSettingsDetails: AccountingSettingsDetails;

//#region Disable/enable void invoice settings
Given("the user logged in and navigate to {string} in maintenance menu", (AccountingSettings) => {
    cy.Login()
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(AccountingSettings, MaintenanceSelectors.AccountingSettingsMaintenanceItem)
});
Given("the user update Receivables Accounting Settings as following", (dataTable) => {
    accountingSettingsDetails = Assists.CreateInstance<AccountingSettingsDetails>(dataTable, true);
    MaintenanceActions.ChangeAccountingSettings(accountingSettingsDetails)
});

When("the user save the changes", () => {
    MaintenanceActions.UpdateInvoiceSettings();
});

Then("the new settings is saved", () => {
    MaintenanceActions.AssertUpdateAccountingSettings()
});
Given("the user update Payables Accounting Settings as following", (dataTable) => {
    accountingSettingsDetails = Assists.CreateInstance<AccountingSettingsDetails>(dataTable, true);
    MaintenanceActions.ChangeAccountingSettings(accountingSettingsDetails)
});

Given("the user navigates to {string} in maintenance menu", (AccountingSettings) => {
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(AccountingSettings, MaintenanceSelectors.AccountingSettingsMaintenanceItem)
});
When("the user updates the others as following", (dataTable) => {
    accountingSettingsDetails = Assists.CreateInstance<AccountingSettingsDetails>(dataTable, true);
    MaintenanceActions.FillAccountingSettingsVATNumber(accountingSettingsDetails.VATNumber)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});

