"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var APInvoiceEntityPM = /** @class */ (function () {
    function APInvoiceEntityPM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(APInvoiceEntityPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "APInvoiceId", {
        get: function () { return this.aPInvoiceId; },
        set: function (newValue) { this.aPInvoiceId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "EntityId", {
        get: function () { return this.entityId; },
        set: function (newValue) { this.entityId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "EntityReference", {
        get: function () { return this.entityReference; },
        set: function (newValue) { this.entityReference = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { this.indexOrder = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceEntityPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    APInvoiceEntityPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    APInvoiceEntityPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    APInvoiceEntityPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return APInvoiceEntityPM;
}());
exports.APInvoiceEntityPM = APInvoiceEntityPM;
//# sourceMappingURL=APInvoiceEntityPM.js.map