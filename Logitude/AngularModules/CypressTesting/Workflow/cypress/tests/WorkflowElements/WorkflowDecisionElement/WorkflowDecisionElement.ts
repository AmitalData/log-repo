import * as Actions from "../../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../.../../../../Base/cypress/assists/Assists";
import { StartNodeDetails } from "../../../models/StartNodeDetails";
import { WorkflowDetails } from "../../../models/WorkflowDetails";
import { ConditionDetails } from "../../../models/ConditionDetails";
import { DecisionElementDetails } from "./../../../models/DecisionElementDetails";

let secondGroupSelector = 0;

Given("the user logged in and navigates to automation workspace", () => {
    cy.Login();
    Actions.NavigatesToAutomationsWorkspace();
});

Given("open workflows list", () => {
    Actions.OpenWorkflowsInAutomationTab();
});

Given("a flow with following details", (dataTable) => {
    let workflowDetails = Assists.CreateInstance<WorkflowDetails>(dataTable, true);
    Actions.FillWorkflowDetails(workflowDetails);
});

When("create flow", () => {
    Actions.CreateNewWorkflow();
});

Then("the flow should create successfully", () => {
    Actions.AssertCreateWorkflow();
});

Given("edit start configration with following details", (dataTable) => {
    let startNodeDetails = Assists.CreateInstance<StartNodeDetails>(dataTable, true);
    Actions.FillEditFlowStartNodeDetails(startNodeDetails);
    Actions.CloseEditStartNodeWindow();
});

Given("add decision element with following details", (dataTable) => {
    let decisonElementDetails = Assists.CreateInstance<DecisionElementDetails>(dataTable, true);
    Actions.FillDecisionElementDetails(decisonElementDetails);
});

Given("add condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupRootConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillDecisionRootConditionsDetails(GroupOperation, groupRootConditionDetailsList);
    secondGroupSelector = secondGroupSelector + groupRootConditionDetailsList.length;
});

When("save flow", () => {
    Actions.SaveDecisionWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("add second level condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillDecisionGroupConditionDetails(false, GroupOperation, groupConditionDetailsList);
    Actions.CloseEditNodeWindow();
});

Given("add third level condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillDecisionGroupConditionDetails(false, GroupOperation, groupConditionDetailsList);
    Actions.CloseEditNodeWindow();

});

Given("add another second level condition group met with {string} with the following details", (GroupOperation, dataTable) => {
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillDecisionNestedGroupConditionDetails(secondGroupSelector + 1, GroupOperation, groupConditionDetailsList, true);

});
When("save flow", () => {
    Actions.SaveDecisionWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});