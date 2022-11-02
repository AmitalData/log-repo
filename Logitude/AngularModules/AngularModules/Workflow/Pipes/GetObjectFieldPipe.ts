import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "GetObjectFieldPipe"
})

export class GetObjectFieldPipe implements PipeTransform {

    transform(field: string, objectFields: any) {
        if (field && objectFields) {
            let fieldCode = Formatter.getFieldCode(field);
            return fieldCode ? objectFields.find((o: any) => o.FieldCode === fieldCode) : null;
        }
        return null;
    }

}