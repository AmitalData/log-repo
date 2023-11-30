import { SortDirections } from "Workflow/Constants/SortDirections";
import { ListItem } from "Workflow/Models/ListItem";

export class SortDirectionList {
    public Items: ListItem[] = [];

    constructor() {
        this.setSortDirections();
    }

    private setSortDirections() {
        this.Items = [
            new ListItem(SortDirections.Ascending),
            new ListItem(SortDirections.Descending),
            new ListItem(SortDirections.NotSorted),
        ]
    }
}