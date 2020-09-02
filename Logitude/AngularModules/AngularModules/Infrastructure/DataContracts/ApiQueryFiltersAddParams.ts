export class ApiQueryFiltersAddParams{
        FieldName: string;
        FieldValue: any;
        FieldValue2: any;
        FieldValue3: any;
        Operator: string;
        IsCustom: boolean;
        DisplayInList: boolean;
        IsCustomField: boolean;
        FieldDataType: string;
        IgnoreFilter: boolean = false;
        IsCacheOnClient: boolean = false;
        ForceEnableAdd:boolean = false;
        IsLookUpFilter:boolean=false;
}