import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ListItem } from "Workflow/Models/ListItem";

export class ConditionOperationsList {
    public Items: ListItem[] = [];

    constructor() {
        this.setConditionOperations();
    }

    private setConditionOperations() {
        this.Items = [
            new ListItem(ConditionOperations.And),
            new ListItem(ConditionOperations.Or),
        ];
    }
}