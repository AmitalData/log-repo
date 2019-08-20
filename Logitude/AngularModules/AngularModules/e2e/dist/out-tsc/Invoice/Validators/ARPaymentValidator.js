"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ARPaymentValidator = /** @class */ (function () {
    function ARPaymentValidator() {
    }
    ARPaymentValidator.prototype.Validate = function (entityPm) {
        var validationResults = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(entityPm, null, validationResults);
        var isNegativeAmountEnabled = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments && entityPm.AccountingPaymentMethodCode == "FS" ? true : false;
        if (entityPm.RegisterDate == null) {
            validationResults.push(msg.replace("%FieldName", "Register Date"));
        }
        else if (Tools_1.DateTool.GetDateParts(entityPm.RegisterDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetFutureDatePayment"));
        }
        if (entityPm.AmountInPaymentCurrency == 0) {
            var isAllowed = false;
            if (entityPm.AccountingPaymentMethodCode != null) {
                if (entityPm.AccountingPaymentMethodCode.toUpperCase() == "FS") {
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
                validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetZeroAmount"));
            }
        }
        if (entityPm.AccountingPaymentMethodCode == "CH") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
                validationResults.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef")));
            }
        }
        if (entityPm.HasInvoicesErrors) {
            validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.PaymentInvoicesHaveErrors"));
        }
        var myLinesPaidAmount = 0;
        var isNoPaidAmount;
        entityPm.PaymentInvoices.forEach(function (item) {
            myLinesPaidAmount += item.PaymentAmount;
            if (Tools_1.AppTool.IsNullOrZero(item.ForeignAmount)) {
                isNoPaidAmount = true;
            }
        });
        if (isNoPaidAmount == true) {
            validationResults.push("Can't connect lines with zero Amount to Pay");
        }
        var myLinesPaidAmountRounded = Tools_1.AppTool.Round(myLinesPaidAmount, 2);
        if (isNegativeAmountEnabled == false) {
            if (entityPm.AmountInPaymentCurrency < 0) {
                validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetMinusAmount"));
            }
            if (myLinesPaidAmountRounded < 0) {
                validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeMinus"));
            }
        }
        if (myLinesPaidAmountRounded > entityPm.AmountInPaymentCurrency) {
            validationResults.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeBigger"));
        }
        if (entityPm.AccountingPaymentMethodCode == "CC") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CreditCardTypeId)) {
                validationResults.push("Credit Card Type Field is required");
            }
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.MetodoPagoCode)) {
                validationResults.push("Metodo Pago Field is Required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.SATPaymentMethodCode)) {
                validationResults.push("Forma Pago Field is Required");
            }
            if (entityPm.SATPaymentMethodCode == "99") {
                validationResults.push("Forma Pago value can't be 'Por Definir'.Please choose another value.");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPm.TipoCadenaPago) && entityPm.TipoCadenaPago == "01" && entityPm.SATPaymentMethodCode == "03") {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CertPago))
                    validationResults.push(msg.replace("%FieldName", "Cert Pago"));
                if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CadPago))
                    validationResults.push(msg.replace("%FieldName", "Cad Pago"));
                if (Tools_1.AppTool.IsNullOrEmpty(entityPm.SelloPago))
                    validationResults.push(msg.replace("%FieldName", "Sello Pago"));
            }
        }
        return validationResults;
    };
    ARPaymentValidator.ValidateCurrenctEntity = function (entityPm) {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(entityPm, null, errors);
        var isNegativeAmountEnabled = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments && entityPm.AccountingPaymentMethodCode == "FS" ? true : false;
        if (entityPm.RegisterDate == null) {
            errors.push(msg.replace("%FieldName", "Register Date"));
        }
        else if (Tools_1.DateTool.GetDateParts(entityPm.RegisterDate).DateTicks > Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetFutureDatePayment"));
        }
        if (entityPm.AmountInPaymentCurrency == 0) {
            var isAllowed = false;
            if (entityPm.AccountingPaymentMethodCode != null) {
                if (entityPm.AccountingPaymentMethodCode.toUpperCase() == "FS") {
                    isAllowed = true;
                    //if (entityPm.PaymentInvoices.length == 0) {
                    //    errors.push("You should have 1 Invoice line at least");
                    //}
                    //else {
                    //    isAllowed = true;
                    //}
                }
            }
            if (!isAllowed) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetZeroAmount"));
            }
        }
        if (entityPm.AccountingPaymentMethodCode == "CH") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef")));
            }
        }
        if (entityPm.HasInvoicesErrors) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.PaymentInvoicesHaveErrors"));
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
            errors.push("Can't connect lines with zero Amount to Pay");
        }
        var paymentAmountPaid = Tools_1.AppTool.Round(result, 2);
        if (isNegativeAmountEnabled == false) {
            if (entityPm.AmountInPaymentCurrency < 0) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantSetMinusAmount"));
            }
            if (paymentAmountPaid < 0) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeMinus"));
            }
        }
        if (paymentAmountPaid > entityPm.AmountInPaymentCurrency) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeBigger"));
        }
        if (entityPm.AccountingPaymentMethodCode == "CC") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CreditCardTypeId)) {
                errors.push("Credit Card Type Field is required");
            }
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.MetodoPagoCode)) {
                errors.push("Metodo Pago Field is Required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entityPm.SATPaymentMethodCode)) {
                errors.push("Forma Pago Field is Required");
            }
            if (entityPm.SATPaymentMethodCode == "99") {
                errors.push("Forma Pago value can't be 'Por Definir'.Please choose another value.");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPm.TipoCadenaPago) && entityPm.TipoCadenaPago == "01" && entityPm.SATPaymentMethodCode == "03") {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CertPago))
                    errors.push(msg.replace("%FieldName", "Cert Pago"));
                if (Tools_1.AppTool.IsNullOrEmpty(entityPm.CadPago))
                    errors.push(msg.replace("%FieldName", "Cad Pago"));
                if (Tools_1.AppTool.IsNullOrEmpty(entityPm.SelloPago))
                    errors.push(msg.replace("%FieldName", "Sello Pago"));
            }
        }
        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }
        return errors;
    };
    return ARPaymentValidator;
}());
exports.ARPaymentValidator = ARPaymentValidator;
//# sourceMappingURL=ARPaymentValidator.js.map