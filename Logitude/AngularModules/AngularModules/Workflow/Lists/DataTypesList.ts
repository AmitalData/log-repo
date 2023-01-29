import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ListItem } from "Workflow/Models/ListItem";

export class DataTypesList {

    private IsPrimitive: boolean;

    public Items: ListItem[] = [];

    constructor(isPrimitive: boolean = false) {
        this.IsPrimitive = isPrimitive;
        this.setDataTypes();
    }

    private setDataTypes() {
        this.Items = [
            new ListItem(FieldTypes.Text),
            new ListItem(FieldTypes.Date),
            new ListItem(FieldTypes.Decimal, "Number"),
            new ListItem(FieldTypes.Boolean)
        ];

        if (!this.IsPrimitive) {
            this.Items.push(new ListItem(FieldTypes.Record));
        }
    }
}