import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { SetValue } from "Workflow/Models/SetValue";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { EditableRecordsTreeList } from "Workflow/TreeLists/EditableRecordsTreeList";
import { FlowReader } from "Workflow/Utilities/FlowReader";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";

@Component({
    templateUrl: "./CreateTaskPropertiesComponent.html"
})

export class CreateTaskPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public IsNew: boolean;
    public Name: string = null;
    public Record: string;
    public SetValues: SetValue[];
    public TaskEntityId: string;
    public FlowObject: any;
    public CurrentNodeId: string;
    public ValidationErrorsList: string[];
    public IsValidSetValues: boolean = true;
    public EditableRecordsTreeItems: TreeSelectItem[];
    public CurrentSession = SessionLocator.SelectedSession;

    public InitialTaskFields: string[] = [
        "Subject",
        "TaskTypeId",
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
        this.initialize();
        this.initializeEditableRecordsTreeItems();
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

    initialize() {
        this.IsNew = Object.keys(this.Data).length === 0;

        this.Name = this.Data["label"] || this.Data["name"] || null;
        this.Record = this.Data["record"] || null;
        this.SetValues = this.Data["setValues"] || [];

        this.TaskEntityId = ObjectTables.getIdByName("Task");

        this.initializeSetValues();
        this.setUIProperties();
    }

    initializeEditableRecordsTreeItems() {
        this.EditableRecordsTreeItems = new EditableRecordsTreeList(this.FlowObject, this.CurrentNodeId).Items;
    }

    initializeSetValues() {
        if (this.SetValues.length === 0) {
            this.getInitialTaskObjectFields().forEach(objectField => {
                this.addSetValue(objectField);
            });
            this.IsValidSetValues = false;
        }
    }

    getInitialTaskObjectFields() {
        return ObjectFields.getByObjectTableId(this.TaskEntityId)
            .filter(o => this.InitialTaskFields.includes(o.FieldName))
            .sort((a, b) => this.getTaskObjectFieldOrder(a.FieldName) - this.getTaskObjectFieldOrder(b.FieldName));
    }

    getTaskObjectFieldOrder(fieldName: string) {
        let taskObjectFieldOrder = this.InitialTaskFields.length + 1;
        if (fieldName) {
            let taskObjectFieldIndex = this.InitialTaskFields.indexOf(fieldName);
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

        this.Record = record;
        this.Data["record"] = record;
        this.Data["entity"] = entity;
        this.Data["isCustomEntity"] = ObjectTables.getIsCustomByName(entity);
        this.Data["recordUsedFrom"] = recordUsedFrom;

        this.setUIProperties();
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
        if (notValidUIProperties.length === 0 && this.IsValidSetValues && isValidName) {
            this.setValuesData();
            console.log(this.Data);
            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidSetValues) {
                this.ValidationErrorsList.push("Invalid Set Values");
            }

            if (!isValidName) {
                this.ValidationErrorsList.push("The Name Should be Unique.");
            }
        }
    }

    setValuesData() {
        this.Data["setValues"] = this.SetValues;
    }
}