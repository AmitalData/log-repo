import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { FormatTool } from '../../../../Infrastructure/Tools';
import { QuoteStageListService } from '../../../../Quote/Services/StandardLists/QuoteStageListService';
import { QuoteStageList } from '../../../../Quote/EntityLists/QuoteStageList';

declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    selector: 'sent-quotes-kpi',
    
    templateUrl: './SentQuotesKPIComponent.html',
})

export class SentQuotesKPIComponent  {
    private chartService: DashboardService;
    private chartArgs: QuoteDashboardArguments;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.SetChartId();
        this.chartService = new DashboardService();
        this.chartArgs = new QuoteDashboardArguments();
        this.chartArgs.ChartCode = "KPI";
    }

    public SentQuotesKPIDashboardId: string = "SentQuotesKPIDashboardId_";
    public SentQuotesKPIChartID: string = null;
    SetChartId() {
        if (this.CurrentSession == null) {
            this.SentQuotesKPIChartID = "SentQuotesKPIChartID_-1_-1";
        }
        else {
            this.SentQuotesKPIChartID = "SentQuotesKPIChartID_" + this.CurrentSession.GetChartId();
        }
        this.SentQuotesKPIDashboardId = this.SentQuotesKPIDashboardId + this.CurrentSession.GetChartId();
    }

    Update(comp: QuoteDashboardComponent) {
        this.chartArgs.OwnerId = comp.OwnerId;
        this.chartArgs.BusinessUnitId = comp.BusinessUnitId;
        this.chartArgs.FromDate = comp.FromDate;
        this.chartArgs.ToDate = comp.ToDate;
        this.LoadDashboardData();
    }

    public SentQuotesKPIData: Array<ChartingDataClass>;
    public NoQuotesData = false;
    LoadDashboardData() {
        this.chartService.GetDashboardChartValues(this.chartArgs).subscribe((myResult: any) => {
            this.SentQuotesKPIData = myResult;
            this.CheckIfEmptyList();
            this.FillDashboardData();
        });
    }

    private CheckIfEmptyList() {
        this.NoQuotesData = false;
        var isEmpty = this.SentQuotesKPIData.filter(a => a.DoubleProperty != 0);
        if (!(isEmpty != null && isEmpty.length > 0)) {
            this.NoQuotesData = true;
        }

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

    public QuotesKPIYAxis: any[] = [];
    DrawSentQuotesKPIChart() {
        var quotesKPIXAxis = [];
        this.QuotesKPIYAxis = [];
        var Graphs = [];
        var index = 0;

        for (var i = 0; i < this.SentQuotesKPIData.length; i++) {
            this.QuotesKPIYAxis[i] = { data: 0.0, maximumValue : 0, label: null, BindingElement: null, };
            this.QuotesKPIYAxis[i].data = this.SentQuotesKPIData[i].DoubleProperty;
            this.QuotesKPIYAxis[i].label = this.SentQuotesKPIData[i].Code;
            this.QuotesKPIYAxis[i].maximumValue = this.SentQuotesKPIData[i].IntegerProperty;
            this.QuotesKPIYAxis[i].data = this.SentQuotesKPIData[i].DoubleProperty;
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
            maximum = this.QuotesKPIYAxis[0].maximumValue;

        if (maximum == null || maximum === undefined)
            maximum = 0;

        this.QuotesKPIYAxis.forEach(element => {
            if (element.maximumValue > maximum) {
                maximum = element.maximumValue;
            }
            if (index == 0) {
                Graphs[0] = {
                    "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + " %",
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
                makeAmBarChart(this.SentQuotesKPIDashboardId, Graphs, DataProvider, maximum, null, null, 0,0,null, "Percentage  (%)");
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
            filterAgrs.addAdditionalFilter("SentQuotesKPIChartFilter", ServiceHelper.GetDateString(this.chartArgs.FromDate), ServiceHelper.GetDateString(this.chartArgs.ToDate) + ";" + item.dataContext.category + "",null , "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
            //filterAgrs.addAdditionalFilter("IsClosed", true, null, null, "Equals", true, false, false, "Boolean");
            //filterAgrs.addAdditionalFilter("StageId", this.acceptedSatgeId, null, null, "Equals", false, false, false, "String");

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
