import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ConditionGroupOperations } from "Workflow/Constants/ConditionGroupOperations";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { Condition } from "Workflow/Models/Condition";
//import { ConditionGroup } from "Workflow/Models/ConditionGroup";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "ConditionGroups",
    templateUrl: "./ConditionGroupsComponent.html"
})

export class ConditionGroupsComponent extends BaseComponent implements OnInit {

    @Input() EntityId: string;
    //@Input() Groups: ConditionGroup[];

    @Input() Conditions: Condition[];

    @Input() IsRootConditions: boolean;

    //@Output() ConditionGroupsChangedEvent = new EventEmitter<ConditionGroup[]>();

    public ConditionGroupOperations: ListItem[] = [
        new ListItem(ConditionGroupOperations.And),
        new ListItem(ConditionGroupOperations.Or),
    ];

    public ConditionOperators: ListItem[] = [
        new ListItem(ConditionOperators.Equals),
        new ListItem(ConditionOperators.NotEquals),
    ];

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    getConditionGroupItem(operationName: string) {
        return this.ConditionGroupOperations.filter(o => o.Name === operationName)[0] || null;
    }

    getConditionOperatorItem(operatorName: string) {
        return this.ConditionOperators.filter(o => o.Name === operatorName)[0] || null;
    }

    updateGroupOperation(operation: any, groupIndex: number) {
        //this.Groups[groupIndex].operation = operation.Name;

        //this.emitConditionGroupsChanged();
    }

    updateConditionOperator(operator: any, conditionIndex: number) {
        this.Conditions[conditionIndex].operator = operator.Name;
    }

    updateConditionGroupOperation(operation: any, conditionIndex: number) {
        this.Conditions[conditionIndex].groupOperation = operation.Name;
    }

    updateCondition(field: any, groupIndex: number, conditionIndex: number) {
        //this.Groups[groupIndex].conditions[conditionIndex].fieldId = field ? field.Id : null;
        //this.Groups[groupIndex].conditions[conditionIndex].field = field ? field.FieldName : null;
        //this.Groups[groupIndex].conditions[conditionIndex].type = field ? field.DataTypeCode : null;

        //this.emitConditionGroupsChanged();
    }

    addCondition(conditionIndex: number, isGroup: boolean) {
        if (conditionIndex === null) {
            this.Conditions.push(new Condition(isGroup));
        } else {
            this.Conditions[conditionIndex].conditions.push(new Condition(isGroup));
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

    // addGroup(groupIndex: number | null = null) {
    //     if (groupIndex === null) {
    //         this.Conditions.push(new Condition(true));
    //     } else {
    //         this.Conditions[groupIndex].conditions.push(new Condition(true));
    //     }
    // }

    // emitConditionGroupsChanged() {
    //     console.log(this.Groups);
    //     //this.ConditionGroupsChangedEvent.emit(this.Groups);
    // }

    getObjectFieldsQueryFilters() {
        let apiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.addAdditionalFilter("ObjectTableId", this.EntityId, null, null, "Equals", false, false, false, "Text");
        return apiQueryFilters;
    }
}