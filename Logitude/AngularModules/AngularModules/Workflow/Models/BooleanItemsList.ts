
import { BooleanItems } from "Workflow/Constants/BooleanItems";
import { ListItem } from "./ListItem";

export class BooleanItemsList {
    public BooleanItems: ListItem[] = [];

    constructor() {
        this.setBooleanItems();
    }

    private setBooleanItems() {
        this.BooleanItems = [
            new ListItem(BooleanItems.True),
            new ListItem(BooleanItems.False),
        ];
    }
}