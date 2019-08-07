"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PackageTypePMInitService = /** @class */ (function () {
    function PackageTypePMInitService() {
    }
    PackageTypePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.AddedManually = true;
        }
    };
    PackageTypePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", entityPM.IsContainer);
        entityPM.UIProperties.SetEnabled("IsVehicle", "PackageType", !entityPM.IsContainer);
    };
    return PackageTypePMInitService;
}());
exports.PackageTypePMInitService = PackageTypePMInitService;
//# sourceMappingURL=PackageTypePMInitService.js.map