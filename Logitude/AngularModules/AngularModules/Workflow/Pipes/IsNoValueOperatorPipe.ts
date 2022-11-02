import { Pipe, PipeTransform } from "@angular/core";
import { ConditionOperators } from "Workflow/Constants/ConditionOperators";

@Pipe({
    name: "IsNoValueOperatorPipe"
})

export class IsNoValueOperatorPipe implements PipeTransform {

    transform(operatorCode: string) {
        if (operatorCode) {
            return operatorCode === ConditionOperators.IsEmpty || operatorCode === ConditionOperators.Changed;
        }
        return false;
    }

}