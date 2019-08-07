"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../Infrastructure/Tools");
var ARInvoiceStockValidator = /** @class */ (function () {
    function ARInvoiceStockValidator() {
    }
    ARInvoiceStockValidator.prototype.Validate = function (entityPM) {
        var errors = [];
        Validator_1.Validator.TryValidateObject(entityPM, "ARInvoiceStock", errors);
        if (entityPM.StartDate != null && entityPM.EndDate != null) {
            if (Tools_1.DateTool.GetDateParts(entityPM.StartDate).DateTicks > Tools_1.DateTool.GetDateParts(entityPM.EndDate).DateTicks) {
                errors.push("Start Date cannot be greater than End Date");
            }
        }
        return errors;
    };
    return ARInvoiceStockValidator;
}());
exports.ARInvoiceStockValidator = ARInvoiceStockValidator;
//# sourceMappingURL=ARInvoiceStockValidator.js.map