
export class WorkflowSelectors {
    public static readonly AutomationsTab = '#GeneralMHAutomations';
    public static readonly WorkflowsBox = "div[data-cy^='Workflows']";

    public static WorkFlowSingleFlow (index: number): string {
		return "#LogGrid_0_0row" + index.toString();
	}
    public static readonly FlowBuilderEditButton = '#Edit';
}
