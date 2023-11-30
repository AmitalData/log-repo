import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { GetRecordTypes } from "Workflow/Constants/GetRecordTypes";
import { FlowReader } from "Workflow/Utilities/FlowReader";
import { Formatter } from "Workflow/Utilities/Formatter";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { ReturnedField } from "Workflow/Models/ReturnedField";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { FlowVariablesTreeListProperties, GlobalVariable } from "Workflow/Types";

export class FlowVariablesTreeList {
    private FlowObject: any;
    private CurrentNodeId: string;
    private FlowVariablesTreeListProperties: FlowVariablesTreeListProperties;
    private ItemKeySplitter: string = "_";

    private RecordsVariablesPrefix: string = "recordsvariables";
    private RecordsCollectionVariablesPrefix: string = "recordscollectionvariables";
    private TriggeringRecordPrefix: string = "triggeringrecord";
    private DeclaredVariablesPrefix: string = "declaredvariables";
    private DeclaredRecordVariablesPrefix: string = "declaredrecordvariables";
    private DeclaredCollectionVariablesPrefix: string = "declaredcollectionvariables";
    private GlobalVariablesPrefix: string = "globalvariables";
    private LoopCurrentItemPrefix: string = "Current item from loop ";

    public Items: TreeSelectItem[] = [];
    public ItemsList: TreeSelectItem[] = [];

    constructor(flowObject: any, currentNodeId: string, flowVariablesTreeListProperties: FlowVariablesTreeListProperties) {
        this.initialize(flowObject, currentNodeId, flowVariablesTreeListProperties);
        this.initializeTreeItems();
    }

    public getItem(itemKey: string) {
        if (itemKey) {
            return this.ItemsList.find((i: TreeSelectItem) => i.key === itemKey) || null;
        }
        return null;
    }

    private initialize(flowObject: any, currentNodeId: string, flowVariablesTreeListProperties: FlowVariablesTreeListProperties) {
        this.FlowObject = flowObject;
        this.CurrentNodeId = currentNodeId;
        this.FlowVariablesTreeListProperties = this.handleFlowVariablesTreeListProperties(flowVariablesTreeListProperties);
    }

    private handleFlowVariablesTreeListProperties(flowVariablesTreeListProperties: FlowVariablesTreeListProperties) {
        let properties: FlowVariablesTreeListProperties = {
            ShowRecordsVariables: flowVariablesTreeListProperties?.ShowRecordsVariables || false,
            ShowDeclaredVariables: flowVariablesTreeListProperties?.ShowDeclaredVariables || false,
            ShowRecordsCollectionVariables: flowVariablesTreeListProperties?.ShowRecordsCollectionVariables || false,
            ShowDeclaredCollectionVariables: flowVariablesTreeListProperties?.ShowDeclaredCollectionVariables || false,
            ShowGlobalVariables: flowVariablesTreeListProperties?.ShowGlobalVariables || false,
            OnlyCurrentLoopItemVariables: flowVariablesTreeListProperties?.OnlyCurrentLoopItemVariables || false,
            IsObjectVariableSelectable: flowVariablesTreeListProperties?.IsObjectVariableSelectable || false,
            IsNoChildrenObjectVariables: flowVariablesTreeListProperties?.IsNoChildrenObjectVariables || false
        };
        return properties;
    }

    private initializeTreeItems() {
        this.initializeRecordsVariables();
        this.initializeRecordsCollectionVariables();
        this.initializeDeclaredVariables();
        this.initializeDeclaredCollectionVariables();
        this.initializeGlobalVariables();
    }

    private initializeRecordsVariables() {
        if (this.FlowVariablesTreeListProperties.ShowRecordsVariables) {
            let recordsVariablesItemChildren = this.getRecordsVariablesItemChildren();
            let recordsVariablesItem = new TreeSelectItem(this.RecordsVariablesPrefix, "Records Variables", false, false, true, true, recordsVariablesItemChildren);
            this.Items.push(recordsVariablesItem);
            this.ItemsList.push(recordsVariablesItem);
        }
    }

    private initializeRecordsCollectionVariables() {
        if (this.FlowVariablesTreeListProperties.ShowRecordsCollectionVariables && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let recordsCollectionVariablesItemChildren = this.getRecordsCollectionVariablesItemChildren();
            let recordsCollectionVariablesItem = new TreeSelectItem(this.RecordsCollectionVariablesPrefix, "Records Collection Variables", false, false, true, true, recordsCollectionVariablesItemChildren);
            this.Items.push(recordsCollectionVariablesItem);
            this.ItemsList.push(recordsCollectionVariablesItem);
        }
    }

