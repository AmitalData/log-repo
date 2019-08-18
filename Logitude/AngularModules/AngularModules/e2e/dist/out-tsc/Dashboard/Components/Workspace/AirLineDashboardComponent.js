"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var DashboardFilters_1 = require("../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var DashboardDomainService_1 = require("../../Services/DashboardDomainService");
var DirectionTransportFilter_1 = require("../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter");
var DailySpotlightClass_1 = require("../../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass");
var Args_1 = require("../../../Infrastructure/Args");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var AirLineDashboardComponent = /** @class */ (function () {
    function AirLineDashboardComponent(componentfactoryResolver) {
        this.componentfactoryResolver = componentfactoryResolver;
        this.FilterList = [];
        this.FilterListActivity = [];
        this.NoShipmentsInActivityStatus = false;
        this.ActivityStatusDashboardId = "ActivityStatusDashboardId_";
        this.TopParticipantsDashboard = "TopParticipantsDashboard_";
        this.BookingInProgressDashboardId = "BookingInProgressDashboardId_";
        this.TopParticipantsDashboardIdExistance = false;
        this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        this.barChartLabels = [];
        this.barChartData = [];
        this.SelectedCurrency = "1";
        this.pieChartLabels = [];
        this.pieChartData = [];
        this.PieChartLabels = [];
        this.lineChartData = [{ data: [], label: '' }];
        this.AmLineChartData = [];
        this.ActivityStatusData = [];
        this.MoneyInLabel = "";
        this.NoActivityStatus = false;
        this.NoBookingInProgress = false;
        this.Participations_Today_Status = true;
        this.Participations_Yesterday_Status = true;
        this.Participations_LastWeek_Status = true;
        this.NewParticipants_Today_Status = true;
        this.NewParticipants_Yesterday_Status = true;
        this.NewParticipants_LastWeek_Status = true;
        this.FWB_Today_Status = true;
        this.FWB_Yesterday_Status = true;
        this.FWB_LastWeek_Status = true;
        this.FHL_Today_Status = true;
        this.FHL_Yesterday_Status = true;
        this.FHL_LastWeek_Status = true;
        this.FFR_Today_Status = true;
        this.FFR_Yesterday_Status = true;
        this.FFR_LastWeek_Status = true;
        this.NewTopParticipantsList = [];
        this.flagEmpty = false;
        this.timeRangeSelectedIndexShow = 0;
        this.timeRangeSelectedIndex = 0;
        this.ActivitiesStatusDashboardId = "ActivitiesStatusDashboardId_";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.dailySpotLightClass = new DailySpotlightClass_1.DailySpotlightClass();
        this.dashboarddomainservice = new DashboardDomainService_1.DashboardDomainService();
        this.ActivityStatusDashboardId = this.ActivityStatusDashboardId + this.CurrentSession.GetChartId();
        this.TopParticipantsDashboard = this.TopParticipantsDashboard + this.CurrentSession.GetChartId();
        this.BookingInProgressDashboardId = this.BookingInProgressDashboardId + this.CurrentSession.GetChartId();
        this.BookingInProgressDashboardLegendId = "BookingInProgressDashboardLegendId_" + this.CurrentSession.GetNewId("BookingInProgressDashboardLegendId");
        this.ActivitiesStatusDashboardId = this.ActivitiesStatusDashboardId + this.CurrentSession.GetChartId();
    }
    AirLineDashboardComponent.prototype.LoadParticipantsQuery = function (days) {
        var _this = this;
        this.dashboarddomainservice.GetTopParticipantsDashBoard(days).subscribe(function (result) {
            if (result.length == 0) {
                _this.TopParticipantsDashboardIdExistance = false;
                try {
                    var elm = document.getElementById(_this.TopParticipantsDashboard);
                }
                catch (er) { }
                elm.innerHTML = "";
            }
            else {
                _this.TopParticipantsDashboardIdExistance = true;
                _this.FillTopParticipants(result);
            }
        });
    };
    AirLineDashboardComponent.prototype.FillTopParticipants = function (List) {
        var _this = this;
        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        var StringArr = new Array();
        var j = 0;
        List = List.filter(function (element) { return element.DataTypeCode == "FWB" || element.DataTypeCode == "FHL" || element.DataTypeCode == "FFR"; });
        List.forEach(function (element) {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], ParticipantId: [], DateTime: [], BusinessUnitId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });
        var Graphs = [];
        var index = 0;
        List.forEach(function (element) {
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
        var barChartColors = [
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
        ];
        this.NewTopParticipantsList = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.NewTopParticipantsList[i] == null) {
                    _this.NewTopParticipantsList[i] = { data: [], label: null, BindingElement: [], ParticipantId: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.NewTopParticipantsList[i].data.push(element.data[i]);
                _this.NewTopParticipantsList[i].BindingElement.push(element.BindingElement[i]);
                _this.NewTopParticipantsList[i].ParticipantId.push(element.ParticipantId[i]);
                _this.NewTopParticipantsList[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                _this.NewTopParticipantsList[i].DateTime.push(element.DateTime[i]);
                _this.NewTopParticipantsList[i].label = element.label;
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
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
                    };
                }
                objectArray[i] = (element.data[i]);
            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2] };
            index++;
        });
        var InProgressBookingDashboardFilterd = new Array();
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
    };
    AirLineDashboardComponent.prototype.DailySpotLightClick = function (Code) {
        var _this = this;
        var myQueryCode = "";
        var displayName = "";
        var myTableName = "";
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.filterAgrs.SortBy = "SentDate";
        this.filterAgrs.SortDirection = "Descending";
        switch (Code) {
            case "PART_TD":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Today Participations";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "String");
                    break;
                }
            case "PART_YS":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Yesterday Participations";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "String");
                    break;
                }
            case "PART_LW":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Last Week Participations";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Participations Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "String");
                    break;
                }
            case "NEWP_TD":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Today New Participants";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "NEWP_YS":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Yesterday New Participants";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "NEWP_LW":
                {
                    myTableName = "Participant";
                    myQueryCode = "Participants";
                    displayName = "Last Week New Participants";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Participants Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FWB_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FWBs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "AR_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FWBs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FWB_LW":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Last Week FWBs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "CS_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FHLs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FHL_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FHLs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FHL_LW":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Last Week FHLs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FHL_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FHLs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FHL Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FWB_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FWBs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FWB Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FFR_TD":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Today FFRs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FFR Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FFR_YS":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Yesterday FFRs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FFR Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
            case "FFR_LW":
                {
                    myTableName = "LogitudeMessagesTransmissionLog";
                    myQueryCode = "All Logitude Transmission Logs";
                    displayName = "Last Week FFRs";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "FFR Zoom");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "String");
                    break;
                }
        }
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.ShowViews = false;
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "Dashboard";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadData(); });
        });
    };
    AirLineDashboardComponent.prototype.LoadData = function () {
        this.LoadPieQueries();
        this.LoadSpotlightQueries();
        this.FilterSelectedChangeActivity(this.SelectedItemActivity);
        this.FilterSelectedChange(this.SelectedItem);
    };
    AirLineDashboardComponent.prototype.TopParticipantClicking = function () {
        if (BarClick() != null) {
            this.OnTopParticipantClick(BarClick());
            ResetItem();
        }
    };
    AirLineDashboardComponent.prototype.OnTopParticipantClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "";
        var myTableName = "AirlineStatistics";
        var typeName = "";
        var displayTitle = "";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (flag) {
            switch (Key + "") {
                case "0": {
                    displayTitle = "FWBs";
                    typeName = "FWB";
                    myQueryCode = "All Airline Statistics";
                    break;
                }
                case "1": {
                    displayTitle = "FHLs";
                    typeName = "FHL";
                    myQueryCode = "All Airline Statistics";
                    break;
                }
                case "2": {
                    displayTitle = "FFRs";
                    typeName = "FFR";
                    myQueryCode = "All Airline Statistics";
                    break;
                }
            }
        }
        var days;
        if (this.timeRangeSelectedIndex == 0)
            days = -7;
        else if (this.timeRangeSelectedIndex == 1)
            days = -30;
        else if (this.timeRangeSelectedIndex == 2)
            days = -90;
        else if (this.timeRangeSelectedIndex == 3)
            days = -365;
        filterAgrs.addAdditionalFilter("SourceTenant", this.NewTopParticipantsList[e.target.columnIndex].ParticipantId[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("TopParticipantsFilter", typeName, days, null, "Equals", true, false, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Dashboard";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadData(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    AirLineDashboardComponent.prototype.LoadSpotlightQueries = function () {
        var _this = this;
        this.dashboarddomainservice.GetAirlineDashboardSpotlightCounts(this.TenantPM.Id).subscribe(function (myResult) {
            _this.dailySpotLightClass = myResult;
            if (_this.dailySpotLightClass.Participations_Today != 0)
                _this.Participations_Today_Status = false;
            if (_this.dailySpotLightClass.NewParticipants_Today != 0)
                _this.NewParticipants_Today_Status = false;
            if (_this.dailySpotLightClass.FWB_Today != 0)
                _this.FWB_Today_Status = false;
            if (_this.dailySpotLightClass.FHL_Today != 0)
                _this.FHL_Today_Status = false;
            if (_this.dailySpotLightClass.FFR_Today != 0)
                _this.FFR_Today_Status = false;
            if (_this.dailySpotLightClass.Participations_Yesterday != 0)
                _this.Participations_Yesterday_Status = false;
            if (_this.dailySpotLightClass.NewParticipants_Yesterday != 0)
                _this.NewParticipants_Yesterday_Status = false;
            if (_this.dailySpotLightClass.FWB_Yesterday != 0)
                _this.FWB_Yesterday_Status = false;
            if (_this.dailySpotLightClass.FHL_Yesterday != 0)
                _this.FHL_Yesterday_Status = false;
            if (_this.dailySpotLightClass.FFR_Yesterday != 0)
                _this.FFR_Yesterday_Status = false;
            if (_this.dailySpotLightClass.Participations_LastWeek != 0)
                _this.Participations_LastWeek_Status = false;
            if (_this.dailySpotLightClass.NewParticipants_LastWeek != 0)
                _this.NewParticipants_LastWeek_Status = false;
            if (_this.dailySpotLightClass.FWB_LastWeek != 0)
                _this.FWB_LastWeek_Status = false;
            if (_this.dailySpotLightClass.FHL_LastWeek != 0)
                _this.FHL_LastWeek_Status = false;
            if (_this.dailySpotLightClass.FFR_LastWeek != 0)
                _this.FFR_LastWeek_Status = false;
        });
    };
    AirLineDashboardComponent.prototype.ActivityStatusClick = function () {
        if (BarClick() != null) {
            this.OnActivityStatusClick(BarClick());
            ResetItem();
        }
    };
    AirLineDashboardComponent.prototype.OnActivityStatusClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "All Logitude Transmission Logs";
        var myTableName = "LogitudeMessagesTransmissionLog";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        var displayName = "Logitude Messages Transmission Logs";
        filterAgrs.SortBy = "SentDate";
        filterAgrs.SortDirection = "Descending";
        if (this.SelectedItemActivityShow != null)
            filterAgrs.addAdditionalFilter("MessageTypeCode", this.SelectedItemActivityShow.Index, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ActivityStatusChartFilter", this.barChartData[e.item.index].Month[e.target.columnIndex], this.barChartData[e.item.index].Year[e.target.columnIndex], null, "Equals", true, false, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "Dashboard";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.fillScreen(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    AirLineDashboardComponent.prototype.LoadChartData = function (days, showType) {
        var _this = this;
        this.dashboarddomainservice.GetActivityStatusByMessagesLogs(days, showType).subscribe(function (myResult) {
            _this.ActivityStatusData = myResult.getAll();
            _this.FillActivitiesStatus(_this.ActivityStatusData);
        });
    };
    AirLineDashboardComponent.prototype.LoadPieQueries = function () {
        var _this = this;
        this.dashboarddomainservice.GetDashBoardBookings(this.TenantPM.Id).subscribe(function (myResult) {
            _this.PieData = myResult;
            if (_this.PieData.length > 0) {
                _this.NoBookingInProgress = false;
                _this.FillPie();
            }
            else
                _this.NoBookingInProgress = true;
        });
    };
    AirLineDashboardComponent.prototype.FillActivitiesStatus = function (data) {
        var _this = this;
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
        data.forEach(function (element) {
            if (_this.barChartData[i] == null)
                _this.barChartData[i] = { data: [], label: null, Month: [], Year: [] };
            _this.barChartData[i].Month.push(element.Month);
            _this.barChartData[i].Year.push(element.Year);
            _this.barChartData[0].label = element.LabelProperty + "";
            _this.barChartData[0].data[i] = element.IntegerProperty + "";
            _this.barChartLabels[i] = element.LabelProperty + "";
            DataProvider[i] = { "category": _this.barChartLabels[i], "col1": _this.barChartData[0].data[i] };
            if (element.IntegerProperty > max)
                max = element.IntegerProperty;
            i++;
        });
        var barChartColors = [
            {
                backgroundColor: "rgb(73,165,191)" /*Safari 5.1-6*/,
                borderColor: "rgba(147,206,222,1)",
                borderWidth: 2
            }
        ];
        var flagEmpty = true;
        if (this.barChartData.length > 0)
            this.barChartData[0].data.forEach(function (p) {
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
    };
    AirLineDashboardComponent.prototype.GetCurrentDirectionTransmodeFilterItem = function () {
        var transmodeId = "";
        var directionId = "";
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
        var filterItem = new DirectionTransportFilter_1.DirectionTransportFilter("", directionId, transmodeId);
        return filterItem;
    };
    AirLineDashboardComponent.prototype.BookingClicking = function () {
        if (PieClick() != null) {
            this.OnBookingClick(PieClick());
            ResetItemPie();
        }
    };
    AirLineDashboardComponent.prototype.OnBookingClick = function (e) {
        var _this = this;
        var item = this.PieData[e.index];
        var myQueryCode = "All Airline Statistics";
        var myTableName = "AirlineStatistics";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("BookingsInProgressFilter", item.Code, null, null, "Equals", true, true, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Bookings";
        listArgs.BackButtonTitle = "Ticket";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadData(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    AirLineDashboardComponent.prototype.FillPie = function () {
        var _this = this;
        var fullData = [];
        this.pieChartLabels = [];
        this.pieChartData = [];
        this.PieData.forEach(function (element) {
            var IntegerProperty = Tools_1.AppTool.Round(element.IntegerProperty, 3);
            fullData.push({ label: element.StringProperty, data: IntegerProperty });
            _this.pieChartLabels.push(element.StringProperty);
            _this.pieChartData.push(element.IntegerProperty);
        });
        if (this.CurrentTop10DebtorsChart != null) {
            this.CurrentTop10DebtorsChart.clear();
            this.CurrentTop10DebtorsChart = null;
        }
        this.CurrentTop10DebtorsChart = makePieChart(this.BookingInProgressDashboardId, fullData, false, true, this.BookingInProgressDashboardLegendId);
    };
    AirLineDashboardComponent.prototype.FillLine = function (data) {
        var _this = this;
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
                offsetGridLines: false,
                scaleShowVerticalLines: false,
                data: [], label: 'Total', tension: 0, scaleShowHorizontalLines: false, scaleStepWidth: 0
            }];
        this.PieChartLabels = [];
        data.getAll().forEach(function (element) {
            _this.lineChartData[0].data[index] = element.YField + "";
            _this.PieChartLabels.push(element.XField);
            index++;
            _this.AmLineChartData.push({
                date: element.XField,
                visits: element.YField + ""
            });
        });
    };
    AirLineDashboardComponent.prototype.fillScreen = function () {
        this.LoadData();
    };
    AirLineDashboardComponent.prototype.FillFilters = function () {
        this.FilterList = [];
        this.FilterListActivity = [];
        var list = LastFilter_1.LastFilter.myList();
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[0].lastTitle, 0 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[1].lastTitle, 1 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[2].lastTitle, 2 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[3].lastTitle, 3 + ""));
        this.SelectedItem = this.FilterList[0];
        this.SelectedItemActivity = this.FilterList[0];
        this.FilterSelectedChangeActivity(this.SelectedItemActivity);
        this.FilterSelectedChange(this.SelectedItem);
        /////
        this.FilterListActivity.push(new DashboardFilters_1.DashBoardFilters("FWB", "FWB"));
        this.FilterListActivity.push(new DashboardFilters_1.DashBoardFilters("FHL", "FHL"));
        this.FilterListActivity.push(new DashboardFilters_1.DashBoardFilters("FSR", "FSR"));
        this.FilterListActivity.push(new DashboardFilters_1.DashBoardFilters("FFR", "FFR"));
        this.FilterListActivity.push(new DashboardFilters_1.DashBoardFilters("FVR", "FVR"));
        this.SelectedItemActivityShow = this.FilterListActivity[0];
        this.MoneyInLabel = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
    };
    AirLineDashboardComponent.prototype.FilterSelectedChangeShow = function (item) {
        this.SelectedItemActivityShow = item;
        var days;
        if (this.SelectedItemActivity.Index == 0)
            days = -7;
        else if (this.SelectedItemActivity.Index == 1)
            days = -30;
        else if (this.SelectedItemActivity.Index == 2)
            days = -90;
        else if (this.SelectedItemActivity.Index == 3)
            days = -365;
        this.timeRangeSelectedIndexShow = item.Index;
        if (this.SelectedItemActivityShow != null) {
            this.LoadChartData(days, this.SelectedItemActivityShow.Index);
        }
        else {
            this.LoadChartData(days, "FWB");
        }
    };
    AirLineDashboardComponent.prototype.FilterSelectedChangeActivity = function (item) {
        var days;
        this.SelectedItemActivity = item;
        if (item.Index == 0)
            days = -7;
        else if (item.Index == 1)
            days = -30;
        else if (item.Index == 2)
            days = -90;
        else if (item.Index == 3)
            days = -365;
        if (this.SelectedItemActivityShow != null) {
            this.LoadChartData(days, this.SelectedItemActivityShow.Index);
        }
        else {
            this.LoadChartData(days, "FWB");
        }
    };
    AirLineDashboardComponent.prototype.FilterSelectedChange = function (item) {
        var days;
        this.SelectedItem = item;
        if (item.Index == 0)
            days = -7;
        else if (item.Index == 1)
            days = -30;
        else if (item.Index == 2)
            days = -90;
        else if (item.Index == 3)
            days = -365;
        this.timeRangeSelectedIndex = item.Index;
        this.LoadParticipantsQuery(days);
    };
    AirLineDashboardComponent.prototype.ngOnInit = function () {
        this.FillFilters();
        this.fillScreen();
    };
    AirLineDashboardComponent = __decorate([
        core_1.Component({
            selector: 'DashBoard',
            moduleId: module.id,
            templateUrl: './AirLineDashboardComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [core_1.ComponentFactoryResolver])
    ], AirLineDashboardComponent);
    return AirLineDashboardComponent;
}());
exports.AirLineDashboardComponent = AirLineDashboardComponent;
//# sourceMappingURL=AirLineDashboardComponent.js.map