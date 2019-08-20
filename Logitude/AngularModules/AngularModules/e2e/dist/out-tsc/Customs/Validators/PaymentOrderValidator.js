"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var PaymentOrderValidator = /** @class */ (function () {
    function PaymentOrderValidator() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    PaymentOrderValidator.prototype.SetEntityPM = function (paymentOrderPM) {
        this._PaymentOrderPM = paymentOrderPM;
    };
    PaymentOrderValidator.prototype.SubmitDateTimeCheck = function () {
        var errorMessage = "";
        if (this._PaymentOrderPM != null) {
            //if (this._PaymentOrderPM.SubmitDate == null) {
            //    errorMessage = errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            //}
            //else{
            //    //if ((this._PaymentOrderPM.SubmitDate.getFullYear != Date.) ||
            //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
            //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day)) {
            //    //    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            //    //}
            //}
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    PaymentOrderValidator.prototype.Validate = function (entityPM) {
        var result = [];
        this._PaymentOrderPM = entityPM;
        return this.ValidationErrorMessageCodes;
    };
    PaymentOrderValidator.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    return PaymentOrderValidator;
}());
exports.PaymentOrderValidator = PaymentOrderValidator;
//# sourceMappingURL=PaymentOrderValidator.js.map