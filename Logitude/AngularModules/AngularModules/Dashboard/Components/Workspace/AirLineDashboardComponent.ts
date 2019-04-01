import {Component, OnInit, ElementRef, ComponentFactoryResolver, ComponentRef, OnDestroy,ViewEncapsulation} from '@angular/core'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {DashBoardFilters} from '../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {DashboardDomainService} from '../../Services/DashboardDomainService';
import {DirectionTransportFilter} from '../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter';
import {DashBoardClass} from '../../../Infrastructure/DataContracts/Dashboard/DashBoardClass';
import {GroupByClass} from '../../../Infrastructure/DataContracts/Dashboard/GroupByClass';
import {FunctionsCRM} from '../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM';
import {List} from '../../../Infrastructure/DataContracts/Dashboard/List';
import {DailySpotlightClass} from '../../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {FormatTool, AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {ChartingDataClass} from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
declare var makeAMLineChart, BarClick, ResetItem, makeAmBarChart, makePieChart, PieClick, ResetItemPie;

@Component({
    selector: 'DashBoard',
    moduleId: module.id,
    templateUrl: './AirLineDashboardComponent.html',
    encapsulation: ViewEncapsulation.None,
})

export class AirLineDashboardComponent implements OnInit {

    private dashboarddomainservice: DashboardDomainService;
    public TenantPM: TenantPM;
    FilterList: DashBoardFilters[] = [];
    FilterListActivity: DashBoardFilters[] = [];
    SelectedItemActivityShow: any;
    SelectedItem: any;
    public NoShipmentsInActivityStatus: boolean = false;
    private CurrentTop10DebtorsChart: any;
    SelectedItemActivity: any;
    public ActivityStatusDashboardId: string = "ActivityStatusDashboardId_";
    public TopParticipantsDashboard: string = "TopParticipantsDashboard_";
    public BookingInProgressDashboardId: string = "BookingInProgressDashboardId_";
    public TopParticipantsDashboardIdExistance: boolean = false;
    public BookingInProgressDashboardLegendId: string;
    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public BarData: any;
    public barChartLabels: string[] = [];
    public barChartData: any[] = [];
    public SelectedCurrency: string = "1";
    public PieData: any;
    public pieChartLabels: string[] = [];
    public pieChartData: number[] = [];
    public PieChartLabels: string[] = [];
    public lineChartData: any[] = [{ data: [], label: '' }];
    public AmLineChartData: any = [];
    public ActivityStatusData: any = [];
    public MoneyInLabel: string = "";
    public NoActivityStatus: boolean = false;
    public NoBookingInProgress: boolean = false;
    public Participations_Today_Status: boolean = true;
    public Participations_Yesterday_Status: boolean = true;
    public Participations_LastWeek_Status: boolean = true;

    public NewParticipants_Today_Status: boolean = true;
    public NewParticipants_Yesterday_Status: boolean = true;
    public NewParticipants_LastWeek_Status: boolean = true;



    public FWB_Today_Status: boolean = true;
    public FWB_Yesterday_Status: boolean = true;
    public FWB_LastWeek_Status: boolean = true;


    public FHL_Today_Status: boolean = true;
    public FHL_Yesterday_Status: boolean = true;
    public FHL_LastWeek_Status: boolean = true;

    public FFR_Today_Status: boolean = true;
    public FFR_Yesterday_Status: boolean = true;
    public FFR_LastWeek_Status: boolean = true;



    public dailySpotLightClass: DailySpotlightClass;

    LoadParticipantsQuery(days: number) {
        this.dashboarddomainservice.GetTopParticipantsDashBoard(days).subscribe(result => {
            if (result.length == 0) {
                this.TopParticipantsDashboardIdExistance = false;
                try {
                    var elm = document.getElementById(this.TopParticipantsDashboard);
                }
                catch (er) { }
                elm.innerHTML = "";

            }
            else {
                this.TopParticipantsDashboardIdExistance = true;
                this.FillTopParticipants(result);
            }

        });
    }
    public NewTopParticipantsList: Array<any> = [];

    FillTopParticipants(List:any) {


        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        var StringArr: Array<string> = new Array<string>();
        var j = 0;
        List = List.filter(element => element.DataTypeCode == "FWB" || element.DataTypeCode == "FHL" || element.DataTypeCode == "FFR");
        List.forEach(element => {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], ParticipantId: [], DateTime: [], BusinessUnitId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });

