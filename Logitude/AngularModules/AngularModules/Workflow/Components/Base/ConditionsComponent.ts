import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Lists/ConditionOperationsList";
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
    @Input() AtLeastOneCondition: boolean = false;
    @Input() IsEntityField: boolean = false;
    @Input() IsEntityFieldValue: boolean = false;
    @Input() FlowObject: any;
    @Input() CurrentNodeId: string;
    @Input() EnableAdd: boolean = true;
    @Input() ShowConditionsOperation: boolean = true;
    @Input() IsOneLevelConditions: boolean = false;
    @Input() IsDummyField: boolean = false;
    @Input() GetFieldFromFirstCondition: boolean = false;

    @Output() ConditionsOperationChange = new EventEmitter<string>();
    @Output() IsValidConditionsChange = new EventEmitter<boolean>();

    public DataContext: any = this;
    public IsValidConditions: boolean = true;
    public ConditionsCounter: number = 1;

    public ComboBoxMaxHeight: number = 120;

    public ConditionOperationsItems: ListItem[] = new ConditionOperationsList().Items;

    constructor() {
        super();
    }

    ngOnInit() {
        this.initializeConditionsIds();
        this.IsValidConditions = this.isValidConditions();
        this.IsValidConditionsChange.emit(this.IsValidConditions);
    }

    ngOnChanges() {
        this.conditionsChanged();
    }

    updateConditionsOperation(conditionsOperation: any) {
        this.ConditionsOperationChange.emit(conditionsOperation ? conditionsOperation.Code : null);
    }

    conditionsChanged(event: any = null) {
        this.IsValidConditions = this.isValidConditions();
        this.IsValidConditionsChange.emit(this.IsValidConditions);
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
            condition.id = this.ConditionsCounter;
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