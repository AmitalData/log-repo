import { FlowReader } from "./FlowReader";
import { TreeSelectItem } from "./TreeSelectItem";

type ChildEntity = { Code: string, Name: string, ParentEntityCode: string, ChildField: string };

export class CollectionFilterEntitiesTreeList {
    public Items: TreeSelectItem[] = [];
    private FlowObject: any;
    private CurrentNodeId: string;
    private ItemKeySplitter: string = "_";

    private ChildEntities: ChildEntity[] = [
        { Code: "Container", Name: "Container", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentPackage", Name: "Package", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentReceivable", Name: "Receivable", ParentEntityCode: "Shipment", ChildField: "ShipmentId" },
        { Code: "ShipmentPayable", Name: "Payable", ParentEntityCode: "Shipment", ChildField: "ShipmentId" }
    ];

    constructor(flowObject: any, currentNodeId: string) {
        this.initialize(flowObject, currentNodeId);
        this.getGetRecordItemChildren();
    }

    private initialize(flowObject: any, currentNodeId: string) {
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
    }

    private getGetRecordItemChildren() {
        let childrenItem = this.getChildrenItems("triggeringrecord")
        let triggeringrecordItem = new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, childrenItem)
        this.Items.push(triggeringrecordItem);

        this.getGetRecordNodes("FirstRecord", "shipment").forEach((getRecordNode: any) => {
            let treeSelectItemName = getRecordNode.data["name"];
            let treeSelectItemKey = this.formatFlowElementName(treeSelectItemName);
            let childrenItems = this.getChildrenItems(treeSelectItemKey)
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, false, false, false, false, childrenItems);
            this.Items.push(treeSelectItem);
        });
    }

    private getChildrenItems(parentEntityKey: string) {
        let childrenItems = [];
        this.ChildEntities.forEach(childEntity => {
            let childrenItem = new TreeSelectItem(parentEntityKey + "." + childEntity.Code, childEntity.Name, true, true, false, false, []);
            childrenItems.push(childrenItem);
        });
        return childrenItems;
    }

    private getGetRecordNodes(recordsLimit: "FirstRecord" | "AllRecords", entity: string) {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === recordsLimit && n.data["entity"].toLowerCase() === entity);
        }
        return [];
    }

    private formatFlowElementName(name: string) {
        return name ? name.replace(/\ /gi, "").replace(new RegExp(this.ItemKeySplitter, "gi"), "").toLowerCase() : "";
    }
}
