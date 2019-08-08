"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var WarehouseReleaseValidator = /** @class */ (function () {
    function WarehouseReleaseValidator() {
    }
    WarehouseReleaseValidator.prototype.Validate = function (entityPM) {
        var error = [];
        var message = "Can't set Field to future date";
        var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (entityPM.ActualReleaseDate) {
            if (entityPM.ActualReleaseDate.valueOf() > todayDateTime.valueOf()) {
                error.push(message.replace("Field", "Actual Release Date"));
            }
        }
        if ((entityPM.WarehouseReleasePackages && entityPM.WarehouseReleasePackages.length == 0) || !entityPM.WarehouseReleasePackages) {
            error.push("You should at least choose one package");
        }
        return error;
    };
    return WarehouseReleaseValidator;
}());
exports.WarehouseReleaseValidator = WarehouseReleaseValidator;
//# sourceMappingURL=WarehouseReleaseValidator.js.map