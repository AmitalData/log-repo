"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var TermsofUsePM = /** @class */ (function () {
    function TermsofUsePM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(TermsofUsePM.prototype, "Version", {
        get: function () { return this.version; },
        set: function (newValue) { this.version = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TermsofUsePM.prototype, "Date", {
        get: function () { return this.date; },
        set: function (newValue) { this.date = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TermsofUsePM.prototype, "DivSelectBackgroud", {
        get: function () { return this.divSelectBackgroud; },
        set: function (newValue) { this.divSelectBackgroud = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    TermsofUsePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    TermsofUsePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    TermsofUsePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return TermsofUsePM;
}());
exports.TermsofUsePM = TermsofUsePM;
//# sourceMappingURL=TermsofUsePM.js.map