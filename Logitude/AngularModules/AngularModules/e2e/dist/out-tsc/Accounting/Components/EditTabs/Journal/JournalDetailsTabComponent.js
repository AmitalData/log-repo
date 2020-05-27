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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var JournalLinePM_1 = require("../../../EntityPMs/JournalLinePM");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var AccountingPeriodExtendedListService_1 = require("../../../Services/ExtendedLists/AccountingPeriodExtendedListService");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var AccountingPeriodListService_1 = require("../../../Services/StandardLists/AccountingPeriodListService");
var RatesTableExtendedListService_1 = require("../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var JournalValidator_1 = require("../../../Validators/JournalValidator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var JournalDetailsTabComponent = /** @class */ (function (_super) {
    __extends(JournalDetailsTabComponent, _super);
    function JournalDetailsTabComponent(entityArgs, currencyListService, accountingPeriodListService, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.currencyListService = currencyListService;
        _this.accountingPeriodListService = accountingPeriodListService;
        _this.CD = CD;
        _this.EntityPM = null;
        _this.ObjectTableName = "Journal";
        _this.DataContext = _this;
        _this.defaultCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        _this.creditTotal = 0;
        _this.debitTotal = 0;
        _this.journalDisabled = false;
        _this.forceFocus = false;
        _this.PointerEvents = 'auto';
        _this.Opacity = "1";
        _this.Approved = false;
        _this.AccountingPeriods = [];
        _this._AccountingPeriodListService = new AccountingPeriodListService_1.AccountingPeriodListService();
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.TabSelectedEvent = null;
        _this.txt_Reference = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Reference");
        _this.txt_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("JournalLine.F.LocalAmount");
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.JournalLines = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityPM = entityArgs.EntityPM;
        if (_this.AccountingDate == null)
            _this.AccountingDate = new Date();
        _this.FillGrid();
        _this.SetUIProperties();
        // redraw
        _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isSuccess) {
            if (isSuccess) {
                _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                _this.FillGrid();
                _this.SetUIProperties();
            }
        });
        _this.Listen();
        return _this;
    }
    JournalDetailsTabComponent.prototype.OnRowEnded = function ($event) {
        console.log("this.JournalLines.Length : " + this.JournalLines.Length);
        if (($event) == this.JournalLines.Length) {
            this.AddLine();
            //this.CurrentSession.ResetRowIndex();
        }
    };
    JournalDetailsTabComponent.prototype.OnFocus = function () {
        if (this.JournalLines.Length == 0) {
            this.AddLine();
        }
    };
    JournalDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.FillGrid();
                        _this.SetUIProperties();
                    }
                });
            }
            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.FillGrid();
                        _this.SetUIProperties();
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    };
    JournalDetailsTabComponent.prototype.SetUIProperties = function () {
        //Display only
        if (this.EntityPM.StatusCode == "3") { // 3-Voided and 2-Approved
            //disable controls
            this.journalDisabled = true;
            this.PointerEvents = 'none';
            this.Opacity = "1";
        }
        else if (this.EntityPM.StatusCode == "2") { // 3-Voided and 2-Approved
            //disable controls
            this.journalDisabled = true;
            this.PointerEvents = 'none';
            this.Opacity = "1";
            this.referencesDivHeight = 0;
            this.Approved = true;
            this.UIProperties.SetVisibility("Reference1", "Journal", false);
            this.UIProperties.SetVisibility("Reference2", "Journal", false);
            this.UIProperties.SetVisibility("Reference3", "Journal", false);
            this.UIProperties.SetVisibility("Notes", "Journal", false);
        }
    };
    JournalDetailsTabComponent.prototype.FillGrid = function () {
        // if entity in edit mode
        if (this.EntityPM.Id != undefined) {
            var tempItemSource = [];
            if (this.EntityPM.JournalLines != null) {
                for (var i = 0; i < this.EntityPM.JournalLines.length; i++) {
                    var line = new JournalLineModel(this.EntityPM.JournalLines[i], this);
                    tempItemSource.push(line);
                    //this.JournalLines.Insert(line);
                }
                this.JournalLines.InsertCollection(tempItemSource);
                //for (let item of this.EntityPM.JournalLines) {
                //    var line = new JournalLineModel(item, this);
                //    this.JournalLines.Insert(line);
                //}
            }
            this.CalculateTotals();
        }
        else {
            this.EntityPM.StatusCode = "0"; // Draft
            this.EntityPM.TypeCode = "0"; // Manual
            this.EntityPM.AccountingEntityCode = "1"; // Journal
            this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            var journalLine = new JournalLinePM_1.JournalLinePM(this.EntityPM);
            journalLine.Line = 1;
            journalLine.Tenant = this.EntityPM.Tenant;
            this.EntityPM.AddJournalLine(journalLine);
            var line = new JournalLineModel(journalLine, this);
            this.JournalLines.Insert(line);
        }
    };
    JournalDetailsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.CurrentSession.LostFocusEvent.subscribe(function (res) {
            if (_this.CD) {
                var isDestroyed = _this.CD['destroyed'];
                if (!isDestroyed) {
                    _this.CD.detectChanges();
                    //console.log("AfterLostFocus");
                }
            }
        });
        this.GetDefaultValues();
        //set focus on accounting date
        var t = setTimeout(function () { _this.forceFocus = true; }, 1);
    };
    Object.defineProperty(JournalDetailsTabComponent.prototype, "Reference1", {
        get: function () { return this.reference1; },
        set: function (value) {
            if (this.reference1 != value) {
                for (var _i = 0, _a = this.JournalLines.Collection; _i < _a.length; _i++) {
                    var line = _a[_i];
                    if (line.Reference1 == this.reference1) {
                        line.Reference1 = value;
                    }
                }
                this.reference1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "Reference2", {
        get: function () { return this.reference2; },
        set: function (value) {
            if (this.reference2 != value) {
                for (var _i = 0, _a = this.JournalLines.Collection; _i < _a.length; _i++) {
                    var line = _a[_i];
                    if (line.Reference2 == this.reference2) {
                        line.Reference2 = value;
                    }
                }
                this.reference2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "Reference3", {
        get: function () { return this.reference3; },
        set: function (value) {
            if (this.reference3 != value) {
                for (var _i = 0, _a = this.JournalLines.Collection; _i < _a.length; _i++) {
                    var line = _a[_i];
                    if (line.Reference3 == this.reference3) {
                        line.Reference3 = value;
                    }
                }
                this.reference3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (value) {
            if (this.notes != value) {
                for (var _i = 0, _a = this.JournalLines.Collection; _i < _a.length; _i++) {
                    var line = _a[_i];
                    if (line.Notes == this.Notes) {
                        line.Notes = value;
                    }
                }
                this.notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "Currency", {
        get: function () { return this.currency; },
        set: function (value) {
            if (this.currency != value) {
                this.currency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "AccountingDate", {
        get: function () { return this.EntityPM.AccountingDate; },
        set: function (value) {
            if (this.EntityPM.AccountingDate != value) {
                if (value != null) {
                    //CLOSED MONTH VALIDATION
                    // Get Accounting Period by year
                    var accountingPeriod = this.AccountingPeriods.find(function (d) { return d.Year == value.getFullYear(); });
                    if (accountingPeriod) {
                        var month = value.getMonth() + 1;
                        // Valid Month => (ClosedMonth < month <= OpenMonth)
                        if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = []; // empty errors list
                        }
                        else { // invalid (closed month)
                            // push the error to errors list
                            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth");
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(msg);
                            this.EntityPM.AccountingDate = value;
                            return;
                        }
                    }
                    //FUTURE DATE VALIDATION
                    if (value > Tools_1.DateTool.GetCurrentDateTimeAsUtc()) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.M.FutureDateForbidden");
                        this.UIProperties.SetValidity("AccountingDate", this.ObjectTableName, false, msg);
                        // push the error to errors list
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(msg);
                        this.EntityPM.AccountingDate = value;
                        return;
                    }
                    else {
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = []; // empty errors list
                        this.UIProperties.SetValidity("AccountingDate", this.ObjectTableName, true, "OK");
                    }
                }
                this.EntityPM.AccountingDate = value;
                this.UpdateLinesAccountingDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "AccountingPeriod", {
        get: function () { return this.accountingPeriod; },
        set: function (value) {
            if (this.accountingPeriod != value) {
                this.accountingPeriod = value;
            }
            if (value != null) {
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalDetailsTabComponent.prototype, "LocalAmountHeader", {
        get: function () { return this.localAmountHeader; },
        set: function (value) {
            if (this.localAmountHeader != value) {
                this.localAmountHeader = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    JournalDetailsTabComponent.prototype.AddLine = function () {
        if (this.journalDisabled)
            return;
        var errors = [];
        if (this.JournalLines.Collection.length > 0) {
            // Validation
            var lastRow = this.JournalLines.Collection[this.JournalLines.Collection.length - 1];
            errors = JournalValidator_1.JournalValidator.ValidateJournalLine(lastRow);
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            if (errors.length > 0) {
                return;
            }
            //lastRow.SplittedCheck();
        }
        //
        // Adding New Line
        var journalLine = new JournalLinePM_1.JournalLinePM(this.EntityPM);
        journalLine.Tenant = this.EntityPM.Tenant;
        this.EntityPM.AddJournalLine(journalLine);
        journalLine.Line = this.JournalLines.Collection.length > 0 ? (lastRow.Line + 1) : 1;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM)) {
            journalLine.JournalId = this.EntityPM.Id;
        }
        var line = new JournalLineModel(journalLine, this);
        line.Reference1 = this.reference1;
        line.Reference2 = this.reference2;
        line.Reference3 = this.reference3;
        line.Notes = this.notes;
        this.JournalLines.Insert(line);
    };
    JournalDetailsTabComponent.prototype.DetectChanges = function () {
        this.CD.detectChanges();
    };
    JournalDetailsTabComponent.prototype.RemoveLine = function (line) {
        var _this = this;
        if (this.journalDisabled)
            return;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Areyousuredeleteline") + " " + line.Line + " ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.JournalLines.Remove(line);
                _this.EntityPM.JournalLines.splice(line.Line - 1, 1);
                //var ItemsSource = [];
                // Recalculate line numbers
                for (var i = 0; i < _this.JournalLines.Collection.length; i++) {
                    var oldItem = _this.JournalLines.Collection[i];
                    var updatedItem = _this.JournalLines.Collection[i];
                    updatedItem.Line = i + 1;
                    _this.JournalLines.Update(oldItem, updatedItem);
                }
                //this.JournalLines.Collection.forEach((item) => {
                //    ItemsSource.push(item);
                //});
                //this.JournalLines = ItemsSource;
                _this.CalculateTotals();
            }
        });
    };
    JournalDetailsTabComponent.prototype.TextChanged = function (searchtext) {
        //console.log(this.JournalLines);
        //console.log(this.EntityPM.JournalLines);
    };
    JournalDetailsTabComponent.prototype.GetDefaultValues = function () {
        var _this = this;
        // Tenant currency
        this.currencyListService.getSingle(this.defaultCurrencyId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.Currency = myResponse.Result;
                    _this.localAmountHeader = "Amount (" + myResponse.Result.Code + ")";
                    console.log(">>Tenant Currency: ", myResponse.Result);
                }
            }
        });
        this.GetAccountingPeriods();
        // Current Accouting Period
        //var periodTypeCode = "1" // 1-Regular
        //this.accountingPeriodListService.getByYear(new Date().getFullYear(), periodTypeCode).subscribe((myResponse: ServiceResponse) => {
        //    if (myResponse != null) {
        //        if (!myResponse.HasError) {
        //            this.AccountingPeriod = myResponse.Result;
        //            console.log(">>Current Accounting Period: ", myResponse.Result);
        //        }
        //    }
        //});
    };
    JournalDetailsTabComponent.prototype.CalculateTotals = function () {
        this.creditTotal = this.debitTotal = 0;
        for (var _i = 0, _a = this.JournalLines.Collection; _i < _a.length; _i++) {
            var line = _a[_i];
            if (!Tools_1.AppTool.IsNullOrEmpty(line.LocalAmount)) {
                if (line.ActionCode == "1")
                    this.creditTotal += line.LocalAmount;
                else if (line.ActionCode == "2")
                    this.debitTotal += line.LocalAmount;
                else if (line.ActionCode == "3") {
                    this.creditTotal += line.LocalAmount;
                    this.debitTotal += line.LocalAmount;
                }
                else if (line.ActionCode == "4") {
                    this.creditTotal += line.LocalAmount;
                    this.debitTotal += line.LocalAmount;
                }
            }
        }
    };
    JournalDetailsTabComponent.prototype.GetAccountingPeriods = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters(true);
        filters.addAdditionalFilter("PeriodTypeCode", "1", null, null, "Equals", false, false, false, "string"); // 1-Regular
        this._AccountingPeriodListService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.AccountingPeriods = myResponse.Result;
                    console.log(">>Accounting Periods: ", myResponse.Result);
                }
            }
        });
    };
    JournalDetailsTabComponent.prototype.UpdateLinesAccountingDates = function () {
        var _this = this;
        var lines = this.JournalLines.Collection;
        if (lines) {
            lines.forEach(function (line) {
                var headerDate = _this.AccountingDate;
                if (headerDate) {
                    var header_year = headerDate.getFullYear();
                    var header_month = headerDate.getMonth() + 1;
                    var header_day = headerDate.getDate();
                    var lineDate = new Date(line.AccountingDate.toString()); // somtimes this.AccountingDate contains string date o.O
                    var line_year = lineDate.getFullYear();
                    var line_month = lineDate.getMonth() + 1;
                    var line_day = lineDate.getDate();
                    if (line_year != header_year)
                        line_year = header_year;
                    if (line_month != header_month)
                        line_month = header_month;
                    //validate day according to month
                    if (line_day > _this.lastDay(line_year, line_month - 1)) {
                        // Set new Date
                        lineDate = null;
                        line.AccDay = null;
                        console.log("[!] the value of day (" + line_day + ") is outside month range (" + line_month + ")");
                    }
                    else {
                        // Set new Date
                        lineDate.setFullYear(line_year);
                        lineDate.setMonth(line_month - 1);
                        lineDate.setDate(line_day);
                        console.log("[!] AccountingDate for line " + line.Line + " is changed to " + lineDate.toString());
                    }
                }
            });
        }
    };
    JournalDetailsTabComponent.prototype.lastDay = function (year, month) {
        return new Date(year, month + 1, 0).getDate();
    };
    JournalDetailsTabComponent.prototype.Test = function () {
        var entity = this.EntityPM;
        var lines = this.JournalLines;
        console.log("[TEST] ", entity, this.JournalLines);
    };
    JournalDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './JournalDetailsTabComponent.html',
            providers: [CurrencyListService_1.CurrencyListService,
                AccountingPeriodExtendedListService_1.AccountingPeriodExtendedListService,
                RatesTableExtendedListService_1.RatesTableExtendedListService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs,
            CurrencyListService_1.CurrencyListService,
            AccountingPeriodExtendedListService_1.AccountingPeriodExtendedListService,
            core_1.ChangeDetectorRef])
    ], JournalDetailsTabComponent);
    return JournalDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.JournalDetailsTabComponent = JournalDetailsTabComponent;
var JournalLineModel = /** @class */ (function (_super) {
    __extends(JournalLineModel, _super);
    function JournalLineModel(journalLine, parent) {
        var _this = _super.call(this) || this;
        _this.journalLine = journalLine;
        _this.parent = parent;
        _this.JournalLinePM = null;
        _this.ObjectTableName = "JournalLine";
        _this.DataContext = _this;
        _this.accountingDayMustBeInRange = TextCodeTranslator_1.TextCodeTranslator.Translate("Journal.O.TheAccountingDayMustBeInRange");
        _this.isValid = true;
        _this.__UserCanSetRateManually = false; // user can set rate manually by insert forign amount with local amount empty (see WI 24999)
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isRateCoverted = false;
        _this.isRateManualy = false;
        _this.isLocalEntered = false;
        _this.isForeignEntered = false;
        _this.isMouseIn = false;
        _this.EntityPM = _this.parent.EntityPM;
        _this.JournalLinePM = journalLine;
        if (_this.JournalLinePM.AccountingDate) {
        }
        else {
            _this.AccountingDate = _this.parent.AccountingDate;
        }
        // Set Acc. Day from journalLine.AccountingDay
        if (_this.AccountingDate) {
            var date = new Date(_this.AccountingDate.toString());
            _this.accDay = date.getDate();
        }
        _this.ratesTableExtendedListService = new RatesTableExtendedListService_1.RatesTableExtendedListService();
        _this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        //#region initialize query filters for Accounts LOV
        _this.CreditAccountFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.CreditAccountFilterItems.addAdditionalFilter("AccountTypeCode", "5,4", null, null, "Exclude", false, false, false, "string", false, true);
        _this.DebitAccountFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.DebitAccountFilterItems.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string", false, true);
        return _this;
        //#endregion
    }
    Object.defineProperty(JournalLineModel.prototype, "AccountingDate", {
        get: function () { return this.JournalLinePM.AccountingDate; },
        set: function (value) {
            if (this.JournalLinePM.AccountingDate != value) {
                this.JournalLinePM.AccountingDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "currencyRate", {
        get: function () { return this.JournalLinePM.ExchangeRate; },
        set: function (value) {
            if (this.JournalLinePM.ExchangeRate != value) {
                this.JournalLinePM.ExchangeRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "Line", {
        //#region Line Original Properties
        get: function () { return this.JournalLinePM.Line; },
        set: function (value) {
            if (this.JournalLinePM.Line != value) {
                this.JournalLinePM.Line = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    JournalLineModel.prototype.OnSelectedItemChanged = function ($event) {
        console.log($event);
    };
    Object.defineProperty(JournalLineModel.prototype, "ActionCode", {
        get: function () { return this.JournalLinePM.ActionCode; },
        set: function (value) {
            if (this.JournalLinePM.ActionCode != value) {
                this.JournalLinePM.ActionCode = value;
                this.parent.CalculateTotals();
            }
            if (value != null) {
                this.CurrencyId = null;
                this.Currency = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "ActionName", {
        get: function () { return this.JournalLinePM.ActionName == null ? "" : this.JournalLinePM.ActionName; },
        set: function (value) {
            if (this.JournalLinePM.ActionName != value) {
                this.JournalLinePM.ActionName = value;
                //alert(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "JournalActionType", {
        get: function () { return this.journalActionType; },
        set: function (value) {
            if (this.journalActionType != value) {
                this.journalActionType = value;
                if (value != null) {
                    this.ActionCode = value.Code;
                    this.ActionName = value.LocalName;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "DocumentDate", {
        get: function () { return this.JournalLinePM.DocumentDate; },
        set: function (value) {
            if (this.JournalLinePM.DocumentDate != value) {
                this.JournalLinePM.DocumentDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "DueDate", {
        get: function () { return this.JournalLinePM.DueDate; },
        set: function (value) {
            if (this.JournalLinePM.DueDate != value) {
                this.JournalLinePM.DueDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "CreditAccountId", {
        get: function () { return this.JournalLinePM.CreditAccountId; },
        set: function (value) {
            if (this.JournalLinePM.CreditAccountId != value) {
                this.JournalLinePM.CreditAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "DebitAccountId", {
        get: function () { return this.JournalLinePM.DebitAccountId; },
        set: function (value) {
            if (this.JournalLinePM.DebitAccountId != value) {
                this.JournalLinePM.DebitAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "CurrencyId", {
        // ساحة المعركة
        get: function () { return this.JournalLinePM.CurrencyId; },
        set: function (value) {
            var _this = this;
            if (this.JournalLinePM.CurrencyId != value) {
                this.JournalLinePM.CurrencyId = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value) && !Tools_1.AppTool.IsNullOrEmpty(this.parent.currency)) {
                    if (value != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                        this.ratesTableExtendedListService.getClosestRate(this.parent.currency.Id, value).subscribe(function (myResponse) {
                            if (myResponse != null) {
                                if (!myResponse.HasError) {
                                    if (myResponse.Result != undefined && myResponse.Result != null) {
                                        _this.isRateManualy = false;
                                        var rate = myResponse.Result;
                                        _this.currencyRate = rate.Rate;
                                        // Recalculate local amount
                                        if (_this.LocalAmount) {
                                            _this.isRateCoverted = true;
                                            _this.ForeignAmount = (_this.LocalAmount / _this.currencyRate);
                                        }
                                        else if (_this.ForeignAmount) {
                                            _this.isRateCoverted = true;
                                            _this.LocalAmount = (_this.ForeignAmount * _this.currencyRate);
                                        }
                                        console.log(">Ex. Rate: ", _this.currencyRate);
                                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                                    }
                                    else {
                                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("The selected currency does not have Exchange Rate!");
                                        _this.LocalAmount = null;
                                        _this.ForeignAmount = null;
                                    }
                                }
                            }
                        });
                    }
                    else {
                        // Local Currency
                        this.isRateManualy = false;
                        this.currencyRate = 1;
                        if (this.LocalAmount) {
                            this.isRateCoverted = true;
                            this.ForeignAmount = (this.LocalAmount / this.currencyRate);
                        }
                        else if (this.ForeignAmount) {
                            this.isRateCoverted = true;
                            this.LocalAmount = (this.ForeignAmount * this.currencyRate);
                        }
                    }
                }
                else {
                    //this.CurrencyCode = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "LocalAmount", {
        // [!]
        // [!] Warning!! Any changes in one function must done in another function
        // [!]
        // SYNCRONIZED CODE WITH ForeignAmount
        get: function () { return this.JournalLinePM.LocalAmount; },
        set: function (value) {
            if (this.JournalLinePM.LocalAmount != value) {
                // set value
                this.JournalLinePM.LocalAmount = value;
                this.parent.CalculateTotals();
                // convert amount
                if (!Tools_1.AppTool.IsNullOrEmpty(value) && this.CurrencyId && this.currencyRate) {
                    if (!this.isRateCoverted) {
                        if (this.isForeignEntered)
                            this.isRateManualy = true;
                        this.isLocalEntered = true;
                    }
                    if (!this.isForeignEntered) {
                        this.isRateCoverted = true;
                        this.ForeignAmount = (value / this.currencyRate);
                    }
                    else {
                        this.isRateCoverted = false;
                    }
                }
                else {
                    this.isLocalEntered = false;
                    this.isForeignEntered = false;
                    this.ForeignAmount = null;
                    this.isRateManualy = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "ForeignAmount", {
        // SYNCRONIZED CODE WITH LocalAmount
        get: function () { return this.JournalLinePM.ForeignAmount; },
        set: function (value) {
            if (this.JournalLinePM.ForeignAmount != value) {
                // set value
                this.JournalLinePM.ForeignAmount = value;
                this.parent.CalculateTotals();
                // convert amount
                if (value != null && this.CurrencyId && this.currencyRate) {
                    if (!this.isRateCoverted) {
                        if (this.isLocalEntered)
                            this.isRateManualy = true;
                        this.isForeignEntered = true;
                    }
                    if (!this.isLocalEntered) {
                        this.isRateCoverted = true;
                        this.LocalAmount = (value * this.currencyRate);
                    }
                    else {
                        this.isRateCoverted = false;
                    }
                }
                else {
                    this.isLocalEntered = false;
                    this.isForeignEntered = false;
                    this.LocalAmount = null;
                    this.isRateManualy = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "Reference1", {
        // [!]
        // [!]
        get: function () { return this.JournalLinePM.Reference1; },
        set: function (value) {
            if (this.JournalLinePM.Reference1 != value) {
                this.JournalLinePM.Reference1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "Reference2", {
        get: function () { return this.JournalLinePM.Reference2; },
        set: function (value) {
            if (this.JournalLinePM.Reference2 != value) {
                this.JournalLinePM.Reference2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "Reference3", {
        get: function () { return this.JournalLinePM.Reference3; },
        set: function (value) {
            if (this.JournalLinePM.Reference3 != value) {
                this.JournalLinePM.Reference3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "Notes", {
        get: function () { return this.JournalLinePM.Notes; },
        set: function (value) {
            if (this.JournalLinePM.Notes != value) {
                this.JournalLinePM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "CreditAccountName", {
        //#endregion
        get: function () { return this.JournalLinePM.CreditAccountName; },
        set: function (value) {
            if (this.JournalLinePM.CreditAccountName != value) {
                this.JournalLinePM.CreditAccountName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "DebitAccountName", {
        get: function () { return this.JournalLinePM.DebitAccountName; },
        set: function (value) {
            if (this.JournalLinePM.DebitAccountName != value) {
                this.JournalLinePM.DebitAccountName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "CurrencyCode", {
        get: function () { return this.JournalLinePM.CurrencyCode; },
        set: function (value) {
            if (this.JournalLinePM.CurrencyCode != value) {
                this.JournalLinePM.CurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "CreditAccount", {
        get: function () { return this.creditAccount; },
        set: function (value) {
            //console.log("-creditAccount-");
            if (this.creditAccount != value) {
                this.creditAccount = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.CreditAccountName = value.LocalName;
                if (this.ActionCode == "1" && !this.creditAccount.IsMultiCurrency) {
                    this.CurrencyId = this.creditAccount.CurrencyId;
                    this.CurrencyCode = this.creditAccount.CurrencyCode;
                }
                else if (this.ActionCode == "3" && !this.creditAccount.IsMultiCurrency) {
                    this.CurrencyId = this.creditAccount.CurrencyId;
                    this.CurrencyCode = this.creditAccount.CurrencyCode;
                }
                else {
                    //this.CurrencyId = null;
                    //this.CurrencyCode = null;
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Currency)) {
                    this.SplittedCheck();
                }
            }
            else {
                this.CreditAccountName = null;
                this.CreditAccountId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "DebitAccount", {
        get: function () { return this.debitAccount; },
        set: function (value) {
            //console.log("-debitAccount-");
            if (this.debitAccount != value) {
                this.debitAccount = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.DebitAccountName = value.LocalName;
                if (this.ActionCode == "2" && !this.debitAccount.IsMultiCurrency) {
                    this.CurrencyId = this.debitAccount.CurrencyId;
                    this.CurrencyCode = this.debitAccount.CurrencyCode;
                }
                else if (this.ActionCode == "3" && !this.debitAccount.IsMultiCurrency) {
                    this.CurrencyId = this.debitAccount.CurrencyId;
                    this.CurrencyCode = this.debitAccount.CurrencyCode;
                }
                else {
                    //this.CurrencyId = null;
                    //this.CurrencyCode = null;
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Currency)) {
                    this.SplittedCheck();
                }
            }
            else {
                this.DebitAccountName = null;
                this.DebitAccountId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalLineModel.prototype, "AccDay", {
        get: function () {
            return this.accDay;
        },
        set: function (value) {
            if (this.accDay != value) {
                this.accDay = value;
                //1- check parent accounting date if changed?
                if (this.parent.AccountingDate != this.AccountingDate) {
                    this.AccountingDate = this.parent.AccountingDate;
                }
                //2- check day range
                var date = new Date(this.AccountingDate.toString()); // somtimes this.AccountingDate contains string date o.O
                var newDate = new Date();
                newDate.setUTCFullYear(date.getFullYear());
                newDate.setUTCMonth(date.getMonth());
                newDate.setUTCDate(date.getDate());
                newDate.setUTCHours(0);
                newDate.setUTCMinutes(0);
                newDate.setUTCSeconds(0);
                newDate.setUTCMilliseconds(0);
                date = newDate;
                this.IsAccDayValid(date, value);
                var valid = JournalValidator_1.JournalValidator.IsAccDayValid(date, value);
                this.isValid = valid;
                if (valid) {
                    this.UIProperties.SetValidity("AccDay", this.ObjectTableName, true, "valid");
                }
                else {
                    this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
                }
                //3- set the accounting date with new day
                this.AccountingDate.setUTCDate(date.getDate());
            }
        },
        enumerable: true,
        configurable: true
    });
    JournalLineModel.prototype.IsAccDayValid = function (date, day) {
        if (day > 0 && day < 32) {
            var lastDayOfMonth = this.lastDay(date.getFullYear(), date.getMonth());
            if (day > lastDayOfMonth) {
                //error
                this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
                this.isValid = false;
                return false;
                //var t = setTimeout(() => {
                //    this.AccDay = value;
                //});
            }
            else {
                this.UIProperties.SetValidity("AccDay", this.ObjectTableName, true, "valid");
                this.AccountingDate = new Date(date.setDate(day));
                this.isValid = true;
                return true;
            }
        }
        else {
            //error
            this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
            this.isValid = false;
            //var t = setTimeout(() => {
            //    this.AccDay = value;
            //});
            return false;
        }
    };
    JournalLineModel.prototype.OnMouseOver = function () {
        var _this = this;
        this.isMouseIn = true;
        if (this.currencyRate) {
            this.timerToken = setTimeout(function () {
                var item = document.getElementById("tooltip-" + _this.Line);
                if (Tools_1.AppTool.IsNullOrEmpty(item))
                    return;
                var itemRect = item.getBoundingClientRect();
                if (_this.isMouseIn) {
                    document.getElementById("tooltip-body-" + _this.Line).style.position = "fixed";
                    document.getElementById("tooltip-body-" + _this.Line).style.top = (itemRect.top - 35) + 'px';
                    document.getElementById("tooltip-body-" + _this.Line).style.left = (itemRect.left + 60) + 'px';
                    document.getElementById("tooltip-body-" + _this.Line).style.visibility = "visible";
                    _this.timerToken = setTimeout(function () {
                        document.getElementById("tooltip-body-" + _this.Line).style.visibility = "hidden";
                    }, 2500);
                }
            }, 700);
        }
    };
    JournalLineModel.prototype.OnMouseLeave = function () {
        var _this = this;
        this.isMouseIn = false;
        if (this.currencyRate) {
            this.timerToken = setTimeout(function () {
                document.getElementById("tooltip-body-" + _this.Line).style.visibility = "hidden";
            }, 400);
        }
    };
    JournalLineModel.prototype.GetManualyRate = function () {
        if (this.LocalAmount && this.ForeignAmount) {
            var myResult = (this.LocalAmount / this.ForeignAmount).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return myResult;
        }
        else {
            return null;
        }
    };
    Object.defineProperty(JournalLineModel.prototype, "Currency", {
        get: function () { return this.currency; },
        set: function (value) {
            if (this.currency != value) {
                this.currency = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.CurrencyCode = value.Code;
                }
                else {
                    this.CurrencyCode = null;
                }
                this.SplittedCheck();
            }
        },
        enumerable: true,
        configurable: true
    });
    JournalLineModel.prototype.GetGLAccountCurency = function (isCredit, isDebit, currencyId) {
        // credit and debit
        if (isCredit && isDebit) {
            this.GetGLAccountCurency(true, false, currencyId);
            this.GetGLAccountCurency(false, true, currencyId);
            // credit or debit
        }
        else {
            this._GLAccountExtendedListService.GetAccountCurrencies(isCredit ? this.CreditAccountId : this.DebitAccountId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (myResponse.HasError) {
                        console.error(myResponse.ErrorsArray);
                    }
                    else {
                        var currencies = myResponse.Result;
                        if (!Tools_1.AppTool.IsNullOrEmpty(currencies)) {
                            var currency = currencies.find(function (d) { return d.CurrencyId == currencyId; });
                            if (currency != null) {
                                //
                                // Task 41399: Journal Line--> No need for the validation in case the user chose Multi currency GLAccount
                                //
                                ////// Show prompt
                                ////var confirmWindow = new ConfirmWindow();
                                ////confirmWindow.YesButtonText = TextCodeTranslator.Translate("Accounting.General.B.OK");//"Ok";
                                ////confirmWindow.NoButtonText = TextCodeTranslator.Translate("Accounting.General.B.Cancel");//"Cancel";
                                ////confirmWindow.Width = 500;
                                //////confirmWindow.Show("There is a splitted GLAccounts for the chosen multi currency " + (isCredit ? 'Credit' : 'Debit')
                                //////    + " Account, the transactions will be registered in the Splitted By Currency GLAccount ");
                                ////if (isCredit)
                                ////    confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.SplittedAccountMsgCredit"));
                                ////else
                                ////    confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.SplittedAccountMsgDebit"));
                                ////console.log("DetectChanges");
                                //////this.parent.DetectChanges();
                                ////confirmWindow.WindowClosed.subscribe((event: any) => {
                                ////    if (confirmWindow.Yes) {
                                ////        if (isCredit) {
                                ////            this.CreditAccountId = currency.GLAccountId;
                                ////            this.CreditAccountName = currency.GLAccountName;
                                ////        }
                                ////        if (isDebit) {
                                ////            this.DebitAccountId = currency.GLAccountId;
                                ////            this.DebitAccountName = currency.GLAccountName;
                                ////        }
                                ////    } else if (confirmWindow.No) {
                                ////        if (isCredit) this.CreditAccount = null;
                                ////        if (isDebit) this.DebitAccount = null;
                                ////    }
                                ////});
                            }
                            else {
                                return null;
                            }
                        }
                    }
                }
            });
        }
    };
    JournalLineModel.prototype.ShowPrompt = function () {
    };
    JournalLineModel.prototype.SplittedCheck = function () {
        var currency = this.Currency;
        if (!Tools_1.AppTool.IsNullOrEmpty(currency)) {
            var account;
            var checkTwoAccount = false;
            if (this.ActionCode == "1") { // Credit
                if (Tools_1.AppTool.IsNullOrEmpty(this.CreditAccount) || !this.CreditAccount.IsMultiCurrency) {
                    return;
                }
                this.GetGLAccountCurency(true, false, currency.Id);
            }
            else if (this.ActionCode == "2") { // Debit
                if (Tools_1.AppTool.IsNullOrEmpty(this.DebitAccount) || !this.DebitAccount.IsMultiCurrency) {
                    return;
                }
                this.GetGLAccountCurency(false, true, currency.Id);
            }
            else if (this.ActionCode == "3" || this.ActionCode == "4") { // Credit and Debit
                if (Tools_1.AppTool.IsNullOrEmpty(this.CreditAccount) || !this.CreditAccount.IsMultiCurrency) {
                    return;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.DebitAccount) || !this.DebitAccount.IsMultiCurrency) {
                    return;
                }
                this.GetGLAccountCurency(true, true, currency.Id);
            }
        }
        else {
            //console.warn("SplittedCheck: no currency!");
        }
    };
    JournalLineModel.prototype.lastDay = function (year, month) {
        return new Date(year, month + 1, 0).getDate();
    };
    //#region GLAccount HyberLink
    JournalLineModel.prototype.GLAccountHyperlinkClicked = function () {
        this.EditEntity("GLAccount", this.CreditAccountId, null, "GATR");
    };
    JournalLineModel.prototype.DebitAccountHyperlinkClicked = function () {
        this.EditEntity("GLAccount", this.DebitAccountId, null, "GATR");
    };
    JournalLineModel.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
        });
    };
    return JournalLineModel;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=JournalDetailsTabComponent.js.map