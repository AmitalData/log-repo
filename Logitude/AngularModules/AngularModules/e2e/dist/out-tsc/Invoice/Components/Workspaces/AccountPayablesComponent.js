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
var InvoiceDomainService_1 = require("../../../Invoice/Services/InvoiceDomainService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var Args_1 = require("../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var APPaymentPM_1 = require("../../EntityPMs/APPaymentPM");
var CurrencyListService_1 = require("../../../Common/Services/StandardLists/CurrencyListService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DashboardFilters_1 = require("../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var ChartsService_1 = require("../../../Infrastructure/Services/ChartsService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var AccountPayablesComponent = /** @class */ (function () {
    function AccountPayablesComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.MoneyOutLocalCurrency = "(" + SessionLocator_1.SessionLocator.LocalCurrencyCode + ")";
        this.CreditorsObsList = [];
        this.myViewsQueryVisibility = false;
        this.APInvoiceErrorInTransferVisibility = false;
        this.APPaymentErrorInTransferVisibility = false;
        this.PayablesChartId = "PayablesChartId";
        this.FilterList = [];
        this.PayablesChartMoneyInOutId = "PayablesChartMoneyInOutId";
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Grid View
        this.CurrencyCodeLocal = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        this.CurrencyCodeProfit = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        this.selectedCurrencyIndex_APGrid = 1;
        // Dashboard
        this.selectedCurrencyIndex_APChart = 1;
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
        this.PayablesChartMoneyInOutId += this.CurrentSession.GetChartId();
        this.PayablesChartId += this.CurrentSession.GetChartId();
        this.myChartsService = new ChartsService_1.ChartsService();
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        this.FillFilters();
    }
    AccountPayablesComponent.prototype.FillFilters = function () {
        var list = LastFilter_1.LastFilter.myList();
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[0].lastTitle, 0 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[1].lastTitle, 1 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[2].lastTitle, 2 + ""));
        this.FilterList.push(new DashboardFilters_1.DashBoardFilters(list[3].lastTitle, 3 + ""));
        this.SelectedItem = this.FilterList[1];
        this.FilterSelectedChange(this.SelectedItem);
    };
    AccountPayablesComponent.prototype.LoadBarQueries = function (months, days, index, currency) {
        var _this = this;
        this.myChartsService.GetMoneyOutStatusForTenant(months, days, this.TenantPM.Id, index, currency).subscribe(function (myResult) {
            _this.FillBarsMoney(myResult);
        });
    };
    AccountPayablesComponent.prototype.FillBarsMoney = function (MoneyInBarData) {
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
        makeAmBarChart(this.PayablesChartMoneyInOutId, Graphs, DataProvider, max, null, null, null, null, poisition);
    };
    AccountPayablesComponent.prototype.FilterSelectedChange = function (item) {
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
    AccountPayablesComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.APInvoiceErrorInTransferVisibility = (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "ErrorInTransfer")) ? true : false;
        this.APPaymentErrorInTransferVisibility = (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "ErrorInTransfer")) ? true : false;
    };
    AccountPayablesComponent.prototype.LoadAllScreenData = function () {
        //this.LoadCurrencyCodeLocal();
        // this.LoadCurrencyCodeProfit();
        this.LoadQueriesCounts();
        this.LoadGridDataAP();
        this.LoadChartDataAP();
    };
    AccountPayablesComponent.prototype.LoadCurrencyCodeLocal = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getSingleFromCache(SessionLocator_1.SessionLocator.TenantPM.CurrencyId).subscribe(function (myResult) {
            if (myResult) {
                _this.CurrencyCodeLocal = myResult.Result.Code;
            }
        });
    };
    AccountPayablesComponent.prototype.LoadCurrencyCodeProfit = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getSingleFromCache(SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId).subscribe(function (myResult) {
            if (myResult) {
                _this.CurrencyCodeProfit = myResult.Result.Code;
            }
        });
    };
    Object.defineProperty(AccountPayablesComponent.prototype, "SelectedCurrencyIndex_APGrid", {
        get: function () {
            return this.selectedCurrencyIndex_APGrid;
        },
        set: function (value) {
            if (this.selectedCurrencyIndex_APGrid != value) {
                this.selectedCurrencyIndex_APGrid = value;
                this.LoadGridDataAP();
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountPayablesComponent.prototype.SetSelectedCurrencyIndex = function (args) {
        this.SelectedCurrencyIndex_APGrid = args;
    };
    AccountPayablesComponent.prototype.LoadGridDataAP = function () {
        var _this = this;
        this.ItemsSource.Clear();
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        }
        this.invoiceDomainService.GetCreditorExposure(this.SelectedCurrencyIndex_APGrid).subscribe(function (myResult) {
            if (myResult) {
                if (!myResult.HasError) {
                    _this.CreditorsObsList = [];
                    _this.CreditorsObsList = myResult.Result;
                    _this.ItemsSource.InsertCollection(_this.CreditorsObsList);
                }
            }
        });
    };
    Object.defineProperty(AccountPayablesComponent.prototype, "SelectedCurrencyIndex_APChart", {
        get: function () {
            return this.selectedCurrencyIndex_APChart;
        },
        set: function (value) {
            if (this.selectedCurrencyIndex_APChart != value) {
                this.selectedCurrencyIndex_APChart = value;
                this.LoadChartDataAP();
            }
        },
        enumerable: true,
        configurable: true
    });
    AccountPayablesComponent.prototype.SetSelectedCurrencyAPChartIndex = function (args) {
        this.SelectedCurrencyIndex_APChart = args;
    };
    AccountPayablesComponent.prototype.LoadChartDataAP = function () {
        var _this = this;
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        }
        this.invoiceDomainService.GetAgingReportAPInvioceData(this.SelectedCurrencyIndex_APChart).subscribe(function (myResult) {
            _this.BarData = myResult.Result;
            _this.FillBars();
        });
    };
    AccountPayablesComponent.prototype.FillBars = function () {
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
        makeAmBarChart(this.PayablesChartId, Graphs, dataProvider, max, null, null, null, null, poisition);
    };
    AccountPayablesComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        if (this.invoiceDomainService == null) {
            this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        }
        this.invoiceDomainService.GetAccountPayablesSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.APInvoicesDraftsCount = myResult.APInvoicesDraftsCount > 1000 ? "1000+" : myResult.APInvoicesDraftsCount.toString();
                _this.APInvoicesUnpaidCount = myResult.APInvoicesUnpaidCount > 1000 ? "1000+" : myResult.APInvoicesUnpaidCount.toString();
                _this.APPaymentsDraftsCount = myResult.APPaymentsDraftsCount > 1000 ? "1000+" : myResult.APPaymentsDraftsCount.toString();
                _this.APPaymentsOpenedCount = myResult.APPaymentsOpenedCount > 1000 ? "1000+" : myResult.APPaymentsOpenedCount.toString();
                _this.APInvoiceErrorInTransferCount = myResult.ARInvoicesFailedCount > 1000 ? "1000+" : myResult.APInvoicesFailedCount.toString();
                _this.APPaymentErrorInTransferCount = myResult.ARPaymentFailedCount > 1000 ? "1000+" : myResult.APPaymentFailedCount.toString();
            }
        });
    };
    AccountPayablesComponent.prototype.ViewInvoiceQuery = function (args) {
        var _this = this;
        if (args != null) {
            var backButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Accounting");
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            var displayTitle = queryCode;
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
            //var query = window.Queries.filter(q => q.ObjectTableId == ObjectTable.Id && q.Code == queryCode)[0];
            //if (window.PreDefinedFilters.filter(d => d.QueryId == query.Id) != null) {
            //    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryId == query.Id);
            //    predefinedFilters.forEach((filter, key) => {
            //        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
            //        var value1 = filter.PredefinedValue;
            //        var value2 = filter.PredefinedValue2;
            //        if (value2 != null) {
            //            filterOperator = "Between";
            //        }
            //        this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
            //    });
            //}
            //this.filterAgrs.ObjectTableName = query.ObjectTableName;
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
    // Edit Customer
    AccountPayablesComponent.prototype.LoadEditCustomer = function (entity) {
        var objectTableName = null;
        var editedPartnerId = entity.CreditorId;
        switch (entity.CreditorType) {
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
    AccountPayablesComponent.prototype.NewEntity = function () {
    };
    AccountPayablesComponent.prototype.NewEntityClicked = function (args) {
        if (args != null) {
            switch (args) {
                case "APPayment":
                    {
                        this.NewAPPaymentMethod();
                        break;
                    }
                case "APInvoiceMultiple":
                    {
                        this.NewMultipleInvoiceMethod();
                        break;
                    }
            }
        }
    };
    AccountPayablesComponent.prototype.NewAPPaymentMethod = function () {
        var message = "";
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("APPayment", "NEW", true)) {
            return;
        }
        var newApPaymentPM = new APPaymentPM_1.APPaymentPM();
        newApPaymentPM.StatusCode = "DR";
        newApPaymentPM.StatusName = "Draft";
        newApPaymentPM.Tenant = this.TenantPM.Id;
        newApPaymentPM.IsClosed = false;
        newApPaymentPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newApPaymentPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newApPaymentPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        newApPaymentPM.LocalCurrencyId = this.TenantPM.CurrencyId;
        newApPaymentPM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newApPaymentPM.RegisterDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var backButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Accounting");
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: newApPaymentPM.Id, EntityPM: newApPaymentPM, BackButtonLabel: backButtonLabel, ObjectTableName: 'APPayment' });
        });
    };
    AccountPayablesComponent.prototype.NewMultipleInvoiceMethod = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { IsMultipleEntities: true };
        logWindow.Title = "Receive Multiple AP Invoice";
        logWindow.Show("./InvoiceModules/APInvoice/Components/NewEntity/NewAPInvoiceComponent");
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'APInvoice', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Accounting") });
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
    Object.defineProperty(AccountPayablesComponent.prototype, "MyViewsQueryVisibility", {
        // My Views 
        get: function () {
            return this.myViewsQueryVisibility;
        },
        enumerable: true,
        configurable: true
    });
    AccountPayablesComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    AccountPayablesComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], AccountPayablesComponent.prototype, "ReloadUserQueries", void 0);
    AccountPayablesComponent = __decorate([
        core_1.Component({
            selector: 'AccountPayablesComponent',
            moduleId: module.id,
            templateUrl: './AccountPayablesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AccountPayablesComponent);
    return AccountPayablesComponent;
}());
exports.AccountPayablesComponent = AccountPayablesComponent;
//# sourceMappingURL=AccountPayablesComponent.js.map