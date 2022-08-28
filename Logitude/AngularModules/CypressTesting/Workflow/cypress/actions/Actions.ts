import { WorkflowSelectors } from "../selectors/Selectors"
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { URLs } from '../constants/URLs';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { StartNodeDetails } from "../models/StartNodeDetails";
import { WorkflowDetails } from "../models/WorkflowDetails";
import * as GenerateRandoms from '../../../Base/cypress/actions/GenerateRandoms';

export function NavigatesToAutomationsWorkspace() {
    cy.Click(WorkflowSelectors.AutomationsTab, null)
}

export function OpenWorkflowsInAutomationTab() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.GetWorkflowViews);
    cy.Click(WorkflowSelectors.WorkflowsBox, null)
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
}

export function OpenFirstFlowInWorkFlowList() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowFlowBuilder, RequestAliases.GetWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkFlowSingleFlow(1), null);
}

export function AssertOpenFlowBuilder() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
    BaseAssertion.AssertElementExist(WorkflowSelectors.FlowBuilderEditButton)
}

export function FillEditFlowStartNodeDetails(startNodeDetails: StartNodeDetails) {
    OpenEditStartNode();
    cy.SelectComboDropDownListItem(WorkflowSelectors.WorkflowStartNodeObject, startNodeDetails.Object, 0)
    cy.ClickRadio(WorkflowSelectors.FlowTriggerRadioButton(startNodeDetails.ConfigureTrigger))
}

export function FillWorkflowDetails(workflowDetails: WorkflowDetails) {
    OpenNewWorkflow();
    let FlowName = workflowDetails.Name.toLocaleLowerCase() == "random" ?
    GenerateRandoms.GenerateRandomString(5, true) : null;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowName, FlowName)
}

export function OpenEditStartNode() {
    cy.get(WorkflowSelectors.WorkflowStartNode).then(() => {
        cy.Click(WorkflowSelectors.WorkflowSpan, WorkflowSelectors.WorkflowEdit, null)
    })
}

export function OpenNewWorkflow() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetNewWorkflow, RequestAliases.GetNewWorkflow);
    cy.Click(WorkflowSelectors.NewWorkflow, null)
    BaseAssertion.AssertStatusCode(RequestAliases.GetNewWorkflow, 200)
}

export function CreateNewWorkflow() {
    cy.DefineRequestWait(RestAPI.POST, URLs.WorkflowRequest, RequestAliases.PostWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowButton, WorkflowSelectors.WorkflowCreate, null)
}

export function CloseEditStartNodeWindow() {
    cy.Click(WorkflowSelectors.WorkflowOkButton, WorkflowSelectors.WorkflowOK, null )
}


export function SaveWorkflow() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowSaveButton, WorkflowSelectors.WorkflowSave, true)
}

export function AssertSaveWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWorkflowFlowBuilder, 200)
}
 
export function AssertCreateWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostWorkflowFlowBuilder, 200)
}