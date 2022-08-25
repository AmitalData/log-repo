import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ConditionGroupOperations } from "Workflow/Constants/ConditionGroupOperations";
import { Condition } from "Workflow/Models/Condition";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "Conditions",
    templateUrl: "./ConditionsComponent.html"
})

export class ConditionsComponent extends BaseComponent implements OnInit {

    @Input() EntityId: string;
    @Input() ConditionsOperation: string;
    @Input() Conditions: Condition[];

    @Output() ConditionsOperationChange = new EventEmitter<string>();

    public ConditionGroupOperations: ListItem[] = [
        new ListItem(ConditionGroupOperations.And),
        new ListItem(ConditionGroupOperations.Or),
    ];

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    updateConditionsOperation(conditionsOperation: any) {
        this.ConditionsOperationChange.emit(conditionsOperation ? conditionsOperation.Name : null);
    }

    getConditionGroupItem(operationName: string) {
        return this.ConditionGroupOperations.filter(o => o.Name === operationName)[0] || null;
    }
}