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

When("export instances run history", () => {
    Actions.ExportRunHistoryInstances();
});

Then("the instances excel file should export successfully", () => {
    Actions.AsserExportRunHistoryInstances();
});

When("sort instances run history by {string}", (SortField: string) => {
    Actions.SortRunHistoryInstances(SortField);
});

Then("the instances should sort successfully", () => {
    Actions.AssertRefreshRunHistory();
});

When("search instance in run history", () => {
    Actions.SearchInstanceByBusinessKey();
});

Then("the result instances should appear successfully", () => {
    Actions.AssertSearchInstanceByBusinessKey();
});

When("filter instances by current date", () => {
    Actions.FilterInstancesBycurrentdate();
});

Then("the instances should filter successfully", () => {
    Actions.AssertFilterInstancesBycurrentdate();
});

When("open instance activity list", () => {
    Actions.OpenSingleInstanceActivityList();
});

Then("the activity list should appear successfully", () => {
    Actions.AssertOpenSingleInstanceActivityList();
});

When("refresh instance activity list", () => {
    Actions.RefreshSingleInstanceActivityList();
});

Then("the activity list should refresh successfully", () => {
    Actions.AssertRefreshSingleInstanceActivityList();
});

