    private initializeDeclaredVariables() {
        if (this.FlowVariablesTreeListProperties.ShowDeclaredVariables) {
            let declaredVariablesItemChildren = this.getDeclaredVariablesItemChildren();
            let declaredVariablesItem = new TreeSelectItem(this.DeclaredVariablesPrefix, "Declared Variables", false, false, true, true, declaredVariablesItemChildren);
            this.Items.push(declaredVariablesItem);
            this.ItemsList.push(declaredVariablesItem);
        }
    }

    private initializeDeclaredCollectionVariables() {
        if (this.FlowVariablesTreeListProperties.ShowDeclaredCollectionVariables && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let declaredCollectionVariablesItemChildren = this.getDeclaredCollectionVariablesItemChildren();
            let declaredCollectionVariablesItem = new TreeSelectItem(this.DeclaredCollectionVariablesPrefix, "Declared Collection Variables", false, false, true, true, declaredCollectionVariablesItemChildren);
            this.Items.push(declaredCollectionVariablesItem);
            this.ItemsList.push(declaredCollectionVariablesItem);
        }
    }

    private initializeGlobalVariables() {
        if (this.FlowVariablesTreeListProperties.ShowGlobalVariables && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let globalVariablesItemChildren = this.getGlobalVariablesItemChildren();
            let globalVariablesItem = new TreeSelectItem(this.GlobalVariablesPrefix, "Global Variables", false, false, true, true, globalVariablesItemChildren);
            this.Items.push(globalVariablesItem);
            this.ItemsList.push(globalVariablesItem);
        }
    }

    private getRecordsVariablesItemChildren() {
        let recordsVariablesItemChildren: TreeSelectItem[] = [];

        if (!this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
            let triggeringRecordEntity = FlowReader.getStartNodeEntity(this.FlowObject);
            let triggeringRecordItemChildren = this.getObjectFieldsItems(this.TriggeringRecordPrefix, triggeringRecordEntity, null, null);
            let triggeringRecordItem = new TreeSelectItem(this.TriggeringRecordPrefix, "Triggering Record", false, false, false, false, triggeringRecordItemChildren);
            recordsVariablesItemChildren.push(triggeringRecordItem);
            this.ItemsList.push(triggeringRecordItem);

            this.getGetRecordNodes("FirstRecord").forEach((getRecordNode: any) => {
                let entity = getRecordNode.data["entity"];
                let returnedFields = getRecordNode.data["returnedFields"];
                let returnedFieldsCodes = returnedFields ? returnedFields.map((returnedField: ReturnedField) => { return returnedField.fieldCode }) : null;
                let treeSelectItemTitle = getRecordNode.data["label"] || null;
                let treeSelectItemName = getRecordNode.data["name"];
                let isReadOnly = getRecordNode.data["recordsType"] === GetRecordTypes.ReadOnly;
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let treeSelectItemChildren = this.getObjectFieldsItems(treeSelectItemKey, entity, returnedFieldsCodes, getRecordNode.id, isReadOnly);
                let itemData = { isReadOnlyVariable: isReadOnly, type: (entity || null), nodeId: getRecordNode.id };
                let selectable = this.FlowVariablesTreeListProperties.IsObjectVariableSelectable || this.FlowVariablesTreeListProperties.IsNoChildrenObjectVariables;
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, false, selectable, false, false, treeSelectItemChildren, itemData);
                recordsVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
            });
        }

