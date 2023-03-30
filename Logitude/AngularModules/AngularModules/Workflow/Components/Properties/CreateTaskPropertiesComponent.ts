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
import { FieldApiQueryFilter } from "Workflow/Models/FieldApiQueryFilter";
import { SetValue } from "Workflow/Models/SetValue";
import { TaskField } from "Workflow/Models/TaskField";
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
    public TaskObjectTableId: string;
    public Name: string = null;
    public Record: string;
    public Entity: string;
    public IsRecordEntityChanged: boolean = false;
    public TaskTypeId: string;
    public SetValues: SetValue[];
    public TaskFields: TaskField[];
    public FlowObject: any;
    public CurrentNodeId: string;
    public ValidationErrorsList: string[];
    public IsValidSetValues: boolean = true;
    public IsValidTaskFields: boolean = true;
    public EditableRecordsTreeItems: TreeSelectItem[];
    public ObjectFieldsTreeItems: TreeSelectItem[];
    public TaskTypeQueryFilters: ApiQueryFilters;
    public CurrentSession = SessionLocator.SelectedSession;
    public TaskBaseType: string = null;
    public RelatedToEntity: string = "RelatedToEntity";
    public FieldApiQueryFilters: FieldApiQueryFilter[] = [];

    public InitialSetValueFields: string[] = [];
    public RelatedEntitySetValueFields: string[] = [
        "Subject",
        "OwnerId",
        "DueDate",
        "PriorityId"
    ];
    public StandaloneSetValueFields: string[] = [
        "Subject",
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
        this.TaskObjectTableId = ObjectTables.getIdByName("Task");

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.Record = this.Data["record"] || null;
        this.Entity = this.Data["entity"] || null;
        this.TaskTypeId = this.Data["taskTypeId"] || null;
        this.SetValues = this.Data["setValues"] || [];
        this.TaskFields = this.Data["fields"] || [];

        this.TaskBaseType = this.Data["taskBaseType"] || this.RelatedToEntity;
        this.InitialSetValueFields = this.RelatedEntitySetValueFields;

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
        let apiQueryFilters = new ApiQueryFilters();
        let entityObjectTableId = ObjectTables.getIdByName(this.Entity);
        apiQueryFilters.addAdditionalFilter("EntityObjectTableId", (entityObjectTableId || "null"), null, null, "Equals", false, false, false, "Text");
        this.TaskTypeQueryFilters = apiQueryFilters;

        let taskTypeApiQueryFilter = this.FieldApiQueryFilters.find(e => e.fieldCode = "TaskTypeId");
        if (taskTypeApiQueryFilter) {
            taskTypeApiQueryFilter.apiQueryFilters = apiQueryFilters
        } else {
            let fieldApiQueryFilter = new FieldApiQueryFilter();
            fieldApiQueryFilter.fieldCode = "TaskTypeId";
            fieldApiQueryFilter.apiQueryFilters = apiQueryFilters;
            this.FieldApiQueryFilters.push(fieldApiQueryFilter);
        }
    }

    initializeSetValues() {
        if (this.SetValues.length === 0) {
            this.getInitialSetValueObjectFields().forEach(objectField => {
                this.addSetValue(objectField);
            });
            this.IsValidSetValues = false;
        }
    }

    getInitialSetValueObjectFields() {
        return ObjectFields.getByObjectTableId(this.TaskObjectTableId)
            .filter(o => this.InitialSetValueFields.includes(o.FieldName))
            .sort((a, b) => this.getTaskObjectFieldOrder(a.FieldName) - this.getTaskObjectFieldOrder(b.FieldName));
    }

    getTaskObjectFieldOrder(fieldName: string) {
        let taskObjectFieldOrder = this.InitialSetValueFields.length + 1;
        if (fieldName) {
            let taskObjectFieldIndex = this.InitialSetValueFields.indexOf(fieldName);
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

    addTaskField() {
        if (this.IsValidTaskFields) {
            let taskField = new TaskField();
            this.TaskFields.push(taskField);
            this.IsValidTaskFields = false;
        }
    }

    deleteTaskField(taskFieldIndex: number) {
        let taskField = this.TaskFields[taskFieldIndex];
        if (taskField) {
            this.TaskFields.splice(taskFieldIndex, 1);
            this.validateTaskFields();
        }
    }

    resetTaskFields() {
        this.TaskFields = [];
        this.IsValidTaskFields = true;
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


    updateTaskBaseType(type: string) {
        this.Data["taskBaseType"] = type
        this.TaskBaseType = type;

        this.resetRecordFields();

        if (type === this.RelatedToEntity) {
            this.InitialSetValueFields = this.RelatedEntitySetValueFields;
        } else {
            this.InitialSetValueFields = this.StandaloneSetValueFields;
        }
        this.initializeSetValues();
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
            this.resetTaskTypeValue();
            this.resetTaskFields();
            this.initializeObjectFieldsTreeItems();
            this.IsRecordEntityChanged = !this.IsRecordEntityChanged;
        }

        this.setUIProperties();
    }

    updateTaskTypeId(taskTypeId: string | null) {
        this.TaskTypeId = taskTypeId || null;
        this.Data["taskTypeId"] = taskTypeId || null;
    }

    updateTaskField(objectFieldItem: TreeSelectItem, taskFieldIndex: number) {
        let objectField = objectFieldItem ? (objectFieldItem.data["objectField"] || null) : null;
        this.TaskFields[taskFieldIndex].fieldCode = objectField ? objectField.FieldCode : null;
        this.TaskFields[taskFieldIndex].field = objectField ? objectField.FieldName : null;
        this.validateTaskFields();
    }

    validateTaskFields() {
        if (this.TaskFields) {
            this.IsValidTaskFields = this.TaskFields.filter(t => !t.fieldCode).length === 0;
        } else {
            this.IsValidTaskFields = true;
        }
    }

    updateTaskFieldRequired(isRequired: boolean, taskFieldIndex: number) {
        this.TaskFields[taskFieldIndex].isRequired = isRequired;
    }

    resetRecordFields() {
        this.SetValues = [];
        this.Record = null;
        this.Entity = null;
        this.Data["record"] = null;
        this.Data["entity"] = null;
        this.Data["isCustomEntity"] = false;
        this.Data["recordUsedFrom"] = null;
    }
    
    resetTaskTypeValue() {
        this.FieldApiQueryFilters.forEach(fieldApiQueryFilter => {
            let setvalue = this.SetValues.find(s => s.field == fieldApiQueryFilter.fieldCode)
            if (setvalue) {
                setvalue.value = null;
                setvalue.fieldChangedToggle = !setvalue.fieldChangedToggle;
            }
        });
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        if (this.TaskBaseType === this.RelatedToEntity) {
            this.UIProperties.SetRequired("Record", null, AppTool.IsNullOrEmpty(this.Record));
        } else {
            this.UIProperties.SetRequired("Record", null, false);
        }
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        let isValidName = !this.IsNew || !FlowReader.isNodeCodeExists(this.FlowObject, this.Name);
        let isValidSave = notValidUIProperties.length === 0 && this.IsValidSetValues && this.IsValidTaskFields && isValidName;

        if (isValidSave) {
            this.completeSave();
        } else {
            this.handleSaveValidationErrors(notValidUIProperties, isValidName);
        }
    }

    completeSave() {
        this.setSetValuesData();
        this.setTaskFieldsData();
        this.setDoneConditionsFromFields();
        //console.log(this.Data);
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
        if (!this.IsValidTaskFields) {
            this.ValidationErrorsList.push("Invalid Fields");
        }
    }

    setSetValuesData() {
        this.Data["setValues"] = this.SetValues;
    }

    setTaskFieldsData() {
        this.Data["fields"] = this.TaskFields && this.TaskFields.length > 0 ? this.TaskFields : null;
    }

    setDoneConditionsFromFields() {
        let conditions = this.getConditionsFromTaskFields();

        let doneConditions = null;

        if (conditions && conditions.length > 0) {
            doneConditions = {
                Operation: ConditionOperations.And,
                Conditions: conditions
            };
        }

        this.Data["doneConditions"] = doneConditions;
    }

    getConditionsFromTaskFields() {
        if (this.TaskFields && this.TaskFields.length > 0) {
            return this.TaskFields.filter(taskField => taskField.isRequired)
                .map(taskField => { return this.getConditionFromTaskField(taskField); })
                .filter(condition => condition !== null);
        }
        return [];
    }

    getConditionFromTaskField(taskField: TaskField) {
        let objectField = ObjectFields.getByCode(taskField?.fieldCode);
        if (taskField && taskField.isRequired && objectField) {
            let condition = new Condition();
            condition.fieldCode = objectField.FieldCode;
            condition.field = objectField.FieldName;
            condition.type = objectField.DataTypeCode;
            condition.lookupType = objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
            condition.picklistType = objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
            condition.operator = ConditionOperators.IsEmpty;
            condition.value = "False";
            return condition;
        }
        return null;
    }
}