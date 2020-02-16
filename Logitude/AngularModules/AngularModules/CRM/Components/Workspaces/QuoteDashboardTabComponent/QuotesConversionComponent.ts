import { Component, OnInit } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { FormatTool } from '../../../../Infrastructure/Tools';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    selector: 'quote-conversion',
    moduleId: module.id,
    templateUrl: './QuotesConversionComponent.html',
})

export class QuotesConversionComponent implements OnInit {
    private Wizard: QuoteDashboardComponent;   
    private CurrentSession = SessionLocator.SelectedSession;
    private dashboardArgs: QuoteDashboardArguments;
    private dashboardService: DashboardService;
    public QuoteConversionDashboard: Array<ChartingDataClass>;
    public QuoteConversionId: string;
    public legenddiv: string = "Legends_ID_";
    public chartColrs: ChartColors;
    public NoQuotes: boolean = false;
    constructor(private _entityResourceService: EntityResourceService) {        
        this.QuoteConversionId = "QuoteConversionId+" + this.CurrentSession.GetNewId("QuoteConversionDashboard");
        this.chartColrs = new ChartColors;
    }

    ngOnInit() {
        this.dashboardArgs = new QuoteDashboardArguments();
        this.dashboardService = new DashboardService();

        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            this.FillDashboardArgs();
            this.LoadDashboardData();
        });
    }

    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
    }
    RefreshTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        this.FillDashboardArgs();
        this.LoadDashboardData();
    }

    private FillDashboardArgs() {
        this.dashboardArgs.OwnerId = this.Wizard.OwnerId;
        this.dashboardArgs.BusinessUnitId = this.Wizard.BusinessUnitId;
        this.dashboardArgs.FromDate = this.Wizard.FromDate;
        this.dashboardArgs.ToDate = this.Wizard.ToDate;
        this.dashboardArgs.ChartCode = "QCV";
    }

    public CountriesData: ChartingDataClass[];
    private LoadDashboardData() {
        this.QuoteConversionDashboard = new Array<ChartingDataClass>();

        this.dashboardService.GetDashboardChartValues(this.dashboardArgs).subscribe((myResult: any) => {
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
                "color": this.chartColrs["Color_" + element.groupedId],
                "opacity": element.data2 == 0 ? 1 : 0.2,
            };
            
            index++;
        });

        Graphs =
            [{
                "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                "alphaField": "opacity",
                "lineAlpha": 1,
                "fillColorsField": "color",
                "lineColorField": "color",
                "type": "column",
                "valueField": "value1",
                "clustered": false,
                "showHandOnHover": true,
            },
            {
                "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                "fillAlphas": 1,
                "lineAlpha": 1,
                "fillColorsField": "color",
                "lineColorField": "color",
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
                
                makeAmBarChart(this.QuoteConversionId, Graphs, DataProvider, maximum, false);
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

            filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper.GetDateString(this.Wizard.FromDate), ServiceHelper.GetDateString(this.Wizard.ToDate), null, "Equals", true, false, false, "String");
            //filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("SalesmanUserId", this.Wizard.OwnerId, null, null, "Equals", false, false, false, "String");
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
