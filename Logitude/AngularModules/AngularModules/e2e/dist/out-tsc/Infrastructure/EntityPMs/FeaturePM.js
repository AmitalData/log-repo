"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeaturePM = /** @class */ (function () {
    function FeaturePM() {
    }
    Object.defineProperty(FeaturePM.prototype, "RoleId", {
        get: function () { return this.roleId; },
        set: function (newValue) { if (this.roleId != newValue) {
            this.roleId = newValue;
            this.MarkAsDirty();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FeaturePM.prototype, "AccessLevelCode", {
        get: function () { return this.accessLevelCode; },
        set: function (newValue) { if (this.accessLevelCode != newValue) {
            this.accessLevelCode = newValue;
            this.MarkAsDirty();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FeaturePM.prototype, "IsAdded", {
        get: function () { return this.isAdded; },
        set: function (newValue) { if (this.isAdded != newValue) {
            this.isAdded = newValue;
            this.MarkAsDirty();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FeaturePM.prototype, "IsRemoved", {
        get: function () { return this.isRemoved; },
        set: function (newValue) { if (this.isRemoved != newValue) {
            this.isRemoved = newValue;
            this.MarkAsDirty();
        } },
        enumerable: true,
        configurable: true
    });
    FeaturePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return FeaturePM;
}());
exports.FeaturePM = FeaturePM;
//# sourceMappingURL=FeaturePM.js.map