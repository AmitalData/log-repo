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
import { WorkflowRunHistoryFixturePath } from '../fixtures/WorkflowRunHistory/WorkflowRunHistoryFixturePath'
import { DecisionElementDetails } from "../models/DecisionElementDetails";
import { WorkflowlistFixturePath } from "../fixtures/WorkflowList/WorkflowListFixturePath";
import { PrimitiveVariableDetails } from "../models/PrimitiveVariableDetails";
import { RecordVariableDetails } from "../models/RecordVariableDetails";
import { AssignmentDetails } from "../models/AssignmentDetails";
import { AssignVariablesDetails } from "../models/AssignVariablesDetails";
import { GetEditableDetails } from "../models/GetEditableDetails";

let ConditionCounter = 2;
let ConditionGroupButton = 1;
let SearchworkflowName;
let InstanceBusinessKey;
let SearchInstanceBusinessKey;

export function NavigatesToAutomationsWorkspace() {
    cy.Click(WorkflowSelectors.AutomationsTab, null)
}

export function OpenWorkflowsInAutomationTab() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.GetWorkflowViews);
    cy.Click(WorkflowSelectors.WorkflowsBox, null)
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
}

export function RefreshWorkflowList() {
    cy.fixture(WorkflowlistFixturePath.MockWorkflowList).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.GetWorkflowViews, response);
        SearchworkflowName = response.Result[0].Name;
    });
    cy.Click(WorkflowSelectors.WorkflowListRefreshButton, null)
};

export function AssertWorkflowListReresh() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200);
}

export function ExportWorkflowList() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetQueryExportExecution, RequestAliases.GetQueryExportExecution);
    cy.Click(WorkflowSelectors.WorkflowListExcelExport, null)
};

export function AsserExportWorkflowList() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQueryExportExecution, 200);
    BaseAssertion.AssertElementContain(WorkflowSelectors.WorkflowLinkButton, 'Download file');
    cy.Click(BaseSelectors.button, BaseSelectors.ContainsCancel)
}

function BackToWorkflowList() {
    cy.Click(WorkflowSelectors.BackToWorkflowListButton, 'Workflows', null);
}


export function OpenFirstFlowInWorkFlowList() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetWorkflowViews, 200)
    cy.Click(WorkflowSelectors.WorkFlowFlowRow + BaseSelectors.FirstElement, null, true);
}

export function AssertOpenFlowBuilder() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetWorkflowFlowBuilder, RequestAliases.GetWorkflowFlowBuilder);
    BackToWorkflowList();
}

export function FillEditFlowStartNodeDetails(startNodeDetails: StartNodeDetails) {
    cy.get(WorkflowSelectors.WorkflowStartNodeObject).find(BaseSelectors.input).click().type(startNodeDetails.Object).then(() => {
        cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains(startNodeDetails.Object).eq(0).click()
    });
    cy.ClickRadio(WorkflowSelectors.FlowTriggerRadioButton(startNodeDetails.ConfigureTrigger));
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    FillConditionFieldName(true, 1, 'Notes');
}

export function FillWorkflowDetails(workflowDetails: WorkflowDetails) {
    OpenNewWorkflow();
    let FlowName = workflowDetails.Name.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : null;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowName, FlowName)
    cy.FillLogTextBox(WorkflowSelectors.WorkflowDescription, workflowDetails.Description);
}

export function OpenEditStartNode() {
    cy.get(WorkflowSelectors.WorkflowStartNode).then(() => {
        cy.Click(WorkflowSelectors.WorkflowStartEditButton, null)
    })
}

export function OpenNewWorkflow() {
   // cy.DefineRequestWait(RestAPI.GET, URLs.GetNewWorkflow, RequestAliases.GetNewWorkflow);
    cy.Click(WorkflowSelectors.NewWorkflow, null);
   // BaseAssertion.AssertStatusCode(RequestAliases.GetNewWorkflow, 200);
}

export function SearchFlowByName() {
    cy.fixture(WorkflowlistFixturePath.MockSingleWorkflow).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.GetWorkflowViews, RequestAliases.MockSingleWorkflowView, response);
    });
    cy.FillLogTextBox(WorkflowSelectors.WorkflowSearchBox, SearchworkflowName);
    console.log(SearchworkflowName);
};

export function AssertSearchFlowByName() {
    BaseAssertion.AssertStatusCode(RequestAliases.MockSingleWorkflowView, 200);
};

