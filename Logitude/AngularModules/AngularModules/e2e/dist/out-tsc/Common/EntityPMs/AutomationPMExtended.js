"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var AutomationPM = /** @class */ (function () {
    function AutomationPM() {
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(AutomationPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty("Id"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty("Name"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; this.MarkAsDirty("ObjectTableId"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Type", {
        get: function () { return this.type; },
        set: function (newValue) { this.type = newValue; this.MarkAsDirty("Type"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "ResultCode", {
        get: function () { return this.resultCode; },
        set: function (newValue) { this.resultCode = newValue; this.MarkAsDirty("ResultCode"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { this.description = newValue; this.MarkAsDirty("Description"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Inactive", {
        get: function () { return this.inactive; },
        set: function (newValue) { this.inactive = newValue; this.MarkAsDirty("Inactive"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "CreateDate", {
        get: function () { return this.createDate; },
        set: function (newValue) { this.createDate = newValue; this.MarkAsDirty("CreateDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "UpdateDate", {
        get: function () { return this.updateDate; },
        set: function (newValue) { this.updateDate = newValue; this.MarkAsDirty("UpdateDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "CreatedByUserId", {
        get: function () { return this.createdByUserId; },
        set: function (newValue) { this.createdByUserId = newValue; this.MarkAsDirty("CreatedByUserId"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "UpdatedByUserId", {
        get: function () { return this.updatedByUserId; },
        set: function (newValue) { this.updatedByUserId = newValue; this.MarkAsDirty("UpdatedByUserId"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "DocumentTypeId", {
        get: function () { return this.documentTypeId; },
        set: function (newValue) { this.documentTypeId = newValue; this.MarkAsDirty("DocumentTypeId"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "TemplateId", {
        get: function () { return this.templateId; },
        set: function (newValue) { this.templateId = newValue; this.MarkAsDirty("TemplateId"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "CreatedByUserName", {
        get: function () { return this.createdByUserName; },
        set: function (newValue) { this.createdByUserName = newValue; this.MarkAsDirty("CreatedByUserName"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "UpdatedByUserName", {
        get: function () { return this.updatedByUserName; },
        set: function (newValue) { this.updatedByUserName = newValue; this.MarkAsDirty("UpdatedByUserName"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "From", {
        get: function () { return this.from; },
        set: function (newValue) { this.from = newValue; this.MarkAsDirty("From"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "FromEmail", {
        get: function () { return this.fromEmail; },
        set: function (newValue) { this.fromEmail = newValue; this.MarkAsDirty("FromEmail"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "AutomationXML", {
        get: function () { return this.automationXML; },
        set: function (newValue) { this.automationXML = newValue; this.MarkAsDirty("AutomationXML"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Version", {
        get: function () { return this.version; },
        set: function (newValue) { this.version = newValue; this.MarkAsDirty("Version"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Order", {
        get: function () { return this.order; },
        set: function (newValue) { this.order = newValue; this.MarkAsDirty("Order"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "AutomatedDataBackup", {
        get: function () { return this.automatedDataBackup; },
        set: function (newValue) { this.automatedDataBackup = newValue; this.MarkAsDirty("AutomatedDataBackup"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "IsChangeAutomationXaml", {
        get: function () { return this.isChangeAutomationXaml; },
        set: function (newValue) { this.isChangeAutomationXaml = newValue; this.MarkAsDirty("IsChangeAutomationXaml"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "AutomationResultEmailRecipientLists", {
        get: function () { return this.automationResultEmailRecipientLists; },
        set: function (newValue) { this.automationResultEmailRecipientLists = newValue; this.MarkAsDirty("AutomationResultEmailRecipientLists"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationPM.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { if (this.code != newValue) {
            this.code = newValue;
            this.MarkAsDirty("Code");
        } },
        enumerable: true,
        configurable: true
    });
    AutomationPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Automation");
        }
    };
    AutomationPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    AutomationPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return AutomationPM;
}());
exports.AutomationPM = AutomationPM;
//# sourceMappingURL=AutomationPMExtended.js.map