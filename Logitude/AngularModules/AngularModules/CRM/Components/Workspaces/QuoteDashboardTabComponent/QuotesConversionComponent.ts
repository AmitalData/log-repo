import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { FormatTool } from '../../../../Infrastructure/Tools';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    selector: 'quote-conversion',
    
    templateUrl: './QuotesConversionComponent.html',
})

export class QuotesConversionComponent {
    public ChartId: string;
    private chartArgs: QuoteDashboardArguments;
    private chartService: DashboardService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.chartService = new DashboardService();
        this.chartArgs = new QuoteDashboardArguments();
        this.chartArgs.ChartCode = "QCV";
        this.ChartId = "QuoteConversionId+" + this.CurrentSession.GetNewId("QuoteConversionDashboard");
        this.chartColrs = new ChartColors;
    }

    Update(comp: QuoteDashboardComponent) {
        this.chartArgs.OwnerId = comp.OwnerId;
        this.chartArgs.BusinessUnitId = comp.BusinessUnitId;
        this.chartArgs.FromDate = comp.FromDate;
        this.chartArgs.ToDate = comp.ToDate;
        this.LoadDashboardData();
    }

    public chartColrs: ChartColors;
    public NoQuotes: boolean = false;
    public CountriesData: ChartingDataClass[];
    public QuoteConversionDashboard: Array<ChartingDataClass>;
    private LoadDashboardData() {
        this.QuoteConversionDashboard = new Array<ChartingDataClass>();

        this.chartService.GetDashboardChartValues(this.chartArgs).subscribe((myResult: any) => {
            this.QuoteConversionDashboard = myResult;            
            this.FillQuoteConversionDashboardData();
        });
    }
     
    private FillQuoteConversionDashboardData() {
        if (this.QuoteConversionDashboard.length == 0) {
            this.NoQuotes = true;
        }

        else {
            this.DrawChart();
            this.NoQuotes = false;
        }        
    }

    public YAxis: any[] = [];
    public XAxis: string[] = []; 
    private DrawChart() {
        this.XAxis = [];
        this.YAxis = [];
        var j = 0;

        this.QuoteConversionDashboard.forEach(element => {
            if (!this.XAxis.includes(element.StringProperty) && element.StringProperty != null) {
                this.XAxis.push(element.StringProperty);
                this.YAxis[j] = { data1: null, data2: null, directionId: null, transportModeId: null, groupedId: null };
                j++;
            }
        });
        
        this.QuoteConversionDashboard.forEach(element => {
            for (var i = 0; i < this.XAxis.length; i++) {
                if (element.StringProperty == this.XAxis[i]) {
                    this.YAxis[i].directionId = element.DirectionId;
                    this.YAxis[i].transportModeId = element.TransportModeId;
                    this.YAxis[i].groupedId = element.GroupedId;
                    this.YAxis[i].data1 = element.Count_All;
                    this.YAxis[i].data2 = element.Count_Converted;                    
                }
            }
        });

        var Graphs = [];
        var index = 0;
        var DataProvider = [];
        var maximum = 0;        

        if (this.YAxis.length > 0) {
            maximum = this.YAxis[0].data1;
        }

        this.YAxis.forEach(element => {
            if (element.data1 > maximum) {
                maximum = element.data1;
            }

            DataProvider[index] = {
                "category": this.XAxis[index],
                "value1": element.data1,
                "value2": element.data2,
            };
            
            index++;
        });

        Graphs =
            [{
                "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                "fillAlphas": 0.2,
                "lineAlpha": 1,
                "fillColors": "#FFFFFF",
                "lineColor": "#487E9F",
                "lineThickness": 1,
                "type": "column",
                "valueField": "value1",
                "clustered": false,
                "showHandOnHover": true,
            },
            {
                "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                "fillAlphas": 1,
                "lineAlpha": 1,
                "fillColors": "#487E9F",
                "lineColor": "#487E9F",
                "lineThickness": 1,
                "type": "column",
                "valueField": "value2",
                "clustered": false,
                "showHandOnHover": true,
            }];

        try {
            if (this.XAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }

                makeAmBarChart(this.ChartId, Graphs, DataProvider, maximum, false);
            }
        }

        catch (e) {

        }
    }
    
    BarClicking() {
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
        var Key = e.target.columnIndex;
        var myQueryCode: string = "All Quotes";
        var myTableName: string = "Quote";
        var filterAgrs = new ApiQueryFilters();

        if (flag) {

            filterAgrs.addAdditionalFilter("QuoteConversionDateFilter", this.chartArgs.FromDate, this.chartArgs.ToDate, null, "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("SalesmanUserId", this.chartArgs.OwnerId, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("BusinessUnitId", this.chartArgs.BusinessUnitId, null, null, "Equals", false, false, false, "string");
            filterAgrs.addAdditionalFilter("TransportModeId", this.YAxis[e.index].transportModeId, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("DirectionId", this.YAxis[e.index].directionId, null, null, "Equals", false, false, false, "String");

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

export class ChartColors {
    public Color_AD: string = "#FF0F00";
    public Color_AE: string = "#FF6600";
    public Color_AI: string = "#FF9E01";
    public Color_AR: string = "#CD0D74";
    public Color_ID: string = "#F8FF01";
    public Color_IE: string = "#B0DE09";
    public Color_II: string = "#04D215";
    public Color_IR: string = "#0D8ECF";
    public Color_OD: string = "#0D52D1";
    public Color_OE: string = "#2A0CD0";
    public Color_OI: string = "#8A0CCF";
    public Color_OR: string = "#FCD202";
}
