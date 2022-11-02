import { Pipe, PipeTransform } from "@angular/core";
import { FieldTypes } from "Workflow/Constants/FieldTypes";

@Pipe({
    name: "IsDateTimeTypePipe"
})

export class IsDateTimeTypePipe implements PipeTransform {

    transform(type: string) {
        if (type) {
            return type === FieldTypes.DateTime || type === FieldTypes.Date;
        }
        return false;
    }

}