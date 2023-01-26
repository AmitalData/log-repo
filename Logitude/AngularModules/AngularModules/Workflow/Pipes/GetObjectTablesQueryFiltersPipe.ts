import { Pipe, PipeTransform } from "@angular/core";
import { ApiQueryFiltersBuilder } from "Workflow/Utilities/ApiQueryFiltersBuilder";

@Pipe({
    name: "GetObjectTablesQueryFiltersPipe"
})

export class GetObjectTablesQueryFiltersPipe implements PipeTransform {

    transform(name: string) {
        return ApiQueryFiltersBuilder.getObjectTablesApiQueryFilters(name);
    }

}