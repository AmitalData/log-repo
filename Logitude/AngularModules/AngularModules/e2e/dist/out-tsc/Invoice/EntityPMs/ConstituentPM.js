"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ConstituentPM = /** @class */ (function () {
    function ConstituentPM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ConstituentPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstituentPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstituentPM.prototype, "ConsolidationInvoiceId", {
        get: function () { return this.consolidationInvoiceId; },
        set: function (newValue) { this.consolidationInvoiceId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstituentPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstituentPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    ConstituentPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    ConstituentPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ConstituentPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return ConstituentPM;
}());
exports.ConstituentPM = ConstituentPM;
//# sourceMappingURL=ConstituentPM.js.map