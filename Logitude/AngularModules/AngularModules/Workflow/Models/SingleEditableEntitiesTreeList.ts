import { GetRecordLimits } from "Workflow/Constants/GetRecordLimits";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { TreeSelectItem } from "./TreeSelectItem";

type ChildEntity = { Code: string, Name: string, ParentEntityCode: string };

export class SingleEditableEntitiesTreeList {
    private FlowObject: any;
    private CurrentNodeId: string;
    private ItemKeySplitter: string = "_";

    public Items: TreeSelectItem[] = [];

    private ChildEntities: ChildEntity[] = [
        { Code: "Container", Name: "Container", ParentEntityCode: "Shipment" },
        { Code: "ShipmentPackage", Name: "Package", ParentEntityCode: "Shipment" },
        { Code: "ShipmentReceivable", Name: "Receivable", ParentEntityCode: "Shipment" },
        { Code: "ShipmentPayable", Name: "Payable", ParentEntityCode: "Shipment" }
    ];

    constructor(flowObject: any, currentNodeId: string) {
        this.initialize(flowObject, currentNodeId);
        this.initializeTreeItems();
    }

    private initialize(flowObject: any, currentNodeId: string) {
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
    }

    private initializeTreeItems() {
        this.initializeTriggerRecordTreeSelectItem();
        this.initializeGetRecordTreeSelectItems();
    }

    private initializeTriggerRecordTreeSelectItem() {
        let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
        let triggeringRecordchildrenItem = this.getGetRecordTreeSelectItemChildren("triggeringrecord", triggeringRecordEntity);
        let triggeringrecordItem = new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, triggeringRecordchildrenItem);
        this.Items.push(triggeringrecordItem);
    }

    private initializeGetRecordTreeSelectItems() {
        this.getSingleEditableGetRecordNodes().forEach((getRecordNode: any) => {
            let entity = getRecordNode.data["entity"];
            let treeSelectItemName = getRecordNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let childrenItems = this.getGetRecordTreeSelectItemChildren(treeSelectItemKey, entity);
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, false, false, false, false, childrenItems);
            this.Items.push(treeSelectItem);
        });
    }

    private getGetRecordTreeSelectItemChildren(treeItemPrefix: string, entity: string) {
        let getRecordTreeSelectItemChildren = [];
        this.ChildEntities.filter(c => c.ParentEntityCode === entity).forEach(childEntity => {
            let itemData = { entity: childEntity.Code };
            let treeSelectItem = new TreeSelectItem(treeItemPrefix + this.ItemKeySplitter + childEntity.Code, childEntity.Name, true, true, false, false, [], itemData);
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