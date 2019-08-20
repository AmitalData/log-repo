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
var core_1 = require("@angular/core");
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var ShipmentPickUpPM = /** @class */ (function () {
    function ShipmentPickUpPM(_entityParentPM) {
        this.PropertyChanged = new core_1.EventEmitter();
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ShipmentPickUpPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ShipmentId", {
        get: function () { return this.shipmentId; },
        set: function (newValue) { this.shipmentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "PickUpDeliveryNumber", {
        get: function () { return this.pickUpDeliveryNumber; },
        set: function (newValue) { this.pickUpDeliveryNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "PickUpDeliveryTypeCode", {
        get: function () { return this.pickUpDeliveryTypeCode; },
        set: function (newValue) { this.pickUpDeliveryTypeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FullResponsibility", {
        get: function () { return this.fullResponsibility; },
        set: function (newValue) { this.fullResponsibility = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "PickUpDeliveryFromTypeCode", {
        get: function () { return this.pickUpDeliveryFromTypeCode; },
        set: function (newValue) { this.pickUpDeliveryFromTypeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "PickUpDeliveryToTypeCode", {
        get: function () { return this.pickUpDeliveryToTypeCode; },
        set: function (newValue) { this.pickUpDeliveryToTypeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromPortId", {
        get: function () { return this.fromPortId; },
        set: function (newValue) { this.fromPortId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToPortId", {
        get: function () { return this.toPortId; },
        set: function (newValue) { this.toPortId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromPartnerCardId", {
        get: function () { return this.fromPartnerCardId; },
        set: function (newValue) { this.fromPartnerCardId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressId", {
        get: function () { return this.fromAddressId; },
        set: function (newValue) { this.fromAddressId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddress", {
        get: function () { return this.fromAddress; },
        set: function (newValue) { this.fromAddress = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressCity", {
        get: function () { return this.fromAddressCity; },
        set: function (newValue) { this.fromAddressCity = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressZipCode", {
        get: function () { return this.fromAddressZipCode; },
        set: function (newValue) { this.fromAddressZipCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressCountryId", {
        get: function () { return this.fromAddressCountryId; },
        set: function (newValue) { this.fromAddressCountryId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToPartnerCardId", {
        get: function () { return this.toPartnerCardId; },
        set: function (newValue) { this.toPartnerCardId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressId", {
        get: function () { return this.toAddressId; },
        set: function (newValue) { this.toAddressId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddress", {
        get: function () { return this.toAddress; },
        set: function (newValue) { this.toAddress = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressCity", {
        get: function () { return this.toAddressCity; },
        set: function (newValue) { this.toAddressCity = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressZipCode", {
        get: function () { return this.toAddressZipCode; },
        set: function (newValue) { this.toAddressZipCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressCountryId", {
        get: function () { return this.toAddressCountryId; },
        set: function (newValue) { this.toAddressCountryId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromPortCode", {
        get: function () { return this.fromPortCode; },
        set: function (newValue) { this.fromPortCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromPortName", {
        get: function () { return this.fromPortName; },
        set: function (newValue) { this.fromPortName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromPortCountryCode", {
        get: function () { return this.fromPortCountryCode; },
        set: function (newValue) { this.fromPortCountryCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromPortCountryName", {
        get: function () { return this.fromPortCountryName; },
        set: function (newValue) { this.fromPortCountryName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToPortCode", {
        get: function () { return this.toPortCode; },
        set: function (newValue) { this.toPortCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToPortName", {
        get: function () { return this.toPortName; },
        set: function (newValue) { this.toPortName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToPortCountryCode", {
        get: function () { return this.toPortCountryCode; },
        set: function (newValue) { this.toPortCountryCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToPortCountryName", {
        get: function () { return this.toPortCountryName; },
        set: function (newValue) { this.toPortCountryName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressCity_Dummy", {
        get: function () { return this.fromAddressCity_Dummy; },
        set: function (newValue) { this.fromAddressCity_Dummy = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressCountryCode", {
        get: function () { return this.fromAddressCountryCode; },
        set: function (newValue) { this.fromAddressCountryCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromAddressCountryName", {
        get: function () { return this.fromAddressCountryName; },
        set: function (newValue) { this.fromAddressCountryName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "FromLocation", {
        get: function () { return this.fromLocation; },
        set: function (newValue) { this.fromLocation = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressCity_Dummy", {
        get: function () { return this.toAddressCity_Dummy; },
        set: function (newValue) { this.toAddressCity_Dummy = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressCountryCode", {
        get: function () { return this.toAddressCountryCode; },
        set: function (newValue) { this.toAddressCountryCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToAddressCountryName", {
        get: function () { return this.toAddressCountryName; },
        set: function (newValue) { this.toAddressCountryName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ToLocation", {
        get: function () { return this.toLocation; },
        set: function (newValue) { this.toLocation = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ATD", {
        get: function () { return this.aTD; },
        set: function (newValue) { this.aTD = newValue; this.MarkAsDirty("ATD"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ATA", {
        get: function () { return this.aTA; },
        set: function (newValue) { this.aTA = newValue; this.MarkAsDirty("ATA"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ETD", {
        get: function () { return this.eTD; },
        set: function (newValue) { this.eTD = newValue; this.MarkAsDirty("ETD"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ETA", {
        get: function () { return this.eTA; },
        set: function (newValue) { this.eTA = newValue; this.MarkAsDirty("ETA"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "CarrierId", {
        get: function () { return this.carrierId; },
        set: function (newValue) { this.carrierId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "CarrierCode", {
        get: function () { return this.carrierCode; },
        set: function (newValue) { this.carrierCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "CarrierName", {
        get: function () { return this.carrierName; },
        set: function (newValue) { this.carrierName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "CarrierNumber", {
        get: function () { return this.carrierNumber; },
        set: function (newValue) { this.carrierNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "CarrierWebSite", {
        get: function () { return this.carrierWebSite; },
        set: function (newValue) { this.carrierWebSite = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "Driver", {
        get: function () { return this.driver; },
        set: function (newValue) { this.driver = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "TruckNumber", {
        get: function () { return this.truckNumber; },
        set: function (newValue) { this.truckNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "TrailerNumber", {
        get: function () { return this.trailerNumber; },
        set: function (newValue) { this.trailerNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (newValue) { this.notes = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "EmptyPickupContainerPartnerId", {
        get: function () { return this.emptyPickupContainerPartnerId; },
        set: function (newValue) { this.emptyPickupContainerPartnerId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "EmptyDeliveryContainerPartnerId", {
        get: function () { return this.emptyDeliveryContainerPartnerId; },
        set: function (newValue) { this.emptyDeliveryContainerPartnerId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "EmptyPickupDepotReference", {
        get: function () { return this.emptyPickupDepotReference; },
        set: function (newValue) { this.emptyPickupDepotReference = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "EmptyDeliveryDepotReference", {
        get: function () { return this.emptyDeliveryDepotReference; },
        set: function (newValue) { this.emptyDeliveryDepotReference = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (newValue) { this.customerId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "DirectionId", {
        get: function () { return this.directionId; },
        set: function (newValue) { this.directionId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "MasterNumber", {
        get: function () { return this.masterNumber; },
        set: function (newValue) { this.masterNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "AgentName", {
        get: function () { return this.agentName; },
        set: function (newValue) { this.agentName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ShipmentNumber", {
        get: function () { return this.shipmentNumber; },
        set: function (newValue) { this.shipmentNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ShippingLine", {
        get: function () { return this.shippingLine; },
        set: function (newValue) { this.shippingLine = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "PackageTEU", {
        get: function () { return this.packageTEU; },
        set: function (newValue) { this.packageTEU = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "AgentId", {
        get: function () { return this.agentId; },
        set: function (newValue) { this.agentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "TransportModeCode", {
        get: function () { return this.transportModeCode; },
        set: function (newValue) { this.transportModeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "ShipmentPickUpDeliveryPackages", {
        get: function () {
            if (this.shipmentPickUpDeliveryPackages == null) {
                this.shipmentPickUpDeliveryPackages = [];
            }
            return this.shipmentPickUpDeliveryPackages;
        },
        set: function (newValue) {
            if (this.shipmentPickUpDeliveryPackages != newValue) {
                this.shipmentPickUpDeliveryPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPickUpPM.prototype.AddPackage = function (item) {
        if (item != null) {
            var index = this.ShipmentPickUpDeliveryPackages.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPickUpDeliveryPackages.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPickUpPM.prototype.RemovePackage = function (item) {
        if (item != null) {
            var index = this.ShipmentPickUpDeliveryPackages.indexOf(item);
            if (index > -1) {
                this.ShipmentPickUpDeliveryPackages.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPickUpPM.prototype, "ShipmentPickUpPackagesChangeSet", {
        get: function () {
            if (this.shipmentPickUpPackagesChangeSet == null) {
                this.shipmentPickUpPackagesChangeSet = [];
            }
            return this.shipmentPickUpPackagesChangeSet;
        },
        set: function (newValue) {
            if (this.shipmentPickUpPackagesChangeSet != newValue) {
                this.shipmentPickUpPackagesChangeSet = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    ShipmentPickUpPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ShipmentPickUpPM.prototype, "PropertyChanged", void 0);
    return ShipmentPickUpPM;
}());
exports.ShipmentPickUpPM = ShipmentPickUpPM;
//# sourceMappingURL=ShipmentPickUpPM.js.map