import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { BooleanItems } from "Workflow/Constants/BooleanItems";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { BooleanItemsList } from "Workflow/Models/BooleanItemsList";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Models/ConditionOperationsList";
import { ConditionOperatorsListsDictionary } from "Workflow/Models/ConditionOperatorsListsDictionary";
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

    @Output() ConditionsChangedEvent = new EventEmitter();

    public ObjectFields: any = {};

    public ConditionOperations: ListItem[] = new ConditionOperationsList().ConditionOperations;

    public BooleanItems: ListItem[] = new BooleanItemsList().BooleanItems;

    public ConditionOperators = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ConditionOperatorsLists;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    ngOnChanges() {
        this.ConditionOperators = new ConditionOperatorsListsDictionary(this.ShowChangedOperator).ConditionOperatorsLists;
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
            this.Conditions[conditionIndex].fieldChangedToggle = !this.Conditions[conditionIndex].fieldChangedToggle;

            this.emitConditionsChanged();
        }
    }

    updateConditionOperator(operatorCode: string, conditionIndex: number) {
        if (operatorCode !== this.Conditions[conditionIndex]?.operator) {

            if (this.isNoValueOperator(operatorCode) || this.isNoValueOperator(this.Conditions[conditionIndex]?.operator)) {
                let value = this.isNoValueOperator(operatorCode) ? BooleanItems.True : null;
                this.updateConditionValue(value, conditionIndex);
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

    addCondition(conditionIndex: number, isGroup: boolean) {
        if (this.IsValidConditions) {
            if (conditionIndex === null) {
                this.Conditions.push(new Condition(isGroup));
            } else {
                this.Conditions[conditionIndex].conditions.push(new Condition(isGroup));
            }

            this.emitConditionsChanged();
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

    emitConditionsChanged() {
        this.ConditionsChangedEvent.emit();
    }
}