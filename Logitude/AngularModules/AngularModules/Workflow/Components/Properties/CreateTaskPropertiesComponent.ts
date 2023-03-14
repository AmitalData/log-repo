import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { UIProperty } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { Condition } from "Workflow/Models/Condition";
import { SetValue } from "Workflow/Models/SetValue";
import { TaskEntityField } from "Workflow/Models/TaskEntityField";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { EditableRecordsTreeList } from "Workflow/TreeLists/EditableRecordsTreeList";
import { ObjectFieldsTreeList } from "Workflow/TreeLists/ObjectFieldsTreeList";
import { FlowReader } from "Workflow/Utilities/FlowReader";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";

@Component({
    templateUrl: "./CreateTaskPropertiesComponent.html"
})

export class CreateTaskPropertiesComponent extends BaseComponent implements OnInit {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
    public TaskEntityId: string;
    public Name: string = null;
    public Record: string;
    public Entity: string;
    public IsRecordEntityChanged: boolean = false;
    public TaskTypeId: string;
    public SetValues: SetValue[];
    public EntityFields: TaskEntityField[];
    public FlowObject: any;
    public CurrentNodeId: string;
    public ValidationErrorsList: string[];
    public IsValidSetValues: boolean = true;
    public IsValidEntityFields: boolean = true;
    public EditableRecordsTreeItems: TreeSelectItem[];
    public ObjectFieldsTreeItems: TreeSelectItem[];
    public TaskTypeQueryFilters: ApiQueryFilters;
    public CurrentSession = SessionLocator.SelectedSession;

