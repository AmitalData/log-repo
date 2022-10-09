import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("the user logged in and navigates to automation workspace", () => {
    cy.Login();
    Actions.NavigatesToAutomationsWorkspace();
});

Given("open workflows list", () => {
    Actions.OpenWorkflowsInAutomationTab();
});

Given("open a flow", () => {
    Actions.OpenFirstFlowInWorkFlowList();
});

When("open run history", () => {
    Actions.OpenFlowRunHistory();
});

Then("the instances should appear successfully", () => {
    Actions.AssertOpenFlowRunHistory();
});

When("refresh run history", () => {
    Actions.RefreshRunHistory();
});

Then("the instances should refresh successfully", () => {
    Actions.AssertRefreshRunHistory();
});

When("search instance in run history", () => {
    Actions.SearchInstanceByBusinessKey();
});

Then("the instances should refresh successfully", () => {
    Actions.AssertSearchInstanceByBusinessKey();
});


































