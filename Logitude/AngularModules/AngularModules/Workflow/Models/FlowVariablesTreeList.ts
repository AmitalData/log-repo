import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { FlowReader } from "./FlowReader";
import { ObjectTables } from "./ObjectTables";
import { TreeSelectItem } from "./TreeSelectItem";

export class FlowVariablesTreeList {
    private FlowObjectFields: ObjectFieldPM[];
    private FlowObject: any;
    private CurrentNodeId: string;
    private ItemKeySplitter: string = "_";
    public Items: TreeSelectItem[] = [];

    constructor(flowObjectFields: ObjectFieldPM[], flowObject: any, currentNodeId: string) {
        this.initialize(flowObjectFields, flowObject, currentNodeId);
        this.setVariablesTreeItems();
    }

    private initialize(flowObjectFields: ObjectFieldPM[], flowObject: any, currentNodeId: string) {
        this.FlowObjectFields = flowObjectFields;
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
    }

    private setVariablesTreeItems() {
        let recordsVariablesItemChildren = this.getRecordsVariablesItemChildren();
        let declaredVariablesItemChildren = this.getDeclaredVariablesItemChildren();

        let recordsVariablesItem = new TreeSelectItem("recordsvariables", "Records Variables", false, false, true, true, recordsVariablesItemChildren);
        let declaredVariablesItem = new TreeSelectItem("declaredvariables", "Declared Variables", false, false, true, true, declaredVariablesItemChildren);

        this.Items.push(recordsVariablesItem);
        this.Items.push(declaredVariablesItem);
    }

    private getRecordsVariablesItemChildren() {
        let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
        let triggeringRecordItemChildren = this.getObjectFieldsItems("triggeringrecord", triggeringRecordEntity);

        let recordsVariablesItemChildren = [
            new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, triggeringRecordItemChildren)
        ];

        this.getGetRecordNodesWithFirstRecordOption().forEach((node: any) => {
            let treeSelectItemName = node.data["name"];
            let treeSelectItemKey = treeSelectItemName ? treeSelectItemName.replace(/\ /gi, "").replace(new RegExp(this.ItemKeySplitter, "gi"), "").toLowerCase() : "";
            let treeSelectItemChildren = this.getObjectFieldsItems(treeSelectItemKey, node.data["entity"]);
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, false, false, false, false, treeSelectItemChildren);
            recordsVariablesItemChildren.push(treeSelectItem);
        });

        return recordsVariablesItemChildren;
    }

    private getDeclaredVariablesItemChildren() {
        let declaredVariablesItemChildren = [];

        this.getDeclareVariableNodes().forEach((node: any) => {
            let treeSelectItemName = node.data["variableName"];
            let treeSelectItemKey = "declaredvariables" + this.ItemKeySplitter + node.data["variableCode"];
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, []);
            declaredVariablesItemChildren.push(treeSelectItem);
        });

        let sortedDeclaredVariablesItemChildren = this.sortTreeSelectItems(declaredVariablesItemChildren);
        return sortedDeclaredVariablesItemChildren;
    }

    private getGetRecordNodesWithFirstRecordOption() {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === "FirstRecord");
        }
        return [];
    }

    private getDeclareVariableNodes() {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "declareVariableNode");
        }
        return [];
    }

    private getObjectFieldsItems(itemsKeyPrefix: string, entity: string) {
        let entityId = ObjectTables.getIdByName(entity);
        let objectFields = this.FlowObjectFields.filter(o => o.ObjectTableId === entityId);
        let objectFieldsItems = [];

        objectFields.forEach((objectField: ObjectFieldPM) => {
            let treeSelectItemName = objectField.FullNameTextCodeDefaultText.trim();
            let treeSelectItemKey = itemsKeyPrefix + this.ItemKeySplitter + objectField.FieldCode;
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, []);
            objectFieldsItems.push(treeSelectItem);
        });

        let sortedObjectFieldsItems = this.sortTreeSelectItems(objectFieldsItems);
        return sortedObjectFieldsItems;
    }

    private sortTreeSelectItems(items: any) {
        if (items) {
            let sortedItems = items.sort((a: any, b: any) => {
                let textA = a.title.toLowerCase();
                let textB = b.title.toLowerCase();
                return (textA < textB) ? -1 : (textA > textB) ? 1 : 0;
            });
            return sortedItems;
        }
        return items;
    }
}