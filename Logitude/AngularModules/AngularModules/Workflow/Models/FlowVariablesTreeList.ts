import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { FlowReader } from "./FlowReader";
import { Formatter } from "./Formatter";
import { ObjectTables } from "./ObjectTables";
import { ReturnedField } from "./ReturnedField";
import { TreeSelectItem } from "./TreeSelectItem";
import { FlowVariablesTreeListProperties } from "./Types";

export class FlowVariablesTreeList {
    private FlowObjectFields: ObjectFieldList[];
    private FlowObject: any;
    private CurrentNodeId: string;
    private FlowVariablesTreeListProperties: FlowVariablesTreeListProperties;
    private ItemKeySplitter: string = "_";

    private TriggeringRecordPrefix: string = "triggeringrecord";
    private DeclaredVariablesPrefix: string = "declaredvariables";
    private DeclaredRecordVariablesPrefix: string = "declaredrecordvariables";
    private LoopCurrentItemPrefix: string = "Current item from loop ";

    public Items: TreeSelectItem[] = [];
    public ItemsList: TreeSelectItem[] = [];

    constructor(flowObjectFields: ObjectFieldList[], flowObject: any, currentNodeId: string, flowVariablesTreeListProperties: FlowVariablesTreeListProperties) {
        this.initialize(flowObjectFields, flowObject, currentNodeId, flowVariablesTreeListProperties);
        this.initializeTreeItems();
    }

    public getItem(itemKey: string) {
        if (itemKey) {
            return this.ItemsList.find((i: TreeSelectItem) => i.key === itemKey) || null;
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
        let validTypes = [FieldTypes.Text, FieldTypes.NText, FieldTypes.LookUp, FieldTypes.PickList];
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
        let fieldItemTypeToCompare = Formatter.getEntity(fieldItemType);
        let compareWithTypeToCompare = Formatter.getEntity(compareWithType);
        if (fieldItemType !== undefined && (fieldItemType === null || fieldItemTypeToCompare !== compareWithTypeToCompare)) {
            return false;
        }
        return true;
    }

    private initialize(flowObjectFields: ObjectFieldList[], flowObject: any, currentNodeId: string, flowVariablesTreeListProperties: FlowVariablesTreeListProperties) {
        this.FlowObjectFields = flowObjectFields ? flowObjectFields : [];
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
        this.FlowVariablesTreeListProperties = flowVariablesTreeListProperties || this.getDefaultFlowVariablesTreeListProperties();
    }

    private getDefaultFlowVariablesTreeListProperties() {
        return {
            ShowRecordsVariables: false,
            ShowDeclaredVariables: false,
            ShowRecordsCollectionVariables: false,
            ShowDeclaredCollectionVariables: false,
            OnlyCurrentLoopItemVariables: false,
            IsObjectVariableSelectable: false
        };
    }

    private initializeTreeItems() {
        this.initializeRecordsVariables();
        this.initializeRecordsCollectionVariables();
        this.initializeDeclaredVariables();
        this.initializeDeclaredCollectionVariables();
    }

    private initializeRecordsVariables() {
        if (this.FlowVariablesTreeListProperties && this.FlowVariablesTreeListProperties.ShowRecordsVariables) {
            let recordsVariablesItemChildren = this.getRecordsVariablesItemChildren();
            let recordsVariablesItem = new TreeSelectItem("recordsvariables", "Records Variables", false, false, true, true, recordsVariablesItemChildren);
            this.Items.push(recordsVariablesItem);
            this.ItemsList.push(recordsVariablesItem);
        }
    }

    private initializeRecordsCollectionVariables() {
        if (this.FlowVariablesTreeListProperties && this.FlowVariablesTreeListProperties.ShowRecordsCollectionVariables && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let recordsCollectionVariablesItemChildren = this.getRecordsCollectionVariablesItemChildren();
            let recordsCollectionVariablesItem = new TreeSelectItem("recordscollectionvariables", "Records Collection Variables", false, false, true, true, recordsCollectionVariablesItemChildren);
            this.Items.push(recordsCollectionVariablesItem);
            this.ItemsList.push(recordsCollectionVariablesItem);
        }
    }

    private initializeDeclaredVariables() {
        if (this.FlowVariablesTreeListProperties && this.FlowVariablesTreeListProperties.ShowDeclaredVariables) {
            let declaredVariablesItemChildren = this.getDeclaredVariablesItemChildren();
            let declaredVariablesItem = new TreeSelectItem(this.DeclaredVariablesPrefix, "Declared Variables", false, false, true, true, declaredVariablesItemChildren);
            this.Items.push(declaredVariablesItem);
            this.ItemsList.push(declaredVariablesItem);
        }
    }

    private initializeDeclaredCollectionVariables() {
        if (this.FlowVariablesTreeListProperties && this.FlowVariablesTreeListProperties.ShowDeclaredCollectionVariables && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let declaredCollectionVariablesItemChildren = this.getDeclaredCollectionVariablesItemChildren();
            let declaredCollectionVariablesItem = new TreeSelectItem("declaredcollectionvariables", "Declared Collection Variables", false, false, true, true, declaredCollectionVariablesItemChildren);
            this.Items.push(declaredCollectionVariablesItem);
            this.ItemsList.push(declaredCollectionVariablesItem);
        }
    }

    private getRecordsVariablesItemChildren() {
        let recordsVariablesItemChildren: TreeSelectItem[] = [];

        if (!this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
            let triggeringRecordItemChildren = this.getObjectFieldsItems(this.TriggeringRecordPrefix, triggeringRecordEntity, null);
            let triggeringRecordItem = new TreeSelectItem(this.TriggeringRecordPrefix, "Triggering Record", false, false, false, false, triggeringRecordItemChildren);
            recordsVariablesItemChildren.push(triggeringRecordItem);
            this.ItemsList.push(triggeringRecordItem);

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
                this.ItemsList.push(treeSelectItem);
            });
        }

