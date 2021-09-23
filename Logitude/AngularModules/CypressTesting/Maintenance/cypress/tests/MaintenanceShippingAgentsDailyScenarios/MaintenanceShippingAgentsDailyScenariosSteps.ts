import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as GeneralActions from "../../actions/BaseActions";
import { CardDetails } from "../../models/CardDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ContactDetails } from "../../models/ContactDetails";
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from '../../constants/Constants'

let shippingAgentDetails: CardDetails
let contactDetails: ContactDetails
let shippingAgentBillingTabDetails: CardBillingTabDetails

//#region Add shipping agent city with lenght more than 25
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShippingAgents)
});

When("add {string} as city", (city) => {
    MaintenanceActions.OpenNewWizard(Constants.ShippingAgent);
    cy.FillLogTextBox(MaintenanceSelectors.CardCity, city);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Assert create shipping agent without compnay name
Given("the user fill the required fields except the company", () => {
    MaintenanceActions.FillCardsCityAndCountryFeilds()
});

When("create shipping agent", () => {
    MaintenanceActions.CreateShippingAgentMockCreate()
});

Then("a validation single message with {string} error should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Create new shipping agent
Given("a shipping agent with the following details", (dataTable) => {
    shippingAgentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    MaintenanceActions.FillShippingAgentsDetails(shippingAgentDetails)
});

Given("a shipping agent contact with the following details", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    MaintenanceActions.FillShippingAgentsContactDetails(contactDetails)
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
Given("{string} as shipping agent notes", (notes) => {
    MaintenanceActions.FillShippingAgentGenaralTabNotes(notes)
});

Given("inactivate the shipping agent", () => {
    cy.ClickCheckBox(MaintenanceSelectors.InActiveShippingAgentCheckBox)
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