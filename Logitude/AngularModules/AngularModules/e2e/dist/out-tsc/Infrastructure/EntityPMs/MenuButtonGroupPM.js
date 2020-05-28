"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var MenuButtonGroupPM = /** @class */ (function () {
    function MenuButtonGroupPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(MenuButtonGroupPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonGroupPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonGroupPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonGroupPM.prototype, "MenuButtonGroupType", {
        get: function () { return this.menuButtonGroupType; },
        set: function (newValue) { this.menuButtonGroupType = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonGroupPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonGroupPM.prototype, "ObjectTableName", {
        get: function () { return this.objectTableName; },
        set: function (newValue) { this.objectTableName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MenuButtonGroupPM.prototype, "MenuButtons", {
        get: function () {
            if (this.menuButtons == null) {
                this.menuButtons = [];
            }
            return this.menuButtons;
        },
        set: function (newValue) {
            if (this.menuButtons != newValue) {
                this.menuButtons = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    MenuButtonGroupPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return MenuButtonGroupPM;
}());
exports.MenuButtonGroupPM = MenuButtonGroupPM;
//# sourceMappingURL=MenuButtonGroupPM.js.map