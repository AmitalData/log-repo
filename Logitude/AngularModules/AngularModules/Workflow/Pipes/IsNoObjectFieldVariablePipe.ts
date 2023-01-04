import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: "IsNoObjectFieldVariablePipe"
})

export class IsNoObjectFieldVariablePipe implements PipeTransform {

    transform(field: string) {
        if (field) {
            let isNoObjectFieldVariable = (field.toString().startsWith("declaredvariables_")) ||
                (field.toString().startsWith("declaredrecordvariables_") && field.toString().split("_").length === 2);
            return isNoObjectFieldVariable;
        }
        return false;
    }

}