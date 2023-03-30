import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { SetValue } from "Workflow/Models/SetValue";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ExpressionValue } from "Workflow/Types";
import { ObjectFieldPipe } from "Workflow/Pipes/ObjectFieldPipe";
import { IsNoObjectFieldVariablePipe } from "Workflow/Pipes/IsNoObjectFieldVariablePipe";
import { ObjectFieldsTreeList } from "Workflow/TreeLists/ObjectFieldsTreeList";
import { SetValueDisabledPipe } from "Workflow/Pipes/SetValueDisabledPipe";
import { FieldApiQueryFilter } from "Workflow/Models/FieldApiQueryFilter";

@Component({
    selector: "SetValues",
    templateUrl: "./SetValuesComponent.html"
})

export class SetValuesComponent extends BaseComponent implements OnInit, OnChanges {

    @Input() SetValues: SetValue[];
    @Input() FlowObject: any;
    @Input() CurrentNodeId: string;
    @Input() EntityId: string;
    @Input() IsEntityField: boolean = false;
    @Input() FieldApiQueryFilters: FieldApiQueryFilter[] = null;

    @Output() IsValidSetValuesChange = new EventEmitter<boolean>();

    public DataContext: any = this;
    public IsValidSetValues: boolean = true;
    public SetValuesCounter: number = 1;

    public ComboBoxMaxHeight: number = 120;

