import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as MaintenanceActions from "../../actions/Actions";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import { CustomerSettingsDetails } from "../../models/CustomerSettingsDetails";
import { CustomerDetails } from "../../../../Common/cypress/models/CustomerDetails";

//#region variable
let customerSettingsDetails: CustomerSettingsDetails;
let customerDetails: CustomerDetails;
//#endregion
//#region Enable phone, fax and address 1 to be mandatory
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.CustomerSettingsMaintenanceItem)
});
Given("a customer settings with the following details", (dataTable) => {
    customerSettingsDetails = Assists.CreateInstance<CustomerSettingsDetails>(dataTable, true);
    MaintenanceActions.FillCustomerSettingsDetails(customerSettingsDetails)
});
When("update customer settings", () => {
    MaintenanceActions.UpdateCustomerSettings();
});
Then("the settings should update successfully", () => {
    MaintenanceActions.AssertPutCustomerSettings();
});
//#endregion
//#region Add potential customer
Given("the user navigates to customer workspace in CRM menu", () => {
    MaintenanceActions.NavigatesToCustomerCRMWorkspace();
});
Given("a potential customer with the following details", (dataTable) => {
    customerDetails = Assists.CreateInstance<CustomerDetails>(dataTable, true);
    MaintenanceActions.OpenNewPotentialCustomerWizard();
    MaintenanceActions.FillPotentialCustomerDetails(customerDetails);
});
When("add potential customer", () => {
    MaintenanceActions.AddPotentialCustomer();
});
Then("the customer should add successfully", () => {
    MaintenanceActions.AssertAddPotentialCustomer();
});
Then("the customer status should be {string}", (CustomerStatus) => {
    MaintenanceActions.SearchCustomer();
    MaintenanceActions.AssertCustomerStatus(CustomerStatus)
});
//#endregion
//#region Activate customer
Given("an active customer with the following details", (dataTable) => {
    customerDetails = Assists.CreateInstance<CustomerDetails>(dataTable, true);
    cy.Click(MaintenanceSelectors.ActivateCustomerButton, null);
    MaintenanceActions.FillCustomerActivationWindow(customerDetails);
});
When("activate customer", () => {
    MaintenanceActions.ActivateCustomer();
});
Then("the customer should activate successfully", () => {
    MaintenanceActions.AssertActivateCustomer();
});
Then("the customer status should change to {string}", (CustomerStatus) => {
    MaintenanceActions.AssertCustomerStatus(CustomerStatus)
});
//#endregion