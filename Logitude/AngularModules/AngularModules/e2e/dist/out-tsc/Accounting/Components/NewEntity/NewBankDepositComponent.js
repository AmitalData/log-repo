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
var BankDepositPM_1 = require("../../EntityPMs/BankDepositPM");
var Tools_1 = require("../../../Infrastructure/Tools");
var CashBookPMService_1 = require("../../Services/StandardPMs/CashBookPMService");
var AccountingPeriodExtendedListService_1 = require("../../Services/ExtendedLists/AccountingPeriodExtendedListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var RatesTableExtendedListService_1 = require("../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService");
var NewBankDepositComponent = /** @class */ (function (_super) {
    __extends(NewBankDepositComponent, _super);
    function NewBankDepositComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "BankDeposit";
        _this.ValidationErrorsList = [];
        _this.CashBookTotal = 0;
        _this.cashBookPMService = new CashBookPMService_1.CashBookPMService();
        _this.myAccountingPeriodListService = new AccountingPeriodExtendedListService_1.AccountingPeriodExtendedListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ratesTableExtendedListService = new RatesTableExtendedListService_1.RatesTableExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._entityResourceService.getEntityResourceByTableName("AccountingPeriod").subscribe(function (res) { });
        _this.EntityPM = new BankDepositPM_1.BankDepositPM();
        _this.EntityPM.AccountingDate = new Date();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.DepositDate = new Date();
        _this.EntityPM.DepositNumber = 0;
        _this.GetClosedMonth();
        return _this;
    }
    NewBankDepositComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(args.CashBookId)) {
                this.CashBookId = args.CashBookId;
                this.UIProperties.SetEnabled("CashBookId", this.ObjectTableName, false); // lock field
            }
        }
    };
    NewBankDepositComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(NewBankDepositComponent.prototype, "AccountingDate", {
        //#region Properties
        get: function () { return this.EntityPM.AccountingDate; },
        set: function (value) {
            if (this.EntityPM.AccountingDate != value) {
                this.EntityPM.AccountingDate = value;
                this.EntityPM.DepositDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankDepositComponent.prototype, "CashBookId", {
        get: function () { return this.EntityPM.CashBookId; },
        set: function (value) {
            if (this.EntityPM.CashBookId != value) {
                this.EntityPM.CashBookId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankDepositComponent.prototype, "DepositBankAccountId", {
        get: function () { return this.EntityPM.DepositBankAccountId; },
        set: function (value) {
            if (this.EntityPM.DepositBankAccountId != value) {
                this.EntityPM.DepositBankAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankDepositComponent.prototype, "CashBook", {
        get: function () { return this.cashbook; },
        set: function (value) {
            if (this.cashbook != value) {
                this.cashbook = value;
                if (this.cashbook) {
                    this.EntityPM.DepositCurrencyId = this.cashbook.CurrencyId;
                    this.CashBookTotal = this.cashbook.TotalAmount == null ? 0 : this.cashbook.TotalAmount;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBankDepositComponent.prototype, "DepositBankAccount", {
        get: function () { return this.bankAccount; },
        set: function (value) {
            if (this.bankAccount != value) {
                this.bankAccount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    NewBankDepositComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        // Required check
        if (Tools_1.AppTool.IsNullOrEmpty(this.DepositBankAccountId) || Tools_1.AppTool.IsNullOrEmpty(this.CashBookId)) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.AllFieldsRequired"));
        }
        if (this.EntityPM.DepositCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
            //check rate
            this.CurrentSession.CurrentWindow.StartBusyIndicator("...");
            this.ratesTableExtendedListService.getClosestRate(SessionLocator_1.SessionLocator.TenantPM.CurrencyId, this.EntityPM.DepositCurrencyId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        if (myResponse.Result != undefined && myResponse.Result != null) {
                            //continu saving
                            _this.CheckIfThereIsCheques(_this.CashBookId);
                        }
                        else {
                            _this.ValidationErrorsList = [];
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NoExchangeRateForLocalCurrency"));
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    }
                }
            });
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        }
        else {
            //continu saving
            this.CheckIfThereIsCheques(this.CashBookId);
        }
    };
    NewBankDepositComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBankDepositComponent.prototype.AccountingDateLostFocus = function () {
        this.validateAccountingPeriod();
    };
    NewBankDepositComponent.prototype.CheckIfThereIsCheques = function (id) {
        var _this = this;
        // 1-get cashbook pm
        this.cashBookPMService.get(id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var entityPm;
                entityPm = myResponse.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(entityPm)) {
                    //2-check
                    if (entityPm.CashBookTypeCode == "2") { // 2-cheques
                        var exist = entityPm.CashBookLines.find(function (d) { return d.IsDeposited == false; });
                        if (exist) {
                            _this.SubmitChanges();
                        }
                        else {
                            _this.ValidationErrorsList = [];
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.noChequesinCashbook"));
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    }
                    else if (entityPm.CashBookTypeCode == "1") { // 1-cash
                        if (Tools_1.AppTool.IsNullOrZero(entityPm.TotalAmount)) {
                            _this.ValidationErrorsList = [];
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.noCashICashbook"));
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                        else {
                            _this.SubmitChanges();
                        }
                    }
                }
            }
            else {
                console.log("error");
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        });
    };
    NewBankDepositComponent.prototype.SubmitChanges = function () {
        var _this = this;
        var errors = [];
        //#region currency check
        if (this.bankAccount.GLAccountCurrencyId != null && this.bankAccount.GLAccountCurrencyId != "multi") {
            if (this.bankAccount.GLAccountCurrencyId != this.cashbook.CurrencyId) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Currencydifferent"));
            }
        }
        else {
            console.warn("maybe the glaccount of bank account is multi or null!");
        }
        this.EntityPM.IsCashDeposit = this.CashBook.CashBookTypeCode == "1";
        //#endregion
        //closed month check
        var isValid = this.IsMonthOpenForAccountingDate();
        if (!isValid)
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth")); // closed month
        if (errors.length == 0) {
            this.EntityPM.CashBookName = this.CashBook.LocalName;
            this.EntityPM.BankAccountNumber = this.DepositBankAccount.AccountNumber;
            if (this.EntityPM != null) {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityPM: _this.EntityPM, ObjectTableName: 'BankDeposit' });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.CancelButtonClicked();
                    });
                });
            }
        }
        else {
            this.ValidationErrorsList = errors;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    //#region closed month
    NewBankDepositComponent.prototype.validateAccountingPeriod = function () {
        var isValid = this.IsMonthOpenForAccountingDate();
        if (!isValid) {
            //this.ValidationErrorsList = [];
            //this.ValidationErrorsList.push("חודש סגור!"); // closed month
            this.UIProperties.SetValidity("AccountingDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth"));
        }
        else {
            this.UIProperties.SetValidity("AccountingDate", this.ObjectTableName, true, "");
        }
        return isValid;
    };
    NewBankDepositComponent.prototype.IsMonthOpenForAccountingDate = function () {
        var valid = true;
        if (this.accountingPeriod == null) {
            valid = false;
            //errorsList.Add(transText);
        }
        else {
            var accountingDateMonth = this.EntityPM.AccountingDate.getMonth() + 1;
            if (accountingDateMonth > this.accountingPeriod.ClosedMonth) {
                //Valid ... AccountingDateMonth must be greater than close Mounth
            }
            else {
                //Not Valid ... AccountingDateMonth must be greater than close Mounth
                //not valid  8>=8
                //not valid  0>=1 - Must Open mounth before work on year !!
                valid = false;
                //errorsList.Add(transText); //ClosedMonth Must B
            }
            if (accountingDateMonth == this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  equal to OpenMonth
            }
            else if (accountingDateMonth < this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  less than OpenMonth
            }
            else {
                valid = false;
                //errorsList.Add(transText);
            }
        }
        return valid;
    };
    NewBankDepositComponent.prototype.GetClosedMonth = function () {
        var _this = this;
        var periodTypeCode = "1"; // 1-Regular
        this.myAccountingPeriodListService.getByYear(new Date().getFullYear(), periodTypeCode).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.accountingPeriod = myResponse.Result;
                }
            }
        });
    };
    NewBankDepositComponent = __decorate([
        core_1.Component({
            selector: 'NewBankDepositComponent',
            moduleId: module.id,
            templateUrl: './NewBankDepositComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewBankDepositComponent);
    return NewBankDepositComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBankDepositComponent = NewBankDepositComponent;
//# sourceMappingURL=NewBankDepositComponent.js.map