import { ApiQueryFiltersAddParams } from './ApiQueryFiltersAddParams';
export class ApiQueryFilters {
    constructor(getAll: boolean = false) {
        this.GetAll = getAll;
    }
    public PageIndex: number;
    public PageSize: number;
    public SortBy: string;
    public SortDirection: string;
    public GetCount: boolean;
    public Tenant: number;
    public AdditionalFilters: FilterItem[] = [];
    public ForceCacheRefresh: boolean = false;
    public DontApplyVirtualization: boolean = false;

    addAdditionalFilter(
        FieldName: string,
        FieldValue: any,
        FieldValue2: any,
        FieldValue3: any,
        Operator: string,
        IsCustom: boolean,
        DisplayInList: boolean,
        IsCustomField: boolean,
        FieldDataType: string,
        IgnoreFilter: boolean = false,
        IsCacheOnClient: boolean = false,
        ForceEnableAdd: boolean = false
    ) {
        let apiQueryFiltersAddParams = new ApiQueryFiltersAddParams();
        apiQueryFiltersAddParams.FieldName = FieldName;
        apiQueryFiltersAddParams.FieldValue = FieldValue;
        apiQueryFiltersAddParams.FieldValue2 = FieldValue2;
        apiQueryFiltersAddParams.FieldValue3 = FieldValue3;
        apiQueryFiltersAddParams.Operator = Operator;
        apiQueryFiltersAddParams.IsCustom = IsCustom;
        apiQueryFiltersAddParams.DisplayInList = DisplayInList;
        apiQueryFiltersAddParams.IsCustomField = IsCustomField;
        apiQueryFiltersAddParams.FieldDataType = FieldDataType;
        apiQueryFiltersAddParams.IgnoreFilter = IgnoreFilter;
        apiQueryFiltersAddParams.IsCacheOnClient = IsCacheOnClient;
        apiQueryFiltersAddParams.ForceEnableAdd = ForceEnableAdd;
        this.pushAdditionalFilter(apiQueryFiltersAddParams);
    }
    replaceStr(text: string) {
        var replacedText = '';
        for (var i = 0; i < text.length; i++) {
            if (text[i] == '"') {
                replacedText = replacedText + '\\"';
            } else {
                replacedText = replacedText + text[i];
            }
        }
        return replacedText;
    }
    removeAdditionalFilter(fieldName: string) {
        var item = this.AdditionalFilters.filter(
            (d) => d.FieldName == fieldName && d.IsLookUpfilter == true
        )[0];
        if (item) {
            var index = this.AdditionalFilters.indexOf(item);
            this.AdditionalFilters.splice(index, 1);
        }
    }

    pushAdditionalFilter(apiQueryFiltersAddParams: ApiQueryFiltersAddParams) {
        var existedItem = this.AdditionalFilters.find(
            (d) => d.FieldName == apiQueryFiltersAddParams.FieldName
        );
        if (!existedItem || apiQueryFiltersAddParams.ForceEnableAdd) {
            var item = new FilterItem(
                apiQueryFiltersAddParams.FieldName,
                apiQueryFiltersAddParams.FieldValue,
                apiQueryFiltersAddParams.FieldValue2,
                apiQueryFiltersAddParams.FieldValue3,
                apiQueryFiltersAddParams.Operator,
                apiQueryFiltersAddParams.IsCustom,
                apiQueryFiltersAddParams.DisplayInList,
                apiQueryFiltersAddParams.IsCustomField,
                apiQueryFiltersAddParams.FieldDataType,
                apiQueryFiltersAddParams.IgnoreFilter,
                apiQueryFiltersAddParams.IsCacheOnClient,
                apiQueryFiltersAddParams.IsLookUpFilter
            );
            this.AdditionalFilters.push(item);
        }
    }

    public queryId: string;
    public queryCode: string;

    public userid: string;
    public ObjectTableName: string;

    public Filter1Name: string;
    public Filter1Value: Object;
    public Filter1Operator: string;
    public Filter1Value2: Object;

    public Filter2Name: string;
    public Filter2Value: Object;
    public Filter2Operator: string;
    public Filter2Value2: Object;

    public Filter3Name: string;
    public Filter3Value: Object;
    public Filter3Operator: string;
    public Filter3Value2: Object;

    public Filter4Name: string;
    public Filter4Value: Object;
    public Filter4Operator: string;
    public Filter4Value2: Object;

    public Filter5Name: string;
    public Filter5Value: Object;
    public Filter5Operator: string;
    public Filter5Value2: Object;

    public Filter6Name: string;
    public Filter6Value: Object;
    public Filter6Operator: string;
    public Filter6Value2: Object;

    public Filter7Name: string;
    public Filter7Value: Object;
    public Filter7Operator: string;
    public Filter7Value2: Object;

    public Filter8Name: string;
    public Filter8Value: Object;
    public Filter8Operator: string;
    public Filter8Value2: Object;

    public Filter9Name: string;
    public Filter9Value: Object;
    public Filter9Operator: string;
    public Filter9Value2: Object;

    public Filter10Name: string;
    public Filter10Value: Object;
    public Filter10Operator: string;
    public Filter10Value2: Object;

    public GetAll: boolean;
}

export class FilterItem {
    constructor(
        public FieldName: string,
        public FieldValue: any,
        public FieldValue2: any,
        public FieldValue3: any,
        public Operator: string,
        public IsCustom: boolean,
        public DisplayInList: boolean,
        public IsCustomField: boolean,
        public FieldDataType: string,
        public IgnoreFilter: boolean,
        public IsCacheOnClient: boolean = false,
        public IsLookUpfilter: boolean = false
    ) {}
}