export function FillUpdateWorkflowDetails(workflowDetails: WorkflowDetails) {

    cy.Click(WorkflowSelectors.WorkflowGeneralTab, null);
    let FlowName = workflowDetails.Name.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : null;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowName, FlowName);
    cy.FillLogTextBox(WorkflowSelectors.WorkflowDescription, workflowDetails.Description);
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
    cy.Click(WorkflowSelectors.WorkflowStartOkButton, null);
}

export function SaveWorkflow() {
    CloseEditStartNodeWindow();
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowVersionRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowSaveDraft, null)
}

export function AssertSaveWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWorkflowFlowBuilder, 200);
}

export function AssertCreateWorkflow() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostWorkflowFlowBuilder, 200);
}

export function FillRootConditionsDetails(groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    cy.Click(WorkflowSelectors.WorkflowAddRootCondition, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowRootOperation, groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, true, (ConditionCounter + conditionDetailsList.length), true);
    ConditionGroupButton = ConditionCounter;
}

export function FillGroupConditionDetails(IsRootGroup: boolean, groupCondition: string, conditionDetailsList: ConditionDetails[], IsFromList: boolean) {
    OpenEditStartNode();
    if (!IsRootGroup) {
        cy.Click(WorkflowSelectors.WorkflowRootGroupCondition, null);
    }
    else {
        cy.Click(WorkflowSelectors.WorkflowGroupButton(ConditionGroupButton - conditionDetailsList.length), null)
    }
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, false, ((ConditionCounter + conditionDetailsList.length)), IsFromList);
    ConditionGroupButton = ConditionCounter;
}

export function FillNestedGroupConditionDetails(secondGroupSelector: number, groupCondition: string, conditionDetailsList: ConditionDetails[], IsFromList: boolean) {
    OpenEditStartNode();
    cy.Click(WorkflowSelectors.WorkflowGroupButton(secondGroupSelector), null)
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, false, ((ConditionCounter + conditionDetailsList.length)), IsFromList);
    ConditionGroupButton = ConditionCounter;
}

export function FillDecisionNestedGroupConditionDetails(secondGroupSelector: number, groupCondition: string, conditionDetailsList: ConditionDetails[], IsFromList: boolean) {
    OpenEditDecisionElement(1);
    cy.Click(WorkflowSelectors.WorkflowGroupButton(secondGroupSelector), null)
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, false, ((ConditionCounter + conditionDetailsList.length)), IsFromList);
    ConditionGroupButton = ConditionCounter;
}

export function FillConditionsGroup(conditionDetailsList: ConditionDetails[], IsRootConditions: boolean, LoopCounter: number, isFromList: boolean) {
    var ListCounter = 0
    for (let i = ConditionCounter; i < LoopCounter; i++, ListCounter++) {
        FillConditionFieldName(isFromList, i, conditionDetailsList[ListCounter].Field);
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

export function OpenFlowRunHistory() {
    cy.Click(WorkflowSelectors.WorkflowRunHistory, null);
    cy.fixture(WorkflowRunHistoryFixturePath.MockWorkflowInstaces).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetMockWorkflowInstances, response);
        InstanceBusinessKey = response.Result[0].BusinessKey
    });
    cy.Click(WorkflowSelectors.FlowRunHistory, null);
}

export function AssertOpenFlowRunHistory() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockWorkflowInstances, 200);
}

export function RefreshRunHistory() {
    cy.fixture(WorkflowRunHistoryFixturePath.MockWorkflowInstaces).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetMockWorkflowInstances, response);
    });
    cy.Click(WorkflowSelectors.RunHistoryRefreshButton, null)
}

export function AssertRefreshRunHistory() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockWorkflowInstances, 200);
}

export function ExportRunHistoryInstances() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetQueryToExcelData, RequestAliases.GetQueryToExcelData);
    cy.Click(WorkflowSelectors.RunHistoryExportFile, null)
}

export function AsserExportRunHistoryInstances() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQueryToExcelData, 200);
    BaseAssertion.AssertElementContain(WorkflowSelectors.WorkflowLinkButton, 'Download file');
    cy.Click(BaseSelectors.button, BaseSelectors.ContainsCancel);
}

export function SortRunHistoryInstances(SortField: string) {
    cy.fixture(WorkflowRunHistoryFixturePath.MockWorkflowInstaces).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetMockWorkflowInstances, response);
    });
    cy.Click(WorkflowSelectors.RunHistoryHeaderColumnSelector(SortField), null)
}

