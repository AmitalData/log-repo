import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ConditionOperatorsList } from "./ConditionOperatorsList";
import { ListItem } from "./ListItem";

export class ConditionOperatorsListsDictionary {
    public ConditionOperatorsLists: { [key: string]: ListItem[] } = {};
    private ShowChangedOperator: boolean;

    constructor(showChangedOperator: boolean) {
        this.ShowChangedOperator = showChangedOperator;
        this.setConditionOperatorsLists();
    }

    private setConditionOperatorsLists() {
        this.ConditionOperatorsLists = {
            [FieldTypes.LookUp]: new ConditionOperatorsList(FieldTypes.LookUp, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.NText]: new ConditionOperatorsList(FieldTypes.NText, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.Text]: new ConditionOperatorsList(FieldTypes.Text, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.Boolean]: new ConditionOperatorsList(FieldTypes.Boolean, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.DateTime]: new ConditionOperatorsList(FieldTypes.DateTime, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.Date]: new ConditionOperatorsList(FieldTypes.Date, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.BigInteger]: new ConditionOperatorsList(FieldTypes.BigInteger, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.Decimal]: new ConditionOperatorsList(FieldTypes.Decimal, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.Double]: new ConditionOperatorsList(FieldTypes.Double, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.Integer]: new ConditionOperatorsList(FieldTypes.Integer, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.SigDouble]: new ConditionOperatorsList(FieldTypes.SigDouble, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.UnsDecimal]: new ConditionOperatorsList(FieldTypes.UnsDecimal, this.ShowChangedOperator).ConditionOperators,
            [FieldTypes.UnsInteger]: new ConditionOperatorsList(FieldTypes.UnsInteger, this.ShowChangedOperator).ConditionOperators,

            "default": new ConditionOperatorsList("default", this.ShowChangedOperator).ConditionOperators
        };
    }

}