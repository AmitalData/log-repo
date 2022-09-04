import { ConditionOperations } from "Workflow/Constants/ConditionOperations";
import { ListItem } from "./ListItem";

export class ConditionOperationsList {
    public ConditionOperations: ListItem[] = [];

    constructor() {
        this.setConditionOperations();
    }

    private setConditionOperations() {
        this.ConditionOperations = [
            new ListItem(ConditionOperations.And),
            new ListItem(ConditionOperations.Or),
        ];
    }
}