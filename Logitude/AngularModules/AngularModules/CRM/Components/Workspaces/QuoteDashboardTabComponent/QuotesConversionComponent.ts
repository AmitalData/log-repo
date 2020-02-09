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
    public ChartID: string = null;
    public QuoteConversionDashboard: Array<ChartingDataClass>;
    public QuoteConversionId: string = "QuoteConversionId_";
    public legenddiv: string = "Legends_ID_";
    constructor(private _entityResourceService: EntityResourceService) {
        if (this.CurrentSession == null) {
            this.ChartID = "ChartID_-1_-1";
            this.legenddiv = "Legends_ID_-1_-1";
        }

        else {
            this.ChartID = "ChartID_" + this.CurrentSession.GetChartId();
            this.legenddiv = this.legenddiv + this.CurrentSession.GetChartId();
        }

        this.QuoteConversionId = this.QuoteConversionId + this.CurrentSession.GetChartId();
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

    public QuoteConversionYAxis: any[] = [];
    public QuoteConversionXAxis: string[] = [];
    FillQuoteConversionDashboardData() {
        var index = 0;
        this.QuoteConversionXAxis = [];        
        var StringArr: Array<string> = new Array<string>();
        var j = 0;

        this.QuoteConversionDashboard.forEach(element => {
            if (!StringArr.includes(element.SalesmanUserName) && element.SalesmanUserName != null) {
                StringArr.push(element.SalesmanUserName);
                this.QuoteConversionYAxis[j] = { data1: [], data2: [], ProductTypes: [], SalesmanId: null, ProductTypesFilters: [] };
                this.QuoteConversionYAxis[j].data = [];
                j++;
            }
        });

        StringArr.sort((a, b) => { return (a === b) ? 0 : (a < b) ? -1 : 1 });
        var Graphs = [];
        var index = 0;
        this.QuoteConversionDashboard.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.SalesmanUserName == StringArr[i]) {
                    this.QuoteConversionYAxis[i].data1.push(element.Count_All);
                    this.QuoteConversionYAxis[i].data2.push(element.Count_Convert);
                    this.QuoteConversionYAxis[i].ProductTypes.push(element.TransportModeDirection);
                    this.QuoteConversionYAxis[i].ProductTypesFilters.push(element.TransportModeDirection_Display);
                    this.QuoteConversionYAxis[i].SalesmanId = element.SalesmanUserId;
                    if (!this.QuoteConversionXAxis.includes(element.SalesmanUserName) && element.SalesmanUserName != null) {
                        if (this.QuoteConversionXAxis[i] == null)
                            this.QuoteConversionXAxis[i] = (element.SalesmanUserName);

                    }
                }
            }
        });
        
        var DataProvider = [];
        var objectArray_1 = [];
        var objectArray_2 = [];
        var maximum = 0;

        objectArray_1.length = 20;
        objectArray_2.length = 20;

        if (this.QuoteConversionYAxis.length > 0) {
            maximum = this.QuoteConversionYAxis[0].data1[0];
        }

        this.QuoteConversionYAxis.forEach(element => {
            objectArray_1 = [];
            objectArray_2 = [];

            for (var i = 0; i < element.ProductTypes.length; i++) {                

                if (element.data1[i] > maximum) {
                    maximum = element.data1[i];
                }
                
                if (index == 0) {
                    Graphs[i] =
                        {
                            "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                            "fillAlphas": 0.3,
                            "lineAlpha": 1,
                            "id": "AmGraph-1" + i,
                            "title": element.ProductTypes[i] + "",
                            "type": "column",
                            "valueField": "acol" + (i + 1),
                            "lineColor": this.barChartColors[i].backgroundColor1,
                            "borderAlpha": 0,
                            "showHandOnHover": true,
                            //"clustered": false
                        };
                    //    {
                    //        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                    //        "fillAlphas": 1,
                    //        "lineAlpha": 1,
                    //        "id": "AmGraph-1_1" + i,
                    //        "title": element.ProductTypes[i] + "",
                    //        "type": "column",
                    //        "valueField": "ccol" + (i + 1),
                    //        "lineColor": this.barChartColors[i].backgroundColor1,
                    //        "borderAlpha": 0,
                    //        "showHandOnHover": true,
                    //        //"clustered": false
                    //    }
                    //];
                }

                objectArray_1[i] = (element.data1[i]);
                objectArray_2[i] = (element.data2[i]);
            }

            DataProvider[index] = {
                "category": this.QuoteConversionXAxis[index],
                "acol1": objectArray_1[0],
                "acol2": objectArray_1[1],
                "acol3": objectArray_1[2],
                "acol4": objectArray_1[3],
                "acol5": objectArray_1[4],

                "ccol1": objectArray_2[0],
                "ccol2": objectArray_2[1],
                "ccol3": objectArray_2[2],
                "ccol4": objectArray_2[3],
                "ccol5": objectArray_2[4]
            };
            index++;
        });
        
        try {
            if (this.QuoteConversionXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }

                //name, graphs, dataprovider, max, legendFlag, LegendDiv, minimum, stacked, IsRtl
                makeAmBarChart(this.QuoteConversionId, Graphs, DataProvider, maximum, true, this.legenddiv, null, false, false);
            }
        }

        catch (e) {

        }
    }

    isResizing: boolean = false;
    ChartLeft: number = 0;
    lastDownX: number = 0;
    lastDownY: number = 0;
    OnMyMouseDown($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.ChartID);
        var rec = grid.getBoundingClientRect();
        this.ChartLeft = rec.left;
        this.lastDownY = ($event.clientY - rec.bottom);
        this.lastDownX = ($event.clientX - this.ChartLeft);
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
            var product: string = this.QuoteConversionYAxis[e.index].ProductTypesFilters[e.target.columnIndex];
            var array: string[] = product.split(",");

            filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper.GetDateString(this.Wizard.FromDate), ServiceHelper.GetDateString(this.Wizard.ToDate), null, "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("SalesmanUserId", this.QuoteConversionYAxis[e.index].SalesmanId, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("TransportModeId", array[0], null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("DirectionId", array[1], null, null, "Equals", false, false, false, "String");

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

    barChartColors: any[] = [
        {
            backgroundColor1: '#487E9F',
        },

        {
            backgroundColor1: '#DA7B38',
        },

        {
            backgroundColor1: '#21782E',
        },

        {
            backgroundColor1: '#FF00B2',
        },

        {
            backgroundColor1: '#FF0000',
        },

        {
            backgroundColor1: '#FF00B2',
        },

        {
            backgroundColor1: '#4D3AAF',
        },

        {
            backgroundColor1: '#956027',
        },

        {
            backgroundColor1: '#540000',
        },

        {
            backgroundColor1: '#FF6270',
        },

        {
            backgroundColor1: '#41D900',
        },
    ]
}
