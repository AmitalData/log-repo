export class QueryOperations {

    public PageSize: number;
    public PageIndex: number;
    public DataCount: number;
    public SortByColumnName: string;
    public SortDirectin: string;

    public QueryFilterItems: Array<QueryFilterItem> = [];
}

export class QueryFilterItem {
    public FieldName: number;
    public FieldValue: any;
    public FieldValue2: any;
    public FieldValue3: any;
    public Operator: string;
    public IsCustom: boolean;
    public DisplayInList: boolean;
    public IsCustomField: boolean;
    public FieldDataType: string;
    
}
 
 