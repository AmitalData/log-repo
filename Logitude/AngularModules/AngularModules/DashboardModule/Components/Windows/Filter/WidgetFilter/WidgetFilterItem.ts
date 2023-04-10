import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { AppTool } from 'Infrastructure/Tools';
import { AnalyticsFactsFieldsMetaDataList } from 'DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { FieldValueResolver } from 'Infrastructure/Utilities/FieldValueResolver';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';


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
    public FieldValue2: any = null
    public FieldValue3: any = null
    public QueryFilterItems: WidgetFilterItem[] = [];
    public DateGroupCode: string;
    public DashboardId: string;

    constructor(field: WidgetFilterItem = null, buildRootFilter: boolean = false, dashboardId: string = null) {
        this.UIProperties = new UIProperties;
        this.DashboardId = dashboardId;
        if (!buildRootFilter && field) this.BuildFieldData(field);
        else if (buildRootFilter) this.BuildRootFitler(field);
    }

    BuildRootFitler(oldValue: WidgetFilterItem) {
        this.QueryFilterItems.push(this.BuildGroupFilter(oldValue));
    }

    private BuildGroupFilter(oldFilter: WidgetFilterItem) {
        let groupTreeFilter = new WidgetFilterItem(null, false, this.DashboardId);
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(oldFilter.FilterType);
        oldFilter.QueryFilterItems?.forEach((oldField) => {
            groupTreeFilter.QueryFilterItems.push(this.BuildFilter(oldField));
        });
        return groupTreeFilter;
    }

    private BuildFilter(oldField: WidgetFilterItem) {
        if (oldField.QueryFilterItems.length == 0) return new WidgetFilterItem(oldField, false, this.DashboardId);
        return this.BuildGroupFilter(oldField);
    }

    BuildFieldData(field: WidgetFilterItem) {
        this.DontRefreshFieldData = true;
        this.FieldId = field.FieldId;
        this.FieldName = field.FieldName;
        this.DateGroupCode = field.DateGroupCode;
        this.FieldDataType = field.FieldDataType;
        this.FillFieldValue(field);
        this.Operator = field.Operator;
        this.FilterType = field.FilterType;
        this.QueryFilterItems = field.QueryFilterItems;
        this.FillOperators(this.FieldDataType);
        this.SelectedOperator = this.Operators.filter(x => x.Code == field.Operator)[0];
    }

    FillFieldValue(field: WidgetFilterItem) {
        if ((this.FieldDataType == 'DateTime' || this.FieldDataType == 'Date') && (field.Operator == "GreaterThan" || field.Operator == "LessThan" || field.Operator == "Between")) {
            this.FieldValue = FieldValueResolver.ConvertToDate(field.FieldValue, "TreeFilter");
            if (field.Operator == "Between") this.FieldValue2 = FieldValueResolver.ConvertToDate(field.FieldValue2, "TreeFilter");
            return;
        }
        if ((this.FieldDataType == 'DateTime' || this.FieldDataType == 'Date') && (field.Operator == "Previous" || field.Operator == "Next")) {
            this.FieldValue3 = field.FieldValue3;
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
            this.AddLookupEqualOperators(true);
            this.DontRefreshFieldData = false;
            return;
        }
        this.FillOperators(field.DataTypeCode);
        this.Operator = null;
        this.SelectedOperator = null;
        this.DateGroupCode = null;
    }


    private FillOperators(dataTypeCode: string) {
        this.Operators = [];
        switch (dataTypeCode) {
            case "DateTime":
            case "Date":
                this.Operators.push(new Operator("After", "GreaterThan"));
                this.Operators.push(new Operator("Before", "LessThan"));
                this.Operators.push(new Operator("Previous", "Previous"));
                this.Operators.push(new Operator("Current", "Current"));
                this.Operators.push(new Operator("Next", "Next"));
                this.Operators.push(new Operator("Between", "Between"));
                break;

            case "Integer":
            case "Decimal":
            case "Double":
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
                this.Operators.push(new Operator("Greater Than", "GreaterThan"));
                this.Operators.push(new Operator("Less Than", "LessThan"));
                this.Operators.push(new Operator("Greater Than Or Equal", "GreaterThanOrEqual"));
                this.Operators.push(new Operator("Less Than Or Equal", "LessThanOrEqual"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;

            case "Boolean":
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;

            case "LookUp":
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                this.AddLookupEqualOperators();
                break;
            default:
                this.Operators.push(new Operator("Equal", "Equal"));
                this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
                this.Operators.push(new Operator("Contains", "Contains"));
                this.Operators.push(new Operator("Does Not Contain", "NotContains"));
                this.Operators.push(new Operator("Is Empty", "IsEmpty"));
                this.Operators.push(new Operator("Is not Empty", "IsNotEmpty"));
                break;
        }
    }

    private AddLookupEqualOperators(setMissingOperator: boolean = false) {
        if (this.FieldDataType != 'LookUp') return;
        if (SessionInfo.LoggedUserTenant == 0 && !this.SelectedField?.AllowTenantZeroFilter) return;
        this.Operators.push(new Operator("Equal", "Equal"));
        this.Operators.push(new Operator("Does Not Equal", "NotEqual"));
        if (setMissingOperator) this.SelectedOperator = this.Operators.filter(x => x.Code == this.Operator)[0];
    }

    public OperationValueChanged(operator) {
        this.SelectedOperator = operator;
        this.Operator = operator ? operator.Code : null;
        this.GetDefaultFieldValue();
        this.FieldValue2 = "";
        this.GetDefaultDateGroup();
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Operator Change", Message: "Changed To " + operator.Code });
    }

    private GetDefaultFieldValue() {
        if ((this.FieldDataType == 'Date' || this.FieldDataType == 'DateTime') && (this.Operator == 'Next' || this.Operator == 'Previous')) {
            this.FieldValue3 = 1;
            this.FieldValue = "";
        }
        else {
            this.FieldValue = "";
            this.FieldValue3 = "";
        }

    }

    private GetDefaultDateGroup() {
        if (this.Operator == "Previous" || this.Operator == "Current" || this.Operator == "Next") this.DateGroupCode = "Day";
        else this.DateGroupCode = null;
    }

    TextBoxValueChange(newValue) {
        if (this.FieldDataType == 'Date' || this.FieldDataType == 'DateTime') this.FieldValue3 = newValue;
        else this.FieldValue = newValue;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + newValue });
    }

    BooleanListValueChanged(newValue: boolean) {
        this.IsChecked = newValue;
        this.FieldValue = this.IsChecked ? "true" : "false";
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + newValue });
    }

    DatePickerCondationValueChange(date) {
        this.FieldValue = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Value Change", Message: "Changed To " + this.FieldValue });
    }

    SecondDatePickerCondationValueChange(date) {
        this.FieldValue2 = date ? FieldValueResolver.ConvertUTCDateToString(date, "TreeFilter") : "";
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Second Date Value Change", Message: "Changed To " + this.FieldValue });
    }

    public AndOrOpsChanged(value) {
        this.AndOr = value;
        this.FilterType = value;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter AndOr Operator Change", Message: "Changed To " + value });
    }

    public DateGroupCodeChange(DateGroupCode: string) {
        this.DateGroupCode = DateGroupCode;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Date Group Change", Message: "Changed To " + DateGroupCode });
    }

    LogLovCondationValueChange(newValue) {
        this.FieldValue = newValue ? !AppTool.IsNullOrEmpty(newValue.Id) ? newValue.Id : newValue.Code : "";
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Filter Operator Change", Message: "Changed To " + this.FieldValue });
    }


    ShowTextBox(item: WidgetFilterItem) {
        return this.IsNotEmptyNotEmtyOperator(item) &&
            (!item.FieldDataType || item.FieldDataType == '' ||
                item.FieldDataType == 'Text' || item.FieldDataType == 'nText' ||
                item.FieldDataType == 'Integer' || item.FieldDataType == 'Double' ||
                item.FieldDataType == 'SigDouble' || item.FieldDataType == 'Decimal' ||
                ((item.FieldDataType == 'Date' || item.FieldDataType == 'DateTime') && (item.Operator == 'Next' || item.Operator == 'Previous'))
            )
    }

    IsNotEmptyNotEmtyOperator(item: WidgetFilterItem) {
        return item.Operator && item.Operator != 'IsEmpty' && item.Operator != 'IsNotEmpty';
    }

    GetTextInputType(item: WidgetFilterItem) {
        if (item.FieldDataType == 'Date' || item.FieldDataType == 'DateTime') return "Integer";
        if (item.FieldDataType == "Text") return "nText";
        return item.FieldDataType;
    }

    ShowFirstDatePicker(item: WidgetFilterItem) {
        return this.IsNotEmptyNotEmtyOperator(item) && (item.FieldDataType == 'DateTime' || item.FieldDataType == 'Date') &&
            (item.Operator == "GreaterThan" || item.Operator == "LessThan" || item.Operator == "Between");
    }

    ShowSecondDatePicker(item: WidgetFilterItem) {
        return this.IsNotEmptyNotEmtyOperator(item) && (item.FieldDataType == 'DateTime' || item.FieldDataType == 'Date') && item.Operator == "Between";
    }

    ShowDateGroups(item: WidgetFilterItem) {
        return this.IsNotEmptyNotEmtyOperator(item) && (item.FieldDataType == 'DateTime' || item.FieldDataType == 'Date') &&
            (item.Operator == "Previous" || item.Operator == "Current" || item.Operator == "Next");
    }

    get TextFieldValue(): string {
        if (this.FieldDataType == 'Date' || this.FieldDataType == 'DateTime') return this.FieldValue3;
        return this.FieldValue;
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