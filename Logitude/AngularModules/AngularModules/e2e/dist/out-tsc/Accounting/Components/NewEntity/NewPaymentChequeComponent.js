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
var PaymentChequePM_1 = require("../../EntityPMs/PaymentChequePM");
var PaymentChequePMService_1 = require("../../Services/StandardPMs/PaymentChequePMService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var PaymentChequeValidator_1 = require("../../Validators/PaymentChequeValidator");
var CurrencyRatesService_1 = require("../../../Common/Services/CurrencyRatesService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var NewPaymentChequeComponent = /** @class */ (function (_super) {
    __extends(NewPaymentChequeComponent, _super);
    function NewPaymentChequeComponent(CD, entityListService) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.entityListService = entityListService;
        _this.DataContext = _this;
        _this.ObjectTableName = "PaymentCheque";
        _this.entityPM = new PaymentChequePM_1.PaymentChequePM();
        _this.ValidationErrorsList = [];
        _this.PaymentChequePMService = new PaymentChequePMService_1.PaymentChequePMService();
        _this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.visible = false;
        _this.Height = 50;
        _this.IsForignAmountVisibile = true;
        _this.paymentChequeValidator = new PaymentChequeValidator_1.PaymentChequeValidator();
        _this.CurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        _this.LastRatesList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.bankName = "LocalName";
        _this.isAccountValid = true;
        _this.FIELD_IS_REQUIERD = null;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityResourceService.getEntityResourceByTableName("PaymentCheque").subscribe(function (response) {
            _this.visible = true;
            _this.LocalAmountFieldLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentCheque.F.LocalAmount") + " " + "(" + SessionLocator_1.SessionLocator.LocalCurrencyCode + ")";
        });
        _this.SetFilters();
        _this.entityPM.Tenant = _this.TenantPM.Id;
        _this.CurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.ValueDate = new Date();
        _this.CurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.LocalCurrencyId, _this.ValueDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
        return _this;
    }
    NewPaymentChequeComponent.prototype.SetFilters = function () {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string");
    };
    Object.defineProperty(NewPaymentChequeComponent.prototype, "PayToGLAccountId", {
        get: function () { return this.entityPM.PayToGLAccountId; },
        set: function (value) {
            if (this.entityPM.PayToGLAccountId != value) {
                this.entityPM.PayToGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "PayToName", {
        get: function () { return this.entityPM.PayToName; },
        set: function (value) {
            if (this.entityPM.PayToName != value) {
                this.entityPM.PayToName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "CurrencyId", {
        get: function () { return this.entityPM.CurrencyId; },
        set: function (value) {
            if (this.entityPM.CurrencyId != value) {
                this.entityPM.CurrencyId = value;
                //if (!AppTool.IsNullOrEmpty(value)) {
                //    this.CurrencyRatesService.GetCurrenciesExchangeRateByValueDate(this.CurrencyId, new Date()).subscribe((myResponse: ServiceResponse) => {
                //        if (!myResponse.HasError) {
                //            this.LastRatesList = myResponse.Result;
                //            var rate: number = null;
                //            var rateDate: Date = null;
                //            var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.bankAccount.GLAccountCurrencyId)[0];
                //            if (lastRate != null) {
                //                rate = lastRate.Rate;
                //                this.entityPM.ExchangeRate = rate;
                //                this.ForeignAmount = this.LocalAmount / rate;
                //            }
                //        }
                //        //else {
                //        //    this.CurrentSession.StopBusyIndicator();
                //        //}
                //    });
                // }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "LocalAmount", {
        get: function () { return this.entityPM.LocalAmount; },
        set: function (value) {
            var _this = this;
            if (this.entityPM.LocalAmount != value) {
                this.entityPM.LocalAmount = value;
                if (value && this.BankAccount) {
                    var rate = null;
                    var rateDate = null;
                    var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == _this.bankAccount.GLAccountCurrencyId; })[0];
                    if (lastRate != null) {
                        rate = lastRate.Rate;
                        this.entityPM.ExchangeRate = rate;
                        this.ForeignAmount = this.LocalAmount / rate;
                    }
                }
                else {
                    this.ForeignAmount = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "ForeignAmount", {
        get: function () { return this.entityPM.ForeignAmount; },
        set: function (value) {
            if (this.entityPM.ForeignAmount != value) {
                this.entityPM.ForeignAmount = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "Notes", {
        get: function () { return this.entityPM.Notes; },
        set: function (value) {
            if (this.entityPM.Notes != value) {
                this.entityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "BankName", {
        get: function () { return this.bankName; },
        set: function (value) {
            if (this.bankName != value) {
                this.bankName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "ValueDate", {
        get: function () { return this.entityPM.ValueDate; },
        set: function (value) {
            if (this.entityPM.ValueDate != value) {
                this.entityPM.ValueDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "BankAccountId", {
        get: function () { return this.entityPM.BankAccountId; },
        set: function (value) {
            if (this.entityPM.BankAccountId != value) {
                this.entityPM.BankAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "Account", {
        get: function () { return this.account; },
        set: function (value) {
            if (this.account != value) {
                this.account = value;
                if (value != null) {
                    if (value.AccountTypeCode == "3") {
                        this.isAccountValid = false;
                        this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.VendorsGLAccount"));
                    }
                    else {
                        this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, true, null);
                        this.isAccountValid = true;
                        this.PayToName = value.LocalName;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentChequeComponent.prototype, "BankAccount", {
        get: function () { return this.bankAccount; },
        set: function (value) {
            var _this = this;
            if (this.bankAccount != value) {
                this.bankAccount = value;
                if (this.bankAccount) {
                    if (this.bankAccount.LocalName) {
                        this.BankName = "LocalName";
                    }
                    else {
                        this.BankName = "EnglishName";
                    }
                    this.CurrencyId = this.bankAccount.GLAccountCurrencyId;
                    if (this.bankAccount.GLAccountCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                        this.UIProperties.SetVisibility("ForeignAmount", this.ObjectTableName, false);
                        this.UIProperties.SetVisibility("CurrencyId", this.ObjectTableName, false);
                        // this.Height = 0;
                        this.IsForignAmountVisibile = false;
                    }
                    else {
                        this.UIProperties.SetVisibility("ForeignAmount", this.ObjectTableName, true);
                        this.UIProperties.SetVisibility("CurrencyId", this.ObjectTableName, true);
                        //this.Height = 0;
                        this.IsForignAmountVisibile = true;
                        this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, true);
                        if (this.LocalAmount) {
                            var rate = null;
                            var rateDate = null;
                            var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == _this.bankAccount.GLAccountCurrencyId; })[0];
                            if (lastRate != null) {
                                rate = lastRate.Rate;
                                this.entityPM.ExchangeRate = rate;
                                this.ForeignAmount = this.LocalAmount / rate;
                            }
                        }
                    }
                }
                else {
                    this.UIProperties.SetVisibility("ForeignAmount", this.ObjectTableName, true);
                    this.UIProperties.SetVisibility("CurrencyId", this.ObjectTableName, true);
                    //this.Height = 0;
                    this.IsForignAmountVisibile = true;
                    this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, true);
                    this.ForeignAmount = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewPaymentChequeComponent.prototype.OkButtonClicked = function () {
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.ValidationErrorsList = [];
        this.CheckCurrency();
        if (!this.isAccountValid) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.VendorsGLAccount"));
        }
        if (this.ValidationErrorsList.length == 0) {
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
            if (errors.length == 0) {
                this.SubmitChanges();
            }
            else {
                this.ValidationErrorsList = errors;
            }
        }
    };
    NewPaymentChequeComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.entityPM.BankAccountGLAccountId = this.BankAccount.DeferredGLAccountId;
        this.entityPM.PaymentChequeStatusCode = "1";
        this.PaymentChequePMService.insert(this.entityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                _this.CurrentSession.CloseCurrentWindowEmit("ok"); // this.CurrentSession.CloseCurrentWindowEmit("ok");
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: _this.ObjectTableName });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.CancelButtonClicked();
                    });
                });
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewPaymentChequeComponent.prototype.CheckCurrency = function () {
        if (this.BankAccount) {
            if (SessionLocator_1.SessionLocator.LocalCurrencyId != this.BankAccount.GLAccountCurrencyId) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.DifferentBankCurrency"));
            }
        }
        if (this.Account) {
            if (!this.Account.IsMultiCurrency) {
                if (this.Account.CurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                    //  this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.DifferentCurrencies"));
                }
            }
        }
    };
    NewPaymentChequeComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewPaymentChequeComponent = __decorate([
        core_1.Component({
            selector: 'NewPaymentChequeComponent',
            moduleId: module.id,
            templateUrl: './NewPaymentChequeComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], NewPaymentChequeComponent);
    return NewPaymentChequeComponent;
}(BaseComponent_1.BaseComponent));
exports.NewPaymentChequeComponent = NewPaymentChequeComponent;
//# sourceMappingURL=NewPaymentChequeComponent.js.map