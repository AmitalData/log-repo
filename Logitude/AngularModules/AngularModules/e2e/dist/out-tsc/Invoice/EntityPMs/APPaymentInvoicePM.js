"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var APPaymentInvoicePM = /** @class */ (function () {
    function APPaymentInvoicePM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(APPaymentInvoicePM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "APPaymentId", {
        get: function () { return this.aPPaymentId; },
        set: function (newValue) { this.aPPaymentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "APInvoiceId", {
        get: function () { return this.aPInvoiceId; },
        set: function (newValue) { this.aPInvoiceId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "LocalAmount", {
        get: function () { return this.localAmount; },
        set: function (newValue) { this.localAmount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "ForeignAmount", {
        get: function () { return this.foreignAmount; },
        set: function (newValue) { this.foreignAmount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "ForeignCurrencyId", {
        get: function () { return this.foreignCurrencyId; },
        set: function (newValue) { this.foreignCurrencyId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "APInvoiceNumber", {
        get: function () { return this.aPInvoiceNumber; },
        set: function (newValue) { this.aPInvoiceNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "ExchangeRate", {
        get: function () { return this.exchangeRate; },
        set: function (newValue) { if (this.exchangeRate != newValue) {
            this.exchangeRate = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "PaymentAmount", {
        get: function () { return this.paymentAmount; },
        set: function (newValue) { if (this.paymentAmount != newValue) {
            this.paymentAmount = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoicePM.prototype, "APInvoiceTransferStatusCode", {
        get: function () { return this.aPInvoiceTransferStatusCode; },
        set: function (newValue) { this.aPInvoiceTransferStatusCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    APPaymentInvoicePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    APPaymentInvoicePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    APPaymentInvoicePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return APPaymentInvoicePM;
}());
exports.APPaymentInvoicePM = APPaymentInvoicePM;
//# sourceMappingURL=APPaymentInvoicePM.js.map