export function AssertSortRunHistoryInstances() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockWorkflowInstances, 200);
}

export function FilterInstancesBycurrentdate() {
    cy.fixture(WorkflowRunHistoryFixturePath.MockWorkflowInstaces).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetMockWorkflowInstances, response);
    });
    cy.Click(WorkflowSelectors.RunHistoryFilterIcon, null).then(() => {
        cy.get(WorkflowSelectors.RunHistoryAddFilter).click();
        cy.ClickCheckBox(WorkflowSelectors.FilterCreateDateCheckBox).then(() => {
            cy.Click(WorkflowSelectors.CustomDatePicker, null).then(() => {
                cy.get(WorkflowSelectors.DatePickertodayDate).click();
            })
        });
    });
}

export function AssertFilterInstancesBycurrentdate() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockWorkflowInstances, 200);
}

export function SearchInstanceByBusinessKey() {
    cy.fixture(WorkflowRunHistoryFixturePath.MockWorkflowSingleInstace).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetMockWorkflowSingleInstance, response);
        SearchInstanceBusinessKey = response.Result[0].BusinessKey;
    });
    cy.FillLogTextBox(WorkflowSelectors.RunHistorySearchBox, InstanceBusinessKey)
}

export function AssertSearchInstanceByBusinessKey() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockWorkflowSingleInstance, 200)
    BaseAssertion.AssertElementContain(WorkflowSelectors.FirstWorkflowInstanceBusinessKey, SearchInstanceBusinessKey)
}

export function OpenSingleInstanceActivityList() {
    cy.fixture(WorkflowRunHistoryFixturePath.MockWorkflowInstaces).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.Getworkflowinstance, RequestAliases.GetMockWorkflowInstances, response);
    });
    cy.fixture(WorkflowRunHistoryFixturePath.MockSingleInstaceActivityList).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.GetSingleInstanceActivityList, RequestAliases.GetMockSingleInstanceActivityList, response);
    });
    cy.fixture(WorkflowRunHistoryFixturePath.MockSingleInstaceVariables).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.GetSingleInstanceVariables, RequestAliases.GetMockSingleInstanceVariables, response);
    });
    cy.Click(WorkflowSelectors.FirstWorkflowInstanceBusinessKey + BaseSelectors.FirstElement, null,true)
}

export function AssertOpenSingleInstanceActivityList() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockSingleInstanceActivityList, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockSingleInstanceVariables, 200);

}

export function RefreshSingleInstanceActivityList() {
    cy.fixture(WorkflowRunHistoryFixturePath.MockSingleInstaceActivityList).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.GetSingleInstanceActivityList, RequestAliases.GetMockSingleInstanceActivityList, response);
    });
    cy.fixture(WorkflowRunHistoryFixturePath.MockSingleInstaceVariables).then(response => {
        cy.DefineMockRequestWait(RestAPI.GET, URLs.GetSingleInstanceVariables, RequestAliases.GetMockSingleInstanceVariables, response);
    });
    cy.Click(WorkflowSelectors.SingleInstanceActivityListRefreshButton, null)
}

export function AssertRefreshSingleInstanceActivityList() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockSingleInstanceActivityList, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.GetMockSingleInstanceVariables, 200);
}

/// decision 
export function FillDecisionElementDetails(decisionElementDetails: DecisionElementDetails) {
    AddDecisionElement();
    cy.FillLogTextBox(WorkflowSelectors.DecisionElementName, decisionElementDetails.Title);
    cy.FillLogTextBox(WorkflowSelectors.DecisionMetLabel, decisionElementDetails.MetLabel);
    cy.FillLogTextBox(WorkflowSelectors.DecisionOtherwiseLabel, decisionElementDetails.OtherwiseLabel);
    ConditionCounter = 1;
}

function AddDecisionElement() {
    cy.Click(WorkflowSelectors.FirstConnectorButton, null);
    cy.Click(WorkflowSelectors.AddDecisionNode, null);
}

export function FillDecisionRootConditionsDetails(groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetObjectFieldViews, RequestAliases.GetObjectFieldViews);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowRootOperation, groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, true, (ConditionCounter + conditionDetailsList.length), true);
    ConditionGroupButton = ConditionCounter;
}

export function CloseEditNodeWindow() {
    cy.Click(WorkflowSelectors.WorkflowEditOkButton, null);
}

export function SaveDecisionWorkflow() {
    CloseEditNodeWindow();
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowVersionRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowSaveDraft, null);
}

