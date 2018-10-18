export class ApiQueryFilters {

    public PageIndex: number;
    public PageSize: number;
    public SortBy: string;
    public SortDirection: string;
    public GetCount: boolean;

    public AdditionalFilters: FilterItem[] = [];

    addAdditionalFilter(
        FieldName: string,
        FieldValue: any,
        FieldValue2: any,
        FieldValue3: any,
        Operator: string,
        IsCustom: boolean,
        DisplayInList: boolean,
        IsCustomField: boolean,
        FieldDataType: string) {

        var item = new FilterItem(FieldName, FieldValue,
            Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType);


        this.AdditionalFilters.push(item);
    }


    public Filter1Name: string;
    public Filter1Value: string;
    public Filter1Operator: string;

    public Filter2Name: string;
    public Filter2Value: string;
    public Filter2Operator: string;

    public Filter3Name: string;
    public Filter3Value: string;
    public Filter3Operator: string;

    public Filter4Name: string;
    public Filter4Value: string;
    public Filter4Operator: string;

    public Filter5Name: string;
    public Filter5Value: string;
    public Filter5Operator: string;

    public Filter6Name: string;
    public Filter6Value: string;
    public Filter6Operator: string;

    public Filter7Name: string;
    public Filter7Value: string;
    public Filter7Operator: string;

    public Filter8Name: string;
    public Filter8Value: string;
    public Filter8Operator: string;

    public Filter9Name: string;
    public Filter9Value: string;
    public Filter9perator: string;

    public Filter10Name: string;
    public Filter10Value: string;
    public Filter10Operator: string;
}

export class FilterItem {
    constructor(
        public FieldName: string,
        public FieldValue: any,
        public Operator: string,
        public IsCustom: boolean,
        public DisplayInList: boolean,
        public IsCustomField: boolean,
        public FieldDataType: string) { }

}