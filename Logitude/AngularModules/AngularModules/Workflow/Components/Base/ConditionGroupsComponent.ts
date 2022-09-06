import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { BooleanItems } from "Workflow/Constants/BooleanItems";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { DateTimeValueExpressions } from "Workflow/Constants/DateTimeValueExpressions";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanItemsList } from "Workflow/Models/BooleanItemsList";
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

    @Output() ConditionsChangedEvent = new EventEmitter();

    public ObjectFields: any = {};

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().ConditionOperations;

    public BooleanItems: ListItem[] = new BooleanItemsList().BooleanItems;

    public DateTimeValueExpressionsItems: ListItem[] = new DateTimeValueExpressionsList().DateTimeValueExpressions;

    public ConditionOperatorsItems = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ConditionOperatorsLists;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    public DateTimeValueExpressions = DateTimeValueExpressions;

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    ngOnChanges() {
        this.ConditionOperatorsItems = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ConditionOperatorsLists;
    }

    updateConditionGroupOperation(operationCode: string, conditionIndex: number) {
        if (operationCode !== this.Conditions[conditionIndex]?.groupOperation) {
            this.Conditions[conditionIndex].groupOperation = operationCode;

            this.emitConditionsChanged();
        }
    }

    updateConditionField(field: any, conditionIndex: number) {
        if (field) {
            this.ObjectFields[field.FieldCode] = field;
        }

        if (field?.FieldCode !== this.Conditions[conditionIndex]?.fieldCode) {
            this.Conditions[conditionIndex].fieldCode = field ? field.FieldCode : null;
            this.Conditions[conditionIndex].field = field ? field.FieldName : null;
            this.Conditions[conditionIndex].type = field ? field.DataTypeCode : null;
            this.Conditions[conditionIndex].operator = ConditionOperators.Equals;
            this.Conditions[conditionIndex].value = null;
            this.Conditions[conditionIndex].valueExpression = this.isDateTimeType(field?.DataTypeCode) ? DateTimeValueExpressions.Date : null;
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;

            this.emitConditionsChanged();
        }
    }

    updateConditionOperator(operatorCode: string, conditionIndex: number) {
        if (operatorCode !== this.Conditions[conditionIndex]?.operator) {

            if (this.isNoValueOperator(operatorCode) || this.isNoValueOperator(this.Conditions[conditionIndex]?.operator)) {
                let value = this.isNoValueOperator(operatorCode) ? BooleanItems.True : null;
                this.updateConditionValue(value, conditionIndex);

                let valueExpression = this.isNoValueOperator(operatorCode) ? null : (this.isDateTimeType(this.Conditions[conditionIndex]?.type) ? DateTimeValueExpressions.Date : null);
                this.updateConditionValueExpression(valueExpression, conditionIndex, false);
            }

            this.Conditions[conditionIndex].operator = operatorCode;

            this.emitConditionsChanged();
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
                this.updateConditionValue(null, conditionIndex);
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
            firstChildCondition.isGroup = true;
            firstChildCondition.groupOperation = condition.groupOperation;
            firstChildCondition.conditions = condition.conditions.slice(1);
            this.Conditions[conditionIndex] = firstChildCondition;
        } else {
            this.Conditions.splice(conditionIndex, 1);
        }

        this.emitConditionsChanged();
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
        let objectField = this.ObjectFields[fieldCode];
        return objectField && (objectField.DataTypeCode === FieldTypes.DateTime || objectField.DataTypeCode === FieldTypes.Date);
    }

    isDateTimeType(fieldType: string) {
        return fieldType && (fieldType === FieldTypes.DateTime || fieldType === FieldTypes.Date);
    }

    emitConditionsChanged(event: any = null) {
        this.ConditionsChangedEvent.emit(event);
    }
}