        var Graphs = [];
        var index = 0;
        List.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    var k = 1;
                    if (element.DataTypeCode == "FFR")
                        k = 2;
                    else if (element.DataTypeCode == "FWB")
                        k = 0;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(3);

                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].BusinessUnitId[k] = element.BusinessUnitId;
                    NewCustomerYAxis[i].DateTime[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].ParticipantId[k] = element.ParticipantId;
                    if (!NewCustomerXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.StringProperty);

                    }
                }
            }
        });

        var barChartColors: any[] = [
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',                
                borderWidth: 0
            },

            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',

                borderWidth: 0,
            },

            {              
                backgroundColor1: '#21782E',
                backgroundColor2: '#90bb96',

                borderWidth: 0,
            },
        ]
        this.NewTopParticipantsList = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.NewTopParticipantsList[i] == null) {
                    this.NewTopParticipantsList[i] = { data: [], label: null, BindingElement: [], ParticipantId: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.NewTopParticipantsList[i].data.push(element.data[i]);
                this.NewTopParticipantsList[i].BindingElement.push(element.BindingElement[i]);
                this.NewTopParticipantsList[i].ParticipantId.push(element.ParticipantId[i]);
                this.NewTopParticipantsList[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                this.NewTopParticipantsList[i].DateTime.push(element.DateTime[i]);
                this.NewTopParticipantsList[i].label = element.label;
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [barChartColors[i].backgroundColor1 + "", barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2] };
            index++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (NewCustomerXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }
                makeAmBarChart(this.TopParticipantsDashboard, Graphs, DataProvider, maximum, null, null, 0);
            }

        }
        catch (e) {

        }



    }


    filterAgrs: ApiQueryFilters;
    DailySpotLightClick(Code) {

        var myQueryCode: string = "";
        var displayName: string = "";
        var myTableName: string = "";
        this.filterAgrs = new ApiQueryFilters();
        this.filterAgrs.SortBy = "SentDate";
        this.filterAgrs.SortDirection = "Descending";

        switch (Code) {
            case "PART_TD":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Today Participations";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "String");
                    break;
                }

            case "PART_YS":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Yesterday Participations";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "String");
                    break;
                }

            case "PART_LW":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Last Week Participations";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Participations Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "String");
                    break;
                }

            case "NEWP_TD":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Today New Participants";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "NEWP_YS":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Yesterday New Participants";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "NEWP_LW":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Last Week New Participants";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }

            case "FWB_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FWBs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");

                    break;
                }
            case "AR_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FWBs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");

                    break;
                }
            case "FWB_LW":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Last Week FWBs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }

            case "CS_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FHLs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");

                    break;
                }

            case "FHL_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FHLs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }

            case "FHL_LW":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Last Week FHLs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }

            case "FHL_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FHLs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                   this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");

                    break;
                }



            case "FWB_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FWBs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }


            case "FFR_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FFRs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FFR Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");

                    break;
                }

            case "FFR_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FFRs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FFR Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }

            case "FFR_LW":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Last Week FFRs";
                    ServiceLocator.SendTotangoUserActivity("Dashboard", "FFR Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
        }

        var listArgs = new ListComponentArgs();
        listArgs.ShowViews = false;
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "Dashboard"
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadData());
            });
    }
    LoadData() {
        this.LoadPieQueries();
        this.LoadSpotlightQueries();
        this.FilterSelectedChangeActivity(this.SelectedItemActivity);
        this.FilterSelectedChange(this.SelectedItem);

    }

    TopParticipantClicking() {
        if (BarClick() != null) {
            this.OnTopParticipantClick(BarClick());
            ResetItem();
        }
    }


    OnTopParticipantClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "";
        var myTableName: string = "AirlineStatistics";
        var typeName = "";
