"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ChargesTypeValidator = /** @class */ (function () {
    function ChargesTypeValidator() {
    }
    ChargesTypeValidator.prototype.Validate = function (entityPM) {
        var errors = [];
        if (entityPM.IsAutoDisplayInQuote || entityPM.IsAutoDisplayInShipment || entityPM.IsAutoDisplayInConsolidation || entityPM.IsAutoDisplayInCustoms) {
            if (!entityPM.IsExport && !entityPM.IsImport && !entityPM.IsDomestic && !entityPM.IsDrop) {
                errors.push("Please select at least one direction (export, import, domestic or drop)");
            }
        }
        return errors;
    };
    return ChargesTypeValidator;
}());
exports.ChargesTypeValidator = ChargesTypeValidator;
//# sourceMappingURL=ChargesTypeValidator.js.map