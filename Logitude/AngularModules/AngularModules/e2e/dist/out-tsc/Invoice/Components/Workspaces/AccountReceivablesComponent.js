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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../Infrastructure/Args");
var InvoiceDomainService_1 = require("../../../Invoice/Services/InvoiceDomainService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CurrencyListService_1 = require("../../../Common/Services/StandardLists/CurrencyListService");
var DashboardFilters_1 = require("../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var ChartsService_1 = require("../../../Infrastructure/Services/ChartsService");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var AccountReceivablesComponent = /** @class */ (function () {
    function AccountReceivablesComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.DebtorsObsList = [];
        this.FilterList = [];
        this.MoneyInLocalCurrency = "(" + SessionLocator_1.SessionLocator.LocalCurrencyCode + ")";
        this.myViewsQueryVisibility = false;
        this.consolidationButtonVisibility = false;
        this.ReceivablesChartId = "ReceivablesChartId";
        this.ReceivablesChartMoneyInOutId = "ReceivablesChartMoneyInOutId";
        this.isRTL = false;
        this.ARInvoicesSATFailedVisibility = false;
        this.ARPaymentsSATFailedVisibility = false;
        this.ARInvoiceErrorInTransferVisibility = false;
        this.ARPaymentErrorInTransferVisibility = false;
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrencyCodeLocal = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        this.CurrencyCodeProfit = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        this.selectedCurrencyIndex_ARGrid = 1;
        this.selectedCurrencyIndex_ARChart = 1;
        this.barChartColors = [
            {
                backgroundColor: "rgb(23, 130, 184)",
            },
        ];
        this.barChartLabels = [];
        this.barChartData = [{ data: [], label: '' }];
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.myViewsQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        this.consolidationButtonVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent") ? true : false;
        this.ReceivablesChartId += this.CurrentSession.GetChartId();
        this.ReceivablesChartMoneyInOutId += this.CurrentSession.GetChartId();
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        this.myChartsService = new ChartsService_1.ChartsService();
    }
    AccountReceivablesComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.ARInvoicesSATFailedVisibility = (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "SATFAILEDINVOICES") && SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") ? true : false;
        this.ARPaymentsSATFailedVisibility = (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "SATFAILEDPAYMENTS") && SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") ? true : false;
        this.ARInvoiceErrorInTransferVisibility = (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ErrorInTransfer")) ? true : false;
        this.ARPaymentErrorInTransferVisibility = (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "ErrorInTransfer")) ? true : false;
    };
    AccountReceivablesComponent.prototype.FillFilters = function () {
        var list = LastFilter_1.LastFilter.myList();
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[0].lastTitle, 0 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[1].lastTitle, 1 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[2].lastTitle, 2 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[3].lastTitle, 3 + ""));
        this.SelectedItem = this.FilterList[1];
        this.FilterSelectedChange(this.SelectedItem);
    };
    AccountReceivablesComponent.prototype.FilterSelectedChange = function (item) {
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
        this.LoadBarQueries(0, days, item.Index, 1);
    };
    AccountReceivablesComponent.prototype.LoadBarQueries = function (months, days, index, currency) {
        var _this = this;
        this.myChartsService.GetMoneyStatusForTenant(null, months, days, this.TenantPM.Id, index, currency).subscribe(function (myResult) {
            _this.FillBarsMoney(myResult);
        });
    };
    AccountReceivablesComponent.prototype.LoadAllScreenData = function () {
        // this.LoadCurrencyCodeLocal();
        //this.LoadCurrencyCodeProfit();
        this.LoadQueriesCounts();
        this.LoadGridDataAR();
        this.LoadChartDataAR();
    };
    AccountReceivablesComponent.prototype.LoadCurrencyCodeLocal = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getSingleFromCache(SessionLocator_1.SessionLocator.TenantPM.CurrencyId).subscribe(function (myResult) {
            if (myResult) {
                _this.CurrencyCodeLocal = myResult.Result.Code;
            }
        });
    };
    AccountReceivablesComponent.prototype.LoadCurrencyCodeProfit = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getSingleFromCache(SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId).subscribe(function (myResult) {
            if (myResult) {
                _this.CurrencyCodeProfit = myResult.Result.Code;
            }
        });
    };
    AccountReceivablesComponent.prototype.ngOnInit = function () {
        this.FillFilters();
    };
    Object.defineProperty(AccountReceivablesComponent.prototype, "SelectedCurrencyIndex_ARGrid", {
        get: function () {
            return this.selectedCurrencyIndex_ARGrid;
        },
        set: function (value) {
            if (this.selectedCurrencyIndex_ARGrid != value) {
                this.selectedCurrencyIndex_ARGrid = value;
                this.LoadGridDataAR();
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountReceivablesComponent.prototype.SetSelectedCurrencyARGridIndex = function (args) {
        this.SelectedCurrencyIndex_ARGrid = args;
    };
    AccountReceivablesComponent.prototype.LoadGridDataAR = function () {
        var _this = this;
        this.ItemsSource.Clear();
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        }
        this.invoiceDomainService.GetDebrotExposureForGridControl(this.SelectedCurrencyIndex_ARGrid).subscribe(function (myResult) {
            if (myResult) {
                if (!myResult.HasError) {
                    _this.DebtorsObsList = [];
                    _this.DebtorsObsList = myResult.Result;
                    _this.ItemsSource.InsertCollection(_this.DebtorsObsList);
                }
            }
        });
    };
    Object.defineProperty(AccountReceivablesComponent.prototype, "SelectedCurrencyIndex_ARChart", {
        get: function () {
            return this.selectedCurrencyIndex_ARChart;
        },
        set: function (value) {
            if (this.selectedCurrencyIndex_ARChart != value) {
                this.selectedCurrencyIndex_ARChart = value;
                this.LoadChartDataAR();
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountReceivablesComponent.prototype.SetSelectedCurrencyARChartIndex = function (args) {
        this.SelectedCurrencyIndex_ARChart = args;
    };
    AccountReceivablesComponent.prototype.LoadChartDataAR = function () {
        var _this = this;
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        }
        this.invoiceDomainService.GetAgingReportARInvioceData(this.SelectedCurrencyIndex_ARChart, null).subscribe(function (myResult) {
            _this.BarData = myResult.Result;
            _this.FillBars();
        });
    };
    AccountReceivablesComponent.prototype.FillBars = function () {
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        var Graphs = [{
                "balloonText": "[[value]]",
                "fillAlphas": 0.8,
                "type": "column",
                "valueField": "ammount",
                "fillColors": ["#487E9F", "#c8d8e2"],
                "lineAlpha": 0,
            }];
        var dataProvider = [];
        var max = 0;
        this.BarData.forEach(function (element) {
            if (element.Amount > max)
                max = element.Amount;
            dataProvider.push({ category: element.DateRange, ammount: element.Amount });
        });
        var poisition = this.isRTL == true ? "right" : "left";
        makeAmBarChart(this.ReceivablesChartId, Graphs, dataProvider, max, null, null, null, null, poisition);
    };
    AccountReceivablesComponent.prototype.FillBarsMoney = function (MoneyInBarData) {
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        var barChartData = [{ data: [], label: '' }, { data: [], label: '' }];
        barChartData[0].data = [];
        barChartData[1].data = [];
        var barChartLabels = [];
        var max = 0;
        MoneyInBarData.forEach(function (element) {
            if (i == 0) {
                Graphs = [{
                        "balloonText": "[[value]]",
                        "fillAlphas": 1,
                        "id": "AmGraph-1" + i,
                        "title": "Invoices",
                        "type": "column",
                        "valueField": "col1",
                        "fillColors": ["#d29127", "#dfb267"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "lineAlpha": 0,
                    },
                    {
                        "balloonText": "[[value]]",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": "Payments",
                        "type": "column",
                        "valueField": "col2",
                        "fillColors": ["#1d758e", "#2186a3"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "lineAlpha": 0,
                    }
                ];
            }
            if (element.DataType == 'Invoices') {
                barChartData[0].label = element.DataType;
                barChartData[0].data[i] = element.TotalAmount;
            }
            else if (element.DataType == 'Payments') {
                barChartData[1].label = element.DataType;
                barChartData[1].data[i] = element.TotalAmount;
            }
            if (!barChartLabels.includes(element.DateRange)) {
                barChartLabels[i] = element.DateRange;
                i++;
            }
            if (element.TotalAmount > max)
                max = element.TotalAmount;
        });
        var i = 0;
        barChartLabels.forEach(function (item) {
            DataProvider[i] = { "category": barChartLabels[i], "col1": barChartData[0].data[i], "col2": barChartData[1].data[i] };
            i++;
        });
        var poisition = this.isRTL == true ? "right" : "left";
        makeAmBarChart(this.ReceivablesChartMoneyInOutId, Graphs, DataProvider, max, null, null, null, null, poisition);
    };
    Object.defineProperty(AccountReceivablesComponent.prototype, "ConsolidationButtonVisibility", {
        //Props
        get: function () {
            return this.consolidationButtonVisibility;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountReceivablesComponent.prototype, "MyViewsQueryVisibility", {
        get: function () {
            return this.myViewsQueryVisibility;
        },
        enumerable: true,
        configurable: true
    });
    AccountReceivablesComponent.prototype.ViewInvoiceQuery = function (args) {
        var _this = this;
        if (args != null) {
            var backButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Accounting");
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
            //var query = window.Queries.filter(q => q.ObjectTableId == ObjectTable.Id && q.Code == queryCode)[0];
            //if (window.PreDefinedFilters.filter(d => d.QueryId == query.Id) != null) {
            //    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == query.Id);
            //    predefinedFilters.forEach((filter, key) => {
            //        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
            //        var value1 = filter.PredefinedValues
            //        var value2 = filter.PredefinedValue2;
            //        if (value2 != null) {
            //            filterOperator = "Between";
            //        }
            //        this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
            //    });
            //}
            // this.filterAgrs.ObjectTableName = query.ObjectTableName;
            var listArgs = new Args_1.ListComponentArgs();
            //listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    AccountReceivablesComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        }
        this.invoiceDomainService.GetAccountingReceivablesSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.ARInvoicesDraftsCount = myResult.ARInvoicesDraftsCount > 1000 ? "1000+" : myResult.ARInvoicesDraftsCount.toString();
                _this.ARInvoicesUnpaidCount = myResult.ARInvoicesUnpaidCount > 1000 ? "1000+" : myResult.ARInvoicesUnpaidCount.toString();
                _this.ARInvoicesOpenConstituentCount = myResult.ARInvoicesOpenConstituentCount > 1000 ? "1000+" : myResult.ARInvoicesOpenConstituentCount.toString();
                _this.ARPaymentsDraftsCount = myResult.ARPaymentsDraftsCount > 1000 ? "1000+" : myResult.ARPaymentsDraftsCount.toString();
                _this.ARPaymentsOpenedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARPaymentsOpenedCount.toString();
                _this.ARInvoicesSATFailedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARInvoicesSATFailedCount.toString();
                _this.ARPaymentsSATFailedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARPaymentsSATFailedCount.toString();
                _this.ARInvoiceErrorInTransferCount = myResult.ARInvoicesFailedCount > 1000 ? "1000+" : myResult.ARInvoicesFailedCount.toString();
                _this.ARPaymentErrorInTransferCount = myResult.ARPaymentFailedCount > 1000 ? "1000+" : myResult.ARPaymentFailedCount.toString();
            }
        });
    };
    // Edit Customer
    AccountReceivablesComponent.prototype.LoadEditCustomer = function (entity) {
        var objectTableName = null;
        var editedPartnerId = entity.DebtorId;
        switch (entity.DebtorType) {
            case "AG":
            case "CO":
            case "FL":
                {
                    objectTableName = "Agent";
                    break;
                }
            case "WH":
            case "CC":
                {
                    objectTableName = "Warehouse";
                    break;
                }
            case "CS":
            case "PO":
                {
                    objectTableName = "Customer";
                    break;
                }
            case "AL": {
                objectTableName = "Airline";
                break;
            }
            case "CG": {
                objectTableName = "CustomAgent";
                break;
            }
            case "SG": {
                objectTableName = "ShippingAgent";
                break;
            }
            case "SL": {
                objectTableName = "ShippingLine";
                break;
            }
            case "TR": {
                objectTableName = "Trucker";
                break;
            }
            case "VD": {
                objectTableName = "Vendor";
                break;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: editedPartnerId, ObjectTableName: objectTableName });
        });
    };
    // Commands
    AccountReceivablesComponent.prototype.NewEntityClicked = function (args) {
        if (args != null) {
            switch (args) {
                case "ARPayment":
                    {
                        this.NewARPaymentMethod();
                        break;
                    }
                case "ARInvoiceConsolidation:IN":
                    {
                        this.NewConsolidationMethod("IN");
                        break;
                    }
                case "ARInvoiceConsolidation:CD":
                    {
                        this.NewConsolidationMethod("CD");
                        break;
                    }
            }
        }
    };
    AccountReceivablesComponent.prototype.NewARPaymentMethod = function () {
        var message = "";
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("ARPayment", "NEW", true)) {
            return;
        }
        var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
        str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable("ARPayment"));
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = str;
        logWindow.Width = 900;
        logWindow.Height = 570;
        logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
        //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //    .then(cmpRef => {
        //        cmpRef.instance.ComponentRef = cmpRef;
        //        cmpRef.instance.Run({ EntityId: "", EntityPM: new ARPaymentPM(), ObjectTableName: 'ARPayment' });
        //    });
    };
    AccountReceivablesComponent.prototype.NewConsolidationMethod = function (typeCode) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = typeCode;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.NewConsolidation");
        logWindow.Show("./InvoiceModules/ARInvoice/Components/NewEntity/NewConsolidationComponent");
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'ARInvoice', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Accounting") });
                        var isEditComponentSaved = false;
                        cmpRef.instance.BackCompleted.subscribe(function (bk) {
                            if (isEditComponentSaved) {
                                _this.LoadAllScreenData();
                            }
                        });
                        cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                            if (isSaveSuccess) {
                                isEditComponentSaved = true;
                            }
                        });
                    });
                }
            });
        });
    };
    // My Views 
    AccountReceivablesComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    AccountReceivablesComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], AccountReceivablesComponent.prototype, "ReloadUserQueries", void 0);
    AccountReceivablesComponent = __decorate([
        core_1.Component({
            selector: 'OperationsComponent',
            moduleId: module.id,
            templateUrl: './AccountReceivablesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AccountReceivablesComponent);
    return AccountReceivablesComponent;
}());
exports.AccountReceivablesComponent = AccountReceivablesComponent;
//# sourceMappingURL=AccountReceivablesComponent.js.map