    public InitialSetValueTaskFields: string[] = [
        "Subject",
        "OwnerId",
        "DueDate",
        "PriorityId"
    ];

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
    }

    ngOnInit() {
        this.initializeWindowEvents();
        this.initializeEditableRecordsTreeItems();
        this.initialize();
        this.initializeObjectFieldsTreeItems();
    }

    initializeWindowEvents() {
        this.CurrentSession.CurrentWindow.FooterButtonsClicked.subscribe((e: any) => {
            if (e === "submit") {
                this.saveButtonClicked();
            } else {
                this.cancelButtonClicked();
            }
        });
    }

    initializeEditableRecordsTreeItems() {
        this.EditableRecordsTreeItems = new EditableRecordsTreeList(this.FlowObject, this.CurrentNodeId).Items;
    }

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;
        this.TaskEntityId = ObjectTables.getIdByName("Task");

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.Record = this.Data["record"] || null;
        this.Entity = this.Data["entity"] || null;
        this.TaskTypeId = this.Data["taskTypeId"] || null;
        this.SetValues = this.Data["setValues"] || [];
        this.EntityFields = this.Data["fields"] || [];

        this.handleRecordEntityChanged();
        this.setTaskTypeQueryFilters();
        this.initializeSetValues();
        this.setUIProperties();
    }

    initializeObjectFieldsTreeItems() {
        let entityId = ObjectTables.getIdByName(this.Entity);
        this.ObjectFieldsTreeItems = new ObjectFieldsTreeList(entityId).Items;
    }

    handleRecordEntityChanged() {
        if (this.Record) {
            let editableRecordItem = this.EditableRecordsTreeItems.filter(i => i.key === this.Record)[0];
            if (editableRecordItem && editableRecordItem.data && editableRecordItem.data["entity"] !== this.Entity) {
                this.updateRecord(editableRecordItem);
            }
        }
    }

    setTaskTypeQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters(false);
        let entityObjectTableId = ObjectTables.getIdByName(this.Entity);
        apiQueryFilters.addAdditionalFilter("EntityObjectTableId", entityObjectTableId, null, null, "Equals", false, false, false, "Text");
        this.TaskTypeQueryFilters = apiQueryFilters;
    }

    initializeSetValues() {
        if (this.SetValues.length === 0) {
            this.getInitialSetValueTaskObjectFields().forEach(objectField => {
                this.addSetValue(objectField);
            });
            this.IsValidSetValues = false;
        }
    }

    getInitialSetValueTaskObjectFields() {
        return ObjectFields.getByObjectTableId(this.TaskEntityId)
            .filter(o => this.InitialSetValueTaskFields.includes(o.FieldName))
            .sort((a, b) => this.getTaskObjectFieldOrder(a.FieldName) - this.getTaskObjectFieldOrder(b.FieldName));
    }

    getTaskObjectFieldOrder(fieldName: string) {
        let taskObjectFieldOrder = this.InitialSetValueTaskFields.length + 1;
        if (fieldName) {
            let taskObjectFieldIndex = this.InitialSetValueTaskFields.indexOf(fieldName);
            return taskObjectFieldIndex === -1 ? taskObjectFieldOrder : taskObjectFieldIndex;
        }
        return taskObjectFieldOrder;
    }

    addSetValue(objectField: ObjectFieldList) {
        if (objectField) {
            let setValue = new SetValue();
            setValue.fieldCode = objectField.FieldCode;
            setValue.field = objectField.FieldName;
            setValue.type = objectField.DataTypeCode;
            setValue.lookupType = objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
            setValue.picklistType = objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
            setValue.operator = SetValueOperators.Equals;
            setValue.disabled = "d,f";
            this.SetValues.push(setValue);
        }
    }

    addEntityField() {
        if (this.IsValidEntityFields) {
            let entityField = new TaskEntityField();
            this.EntityFields.push(entityField);
            this.IsValidEntityFields = false;
        }
    }

    deleteEntityField(entityFieldIndex: number) {
        let entityField = this.EntityFields[entityFieldIndex];
        if (entityField) {
            this.EntityFields.splice(entityFieldIndex, 1);
            this.validateEntityFields();
        }
    }

    resetEntityFields() {
        this.EntityFields = [];
        this.IsValidEntityFields = true;
    }

    updateIsValidSetValues(isValidSetValues: boolean) {
        this.IsValidSetValues = isValidSetValues;
    }

    updateName(name: string) {
        if (this.IsNew) {
            this.Data["name"] = name;
        }

        this.Data["label"] = name;
        this.Name = name;

        this.setUIProperties();
    }

    updateRecord(recordItem: TreeSelectItem) {
        let entity = recordItem && recordItem.data ? (recordItem.data["entity"] || null) : null;
        let record = recordItem ? recordItem.key : null;
        let recordUsedFrom = recordItem && recordItem.data ? (recordItem.data["nodeId"] || null) : null;

        let isEntityChanged = this.Data["entity"] !== entity;

        this.Record = record;
        this.Entity = entity;
        this.Data["record"] = record;
        this.Data["entity"] = entity;
        this.Data["isCustomEntity"] = ObjectTables.getIsCustomByName(entity);
        this.Data["recordUsedFrom"] = recordUsedFrom;

        if (isEntityChanged) {
            this.updateTaskTypeId(null);
            this.setTaskTypeQueryFilters();
            this.resetEntityFields();
            this.initializeObjectFieldsTreeItems();
            this.IsRecordEntityChanged = !this.IsRecordEntityChanged;
        }

        this.setUIProperties();
    }

    updateTaskTypeId(taskTypeId: string | null) {
        this.TaskTypeId = taskTypeId || null;
        this.Data["taskTypeId"] = taskTypeId || null;
    }

    updateEntityField(objectFieldItem: TreeSelectItem, entityFieldIndex: number) {
        let objectField = objectFieldItem ? (objectFieldItem.data["objectField"] || null) : null;
        this.EntityFields[entityFieldIndex].fieldCode = objectField ? objectField.FieldCode : null;
        this.EntityFields[entityFieldIndex].field = objectField ? objectField.FieldName : null;
        this.validateEntityFields();
    }

    validateEntityFields() {
        if (this.EntityFields) {
            this.IsValidEntityFields = this.EntityFields.filter(e => !e.fieldCode).length === 0;
        } else {
            this.IsValidEntityFields = true;
        }
    }

    updateEntityFieldRequired(isRequired: boolean, entityFieldIndex: number) {
        this.EntityFields[entityFieldIndex].isRequired = isRequired;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Record", null, AppTool.IsNullOrEmpty(this.Record));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        let isValidSave = notValidUIProperties.length === 0 && this.IsValidSetValues && this.IsValidEntityFields && isValidName;

        if (isValidSave) {
            this.completeSave();
        } else {
            this.handleSaveValidationErrors(notValidUIProperties, isValidName);
        }
    }

    completeSave() {
        this.setSetValuesData();
        this.setEntityFieldsData();
        this.setDoneConditionsFromFields();
        console.log(this.Data);
        this.CurrentSession.CurrentWindow.Close(this.Data);
    }

    handleSaveValidationErrors(notValidUIProperties: UIProperty[], isValidName: boolean) {
        let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
        this.ValidationErrorsList = validationErrors;
        if (!isValidName) {
            this.ValidationErrorsList.push("The Name Should be Unique.");
        }
        if (!this.IsValidSetValues) {
            this.ValidationErrorsList.push("Invalid Set Values");
        }
        if (!this.IsValidEntityFields) {
            this.ValidationErrorsList.push("Invalid Fields");
        }
    }

    setSetValuesData() {
        this.Data["setValues"] = this.SetValues;
    }

    setEntityFieldsData() {
        this.Data["fields"] = this.EntityFields && this.EntityFields.length > 0 ? this.EntityFields : null;
    }

    setDoneConditionsFromFields() {
        let doneConditions = this.getConditionsFromEntityFields(this.EntityFields);
        this.Data["doneConditions"] = doneConditions && doneConditions.length > 0 ? doneConditions : null;
        this.Data["doneConditionsOperation"] = doneConditions && doneConditions.length > 0 ? ConditionOperations.And : null;
    }

    getConditionsFromEntityFields(entityFields: TaskEntityField[]) {
        if (entityFields && entityFields.length > 0) {
            return entityFields.filter(entityField => entityField.isRequired)
                .map(entityField => { return this.getConditionFromEntityField(entityField); })
                .filter(doneCondition => doneCondition !== null);
        }
        return [];
    }

    getConditionFromEntityField(entityField: TaskEntityField) {
        let objectField = ObjectFields.getByCode(entityField?.fieldCode);
        if (entityField && entityField.isRequired && objectField) {
            let doneCondition = new Condition();
            doneCondition.fieldCode = objectField.FieldCode;
            doneCondition.field = objectField.FieldName;
            doneCondition.type = objectField.DataTypeCode;
            doneCondition.lookupType = objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
            doneCondition.picklistType = objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
            doneCondition.operator = ConditionOperators.IsEmpty;
            doneCondition.value = "False";
            return doneCondition;
        }
        return null;
    }
}