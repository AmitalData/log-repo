"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PackageTypePMCustomCode = /** @class */ (function () {
    function PackageTypePMCustomCode() {
    }
    PackageTypePMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        if (propertyName == "IsContainer") {
            if (entityPM.IsContainer) {
                entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", true);
                entityPM.UIProperties.SetEnabled("IsVehicle", "PackageType", false);
                if (entityPM.IsVehicle) {
                    entityPM.IsVehicle = false;
                }
            }
            else {
                entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", false);
                entityPM.UIProperties.SetEnabled("IsVehicle", "PackageType", true);
                if (entityPM.IsRefrigerated) {
                    entityPM.IsRefrigerated = false;
                }
            }
        }
        else if (propertyName == "IsVehicle") {
            if (entityPM.IsVehicle) {
                entityPM.UIProperties.SetEnabled("IsContainer", "PackageType", false);
                if (entityPM.IsContainer) {
                    entityPM.IsContainer = false;
                }
            }
            else {
                entityPM.UIProperties.SetEnabled("IsContainer", "PackageType", true);
            }
        }
    };
    return PackageTypePMCustomCode;
}());
exports.PackageTypePMCustomCode = PackageTypePMCustomCode;
//# sourceMappingURL=PackageTypePMCustomCode.js.map