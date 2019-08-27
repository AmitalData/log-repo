"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WarehouseTools = /** @class */ (function () {
    function WarehouseTools() {
    }
    WarehouseTools.GetFromPortLabels = function (transportModeId) {
        var fromPortText = "";
        switch (transportModeId) {
            case "A": {
                fromPortText = "Gateway";
                break;
            }
            case "O": {
                fromPortText = "Loading Port";
                break;
            }
            case "I": {
                fromPortText = "From";
                break;
            }
            default: {
                fromPortText = "From";
                break;
            }
        }
        return fromPortText;
    };
    WarehouseTools.GetToPortLabels = function (transportModeId) {
        var toPortText = "";
        switch (transportModeId) {
            case "A": {
                toPortText = "Destination";
                break;
            }
            case "O": {
                toPortText = "Discharge Port";
                break;
            }
            case "I": {
                toPortText = "To";
                break;
            }
            default: {
                toPortText = "To";
                break;
            }
        }
        return toPortText;
    };
    WarehouseTools.IsInlandDomestic = function (transportModeId, directionId) {
        return transportModeId == "I" && directionId == "D" ? true : false;
    };
    return WarehouseTools;
}());
exports.WarehouseTools = WarehouseTools;
//# sourceMappingURL=Tools.js.map