import { Pipe, PipeTransform } from "@angular/core";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "GetObjectFieldsQueryFiltersPipe"
})

export class GetObjectFieldsQueryFiltersPipe implements PipeTransform {

    transform(field: string | null, objectFields: any, entityId: string | null) {
        let fieldCode = Formatter.getFieldCode(field);
        let objectField = (objectFields && fieldCode) ? objectFields.find((o: any) => o.FieldCode === fieldCode) : null;
        if (objectField) {
            let lookupTableIdFilterValue = objectField.DataTypeCode === FieldTypes.LookUp ? objectField.LookUpTableId : null;
            return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(entityId, objectField.DataTypeCode, lookupTableIdFilterValue);
        }
        return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(entityId, null, null);
    }

}