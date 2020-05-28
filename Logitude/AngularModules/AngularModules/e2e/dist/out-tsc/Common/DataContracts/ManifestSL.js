"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ManifestSL = /** @class */ (function () {
    function ManifestSL() {
    }
    Object.defineProperty(ManifestSL.prototype, "Houses", {
        get: function () {
            if (this.houses == null) {
                this.houses = new Array();
            }
            return this.houses;
        },
        set: function (value) {
            this.houses = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ManifestSL.prototype, "ShipmentPackages", {
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
    return ManifestSL;
}());
exports.ManifestSL = ManifestSL;
//# sourceMappingURL=ManifestSL.js.map