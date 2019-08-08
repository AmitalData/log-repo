"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TMBudgetPMInitService = /** @class */ (function () {
    function TMBudgetPMInitService() {
    }
    TMBudgetPMInitService.InitValues = function (entityPM, isNew) {
    };
    TMBudgetPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("Inactive", "TMBudget", false);
        }
    };
    return TMBudgetPMInitService;
}());
exports.TMBudgetPMInitService = TMBudgetPMInitService;
//# sourceMappingURL=TMBudgetPMInitService.js.map