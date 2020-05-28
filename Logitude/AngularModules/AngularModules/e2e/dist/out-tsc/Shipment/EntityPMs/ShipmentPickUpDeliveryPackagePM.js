"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var ShipmentPickUpDeliveryPackagePM = /** @class */ (function () {
    function ShipmentPickUpDeliveryPackagePM(_entityParentPM) {
        this.PickUpDeliveryPackageHarmonizesChangeSet = [];
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "ContainerNumber", {
        get: function () { return this.containerNumber; },
        set: function (newValue) { this.containerNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "PackageTypeId", {
        get: function () { return this.packageTypeId; },
        set: function (newValue) { this.packageTypeId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "PackageTypeName", {
        get: function () { return this.packageTypeName; },
        set: function (newValue) { this.packageTypeName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "PackageTypeTEU", {
        get: function () { return this.packageTypeTEU; },
        set: function (newValue) { this.packageTypeTEU = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "ShipmentPickUpDeliveryId", {
        get: function () { return this.shipmentPickUpDeliveryId; },
        set: function (newValue) { this.shipmentPickUpDeliveryId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Quantity", {
        get: function () { return this.quantity; },
        set: function (newValue) { this.quantity = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Volume", {
        get: function () { return this.volume; },
        set: function (newValue) { this.volume = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Weight", {
        get: function () { return this.weight; },
        set: function (newValue) { this.weight = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { this.description = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Harmonize", {
        get: function () { return this.harmonize; },
        set: function (newValue) { this.harmonize = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "ShipperSeal", {
        get: function () { return this.shipperSeal; },
        set: function (newValue) { this.shipperSeal = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Width", {
        get: function () { return this.width; },
        set: function (newValue) { this.width = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Height", {
        get: function () { return this.height; },
        set: function (newValue) { this.height = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Length", {
        get: function () { return this.length; },
        set: function (newValue) { this.length = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "OriginalShipmentPackageId", {
        get: function () { return this.originalShipmentPackageId; },
        set: function (newValue) { this.originalShipmentPackageId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "IsMultiHarmonize", {
        get: function () { return this.isMultiHarmonize; },
        set: function (newValue) { if (this.isMultiHarmonize != newValue) {
            this.isMultiHarmonize = newValue;
            this.MarkAsDirty("IsMultiHarmonize");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Make", {
        get: function () { return this.make; },
        set: function (newValue) { if (this.make != newValue) {
            this.make = newValue;
            this.MarkAsDirty("Make");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Model", {
        get: function () { return this.model; },
        set: function (newValue) { if (this.model != newValue) {
            this.model = newValue;
            this.MarkAsDirty("Model");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Year", {
        get: function () { return this.year; },
        set: function (newValue) { if (this.year != newValue) {
            this.year = newValue;
            this.MarkAsDirty("Year");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "Color", {
        get: function () { return this.color; },
        set: function (newValue) { if (this.color != newValue) {
            this.color = newValue;
            this.MarkAsDirty("Color");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "ChassisNumber", {
        get: function () { return this.chassisNumber; },
        set: function (newValue) { if (this.chassisNumber != newValue) {
            this.chassisNumber = newValue;
            this.MarkAsDirty("ChassisNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "RegistrationNumber", {
        get: function () { return this.registrationNumber; },
        set: function (newValue) { if (this.registrationNumber != newValue) {
            this.registrationNumber = newValue;
            this.MarkAsDirty("RegistrationNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "CountryId", {
        get: function () { return this.countryId; },
        set: function (newValue) { if (this.countryId != newValue) {
            this.countryId = newValue;
            this.MarkAsDirty("CountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "PickUpDeliveryPackageHarmonizes", {
        get: function () {
            if (this.pickUpDeliveryPackageHarmonizes == null) {
                this.pickUpDeliveryPackageHarmonizes = [];
            }
            return this.pickUpDeliveryPackageHarmonizes;
        },
        set: function (newValue) {
            if (this.pickUpDeliveryPackageHarmonizes != newValue) {
                this.pickUpDeliveryPackageHarmonizes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPickUpDeliveryPackagePM.prototype.AddPickUpDeliveryPackageHarmonizePM = function (item) {
        if (item != null) {
            var index = this.PickUpDeliveryPackageHarmonizes.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.PickUpDeliveryPackageHarmonizes.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPickUpDeliveryPackagePM.prototype.RemovePickUpDeliveryPackageHarmonizePM = function (item) {
        if (item != null) {
            var index = this.PickUpDeliveryPackageHarmonizes.indexOf(item);
            if (index > -1) {
                this.PickUpDeliveryPackageHarmonizes.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliveryPackagePM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    ShipmentPickUpDeliveryPackagePM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ShipmentPickUpDeliveryPackage");
        }
    };
    ShipmentPickUpDeliveryPackagePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ShipmentPickUpDeliveryPackagePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return ShipmentPickUpDeliveryPackagePM;
}());
exports.ShipmentPickUpDeliveryPackagePM = ShipmentPickUpDeliveryPackagePM;
//# sourceMappingURL=ShipmentPickUpDeliveryPackagePM.js.map