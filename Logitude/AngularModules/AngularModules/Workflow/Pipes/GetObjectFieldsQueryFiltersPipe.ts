import { Pipe, PipeTransform } from "@angular/core";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ApiQueryFiltersBuilder } from "Workflow/Utilities/ApiQueryFiltersBuilder";
import { Formatter } from "Workflow/Utilities/Formatter";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";

@Pipe({
    name: "GetObjectFieldsQueryFiltersPipe"
})

export class GetObjectFieldsQueryFiltersPipe implements PipeTransform {

    transform(field: string | null, entityId: string | null) {
        let fieldCode = Formatter.getFieldCode(field);
        let objectField = ObjectFields.getByCode(fieldCode);
        if (objectField) {
            let lookupTableIdFilterValue = objectField.DataTypeCode === FieldTypes.LookUp ? objectField.LookUpTableId : null;
            return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(entityId, objectField.DataTypeCode, lookupTableIdFilterValue);
        }
        return ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(entityId, null, null);
    }

}