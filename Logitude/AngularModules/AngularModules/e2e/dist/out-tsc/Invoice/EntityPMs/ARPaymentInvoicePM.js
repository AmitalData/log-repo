"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ARPaymentInvoicePM = /** @class */ (function () {
    function ARPaymentInvoicePM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ARPaymentInvoicePM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ARPaymentId", {
        get: function () { return this.aRPaymentId; },
        set: function (newValue) { this.aRPaymentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ARInvoiceId", {
        get: function () { return this.aRInvoiceId; },
        set: function (newValue) { this.aRInvoiceId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "LocalAmount", {
        get: function () { return this.localAmount; },
        set: function (newValue) { this.localAmount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ForeignAmount", {
        get: function () { return this.foreignAmount; },
        set: function (newValue) { this.foreignAmount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ForeignCurrencyId", {
        get: function () { return this.foreignCurrencyId; },
        set: function (newValue) { this.foreignCurrencyId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ARInvoiceNumber", {
        get: function () { return this.aRInvoiceNumber; },
        set: function (newValue) { this.aRInvoiceNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ExchangeRate", {
        get: function () { return this.exchangeRate; },
        set: function (newValue) { if (this.exchangeRate != newValue) {
            this.exchangeRate = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "PaymentAmount", {
        get: function () { return this.paymentAmount; },
        set: function (newValue) { if (this.paymentAmount != newValue) {
            this.paymentAmount = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ARInvoiceMetodoPagoCode", {
        get: function () { return this.aRInvoiceMetodoPagoCode; },
        set: function (newValue) { this.aRInvoiceMetodoPagoCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ARInvoiceTransferStatusCode", {
        get: function () { return this.aRInvoiceTransferStatusCode; },
        set: function (newValue) { this.aRInvoiceTransferStatusCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoicePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    ARPaymentInvoicePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    ARPaymentInvoicePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ARPaymentInvoicePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return ARPaymentInvoicePM;
}());
exports.ARPaymentInvoicePM = ARPaymentInvoicePM;
//# sourceMappingURL=ARPaymentInvoicePM.js.map