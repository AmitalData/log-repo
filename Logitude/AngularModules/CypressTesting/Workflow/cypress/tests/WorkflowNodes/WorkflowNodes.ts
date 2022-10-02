import * as Actions from "../../actions/Actions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../.../../../../Base/cypress/assists/Assists";
import { StartNodeDetails } from "../../models/StartNodeDetails";
import { WorkflowDetails } from "../../models/WorkflowDetails";
import { ConditionDetails } from "../../models/ConditionDetails";

let secondGroupSelector = 0;

Given("the user logged in and navigates to automation workspace", () => {
    cy.Login(true);
    Actions.NavigatesToAutomationsWorkspace();
});

Given("open workflows", () => {
    Actions.OpenWorkflowsInAutomationTab();
});

Given("a flow with following details", (dataTable) => {
    let workflowDetails = Assists.CreateInstance<WorkflowDetails>(dataTable, true);
    Actions.FillWorkflowDetails(workflowDetails);
});

When("click create", () => {
    Actions.CreateNewWorkflow();
});

Then("the flow should create successfully", () => {
    Actions.AssertCreateWorkflow();
});

Given("edit workflow general inforamtion with following details", (dataTable) => {
    let workflowDetails = Assists.CreateInstance<WorkflowDetails>(dataTable, true);
    Actions.FillUpdateWorkflowDetails(workflowDetails);
});

When("save workflow", () => {
    Actions.UpdateNewWorkflow();
});

Then("the workflow should update successfully", () => {
    Actions.AssertUpdateWorkflow();
});

When("open run history", () => {
    Actions.OpenFlowRunHistory();
});

Then("the instances should appear successfully", () => {
    Actions.AssertOpenFlowRunHistory();
});

Given("edit start configration with following details", (dataTable) => {
    let startNodeDetails = Assists.CreateInstance<StartNodeDetails>(dataTable, true);
    Actions.FillEditFlowStartNodeDetails(startNodeDetails);
});

Given("add condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupRootConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillRootConditionsDetails(GroupOperation, groupRootConditionDetailsList);
    secondGroupSelector = secondGroupSelector + groupRootConditionDetailsList.length;

});

When("save flow", () => {
    Actions.SaveWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("add second level condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillGroupConditionDetails(false, GroupOperation, groupConditionDetailsList);
});

When("save flow", () => {
    Actions.SaveWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("add third level condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillGroupConditionDetails(true, GroupOperation, groupConditionDetailsList);
});

When("save flow", () => {
    Actions.SaveWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("add another second level condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillNestedGroupConditionDetails(secondGroupSelector + 1, GroupOperation, groupConditionDetailsList);
});

When("save flow", () => {
    Actions.SaveWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});