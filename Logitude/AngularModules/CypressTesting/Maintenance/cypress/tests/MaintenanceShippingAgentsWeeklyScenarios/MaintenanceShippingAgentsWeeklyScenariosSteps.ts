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
    MaintenanceActions.OpenCard(Constants.ShippingAgent)
});

Then("the shipping agent should open successfully", () => {
    MaintenanceActions.AssertOpenShippingAgent()
});

Then("the shipping agent address should have the following details", (dataTable) => {
    shippingAgentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.AssertShippingAgentAddress(shippingAgentDetails)
});

Then("the shipping agent contact should have the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.AssertShippingAgentContact(contactDetails)
});
//#endregion
//#region Edit the shipping agent
Given("the user fill the following shipping agent general details", (dataTable) => {
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