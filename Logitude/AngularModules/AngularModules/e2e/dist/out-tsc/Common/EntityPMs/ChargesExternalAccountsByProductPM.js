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
var ChargesExternalAccountsByProductPM = /** @class */ (function () {
    function ChargesExternalAccountsByProductPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "PayablesGLAccount", {
        get: function () { return this.payablesGLAccount; },
        set: function (newValue) { if (this.payablesGLAccount != newValue) {
            this.payablesGLAccount = newValue;
            this.MarkAsDirty("PayablesGLAccount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "PayablesCostCenter", {
        get: function () { return this.payablesCostCenter; },
        set: function (newValue) { if (this.payablesCostCenter != newValue) {
            this.payablesCostCenter = newValue;
            this.MarkAsDirty("PayablesCostCenter");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "ReceivablesGLAccount", {
        get: function () { return this.receivablesGLAccount; },
        set: function (newValue) { if (this.receivablesGLAccount != newValue) {
            this.receivablesGLAccount = newValue;
            this.MarkAsDirty("ReceivablesGLAccount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "ReceivablesCostCenter", {
        get: function () { return this.receivablesCostCenter; },
        set: function (newValue) { if (this.receivablesCostCenter != newValue) {
            this.receivablesCostCenter = newValue;
            this.MarkAsDirty("ReceivablesCostCenter");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "UpdateDate", {
        get: function () { return this.updateDate; },
        set: function (newValue) { if (this.updateDate != newValue) {
            this.updateDate = newValue;
            this.MarkAsDirty("UpdateDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "ChargesTypeId", {
        get: function () { return this.chargesTypeId; },
        set: function (newValue) { if (this.chargesTypeId != newValue) {
            this.chargesTypeId = newValue;
            this.MarkAsDirty("ChargesTypeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (newValue) { if (this.productTypeCode != newValue) {
            this.productTypeCode = newValue;
            this.MarkAsDirty("ProductTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "UpdatedByUserId", {
        get: function () { return this.updatedByUserId; },
        set: function (newValue) { if (this.updatedByUserId != newValue) {
            this.updatedByUserId = newValue;
            this.MarkAsDirty("UpdatedByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "ProductTypeName", {
        get: function () { return this.productTypeName; },
        set: function (newValue) { if (this.productTypeName != newValue) {
            this.productTypeName = newValue;
            this.MarkAsDirty("ProductTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChargesExternalAccountsByProductPM.prototype, "UpdatedByUserName", {
        get: function () { return this.updatedByUserName; },
        set: function (newValue) { if (this.updatedByUserName != newValue) {
            this.updatedByUserName = newValue;
            this.MarkAsDirty("UpdatedByUserName");
        } },
        enumerable: true,
        configurable: true
    });
    ChargesExternalAccountsByProductPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ChargesExternalAccountsByProduct");
        }
    };
    ChargesExternalAccountsByProductPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ChargesExternalAccountsByProductPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ChargesExternalAccountsByProductPM.prototype, "PropertyChanged", void 0);
    return ChargesExternalAccountsByProductPM;
}());
exports.ChargesExternalAccountsByProductPM = ChargesExternalAccountsByProductPM;
//# sourceMappingURL=ChargesExternalAccountsByProductPM.js.map