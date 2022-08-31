import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { AppTool } from 'Infrastructure/Tools';
import { AnalyticsFactsFieldsMetaDataList } from 'DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { FieldValueResolver } from 'Infrastructure/Utilities/FieldValueResolver';


export class WidgetFilterItem {
    public UIProperties: UIProperties;
    public FieldName: string;
    public FieldId: string;
    public IsGroup: boolean = false;


    public FilterType: string = 'And'
    private andOr: string = "And";
    public AndOrOps = ["And", "Or"];
    public BooleanList: boolean[] = [true, false];
    public Operator: string = '';
    public FieldDataType: string = '';
    public Operators: any;
    public SelectedOperator: Operator;
    public IsChecked: boolean;
    private DontRefreshFieldData: boolean = false;

    private SelectedField: AnalyticsFactsFieldsMetaDataList;
    public FieldValue: any = null
    public QueryFilterItems: WidgetFilterItem[] = [];

    constructor(field: WidgetFilterItem = null) {
        this.UIProperties = new UIProperties;
        if (field) this.BuildFieldData(field);
    }

    BuildFieldData(field: WidgetFilterItem) {
        this.DontRefreshFieldData = true;
        this.FieldId = field.FieldId;
        this.FieldName = field.FieldName;
        this.FillFieldValue(field);
        this.Operator = field.Operator;
        this.FieldDataType = field.FieldDataType;
        this.FilterType = field.FilterType;
        this.QueryFilterItems = field.QueryFilterItems;
        this.FillOperators(this.FieldDataType);
        this.SelectedOperator = this.Operators.filter(x => x.Code == field.Operator)[0];
    }

    FillFieldValue(field: WidgetFilterItem) {
        if (this.FieldDataType == 'DateTime' || this.FieldDataType == 'Date') return;
        if (this.FieldDataType == 'Boolean') {
            this.BooleanListValueChanged(field.FieldValue?.toString() == 'true');
            return;
        }
        this.FieldValue = field.FieldValue;
    }

    public setAndOrOperation(Newvalue: string) {
        this.AndOr = Newvalue;
    }

    public set AndOr(newValue: string) {
        this.FilterType = newValue;
        this.andOr = newValue;
    }

    public get AndOr() {
        let temp = AppTool.IsNullOrEmpty(this.andOr) ? "And" : this.andOr;
        this.FilterType = temp;
        return temp;
    }

    FieldChanged(field: AnalyticsFactsFieldsMetaDataList) {
        if (!field) {
            this.ResetField();
            return;
        }
        if (this.SelectedField && this.SelectedField.Id == field.Id) return;
        this.FieldSelectedChanged(field);
    }

    private ResetField() {
        this.FieldName = "";
        this.FieldId = "";
        this.Operator = null;
        this.SelectedField = null;
    }

    FieldSelectedChanged(field: AnalyticsFactsFieldsMetaDataList) {
        this.SelectedField = field;
        this.FieldName = field.FieldCode;
        this.FieldId = field.Id;
        this.FieldDataType = field.DataTypeCode;
        if (this.DontRefreshFieldData) {
            this.DontRefreshFieldData = false;
            return;
        }
        this.FillOperators(field.DataTypeCode);
        this.Operator = null;
        this.SelectedOperator = null;
    }


    private FillOperators(dataTypeCode: string) {
        this.Operators = [];

        if (dataTypeCode == "DateTime" || dataTypeCode == "Date" || dataTypeCode == "Integer" || dataTypeCode == "Decimal" || dataTypeCode == "Double") {
            this.Operators.push(new Operator("Equal", "Equal"));
            this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
            this.Operators.push(new Operator("Greater Than", "GreaterThan"));
            this.Operators.push(new Operator("Less Than", "LessThan"));
            this.Operators.push(new Operator("Greater Than Or Equal", "GreaterThanOrEqual"));
            this.Operators.push(new Operator("Less Than Or Equal", "LessThanOrEqual"));
        }
        else if (dataTypeCode == "Boolean") {
            this.Operators.push(new Operator("Equal", "Equal"));
        }
        else if (dataTypeCode == "LookUp") {
            this.Operators.push(new Operator("Equal", "Equal"));
            this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
        }
        else {
            this.Operators.push(new Operator("Equal", "Equal"));
            this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
            this.Operators.push(new Operator("Contains", "Contains"));
            this.Operators.push(new Operator("Does Not Contain", "NotContains"));
        }

        this.Operators.push(new Operator("Is Empty", "IsEmpty"));
        this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
    }

    public OperationValueChanged(operator) {
        this.SelectedOperator = operator;
        this.Operator = operator ? operator.Code : null;
        this.FieldValue = "";
    }

    TextBoxCondationValueChange(newValue) {
        this.FieldValue = newValue;
    }

    BooleanListValueChanged(newValue: boolean) {
        this.IsChecked = newValue;
        this.FieldValue = this.IsChecked ? "true" : "false";
    }

    DatePickerCondationValueChange(newValue) {
        this.FieldValue = newValue ? FieldValueResolver.ConvertUTCDateToString(newValue) : "";
    }

    public AndOrOpsChanged(value) {
        this.AndOr = value;
        this.FilterType = value;
    }

}

export class Operator {
    Code: string;
    Name: string;

    constructor(name: string, code: string) {
        this.Code = code;
        this.Name = name;
    }
}
