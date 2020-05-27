"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var EmailAlertSettingPM = /** @class */ (function () {
    function EmailAlertSettingPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(EmailAlertSettingPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { this.description = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "InActive", {
        get: function () { return this.inActive; },
        set: function (newValue) { this.inActive = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "SettingLevelCode", {
        get: function () { return this.settingLevelCode; },
        set: function (newValue) { this.settingLevelCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "To", {
        get: function () { return this.to; },
        set: function (newValue) { this.to = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmailAlertSettingPM.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { this.indexOrder = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    EmailAlertSettingPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return EmailAlertSettingPM;
}());
exports.EmailAlertSettingPM = EmailAlertSettingPM;
//# sourceMappingURL=EmailAlertSettingPM.js.map