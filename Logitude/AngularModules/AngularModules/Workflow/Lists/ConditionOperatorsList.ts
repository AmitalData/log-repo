import { ConditionOperators } from "Workflow/Constants/ConditionOperators";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ListItem } from "Workflow/Models/ListItem";

export class ConditionOperatorsList {
    public Items: ListItem[] = [];
    private ConditionFieldType: string;
    private ShowChangedOperator: boolean;

    constructor(conditionFieldType: string, showChangedOperator: boolean) {
        this.ConditionFieldType = conditionFieldType;
        this.ShowChangedOperator = showChangedOperator;
        this.setConditionOperators();
    }

    private setConditionOperators() {
        if (this.ConditionFieldType) {
            switch (this.ConditionFieldType) {
                case FieldTypes.Text:
                case FieldTypes.NText:
                    this.Items = [
                        new ListItem(ConditionOperators.Equals),
                        new ListItem(ConditionOperators.EqualsField),
                        new ListItem(ConditionOperators.NotEquals),
                        new ListItem(ConditionOperators.NotEqualsField),
                        new ListItem(ConditionOperators.Contains),
                        new ListItem(ConditionOperators.ContainsField),
                        new ListItem(ConditionOperators.NotContains),
                        new ListItem(ConditionOperators.NotContainsField),
                        new ListItem(ConditionOperators.StartsWith),
                        new ListItem(ConditionOperators.StartsWithField),
                        new ListItem(ConditionOperators.EndsWith),
                        new ListItem(ConditionOperators.EndsWithField),
                        new ListItem(ConditionOperators.IsEmpty),
                        new ListItem(ConditionOperators.Changed)
                    ];
                    break;
                case FieldTypes.LookUp:
                    this.Items = [
                        new ListItem(ConditionOperators.Equals),
                        new ListItem(ConditionOperators.EqualsField),
                        new ListItem(ConditionOperators.NotEquals),
                        new ListItem(ConditionOperators.NotEqualsField),
                        new ListItem(ConditionOperators.IsEmpty),
                        new ListItem(ConditionOperators.Changed)
                    ];
                    break;
                case FieldTypes.PickList:
                    this.Items = [
                        new ListItem(ConditionOperators.Equals),
                        new ListItem(ConditionOperators.NotEquals),
                        new ListItem(ConditionOperators.IsEmpty),
                        new ListItem(ConditionOperators.Changed)
                    ];
                    break;
                case FieldTypes.Boolean:
                    this.Items = [
                        new ListItem(ConditionOperators.Equals),
                        new ListItem(ConditionOperators.EqualsField),
                        new ListItem(ConditionOperators.NotEquals),
                        new ListItem(ConditionOperators.NotEqualsField),
                        new ListItem(ConditionOperators.Changed)
                    ];
                    break;
                case FieldTypes.DateTime:
                case FieldTypes.Date:
                case FieldTypes.BigInteger:
                case FieldTypes.Decimal:
                case FieldTypes.Double:
                case FieldTypes.Integer:
                case FieldTypes.SigDouble:
                case FieldTypes.UnsDecimal:
                case FieldTypes.UnsInteger:
                    this.Items = [
                        new ListItem(ConditionOperators.Equals),
                        new ListItem(ConditionOperators.EqualsField),
                        new ListItem(ConditionOperators.NotEquals),
                        new ListItem(ConditionOperators.NotEqualsField),
                        new ListItem(ConditionOperators.GreaterThan),
                        new ListItem(ConditionOperators.GreaterThanField),
                        new ListItem(ConditionOperators.LessThan),
                        new ListItem(ConditionOperators.LessThanField),
                        new ListItem(ConditionOperators.GreaterThanOrEquals),
                        new ListItem(ConditionOperators.GreaterThanOrEqualsField),
                        new ListItem(ConditionOperators.LessThanOrEquals),
                        new ListItem(ConditionOperators.LessThanOrEqualsField),
                        new ListItem(ConditionOperators.IsEmpty),
                        new ListItem(ConditionOperators.Changed)
                    ];
                    break;
                default:
                    this.Items = [
                        new ListItem(ConditionOperators.Equals)
                    ];
                    break;
            }

            if (!this.ShowChangedOperator) {
                this.Items = this.Items.filter(i => i.Code !== ConditionOperators.Changed);
            }
        }
    }
}