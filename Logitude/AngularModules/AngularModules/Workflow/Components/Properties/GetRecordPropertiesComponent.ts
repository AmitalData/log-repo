import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { GetRecordsLimit } from "Workflow/Constants/GetRecordsLimit";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { Condition } from "Workflow/Models/Condition";
import { ReturnedField } from "Workflow/Models/ReturnedField";

@Component({
    templateUrl: "./GetRecordPropertiesComponent.html"
})

export class GetRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;
    public Name: string = null;
    public Entity: string = null;
    public EntityId: string = null;
    public RecordsLimit: string = null;
    public ReturnedFields: ReturnedField[];
    public Conditions: Condition[];
    public ConditionsOperation: string;

    public ValidationErrorsList: string[];
    public IsValidConditions: boolean = true;
    public IsValidReturnedFields: boolean = true;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldPM[];

    //public PrimaryObjectFieldCode: string | null = null;

    public CurrentSession = SessionLocator.SelectedSession;


    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.initialize();
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.Entity = this.Data["entity"] || null;
        this.RecordsLimit = this.Data["recordsLimit"] ? this.Data["recordsLimit"] : GetRecordsLimit.FirstRecord
        this.Conditions = this.Data["conditions"] || [];
        this.ConditionsOperation = this.Data["conditionsOperation"] || ConditionOperations.And;
        this.ReturnedFields = this.Data["returnedFields"] || [];

        this.Data["recordsLimit"] = this.RecordsLimit;

        this.EntityId = this.getEntityId(this.Entity);

        this.initializeConditions();
        this.initializeReturnedFields();

        this.setUIProperties();
    }

    initializeConditions(reset: boolean = false) {
        if (reset) {
            this.Conditions = [];
            this.ConditionsOperation = ConditionOperations.And;
        }
        if (this.Conditions.length === 0) {
            let condition = new Condition();
            this.Conditions.push(condition);
            this.IsValidConditions = false;
        }
    }

    initializeReturnedFields(reset: boolean = false) {
        if (reset) {
            this.ReturnedFields = [];
        }
        if (this.EntityId && this.ReturnedFields.length === 0) {
            let primaryObjectField = this.FlowObjectFields.find(o => o.ObjectTableId === this.EntityId && o.FieldName === "Id");
            if (primaryObjectField) {
                let field = new ReturnedField();
                field.fieldCode = primaryObjectField.FieldCode;
                field.type = primaryObjectField.DataTypeCode;
                this.ReturnedFields.push(field);
            }
            this.ReturnedFields.push(new ReturnedField());
            this.IsValidReturnedFields = false;
        }
    }

    updateName(Name: any) {
        this.Data["name"] = Name;
        this.Name = Name;

        this.setUIProperties();
    }

    updateEntity(entity: any) {
        let isEntityChanged = this.Data["entity"] !== entity?.Name;
        this.Data["entity"] = entity ? entity.Name : null;
        this.Entity = entity ? entity.Name : null;
        this.EntityId = this.getEntityId(entity ? entity.Name : null);

        if (isEntityChanged) {
            this.initializeConditions(true);
            this.initializeReturnedFields(true);
        }

        this.setUIProperties();
    }

    updateRecordsLimit(recordsLimit: string) {
        this.Data["recordsLimit"] = recordsLimit;
        this.RecordsLimit = recordsLimit;
    }

    getObjectTablesQueryFilters() {
        return ApiQueryFiltersBuilder.getObjectTablesApiQueryFilters("Shipment");
    }

    UpdateIsValidConditions(isValidConditions: boolean) {
        this.IsValidConditions = isValidConditions;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
        this.UIProperties.SetRequired("Object", null, AppTool.IsNullOrEmpty(this.EntityId));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidConditions && this.IsValidReturnedFields) {
            this.setData();

            //console.log(this.Data);

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors;

            if (!this.IsValidConditions)
                this.ValidationErrorsList.push("Invalid Conditions");

            if (!this.IsValidReturnedFields)
                this.ValidationErrorsList.push("Invalid Selected Fields");
        }
    }

    setData() {
        this.Data["conditions"] = this.Conditions;
        this.Data["conditionsOperation"] = this.Conditions.length === 0 ? null : this.ConditionsOperation;
        this.Data["returnedFields"] = this.ReturnedFields;
    }

    getEntityId(entity: string) {
        if (entity) {
            let entityObjectTable = (window as any).ObjectTables.filter((o: any) => o.Name === entity)[0];
            return entityObjectTable ? entityObjectTable.Id : null;
        }
        return null;
    }

    getObjectFieldsQueryFilters() {
        return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(this.EntityId, null, null);
    }

    updateSelectedField(selectedField: ObjectFieldPM, index: number) {
        this.ReturnedFields[index].fieldCode = selectedField ? selectedField.FieldCode : null;
        this.ReturnedFields[index].type = selectedField ? selectedField.DataTypeCode : null;
        this.IsValidReturnedFields = this.ReturnedFields.filter(r => r.fieldCode === null).length === 0;
    }

    addEmptyField() {
        if (this.IsValidReturnedFields) {
            this.ReturnedFields.push(new ReturnedField());
            this.IsValidReturnedFields = false;
        }
    }

    deleteField(index: number) {
        this.ReturnedFields.splice(index, 1);
        this.IsValidReturnedFields = this.ReturnedFields.filter(r => r.fieldCode === null).length === 0;
    }
}