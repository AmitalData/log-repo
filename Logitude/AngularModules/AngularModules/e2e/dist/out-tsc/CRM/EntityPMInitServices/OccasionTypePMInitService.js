"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var OccasionTypePMInitService = /** @class */ (function () {
    function OccasionTypePMInitService() {
    }
    OccasionTypePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
        }
    };
    OccasionTypePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("Code", "OccasionType", false);
        }
    };
    return OccasionTypePMInitService;
}());
exports.OccasionTypePMInitService = OccasionTypePMInitService;
//# sourceMappingURL=OccasionTypePMInitService.js.map