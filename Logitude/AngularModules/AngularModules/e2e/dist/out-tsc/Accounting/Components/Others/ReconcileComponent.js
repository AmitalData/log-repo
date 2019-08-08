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
var LedgerTransactionPM_1 = require("../../EntityPMs/LedgerTransactionPM");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../Infrastructure/Tools");
var ReconcileEventManager_1 = require("../../Utilities/ReconcileEventManager");
var ReconciliationExtendedPMService_1 = require("../../Services/ExtendedPMs/ReconciliationExtendedPMService");
var LedgerTransactionExtendedListService_1 = require("../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var LineModel = /** @class */ (function (_super) {
    __extends(LineModel, _super);
    function LineModel(ledgerTransaction, parent, myRowIndex) {
        var _this = _super.call(this) || this;
        _this.ledgerTransaction = ledgerTransaction;
        _this.parent = parent;
        _this.myRowIndex = myRowIndex;
        _this.LedgerTransactionPM = null;
        _this.ObjectTableName = "LedgerTransaction";
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.isLineValid = true;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = _this.parent.EntityPM;
        _this.LedgerTransactionPM = ledgerTransaction;
        _this.OriginalAmount = parent.CalculateOriginalAmount(_this);
        if (!_this.AmountToReconcile)
            _this.AmountToReconcile = _this.ledgerTransaction.OpenAmount;
        _this.RowIndex = myRowIndex;
        _this.OddEven = _this.ColorMe();
        //#region Set Icons
        _this.IconCode = AccountingEntityHelper_1.AccountingEntityHelper.getEntityIcon(_this.LedgerTransactionPM.SourceTypeCode);
        return _this;
        //#endregion
    }
    Object.defineProperty(LineModel.prototype, "GroupHash", {
        get: function () { return this.LedgerTransactionPM.GroupHash; },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(LineModel.prototype, "OriginalAmount", {
        get: function () { return this.parent.CalculateOriginalAmount(this); },
        set: function (value) {
            if (this.originalAmount != value) {
                this.originalAmount = this.parent.CalculateOriginalAmount(this);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "AmountToReconcile", {
        get: function () { return this.LedgerTransactionPM.AmountToReconcile; },
        set: function (value) {
            if (this.LedgerTransactionPM.AmountToReconcile != value) {
                this.LedgerTransactionPM.AmountToReconcile = value;
                //if (this.parent.IsEntityValid) {
                if (this.OpenAmount < 0) { // debit
                    if (value < this.OpenAmount || value > 0) {
                        this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
                        this.parent.IsEntityValid = false;
                        this.isLineValid = false;
                    }
                    else {
                        this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
                        this.parent.IsEntityValid = true;
                        this.isLineValid = true;
                    }
                }
                else { // credit
                    if (value > this.OpenAmount || value < 0) {
                        this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
                        this.parent.IsEntityValid = false;
                        this.isLineValid = false;
                    }
                    else {
                        this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
                        this.parent.IsEntityValid = true;
                        this.isLineValid = true;
                    }
                }
                //WI26522
                if (this.parent.IsEntityValid) {
                    if (Math.abs(value) > Math.abs(this.OpenAmount)) {
                        this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
                        this.parent.IsEntityValid = false;
                        this.isLineValid = false;
                    }
                    else {
                        this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
                        this.parent.IsEntityValid = true;
                        this.isLineValid = true;
                    }
                }
                //}
                this.parent.CalculateTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Id", {
        //#region Other Properties
        get: function () { return this.LedgerTransactionPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Tenant", {
        get: function () { return this.LedgerTransactionPM.Tenant; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "AccountingDate", {
        get: function () { return this.LedgerTransactionPM.AccountingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "DocumentDate", {
        get: function () { return this.LedgerTransactionPM.DocumentDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "JournalNumber", {
        get: function () { return this.LedgerTransactionPM.JournalNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Source", {
        get: function () { return this.LedgerTransactionPM.Source; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "SourceType", {
        get: function () { return this.LedgerTransactionPM.SourceType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "SourceId", {
        get: function () { return this.LedgerTransactionPM.SourceId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "DueDate", {
        get: function () { return this.LedgerTransactionPM.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "LocalAmountCredit", {
        get: function () { return this.LedgerTransactionPM.LocalAmountCredit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "LocalAmountDebit", {
        get: function () { return this.LedgerTransactionPM.LocalAmountDebit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "ForeignAmountCredit", {
        get: function () { return this.LedgerTransactionPM.ForeignAmountCredit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "ForeignAmountDebit", {
        get: function () { return this.LedgerTransactionPM.ForeignAmountDebit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "OpenAmount", {
        get: function () { return this.LedgerTransactionPM.OpenAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "OpenAmountCurrencyCode", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "OpenAmountCurrencySign", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencySign; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "CurrencyId", {
        get: function () { return this.LedgerTransactionPM.CurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Reference1", {
        get: function () { return this.LedgerTransactionPM.Reference1; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Reference2", {
        get: function () { return this.LedgerTransactionPM.Reference2; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Reference3", {
        get: function () { return this.LedgerTransactionPM.Reference3; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "Notes", {
        get: function () { return this.LedgerTransactionPM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "IsPartial", {
        get: function () { return this.OpenAmount != this.AmountToReconcile; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "OpenAmountCurrencyId", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "SourceTypeCode", {
        get: function () { return this.LedgerTransactionPM.SourceTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "SourceNumber", {
        get: function () { return this.LedgerTransactionPM.SourceNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineModel.prototype, "GroupNumber", {
        get: function () { return this.LedgerTransactionPM.GroupHash; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Row Coloring
    LineModel.prototype.ColorMe = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.parent.lastGroupNumber))
            this.parent.lastGroupNumber = this.GroupHash;
        if (this.parent.lastGroupNumber == this.GroupHash) {
            return this.parent.lastColorOperation == true;
        }
        else {
            this.parent.lastGroupNumber = this.GroupHash;
            this.parent.lastColorOperation = !this.parent.lastColorOperation;
            return this.parent.lastColorOperation == true;
        }
    };
    //#endregion
    LineModel.prototype.CalculatOriginalCurruncy = function () {
        //
        // [i] copied from list template
        //
        if (!Tools_1.AppTool.IsNullOrEmpty(this.parent.GLAccountPM.ReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (this.parent.GLAccountPM.ReconcileMethodCode == "0") { // 0-local currency
                // local
                return SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
            }
            else if (this.parent.GLAccountPM.ReconcileMethodCode == "1") { // 1-foreign currency
                // foreign
                return this.ledgerTransaction.CurrencySign;
            }
        }
    };
    return LineModel;
}(BaseComponent_1.BaseComponent));
exports.LineModel = LineModel;
var ReconcileComponent = /** @class */ (function (_super) {
    __extends(ReconcileComponent, _super);
    function ReconcileComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "LedgerTransaction"; //Reconciliation
        _this.ValidationErrorsList = [];
        _this.FireCheckBoxChecked = new core_1.EventEmitter();
        _this.ColumnsReady = new core_1.EventEmitter();
        _this.MarkIsChecked = new core_1.EventEmitter();
        _this.lastColorOperation = false;
        _this.isRTL = false;
        _this.OperatorsList = [];
        _this.IsEntityValid = true;
        _this._LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this._ReconciliationExtendedPMService = new ReconciliationExtendedPMService_1.ReconciliationExtendedPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsAutoRecClicked = false;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.columns = null;
        _this.MustIgnoreItems = [];
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            //sortingCol: "CreateDateTime",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        //#endregion
        //#region Totals Work
        _this.TotalCredit = 0;
        _this.TotalDebit = 0;
        _this.TotalsDeference = 0;
        _this.showAlert = false;
        _this.openAmountCurrency = "";
        _this.originalAmountCurrency = "";
        //#region SaveAsDraft
        _this.isDraftReconciliation = false;
        _this.CheckBoxFilterChanged = new core_1.EventEmitter();
        _this.IsDraft = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this._entityListService = new EntityListService_1.EntityListService();
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM = new LedgerTransactionPM_1.LedgerTransactionPM();
        _this.EntityPM.Tenant = _this.TenantPM.Id;
        _this.SelectedLines = new ObservableCollection_1.ObservableCollection([]);
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        _this.CurrencyId = _this.EntityPM.CurrencyId;
        //#region initialize operators
        //this.OperatorsList =   ['Equals',
        //                        'Not Equal',
        //                        'Larger Than',
        //                        'Less Than',
        //                        'Less Than Or Equal',
        //    'Greater Than Or Equal',];
        _this.OperatorsList =
            [{ EnglishName: 'Equals', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Equals") },
                { EnglishName: 'Not Equal', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
                { EnglishName: 'Larger Than', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
                { EnglishName: 'Less Than', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
                { EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
                { EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
            ];
        return _this;
        //#endregion
    }
    ReconcileComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.GLAccountPM = args.GLAccountPM;
            ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode = this.GLAccountPM.ReconcileMethodCode;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.GLAccountPM.CurrencyId)) {
                this.CurrencyId = this.GLAccountPM.CurrencyId;
            }
            this.AutomaticReconcileId = this.GLAccountPM.AutomaticReconcileId;
            this.SetUIProperty();
            this.openAmountCurrency = args.openAmountCurrency;
            this.originalAmountCurrency = args.originalAmountCurrency;
            this.CheckIfThereIsDraftReconcile();
        }
    };
    ReconcileComponent.prototype.SetUIProperty = function () {
        if (this.GLAccountPM.IsMultiCurrency) {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }
    };
    ReconcileComponent.prototype.ngOnInit = function () {
        this.BuildColumns();
        //this.ColumnsReady.emit("");
    };
    Object.defineProperty(ReconcileComponent.prototype, "CurrencyId", {
        get: function () { return this.currencyId; },
        set: function (value) {
            if (this.currencyId != value) {
                this.currencyId = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.currencyFilter = new ApiQueryFilters_1.FilterItem("CurrencyId", value, null, null, "Equals", false, false, false, "string", false);
                }
                else {
                    this.currencyFilter = null;
                }
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReconcileComponent.prototype, "OpenAmount", {
        get: function () { return this.openAmount; },
        set: function (value) {
            if (this.openAmount != value) {
                this.openAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReconcileComponent.prototype, "SelectedOperator", {
        get: function () { return this.selectedOperator; },
        set: function (value) {
            if (this.selectedOperator != value) {
                this.selectedOperator = value;
                this.OpenAmountTextChanged(this.openAmount, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReconcileComponent.prototype, "AutomaticReconcileId", {
        get: function () { return this.automaticReconcileId; },
        set: function (value) {
            if (this.automaticReconcileId != value) {
                this.automaticReconcileId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReconcileComponent.prototype, "AutomaticReconcileMethodList", {
        get: function () { return this.automaticReconcile; },
        set: function (value) {
            if (this.automaticReconcile != value) {
                this.automaticReconcile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ReconcileComponent.prototype.AutomaticReconcileChanged = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.AutomaticReconcileMethodList = item;
        }
    };
    ReconcileComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(function () {
                _this.searchFieldFilter = new ApiQueryFilters_1.FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                _this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters(), MustIgnoreItems: _this.MustIgnoreItems }); // refresh grid
            }, 700);
        }
        else {
            this.searchFieldFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters(), MustIgnoreItems: this.MustIgnoreItems }); // refresh grid
        }
    };
    ReconcileComponent.prototype.OpenAmountTextChanged = function (searchtext, OperatorChanged) {
        var _this = this;
        if (OperatorChanged === void 0) { OperatorChanged = false; }
        if (!Tools_1.AppTool.IsNullOrEmpty(searchtext) && !Tools_1.AppTool.IsNullOrEmpty(this.SelectedOperator)) {
            this.timerToken = setTimeout(function () {
                var OpenAmountFilterOperator = _this.SelectedOperator.EnglishName.replace(/ /g, ''); // remove white spaces
                if (OpenAmountFilterOperator == "Equals") {
                    _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("OpenAmount", searchtext, -1 * searchtext, null, OpenAmountFilterOperator, false, false, false, "number", false);
                }
                else if (OpenAmountFilterOperator == "LessThan") {
                    searchtext = Math.abs(searchtext);
                    _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("OpenAmount", -1 * --searchtext, +searchtext, null, "Between", false, false, false, "number", false);
                }
                else if (OpenAmountFilterOperator == "LessThanOrEqual") {
                    searchtext = Math.abs(searchtext);
                    _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("OpenAmount", -1 * searchtext, +searchtext, null, "Between", false, false, false, "number", false);
                }
                else {
                    _this.openAmountFilter = new ApiQueryFilters_1.FilterItem("OpenAmount", searchtext, null, null, OpenAmountFilterOperator, false, false, false, "number", false);
                }
                _this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
            }, 700);
        }
        else {
            if (OperatorChanged == true && Tools_1.AppTool.IsNullOrEmpty(searchtext)) {
                return;
            }
            this.openAmountFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
        }
        var TempData = [];
        if (this.IsDraft == true) {
            this.SelectedLines.Collection.forEach(function (value, key) {
                if (value.ledgerTransaction.Mark == true) {
                    TempData.push(value);
                }
            });
        }
        this.SelectedLines = new ObservableCollection_1.ObservableCollection(TempData);
    };
    //#endregion
    //#region Buttons Handlers
    ReconcileComponent.prototype.ReconcilButton = function () {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = errors;
        // Local Validate
        if (this.SelectedLines.Length == 0) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NotransactionsSelected")); //"No transactions selected
        }
        // lines errors
        if (this.SelectedLines.Collection.find(function (d) { return d.isLineValid == false; })) {
            // errors.push(TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Reconciliations.O.ErrorsInSelectedLines"));
        }
        //multiple payment check
        var paymentsCount = this.SelectedLines.Collection.filter(function (d) { return d.SourceTypeCode == "3" || d.SourceTypeCode == "5"; }).length;
        if (paymentsCount > 1) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.CantIncludeTwoOrMorePayment"));
            this.ValidationErrorsList = errors;
            return;
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            //Adjust
            if (this.SelectedLines.Length > 0 && this.TotalsDeference != 0) {
                //errors.push(TextCodeTranslator.Translate("Accounting.General.O.DifferenceMustEqual0"));//"The difference must be equal to zero"
                //this.AdjustButton();
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 390;
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NewReconcileWithAdjusment"));
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.AdjustWithNewJournalScreen();
                    }
                    else if (confirmWindow.No) {
                    }
                });
                return;
            }
            //
            this.CurrentSession.StartBusyIndicatorSaving();
            var entity = this.CreateReconciliation();
            this.SubmitChanges(entity);
        }
    };
    ReconcileComponent.prototype.AutomaticReconcileButton = function () {
        var _this = this;
        this.IsAutoRecClicked = true;
        if (this.SelectedLines.Length > 0) {
            // Show prompt
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 390;
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.AutomaticReconcileWillClearAllSelectedLines"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    for (var i = 0; i < _this.SelectedLines.Collection.length; i++) {
                        var line = _this.SelectedLines.Collection[i]; //new LineModel(result[i], this, -1);
                        _this.FireCheckBoxChecked.emit({ rowData: line.LedgerTransactionPM, IsChecked: false, RowIndex: -1, ById: true });
                    }
                    _this.SelectedLines.Clear(); // = [];
                    _this.RunAutomaticReconcile();
                }
                else if (confirmWindow.No) {
                }
            });
        }
        else {
            this.RunAutomaticReconcile();
        }
    };
    ReconcileComponent.prototype.RunAutomaticReconcile = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        //#region filters
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filters.AdditionalFilters.push(this.openAmountFilter);
        }
        filters.PageSize = 30;
        filters.PageIndex = 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;
        //#endregion
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.PrepareTransactions")); //"Preparing Transactions..."
        var m1 = null;
        var m2 = null;
        var m3 = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AutomaticReconcileMethodList)) {
            m1 = this.AutomaticReconcileMethodList.AutomaticReconcile1;
            m2 = this.AutomaticReconcileMethodList.AutomaticReconcile2;
            m3 = this.AutomaticReconcileMethodList.AutomaticReconcile3;
        }
        this._LedgerTransactionExtendedListService.getAutomaticReconcileByFilter(m1, m2, m3, this.GLAccountPM.Id, filters).subscribe(function (myResult) {
            var mm = myResult;
            var result = mm.Result;
            if (!mm.HasError) {
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    _this.SelectedLines.Clear();
                    //this.SelectedLines = [];
                    var array = [];
                    for (var i = 0; i < result.length; i++) {
                        var line = new LineModel(result[i], _this, -1);
                        array.push(line);
                        //this.MarkIsChecked.emit({ MyRecord: result[i], AllRecords: result });
                        _this.FireCheckBoxChecked.emit({ rowData: line.LedgerTransactionPM, IsChecked: true, RowIndex: -1, ById: true });
                    }
                    _this.SelectedLines.InsertCollection(array);
                    //this.SelectedLines.Length = this.SelectedLines.length;
                    //this.SelectedLines.Changed.emit(this.SelectedLines);
                    if (_this.SelectedLines.Length == 0) {
                        _this.ShowEmptyAutoReco();
                    }
                    _this.CalculateTotals();
                }
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ReconcileComponent.prototype.SaveAsDraftButton = function () {
        var _this = this;
        if (this.SelectedLines.Length > 0) {
            // 1- prepare transactions
            var transactionsList = [];
            this.SelectedLines.Collection.forEach(function (lineModel) {
                var transaction = lineModel.LedgerTransactionPM;
                //transaction.Mark = !transaction.Mark; // the service will take this misson
                transactionsList.push(transaction);
            });
            // 2- call the service
            this.CurrentSession.StartBusyIndicatorSaving();
            this._ReconciliationExtendedPMService.delsertDraftLedgerTransaction(transactionsList).subscribe(function (serviceResponse) {
                console.log("_ReconciliationExtendedPMService.delsertDraftLedgerTransaction", serviceResponse);
                _this.CurrentSession.StopBusyIndicator();
                var result = serviceResponse.Result;
                var msg = new MessageWindow_1.MessageWindow();
                msg.ShowSuccessIcon = true;
                msg.RTL = _this.isRTL;
                msg.Width = 400;
                msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Reconciliations.Q.reconciliationwassavedas"));
                msg.WindowClosed.subscribe(function (event) {
                    _this.CancelButtonClicked();
                });
            });
        }
        else {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NotransactionsSelected")); //"No transactions selected!"
        }
    };
    ReconcileComponent.prototype.AdjustWithNewJournalScreen = function () {
        var _this = this;
        //this.ReloadScreen();
        if (this.SelectedLines.Length > 0 && this.TotalsDeference != 0) {
            //this.ToSend()
            var next = true;
            if (next) {
                this.CurrentSession.entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) {
                    _this.CurrentSession.entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(function (response) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 400;
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.B.Adjust");
                        logitudeWindow.WindowArgs = { "SelectedLines": _this.SelectedLines, "GLAccountPMId": _this.GLAccountPM.Id };
                        logitudeWindow.Show('./Accounting/Components/Others/JournalReconcileComponent');
                        logitudeWindow.WindowClosed.subscribe(function ($event) {
                            // Close Reconcile window
                            //this.CurrentSession.CloseCurrentWindow();
                            //this.CancelButtonClicked();
                            // Refresh Data
                            _this.ReloadScreen();
                        });
                    });
                });
            }
        }
        else {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.RTL = this.isRTL;
            myMessageWindow.Show("!(this.SelectedLines.Length > 0 && this.TotalsDeference) ");
        }
    };
    ReconcileComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ReconcileComponent.prototype.BuildColumns = function () {
        var _this = this;
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
            FieldName: 'SelectCheckBox',
            DataTypeCode: 'Boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'AccountingDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.AccountingDate"),
            Styles: { width: '105px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DocumentDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.DocumentDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DueDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.DueDate"),
            Styles: { width: '100px' },
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
            FieldName: 'OriginalAmount',
            DataTypeCode: 'String',
            //Display: 'Original Amount (' + this.originalAmountCurrency + ')',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.OriginalAmount") + ' (' + (this.GLAccountPM.IsMultiCurrency ? 'multi' : this.originalAmountCurrency) + ')',
            Styles: { width: '150px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true,
        });
        //this.columns.push({
        //    FieldName: 'OpenAmountCurrencyCode',
        //    DataTypeCode: 'String',
        //    Display: 'Open Amount Currency',
        //    Styles: { width: '120px' },
        //    IsCustomTemplate: true
        //});
        this.columns.push({
            FieldName: 'OpenAmount',
            DataTypeCode: 'String',
            //Display: 'Open Amount (' + this.openAmountCurrency + ')',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount") + ' (' + (this.GLAccountPM.IsMultiCurrency ? this.TenantPM.CurrencyCode : this.openAmountCurrency) + ')',
            Styles: { width: '114px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
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
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.JournalNumber"),
            Styles: { width: '80px' },
            HtmlListComponentName: 'GlAccountLedgerTransactionsListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/GlAccountLedgerTransactionsListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Notes',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("LedgerTransaction.F.Notes"),
            Styles: { width: '77px' },
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
                if (_this.MustIgnoreItems.filter(function (a) { return a.Id == rowId; }).length > 0) {
                    _this.MustIgnoreItems = _this.MustIgnoreItems.filter(function (a) { return a.Id != rowId; });
                }
                _this.MustIgnoreItems.push({ Id: rowId, IsChecked: isChecked });
                _this.FireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });
            }
        });
    };
    ReconcileComponent.prototype.GetIndicatorText = function (transaction) {
        var showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        if (transaction['OpenAmount'] != this.CalculateOriginalAmount(transaction))
            return showLocal ? 'סכום פתוח חלקית' : 'Partial transaction';
        else
            return showLocal ? 'סכום פתוח ' : 'Open transaction';
    };
    ReconcileComponent.prototype.onDataLoaded = function () {
        this.CheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue: true, IsAutoRecClicked: this.IsAutoRecClicked });
    };
    ReconcileComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        var filters = new ApiQueryFilters_1.ApiQueryFilters;
        //if (this.dateFilter) {
        //    filters.AdditionalFilters.push(this.dateFilter);
        //} else {
        //    return;
        //}
        if (this.currencyFilter) {
            filters.AdditionalFilters.push(this.currencyFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        if (this.openAmountFilter) {
            filters.AdditionalFilters.push(this.openAmountFilter);
        }
        filters.PageSize = take;
        filters.PageIndex = skip + 1; // decremented 1 in the service
        filters.GetAll = true;
        filters.GetCount = true;
        //filters.addAdditionalFilter("AccountingDate", true, null, null, "Between", false, false, false, "datetime");
        return this._entityListService.getOpenReconciliationsByFilter("LedgerTransaction", this.GLAccountPM.Id, filters); //this.ledgerTransactionListExtendedService.getByFilters(filters);
    };
    ReconcileComponent.prototype.PushLine = function (row, RowIndex) {
        var index = this.SelectedLines.Collection.findIndex(function (c) { return c.Id == row.Id; });
        if (index < 0) { // DNE
            row.AmountToReconcile = row.OpenAmount;
            var r = new LineModel(row, this, RowIndex);
            this.SelectedLines.Insert(r);
            //this.SelectedLines.push(r);
            this.CalculateTotals();
        }
    };
    ReconcileComponent.prototype.PopLine = function (id) {
        //var index = this.SelectedLines.findIndex(c => c.Id == id);
        //this.SelectedLines.splice(index, 1);
        this.SelectedLines.Remove(this.SelectedLines.Collection.find(function (c) { return c.Id == id; }));
        //ReconcileEventManager.RowUnselected.emit({ id: id });
        this.CalculateTotals();
    };
    ReconcileComponent.prototype.CheckBoxValueChanged = function (Row) {
        this.FireCheckBoxChecked.emit({ rowData: Row.LedgerTransactionPM, IsChecked: false, RowIndex: Row.myRowIndex, ById: true });
        this.PopLine(Row.LedgerTransactionPM.Id);
    };
    ReconcileComponent.prototype.CalculateOriginalAmount = function (row) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.GLAccountPM.ReconcileMethodCode)) {
            if (this.GLAccountPM.ReconcileMethodCode == "0") { // 0-local currency
                if (Tools_1.AppTool.IsNullOrZero(row.LocalAmountCredit)) {
                    return row.LocalAmountDebit;
                }
                else {
                    return row.LocalAmountCredit; //-1 *
                }
            }
            else if (this.GLAccountPM.ReconcileMethodCode == "1") { // 1-foreign currency
                if (Tools_1.AppTool.IsNullOrZero(row.ForeignAmountCredit)) {
                    return row.ForeignAmountDebit;
                }
                else {
                    return row.ForeignAmountCredit; //-1 *
                }
            }
        }
    };
    ReconcileComponent.prototype.ReloadScreen = function () {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() }); // refresh grid
        //this.SelectedLines = [];
        this.SelectedLines.Clear();
        this.CalculateTotals();
    };
    ReconcileComponent.prototype.CalculateTotals = function () {
        this.TotalCredit = 0;
        this.TotalDebit = 0;
        for (var _i = 0, _a = this.SelectedLines.Collection; _i < _a.length; _i++) {
            var line = _a[_i];
            if (line.AmountToReconcile < 0)
                this.TotalDebit += +line.AmountToReconcile * -1; //cast number
            else
                this.TotalCredit += +line.AmountToReconcile;
            // due this.TotalCredit + amountToReconcile;  == 335.78999999999996 <>335.79
            this.TotalDebit = Tools_1.AppTool.Round(this.TotalDebit, 2);
            this.TotalCredit = Tools_1.AppTool.Round(this.TotalCredit, 2);
        }
        var def = (this.TotalCredit - this.TotalDebit);
        this.TotalsDeference = def < 0 ? def * -1 : def;
    };
    //#endregion
    ReconcileComponent.prototype.CreateReconciliation = function () {
        var newEntity = {};
        newEntity.Id = "new";
        newEntity.ChangeSetOp = 1;
        newEntity.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newEntity.AccountId = this.GLAccountPM.Id;
        newEntity.Number = "get";
        newEntity.CreateDate = new Date();
        newEntity.CreatedByUserId = null;
        newEntity.CreatedByUserId = null;
        newEntity.AccountCurrencyId = this.GLAccountPM.CurrencyId;
        newEntity.CurrencyCode = this.GLAccountPM.CurrencyCode;
        newEntity.AccountReconcileMethodCode = this.GLAccountPM.ReconcileMethodCode;
        newEntity.ReconciliationLines = [];
        for (var i = 0; i < this.SelectedLines.Length; i++) {
            var selectedTransaction = this.SelectedLines.Collection[i];
            var newLine = {};
            newLine.ChangeSetOp = "1";
            newLine.ReconciliationId = newEntity.Id;
            newLine.Tenant = newEntity.Tenant;
            newLine.Line = i;
            newLine.CurrencyId = selectedTransaction.OpenAmountCurrencyId;
            newLine.TransactionId = selectedTransaction.Id;
            newLine.ReconciliationAmount = selectedTransaction.AmountToReconcile;
            newLine.IsPartial = selectedTransaction.IsPartial;
            newLine.GroupNumber = selectedTransaction.GroupHash;
            newEntity.ReconciliationLines.push(newLine);
        }
        return newEntity;
    };
    ReconcileComponent.prototype.SubmitChanges = function (entity) {
        var _this = this;
        this._ReconciliationExtendedPMService.insert(entity).subscribe(function (myResult) {
            var mm = myResult;
            var _callback = mm.Result;
            if (!mm.HasError) {
                //var windowArgs: any = {};
                //windowArgs.ReconciliationPM = entity;
                //var logitudeWindow = new LogitudeWindow();
                //logitudeWindow.Width = 400;
                //logitudeWindow.Height = 140;
                //logitudeWindow.Title = "Reconciled";
                //logitudeWindow.WindowArgs = windowArgs;
                //logitudeWindow.Show('./Accounting/Components/Others/ReconciledMessage');
                //logitudeWindow.WindowClosed.subscribe(($event: any) => {
                //    // Close Reconcile window
                //    //this.CurrentSession.CloseCurrentWindow();
                //    //this.CancelButtonClicked();
                //    // Refresh Data
                //    this.ReloadScreen();
                //});
                //logitudeWindow.ComponentLoaded.subscribe(($event: any) => {
                //    this.timerToken = setTimeout(() => {
                //        logitudeWindow.Close("ok");
                //    }, 5000);
                //});
                if (_callback) {
                    _this.RecoPM = _callback.reconciliationPM;
                }
                _this.recoCallback = _callback;
                _this.ShowSuccessAlert();
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ReconcileComponent.prototype.OpenSource = function (id, sourceTypeCode) {
        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = "Journal";
        switch (sourceTypeCode) {
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
    ReconcileComponent.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    ReconcileComponent.prototype.OpenReco = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.RecoPM.Id)) {
            this.showAlert = false;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.RecoPM.Id, ObjectTableName: 'Reconciliation', BackButtonLabel: 'Back' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this.CurrentSession.CloseCurrentWindow();
                });
            });
        }
    };
    ReconcileComponent.prototype.ShowSuccessAlert = function () {
        var _this = this;
        this.ReloadScreen();
        this.showAlert = true;
        this.timerToken = setTimeout(function () {
            _this.showAlert = false;
        }, 5000); // 5 sec
    };
    ReconcileComponent.prototype.ShowEmptyAutoReco = function () {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Width = 400;
        messageWindow.Height = 150;
        messageWindow.RTL = this.isRTL;
        messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.NoReconcile");
        messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NoReconciliationFound"));
    };
    ReconcileComponent.prototype.CloseAlert = function () {
        this.showAlert = false;
    };
    ReconcileComponent.prototype.CheckIfThereIsDraftReconcile = function () {
        var _this = this;
        this._ReconciliationExtendedPMService.getDraftReconciliations(this.GLAccountPM.Id).subscribe(function (response) {
            console.log("_ReconciliationExtendedPMService.getDraftReconciliations", response);
            var list = response.Result;
            if (list && list.length > 0) {
                var array = [];
                for (var i = 0; i < list.length; i++) {
                    var line = new LineModel(list[i], _this, -1);
                    array.push(line);
                }
                _this.SelectedLines.InsertCollection(array);
                _this.CalculateTotals();
                _this.isDraftReconciliation = true;
                //set first grid checkboxs
                //this.SelectedLines.Collection.forEach((elem: LineModel) => {
                //    this.FireCheckBoxChecked.emit({ rowData: elem.LedgerTransactionPM, IsChecked: true, RowIndex: elem.RowIndex });
                //    console.log("event fired for:", { rowData: elem.LedgerTransactionPM, IsChecked: true, RowIndex: elem.RowIndex } );
                //});
                //this.CheckBoxFilterChanged.emit({ UseFilteredCheckBox: true, FilteredRecordsCheckedFieldName: "Mark", FilteredRecordsCheckedFieldValue:"true"});
                // Show confirm window
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Reconciliations.Q.ThereisUncompletedReconciliation"));
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        if (_this.IsAutoRecClicked == true) {
                            _this.IsAutoRecClicked = false;
                        }
                        _this.IsDraft = true;
                    }
                    else if (confirmWindow.No) {
                        // Delete draft transactions
                        _this.DeleteDraftReconciliation();
                        _this.IsDraft = false;
                    }
                });
            }
        });
    };
    ReconcileComponent.prototype.DeleteDraftReconciliation = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._ReconciliationExtendedPMService.deleteResetDraftOpenReconciliation(this.GLAccountPM.Id).subscribe(function (response) {
            console.log("_ReconciliationExtendedPMService.deleteResetDraftOpenReconciliation", response);
            _this.CurrentSession.StopBusyIndicator();
            _this.SelectedLines.Clear();
            _this.ReloadScreen();
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ReconcileComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ReconcileComponent.prototype, "onQueryChangeEvent", void 0);
    ReconcileComponent = __decorate([
        core_1.Component({
            selector: 'ReconcileComponent',
            moduleId: './Accounting/Components/Others/',
            providers: [EntityListService_1.EntityListService],
            templateUrl: 'ReconcileComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], ReconcileComponent);
    return ReconcileComponent;
}(BaseComponent_1.BaseComponent));
exports.ReconcileComponent = ReconcileComponent;
//# sourceMappingURL=ReconcileComponent.js.map