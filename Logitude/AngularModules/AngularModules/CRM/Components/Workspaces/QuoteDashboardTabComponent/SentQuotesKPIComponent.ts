import { Component, OnInit } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { FormatTool } from '../../../../Infrastructure/Tools';
declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    selector: 'sent-quotes-kpi',
    moduleId: module.id,
    templateUrl: './SentQuotesKPIComponent.html',
})

export class SentQuotesKPIComponent implements OnInit {

    private Wizard: QuoteDashboardComponent;
    public SentQuotesKPIDashboardId: string = "SentQuotesKPIDashboardId_";
    public SentQuotesKPIChartID: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    private dashboardArgs: QuoteDashboardArguments;
    private dashboardService: DashboardService;
    public SentQuotesKPIData: Array<ChartingDataClass>;

    constructor(private _entityResourceService: EntityResourceService) {
        this.SetChartId();
    }

    SetChartId() {
        if (this.CurrentSession == null) {
            this.SentQuotesKPIChartID = "SentQuotesKPIChartID_-1_-1";
        }
        else {
            this.SentQuotesKPIChartID = "SentQuotesKPIChartID_" + this.CurrentSession.GetChartId();
        }
        this.SentQuotesKPIDashboardId = this.SentQuotesKPIDashboardId + this.CurrentSession.GetChartId();
    }

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

    ngOnInit() {
        this.dashboardArgs = new QuoteDashboardArguments();
        this.dashboardService = new DashboardService();

        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            this.FillDashboardArgs();
            this.LoadDashboardData();
        });
    }

    FillDashboardArgs() {
        this.dashboardArgs.OwnerId = this.Wizard.OwnerId;
        this.dashboardArgs.BusinessUnitId = this.Wizard.BusinessUnitId;
        this.dashboardArgs.FromDate = this.Wizard.FromDate;
        this.dashboardArgs.ToDate = this.Wizard.ToDate;
        this.dashboardArgs.ChartCode = "KPI";
    }

    LoadDashboardData() {
        this.dashboardService.GetDashboardChartValues(this.dashboardArgs).subscribe((myResult: any) => {
            this.SentQuotesKPIData = myResult;
            this.FillDashboardData();
        });
    }


    private SentQuotesKPIChart: any;
    public IsNoData: boolean = false;
    FillDashboardData() {
        try {
            if (this.SentQuotesKPIChart != null) {
                this.SentQuotesKPIChart.clear();
                this.SentQuotesKPIChart = null;
            }
        }
        catch (er) { }

        this.DrawSentQuotesKPIChart();
    }

    DrawSentQuotesKPIChart11() {
        var maximum = 0;
        var Graphs = [];
        var DataProvider = [];
        var quoteKPIYAxis = [];
        var quoteKPIXAxis = [];
        var j = 0;
        var StringArr: Array<string> = new Array<string>();
        this.SentQuotesKPIData.forEach(element => {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                quoteKPIYAxis[j] = { data: [], label: null, BindingElement: [] };
                quoteKPIYAxis[j].data = [];
                j++;
            }
        });


        try {
            if (quoteKPIXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.SentQuotesKPIDashboardId, Graphs, DataProvider, maximum);
            }
        }
        catch (e) {

        }
        this.IsNoData = false;
    }

    public QuotesKPIYAxis: any[] = [];
    DrawSentQuotesKPIChart() {
        var quotesKPIXAxis = [];
        this.QuotesKPIYAxis = [];
        var Graphs = [];
        var index = 0;

        for (var i = 0; i < this.SentQuotesKPIData.length; i++) {
            this.QuotesKPIYAxis[i] = { data: 0, label: null, BindingElement: null, };
            this.QuotesKPIYAxis[i].data = this.SentQuotesKPIData[i].IntegerProperty;
            this.QuotesKPIYAxis[i].label = this.SentQuotesKPIData[i].Code;
            this.QuotesKPIYAxis[i].BindingElement = this.SentQuotesKPIData[i].StringProperty;

            if (!quotesKPIXAxis.includes(this.SentQuotesKPIData[i].StringProperty) && this.SentQuotesKPIData[i].StringProperty != null) {
                if (quotesKPIXAxis[i] == null) {
                    quotesKPIXAxis[i] = (this.SentQuotesKPIData[i].StringProperty);
                }
            }
        }

        var DataProvider = [];
        var objectArray = [];

        var maximum = 0;
        if (this.QuotesKPIYAxis.length > 0)
            maximum = this.QuotesKPIYAxis[0].data;

        if (maximum == null || maximum === undefined)
            maximum = 0;

        this.QuotesKPIYAxis.forEach(element => {
            if (element.data > maximum) {
                maximum = element.data;
            }
            if (index == 0) {
                Graphs[0] = {
                    "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                    "fillAlphas": 1,
                    "lineAlpha": 0,
                    "id": "AmGraph-1",
                    "title": element.BindingElement + "",
                    "type": "column",
                    "valueField": "col1",
                    "fillColors": ["#007f7f"],
                    "gradientOrientation": "horizontal",
                    "borderAlpha": 0,
                    "showHandOnHover": true,
                };
            }

            objectArray[0] = (element.data);

            DataProvider[index] = { "category": quotesKPIXAxis[index], "col1": objectArray[0] };
            index++;
        });

        try {
            if (quotesKPIXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.SentQuotesKPIDashboardId, Graphs, DataProvider, maximum, null, null, 0);
            }
        }
        catch (e) { }
    }

    isResizing: boolean = false;
    ChartLeft: number = 0;
    lastDownX: number = 0;
    lastDownY: number = 0;
    OnChartMouseDown($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.SentQuotesKPIChartID);
        var rec = grid.getBoundingClientRect();
        this.ChartLeft = rec.left;
        this.lastDownY = ($event.clientY - rec.bottom);
        this.lastDownX = ($event.clientX - this.ChartLeft);
    }

    Click_SentQuotesKPIBar() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }
    }

    OnBarClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null) {
            flag = true;
        }
        item = e.item;
        var myQueryCode: string = "All Quotes";
        var myTableName: string = "Quote";
        var filterAgrs = new ApiQueryFilters();
      
        if (flag) {
            filterAgrs.addAdditionalFilter("SentQuotesKPIChartFilter", ServiceHelper.GetDateString(this.Wizard.FromDate), ServiceHelper.GetDateString(this.Wizard.ToDate) + ";" + item.dataContext.category + "",null , "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
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
}
