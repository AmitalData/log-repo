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
var Tools_1 = require("../../../../Infrastructure/Tools");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var LastFilterClass_1 = require("../../../../Infrastructure/Utilities/LastFilterClass");
var Tools_2 = require("../../../../Infrastructure/Tools");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var CustomerStatisticsTabComponent = /** @class */ (function (_super) {
    __extends(CustomerStatisticsTabComponent, _super);
    function CustomerStatisticsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Customer";
        _this.FilterList = [];
        _this.FilterListActivity = [];
        _this.timeRangeSelectedIndex = 0;
        _this.LineData = [];
        _this.AmLineChartTest = [];
        _this.actualDataList = [];
        _this.flagEmpty = true;
        _this.ActivityStatusOverViewDashboardId = "ActivityStatusOverViewDashboardId_";
        _this.CurrencyCodeLocal = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.lineChartLabels = [];
        _this.lineChartData = [{ data: [], label: '' }];
        _this.CompareComboBoxItems = [];
        _this.barChartLabels = [];
        _this.ActualVsPotentialChart = "ActualVsPotential_ID_";
        _this.legenddiv = "ActualVsPotentialLegends_ID_";
        _this.barChartData = [{ data: [], label: '' }, { data: [], label: '' }];
        _this.NewActualVsPotential = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.filterName_DateType = "DateType";
        _this.filterName_TimeRange = "TimeRange";
        _this.filterControlNameSpace = "Components.Partners.EditTabs.Customer.CustomerOverviewTabComponent";
        _this.NoData = true;
        _this.RankListArr = [];
        _this.EntityId = "";
        _this.EntityName = "";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsShowMessageComplate = false;
        _this.IsShowProgressLoading = false;
        _this.LogoFileHtmlId = Guid_1.Guid.NewRandomString();
        _this.DataContext = _this;
        _this.ComponentRef = null;
        _this.CustomerOverViewTabHide = false;
        _this.isWindowOpened = false;
        _this.ARPayments = 0.0;
        _this.InvoicesDue = 0.0;
        _this.OpenARInvoices = 0.0;
        _this.OpenReceivables = 0.0;
        _this.EntityNotes = "";
        _this.EntityPM = entityArgs.EntityPM;
        _this.EntityId = _this.EntityPM.Id;
        _this.EntityName = "Customer";
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityNotes = _this.EntityPM.Notes;
        _this.ActivityStatusOverViewDashboardId = _this.ActivityStatusOverViewDashboardId + _this.CurrentSession.GetChartId();
        _this.ActualVsPotentialChart = _this.ActualVsPotentialChart + _this.CurrentSession.GetChartId();
        _this.legenddiv = _this.legenddiv + _this.CurrentSession.GetChartId();
        return _this;
    }
    CustomerStatisticsTabComponent.prototype.CompareComboBoxItemsChange = function (item) {
        this.SelectedCompareComboBoxItems = item;
        this.LoadActuals();
    };
    CustomerStatisticsTabComponent.prototype.NewARPayment = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(function (response) {
            var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable("ARPayment"));
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.WindowArgs = { CustomerId: _this.EntityPM.Id };
            logWindow.Title = str;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
            logWindow.WindowClosed.subscribe(function (s) {
                _this.isWindowOpened = false;
                if (s) {
                    _this.LoadMoneyQueries();
                }
            });
        });
    };
    CustomerStatisticsTabComponent.prototype.LoadActuals = function () {
        var _this = this;
        var myMonth = Tools_1.DateTool.GetCurrentDateAsUtc().getMonth() + 1;
        var myYear = Tools_1.DateTool.GetCurrentDateAsUtc().getFullYear();
        if (myMonth == 1) {
            myMonth = 12;
            myYear = myYear - 1;
        }
        else {
            myMonth = myMonth - 1;
        }
        var partnersdomainService = new PartnersDomainService_1.PartnersDomainService();
        partnersdomainService.GetCustomerActualData(this.EntityId, myYear, myMonth).subscribe(function (result) {
            _this.actualDataList = result;
            _this.BuildCompareChartData();
        });
    };
    CustomerStatisticsTabComponent.prototype.ActualVsPotentialChartClick = function () {
        if (BarClick() != null) {
            this.OnActualVsPotentialClick(BarClick());
            ResetItem();
        }
    };
    CustomerStatisticsTabComponent.prototype.OnActualVsPotentialClick = function (e) {
        var _this = this;
        if (e.target.columnIndex == 1) {
            var objectTableName = "Shipment";
            var queryCode = "CustomerShipmentActualData";
            var myProductCode = null;
            if (!Tools_2.AppTool.IsNullOrEmpty(this.NewActualVsPotential[e.target.columnIndex].ProductTypeCode[e.item.index])) {
                myProductCode = this.NewActualVsPotential[e.target.columnIndex].ProductTypeCode[e.item.index];
            }
            var actualDate = Tools_1.DateTool.GetDateParts(new Date(this.NewActualVsPotential[e.target.columnIndex].Year[e.item.index], this.NewActualVsPotential[e.target.columnIndex].Month, 1)).DateObject;
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
            filterAgrs.addAdditionalFilter("ProductCode", myProductCode, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("ActualDataDateYearMonth", this.NewActualVsPotential[e.target.columnIndex].Year[e.item.index], this.NewActualVsPotential[e.target.columnIndex].Month[e.item.index], null, "Equals", true, true, false, "Date");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = "Customer Actual Data";
            listArgs.BackButtonTitle = "Back";
            listArgs.ShowViews = false;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadQueries(); });
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    CustomerStatisticsTabComponent.prototype.ShowOpenReceivables = function () {
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
    CustomerStatisticsTabComponent.prototype.ShowARPayments = function () {
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
    CustomerStatisticsTabComponent.prototype.ShowOpenARInvoices = function () {
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
    CustomerStatisticsTabComponent.prototype.ShowInvoicesDue = function () {
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
    CustomerStatisticsTabComponent.prototype.Month_Year = function () {
        var s = "";
        var myMonth = Tools_1.DateTool.GetCurrentDateTimeAsUtc().getUTCMonth();
        var myYear = Tools_1.DateTool.GetCurrentDateTimeAsUtc().getUTCFullYear();
        if (myMonth == 1) {
            myMonth = 12;
            myYear = myYear - 1;
        }
        else {
            myMonth = myMonth - 1;
        }
        var myFormats = Tools_1.DateTool.GetDateFormats(new Date(myYear, myMonth, 1));
        s += myFormats.MonthNameShort;
        s += "-" + myYear;
        return "Actual (" + s + ")";
    };
    CustomerStatisticsTabComponent.prototype.BuildCompareChartData = function () {
        var _this = this;
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        var compareCode = "S";
        if (this.SelectedCompareComboBoxItems != null) {
            compareCode = this.SelectedCompareComboBoxItems.Code;
        }
        var productTypeListServce = new ProductTypeListService_1.ProductTypeListService();
        productTypeListServce.getAllFromCache().subscribe(function (result) {
            if (result.Result != null) {
                var i = 0;
                var index = 0;
                var Graphs = [];
                var DataProvider = [];
                var objectArray = [];
                var maximum = 0;
                _this.barChartData[0].data = [];
                _this.barChartData[1].data = [];
                _this.barChartLabels = [];
                var max = 0;
                var list = result.Result.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
                ;
                _this.NewActualVsPotential = [];
                list.forEach(function (element) {
                    var productItem = _this.EntityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == element.Code; })[0];
                    var actualItem = _this.actualDataList.filter(function (d) { return d.ProductTypeCode == element.Code; })[0];
                    if (_this.NewActualVsPotential[1] == null) {
                        _this.NewActualVsPotential[1] = { Year: [], label: null, ProductTypeCode: [], Month: [], DateTime: [], BusinessUnitId: [] };
                    }
                    if (actualItem != null) {
                        _this.NewActualVsPotential[1].Year.push(actualItem.Year);
                        _this.NewActualVsPotential[1].ProductTypeCode.push(actualItem.ProductTypeCode);
                        _this.NewActualVsPotential[1].Month.push(actualItem.Month);
                    }
                    else if (productItem != null) {
                        _this.NewActualVsPotential[1].Year.push(null);
                        _this.NewActualVsPotential[1].ProductTypeCode.push(productItem.ProductTypeCode);
                        _this.NewActualVsPotential[1].Month.push(null);
                    }
                    else {
                        _this.NewActualVsPotential[1].Year.push(null);
                        _this.NewActualVsPotential[1].ProductTypeCode.push(element.Code);
                        _this.NewActualVsPotential[1].Month.push(null);
                    }
                    if (i == 0) {
                        Graphs = [{
                                "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                                "fillAlphas": 1,
                                "id": "AmGraph-1" + i,
                                "title": "Potential",
                                "type": "column",
                                "valueField": "col1",
                                "fillColors": ["#c80d05", "#fb5851"],
                                "lineAlpha": 0,
                                "gradientOrientation": "horizontal",
                                "borderAlpha": 0,
                            },
                            {
                                "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                                "fillAlphas": 1,
                                "id": "AmGraph-2" + i,
                                "title": _this.Month_Year(),
                                "type": "column",
                                "lineAlpha": 0,
                                "valueField": "col2",
                                "fillColors": ["#0f7816", "#5ed967"],
                                "gradientOrientation": "horizontal",
                                "borderAlpha": 0,
                                "showHandOnHover": true,
                            }
                        ];
                    }
                    var actualValue = 0;
                    var productValue = 0;
                    switch (compareCode) {
                        case "S":
                            {
                                if (actualItem != null) {
                                    actualValue = actualItem.NumberOfShipments;
                                }
                                if (productItem != null) {
                                    productValue = productItem.PotentialNumberOfShipments;
                                }
                                break;
                            }
                        case "T":
                            {
                                if (actualItem != null) {
                                    actualValue = actualItem.TEU;
                                }
                                if (productItem != null) {
                                    productValue = productItem.PotentialTEU;
                                }
                                break;
                            }
                        case "R":
                            {
                                if (actualItem != null) {
                                    actualValue = actualItem.Revenue;
                                }
                                if (productItem != null) {
                                    productValue = productItem.PotentialRevenue;
                                }
                                break;
                            }
                        case "C":
                            {
                                if (actualItem != null) {
                                    actualValue = actualItem.ChargeableWeight;
                                }
                                if (productItem != null) {
                                    productValue = productItem.PotentialChargeableWeight;
                                }
                                break;
                            }
                    }
                    if (actualValue == null) {
                        actualValue = 0;
                    }
                    if (productValue == null) {
                        productValue = 0;
                    }
                    _this.barChartData[0].label = "C";
                    _this.barChartData[0].data[i] = productValue;
                    _this.barChartData[1].label = "A";
                    _this.barChartData[1].data[i] = actualValue;
                    if (!_this.barChartLabels.includes(element.Name)) {
                        _this.barChartLabels[i] = element.Name;
                    }
                    i++;
                    if (actualValue > max)
                        max = actualValue;
                    if (productValue > max)
                        max = productValue;
                });
                var i = 0;
                _this.barChartLabels.forEach(function (item) {
                    DataProvider[i] = { "category": _this.barChartLabels[i], "col1": _this.barChartData[0].data[i], "col2": _this.barChartData[1].data[i] };
                    i++;
                });
                if (max < 5)
                    max = 5;
                makeAmBarChart(_this.ActualVsPotentialChart, Graphs, DataProvider, max, true, _this.legenddiv);
            }
        });
    };
    CustomerStatisticsTabComponent.prototype.LoadQueries = function () {
        this.LoadActuals();
        this.LoadMoneyQueries();
        this.LoadLineQueries();
    };
    CustomerStatisticsTabComponent.prototype.FillShowFilterList = function () {
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
    Object.defineProperty(CustomerStatisticsTabComponent.prototype, "SelectedDateTypeItem", {
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
    CustomerStatisticsTabComponent.prototype.FillDateTypeFilterList = function () {
        this.DateTypeFilterList = [];
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Operational Date", "OperationalDate"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_DateType);
        if (Tools_2.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CreateDate";
        }
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(function (d) { return d.Index == defaultFilterCode; })[0];
    };
    CustomerStatisticsTabComponent.prototype.FillTimeRangeFilterList = function () {
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
    CustomerStatisticsTabComponent.prototype.FillCompareList = function () {
        var comboBoxItemClass = new ComboBoxIemClass();
        comboBoxItemClass.GetCompareList = new Array();
        var item = new ComboBoxIemClass();
        item.Code = "S";
        item.Name = "Shipments";
        comboBoxItemClass.GetCompareList.push(item);
        item = new ComboBoxIemClass();
        item.Code = "T";
        item.Name = "TEU";
        comboBoxItemClass.GetCompareList.push(item);
        item = new ComboBoxIemClass();
        item.Code = "R";
        item.Name = "Revenue";
        comboBoxItemClass.GetCompareList.push(item);
        item = new ComboBoxIemClass();
        item.Code = "C";
        item.Name = "Chargeable Weight";
        comboBoxItemClass.GetCompareList.push(item);
        this.CompareComboBoxItems = comboBoxItemClass.GetCompareList;
        this.SelectedCompareComboBoxItems = this.CompareComboBoxItems.filter(function (d) { return d.Code == "S"; })[0];
    };
    CustomerStatisticsTabComponent.prototype.FillFilters = function () {
        this.FillDateTypeFilterList();
        this.FillTimeRangeFilterList();
        this.FillShowFilterList();
        this.FillCompareList();
        this.LoadQueries();
    };
    CustomerStatisticsTabComponent.prototype.LoadLineQueries = function () {
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
    CustomerStatisticsTabComponent.prototype.GetCurrentDirectionTransmodeFilterItem = function () {
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
    CustomerStatisticsTabComponent.prototype.FillLine = function (data) {
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
    Object.defineProperty(CustomerStatisticsTabComponent.prototype, "SelectedShowItem", {
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
    Object.defineProperty(CustomerStatisticsTabComponent.prototype, "SelectedTimeRangeItem", {
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
    CustomerStatisticsTabComponent.prototype.ComputeDays = function () {
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
        if (this.SelectedTimeRangeItem.Index == "-1") {
            this.activityFromDate = null;
            this.activityToDate = null;
        }
        return days;
    };
    CustomerStatisticsTabComponent.prototype.FilterSelectedShow = function () {
        this.FillLineQueries();
    };
    Object.defineProperty(CustomerStatisticsTabComponent.prototype, "ActivityFromDate", {
        get: function () { return this.activityFromDate; },
        set: function (value) {
            if (value != this.activityFromDate) {
                this.activityFromDate = value;
                this.selectedTimeRangeItem = this.TimeRangeFilterList[3];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerStatisticsTabComponent.prototype, "ActivityToDate", {
        get: function () { return this.activityToDate; },
        set: function (value) {
            if (value != this.activityToDate) {
                this.activityToDate = value;
                this.selectedTimeRangeItem = this.TimeRangeFilterList[3];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ActivityToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
                this.LoadLineQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerStatisticsTabComponent.prototype.FillLineQueries = function () {
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
    CustomerStatisticsTabComponent.prototype.ngOnInit = function () {
        this.LoadMoneyQueries();
        this.FillFilters();
    };
    CustomerStatisticsTabComponent.prototype.MoreDetails = function () {
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
    CustomerStatisticsTabComponent.prototype.DetailsTabEvent = function () {
        if (this.ComponentRef != null)
            this.ComponentRef.destroy();
        this.CustomerOverViewTabHide = false;
        this.LoadQueries();
    };
    CustomerStatisticsTabComponent.prototype.BusinessClickNew = function (code) {
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
                }
            });
        });
    };
    CustomerStatisticsTabComponent.prototype.LoadMoneyQueries = function () {
        var _this = this;
        var service = new QuoteDomainService_1.QuoteDomainService();
        service.GetCRMMoneyInformation(this.TenantPM.Id, this.EntityPM.Id).subscribe(function (myResult) {
            _this.ARPayments = myResult.Result.ARPayments;
            _this.InvoicesDue = myResult.Result.InvoicesDue;
            _this.OpenARInvoices = myResult.Result.OpenARInvoices;
            _this.OpenReceivables = myResult.Result.OpenReceivables;
        });
    };
    CustomerStatisticsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerStatisticsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerStatisticsTabComponent);
    return CustomerStatisticsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerStatisticsTabComponent = CustomerStatisticsTabComponent;
var ComboBoxIemClass = /** @class */ (function () {
    function ComboBoxIemClass() {
    }
    Object.defineProperty(ComboBoxIemClass.prototype, "Code", {
        get: function () { return this.code; },
        set: function (value) { this.code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ComboBoxIemClass.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) { this.name = value; },
        enumerable: true,
        configurable: true
    });
    return ComboBoxIemClass;
}());
exports.ComboBoxIemClass = ComboBoxIemClass;
//# sourceMappingURL=CustomerStatisticsTabComponent.js.map