function OpenEditDecisionElement(index: number) {
    cy.Click(WorkflowSelectors.WorkflowDecisionElement(index), null);
    ClickEditElementButton(WorkflowSelectors.NodeSettingContainer);
}

function ClickEditElementButton(Selector: string) {
    cy.get(Selector).find(WorkflowSelectors.WorkflowEditElementButton).click();
}

export function FillDecisionGroupConditionDetails(IsRootGroup: boolean, groupCondition: string, conditionDetailsList: ConditionDetails[]) {
    OpenEditDecisionElement(1);
    if (!IsRootGroup) {
        cy.Click(WorkflowSelectors.WorkflowRootGroupCondition, null);
    }
    else {
        cy.Click(WorkflowSelectors.WorkflowGroupButton(ConditionGroupButton - conditionDetailsList.length), null)
    }
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowGroupOperation(ConditionCounter), groupCondition, 0);
    FillConditionsGroup(conditionDetailsList, false, ((ConditionCounter + conditionDetailsList.length)), true);
    ConditionGroupButton = ConditionCounter;
}

function FillConditionFieldName(IsList: boolean, index: number, FieldName: string) {
    if (IsList) {
        cy.get(WorkflowSelectors.WorkflowConditionFieldFromList(index)).find(BaseSelectors.input).click().type(FieldName).then(() => {
            cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains(FieldName).eq(0).click()
        });
    } else {
        cy.SelectDropDownListItemUsingSearch(WorkflowSelectors.WorkflowConditionField(index), FieldName);
        BaseAssertion.AssertStatusCode(RequestAliases.GetObjectFieldViews, 200);
    }
}

//Variables
function addDeclareVaraible(index: number) {
    cy.get(WorkflowSelectors.WorkflowConnector(index)).find('img').click().then(() => {
        cy.Click(WorkflowSelectors.WorkflowAddDeclareVariableNode, null)
    })
}

export function FillPrimitiveVariableDetails(primitivevariableDetails: PrimitiveVariableDetails, index: number) {
    addDeclareVaraible(index);
    let VariableName = primitivevariableDetails.Name.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : primitivevariableDetails.Name;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowDeclareVariableName, VariableName);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowDeclareVariableDataType, primitivevariableDetails.DataType, 0);
    let VariableDefaultValue = primitivevariableDetails.DefaultValue.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : null;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowDeclareVariabledefaultValue, VariableDefaultValue);
}

export function FillRecordVariableDetails(recordvariableDetails: RecordVariableDetails, index: number) {
    addDeclareVaraible(index);
    let VariableName = recordvariableDetails.Name.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : recordvariableDetails.Name;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowDeclareVariableName, VariableName);
    cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowDeclareVariableDataType, recordvariableDetails.DataType, 0);
    cy.get(WorkflowSelectors.WorkflowRecordVariableObject).find(BaseSelectors.input).click().type(recordvariableDetails.Object).then(() => {
        cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains(recordvariableDetails.Object).eq(0).click()
    });
}

export function CloseEditDeclareVariableNodeWindow() {
    cy.Click(WorkflowSelectors.WorkflowEditOkButton, null);
}

export function SaveDeclareVariableWorkflow() {
    CloseEditNodeWindow();
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowVersionRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowSaveDraft, null);
}

function addAssignmentNode(index: number) {
    cy.get(WorkflowSelectors.WorkflowConnector(index)).find('img').click().then(() => {
        cy.Click(WorkflowSelectors.WorkflowAddAssignmentNode, null)
    })
}

export function FillAssignmentDetails(assignmentDetails: AssignmentDetails, index: number) {
    addAssignmentNode(index);
    cy.FillLogTextBox(WorkflowSelectors.WorkflowAssignmentName, assignmentDetails.Name);
}

export function FillAssignVariablesDetails(assignVariablesDetails: AssignVariablesDetails[], matchedType: boolean) {
    for (let i = 0; i < assignVariablesDetails.length; i++) {
        fillVariableNameAssignment(i + 1, assignVariablesDetails[i].VariableName);
        cy.SelectDefinedComboDropDownListItem(WorkflowSelectors.WorkflowAssignFieldOperation(i + 1), assignVariablesDetails[i].Operation, 0)
        fillVariableValueAssignment(i + 1, assignVariablesDetails[i].FromList, assignVariablesDetails[i].Value, matchedType)
        if (i < (assignVariablesDetails.length - 1)) {
            cy.Click(WorkflowSelectors.AddNewAssignVariable, null)
        }
    }
}