        this.getLoopNodes().forEach((loopNode: any) => {
            let collectionVariable: string = loopNode.data["collectionVariable"] || null;
            let isCollectionFilterVariable: boolean = loopNode.data["isCollectionFilterVariable"] || false;
            let isEditableEntity: boolean = loopNode.data["isEditableEntity"] || false;

            let collectionNode: any;

            if (isCollectionFilterVariable) {
                collectionNode = this.getCollectionFilterNodes().find(n => Formatter.getCodeFromName(n.data["name"]) === collectionVariable);
            } else {
                collectionNode = this.getGetRecordNodes("AllRecords").find(n => Formatter.getCodeFromName(n.data["name"]) === collectionVariable);
            }

            if (!collectionNode && isEditableEntity) {
                let onlyCurrentLoopItem = this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables;
                let entity = collectionVariable?.split("_")[1];
                let treeSelectItemName = loopNode.data["name"];
                let isReadOnly = false;
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let treeSelectItemChildren = onlyCurrentLoopItem ? [] : this.getObjectFieldsItems(treeSelectItemKey, entity, null, loopNode.id, isReadOnly);
                let loopNodeLabel = loopNode.data["label"] || null;
                let treeSelectItemTitle = loopNodeLabel ? (this.LoopCurrentItemPrefix + loopNodeLabel) : null;
                let treeSelectItemLoopName = this.LoopCurrentItemPrefix + treeSelectItemName;
                let treeSelectItemSelectable = this.FlowVariablesTreeListProperties.IsObjectVariableSelectable || this.FlowVariablesTreeListProperties.IsNoChildrenObjectVariables;
                let treeSelectItemData = { isReadOnlyVariable: isReadOnly, type: (entity || null), nodeId: loopNode.id };
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemLoopName, onlyCurrentLoopItem, onlyCurrentLoopItem || treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
                recordsVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
            }

            if (collectionNode && !this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
                let isPrimitiveType = collectionNode.data["isPrimitiveType"] || false;
                let entity = collectionNode.data["entity"] || null;
                let type = collectionNode.data["type"] || null;
                let returnedFields = collectionNode.data["returnedFields"];
                let returnedFieldsCodes = returnedFields ? returnedFields.map((returnedField: ReturnedField) => { return returnedField.fieldCode }) : null;
                let treeSelectItemName = loopNode.data["name"];
                let isReadOnly = isCollectionFilterVariable ? false : (collectionNode.data["recordsType"] === GetRecordTypes.ReadOnly);
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let treeSelectItemChildren = isPrimitiveType ? [] : this.getObjectFieldsItems(treeSelectItemKey, entity, returnedFieldsCodes, loopNode.id, isReadOnly);
                let loopNodeLabel = loopNode.data["label"] || null;
                let treeSelectItemTitle = loopNodeLabel ? (this.LoopCurrentItemPrefix + loopNodeLabel) : null;
                let treeSelectItemLoopName = this.LoopCurrentItemPrefix + treeSelectItemName;
                let treeSelectItemSelectable = isPrimitiveType || this.FlowVariablesTreeListProperties.IsObjectVariableSelectable || this.FlowVariablesTreeListProperties.IsNoChildrenObjectVariables;
                let treeSelectItemData = { isReadOnlyVariable: isReadOnly, type: (isPrimitiveType ? type : entity), nodeId: loopNode.id };
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemLoopName, isPrimitiveType, treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
                recordsVariablesItemChildren.push(treeSelectItem);
                this.ItemsList.push(treeSelectItem);
            }

            if (collectionNode && this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables) {
                let isPrimitiveType = collectionNode.data["isPrimitiveType"] || false;
                let entity = collectionNode.data["entity"] || null;
                let type = collectionNode.data["type"] || null;
                let treeSelectItemName = loopNode.data["name"];
                let isReadOnly = isCollectionFilterVariable ? false : (collectionNode.data["recordsType"] === GetRecordTypes.ReadOnly);
                let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
                let loopNodeLabel = loopNode.data["label"] || null;
                let treeSelectItemTitle = loopNodeLabel ? (this.LoopCurrentItemPrefix + loopNodeLabel) : null;
                let treeSelectItemLoopName = this.LoopCurrentItemPrefix + treeSelectItemName;
                let treeSelectItemData = { isReadOnlyVariable: isReadOnly, type: (isPrimitiveType ? type : entity), nodeId: loopNode.id };
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemLoopName, true, true, false, false, [], treeSelectItemData);
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
            let treeSelectItemTitle = getRecordNode.data["label"] || null;
            let treeSelectItemName = getRecordNode.data["name"];
            let isReadOnly = getRecordNode.data["recordsType"] === GetRecordTypes.ReadOnly;
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let itemData = { isReadOnlyVariable: isReadOnly, type: (entity ? (entity + "[]") : null), nodeId: getRecordNode.id };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, true, true, false, false, [], itemData);
            recordsCollectionVariablesItemChildren.push(treeSelectItem);
            this.ItemsList.push(treeSelectItem);
        });

        this.getCollectionFilterNodes().forEach((collectionFilterNode: any) => {
            let entity = collectionFilterNode.data["entity"];
            let treeSelectItemTitle = collectionFilterNode.data["label"] || null;
            let treeSelectItemName = collectionFilterNode.data["name"];
            let treeSelectItemKey = Formatter.getCodeFromName(treeSelectItemName);
            let itemData = { isReadOnlyVariable: false, type: (entity ? (entity + "[]") : null), isCollectionFilterVariable: true, nodeId: collectionFilterNode.id };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, true, true, false, false, [], itemData);
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
                let treeSelectItemTitle = declareVariableNode.data["label"] || null;
                let treeSelectItemName = variableName;
                let treeSelectItemKey = (isRecordVariableType ? this.DeclaredRecordVariablesPrefix : this.DeclaredVariablesPrefix) + this.ItemKeySplitter + variableCode;
                let declaredVariableType = this.formatDeclaredVariableType(variableType, recordType);
                let treeSelectItemChildren = isRecordVariableType ? this.getObjectFieldsItems(treeSelectItemKey, recordType, null, declareVariableNode.id) : [];
                let treeSelectItemData = { type: declaredVariableType, nodeId: declareVariableNode.id };
                let treeSelectItemSelectable = !isRecordVariableType || this.FlowVariablesTreeListProperties.IsObjectVariableSelectable || this.FlowVariablesTreeListProperties.IsNoChildrenObjectVariables;
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, !isRecordVariableType, treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
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
                let onlyCurrentLoopItem = this.FlowVariablesTreeListProperties.OnlyCurrentLoopItemVariables;
                let variableType = declareVariableNode.data["variableType"] || null;
                let recordType = declareVariableNode.data["recordType"] || null;
                let loopName = loopNode.data["name"] || null;
                let isRecordVariableType = this.isRecordVariableType(variableType);
                let loopNodeLabel = loopNode.data["label"] || null;
                let treeSelectItemTitle = loopNodeLabel ? (this.LoopCurrentItemPrefix + loopNodeLabel) : null;
                let treeSelectItemName = this.LoopCurrentItemPrefix + loopName;
                let treeSelectItemKey = Formatter.getCodeFromName(loopName);
                let declaredVariableType = this.formatDeclaredVariableType(variableType, recordType);
                let treeSelectItemChildren = onlyCurrentLoopItem ? [] : (isRecordVariableType ? this.getObjectFieldsItems(treeSelectItemKey, recordType, null, loopNode.id) : []);
                let treeSelectItemData = { type: (declaredVariableType ? declaredVariableType.replace("[]", "") : null), nodeId: loopNode.id };
                let treeSelectItemSelectable = !isRecordVariableType || this.FlowVariablesTreeListProperties.IsObjectVariableSelectable || this.FlowVariablesTreeListProperties.IsNoChildrenObjectVariables;
                let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, onlyCurrentLoopItem || !isRecordVariableType, onlyCurrentLoopItem || treeSelectItemSelectable, false, false, treeSelectItemChildren, treeSelectItemData);
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
            let treeSelectItemTitle = declareVariableNode.data["label"] || null;
            let treeSelectItemName = variableName;
            let treeSelectItemKey = (isRecordVariableType ? this.DeclaredRecordVariablesPrefix : this.DeclaredVariablesPrefix) + this.ItemKeySplitter + variableCode;
            let declaredVariableType = this.formatDeclaredVariableType(variableType, recordType);
            let treeSelectItemChildren = [];
            let treeSelectItemData = { type: declaredVariableType, isDeclaredCollectionVariable: true, nodeId: declareVariableNode.id };
            let treeSelectItem = new TreeSelectItem(treeSelectItemKey, treeSelectItemTitle || treeSelectItemName, true, true, false, false, treeSelectItemChildren, treeSelectItemData);
            declaredCollectionVariablesItemChildren.push(treeSelectItem);
            this.ItemsList.push(treeSelectItem);
        });

        let sortedDeclaredCollectionVariablesItemChildren = this.sortTreeSelectItems(declaredCollectionVariablesItemChildren);
        return sortedDeclaredCollectionVariablesItemChildren;
    }

    private getGlobalVariablesItemChildren() {
        let globalVariablesItemChildren: TreeSelectItem[] = [];

        let workflowItem = this.getGlobalVariablesItem("workflow", "Workflow");
        let userItem = this.getGlobalVariablesItem("user", "User");
        let tenantItem = this.getGlobalVariablesItem("tenant", "Tenant");
        let currentDatetimeItem = this.getGlobalVariablesItem("currentdatetime", "Current Datetime");

        globalVariablesItemChildren.push(workflowItem);
        globalVariablesItemChildren.push(userItem);
        globalVariablesItemChildren.push(tenantItem);
        globalVariablesItemChildren.push(currentDatetimeItem);

        this.ItemsList.push(workflowItem);
        this.ItemsList.push(userItem);
        this.ItemsList.push(tenantItem);
        this.ItemsList.push(currentDatetimeItem);

        return globalVariablesItemChildren;
    }

    private getGlobalVariablesItem(sectionCode: string, sectionName: string) {
        sectionCode = sectionCode?.toLowerCase();
        let itemKey = this.GlobalVariablesPrefix + this.ItemKeySplitter + sectionCode;
        let globalVariables = this.getGlobalVariables(sectionCode);
        let itemChildren = this.getGlobalVariablesItems(itemKey, globalVariables);
        let item = new TreeSelectItem(itemKey, sectionName, false, false, false, false, itemChildren);
        return item;
    }

    private getGlobalVariables(sectionCode: string) {
        switch (sectionCode) {
            case "workflow":
                return this.getWorkflowGlobalVariables();
            case "user":
                return this.getUserGlobalVariables();
            case "tenant":
                return this.getTenantGlobalVariables();
            case "currentdatetime":
                return this.getCurrentDatetimeGlobalVariables();
            default:
                return [];
        }
    }

    private getWorkflowGlobalVariables() {
        let globalVariables: GlobalVariable[] = [
            {
                Code: "InstanceStartTime",
                Name: "Instance Start Time",
                Type: FieldTypes.DateTime,
            },
            {
                Code: "FaultMessage",
                Name: "Fault Message",
                Type: FieldTypes.Text
            }
        ];
        return globalVariables;
    }

    private getUserGlobalVariables() {
        let globalVariables: GlobalVariable[] = [
            {
                Code: "Id",
                Type: FieldTypes.LookUp,
                LookupType: "User"
            },
            {
                Code: "Name",
                Type: FieldTypes.Text
            },
            {
                Code: "Email",
                Type: FieldTypes.Text
            }
        ];
        return globalVariables;
    }

    private getTenantGlobalVariables() {
        let globalVariables: GlobalVariable[] = [
            {
                Code: "Id",
                Type: FieldTypes.Integer
            },
            {
                Code: "Name",
                Type: FieldTypes.Text
            },
            {
                Code: "TimeZone",
                Name: "Time Zone",
                Type: FieldTypes.Text
            }
        ];
        return globalVariables;
    }

    private getCurrentDatetimeGlobalVariables() {
        let globalVariables: GlobalVariable[] = [
            {
                Code: "UTCDatetime",
                Name: "UTC Time",
                Type: FieldTypes.DateTime,
            },
            {
                Code: "TenantDatetime",
                Name: "Tenant Time",
                Type: FieldTypes.DateTime,
            }
        ];
        return globalVariables;
    }

    private getGlobalVariablesItems(prefix: string, globalVariables: GlobalVariable[]) {
        let globalVariablesItems: TreeSelectItem[] = [];

        globalVariables.forEach(globalVariable => {
            let itemKey = prefix + this.ItemKeySplitter + globalVariable.Code;
            let itemData = {
                type: globalVariable.Type,
                lookupType: (globalVariable.LookupType || null),
                picklistType: (globalVariable.PicklistType || null),
                fieldCode: itemKey,
                isReadOnlyVariable: true
            };
            let treeSelectItem = new TreeSelectItem(itemKey, (globalVariable.Name || globalVariable.Code), true, true, false, false, [], itemData);
            globalVariablesItems.push(treeSelectItem);
            this.ItemsList.push(treeSelectItem);
        });

        return globalVariablesItems;
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
            } else {
                return loopNodes.filter(n => !n.data["isDeclaredCollectionVariable"]);
            }
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

    private getObjectFieldsItems(itemsKeyPrefix: string, entity: string, returnedFieldsCodes: string[] | null, nodeId: string | null, isReadOnlyFields: boolean = false) {
        let objectFieldsItems: TreeSelectItem[] = [];

        if ((returnedFieldsCodes && returnedFieldsCodes.length === 0) || this.FlowVariablesTreeListProperties.IsNoChildrenObjectVariables) {
            return objectFieldsItems;
        }

        this.getObjectFields(entity, returnedFieldsCodes).forEach((objectField: ObjectFieldList) => {
            let treeSelectItemName = objectField.FullNameTextCodeDefaultText.trim();
            let treeSelectItemKey = itemsKeyPrefix + this.ItemKeySplitter + objectField.FieldCode;
            let data = {
                type: objectField.DataTypeCode,
                lookupType: (objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null),
                picklistType: (objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null),
                fieldCode: objectField.FieldCode,
                isReadOnlyVariable: isReadOnlyFields,
                nodeId: nodeId,
                tooltip: objectField.FieldName
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
        let objectFields = ObjectFields.getByObjectTableId(entityId);
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