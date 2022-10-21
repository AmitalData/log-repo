import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { FlowReader } from "Workflow/Models/FlowReader";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { SetValue } from "Workflow/Models/SetValue";
import { SetValueOperatorsList } from "Workflow/Models/SetValueOperatorsList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { TreeSelectDataFilter } from "Workflow/Models/Types";

@Component({
    selector: "SetValues",
    templateUrl: "./SetValuesComponent.html"
})

export class SetValuesComponent extends BaseComponent implements OnInit, OnChanges {

    @Input() SetValues: SetValue[];
    @Input() FlowObject: any;
    @Input() FlowObjectFields: ObjectFieldPM[];
    @Input() CurrentNodeId: string;
    @Input() EntityId: string;
    @Input() SetEntityField: boolean = false;

    @Output() IsValidSetValuesChange = new EventEmitter<boolean>();

    public DataContext: any = this;
    public IsValidSetValues: boolean = true;

    public FlowVariablesTreeItems: TreeSelectItem[];

    public SetValuesOperatorsItems = new SetValueOperatorsList().Items;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeFlowVariablesTreeItems();
        this.IsValidSetValues = this.isValidSetValues();
        this.IsValidSetValuesChange.emit(this.IsValidSetValues);
    }

    ngOnChanges() {
        this.setValuesChanged();
    }

    initializeFlowVariablesTreeItems() {
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId).Items;
    }

    setValuesChanged() {
        this.IsValidSetValues = this.isValidSetValues();
        this.IsValidSetValuesChange.emit(this.IsValidSetValues);
    }

    updateSetValueField(field: string, setValueIndex: number) {
        if (field !== this.SetValues[setValueIndex]?.field) {
            this.SetValues[setValueIndex].field = field ? field : null;
            this.SetValues[setValueIndex].fieldCode = null;
            this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
            this.SetValues[setValueIndex].value = null;
            this.SetValues[setValueIndex].type = field ? this.getFieldType(field) : null;
            this.SetValues[setValueIndex].lookupType = field ? this.getFieldLookupType(field) : null;
            this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;

            this.setValuesChanged();
        }
    }

    updateSetValueEntityField(objectField: ObjectFieldPM, setValueIndex: number) {
        if (objectField?.FieldCode !== this.SetValues[setValueIndex]?.fieldCode) {
            this.SetValues[setValueIndex].field = objectField ? objectField.FieldName : null;
            this.SetValues[setValueIndex].fieldCode = objectField ? objectField.FieldCode : null;
            this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
            this.SetValues[setValueIndex].value = null;
            this.SetValues[setValueIndex].type = objectField ? objectField.DataTypeCode : null;
            this.SetValues[setValueIndex].lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
            this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;

            this.setValuesChanged();
        }
    }

    updateSetValueOperator(operatorCode: string, setValueIndex: number) {
        if (operatorCode !== this.SetValues[setValueIndex]?.operator) {
            this.SetValues[setValueIndex].operator = operatorCode;
            this.SetValues[setValueIndex].value = null;
            this.setValuesChanged();
        }
    }

    updateSetValue(value: string, setValueIndex: number) {
        if (value !== this.SetValues[setValueIndex]?.value) {
            this.SetValues[setValueIndex].value = value;
            this.setValuesChanged();
        }
    }

    addSetValue() {
        if (this.IsValidSetValues) {
            let setvalue = new SetValue();
            this.SetValues.push(setvalue);
            this.setValuesChanged();
        }
    }

    deleteSetValue(setValueIndex: number) {
        let setvalue = this.SetValues[setValueIndex];
        if (setvalue) {
            this.SetValues.splice(setValueIndex, 1);
        }
        this.setValuesChanged();
    }

    getFieldType(field: string) {
        if (this.isObjectField(field)) {
            let fieldCode = this.getFieldCode(field);
            let objectField = this.getObjectField(fieldCode);
            return objectField ? objectField.DataTypeCode : null;
        } else {
            return this.getDeclareVariableType(field);
        }
    }

    getFieldLookupType(field: string) {
        if (this.isObjectField(field)) {
            let fieldCode = this.getFieldCode(field);
            let objectField = this.getObjectField(fieldCode);
            return objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
        } else {
            return null;
        }
    }

    getDeclareVariableType(field: string) {
        let fieldCode = this.getFieldCode(field);
        let declareVariableNode = FlowReader.getNodes(this.FlowObject, "declareVariableNode").find((n: any) => n.data["variableCode"] === fieldCode);
        return declareVariableNode ? declareVariableNode.data["variableType"] : null;
    }

    isFieldCompareOperator(operatorCode: string) {
        return operatorCode && operatorCode.endsWith("<field>");
    }

    isObjectField(field: string) {
        let parent = field.split('_')[0]
        return parent != "declaredvariables";
    }

    isValidSetValues() {
        let result = true;
        for (let setValues of (this.SetValues)) {
            if (!setValues.field || !setValues.value || !setValues.operator) {
                result = false;
                break;
            }
        }
        return result;
    }

    getFieldCode(field: string) {
        if (field.indexOf("_") === -1) {
            return field;
        }
        let fieldCode = field.split('_')[1];
        return fieldCode;
    }

    getObjectField(fieldCode: string) {
        return this.FlowObjectFields.find(o => o.FieldCode === fieldCode);
    }

    getObjectFieldsQueryFilters() {
        return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(this.EntityId, null, null);
    }

    getTreeSelectDataFilters(setValueIndex: number) {
        let setValue = this.SetValues[setValueIndex];
        let dataFilters: TreeSelectDataFilter[] = [];
        if(setValue.type){
            dataFilters.push({ Key: "type", Value: setValue.type });
        }
        if(setValue.lookupType){
            dataFilters.push({ Key: "lookupType", Value: setValue.lookupType });
        }
        return dataFilters;
    }
}