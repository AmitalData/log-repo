import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CardDetails } from "../../models/CardDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import {CardGeneralTabDetails} from "../../models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from '../../constants/Constants'

let shippingAgentDetails: CardDetails
let contactDetails: ContactDetails
let shippingAgentGeneralTabDetails:CardGeneralTabDetails
let shippingAgentBillingTabDetails:CardBillingTabDetails
//#region Create new shipping agent
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShippingAgents)
});

Given("a shipping agent with the following details", (dataTable) => {
    shippingAgentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
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
    MaintenanceActions.SearchCardByValue(shippingAgent)
});

Then("the {string} shipping agent should appear successfully", (shippingAgent) => {
    MaintenanceActions.AssertSearchShippingAgent(shippingAgent)
});

//#endregion
//#region Open the shipping agent
When("open shipping agent", () => {
    MaintenanceActions.OpenCard(Constants.ShippingAgent)
});

Then("the shipping agent should open successfully", () => {
    MaintenanceActions.AssertOpenShippingAgent()
});
//#endregion
//#region Edit the shipping agent
Given("the user fill the following shipping agent details", (dataTable) => {
    shippingAgentGeneralTabDetails = Assists.CreateInstance<CardGeneralTabDetails>(dataTable, true);
    MaintenanceActions.FillShippingAgentGeneralTab(shippingAgentGeneralTabDetails)
});
 
Given("fill the following shipping agent Billing details", (dataTable) => {
    shippingAgentBillingTabDetails = Assists.CreateInstance<CardBillingTabDetails>(dataTable, true);
    MaintenanceActions.FillShippingAgentBillingTab(shippingAgentBillingTabDetails)
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