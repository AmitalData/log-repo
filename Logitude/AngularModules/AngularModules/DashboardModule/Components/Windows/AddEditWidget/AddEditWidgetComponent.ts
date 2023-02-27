import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { WidgetPM } from '../../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPM } from '../../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetMeasurePM } from '../../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { AnalyticsFactsFieldsMetaDataList } from 'DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { AppTool } from 'Infrastructure/Tools';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { WidgetFilterItem } from '../Filter/WidgetFilter/WidgetFilterItem';
import { AnalyticsFactsFieldsMetaDataListService } from 'DashboardModule/Services/StandardLists/AnalyticsFactsFieldsMetaDataListService';

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
    private FirstTimeForSecondaryGrouping: boolean = true;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Widget";
    public ChartImageSrc: string;
    public WidgetMeasuresList: WidgetMeasureItem[];
    public WidgetMeasuresClone: WidgetMeasurePM[];
    public RootFilter: WidgetFilterItem = new WidgetFilterItem(null, false, this.DashboardPM?.Id);
    public IsAddNewMeasureVisible: boolean = true;
    public DateGroupCodes = ['Day', 'Month', 'Year', 'Quarter'];
    public SortByCodes = [];
    public PeriodOperators = ['Previous', 'Between'];
    public SortByDirections = [{ name: 'Ascending', code: 'asc' }, { name: 'Descending', code: 'desc' }];
    public LabelsPositions = [{ name: 'On Chart', code: 'OnChart' }, { name: 'In Legend', code: 'InLegend' }];
    public IncreaseDecreases = ['Positive', 'Negative'];
    public GroupByQueryFilters: ApiQueryFilters;
    public SecondaryGroupByQueryFilters: ApiQueryFilters;
    public isGroupByVisible: boolean = true;
    public isSortByVisible: boolean = true;
    public IsBetweenDatesVisible: boolean = false;
    public IsAddNewGroupVisible: boolean = true;
    public IsSecondaryGroupByVisible: boolean = false;
    public IsDeleteGroupByVisible: boolean = false;
    public MeasureRenderList = [];
    public YAxisTypes = [];
    public ImgWitdh: number = 0;
    public Alignments = ['Left', 'Center'];
    public Abbreviations = ['1k', '10k', '100k', '1M', '10M', '100M'];
    public isMeasureNumber: boolean = false;
    public IsDisplaySettingVisibile: boolean = false;
    public IsTimeOverTimeVisible: boolean = false;
    public analyticFieldListService: AnalyticsFactsFieldsMetaDataListService;

    constructor() {
        super();
        this.WidgetMeasuresList = [];
        this.analyticFieldListService = new AnalyticsFactsFieldsMetaDataListService();
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DashboardPM = windowArgs['DashboardPM'];
        this.isNew = windowArgs['IsNew'];
        this.FirstTime = this.isNew != true;
        this.FirstTimeForSecondaryGrouping = this.isNew != true;
        this.DataContext = this;
        this.ComputeChartImageSrc();
        this.SetMeasureData();
        this.BuildMeasures();
        this.CheckMeasureAddVisiblity();
        this.Clone();
        this.GetFilters();
        this.BuildQueryFilters();
        this.InitView();
        this.CheckGroupAddVisiblity();
        this.BuildSecondaryGroup();
        this.SetUIProperties();
        this.SetTimeOverTimeValue();
        this.SetUIForOperator();
        this.CompareDisplaySettingWithDefaultValue();
    }

    SetUIProperties() {
        this.UIProperties.SetValidity("MaximumGrouping", this.ObjectTableName, true, "");

        if (this.MaximumGrouping < 1 || this.MaximumGrouping > 50) {
            this.UIProperties.SetValidity("MaximumGrouping", this.ObjectTableName, false, "Maximum Grouping must be Greater Than 1 and Less Than 50");
        }
        this.UIProperties.SetValidity("DecimalPlaces", this.ObjectTableName, true, "");
        if (this.DecimalPlaces < 0 || this.DecimalPlaces > 2) {
            this.UIProperties.SetValidity("DecimalPlaces", this.ObjectTableName, false, "Decimal Places must be Greater Than or Equal to 0 and Less than or Equal to 2");
        }
        this.UIProperties.SetEnabled("ComparisonPeriod", this.ObjectTableName, this.TimeOverTime);

        this.UIProperties.SetEnabled("GroupById", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityId));
        this.UIProperties.SetEnabled("SecondaryGroupById", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityId));

        this.WidgetMeasuresList.forEach(item => {
            item.SetUIProperties();
        });
        //this.SetUIProprtiesForDisplaySettings();
    }

    BuildQueryFilters() {
        this.GroupByQueryFilters = new ApiQueryFilters();
        this.GroupByQueryFilters.addAdditionalFilter("DataTypeCode", "PickList,LookUp,DateTime,Date", null, null, "InList", false, true, false, "string", false, true, true);

        this.SecondaryGroupByQueryFilters = new ApiQueryFilters();
        this.SecondaryGroupByQueryFilters.addAdditionalFilter("DataTypeCode", "PickList,LookUp", null, null, "InList", false, true, false, "string", false, true, true);
        this.SecondaryGroupByQueryFilters.addAdditionalFilter("CanSecondaryGroup", true, null, null, "Equals", false, false, false, "Boolean");
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
                this.ImgWitdh = 100;
                break;
            }

            case "bar": {
                this.ChartImageSrc = "./Images/Charts/BarChart.png";
                this.ImgWitdh = 150;
                break;
            }

            case "line": {
                this.ChartImageSrc = "./Images/Charts/LineChart.png";
                this.ImgWitdh = 150;
                break;
            }

            case "donut": {
                this.ChartImageSrc = "./Images/Charts/DonutChart.png";
                this.ImgWitdh = 100;
                break;
            }
            case "column": {
                this.ChartImageSrc = "./Images/Charts/ColumnChart.png";
                this.ImgWitdh = 150;
                break;
            }

            case "kpi": {
                this.ChartImageSrc = null;
                this.ImgWitdh = 100;
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

    public BuildSecondaryGroup() {
        if (this.SecondaryGroupById && !this.isNew) {
            this.IsSecondaryGroupByVisible = true;
            this.IsDeleteGroupByVisible = true;
        }
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

    public CheckGroupAddVisiblity() {
        var isAddVisible: boolean = true;

        if (this.EntityPM.TypeCode != "bar" && this.EntityPM.TypeCode != "column") {
            isAddVisible = false;
            if (this.SecondaryGroupById) this.SecondaryGroupById = null;
            if (this.SecondaryDateGroupCode) this.SecondaryDateGroupCode = null;
            this.IsDeleteGroupByVisible = false;
            this.IsSecondaryGroupByVisible = false;
        }

        else if (this.SecondaryGroupById) {
            isAddVisible = false;
        }

        this.BuildSortCodes();
        this.IsAddNewGroupVisible = isAddVisible;
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
            this.SecondaryGroupById = null;
            this.WidgetMeasuresList.forEach(item => {
                item.MeasureFieldId = null;
            });

            this.RootFilter = new WidgetFilterItem(null, false, this.DashboardPM?.Id);
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Entity Change ", Message: "Changed To" + this.EntityPM.EntityId, DashboardId: this.DashboardPM?.Id });
        }
        this.TimeOverTime = false;
        this.SetTimeOverTimeValue();
        this.SetUIProperties();
    }


    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            this.EntityPM.TypeCode = value;
            this.ComputeChartImageSrc();
            this.CheckMeasureAddVisiblity();
            this.InitView();
            this.SetTimeOverTimeDefaultValue();
            this.SetTimeOverTimeValue();
            this.CheckGroupAddVisiblity();
            this.SetMaximumGrouping();
            this.SetMeasureData();
            this.WidgetMeasuresList.forEach(item => {
                item.FilterMeasureFields();
            });
            this.SetDefaultValuesForDisplaySettings();
            this.SetUIProprtiesForDisplaySettings();
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Type Change ", Message: "Changed To" + this.EntityPM.TypeCode, DashboardId: this.DashboardPM?.Id });
        }
    }

    SetMeasureData() {
        switch (this.EntityPM.TypeCode) {
            case "column":
                this.MeasureRenderList = [{ name: 'Show as Column', code: 'default' }, { name: 'Show as Line', code: 'line' }];
                this.YAxisTypes = [{ name: 'Single Y-Axis', code: 'single' }, { name: 'Dual Y-Axis', code: 'dual' }];
                break;
            default:
                this.MeasureRenderList = [];
                this.YAxisTypes = [];
                break;
        }
        if (this.WidgetMeasuresList && this.WidgetMeasuresList.length > 1) {
            this.SetDefaultRender(this.WidgetMeasuresList[1]);
            this.SetDefaultYAxisType(this.WidgetMeasuresList[1]);
        }
    }

    private SetTimeOverTimeDefaultValue() {
        if (this.TypeCode != "kpi") {
            this.TimeOverTime = false;
        }
    }

    private SetTimeOverTimeValue() {
        if (this.TypeCode == "kpi" && this.TimeOverTime) {
            this.Increase = this.Increase ? this.Increase : "Positive";
            return;
        }
        this.ComparisonOperator = null;
        this.ComparisonPeriod = null;
        this.ComparisonDateGroup = null;
        this.FromDate = null;
        this.ToDate = null;
        this.Increase = null;
    }

    SetUIForOperator() {
        this.IsBetweenDatesVisible = false;
        if (this.ComparisonOperator == "Between") {
            this.IsBetweenDatesVisible = true;
        }
    }

    private SetMaximumGrouping() {
        if (!AppTool.IsNullOrEmpty(this.TypeCode) && this.TypeCode != "kpi") {
            if (AppTool.IsNullOrZero(this.MaximumGrouping))
                this.MaximumGrouping = 10;
        }
        else this.MaximumGrouping = null;

    }

    SetDefaultValuesForDisplaySettings() {
        this.ThousandSeparator = false;
        this.UseNumberAbbreviation = false;
        this.UseAbbreviationAfter = null;
        this.DecimalPlaces = null;
        this.LabelsPosition = null;

        if (this.TypeCode == "kpi" && (this.WidgetMeasuresList[0].MeasureFieldId == null || this.WidgetMeasuresList[0].IsNumeric)) {
            this.ThousandSeparator = true;
            this.UseNumberAbbreviation = true;
            this.UseAbbreviationAfter = "100k";
            this.DecimalPlaces = 2;
            this.Alignment = "Center";
        }
        if (this.TypeCode == "pie" || this.TypeCode == "donut") {
            this.LabelsPosition = 'OnChart';
        }
    }
    SetDisplaySettingsDefaultsForKpiAndMeasureCode() {
        this.Alignment = "Center";
        this.ThousandSeparator = true;
        this.UseNumberAbbreviation = true;
        this.UseAbbreviationAfter = "100k";
        this.DecimalPlaces = 2;

    }

    SetUIProprtiesForDisplaySettings() {
        if (!this.WidgetMeasuresList[0].IsNumeric) {
            this.UIProperties.SetEnabled("DecimalPlaces", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("UseNumberAbbreviation", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ThousandSeparator", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("UseAbbreviationAfter", this.ObjectTableName, false);
            return;
        }


        this.UIProperties.SetEnabled("DecimalPlaces", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("UseNumberAbbreviation", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ThousandSeparator", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("UseAbbreviationAfter", this.ObjectTableName, this.UseNumberAbbreviation);
    }

    CompareDisplaySettingWithDefaultValue() {
        if (this.isNew) return;
        if (this.Alignment != "Center") {
            this.IsDisplaySettingVisibile = true;
            return;
        }

        if (this.ThousandSeparator == null || this.UseNumberAbbreviation == null || this.DecimalPlaces == null) return;

        if (this.ThousandSeparator == false || this.UseNumberAbbreviation == false || (this.UseNumberAbbreviation == true && this.UseAbbreviationAfter != "100k") || this.DecimalPlaces != 2) {
            this.IsDisplaySettingVisibile = true;
            return;
        }
        this.IsDisplaySettingVisibile = false;
    }

    InitView() {
        this.isGroupByVisible = this.TypeCode != "kpi";
        this.isSortByVisible = this.TypeCode != "kpi";

        if (!this.isGroupByVisible) {
            this.GroupById = null;
            this.DateGroupCode = null;
            this.SecondaryGroupById = null;
            this.SecondaryDateGroupCode = null;
        }
        if (!this.isSortByVisible) {
            this.SortBy = null;
            this.SortDirection = null;
        }
        if (this.TypeCode == "kpi") {
            this.MaximumGrouping = null;
        }
    }

    get GroupById() { return this.EntityPM.GroupById; }
    set GroupById(value: string) {
        if (this.EntityPM.GroupById != value) {
            this.EntityPM.GroupById = value;
        }
    }

    get SecondaryGroupById() { return this.EntityPM.SecondaryGroupById; }
    set SecondaryGroupById(value: string) {
        if (this.EntityPM.SecondaryGroupById != value) {
            this.EntityPM.SecondaryGroupById = value;
        }

    }

    get SecondaryDateGroupCode() { return this.EntityPM.SecondaryDateGroupCode; }
    set SecondaryDateGroupCode(value: string) {
        if (this.EntityPM.SecondaryDateGroupCode != value) {
            this.EntityPM.SecondaryDateGroupCode = value;
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

    get TimeOverTime() { return this.EntityPM.TimeOverTime; }
    set TimeOverTime(value: boolean) {
        if (this.EntityPM.TimeOverTime != value) {
            this.EntityPM.TimeOverTime = value;
            this.SetUIProperties();
            this.SetTimeOverTimeValue();
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

    get LabelsPosition() { return this.EntityPM.LabelsPosition; }
    set LabelsPosition(value: string) {
        if (this.EntityPM.LabelsPosition != value) {
            this.EntityPM.LabelsPosition = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Labels Position Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }


    get MaximumGrouping() {
        return this.EntityPM.MaximumGrouping;
    }
    set MaximumGrouping(value: number) {
        if (this.EntityPM.MaximumGrouping != value) {
            this.EntityPM.MaximumGrouping = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Maximum Grouping Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
            this.SetUIProperties();

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

    get ComparisonOperator() { return this.EntityPM.ComparisonOperator; }
    set ComparisonOperator(value: string) {
        if (this.EntityPM.ComparisonOperator != value) {
            this.EntityPM.ComparisonOperator = value;

            this.SetUIForOperator();
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Comparison Period Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    get ComparisonPeriod() { return this.EntityPM.ComparisonPeriod; }
    set ComparisonPeriod(value: number) {
        if (this.EntityPM.ComparisonPeriod != value) {
            this.EntityPM.ComparisonPeriod = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Comparison Period Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get ComparisonDateGroup() { return this.EntityPM.ComparisonDateGroup; }
    set ComparisonDateGroup(value: string) {
        if (this.EntityPM.ComparisonDateGroup != value) {
            this.EntityPM.ComparisonDateGroup = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Date Group Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get Increase() { return this.EntityPM.Increase; }
    set Increase(value: string) {
        if (this.EntityPM.Increase != value) {
            this.EntityPM.Increase = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Increase Decrease Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }


    get FromDate() { return this.EntityPM.FromDate; }
    set FromDate(value: Date) {
        if (this.EntityPM.FromDate != value) {
            this.EntityPM.FromDate = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget From Date Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get ToDate() { return this.EntityPM.ToDate; }
    set ToDate(value: Date) {
        if (this.EntityPM.ToDate != value) {
            this.EntityPM.ToDate = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget To Date Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    public GetSelectedSort() {
        return this.SortByCodes.find(x => x.Code == this.SortBy);
    }
    public GetSelectedSortDirection() {
        return this.SortByDirections.find(x => x.code == this.SortDirection);
    }

    public GetSelectedLabelsPosition() {
        return this.LabelsPositions.find(x => x.code == this.LabelsPosition);
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



    public selectedSecondaryGroupField: AnalyticsFactsFieldsMetaDataList = null;
    get SelectedSecondaryGroupField() { return this.selectedSecondaryGroupField; }
    set SelectedSecondaryGroupField(value: AnalyticsFactsFieldsMetaDataList) {
        if (this.selectedSecondaryGroupField?.Id == value?.Id) {
            this.FirstTimeForSecondaryGrouping = false;
            return;
        }
        this.selectedSecondaryGroupField = value;
        if (this.FirstTimeForSecondaryGrouping) {
            this.FirstTimeForSecondaryGrouping = false;
            return;
        }
        this.EntityPM.SecondaryDateGroupCode = (value?.DataTypeCode == 'DateTime' || value?.DataTypeCode == 'Date') ? this.DateGroupCodes[0] : null;
        this.FirstTimeForSecondaryGrouping = false;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Secondary Group Change", Message: "Changed To " + this.selectedSecondaryGroupField?.DisplayName, DashboardId: this.DashboardPM?.Id });

    }

    get Alignment() { return this.EntityPM.Alignment; }
    set Alignment(value: string) {
        if (this.EntityPM.Alignment != value) {
            this.EntityPM.Alignment = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Alignment Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get ThousandSeparator() { return this.EntityPM.ThousandSeparator; }
    set ThousandSeparator(value: boolean) {
        if (this.EntityPM.ThousandSeparator != value) {
            this.EntityPM.ThousandSeparator = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Thousand Separator Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get UseNumberAbbreviation() { return this.EntityPM.UseNumberAbbreviation; }
    set UseNumberAbbreviation(value: boolean) {
        if (this.EntityPM.UseNumberAbbreviation != value) {
            this.EntityPM.UseNumberAbbreviation = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Use Number Abbreviation Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get UseAbbreviationAfter() { return this.EntityPM.UseAbbreviationAfter; }
    set UseAbbreviationAfter(value: string) {
        if (this.EntityPM.UseAbbreviationAfter != value) {
            this.EntityPM.UseAbbreviationAfter = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget UseAbbreviationAfter Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }
    get DecimalPlaces() { return this.EntityPM.DecimalPlaces; }
    set DecimalPlaces(value: number) {
        if (this.EntityPM.DecimalPlaces != value) {
            this.EntityPM.DecimalPlaces = value;
            this.SetUIProperties();
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget DecimalPlaces Value Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    SetDefaultSortByField() {
        if (!this.selectedGroupField) return;
        if (this.selectedGroupField.DataTypeCode == "DateTime" || this.selectedGroupField.DataTypeCode == 'Date') {
            this.SortBy = null;
            this.SortDirection = "asc";
            return;
        }
        this.SortBy = 1;
        this.SortDirection = "desc";
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
        if (this.TypeCode != "kpi" && !this.MaximumGrouping && this.MaximumGrouping != 0) {
            errors.push("Maximum Grouping Field is Required");
        }
        if (this.TypeCode != "kpi" && this.MaximumGrouping && (this.MaximumGrouping < 1 || this.MaximumGrouping > 50)) {
            errors.push("Maximum Grouping must be greater than or equal 1 and less than or equal 50");
        }
        if (this.TypeCode != "kpi" && this.MaximumGrouping == 0) {
            errors.push("Maximum Grouping must be greater than or equal 1 and less than or equal 50");
        }
        if (this.isSortByVisible && !this.SortDirection) {
            errors.push("Sort By Direction Field is Required");
        }
        if (this.IsSecondaryGroupByVisible && !this.SecondaryGroupById) {
            errors.push("Secondary Group Field is Required");
        }

        this.ValidateTimeOverTime(errors);
        this.ValidateMeasures(errors);
        this.ValidateSort(errors);
        this.ValidateDisplaySettings(errors);
        if (this.RootFilter && this.RootFilter.QueryFilterItems && this.RootFilter.QueryFilterItems.length != 0) this.ValidateFilters(errors, this.RootFilter);
    }
    private ValidateDisplaySettings(errors: string[]) {
        if (this.TypeCode != "kpi") return;
        if (!this.Alignment) {
            errors.push("Alignment Field is Required");
        }
        if (!this.WidgetMeasuresList[0].IsNumeric) return;

        if (this.UseNumberAbbreviation && !this.UseAbbreviationAfter) {
            errors.push("Use Abbreviation After Field is Required");
        }

        if (this.DecimalPlaces && (this.DecimalPlaces < 0 || this.DecimalPlaces > 2)) {
            errors.push("Decimal Places must be Greater Than or Equal to 0 and Less than or Equal to 2");
        }

        if (!this.DecimalPlaces && this.DecimalPlaces != 0) {
            errors.push("Decimal Places Field is Required");
        }
    }
    private ValidateTimeOverTime(errors: string[]) {

        if (this.TypeCode != "kpi") return;
        if (!this.TimeOverTime) return;

        if (!this.Increase) {
            errors.push("Increase Field is Required");
        }

        if (!this.ComparisonOperator) {
            errors.push("Comparison Operater Field is Required");
            return;
        }

        if (this.ComparisonOperator == "Between") {
            if (!this.FromDate) {
                errors.push("From Date Field is Required");
            }
            if (!this.ToDate) {
                errors.push("To Date Field is Required");
            }
            if (this.FromDate && this.ToDate && this.FromDate > this.ToDate) {
                errors.push("From Date Value should be Before To Date Value");
            }
        }

        else {
            if (!this.ComparisonPeriod) {
                errors.push("Comparison Period Field is Required");
            }
            if (this.ComparisonPeriod && this.ComparisonPeriod < 1) {
                errors.push("Comparison Period should be Graeter than 0");
            }
            if (!this.ComparisonDateGroup) {
                errors.push("Comparison Date Group Field is Required");
            }
        }
    }

    GroupByDateIsNotValid() {
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
        this.myCloner.AddField('GroupById');
        this.myCloner.AddField('StartPotistion');
        this.myCloner.AddField('EndPosition');
        this.myCloner.AddField('EntityId');
        this.myCloner.AddField('DateGroupCodes');
        this.myCloner.AddField('SortDirection');
        this.myCloner.AddField('SortBy');
        this.myCloner.AddField('MaximumGrouping');
        this.myCloner.AddField('TimeOverTime');
        this.myCloner.AddField('ComparisonOperator');
        this.myCloner.AddField('ComparisonPeriod');
        this.myCloner.AddField('ComparisonDateGroup');
        this.myCloner.AddField('Increase');
        this.myCloner.AddField('FromDate');
        this.myCloner.AddField('ToDate');
        this.myCloner.AddField('SecondaryGroupById');
        this.myCloner.AddField('SecondaryDateGroupCode');
        this.myCloner.AddField('Alignment');
        this.myCloner.AddField('ThousandSeparator');
        this.myCloner.AddField('UseNumberAbbreviation');
        this.myCloner.AddField('UseAbbreviationAfter');
        this.myCloner.AddField('DecimalPlaces');
        this.myCloner.AddField('LabelsPosition');
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
        this.SetDefaultRender(newWidgetMeasureItem);
        this.SetDefaultYAxisType(newWidgetMeasureItem);
        this.WidgetMeasuresList.push(newWidgetMeasureItem);
        newWidgetMeasureItem.CheckMeasureDeleteVisiblity();

        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Add Click", DashboardId: this.DashboardPM?.Id });
        this.CheckMeasureAddVisiblity();
    }

    private SetDefaultRender(widgetMeasureItem: WidgetMeasureItem) {
        if (this.MeasureRenderList.length > 0) widgetMeasureItem.RenderAs = this.MeasureRenderList[0].code;
        else widgetMeasureItem.RenderAs = null;
    }

    private SetDefaultYAxisType(widgetMeasureItem: WidgetMeasureItem) {
        if (this.YAxisTypes.length > 0) widgetMeasureItem.YAxisType = this.YAxisTypes[0].code;
        else widgetMeasureItem.YAxisType = null;
    }

    AddNewGroupByClicked() {
        this.IsAddNewGroupVisible = false;
        this.IsSecondaryGroupByVisible = true;
        this.IsDeleteGroupByVisible = true;
        this.selectedSecondaryGroupField = null;
        this.FirstTimeForSecondaryGrouping = false;
    }

    DeleteGroupByClicked() {
        this.IsSecondaryGroupByVisible = false;
        this.IsAddNewGroupVisible = true;
        this.SecondaryGroupById = null;
        this.SecondaryDateGroupCode = null;
        this.SelectedSecondaryGroupField = null;
    }


    get IsMaximumGrouping(): boolean {
        return this.TypeCode && this.TypeCode != 'kpi';
    }

    get IsKpi(): boolean {
        return this.TypeCode == 'kpi';
    }

    get IsUseAbbreviationAfterDisabled(): boolean {
        if (!this.UseNumberAbbreviation) return true;
        if (this.WidgetMeasuresList[0].MeasureCode != 'Count' && !this.WidgetMeasuresList[0].MeasureFieldId) return true;
        if (this.WidgetMeasuresList[0].MeasureCode != 'Count' && !this.WidgetMeasuresList[0].IsNumeric) return true;

        return false;
    }

    public get CanSort(): boolean {
        return this.SelectedGroupField && (this.SelectedGroupField.DataTypeCode != "DateTime" && this.SelectedGroupField.DataTypeCode != 'Date');
    }
}

export class WidgetMeasureItem extends BaseComponent {
    public ObjectTableName: string = "WidgetMeasure";
    public EntityPM: WidgetMeasurePM;
    public Widget: WidgetPM;
    public DataContext: WidgetMeasureItem = this;
    public IsNew: boolean = false;
    public FielHasOldValue: boolean = false;
    public IsDeleteMeasureVisible: boolean = false;
    public FieldQueryFilters: ApiQueryFilters;
    public DashboardPM: DashboardPM;
    private selectedField: AnalyticsFactsFieldsMetaDataList;



    public get DefaultRender(): any {
        return this.fatherComponent?.MeasureRenderList?.find(x => x.code == this.RenderAs);
    }

    public get DefaultYAxisType(): any {
        return this.fatherComponent?.YAxisTypes?.find(x => x.code == this.YAxisType);
    }

    constructor(entityPM: WidgetMeasurePM, isNew: boolean, public fatherComponent: AddEditWidgetComponent) {
        super();
        this.EntityPM = entityPM;
        this.Widget = fatherComponent.EntityPM;
        this.IsNew = isNew;
        this.FielHasOldValue = isNew == false;
        this.DashboardPM = fatherComponent?.DashboardPM;
        this.FilterMeasureFields();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("MeasureCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.fatherComponent.EntityId));
        this.UIProperties.SetEnabled("MeasureFieldId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.fatherComponent.EntityId));
    }

    public CheckMeasureDeleteVisiblity() {
        var index = this.fatherComponent.WidgetMeasuresList.indexOf(this);
        this.IsDeleteMeasureVisible = (index == 1);
    }

    public get ShowRenderList(): boolean {
        return this.fatherComponent.WidgetMeasuresList.indexOf(this) == 1 && this.Widget.TypeCode == "column";
    }

    public get ShowYAxisTypes(): boolean {
        return this.fatherComponent.WidgetMeasuresList.indexOf(this) == 1 && this.Widget.TypeCode == "column";
    }

    get MeasureFieldId() { return this.EntityPM.MeasureFieldId; }
    set MeasureFieldId(value: string) {
        if (this.EntityPM.MeasureFieldId != value) {
            this.EntityPM.MeasureFieldId = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Field Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
            if (!value) this.FieldChanged(null);
        }
    }

    get MeasureCode() { return this.EntityPM.MeasureCode; }
    set MeasureCode(value: string) {
        if (this.EntityPM.MeasureCode == value) return;

        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Type Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        this.EntityPM.MeasureCode = value;
        this.MeasureFieldId = null;
        this.FilterMeasureFields();
        this.fatherComponent.SetUIProprtiesForDisplaySettings();
        this.fatherComponent.SetDefaultValuesForDisplaySettings()
    }

    get RenderAs() { return this.EntityPM.RenderAs; }
    set RenderAs(value: string) {
        if (this.EntityPM.RenderAs == value) return;
        this.EntityPM.RenderAs = value;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Show as Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
    }

    get YAxisType() { return this.EntityPM.YAxisType; }
    set YAxisType(value: string) {
        if (this.EntityPM.YAxisType == value) return;
        this.EntityPM.YAxisType = value;
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Y Axis Type as Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
    }

    get SelectedField() { return this.selectedField; }
    set SelectedField(value: AnalyticsFactsFieldsMetaDataList) {
        if (this.selectedField != value) {
            this.selectedField = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Widget Measure Field Change", Message: "Changed To " + value, DashboardId: this.DashboardPM?.Id });
        }
    }

    FilterMeasureFields() {
        this.FieldQueryFilters = new ApiQueryFilters();

        if (this.fatherComponent.TypeCode != "kpi") {
            this.FieldQueryFilters.addAdditionalFilter("DataTypeCode", "Integer,Decimal", null, null, "InList", false, true, false, "string", false, true, true);
            return;
        }

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

    FieldChanged(field: AnalyticsFactsFieldsMetaDataList) {
        if (this.FielHasOldValue) {
            this.FielHasOldValue = false;
            this.selectedField = field;
            if (!this.isNumericType(this.selectedField?.DataTypeCode)) this.fatherComponent.SetDefaultValuesForDisplaySettings();
            this.fatherComponent.SetUIProprtiesForDisplaySettings();
            return;
        }

        var typeChange = this.selectedField == null || this.isNumericType(this.selectedField?.DataTypeCode) != this.isNumericType(field?.DataTypeCode);
        this.selectedField = field;
        this.fatherComponent.SetUIProprtiesForDisplaySettings();
        if (typeChange || this.MeasureCode == "Count") this.fatherComponent.SetDefaultValuesForDisplaySettings();
    }


    get IsNumeric(): boolean {
        return this.isNumericType(this.selectedField?.DataTypeCode);
    }

    isNumericType(typeCode: string): boolean {
        return typeCode == "Integer" || typeCode == "Decimal" || this.MeasureCode == "Count";
    }
}
