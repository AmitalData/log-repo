import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { WidgetFilterItem } from './Filter/WidgetFilterItem';
import { AnalyticsFactsFieldsMetaDataList } from 'DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { AppTool } from 'Infrastructure/Tools';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { retry } from 'rxjs/operators';
import { Guid } from 'Infrastructure/Utilities/Guid';

@Component({
    templateUrl: './AddEditWidgetComponent.html',
})

export class AddEditWidgetComponent extends BaseComponent {
    public EntityPM: WidgetPM;
    public DashboardPM: DashboardPM = null;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditWidgetComponent;
    private isNew: boolean = false;
    private FirstTime: boolean = true;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Widget";
    public ChartImageSrc: string;
    public WidgetMeasuresList: WidgetMeasureItem[];
    public WidgetMeasuresClone: WidgetMeasurePM[];
    //public RootFilter: WidgetFilterItem;
    public RootFilter: WidgetFilterItem = new WidgetFilterItem(null, false, this.DashboardPM?.Id);
    public IsAddNewMeasureVisible: boolean = true;
    public DateGroupCodes = ['Day', 'Month', 'Year', 'Quarter'];
    public SortByCodes = [];
    public SortByDirections = [{name:'Ascending',code:'asc'},{name:'Descending',code: 'desc'}];
    public GroupByQueryFilters: ApiQueryFilters;
    public isGroupByVisible: boolean = true;
    public isSortByVisible: boolean = true;
    public isMaximumGroupingVisible: boolean = true;
    public showAdvancedSetting : boolean = false;

    constructor() {
        super();
        this.WidgetMeasuresList = [];
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DashboardPM = windowArgs['DashboardPM'];
        this.isNew = windowArgs['IsNew'];
        this.FirstTime = this.isNew != true;
        this.DataContext = this;
        this.ComputeChartImageSrc();
        this.BuildMeasures();
        this.CheckMeasureAddVisiblity();
        this.Clone();
        this.GetFilters();
        this.BuildQueryFilters();
        this.InitView();
    }

    BuildQueryFilters() {
        this.GroupByQueryFilters = new ApiQueryFilters();
        this.GroupByQueryFilters.addAdditionalFilter("DataTypeCode", "PickList,LookUp,DateTime,Date", null, null, "InList", false, true, false, "string", false, true, true);
    }

    GetFilters() {
        if (!this.EntityPM.Filters) return;
        var filters = JSON.parse(this.EntityPM.Filters);
        if (!filters) return;
        this.RootFilter = new WidgetFilterItem(filters, true, this.DashboardPM?.Id);
    }

    private ComputeChartImageSrc() {
        switch (this.EntityPM.TypeCode) {
            case "pie": {
                this.ChartImageSrc = "./Images/Charts/PieChart.png";
                break;
            }

            case "bar": {
                this.ChartImageSrc = "./Images/Charts/BarChart.png";
                break;
            }

            case "line": {
                this.ChartImageSrc = "./Images/Charts/LineChart.png";
                break;
            }

            case "donut": {
                this.ChartImageSrc = "./Images/Charts/DonutChart.png";
                break;
            }

            case "kpi": {
                this.ChartImageSrc = null;
                break;
            }
        }
    }

    public BuildMeasures() {
        this.WidgetMeasuresList = [];

        this.EntityPM.WidgetMeasures.forEach(item => {
            this.WidgetMeasuresList.push(new WidgetMeasureItem(item, false, this));
        });

        if (this.WidgetMeasuresList.length == 0) {
            var newItem: WidgetMeasurePM = new WidgetMeasurePM(null);
            newItem.Tenant = SessionInfo.LoggedUserTenant;
            newItem.WidgetId = this.EntityPM.Id;
            newItem.MeasureCode = "Sum";
            this.WidgetMeasuresList.push(new WidgetMeasureItem(newItem, true, this));
        }

        this.WidgetMeasuresList.forEach(item => {
            item.CheckMeasureDeleteVisiblity();
        });
    }

    public CheckMeasureAddVisiblity() {
        var isAddVisible: boolean = true;

        if (this.EntityPM.TypeCode == "donut" || this.EntityPM.TypeCode == "pie" || this.EntityPM.TypeCode == "kpi") {
            isAddVisible = false;
            if (this.WidgetMeasuresList && this.WidgetMeasuresList.length > 1) this.WidgetMeasuresList.splice(1, 1);
        }

        else if (this.WidgetMeasuresList.length != 1) {
            isAddVisible = false;
        }

        this.BuildSortCodes();
        this.IsAddNewMeasureVisible = isAddVisible;
    }

