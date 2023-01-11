import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { TreeSelectItem } from "./TreeSelectItem";

export class DeclaredRecordsTreeList {
    public Items: TreeSelectItem[] = [];
    private FlowObject: any;
    private CurrentNodeId: string;

    constructor(flowObject: any, currentNodeId: string) {
        this.initialize(flowObject, currentNodeId);
        this.initializeTreeItems();
    }

    private initialize(flowObject: any, currentNodeId: string) {
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
    }

    private initializeTreeItems() {
        this.initializeRecordsItems();
    }

    private initializeRecordsItems() {
        this.getDeclareRecordVariableNodes().forEach((recordDeclareVariableNode: any) => {
            let name = recordDeclareVariableNode.data["name"] || null;
            let label = recordDeclareVariableNode.data["label"] || null;
            let recordType = recordDeclareVariableNode.data["recordType"] || null;
            let treeSelectItemKey = Formatter.getCodeFromName(name);
            let treeSelectItemData = { type: Formatter.getEntity(recordType) };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, label, false, true, false, false, [], treeSelectItemData);
            this.Items.push(treeSelectItem);
        });
    }

    private getDeclareRecordVariableNodes() {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "declareVariableNode")
                .filter((n: any) => n.data["variableType"] && n.data["variableType"] === FieldTypes.Record);
        }
        return [];
    }
}