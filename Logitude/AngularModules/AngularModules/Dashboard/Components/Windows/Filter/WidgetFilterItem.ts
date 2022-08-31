import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { AppTool } from 'Infrastructure/Tools';
import { AnalyticsFactsFieldsMetaDataList } from 'DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { FieldValueResolver } from 'Infrastructure/Utilities/FieldValueResolver';


export class WidgetFilterItem {
    public UIProperties: UIProperties;
    public FieldName: string;
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

    private SelectedField: AnalyticsFactsFieldsMetaDataList;
    public FieldValue: any = null
    public QueryFilterItems: WidgetFilterItem[] = [];
    
    constructor() {
        this.UIProperties = new UIProperties;
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
        this.Operator = null;
        this.SelectedField = null;
    }

    FieldSelectedChanged(field: AnalyticsFactsFieldsMetaDataList) {
        this.SelectedField = field;
        this.FieldName = field.FieldCode;

        this.FieldDataType = field.DataTypeCode;
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
