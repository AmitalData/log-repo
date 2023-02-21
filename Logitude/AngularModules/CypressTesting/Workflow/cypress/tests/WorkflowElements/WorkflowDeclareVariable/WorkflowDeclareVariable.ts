import * as Actions from "../../../actions/Actions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { StartNodeDetails } from "../../../models/StartNodeDetails";
import { WorkflowDetails } from "../../../models/WorkflowDetails";
import { ConditionDetails } from "../../../models/ConditionDetails";
import { PrimitiveVariableDetails } from "../../../models/PrimitiveVariableDetails";
import { RecordVariableDetails } from "../../../models/RecordVariableDetails";
let secondGroupSelector = 1;

Given("the user logged in and navigates to automation workspace", () => {
    cy.Login();
    Actions.NavigatesToAutomationsWorkspace();
});

Given("open workflows", () => {
    Actions.OpenWorkflowsInAutomationTab();
});

Given("a flow with following details", (dataTable) => {
    let workflowDetails = Assists.CreateInstance<WorkflowDetails>(dataTable, true);
    Actions.FillWorkflowDetails(workflowDetails);
});

When("create workflow", () => {
    Actions.CreateNewWorkflow();
});

Then("the flow should create successfully", () => {
    Actions.AssertCreateWorkflow();
});

Given("edit start configration with following details", (dataTable) => {
    let startNodeDetails = Assists.CreateInstance<StartNodeDetails>(dataTable, true);
    Actions.FillEditFlowStartNodeDetails(startNodeDetails);
});

When("save flow", () => {
    Actions.SaveWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("declare a primitive variable with following details", (dataTable) => {
    let PrimitiveVariableDetails = Assists.CreateInstance<PrimitiveVariableDetails>(dataTable, true);
    Actions.FillPrimitiveVariableDetails(PrimitiveVariableDetails, 1);

});
When("save flow", () => {
    Actions.SaveDeclareVariableWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("declare a Record variable with following details", (dataTable) => {
    let RecordVariableDetails = Assists.CreateInstance<RecordVariableDetails>(dataTable, true);
    Actions.FillRecordVariableDetails(RecordVariableDetails, 2);
});

When("save flow", () => {
    Actions.SaveDeclareVariableWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});
