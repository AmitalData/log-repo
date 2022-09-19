import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { AppTool } from 'Infrastructure/Tools';
import { AnalyticsFactsFieldsMetaDataList } from 'DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { FieldValueResolver } from 'Infrastructure/Utilities/FieldValueResolver';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';


export class WidgetFilterItem {
    public UIProperties: UIProperties;
    public FieldName: string;
    public FieldId: string;
    public IsGroup: boolean = false;
    public IsAnalyticsMetadatas: boolean = true;


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
    public DateGroupCode: string;
    public Quarter: string;

    constructor(field: WidgetFilterItem = null, buildRootFilter: boolean = false) {
        this.UIProperties = new UIProperties;
        if (!buildRootFilter && field) this.BuildFieldData(field);
        else if (buildRootFilter) this.BuildRootFitler(field);
    }

    BuildRootFitler(oldValue: WidgetFilterItem) {
        this.QueryFilterItems.push(this.BuildGroupFilter(oldValue));
    }

    private BuildGroupFilter(oldFilter: WidgetFilterItem) {
        let groupTreeFilter = new WidgetFilterItem();
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(oldFilter.FilterType);
        oldFilter.QueryFilterItems?.forEach((oldField) => {
            groupTreeFilter.QueryFilterItems.push(this.BuildFilter(oldField));
        });
        return groupTreeFilter;
    }

    private BuildFilter(oldField: WidgetFilterItem) {
        if (oldField.QueryFilterItems.length == 0) return new WidgetFilterItem(oldField);
        return this.BuildGroupFilter(oldField);
    }

    BuildFieldData(field: WidgetFilterItem) {
        this.DontRefreshFieldData = true;
        this.FieldId = field.FieldId;
        this.FieldName = field.FieldName;
        this.DateGroupCode = field.DateGroupCode;
        this.FieldDataType = field.FieldDataType;
        this.Quarter = field.Quarter;
        this.FillFieldValue(field);
        this.Operator = field.Operator;
        this.FilterType = field.FilterType;
        this.QueryFilterItems = field.QueryFilterItems;
        this.FillOperators(this.FieldDataType);
        this.SelectedOperator = this.Operators.filter(x => x.Code == field.Operator)[0];
    }

    FillFieldValue(field: WidgetFilterItem) {
        if (this.FieldDataType == 'DateTime' || this.FieldDataType == 'Date') {
            this.FieldValue = FieldValueResolver.ConvertToDate(field.FieldValue, "TreeFilter");
            return;
        }
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
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Field Change", Message: "Changed To " + field.DisplayName });
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
        this.Quarter = null;
        this.DateGroupCode = null;
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
        this.Quarter = null;
        this.DateGroupCode = null;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Operator Change", Message: "Changed To " + operator.Code });
    }

    TextBoxCondationValueChange(newValue) {
        this.FieldValue = newValue;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + newValue });
    }

    BooleanListValueChanged(newValue: boolean) {
        this.IsChecked = newValue;
        this.FieldValue = this.IsChecked ? "true" : "false";
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + newValue });
    }

    DatePickerCondationValueChange(date, quarter) {
        this.FieldValue = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
        this.Quarter = this.DateGroupCode == "Quarter" ? quarter : null;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + this.FieldValue });
    }

    public AndOrOpsChanged(value) {
        this.AndOr = value;
        this.FilterType = value;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter AndOr Operator Change", Message: "Changed To " + value });
    }

    public DateGroupCodeChange(DateGroupCode: string) {
        this.DateGroupCode = DateGroupCode;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Date Group Change", Message: "Changed To " + DateGroupCode });
        this.FieldValue = "";
        this.Quarter = null;
    }

    LogLovCondationValueChange(newValue) {
        this.FieldValue = newValue ? !AppTool.IsNullOrEmpty(newValue.Id) ? newValue.Id : newValue.Code : "";
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Operator Change", Message: "Changed To " + this.FieldValue });
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
