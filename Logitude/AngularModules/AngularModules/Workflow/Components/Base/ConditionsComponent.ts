import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Condition } from "Workflow/Models/Condition";
import { ConditionOperationsList } from "Workflow/Models/ConditionOperationsList";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "Conditions",
    templateUrl: "./ConditionsComponent.html"
})

export class ConditionsComponent extends BaseComponent implements OnInit {

    @Input() EntityId: string;
    @Input() ShowChangedOperator: boolean;
    @Input() ConditionsOperation: string;
    @Input() Conditions: Condition[];

    @Output() ConditionsOperationChange = new EventEmitter<string>();

    public ConditionOperations: ListItem[] = new ConditionOperationsList().ConditionOperations;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    updateConditionsOperation(conditionsOperation: any) {
        this.ConditionsOperationChange.emit(conditionsOperation ? conditionsOperation.Code : null);
    }
}