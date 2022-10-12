import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { ListItem } from "./ListItem";

export class SetValueOperatorsList {
    public Items: ListItem[] = [];

    constructor() {
        this.setConditionOperators();
    }

    private setConditionOperators() {
        this.Items = [
            new ListItem(SetValueOperators.Equals),
            new ListItem(SetValueOperators.EqualsField),
        ]
    }
}
