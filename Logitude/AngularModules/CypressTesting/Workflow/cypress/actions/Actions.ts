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
    cy.Click(WorkflowSelectors.WorkFlowFlowRow + BaseSelectors.FirstElement, null);
}

export function AssertOpenFlowBuilder() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
    BaseAssertion.AssertElementExist(WorkflowSelectors.FlowBuilderEditButton)
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
    cy.FillLogTextBox(WorkflowSelectors.WorkflowDescription, workflowDetails.Description);
    cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowOwner,workflowDetails.Owner)
}

export function OpenEditStartNode() {
    cy.get(WorkflowSelectors.WorkflowStartNode).then(() => {
        cy.Click(WorkflowSelectors.WorkflowStartEditButton, null)
    })
}

export function OpenNewWorkflow() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetNewWorkflow, RequestAliases.GetNewWorkflow);
    cy.Click(WorkflowSelectors.NewWorkflow, null)
    BaseAssertion.AssertStatusCode(RequestAliases.GetNewWorkflow, 200)
}

export function CreateNewWorkflow() {
    cy.DefineRequestWait(RestAPI.POST, URLs.WorkflowRequest, RequestAliases.PostWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowCreateButton, null)
}

export function SaveWorkflow() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowStartOkButton, 'Ok', true)
    cy.Click(WorkflowSelectors.WorkflowSaveButton, null)
}

export function AssertSaveWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWorkflowFlowBuilder, 200)
}

export function AssertCreateWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostWorkflowFlowBuilder, 200)
}

export function FillRootConditionsDetails(groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    cy.Click(WorkflowSelectors.WorkflowFirstAddCondition, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowRootOperation, groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, true, (ConditionCounter + conditionDetailsList.length));
    ConditionGroupButton = ConditionCounter;
}

export function FillGroupConditionDetails(IsRootGroup: boolean, groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    OpenEditStartNode();
    if (!IsRootGroup) {
        cy.Click(WorkflowSelectors.WorkflowRootGroupCondition, null);
    }
    else {
        cy.Click(WorkflowSelectors.WorkflowGroupButton(ConditionGroupButton - conditionDetailsList.length), null)
    }
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);

    FillConditionsGroup(conditionDetailsList, false, ((ConditionCounter + conditionDetailsList.length)));
    ConditionGroupButton = ConditionCounter;
}

export function FillNestedGroupConditionDetails(secondGroupSelector: number, groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    OpenEditStartNode();
    cy.Click(WorkflowSelectors.WorkflowGroupButton(secondGroupSelector), null)
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);

    FillConditionsGroup(conditionDetailsList, false, ((ConditionCounter + conditionDetailsList.length)));
    ConditionGroupButton = ConditionCounter;
}

export function FillConditionsGroup(conditionDetailsList: ConditionDetails[], IsRootConditions: boolean, LoopCounter: number) {
    var ListCounter = 0
    for (let i = ConditionCounter; i < LoopCounter; i++, ListCounter++) {
        cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowConditionField(i), conditionDetailsList[ListCounter].Field);
        BaseAssertion.AssertStatusCode(RequestAliases.GetObjectFieldViews, 200);
        cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowConditionOperation(i), conditionDetailsList[ListCounter].Operation, 0);
        FillConditionValue(WorkflowSelectors.WorkflowConditionValue(i), conditionDetailsList[ListCounter].Value, conditionDetailsList[ListCounter].Field);
        ConditionCounter++;
        if (i < (LoopCounter - 1) && IsRootConditions) {
            cy.Click(WorkflowSelectors.WorkflowAddRootCondition, null);
        }
        else if (i < (LoopCounter - 1) && IsRootConditions == false) {
            cy.Click(WorkflowSelectors.WorkflowAddConditionButton(ConditionGroupButton), null);
        }
    }
}

function FillConditionValue(selector: string, value: string, condition: string) {
    switch (condition) {
        case "Main Carriage Final ATA":
            return cy.FillDate("input" + selector, value);
        case "Profit Differences":
            return cy.SelectDropDownListItem2(selector, value);
        case "Containers Numbers":
            return cy.FillLogTextBox(selector, value);
        case "Agent":
            return cy.SelectDropDownListItem2(selector, value);
        case "Description of Goods":
            return cy.FillLogTextBox(selector, value);
        case "Create Date":
            return cy.FillDate("input" + selector, value);
        case "Chargeable Weight":
            return cy.FillLogTextBox(selector, value);
        case "Customer":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
        case "Department":
            return cy.SelectDropDownListItem2(selector, value);
        case "Order Gross Weight":
            return cy.FillLogTextBox(selector, value);
        case "Accounting Closed":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
        case "Notes":
            return cy.FillLogTextBox(selector, value);
    }
}
