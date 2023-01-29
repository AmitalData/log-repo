
import { BooleanValues } from "Workflow/Constants/BooleanValues";
import { ListItem } from "Workflow/Models/ListItem";

export class BooleanValuesList {
    public Items: ListItem[] = [];

    constructor() {
        this.setBooleanValues();
    }

    private setBooleanValues() {
        this.Items = [
            new ListItem(BooleanValues.True),
            new ListItem(BooleanValues.False),
        ];
    }
}