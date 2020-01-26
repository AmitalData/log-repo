import { Component, OnInit } from '@angular/core';import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { DirectionTransportFilter } from '../../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter';

@Component({
    selector: 'quotes-by-country',
    moduleId: module.id,
    templateUrl: './QuotesByCountryComponent.html'
})

export class QuotesByCountryComponent implements OnInit {
    public CountriesDashboardId: string = "CountriesDashboardId_";
    public CountriesDashboardLegendId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    private dashboardArgs: QuoteDashboardArguments;
    private dashboardService: DashboardService;

    constructor(private _entityResourceService: EntityResourceService) {
        this.CountriesDashboardId = this.CountriesDashboardId + this.CurrentSession.GetChartId();
        this.CountriesDashboardLegendId = "CountriesDashboardLegendId_" + this.CurrentSession.GetNewId("CountriesDashboardLegendId");    
    }

    ngOnInit() {
        this.dashboardArgs = new QuoteDashboardArguments();
        this.dashboardService = new DashboardService();

        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            this.FillDashboardArgs();
            this.LoadDashboardData();
        });
    }

    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        console.log("Init Tab");
    }
    RefreshTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        this.FillDashboardArgs();
        this.LoadDashboardData();
        console.log("Refresh Tab");
    }

    private FillDashboardArgs() {
        this.dashboardArgs.OwnerId = this.Wizard.OwnerId;
        this.dashboardArgs.BusinessUnitId = this.Wizard.BusinessUnitId;
        this.dashboardArgs.FromDate = this.Wizard.FromDate;
        this.dashboardArgs.ToDate = this.Wizard.ToDate;
        this.dashboardArgs.ChartCode = "QOC";
        this.dashboardArgs.TransportModeId = this.SelectedTransportFilter;
        this.dashboardArgs.DirectionId = this.SelectedDirectionFilter;
        this.dashboardArgs.IncludeOthersCountries = this.IncludeOthersCountries;
        this.dashboardArgs.TopCountries = this.TopCountries;
    }

    public CountriesData: any;
    private LoadDashboardData() {
        //var directtionTransportFilter: DirectionTransportFilter = this.GetCurrentDirectionTransmodeFilterItemCountries();

        this.dashboardService.GetDashboardChartValues(this.dashboardArgs).subscribe((myResult: any) => {
            this.CountriesData = myResult;
            this.FillDashboardData();
        });
    }

    public NoCountries: boolean = false;
    FillDashboardData() {
        //var fullData = [];
        //var pieChartLabels = [];
        //var pieChartData = [];

        //data.getAll() != null ? data.getAll().forEach(element => {
        //    if (element.YField != 0) {
        //        fullData.push({ label: element.XField, data: element.YField })
        //        pieChartLabels.push(element.XField);
        //        pieChartData.push(element.YField);
        //    }
        //}) : null;
        
        //var flagEmpty = true;
        //pieChartData.forEach(p => {
        //    if (p != "0")
        //        flagEmpty = false;
        //});

        //if (this.CurrentCountriesChart != null) {
        //    this.CurrentCountriesChart.clear();
        //    this.CurrentCountriesChart = null;
        //}
        //if (!flagEmpty) {

        //    this.CurrentCountriesChart = makePieChart(this.CountriesDashboardId, fullData, false, true, this.CountriesDashboardLegendId);

        //    this.NoCountries = false;

        //}

        //else {
        //    this.NoCountries = true;
        //}
    }

    //private GetCurrentDirectionTransmodeFilterItemCountries() {
    //    var transmodeId: string = "";
    //    var directionId: string = "";

    //    if (this.SelectedDirectionFilter == "All") {
    //        directionId = "";
    //    }
    //    else {
    //        directionId = this.SelectedDirectionFilter;
    //    }

    //    if (this.SelectedTransportFilter == "All") {
    //        transmodeId = "";
    //    }
    //    else {
    //        transmodeId = this.SelectedTransportFilter;
    //    }

    //    var filterItem: DirectionTransportFilter = new DirectionTransportFilter("", directionId, transmodeId);
    //    return filterItem;
    //}

    private selectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.selectedTransportFilter; }
    set SelectedTransportFilter(newValue: string) {
        if (this.selectedTransportFilter != newValue) {
            this.selectedTransportFilter = newValue;

            this.FillDashboardArgs();
            this.LoadDashboardData();
        }
    }

    private selectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.selectedDirectionFilter; }
    set SelectedDirectionFilter(newValue: string) {
        if (this.selectedDirectionFilter != newValue) {
            this.selectedDirectionFilter = newValue;

            this.FillDashboardArgs();
            this.LoadDashboardData();
        }
    }

    public includeOthersCountries: any = true;
    public get IncludeOthersCountries() { return this.includeOthersCountries; }
    public set IncludeOthersCountries(newValue: boolean) {
        if (this.includeOthersCountries != newValue) {
            this.includeOthersCountries = newValue;

            this.FillDashboardArgs();
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

        this.FillDashboardArgs();
        this.LoadDashboardData();
    }
}
