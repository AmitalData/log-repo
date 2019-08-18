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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var ReconciliationExtendedPMService_1 = require("../../Services/ExtendedPMs/ReconciliationExtendedPMService");
var AccountingPeriodListService_1 = require("../../Services/StandardLists/AccountingPeriodListService");
var FullAccountingSettingListService_1 = require("../../Services/StandardLists/FullAccountingSettingListService");
var GLAccountPMService_1 = require("../../Services/StandardPMs/GLAccountPMService");
var JournalReconcileComponent = /** @class */ (function (_super) {
    __extends(JournalReconcileComponent, _super);
    function JournalReconcileComponent(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.EntityPM = null;
        _this.ObjectTableName = "Journal";
        _this.DataContext = _this;
        _this.defaultCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        _this.creditTotal = 0;
        _this.debitTotal = 0;
        _this.journalDisabled = false;
        _this.forceFocus = false;
        _this.AccountingPeriods = [];
        _this._AccountingPeriodListService = new AccountingPeriodListService_1.AccountingPeriodListService();
        _this._ReconciliationExtendedPMService = new ReconciliationExtendedPMService_1.ReconciliationExtendedPMService();
        _this.fullAccountingSettingListService = new FullAccountingSettingListService_1.FullAccountingSettingListService();
        _this.gLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.txt_Reference = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Reference");
        _this.txt_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("JournalLine.F.LocalAmount");
        //#region Properties
        _this.reference1 = "";
        _this.reference2 = "";
        _this.reference3 = "";
        _this.notes = "";
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.fullAccountingSettingListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var list;
                    list = res;
                    _this.fullAccountingSettingList = list[0];
                    if (_this.fullAccountingSettingList && _this.fullAccountingSettingList.DefaultDifferencesGLAccountId) {
                        _this.GLAccountId = _this.fullAccountingSettingList.DefaultDifferencesGLAccountId;
                        _this.gLAccountPMService.get(_this.glAcccountId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var res = myResponse.Result;
                                _this.GLAccount = res;
                            }
                        });
                    }
                }
            }
        });
        _this.GLAccountsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.GLAccountsFilterItems.addAdditionalFilter("AccountTypeCode", "5,4", null, null, "Exclude", false, false, false, "string", false, true);
        _this.UIProperties.SetRequired("AccountingDate", _this.ObjectTableName, true);
        if (_this.AccountingDate == null)
            _this.AccountingDate = new Date();
        return _this;
        //this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
    }
    Object.defineProperty(JournalReconcileComponent.prototype, "GLAccountId", {
        get: function () { return this.glAcccountId; },
        set: function (value) {
            if (this.glAcccountId != value) {
                this.glAcccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalReconcileComponent.prototype, "GLAccount", {
        get: function () { return this.glAccount; },
        set: function (value) {
            if (this.glAccount != value) {
                this.glAccount = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.glAccount)) {
                this.UIProperties.SetRequired("GLAccount", this.ObjectTableName, false);
                this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("GLAccount", this.ObjectTableName, true);
                this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    JournalReconcileComponent.prototype.ngOnInit = function () {
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
        this.GetAccountingPeriods();
        //set focus on accounting date
        var t = setTimeout(function () { _this.forceFocus = true; }, 1);
    };
    Object.defineProperty(JournalReconcileComponent.prototype, "Reference1", {
        get: function () { return this.reference1; },
        set: function (value) {
            if (this.reference1 != value) {
                this.reference1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalReconcileComponent.prototype, "Reference2", {
        get: function () { return this.reference2; },
        set: function (value) {
            if (this.reference2 != value) {
                this.reference2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalReconcileComponent.prototype, "Reference3", {
        get: function () { return this.reference3; },
        set: function (value) {
            if (this.reference3 != value) {
                this.reference3 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalReconcileComponent.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (value) {
            if (this.notes != value) {
                this.notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalReconcileComponent.prototype, "AccountingDate", {
        get: function () { return this._AccountingDate; },
        set: function (value) {
            if (this._AccountingDate != value) {
                if (value != null) {
                    // Get Accounting Period by year
                    var accountingPeriod = this.AccountingPeriods.find(function (d) { return d.Year == value.getFullYear(); });
                    if (accountingPeriod) {
                        var month = value.getMonth() + 1;
                        this.ValidationErrorsList = []; // empty errors list
                        // Valid Month => (ClosedMonth < month <= OpenMonth)
                        if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)
                            this.ValidationErrorsList = []; // empty errors list
                        }
                        else { // invalid (closed month)
                            // push the error to errors list
                            this.ValidationErrorsList.push("Closed Month!");
                            this._AccountingDate = value;
                            return;
                        }
                    }
                }
                this._AccountingDate = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(this._AccountingDate)) {
                    this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, false);
                }
                else {
                    //this.AccountingDate = null;
                    this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JournalReconcileComponent.prototype, "AccountingPeriod", {
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
    //#endregion
    JournalReconcileComponent.prototype.DetectChanges = function () {
        this.CD.detectChanges();
    };
    JournalReconcileComponent.prototype.GetAccountingPeriods = function () {
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
    JournalReconcileComponent.prototype.lastDay = function (year, month) {
        return new Date(year, month + 1, 0).getDate();
    };
    JournalReconcileComponent.prototype.SetWindowArgs = function (winArgs) {
        this._SelectedLines = winArgs.SelectedLines;
        this._GLAccountPMId = winArgs.GLAccountPMId;
    };
    JournalReconcileComponent.prototype.FillErrors = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.glAccount)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("GLAccount is Required");
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.AccountingDate)) {
            this.ValidationErrorsList.push("Accounting Date is Required");
        }
        else {
            this.ValidationErrorsList = [];
        }
    };
    JournalReconcileComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        var myReconciliationLines = [];
        for (var i = 0; i < this._SelectedLines.Length; i++) {
            var selectedTransaction = this._SelectedLines.Collection[i];
            var newLine = {};
            newLine.ChangeSetOp = "1";
            newLine.ReconciliationId = "new";
            newLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
            ;
            newLine.Line = i;
            newLine.CurrencyId = selectedTransaction.OpenAmountCurrencyId;
            newLine.TransactionId = selectedTransaction.Id;
            newLine.ReconciliationAmount = selectedTransaction.AmountToReconcile;
            newLine.IsPartial = selectedTransaction.IsPartial;
            //newLine.GroupNumber = selectedTransaction.GroupHash;
            myReconciliationLines.push(newLine);
        }
        //var AdjustAccountId: string = "1-19";
        this._ReconciliationExtendedPMService.CreateJournalReconcile(myReconciliationLines, this._GLAccountPMId, this.GLAccount.Id, this.AccountingDate.toUTCString(), this.reference1, this.reference2, this.reference3, this.Notes)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            if (res.HasError) {
                _this.ValidationErrorsList = res.ErrorsArray;
            }
            else {
                var journalPM;
                journalPM = res.Result;
                console.log(journalPM);
                _this._NewJournalPM = journalPM;
            }
        });
    };
    JournalReconcileComponent.prototype.OpenJournal = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this._NewJournalPM.Id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this._NewJournalPM.Id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    JournalReconcileComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    JournalReconcileComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './JournalReconcileComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], JournalReconcileComponent);
    return JournalReconcileComponent;
}(BaseComponent_1.BaseComponent));
exports.JournalReconcileComponent = JournalReconcileComponent;
//# sourceMappingURL=JournalReconcileComponent.js.map