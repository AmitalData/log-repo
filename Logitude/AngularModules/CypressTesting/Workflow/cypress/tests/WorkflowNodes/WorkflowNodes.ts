import * as Actions from "../../actions/Actions"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../.../../../../Base/cypress/assists/Assists";
import { StartNodeDetails } from "../../models/StartNodeDetails";
import { WorkflowDetails } from "../../models/WorkflowDetails";
import { ConditionDetails } from "../../models/ConditionDetails";


Given("the user logged in and navigates to automation workspace", () => {
    cy.Login(true)
    Actions.NavigatesToAutomationsWorkspace()
});

Given("open workflows", () => {
    Actions.OpenWorkflowsInAutomationTab()
});

Given("a flow with following details", (dataTable) => {
    let workflowDetails = Assists.CreateInstance<WorkflowDetails>(dataTable, true);
    Actions.FillWorkflowDetails(workflowDetails);
});

When("click create", () => {
    Actions.CreateNewWorkflow()
});

Then("the flow should create successfully", () => {
    Actions.AssertCreateWorkflow()
})

Given("edit start configration with following details", (dataTable) => {
    let startNodeDetails = Assists.CreateInstance<StartNodeDetails>(dataTable, true);
    Actions.FillEditFlowStartNodeDetails(startNodeDetails)
});

Given("add condition with following details", (dataTable) => {
    let conditionDetails = Assists.CreateInstance<ConditionDetails>(dataTable, true);
    Actions.FillConditionDetails(conditionDetails);
});

Given("add condition group met with {string} with the following details", (GroupOperation,dataTable) => {
    debugger
    let groupConditionDetailsList = Assists.CreateSet<ConditionDetails>(dataTable);
    Actions.FillGroupConditionDetails(GroupOperation, groupConditionDetailsList);
});

Given("click Ok", () => {
    Actions.CloseEditStartNodeWindow();
});

When("click save", () => {
    Actions.SaveWorkflow();
});

Then("the flow should save successfully", () => {
    Actions.AssertSaveWorkflow();
});