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
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var DashboardFilters_1 = require("../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var DashboardDomainService_1 = require("../../Services/DashboardDomainService");
var DirectionTransportFilter_1 = require("../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter");
var GroupByClass_1 = require("../../../Infrastructure/DataContracts/Dashboard/GroupByClass");
var FunctionsCRM_1 = require("../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM");
var List_1 = require("../../../Infrastructure/DataContracts/Dashboard/List");
var DailySpotlightClass_1 = require("../../../Infrastructure/DataContracts/Dashboard/DailySpotlightClass");
var Args_1 = require("../../../Infrastructure/Args");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var DashboardComponent = /** @class */ (function (_super) {
    __extends(DashboardComponent, _super);
    function DashboardComponent(componentfactoryResolver) {
        var _this = _super.call(this) || this;
        _this.componentfactoryResolver = componentfactoryResolver;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.NoShipmentsInActivityStatus = false;
        _this.ActivityStatusDashboardId = "ActivityStatusDashboardId_";
        _this.MoneyInDashboardId = "MoneyInDashboardId_";
        _this.TopFiveDashboardId = "TopFiveDashboardId_";
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.isNotMoreDetails = true;
        _this.barChartLabels = [];
        _this.barChartData = [{ data: [], label: '' }, { data: [], label: '' }];
        _this.SelectedCurrency = "1";
        _this.LineData = [];
        _this.lineChartLabels = [];
        _this.lineChartData = [{ data: [], label: '' }];
        _this.AmLineChartData = [];
        _this.MoneyInLabel = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Shipments_Today_Status = true;
        _this.Shipments_Yesterday_Status = true;
        _this.Shipments_LastWeek_Status = true;
        _this.ARInvoices_Today_Status = true;
        _this.ARInvoices_Yesterday_Status = true;
        _this.ARInvoices_LastWeek_Status = true;
        _this.Quotes_Today_Status = true;
        _this.Quotes_Yesterday_Status = true;
        _this.Quotes_LastWeek_Status = true;
        _this.Customers_Today_Status = true;
        _this.Customers_Yesterday_Status = true;
        _this.Customers_LastWeek_Status = true;
        _this.filterName_DateType = "DateType";
        _this.filterName_TimeRange = "TimeRange";
        _this.filterName_TimeRangeMoney = "TimeRangeMoney";
        _this.filterControlNameSpace = "Workspace.Dashboard";
        _this.pieChartLabels = [];
        _this.pieChartData = [];
        _this.flagEmpty = false;
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.dailySpotLightClass = new DailySpotlightClass_1.DailySpotlightClass();
        _this.dashboarddomainservice = new DashboardDomainService_1.DashboardDomainService();
        _this.ActivityStatusDashboardId = _this.ActivityStatusDashboardId + _this.CurrentSession.GetChartId();
        _this.MoneyInDashboardId = _this.ActivityStatusDashboardId + _this.CurrentSession.GetChartId();
        _this.TopFiveDashboardId = _this.TopFiveDashboardId + _this.CurrentSession.GetChartId();
        _this.TopFiveDashboardLegendId = "TopFiveDashboardLegendId_" + _this.CurrentSession.GetNewId("TopFiveDashboardLegendId");
        return _this;
    }
    DashboardComponent.prototype.ngOnInit = function () {
        this.FillScreen();
    };
    DashboardComponent.prototype.ngOnDestroy = function () {
        if (this.ActivityStatusPage != null) {
            this.ActivityStatusPage.destroy();
        }
    };
    DashboardComponent.prototype.FillScreen = function () {
        this.FillFilters();
        this.LoadSpotlightQueries();
        this.LoadPieQueries();
    };
    DashboardComponent.prototype.LoadSpotlightQueries = function () {
        var _this = this;
        this.dashboarddomainservice.GetDashboardSpotlightCounts(this.TenantPM.Id).subscribe(function (myResult) {
            _this.dailySpotLightClass = myResult;
            if (_this.dailySpotLightClass.Shipments_Today != 0)
                _this.Shipments_Today_Status = false;
            if (_this.dailySpotLightClass.ARInvoices_Today != 0)
                _this.ARInvoices_Today_Status = false;
            if (_this.dailySpotLightClass.Quotes_Today != 0)
                _this.Quotes_Today_Status = false;
            if (_this.dailySpotLightClass.Customers_Today != 0)
                _this.Customers_Today_Status = false;
            if (_this.dailySpotLightClass.Shipments_Yesterday != 0)
                _this.Shipments_Yesterday_Status = false;
            if (_this.dailySpotLightClass.ARInvoices_Yesterday != 0)
                _this.ARInvoices_Yesterday_Status = false;
            if (_this.dailySpotLightClass.Quotes_Yesterday != 0)
                _this.Quotes_Yesterday_Status = false;
            if (_this.dailySpotLightClass.Customers_Yesterday != 0)
                _this.Customers_Yesterday_Status = false;
            if (_this.dailySpotLightClass.Shipments_LastWeek != 0)
                _this.Shipments_LastWeek_Status = false;
            if (_this.dailySpotLightClass.ARInvoices_LastWeek != 0)
                _this.ARInvoices_LastWeek_Status = false;
            if (_this.dailySpotLightClass.Quotes_LastWeek != 0)
                _this.Quotes_LastWeek_Status = false;
            if (_this.dailySpotLightClass.Customers_LastWeek != 0)
                _this.Customers_LastWeek_Status = false;
        });
    };
    DashboardComponent.prototype.LoadPieQueries = function () {
        var _this = this;
        this.dashboarddomainservice.GetDebrotExposure(this.TenantPM.Id, parseInt(this.SelectedCurrency)).subscribe(function (myResult) {
            _this.PieData = myResult;
            _this.FillPie();
        });
    };
    DashboardComponent.prototype.FillPie = function () {
        var _this = this;
        var fullData = [];
        this.pieChartLabels = [];
        this.pieChartData = [];
        this.PieData.forEach(function (element) {
            var amount = Tools_1.AppTool.Round(element.Amount, 3);
            fullData.push({ label: element.DebtorName, data: amount });
            _this.pieChartLabels.push(element.DebtorName);
            _this.pieChartData.push(element.Amount);
        });
        if (this.CurrentTop10DebtorsChart != null) {
            this.CurrentTop10DebtorsChart.clear();
            this.CurrentTop10DebtorsChart = null;
        }
        this.CurrentTop10DebtorsChart = makePieChart(this.TopFiveDashboardId, fullData, false, true, this.TopFiveDashboardLegendId);
    };
    Object.defineProperty(DashboardComponent.prototype, "ActivityFromDate", {
        get: function () { return this.activityFromDate; },
        set: function (value) {
            if (value != this.activityFromDate) {
                this.activityFromDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                //  this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashboardComponent.prototype, "ActivityToDate", {
        get: function () { return this.activityToDate; },
        set: function (value) {
            if (value != this.activityToDate) {
                this.activityToDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[4];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                // this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashboardComponent.prototype, "MoneyFromDate", {
        get: function () { return this.moneyFromDate; },
        set: function (value) {
            if (value != this.moneyFromDate) {
                this.moneyFromDate = value;
                this.SelectedTimeRangeItem2 = this.TimeRangeFilterList2[4];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDateMoney", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashboardComponent.prototype, "MoneyToDate", {
        get: function () { return this.moneyToDate; },
        set: function (value) {
            if (value != this.moneyToDate) {
                this.moneyToDate = value;
                this.SelectedTimeRangeItem2 = this.TimeRangeFilterList2[4];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDateMoney", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashboardComponent.prototype, "SelectedTimeRangeItem", {
        get: function () { return this.selectedTimeRangeItem; },
        set: function (value) {
            if (this.selectedTimeRangeItem != value) {
                this.selectedTimeRangeItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRange, (value == null ? null : value.Index));
                if (value.Index == "-1") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityFromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityToDate)));
                }
                //  this.LoadLineQueries();
            }
            this.LoadLineQueries();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashboardComponent.prototype, "SelectedTimeRangeItem2", {
        get: function () { return this.selectedTimeRangeItem2; },
        set: function (value) {
            if (this.selectedTimeRangeItem2 != value) {
                this.selectedTimeRangeItem2 = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRangeMoney, (value == null ? null : value.Index));
                if (value.Index == "-1") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDateMoney", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.MoneyFromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDateMoney", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.MoneyToDate)));
                }
            }
            this.LoadBarQueries();
        },
        enumerable: true,
        configurable: true
    });
    DashboardComponent.prototype.FilterSelectedShow = function () {
        this.FillLineQueries();
    };
    Object.defineProperty(DashboardComponent.prototype, "SelectedShowItem", {
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
    DashboardComponent.prototype.LoadBarQueries = function () {
        var _this = this;
        if (this.SelectedTimeRangeItem2.Index == "-1") {
            if (this.MoneyFromDate != null && this.MoneyToDate != null) {
                this.dashboarddomainservice.GetMoneyStatusForTenantCustom("CreateDate", this.MoneyToDate, this.MoneyFromDate).subscribe(function (myResult) {
                    _this.BarData = myResult;
                    if (_this.BarData.length != 0) {
                        var list = [];
                        _this.BarData.forEach(function (item) {
                            var newItem = new GroupByClass_1.GroupByClass();
                            newItem.XField = item.DateRange;
                            newItem.DataType = item.DataType;
                            newItem.YField = item.TotalAmount;
                            list.push(newItem);
                        });
                        _this.BarData = list;
                        _this.FillBars();
                    }
                });
            }
        }
        else {
            var days = this.ComputeDays("money");
            this.dashboarddomainservice.GetMoneyStatusForTenant("CreateDate", 0, days, this.TenantPM.Id, +this.SelectedTimeRangeItem2.Index, 1).subscribe(function (myResult) {
                _this.BarData = myResult;
                var groupedData = [];
                var barData2 = [];
                _this.BarData.sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                _this.BarData.forEach(function (item) {
                    var existsedItem = groupedData.filter(function (f) { return f.DateRange == item.DateRange && f.DataType == item.DataType; })[0];
                    if (existsedItem == null) {
                        existsedItem = new GroupByClass_1.GroupByClass();
                        existsedItem.XField = item.DateRange;
                        existsedItem.DataType = item.DataType;
                        if (item.TotalAmount == null)
                            existsedItem.YField = 0;
                        else
                            existsedItem.YField = parseInt(item.TotalAmount + "");
                        existsedItem.dateRange = item.DateRange;
                        groupedData.push(existsedItem);
                    }
                    else {
                        if (item.TotalAmount == null)
                            existsedItem.YField += 0;
                        else
                            existsedItem.YField += parseInt(item.TotalAmount + "");
                    }
                });
                _this.BarData = groupedData;
                _this.FillBars();
            });
        }
    };
    DashboardComponent.prototype.DailySpotLightClick = function (Code) {
        var _this = this;
        var myQueryCode = "";
        var displayName = "";
        var myTableName = "";
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        switch (Code) {
            case "QT_TD":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Today Quotes";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "string");
                    break;
                }
            case "QT_YS":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Yesterday Quotes";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "string");
                    break;
                }
            case "QT_LW":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Last Week Quotes";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, null, null, "Equals", true, false, true, "string");
                    break;
                }
            case "SH_TD":
                {
                    myTableName = "Shipment";
                    myQueryCode = "Shipments";
                    displayName = "Today Shipments";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Shipments Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, "", "", "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("ShipmentLevelCode", "C", "", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "SH_YS":
                {
                    myTableName = "Shipment";
                    myQueryCode = "Shipments";
                    displayName = "Yesterday Shipments";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Shipments Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("ShipmentLevelCode", "C", "", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "SH_LW":
                {
                    myTableName = "Shipment";
                    myQueryCode = "Shipments";
                    displayName = "Last Week Shipments";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Shipments Zoom");
                    this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("ShipmentLevelCode", "C", "", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "AR_TD":
                {
                    myTableName = "ARInvoice";
                    myQueryCode = "All Invoices";
                    displayName = "Today Invoices";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Invoices Zoom");
                    this.filterAgrs.addAdditionalFilter("StatusCode", "LL", "VD", null, "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "AR_YS":
                {
                    myTableName = "ARInvoice";
                    myQueryCode = "All Invoices";
                    displayName = "Yesterday Invoices";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Invoices Zoom");
                    this.filterAgrs.addAdditionalFilter("StatusCode", "LL", "VD", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "AR_LW":
                {
                    myTableName = "ARInvoice";
                    myQueryCode = "All Invoices";
                    displayName = "Last Week Invoices";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Invoices Zoom");
                    this.filterAgrs.addAdditionalFilter("StatusCode", "LL", "VD", "", "NotEqual", false, false, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "CS_TD":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Today Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "CS_YS":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Yesterday Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
            case "CS_LW":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Last Week Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "Boolean");
                    this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
                    this.filterAgrs.addAdditionalFilter("DailySpotlightFilter", Code, "", "", "Equals", true, false, false, "string");
                    break;
                }
        }
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "Dashboard";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadData(); });
            });
        });
    };
    DashboardComponent.prototype.LoadData = function () {
        this.LoadPieQueries();
        this.LoadSpotlightQueries();
        this.LoadLineQueries();
        this.LoadBarQueries();
    };
    DashboardComponent.prototype.LoadLineQueries = function () {
        var _this = this;
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                this.dashboarddomainservice.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", null, null).subscribe(function (myResult) {
                    _this.LineData = myResult;
                    _this.FillLineQueries();
                });
            }
            else {
                this.FillLineQueries();
            }
        }
        else {
            var days = this.ComputeDays("activity");
            this.dashboarddomainservice.GetActivityStatus(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id).subscribe(function (myResult) {
                _this.LineData = myResult;
                _this.FillLineQueries();
            });
        }
    };
    DashboardComponent.prototype.ComputeDays = function (FilterAction) {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (FilterAction == "activity") {
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
        }
        else {
            this.moneyFromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
            if (this.SelectedTimeRangeItem2.Index == "0") {
                days = -7;
                Todate.setDate(Todate.getDate() - 6);
                this.moneyToDate = Todate;
            }
            else if (this.SelectedTimeRangeItem2.Index == "1") {
                days = -30;
                Todate.setMonth(Todate.getMonth() - 1);
                this.moneyToDate = Todate;
            }
            else if (this.SelectedTimeRangeItem2.Index == "2") {
                days = -90;
                Todate.setMonth(Todate.getMonth() - 3);
                this.moneyToDate = Todate;
            }
            else if (this.SelectedTimeRangeItem2.Index == "3") {
                days = -365;
                Todate.setMonth(Todate.getMonth() - 12);
                this.moneyToDate = Todate;
            }
        }
        return days;
    };
    DashboardComponent.prototype.FillLineQueries = function () {
        var _this = this;
        var showIndex = parseInt(this.SelectedShowItem.Index);
        var timeIndex = parseInt(this.SelectedTimeRangeItem.Index);
        var byMonthData = this.LineData;
        var dtf = this.GetCurrentDirectionTransmodeFilterItem();
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
            this.FillLine(monthQuartersList);
        }
        else {
            var FilteredList = new List_1.List();
            this.LineData.items != null ? this.LineData.items.forEach(function (item) {
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
            this.FillLine(FilteredList);
        }
        this.flagEmpty = true;
        this.lineChartData[0].data.forEach(function (p) {
            if (p != "0")
                _this.flagEmpty = false;
        });
        if (!this.flagEmpty) {
            makeAMLineChart(this.ActivityStatusDashboardId, this.AmLineChartData);
            this.NoShipmentsInActivityStatus = false;
        }
        else {
            try {
                var elm = document.getElementById(this.ActivityStatusDashboardId);
                elm.innerHTML = "";
            }
            catch (exce) { }
            this.NoShipmentsInActivityStatus = true;
        }
    };
    DashboardComponent.prototype.GetCurrentDirectionTransmodeFilterItem = function () {
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
    DashboardComponent.prototype.FillLine = function (data) {
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
        this.lineChartLabels = [];
        data.getAll().forEach(function (element) {
            var myResult;
            var myStringNumber = Tools_1.AppTool.Round(element.YField, 2) + "";
            var myStringNumber1 = myStringNumber.split('.')[0];
            var myStringNumber2 = myStringNumber.split('.')[1];
            myResult = myStringNumber1.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            if (!Tools_1.AppTool.IsNullOrEmpty(myStringNumber2)) {
                myResult += "." + myStringNumber2;
            }
            _this.lineChartData[0].data[index] = Tools_1.AppTool.Round(element.YField, 2) + "";
            _this.lineChartLabels.push(element.XField);
            index++;
            _this.AmLineChartData.push({
                date: element.XField,
                visits: Tools_1.AppTool.Round(element.YField, 2) + ""
            });
        });
    };
    DashboardComponent.prototype.FillBars = function () {
        var _this = this;
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        this.barChartData[0].data = [];
        this.barChartData[1].data = [];
        this.barChartLabels = [];
        var max = 0;
        var even = 0;
        this.BarData.forEach(function (element) {
            if (i == 0) {
                Graphs = [{
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-1" + i,
                        "title": "Invoices",
                        "type": "column",
                        "valueField": "col1",
                        "fillColors": ["#d29127", "#dfb267"],
                        "lineAlpha": 0,
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    },
                    {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": "Payments",
                        "type": "column",
                        "lineAlpha": 0,
                        "valueField": "col2",
                        "fillColors": ["#1d758e", "#2186a3"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    }
                ];
            }
            if (element.DataType == 'Invoices') {
                _this.barChartData[0].label = element.DataType;
                _this.barChartData[0].data[i] = element.YField;
            }
            else if (element.DataType == 'Payments') {
                _this.barChartData[1].label = element.DataType;
                _this.barChartData[1].data[i] = element.YField;
            }
            if (!_this.barChartLabels.includes(element.XField)) {
                _this.barChartLabels[i] = element.XField;
            }
            even++;
            if (even % 2 == 0)
                i++;
            if (element.YField > max)
                max = element.YField;
        });
        var i = 0;
        this.barChartLabels.forEach(function (item) {
            DataProvider[i] = { "category": _this.barChartLabels[i], "col1": _this.barChartData[0].data[i], "col2": _this.barChartData[1].data[i] };
            i++;
        });
        makeAmBarChart(this.MoneyInDashboardId, Graphs, DataProvider, max);
    };
    DashboardComponent.prototype.change = function (cmpRef) {
        cmpRef.destroy();
        this.isNotMoreDetails = true;
        this.FillScreen();
    };
    DashboardComponent.prototype.OpenDashBoard = function () {
        var _this = this;
        this.isNotMoreDetails = false;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Dashboard/Components/Workspace/ActivityStatusDetailsComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.logoff.subscribe(function ($event) { return _this.change(cmpRef); });
            _this.ActivityStatusPage = cmpRef;
        });
    };
    DashboardComponent.prototype.FillFilters = function () {
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.FillOtherElements();
        this.FillDateTypeFilterList();
        this.LoadData();
    };
    DashboardComponent.prototype.FillOtherElements = function () {
        this.MoneyInLabel = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
    };
    DashboardComponent.prototype.FillDateTypeFilterList = function () {
        this.DateTypeFilterList = [];
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Operational Date", "OperationalDate"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
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
        if (this.selectedTimeRangeItem2.Index == "-1") {
            var moneyFromDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityFromDateMoney");
            if (!Tools_1.AppTool.IsNullOrEmpty(moneyFromDate)) {
                var moneyDate = new Date();
                var moneyFromDateString = moneyFromDate.split(':');
                moneyDate.setFullYear(moneyFromDateString[0], moneyFromDateString[1] - 1, moneyFromDateString[2]);
                this.moneyFromDate = Tools_1.DateTool.GetDateParts(moneyDate).DateObject;
            }
            var moneyToDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityToDateMoney");
            if (!Tools_1.AppTool.IsNullOrEmpty(moneyToDate)) {
                var moneyDate = new Date();
                var moneyToDateString = moneyToDate.split(':');
                moneyDate.setFullYear(moneyToDateString[0], moneyToDateString[1] - 1, moneyToDateString[2]);
                this.moneyToDate = Tools_1.DateTool.GetDateParts(moneyDate).DateObject;
            }
        }
    };
    Object.defineProperty(DashboardComponent.prototype, "SelectedDateTypeItem", {
        get: function () { return this.selectedDateTypeItem; },
        set: function (value) {
            if (this.selectedDateTypeItem != value) {
                this.selectedDateTypeItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_DateType, (value == null ? null : value.Index));
                this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    DashboardComponent.prototype.FillTimeRangeFilterList = function () {
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
        var list2 = LastFilter_1.LastFilter.ActivitymyList();
        this.TimeRangeFilterList2 = [];
        this.TimeRangeFilterList2.push(new DashboardFilters_1.DashBoardFilters(list2[0].lastTitle, "0"));
        this.TimeRangeFilterList2.push(new DashboardFilters_1.DashBoardFilters(list2[1].lastTitle, "1"));
        this.TimeRangeFilterList2.push(new DashboardFilters_1.DashBoardFilters(list2[2].lastTitle, "2"));
        this.TimeRangeFilterList2.push(new DashboardFilters_1.DashBoardFilters(list2[3].lastTitle, "3"));
        this.TimeRangeFilterList2.push(new DashboardFilters_1.DashBoardFilters(list2[4].lastTitle, "-1"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRangeMoney);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }
        this.selectedTimeRangeItem2 = this.TimeRangeFilterList2.filter(function (d) { return d.Index == defaultFilterCode; })[0];
    };
    DashboardComponent.prototype.FillShowFilterList = function () {
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
    DashboardComponent.prototype.ChangeCurrency = function (code) {
        if (code == this.LocalCurrencyCode)
            code = "1";
        else
            code = "2";
        if (code != this.SelectedCurrency) {
            this.SelectedCurrency = code;
            this.LoadPieQueries();
        }
    };
    DashboardComponent = __decorate([
        core_1.Component({
            selector: 'DashBoard',
            moduleId: module.id,
            templateUrl: './DashBoardComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [core_1.ComponentFactoryResolver])
    ], DashboardComponent);
    return DashboardComponent;
}(BaseComponent_1.BaseComponent));
exports.DashboardComponent = DashboardComponent;
//# sourceMappingURL=DashboardComponent.js.map