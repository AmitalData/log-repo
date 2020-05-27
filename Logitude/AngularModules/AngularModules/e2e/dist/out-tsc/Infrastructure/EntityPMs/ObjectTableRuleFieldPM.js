"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ObjectTableRuleFieldPM = /** @class */ (function () {
    function ObjectTableRuleFieldPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ObjectTableRuleFieldPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    ObjectTableRuleFieldPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return ObjectTableRuleFieldPM;
}());
exports.ObjectTableRuleFieldPM = ObjectTableRuleFieldPM;
//# sourceMappingURL=ObjectTableRuleFieldPM.js.map