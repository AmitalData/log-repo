import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { BooleanValues } from "Workflow/Constants/BooleanValues";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Models/ConditionOperationsList";
import { ConditionOperatorsListsDictionary } from "Workflow/Models/ConditionOperatorsListsDictionary";
import { DateTimeValueExpressionsList } from "Workflow/Models/DateTimeValueExpressionsList";
import { ListItem } from "Workflow/Models/ListItem";

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

    @Output() ConditionsChangedEvent = new EventEmitter();

    public DataContext: any = this;
    public ObjectFieldsDictionary: any = {};
    public DateTimeValueExpressions = DateTimeValueExpressions;

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().Items;
    public BooleanValuesItems: ListItem[] = new BooleanValuesList().Items;
    public DateTimeValueExpressionsItems: ListItem[] = new DateTimeValueExpressionsList().Items;
    public ConditionOperatorsItemsDictionary = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ItemsDictionary;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    constructor() {
        super();
    }

    ngOnInit() {

    }

    ngOnChanges() {
        this.ConditionOperatorsItemsDictionary = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ItemsDictionary;
    }

    updateConditionGroupOperation(operationCode: string, conditionIndex: number) {
        if (operationCode !== this.Conditions[conditionIndex]?.groupOperation) {
            this.Conditions[conditionIndex].groupOperation = operationCode;
            this.emitConditionsChanged();
        }
    }

    updateConditionField(objectField: ObjectFieldPM, conditionIndex: number) {
        this.saveInObjectFieldsDictionary(objectField);
        if (objectField?.FieldCode !== this.Conditions[conditionIndex]?.fieldCode) {
            this.Conditions[conditionIndex].fieldCode = objectField ? objectField.FieldCode : null;
            this.Conditions[conditionIndex].field = objectField ? objectField.FieldName : null;
            this.Conditions[conditionIndex].type = objectField ? objectField.DataTypeCode : null;
            this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueExpression = this.isDateTimeType(objectField?.DataTypeCode) ? DateTimeValueExpressions.Date : null;
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;
            this.emitConditionsChanged();
        }
    }

    saveInObjectFieldsDictionary(objectField: ObjectFieldPM) {
        if (objectField) {
            this.ObjectFieldsDictionary[objectField.FieldCode] = objectField;
        }
    }

    updateConditionOperator(operatorCode: string, conditionIndex: number) {
        if (operatorCode !== this.Conditions[conditionIndex]?.operator) {
            this.updateConditionValueAccordingToUpdateOperator(operatorCode, conditionIndex);
            this.Conditions[conditionIndex].operator = operatorCode;
            this.emitConditionsChanged();
        }
    }

    updateConditionValueAccordingToUpdateOperator(operatorCode: string, conditionIndex: number) {
        if (this.isNoValueOperator(operatorCode) || this.isNoValueOperator(this.Conditions[conditionIndex]?.operator)) {
            let value = this.isNoValueOperator(operatorCode) ? BooleanValues.True : null;
            let valueExpression = this.isDateTimeType(this.Conditions[conditionIndex]?.type) ? DateTimeValueExpressions.Date : null;
            this.updateConditionValue(value, conditionIndex);
            this.updateConditionValueExpression(valueExpression, conditionIndex, false);
        }
    }

    updateConditionValue(value: string, conditionIndex: number) {
        if (value !== this.Conditions[conditionIndex]?.value) {
            this.Conditions[conditionIndex].value = value ? value.toString() : null;
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

    emitConditionsChanged(event: any = null) {
        this.ConditionsChangedEvent.emit(event);
    }

    getObjectFieldsQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("ObjectTableId", this.EntityId, null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }

    isNoValueOperator(operatorCode: string) {
        return operatorCode === ConditionOperators.IsEmpty || operatorCode === ConditionOperators.Changed;
    }

    isDateTimeField(fieldCode: string) {
        let objectField = this.ObjectFieldsDictionary[fieldCode];
        return objectField && (objectField.DataTypeCode === FieldTypes.DateTime || objectField.DataTypeCode === FieldTypes.Date);
    }

    isDateTimeType(fieldType: string) {
        return fieldType && (fieldType === FieldTypes.DateTime || fieldType === FieldTypes.Date);
    }
}