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
        public IsLookUpfilter:boolean=false) { }
}
