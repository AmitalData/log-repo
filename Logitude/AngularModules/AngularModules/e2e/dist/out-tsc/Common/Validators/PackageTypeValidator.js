"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var PackageTypeValidator = /** @class */ (function () {
    function PackageTypeValidator() {
    }
    PackageTypeValidator.prototype.Validate = function (entityPM) {
        var errors = [];
        var valid = ((entityPM.IsAir) || (entityPM.IsOcean) || (entityPM.IsInland));
        if (!valid) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("PackageType.M.ChoosePackageTypeTransportation"));
        }
        return errors;
    };
    return PackageTypeValidator;
}());
exports.PackageTypeValidator = PackageTypeValidator;
//# sourceMappingURL=PackageTypeValidator.js.map