import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CardDetails } from "../../models/CardDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as GeneralActions from "../../actions/BaseActions";
import { Constants } from '../../constants/Constants'

let customAgentDetails: CardDetails
let contactDetails: ContactDetails
let customAgentBillingTabDetails: CardBillingTabDetails

//#region Add custom agent city with lenght more than 25
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCustomAgents)
});

When("add {string} as city", (city) => {
    MaintenanceActions.OpenNewWizard(Constants.CustomAgent);
    cy.FillLogTextBox(MaintenanceSelectors.CardCity, city);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Assert create custom agent without compnay name
Given("the user fill the required fields except the company", () => {
    MaintenanceActions.FillCardsCityAndCountryFeilds()
});

When("create custom agent", () => {
    MaintenanceActions.CreateCustomAgent()
});

Then("a validation single message with {string} error should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Create new custom agent
Given("a custom agent with the following details", (dataTable) => {
    customAgentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.FillCustomAgentsDetails(customAgentDetails)
});

Given("a custom agent contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillCustomAgentsContactDetails(contactDetails)
});

Then("the custom agent should create successfully", () => {
    MaintenanceActions.AssertCreateCustomAgent()
});
//#endregion

//#region Search for the custom agent by code
When("search custom agent", () => {
    MaintenanceActions.SearchCustomAgent()
});

Then("the custom agent should appear successfully", () => {
    MaintenanceActions.AssertSearchCustomAgent(customAgentDetails.CompanyName)
});
//#endregion

//#region Open the custom agent
When("open custom agent", () => {
    MaintenanceActions.OpenCard(Constants.CustomAgent)
});

Then("the custom agent should open successfully", () => {
    MaintenanceActions.AssertOpenCustomAgent()
});

Then("the custom agent address should have the following details", (dataTable) => {
    customAgentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.AssertCustomAgentAddress(customAgentDetails)
});

Then("the custom agent contact should have the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.AssertCustomAgentContact(contactDetails)
});
//#endregion

//#region Edit the custom agent
Given("{string} as custom agent notes", (notes) => {
    MaintenanceActions.FillCustomAgentGeneralTabNotes(notes)
});

Given("inactivate the custom agent", () => {
    cy.ClickCheckBox(MaintenanceSelectors.InActiveCustomAgentCheckBox)
});

Given("fill the following custom agent Billing details", (dataTable) => {
    customAgentBillingTabDetails = Assists.CreateInstance<CardBillingTabDetails>(dataTable, true);
    MaintenanceActions.FillCustomAgentBillingTab(customAgentBillingTabDetails)
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