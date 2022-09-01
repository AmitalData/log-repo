import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Models/ConditionOperationsList";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "Conditions",
    templateUrl: "./ConditionsComponent.html"
})

export class ConditionsComponent extends BaseComponent implements OnInit, OnChanges {

    @Input() EntityId: string;
    @Input() ShowChangedOperator: boolean;
    @Input() ConditionsOperation: string;
    @Input() Conditions: Condition[];

    @Output() ConditionsOperationChange = new EventEmitter<string>();

    public IsValidConditions: boolean = true;

    public ConditionOperations: ListItem[] = new ConditionOperationsList().ConditionOperations;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    ngOnChanges() {
        this.conditionsChanged();
    }

    updateConditionsOperation(conditionsOperation: any) {
        this.ConditionsOperationChange.emit(conditionsOperation ? conditionsOperation.Code : null);
    }

    conditionsChanged() {
        this.IsValidConditions = this.isValidConditions();
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