import { WorkflowSelectors } from "../selectors/Selectors"
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { URLs } from '../constants/URLs';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { StartNodeDetails } from "../models/StartNodeDetails";
import { WorkflowDetails } from "../models/WorkflowDetails";
import * as GenerateRandoms from '../../../Base/cypress/actions/GenerateRandoms';
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { ConditionDetails } from "../models/ConditionDetails";

let ConditionCounter = 1;
let ConditionGroupButton = 1;
let workflowName;

export function NavigatesToAutomationsWorkspace() {
    cy.Click(WorkflowSelectors.AutomationsTab, null)
}

export function OpenWorkflowsInAutomationTab() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.GetWorkflowViews);
    cy.Click(WorkflowSelectors.WorkflowsBox, null)
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
}

export function SearchFlowByName() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetBackToWorkflowsList, RequestAliases.GetBackToWorkflowViews);
    BackToWorkflowList();
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.GetWorkflowViews);
    cy.FillLogTextBox(WorkflowSelectors.WorkflowSearchBox, workflowName);
}

export function RefreshWorkflowLisr() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.GetWorkflowViews);
    cy.Click(WorkflowSelectors.WorkflowListRefreshButton, null)
}

export function ExportWorkflowList() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetQueryExportExecution, RequestAliases.GetQueryExportExecution);
    cy.Click(WorkflowSelectors.WorkflowListExcelExport, null)
}

export function AsserExportWorkflowList() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQueryExportExecution, 200);
    BaseAssertion.AssertElementContain(WorkflowSelectors.WorkflowLinkButton, 'Download file')
}

export function AssertWorkflowListReresh() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
}

export function AssertSearchFlowByName() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200);
}

function BackToWorkflowList() {
    cy.Click(WorkflowSelectors.BackToWorkflowListButton, 'Workflows', null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetBackToWorkflowViews, 200);
}

function GetWorkflowNameInBuilder() {
    cy.get(WorkflowSelectors.FlowNameInFlowBuilder).should(($div) => {
        workflowName = $div.text().replace(/\s/g, "");
    })
}

export function OpenFirstFlowInWorkFlowList() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowFlowBuilder, RequestAliases.GetWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkFlowFlowRow + BaseSelectors.FirstElement, null);
}

export function AssertOpenFlowBuilder() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowFlowBuilder, 200);
    BaseAssertion.AssertElementExist(WorkflowSelectors.FlowBuilderEditButton);
    GetWorkflowNameInBuilder();
}

export function FillEditFlowStartNodeDetails(startNodeDetails: StartNodeDetails) {
    OpenEditStartNode();
    cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowStartNodeObject, startNodeDetails.Object)
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
        cy.Click(WorkflowSelectors.WorkflowStartEditButton, null)
    })
}

export function OpenNewWorkflow() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetNewWorkflow, RequestAliases.GetNewWorkflow);
    cy.Click(WorkflowSelectors.NewWorkflow, null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetNewWorkflow, 200);
}

export function FillUpdateWorkflowDetails(workflowDetails: WorkflowDetails) {
    cy.Click(WorkflowSelectors.FlowEditButton, null);
    let FlowName = workflowDetails.Name.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : null;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowName, FlowName);
}

export function CreateNewWorkflow() {
    cy.DefineRequestWait(RestAPI.POST, URLs.WorkflowRequest, RequestAliases.PostWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowCreateButton, null);
}

export function UpdateNewWorkflow() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowGeneralSaveButton, null);
}

export function AssertUpdateWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWorkflowFlowBuilder, 200)
}

export function CloseEditStartNodeWindow() {
    cy.Click(WorkflowSelectors.WorkflowStartOkButton, 'Ok', true);
}

export function OpenFlowRunHistory() {
    cy.DefineRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetWorkflowInstance);
    cy.Click(WorkflowSelectors.FlowRunHistory, null);
}

export function AssertOpenFlowRunHistory() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowInstance, 200);
    cy.Click(WorkflowSelectors.WorkflowGeneralBackButton, null);
}

export function SaveWorkflow() {
    CloseEditStartNodeWindow();
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowSaveButton, null)
}

export function AssertSaveWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWorkflowFlowBuilder, 200);
}

export function AssertCreateWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostWorkflowFlowBuilder, 200);
}

export function FillConditionDetails(conditionDetails: ConditionDetails) {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.Click(WorkflowSelectors.WorkflowAddCondition, 'Add Condition', null).then(() => {
        BaseAssertion.AssertStatusCode(RequestAliases.GetObjectFieldViews, 200);
        cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowConditionField(1), conditionDetails.Field);
        cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowConditionOperation(1), conditionDetails.Operation, 0);
        cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowConditionValue(1), conditionDetails.Value);
        ConditionCounter++;
    })
}

export function FillGroupConditionDetails(groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    cy.Click(WorkflowSelectors.WorkflowRootGroupCondition, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);
    ConditionGroupButton = ConditionCounter;
    FillConditionsGroup(conditionDetailsList);
}

function FillConditionsGroup(conditionDetailsList: ConditionDetails[]) {
    for (let i = ConditionCounter, ListCounter = 0; i <= (conditionDetailsList.length + 1); i++, ListCounter++) {
        cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowConditionField(i), conditionDetailsList[ListCounter].Field);
        BaseAssertion.AssertStatusCode(RequestAliases.GetObjectFieldViews, 200);
        cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowConditionOperation(i), conditionDetailsList[ListCounter].Operation, 0);
        FillConditionValue(WorkflowSelectors.WorkflowConditionValue(i), conditionDetailsList[ListCounter].Value, conditionDetailsList[ListCounter].Field);
        ConditionCounter++;
        if (i < (conditionDetailsList.length + 1))
            cy.Click(WorkflowSelectors.WorkflowAddConditionButton(ConditionGroupButton), null);
    }
}

function FillConditionValue(selector: string, value: string, condition: string) {
    switch (condition) {
        case "Custom Lookup":
            return cy.SelectDropDownListItem2(selector, value);
        case "Custom text":
            return cy.FillLogTextBox(selector, value);
        case "Custom Ntext":
            return cy.FillLogTextBox(selector, value);
        case "Custom Bool":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
    }
}
