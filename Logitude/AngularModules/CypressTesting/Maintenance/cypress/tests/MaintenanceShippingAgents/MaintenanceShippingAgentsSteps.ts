import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { ShippingAgentDetails } from "../../models/ShippingAgentDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import {ShippingAgentGeneralTabDetails} from "../../models/ShippingAgentGeneralTabDetails";
import { ShippingAgentBillingTabDetails } from "../../models/ShippingAgentBillingTabDetails";
let shippingAgentDetails: ShippingAgentDetails
let contactDetails: ContactDetails
let shippingAgentGeneralTabDetails:ShippingAgentGeneralTabDetails
let shippingAgentBillingTabDetails:ShippingAgentBillingTabDetails
//#region Create new shipping agent
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShippingAgents)
});

Given("a shipping agent with the following details", (dataTable) => {
    shippingAgentDetails = Assists.CreateInstance<ShippingAgentDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard("ShippingAgent");
    MaintenanceActions.FillShippingAgentsDetails(shippingAgentDetails)
});

Given("a shipping agent contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillShippingAgentsContactDetails(contactDetails)
});

When("create shipping agent", () => {
    MaintenanceActions.CreateShippingAgent()
});

Then("the shipping agent should create successfully", () => {
    MaintenanceActions.AssertCreateShippingAgent()
});

//#endregion

//#region Search for the shipping agent by code
When("search shipping agent", () => {
    MaintenanceActions.SearchShippingAgent()
});

Then("the shipping agent should appear successfully", () => {
    MaintenanceActions.AssertSearchShippingAgent(shippingAgentDetails.CompanyName)
});

//#endregion

//#region Open the shipping agent
When("open shipping agent", () => {
    MaintenanceActions.OpenShippingAgent()
});

Then("the shipping agent should open successfully", () => {
    MaintenanceActions.AssertOpenShippingAgent()
    MaintenanceActions.AssertShippingAgentAddress(shippingAgentDetails)
    MaintenanceActions.AssertShippingAgentContact(contactDetails)
});

//#endregion
//#region Edit the shipping agent
Given("the user fill the following shipping agent details", (dataTable) => {
    shippingAgentGeneralTabDetails = Assists.CreateInstance<ShippingAgentGeneralTabDetails>(dataTable, true);
    MaintenanceActions.EditShippingAgentGeneralTab(shippingAgentGeneralTabDetails)
});
 
Given("fill the following shipping agent Billing details", (dataTable) => {
    shippingAgentBillingTabDetails = Assists.CreateInstance<ShippingAgentBillingTabDetails>(dataTable, true);
    MaintenanceActions.EditShippingAgentBillingTab(shippingAgentBillingTabDetails)
});
 
When("update shipping agent", () => {
    MaintenanceActions.UpdateShippingAgent()
});
 
Then("the shipping agent should update successfully", () => {
    MaintenanceActions.AssertUpdateShippingAgent()
});
 
//#endregion