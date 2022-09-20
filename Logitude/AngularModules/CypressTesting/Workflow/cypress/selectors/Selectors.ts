
export class WorkflowSelectors {
    public static readonly AutomationsTab = '#GeneralMHAutomations';
    public static readonly WorkflowsBox = "div[data-cy^='Workflows']";

    public static WorkFlowFlowRow = "div[class^='Row']"
    public static readonly FlowBuilderEditButton = '#Edit';
    public static readonly WorkflowStartNode = "div[data-selector^='start-node-1']";
    public static readonly WorkflowEditNodeButton = "button[class^='edit-button']";
    public static readonly WorkflowStartNodeObject = "input[data-cy^='start-entity']";
    public static FlowTriggerRadioButton(trigger: string): string {
        return "input[id^='"+ trigger.replace(/\s/g, '') + "_TriggerRadio']";
    }
    public static readonly WorkflowStartOkButton = "button[data-cy^='start-properties-save-button']";
    public static readonly WorkflowName = "#WorkFlow_Name";
    public static readonly NewWorkflow = "#NewButton_WorkFlow";
    public static readonly WorkflowCreateButton = "button[data-cy^='workflow-save-button'";
    public static readonly WorkflowSaveButton = "button[class^='EntityChangesButton']";
    public static readonly WorkflowStartEditButton = ".edit-button";
    public static readonly WorkflowFirstAddCondition = "[data-cy^='AddConditionButton']";
    public static readonly WorkflowAddRootCondition = "[data-cy^='AddRootConditionButton']";
    public static readonly WorkflowRootOperation = "[data-cy^='ConditionsOperation']";

    public static WorkflowConditionField(index: number): string {
        return "input[data-cy^='ConditionField_"+ index.toString()+ "']";
    }

    public static WorkflowConditionOperation(index: number): string {
        return "div[data-cy^='ConditionOperator_"+ index.toString()+ "']";
    }

    public static WorkflowConditionValue(index: number): string {
        return "input[data-cy^='ConditionValue_"+ index.toString()+ "']";
    }

    public static readonly WorkflowRootGroupCondition = "span[data-cy^='AddRootGroupButton']";

    public static WorkflowAddGroupCondition(index: number): string {
        return "div[data-cy^='ConditionValue_"+ index.toString()+ "']";
    }

    public static WorkflowAddConditionButton(index: number): string {
        return "span[data-cy^='AddConditionButton_"+ index.toString()+ "']";
    }

    public static WorkflowGroupCondition(index: number): string {
        return "span[data-cy^='AddConditionButton_"+ index.toString()+ "']";
    }

    public static WorkflowGroupOperation(index: number): string {
        return "div[data-cy^='ConditionOperation_"+ index.toString()+ "']";
    }


}
