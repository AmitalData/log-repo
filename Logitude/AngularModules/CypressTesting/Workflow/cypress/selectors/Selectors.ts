export class WorkflowSelectors {
    public static readonly AutomationsTab = '#GeneralMHAutomations';
    public static readonly WorkflowsBox = "div[data-cy^='Workflows']";

    public static WorkFlowFlowRow = "[class^='Row']"
    public static readonly FlowBuilderEditButton = '#Edit';
    public static readonly WorkflowStartNode = "div[data-selector^='start-node-1']";
    public static readonly WorkflowEditNodeButton = "button[class^='edit-button']";
    public static readonly WorkflowStartNodeObject = "[data-cy^='TriggerEntity']";
    public static FlowTriggerRadioButton(trigger: string): string {
        return "input[id^='" + trigger.replace(/\s/g, '') + "_TriggerRadio']";
    }
    public static readonly WorkflowStartOkButton = "[data-cy^='SubmitButton']";
    public static readonly WorkflowName = "#WorkFlow_Name";
    public static readonly NewWorkflow = "#NewButton_WorkFlow";
    public static readonly WorkflowCreateButton = "button[data-cy^='workflow-save-button'";
    public static readonly WorkflowSaveButton = "button[data-cy^='save-workflow-changes-button']";
    public static readonly WorkflowStartEditButton = ".edit-container";
    public static readonly WorkflowAddCondition = ".link-button";
    public static readonly FlowNameInFlowBuilder = ".TabTitleRow";
    public static readonly BackToWorkflowListButton = ".BackBottun";
    public static readonly WorkflowSearchBox = "searchtextbox > input";
    public static readonly WorkflowNameInListDiv = "div[class^='row-cell']";
    public static readonly WorkflowListRefreshButton = "#Refresh";
    public static readonly WorkflowListExcelExport = "img[title^='Export To Excel']";
    public static readonly WorkflowLinkButton = "span[class^='linkbtn']";
    public static readonly WorkflowGeneralTab = "#WorkFlowTHGeneral";
    public static readonly FlowRunHistory = "#WorkFlowTHRunHistory";
    public static readonly WorkflowGeneralSaveButton = "[data-cy^='EditWorkFlow_Save']";
    public static readonly WorkflowGeneralBackButton = "#BackButton";
    public static readonly WorkflowFirstAddCondition = "[data-cy^='AddConditionButton']";
    public static readonly WorkflowAddRootCondition = "[data-cy^='AddRootConditionButton']";
    public static readonly WorkflowRootOperation = "[data-cy^='ConditionsOperation']";
    public static readonly WorkflowDescription = "#WorkFlow_Description";
    public static readonly WorkflowOwner = "#WorkFlow_OwnerId";
    public static readonly RunHistoryRefreshButton = "[data-cy^='RefreshWorkFlowInstance']";
    public static readonly RunHistorySearchBox = "[data-cy^='SearchWorkFlowInstance']";
    public static readonly FirstWorkflowInstanceBusinessKey = "[data-cy^='WorkFlowInstance_0_col_0']";
    public static readonly SingleInstanceActivityListRefreshButton = "[data-cy^='RefreshDetailsButton']";
    public static readonly FirstConnectorButton = "[data-selector^='connector-node-1']";
    public static readonly DecisionElementName = "[data-cy^='condition-title']";
    public static readonly DecisionMetLabel = "[data-cy^='met-label']";
    public static readonly DecisionOtherwiseLabel = "[data-cy^='otherwise-label']";
    public static readonly WorkflowDecisionOkButton = "[data-cy^='SubmitButton']";
    public static readonly NodeSettingContainer = ".right-node-template-container";
    public static readonly AddDecisionNode = ".add-condition-node";
    public static readonly WorkflowEditElementButton = ".edit-label";
    public static readonly WorkflowfieldsListTitle = "nz-tree-node-title";
    public static readonly RunHistoryFilterIcon = "[data-cy^='WorkFlowInstance_AddFilterIcon']";
    public static readonly RunHistoryExportFile = "[data-cy^='ExportWorkFlowInstance']";
    public static readonly DatePickerList = "[data-cy^='RunHistoryDatePickerdropDown']";
    public static readonly RunHistoryAddFilter = "[data-selector^='WorkFlowInstance_AddFilterBtn']";
    public static readonly WorkflowSaveDraft = "[data-cy^='EditWorkFlow_Save']";
    public static readonly WorkflowRunHistory = "#WorkFlowTHRunHistory";
    public static readonly WorkflowEditOkButton = "[data-cy^='SubmitButton']";
    public static readonly WorkflowAddDeclareVariableNode = ".add-declare-variable-node";
    public static readonly WorkflowDeclareVariableName = "[data-cy^='variable-name']";
    public static readonly WorkflowDeclareVariableDataType = ".input-container";
    public static readonly WorkflowDeclareVariabledefaultValue = "[data-cy^='defaultValue']";
    public static readonly WorkflowRecordVariableObject = ".ant-tree-select";
    public static readonly WorkflowAddAssignmentNode = ".add-set-value-node";
    public static readonly WorkflowAssignmentName = "[data-cy^='name']";
    // public static readonly AssignmentVariableName = ".ant-tree-select";
    public static readonly AssignmentVariableOperation = '.set-value-operator-row'
    // public static readonly AssignmentVariableValue = ".set-value-value-container";
    public static readonly AssignmentValueInput = "[data-cy^='setValue_']";
    public static readonly AddNewAssignVariable = "[data-cy^='AddSetValue']";
    public static readonly FilterStartDateCheckBox = "[data-cy^='CheckBox_WorkFlowInstance.F.StartTime']";
    public static readonly FilterCreateDateCheckBox = "[data-cy^='CheckBox_WorkFlowInstance.F.CreateDate']";
    //getEditable Record
    public static readonly WorkflowAddGetEditableRecordNode = ".add-get-record-node";
    public static readonly WorkflowEditableRecordName = "[data-cy^='record-name']";
    public static readonly WorkflowGetRecordType = "#Editablerecords_RecordsTypeRadio";

    //
    public static readonly CustomDatePicker = "customdatepicker";
    public static readonly DatePickertodayDate = "[data-cy^='TodayDateItem']";
    public static readonly WorkflowGetRecordObject = ".ant-tree-select";
    public static readonly WorkflowEditableReordIDValue = "[data-cy^='ConditionFieldValue_1']";


    public static WorkflowAssignFieldName(index: number) {
        return "[data-cy^='SetValueField_" + index.toString() + "']";
    }

    public static WorkflowAssignFieldValue(index: number) {
        return "nz-tree-select[data-cy^='SetValueValue_" + index.toString() + "']";
    }

    public static WorkflowAssignFieldValueInput(index: number) {
        return "input[data-cy^='SetValueValue_" + index.toString() + "']";
    }

    public static WorkflowAssignFieldOperation(index: number) {
        return "[data-cy^='SetValueOperator_" + index.toString() + "']";
    }

    public static WorkflowConnector(index: number) {
        return "[data-selector^='connector-node-" + index.toString() + "']";
    }

    public static RunHistoryHeaderColumnSelector(SortField: string): string {
        return "[data-cy^='" + SortField + "']";
    }

    public static WorkflowConditionField(index: number): string {
        return "nz-tree-select[data-cy^='ConditionField_" + index.toString() + "']";
    }

    public static WorkflowConditionOperation(index: number): string {
        return "[data-cy^='ConditionOperator_" + index.toString() + "']";
    }

    public static WorkflowConditionValue(index: number): string {
        return "[data-cy^='ConditionValue_" + index.toString() + "']";
    }

    public static readonly WorkflowRootGroupCondition = "[data-cy^='AddRootGroupButton']";

    public static WorkflowAddGroupCondition(index: number): string {
        return "[data-cy^='ConditionValue_" + index.toString() + "']";
    }

    public static WorkflowAddConditionButton(index: number): string {
        return "[data-cy^='AddConditionButton_" + index.toString() + "']";
    }

    public static WorkflowGroupCondition(index: number): string {
        return "[data-cy^='AddConditionButton_" + index.toString() + "']";
    }

    public static WorkflowGroupOperation(index: number): string {
        return "[data-cy^='ConditionOperation_" + index.toString() + "']";
    }

    public static WorkflowGroupButton(index: number): string {
        return "[data-cy^='AddGroupButton_" + index.toString() + "']";
    }

    public static WorkflowConditionFieldFromList(index: number): string {
        return "nz-tree-select[data-cy^='ConditionField_" + index.toString() + "']";
    }

    public static WorkflowDecisionElement(index: number): string {
        return "[data-selector^='condition-node-" + index.toString() + "']";
    }
}
