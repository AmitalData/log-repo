
export class QueryFilterItem {
    FieldName: string;
    FieldValue: any;
    FieldValue2: any;

    FieldValue3: any;
    Operator: string

    IsCustom: boolean;
    DisplayInList: boolean


    IsCustomField: boolean;
    FieldDataType: string;

    constructor(fieldName: string = null, fieldValue: any = null, fieldValue2: any = null,fieldDataType: string = null) {
        this.FieldName = fieldName;
        this.FieldValue = fieldValue;
        this.FieldValue2 = fieldValue2;
        this.FieldDataType = fieldDataType;
    }
}


