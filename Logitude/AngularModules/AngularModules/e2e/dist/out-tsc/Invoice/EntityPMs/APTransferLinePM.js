"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var APTransferLinePM = /** @class */ (function () {
    function APTransferLinePM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(APTransferLinePM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "APInvoiceId", {
        get: function () { return this.aPInvoiceId; },
        set: function (newValue) { this.aPInvoiceId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "Table", {
        get: function () { return this.table; },
        set: function (newValue) { this.table = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "FieldName", {
        get: function () { return this.fieldName; },
        set: function (newValue) { this.fieldName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "FieldExternalTableId", {
        get: function () { return this.fieldExternalTableId; },
        set: function (newValue) { this.fieldExternalTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "FieldValue", {
        get: function () { return this.fieldValue; },
        set: function (newValue) { this.fieldValue = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { this.description = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "DescriptionValue", {
        get: function () { return this.descriptionValue; },
        set: function (newValue) { this.descriptionValue = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "DescriptionHelp", {
        get: function () { return this.descriptionHelp; },
        set: function (newValue) { this.descriptionHelp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APTransferLinePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    APTransferLinePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    APTransferLinePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    APTransferLinePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return APTransferLinePM;
}());
exports.APTransferLinePM = APTransferLinePM;
//# sourceMappingURL=APTransferLinePM.js.map