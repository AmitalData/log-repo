import { Component, Input, OnInit } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ConditionGroupOperations } from "Workflow/Constants/ConditionGroupOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { Condition } from "Workflow/Models/Condition";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "ConditionGroups",
    templateUrl: "./ConditionGroupsComponent.html"
})

export class ConditionGroupsComponent extends BaseComponent implements OnInit {

    @Input() EntityId: string;
    @Input() Conditions: Condition[];
    @Input() IsRootConditions: boolean = true;

    public ObjectFields: any = {};

    public IsEmptyOperator = ConditionOperators.IsEmpty;

    public ConditionGroupOperations: ListItem[] = [
        new ListItem(ConditionGroupOperations.And),
        new ListItem(ConditionGroupOperations.Or),
    ];

    public ConditionOperators: ListItem[] = [
        new ListItem(ConditionOperators.Equals),
        new ListItem(ConditionOperators.NotEquals),
        new ListItem(ConditionOperators.IsEmpty),
    ];

    public BooleanFieldValues: ListItem[] = [
        new ListItem("True"),
        new ListItem("False"),
    ];

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    updateConditionGroupOperation(operationCode: any, conditionIndex: number) {
        if (operationCode !== this.Conditions[conditionIndex]?.groupOperation) {
            this.Conditions[conditionIndex].groupOperation = operationCode;
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
        }
    }

    updateConditionOperator(operatorCode: any, conditionIndex: number) {
        if (operatorCode !== this.Conditions[conditionIndex]?.operator) {
            if (operatorCode === ConditionOperators.IsEmpty || this.Conditions[conditionIndex]?.operator === ConditionOperators.IsEmpty) {
                this.updateConditionValue(null, conditionIndex);
            }
            this.Conditions[conditionIndex].operator = operatorCode;
        }
    }

    updateConditionValue(value: string, conditionIndex: number) {
        if (value !== this.Conditions[conditionIndex]?.value) {
            this.Conditions[conditionIndex].value = value?.toString();
        }
    }

    addCondition(conditionIndex: number, isGroup: boolean) {
        if (this.isValidConditions()) {
            if (conditionIndex === null) {
                this.Conditions.push(new Condition(isGroup));
            } else {
                this.Conditions[conditionIndex].conditions.push(new Condition(isGroup));
            }
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
    }

    getObjectFieldsQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("ObjectTableId", this.EntityId, null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }

    getListItem(itemsList: ListItem[], itemCode: string) {
        return itemsList.filter(o => o.Code === itemCode)[0] || null;
    }

    isValidConditions(conditions: Condition[] | null = null) {
        let result = true;
        for (let condition of (conditions || this.Conditions)) {
            if (!condition.fieldCode || !condition.value) {
                result = false;
                break;
            }
            if (condition.isGroup && condition.conditions && condition.conditions.length > 0) {
                result = this.isValidConditions(condition.conditions);
                if (!result) {
                    break;
                }
            }
        }
        return result;
    }
}