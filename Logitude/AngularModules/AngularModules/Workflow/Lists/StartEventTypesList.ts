import { StartEventTypes } from "Workflow/Constants/StartEventTypes";
import { ListItem } from "Workflow/Models/ListItem";

export class StartEventTypesList {
    public Items: ListItem[] = [];

    constructor() {
        this.setStartEventTypes();
    }

    private setStartEventTypes() {
        this.Items = [
            new ListItem(StartEventTypes.NewEmail)
        ]
    }
}