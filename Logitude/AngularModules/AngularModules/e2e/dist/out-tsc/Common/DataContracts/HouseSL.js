"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var HouseSL = /** @class */ (function () {
    function HouseSL() {
    }
    Object.defineProperty(HouseSL.prototype, "ShipmentPackages", {
        get: function () {
            if (this.shipmentPackages == null) {
                this.shipmentPackages = [];
            }
            return this.shipmentPackages;
        },
        set: function (newValue) {
            if (this.shipmentPackages != newValue) {
                this.shipmentPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return HouseSL;
}());
exports.HouseSL = HouseSL;
//# sourceMappingURL=HouseSL.js.map