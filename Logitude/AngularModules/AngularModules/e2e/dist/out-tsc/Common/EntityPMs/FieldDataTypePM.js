"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var FieldDataTypePM = /** @class */ (function () {
    function FieldDataTypePM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(FieldDataTypePM.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldDataTypePM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FieldDataTypePM.prototype, "SearchFields", {
        get: function () { return this.searchFields; },
        set: function (newValue) { this.searchFields = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    FieldDataTypePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return FieldDataTypePM;
}());
exports.FieldDataTypePM = FieldDataTypePM;
//# sourceMappingURL=FieldDataTypePM.js.map