"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var EntityChangePM = /** @class */ (function () {
    function EntityChangePM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(EntityChangePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "EntityId", {
        get: function () { return this.entityId; },
        set: function (newValue) { this.entityId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "CreateDate", {
        get: function () { return this.createDate; },
        set: function (newValue) { this.createDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "CreateByUserId", {
        get: function () { return this.createByUserId; },
        set: function (newValue) { this.createByUserId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "CreateByUserName", {
        get: function () { return this.createByUserName; },
        set: function (newValue) { this.createByUserName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "AutomationConditionFieldsXml", {
        get: function () { return this.automationConditionFieldsXml; },
        set: function (newValue) { this.automationConditionFieldsXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "ChangesFieldsXml", {
        get: function () { return this.changesFieldsXml; },
        set: function (newValue) { this.changesFieldsXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "ChangesAutomationFieldsXml", {
        get: function () { return this.changesAutomationFieldsXml; },
        set: function (newValue) { this.changesAutomationFieldsXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "SetAutomationSsucceedXml", {
        get: function () { return this.setAutomationSsucceedXml; },
        set: function (newValue) { this.setAutomationSsucceedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "EmailAutomationSsucceedXml", {
        get: function () { return this.emailAutomationSsucceedXml; },
        set: function (newValue) { this.emailAutomationSsucceedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "SetAutomationFailedXml", {
        get: function () { return this.setAutomationFailedXml; },
        set: function (newValue) { this.setAutomationFailedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "EmailAutomationFailedXml", {
        get: function () { return this.emailAutomationFailedXml; },
        set: function (newValue) { this.emailAutomationFailedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "HasExecutedRecord", {
        get: function () { return this.hasExecutedRecord; },
        set: function (newValue) { this.hasExecutedRecord = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "CheckStartDate", {
        get: function () { return this.checkStartDate; },
        set: function (newValue) { this.checkStartDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "DoneDate", {
        get: function () { return this.doneDate; },
        set: function (newValue) { this.doneDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "ExecutionTime", {
        get: function () { return this.executionTime; },
        set: function (newValue) { this.executionTime = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "FollowUpAutomationFailedXml", {
        get: function () { return this.followUpAutomationFailedXml; },
        set: function (newValue) { this.followUpAutomationFailedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "SetSLAAutomationFailedXml", {
        get: function () { return this.setSLAAutomationFailedXml; },
        set: function (newValue) { this.setSLAAutomationFailedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "FollowUpAutomationSsucceedXml", {
        get: function () { return this.followUpAutomationSsucceedXml; },
        set: function (newValue) { this.followUpAutomationSsucceedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EntityChangePM.prototype, "SetSLAAutomationSsucceedXml", {
        get: function () { return this.setSLAAutomationSsucceedXml; },
        set: function (newValue) { this.setSLAAutomationSsucceedXml = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    EntityChangePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return EntityChangePM;
}());
exports.EntityChangePM = EntityChangePM;
//# sourceMappingURL=EntityChangePM.js.map