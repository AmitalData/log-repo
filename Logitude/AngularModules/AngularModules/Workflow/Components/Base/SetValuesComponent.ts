import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { FlowReader } from "Workflow/Models/FlowReader";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { Formatter } from "Workflow/Models/Formatter";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { SetValue } from "Workflow/Models/SetValue";
import { SetValueOperatorsList } from "Workflow/Models/SetValueOperatorsList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { GetObjectFieldPipe } from "Workflow/Pipes/GetObjectFieldPipe";
import { IsDeclaredVariablePipe } from "Workflow/Pipes/IsDeclaredVariablePipe";

@Component({
    selector: "SetValues",
    templateUrl: "./SetValuesComponent.html"
})

export class SetValuesComponent extends BaseComponent implements OnInit, OnChanges {

    @Input() SetValues: SetValue[];
    @Input() FlowObject: any;
    @Input() FlowObjectFields: ObjectFieldList[];
    @Input() CurrentNodeId: string;
    @Input() EntityId: string;
    @Input() IsEntityField: boolean = false;

    @Output() IsValidSetValuesChange = new EventEmitter<boolean>();

    public DataContext: any = this;
    public IsValidSetValues: boolean = true;

    public FlowVariablesTreeList: FlowVariablesTreeList;
    public FlowVariablesTreeItems: TreeSelectItem[];

    public SetValuesOperatorsItems = new SetValueOperatorsList().Items;

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeFlowVariablesTree();
        this.IsValidSetValues = this.isValidSetValues();
        this.IsValidSetValuesChange.emit(this.IsValidSetValues);
    }

    ngOnChanges() {
        this.setValuesChanged();
    }

    initializeFlowVariablesTree() {
        this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId);
        this.FlowVariablesTreeItems = this.FlowVariablesTreeList.Items;
    }

    setValuesChanged() {
        this.IsValidSetValues = this.isValidSetValues();
        this.IsValidSetValuesChange.emit(this.IsValidSetValues);
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

    updateSetValueEntityField(objectField: ObjectFieldPM, setValueIndex: number) {
        if (objectField?.FieldCode !== this.SetValues[setValueIndex]?.fieldCode) {
            if (objectField) {
                this.updateSetValueFieldByObjectField(objectField, setValueIndex);
            } else {
                this.resetSetValueField(setValueIndex);
            }
            this.setValuesChanged();
        }
    }

    updateSetValueField(field: string, setValueIndex: number) {
        if (field !== this.SetValues[setValueIndex]?.field) {
            if (field) {
                if (this.isDeclaredVariable(field)) {
                    this.updateSetValueFieldByDeclaredVariableField(field, setValueIndex);
                } else {
                    let objectField = this.getObjectField(field);
                    this.updateSetValueFieldByObjectField(objectField, setValueIndex, field);
                }
            } else {
                this.resetSetValueField(setValueIndex);
            }
            this.setValuesChanged();
        }
    }

    updateSetValueFieldByObjectField(objectField: ObjectFieldPM | ObjectFieldList, setValueIndex: number, field: string | null = null) {
        this.SetValues[setValueIndex].fieldCode = field ? field : (objectField ? objectField.FieldCode : null);
        this.SetValues[setValueIndex].field = field ? field : (objectField ? objectField.FieldName : null);
        this.SetValues[setValueIndex].type = objectField ? objectField.DataTypeCode : null;
        this.SetValues[setValueIndex].lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
        this.SetValues[setValueIndex].picklistType = objectField && objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
        this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
        this.SetValues[setValueIndex].value = null;
        this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;
    }

    updateSetValueFieldByDeclaredVariableField(field: string, setValueIndex: number) {
        this.SetValues[setValueIndex].fieldCode = field ? field : null;
        this.SetValues[setValueIndex].field = field ? field : null;
        this.SetValues[setValueIndex].type = field ? this.getDeclaredVariableFieldType(field) : null;
        this.SetValues[setValueIndex].lookupType = null;
        this.SetValues[setValueIndex].picklistType = null;
        this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
        this.SetValues[setValueIndex].value = null;
        this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;
    }

    resetSetValueField(setValueIndex: number) {
        this.SetValues[setValueIndex].fieldCode = null;
        this.SetValues[setValueIndex].field = null;
        this.SetValues[setValueIndex].type = null;
        this.SetValues[setValueIndex].lookupType = null;
        this.SetValues[setValueIndex].picklistType = null;
        this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
        this.SetValues[setValueIndex].value = null;
        this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;
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

    getDeclaredVariableFieldType(field: string) {
        if (field) {
            let fieldCode = Formatter.getFieldCode(field);
            let declareVariableNode = fieldCode ? FlowReader.getNodes(this.FlowObject, "declareVariableNode").find((n: any) => n.data["variableCode"] === fieldCode) : null;
            return declareVariableNode ? declareVariableNode.data["variableType"] : null;
        }
        return null;
    }

    isDeclaredVariable(field: string) {
        return new IsDeclaredVariablePipe().transform(field);
    }

    getObjectField(field: string) {
        return new GetObjectFieldPipe().transform(field, this.FlowObjectFields);
    }
}
