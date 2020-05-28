"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var core_1 = require("@angular/core");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var INTTRASettingPM = /** @class */ (function () {
    function INTTRASettingPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(INTTRASettingPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "OutSettingsId", {
        get: function () { return this.outSettingsId; },
        set: function (newValue) { if (this.outSettingsId != newValue) {
            this.outSettingsId = newValue;
            this.MarkAsDirty("OutSettingsId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "OutSettingsHost", {
        get: function () { return this.outSettingsHost; },
        set: function (newValue) { if (this.outSettingsHost != newValue) {
            this.outSettingsHost = newValue;
            this.MarkAsDirty("OutSettingsHost");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "InSettingsId", {
        get: function () { return this.inSettingsId; },
        set: function (newValue) { if (this.inSettingsId != newValue) {
            this.inSettingsId = newValue;
            this.MarkAsDirty("InSettingsId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "InSettingsHost", {
        get: function () { return this.inSettingsHost; },
        set: function (newValue) { if (this.inSettingsHost != newValue) {
            this.inSettingsHost = newValue;
            this.MarkAsDirty("InSettingsHost");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "INTTRASettingModeCode", {
        get: function () { return this.iNTTRASettingModeCode; },
        set: function (newValue) { if (this.iNTTRASettingModeCode != newValue) {
            this.iNTTRASettingModeCode = newValue;
            this.MarkAsDirty("INTTRASettingModeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "INTTRAId", {
        get: function () { return this.iNTTRAId; },
        set: function (newValue) { if (this.iNTTRAId != newValue) {
            this.iNTTRAId = newValue;
            this.MarkAsDirty("INTTRAId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(INTTRASettingPM.prototype, "INTTRAAlias", {
        get: function () { return this.iNTTRAAlias; },
        set: function (newValue) { if (this.iNTTRAAlias != newValue) {
            this.iNTTRAAlias = newValue;
            this.MarkAsDirty("INTTRAAlias");
        } },
        enumerable: true,
        configurable: true
    });
    INTTRASettingPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "INTTRASetting");
        }
    };
    INTTRASettingPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    INTTRASettingPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], INTTRASettingPM.prototype, "PropertyChanged", void 0);
    return INTTRASettingPM;
}());
exports.INTTRASettingPM = INTTRASettingPM;
//# sourceMappingURL=INTTRASettingPM.js.map