
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


}
