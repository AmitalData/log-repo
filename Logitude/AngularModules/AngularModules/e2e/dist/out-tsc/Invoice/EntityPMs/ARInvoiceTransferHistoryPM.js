"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ARInvoiceTransferHistoryPM = /** @class */ (function () {
    function ARInvoiceTransferHistoryPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ARInvoiceTransferHistoryPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferHistoryPM.prototype, "ARInvoiceId", {
        get: function () { return this.aRInvoiceId; },
        set: function (newValue) { this.aRInvoiceId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferHistoryPM.prototype, "TransferNumber", {
        get: function () { return this.transferNumber; },
        set: function (newValue) { this.transferNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferHistoryPM.prototype, "TransferDate", {
        get: function () { return this.transferDate; },
        set: function (newValue) { this.transferDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferHistoryPM.prototype, "FileName", {
        get: function () { return this.fileName; },
        set: function (newValue) { this.fileName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    ARInvoiceTransferHistoryPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    ARInvoiceTransferHistoryPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ARInvoiceTransferHistoryPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return ARInvoiceTransferHistoryPM;
}());
exports.ARInvoiceTransferHistoryPM = ARInvoiceTransferHistoryPM;
//# sourceMappingURL=ARInvoiceTransferHistoryPM.js.map