function fillVariableValueAssignment(index: number, isListValue: string, variableVaue: string, matchedType: boolean) {
    if (isListValue == 'True') {
        fillVariableValueListAssignment(index, variableVaue, matchedType)
    }
    else {
        cy.get(WorkflowSelectors.WorkflowAssignFieldValueInput(index)).type(variableVaue)
    }
}

function fillVariableValueListAssignment(index: number, VaraibleValue: string, matchedType) {
    if (matchedType) {
        cy.get(WorkflowSelectors.WorkflowAssignFieldValue(index)).find(BaseSelectors.input).click().type(VaraibleValue).then(() => {
            cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains(VaraibleValue).eq(0).click()
        });
    }
    else {
        cy.get(WorkflowSelectors.WorkflowAssignFieldValue(index)).find(BaseSelectors.input).click().type(VaraibleValue)
    }
}

function fillVariableNameAssignment(index: number, VaraibleName: string) {
    cy.get(WorkflowSelectors.WorkflowAssignFieldName(index)).find(BaseSelectors.input).click().type(VaraibleName).then(() => {
        cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains(VaraibleName).eq(0).click()
    });
}

export function AssertFieldDoseNotExists() {
    BaseAssertion.AssertElementContain('.ant-select-tree-dropdown', 'No Results Found')
}

//GetEditable
function addGetEditableRecord(index: number) {
    cy.get(WorkflowSelectors.WorkflowConnector(index)).find('img').click().then(() => {
        cy.Click(WorkflowSelectors.WorkflowAddGetEditableRecordNode, null)
    })
}

export function FilEditableRecordDetails(getEditablRecordDetails: GetEditableDetails, index: number) {
    addGetEditableRecord(index);
    let VariableName = getEditablRecordDetails.Name.toLocaleLowerCase() == "random" ?
        GenerateRandoms.GenerateRandomString(5, true) : getEditablRecordDetails.Name;
    cy.FillLogTextBox(WorkflowSelectors.WorkflowEditableRecordName, VariableName);
    cy.ClickRadio(WorkflowSelectors.WorkflowGetRecordType);
    fillGetRecordObject(getEditablRecordDetails.Object)
    fillEditableRecordIDValue(getEditablRecordDetails.FilterRecord)
}

function fillGetRecordObject(recordObject: string) {
    cy.get(WorkflowSelectors.WorkflowGetRecordObject).find(BaseSelectors.input).click().type(recordObject).then(() => {
        cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains(recordObject).eq(0).click()
    });
}

function fillEditableRecordIDValue(filterRecord: string) {
    if(filterRecord == 'From Trigger record') {
        cy.get(WorkflowSelectors.WorkflowEditableReordIDValue).find(BaseSelectors.input).click().type('Id').then(() => {
            cy.get(WorkflowSelectors.WorkflowfieldsListTitle).contains('Id').eq(0).click()
        });  
    }
}

export function SaveGetEditableRecordWorkflow() {
    CloseEditNodeWindow();
    cy.DefineRequestWait(RestAPI.PUT, URLs.WorkflowVersionRequest, RequestAliases.PutWorkflowFlowBuilder);
    cy.Click(WorkflowSelectors.WorkflowSaveDraft, null);
}

function FillConditionValue(selector: string, value: string, condition: string) {
    switch (condition) {
        case "Main Carriage Final ATA":
            return cy.FillDate("input" + selector, value);
        case "Profit Differences":
            return cy.SelectDropDownListItemUsingSearch(selector, value);
        case "Containers Numbers":
            return cy.FillLogTextBox(selector, value);
        case "Agent":
            return cy.FillLogTextBox(selector, value);
        case "Description of Goods":
            return cy.FillLogTextBox(selector, value);
        case "Create Date":
            return cy.FillDate("input" + selector, value);
        case "Chargeable Weight":
            return cy.FillLogTextBox(selector, value);
        case "Customer":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
        case "Department":
            return cy.FillLogTextBox(selector, value);
        case "Order Gross Weight":
            return cy.FillLogTextBox(selector, value);
        case "Accounting Closed":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
        case "Notes":
            return cy.FillLogTextBox(selector, value);
        case "Main Carriage Transport Mode":
            return cy.SelectDropDownListItemUsingSearch(selector, value);
        case "Incoterm":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
        case "Main Carriage ATA":
            return cy.SelectDefinedComboDropDownListItem(selector, value, 0);
    }
}