    public FlowVariablesTreeItems: TreeSelectItem[];
    public ObjectFieldsTreeItems: TreeSelectItem[];

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeSetValuesIds();
        this.initializeFlowVariablesTree();
        this.IsValidSetValues = this.isValidSetValues();
        this.IsValidSetValuesChange.emit(this.IsValidSetValues);
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes && changes.EntityId && changes.EntityId.currentValue !== changes.EntityId.previousValue && this.IsEntityField) {
            this.ObjectFieldsTreeItems = new ObjectFieldsTreeList(changes.EntityId.currentValue).Items;
        }
        this.setValuesChanged();
    }

    initializeFlowVariablesTree() {
        let props = {
            ShowRecordsVariables: true,
            ShowDeclaredVariables: true,
            //ShowDeclaredCollectionVariables: true,
            ShowGlobalVariables: true,
            IsObjectVariableSelectable: true
        };
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props).Items;
    }

    setValuesChanged(event: any = null) {
        this.IsValidSetValues = this.isValidSetValues();
        this.IsValidSetValuesChange.emit(this.IsValidSetValues);
        if (event === "add") {
            this.increaseSetValuesCounter();
        }
    }

    isValidSetValues() {
        let result = true;
        for (let setValues of this.SetValues) {
            if (setValues.operator === SetValueOperators.Expression) {
                if (!setValues.field || !setValues.expressionValue || !setValues.expressionValue.expression || setValues.expressionValue.expression === "") {
                    result = false;
                    break;
                }
            } else {
                if (!setValues.field || !setValues.operator || !setValues.value) {
                    result = false;
                    break;
                }
            }
        }
        return result;
    }

    updateSetValueEntityField(objectFieldItem: TreeSelectItem, setValueIndex: number) {
        let objectField = objectFieldItem ? (objectFieldItem.data["objectField"] || null) : null;
        if (objectField?.FieldCode !== this.SetValues[setValueIndex]?.fieldCode) {
            if (objectField) {
                this.updateSetValueFieldByObjectField(objectField, setValueIndex);
            } else {
                this.resetSetValueField(setValueIndex);
            }
            this.setValuesChanged();
        }
    }

    updateSetValueField(fieldItem: TreeSelectItem, setValueIndex: number) {
        let field = fieldItem ? fieldItem.key : null;
        if (field !== this.SetValues[setValueIndex]?.field) {
            if (field) {
                if (this.isNoObjectFieldVariable(field)) {
                    let fieldType = fieldItem.data["type"] || null;
                    let lookupType = fieldItem.data["lookupType"] || null;
                    let picklistType = fieldItem.data["picklistType"] || null;
                    this.updateSetValueFieldByDeclaredVariableField(field, fieldType, lookupType, picklistType, setValueIndex);
                } else {
                    let objectField = this.getObjectField(field);
                    this.updateSetValueFieldByObjectField(objectField, setValueIndex, field);
                }

                if (fieldItem && fieldItem.data && fieldItem.data["nodeId"]) {
                    this.SetValues[setValueIndex].fieldUsedFrom = fieldItem.data["nodeId"];
                } else {
                    this.SetValues[setValueIndex].fieldUsedFrom = null;
                }
            } else {
                this.resetSetValueField(setValueIndex);
            }
            this.setValuesChanged();
        }
    }

    updateSetValueFieldByObjectField(objectField: ObjectFieldList, setValueIndex: number, field: string | null = null) {
        this.SetValues[setValueIndex].fieldCode = field ? field : (objectField ? objectField.FieldCode : null);
        this.SetValues[setValueIndex].field = field ? field : (objectField ? objectField.FieldName : null);
        this.SetValues[setValueIndex].type = objectField ? objectField.DataTypeCode : null;
        this.SetValues[setValueIndex].lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
        this.SetValues[setValueIndex].picklistType = objectField && objectField.DataTypeCode === FieldTypes.PickList ? objectField.CustomPickListCode : null;
        this.SetValues[setValueIndex].operator = SetValueOperators.Equals;
        this.SetValues[setValueIndex].value = null;
        this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;
    }

    updateSetValueFieldByDeclaredVariableField(field: string, fieldType: string, lookupType: string, picklistType: string, setValueIndex: number) {
        this.SetValues[setValueIndex].fieldCode = field ? field : null;
        this.SetValues[setValueIndex].field = field ? field : null;
        this.SetValues[setValueIndex].type = fieldType;
        this.SetValues[setValueIndex].lookupType = lookupType;
        this.SetValues[setValueIndex].picklistType = picklistType;
        this.SetValues[setValueIndex].operator = this.getSetValueDefaultOperator(fieldType);
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
        this.SetValues[setValueIndex].fieldUsedFrom = null;
        this.SetValues[setValueIndex].valueUsedFrom = null;
    }

    updateSetValueOperator(operatorCode: string, setValueIndex: number) {
        if (operatorCode !== this.SetValues[setValueIndex]?.operator) {
            this.SetValues[setValueIndex].operator = operatorCode;
            this.SetValues[setValueIndex].value = null;
            this.SetValues[setValueIndex].expressionValue = null;
            this.SetValues[setValueIndex].fieldChangedToggle = !this.SetValues[setValueIndex].fieldChangedToggle;
            this.setValuesChanged();
        }
    }

    updateSetValue(value: string, setValueIndex: number, nodeId: string | null = null) {
        if (value !== this.SetValues[setValueIndex]?.value) {
            this.SetValues[setValueIndex].value = value;

            this.SetValues[setValueIndex].valueUsedFrom = nodeId;

            this.setValuesChanged();
        }
    }

    updateSetValueExpression(expressionValue: ExpressionValue, setValueIndex: number) {
        this.SetValues[setValueIndex].expressionValue = expressionValue;
        this.setValuesChanged();
    }

    addSetValue() {
        if (this.IsValidSetValues) {
            let setvalue = new SetValue();
            setvalue.id = this.SetValuesCounter;
            this.SetValues.push(setvalue);
            this.setValuesChanged("add");
        }
    }

    deleteSetValue(setValueIndex: number) {
        let setValue = this.SetValues[setValueIndex];
        let isDeleteDisabled = setValue ? (new SetValueDisabledPipe().transform(setValue.disabled, "d")) : false;
        if (setValue && !isDeleteDisabled) {
            this.SetValues.splice(setValueIndex, 1);
            this.setValuesChanged();
        }
    }

    getSetValueDefaultOperator(setValueType: string) {
        if (setValueType) {
            // if (setValueType.toString().endsWith("[]")) {
            //     return SetValueOperators.EqualsCollection;
            // }
            let types = Object.values(FieldTypes).map((type) => (type as string));
            if (!types.includes(setValueType)) {
                return SetValueOperators.EqualsRecord;
            }
        }
        return SetValueOperators.Equals;
    }

    isNoObjectFieldVariable(field: string) {
        return new IsNoObjectFieldVariablePipe().transform(field);
    }

    getObjectField(field: string) {
        return new ObjectFieldPipe().transform(field);
    }

    initializeSetValuesIds() {
        for (let setValue of this.SetValues) {
            setValue.id = this.SetValuesCounter;
            this.increaseSetValuesCounter();
        }
    }

    increaseSetValuesCounter() {
        this.SetValuesCounter++;
    }
}