import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ContactDetails } from "../../models/ContactDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { CardDetails } from "../../models/CardDetails";
import { Constants } from "../../constants/Constants";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { CardBillingTabDetails } from "../../models/CardBillingTabDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as MaintenanceBaseActions from "../../actions/BaseActions"

let code = null
//#region Create new Agent
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.AgentMaintenanceItem)
});

Given("an agent with the following details", (dataTable) => {
    let agentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    Actions.OpenNewWizard("Agent");
    Actions.FillCardDetails(agentDetails, null)
});

Given("an agent contact with the following details", (dataTable) => {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    Actions.FillCardContactDetails(contactDetails)
});

When("create agent", () => {
    Actions.CreateCard();
});

Then("the agent should create successfully", () => {
    Actions.AssertCreateCard(Constants.Agent)
});
//#endregion

//#region Search for the agent by code
When("search agent", () => {
    code = Actions.getCardCode()
    MaintenanceBaseActions.Search(code)
});

Then("the agent should appear successfully", () => {
    MaintenanceBaseActions.AssertSearch(code);
});
//#endregion

//#region Open the agent
When("open agent", () => {
    Actions.OpenCard(Constants.Agent)
});

Then("the agent should open successfully", () => {
    Actions.AssertOpenCard()
});
//#endregion

//#region Edit the agent
Given("{string} as agent notes", (notes) => {
    cy.FillLogTextBox(MaintenanceSelectors.AgentNotes, notes)
});

When("update agent", () => {
    Actions.UpdateAgent()
});

Then("the agent should update successfully", () => {
    Actions.AssertUpdateAgent()
});
//#endregion