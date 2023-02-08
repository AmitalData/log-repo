import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../.../../../../Base/cypress/assists/Assists";
import { WorkflowDetails } from "../../models/WorkflowDetails";
import { StartNodeDetails } from "../../models/StartNodeDetails";

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

Given("edit workflow general inforamtion with following details", (dataTable) => {
    let workflowDetails = Assists.CreateInstance<WorkflowDetails>(dataTable, true);
    Actions.FillUpdateWorkflowDetails(workflowDetails);
});

When("update flow", () => {
    Actions.UpdateNewWorkflow();
});

Then("the flow should update successfully", () => {
    Actions.AssertUpdateWorkflow();
});