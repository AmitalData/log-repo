import * as Actions from "../../actions/Actions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("the user logged in and navigates to automation workspace", () => {
    cy.Login(true)
    Actions.NavigatesToAutomationsWorkspace();
});

Given("open workflows", () => {
    Actions.OpenWorkflowsInAutomationTab();
});

When("open flow", () => {
    Actions.OpenFirstFlowInWorkFlowList();
});

Then("the flow builder open successfully", () => {
    Actions.AssertOpenFlowBuilder();
});