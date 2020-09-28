import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {CRMDomainService} from '../../../../../CRM/Services/CRMDomainService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../../Infrastructure/Tools';
declare var makeAMLineChartMultiple: any;
@Component({
    selector: 'TicketOverviewTabComponent',
    
    templateUrl: './TicketOverviewTabComponent.html',
})

export class TicketOverviewTabComponent {
    public EntityPM: TicketPM = null;
    public ObjectTableName = "Ticket";
    public DataContext: this;
    public PerformanceChartId: string;
    public LegendDiv: string;
    public PerformanceChartIdExistance: Boolean = false;
    public _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.LoadAllData();
    }

    private CRMDomainService: CRMDomainService;
    private InitializeServices() {
        this.CRMDomainService = new CRMDomainService();
        this.PerformanceChartId = "PerformanceChartId_" + this.CurrentSession.GetNewId("PerformanceChartId");
        this.LegendDiv = "LegendDiv_" + this.CurrentSession.GetNewId("LegendDiv");        
    }

    private LoadAllData() {
        this.LoadStatisticsDataCounts();
        this.LoadOpenedPerformanceData();
        this.LoadLogsData();
    }

    // Load Data 
    public ResponseSLAViolated;
    public ResolveSLAViolated;
    public ResponseSLAViolatedTotalMinutes: number;
    public ResolveSLAViolatedTotalMinutes: number;
    public SummaryOpenPeriodMinutes: number;

    private LoadLogsData() {
        this.CRMDomainService.GetBusinessHours(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (response != null && !response.HasError) {
                var result = response.Result;
                this.ResolveSLAViolated = result.ResolveSLAViolated;
                this.ResponseSLAViolated = result.ResponseSLAViolated;
                this.ResponseSLAViolatedTotalMinutes = result.ResponseSLAViolatedTotalMinutes;
                this.ResolveSLAViolatedTotalMinutes = result.ResolveSLAViolatedTotalMinutes;
                this.SummaryOpenPeriodMinutes = result.OpenPeriodMinutes;
                this.RefreshLogData();
            }
        });
    }

    private LoadStatisticsDataCounts() {
        this.CRMDomainService.GetTicketOverViewStatisticsSummary(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (response != null && !response.HasError) {
                var result = response.Result;
                this.NumberOfExCorrespondences = result.Tickets_ExternalLines;
                this.NumberOfInCorrespondences = result.Tickets_InternalLines;
                this.NumberOfAllActivities = result.Tickets_Activities;
                this.NumberOfEscalations = result.Tickets_Escalations;
            }
        });
    }

    private LoadOpenedPerformanceData() {

        this.CRMDomainService.GetTicketOverviewPerformance(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (response != null && !response.HasError) {
                var result = response.Result;
                this.fillPerformanceChart(result, this);
            }
            else
                this.PerformanceChartIdExistance = false;
        });        
    }

    fillPerformanceChart(data,viewModel) {
        var lineData = this.FillLine(data);        
        var graphs = [{
        id: "g3",
            "useNegativeColorIfDown": false,
                "bullet": "round",
                    "balloonFunction": function (item, graph) {
                        return viewModel.MathValues(item.dataContext.data1);
                    },
        "bulletBorderAlpha": 1,
            "bulletBorderColor": "#FFFFFF",
                "hideBulletsCount": 50,
                "lineThickness": 2,
                "dashLength":3,
                        "lineColor": "#3a5cba",
                            "negativeLineColor": "#3a5cba",
                            "valueField": "data1",
                            "title": "SLA"

        },

            {
                id: "g4",
                "useNegativeColorIfDown": false,
                "bullet": "square",
                "balloonFunction": function (item, graph) {
                    return viewModel.MathValues(item.dataContext.data2);
                },
                "bulletBorderAlpha": 1,
                "bulletBorderColor": "#FFFFFF",
                "hideBulletsCount": 50,
                "lineThickness": 2,
                "lineColor": "#FF0000",
                "negativeLineColor": "#FF0000",
                "valueField": "data2",
                "title":"Actual"


            }

        ];

        if (this.PerformanceChartIdExistance)
        makeAMLineChartMultiple(this.PerformanceChartId, lineData, null, graphs,true,this.LegendDiv, "Hours");

    }

    MathValues(values) {

        if (values) {
            if (values % 2 == 0)
                return values + " Hours 0 Min";
            else {
                var stringValue: string = values.toString();
                var arrValue: string[] = stringValue.split('.');
                var min = parseFloat("0."+arrValue[1]) * 60;
                var mins: number = AppTool.Round(min, 0);
                return arrValue[0] + " Hours " + AppTool.Round(min, 1)  + " Min";
            }
        }       
    }
    FillLine(data) {

        data = data.filter(d => d.DataTypeCode == "SLA" || d.DataTypeCode == "Actual");
        if (data)
            if (data.Length != 0)
                this.PerformanceChartIdExistance = true;
        var AmLineChartTest: any[] = [{ "Category": '', "data1": '', "data2": '' }, { "Category": '', "data1": '', "data2": '' }];
        if (this.PerformanceChartIdExistance) {
            var index = 0;
            data.forEach(element => {
                if (element.DataTypeCode == "SLA") {
                    if (index == 0)
                        AmLineChartTest[0].Category = element.LabelProperty;
                    if (index % 2 == 0)
                        AmLineChartTest[0].data1 = !AppTool.IsNullOrEmpty(AmLineChartTest[0].data1) ? (parseInt(AmLineChartTest[0].data1) + parseInt(element.IntegerProperty + "") / 60) + "" : AmLineChartTest[0].data1 = parseInt(element.IntegerProperty + "") / 60 + ""
                    else
                        AmLineChartTest[1].data1 = !AppTool.IsNullOrEmpty(AmLineChartTest[1].data1) ? (parseInt(AmLineChartTest[1].data1) + parseInt(element.IntegerProperty + "") / 60) + "" : AmLineChartTest[1].data1 = parseInt(element.IntegerProperty + "") / 60 + ""

                }
                else {
                    if (index == 1)
                        AmLineChartTest[1].Category = element.LabelProperty;
                    if (index % 2 == 0)
                        AmLineChartTest[0].data2 = !AppTool.IsNullOrEmpty(AmLineChartTest[0].data2) ? (parseFloat(AmLineChartTest[0].data2) + element.DoubleProperty) + "" : AmLineChartTest[0].data2 = element.DoubleProperty + ""
                    else
                        AmLineChartTest[1].data2 = !AppTool.IsNullOrEmpty(AmLineChartTest[1].data2) ? (parseFloat(AmLineChartTest[1].data2) + element.DoubleProperty) + "" : AmLineChartTest[1].data2 = element.DoubleProperty + ""
                    index++;

                }
            });
        }
        return AmLineChartTest;
    }

    // Props
    get CreateDate() { return this.EntityPM.CreateDate; }
    set CreateDate(value: Date) {
        if (this.EntityPM.CreateDate != value) {
            this.EntityPM.CreateDate = value;
        }
    }

    get FirstResponseTime() { return this.EntityPM.FirstResponseTime; }
    set FirstResponseTime(value: Date) {
        if (this.EntityPM.FirstResponseTime != value) {
            this.EntityPM.FirstResponseTime = value;
        }
    }

    get FullResolvedTime() { return this.EntityPM.FullResolvedTime; }
    set FullResolvedTime(value: Date) {
        if (this.EntityPM.FullResolvedTime != value) {
            this.EntityPM.FullResolvedTime = value;
        }
    }

    // Performance



    // Logs
    public ResponseSLAViolatedString;
    public ResolveSLAViolatedString;
    public OpenPeriodMinutes;
    public TotalOpenPeriodVisibility;
    public ResponseSLAViolatedVisibility;
    public ResolveSLAViolatedVisibility;
    private RefreshLogData() {
        this.GetResponseSLAViolatedString();
        this.GetResolveSLAViolatedString();
        this.GetOpenPeriodMinutes();
        this.GetTotalOpenPeriodVisibility();
        this.GetResponseSLAViolatedVisibility();
        this.GetResolveSLAViolatedVisibility();
    }
    GetResponseSLAViolatedString() {
        var result = "";
        var val = this.ResponseSLAViolatedTotalMinutes;
        var h = val / 60 | 0,
            m = val % 60 | 0;
        var result = h + ":" + AppTool.PadLeft(""+m, 2, '0');
        this.ResponseSLAViolatedString= result;
    }
    GetResolveSLAViolatedString() {
        var result = "";
        var val = this.ResolveSLAViolatedTotalMinutes;
        var h = val / 60 | 0,
            m = val % 60 | 0;
        var result = h + ":" + AppTool.PadLeft("" + m, 2, '0');
        this.ResolveSLAViolatedString= result;
    }
    GetOpenPeriodMinutes() {
        var val = this.SummaryOpenPeriodMinutes;
        var h = val / 60 | 0,
            m = val % 60 | 0;
        var result = h + ":" + AppTool.PadLeft("" + m, 2, '0');
        this.OpenPeriodMinutes = result;
    }
    GetTotalOpenPeriodVisibility() {
        var myResult = false;
        if (this.SummaryOpenPeriodMinutes != 0) {
            myResult = true;
        }
        this.TotalOpenPeriodVisibility = myResult;
    }
    GetResponseSLAViolatedVisibility() {
        var myResult = false;
        if (this.ResponseSLAViolatedTotalMinutes != 0) {
            myResult = true;
        }
        this.ResponseSLAViolatedVisibility = myResult;
    }
    GetResolveSLAViolatedVisibility() {
        var myResult = false;
        if (this.ResolveSLAViolatedTotalMinutes != 0) {
            myResult = true;
        }
        this.ResolveSLAViolatedVisibility = myResult;
    }

    // Statistics
    public NumberOfExCorrespondences: number = 0;
    public NumberOfInCorrespondences: number = 0;
    public NumberOfAllActivities: number = 0;
    public NumberOfEscalations: number = 0;

    ViewAllData(arg) {

        switch (arg) {
            case "Escalations":
                {
                    var filterAgrs = new ApiQueryFilters();
                    filterAgrs.addAdditionalFilter("TicketId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string", false, false);
                    var listArgs = new ListComponentArgs();
                    listArgs.Filters = filterAgrs;
                    listArgs.QueryCode = "All Ticket Escalations";
                    listArgs.ObjectTableName = "TicketEscalation";
                    listArgs.BackButtonTitle = "Tickets";
                    this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                            });
                    });
                    break;
                }
            case "Activites":
                {
                    var filterAgrs = new ApiQueryFilters();
                    filterAgrs.addAdditionalFilter("TicketId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string", false, false);
                    var listArgs = new ListComponentArgs();
                    listArgs.Filters = filterAgrs;
                    listArgs.QueryCode = "All Activities";
                    listArgs.ObjectTableName = "Activity";
                    listArgs.BackButtonTitle = "Tickets";
                    this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                            });
                    });

                    break;
                }
        }
    }

    // Refresh Bbutton 
    RefreshButtonClicked() {
        this.LoadAllData();
    }
}
