import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Utilities/Formatter";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";

@Pipe({
    name: "IsNoObjectFieldVariablePipe"
})

export class IsNoObjectFieldVariablePipe implements PipeTransform {

    transform(field: string) {
        if (field) {
            let fieldCode = Formatter.getFieldCode(field);
            return fieldCode ? !ObjectFields.getAll().some((o: any) => o.FieldCode === fieldCode) : false;
        }
        return false;
    }

}