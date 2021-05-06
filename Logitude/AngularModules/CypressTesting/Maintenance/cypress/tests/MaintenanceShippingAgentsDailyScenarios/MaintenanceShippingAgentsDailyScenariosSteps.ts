import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { AgentDetails } from "../../models/AgentDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import {AgentGeneralTabDetails} from "../../models/AgentGeneralTabDetails";
import { AgentBillingTabDetails } from "../../models/AgentBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from '../../constants/Constants'

let shippingAgentDetails: AgentDetails
let contactDetails: ContactDetails
let shippingAgentGeneralTabDetails:AgentGeneralTabDetails
let shippingAgentBillingTabDetails:AgentBillingTabDetails
//#region Create new shipping agent
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShippingAgents)
});

Given("a shipping agent with the following details", (dataTable) => {
    shippingAgentDetails = Assists.CreateInstance<AgentDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.ShippingAgent);
    MaintenanceActions.FillShippingAgentsDetails(shippingAgentDetails)
});

Given("a shipping agent contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillShippingAgentsContactDetails(contactDetails)
});

When("create shipping agent", () => {
    MaintenanceActions.CreateShippingAgentMockCreate()
});

Then("the shipping agent should create successfully", () => {
    MaintenanceActions.AssertCreateShippingAgentMockCreate()
});

//#endregion
//#region Search for the shipping agent by code
When("search for {string} shipping agent", (shippingAgent) => {
    MaintenanceActions.SearchAgentByValue(shippingAgent)
});

Then("the {string} shipping agent should appear successfully", (shippingAgent) => {
    MaintenanceActions.AssertSearchShippingAgent(shippingAgent)
});

//#endregion
//#region Open the shipping agent
When("open shipping agent", () => {
    MaintenanceActions.OpenAgent(Constants.ShippingAgent)
});

Then("the shipping agent should open successfully", () => {
    MaintenanceActions.AssertOpenShippingAgent()
});
//#endregion
//#region Edit the shipping agent
Given("the user fill the following shipping agent details", (dataTable) => {
    shippingAgentGeneralTabDetails = Assists.CreateInstance<AgentGeneralTabDetails>(dataTable, true);
    MaintenanceActions.EditShippingAgentGeneralTab(shippingAgentGeneralTabDetails)
});
 
Given("fill the following shipping agent Billing details", (dataTable) => {
    shippingAgentBillingTabDetails = Assists.CreateInstance<AgentBillingTabDetails>(dataTable, true);
    MaintenanceActions.EditShippingAgentBillingTab(shippingAgentBillingTabDetails)
});
 
When("update shipping agent", () => {
    MaintenanceActions.UpdateShippingAgent()
});
 
Then("the shipping agent should update successfully", () => {
    MaintenanceActions.AssertUpdateShippingAgent()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.ShippingAgentEventsTab);
});
//#endregion