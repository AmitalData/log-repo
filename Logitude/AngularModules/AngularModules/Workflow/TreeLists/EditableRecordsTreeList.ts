import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { FlowReader } from "Workflow/Utilities/FlowReader";
import { Formatter } from "Workflow/Utilities/Formatter";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

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
        let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
        let data = { entity: triggeringRecordEntity };
        let triggeringrecordItem = new TreeSelectItem("triggeringrecord", "Triggering record", true, true, false, false, [], data);
        this.Items.push(triggeringrecordItem);
    }

    private initializeRecordsItems() {
        this.getEditableGetRecordNodes().forEach((getRecordNode: any) => {
            let entity = getRecordNode.data["entity"];
            let treeSelectItemTitle = getRecordNode.data["label"] || null;
            let treeSelectItemName = getRecordNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let data = { entity: entity, nodeId: getRecordNode.id };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, true, true, false, false, [], data);
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