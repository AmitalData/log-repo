import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { BooleanValues } from "Workflow/Constants/BooleanValues";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Models/ConditionOperationsList";
import { ConditionOperatorsListsDictionary } from "Workflow/Models/ConditionOperatorsListsDictionary";
import { DateTimeValueExpressionsList } from "Workflow/Models/DateTimeValueExpressionsList";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { TreeSelectDataFilter } from "Workflow/Models/Types";

@Component({
    selector: "ConditionGroups",
    templateUrl: "./ConditionGroupsComponent.html"
})

export class ConditionGroupsComponent extends BaseComponent implements OnInit, OnChanges {

    @Input() EntityId: string;
    @Input() ShowChangedOperator: boolean = true;
    @Input() Conditions: Condition[];
    @Input() IsRootConditions: boolean = true;
    @Input() IsValidConditions: boolean = true;
    @Input() ConditionsCounter: number = 1;
    @Input() AtLeastOneCondition: boolean;

    @Input() ShowFlowVariablesTree: boolean = false;
    @Input() FlowObject: any;
    @Input() FlowObjectFields: ObjectFieldPM[];
    @Input() CurrentNodeId: string;

    @Output() ConditionsChangedEvent = new EventEmitter();

    public DataContext: any = this;
    //public ObjectFieldsDictionary: any = {};
    public DateTimeValueExpressions = DateTimeValueExpressions;

