"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ProductTypePMInitService = /** @class */ (function () {
    function ProductTypePMInitService() {
    }
    ProductTypePMInitService.InitValues = function (entityPM, isNew) {
    };
    ProductTypePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        entityPM.UIProperties.SetEnabled("Code", "ProductType", false);
        entityPM.UIProperties.SetEnabled("Name", "ProductType", false);
    };
    return ProductTypePMInitService;
}());
exports.ProductTypePMInitService = ProductTypePMInitService;
//# sourceMappingURL=ProductTypePMInitService.js.map