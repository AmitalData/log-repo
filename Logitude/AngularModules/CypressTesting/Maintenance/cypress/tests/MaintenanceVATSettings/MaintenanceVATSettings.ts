import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { VATSettingsDetails } from "../../models/VATSettingsDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region Open VATSettings
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemVATSettings);
});

Given("a VAT settings with the following details", (dataTable) => {
    let vatSettingsDetails = Assists.CreateInstance<VATSettingsDetails>(dataTable, true);
    MaintenanceActions.FillVATSettingsDetails(vatSettingsDetails)
});

When("update VAT Setting", () => {
    MaintenanceActions.UpdateVATSettings()
});

Then("the VATSetting should update successfully", () => {
    MaintenanceActions.AssertUpdateVATSettings()
});

Given("VAT settings with the following details", (dataTable) => {
    let vatSettingsDetails = Assists.CreateInstance<VATSettingsDetails>(dataTable, true);
    MaintenanceActions.FillVATSettingsDetailsNoFormat(vatSettingsDetails)
});