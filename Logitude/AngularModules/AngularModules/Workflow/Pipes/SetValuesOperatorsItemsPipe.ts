import { Pipe, PipeTransform } from "@angular/core";
import { SetValueOperatorsList } from "Workflow/Lists/SetValueOperatorsList";

@Pipe({
    name: "SetValuesOperatorsItemsPipe"
})

export class SetValuesOperatorsItemsPipe implements PipeTransform {

    transform(setValueType: string) {
        if (setValueType) {
            let operatorsItems = new SetValueOperatorsList(setValueType).Items;
            return operatorsItems;
        }
        return [];
    }

}