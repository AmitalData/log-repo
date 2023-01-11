import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { TreeSelectItem } from "./TreeSelectItem";

export class EditableRecordsTreeList {
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
        this.initializeTriggeringRecordItem();
        this.initializeRecordsItems();
    }

    private initializeTriggeringRecordItem() {
        let triggeringrecordItem = new TreeSelectItem("triggeringrecord", "Triggering record", true, true, false, false, []);
        this.Items.push(triggeringrecordItem);
    }

    private initializeRecordsItems() {
        this.getEditableGetRecordNodes().forEach((getRecordNode: any) => {
            let treeSelectItemName = getRecordNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, []);
            this.Items.push(treeSelectItem);
        });
    }

    private getEditableGetRecordNodes() {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === "FirstRecord" && n.data["recordsType"] === GetRecordTypes.Editable);
        }
        return [];
    }
}