    private BuildSortCodes() {
        this.SortByCodes = [{ Text: 'Group' }, { Code: 1, Text: 'Measure 1' }];

        if (this.EntityPM.TypeCode != "donut" && this.EntityPM.TypeCode != "pie" && this.WidgetMeasuresList.length != 1) {
            this.SortByCodes.push({ Code: 2, Text: 'Measure 2' });
        }
    }

    get Title() { return this.EntityPM.Title }
    set Title(value: string) {
        if (this.EntityPM.Title != value) {
            this.EntityPM.Title = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Title Change ", Message: "Changed To" + this.EntityPM.Title, DashboardId: this.DashboardPM?.Id });
        }
    }

    get EntityId() { return this.EntityPM.EntityId }
    set EntityId(value: string) {
        if (this.EntityPM.EntityId != value) {
            this.EntityPM.EntityId = value;

            this.GroupById = null;
            this.WidgetMeasuresList.forEach(item => {
                item.MeasureFieldId = null;
            });

            this.RootFilter = new WidgetFilterItem(null, false, this.DashboardPM?.Id);
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Entity Change ", Message: "Changed To" + this.EntityPM.EntityId, DashboardId: this.DashboardPM?.Id });
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            this.EntityPM.TypeCode = value;
            this.ComputeChartImageSrc();
            this.CheckMeasureAddVisiblity();
            this.InitView();
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Type Change ", Message: "Changed To" + this.EntityPM.TypeCode, DashboardId: this.DashboardPM?.Id });
        }
    }

    InitView() {
        this.isGroupByVisible = this.TypeCode != "kpi";
        this.isSortByVisible = this.TypeCode != "kpi";
        this.isMaximumGroupingVisible = this.TypeCode != "kpi";

        if (!this.isGroupByVisible) {
            this.GroupById = null;
            this.DateGroupCode = null;
        }
        if (!this.isSortByVisible) {
            this.SortBy = null;
            this.SortDirection = null;
        }
        if (!this.isMaximumGroupingVisible) {
            this.MaximumGrouping = null;
        }
    }

    get GroupById() { return this.EntityPM.GroupById; }
    set GroupById(value: string) {
        if (this.EntityPM.GroupById != value) {
            this.EntityPM.GroupById = value;
        }
    }

    get StartPotistion() { return this.EntityPM.StartPotistion; }
    set StartPotistion(value: string) {
        if (this.EntityPM.StartPotistion != value) {
            this.EntityPM.StartPotistion = value;
        }
    }

    get EndPosition() { return this.EntityPM.EndPosition; }
    set EndPosition(value: string) {
        if (this.EntityPM.EndPosition != value) {
            this.EntityPM.EndPosition = value;
        }
    }

