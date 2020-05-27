"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var VesselPMInitService = /** @class */ (function () {
    function VesselPMInitService() {
    }
    VesselPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.AddedManually = true;
        }
    };
    VesselPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return VesselPMInitService;
}());
exports.VesselPMInitService = VesselPMInitService;
//# sourceMappingURL=VesselPMInitService.js.map