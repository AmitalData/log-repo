"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var AutomationHistoryPM = /** @class */ (function () {
    function AutomationHistoryPM() {
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(AutomationHistoryPM.prototype, "AutomationsId", {
        get: function () { return this.automationsId; },
        set: function (newValue) { this.automationsId = newValue; this.MarkAsDirty("AutomationsId"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationHistoryPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationHistoryPM.prototype, "CreateDate", {
        get: function () { return this.createDate; },
        set: function (newValue) { this.createDate = newValue; this.MarkAsDirty("CreateDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationHistoryPM.prototype, "Version", {
        get: function () { return this.version; },
        set: function (newValue) { this.version = newValue; this.MarkAsDirty("Version"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationHistoryPM.prototype, "AutomationXML", {
        get: function () { return this.automationXML; },
        set: function (newValue) { this.automationXML = newValue; this.MarkAsDirty("AutomationXML"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationHistoryPM.prototype, "AutomatedDataBackup", {
        get: function () { return this.automatedDataBackup; },
        set: function (newValue) { this.automatedDataBackup = newValue; this.MarkAsDirty("AutomatedDataBackup"); },
        enumerable: true,
        configurable: true
    });
    AutomationHistoryPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "AutomationHistory");
        }
    };
    return AutomationHistoryPM;
}());
exports.AutomationHistoryPM = AutomationHistoryPM;
//# sourceMappingURL=AutomationHistoryPM.js.map