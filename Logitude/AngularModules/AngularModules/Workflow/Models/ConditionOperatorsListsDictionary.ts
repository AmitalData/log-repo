import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ConditionOperatorsList } from "./ConditionOperatorsList";
import { ListItem } from "./ListItem";

export class ConditionOperatorsListsDictionary {
    public ItemsDictionary: { [key: string]: ListItem[] } = {};
    private ShowChangedOperator: boolean;

    constructor(showChangedOperator: boolean) {
        this.ShowChangedOperator = showChangedOperator;
        this.setConditionOperatorsLists();
    }

    private setConditionOperatorsLists() {
        this.ItemsDictionary = {
            [FieldTypes.LookUp]: this.getConditionOperators(FieldTypes.LookUp),
            [FieldTypes.PickList]: this.getConditionOperators(FieldTypes.PickList),
            [FieldTypes.NText]: this.getConditionOperators(FieldTypes.NText),
            [FieldTypes.Text]: this.getConditionOperators(FieldTypes.Text),
            [FieldTypes.Boolean]: this.getConditionOperators(FieldTypes.Boolean),
            [FieldTypes.DateTime]: this.getConditionOperators(FieldTypes.DateTime),
            [FieldTypes.Date]: this.getConditionOperators(FieldTypes.Date),
            [FieldTypes.BigInteger]: this.getConditionOperators(FieldTypes.BigInteger),
            [FieldTypes.Decimal]: this.getConditionOperators(FieldTypes.Decimal),
            [FieldTypes.Double]: this.getConditionOperators(FieldTypes.Double),
            [FieldTypes.Integer]: this.getConditionOperators(FieldTypes.Integer),
            [FieldTypes.SigDouble]: this.getConditionOperators(FieldTypes.SigDouble),
            [FieldTypes.UnsDecimal]: this.getConditionOperators(FieldTypes.UnsDecimal),
            [FieldTypes.UnsInteger]: this.getConditionOperators(FieldTypes.UnsInteger),
            "default": this.getConditionOperators(null)
        };
    }

    private getConditionOperators(conditionFieldType: string) {
        return new ConditionOperatorsList(conditionFieldType, this.ShowChangedOperator).Items;
    }

}