"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var SharedLogisticContactPM = /** @class */ (function () {
    function SharedLogisticContactPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(SharedLogisticContactPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "CardId", {
        get: function () { return this.cardId; },
        set: function (newValue) { this.cardId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "ContactId", {
        get: function () { return this.contactId; },
        set: function (newValue) { this.contactId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "Email", {
        get: function () { return this.email; },
        set: function (newValue) { this.email = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "InternetAccess", {
        get: function () { return this.internetAccess; },
        set: function (newValue) { this.internetAccess = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "EnglishName", {
        get: function () { return this.englishName; },
        set: function (newValue) { this.englishName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "Position", {
        get: function () { return this.position; },
        set: function (newValue) { this.position = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "BusinessPhone", {
        get: function () { return this.businessPhone; },
        set: function (newValue) { this.businessPhone = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "Mobile", {
        get: function () { return this.mobile; },
        set: function (newValue) { this.mobile = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "Fax", {
        get: function () { return this.fax; },
        set: function (newValue) { this.fax = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedLogisticContactPM.prototype, "LastLoginDate", {
        get: function () { return this.lastLoginDate; },
        set: function (newValue) { this.lastLoginDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    SharedLogisticContactPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    SharedLogisticContactPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    SharedLogisticContactPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return SharedLogisticContactPM;
}());
exports.SharedLogisticContactPM = SharedLogisticContactPM;
//# sourceMappingURL=SharedLogisticContactPM.js.map