    get DateGroupCode() { return this.EntityPM.DateGroupCode; }
    set DateGroupCode(value: string) {
        if (this.EntityPM.DateGroupCode != value) {
            this.EntityPM.DateGroupCode = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Date Group Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });

        }
    }

    get SortBy() { return this.EntityPM.SortBy; }
    set SortBy(value: number) {
        if (this.EntityPM.SortBy != value) {
            this.EntityPM.SortBy = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Sort By Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    get MaximumGrouping() {
        if (!this.EntityPM?.MaximumGrouping) this.EntityPM.MaximumGrouping = 10;
        return this.EntityPM.MaximumGrouping;
    }
    set MaximumGrouping(value: number) {
        if (this.EntityPM.MaximumGrouping != value) {
            this.EntityPM.MaximumGrouping = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Maximum Grouping Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    get SortDirection() {
        return this.EntityPM.SortDirection;
    }
    set SortDirection(value: string) {
        if (this.EntityPM.SortDirection != value) {
            this.EntityPM.SortDirection = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Sort By Direction Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    public GetSelectedSort() {
        return this.SortByCodes.find(x => x.Code == this.SortBy);
    }
    public GetSelectedSortDirection() {
        return this.SortByDirections.find(x => x.code == this.SortDirection);
    }

    public selectedGroupField: AnalyticsFactsFieldsMetaDataList = null;
    get SelectedGroupField() { return this.selectedGroupField; }
    set SelectedGroupField(value: AnalyticsFactsFieldsMetaDataList) {
        if (this.selectedGroupField?.Id == value?.Id) {
            this.FirstTime = false;
            return;
        }
        this.selectedGroupField = value;
        if (this.FirstTime) {
            this.FirstTime = false;
            return;
        }
        this.EntityPM.DateGroupCode = (value?.DataTypeCode == 'DateTime' || value?.DataTypeCode == 'Date') ? this.DateGroupCodes[0] : null;
        this.SetDefaultSortByField();
        this.FirstTime = false;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Group Change", Message: "Changed To " + this.selectedGroupField?.DisplayName, DashboardId: this.DashboardPM?.Id });

    }


    SetDefaultSortByField() {
        if (!this.selectedGroupField) return;
        if (this.selectedGroupField.DataTypeCode == "DateTime" || this.selectedGroupField.DataTypeCode == 'Date') {
            this.SortBy = null;
            this.SortDirection = "desc";
            return;
        }
        this.SortBy = 1;
        this.SortDirection = "asc";
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.WidgetMeasuresList.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);
        });

        this.ValidateInputs(errors);
        this.ValidationErrorsList = errors;
        if (errors.length != 0) return;

        this.SetFiltersString();
        this.WidgetMeasuresList.forEach(item => {
            if (item.IsNew && this.EntityPM.WidgetMeasures.indexOf(item.EntityPM) == -1) {
                item.IsNew = false;
                this.DataContext.EntityPM.AddWidgetMeasure(item.EntityPM);
            }
        });
        if (this.isNew) {
            this.isNew = false;
            this.EntityPM.Tenant = SessionInfo.LoggedUserTenant;
            this.EntityPM.Key = Guid.newGuid();
            this.DashboardPM.AddWidget(this.EntityPM);
        }
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    ValidateInputs(errors: string[]) {
        if (this.isGroupByVisible && !this.GroupById) {
            errors.push("Group By Field is Required");
        }
        if (this.GroupByDateIsNotValid()) {
            errors.push("Date Group Type Field is Required");
        }
        if (this.isMaximumGroupingVisible && !this.MaximumGrouping) {
            errors.push("Group By Field is Required");
        }
        if (this.isSortByVisible && !this.SortDirection) {
            errors.push("Sort By Direction Field is Required");
        }
        this.ValidateMeasures(errors);
        this.ValidateSort(errors);
        if (this.RootFilter && this.RootFilter.QueryFilterItems && this.RootFilter.QueryFilterItems.length != 0) this.ValidateFilters(errors, this.RootFilter);
    }

    GroupByDateIsNotValid(){
        return this.isGroupByVisible && this.SelectedGroupField && (this.SelectedGroupField.DataTypeCode == 'DateTime' || this.SelectedGroupField?.DataTypeCode == 'Date') && !this.DateGroupCode;
    }

    ValidateSort(errors: string[]) {
        if (!this.isSortByVisible) return;
        if (this.WidgetMeasuresList.length <= 1 && this.SortBy == 2) errors.push("Invalid Sort On Measure 2");
    }

    ValidateMeasures(errors: string[]) {
        this.WidgetMeasuresList.forEach(measure => {
            if (!AppTool.IsNullOrEmpty(measure.MeasureCode) && measure.MeasureCode != "Count" && AppTool.IsNullOrEmpty(measure.MeasureFieldId))
                errors.push("Measure Field is Required");
        });
    }

    ValidateFilters(errors: string[], filter: WidgetFilterItem) {
        if (filter.QueryFilterItems && filter.QueryFilterItems.length != 0) {
            filter.QueryFilterItems.forEach(filterItem => { this.ValidateFilters(errors, filterItem); });
            return;
        }
        if (!filter.FieldId) {
            errors.push("Filter Field is Required");
            return;
        }
        if (!filter.Operator) {
            errors.push("Filter Operator is Required");
            return;
        }
        if (filter.Operator == "IsEmpty" || filter.Operator == "IsNotEmpty") return;
        if (filter.Operator != "Previous" && filter.Operator != "Next" && filter.Operator != "Current" && (!filter.FieldValue || filter.FieldValue == "")) errors.push("Filter Value is Required");
        if ((filter.Operator == "Previous" || filter.Operator == "Next") && (!filter.FieldValue3 || filter.FieldValue3 == "")) errors.push("Filter Value is Required");
        if (filter.Operator == "Between") this.ValidateBetweenOperator(errors, filter);
    }

    ValidateBetweenOperator(errors: string[], filter: WidgetFilterItem) {
        if ((!filter.FieldValue2 || filter.FieldValue2 == "")) {
            errors.push("Filter Second Date is Required");
        }
        if (filter.FieldValue2 <= filter.FieldValue) errors.push("Filter First Date Must Be Bigger Than Second Date");
    }


    private myCloner: Cloner;
    private SetFiltersString() {
        if (!this.RootFilter || !this.RootFilter.QueryFilterItems || this.RootFilter.QueryFilterItems.length == 0) {
            this.EntityPM.Filters = null;
            return;
        }
        this.EntityPM.Filters = JSON.stringify(this.RootFilter.QueryFilterItems[0], function (key, val) {
            if (key !== "UIProperties" && key !== "IsGroup" && key !== "andOr" &&
                key !== "AndOrOps" && key !== "BooleanList" && key !== "IndexOrder" &&
                key !== "FieldCode" && key !== "Operators" && key !== "SelectedOperator" &&
                key !== "null" && key !== "SelectedField" && key !== "DontRefreshFieldData" && key !== "DashboardId") return val;
        });
    }

    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Title');
        this.myCloner.AddField('TypeCode');
        this.myCloner.AddField('GroupBy');
        this.myCloner.AddField('StartPotistion');
        this.myCloner.AddField('EndPosition');
        this.myCloner.AddField('EntityId');
        this.myCloner.AddField('DateGroupCodes');
        this.myCloner.AddField('SortDirection');
        this.myCloner.AddField('SortBy');
        this.myCloner.AddField('MaximumGrouping');

        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DashboardPM);


        this.EntityPM.WidgetMeasures.forEach(item => {
            if (!this.WidgetMeasuresClone) this.WidgetMeasuresClone = [];
            this.WidgetMeasuresClone.push(Object.assign(new WidgetMeasurePM(null), item));
        });

    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
        this.EntityPM.WidgetMeasures = this.WidgetMeasuresClone;
    }

    AddNewMeasureClicked() {
        var newItem: WidgetMeasurePM = new WidgetMeasurePM(null);
        newItem.Tenant = SessionInfo.LoggedUserTenant;
        newItem.WidgetId = this.EntityPM.Id;
        newItem.MeasureCode = "Sum";
        var newWidgetMeasureItem: WidgetMeasureItem = new WidgetMeasureItem(newItem, true, this);
        this.WidgetMeasuresList.push(newWidgetMeasureItem);
        newWidgetMeasureItem.CheckMeasureDeleteVisiblity();

        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Add Click", DashboardId: this.DashboardPM?.Id });
        this.CheckMeasureAddVisiblity();
    }
}

export class WidgetMeasureItem extends BaseComponent {
    public ObjectTableName: string = "WidgetMeasure";
    public EntityPM: WidgetMeasurePM;
    public Widget: WidgetPM;
    public DataContext: WidgetMeasureItem = this;
    public IsNew: boolean = false;
    public IsDeleteMeasureVisible: boolean = false;
    public FieldQueryFilters: ApiQueryFilters;
    DashboardPM: DashboardPM;

    constructor(entityPM: WidgetMeasurePM, isNew: boolean, public fatherComponent: AddEditWidgetComponent) {
        super();
        this.EntityPM = entityPM;
        this.Widget = fatherComponent.EntityPM;
        this.IsNew = isNew;
        this.DashboardPM = fatherComponent?.DashboardPM;
        this.FilterMeasureFields();
    }

    public CheckMeasureDeleteVisiblity() {
        var index = this.fatherComponent.WidgetMeasuresList.indexOf(this);
        this.IsDeleteMeasureVisible = (index == 1);
    }

    get MeasureFieldId() { return this.EntityPM.MeasureFieldId; }
    set MeasureFieldId(value: string) {
        if (this.EntityPM.MeasureFieldId != value) {
            this.EntityPM.MeasureFieldId = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Field Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    get MeasureCode() { return this.EntityPM.MeasureCode; }
    set MeasureCode(value: string) {
        if (this.EntityPM.MeasureCode == value) return;

        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Type Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        this.EntityPM.MeasureCode = value;
        this.MeasureFieldId = null;
        this.FilterMeasureFields();
    }

    FilterMeasureFields() {
        this.FieldQueryFilters = new ApiQueryFilters();

        switch (this.EntityPM.MeasureCode) {
            case "Avg":
            case "Sum":
                this.FieldQueryFilters.addAdditionalFilter("DataTypeCode", "Integer,Decimal", null, null, "InList", false, true, false, "string", false, true, true);
                break;

            case "Min":
            case "Max":
                this.FieldQueryFilters.addAdditionalFilter("DataTypeCode", "Integer,Decimal,DateTime,Date", null, null, "InList", false, true, false, "string", false, true, true);
                break;

            default:
                break;
        }
    }

    DeleteMeasureClicked() {
        var index = this.fatherComponent.WidgetMeasuresList.indexOf(this);
        if (index != -1) {
            this.fatherComponent.WidgetMeasuresList.splice(index, 1);
        }

        index = this.Widget.WidgetMeasures.indexOf(this.EntityPM);
        if (index != -1) {
            this.Widget.RemoveWidgetMeasure(this.EntityPM);
        }

        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Delete Click", DashboardId: this.DashboardPM?.Id });
        this.fatherComponent.CheckMeasureAddVisiblity();
    }

    
}
