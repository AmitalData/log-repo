import { Pipe, PipeTransform } from "@angular/core";
import { FieldApiQueryFilter } from "Workflow/Models/FieldApiQueryFilter";

@Pipe({
    name: "GetFieldApiQueryFiltersPipe"
})

export class GetFieldApiQueryFiltersPipe implements PipeTransform {

    transform(fieldCode: string, fieldApiQueryFilters: FieldApiQueryFilter[] | null = null) {
        if (fieldCode && fieldApiQueryFilters && fieldApiQueryFilters.length > 0) {
            let fieldApiQueryFilter = fieldApiQueryFilters.find(i => i.fieldCode === fieldCode);
            return fieldApiQueryFilter ? fieldApiQueryFilter.apiQueryFilters : null;
        }
        return null;
    }

}