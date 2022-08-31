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

            "default": new ConditionOperatorsList("default", this.ShowChangedOperator).ConditionOperators
        };
    }

}