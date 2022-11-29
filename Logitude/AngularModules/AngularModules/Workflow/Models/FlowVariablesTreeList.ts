import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { ObjectTables } from "./ObjectTables";
import { ReturnedField } from "./ReturnedField";
import { TreeSelectItem } from "./TreeSelectItem";
import { ShowVariables } from "./Types";

export class FlowVariablesTreeList {
    private FlowObjectFields: ObjectFieldList[];
    private FlowObject: any;
    private CurrentNodeId: string;
    private ShowVariables: ShowVariables;
    private ItemKeySplitter: string = "_";
    public Items: TreeSelectItem[] = [];

    constructor(flowObjectFields: ObjectFieldList[], flowObject: any, currentNodeId: string, showVariables: ShowVariables) {
        this.initialize(flowObjectFields, flowObject, currentNodeId, showVariables);
        this.initializeTreeItems();
    }

    public getItem(itemKey: string, items: TreeSelectItem[] | null = null) {
        if (itemKey) {
            let result: TreeSelectItem | null = null;
            for (let item of (items || this.Items)) {
                if (item.key === itemKey) {
                    result = item;
                    break;
                }
                if (item.children && item.children.length > 0) {
                    result = this.getItem(itemKey, item.children);
                }
            }
            return result;
        }
        return null;
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
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemType !== FieldTypes.PickList)) {
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

    private initialize(flowObjectFields: ObjectFieldList[], flowObject: any, currentNodeId: string, showVariables: ShowVariables) {
        this.FlowObjectFields = flowObjectFields ? flowObjectFields : [];
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
        this.ShowVariables = showVariables;
    }

    private initializeTreeItems() {
        this.initializeRecordsVariables();
        this.initializeRecordsCollectionVariables();
        this.initializeDeclaredVariables();
        this.initializeDeclaredCollectionVariables();
    }

    private initializeRecordsVariables() {
        if (this.ShowVariables && this.ShowVariables.ShowRecordsVariables) {
            let recordsVariablesItemChildren = this.getRecordsVariablesItemChildren();
            let recordsVariablesItem = new TreeSelectItem("recordsvariables", "Records Variables", false, false, true, true, recordsVariablesItemChildren);
            this.Items.push(recordsVariablesItem);
        }
    }

    private initializeRecordsCollectionVariables() {
        if (this.ShowVariables && this.ShowVariables.ShowRecordsCollectionVariables) {
            let recordsCollectionVariablesItemChildren = this.getRecordsCollectionVariablesItemChildren();
            let recordsCollectionVariablesItem = new TreeSelectItem("recordscollectionvariables", "Records Collection Variables", false, false, true, true, recordsCollectionVariablesItemChildren);
            this.Items.push(recordsCollectionVariablesItem);
        }
    }

    private initializeDeclaredVariables() {
        if (this.ShowVariables && this.ShowVariables.ShowDeclaredVariables) {
            let declaredVariablesItemChildren = this.getDeclaredVariablesItemChildren();
            let declaredVariablesItem = new TreeSelectItem("declaredvariables", "Declared Variables", false, false, true, true, declaredVariablesItemChildren);
            this.Items.push(declaredVariablesItem);
        }
    }

    private initializeDeclaredCollectionVariables() {
        if (this.ShowVariables && this.ShowVariables.ShowDeclaredCollectionVariables) {
            let declaredCollectionVariablesItemChildren = [];
            let declaredCollectionVariablesItem = new TreeSelectItem("declaredcollectionvariables", "Declared Collection Variables", false, false, true, true, declaredCollectionVariablesItemChildren);
            this.Items.push(declaredCollectionVariablesItem);
        }
    }

    private getRecordsVariablesItemChildren() {
        let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
        let triggeringRecordItemChildren = this.getObjectFieldsItems("triggeringrecord", triggeringRecordEntity, null);

        let recordsVariablesItemChildren: TreeSelectItem[] = [
            new TreeSelectItem("triggeringrecord", "Triggering record", false, false, false, false, triggeringRecordItemChildren)
        ];

        this.getGetRecordNodes("FirstRecord").forEach((getRecordNode: any) => {
            let entity = getRecordNode.data["entity"];
            let returnedFields = getRecordNode.data["returnedFields"];
            let returnedFieldsCodes = returnedFields ? returnedFields.map((returnedField: ReturnedField) => { return returnedField.fieldCode }) : null;
            let treeSelectItemName = getRecordNode.data["name"];
            let isReadOnly = getRecordNode.data["recordsType"] === GetRecordTypes.ReadOnly;
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let treeSelectItemChildren = this.getObjectFieldsItems(treeSelectItemKey, entity, returnedFieldsCodes);
            let itemData = { isReadOnlyVariable: isReadOnly };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, false, false, false, false, treeSelectItemChildren, itemData);
            recordsVariablesItemChildren.push(treeSelectItem);
        });

        this.getLoopNodes().forEach((loopNode: any) => {
            let collectionVariable = loopNode.data["collectionVariable"] || null;
            let getRecordNode = this.getGetRecordNodes("AllRecords").find(n => Formatter.getCodeFromName(n.data["name"]) === collectionVariable);
            if (getRecordNode) {
                let entity = getRecordNode.data["entity"];
                let returnedFields = getRecordNode.data["returnedFields"];
                let returnedFieldsCodes = returnedFields ? returnedFields.map((returnedField: ReturnedField) => { return returnedField.fieldCode }) : null;
                let treeSelectItemName = loopNode.data["name"];
                let isReadOnly = getRecordNode.data["recordsType"] === GetRecordTypes.ReadOnly;
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let treeSelectItemChildren = this.getObjectFieldsItems(treeSelectItemKey, entity, returnedFieldsCodes);
                let treeSelectItemLoopName = "Current item from loop " + treeSelectItemName;
                let itemData = { isReadOnlyVariable: isReadOnly };
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemLoopName, false, false, false, false, treeSelectItemChildren, itemData);
                recordsVariablesItemChildren.push(treeSelectItem);
            }
        });

        return recordsVariablesItemChildren;
    }

    private getRecordsCollectionVariablesItemChildren() {
        let recordsCollectionVariablesItemChildren: TreeSelectItem[] = [];

        this.getGetRecordNodes("AllRecords").forEach((getRecordNode: any) => {
            let entity = getRecordNode.data["entity"];
            let treeSelectItemName = getRecordNode.data["name"];
            let isReadOnly = getRecordNode.data["recordsType"] === GetRecordTypes.ReadOnly;
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let itemData = { isReadOnlyVariable: isReadOnly, type: (entity ? (entity + "[]") : null) };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, [], itemData);
            recordsCollectionVariablesItemChildren.push(treeSelectItem);
        });

        return recordsCollectionVariablesItemChildren;
    }

    private getDeclaredVariablesItemChildren() {
        let declaredVariablesItemChildren: TreeSelectItem[] = [];

        this.getDeclareVariableNodes().forEach((declareVariableNode: any) => {
            let treeSelectItemName = declareVariableNode.data["variableName"];
            let treeSelectItemKey = "declaredvariables" + this.ItemKeySplitter + declareVariableNode.data["variableCode"];
            let data = {
                type: (declareVariableNode.data["variableType"] || null)
            };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, [], data);
            declaredVariablesItemChildren.push(treeSelectItem);
        });

        let sortedDeclaredVariablesItemChildren = this.sortTreeSelectItems(declaredVariablesItemChildren);
        return sortedDeclaredVariablesItemChildren;
    }

    private getGetRecordNodes(recordsLimit: "FirstRecord" | "AllRecords") {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === recordsLimit);
        }
        return [];
    }

    private getLoopNodes() {
        if (this.FlowObject) {
            let allPreviousNodes = FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId);
            let repeaterNodes = allPreviousNodes.filter(n => n.type === "repeaterNode");
            let loopNodes = allPreviousNodes.filter(n => n.type === "loopNode" && repeaterNodes.filter(rn => rn.data["parentNodeId"] === n.id).length === 0);
            return loopNodes;
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

        this.getObjectFields(entity, returnedFieldsCodes).forEach((objectField: ObjectFieldList) => {
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