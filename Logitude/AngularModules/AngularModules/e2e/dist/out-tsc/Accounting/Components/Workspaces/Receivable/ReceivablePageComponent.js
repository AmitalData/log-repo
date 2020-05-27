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
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
// Services
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var GLAccountTotalByMonthListService_1 = require("../../../Services/StandardLists/GLAccountTotalByMonthListService");
var FullAccountingSettingListService_1 = require("../../../Services/StandardLists/FullAccountingSettingListService");
var AccountingSummery_1 = require("../../../DataContracts/AccountingSummery");
var AgingReportParameters_1 = require("../../../DataContracts/AgingReportParameters");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ModulesService_1 = require("../../../Services/ModulesService");
var ReceivablePageComponent = /** @class */ (function () {
    function ReceivablePageComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        this._GLAccountTotalByMonthListService = new GLAccountTotalByMonthListService_1.GLAccountTotalByMonthListService();
        this._FullAccountingSettingListService = new FullAccountingSettingListService_1.FullAccountingSettingListService();
        this.glAccountSummary = new AccountingSummery_1.GLAccountSummary();
        //#region Queries + Counts
        this.collectorsGLAVisibility = false;
        this.debetorsGLAVisibility = false;
        this.activeCustomersGLAVisibility = false;
        this.inactiveCustomersGlaVisibility = false;
        this.CLIENTGLACCOUNTSGlaVisibility = false;
        //#endregion
        this.RecentGLAccountsCount = 0;
        this.isRTL = false;
        this.isReady = false;
        this.chartId = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.barChartLabels = [];
        this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        //#endregion
        //#region Filters Code
        this.monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
        // aging chart
        //public FiltersList: string[] = [ 'Last 3 Month',
        //                                'Last 6 Month',
        //                                'Last 9 Month'];
        this.FiltersList = [
            { EnglishName: 'Last 3 Month', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LastXMonth").replace("#number", "שלושה") },
            { EnglishName: 'Last 6 Month', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LastXMonth").replace("#number", "שישה") },
            { EnglishName: 'Last 9 Month', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LastXMonth").replace("#number", "תשעה") }
        ];
        // Deptors
        this.DeptorsFiltersList = [];
        this.chartId = "Receivable_" + this.CurrentSession.GetChartId();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.LoadResources();
        //this.LoadAllScreenData();
        //this.SelectedFilter = "Last 6 Months";
        this.SelectedFilter = this.FiltersList[1];
        this.PopulateDeptorsFilterData();
    }
    ReceivablePageComponent.prototype.LoadResources = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("ARPayment").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(function (response) {
                    _this.isReady = true;
                });
            });
        });
    };
    ReceivablePageComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    };
    //#region Customers
    ReceivablePageComponent.prototype.RunNewCustomerWizard = function () {
        //var windowTitle = "New Customer";
        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 700;
        //logWindow.Height = 500;
        //logWindow.Title = windowTitle;
        ////logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        //logWindow.Show('./Accounting/Components/NewEntity/NewCashBookComponent');
    };
    ReceivablePageComponent.prototype.ViewCustomerQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "allcustomerquery";
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            switch (myQueryCode) {
                // MyCustomersAsCollectors
                // DebetorsCustomers
                // ActiveCustomersGLAccounts
                // InactiveCustomersGLAccount
                case "MyCustomersAsCollectors":
                    {
                        displayTitle = "My Customers (As Collectors)";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.Collectors");
                        break;
                    }
                case "DebetorsCustomers":
                    {
                        displayTitle = "Debtors Customers";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.debetors");
                        break;
                    }
                case "ActiveCustomersGLAccounts":
                    {
                        displayTitle = "Active Customers";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.ActiveCustomers");
                        break;
                    }
                case "InactiveCustomersGLAccount":
                    {
                        displayTitle = "Inactive Customers";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.InactiveCustomers");
                        break;
                    }
                case "All Customers":
                    {
                        displayTitle = "All Customers";
                        displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.Q.AllCustomers");
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Receivables");
            listArgs.Perspective = "GLAccountRecievable";
            listArgs.IgnoreSelectedPerspective = true;
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
    //#region ARPayments
    ReceivablePageComponent.prototype.NewARPaymentMethod = function () {
        var _this = this;
        var GeneralText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
        var ChangedText = GeneralText.split('%')[0];
        var NewText = TextCodeTranslator_1.TextCodeTranslator.TranslateTable("ARPayment");
        var showlocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        var FinalText = showlocal ? (NewText + " " + ChangedText) : (ChangedText + " " + NewText);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = FinalText;
        logWindow.Width = 900;
        logWindow.Height = 570;
        logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
        logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
        //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //    .then(cmpRef => {
        //        cmpRef.instance.ComponentRef = cmpRef;
        //        cmpRef.instance.Run({ EntityId: "", EntityPM: new ARPaymentPM(), ObjectTableName: 'ARPayment' });
        //    });
    };
    ReceivablePageComponent.prototype.ViewInvoiceQuery = function (args) {
        var _this = this;
        if (args != null) {
            var backButtonTitle = "Accounting";
            var objectTableName = args.split(':')[0];
            var queryCode = args.split(':')[1];
            var displayTitle = queryCode;
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
            var query = window.Queries.filter(function (q) { return q.ObjectTableId == ObjectTable.Id && q.Code == queryCode; })[0];
            if (window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; }) != null) {
                var predefinedFilters = window.PreDefinedFilters.filter(function (d) { return d.QueryId == query.Id; });
                predefinedFilters.forEach(function (filter, key) {
                    var filterOperator = (!Tools_1.AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                    var value1 = filter.PredefinedValue;
                    var value2 = filter.PredefinedValue2;
                    if (value2 != null) {
                        filterOperator = "Between";
                    }
                    _this.filterAgrs.addAdditionalFilter(filter.ObjectFieldName, value1, value2, null, filterOperator, filter.IsCustomFilter, filter.DisplayInList, false, filter.DataTypeCode);
                });
            }
            this.filterAgrs.ObjectTableName = query.ObjectTableName;
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Receivables");
            //listArgs.DisplayTitle = displayTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    //#endregion
    //#region General ARInvoice
    ReceivablePageComponent.prototype.NewGeneralARInvoice = function (type) {
        var _this = this;
        //var str = TextCodeTranslator.Translate("General.O.NewEntity");
        //str = str.replace("%Entity", "General Invoice");
        var str_NewGeneralInvoice = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewGeneralInvoice");
        var str_NewCreditNote = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NewCreditNote");
        var windowTitle = (type == 'IN' ? str_NewGeneralInvoice : str_NewCreditNote);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { InvoiceTypeCode: type };
        logWindow.Title = windowTitle;
        logWindow.Width = 550;
        logWindow.Height = 450;
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'ARInvoice' });
                        cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    });
                }
            });
        });
        //logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show("./InvoiceModules/ARInvoice/Components/NewEntity/NewGeneralARInvoiceComponent");
    };
    //#endregion
    ReceivablePageComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    ReceivablePageComponent.prototype.LoadAllScreenData = function () {
        this.LoadDefaultValues();
        this.LoadRecentGLAccounts();
        this.LoadQueriesCounts();
        this.LoadChartData();
        this.LoadDebtors();
    };
    ReceivablePageComponent.prototype.SetQueriesVisibility = function () {
        this.collectorsGLAVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "collectorsGLA") ? true : false;
        this.debetorsGLAVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "debetorsGLA") ? true : false;
        this.activeCustomersGLAVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "activeCustomersGLA") ? true : false;
        this.inactiveCustomersGlaVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "inactiveCustomersGla") ? true : false;
        this.CLIENTGLACCOUNTSGlaVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("GLAccount", "CLIENTGLACCOUNTS") ? true : false;
    };
    ReceivablePageComponent.prototype.LoadRecentGLAccounts = function () {
        var _this = this;
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;
        this._GLAccountExtendedListService.GetRecentGLAccounts("2").subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    _this.RecentGLAccountsList = myResult;
                    _this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    };
    ReceivablePageComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this._GLAccountExtendedListService.GetGLAccountsSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.glAccountSummary.ActiveCustomersCount = myResult.ActiveCustomersCount > 1000 ? "1000+" : myResult.ActiveCustomersCount.toString();
                _this.glAccountSummary.InactiveCustomersCount = myResult.InactiveCustomersCount > 1000 ? "1000+" : myResult.InactiveCustomersCount.toString();
                _this.glAccountSummary.CollectorsCount = myResult.CollectorsCount > 1000 ? "1000+" : myResult.CollectorsCount.toString();
                _this.glAccountSummary.DebitorsCount = myResult.DebitorsCount > 1000 ? "1000+" : myResult.DebitorsCount.toString();
                _this.glAccountSummary.AllCustomersCount = myResult.AllCustomersCount > 1000 ? "1000+" : myResult.AllCustomersCount.toString();
            }
        });
        // ARPayments
        var myService = new ModulesService_1.ModulesService();
        myService.GetAccountingReceivablesSummary().subscribe(function (myResult) {
            if (myResult != null) {
                _this.ARInvoicesDraftsCount = myResult.ARInvoicesDraftsCount > 1000 ? "1000+" : myResult.ARInvoicesDraftsCount.toString();
                _this.ARInvoicesUnpaidCount = myResult.ARInvoicesUnpaidCount > 1000 ? "1000+" : myResult.ARInvoicesUnpaidCount.toString();
                _this.ARInvoicesOpenConstituentCount = myResult.ARInvoicesOpenConstituentCount > 1000 ? "1000+" : myResult.ARInvoicesOpenConstituentCount.toString();
                _this.ARPaymentsDraftsCount = myResult.ARPaymentsDraftsCount > 1000 ? "1000+" : myResult.ARPaymentsDraftsCount.toString();
                _this.ARPaymentsOpenedCount = myResult.ARPaymentsOpenedCount > 1000 ? "1000+" : myResult.ARPaymentsOpenedCount.toString();
                _this.ARGeneralInvoiceDraftCount = myResult.ARGeneralInvoiceDraftCount > 1000 ? "1000+" : myResult.ARGeneralInvoiceDraftCount.toString();
            }
        });
    };
    ReceivablePageComponent.prototype.EditGLAccount = function (entity) {
        var _this = this;
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Receivables") });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    ReceivablePageComponent.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    ReceivablePageComponent.prototype.LoadDefaultValues = function () {
        var _this = this;
        // Full Accounting Settings
        this._FullAccountingSettingListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var list;
                    list = res;
                    _this.accSettings = list[0]; // because there is only one record for each tenant
                    console.log("FullAccountingSettingList: ", list);
                    _this.LoadChartData();
                }
            }
        });
    };
    ReceivablePageComponent.prototype.LoadChartData = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.accSettings))
            return;
        var numberOfmonthsbackwards = 6;
        switch (this.SelectedFilter.EnglishName) {
            case 'Last 3 Month': {
                numberOfmonthsbackwards = 3;
                break;
            }
            case 'Last 6 Month': {
                numberOfmonthsbackwards = 6;
                break;
            }
            case 'Last 9 Month': {
                numberOfmonthsbackwards = 9;
                break;
            }
        }
        var args = new AgingReportParameters_1.AgingReportParameters();
        args.Tenant = SessionLocator_1.SessionLocator.Tenant,
            args.AgingForDate = new Date();
        args.NumberOfmonthsbackwards = numberOfmonthsbackwards == null ? 3 : numberOfmonthsbackwards;
        args.VendorCustomerId = Tools_1.AppTool.IsNullOrEmpty(this.accSettings) ? "" : this.accSettings.CustomerControlAccountId;
        //args.Category1Id = "";
        //args.Category2Id = "";
        //args.Category3Id = "";
        //args.Category4Id = "";
        //args.Category5Id = "";
        //args.CollectorId = SessionLocator.LoggedUserId;
        //args.SalesmanId = "";
        args.IsCustomer = false;
        args.ForceUseMonthMethod = true;
        this._GLAccountExtendedListService.GetAgingReport(args).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var periods;
                    periods = res;
                    console.log("res: ", periods);
                    _this.LoadChart(periods);
                }
            }
        });
    };
    ReceivablePageComponent.prototype.OrganizeData = function (data) {
        //var oData: any[];
        //var takeNumber = 5; // Number of columns to show in the graph , execlude "Others" column
        //if (!AppTool.IsNullOrEmpty(data)) {
        //    if (data.length > takeNumber) {
        //         1-Take first section
        //        oData = data.slice(0, takeNumber);
        //         2-Calculate "Others" Column
        //        var othersColumn = new GLAccountList();
        //        othersColumn.TotalAmount = 0;
        //        othersColumn.GLAccountTypeCode = "-1"; // manual entry "Others"
        //        for (var i = takeNumber; i < data.length; i++) {
        //            var amount = data[i].TotalAmount;
        //            amount = this.ConvertToLocal(amount, data[i].CurrencyId);
        //            othersColumn.TotalAmount += ((AppTool.IsNullOrEmpty(amount)) ? 0 : amount);
        //        }
        //        oData.push(othersColumn);
        //    } else {
        //        return data
        //    }
        //}
        // 3-Return data
        //return oData;
    };
    ReceivablePageComponent.prototype.LoadChart = function (data) {
        var _this = this;
        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        //Graph Properties
        var Graphs = Graphs = [{
                "balloonText": "Amount: <br>[[value]]",
                "fillAlphas": 1,
                "id": "AmGraph-1" + i,
                "title": "Aging",
                "type": "column",
                "valueField": "dataCol",
                "fillColors": ["#BADFE8", "#7AC2D4", "#73BFD2", "#7AC2D4", "#BADFE8",],
                "gradientOrientation": "horizontal",
                "borderAlpha": 0,
                "lineColor": "#fff",
                "fixedColumnWidth": 70,
            }];
        //#endregion
        data.forEach(function (element) {
            //if (element.Total <= 0) return;
            _this.barChartData[0].label = "Amount";
            // Amount
            var value = element.Total; // + (Math.floor((Math.random() * 2500) + 1));
            _this.barChartData[0].data[i] = value.toString();
            // Labels
            var label = element.PeriodName.replace("b4", !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal ? "עד" : "Before"); // replace 'b4' with 'Before'
            // var label = element.PeriodName.replace("b4", "Before"); // replace 'b4' with 'Before'
            label = label.startsWith("Before") ? label.replace("/20", "/") : label; // minimize year in 'Before' Column
            _this.barChartLabels[i] = label;
            // Data
            DataProvider[i] = { "category": _this.barChartLabels[i], "dataCol": _this.barChartData[0].data[i] };
            if (value > max)
                max = value;
            i++;
        });
        makeAmBarChart(this.chartId, Graphs, DataProvider, max);
    };
    Object.defineProperty(ReceivablePageComponent.prototype, "SelectedFilter", {
        get: function () { return this.selectedFilter; },
        set: function (value) {
            if (this.selectedFilter != value) {
                this.selectedFilter = value;
                this.LoadChartData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReceivablePageComponent.prototype, "SelectedDeptorsFilter", {
        get: function () { return this.selectedDeptorsFilter; },
        set: function (value) {
            if (this.selectedDeptorsFilter != value) {
                this.selectedDeptorsFilter = value;
                this.LoadDebtors();
            }
        },
        enumerable: true,
        configurable: true
    });
    ReceivablePageComponent.prototype.PopulateDeptorsFilterData = function () {
        //this.DeptorsFiltersList = ['Accounting Balance', 'Balance Due'];
        this.DeptorsFiltersList =
            [{ EnglishName: 'Accounting Balance', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.AccountingBalance") },
                { EnglishName: 'Balance Due', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.BalanceDue") }];
        this.SelectedDeptorsFilter = this.DeptorsFiltersList[1];
    };
    ReceivablePageComponent.prototype.LoadDebtors = function () {
        //// Get month number from name
        //if (this.SelectedDeptorsFilter == undefined) {
        //    var monthNumber = new Date().getMonth() + 1;
        //} else {
        //    var op = this.SelectedDeptorsFilter.split(' ');
        //    var monthName = op[0];
        //    var monthNumber = this.monthNames.indexOf(monthName) + 1;
        //}
        var _this = this;
        var monthNumber = new Date().getMonth() + 1;
        var year = new Date().getFullYear();
        if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedDeptorsFilter)) {
            //this.SelectedDeptorsFilter = "Balance Due";
            this.SelectedDeptorsFilter = this.DeptorsFiltersList[1];
        }
        if (this.SelectedDeptorsFilter) {
            this._GLAccountExtendedListService.GetTopDeptors(this.SelectedDeptorsFilter.EnglishName, '2').subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult = myResponse.Result;
                        _this.TopDebtorsList = myResult;
                        console.log("DEPTORS: ", myResult);
                    }
                }
            });
        }
    };
    ReceivablePageComponent.prototype.DeptorsClick = function (entity) {
        //if (entity != null) {
        //    var windowArgs: any = {};
        //    windowArgs.accountId = entity.AccountId;
        //    windowArgs.cardId = entity.CardId;
        //    windowArgs.entity = entity;
        //    var windowTitle = "Aging For Customer";
        //    var logWindow = new LogitudeWindow();
        //    logWindow.Width = 550;
        //    logWindow.Height = 400;
        //    logWindow.WindowArgs = windowArgs;
        //    logWindow.Title = windowTitle;
        //    logWindow.ShowCloseButton = true;
        //    logWindow.Show("./Accounting/Components/Others/Aging4CustomerChartWindowComponent");
        //}
    };
    //#endregion
    ReceivablePageComponent.prototype.getScreenHeight = function () {
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
    ReceivablePageComponent.prototype.getScreenWidth = function () {
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
    ReceivablePageComponent.prototype.getLocalCurr = function () {
        return SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
    };
    ReceivablePageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReceivablePageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ReceivablePageComponent);
    return ReceivablePageComponent;
}());
exports.ReceivablePageComponent = ReceivablePageComponent;
//# sourceMappingURL=ReceivablePageComponent.js.map