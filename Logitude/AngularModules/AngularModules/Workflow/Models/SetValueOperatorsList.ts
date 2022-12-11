import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { ListItem } from "./ListItem";

export class SetValueOperatorsList {
    private IsCollectionOperators: boolean;
    public Items: ListItem[] = [];

    constructor(isCollectionOperators: boolean) {
        this.IsCollectionOperators = isCollectionOperators;
        this.setSetValueOperators();
    }

    private setSetValueOperators() {
        if (this.IsCollectionOperators) {
            this.Items = [
                new ListItem(SetValueOperators.EqualsCollection),
                new ListItem(SetValueOperators.Add),
                new ListItem(SetValueOperators.AddField)
            ]
        } else {
            this.Items = [
                new ListItem(SetValueOperators.Equals),
                new ListItem(SetValueOperators.EqualsField),
                new ListItem(SetValueOperators.Expression)
            ];
        }
    }
}