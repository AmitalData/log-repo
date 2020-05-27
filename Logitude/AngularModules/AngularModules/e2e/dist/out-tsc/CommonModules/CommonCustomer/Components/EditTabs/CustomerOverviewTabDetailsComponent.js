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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DashboardFilters_1 = require("../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var List_1 = require("../../../../Infrastructure/DataContracts/Dashboard/List");
var FunctionsCRM_1 = require("../../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM");
var DirectionTransportFilter_1 = require("../../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter");
var GroupByClass_1 = require("../../../../Infrastructure/DataContracts/Dashboard/GroupByClass");
var LastFilter_1 = require("../../../../Infrastructure/Utilities/LastFilter");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var DashboardDomainService_1 = require("../../../../Dashboard/Services/DashboardDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LastFilterClass_1 = require("../../../../Infrastructure/Utilities/LastFilterClass");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var CustomerOverviewTabDetailsComponent = /** @class */ (function (_super) {
    __extends(CustomerOverviewTabDetailsComponent, _super);
    function CustomerOverviewTabDetailsComponent() {
        var _this = _super.call(this) || this;
        _this.flag = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ShipmentQuantityByTimeOverViewId = "ShipmentQuantityByTimeOverViewId_";
        _this.CountriesOverviewId = "CountriesOverviewId_";
        _this.DirectionAndTransportModeOverViewId = "DirectionAndTransportModeOverViewId_";
        _this.NoShipmentsQuantityByTime = false;
        _this.NoDirectionAndTransportMode = false;
        _this.NoCountries = false;
        _this.FilterList = [];
        _this.FilterListActivity = [];
        _this.timeRangeSelectedIndex = 0;
        _this.DataContext = _this;
        _this.topCountries = 10;
        _this.filterName_TimeRange = "TimeRange";
        _this.filterControlNameSpace = "Components.Partners.EditTabs.Customer.CustomerOverviewDetailsTabComponent";
        _this.includeOthersCountries = true;
        _this.selectedTransportFilterCustomers = "All";
        _this.filterName_DateType = "DateType";
        _this.barChartLabels = [];
        _this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        _this.selectedDirectionFilterShipment = "All";
        _this.selectedTransportFilterShipment = "All";
        _this.selectedDirectionFilterCountries = "All";
        _this.selectedTransportFilterCountries = "All";
        _this.BackCompleted = new core_1.EventEmitter();
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.myChartsService = new DashboardDomainService_1.DashboardDomainService();
        _this.ShipmentQuantityByTimeOverViewId = _this.ShipmentQuantityByTimeOverViewId + _this.CurrentSession.GetChartId();
        _this.DirectionAndTransportModeOverViewId = _this.DirectionAndTransportModeOverViewId + _this.CurrentSession.GetChartId();
        _this.CountriesOverviewId = _this.CountriesOverviewId + _this.CurrentSession.GetChartId();
        _this.CountriesDashboardLegendId = "CountriesDashboardLegendId_" + _this.CurrentSession.GetNewId("CountriesDashboardLegendId");
        _this.DirectionAndtransportModeLegendId = "DirectionAndtransportModeLegendId_" + _this.CurrentSession.GetNewId("DirectionAndtransportModeLegendId");
        return _this;
    }
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "TopCountries", {
        get: function () { return this.topCountries; },
        set: function (value) { this.topCountries = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "IncludeOthersCountries", {
        get: function () { return this.includeOthersCountries; },
        set: function (value) {
            this.includeOthersCountries = value;
            this.CommonFiltersCountries();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedDirectionFilterShipment", {
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
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedTransportFilterShipment", {
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
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedDirectionFilterCountries", {
        get: function () { return this.selectedDirectionFilterCountries; },
        set: function (newValue) {
            if (this.selectedDirectionFilterCountries != newValue) {
                this.selectedDirectionFilterCountries = newValue;
                this.CommonFiltersCountries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedTransportFilterCountries", {
        get: function () { return this.selectedTransportFilterCountries; },
        set: function (newValue) {
            if (this.selectedTransportFilterCountries != newValue) {
                this.selectedTransportFilterCountries = newValue;
                this.CommonFiltersCountries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedDateTypeItem", {
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
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedTimeRangeItem", {
        get: function () { return this.selectedTimeRangeItem; },
        set: function (value) {
            if (this.selectedTimeRangeItem != value) {
                this.selectedTimeRangeItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRange, (value == null ? null : value.Index));
                if (value.Index == "-1") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityFromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityToDate)));
                }
            }
            this.LoadQuires();
        },
        enumerable: true,
        configurable: true
    });
    CustomerOverviewTabDetailsComponent.prototype.LoadActivityStatus = function () {
        var _this = this;
        var service = new DashboardDomainService_1.DashboardDomainService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", this.SelectedDirectionFilterShipment, this.SelectedTransportFilterShipment, this.Customer.Id).subscribe(function (myResult) {
                    _this.FinalShipmentData = myResult;
                    _this.CommonFiltersShipment();
                });
            }
        }
        else {
            var days = this.ComputeDays();
            var month = 0;
            if (days == -1095) {
                month = -36;
                days = 0;
            }
            service.GetActivityStatus(this.SelectedDateTypeItem.Index, month, days, this.TenantPM.Id, this.Customer.Id).subscribe(function (myResult) {
                _this.FinalShipmentData = myResult;
                _this.CommonFiltersShipment();
            });
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.LoadQuires = function () {
        var _this = this;
        var service = new DashboardDomainService_1.DashboardDomainService();
        this.LoadActivityStatus();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            service.GetShipmentByDirectionAndTransmodeCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.Customer.Id).subscribe(function (myResult) {
                _this.FinalDirectionAndTransportData = myResult;
                _this.CommonFiltersDirectionAndTransportMode();
            });
            this.CommonFiltersCountries();
        }
        else {
            var days = this.ComputeDays();
            var month = 0;
            if (days == -1095) {
                month = -36;
                days = 0;
            }
            service.GetShipmentByDirectionAndTransmode(this.SelectedDateTypeItem.Index, month, days, this.TenantPM.Id, this.Customer.Id).subscribe(function (myResult) {
                _this.FinalDirectionAndTransportData = myResult;
                _this.CommonFiltersDirectionAndTransportMode();
            });
            this.CommonFiltersCountries();
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.ComputeDays = function () {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        this.activityFromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedTimeRangeItem.Index == "0") {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.activityToDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "1") {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.activityToDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -1095;
            Todate.setMonth(Todate.getMonth() - 36);
            this.activityToDate = Todate;
        }
        return days;
    };
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "ActivityFromDate", {
        get: function () { return this.activityFromDate; },
        set: function (value) {
            if (value != this.activityFromDate) {
                this.activityFromDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "ActivityToDate", {
        get: function () { return this.activityToDate; },
        set: function (value) {
            if (value != this.activityToDate) {
                this.activityToDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerOverviewTabDetailsComponent.prototype.GetCurrentDirectionTransmodeFilterItemShipment = function () {
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
    CustomerOverviewTabDetailsComponent.prototype.GetCurrentDirectionTransmodeFilterItemCountries = function () {
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
    CustomerOverviewTabDetailsComponent.prototype.CommonFiltersDirectionAndTransportMode = function () {
        var byMonthData = this.FinalDirectionAndTransportData;
        var measurmentFilteredList = FunctionsCRM_1.FunctionsCRM.getMeasurmentFilterListForDirectionAndTransmode(parseInt(this.SelectedShowItem.Index), byMonthData, this.timeRangeSelectedIndex, this.Customer.Id);
        this.FillDirectionPie(measurmentFilteredList);
    };
    CustomerOverviewTabDetailsComponent.prototype.CommonFiltersCountries = function () {
        var _this = this;
        if (this.SelectedTimeRangeItem.Index == "-1") {
            var dtf = this.GetCurrentDirectionTransmodeFilterItemCountries();
            var service = new DashboardDomainService_1.DashboardDomainService();
            service.GetShipmentsByTop10CountriesDashBoardCustom(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, this.Customer.Id, dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(function (myResult) {
                _this.FinalCountriesData = myResult;
                var countriesFilterdList = FunctionsCRM_1.FunctionsCRM.getCountriesFilterdList(_this.FinalCountriesData, parseInt(_this.SelectedShowItem.Index), _this.TopCountries, _this.IncludeOthersCountries);
                _this.fillCountriesPie(countriesFilterdList);
            });
        }
        else {
            var days = this.ComputeDays();
            var month = 0;
            if (days == -1095) {
                month = -36;
                days = 0;
            }
            var dtf = this.GetCurrentDirectionTransmodeFilterItemCountries();
            var service = new DashboardDomainService_1.DashboardDomainService();
            service.GetShipmentsByTop10CountriesDashBoard(this.SelectedDateTypeItem.Index, month, days, parseInt(this.SelectedShowItem.Index), this.TenantPM.Id, this.TopCountries, this.IncludeOthersCountries, this.Customer.Id, dtf.FilterDirectionID, dtf.FilterTransportID).subscribe(function (myResult) {
                _this.FinalCountriesData = myResult;
                var countriesFilterdList = FunctionsCRM_1.FunctionsCRM.getCountriesFilterdList(_this.FinalCountriesData, parseInt(_this.SelectedShowItem.Index), _this.TopCountries, _this.IncludeOthersCountries);
                _this.fillCountriesPie(countriesFilterdList);
            });
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.fillCountriesPie = function (data) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        console.log(data);
        data.getAll().forEach(function (element) {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField });
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        });
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
            this.CurrentCountriesChart = makePieChart(this.CountriesOverviewId, fullData, false, true, this.CountriesDashboardLegendId);
            this.NoCountries = false;
        }
        else {
            this.NoCountries = true;
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.FillDirectionPie = function (data) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        data.getAll().forEach(function (element) {
            if (element.YField != 0) {
                fullData.push({ label: element.XField, data: element.YField });
                pieChartLabels.push(element.XField);
                pieChartData.push(element.YField);
            }
        });
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
            this.CurrentDirectionAndTransportModeChart = makePieChart(this.DirectionAndTransportModeOverViewId, fullData, false, true, this.DirectionAndtransportModeLegendId);
            this.NoDirectionAndTransportMode = false;
        }
        else {
            this.NoDirectionAndTransportMode = true;
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.CommonFiltersShipment = function () {
        var _this = this;
        var showIndex = parseInt(this.SelectedShowItem.Index);
        var byMonthData = this.FinalShipmentData;
        var dtf = this.GetCurrentDirectionTransmodeFilterItemShipment();
        var directionFilteredList = FunctionsCRM_1.FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);
        if (this.SelectedTimeRangeItem.Index != "-1") {
            var measurmentFilteredList = FunctionsCRM_1.FunctionsCRM.getMeasurmentFilterList(showIndex, directionFilteredList, +this.SelectedTimeRangeItem.Index, this.Customer.Id);
            var monthQuartersList = FunctionsCRM_1.FunctionsCRM.getmonthQuartersList(+this.SelectedTimeRangeItem.Index, measurmentFilteredList, this.Customer.Id);
            this.FillBars(monthQuartersList);
        }
        else {
            var FilteredList = new List_1.List();
            this.FinalShipmentData.items != null ? this.FinalShipmentData.items.forEach(function (item) {
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
    CustomerOverviewTabDetailsComponent.prototype.FilterSelectedChangeShow = function (item) {
        this.SelectedShowItem = item;
        this.CommonFiltersShipment();
        this.CommonFiltersDirectionAndTransportMode();
        this.CommonFiltersCountries();
    };
    CustomerOverviewTabDetailsComponent.prototype.FillBars = function (data) {
        var _this = this;
        var Graphs = Graphs = [{
                "balloonText": "[[value]]",
                "fillAlphas": 1,
                "id": "AmGraph-1" + i,
                "title": "Invoices",
                "type": "column",
                "lineAlpha": 0,
                "valueField": "col1",
                "fillColors": ["#487E9F", "#c8d8e2"],
                "gradientOrientation": "horizontal",
                "borderAlpha": 0,
            }];
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        data.getAll().forEach(function (element) {
            _this.barChartData[0].label = "Quantity";
            _this.barChartData[0].data[i] = element.YField + "";
            _this.barChartLabels[i] = element.XField + "";
            ;
            DataProvider[i] = { "category": _this.barChartLabels[i], "col1": _this.barChartData[0].data[i] };
            if (element.YField > max)
                max = element.YField;
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
        this.barChartData[0].data.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
            makeAmBarChart(this.ShipmentQuantityByTimeOverViewId, Graphs, DataProvider, max, false, null);
            this.NoShipmentsQuantityByTime = false;
        }
        else {
            try {
                var elm = document.getElementById(this.ShipmentQuantityByTimeOverViewId);
                elm.innerHTML = "";
            }
            catch (exc) { }
            this.NoShipmentsQuantityByTime = true;
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.FillFilters = function () {
        this.FillDateTypeFilterList();
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.LoadQuires();
    };
    Object.defineProperty(CustomerOverviewTabDetailsComponent.prototype, "SelectedShowItem", {
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
    CustomerOverviewTabDetailsComponent.prototype.FilterSelectedShow = function () {
        this.CommonFiltersShipment();
        this.CommonFiltersDirectionAndTransportMode();
        this.CommonFiltersCountries();
    };
    CustomerOverviewTabDetailsComponent.prototype.FillShowFilterList = function () {
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
    CustomerOverviewTabDetailsComponent.prototype.FillDateTypeFilterList = function () {
        this.DateTypeFilterList = [];
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Operational Date", "OperationalDate"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
    };
    CustomerOverviewTabDetailsComponent.prototype.FillTimeRangeFilterList = function () {
        this.TimeRangeFilterList = [];
        var list = LastFilter_1.LastFilter.MyCRMListDefault();
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[3].lastTitle, "-1"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRange);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }
        this.selectedTimeRangeItem = this.TimeRangeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
        if (this.selectedTimeRangeItem == null)
            this.selectedTimeRangeItem = this.TimeRangeFilterList[0];
        if (this.selectedTimeRangeItem.Index == "-1") {
            var ActiviytFromDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityFromDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.activityFromDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
            var ActiviytToDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityToDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.activityToDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
        }
    };
    CustomerOverviewTabDetailsComponent.prototype.ngOnInit = function () {
        this.FillFilters();
    };
    CustomerOverviewTabDetailsComponent.prototype.BackButtonClicked = function () {
        this.BackCompleted.emit();
    };
    CustomerOverviewTabDetailsComponent.prototype.CountriesTopValueChanged = function (flag) {
        if (flag)
            this.TopCountries = this.TopCountries + 1;
        else
            this.TopCountries = this.TopCountries - 1;
        if (this.TopCountries < 0)
            this.TopCountries = 0;
        this.CommonFiltersCountries();
    };
    CustomerOverviewTabDetailsComponent.prototype.Change = function () {
        console.log("Fired2");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomerOverviewTabDetailsComponent.prototype, "BackCompleted", void 0);
    CustomerOverviewTabDetailsComponent = __decorate([
        core_1.Component({
            selector: 'CustomerOverviewTabDetailsComponent',
            moduleId: module.id,
            templateUrl: './CustomerOverviewTabDetailsComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [])
    ], CustomerOverviewTabDetailsComponent);
    return CustomerOverviewTabDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerOverviewTabDetailsComponent = CustomerOverviewTabDetailsComponent;
//# sourceMappingURL=CustomerOverviewTabDetailsComponent.js.map