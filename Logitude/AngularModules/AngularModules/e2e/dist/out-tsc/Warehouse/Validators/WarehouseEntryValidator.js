"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var Tools_2 = require("../Tools");
var WarehouseEntryValidator = /** @class */ (function () {
    function WarehouseEntryValidator() {
    }
    WarehouseEntryValidator.prototype.Validate = function (entityPM) {
        var error = [];
        var message = "Can't set Field to future date";
        var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (entityPM.ActualEntryDate) {
            if (entityPM.ActualEntryDate.valueOf() > todayDateTime.valueOf()) {
                error.push(message.replace("Field", "Actual Entry Date"));
            }
        }
        if ((entityPM.WarehouseEntryPackages && entityPM.WarehouseEntryPackages.length == 0) || !entityPM.WarehouseEntryPackages) {
            error.push("You should at least add one package");
        }
        if (!Tools_2.WarehouseTools.IsInlandDomestic(entityPM.TransportModeId, entityPM.DirectionId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.FromPortId)) {
                error.push("Origin field is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ToPortId)) {
                error.push("Destination field is required");
            }
        }
        return error;
    };
    return WarehouseEntryValidator;
}());
exports.WarehouseEntryValidator = WarehouseEntryValidator;
//# sourceMappingURL=WarehouseEntryValidator.js.map