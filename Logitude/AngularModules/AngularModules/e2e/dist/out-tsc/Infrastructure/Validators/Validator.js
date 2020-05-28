"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ClassLevelValidator_1 = require("./ClassLevelValidator");
var Validator = /** @class */ (function () {
    function Validator() {
    }
    Validator.TryValidateObject = function (instance, tableName, errors) {
        if (instance != null) {
            if (tableName != null) {
                var levelValidator = new ClassLevelValidator_1.ClassLevelValidator();
                var myResult = levelValidator.Validate(tableName, instance);
                if (myResult != null) {
                    if (errors == null) {
                        errors = [];
                    }
                    myResult.forEach(function (item) {
                        errors.push(item);
                    });
                }
            }
        }
    };
    return Validator;
}());
exports.Validator = Validator;
//# sourceMappingURL=Validator.js.map