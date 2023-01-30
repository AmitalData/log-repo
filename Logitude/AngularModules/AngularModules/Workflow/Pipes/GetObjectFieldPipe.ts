import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Utilities/Formatter";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";

@Pipe({
    name: "GetObjectFieldPipe"
})

export class GetObjectFieldPipe implements PipeTransform {

    transform(field: string) {
        if (field) {
            let fieldCode = Formatter.getFieldCode(field);
            return ObjectFields.getByCode(fieldCode);
        }
        return null;
    }

}