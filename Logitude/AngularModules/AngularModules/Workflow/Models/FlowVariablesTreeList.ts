import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { FlowReader } from "./FlowReader";
import { ObjectTables } from "./ObjectTables";
import { ReturnedField } from "./ReturnedField";
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

    public compareItemType(item: TreeSelectItem, compareWithType: string, compareWithLookupOrPickListType: string) {
        switch (compareWithType) {
            case FieldTypes.LookUp:
                return this.compareItemLookupType(item, compareWithLookupOrPickListType);
            case FieldTypes.PickList:
                return this.compareItemPickListType(item, compareWithLookupOrPickListType);
            case FieldTypes.Text:
            case FieldTypes.NText:
                return this.compareItemTextType(item);
            case FieldTypes.Boolean:
                return this.compareItemBooleanType(item);
            case FieldTypes.Date:
            case FieldTypes.DateTime:
                return this.compareItemDateType(item);
            case FieldTypes.BigInteger:
            case FieldTypes.Decimal:
            case FieldTypes.Double:
            case FieldTypes.Integer:
            case FieldTypes.SigDouble:
            case FieldTypes.UnsDecimal:
            case FieldTypes.UnsInteger:
                return this.compareItemNumberType(item);
            default:
                return this.compareItemDefaultType(item, compareWithType);
        }
    }

    private compareItemLookupType(item: TreeSelectItem, compareWithLookupOrPickListType: string) {
        let fieldItemType = item.data["type"];
        let fieldItemLookupType = item.data["lookupType"];
        let fieldItemCode = item.data["fieldCode"];
        if (compareWithLookupOrPickListType) {
            let lookupField = compareWithLookupOrPickListType + "." + ObjectTables.getKeyPropertyPathByName(compareWithLookupOrPickListType);
            if (fieldItemCode === lookupField) {
                return true;
            }
        }
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.LookUp)) {
            return false;
        }
        if (fieldItemLookupType !== undefined && (fieldItemLookupType === null || fieldItemLookupType !== compareWithLookupOrPickListType)) {
            return false;
        }
        return true;
    }

    private compareItemPickListType(item: TreeSelectItem, compareWithLookupOrPickListType: string) {
        let fieldItemType = item.data["type"];
        let fieldItemPickListType = item.data["picklistType"];
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.LookUp)) {
            return false;
        }
        if (fieldItemPickListType !== undefined && (fieldItemPickListType === null || fieldItemPickListType !== compareWithLookupOrPickListType)) {
            return false;
        }
        return true;
    }

    private compareItemTextType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        let validTypes = [FieldTypes.Text, FieldTypes.NText];
        if (fieldItemType !== undefined && (fieldItemType === null || validTypes.indexOf(fieldItemType) === -1)) {
            return false;
        }
        return true;
    }

    private compareItemBooleanType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.Boolean)) {
            return false;
        }
        return true;
    }

    private compareItemDateType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        let validTypes = [FieldTypes.Date, FieldTypes.DateTime];
        if (fieldItemType !== undefined && (fieldItemType === null || validTypes.indexOf(fieldItemType) === -1)) {
            return false;
        }
        return true;
    }

    private compareItemNumberType(item: TreeSelectItem) {
        let fieldItemType = item.data["type"];
        let validTypes = [FieldTypes.BigInteger, FieldTypes.Decimal, FieldTypes.Double, FieldTypes.Integer, FieldTypes.SigDouble, FieldTypes.UnsDecimal, FieldTypes.UnsInteger];
        if (fieldItemType !== undefined && (fieldItemType === null || validTypes.indexOf(fieldItemType) === -1)) {
            return false;
        }
        return true;
    }

    private compareItemDefaultType(item: TreeSelectItem, compareWithType: string) {
        let fieldItemType = item.data["type"];
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== compareWithType)) {
            return false;
        }
        return true;
    }

    private initialize(flowObjectFields: ObjectFieldPM[], flowObject: any, currentNodeId: string) {
        this.FlowObjectFields = flowObjectFields ? flowObjectFields : [];
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
        let triggeringRecordItemChildren = this.getObjectFieldsItems("triggeringrecord", triggeringRecordEntity, null);

        let recordsVariablesItemChildren: TreeSelectItem[] = [
            new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, triggeringRecordItemChildren)
        ];

        this.getGetRecordNodesWithFirstRecordOption().forEach((node: any) => {
            let entity = node.data["entity"];
            let returnedFields = node.data["returnedFields"];
            let returnedFieldsCodes = returnedFields ? returnedFields.map((returnedField: ReturnedField) => { return returnedField.fieldCode }) : [];
            let treeSelectItemName = node.data["name"];
            let treeSelectItemKey = treeSelectItemName ? treeSelectItemName.replace(/\ /gi, "").replace(new RegExp(this.ItemKeySplitter, "gi"), "").toLowerCase() : "";
            let treeSelectItemChildren = this.getObjectFieldsItems(treeSelectItemKey, entity, returnedFieldsCodes);
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, false, false, false, false, treeSelectItemChildren);
            recordsVariablesItemChildren.push(treeSelectItem);
        });

        return recordsVariablesItemChildren;
    }

    private getDeclaredVariablesItemChildren() {
        let declaredVariablesItemChildren: TreeSelectItem[] = [];

        this.getDeclareVariableNodes().forEach((node: any) => {
            let treeSelectItemName = node.data["variableName"];
            let treeSelectItemKey = "declaredvariables" + this.ItemKeySplitter + node.data["variableCode"];
            let data = {
                type: (node.data["variableType"] || null)
            };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, [], data);
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

    private getObjectFieldsItems(itemsKeyPrefix: string, entity: string, returnedFieldsCodes: string[] | null) {
        let objectFieldsItems: TreeSelectItem[] = [];

        if (returnedFieldsCodes && returnedFieldsCodes.length === 0) {
            return objectFieldsItems;
        }

        this.getObjectFields(entity, returnedFieldsCodes).forEach((objectField: ObjectFieldPM) => {
            let treeSelectItemName = objectField.FullNameTextCodeDefaultText.trim();
            let treeSelectItemKey = itemsKeyPrefix + this.ItemKeySplitter + objectField.FieldCode;
            let data = {
                type: objectField.DataTypeCode,
                lookupType: (objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null),
                picklistType: (objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null),
                fieldCode: objectField.FieldCode
            };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, [], data);
            objectFieldsItems.push(treeSelectItem);
        });

        let sortedObjectFieldsItems = this.sortTreeSelectItems(objectFieldsItems);
        return sortedObjectFieldsItems;
    }

    private getObjectFields(entity: string, returnedFieldsCodes: string[] | null) {
        let entityId = ObjectTables.getIdByName(entity);
        let objectFields = this.FlowObjectFields.filter(o => o.ObjectTableId === entityId);
        if (returnedFieldsCodes) {
            objectFields = objectFields.filter(o => returnedFieldsCodes.indexOf(o.FieldCode) !== -1);
        }
        return objectFields;
    }

    private sortTreeSelectItems(items: TreeSelectItem[]) {
        if (items) {
            let sortedItems = items.sort((leftItem: TreeSelectItem, rightItem: TreeSelectItem) => {
                let leftItemTitle = leftItem.title.toLowerCase();
                let rightItemTitle = rightItem.title.toLowerCase();
                return (leftItemTitle < rightItemTitle) ? -1 : (leftItemTitle > rightItemTitle) ? 1 : 0;
            });
            return sortedItems;
        }
        return items;
    }
}