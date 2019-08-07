"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var DashboardFilters_1 = require("../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DirectionTransportFilter_1 = require("../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter");
var GroupByClass_1 = require("../../../Infrastructure/DataContracts/Dashboard/GroupByClass");
var FunctionsCRM_1 = require("../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM");
var List_1 = require("../../../Infrastructure/DataContracts/Dashboard/List");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var DashboardDomainService_1 = require("../../Services/DashboardDomainService");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var Tools_1 = require("../../../Infrastructure/Tools");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ActivityStatusDetailsComponent = /** @class */ (function (_super) {
    __extends(ActivityStatusDetailsComponent, _super);
    function ActivityStatusDetailsComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.FilterList = [];
        _this.FilterListActivity = [];
        _this.ShipmentsQuantityByTimeDashboardId = "ShipmentsQuantityByTimeDashboardId_";
        _this.DirectionAndTransportModeDashboardId = "DirectionAndTransportModeDashboardId_";
        _this.CountriesDashboardId = "CountriesDashboardId_";
        _this.CustomersDashboardId = "CustomersDashboardId_";
        _this.DataContext = _this;
        _this.topCustomers = 10;
        _this.topCountries = 10;
        _this.includeOthersCountries = true;
        _this.includeOthersCustomers = false;
        _this.barChartLabels = [];
        _this.NoShipmentsQuantityByTime = false;
        _this.NoDirectionAndTransportMode = false;
        _this.NoCountries = false;
        _this.NoCustomers = false;
        _this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        _this.selectedDirectionFilterShipment = "All";
        _this.selectedTransportFilterShipment = "All";
        _this.selectedDirectionFilterCountries = "All";
        _this.selectedTransportFilterCountries = "All";
        _this.selectedDirectionFilterCustomers = "All";
        _this.selectedTransportFilterCustomers = "All";
        _this.filterName_DateType = "DateType";
        _this.filterName_TimeRange = "TimeRange";
        _this.filterControlNameSpace = "Components.Partners.EditTabs.Customer.CustomerOverviewTabComponent";
        _this.logoff = new core_1.EventEmitter();
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.DirectionAndTransportModeDashboardId = _this.DirectionAndTransportModeDashboardId + _this.CurrentSession.GetChartId();
        _this.ShipmentsQuantityByTimeDashboardId = _this.ShipmentsQuantityByTimeDashboardId + _this.CurrentSession.GetChartId();
        _this.CountriesDashboardId = _this.CountriesDashboardId + _this.CurrentSession.GetChartId();
        _this.CustomersDashboardId = _this.CustomersDashboardId + _this.CurrentSession.GetChartId();
        _this.CountriesDashboardLegendId = "CountriesDashboardLegendId_" + _this.CurrentSession.GetNewId("CountriesDashboardLegendId");
        _this.CustomersDashboardLegendId = "CustomersDashboardLegendId_" + _this.CurrentSession.GetNewId("CustomersDashboardLegendId");
        _this.DirectionAndtransportModeLegendId = "DirectionAndtransportModeLegendId_" + _this.CurrentSession.GetNewId("DirectionAndtransportModeLegendId");
        return _this;
    }
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "TopCountries", {
        get: function () { return this.topCountries; },
        set: function (value) { this.topCountries = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "TopCustomers", {
        get: function () { return this.topCustomers; },
        set: function (value) {
            this.topCustomers = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "IncludeOthersCountries", {
        get: function () { return this.includeOthersCountries; },
        set: function (value) {
            this.includeOthersCountries = value;
            this.LoadShipmentsByTop10Countries();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "IncludeOthersCustomers", {
        get: function () { return this.includeOthersCustomers; },
        set: function (value) {
            this.includeOthersCustomers = value;
            this.LoadCustomers();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedDirectionFilterShipment", {
        get: function () { return this.selectedDirectionFilterShipment; },
        set: function (newValue) {
            if (this.selectedDirectionFilterShipment != newValue) {
                this.selectedDirectionFilterShipment = newValue;
                if (this.SelectedTimeRangeItem.Index == "-1") {
                    this.LoadActivityStatus();
                }
                else {
                    this.CommonFiltersShipment();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedTransportFilterShipment", {
        get: function () { return this.selectedTransportFilterShipment; },
        set: function (newValue) {
            if (this.selectedTransportFilterShipment != newValue) {
                this.selectedTransportFilterShipment = newValue;
                if (this.SelectedTimeRangeItem.Index == "-1") {
                    this.LoadActivityStatus();
                }
                else {
                    this.CommonFiltersShipment();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedDirectionFilterCountries", {
        get: function () { return this.selectedDirectionFilterCountries; },
        set: function (newValue) {
            if (this.selectedDirectionFilterCountries != newValue) {
                this.selectedDirectionFilterCountries = newValue;
                this.LoadShipmentsByTop10Countries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedTransportFilterCountries", {
        get: function () { return this.selectedTransportFilterCountries; },
        set: function (newValue) {
            if (this.selectedTransportFilterCountries != newValue) {
                this.selectedTransportFilterCountries = newValue;
                this.LoadShipmentsByTop10Countries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedDirectionFilterCustomers", {
        get: function () { return this.selectedDirectionFilterCustomers; },
        set: function (newValue) {
            if (this.selectedDirectionFilterCustomers != newValue) {
                this.selectedDirectionFilterCustomers = newValue;
                this.LoadCustomers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedTransportFilterCustomers", {
        get: function () { return this.selectedTransportFilterCustomers; },
        set: function (newValue) {
            if (this.selectedTransportFilterCustomers != newValue) {
                this.selectedTransportFilterCustomers = newValue;
                this.LoadCustomers();
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityStatusDetailsComponent.prototype.LoadActivityStatus = function () {
        var _this = this;
        var service = new DashboardDomainService_1.DashboardDomainService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", this.SelectedDirectionFilterShipment, this.SelectedTransportFilterShipment).subscribe(function (myResult) {
                    _this.FinalShipmentData = myResult;
                    _this.CommonFiltersShipment();
                });
            }
        }
        else {
            var days = this.ComputeDays();
            service.GetActivityStatus(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id).subscribe(function (myResult) {
                _this.FinalShipmentData = myResult;
                _this.CommonFiltersShipment();
            });
        }
    };
    ActivityStatusDetailsComponent.prototype.LoadDirectionAndTransportmode = function () {
        var _this = this;
        var service = new DashboardDomainService_1.DashboardDomainService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetShipmentByDirectionAndTransmodeCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate).subscribe(function (myResult) {
                    _this.FinalDirectionAndTransportData = myResult;
                    _this.CommonFiltersDirectionAndTransportMode();
                });
            }
        }
        else {
            var days = this.ComputeDays();
            service.GetShipmentByDirectionAndTransmode(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id, null).subscribe(function (myResult) {
                _this.FinalDirectionAndTransportData = myResult;
                _this.CommonFiltersDirectionAndTransportMode();
            });
        }
    };
    ActivityStatusDetailsComponent.prototype.LoadShipmentsByTop10Countries = function () {
        var _this = this;
        var service = new DashboardDomainService_1.DashboardDomainService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                var dtf = this.GetCurrentDirectionTransmodeFilterItemCountries();
                service.GetShipmentsByTop10CountriesDashBoardCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, "", dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(function (myResult) {
                    _this.FinalCountriesData = myResult;
                    var countriesFilterdList = FunctionsCRM_1.FunctionsCRM.getCountriesFilterdList(_this.FinalCountriesData, parseInt(_this.SelectedShowItem.Index), _this.TopCountries, _this.IncludeOthersCountries);
                    _this.fillCountriesPie(countriesFilterdList);
                });
            }
        }
        else {
            this.CommonFiltersCountries();
        }
    };
    ActivityStatusDetailsComponent.prototype.LoadCustomers = function () {
        var _this = this;
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                var service = new DashboardDomainService_1.DashboardDomainService();
                service.GetTop10DashBoardCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCustomers, this.IncludeOthersCustomers, this.SelectedDirectionFilterCustomers, this.SelectedTransportFilterCustomers).subscribe(function (myResult) {
                    _this.FinalCustomersData = myResult;
                    _this.FillCustomersPie();
                });
            }
        }
        else {
            this.CommonFiltersCustomers();
        }
    };
    ActivityStatusDetailsComponent.prototype.LoadQuires = function () {
        this.LoadActivityStatus();
        this.LoadDirectionAndTransportmode();
        this.LoadShipmentsByTop10Countries();
        this.LoadCustomers();
    };
    ActivityStatusDetailsComponent.prototype.CommonFiltersCustomers = function () {
        var _this = this;
        var service = new DashboardDomainService_1.DashboardDomainService();
        var days = this.ComputeDays();
        service.GetTop10DashBoard(this.SelectedDateTypeItem.Index, 0, days, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCustomers, this.IncludeOthersCustomers, this.SelectedDirectionFilterCustomers, this.SelectedTransportFilterCustomers).subscribe(function (myResult) {
            _this.FinalCustomersData = myResult;
            _this.FillCustomersPie();
        });
    };
    ActivityStatusDetailsComponent.prototype.FillCustomersPie = function () {
        if (this.FinalCustomersData != null) {
            var dtf = this.GetCurrentDirectionTransmodeFilterItemCustomers();
            var directionFilteredList = FunctionsCRM_1.FunctionsCRM.getDirectionFilteredList(dtf, this.FinalCustomersData);
            var measurmentFilteredList = FunctionsCRM_1.FunctionsCRM.getCustomersFilterdList(directionFilteredList, parseInt(this.SelectedShowItem.Index));
            var pieChartLabels = [];
            var pieChartData = [];
            var fullData = [];
            measurmentFilteredList.getAll() != null ? measurmentFilteredList.getAll().forEach(function (element) {
                if (element.YField != 0) {
                    fullData.push({ label: element.XField, data: element.YField });
                    pieChartLabels.push(element.XField);
                    pieChartData.push(element.YField);
                }
            }) : null;
            var flagEmpty = true;
            pieChartData.forEach(function (p) {
                if (p != "0")
                    flagEmpty = false;
            });
            if (this.CurrentCustomersChart != null) {
                this.CurrentCustomersChart.clear();
                this.CurrentCustomersChart = null;
            }
            if (!flagEmpty) {
                this.CurrentCustomersChart = makePieChart(this.CustomersDashboardId, fullData, false, true, this.CustomersDashboardLegendId);
                this.NoCustomers = false;
            }
            else {
                try {
                    //   var elm = document.getElementById(this.CustomersDashboardId);
                }
                catch (exc) { }
                //  elm.innerHTML = "";
                this.NoCustomers = true;
            }
        }
    };
    ActivityStatusDetailsComponent.prototype.GetCurrentDirectionTransmodeFilterItemShipment = function () {
        var transmodeId = "";
        var directionId = "";
        if (this.SelectedDirectionFilterShipment == "All")
            directionId = "";
        else
            directionId = this.SelectedDirectionFilterShipment;
        if (this.SelectedTransportFilterShipment == "All")
            transmodeId = "";
        else
            transmodeId = this.SelectedTransportFilterShipment;
        var filterItem = new DirectionTransportFilter_1.DirectionTransportFilter("", directionId, transmodeId);
        return filterItem;
    };
    ActivityStatusDetailsComponent.prototype.GetCurrentDirectionTransmodeFilterItemCountries = function () {
        var transmodeId = "";
        var directionId = "";
        if (this.SelectedDirectionFilterCountries == "All")
            directionId = "";
        else
            directionId = this.SelectedDirectionFilterCountries;
        if (this.SelectedTransportFilterCountries == "All")
            transmodeId = "";
        else
            transmodeId = this.SelectedTransportFilterCountries;
        var filterItem = new DirectionTransportFilter_1.DirectionTransportFilter("", directionId, transmodeId);
        return filterItem;
    };
    ActivityStatusDetailsComponent.prototype.GetCurrentDirectionTransmodeFilterItemCustomers = function () {
        var transmodeId = "";
        var directionId = "";
        if (this.SelectedDirectionFilterCustomers == "All")
            directionId = "";
        else
            directionId = this.SelectedDirectionFilterCustomers;
        if (this.SelectedTransportFilterCustomers == "All")
            transmodeId = "";
        else
            transmodeId = this.SelectedTransportFilterCustomers;
        var filterItem = new DirectionTransportFilter_1.DirectionTransportFilter("", directionId, transmodeId);
        return filterItem;
    };
    ActivityStatusDetailsComponent.prototype.CommonFiltersDirectionAndTransportMode = function () {
        var _this = this;
        if (this.SelectedTimeRangeItem.Index != "-1") {
            var byMonthData = this.FinalDirectionAndTransportData != null ? this.FinalDirectionAndTransportData : [];
            var measurmentFilteredList = FunctionsCRM_1.FunctionsCRM.getMeasurmentFilterListForDirectionAndTransmode(parseInt(this.SelectedShowItem.Index), byMonthData, parseInt(this.SelectedTimeRangeItem.Index), null);
            this.FillDirectionPie(measurmentFilteredList);
        }
        else {
            var FilteredList = new List_1.List();
            this.FinalDirectionAndTransportData != null ? this.FinalDirectionAndTransportData.items.forEach(function (item) {
                var obj = new GroupByClass_1.GroupByClass();
                obj.XField = item.DirectionName + "/" + item.TransportModeName;
                switch (parseInt(_this.SelectedShowItem.Index)) {
                    case 0:
                        {
                            obj.YField = item.Total != null ? item.Total : 0;
                            break;
                        }
                    case 1:
                        {
                            obj.YField = item.SumChargeableWeight != null ? item.SumChargeableWeight : 0;
                            break;
                        }
                    case 2:
                        {
                            obj.YField = item.SumGrossWeight != null ? item.SumGrossWeight : 0;
                            break;
                        }
                    case 3:
                        {
                            obj.YField = item.TotalProfitInLocalCurrency != null ? item.TotalProfitInLocalCurrency : 0;
                            break;
                        }
                    case 4:
                        {
                            obj.YField = item.TotalProfitInProfitCurrency != null ? item.TotalProfitInProfitCurrency : 0;
                            break;
                        }
                    case 5:
                        {
                            obj.YField = item.ReceivablesInLocalCurrency != null ? item.ReceivablesInLocalCurrency : 0;
                            break;
                        }
                    case 6:
                        {
                            obj.YField = item.ReceivablesInProfitCurrency != null ? item.ReceivablesInProfitCurrency : 0;
                            break;
                        }
                }
                FilteredList.add(obj);
            }) : null;
            this.FillDirectionPie(FilteredList);
        }
    };
    ActivityStatusDetailsComponent.prototype.CommonFiltersCountries = function () {
        var _this = this;
        var days = this.ComputeDays();
        var dtf = this.GetCurrentDirectionTransmodeFilterItemCountries();
        var service = new DashboardDomainService_1.DashboardDomainService();
        service.GetShipmentsByTop10CountriesDashBoard(this.SelectedDateTypeItem.Index, 0, days, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, "", dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(function (myResult) {
            _this.FinalCountriesData = myResult;
            var countriesFilterdList = FunctionsCRM_1.FunctionsCRM.getCountriesFilterdList(_this.FinalCountriesData, parseInt(_this.SelectedShowItem.Index), _this.TopCountries, _this.IncludeOthersCountries);
            _this.fillCountriesPie(countriesFilterdList);
        });
    };
    ActivityStatusDetailsComponent.prototype.fillCountriesPie = function (data) {
        var fullData = [];
        var pieChartLabels = [];
        var pieChartData = [];
        data.getAll() != null ? data.getAll().forEach(function (element) {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField });
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        }) : null;
        var flagEmpty = true;
        pieChartData.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (this.CurrentCountriesChart != null) {
            this.CurrentCountriesChart.clear();
            this.CurrentCountriesChart = null;
        }
        if (!flagEmpty) {
            this.CurrentCountriesChart = makePieChart(this.CountriesDashboardId, fullData, false, true, this.CountriesDashboardLegendId);
            this.NoCountries = false;
        }
        else {
            this.NoCountries = true;
        }
    };
    ActivityStatusDetailsComponent.prototype.FillDirectionPie = function (data) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        data.getAll() != null ? data.getAll().forEach(function (element) {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField });
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        }) : null;
        var flagEmpty = true;
        pieChartData.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (this.CurrentDirectionAndTransportModeChart != null) {
            this.CurrentDirectionAndTransportModeChart.clear();
            this.CurrentDirectionAndTransportModeChart = null;
        }
        if (!flagEmpty) {
            this.CurrentDirectionAndTransportModeChart = makePieChart(this.DirectionAndTransportModeDashboardId, fullData, false, true, this.DirectionAndtransportModeLegendId);
            this.NoDirectionAndTransportMode = false;
        }
        else {
            this.NoDirectionAndTransportMode = true;
        }
    };
    ActivityStatusDetailsComponent.prototype.CommonFiltersShipment = function () {
        var _this = this;
        var showIndex = parseInt(this.SelectedShowItem.Index);
        var timeIndex = parseInt(this.SelectedTimeRangeItem.Index);
        var byMonthData = this.FinalShipmentData != null ? this.FinalShipmentData : new List_1.List();
        var dtf = this.GetCurrentDirectionTransmodeFilterItemShipment();
        var directionFilteredList = FunctionsCRM_1.FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);
        if (timeIndex == -1) {
            var diff = Tools_1.DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate) - 1;
            if (diff <= 7) {
                timeIndex = 0;
            }
            else if (diff <= 30 || (diff <= 32 && this.ActivityFromDate.getDate() == this.ActivityFromDate.getDate())) {
                timeIndex = 1;
            }
            else if (diff <= 90 || (diff <= 93 && this.ActivityFromDate.getDate() == this.ActivityFromDate.getDate())) {
                timeIndex = 2;
            }
            else if (diff <= 365 || (diff <= 365 && this.ActivityFromDate.getDate() == this.ActivityFromDate.getDate())) {
                timeIndex = 3;
            }
            else {
                timeIndex = 4;
            }
        }
        if (this.SelectedTimeRangeItem.Index != "-1") {
            var measurmentFilteredList = FunctionsCRM_1.FunctionsCRM.getMeasurmentFilterList(showIndex, directionFilteredList, timeIndex, null);
            var monthQuartersList = FunctionsCRM_1.FunctionsCRM.getmonthQuartersList(timeIndex, measurmentFilteredList, null);
            this.FillBars(monthQuartersList);
        }
        else {
            var FilteredList = new List_1.List();
            this.FinalShipmentData != null ? this.FinalShipmentData.items.forEach(function (item) {
                var obj = new GroupByClass_1.GroupByClass();
                obj.XField = item.DateRange;
                switch (parseInt(_this.SelectedShowItem.Index)) {
                    case 0:
                        {
                            obj.YField = item.Total != null ? item.Total : 0;
                            break;
                        }
                    case 1:
                        {
                            obj.YField = item.SumChargeableWeight != null ? item.SumChargeableWeight : 0;
                            break;
                        }
                    case 2:
                        {
                            obj.YField = item.SumGrossWeight != null ? item.SumGrossWeight : 0;
                            break;
                        }
                    case 3:
                        {
                            obj.YField = item.TotalProfitInLocalCurrency != null ? item.TotalProfitInLocalCurrency : 0;
                            break;
                        }
                    case 4:
                        {
                            obj.YField = item.TotalProfitInProfitCurrency != null ? item.TotalProfitInProfitCurrency : 0;
                            break;
                        }
                    case 5:
                        {
                            obj.YField = item.ReceivablesInLocalCurrency != null ? item.ReceivablesInLocalCurrency : 0;
                            break;
                        }
                    case 6:
                        {
                            obj.YField = item.ReceivablesInProfitCurrency != null ? item.ReceivablesInProfitCurrency : 0;
                            break;
                        }
                }
                FilteredList.add(obj);
            }) : null;
            this.FillBars(FilteredList);
        }
    };
    ActivityStatusDetailsComponent.prototype.FilterSelectedChangeShow = function (item) {
        this.SelectedShowItem = item;
        this.CommonFiltersShipment();
        this.CommonFiltersDirectionAndTransportMode();
        this.LoadShipmentsByTop10Countries();
        this.LoadCustomers();
    };
    ActivityStatusDetailsComponent.prototype.FillBars = function (data) {
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
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        data.getAll() != null ? data.getAll().forEach(function (element) {
            _this.barChartData[0].label = "Quantity";
            _this.barChartData[0].data[i] = element.YField + "";
            ;
            _this.barChartLabels[i] = element.XField + "";
            DataProvider[i] = { "category": _this.barChartLabels[i], "col1": _this.barChartData[0].data[i] };
            if (element.YField > max)
                max = element.YField;
            i++;
        }) : null;
        var barChartColors = [
            {
                backgroundColor: "rgb(73,165,191)" /*Safari 5.1-6*/,
                borderColor: "rgba(147,206,222,1)",
                borderWidth: 2
            }
        ];
        var flagEmpty = true;
        this.barChartData[0].data.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
            makeAmBarChart(this.ShipmentsQuantityByTimeDashboardId, Graphs, DataProvider, max);
            this.NoShipmentsQuantityByTime = false;
        }
        else {
            try {
                var elm = document.getElementById(this.ShipmentsQuantityByTimeDashboardId);
                elm.innerHTML = "";
            }
            catch (exc) { }
            this.NoShipmentsQuantityByTime = true;
        }
    };
    ActivityStatusDetailsComponent.prototype.FillFilters = function () {
        this.FillDateTypeFilterList();
        this.FillShowFilterList();
        this.FillTimeRangeFilterList();
        this.LoadQuires();
    };
    ActivityStatusDetailsComponent.prototype.ngOnInit = function () {
        this.FillFilters();
    };
    ActivityStatusDetailsComponent.prototype.FillDateTypeFilterList = function () {
        this.DateTypeFilterList = [];
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Operational Date", "OperationalDate"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
    };
    ActivityStatusDetailsComponent.prototype.FillTimeRangeFilterList = function () {
        this.TimeRangeFilterList = [];
        var list = LastFilter_1.LastFilter.ActivitymyList();
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[3].lastTitle, "3"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[4].lastTitle, "-1"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRange);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }
        this.selectedTimeRangeItem = this.TimeRangeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
        if (this.selectedTimeRangeItem.Index == "-1") {
            var ActiviytFromDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "DetailsActivityFromDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.activityFromDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
            var ActiviytToDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "DetailsActivityToDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.activityToDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
        }
    };
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedShowItem", {
        get: function () { return this.selectedShowItem; },
        set: function (value) {
            if (this.selectedShowItem != value) {
                this.selectedShowItem = value;
                this.FilterSelectedShow();
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityStatusDetailsComponent.prototype.FilterSelectedShow = function () {
        this.CommonFiltersShipment();
        this.CommonFiltersDirectionAndTransportMode();
        this.LoadShipmentsByTop10Countries();
        this.LoadCustomers();
    };
    ActivityStatusDetailsComponent.prototype.FillShowFilterList = function () {
        this.ShowFilterList = [];
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("Shipments", "0"));
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("ChargeWeight", "1"));
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("GrossWeight", "2"));
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("Profit (" + SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode + ")", "3"));
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("Profit (" + SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode + ")", "4"));
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("Receivables (" + SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode + ")", "5"));
        this.ShowFilterList.push(new DashboardFilters_1.DashBoardFilters("Receivables (" + SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode + ")", "6"));
        this.selectedShowItem = this.ShowFilterList[0];
    };
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedDateTypeItem", {
        get: function () { return this.selectedDateTypeItem; },
        set: function (value) {
            if (this.selectedDateTypeItem != value) {
                this.selectedDateTypeItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_DateType, (value == null ? null : value.Index));
                this.LoadQuires();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "SelectedTimeRangeItem", {
        get: function () { return this.selectedTimeRangeItem; },
        set: function (value) {
            if (this.selectedTimeRangeItem != value) {
                this.selectedTimeRangeItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRange, (value == null ? null : value.Index));
                if (value.Index == "-1") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityFromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityToDate)));
                }
            }
            this.LoadQuires();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "ActivityFromDate", {
        get: function () { return this.activityFromDate; },
        set: function (value) {
            if (value != this.activityFromDate) {
                this.activityFromDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                //  this.LoadQuires();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDetailsComponent.prototype, "ActivityToDate", {
        get: function () { return this.activityToDate; },
        set: function (value) {
            if (value != this.activityToDate) {
                this.activityToDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "DetailsActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                // this.LoadQuires();
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityStatusDetailsComponent.prototype.ComputeDays = function () {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        this.activityFromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedTimeRangeItem.Index == "0") {
            days = -7;
            Todate.setDate(Todate.getDate() - 6);
            this.activityToDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "1") {
            days = -30;
            Todate.setMonth(Todate.getMonth() - 1);
            this.activityToDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.activityToDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "3") {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.activityToDate = Todate;
        }
        return days;
    };
    ActivityStatusDetailsComponent.prototype.BackButtonClicked = function () {
        this.logoff.emit();
    };
    ActivityStatusDetailsComponent.prototype.CountriesTopValueChanged = function (flag) {
        if (flag)
            this.TopCountries = this.TopCountries + 1;
        else
            this.TopCountries = this.TopCountries - 1;
        if (this.TopCountries < 0)
            this.TopCountries = 0;
        this.LoadShipmentsByTop10Countries();
    };
    ActivityStatusDetailsComponent.prototype.CustomersTopValueChanged = function (flag) {
        if (flag)
            this.TopCustomers = this.TopCustomers + 1;
        else
            this.TopCustomers = this.TopCustomers - 1;
        if (this.TopCustomers < 0)
            this.TopCustomers = 0;
        this.LoadCustomers();
    };
    ActivityStatusDetailsComponent.prototype.Change = function () {
        console.log("Fired2");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ActivityStatusDetailsComponent.prototype, "logoff", void 0);
    ActivityStatusDetailsComponent = __decorate([
        core_1.Component({
            selector: 'ActivityStatusDetailsComponent',
            moduleId: module.id,
            templateUrl: './ActivityStatusDetailsComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [])
    ], ActivityStatusDetailsComponent);
    return ActivityStatusDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.ActivityStatusDetailsComponent = ActivityStatusDetailsComponent;
//# sourceMappingURL=ActivityStatusDetailsComponent.js.map