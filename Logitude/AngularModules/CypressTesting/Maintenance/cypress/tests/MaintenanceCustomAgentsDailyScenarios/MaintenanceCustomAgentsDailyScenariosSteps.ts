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

let customAgentDetails: AgentDetails
let contactDetails: ContactDetails
let customAgentGeneralTabDetails:AgentGeneralTabDetails
let customAgentBillingTabDetails:AgentBillingTabDetails
//#region Create new custom agent
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCustomAgents)
});

Given("a custom agent with the following details", (dataTable) => {
    customAgentDetails = Assists.CreateInstance<AgentDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.CustomAgent);
    MaintenanceActions.FillCustomAgentsDetails(customAgentDetails)
});

Given("a custom agent contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillCustomAgentsContactDetails(contactDetails)
});

When("create custom agent", () => {
    MaintenanceActions.CreateCustomAgentMockCreate()
});

Then("the custom agent should create successfully", () => {
    MaintenanceActions.AssertCreateCustomAgentMockCreate()
});

//#endregion
//#region Search for the custom agent
When("search for {string} custom agent", (customAgent) => {
    MaintenanceActions.SearchAgentByValue(customAgent)
});

Then("the {string} custom agent should appear successfully", (customAgent) => {
    MaintenanceActions.AssertSearchCustomAgent(customAgent)
});

//#endregion
//#region Open the custom agent
When("open custom agent", () => {
    MaintenanceActions.OpenAgent(Constants.CustomAgent)
});

Then("the custom agent should open successfully", () => {
    MaintenanceActions.AssertOpenCustomAgent()
});
//#endregion
//#region Edit the custom agent
Given("the user fill the following custom agent details", (dataTable) => {
    customAgentGeneralTabDetails = Assists.CreateInstance<AgentGeneralTabDetails>(dataTable, true);
    MaintenanceActions.EditCustomAgentGeneralTab(customAgentGeneralTabDetails)
});
 
Given("fill the following custom agent Billing details", (dataTable) => {
    customAgentBillingTabDetails = Assists.CreateInstance<AgentBillingTabDetails>(dataTable, true);
    MaintenanceActions.EditCustomAgentBillingTab(customAgentBillingTabDetails)
});
 
When("update custom agent", () => {
    MaintenanceActions.UpdateCustomAgent()
});
 
Then("the custom agent should update successfully", () => {
    MaintenanceActions.AssertUpdateCustomAgent()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.CustomAgentEventsTab);
});
//#endregion