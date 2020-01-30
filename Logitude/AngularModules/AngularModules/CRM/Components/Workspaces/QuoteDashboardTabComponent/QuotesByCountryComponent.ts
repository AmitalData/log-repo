import { Component, OnInit } from '@angular/core';import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
declare var makeAmBarChart, PieClick, makePieChart, ResetItemPie: any;

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
        this.CountriesDashboardId = "CountriesDashboardId_" + this.CurrentSession.GetNewId("CountriesDashboardLegendId");    
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

    public CountriesData: ChartingDataClass[];
    private LoadDashboardData() {
        this.dashboardService.GetDashboardChartValues(this.dashboardArgs).subscribe((myResult: any) => {
            this.CountriesData = myResult;
            this.FillDashboardData(this.CountriesData);
        });
    }

    public quotesList: Array<any> = [];
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

        if (data != null && data.length > 0) {
            var pieChartLabels = [];
            var pieChartData = [];
            var fullData = [];
            this.quotesList = data;
            data.forEach(element => {
                fullData.push({ label: element.CountryName, data: element.Total })
                pieChartLabels.push(element.CountryName);
                pieChartData.push(element.Total);

            });
            var flagEmpty = true;
            pieChartData.forEach(p => {
                if (p != "0")
                    flagEmpty = false;
            });
            if (!flagEmpty) {

                this.CurrentCountriesChart = makePieChart(this.CountriesDashboardId, fullData, false, true, this.CountriesDashboardLegendId);
            }

            this.NoCountries = false;
        }

        else {
            this.NoCountries = true;
        }
    }
    
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
    
    ItemClicked() {
        if (PieClick() != null) {
            this.OnDashboardItemClick(PieClick());
            ResetItemPie();
        }
    }
    OnDashboardItemClick(e) {
        var item = this.quotesList[e.index];
        var myQueryCode: string = "All Quotes";
        var myTableName: string = "Quote";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();

        filterAgrs.addAdditionalFilter("CountryForStatisticsId", item.CountryId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper.GetDateString(this.Wizard.FromDate), ServiceHelper.GetDateString(this.Wizard.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
        //filterAgrs.addAdditionalFilter("BusinessUnitId", this.Wizard., null, null, "Equals", true, false, false, "string");

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
