"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AirlinePMCustomCode = /** @class */ (function () {
    function AirlinePMCustomCode() {
    }
    AirlinePMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        if (propertyName == "LimitedLength") {
            if (entityPM.LimitedLength == false) {
                entityPM.CheckDigit = false;
                entityPM.UIProperties.SetEnabled("CheckDigit", "Airline", false);
            }
            else {
                entityPM.UIProperties.SetEnabled("CheckDigit", "Airline", true);
            }
        }
    };
    return AirlinePMCustomCode;
}());
exports.AirlinePMCustomCode = AirlinePMCustomCode;
//# sourceMappingURL=AirlinePMCustomCode.js.map