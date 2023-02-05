import { Pipe, PipeTransform } from "@angular/core";
import { ConditionOperatorsList } from "Workflow/Lists/ConditionOperatorsList";

@Pipe({
    name: "ConditionOperatorsItemsPipe"
})

export class ConditionOperatorsItemsPipe implements PipeTransform {

    transform(fieldType: string, showChangedOperator: boolean = true) {
        if (fieldType) {
            return new ConditionOperatorsList(fieldType, showChangedOperator).Items;
        }
        return [];
    }

}