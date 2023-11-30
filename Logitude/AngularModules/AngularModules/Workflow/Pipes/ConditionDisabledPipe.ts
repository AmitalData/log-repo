import { Pipe, PipeTransform } from "@angular/core";
import { ConditionDisabled } from "Workflow/Types";

@Pipe({
    name: "ConditionDisabledPipe"
})

export class ConditionDisabledPipe implements PipeTransform {

    transform(conditionDisabled: ConditionDisabled, conditionSectionCode: string) {
        if (conditionDisabled && conditionSectionCode) {
            if (conditionDisabled.toString().indexOf(",") !== -1) {
                return conditionDisabled.toString().toLowerCase().split(",").indexOf(conditionSectionCode.toString().toLowerCase()) !== -1;
            }
            return conditionDisabled.toString().toLowerCase() === conditionSectionCode.toString().toLowerCase();
        }
        return false;
    }

}