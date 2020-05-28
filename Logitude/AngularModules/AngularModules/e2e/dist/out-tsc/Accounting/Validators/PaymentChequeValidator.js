"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PaymentChequeService_1 = require("../Services/Others/PaymentChequeService");
var PaymentChequeValidator = /** @class */ (function () {
    function PaymentChequeValidator() {
    }
    PaymentChequeValidator.prototype.Validate = function (entityPM) {
        var errors = [];
        var result = [];
        this.CheckIfExists(entityPM);
        return errors;
    };
    PaymentChequeValidator.prototype.CheckIfExists = function (entityPM) {
        var errors = [];
        var paymentChequeService = new PaymentChequeService_1.PaymentChequeService();
        paymentChequeService.CheckIfPaymentChequeExists(entityPM.BankAccountId, entityPM.ChequeNumber).subscribe(function (exist) {
            if (exist) {
                errors.push("Payment Cheque already exists");
                //SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
                //SessionLocator.CurrentSession.CurrentEditComponent.IsEditValid = false;
                return errors;
            }
            else {
                //SessionLocator.CurrentSession.CurrentEditComponent.IsEditValid = true;
                //SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            }
        });
        //}
    };
    return PaymentChequeValidator;
}());
exports.PaymentChequeValidator = PaymentChequeValidator;
//# sourceMappingURL=PaymentChequeValidator.js.map