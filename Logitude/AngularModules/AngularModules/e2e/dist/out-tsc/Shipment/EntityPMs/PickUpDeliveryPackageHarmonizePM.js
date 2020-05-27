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
var PickUpDeliveryPackageHarmonizePM = /** @class */ (function () {
    function PickUpDeliveryPackageHarmonizePM(_entityParentPM) {
        this.PropertyChanged = new core_1.EventEmitter();
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(PickUpDeliveryPackageHarmonizePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickUpDeliveryPackageHarmonizePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickUpDeliveryPackageHarmonizePM.prototype, "PackageId", {
        get: function () { return this.packageId; },
        set: function (newValue) { if (this.packageId != newValue) {
            this.packageId = newValue;
            this.MarkAsDirty("PackageId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickUpDeliveryPackageHarmonizePM.prototype, "Harmonize", {
        get: function () { return this.harmonize; },
        set: function (newValue) { if (this.harmonize != newValue) {
            this.harmonize = newValue;
            this.MarkAsDirty("Harmonize");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickUpDeliveryPackageHarmonizePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { if (this.changeSetOp != newValue) {
            this.changeSetOp = newValue;
            this.MarkAsDirty("ChangeSetOp");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickUpDeliveryPackageHarmonizePM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    PickUpDeliveryPackageHarmonizePM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "PickUpDeliveryPackageHarmonize");
        }
    };
    PickUpDeliveryPackageHarmonizePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    PickUpDeliveryPackageHarmonizePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], PickUpDeliveryPackageHarmonizePM.prototype, "PropertyChanged", void 0);
    return PickUpDeliveryPackageHarmonizePM;
}());
exports.PickUpDeliveryPackageHarmonizePM = PickUpDeliveryPackageHarmonizePM;
//# sourceMappingURL=PickUpDeliveryPackageHarmonizePM.js.map