import { StartEventApplications } from "Workflow/Constants/StartEventApplications";
import { ListItem } from "Workflow/Models/ListItem";

export class StartEventApplicationsList {
    public Items: ListItem[] = [];

    constructor() {
        this.setStartEventApplications();
    }

    private setStartEventApplications() {
        this.Items = [
            new ListItem(StartEventApplications.MicrosoftOffice365, "Microsoft Office 365")
        ]
    }
}