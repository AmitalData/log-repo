import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ListItem } from "./ListItem";

export class DataTypesList {
    public Items: ListItem[] = [];

    constructor() {
        this.setDataTypes();
    }

    private setDataTypes() {
        this.Items = [
            new ListItem(FieldTypes.Text),
            new ListItem(FieldTypes.Date),
            new ListItem(FieldTypes.Decimal, "Number"),
            new ListItem(FieldTypes.Boolean),
        ];
    }
}