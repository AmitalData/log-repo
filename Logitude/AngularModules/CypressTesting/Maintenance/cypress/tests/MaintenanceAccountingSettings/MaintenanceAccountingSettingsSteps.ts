import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { AccountingSettingsDetails } from "../../models/AccountingSettingsDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as AccountingSettingsActions  from "../../actions/AccountingSettingsActions";
import { Urls } from "../../constants/Urls";

let accountingSettingsDetails: AccountingSettingsDetails;

//#region Edit receivable and payables in accounting settings
Given("the user logged in and navigate to {string} in maintenance menu", (AccountingSettings) => {
    cy.Login()
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(AccountingSettings, MaintenanceSelectors.MaintenanceItemAccountingSettings)
});
And("the user update accounting settings as following", (dataTable) => {
    accountingSettingsDetails = Assists.CreateInstance<AccountingSettingsDetails>(dataTable, true);
    AccountingSettingsActions.ChangeAccountingSettings(accountingSettingsDetails)
});

When("the user save the changes", () => {
    AccountingSettingsActions.UpdateAccountingSettings();
});

Then("the new settings should saved successfully", () => {
    AccountingSettingsActions.AssertUpdateInvoiceSettings()
});
//#endregion
//#region Edit invoices and payment settings in accounting settings
And("the user update Payables Accounting Settings as following", (dataTable) => {
    accountingSettingsDetails = Assists.CreateInstance<AccountingSettingsDetails>(dataTable, true);
    AccountingSettingsActions.ChangeAccountingSettings(accountingSettingsDetails)
});

Given("the user navigates to {string} in maintenance menu", (AccountingSettings) => {
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(AccountingSettings, MaintenanceSelectors.MaintenanceItemAccountingSettings)
});
//#endregion
//#region Edit others in accounting settings
When("the user updates the others with {string} as VAT number", (Value) => {
    AccountingSettingsActions.FillAccountingSettingsVATNumber(Value)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion
