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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DashboardFilters_1 = require("../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var List_1 = require("../../../../Infrastructure/DataContracts/Dashboard/List");
var FunctionsCRM_1 = require("../../../../Infrastructure/DataContracts/Dashboard/FunctionsCRM");
var DirectionTransportFilter_1 = require("../../../../Infrastructure/DataContracts/Dashboard/DirectionTransportFilter");
var GroupByClass_1 = require("../../../../Infrastructure/DataContracts/Dashboard/GroupByClass");
var LastFilter_1 = require("../../../../Infrastructure/Utilities/LastFilter");
var ChartsService_1 = require("../../../../Infrastructure/Services/ChartsService");
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var RankListService_1 = require("../../../../Common/Services/StandardLists/RankListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LastFilterClass_1 = require("../../../../Infrastructure/Utilities/LastFilterClass");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var Tools_2 = require("../../../../Infrastructure/Tools");
var CustomerOverviewTabComponent = /** @class */ (function (_super) {
    __extends(CustomerOverviewTabComponent, _super);
    function CustomerOverviewTabComponent(entityArgs, _imageLibraryService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._imageLibraryService = _imageLibraryService;
        _this.ObjectTableName = "Customer";
        _this.LineData = [];
        _this.AmLineChartTest = [];
        _this.RankValue = [];
        _this.NoData = true;
        _this.ActivityStatusOverViewDashboardId = "ActivityStatusOverViewDashboardId_";
        _this.CurrencyCodeLocal = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.lineChartLabels = [];
        _this.lineChartData = [{ data: [], label: '' }];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllQuotes = 0;
        _this.AllShipments = 0;
        _this.OpenQuotes = 0;
        _this.OpenShipments = 0;
        _this.ARPayments = 0.0;
        _this.InvoicesDue = 0.0;
        _this.OpenARInvoices = 0.0;
        _this.OpenReceivables = 0.0;
        _this.RankSourceText1 = "./Images/Icons/StarGray.png";
        _this.RankSourceText2 = "./Images/Icons/StarGray.png";
        _this.RankSourceText3 = "./Images/Icons/StarGray.png";
        _this.filterName_DateType = "DateType";
        _this.filterName_TimeRange = "TimeRange";
        _this.filterControlNameSpace = "Components.Partners.EditTabs.Customer.CustomerOverviewTabComponent";
        _this.RankListArr = [];
        _this.ImageId = "";
        _this.EntityId = "";
        _this.EntityName = "";
        _this.LogoInput = Guid_1.Guid.NewRandomString();
        _this.IsShowMessageComplate = false;
        _this.IsShowProgressLoading = false;
        _this.LogoFileHtmlId = Guid_1.Guid.NewRandomString();
        _this.ComponentRef = null;
        _this.CustomerOverViewTabHide = false;
        _this.isWindowOpened = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ImageId = _this.EntityPM.ImageDetailId;
        _this.EntityId = _this.EntityPM.Id;
        _this.EntityName = "Customer";
        _this.StartWorkingDate = _this.EntityPM.StartWorkingDate;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.ActivityStatusOverViewDashboardId = _this.ActivityStatusOverViewDashboardId + _this.CurrentSession.GetChartId();
        _this.rankListService = new RankListService_1.RankListService();
        _this.rankListService.getAllFromCache().subscribe(function (result) {
            _this.RankListArr = result.Result;
        });
        return _this;
    }
    CustomerOverviewTabComponent.prototype.ngOnInit = function () {
        this.LoadQuriesCount();
        this.LoadMoneyQueries();
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
        this.FillFilters();
    };
    CustomerOverviewTabComponent.prototype.LoadQuriesCount = function () {
        var _this = this;
        var service = new QuoteDomainService_1.QuoteDomainService();
        service.GetDataCountsForCRM(this.TenantPM.Id, this.EntityPM.Id).subscribe(function (myResult) {
            _this.AllQuotes = myResult.Result.AllQuotes;
            _this.AllShipments = myResult.Result.AllShipments;
            _this.OpenQuotes = myResult.Result.OpenQuotes;
            _this.OpenShipments = myResult.Result.OpenShipments;
        });
    };
    CustomerOverviewTabComponent.prototype.LoadMoneyQueries = function () {
        var _this = this;
        var service = new QuoteDomainService_1.QuoteDomainService();
        service.GetCRMMoneyInformation(this.TenantPM.Id, this.EntityPM.Id).subscribe(function (myResult) {
            _this.ARPayments = myResult.Result.ARPayments;
            _this.InvoicesDue = myResult.Result.InvoicesDue;
            _this.OpenARInvoices = myResult.Result.OpenARInvoices;
            _this.OpenReceivables = myResult.Result.OpenReceivables;
        });
    };
    CustomerOverviewTabComponent.prototype.RankSource1 = function (rank) {
        if (rank === void 0) { rank = null; }
        if (rank != null) {
            this.RankSourceText1 = "./Images/Icons/StarOrange.png";
        }
        else {
            var myResult = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    };
    CustomerOverviewTabComponent.prototype.RankSource2 = function (rank) {
        if (rank === void 0) { rank = null; }
        if (rank != null)
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        else {
            var myResult = "./Images/Icons/StarOrange.png";
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    };
    CustomerOverviewTabComponent.prototype.RankSource3 = function (rank) {
        if (rank === void 0) { rank = null; }
        if (rank != null) {
            this.RankSourceText3 = "./Images/Icons/StarOrange.png";
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        }
        else {
            var RankCode = this.EntityPM.RankCode;
            switch (RankCode) {
                case "1": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "2": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "3": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                    break;
                }
                default: {
                    this.RankSourceText1 = "./Images/Icons/StarGray.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    };
    CustomerOverviewTabComponent.prototype.FillFilters = function () {
        this.FillDateTypeFilterList();
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.LoadLineQueries();
    };
    CustomerOverviewTabComponent.prototype.FillDateTypeFilterList = function () {
        this.DateTypeFilterList = [];
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Operational Date", "OperationalDate"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (Tools_2.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
    };
    CustomerOverviewTabComponent.prototype.FillTimeRangeFilterList = function () {
        this.TimeRangeFilterList = [];
        var list = LastFilter_1.LastFilter.MyCRMListDefault();
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashboardFilters_1.DashBoardFilters(list[3].lastTitle, "-1"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TimeRange);
        if (Tools_2.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "0";
        }
        this.selectedTimeRangeItem = this.TimeRangeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
        if (this.selectedTimeRangeItem == null)
            this.selectedTimeRangeItem = this.TimeRangeFilterList[0];
        if (this.selectedTimeRangeItem.Index == "-1") {
            var ActiviytFromDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityFromDate");
            if (!Tools_2.AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.activityFromDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
            var ActiviytToDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ActivityToDate");
            if (!Tools_2.AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.activityToDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
        }
    };
    CustomerOverviewTabComponent.prototype.FillShowFilterList = function () {
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
    Object.defineProperty(CustomerOverviewTabComponent.prototype, "SelectedDateTypeItem", {
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
    Object.defineProperty(CustomerOverviewTabComponent.prototype, "SelectedTimeRangeItem", {
        get: function () { return this.selectedTimeRangeItem; },
        set: function (value) {
            if (this.selectedTimeRangeItem != value) {
                this.selectedTimeRangeItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TimeRange, (value == null ? null : value.Index));
                if (value.Index == "-1") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityFromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ActivityToDate)));
                }
                this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabComponent.prototype, "SelectedShowItem", {
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
    CustomerOverviewTabComponent.prototype.ComputeDays = function () {
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
        //else if (this.SelectedTimeRangeItem.Index == "2") {
        //    days = -90;
        //    Todate.setMonth(-3);
        //    this.activityToDate = Todate;
        //}
        else if (this.SelectedTimeRangeItem.Index == "2") {
            days = -1095;
            Todate.setMonth(Todate.getMonth() - 36);
            this.activityToDate = Todate;
        }
        else if (this.SelectedTimeRangeItem.Index == "-1") {
            this.activityFromDate = null;
            this.activityToDate = null;
        }
        return days;
    };
    CustomerOverviewTabComponent.prototype.FilterSelectedShow = function () {
        this.FillLineQueries();
    };
    Object.defineProperty(CustomerOverviewTabComponent.prototype, "ActivityFromDate", {
        get: function () { return this.activityFromDate; },
        set: function (value) {
            if (value != this.activityFromDate) {
                this.activityFromDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerOverviewTabComponent.prototype, "ActivityToDate", {
        get: function () { return this.activityToDate; },
        set: function (value) {
            if (value != this.activityToDate) {
                this.activityToDate = value;
                this.SelectedTimeRangeItem = this.TimeRangeFilterList[3];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerOverviewTabComponent.prototype.LoadLineQueries = function () {
        var _this = this;
        var service = new ChartsService_1.ChartsService();
        if (this.SelectedTimeRangeItem.Index == "-1") {
            if (this.ActivityFromDate != null && this.ActivityToDate != null) {
                service.GetActivityStatusByType(this.SelectedDateTypeItem.Index, this.ActivityToDate, this.ActivityFromDate, this.TenantPM.Id + "", this.EntityPM.Id).subscribe(function (myResult) {
                    _this.LineData = myResult;
                    _this.FillLineQueries();
                });
            }
        }
        else {
            var days = this.ComputeDays();
            service.GetActivityStatus(this.SelectedDateTypeItem.Index, 0, days, this.TenantPM.Id, this.EntityPM.Id).subscribe(function (myResult) {
                _this.LineData = myResult;
                _this.FillLineQueries();
            });
        }
    };
    CustomerOverviewTabComponent.prototype.FillLineQueries = function () {
        var _this = this;
        var showIndex = parseInt(this.SelectedShowItem.Index);
        var timeIndex = parseInt(this.SelectedTimeRangeItem.Index);
        var byMonthData = this.LineData;
        var dtf = this.GetCurrentDirectionTransmodeFilterItem();
        var directionFilteredList = FunctionsCRM_1.FunctionsCRM.getDirectionFilteredList(dtf, byMonthData);
        if (timeIndex == -1) {
            if (Tools_1.DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate) <= 90) {
                timeIndex = 1;
            }
            else if (Tools_1.DateTool.GetDaysBetweenDates(this.ActivityFromDate, this.ActivityToDate) <= 365) {
                timeIndex = 2;
            }
            else {
                timeIndex = 3;
            }
        }
        if (this.SelectedTimeRangeItem.Index != "-1") {
            var measurmentFilteredList = FunctionsCRM_1.FunctionsCRM.getMeasurmentFilterList(showIndex, directionFilteredList, timeIndex, this.EntityPM.Id);
            var monthQuartersList = FunctionsCRM_1.FunctionsCRM.getmonthQuartersList(timeIndex, measurmentFilteredList, this.EntityPM.Id);
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
        this.NoData = true;
        this.lineChartData[0].data.forEach(function (p) {
            if (p != "0")
                _this.NoData = false;
        });
        try {
            var els = document.getElementById(this.ActivityStatusOverViewDashboardId);
            if (!this.NoData) {
                makeAMLineChart(this.ActivityStatusOverViewDashboardId, this.AmLineChartTest, 0.6);
                els.hidden = false;
            }
            else {
                els.hidden = true;
            }
        }
        catch (Ex) { }
    };
    CustomerOverviewTabComponent.prototype.FillLine = function (data) {
        var _this = this;
        var index = 0;
        this.AmLineChartTest = [];
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
            _this.lineChartData[0].data[index] = element.YField + "";
            _this.lineChartLabels.push(element.XField);
            index++;
            _this.AmLineChartTest.push({
                date: element.XField,
                visits: element.YField + ""
            });
        });
    };
    CustomerOverviewTabComponent.prototype.GetCurrentDirectionTransmodeFilterItem = function () {
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
    CustomerOverviewTabComponent.prototype.ImageUploadedCompleted = function (imageId) {
        this.ImageId = imageId;
        this.EntityPM.ImageDetailId = imageId;
    };
    Object.defineProperty(CustomerOverviewTabComponent.prototype, "StartWorkingDate", {
        get: function () { return this.EntityPM.StartWorkingDate; },
        set: function (value) {
            if (this.EntityPM.StartWorkingDate != value)
                this.EntityPM.StartWorkingDate = value;
        },
        enumerable: true,
        configurable: true
    });
    CustomerOverviewTabComponent.prototype.ChangeRank = function (code) {
        var filteredData = this.RankListArr.filter(function (a) { return a.Code === code && a.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
        this.EntityPM.RankCode = filteredData.Code;
        this.EntityPM.RankName = filteredData.Name;
        this.EntityPM.RankId = filteredData.Id;
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
    };
    CustomerOverviewTabComponent.prototype.MoreDetails = function () {
        var _this = this;
        this.CustomerOverViewTabHide = true;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./CommonModules/CommonCustomer/Components/EditTabs/CustomerOverviewTabDetailsComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Customer = _this.EntityPM;
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.DetailsTabEvent(); });
            _this.ComponentRef = cmpRef;
        });
    };
    CustomerOverviewTabComponent.prototype.DetailsTabEvent = function () {
        if (this.ComponentRef != null)
            this.ComponentRef.destroy();
        this.CustomerOverViewTabHide = false;
        this.LoadData();
    };
    CustomerOverviewTabComponent.prototype.LoadData = function () {
        this.LoadLineQueries();
        this.LoadMoneyQueries();
        this.LoadQuriesCount();
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
    };
    CustomerOverviewTabComponent.prototype.BusinessClickNew = function (code) {
        var _this = this;
        var path = "";
        var windowTitle = "";
        switch (code + "") {
            case "Quote": {
                path = './Quote/ComponentsNewEntity/NewQuoteComponent';
                windowTitle = "New Quote";
                break;
            }
            case "Shipment": {
                path = './Shipment/Components/NewShipment/NewShipmentComponent';
                windowTitle = "New Shipment";
                break;
            }
            default: break;
        }
        this._entityResourceService.getEntityResourceByTableName(code, 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe(function (s) {
                _this.isWindowOpened = false;
                if (s) {
                    _this.LoadQuriesCount();
                }
            });
        });
    };
    CustomerOverviewTabComponent.prototype.BusinessClick = function (code) {
        var _this = this;
        var myQueryCode = "";
        var displayName = "";
        var myTableName = "";
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        switch (code + "") {
            case "OpenQuotes": {
                myTableName = "Quote";
                myQueryCode = "Open Quotes";
                displayName = "Open Quotes";
                break;
            }
            case "AllQuotes": {
                myTableName = "Quote";
                myQueryCode = "All Quotes";
                displayName = "All Quotes";
                break;
            }
            case "OpenShipments": {
                myTableName = "Shipment";
                myQueryCode = "Shipments";
                displayName = "Open Shipments";
                break;
            }
            case "AllShipments": {
                myTableName = "Shipment";
                myQueryCode = "All Shipments";
                displayName = "All Shipments";
                break;
            }
            default: break;
        }
        if (myTableName != "") {
            this.filterAgrs.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "Customer";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, this.EntityPM.Tenant).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadQuriesCount(); });
                });
            });
        }
    };
    CustomerOverviewTabComponent.prototype.ShowOpenReceivables = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Shipment'; })[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("OpenReceivablesInLocalCurrency", 0.0, null, null, "LargerThan", false, false, false, "number");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "Shipments";
            listArgs.ObjectTableName = "Shipment";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                });
            });
        }
    };
    CustomerOverviewTabComponent.prototype.ShowARPayments = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'ARPayment'; })[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("BillToId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("StatusCode", "AD,PR", null, null, "InList", false, false, false, "string");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Payments";
            listArgs.ObjectTableName = "ARPayment";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                });
            });
        }
    };
    CustomerOverviewTabComponent.prototype.ShowOpenARInvoices = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'ARInvoice'; })[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("BillToId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsAutoCredit", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("StatusCode", "AD,PP,PR", null, null, "InList", false, false, false, "string");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Invoices";
            listArgs.ObjectTableName = "ARInvoice";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                });
            });
        }
    };
    CustomerOverviewTabComponent.prototype.ShowInvoicesDue = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'ARInvoice'; })[0];
        if (table != null) {
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            this.filterAgrs.addAdditionalFilter("BillToId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("DueDate", Tools_1.DateTool.GetCurrentDateTimeAsUtc(), null, null, "LessThan", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsAutoCredit", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("StatusCode", "AD,PP,PR", null, null, "InList", false, false, false, "string");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = "All Invoices";
            listArgs.ObjectTableName = "ARInvoice";
            listArgs.DisplayTitle = listArgs.QueryCode;
            listArgs.BackButtonTitle = "Back";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    //  cmpRef.instance.BackCompleted.subscribe(($event: any) => this.Load());
                });
            });
        }
    };
    CustomerOverviewTabComponent.prototype.NewARPayment = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(function (response) {
            var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable("ARPayment"));
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = { CustomerId: _this.EntityPM.Id };
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = str;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
            logWindow.WindowClosed.subscribe(function (s) {
                _this.isWindowOpened = false;
                if (s) {
                    _this.LoadQuriesCount();
                }
            });
        });
    };
    CustomerOverviewTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerOverviewTabComponent.html',
            providers: [ImageLibraryService_1.ImageLibraryService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, ImageLibraryService_1.ImageLibraryService])
    ], CustomerOverviewTabComponent);
    return CustomerOverviewTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerOverviewTabComponent = CustomerOverviewTabComponent;
//# sourceMappingURL=CustomerOverviewTabComponent.js.map