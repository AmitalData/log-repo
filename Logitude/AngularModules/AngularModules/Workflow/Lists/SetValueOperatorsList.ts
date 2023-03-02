import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { SetValueOperators } from "Workflow/Constants/SetValueOperators";
import { ListItem } from "Workflow/Models/ListItem";

export class SetValueOperatorsList {
    private SetValueType: string;
    public Items: ListItem[] = [];

    constructor(setValueType: string) {
        this.SetValueType = setValueType;

        if (this.SetValueType) {
            this.setSetValueOperators();
        }
    }

    private setSetValueOperators() {
        let types = Object.values(FieldTypes).map((type) => (type as string));
        if (!types.includes(this.SetValueType.replace("[]", ""))) {
            this.setRecordSetValueOperators();
        } else {
            this.setNotRecordSetValueOperators();
        }
    }

    private setRecordSetValueOperators() {
        if (this.SetValueType.toString().endsWith("[]")) {
            this.Items = [
                //new ListItem(SetValueOperators.EqualsCollection)
            ];
        } else {
            this.Items = [
                new ListItem(SetValueOperators.EqualsRecord)
            ];
        }
    }

    private setNotRecordSetValueOperators() {
        if (this.SetValueType.toString().endsWith("[]")) {
            this.Items = [
                //new ListItem(SetValueOperators.EqualsCollection),
                //new ListItem(SetValueOperators.Add),
                //new ListItem(SetValueOperators.AddField)
            ];
        } else {
            this.Items = [
                new ListItem(SetValueOperators.Equals),
                new ListItem(SetValueOperators.EqualsField),
                new ListItem(SetValueOperators.Expression)
            ];
        }
    }
}