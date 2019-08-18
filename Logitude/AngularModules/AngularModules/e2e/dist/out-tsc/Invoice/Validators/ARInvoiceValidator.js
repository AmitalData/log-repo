"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var VatTypesValidator_1 = require("../../Infrastructure/Validators/VatTypesValidator");
var ARInvoiceValidator = /** @class */ (function () {
    function ARInvoiceValidator() {
        this.Errors = [];
        this.Errors = [];
        this.message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    ARInvoiceValidator.prototype.Validate = function (entity) {
        this.Errors = [];
        this.EntityPM = entity;
        Validator_1.Validator.TryValidateObject(this.EntityPM, "ARInvoice", this.Errors);
        if (this.EntityPM.IsInvoiceNumberManuallySet && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber)) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.YouShouldSetInvoiceNumber"));
        }
        if (this.EntityPM.IsInvoiceNumberFromStock && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceStockId)) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.YouShouldSetInvoiceNumber"));
        }
        if (this.EntityPM.IsInvoiceNumberFromStock && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) && this.EntityPM.IsAutoCredit) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.YouShouldSetInvoiceNumber"));
        }
        if (Tools_1.DateTool.GetDateParts(this.EntityPM.InvoiceDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatNumber)) {
                    this.Errors.push(this.message.replace("%FieldName", "Vat Number"));
                }
            }
        }
        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowManualInvoiceNumber) {
            if (this.EntityPM.IsInvoiceNumberManuallySet) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                    this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.ManualInvoiceNumberNotAllowed"));
                }
            }
        }
        if (this.EntityPM.IsConsolidationInvoice) {
            this.ValidateConsolidationInvoice();
        }
        else {
            this.ValidateNormalInvoice();
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SATPaymentMethodCode)) {
                this.Errors.push("Forma Pago Field is Required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MetodoPagoCode)) {
                this.Errors.push("Metodo Pago Field is Required");
            }
            if (this.EntityPM.MetodoPagoCode == "PUE" && this.EntityPM.SATPaymentMethodCode == "99") {
                this.Errors.push("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
            }
        }
        return this.Errors;
    };
    ARInvoiceValidator.prototype.ValidateNormalInvoice = function () {
        var _this = this;
        if (this.EntityPM.InvoiceLines.length == 0) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast"));
        }
        else {
            var allVatTypes = VatTypesValidator_1.VatTypesValidator.GetAllVatTypes();
            this.EntityPM.InvoiceLines.forEach(function (item) {
                Validator_1.Validator.TryValidateObject(item, "ARInvoiceLine", _this.Errors);
                if (item.VatTypeId == null) {
                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.VatTypeId");
                    _this.Errors.push(_this.message.replace("%FieldName", field));
                }
                else {
                    if (item.VatPercentage == null) {
                        var vattType = allVatTypes.filter(function (d) { return d.Id == item.VatTypeId; })[0];
                        if (vattType != null) {
                            if (!vattType.IsMultiPercentage) {
                                var field = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.VatPercentage");
                                _this.Errors.push(_this.message.replace("%FieldName", field));
                            }
                        }
                    }
                }
            });
            var lineGrouped = [];
            this.EntityPM.InvoiceLines.forEach(function (item) {
                var lineItem = lineGrouped.filter(function (f) { return f.Id == item.ForiegnCurrencyId; })[0];
                if (lineItem == null) {
                    lineGrouped.push(new LineCurrency(item.ForiegnCurrencyId, item.ForiegnCurrencyCode, item.ForiegnExchangeRate));
                }
                else if (lineItem.Rate != item.ForiegnExchangeRate) {
                    if (!lineItem.Validated) {
                        lineItem.Validated = true;
                        _this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.InvoiceLinesHaveDifferentExchangeRates").replace("%CurrencyCode", lineItem.Code));
                    }
                }
            });
            //var listGrouped: InvoiceTotalsClass[] = [];
            //this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null).forEach(item => {
            //    var localAmount = item.VatPercentage * item.LocalCurrencyAmount / 100;
            //    var invoiceAmount = item.VatPercentage * item.InvoiceCurrencyAmount / 100;
            //    if (AppTool.IsNullOrEmpty(localAmount)) {
            //        localAmount = 0;
            //    }
            //    if (AppTool.IsNullOrEmpty(invoiceAmount)) {
            //        invoiceAmount = 0;
            //    }
            //    var itemGrouped: InvoiceTotalsClass = listGrouped.filter(f => f.VatTypeId == item.VatTypeId)[0];
            //    if (itemGrouped == null) {
            //        itemGrouped = new InvoiceTotalsClass();
            //        itemGrouped.VatTypeId = item.VatTypeId;
            //        itemGrouped.RowLabel = item.VatTypeName + " (" + item.VatPercentage + "%)";
            //        itemGrouped.LocalCurrencyAmount = localAmount;
            //        itemGrouped.InvoiceCurrencyAmount = invoiceAmount;
            //        listGrouped.push(itemGrouped);
            //    }
            //    else {
            //        itemGrouped.LocalCurrencyAmount += localAmount;
            //        itemGrouped.InvoiceCurrencyAmount += invoiceAmount;
            //    }
            //});
            if (this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                if (!this.EntityPM.IsAutoCredit) {
                    if (this.EntityPM.SubTotalInInvoiceCurrency > 0) {
                        this.Errors.push("Subtotal amount can't be positive");
                    }
                    if (this.EntityPM.AmountInInvoiceCurrency > 0) {
                        this.Errors.push("Invoice amount can't be positive");
                    }
                    //this.EntityPM.TotalVATs.filter(f => f.InvoiceCurrencyVATAmount > 0).forEach(item => {
                    //    this.Errors.push(item.VatTypeCell + " amount can't be positive");
                    //});
                    //listGrouped.filter(f => f.InvoiceCurrencyAmount > 0).forEach(item => {
                    //    this.Errors.push(item.RowLabel + " amount can't be positive");
                    //});
                }
            }
            else {
                if (this.EntityPM.SubTotalInInvoiceCurrency < 0) {
                    this.Errors.push("Subtotal amount can't be minus");
                }
                if (this.EntityPM.AmountInInvoiceCurrency < 0) {
                    this.Errors.push("Invoice amount can't be minus");
                }
            }
            this.ValidateSingleTaxPerInvoice();
        }
    };
    ARInvoiceValidator.prototype.ValidateConsolidationInvoice = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
            //if (!this.EntityPM.IsBillToAllowConsolidation) {
            //    this.Errors.push(InvoiceTool.GetBillToNotAllowConsolidation());
            //}
        }
        if (this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR") {
            if (this.EntityPM.InvoiceLines.length == 0) {
                this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast"));
            }
        }
        else if (this.EntityPM.ConstituentInvoices.length == 0) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast"));
        }
        else {
            if (this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                if (this.EntityPM.SubTotalInInvoiceCurrency > 0) {
                    this.Errors.push("Subtotal amount can't be positive");
                }
                if (this.EntityPM.AmountInInvoiceCurrency > 0) {
                    this.Errors.push("Invoice amount can't be positive");
                }
            }
            else {
                if (this.EntityPM.SubTotalInInvoiceCurrency < 0) {
                    this.Errors.push("Subtotal amount can't be minus");
                }
                if (this.EntityPM.AmountInInvoiceCurrency < 0) {
                    this.Errors.push("Invoice amount can't be minus");
                }
            }
        }
    };
    ARInvoiceValidator.prototype.ValidateSingleTaxPerInvoice = function () {
        var _this = this;
        var isValidating = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsSingleTaxPerInvoice) {
            isValidating = true;
        }
        else if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            if (SessionLocator_1.SessionLocator.AccountingSystemPM.IsSingleTaxPerInvoice) {
                isValidating = true;
            }
        }
        if (isValidating) {
            var myVatTypeId = null;
            var myVatPercentage = 0;
            this.EntityPM.InvoiceLines.forEach(function (item) {
                if (item.VatPercentage != 0) {
                    if (myVatPercentage != 0 && item.VatPercentage != myVatPercentage) {
                        _this.Errors.push("Only one vat percantage per invoice is allowed!");
                    }
                }
                else if (item.VatPercentage == 0) {
                    if (myVatTypeId != null && item.VatTypeId != myVatTypeId) {
                        _this.Errors.push("Only one vat percantage per invoice is allowed!");
                    }
                    else {
                        myVatTypeId = item.VatTypeId;
                    }
                }
                myVatPercentage = item.VatPercentage;
            });
        }
    };
    return ARInvoiceValidator;
}());
exports.ARInvoiceValidator = ARInvoiceValidator;
var LineCurrency = /** @class */ (function () {
    function LineCurrency(id, code, rate) {
        this.Validated = false;
        this.Id = id;
        this.Code = code;
        this.Rate = rate;
    }
    return LineCurrency;
}());
//# sourceMappingURL=ARInvoiceValidator.js.map