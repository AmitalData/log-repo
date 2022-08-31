import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ListItem } from "./ListItem";

export class ConditionOperatorsList {
    public ConditionOperators: ListItem[] = [];
    private ConditionFieldType: string;
    private ShowChangedOperator: boolean;

    constructor(conditionFieldType: string, showChangedOperator: boolean) {
        this.ConditionFieldType = conditionFieldType;
        this.ShowChangedOperator = showChangedOperator;
        this.setConditionOperators();
    }

    private setConditionOperators() {
        switch (this.ConditionFieldType) {

            case FieldTypes.Text:
            case FieldTypes.NText:
                this.ConditionOperators = [
                    new ListItem(ConditionOperators.Equals),
                    new ListItem(ConditionOperators.NotEquals),
                    new ListItem(ConditionOperators.Contains),
                    new ListItem(ConditionOperators.StartsWith),
                    new ListItem(ConditionOperators.EndsWith),
                    new ListItem(ConditionOperators.IsEmpty),
                    new ListItem(ConditionOperators.Changed)
                ];
                break;

            case FieldTypes.LookUp:
                this.ConditionOperators = [
                    new ListItem(ConditionOperators.Equals),
                    new ListItem(ConditionOperators.NotEquals),
                    new ListItem(ConditionOperators.IsEmpty),
                    new ListItem(ConditionOperators.Changed)
                ];
                break;

            case FieldTypes.Boolean:
                this.ConditionOperators = [
                    new ListItem(ConditionOperators.Equals),
                    new ListItem(ConditionOperators.NotEquals),
                    new ListItem(ConditionOperators.Changed)
                ];
                break;

            default:
                this.ConditionOperators = [
                    new ListItem(ConditionOperators.Equals)
                ];
                break;

        }

        if (!this.ShowChangedOperator) {
            this.ConditionOperators = this.ConditionOperators.filter(o => o.Code !== ConditionOperators.Changed);
        }
    }
}