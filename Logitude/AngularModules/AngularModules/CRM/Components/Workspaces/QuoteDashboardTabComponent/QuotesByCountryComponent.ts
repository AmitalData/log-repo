import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
declare var PieClick, makePieChart, ResetItemPie: any;

@Component({
    selector: 'quotes-by-country',
    
    templateUrl: './QuotesByCountryComponent.html'
})

export class QuotesByCountryComponent {
    public ChartId: string = "CountriesDashboardId_";
    public LegendId: string;
    private chartService: DashboardService;
    private chartArgs: QuoteDashboardArguments;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.chartService = new DashboardService();
        this.chartArgs = new QuoteDashboardArguments();
        this.chartArgs.ChartCode = "QOC";
        this.ChartId = "CountriesDashboardId_" + this.CurrentSession.GetNewId("CountriesDashboardId");    
        this.LegendId = "CountriesDashboardLegendId_" + this.CurrentSession.GetNewId("CountriesDashboardLegendId");
    }

    Update(comp: QuoteDashboardComponent) {
        this.chartArgs.OwnerId = comp.OwnerId;
        this.chartArgs.BusinessUnitId = comp.BusinessUnitId;
        this.chartArgs.FromDate = comp.FromDate;
        this.chartArgs.ToDate = comp.ToDate;
        this.UpdateLocalArgs();
        this.LoadDashboardData();
    }

    UpdateLocalArgs() {
        this.chartArgs.TransportModeId = this.SelectedTransportFilter;
        this.chartArgs.DirectionId = this.SelectedDirectionFilter;
        this.chartArgs.IncludeOthersCountries = this.IncludeOthersCountries;
        this.chartArgs.TopCountries = this.TopCountries;
    }

    private selectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.selectedTransportFilter; }
    set SelectedTransportFilter(newValue: string) {
        if (this.selectedTransportFilter != newValue) {
            this.selectedTransportFilter = newValue;

            this.UpdateLocalArgs();
            this.LoadDashboardData();
        }
    }

    private selectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.selectedDirectionFilter; }
    set SelectedDirectionFilter(newValue: string) {
        if (this.selectedDirectionFilter != newValue) {
            this.selectedDirectionFilter = newValue;

            this.UpdateLocalArgs();
            this.LoadDashboardData();
        }
    }

    public includeOthersCountries: any = true;
    public get IncludeOthersCountries() { return this.includeOthersCountries; }
    public set IncludeOthersCountries(newValue: boolean) {
        if (this.includeOthersCountries != newValue) {
            this.includeOthersCountries = newValue;

            this.UpdateLocalArgs();
            this.LoadDashboardData();
        }
    }

    public topCountries: any = 10;
    public get TopCountries() { return this.topCountries; }
    public set TopCountries(value: any) { this.topCountries = value; }

    CountriesTopValueChanged(flag) {
        if (flag) {
            this.TopCountries = this.TopCountries + 1;
        }

        else {
            this.TopCountries = this.TopCountries - 1;
        }

        if (this.TopCountries < 0) {
            this.TopCountries = 0;
        }

        this.UpdateLocalArgs();
        this.LoadDashboardData();
    }

    public CountriesData: ChartingDataClass[];
    private LoadDashboardData() {
        this.chartService.GetDashboardChartValues(this.chartArgs).subscribe((myResult: any) => {
            this.CountriesData = myResult;
            this.FillDashboardData(this.CountriesData);
        });
    }

    public QuoteList: Array<any> = [];
    private CurrentCountriesChart: any;
    public NoCountries: boolean = false;
    FillDashboardData(data: ChartingDataClass[]) {
        try {
            if (this.CurrentCountriesChart != null) {
                this.CurrentCountriesChart.clear();
                this.CurrentCountriesChart = null;
            }
        }
        catch (er) { }

        if (data.length == 0) {
            this.NoCountries = true;
        }

        else {
            this.DrawPieChart(data);
        }        
    }

    DrawPieChart(data: ChartingDataClass[]) {
        var fullData = [];
        this.QuoteList = data;

        data.forEach(element => {
            fullData.push({ label: element.CountryName, data: element.Total });
        });
        
        this.CurrentCountriesChart = makePieChart(this.ChartId, fullData, false, true, this.LegendId, 150);
        this.NoCountries = false;
    }
    
    ItemClicked() {
        if (PieClick() != null) {
            this.OnDashboardItemClick(PieClick());
            ResetItemPie();
        }
    }
    OnDashboardItemClick(e) {
        var item = this.QuoteList[e.index];
        var myQueryCode: string = "All Quotes";
        var myTableName: string = "Quote";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
        
        filterAgrs.addAdditionalFilter("CountryForStatisticsId", item.CountryId, null, null, "InList", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper.GetDateString(this.chartArgs.FromDate), ServiceHelper.GetDateString(this.chartArgs.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
        filterAgrs.addAdditionalFilter("SalesmanUserId", this.chartArgs.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", this.chartArgs.BusinessUnitId, null, null, "Equals", true, false, false, "string");

        if (this.SelectedDirectionFilter != "All") {
            filterAgrs.addAdditionalFilter("DirectionId", this.SelectedDirectionFilter, null, null, "Equals", false, false, false, "String");
        }

        if (this.SelectedTransportFilter != "All") {
            filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", true, false, false, "string");
        }

        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Quotes";
        listArgs.BackButtonTitle = "CRM";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadDashboardData());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });
    }
}
