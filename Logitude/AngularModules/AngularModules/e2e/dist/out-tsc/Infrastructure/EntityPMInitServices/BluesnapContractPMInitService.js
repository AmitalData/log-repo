"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BluesnapContractPMInitService = /** @class */ (function () {
    function BluesnapContractPMInitService() {
    }
    BluesnapContractPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", true);
            entityPM.UIProperties.SetEnabled("BluesnapContractTypeCode", "BluesnapContract", true);
        }
    };
    BluesnapContractPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (isNew) {
            entityPM.UIProperties.SetEnabled("BluesnapContractTypeCode", "BluesnapContract", true);
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", true);
        }
        else {
            entityPM.UIProperties.SetEnabled("BluesnapContractTypeCode", "BluesnapContract", false);
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", false);
        }
    };
    return BluesnapContractPMInitService;
}());
exports.BluesnapContractPMInitService = BluesnapContractPMInitService;
//# sourceMappingURL=BluesnapContractPMInitService.js.map