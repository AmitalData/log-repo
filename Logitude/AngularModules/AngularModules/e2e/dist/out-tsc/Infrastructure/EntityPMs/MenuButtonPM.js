"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var MenuButtonPM = /** @class */ (function () {
    function MenuButtonPM(entityParent) {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(MenuButtonPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "EventCode", {
        get: function () { return this.eventCode; },
        set: function (newValue) { this.eventCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "LabelTextCodeId", {
        get: function () { return this.labelTextCodeId; },
        set: function (newValue) { this.labelTextCodeId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "LabelTextCodeCode", {
        get: function () { return this.labelTextCodeCode; },
        set: function (newValue) { this.labelTextCodeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "MenuButtonGroupId", {
        get: function () { return this.menuButtonGroupId; },
        set: function (newValue) { this.menuButtonGroupId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "ParentMenuButtonId", {
        get: function () { return this.parentMenuButtonId; },
        set: function (newValue) { this.parentMenuButtonId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "Index", {
        get: function () { return this.index; },
        set: function (newValue) { this.index = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "IsActive", {
        get: function () { return this.isActive; },
        set: function (newValue) { this.isActive = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "IsHidden", {
        get: function () { return this.isHidden; },
        set: function (newValue) { this.isHidden = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "IsDisabled", {
        get: function () { return this.isDisabled; },
        set: function (newValue) { this.isDisabled = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "FeatureId", {
        get: function () { return this.featureId; },
        set: function (newValue) { this.featureId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "ShowMenuButton", {
        get: function () { return this.showMenuButton; },
        set: function (newValue) { this.showMenuButton = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "MenuButtonType", {
        get: function () { return this.menuButtonType; },
        set: function (newValue) { this.menuButtonType = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "DropDownControl", {
        get: function () { return this.dropDownControl; },
        set: function (newValue) { this.dropDownControl = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "Style", {
        get: function () { return this.style; },
        set: function (newValue) { this.style = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "Width", {
        get: function () { return this.width; },
        set: function (newValue) { this.width = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "ControlPath", {
        get: function () { return this.controlPath; },
        set: function (newValue) { this.controlPath = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "HtmlComponentPath", {
        get: function () { return this.htmlComponentPath; },
        set: function (newValue) { this.htmlComponentPath = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonPM.prototype, "DisplayText", {
        get: function () { return this.displayText; },
        set: function (newValue) { this.displayText = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    MenuButtonPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return MenuButtonPM;
}());
exports.MenuButtonPM = MenuButtonPM;
//# sourceMappingURL=MenuButtonPM.js.map