import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { TreeSelectItem } from "./TreeSelectItem";

type ChildEntity = { Code: string, Name: string, ParentEntityCode: string };

export class CollectionFilterEntitiesTreeList {
    public Items: TreeSelectItem[] = [];
    private FlowObject: any;
    private CurrentNodeId: string;

    private ChildEntities: ChildEntity[] = [
        { Code: "Container", Name: "Container", ParentEntityCode: "Shipment" },
        { Code: "ShipmentPackage", Name: "Package", ParentEntityCode: "Shipment" },
        { Code: "ShipmentReceivable", Name: "Receivable", ParentEntityCode: "Shipment" },
        { Code: "ShipmentPayable", Name: "Payable", ParentEntityCode: "Shipment" }
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
        
        this.buildTriggerRecordTreeSelectItem();

        this.getGetRecordNodes("FirstRecord").forEach((getRecordNode: any) => {
            let entity = getRecordNode.data["entity"];
            let treeSelectItemName = getRecordNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let childrenItems = this.getChildrenItems(treeSelectItemKey, entity)
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, false, false, false, false, childrenItems);
            this.Items.push(treeSelectItem);
        });
    }

    private buildTriggerRecordTreeSelectItem(){
        let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
        let triggeringRecordchildrenItem = this.getChildrenItems("triggeringrecord", triggeringRecordEntity)
        let triggeringrecordItem = new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, triggeringRecordchildrenItem)
        this.Items.push(triggeringrecordItem);
    }

    private getChildrenItems(treeItemPrefix: string, entityCode: string) {
        let childrenItems = [];
        let childerItems = this.ChildEntities.filter(c => c.ParentEntityCode === entityCode)
        childerItems.forEach(childEntity => {
            let childrenItem = new TreeSelectItem(treeItemPrefix + "_" + childEntity.Code, childEntity.Name, true, true, false, false, [], {entity:childEntity.Code});
            childrenItems.push(childrenItem);
        });
        return childrenItems;
    }

    private getGetRecordNodes(recordsLimit: "FirstRecord" | "AllRecords") {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === recordsLimit
                    && n.data["entity"] && n.data["entity"].indexOf(".") === -1);
        }
        return [];
    }
}
