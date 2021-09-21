import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ContactDetails } from "../../models/ContactDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { CardDetails } from "../../models/CardDetails";
import { Constants } from "../../constants/Constants";

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