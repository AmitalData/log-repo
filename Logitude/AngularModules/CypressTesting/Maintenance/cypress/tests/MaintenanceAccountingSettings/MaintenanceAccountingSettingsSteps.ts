import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { AccountingSettingsDetails } from "../../models/AccountingSettingsDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as AccountingSettingsActions from "../../actions/AccountingSettingsActions";

let accountingSettingsDetails: AccountingSettingsDetails;

//#region Edit receivable and payables in accounting settings
Given("the user logged in", () => {
    cy.Login()
});

Given("navigate to {string} in maintenance menu", (AccountingSettings) => {
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(AccountingSettings, MaintenanceSelectors.MaintenanceItemAccountingSettings)
});

Given("the user update accounting settings as following", (dataTable) => {
    accountingSettingsDetails = Assists.CreateInstance<AccountingSettingsDetails>(dataTable, true);
    AccountingSettingsActions.ChangeAccountingSettings(accountingSettingsDetails)
});

When("the user save the changes", () => {
    AccountingSettingsActions.UpdateAccountingSettings();
});

Then("the new settings should saved successfully", () => {
    AccountingSettingsActions.AssertUpdateAccountingSettings()
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