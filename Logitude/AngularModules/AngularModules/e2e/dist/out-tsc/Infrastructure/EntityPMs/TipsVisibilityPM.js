"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var TipsVisibilityPM = /** @class */ (function () {
    function TipsVisibilityPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(TipsVisibilityPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TipsVisibilityPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TipsVisibilityPM.prototype, "UserId", {
        get: function () { return this.userId; },
        set: function (newValue) { this.userId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TipsVisibilityPM.prototype, "TipCode", {
        get: function () { return this.tipCode; },
        set: function (newValue) { this.tipCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TipsVisibilityPM.prototype, "IsVisible", {
        get: function () { return this.isVisible; },
        set: function (newValue) { this.isVisible = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    TipsVisibilityPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return TipsVisibilityPM;
}());
exports.TipsVisibilityPM = TipsVisibilityPM;
//# sourceMappingURL=TipsVisibilityPM.js.map