    public FlowVariablesTreeItems: TreeSelectItem[];

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().Items;
    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;
    public DateTimeValueExpressionsItems: ListItem[] = new DateTimeValueExpressionsList().Items;
    public ConditionOperatorsItemsDictionary = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ItemsDictionary;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeFlowVariablesTreeItems();
    }

    ngOnChanges() {
        this.ConditionOperatorsItemsDictionary = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ItemsDictionary;
    }

    initializeFlowVariablesTreeItems() {
        if (this.ShowFlowVariablesTree) {
            this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId).Items;
        }
    }

    updateConditionGroupOperation(operationCode: string, conditionIndex: number) {
        if (operationCode !== this.Conditions[conditionIndex]?.groupOperation) {
            this.Conditions[conditionIndex].groupOperation = operationCode;
            this.emitConditionsChanged();
        }
    }

    updateConditionField(objectField: ObjectFieldPM, conditionIndex: number) {
        //this.saveInObjectFieldsDictionary(objectField);
        if (objectField?.FieldCode !== this.Conditions[conditionIndex]?.fieldCode) {
            this.Conditions[conditionIndex].fieldCode = objectField ? objectField.FieldCode : null;
            this.Conditions[conditionIndex].field = objectField ? objectField.FieldName : null;
            this.Conditions[conditionIndex].type = objectField ? objectField.DataTypeCode : null;
            this.Conditions[conditionIndex].lookupType = objectField && objectField.DataTypeCode === FieldTypes.LookUp ? ObjectTables.getNameById(objectField.LookUpTableId) : null;
            this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
            this.Conditions[conditionIndex].valueExpression = this.isDateTimeType(objectField?.DataTypeCode) ? DateTimeValueExpressions.Date : null;
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
            this.emitConditionsChanged();
        }
    }

    // saveInObjectFieldsDictionary(objectField: ObjectFieldPM) {
    //     if (objectField) {
    //         this.ObjectFieldsDictionary[objectField.FieldCode] = objectField;
    //     }
    // }

    updateConditionOperator(operatorCode: string, conditionIndex: number) {
        if (operatorCode !== this.Conditions[conditionIndex]?.operator) {
            this.updateConditionValueAccordingToUpdateOperator(operatorCode, conditionIndex);
        }
    }

    updateConditionValueAccordingToUpdateOperator(operatorCode: string, conditionIndex: number) {
        if (this.isNoValueOperator(operatorCode)) {
            let value = this.isNoValueOperator(operatorCode) ? BooleanValues.True : null;
            let valueExpression = this.isDateTimeType(this.Conditions[conditionIndex]?.type) ? DateTimeValueExpressions.Date : null;
            this.updateConditionValue(value, conditionIndex);
            this.updateConditionValueExpression(valueExpression, conditionIndex, false);
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
        }
        else if ((this.isFieldCompareOperator(operatorCode) && !this.isFieldCompareOperator(this.Conditions[conditionIndex]?.operator)) ||
            (!this.isFieldCompareOperator(operatorCode) && this.isFieldCompareOperator(this.Conditions[conditionIndex]?.operator))) {
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
            let valueExpression = this.isDateTimeType(this.Conditions[conditionIndex]?.type) ? DateTimeValueExpressions.Date : null;
            this.updateConditionValueExpression(valueExpression, conditionIndex, false);
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
        } else if (!this.isNoValueOperator(operatorCode) && this.isNoValueOperator(this.Conditions[conditionIndex]?.operator)) {
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueCode = null;
        }

        this.Conditions[conditionIndex].operator = operatorCode;
        this.emitConditionsChanged();
    }

    updateConditionValue(value: string, conditionIndex: number) {
        if (value !== this.Conditions[conditionIndex]?.value) {
            this.Conditions[conditionIndex].value = value ? value.toString() : null;
            this.emitConditionsChanged();
        }
    }

    updateConditionFieldValue(objectField: ObjectFieldPM, conditionIndex: number) {
        //this.saveInObjectFieldsDictionary(objectField);
        if (objectField?.FieldCode !== this.Conditions[conditionIndex]?.valueCode) {
            this.Conditions[conditionIndex].value = objectField ? objectField.FieldName : null;
            this.Conditions[conditionIndex].valueCode = objectField ? objectField.FieldCode : null;
            this.emitConditionsChanged();
        }
    }

    updateConditionValueExpression(valueExpressionCode: string, conditionIndex: number, resetValue: boolean = true) {
        if (valueExpressionCode !== this.Conditions[conditionIndex]?.valueExpression) {
            if (resetValue) {
                let value = this.isDateTimeType(this.Conditions[conditionIndex]?.type) && valueExpressionCode === DateTimeValueExpressions.PlusMinusToday ? "0" : null;
                this.updateConditionValue(value, conditionIndex);
            }
            this.Conditions[conditionIndex].valueExpression = valueExpressionCode;
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
            this.emitConditionsChanged();
        }
    }

    addCondition(conditionIndex: number, isGroup: boolean) {
        if (this.IsValidConditions) {
            let condition = new Condition(isGroup);
            condition.id = this.ConditionsCounter;
            if (conditionIndex === null) {
                this.Conditions.push(condition);
            } else {
                this.Conditions[conditionIndex].conditions.push(condition);
            }
            this.emitConditionsChanged("add");
        }
    }

    deleteCondition(conditionIndex: number, isGroup: boolean) {
        let condition = this.Conditions[conditionIndex];
        if (!condition.isDisabled) {
            if (isGroup && condition.conditions.length > 0) {
                let firstChildCondition = condition.conditions[0];
                if (!firstChildCondition.isGroup) {
                    firstChildCondition.isGroup = true;
                    firstChildCondition.groupOperation = condition.groupOperation;
                    firstChildCondition.conditions = condition.conditions.slice(1);
                }
                this.Conditions[conditionIndex] = firstChildCondition;
            } else {
                this.Conditions.splice(conditionIndex, 1);
            }
            this.emitConditionsChanged();
        }
    }

    emitConditionsChanged(event: any = null) {
        this.ConditionsChangedEvent.emit(event);
    }

    getObjectFieldsQueryFilters() {
        return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(this.EntityId, null, null);
    }

    getObjectFieldsValueQueryFilters(objectField: ObjectFieldPM) {
        let lookupTableIdFilterValue = (objectField && objectField.DataTypeCode === FieldTypes.LookUp) ? objectField.LookUpTableId : null;
        return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(this.EntityId, objectField.DataTypeCode, lookupTableIdFilterValue);
    }

    isFieldCompareOperator(operatorCode: string) {
        return operatorCode && operatorCode.endsWith("<field>");
    }

    isNoValueOperator(operatorCode: string) {
        return operatorCode === ConditionOperators.IsEmpty || operatorCode === ConditionOperators.Changed;
    }

    isDateTimeField(fieldCode: string) {
        //let objectField = this.ObjectFieldsDictionary[fieldCode];
        let objectField = this.getObjectField(fieldCode);
        return objectField && (objectField.DataTypeCode === FieldTypes.DateTime || objectField.DataTypeCode === FieldTypes.Date);
    }

    isDateTimeType(fieldType: string) {
        return fieldType && (fieldType === FieldTypes.DateTime || fieldType === FieldTypes.Date);
    }

    getObjectField(fieldCode: string) {
        return this.FlowObjectFields.find(o => o.FieldCode === fieldCode);
    }

    getTreeSelectDataFilters(conditionIndex: number) {
        let condition = this.Conditions[conditionIndex];
        let dataFilters: TreeSelectDataFilter[] = [];
        if (condition.type) {
            dataFilters.push({ Key: "type", Value: condition.type });
        }
        if (condition.lookupType) {
            dataFilters.push({ Key: "lookupType", Value: condition.lookupType });
        }
        return dataFilters;
    }
}