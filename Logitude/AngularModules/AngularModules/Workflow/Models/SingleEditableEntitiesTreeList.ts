import { GetRecordLimits } from "Workflow/Constants/GetRecordLimits";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { Entities } from "./Entities";
import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { TreeSelectItem } from "./TreeSelectItem";

export class SingleEditableEntitiesTreeList {
    private FlowObject: any;
    private CurrentNodeId: string;
    private OnlyTriggeringRecord: boolean;
    private ItemKeySplitter: string = "_";

    public Items: TreeSelectItem[] = [];

    constructor(flowObject: any, currentNodeId: string, onlyTriggeringRecord: boolean = false) {
        this.initialize(flowObject, currentNodeId, onlyTriggeringRecord);
        this.initializeTreeItems();
    }

    private initialize(flowObject: any, currentNodeId: string, onlyTriggeringRecord: boolean) {
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
        this.OnlyTriggeringRecord = onlyTriggeringRecord;
    }

    private initializeTreeItems() {
        this.initializeTriggerRecordTreeSelectItem();
        if (!this.OnlyTriggeringRecord) {
            this.initializeGetRecordTreeSelectItems();
        }
    }

    private initializeTriggerRecordTreeSelectItem() {
        let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
        let triggeringRecordchildrenItem = this.getGetRecordTreeSelectItemChildren("triggeringrecord", triggeringRecordEntity, null);
        let triggeringrecordItem = new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, triggeringRecordchildrenItem);
        this.Items.push(triggeringrecordItem);
    }

    private initializeGetRecordTreeSelectItems() {
        this.getSingleEditableGetRecordNodes().forEach((getRecordNode: any) => {
            let entity = getRecordNode.data["entity"];
            let treeSelectItemTitle = getRecordNode.data["label"] || null;
            let treeSelectItemName = getRecordNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let childrenItems = this.getGetRecordTreeSelectItemChildren(treeSelectItemKey, entity, getRecordNode.id);
            let data = { nodeId: getRecordNode.id };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, false, false, false, false, childrenItems, data);
            this.Items.push(treeSelectItem);
        });
    }

    private getGetRecordTreeSelectItemChildren(treeItemPrefix: string, entity: string, nodeId: string | null) {
        let getRecordTreeSelectItemChildren = [];
        Entities.Children.filter(c => c.Code !== "ARInvoice" && c.Code !== "APInvoice").filter(c => c.ParentEntityCode === entity)
            .forEach(childEntity => {
                let itemData = { entity: childEntity.Code, nodeId: nodeId };
                let treeSelectItemKey = treeItemPrefix + this.ItemKeySplitter + childEntity.Code;
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, childEntity.Name, true, true, false, false, [], itemData);
                getRecordTreeSelectItemChildren.push(treeSelectItem);
            });
        return getRecordTreeSelectItemChildren;
    }

    private getSingleEditableGetRecordNodes() {
        if (this.FlowObject) {
            let getRecordNodes = FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === GetRecordLimits.FirstRecord && n.data["recordsType"] === GetRecordTypes.Editable
                    && n.data["entity"] && n.data["entity"].indexOf(".") === -1);

            return getRecordNodes;
        }
        return [];
    }
}