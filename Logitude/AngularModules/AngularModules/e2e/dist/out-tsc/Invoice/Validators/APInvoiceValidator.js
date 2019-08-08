"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var VatTypesValidator_1 = require("../../Infrastructure/Validators/VatTypesValidator");
var APInvoiceValidator = /** @class */ (function () {
    function APInvoiceValidator() {
        this.Errors = [];
        this.Errors = [];
        this.message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    APInvoiceValidator.prototype.Validate = function (entityPM) {
        var _this = this;
        this.Errors = [];
        this.EntityPM = entityPM;
        Validator_1.Validator.TryValidateObject(entityPM, "APInvoice", this.Errors);
        var allVatTypes = VatTypesValidator_1.VatTypesValidator.GetAllVatTypes();
        if (Tools_1.DateTool.GetDateParts(this.EntityPM.InvoiceDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.VATNumber)) {
                this.Errors.push(this.message.replace("%FieldName", "Vat Number"));
            }
        }
        if (entityPM.IsMultipleEntities) {
        }
        else {
            if (entityPM.InvoiceLines.length == 0) {
                this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.YouShouldHaveOneLineAtLeast"));
            }
            if (entityPM.InvoiceExpectedAmount == null) {
                this.Errors.push(this.message.replace("%FieldName", "Invoice Amount"));
            }
            else {
                entityPM.InvoiceLines.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, "APInvoiceLine", _this.Errors);
                    if (item.InvoiceCurrencyAmount == 0) {
                        _this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.InvoiceLineAmountNotZero"));
                    }
                    if (item.VatTypeId == null) {
                        var field = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.F.VatTypeId");
                        _this.Errors.push(_this.message.replace("%FieldName", field));
                    }
                    else {
                        if (item.VatPercentage == null) {
                            var vattType = allVatTypes.filter(function (d) { return d.Id == item.VatTypeId; })[0];
                            if (vattType != null) {
                                if (!vattType.IsMultiPercentage) {
                                    var field = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.F.VatPercentage");
                                    _this.Errors.push(_this.message.replace("%FieldName", field));
                                }
                            }
                        }
                    }
                });
                if (this.EntityPM.AmountInInvoiceCurrency != this.EntityPM.AmountInInvoiceCurrency_Summary) {
                    this.Errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.InvoiceAmountNotMatched"));
                }
            }
        }
        return this.Errors;
    };
    return APInvoiceValidator;
}());
exports.APInvoiceValidator = APInvoiceValidator;
//# sourceMappingURL=APInvoiceValidator.js.map