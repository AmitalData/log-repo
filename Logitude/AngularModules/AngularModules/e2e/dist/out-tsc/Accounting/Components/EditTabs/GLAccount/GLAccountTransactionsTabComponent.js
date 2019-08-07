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
var Tools_1 = require("./../../../../Infrastructure/Tools");
var core_1 = require("@angular/core");
var Tools_2 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var LTBResponse_1 = require("../../../DataContracts/LTBResponse");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ReconcileEventManager_1 = require("../../../Utilities/ReconcileEventManager");
var LedgerTransactionListService_1 = require("../../../Services/StandardLists/LedgerTransactionListService");
var LedgerTransactionExtendedListService_1 = require("../../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var RatesTableListService_1 = require("../../../../Infrastructure/Services/StandardLists/RatesTableListService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var GLAccountTransactionsTabComponent = /** @class */ (function (_super) {
    __extends(GLAccountTransactionsTabComponent, _super);
    function GLAccountTransactionsTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.EntityPM = null;
        _this.ObjectTableName = "GLAccount";
        _this.DataContext = _this;
        _this._CurrencyListService = new CurrencyListService_1.CurrencyListService();
        _this._RatesTableListService = new RatesTableListService_1.RatesTableListService();
        _this.ledgerTransactionListService = new LedgerTransactionListService_1.LedgerTransactionListService();
        _this.ledgerTransactionListExtendedService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this.glAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        _this._LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this.CurrencyFilters = new ApiQueryFilters_1.ApiQueryFilters();
        _this.LTBSummery = new LTBResponse_1.LTBResponse();
        _this.OpenReconciliationMessage = "There are no Open Transactions";
        _this.isSingleCurrency = false;
        _this.isControlAccount = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.attachedGLAccountCheckBox = false;
        _this.splittedByCurrencyCheckBox = false;
        //#endregion
        //#region Data Source
        _this.columns = null;
        _this.DataSource = {
            pageSize: 50,
            rowCount: null,
            //sortingCol: "CreateDateTime",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.reconciliationCount = 0;
        _this.isValidate = false;
        //#endregion
        //#region ToolTip
        _this.isMouseIn = false;
        //#endregion
        //#region Filter Methods
        _this.filterSelectedValue = 'filter_accounting';
        _this._dateTypeCode = '1';
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this._entityListService = new EntityListService_1.EntityListService();
        _this.EntityPM = entityArgs.EntityPM;
        _this.CurrencyId = _this.EntityPM.CurrencyId;
        _this.isControlAccount = _this.EntityPM.IsControlAccount;
        _this.GetCurrencies();
        _this.LoadDefaultValues();
        _this.LoadAllScreenData();
        //this.RefreshButtonClicked();
        //this.GetNonReconciledTransactionsCount();
        // Set GLAccountReconcileMethodCode to use it in reconcile window
        ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode = _this.EntityPM.ReconcileMethodCode;
        //Set Currency LOV editability
        if (_this.EntityPM.IsMultiCurrency) {
            _this.UIProperties.SetEnabled("CurrencyId", "GLAccount", true);
        }
        else {
            _this.UIProperties.SetEnabled("CurrencyId", "GLAccount", false);
        }
        //event listening
        if (_this.CurrentSession.CurrentEditComponent != null) {
            var _CurrentEditComponentId = _this.CurrentSession.CurrentEditComponent.ComponentId;
            _this.CurrentSession.CurrentEditComponent.SubscriptionAdd(_this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "GATR") {
                        _this.LoadAllScreenData();
                    }
                }
            }));
        }
        return _this;
    }
    GLAccountTransactionsTabComponent.prototype.ngOnInit = function () {
        this.BuildColumns();
    };
    GLAccountTransactionsTabComponent.prototype.ngAfterViewInit = function () {
        //this.RefreshButtonClicked();
        //this.LoadAllScreenData();
        //#region Fill Date Default Values
        var today = new Date();
        this.ToDate = new Date();
        this.oldToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        this.oldFromDate = new Date(lastmonth);
        //#endregion
        this.CD.detectChanges();
    };
    GLAccountTransactionsTabComponent.prototype.LoadDefaultValues = function () {
        var _this = this;
        // Load Tenant rates
        var filters = new ApiQueryFilters_1.ApiQueryFilters(true);
        filters.addAdditionalFilter("BaseCurrencyId", SessionLocator_1.SessionLocator.TenantPM.CurrencyId, null, null, "Equals", false, false, false, "string");
        this._RatesTableListService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var rates = myResponse.Result;
                    if (!Tools_2.AppTool.IsNullOrEmpty(rates)) {
                        _this.ratesTable = rates;
                    }
                }
            }
        });
    };
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "OpenAmountHint", {
        get: function () { return this.openAmountHint; },
        set: function (value) {
            if (this.openAmountHint != value) {
                this.openAmountHint = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.oldFromDate = this.fromDate;
                this.fromDate = value;
                //if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                //    this.dateFilter = new FilterItem("CreateDate", this.FromDate, this.ToDate, null, "Between", false, false, false, "Date", false);
                //    //this.GetTransactions();
                //    //this.GetLTB();
                //}
                if (!this.isValidate)
                    this.validateDates();
                else {
                    this.isValidate = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.oldToDate = this.toDate;
                this.toDate = value;
                //if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                //    this.dateFilter = new FilterItem("CreateDate", this.FromDate, this.ToDate, null, "Between", false, false, false, "Date", false);
                //    //this.GetTransactions();
                //    //this.GetLTB();
                //}
                if (!this.isValidate)
                    this.validateDates();
                else {
                    this.isValidate = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "CurrencyId", {
        get: function () { return this.currencyId; },
        set: function (value) {
            if (this.currencyId != value) {
                this.currencyId = value;
                if (!Tools_2.AppTool.IsNullOrEmpty(value)) {
                    this.currencyFilter = new ApiQueryFilters_1.FilterItem("CurrencyId", value, null, null, "Equals", false, false, false, "string", false);
                    //this.GetTransactions();
                }
                else {
                    this.currencyFilter = null;
                }
                this.GetLTB();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "TotalSum", {
        get: function () {
            if (this.LocalSums)
                return this.LocalSums[this.LocalSums.length - 1];
            return 0;
        },
        set: function (value) {
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "AttachedGLAccountCheckBox", {
        get: function () { return this.attachedGLAccountCheckBox; },
        set: function (value) {
            if (this.attachedGLAccountCheckBox != value) {
                this.attachedGLAccountCheckBox = value;
                this.RefreshButtonClicked();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GLAccountTransactionsTabComponent.prototype, "SplittedByCurrencyCheckBox", {
        get: function () { return this.splittedByCurrencyCheckBox; },
        set: function (value) {
            if (this.splittedByCurrencyCheckBox != value) {
                this.splittedByCurrencyCheckBox = value;
                this.RefreshButtonClicked();
            }
        },
        enumerable: true,
        configurable: true
    });
    GLAccountTransactionsTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'Indicator',
            DataTypeCode: 'text',
            Display: '',
            Styles: { width: '20px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'AccountingDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DocumentDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"),
            Styles: { width: '75px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Source"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.columns.push({
        //    FieldName: 'SourceType',
        //    DataTypeCode: 'String',
        //    Display: 'Source Type',
        //    Styles: { width: '113px' },
        //    IsCustomTemplate: true
        //});
        this.columns.push({
            FieldName: 'LocalAmountCredit',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.LocalAmountCredit"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CumulativeLocalAmount',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.CumulativeLocalAmount"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.columns.push({
        //    FieldName: 'CurrencyCode',
        //    DataTypeCode: 'String',
        //    Display: 'Currency',
        //    Styles: { width: '70px' },
        //    IsCustomTemplate: true
        //});
        if (this.EntityPM.CurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
            this.columns.push({
                FieldName: 'ForeignAmountCredit',
                DataTypeCode: 'String',
                Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmountCredit"),
                Styles: { width: '120px' },
                HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
                HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
                IsCustomTemplate: true
            });
            if (this.EntityPM.IsMultiCurrency != true) {
                this.columns.push({
                    FieldName: 'CumulativeForeignAmount',
                    DataTypeCode: 'String',
                    Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.CumulativeForeignAmount"),
                    Styles: { width: '120px' },
                    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
                    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
                    IsCustomTemplate: true
                });
            }
        }
        this.columns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'OppositeAccountLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.OppositeAccountLocalName"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Notes"),
            Styles: { width: '200px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.CustomColumnsReady.emit(this.columns);
    };
    GLAccountTransactionsTabComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        else {
            return new Promise(function (resolve, reject) { });
        }
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this._dateTypeCode) {
            var dummyFilter = new ApiQueryFilters_1.FilterItem("DateTypeCode", this._dateTypeCode, null, null, "Equals", false, false, false, "string", false);
            filters.AdditionalFilters.push(dummyFilter);
        }
        else {
            var msg = new MessageWindow_1.MessageWindow();
            msg.Show("No filter selected!!!!");
            return;
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.addAdditionalFilter("GLAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("IncludeRelatedCurrenciesAccount", this.splittedByCurrencyCheckBox == null ? false : this.splittedByCurrencyCheckBox, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("IncludeChildAccounts", this.attachedGLAccountCheckBox == null ? false : this.attachedGLAccountCheckBox, null, null, "Equals", false, false, false, "boolean");
        return this._entityListService.getExtendedByFilters("LedgerTransaction", filters); //this.ledgerTransactionListExtendedService.getByFilters(filters);
    };
    GLAccountTransactionsTabComponent.prototype.GetTransactions = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        filters.PageSize = 10000;
        filters.addAdditionalFilter("AccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.ledgerTransactionListService.getByFilters(filters).subscribe(function (myResult) {
            console.log("Response: ", myResult);
            if (myResult == null) {
                _this.ItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                    _this.LocalSums = [];
                    _this.ForeignSums = [];
                    // check if transactions is multi currency
                    _this.isSingleCurrency = true;
                    for (var i = 0; i < _this.ItemsSource.length - 1; i++) {
                        if (_this.ItemsSource[i].CurrencyId != _this.ItemsSource[i + 1].CurrencyId)
                            _this.isSingleCurrency = false;
                    }
                    // Calculate commulative sums
                    for (var i = 0; i < _this.ItemsSource.length; i++) {
                        if (i == 0) {
                            _this.LocalSums[i] = _this.ItemsSource[i].LocalAmountCredit - _this.ItemsSource[i].LocalAmountDebit;
                            _this.ForeignSums[i] = _this.ItemsSource[i].ForeignAmountCredit - _this.ItemsSource[i].ForeignAmountDebit;
                        }
                        else {
                            _this.LocalSums[i] = _this.ItemsSource[i].LocalAmountCredit - _this.ItemsSource[i].LocalAmountDebit + _this.LocalSums[i - 1];
                            if (_this.isSingleCurrency)
                                _this.ForeignSums[i] = _this.ItemsSource[i].ForeignAmountCredit - _this.ItemsSource[i].ForeignAmountDebit + _this.ForeignSums[i - 1];
                        }
                        //console.log("LocalSums[" + i + "]=" + this.LocalSums[i]);
                    }
                }
            }
        });
    };
    GLAccountTransactionsTabComponent.prototype.GetLTB = function () {
        var _this = this;
        // Filters
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        else {
            return;
        }
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this._dateTypeCode) {
            var dummyFilter = new ApiQueryFilters_1.FilterItem("DateTypeCode", this._dateTypeCode, null, null, "Equals", false, false, false, "string", false);
            filters.AdditionalFilters.push(dummyFilter);
        }
        filters.PageSize = 30;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "AccountingDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("GLAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("IncludeRelatedCurrenciesAccount", this.splittedByCurrencyCheckBox == null ? false : this.splittedByCurrencyCheckBox, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("IncludeChildAccounts", this.attachedGLAccountCheckBox == null ? false : this.attachedGLAccountCheckBox, null, null, "Equals", false, false, false, "boolean");
        this.MenuHeaderchangeevent.emit({ Filters: filters, IgnoreFilter: false });
        this.ledgerTransactionListExtendedService.getBalanceByFilters(filters).subscribe(function (myResult) {
            //console.log("Response: ", myResult);
            if (myResult == null) {
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.LTBSummery = myResult.Result;
                    // if (this.EntityPM.IsMultiCurrency) {
                    var text = " &nbsp;";
                    for (var _i = 0, _a = _this.LTBSummery.EndBalanceForeignList; _i < _a.length; _i++) {
                        var item = _a[_i];
                        item.CurrencyCode = _this.GetCurrencyCode(item.CurrencyId);
                        item.CurrencySign = _this.GetCurrencySign(item.CurrencyId);
                    }
                    for (var _b = 0, _c = _this.LTBSummery.StartBalanceForeignList; _b < _c.length; _b++) {
                        var item = _c[_b];
                        item.CurrencyCode = _this.GetCurrencyCode(item.CurrencyId);
                        item.CurrencySign = _this.GetCurrencySign(item.CurrencyId);
                        //text += item.CurrencyCode + ' ' + item.BalanceForeign + ' ' + item.CurrencyCode + '<br>';
                    }
                    _this.OpenAmountHint = text;
                    // }
                    console.log("Result: ", myResult.Result);
                }
            }
        });
    };
    GLAccountTransactionsTabComponent.prototype.OnDataLoaded = function (result) {
        var _this = this;
        if (result && this.EntityPM.IsMultiCurrency) {
            var transactions = result;
            if (!this.currencyFilterValues) {
                // Create filter string that maintain values of current curreincies in the list
                transactions.forEach(function (item) {
                    if (item.rowData) {
                        _this.currencyFilterValues += (item.rowData.CurrencyId + ",");
                    }
                });
                this.CurrencyFilters = new ApiQueryFilters_1.ApiQueryFilters(true);
                this.CurrencyFilters.addAdditionalFilter("Id", this.currencyFilterValues, null, null, "InListExact", false, false, false, "string", false, true);
            }
            console.log(this.currencyFilterValues);
        }
        else {
            this.CurrencyFilters = new ApiQueryFilters_1.ApiQueryFilters(true);
        }
    };
    GLAccountTransactionsTabComponent.prototype.GetNonReconciledTransactionsCount = function () {
        var _this = this;
        this.glAccountExtendedListService.GetAccountReconcilesCount(this.EntityPM.Id).subscribe(function (myResult) {
            _this.reconciliationCount = 0;
            if (!Tools_2.AppTool.IsNullOrEmpty(myResult)) {
                _this.reconciliationCount = myResult;
                if (myResult == 1) {
                    _this.OpenReconciliationMessage = "There is " + _this.reconciliationCount + " Open Transactions";
                }
                else if (myResult > 1) {
                    _this.OpenReconciliationMessage = "There are " + _this.reconciliationCount + " Open Transactions";
                }
            }
        });
    };
    GLAccountTransactionsTabComponent.prototype.GetOpenBalanceCurrencySign = function () {
        var result = "";
        if (this.EntityPM) {
            if (this.EntityPM.IsMultiCurrency) {
                result = this.TenantCurrencySign;
            }
            else {
                result = this.EntityPM.CurrencySign;
                // if (this.EntityPM.ReconcileMethodCode == "0") { // 0- Local Currency
                //     result = this.TenantCurrencySign;
                // } else {
                //     result = this.EntityPM.CurrencySign;
                // }
            }
        }
        return result;
    };
    GLAccountTransactionsTabComponent.prototype.GetOpenBalanceAmount = function () {
        var result = 0;
        if (this.EntityPM) {
            if (this.EntityPM.IsMultiCurrency) {
                if (this.LTBSummery)
                    if (this.LTBSummery.StartBalanceLocal)
                        result = Number(this.LTBSummery.StartBalanceLocal);
            }
            else {
                if (this.LTBSummery)
                    if (this.LTBSummery.StartBalanceForeignList.length > 0)
                        result = Number(this.LTBSummery.StartBalanceForeignList[0].BalanceForeign);
            }
        }
        return result;
    };
    GLAccountTransactionsTabComponent.prototype.validateDates = function () {
        var _this = this;
        if (this.FromDate > this.ToDate) {
            this.timerToken = setTimeout(function () {
                _this.UIProperties.SetValidity("ToDate", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                _this.UIProperties.SetValidity("FromDate", _this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                _this.CD.detectChanges();
            }, 200);
            //// Change dates
            //this.timerToken = setTimeout(() => {
            //    this.isValidate = true;
            //    this.FromDate = this.oldFromDate;
            //    this.isValidate = true;
            //    this.ToDate = this.oldToDate;
            //    this.LoadData();
            //}, 200);
        }
        else {
            this.timerToken = setTimeout(function () {
                _this.UIProperties.SetValidity("ToDate", _this.ObjectTableName, true, "");
                _this.UIProperties.SetValidity("FromDate", _this.ObjectTableName, true, "");
                _this.CD.detectChanges();
            }, 200);
            this.LoadData();
        }
    };
    //load data after validate date
    GLAccountTransactionsTabComponent.prototype.LoadData = function () {
        if (!Tools_2.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_2.AppTool.IsNullOrEmpty(this.FromDate)) {
            var _fromDate = Tools_1.DateTool.GetDate(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0);
            var _toDate = Tools_1.DateTool.GetDate(this.toDate.getFullYear(), this.toDate.getMonth(), this.toDate.getDate(), 23, 59, 59);
            this.dateFilter = new ApiQueryFilters_1.FilterItem("AccountingDate", _fromDate, _toDate, null, "Between", false, false, false, "Date", false);
            console.log(">> Date Filter: ", _fromDate, _toDate);
            // this.dateFilter = new FilterItem("AccountingDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            this.RefreshButtonClicked();
        }
    };
    //#endregion
    //#region Buttons + CheckBox Handlers
    GLAccountTransactionsTabComponent.prototype.ReconcileButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.Id).subscribe(function (serviceResponse) {
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                var transaction = result.Result; // get the data
                var openAmountCurrency = transaction.OpenAmountCurrencySign;
                // original amount currency
                var originalAmountCurrency;
                if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "0")
                    originalAmountCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
                else if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "1")
                    originalAmountCurrency = transaction.CurrencySign;
                var windowArgs = {};
                windowArgs.GLAccountPM = _this.EntityPM;
                windowArgs.openAmountCurrency = openAmountCurrency;
                windowArgs.originalAmountCurrency = originalAmountCurrency;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
                logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 800 ? 700 : screenHeight - 70) : screenHeight - 70;
                logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Reconcile"); //"Reconcile";
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./Accounting/Components/Others/ReconcileComponent');
                logitudeWindow.WindowClosed.subscribe(function ($event) {
                    if ($event == 'ok') {
                        // show alert
                    }
                    _this.RefreshButtonClicked();
                });
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    GLAccountTransactionsTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    GLAccountTransactionsTabComponent.prototype.LoadAllScreenData = function () {
        this.GetLTB();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
        this.GetNonReconciledTransactionsCount();
    };
    GLAccountTransactionsTabComponent.prototype.Export2ExcelClicked = function () {
        var windowArgs = {};
        //windowArgs.query = this.SelectedQuery;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        windowArgs.userid = SessionInfo_1.SessionInfo.LoggedUserId;
        //windowArgs.Filters = this.CurrentQueryFilters
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = "Exporting View Data List To Excel File";
        logitudeWindow.WindowArgs = windowArgs;
        //logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
        //logitudeWindow.WindowClosed.subscribe(($event: any) => {
        //    this.QueryValueChanged({ QueryId: this.SelectedQueryId })
        //});
        //});
    };
    GLAccountTransactionsTabComponent.prototype.AttachedGLAccountChanged = function (event) {
        if (event == true) {
        }
        else {
        }
    };
    GLAccountTransactionsTabComponent.prototype.SplittedByCurrencyChanged = function (event) {
        if (event == true) {
        }
        else {
        }
    };
    GLAccountTransactionsTabComponent.prototype.OnMouseOver = function () {
        var _this = this;
        this.isMouseIn = true;
        //if (this.currencyRate) {
        this.timerToken = setTimeout(function () {
            var item = document.getElementById("tooltip-1");
            if (Tools_2.AppTool.IsNullOrEmpty(item))
                return;
            var itemRect = item.getBoundingClientRect();
            if (_this.isMouseIn) {
                var i = document.getElementById("tooltip-body-1");
                if (Tools_2.AppTool.IsNullOrEmpty(i))
                    return;
                document.getElementById("tooltip-body-1").style.position = "fixed";
                document.getElementById("tooltip-body-1").style.top = (itemRect.top - 70) + 'px';
                document.getElementById("tooltip-body-1").style.left = (itemRect.left + 22) + 'px';
                document.getElementById("tooltip-body-1").style.visibility = "visible";
                //this.timerToken = setTimeout(() => {
                //    document.getElementById("tooltip-body-1").style.visibility = "hidden";
                //}, 15000);
            }
        }, 100);
        //}
    };
    GLAccountTransactionsTabComponent.prototype.OnMouseLeave = function () {
        this.isMouseIn = false;
        //if (this.currencyRate) {
        this.timerToken = setTimeout(function () {
            document.getElementById("tooltip-body-1").style.visibility = "hidden";
        }, 400);
        //}
    };
    GLAccountTransactionsTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(function () {
                _this.searchFieldFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                //this.GetTransactions();
                _this.RefreshButtonClicked();
                //this.GetLTB();
            }, 700);
        }
        else {
            this.searchFieldFilter = null;
            this.RefreshButtonClicked();
        }
    };
    GLAccountTransactionsTabComponent.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    GLAccountTransactionsTabComponent.prototype.getScreenHeight = function () {
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
    GLAccountTransactionsTabComponent.prototype.getScreenWidth = function () {
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
    GLAccountTransactionsTabComponent.prototype.GetCurrencies = function () {
        var _this = this;
        this.TenantCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencyCode;
        this.TenantCurrencySign = SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
        this._CurrencyListService.getAll().subscribe(function (myResult) {
            console.log("Currencies: ", myResult);
            if (myResult == null) {
                _this.Currencies = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.Currencies = myResponse.Result;
                }
            }
        });
    };
    GLAccountTransactionsTabComponent.prototype.GetCurrencyCode = function (currencyId) {
        var c;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.Currencies)) {
            c = this.Currencies.find(function (d) { return d.Id == currencyId; });
        }
        return Tools_2.AppTool.IsNullOrEmpty(c) ? null : c.Code;
    };
    GLAccountTransactionsTabComponent.prototype.GetCurrencySign = function (currencyId) {
        var c;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.Currencies)) {
            c = this.Currencies.find(function (d) { return d.Id == currencyId; });
        }
        return Tools_2.AppTool.IsNullOrEmpty(c) ? null : c.Sign;
    };
    GLAccountTransactionsTabComponent.prototype.CalculateLocalAmount = function (foreignCurrencyId, foreignAmount) {
        if (!Tools_2.AppTool.IsNullOrEmpty(foreignAmount) || !Tools_2.AppTool.IsNullOrEmpty(this.ratesTable)) {
            if (foreignCurrencyId) {
                var rate = this.ratesTable.find(function (d) { return d.ForeignCurrencyId == foreignCurrencyId; });
                if (rate)
                    return rate.Rate * foreignAmount;
                else {
                    console.error("no rate for provided foreignCurrencyId ! ", foreignCurrencyId, this.ratesTable);
                    return 0;
                }
            }
            else {
                console.error("Cannot convert foreign amount to local amount, foreign Currency Id does not provided! ", foreignCurrencyId);
                return 0;
            }
        }
        else {
            console.error("Cannot convert foreign amount to local amount, Amount or Rates Table is empty! ", foreignAmount, this.ratesTable);
            return 0;
        }
    };
    GLAccountTransactionsTabComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterLines();
        }
    };
    GLAccountTransactionsTabComponent.prototype.FilterLines = function () {
        //Task 46666: Transaction Tab - date filter new design
        // <DateTypeCode>2</DateTypeCode> 1/2/3
        // Accounting- - code 1- חשבונאי
        // Due - code 2 - לגביה
        // Reference -code-3-  אסמכתא
        switch (this.filterSelectedValue) {
            case 'filter_accounting':
                this._dateTypeCode = '1';
                break;
            case 'filter_due':
                this._dateTypeCode = '2';
                break;
            case 'filter_reference':
                this._dateTypeCode = '3';
                break;
            default:
                break;
        }
        this.RefreshButtonClicked();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], GLAccountTransactionsTabComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], GLAccountTransactionsTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    GLAccountTransactionsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountTransactionsTabComponent.html',
            providers: [LedgerTransactionListService_1.LedgerTransactionListService, LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService, GLAccountExtendedListService_1.GLAccountExtendedListService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], GLAccountTransactionsTabComponent);
    return GLAccountTransactionsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.GLAccountTransactionsTabComponent = GLAccountTransactionsTabComponent;
//# sourceMappingURL=GLAccountTransactionsTabComponent.js.map