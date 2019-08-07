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
var UIProperties_1 = require("../Components/LogitudeComponents/UIProperties");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var core_1 = require("@angular/core");
var PropertyChangedArgs_1 = require("../EventEmitterArgs/PropertyChangedArgs");
var TenantSettingPM = /** @class */ (function () {
    function TenantSettingPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(TenantSettingPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "Size", {
        get: function () { return this.size; },
        set: function (newValue) { if (this.size != newValue) {
            this.size = newValue;
            this.MarkAsDirty("Size");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "Prefix", {
        get: function () { return this.prefix; },
        set: function (newValue) { if (this.prefix != newValue) {
            this.prefix = newValue;
            this.MarkAsDirty("Prefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "SettingCode", {
        get: function () { return this.settingCode; },
        set: function (newValue) { if (this.settingCode != newValue) {
            this.settingCode = newValue;
            this.MarkAsDirty("SettingCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "SettingValue", {
        get: function () { return this.settingValue; },
        set: function (newValue) { if (this.settingValue != newValue) {
            this.settingValue = newValue;
            this.MarkAsDirty("SettingValue");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { if (this.objectTableId != newValue) {
            this.objectTableId = newValue;
            this.MarkAsDirty("ObjectTableId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantSettingPM.prototype, "DontIncludeDirects", {
        get: function () { return this.dontIncludeDirects; },
        set: function (newValue) { if (this.dontIncludeDirects != newValue) {
            this.dontIncludeDirects = newValue;
            this.MarkAsDirty("DontIncludeDirects");
        } },
        enumerable: true,
        configurable: true
    });
    TenantSettingPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "TenantSetting");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], TenantSettingPM.prototype, "PropertyChanged", void 0);
    return TenantSettingPM;
}());
exports.TenantSettingPM = TenantSettingPM;
//# sourceMappingURL=TenantSettingPM.js.map