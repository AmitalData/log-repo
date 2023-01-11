import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "IsNoObjectFieldVariablePipe"
})

export class IsNoObjectFieldVariablePipe implements PipeTransform {

    // transform(field: string) {
    //     if (field) {
    //         let isNoObjectFieldVariable = (field.toString().startsWith("declaredvariables_")) ||
    //             (field.toString().startsWith("declaredrecordvariables_") && field.toString().split("_").length === 2);
    //         return isNoObjectFieldVariable;
    //     }
    //     return false;
    // }

    transform(field: string, objectFields: any) {
        if (field && objectFields) {
            let fieldCode = Formatter.getFieldCode(field);
            return fieldCode ? !objectFields.some((o: any) => o.FieldCode === fieldCode) : false;
        }
        return false;
    }

}