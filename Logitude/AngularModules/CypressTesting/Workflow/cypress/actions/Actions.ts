
import { WorkflowSelectors } from "../selectors/Selectors"
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { URLs } from '../constants/URLs';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';

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