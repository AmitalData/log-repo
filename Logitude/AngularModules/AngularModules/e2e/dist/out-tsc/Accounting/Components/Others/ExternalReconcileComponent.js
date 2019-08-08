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
var AccountingEntityHelper_1 = require("./../../Utilities/AccountingEntityHelper");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../Infrastructure/Tools");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ReconcileEventManager_1 = require("../../Utilities/ReconcileEventManager");
var FullAccountingSettingPM_1 = require("../../EntityPMs/FullAccountingSettingPM");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
//Entities
var ExternalReconciliationPM_1 = require("../../EntityPMs/ExternalReconciliationPM");
var ExternalReconciliationLinePM_1 = require("../../EntityPMs/ExternalReconciliationLinePM");
//Services
var ExternalReconciliationPMService_1 = require("../../Services/StandardPMs/ExternalReconciliationPMService");
var ExternalReconciliationExtendedPMService_1 = require("../../Services/ExtendedPMs/ExternalReconciliationExtendedPMService");
var LedgerTransactionExtendedListService_1 = require("../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var ExternalReconciliationExtendedListService_1 = require("../../Services/ExtendedLists/ExternalReconciliationExtendedListService");
var ExternalReconcileComponent = /** @class */ (function (_super) {
    __extends(ExternalReconcileComponent, _super);
    function ExternalReconcileComponent(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.DataContext = _this;
        _this.ObjectTableName = "ExternalReconciliation";
        _this.FireCheckBoxChecked = new core_1.EventEmitter();
        _this.ValidationErrorsList = [];
        _this.text_SumOfXRowsSelected = TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPage.O.SumOfXRowsSelected");
        _this.isRTL = false;
        _this.OperatorsList = [];
        _this.DateFilterList = [];
        _this.IsEntityValid = true;
        _this.entityListService = new EntityListService_1.EntityListService();
        _this.ledgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this._ExternalReconciliationExtendedListService = new ExternalReconciliationExtendedListService_1.ExternalReconciliationExtendedListService();
        _this.externalReconciliationExtendedPMService = new ExternalReconciliationExtendedPMService_1.ExternalReconciliationExtendedPMService();
        _this.externalReconciliationPMService = new ExternalReconciliationPMService_1.ExternalReconciliationPMService();
        _this.txt_FiltersSelected = "";
        _this.LoadGrids = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#endregion
        //#region [A] Transactions Data Source
        _this.TransactionFireCheckBoxChecked = new core_1.EventEmitter();
        _this.TransactionColumnsReady = new core_1.EventEmitter();
        _this.TransactionMarkIsChecked = new core_1.EventEmitter();
        _this.TransactionMenuHeaderchangeevent = new core_1.EventEmitter();
        _this.TransactiononQueryChangeEvent = new core_1.EventEmitter();
        _this.TransactionsColumns = null;
        _this.TransactionDataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "DocumentDate",
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        //#endregion
        //#region [B] Bank transactions Data Source
        _this.BankFireCheckBoxChecked = new core_1.EventEmitter();
        _this.BankColumnsReady = new core_1.EventEmitter();
        _this.BankMarkIsChecked = new core_1.EventEmitter();
        _this.BankMenuHeaderchangeevent = new core_1.EventEmitter();
        _this.BankonQueryChangeEvent = new core_1.EventEmitter();
        _this.BankColumns = null;
        _this.BankDataSource = {
            pageSize: 30,
            rowCount: null,
            sortingCol: "ReferenceDate",
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getBankRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        //#endregion
        //#region Totals Work
        _this.accountTransactionsTotal = 0;
        _this.bankTransactionsTotal = 0;
        _this.totalDifference = 0;
        //#endregion
        //#region Automatic Reconcile + Filters
        _this.isFiltersSelected = false;
        _this.isFiltersVisible = false;
        _this.IsAutoReconcile = false;
        _this.AmountCheckBoxChecked = false;
        _this.ReferenceCheckBoxChecked = false;
        _this.ReferenceDateCheckBoxChecked = false;
        _this.showAlert = false;
        _this.openAmountCurrency = "";
        _this.originalAmountCurrency = "";
        _this.FullAccountingSetting = new FullAccountingSettingPM_1.FullAccountingSettingPM();
        _this.reapeatCount = 1;
        //#region TEST PURPOSE
        _this.IsGenerateButtonVisible = false;
        _this.IsGeneratePasswordVisible = false;
        _this.labelCount = 0;
        _this.isRTL = SessionLocator_1.SessionLocator.TenantPM.LayoutDirection === 'rtl';
        _this.ExternalRecoPM = new ExternalReconciliationPM_1.ExternalReconciliationPM();
        _this.ExternalRecoPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.TransactionSelectedLines = new ObservableCollection_1.ObservableCollection([]);
        _this.BankSelectedLines = new ObservableCollection_1.ObservableCollection([]);
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        _this.txt_FiltersSelected = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.FiltersSelected");
        _this.OperatorsList =
            [{ EnglishName: 'Equals', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Equals") },
                { EnglishName: 'Not Equal', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
                { EnglishName: 'Larger Than', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
                { EnglishName: 'Less Than', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
                { EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
                { EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
            ];
        _this.DateFilterList =
            [
                { EnglishName: 'Last 7 days', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Last7days") },
                { EnglishName: 'Last month', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Lastmonth") },
                { EnglishName: 'Last 3 months', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Last3months") },
                { EnglishName: 'Last year', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Lastyear") },
                { EnglishName: 'Custom', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Custom") },
            ];
        //#endregion
        _this.AmountCheckBoxChecked = true;
        _this.GetDefaultValues();
        return _this;
    }
    ExternalReconcileComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.BankAccountPM = args.BankAccountPM;
            this.openAmountCurrency = args.openAmountCurrency;
            this.SetUIProperty();
        }
    };
    ExternalReconcileComponent.prototype.SetUIProperty = function () {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, false);
    };
    ExternalReconcileComponent.prototype.ngOnInit = function () {
        this.TransactionBuildColumns();
        this.BankBuildColumns();
        this.ReloadScreen();
        this.BankReloadScreen();
    };
    ExternalReconcileComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        var t = setTimeout(function () {
            _this.LoadGrids = true;
        }, 100);
    };
    ExternalReconcileComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(function () {
                _this.searchFieldFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                _this.ReloadScreen();
                _this.BankReloadScreen();
            }, 700);
        }
        else {
            this.searchFieldFilter = null;
            this.ReloadScreen();
            this.BankReloadScreen();
        }
    };
    ExternalReconcileComponent.prototype.OpenAmountTextChanged = function (num) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(num) && !Tools_1.AppTool.IsNullOrEmpty(this.SelectedOperator)) {
            this.timerToken = setTimeout(function () {
                if (!Tools_1.AppTool.IsNullOrEmpty(num) && !Tools_1.AppTool.IsNullOrEmpty(_this.SelectedOperator)) {
                    var OpenAmountFilterOperator = _this.SelectedOperator.EnglishName.replace(/ /g, ''); // remove white spaces
                    if (OpenAmountFilterOperator == "Equals") {
                        _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("ForeignAmount", num, -1 * num, null, OpenAmountFilterOperator, false, false, false, "number", false);
                    }
                    else if (OpenAmountFilterOperator == "LessThan") {
                        num = Math.abs(num);
                        _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("ForeignAmount", -1 * --num, +num, null, "Between", false, false, false, "number", false);
                    }
                    else if (OpenAmountFilterOperator == "LessThanOrEqual") {
                        num = Math.abs(num);
                        _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("ForeignAmount", -1 * num, +num, null, "Between", false, false, false, "number", false);
                    }
                    else {
                        _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("ForeignAmount", num, null, null, OpenAmountFilterOperator, false, false, false, "number", false);
                    }
                    _this.ReloadScreen();
                    _this.BankReloadScreen();
                }
                else {
                    _this.openAmountFilter = null;
                    _this.ReloadScreen();
                    _this.BankReloadScreen();
                }
            }, 700);
        }
        else {
            this.timerToken = setTimeout(function () {
                _this.openAmountFilter = null;
                _this.ReloadScreen();
                _this.BankReloadScreen();
            }, 700);
        }
    };
    //#endregion
    //#region Buttons Handlers
    ExternalReconcileComponent.prototype.ReconcilButton = function () {
        var errors = [];
        // Local Validate
        if (this.totalDifference != 0) {
            //errors.push(TextCodeTranslator.Translate("Accounting.General.O.DifferenceMustEqual0"));//"The difference must be equal to zero"
        }
        else {
            //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        }
        if (this.BankSelectedLines.Length == 0 && this.TransactionSelectedLines.Length == 0)
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.SelectTwoTransactionAtLeast"));
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var entity = this.CreateReconciliation();
            this.SubmitChanges(entity);
        }
    };
    ExternalReconcileComponent.prototype.SaveAsDraftButton = function () {
        //if (this.SelectedLines.Length > 0) {
        //    // 1- prepare transactions
        //    var transactionsList = [];
        //    this.SelectedLines.Collection.forEach((lineModel: LineModel) => {
        //        var transaction = lineModel.LedgerTransactionPM;
        //        //transaction.Mark = !transaction.Mark; // the service will take this misson
        //        transactionsList.push(transaction);
        //    });
        //    // 2- call the service
        //    this.CurrentSession.StartBusyIndicatorSaving();
        //    this._ReconciliationExtendedPMService.delsertDraftLedgerTransaction(transactionsList).subscribe((serviceResponse: ServiceResponse) => {
        //        console.log("_ReconciliationExtendedPMService.delsertDraftLedgerTransaction", serviceResponse);
        //        this.CurrentSession.StopBusyIndicator();
        //        var result = serviceResponse.Result;
        //        var msg = new MessageWindow();
        //        msg.ShowSuccessIcon = true;
        //        msg.Width = 400;
        //        msg.Show(TextCodeTranslator.Translate("Reconciliations.Q.reconciliationwassavedas"));
        //        msg.WindowClosed.subscribe((event: any) => {
        //            this.CancelButtonClicked();
        //        });
        //    });
        //} else {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.NotransactionsSelected")); //"No transactions selected!"
        //}
    };
    ExternalReconcileComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("ExternalReco");
    };
    ExternalReconcileComponent.prototype.RefreshButtonClicked = function () {
        this.ReloadScreen();
        this.BankReloadScreen();
        if (this.IsAutoReconcile) {
            this.AutoReco();
        }
    };
    ExternalReconcileComponent.prototype.TransactionBuildColumns = function () {
        var _this = this;
        this.TransactionsColumns = [];
        this.TransactionsColumns.push({
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.TransactionsColumns.push({
        //    FieldName: 'GroupHash',
        //    DataTypeCode: 'String',
        //    Display: '#',
        //    Styles: { width: '30px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        this.TransactionsColumns.push({
            FieldName: 'DocumentDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.TransactionsColumns.push({
        //    FieldName: 'DocumentDate',
        //    DataTypeCode: 'DateTime',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"), //'Ref. Date',
        //    Styles: { width: '100px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        //this.TransactionsColumns.push({
        //    FieldName: 'DueDate',
        //    DataTypeCode: 'DateTime',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"), // 'Due Date',
        //    Styles: { width: '100px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        this.TransactionsColumns.push({
            FieldName: 'Source',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Source"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        //this.TransactionsColumns.push({ // Check ReconcileMethodCode.GLAccounts:
        //    FieldName: 'OriginalAmount',
        //    DataTypeCode: 'String',
        //    Display: TextCodeTranslator.Translate("Accounting.General.O.OriginalAmount") + ' (' + this.originalAmountCurrency + ')',
        //    Styles: { width: '150px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true,
        //});
        //this.TransactionsColumns.push({
        //    FieldName: 'OpenAmount',
        //    DataTypeCode: 'String',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount") + ' (' + this.openAmountCurrency + ')',
        //    Styles: { width: '120px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        this.TransactionsColumns.push({
            FieldName: 'ForeignAmount',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.ForeignAmount") + ' (' + this.openAmountCurrency + ')',
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.TransactionsColumns.push({
            FieldName: 'Reference1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Reference1"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        this.TransactionsColumns.push({
            FieldName: 'Reference2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Reference2"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        this.TransactionsColumns.push({
            FieldName: 'Reference3',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Reference3"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        //this.TransactionsColumns.push({
        //    FieldName: 'JournalNumber',
        //    DataTypeCode: 'String',
        //    Display: TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"), // 'Journal No.',
        //    Styles: { width: '80px' },
        //    HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
        //    IsCustomTemplate: true
        //});
        this.TransactionsColumns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Notes"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        ReconcileEventManager_1.ReconcileEventManager.CheckBoxChecked.subscribe(function ($event) {
            if (!Tools_1.AppTool.IsNullOrEmpty($event)) {
                var row = $event.line;
                var rowId = $event.line.Id;
                var RowIndex = $event.RowIndex;
                var isChecked = $event.isChecked;
                var oneTime = $event.oneTime;
                console.log("---->> Row Selected: ", rowId, row, isChecked);
                if (isChecked) {
                    _this.PushLine(row, RowIndex);
                }
                else {
                    _this.PopLine(rowId);
                }
                _this.TransactionFireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });
            }
        });
    };
    ExternalReconcileComponent.prototype.TransactiononDataLoaded = function () {
        //this.TransactionsCheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue: true, IsAutoRecClicked: this.IsAutoRecClicked});
    };
    ExternalReconcileComponent.prototype.onCountReadyTrans = function (count) {
        this.transCount = count;
    };
    ExternalReconcileComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        //else {
        //    return;
        //}
        //if (this.currencyFilter) {
        //    filters.AdditionalFilters.push(this.currencyFilter);
        //}
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filters.AdditionalFilters.push(this.openAmountFilter);
        }
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.PageSize = take;
        filters.PageIndex = skip + 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;
        filters.addAdditionalFilter("IsExternalReconcile", false, null, null, "Equals", false, false, false, "Boolean");
        return this.entityListService.getReconciliationsByFilter("LedgerTransaction", this.BankAccountPM.GLAccountId, filters);
    };
    ExternalReconcileComponent.prototype.PushLine = function (row, RowIndex) {
        var index = this.TransactionSelectedLines.Collection.findIndex(function (c) { return c.Id == row.Id; });
        if (index < 0) { // DNE
            var r = new TransactionLineModel(row, this, RowIndex);
            this.TransactionSelectedLines.Insert(r);
            this.CalculateTotals();
        }
    };
    ExternalReconcileComponent.prototype.PopLine = function (id, specialCase) {
        if (specialCase === void 0) { specialCase = false; }
        //Automatic reconcile
        if (this.IsAutoReconcile) {
            // 1- find id of opposit line
            var transactionRow = this.TransactionSelectedLines.Collection.find(function (d) { return d.Id == id; });
            var oppositLine = this.BankSelectedLines.Collection.find(function (d) { return d.GroupHash == transactionRow.GroupHash; });
            // 2- popline
            if (!specialCase)
                this.BankPopLine(oppositLine.Id, true);
        }
        //
        this.TransactionSelectedLines.Remove(this.TransactionSelectedLines.Collection.find(function (c) { return c.Id == id; }));
        this.CalculateTotals();
    };
    ExternalReconcileComponent.prototype.CheckBoxValueChanged = function (Row) {
        this.TransactionFireCheckBoxChecked.emit({ rowData: Row.LedgerTransactionPM, IsChecked: false, RowIndex: Row.RowIndex, ById: true });
        this.PopLine(Row.LedgerTransactionPM.Id);
    };
    ExternalReconcileComponent.prototype.CalculateTotals = function () {
        this.accountTransactionsTotal = 0;
        var total = 0;
        for (var _i = 0, _a = this.TransactionSelectedLines.Collection; _i < _a.length; _i++) {
            var line = _a[_i];
            //total += +line.OpenAmount;
            // total += +line.ForeignAmount;
            if (line.IsCredit)
                total -= +line.ForeignAmount;
            else
                total += +line.ForeignAmount;
        }
        this.accountTransactionsTotal = total;
        var def = (this.bankTransactionsTotal - this.accountTransactionsTotal);
        this.totalDifference = def < 0 ? def * -1 : def;
    };
    ExternalReconcileComponent.prototype.ReloadScreen = function () {
        this.TransactiononQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
        this.TransactionSelectedLines.Clear();
        this.CalculateTotals();
    };
    ExternalReconcileComponent.prototype.BankBuildColumns = function () {
        var _this = this;
        this.BankColumns = [];
        this.BankColumns.push({
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true
        });
        //this.BankColumns.push({
        //    FieldName: 'GroupHash',
        //    DataTypeCode: 'String',
        //    Display: '#',
        //    Styles: { width: '30px' },
        //    HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
        //    HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
        //    IsCustomTemplate: true
        //});
        this.BankColumns.push({
            FieldName: 'ReferenceDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.ReferenceDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true
        });
        this.BankColumns.push({
            FieldName: 'Amount',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount") + ' (' + this.openAmountCurrency + ')',
            Styles: { width: '120px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true
        });
        this.BankColumns.push({
            FieldName: 'Reference',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Reference"),
            Styles: { width: '150px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true
        });
        this.BankColumns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Notes"),
            Styles: { width: '150px' },
            HtmlListComponentName: 'ReconcileExternalPageLineListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/ReconcileExternalPageLineListTemplate',
            IsCustomTemplate: true
        });
        ReconcileEventManager_1.ReconcileEventManager.BankCheckBoxChecked.subscribe(function ($event) {
            if (!Tools_1.AppTool.IsNullOrEmpty($event)) {
                var row = $event.line;
                var rowId = $event.line.Id;
                var RowIndex = $event.RowIndex;
                var isChecked = $event.isChecked;
                var oneTime = $event.oneTime;
                console.log("---->> Bank Row Selected: ", rowId, row, isChecked);
                if (isChecked) {
                    _this.BankPushLine(row, RowIndex);
                }
                else {
                    _this.BankPopLine(rowId);
                }
                _this.BankFireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });
            }
        });
    };
    ExternalReconcileComponent.prototype.BankonDataLoaded = function () {
        //this.CheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue: true, IsAutoRecClicked: this.IsAutoRecClicked});
    };
    ExternalReconcileComponent.prototype.onCountReadyBank = function (count) {
        this.bankCount = count;
    };
    ExternalReconcileComponent.prototype.getBankRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.dateFilter) {
            var refDateFilter = new ApiQueryFilters_1.FilterItem("ReferenceDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
            filters.AdditionalFilters.push(refDateFilter);
        }
        //else {
        //    return;
        //}
        //if (this.currencyFilter) {
        //    filters.AdditionalFilters.push(this.currencyFilter);
        //}
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            var amountFilter = new ApiQueryFilters_1.FilterItem("Amount", this.openAmountFilter.FieldValue, this.openAmountFilter.FieldValue2, null, this.openAmountFilter.Operator, false, false, false, "number", false);
            filters.AdditionalFilters.push(amountFilter);
        }
        filters.PageSize = take;
        filters.PageIndex = skip + 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;
        //filters.addAdditionalFilter("AccountingDate", true, null, null, "Between", false, false, false, "datetime");
        return this.entityListService.getExternalReoncilioationsByFilter("ReconcileExternalPage", this.BankAccountPM.Id, filters);
    };
    ExternalReconcileComponent.prototype.BankPushLine = function (row, RowIndex) {
        var index = this.BankSelectedLines.Collection.findIndex(function (c) { return c.Id == row.Id; });
        if (index < 0) { // DNE
            row.AmountToReconcile = row.CreditAmount != 0 ? row.CreditAmount : row.DebitAmount;
            // row.AmountToReconcile = row.CreditAmount!=0?row.CreditAmount*-1:row.DebitAmount;
            // row.Amount = row.AmountToReconcile;
            var r = new BankLineModel(row, this, RowIndex);
            this.BankSelectedLines.Insert(r);
            this.CalculateBankTotals();
        }
    };
    ExternalReconcileComponent.prototype.BankPopLine = function (id, specialCase) {
        if (specialCase === void 0) { specialCase = false; }
        //Automatic reconcile
        if (this.IsAutoReconcile) {
            // 1- find id of opposit line
            var pageLineRow = this.BankSelectedLines.Collection.find(function (d) { return d.Id == id; });
            var oppositLine = this.TransactionSelectedLines.Collection.find(function (d) { return d.GroupHash == pageLineRow.GroupHash; });
            // 2- popline
            if (!specialCase)
                this.PopLine(oppositLine.Id, true);
        }
        //
        this.BankSelectedLines.Remove(this.BankSelectedLines.Collection.find(function (c) { return c.Id == id; }));
        this.CalculateBankTotals();
    };
    ExternalReconcileComponent.prototype.BankCheckBoxValueChanged = function (Row) {
        this.BankFireCheckBoxChecked.emit({ rowData: Row.PageLinePM, IsChecked: false, RowIndex: Row.RowIndex });
        this.BankPopLine(Row.PageLinePM.Id);
    };
    ExternalReconcileComponent.prototype.BankReloadScreen = function () {
        this.BankonQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
        this.BankSelectedLines.Clear();
        this.CalculateBankTotals();
    };
    ExternalReconcileComponent.prototype.CalculateBankTotals = function () {
        this.bankTransactionsTotal = 0;
        var total = 0;
        for (var _i = 0, _a = this.BankSelectedLines.Collection; _i < _a.length; _i++) {
            var line = _a[_i];
            if (line.IsCredit)
                total -= +line.Amount;
            else
                total += +line.Amount;
        }
        this.bankTransactionsTotal = total;
        var def = (this.bankTransactionsTotal - this.accountTransactionsTotal);
        this.totalDifference = def < 0 ? def * -1 : def;
    };
    ExternalReconcileComponent.prototype.AutoReco = function () {
        console.log("[AUTO RECO] ", this.AmountCheckBoxChecked, this.ReferenceCheckBoxChecked, this.ReferenceDateCheckBoxChecked);
        this.IsAutoReconcile = true;
        //this.IsAutoRecClicked = true;
        //if (this.SelectedLines.Length > 0) {
        //    // Show prompt
        //    var confirmWindow = new ConfirmWindow();
        //    confirmWindow.Width = 390;
        //    confirmWindow.Show("Automatic Reconcile will clear all selected lines, continue?"); // "קיימות תנועות שנבחרו , האם להמשיך בהתאמה אוטומטית ?"
        //    confirmWindow.WindowClosed.subscribe((event: any) => {
        //        if (confirmWindow.Yes) {
        //            this.SelectedLines.Clear();// = [];
        //            this.RunAutomaticReconcile();
        //        } else if (confirmWindow.No) {
        //        }
        //    });
        //} else {
        //    this.RunAutomaticReconcile();
        //}
        this.RunAutomaticReconcile();
    };
    ExternalReconcileComponent.prototype.RunAutomaticReconcile = function () {
        var _this = this;
        // Reload screen
        this.ReloadScreen();
        this.BankReloadScreen();
        var t = setTimeout(function () {
            _this.ValidationErrorsList = [];
            //#region filters
            var filters = new ApiQueryFilters_1.ApiQueryFilters;
            if (_this.dateFilter) {
                filters.AdditionalFilters.push(_this.dateFilter);
            }
            if (_this.searchFieldFilter) {
                filters.AdditionalFilters.push(_this.searchFieldFilter);
            }
            if (_this.openAmountFilter) {
                filters.AdditionalFilters.push(_this.openAmountFilter);
            }
            filters.PageSize = 30;
            filters.PageIndex = 0;
            filters.GetAll = true;
            filters.GetCount = true;
            filters.addAdditionalFilter("IsExternalReconcile", false, null, null, "Equals", false, false, false, "Boolean");
            //#endregion
            _this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."
            _this._ExternalReconciliationExtendedListService.getExternalAutomaticReconcilationsByFilter(_this.AmountCheckBoxChecked, _this.ReferenceCheckBoxChecked, _this.ReferenceDateCheckBoxChecked, _this.BankAccountPM.Id, _this.BankAccountPM.GLAccountId, filters).subscribe(function (myResult) {
                var mm = myResult;
                var result = mm.Result;
                if (!mm.HasError) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                        if (result) {
                            if (result.Count > 0) {
                                //fill result
                                //...ledger lines
                                var lineModelList = [];
                                var transactionLines = result.transactionLines;
                                transactionLines.forEach(function (line) {
                                    var newLineModel = new TransactionLineModel(line, _this, -1);
                                    lineModelList.push(newLineModel);
                                });
                                _this.TransactionSelectedLines.InsertCollection(lineModelList, true);
                                _this.transCount = transactionLines.length;
                                //.
                                //...bank lines
                                var banklineModelList = [];
                                var pageLines = result.pageLines;
                                pageLines.forEach(function (line) {
                                    line.AmountToReconcile = line.Amount;
                                    var newLineModel = new BankLineModel(line, _this, -1);
                                    banklineModelList.push(newLineModel);
                                });
                                _this.BankSelectedLines.InsertCollection(banklineModelList, true);
                                _this.bankCount = pageLines.length;
                                //.
                                _this.CalculateTotals();
                                _this.CalculateBankTotals();
                            }
                            else {
                                _this.ShowEmptyAutoReco();
                            }
                        }
                    }
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }, 200);
    };
    ExternalReconcileComponent.prototype.FilterButtonClicked = function () {
        this.isFiltersVisible = !this.isFiltersVisible;
    };
    ExternalReconcileComponent.prototype.ChangeDate = function () {
        if (this.SelectedDateOperator) {
            //var TommorowDate = DateTool.AddDays((new Date()), 1);
            var TommorowDate = new Date();
            TommorowDate.setHours(23, 59, 59, 59);
            var TodayDate = new Date();
            TodayDate.setUTCHours(0, 0, 0, 0);
            var YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
            YesterdayDate.setUTCHours(0, 0, 0, 0);
            var LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThreeMonthDate = Tools_1.DateTool.AddDays((new Date()), -90);
            LastThreeMonthDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
            var LastYearFromDate = Tools_1.DateTool.AddDays((new Date()), -365);
            LastYearFromDate.setUTCHours(0, 0, 0, 0);
            var LastYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
            LastYearToDate.setUTCHours(0, 0, 0, 0);
            this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, false);
            switch (this.SelectedDateOperator.EnglishName) {
                case "Today":
                    {
                        this.FromDate = TodayDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Yesterday":
                    {
                        this.FromDate = YesterdayDate;
                        this.ToDate = TodayDate;
                        break;
                    }
                case "Last 7 days":
                    {
                        this.FromDate = LastSevenDaysDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Last month":
                    {
                        this.FromDate = LastThirtyDaysDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Last 3 months":
                    {
                        this.FromDate = LastThreeMonthDate;
                        this.ToDate = TommorowDate;
                        break;
                    }
                case "Current year":
                    {
                        this.FromDate = CurrentYearFromDate;
                        this.ToDate = CurrentYearToDate;
                        break;
                    }
                case "Last year":
                    {
                        this.FromDate = LastYearFromDate;
                        this.ToDate = LastYearToDate;
                        break;
                    }
                case "Custom":
                    {
                        this.FromDate = null;
                        this.ToDate = null;
                        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, true);
                        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, true);
                        this.CD.detectChanges();
                        break;
                    }
            }
        }
        else {
            this.FromDate = null;
            this.ToDate = null;
        }
        this.ReloadScreen();
        this.BankReloadScreen();
    };
    //Back
    ExternalReconcileComponent.prototype.AutoRecoBackButtonClicked = function () {
        this.IsAutoReconcile = false;
        this.ReloadScreen();
        this.BankReloadScreen();
    };
    ExternalReconcileComponent.prototype.GetAutoRecoBackMSG = function () {
        this.autoRecoCount = this.TransactionSelectedLines.Length;
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.BackFromAutoRecoMSG");
        msg = msg.replace("#number", this.autoRecoCount.toString());
        return msg;
    };
    Object.defineProperty(ExternalReconcileComponent.prototype, "SelectedOperator", {
        get: function () { return this.selectedOperator; },
        set: function (value) {
            if (this.selectedOperator != value) {
                this.selectedOperator = value;
                this.OpenAmountTextChanged(this.openAmount);
                this.FiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalReconcileComponent.prototype, "OpenAmount", {
        get: function () { return this.openAmount; },
        set: function (value) {
            if (this.openAmount != value) {
                this.openAmount = value;
                this.isFiltersSelected = (!Tools_1.AppTool.IsNullOrEmpty(this.OpenAmount) || !Tools_1.AppTool.IsNullOrEmpty(this.FromDate));
                this.FiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalReconcileComponent.prototype, "SelectedDateOperator", {
        get: function () { return this.selecteddateOperator; },
        set: function (value) {
            if (this.selecteddateOperator != value) {
                this.selecteddateOperator = value;
                this.ChangeDate();
                this.FiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalReconcileComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                //Filters Selection
                this.isFiltersSelected = (!Tools_1.AppTool.IsNullOrEmpty(this.OpenAmount) || !Tools_1.AppTool.IsNullOrEmpty(this.FromDate));
                //Date Filter
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                    this.dateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                    this.ReloadScreen();
                    this.BankReloadScreen();
                }
                else {
                    this.dateFilter = null;
                    this.ReloadScreen();
                    this.BankReloadScreen();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ExternalReconcileComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                //Filters Selection
                this.isFiltersSelected = (!Tools_1.AppTool.IsNullOrEmpty(this.OpenAmount) || !Tools_1.AppTool.IsNullOrEmpty(this.FromDate));
                //Date Filter
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ToDate) && !Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
                    this.dateFilter = new ApiQueryFilters_1.FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                    this.ReloadScreen();
                    this.BankReloadScreen();
                }
                else {
                    this.dateFilter = null;
                    this.ReloadScreen();
                    this.BankReloadScreen();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ExternalReconcileComponent.prototype.FiltersChanged = function () {
        var _this = this;
        var t = setTimeout(function () {
            if (_this.IsAutoReconcile) {
                _this.AutoReco();
            }
        }, 700);
    };
    //#endregion
    ExternalReconcileComponent.prototype.CreateReconciliation = function () {
        var newEntity = new ExternalReconciliationPM_1.ExternalReconciliationPM();
        newEntity.Id = "new";
        newEntity.GLAccountId = this.BankAccountPM.GLAccountId;
        newEntity.Tenant = this.BankAccountPM.Tenant;
        newEntity.CreateDate = new Date();
        newEntity.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newEntity.ExternalReconciliationLines = [];
        var lineNumber = 1;
        //insert glaccount transaction lines
        for (var i = 0; i < this.TransactionSelectedLines.Length; i++) {
            var selectedTransaction = this.TransactionSelectedLines.Collection[i];
            var newLine = new ExternalReconciliationLinePM_1.ExternalReconciliationLinePM(newEntity);
            newLine.ChangeSetOp = "1";
            newLine.ReconciliationId = newEntity.Id;
            newLine.Tenant = newEntity.Tenant;
            newLine.Line = lineNumber;
            newLine.GroupNumber = selectedTransaction.GroupHash ? selectedTransaction.GroupHash : 1;
            newLine.LedgerTransactionId = selectedTransaction.Id;
            newLine.ExternalPageLineId = null;
            newEntity.AddExternalReconciliationLine(newLine);
            lineNumber++;
        }
        //insert bank transaction lines
        for (var i = 0; i < this.BankSelectedLines.Length; i++) {
            var selectedTransaction = this.BankSelectedLines.Collection[i];
            var newLine = new ExternalReconciliationLinePM_1.ExternalReconciliationLinePM(newEntity);
            newLine.ChangeSetOp = "1";
            newLine.ReconciliationId = newEntity.Id;
            newLine.Tenant = newEntity.Tenant;
            newLine.Line = lineNumber;
            newLine.GroupNumber = selectedTransaction.GroupHash ? selectedTransaction.GroupHash : 1;
            newLine.ExternalPageLineId = selectedTransaction.Id;
            newLine.LedgerTransactionId = null;
            newEntity.AddExternalReconciliationLine(newLine);
            lineNumber++;
        }
        return newEntity;
    };
    ExternalReconcileComponent.prototype.SubmitChanges = function (entity) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.externalReconciliationPMService.insert(entity).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            var mm = myResult;
            var entity = mm.Result;
            if (!mm.HasError) {
                _this.ExternalRecoPM = entity;
                _this.ShowSuccessAlert();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ExternalReconcileComponent.prototype.OpenSource = function (id, type) {
        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = "Journal";
        switch (type) {
            // 1-Journal
            case '1': {
                tableName = "Journal";
                break;
            }
            // 2-ARInvoice
            case '2': {
                tableName = "ARInvoice";
                break;
            }
            // 3-ARPayment
            case '3': {
                tableName = "ARPayment";
                break;
            }
            // 4-APInvoice
            case '4': {
                tableName = "APInvoice";
                break;
            }
            // 5-APPayment
            case '5': {
                tableName = "APPayment";
                break;
            }
            // 6-Cheque Deposit
            case '6': {
                tableName = "BankDeposit";
                break;
            }
            // 7-Cash Deposit
            case '7': {
                tableName = "BankDeposit";
                break;
            }
            // 8-Revaluation
            case '8': {
                tableName = "Revaluation";
                break;
            }
            // 9-PaymentCheque
            case '9': {
                tableName = "PaymentCheque";
                break;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: tableName,
                BackButtonLabel: 'GLAccount'
            });
        });
    };
    ExternalReconcileComponent.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    ExternalReconcileComponent.prototype.OpenReco = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ExternalRecoPM.Id)) {
            this.showAlert = false;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.ExternalRecoPM.Id, ObjectTableName: 'ExternalReconciliation' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this.CurrentSession.CloseCurrentWindow();
                });
            });
        }
    };
    ExternalReconcileComponent.prototype.ShowSuccessAlert = function () {
        var _this = this;
        if (this.IsAutoReconcile) {
            this.IsAutoReconcile = false;
            this.showAlert = true;
            this.timerToken = setTimeout(function () {
                _this.showAlert = false;
            }, 7000); // 7 sec
            this.AutoRecoBackButtonClicked();
        }
        else {
            this.showAlert = true;
            this.timerToken = setTimeout(function () {
                _this.showAlert = false;
            }, 5000); // 5 sec
            this.ReloadScreen();
            this.BankReloadScreen();
        }
    };
    ExternalReconcileComponent.prototype.ShowEmptyAutoReco = function () {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Width = 400;
        messageWindow.Height = 150;
        messageWindow.RTL = this.isRTL;
        messageWindow.Title = " ";
        messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.Noautorecofoundbymethodchangemethod"));
    };
    ExternalReconcileComponent.prototype.CloseAlert = function () {
        this.showAlert = false;
    };
    ExternalReconcileComponent.prototype.GetAutoRecoMSG = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.AreconcileOfXTransactionCreated");
        return msg.replace("#Number", (this.ExternalRecoPM.ExternalReconciliationLines.length / 2).toString());
    };
    ExternalReconcileComponent.prototype.getTotalText = function (gridName) {
        var number = 0;
        if (gridName == 'glaccount') {
            number = this.TransactionSelectedLines.Length;
        }
        if (gridName == 'bank') {
            number = this.BankSelectedLines.Length;
        }
        var text = this.text_SumOfXRowsSelected;
        text = text.replace('%Number', number.toString());
        return text;
    };
    ExternalReconcileComponent.prototype.GetDefaultValues = function () {
        var _this = this;
        this.entityListService.getSingle(SessionLocator_1.SessionLocator.Tenant + "", "FullAccountingSetting").then(function (res) {
            res.subscribe(function (myResponse) {
                if (myResponse != null) {
                    var res = myResponse.Result;
                    _this.FullAccountingSetting = res;
                    _this.GetAutoRecoMethod();
                }
            });
        });
    };
    ExternalReconcileComponent.prototype.GetAutoRecoMethod = function () {
        var _this = this;
        if (this.FullAccountingSetting.ExternalReconciliationDefault) {
            this.entityListService.getSingle(this.FullAccountingSetting.ExternalReconciliationDefault + "", "AutomaticExternalRconcilMthod").then(function (res) {
                res.subscribe(function (myResponse) {
                    if (myResponse != null) {
                        var res = myResponse.Result;
                        _this.AutoRecoMethod = res;
                        if (_this.AutoRecoMethod) {
                            switch (_this.AutoRecoMethod.Code) {
                                case '1': // 1- Amount
                                    {
                                        _this.AmountCheckBoxChecked = true;
                                        _this.ReferenceCheckBoxChecked = false;
                                        _this.ReferenceDateCheckBoxChecked = false;
                                        break;
                                    }
                                case '2': // 2- Reference
                                    {
                                        _this.AmountCheckBoxChecked = true;
                                        _this.ReferenceCheckBoxChecked = true;
                                        _this.ReferenceDateCheckBoxChecked = false;
                                        break;
                                    }
                                case '3': // 3- Reference Date + Reference
                                    {
                                        _this.AmountCheckBoxChecked = true;
                                        _this.ReferenceCheckBoxChecked = true;
                                        _this.ReferenceDateCheckBoxChecked = true;
                                        break;
                                    }
                                case '4': // 4- Amount + Reference + Reference Date
                                    {
                                        _this.AmountCheckBoxChecked = true;
                                        _this.ReferenceCheckBoxChecked = true;
                                        _this.ReferenceDateCheckBoxChecked = true;
                                        break;
                                    }
                            }
                        }
                    }
                });
            });
        }
    };
    ExternalReconcileComponent.prototype.ResetFilters = function () {
        // Date
        this.SelectedDateOperator = null;
        this.FromDate = null;
        this.ToDate = null;
        // Amount
        this.OpenAmount = null;
        this.SelectedOperator = null;
    };
    ExternalReconcileComponent.prototype.GenerateTestLines = function (txt) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Generate test lines... " + "(" + this.reapeatCount + "/" + 100 + ")");
        this._ExternalReconciliationExtendedListService.getGenerateTestRecordsForExternalReco(this.BankAccountPM.Id, this.BankAccountPM.GLAccountId, txt).subscribe(function (myResult) {
            var mm = myResult;
            var result = mm.Result;
            if (!mm.HasError) {
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    if (_this.reapeatCount == 100) {
                        var msg = new MessageWindow_1.MessageWindow();
                        _this.CurrentSession.StopBusyIndicator();
                        msg.Show("Test lines generated successfully :) ");
                        _this.ReloadScreen();
                        _this.BankReloadScreen();
                    }
                    else {
                        _this.reapeatCount++;
                        _this.GenerateTestLines(txt);
                    }
                }
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ExternalReconcileComponent.prototype.LabelClicked = function () {
        var feature = FeatureLocator_1.FeatureLocator.IsFeatureGrantedByCode("ExtRecoGenerateTestRecords");
        if (feature) {
            this.labelCount++;
            if (this.labelCount == 5) {
                this.IsGeneratePasswordVisible = true;
            }
        }
    };
    ExternalReconcileComponent.prototype.PasswordOkButtonClicked = function () {
        if (this.GeneratePWD == "extrecopwd") {
            this.IsGeneratePasswordVisible = false;
            this.IsGenerateButtonVisible = true;
        }
        else {
            this.GeneratePWD = "";
        }
    };
    //#endregion
    ExternalReconcileComponent.prototype.getScreenHeight = function () {
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
    ExternalReconcileComponent.prototype.getScreenWidth = function () {
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
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ExternalReconcileComponent.prototype, "TransactionMenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ExternalReconcileComponent.prototype, "TransactiononQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ExternalReconcileComponent.prototype, "BankMenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ExternalReconcileComponent.prototype, "BankonQueryChangeEvent", void 0);
    ExternalReconcileComponent = __decorate([
        core_1.Component({
            selector: 'ExternalReconcileComponent',
            moduleId: './Accounting/Components/Others/',
            providers: [EntityListService_1.EntityListService],
            templateUrl: 'ExternalReconcileComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ExternalReconcileComponent);
    return ExternalReconcileComponent;
}(BaseComponent_1.BaseComponent));
exports.ExternalReconcileComponent = ExternalReconcileComponent;
var TransactionLineModel = /** @class */ (function (_super) {
    __extends(TransactionLineModel, _super);
    function TransactionLineModel(ledgerTransaction, parent, myRowIndex) {
        var _this = _super.call(this) || this;
        _this.ledgerTransaction = ledgerTransaction;
        _this.parent = parent;
        _this.myRowIndex = myRowIndex;
        _this.LedgerTransactionPM = null;
        _this.ObjectTableName = "LedgerTransaction";
        _this.DataContext = _this;
        _this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = _this.parent.EntityPM;
        _this.LedgerTransactionPM = ledgerTransaction;
        //this.OriginalAmount = parent.CalculateOriginalAmount(this);
        //if (!this.AmountToReconcile)
        //    this.AmountToReconcile = this.ledgerTransaction.OpenAmount;
        _this.RowIndex = myRowIndex;
        //this.OddEven = this.ColorMe();
        //#region Set Icons
        _this.IconCode = AccountingEntityHelper_1.AccountingEntityHelper.getEntityIcon(_this.LedgerTransactionPM.SourceTypeCode);
        return _this;
        //#endregion
    }
    Object.defineProperty(TransactionLineModel.prototype, "GroupHash", {
        get: function () { return this.LedgerTransactionPM.GroupHash; },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(TransactionLineModel.prototype, "IsCredit", {
        get: function () {
            return this.ledgerTransaction.ForeignAmountCredit != 0;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Id", {
        //#region Other Properties
        get: function () { return this.LedgerTransactionPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Tenant", {
        get: function () { return this.LedgerTransactionPM.Tenant; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "AccountingDate", {
        get: function () { return this.LedgerTransactionPM.AccountingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "DocumentDate", {
        get: function () { return this.LedgerTransactionPM.DocumentDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "JournalNumber", {
        get: function () { return this.LedgerTransactionPM.JournalNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Source", {
        get: function () { return this.LedgerTransactionPM.Source; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceType", {
        get: function () { return this.LedgerTransactionPM.SourceType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceId", {
        get: function () { return this.LedgerTransactionPM.SourceId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "DueDate", {
        get: function () { return this.LedgerTransactionPM.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "LocalAmountCredit", {
        get: function () { return this.LedgerTransactionPM.LocalAmountCredit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "LocalAmountDebit", {
        get: function () { return this.LedgerTransactionPM.LocalAmountDebit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ForeignAmountCredit", {
        get: function () { return this.LedgerTransactionPM.ForeignAmountCredit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ForeignAmountDebit", {
        get: function () { return this.LedgerTransactionPM.ForeignAmountDebit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ForeignAmount", {
        get: function () { return this.LedgerTransactionPM.ForeignAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmount", {
        get: function () { return this.LedgerTransactionPM.OpenAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencyCode", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencySign", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencySign; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "CurrencyId", {
        get: function () { return this.LedgerTransactionPM.CurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Reference1", {
        get: function () { return this.LedgerTransactionPM.Reference1; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Reference2", {
        get: function () { return this.LedgerTransactionPM.Reference2; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Reference3", {
        get: function () { return this.LedgerTransactionPM.Reference3; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Notes", {
        get: function () { return this.LedgerTransactionPM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencyId", {
        //get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceTypeCode", {
        get: function () { return this.LedgerTransactionPM.SourceTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceNumber", {
        get: function () { return this.LedgerTransactionPM.SourceNumber; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Row Coloring
    TransactionLineModel.prototype.ColorMe = function () {
        //if (AppTool.IsNullOrEmpty(this.parent.lastGroupNumber))
        //    this.parent.lastGroupNumber = this.GroupHash;
        //if (this.parent.lastGroupNumber == this.GroupHash) {
        //    return this.parent.lastColorOperation == true;
        //} else {
        //    this.parent.lastGroupNumber = this.GroupHash;
        //    this.parent.lastColorOperation = !this.parent.lastColorOperation;
        //    return this.parent.lastColorOperation == true;
        //}
    };
    //#endregion
    TransactionLineModel.prototype.CalculatOriginalCurruncy = function () {
        //
        // [i] copied from list template
        //
        if (!Tools_1.AppTool.IsNullOrEmpty(ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "0") { // 0-local currency
                // local
                return SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
            }
            else if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "1") { // 1-foreign currency
                // foreign
                return this.ledgerTransaction.CurrencySign;
            }
        }
    };
    return TransactionLineModel;
}(BaseComponent_1.BaseComponent));
var BankLineModel = /** @class */ (function (_super) {
    __extends(BankLineModel, _super);
    function BankLineModel(pageLine, parent, myRowIndex) {
        var _this = _super.call(this) || this;
        _this.pageLine = pageLine;
        _this.parent = parent;
        _this.myRowIndex = myRowIndex;
        _this.PageLinePM = null;
        _this.ObjectTableName = "ReconcileExternalPageLine";
        _this.DataContext = _this;
        _this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = _this.parent.EntityPM;
        _this.PageLinePM = pageLine;
        _this.RowIndex = myRowIndex;
        return _this;
    }
    Object.defineProperty(BankLineModel.prototype, "IsCredit", {
        get: function () {
            return this.pageLine.CreditAmount != 0;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "GroupHash", {
        get: function () { return this.pageLine.GroupHash; },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(BankLineModel.prototype, "Id", {
        //#region Properties
        get: function () { return this.PageLinePM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "Amount", {
        get: function () { return this.PageLinePM.Amount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "Reference", {
        get: function () { return this.PageLinePM.Reference; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "ReferenceDate", {
        get: function () { return this.PageLinePM.ReferenceDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "Notes", {
        get: function () { return this.PageLinePM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "LineNumber", {
        get: function () { return this.PageLinePM.LineNumber; },
        enumerable: true,
        configurable: true
    });
    return BankLineModel;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=ExternalReconcileComponent.js.map