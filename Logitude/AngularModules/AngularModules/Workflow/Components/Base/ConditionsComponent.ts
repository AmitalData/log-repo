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

    public DataContext: any = this;
    public IsValidConditions: boolean = true;
    public ConditionsCounter: number = 1;

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().Items;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeConditionsIds();
    }

    ngOnChanges() {
        this.conditionsChanged();
    }

    updateConditionsOperation(conditionsOperation: any) {
        this.ConditionsOperationChange.emit(conditionsOperation ? conditionsOperation.Code : null);
    }

    conditionsChanged(event: any = null) {
        this.IsValidConditions = this.isValidConditions();
        if (event === "add") {
            this.increaseConditionsCounter();
        }
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

    initializeConditionsIds(conditions: Condition[] | null = null) {
        for (let condition of (conditions || this.Conditions)) {
            if (condition.id === undefined || condition.id === null) {
                condition.id = this.ConditionsCounter;
            }
            this.increaseConditionsCounter();
            if (condition.isGroup && condition.conditions && condition.conditions.length > 0) {
                this.initializeConditionsIds(condition.conditions);
            }
        }
    }

    increaseConditionsCounter() {
        this.ConditionsCounter++;
    }
}