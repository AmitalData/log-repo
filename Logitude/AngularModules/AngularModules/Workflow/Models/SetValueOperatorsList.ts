import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { ListItem } from "./ListItem";

export class SetValueOperatorsList {
    public Items: ListItem[] = [];

    constructor() {
        this.setSetValueOperators();
    }

    private setSetValueOperators() {
        this.Items = [
            new ListItem(SetValueOperators.Equals),
            new ListItem(SetValueOperators.EqualsField),
        ]
    }
}
