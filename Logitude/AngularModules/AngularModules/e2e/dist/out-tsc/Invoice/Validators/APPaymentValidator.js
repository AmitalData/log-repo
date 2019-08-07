"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var APPaymentValidator = /** @class */ (function () {
    function APPaymentValidator() {
    }
    APPaymentValidator.prototype.Validate = function (entityPm) {
        var validationResults = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(entityPm, null, validationResults);
        var isNegativeAmountEnabled = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetAPPayments && entityPm.PaymentMethodCode == "FS" ? true : false;
        if (entityPm.RegisterDate == null) {
            validationResults.push(msg.replace("%FieldName", "Register Date"));
        }
        else if (Tools_1.DateTool.GetDateParts(entityPm.RegisterDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.CantSetFutureDatePayment"));
        }
        if (entityPm.AmountInPaymentCurrency == 0) {
            var isAllowed = false;
            if (entityPm.PaymentMethodCode != null) {
                if (entityPm.PaymentMethodCode.toUpperCase() == "FS") {
                    isAllowed = true;
                    //if (entityPm.PaymentInvoices.length == 0) {
                    //    validationResults.push("You should have 1 Invoice line at least");
                    //}
                    //else {
                    //    isAllowed = true;
                    //}
                }
            }
            if (!isAllowed) {
                validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.CantSetZeroAmount"));
            }
        }
        if (entityPm.PaymentMethodCode == "CH" && !entityPm.AutomaticPaymentCheque) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
                validationResults.push(msg.replace("%FieldName", "Cheque Ref"));
            }
        }
        //else {
        //    if (entityPm.PaymentMethodCode == "CH" && !entityPm.AutomaticPaymentCheque) {
        //        if (AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
        //            validationResults.push(msg.replace("%FieldName", "Cheque Ref"));
        //        }
        //    }
        //}
        if (entityPm.HasInvoicesErrors) {
            validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.PaymentInvoicesHaveErrors"));
        }
        var result = 0;
        var isNoPaidAmount;
        entityPm.PaymentInvoices.forEach(function (item) {
            result += item.PaymentAmount;
            if (Tools_1.AppTool.IsNullOrZero(item.ForeignAmount)) {
                isNoPaidAmount = true;
            }
        });
        if (isNoPaidAmount == true) {
            validationResults.push("Can't connect lines with zero Amount to Pay");
        }
        var paymentAmountPaid = Tools_1.AppTool.Round(result, 2);
        if (isNegativeAmountEnabled == false) {
            if (entityPm.AmountInPaymentCurrency < 0) {
                validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.CantSetMinusAmount"));
            }
            if (paymentAmountPaid < 0) {
                validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.PaymentAmountPaidCantBeMinus"));
            }
        }
        if (paymentAmountPaid > entityPm.AmountInPaymentCurrency) {
            validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.PaymentAmountPaidCantBeBigger"));
        }
        if (entityPm.PaymentMethodCode == "CC") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CreditCardTypeId)) {
                validationResults.push("Credit Card Type Field is required");
            }
        }
        return validationResults;
    };
    return APPaymentValidator;
}());
exports.APPaymentValidator = APPaymentValidator;
//# sourceMappingURL=APPaymentValidator.js.map