        this.getLoopNodes().forEach((loopNode: any) => {
            let collectionVariable = loopNode.data["collectionVariable"] || null;
            let isCollectionFilterVariable = loopNode.data["isCollectionFilterVariable"] || false;

            let collectionNode: any;

            if (isCollectionFilterVariable) {
                collectionNode = this.getCollectionFilterNodes().find(n => Formatter.getCodeFromName(n.data["name"]) === collectionVariable);
            } else {
                collectionNode = this.getGetRecordNodes("AllRecords").find(n => Formatter.getCodeFromName(n.data["name"]) === collectionVariable);
            }

            if (collectionNode && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
                let entity = collectionNode.data["entity"];
                let returnedFields = collectionNode.data["returnedFields"];
                let returnedFieldsCodes = returnedFields ? returnedFields.map((returnedField: ReturnedField) => { return returnedField.fieldCode }) : null;
                let treeSelectItemName = loopNode.data["name"];
                let isReadOnly = isCollectionFilterVariable ? false : (collectionNode.data["recordsType"] === GetRecordTypes.ReadOnly);
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let treeSelectItemChildren = this.getObjectFieldsItems(treeSelectItemKey, entity, returnedFieldsCodes);
                let treeSelectItemLoopName = this.LoopCurrentItemPrefix + treeSelectItemName;
                let treeSelectItemSelectable = this.FlowVariablesTreeListProperties.IsObjectVariableSelectable;
                let treeSelectItemData = { isReadOnlyVariable: isReadOnly, type: (entity || null) };
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemLoopName, false, treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
                recordsVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
            }

            if (collectionNode && this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
                let entity = collectionNode.data["entity"];
                let treeSelectItemName = loopNode.data["name"];
                let isReadOnly = isCollectionFilterVariable ? false : (collectionNode.data["recordsType"] === GetRecordTypes.ReadOnly);
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let treeSelectItemLoopName = this.LoopCurrentItemPrefix + treeSelectItemName;
                let treeSelectItemData = { isReadOnlyVariable: isReadOnly, type: (entity || null) };
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemLoopName, true, true, false, false, [], treeSelectItemData);
                recordsVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
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
            this.ItemsList.push(treeSelectItem);
        });

