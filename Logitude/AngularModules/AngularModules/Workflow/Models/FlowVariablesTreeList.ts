import { TreeSelectItem } from "./TreeSelectItem";

export class FlowVariablesTreeList {
    private FlowObject: any;
    private CurrentNodeId: string;
    public Items: TreeSelectItem[] = [];

    constructor(flowObject: any, currentNodeId: string) {
        this.initialize(flowObject, currentNodeId);
        this.setVariablesTreeItems();
    }

    private initialize(flowObject: any, currentNodeId: string) {
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
    }

    private setVariablesTreeItems() {
        let recordsVariablesItemChildren = this.getRecordsVariablesItemChildren();
        let declaredVariablesItemChildren = this.getDeclaredVariablesItemChildren();

        let recordsVariablesItem = new TreeSelectItem("records_variables", "Records Variables", false, false, true, true, recordsVariablesItemChildren);
        let declaredVariablesItem = new TreeSelectItem("declared_variables", "Declared Variables", false, false, true, true, declaredVariablesItemChildren);

        this.Items.push(recordsVariablesItem);
        this.Items.push(declaredVariablesItem);
    }

    private getRecordsVariablesItemChildren() {
        let recordsVariablesItemChildren = [
            new TreeSelectItem("triggering_record", "Triggering record", false, false, false, false, [])
        ];

        this.getGetRecordNodesWithFirstRecordOption().forEach((node: any) => {
            recordsVariablesItemChildren
                .push(new TreeSelectItem(node.id, node.data["name"], false, false, false, false, []));
        });

        return recordsVariablesItemChildren;
    }

    private getDeclaredVariablesItemChildren() {
        let declaredVariablesItemChildren = [];

        this.getDeclareVariableNodes().forEach((node: any) => {
            declaredVariablesItemChildren
                .push(new TreeSelectItem((node.id + "." + node.data["VariableCode"]), node.data["VariableName"], true, true, false, false, []));
        });

        return declaredVariablesItemChildren;
    }

    private getGetRecordNodesWithFirstRecordOption() {
        if (this.FlowObject && this.FlowObject.nodes) {
            return this.FlowObject.nodes
                .filter((n: any) => n.type === "getRecordNode" && n.data["recordsLimit"] === "FirstRecord" && n.id !== this.CurrentNodeId);
        }
        return [];
    }

    private getDeclareVariableNodes() {
        if (this.FlowObject && this.FlowObject.nodes) {
            return this.FlowObject.nodes
                .filter((n: any) => n.type === "declareVariableNode");
        }
        return [];
    }
}