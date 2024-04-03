import * as Actions from "../../actions/Actions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("the user logged in and navigates to automation workspace", () => {
    cy.Login();
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

When("refresh workflow list", () => {
    Actions.RefreshWorkflowList();
});

Then("the list should refresh successfully", () => {
    Actions.AssertWorkflowListReresh();
});

When("export workflow list", () => {
    Actions.ExportWorkflowList();
});

Then("the list should export successfully", () => {
    Actions.AsserExportWorkflowList();
});

When("search flow", () => {
    Actions.SearchFlowByName();
});

Then("the flow should appear successfully", () => {
    Actions.AssertSearchFlowByName();
});