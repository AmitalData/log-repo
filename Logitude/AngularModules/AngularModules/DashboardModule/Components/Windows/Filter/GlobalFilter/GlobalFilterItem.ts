export class GlobalFilterItem {
    private Component: any;
    constructor(component?: any) {
        this.Component = component
    }

    public Id: string;
    public FieldId: string;
    public DataSetId: string;
    public FieldName: string;
    public DisplayName: string;
    public DataTypeCode: string;
    public IsPreset: boolean;
    public Operator: string;

    public JoinedTableDisplayField: string;
    public CanSearch: boolean;
    public IsMultiSelect: boolean;
    public CompareWithPrevious: boolean;
    public JoinedTableName: string;

    private dateGroupCode: string;
    public get DateGroupCode(): string {
        return this.dateGroupCode;
    }
    public set DateGroupCode(value: string) {
        this.dateGroupCode = value;
        if (this.Component) this.Component.ValueChanged(this);
    }

    private fieldValue: any;
    public get FieldValue(): any {
        return this.fieldValue;
    }
    public set FieldValue(value: any) {
        this.fieldValue = value;
        if (this.Component) this.Component.ValueChanged(this);
    }

    private fieldValue2: any;
    public get FieldValue2(): any {
        return this.fieldValue2;
    }
    public set FieldValue2(value: any) {
        this.fieldValue2 = value;
        if (this.Component) this.Component.ValueChanged(this);
    }

    private fieldValue3: any;
    public get FieldValue3(): any {
        return this.fieldValue3;
    }
    public set FieldValue3(value: any) {
        this.fieldValue3 = value;
        if (this.Component) this.Component.ValueChanged(this);
    }

}