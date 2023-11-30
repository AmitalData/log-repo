import * as Actions from "../../../actions/Actions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { StartNodeDetails } from "../../../models/StartNodeDetails";
import { WorkflowDetails } from "../../../models/WorkflowDetails";
import { ConditionDetails } from "../../../models/ConditionDetails";
import { PrimitiveVariableDetails } from "../../../models/PrimitiveVariableDetails";
import { RecordVariableDetails } from "../../../models/RecordVariableDetails";
import { AssignmentDetails } from "../../../models/AssignmentDetails";
import { AssignVariablesDetails } from "../../../models/AssignVariablesDetails";
let secondGroupSelector = 1;
let ConnectorCount = 1;

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
    let primitiveVariableDetails = Assists.CreateInstance<PrimitiveVariableDetails>(dataTable, true);
    Actions.FillPrimitiveVariableDetails(primitiveVariableDetails, ConnectorCount);
    ConnectorCount++;
    Actions.CloseEditNodeWindow();

});

Given("declare another primitive variable with following details", (dataTable) => {
    let primitiveVariableDetails = Assists.CreateInstance<PrimitiveVariableDetails>(dataTable, true);
    Actions.FillPrimitiveVariableDetails(primitiveVariableDetails, ConnectorCount);
    ConnectorCount++;
});

When("save flow", () => {
    Actions.SaveDeclareVariableWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("add assign element with following details", (dataTable) => {
    let assignmentDetails = Assists.CreateInstance<AssignmentDetails>(dataTable, true);
    Actions.FillAssignmentDetails(assignmentDetails, ConnectorCount);
});

Given("assign primitive varaibles with following details", (dataTable) => {
    let assignVariablesDetails = Assists.CreateSet<AssignVariablesDetails>(dataTable);
    Actions.FillAssignVariablesDetails(assignVariablesDetails, true);
});

When("save flow", () => {
    Actions.SaveDeclareVariableWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});

Given("add second assign element with following details", (dataTable) => {
    let assignmentDetails = Assists.CreateInstance<AssignmentDetails>(dataTable, true);
    Actions.FillAssignmentDetails(assignmentDetails, ConnectorCount);
});

When("assign primitive varaibles in second assignment with following details", (dataTable) => {
    let assignVariablesDetails = Assists.CreateSet<AssignVariablesDetails>(dataTable);
    Actions.FillAssignVariablesDetails(assignVariablesDetails, false);
});

Then("the field should not appear", () => {
    Actions.AssertFieldDoseNotExists();
});