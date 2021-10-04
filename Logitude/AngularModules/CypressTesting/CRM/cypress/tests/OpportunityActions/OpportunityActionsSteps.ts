import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as OpportunityActions from "../../actions/OpportunityActions";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { OpportunityDetails } from "cypress/models/OpportunityDetails";
import { OpportunitySelectors } from "../../selectors/OpportunitySelectors"
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails"

//#region Create new Opportunity
Given("the user logged in and navigates to opportunity workspace", () => {
    cy.Login();
    OpportunityActions.NavigatesOpportunityWorkSpace();
});

Given("fill the following details", (dataTable) => {
    let opportunityDetails = Assists.CreateInstance<OpportunityDetails>(dataTable, true);
    OpportunityActions.FillOpportunityWizardsFields(opportunityDetails);
});

When("create Opportunity", () => {
    OpportunityActions.CreateOpportunity();
});

Then("the Opportunity should create successfully", () => {
    OpportunityActions.AssertCreateOpportunity()
});
//#endregion

//#region Search for the Opportunity by subject
When("search Opportunity", () => {
    OpportunityActions.SearchOpportunity()
});

Then("the Opportunity should appear successfully", () => {
    OpportunityActions.AssertSearchOpportunity()
});
//#endregion

//#region Open the Opportunity
When("open Opportunity", () => {
    OpportunityActions.OpenOpportunity();
});

Then("the Opportunity should open successfully", () => {
    OpportunityActions.AssertOpenOpportunity();
});
//#endregion

//#region Opportunity actions
Given("open {string} action", (action) => {
    OpportunityActions.OpenOpportunityAction(action);
});

When("close as won", () => {
    OpportunityActions.CloseASWon();
});

When("reopen with {string} as satge", (stage) => {
    OpportunityActions.SelectStageWhenReOpen(stage);
});

When("close as lost with {string} as closing reason", (closingReason) => {
    OpportunityActions.CloseASLost(closingReason);
});

Then("opportunity stage status should be {string}", (stageStatus) => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutOpportunities, 200);
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, stageStatus);
});

Then("following events should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, OpportunitySelectors.EventsTab);
});
  //#endregion