        this.getCollectionFilterNodes().forEach((collectionFilterNode: any) => {
            let entity = collectionFilterNode.data["entity"];
            let treeSelectItemName = collectionFilterNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let itemData = { isReadOnlyVariable: false, type: (entity ? (entity + "[]") : null), isCollectionFilterVariable: true };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, [], itemData);
            recordsCollectionVariablesItemChildren.push(treeSelectItem);
            this.ItemsList.push(treeSelectItem);
        });

        return recordsCollectionVariablesItemChildren;
    }

    private getDeclaredVariablesItemChildren() {
        let declaredVariablesItemChildren: TreeSelectItem[] = [];

        if (!this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            this.getDeclareVariableNodes().forEach((declareVariableNode: any) => {
                let variableName = declareVariableNode.data["variableName"] || null;
                let variableCode = declareVariableNode.data["variableCode"] || null;
                let variableType = declareVariableNode.data["variableType"] || null;
                let recordType = declareVariableNode.data["recordType"] || null;
                let isRecordVariableType = this.isRecordVariableType(variableType);
                let treeSelectItemName = variableName;
                let treeSelectItemKey = (isRecordVariableType ? this.DeclaredRecordVariablesPrefix : this.DeclaredVariablesPrefix) + this.ItemKeySplitter + variableCode;
                let declaredVariableType = this.formatDeclaredVariableType(variableType, recordType);
                let treeSelectItemChildren = isRecordVariableType ? this.getObjectFieldsItems(treeSelectItemKey, recordType, null) : [];
                let treeSelectItemData = { type: declaredVariableType };
                let treeSelectItemSelectable = !isRecordVariableType || this.FlowVariablesTreeListProperties.IsObjectVariableSelectable;
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, !isRecordVariableType, treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
                declaredVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
            });
        }

        this.getLoopNodes(true).forEach((loopNode: any) => {
            let collectionVariable = loopNode.data["collectionVariable"] || null;
            let declareVariableNodeCode = collectionVariable ?
                collectionVariable.replace((this.DeclaredVariablesPrefix + this.ItemKeySplitter), "").replace((this.DeclaredRecordVariablesPrefix + this.ItemKeySplitter), "") :
                null;

            let declareVariableNode = this.getDeclareVariableNodes(true).find(n => Formatter.getCodeFromName(n.data["name"]) === declareVariableNodeCode);
            if (declareVariableNode) {
                let variableType = declareVariableNode.data["variableType"] || null;
                let recordType = declareVariableNode.data["recordType"] || null;
                let loopName = loopNode.data["name"] || null;
                let isRecordVariableType = this.isRecordVariableType(variableType);
                let treeSelectItemName = this.LoopCurrentItemPrefix + loopName;
                let treeSelectItemKey = (isRecordVariableType ? this.DeclaredRecordVariablesPrefix : this.DeclaredVariablesPrefix) + this.ItemKeySplitter + Formatter.getCodeFromName(treeSelectItemName);
                let declaredVariableType = this.formatDeclaredVariableType(variableType, recordType);
                let treeSelectItemChildren = isRecordVariableType ? this.getObjectFieldsItems(treeSelectItemKey, recordType, null) : [];
                let treeSelectItemData = { type: declaredVariableType ? declaredVariableType.replace("[]", "") : null };
                let treeSelectItemSelectable = !isRecordVariableType || this.FlowVariablesTreeListProperties.IsObjectVariableSelectable;
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, !isRecordVariableType, treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
                declaredVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
            }
        });

        let sortedDeclaredVariablesItemChildren = this.sortTreeSelectItems(declaredVariablesItemChildren);
        return sortedDeclaredVariablesItemChildren;
    }

    private getDeclaredCollectionVariablesItemChildren() {
        let declaredCollectionVariablesItemChildren: TreeSelectItem[] = [];

        this.getDeclareVariableNodes(true).forEach((declareVariableNode: any) => {
            let variableName = declareVariableNode.data["variableName"] || null;
            let variableCode = declareVariableNode.data["variableCode"] || null;
            let variableType = declareVariableNode.data["variableType"] || null;
            let recordType = declareVariableNode.data["recordType"] || null;
            let isRecordVariableType = this.isRecordVariableType(variableType);
            let treeSelectItemName = variableName;
            let treeSelectItemKey = (isRecordVariableType ? this.DeclaredRecordVariablesPrefix : this.DeclaredVariablesPrefix) + this.ItemKeySplitter + variableCode;
            let declaredVariableType = this.formatDeclaredVariableType(variableType, recordType);
            let treeSelectItemChildren = [];
            let treeSelectItemData = { type: declaredVariableType, isDeclaredCollectionVariable: true };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemName, true, true, false, false, treeSelectItemChildren, treeSelectItemData);
            declaredCollectionVariablesItemChildren.push(treeSelectItem);
            this.ItemsList.push(treeSelectItem);
        });

        let sortedDeclaredCollectionVariablesItemChildren = this.sortTreeSelectItems(declaredCollectionVariablesItemChildren);
        return sortedDeclaredCollectionVariablesItemChildren;
    }

    private getGetRecordNodes(recordsLimit: "FirstRecord" | "AllRecords") {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "getRecordNode")
                .filter((n: any) => n.data["recordsLimit"] === recordsLimit);
        }
        return [];
    }

    private getCollectionFilterNodes() {
        if (this.FlowObject) {
            return FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "collectionFilterNode");
        }
        return [];
    }

    private getLoopNodes(isDeclaredCollectionVariable: boolean = false) {
        if (this.FlowObject) {
            let allPreviousNodes = FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId);
            let repeaterNodes = allPreviousNodes.filter(n => n.type === "repeaterNode");
            let loopNodes = allPreviousNodes.filter(n => n.type === "loopNode" && repeaterNodes.filter(rn => rn.data["parentNodeId"] === n.id).length === 0);

            if (isDeclaredCollectionVariable) {
                return loopNodes.filter(n => n.data["isDeclaredCollectionVariable"] === true);
            }

            return loopNodes;
        }
        return [];
    }

    private getDeclareVariableNodes(isCollection: boolean = false) {
        if (this.FlowObject) {
            let declareVariableNodes = FlowReader.getAllPreviousNodes(this.FlowObject, this.CurrentNodeId, "declareVariableNode");
            if (isCollection) {
                return declareVariableNodes.filter((n: any) => n.data["variableType"] && n.data["variableType"].toString().endsWith("[]"));
            } else {
                return declareVariableNodes.filter((n: any) => n.data["variableType"] && !n.data["variableType"].toString().endsWith("[]"));
            }
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
            this.ItemsList.push(treeSelectItem);
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

    private isRecordVariableType(variableType: string) {
        if (variableType && variableType.toString().startsWith(FieldTypes.Record)) {
            return true;
        }
        return false;
    }

    private formatDeclaredVariableType(variableType: string, recordType: string) {
        return (variableType && variableType.toString().startsWith(FieldTypes.Record) && recordType) ?
            variableType.replace(FieldTypes.Record, recordType) :
            variableType;
    }
}