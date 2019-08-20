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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ARInvoicePaymentPM_1 = require("../../../../Invoice/EntityPMs/ARInvoicePaymentPM");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var EditMultiCurrency = /** @class */ (function (_super) {
    __extends(EditMultiCurrency, _super);
    function EditMultiCurrency(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "ARInvoicePayment";
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.PaymentCurrencyId = null;
        _this.PaymentCurrencyCode = null;
        _this.PaymentExchangeRate = null;
        _this.InvoiceCurrencyId = null;
        _this.InvoiceCurrencyCode = null;
        _this.InvoiceExchangeRate = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ForeignAmountIsValid = true;
        _this.ForeignAmountValidation = null;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        return _this;
    }
    EditMultiCurrency.prototype.SetWindowArgs = function (myParam) {
        var _this = this;
        this.lineComponent = myParam;
        this.PaymentCurrencyId = this.lineComponent.PaymentPM.PaymentCurrencyId;
        this.PaymentCurrencyCode = this.lineComponent.PaymentPM.PaymentCurrencyCode;
        this.PaymentExchangeRate = this.lineComponent.PaymentPM.PaymentCurrencyExchangeRate;
        this.InvoiceCurrencyId = this.lineComponent.Invoice.InvoiceCurrencyId;
        this.InvoiceCurrencyCode = this.lineComponent.Invoice.InvoiceCurrencyCode;
        this.InvoiceExchangeRate = this.lineComponent.Invoice.InvoiceCurrencyExchangeRate;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            var myForeignAmount = null;
            var myPaymentAmount = null;
            var myExchangeRate = null;
            if (_this.lineComponent.EntityPM) {
                myForeignAmount = _this.lineComponent.EntityPM.ForeignAmount;
                myPaymentAmount = _this.lineComponent.EntityPM.PaymentAmount;
                myExchangeRate = _this.lineComponent.EntityPM.ExchangeRate;
            }
            else {
                myForeignAmount = _this.lineComponent.AmountDue;
                myPaymentAmount = _this.lineComponent.GetConnectedAmount_PAY(myForeignAmount);
                myExchangeRate = _this.lineComponent.ExchangeRate;
            }
            if (!Tools_1.AppTool.IsNullOrZero(_this.lineComponent.AmountPaid)) {
                if (myForeignAmount != _this.lineComponent.AmountPaid) {
                    myForeignAmount = _this.lineComponent.AmountPaid;
                    myPaymentAmount = _this.lineComponent.GetConnectedAmount_PAY(myForeignAmount);
                    if (_this.lineComponent.IsConnected) {
                        myPaymentAmount = _this.lineComponent.ConnectedAmount_PAY;
                    }
                }
            }
            _this.EntityPM = new ARInvoicePaymentPM_1.ARInvoicePaymentPM(null);
            _this.EntityPM.ForeignAmount = myForeignAmount;
            _this.EntityPM.PaymentAmount = myPaymentAmount;
            _this.EntityPM.ExchangeRate = myExchangeRate;
            _this.IsResourcesReady = true;
        });
    };
    Object.defineProperty(EditMultiCurrency.prototype, "PaymentAmount", {
        get: function () { return this.EntityPM.PaymentAmount; },
        set: function (value) {
            if (this.EntityPM.PaymentAmount != value) {
                this.EntityPM.PaymentAmount = Tools_1.AppTool.Round(value, 2);
                this.CalculateForeignAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultiCurrency.prototype, "ForeignAmount", {
        get: function () { return this.EntityPM.ForeignAmount; },
        set: function (value) {
            if (this.EntityPM.ForeignAmount != value) {
                this.EntityPM.ForeignAmount = Tools_1.AppTool.Round(value, 2);
                if (Tools_1.AppTool.IsNullOrZero(value)) {
                    this.EntityPM.PaymentAmount = value;
                }
                else {
                    this.EntityPM.PaymentAmount = this.GetConnectedAmount_PAY(value);
                }
                this.ValidateForeignAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditMultiCurrency.prototype, "ExchangeRate", {
        get: function () { return this.EntityPM.ExchangeRate; },
        set: function (value) {
            if (this.EntityPM.ExchangeRate != value) {
                this.EntityPM.ExchangeRate = Tools_1.AppTool.Round(value, 5);
                this.CalculateForeignAmount();
                this.UIProperties.SetRequired("ExchangeRate", this.ObjectTableName, Tools_1.AppTool.IsNullOrZero(value) ? true : false);
            }
        },
        enumerable: true,
        configurable: true
    });
    EditMultiCurrency.prototype.CalculateForeignAmount = function () {
        var myResult = null;
        if (this.PaymentAmount == 0) {
            myResult = 0;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.PaymentAmount) || Tools_1.AppTool.IsNullOrEmpty(this.ExchangeRate)) {
            myResult = null;
        }
        else if (this.InvoiceCurrencyId == this.LocalCurrencyId) {
            myResult = this.PaymentAmount * this.ExchangeRate;
        }
        else {
            myResult = this.PaymentAmount / this.ExchangeRate;
        }
        this.EntityPM.ForeignAmount = Tools_1.AppTool.Round(myResult, 2);
        this.ValidateForeignAmount();
    };
    EditMultiCurrency.prototype.GetConnectedAmount_PAY = function (invoiceAmount) {
        var myResult = 0;
        if (!Tools_1.AppTool.IsNullOrZero(invoiceAmount)) {
            if (this.InvoiceCurrencyId != this.PaymentCurrencyId) {
                if (this.InvoiceCurrencyId == this.LocalCurrencyId) {
                    myResult = invoiceAmount / this.ExchangeRate;
                }
                else {
                    myResult = invoiceAmount * this.ExchangeRate;
                }
            }
            else {
                myResult = invoiceAmount;
            }
        }
        return myResult;
    };
    EditMultiCurrency.prototype.ValidateForeignAmount = function () {
        var isValid = true;
        var isValidMessage = null;
        var inputEntry = this.ForeignAmount == null ? 0 : Tools_1.AppTool.Round(this.ForeignAmount, 2);
        var allowedAmount = Tools_1.AppTool.Round(this.lineComponent.InvoiceAmount - this.lineComponent.OtherPaymentsAmount, 2);
        if (this.lineComponent.Invoice.ARInvoiceTypeCode == "CD" || this.lineComponent.Invoice.ARInvoiceTypeCode == "CC") {
            if (inputEntry > 0) {
                isValid = false;
                isValidMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.OnlyMinusValue");
            }
            else if (inputEntry < allowedAmount) {
                isValid = false;
                isValidMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.AmountPaidNotLess") + " " + allowedAmount + " " + this.InvoiceCurrencyCode;
            }
        }
        else {
            if (inputEntry < 0) {
                isValid = false;
                isValidMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantPayMinusValue");
            }
            else if (inputEntry > allowedAmount) {
                isValid = false;
                isValidMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.AmountPaidLessOrEqual") + " " + allowedAmount + " " + this.InvoiceCurrencyCode;
            }
        }
        this.ForeignAmountIsValid = isValid;
        this.ForeignAmountValidation = isValidMessage;
        this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, isValid, isValidMessage);
    };
    EditMultiCurrency.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditMultiCurrency.prototype.OkButtonClicked = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.ValidateForeignAmount();
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrZero(this.ExchangeRate)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.ExchangeRate");
            errors.push(msg.replace("%FieldName", field));
        }
        if (!this.ForeignAmountIsValid) {
            errors.push(this.ForeignAmountValidation);
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    };
    EditMultiCurrency = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditMultiCurrency.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], EditMultiCurrency);
    return EditMultiCurrency;
}(BaseComponent_1.BaseComponent));
exports.EditMultiCurrency = EditMultiCurrency;
//# sourceMappingURL=EditMultiCurrency.js.map