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
let mai;
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

export function FillConditionDetails(conditionDetails: ConditionDetails) {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.Click(WorkflowSelectors.WorkflowFirstAddCondition, null).then(() => {
        BaseAssertion.AssertStatusCode(RequestAliases.GetObjectFieldViews, 200);
        cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowConditionField(1), conditionDetails.Field);
        cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowConditionOperation(1), conditionDetails.Operation, 0);
        FillConditionValue(WorkflowSelectors.WorkflowConditionValue(1), conditionDetails.Value, conditionDetails.Field);
        ConditionCounter++;
    })
}

export function FillRootConditionsDetails(groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    cy.Click(WorkflowSelectors.WorkflowAddRootCondition, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowRootOperation, groupCondition, 0);
    let ConditionGroupButton = ConditionCounter;
    cy.log("root condition counter" + (ConditionCounter + conditionDetailsList.length))
    FillConditionsGroup(conditionDetailsList, (ConditionGroupButton + conditionDetailsList.length));
}

export function FillGroupConditionDetails(groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    OpenEditStartNode();
    cy.Click(WorkflowSelectors.WorkflowRootGroupCondition, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);
    let ConditionGroupButton = ConditionCounter;
    FillConditionsGroup(conditionDetailsList, ((ConditionGroupButton + conditionDetailsList.length)));
}

export function FillConditionsGroup(conditionDetailsList: ConditionDetails[], sss: number) {
    cy.log("ssssssssssss" + sss)
    for (let i = ConditionCounter, ListCounter = 0; i <= sss; i++, ListCounter++) {
        cy.log("iiiiiiiiiiiiiiiiiiiiiiiiiiiii"+i)
        cy.SelectDropDownListItem2(WorkflowSelectors.WorkflowConditionField(i), conditionDetailsList[ListCounter].Field);
        BaseAssertion.AssertStatusCode(RequestAliases.GetObjectFieldViews, 200);
        cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowConditionOperation(i), conditionDetailsList[ListCounter].Operation, 0);
        FillConditionValue(WorkflowSelectors.WorkflowConditionValue(i), conditionDetailsList[ListCounter].Value, conditionDetailsList[ListCounter].Field);
        ConditionCounter++;
        if (i < (conditionDetailsList.length + 1))
            cy.Click(WorkflowSelectors.WorkflowAddRootCondition, null);
    }
}

function FillConditionValue(selector: string, value: string, condition: string) {
    switch (condition) {
        case "Main Carriage Final ATA":
            return cy.FillDate(selector, value);
        case "Profit Differences":
            return cy.SelectDropDownListItem2(selector, value);
        case "Containers Numbers":
            return cy.FillLogTextBox(selector, value);
        case "Agent":
            return cy.SelectDropDownListItem2(selector, value);
        case "Description of Goods":
            return cy.FillLogTextBox(selector, value);
    }
}