var displayTitle="";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
        if (flag) {
            switch (Key + "") {
                case "0": { displayTitle = "FWBs"; typeName = "FWB"; myQueryCode = "All Airline Statistics"; break; }
                case "1": { displayTitle = "FHLs";typeName = "FHL"; myQueryCode = "All Airline Statistics"; break; }
                case "2": { displayTitle = "FFRs";typeName = "FFR"; myQueryCode = "All Airline Statistics"; break; }
            }
        }     


        var days;
        if (this.timeRangeSelectedIndex == 0) days = -7;

        else if (this.timeRangeSelectedIndex== 1) days = -30;
        else if (this.timeRangeSelectedIndex == 2) days = -90;
        else if (this.timeRangeSelectedIndex == 3) days = -365;    
        filterAgrs.addAdditionalFilter("SourceTenant", this.NewTopParticipantsList[e.target.columnIndex].ParticipantId[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("TopParticipantsFilter", typeName, days, null, "Equals", true, false, false, "String");

        var listArgs = new ListComponentArgs();
    
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Dashboard";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadData());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });

    }


    LoadSpotlightQueries() {

        this.dashboarddomainservice.GetAirlineDashboardSpotlightCounts(this.TenantPM.Id).subscribe(myResult => {

            this.dailySpotLightClass = myResult;
            if (this.dailySpotLightClass.Participations_Today != 0)
                this.Participations_Today_Status = false;
            if (this.dailySpotLightClass.NewParticipants_Today != 0)
                this.NewParticipants_Today_Status = false;
            if (this.dailySpotLightClass.FWB_Today != 0)
                this.FWB_Today_Status = false;
            if (this.dailySpotLightClass.FHL_Today != 0)
                this.FHL_Today_Status = false;
            if (this.dailySpotLightClass.FFR_Today != 0)
                this.FFR_Today_Status = false;

            if (this.dailySpotLightClass.Participations_Yesterday != 0)
                this.Participations_Yesterday_Status = false;
            if (this.dailySpotLightClass.NewParticipants_Yesterday != 0)
                this.NewParticipants_Yesterday_Status = false;
            if (this.dailySpotLightClass.FWB_Yesterday != 0)
                this.FWB_Yesterday_Status = false;
            if (this.dailySpotLightClass.FHL_Yesterday != 0)
                this.FHL_Yesterday_Status = false;
            if (this.dailySpotLightClass.FFR_Yesterday != 0)
                this.FFR_Yesterday_Status = false;

            if (this.dailySpotLightClass.Participations_LastWeek != 0)
                this.Participations_LastWeek_Status = false;
            if (this.dailySpotLightClass.NewParticipants_LastWeek != 0)
                this.NewParticipants_LastWeek_Status = false;
            if (this.dailySpotLightClass.FWB_LastWeek != 0)
                this.FWB_LastWeek_Status = false;
            if (this.dailySpotLightClass.FHL_LastWeek != 0)
                this.FHL_LastWeek_Status = false;
            if (this.dailySpotLightClass.FFR_LastWeek != 0)
                this.FFR_LastWeek_Status = false;
        });





    }

    ActivityStatusClick() {
        if (BarClick() != null) {
            this.OnActivityStatusClick(BarClick());
            ResetItem();
        }
    }

    OnActivityStatusClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "All Logitude Transmission Logs";
        var myTableName: string = "LogitudeMessagesTransmissionLog";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
        var displayName = "Logitude Messages Transmission Logs";

        filterAgrs.SortBy = "SentDate";
        filterAgrs.SortDirection = "Descending";
        if (this.SelectedItemActivityShow != null)
            filterAgrs.addAdditionalFilter("MessageTypeCode", this.SelectedItemActivityShow.Index, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ActivityStatusChartFilter", this.barChartData[e.item.index].Month[e.target.columnIndex], this.barChartData[e.item.index].Year[e.target.columnIndex], null, "Equals", true, false, false, "String");
    
        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "Dashboard";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.fillScreen());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });

    }


    private flagEmpty: boolean = false;
    LoadChartData(days: number, showType: string) {
        this.dashboarddomainservice.GetActivityStatusByMessagesLogs(days, showType).subscribe(myResult => {
            this.ActivityStatusData = myResult.getAll();
            this.FillActivitiesStatus(this.ActivityStatusData);
        });


    }
    LoadPieQueries() {

        this.dashboarddomainservice.GetDashBoardBookings(this.TenantPM.Id).subscribe(myResult => {
            this.PieData = myResult;
            if (this.PieData.length > 0) {
                this.NoBookingInProgress = false;
                this.FillPie();
            }
            else
                this.NoBookingInProgress = true;
        });


    }
    public timeRangeSelectedIndexShow: number = 0;
    public timeRangeSelectedIndex: number = 0;
    public ActivitiesStatusDashboardId: string = "ActivitiesStatusDashboardId_";

    FillActivitiesStatus(data) {
        var i = 0;
        var index = 0;
        var Graphs = Graphs = [{
            "balloonText": "[[value]]",
            "fillAlphas": 1,
            "id": "AmGraph-1" + i,
            "title": "Invoices",
            "type": "column",
            "valueField": "col1",
            "fillColors": ["#487E9F", "#c8d8e2"],
            "gradientOrientation": "horizontal",
            "borderAlpha": 0,
            "lineAlpha": 0,
        }];
        var max = 0;
     
        var DataProvider = [];
        this.barChartData = [];
        var barChartLabels = [];
        data.forEach(element => {
            if (this.barChartData[i] == null)
                this.barChartData[i] = { data: [], label: null, Month: [], Year: [] };
            this.barChartData[i].Month.push(element.Month);
            this.barChartData[i].Year.push(element.Year);
            this.barChartData[0].label = element.LabelProperty + "";
            this.barChartData[0].data[i] = element.IntegerProperty + "";
            this.barChartLabels[i] = element.LabelProperty + "";
            DataProvider[i] = { "category": this.barChartLabels[i], "col1": this.barChartData[0].data[i] };
            if (element.IntegerProperty > max)
                max = element.IntegerProperty;
            i++;
        });
        var barChartColors: any[] = [
            {
                backgroundColor: "rgb(73,165,191)" /*Safari 5.1-6*/
                ,
                borderColor: "rgba(147,206,222,1)",
                borderWidth: 2
            }
        ]
        var flagEmpty = true;
        if (this.barChartData.length > 0)
            this.barChartData[0].data.forEach(p => {
                if (p != "0")
                    flagEmpty = false;
            });
        if (!flagEmpty) {
            max += 1;
            while (max % 5 != 0) {
                max += 1;
            }
            makeAmBarChart(this.ActivitiesStatusDashboardId, Graphs, DataProvider, max, null, null, 0);
            this.NoActivityStatus = false;
        }
        else {
            try {
                var elm = document.getElementById(this.ActivitiesStatusDashboardId);
                elm.innerHTML = "";
            }
            catch (exc) { }
            this.NoActivityStatus = true;
        }

    }


    GetCurrentDirectionTransmodeFilterItem() {
        var transmodeId: string = "";
        var directionId: string = "";
        var x = 10;
        switch (x) {
            case 0:
                {
                    directionId = "";
                    break;
                }

            case 1:
                {
                    directionId = "E";
                    break;
                }

            case 2:
                {
                    directionId = "I";
                    break;
                }

            case 3:
                {
                    directionId = "R";
                    break;
                }

            case 4:
                {
                    directionId = "D";
                    break;
                }
            case 5:
                {
                    directionId = "C";
                    break;
                }
        }

        switch (x) {
            case 0:
                {
                    transmodeId = "";
                    break;
                }
            case 1:
                {
                    transmodeId = "A";
                    break;
                }
            case 2:
                {
                    transmodeId = "O";
                    break;
                }

            case 3:
                {
                    transmodeId = "I";
                    break;
                }
        }

        var filterItem: DirectionTransportFilter = new DirectionTransportFilter("", directionId, transmodeId);
        return filterItem;


    }


    BookingClicking() {
        if (PieClick() != null) {
            this.OnBookingClick(PieClick());
            ResetItemPie();
        }

    }

    OnBookingClick(e) {

        var item = this.PieData[e.index];

        var myQueryCode: string = "All Airline Statistics";
        var myTableName: string = "AirlineStatistics";
        var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
      
        filterAgrs.addAdditionalFilter("BookingsInProgressFilter", item.Code, null, null, "Equals", true, true, false, "String");
       
        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Bookings";
        listArgs.BackButtonTitle = "Ticket";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadData());
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });



    }

    FillPie() {
        var fullData = [];
        this.pieChartLabels = [];
        this.pieChartData = [];        
        this.PieData.forEach(element => {
            var IntegerProperty = AppTool.Round(element.IntegerProperty, 3);
            fullData.push({ label: element.StringProperty, data: IntegerProperty })
            this.pieChartLabels.push(element.StringProperty);
            this.pieChartData.push(element.IntegerProperty);

        });
        if (this.CurrentTop10DebtorsChart != null) {
            this.CurrentTop10DebtorsChart.clear();
            this.CurrentTop10DebtorsChart = null;
        }
        this.CurrentTop10DebtorsChart = makePieChart(this.BookingInProgressDashboardId, fullData, false, true, this.BookingInProgressDashboardLegendId);

    }
    FillLine(data: List<GroupByClass>) {
        this.AmLineChartData = [];
        var index = 0;
        this.lineChartData = [{

            scales: {
                xAxes: [{
                    gridThickness: 0,
                }]
            },

            xAxes: {
                gridThickness: 0,
            },
            offsetGridLines: false
            ,
            scaleShowVerticalLines: false,

            data: [], label: 'Total', tension: 0, scaleShowHorizontalLines: false, scaleStepWidth: 0
        }];
        this.PieChartLabels = [];
        data.getAll().forEach(element => {
            this.lineChartData[0].data[index] = element.YField + "";
            this.PieChartLabels.push(element.XField);
            index++;

            this.AmLineChartData.push({
                date: element.XField,
                visits: element.YField + ""
            });




        });






    }




    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public componentfactoryResolver: ComponentFactoryResolver) {
        this.TenantPM = InfraSettings.TenantPM;
        this.dailySpotLightClass = new DailySpotlightClass();
        this.dashboarddomainservice = new DashboardDomainService();
        this.ActivityStatusDashboardId = this.ActivityStatusDashboardId + this.CurrentSession.GetChartId();
        this.TopParticipantsDashboard = this.TopParticipantsDashboard + this.CurrentSession.GetChartId();
        this.BookingInProgressDashboardId = this.BookingInProgressDashboardId + this.CurrentSession.GetChartId();
        this.BookingInProgressDashboardLegendId = "BookingInProgressDashboardLegendId_" + this.CurrentSession.GetNewId("BookingInProgressDashboardLegendId");
        this.ActivitiesStatusDashboardId = this.ActivitiesStatusDashboardId + this.CurrentSession.GetChartId();

    }


    fillScreen() {
   
        this.LoadData();
    }



    FillFilters() {
        this.FilterList = [];
        this.FilterListActivity = [];
        var list = LastFilter.myList();
        this.FilterList.push(new DashBoardFilters(list[0].lastTitle, 0 + ""));
        this.FilterList.push(new DashBoardFilters(list[1].lastTitle, 1 + ""));
        this.FilterList.push(new DashBoardFilters(list[2].lastTitle, 2 + ""));
        this.FilterList.push(new DashBoardFilters(list[3].lastTitle, 3 + ""));

        this.SelectedItem = this.FilterList[0];
        this.SelectedItemActivity = this.FilterList[0];
        this.FilterSelectedChangeActivity(this.SelectedItemActivity);
        this.FilterSelectedChange(this.SelectedItem);
        /////
        this.FilterListActivity.push(new DashBoardFilters("FWB", "FWB"));
        this.FilterListActivity.push(new DashBoardFilters("FHL", "FHL"));
        this.FilterListActivity.push(new DashBoardFilters("FSR", "FSR"));
        this.FilterListActivity.push(new DashBoardFilters("FFR", "FFR"));
        this.FilterListActivity.push(new DashBoardFilters("FVR", "FVR"));
        this.SelectedItemActivityShow = this.FilterListActivity[0];
        this.MoneyInLabel = SessionLocator.TenantPM.AccountingCurrencyCode;        
    }
    FilterSelectedChangeShow(item) {
        this.SelectedItemActivityShow = item;
        var days;
        if (this.SelectedItemActivity.Index == 0) days = -7;

        else if (this.SelectedItemActivity.Index == 1) days = -30;
        else if (this.SelectedItemActivity.Index == 2) days = -90;
        else if (this.SelectedItemActivity.Index == 3) days = -365;

        this.timeRangeSelectedIndexShow = item.Index;
        if (this.SelectedItemActivityShow != null) {
            this.LoadChartData(days, this.SelectedItemActivityShow.Index);
        }

        else {
            this.LoadChartData(days, "FWB");
        }
    }

    FilterSelectedChangeActivity(item) {

        var days;
        this.SelectedItemActivity = item;
        if (item.Index == 0) days = -7;

        else if (item.Index == 1) days = -30;
        else if (item.Index == 2) days = -90;
        else if (item.Index == 3) days = -365;

        if (this.SelectedItemActivityShow != null) {
            this.LoadChartData(days, this.SelectedItemActivityShow.Index);
        }

        else {
            this.LoadChartData(days, "FWB");
        }

    }

    FilterSelectedChange(item) {
        var days;
        this.SelectedItem = item;
        if (item.Index == 0) days = -7;

        else if (item.Index == 1) days = -30;
        else if (item.Index == 2) days = -90;
        else if (item.Index == 3) days = -365;
        this.timeRangeSelectedIndex = item.Index;

        this.LoadParticipantsQuery(days);

    }


  


    ngOnInit() {
        this.FillFilters();
        this.fillScreen();

    }

}
