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
import { GetEditableDetails } from "../../../models/GetEditableDetails";
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

Given("add editable record with following details", (dataTable) => {
    let getEditablRecordDetails = Assists.CreateInstance<GetEditableDetails>(dataTable, true);
    Actions.FilEditableRecordDetails(getEditablRecordDetails, ConnectorCount);
    ConnectorCount++;
});

When("save flow", () => {
    Actions.SaveGetEditableRecordWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});