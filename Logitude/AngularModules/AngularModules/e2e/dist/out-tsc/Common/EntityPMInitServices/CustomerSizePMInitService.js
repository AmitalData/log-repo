"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomerSizePMInitService = /** @class */ (function () {
    function CustomerSizePMInitService() {
    }
    CustomerSizePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.Order = 0;
        }
    };
    CustomerSizePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return CustomerSizePMInitService;
}());
exports.CustomerSizePMInitService = CustomerSizePMInitService;
//# sourceMappingURL=CustomerSizePMInitService.js.map