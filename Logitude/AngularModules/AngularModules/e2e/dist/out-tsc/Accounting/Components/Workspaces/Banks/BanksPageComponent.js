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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Args_1 = require("../../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CashBookList_1 = require("../../../EntityLists/CashBookList");
var BankDepositExtendedListService_1 = require("../../../Services/ExtendedLists/BankDepositExtendedListService");
var CashBookExtendedListService_1 = require("../../../Services/ExtendedLists/CashBookExtendedListService");
var CashBookListService_1 = require("../../../Services/StandardLists/CashBookListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var RatesTableListService_1 = require("../../../../Infrastructure/Services/StandardLists/RatesTableListService");
//import {List} from '../../../../Infrastructure/DataContracts/Dashboard/List';
var Tools_1 = require("../../../../Infrastructure/Tools");
var BankAccountExtendedListService_1 = require("../../../Services/ExtendedLists/BankAccountExtendedListService");
var AccountingSummery_1 = require("../../../DataContracts/AccountingSummery");
var AccountingSummery_2 = require("../../../DataContracts/AccountingSummery");
var PaymentChequeExtendedListService_1 = require("../../../Services/ExtendedLists/PaymentChequeExtendedListService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var AccountingSummery_3 = require("../../../DataContracts/AccountingSummery");
var AccountingSummery_4 = require("../../../DataContracts/AccountingSummery");
var BanksPageComponent = /** @class */ (function () {
    function BanksPageComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.myBankDepositService = new BankDepositExtendedListService_1.BankDepositExtendedListService();
        this.myCashBookExtendedListService = new CashBookExtendedListService_1.CashBookExtendedListService();
        this.myCashBookListService = new CashBookListService_1.CashBookListService();
        this.currencyListService = new CurrencyListService_1.CurrencyListService();
        this.ratesTableListService = new RatesTableListService_1.RatesTableListService();
        this._BankAccountExtendedListService = new BankAccountExtendedListService_1.BankAccountExtendedListService();
        this.paymentChequeExtendedListService = new PaymentChequeExtendedListService_1.PaymentChequeExtendedListService();
        this.RecentBankDepositCount = 0;
        this._BankAccountSummary = new AccountingSummery_1.BankAccountSummary();
        this.paymentChequeSummary = new AccountingSummery_2.PaymentChequeSummary();
        this.bankDepositSummary = new AccountingSummery_3.BankDepositSummary();
        this.cashBookSummary = new AccountingSummery_4.CashBookSummary();
        this.screenHeight = 0;
        //#region Queries Features
        this.LoadBankPageMENUVisibility = false;
        this.TodayDepositsVisibility = false;
        this.cashDepositsVisibility = false;
        this.chequeDepositVisibility = false;
        this.AllBankDepositsVisibility = false;
        //#endregion
        this.txt_cash = TextCodeTranslator_1.TextCodeTranslator.Translate('BankDeposit.Q.cash');
        this.txt_chequeDeposit = TextCodeTranslator_1.TextCodeTranslator.Translate('BankDeposit.Q.chequeDeposit');
        this.isRTL = false;
        this.chartId = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.barChartLabels = [];
        this.barChartData = [{
                cash: [],
                postdated: [],
                label: '',
                scaleShowVerticalLines: false
            }];
        this.chartId = "CashBookChart_" + this.CurrentSession.GetChartId();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this._entityResourceService.getEntityResourceByTableName("CashBook").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("BankDeposit").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ARPayment").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("BankAccount").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(function (response) { });
        this._entityResourceService.getEntityResourceByTableName("ExternalReconciliationLine").subscribe(function (response) { });
        this.LoadTenantCurrency();
        this.LoadAllScreenData();
    }
    Object.defineProperty(BanksPageComponent.prototype, "ScreenHeight", {
        get: function () { return this.screenHeight; },
        set: function (value) {
            this.screenHeight = value;
        },
        enumerable: true,
        configurable: true
    });
    BanksPageComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.ScreenHeight = this.getScreenHeight();
    };
    BanksPageComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
        this.ScreenHeight = this.getScreenHeight();
    };
    BanksPageComponent.prototype.LoadAllScreenData = function () {
        this.LoadRecentBankDeposits();
        this.SetQueriesVisibility();
        this.LoadTenantCurrency();
        this.LoadChartData();
        this.LoadQueriesCounts();
    };
    BanksPageComponent.prototype.SetQueriesVisibility = function () {
        this.AllBankDepositsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("BankDeposit", "BankDeposit.Q.AllBankDeposits") ? true : false;
        this.cashDepositsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("BankDeposit", "CashBankDeposit") ? true : false;
        this.chequeDepositVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("BankDeposit", "ChequeBankDeposit") ? true : false;
        this.TodayDepositsVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("BankDeposit", "TodayBankDeposit") ? true : false;
        this.LoadBankPageMENUVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("ReconcileExternalPage", "LOADBANKPAGEMENU") ? true : false;
    };
    BanksPageComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this._BankAccountExtendedListService.GetBankAccountsSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this._BankAccountSummary.AllBankAccountsCount = myResult.AllBankAccountsCount > 1000 ? "1000+" : myResult.AllBankAccountsCount.toString();
            }
        });
        this.paymentChequeExtendedListService.GetPymentChequesSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.paymentChequeSummary.AllPaymenChequesCount = myResult.AllPaymentChequesCount > 1000 ? "1000+" : myResult.AllPaymentChequesCount.toString();
            }
        });
        this.myBankDepositService.GetBankDepositsSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.bankDepositSummary.TodaysDepositCount = myResult.TodaysDepositCount > 1000 ? "1000+" : myResult.TodaysDepositCount.toString();
            }
        });
        this.myCashBookExtendedListService.GetCashBookSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.cashBookSummary.AllCashbookCount = myResult.AllCashbookCount > 1000 ? "1000+" : myResult.AllCashbookCount.toString();
                _this.cashBookSummary.CashCashbookCount = myResult.CashCashbookCount > 1000 ? "1000+" : myResult.CashCashbookCount.toString();
                _this.cashBookSummary.ChequeCashbookCount = myResult.ChequeCashbookCount > 1000 ? "1000+" : myResult.ChequeCashbookCount.toString();
            }
        });
    };
    BanksPageComponent.prototype.LoadRecentBankDeposits = function () {
        var _this = this;
        this.RecentBankDepositsList = [];
        this.RecentBankDepositCount = 0;
        this.myBankDepositService.GetRecentBankDeposits().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    _this.RecentBankDepositsList = myResult;
                    _this.RecentBankDepositCount = myResult.length;
                }
            }
        });
    };
    BanksPageComponent.prototype.RunNewDepositWizard = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var windowTitle = "New Deposit";
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 520;
        logWindow.Height = 230;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewBankDepositComponent');
        this.CurrentSession.StopBusyIndicator();
    };
    BanksPageComponent.prototype.ViewDepositQuery = function (myQueryCode) {
        //chequeDeposit
        //cashDeposits
        //TodayDeposits
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "chequeDeposit";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "TodayDeposits":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("BankDeposit.Q.Today");
                        //var fromDate = new Date();
                        //fromDate.setHours(0, 0, 0, 0);
                        //var toDate = new Date();
                        //toDate.setHours(23, 59, 59, 59);
                        //console.log("Date Filter: ", fromDate, toDate);
                        //filters.addAdditionalFilter("AccountingDate", fromDate, toDate, null, "Between", false, false, false, "number");
                        break;
                    }
                case "cashDeposits":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("BankDeposit.Q.cash");
                        break;
                    }
                case "chequeDeposit":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("BankDeposit.Q.chequeDeposit");
                        break;
                    }
                case "AllBankDeposits":
                    {
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.Q.AllDeposits");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "BankDeposit";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
    BanksPageComponent.prototype.EditBankDeposit = function (entity) {
        var _this = this;
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: entity.Id, ObjectTableName: 'BankDeposit', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks')
                });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    //#endregion
    //#region Cashbooks
    BanksPageComponent.prototype.RunNewCashBookWizard = function () {
        var _this = this;
        var windowTitle = "New Cashbook";
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewCashbook");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 530;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewCashBookComponent');
    };
    BanksPageComponent.prototype.ViewCashbookQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "CashBooks";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "CashbookCash":
                    {
                        displayTitle = "Cash Cashbooks";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Cash");
                        break;
                    }
                case "CashbookCheque":
                    {
                        displayTitle = "Cheque Cashbooks";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Cheque");
                        break;
                    }
                case "CashBooks":
                    {
                        displayTitle = "All Cashbooks";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.AllCashbook");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "CashBook";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
    BanksPageComponent.prototype.EditBank = function (entity) {
        var _this = this;
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'CashBook', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks') });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    //#endregion
    //#region BankAccounts
    BanksPageComponent.prototype.RunNewBankAccountWizard = function () {
        var _this = this;
        var windowTitle = "New Bank Accounts";
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewBankAccounts");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewBankAccountComponent');
    };
    BanksPageComponent.prototype.LoadBankPagesFromFile = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsShowCloseButton = true;
        logWindow.Width = 500;
        logWindow.Height = 400;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.BankPagesFromFile");
        logWindow.WindowArgs = {};
        logWindow.WindowClosed.subscribe(function ($event) {
        });
        logWindow.Show('./Accounting/Components/NewEntity/LoadRecoExPageComponent');
    };
    BanksPageComponent.prototype.ViewBankAccountQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "AllBankAccounts";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "AllBankAccounts":
                    {
                        displayTitle = "Bank Accounts";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.AllBankAccounts");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "BankAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
    //#endregion
    //#region payment cheques
    BanksPageComponent.prototype.ViewPaymentChequesQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "AllPaymentCheques";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                case "AllPaymentCheques":
                    {
                        displayTitle = "Payment Cheques";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.AllPaymentCheques");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "PaymentCheque";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
    BanksPageComponent.prototype.NewPaymentCheque = function () {
        var _this = this;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewPaymentCheque");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 650;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        logWindow.Show('./Accounting/Components/NewEntity/NewPaymentChequeComponent');
    };
    BanksPageComponent.prototype.LoadTenantCurrency = function () {
        var _this = this;
        // Tenant currency
        var defaultCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        this.currencyListService.getSingle(defaultCurrencyId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var currency = myResponse.Result;
                    if (currency != undefined || currency != null) {
                        _this.tenantCurrencyCode = currency.Code;
                        _this.tenantCurrencyId = currency.Id;
                        // Tenant currency rates
                        var filters = new ApiQueryFilters_1.ApiQueryFilters(true);
                        filters.addAdditionalFilter("BaseCurrencyId", _this.tenantCurrencyId, null, null, "Equals", false, false, false, "string");
                        _this.ratesTableListService.getByFilters(filters).subscribe(function (myResponse) {
                            if (myResponse != null) {
                                if (!myResponse.HasError) {
                                    var res = myResponse.Result;
                                    if (res != undefined || res != null)
                                        _this.ratesTable = res;
                                    _this.LoadChartData();
                                }
                            }
                        });
                    }
                }
            }
        });
    };
    BanksPageComponent.prototype.ConvertToLocal = function (foreignAmount, currencyId) {
        if (!Tools_1.AppTool.IsNullOrEmpty(foreignAmount)) {
            // if no currency
            if (Tools_1.AppTool.IsNullOrEmpty(currencyId))
                return foreignAmount;
            // calculation
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ratesTable)) {
                var rateRow = this.ratesTable.find(function (d) { return d.ForeignCurrencyId == currencyId; });
                if (rateRow && rateRow != null) {
                    return rateRow.Rate * foreignAmount;
                }
                else {
                    return 0;
                }
            }
            else {
                return 0;
            }
        }
        else {
            return 0;
        }
    };
    BanksPageComponent.prototype.LoadChartData = function () {
        var _this = this;
        this.myCashBookExtendedListService.GetCashBookStatusChartData().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    console.log(res);
                    _this.LoadChart(_this.OrganizeData(res));
                }
            }
        });
    };
    BanksPageComponent.prototype.OrganizeData = function (data) {
        //data = data.slice(0, 2); // Draw only first two columns
        var oData;
        var takeNumber = 5; // Number of columns to show in the graph , execlude "Others" column
        if (!Tools_1.AppTool.IsNullOrEmpty(data)) {
            if (data.length > takeNumber) {
                // 1-Take first section
                oData = data.slice(0, takeNumber);
                // 2-Calculate "Others" Column
                var othersColumn = new CashBookList_1.CashBookList();
                othersColumn.TotalAmount = 0;
                othersColumn.CashBookTypeCode = "-1"; // manual entry "Others"
                for (var i = takeNumber; i < data.length; i++) {
                    var amount = data[i].TotalAmount;
                    amount = this.ConvertToLocal(amount, data[i].CurrencyId);
                    othersColumn.TotalAmount += ((Tools_1.AppTool.IsNullOrEmpty(amount)) ? 0 : amount);
                }
                oData.push(othersColumn);
            }
            else {
                return data;
            }
        }
        // 3-Return data
        return oData;
    };
    BanksPageComponent.prototype.LoadChart = function (data) {
        var _this = this;
        var useLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
                cash: [],
                postdated: [],
                label: '',
                scaleShowVerticalLines: false
            }];
        // Graph Properties
        var Graphs = Graphs =
            //#endregion
            data.forEach(function (element) {
                if (element.TotalAmount <= 0)
                    return;
                _this.barChartData[0].label = useLocal ? "סכום" : "Amount";
                console.log("[chart]", useLocal);
                // Amount
                var amount = _this.ConvertToLocal(element.TotalAmount, element.CurrencyId);
                var cash = element.CashBookTypeCode == "1" ? element.TotalAmount : element.CashTotal;
                var postdated = element.CashBookTypeCode == "1" ? 0 : element.PostdatedTotal;
                var isCheque = false;
                // Labels
                var name = "";
                if (element.CashBookTypeCode == "1") { // 1-cash
                    name = (useLocal ? "מזומן" : "Cash") + " (" + element.CurrencyCode + ")";
                }
                else if (element.CashBookTypeCode == "2") { // 2-cheque
                    name = (useLocal ? "המחאות" : "Cheque") + " (" + element.CurrencyCode + ")";
                    isCheque = true;
                }
                else {
                    name = useLocal ? "המחאה" : "Others";
                }
                // Data
                DataProvider[i] = {
                    "Name": name,
                    "CashCol": cash == null ? 0 : cash.toFixed(2),
                    "PostdatedCol": postdated == null ? 0 : postdated.toFixed(2),
                    "subtitle": isCheque ? "[[title]] : " : "",
                    "entityId": (element.CashBookTypeCode == "-1" ? "otherCol" : element.Id),
                    "currency": SessionLocator_1.SessionLocator.TenantPM.CurrencySign,
                };
                i++;
            });
        var poisition = this.isRTL == true ? "right" : "left";
        makeBanksBarChart(this.chartId, DataProvider, useLocal, poisition);
    };
    BanksPageComponent.prototype.BarClicking = function () {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
        }
    };
    BanksPageComponent.prototype.OnBarClick = function (e) {
        var _this = this;
        //console.log(e);
        if (!Tools_1.AppTool.IsNullOrEmpty(e)) {
            var itemData = e.item.dataContext;
            var id = itemData["entityId"];
            if (id == "otherCol") { // open all cashbook query
                var listArgs = new Args_1.ListComponentArgs();
                listArgs.QueryCode = 'CashBooks';
                listArgs.Filters = new ApiQueryFilters_1.ApiQueryFilters();
                listArgs.ObjectTableName = "CashBook";
                listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.AllCashbook");
                listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks');
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
            else {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        EntityId: id,
                        ObjectTableName: 'CashBook',
                        BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.General.O.Banks')
                    });
                });
            }
        }
    };
    //#endregion
    BanksPageComponent.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    BanksPageComponent.prototype.getScreenWidth = function () {
        if (self.innerWidth) {
            return self.innerWidth;
        }
        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }
        if (document.body) {
            return document.body.clientWidth;
        }
    };
    BanksPageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BanksPageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BanksPageComponent);
    return BanksPageComponent;
}());
exports.BanksPageComponent = BanksPageComponent;
//# sourceMappingURL=BanksPageComponent.js.map