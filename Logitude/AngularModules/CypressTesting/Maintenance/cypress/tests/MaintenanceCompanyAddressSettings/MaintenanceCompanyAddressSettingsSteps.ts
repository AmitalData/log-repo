import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";

//#region Edit the Company address settings
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.CompanyAddressSettingsMaintenanceItem)
});

Given("a {string} as Address2 and {string} as ZipCode", (address2, zipCode) => {
    Actions.FillCompanyAddressSettingsDetails(address2, zipCode)
});

When("update the Company Address Settings", () => {
    Actions.UpdateCompanyAddressSettings();
});

Then("the Company Address Settings should update successfully", () => {
    Actions.AssertUpdateCompanyAddressSettings()
});
//#endregion