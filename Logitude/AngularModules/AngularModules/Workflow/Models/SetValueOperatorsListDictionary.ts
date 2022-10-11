import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { ListItem } from "./ListItem";

export class SetValueOperatorsListDictionary {
    public ItemsDictionary: { [key: string]: ListItem[] } = {};
    public Items: ListItem[] = [];

    constructor() {
        this.setConditionOperators();
    }

    private setConditionOperators() {
        this.ItemsDictionary = {
            "all": this.Items = [
                new ListItem(SetValueOperators.Equals),
                new ListItem(SetValueOperators.EqualsField),
            ],
            "default": this.Items = [
                new ListItem(SetValueOperators.Equals),
            ]
        }
    }
}
