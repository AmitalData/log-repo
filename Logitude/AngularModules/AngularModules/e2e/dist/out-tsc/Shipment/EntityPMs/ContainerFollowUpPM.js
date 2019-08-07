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
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var ContainerFollowUpPM = /** @class */ (function () {
    function ContainerFollowUpPM() {
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(ContainerFollowUpPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipmentId", {
        get: function () { return this.shipmentId; },
        set: function (newValue) { if (this.shipmentId != newValue) {
            this.shipmentId = newValue;
            this.MarkAsDirty("ShipmentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipmentNumber", {
        get: function () { return this.shipmentNumber; },
        set: function (newValue) { if (this.shipmentNumber != newValue) {
            this.shipmentNumber = newValue;
            this.MarkAsDirty("ShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "CustomerName", {
        get: function () { return this.customerName; },
        set: function (newValue) { if (this.customerName != newValue) {
            this.customerName = newValue;
            this.MarkAsDirty("CustomerName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "CustomerContactName", {
        get: function () { return this.customerContactName; },
        set: function (newValue) { if (this.customerContactName != newValue) {
            this.customerContactName = newValue;
            this.MarkAsDirty("CustomerContactName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "CarrierName", {
        get: function () { return this.carrierName; },
        set: function (newValue) { if (this.carrierName != newValue) {
            this.carrierName = newValue;
            this.MarkAsDirty("CarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipperName", {
        get: function () { return this.shipperName; },
        set: function (newValue) { if (this.shipperName != newValue) {
            this.shipperName = newValue;
            this.MarkAsDirty("ShipperName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ConsigneeName", {
        get: function () { return this.consigneeName; },
        set: function (newValue) { if (this.consigneeName != newValue) {
            this.consigneeName = newValue;
            this.MarkAsDirty("ConsigneeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ConsigneeReference", {
        get: function () { return this.consigneeReference; },
        set: function (newValue) { if (this.consigneeReference != newValue) {
            this.consigneeReference = newValue;
            this.MarkAsDirty("ConsigneeReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "LongMaster", {
        get: function () { return this.longMaster; },
        set: function (newValue) { if (this.longMaster != newValue) {
            this.longMaster = newValue;
            this.MarkAsDirty("LongMaster");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "House", {
        get: function () { return this.house; },
        set: function (newValue) { if (this.house != newValue) {
            this.house = newValue;
            this.MarkAsDirty("House");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DirectionId", {
        get: function () { return this.directionId; },
        set: function (newValue) { if (this.directionId != newValue) {
            this.directionId = newValue;
            this.MarkAsDirty("DirectionId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DirectionName", {
        get: function () { return this.directionName; },
        set: function (newValue) { if (this.directionName != newValue) {
            this.directionName = newValue;
            this.MarkAsDirty("DirectionName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "TransportModeId", {
        get: function () { return this.transportModeId; },
        set: function (newValue) { if (this.transportModeId != newValue) {
            this.transportModeId = newValue;
            this.MarkAsDirty("TransportModeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "TransportModeName", {
        get: function () { return this.transportModeName; },
        set: function (newValue) { if (this.transportModeName != newValue) {
            this.transportModeName = newValue;
            this.MarkAsDirty("TransportModeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipmentType", {
        get: function () { return this.shipmentType; },
        set: function (newValue) { if (this.shipmentType != newValue) {
            this.shipmentType = newValue;
            this.MarkAsDirty("ShipmentType");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipmentLevelCode", {
        get: function () { return this.shipmentLevelCode; },
        set: function (newValue) { if (this.shipmentLevelCode != newValue) {
            this.shipmentLevelCode = newValue;
            this.MarkAsDirty("ShipmentLevelCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipmentLevelName", {
        get: function () { return this.shipmentLevelName; },
        set: function (newValue) { if (this.shipmentLevelName != newValue) {
            this.shipmentLevelName = newValue;
            this.MarkAsDirty("ShipmentLevelName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "StatusId", {
        get: function () { return this.statusId; },
        set: function (newValue) { if (this.statusId != newValue) {
            this.statusId = newValue;
            this.MarkAsDirty("StatusId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ContainerTypeName", {
        get: function () { return this.containerTypeName; },
        set: function (newValue) { if (this.containerTypeName != newValue) {
            this.containerTypeName = newValue;
            this.MarkAsDirty("ContainerTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ContainerNumber", {
        get: function () { return this.containerNumber; },
        set: function (newValue) { if (this.containerNumber != newValue) {
            this.containerNumber = newValue;
            this.MarkAsDirty("ContainerNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipperSeal", {
        get: function () { return this.shipperSeal; },
        set: function (newValue) { if (this.shipperSeal != newValue) {
            this.shipperSeal = newValue;
            this.MarkAsDirty("ShipperSeal");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "Volume", {
        get: function () { return this.volume; },
        set: function (newValue) { if (this.volume != newValue) {
            this.volume = newValue;
            this.MarkAsDirty("Volume");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "IsDangerous", {
        get: function () { return this.isDangerous; },
        set: function (newValue) { if (this.isDangerous != newValue) {
            this.isDangerous = newValue;
            this.MarkAsDirty("IsDangerous");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) { if (this.description != newValue) {
            this.description = newValue;
            this.MarkAsDirty("Description");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "MarksAndNumbers", {
        get: function () { return this.marksAndNumbers; },
        set: function (newValue) { if (this.marksAndNumbers != newValue) {
            this.marksAndNumbers = newValue;
            this.MarkAsDirty("MarksAndNumbers");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "IsDeliveryFU", {
        get: function () { return this.isDeliveryFU; },
        set: function (newValue) { if (this.isDeliveryFU != newValue) {
            this.isDeliveryFU = newValue;
            this.MarkAsDirty("IsDeliveryFU");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryId", {
        get: function () { return this.deliveryId; },
        set: function (newValue) { if (this.deliveryId != newValue) {
            this.deliveryId = newValue;
            this.MarkAsDirty("DeliveryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryETD", {
        get: function () { return this.deliveryETD; },
        set: function (newValue) { if (this.deliveryETD != newValue) {
            this.deliveryETD = newValue;
            this.MarkAsDirty("DeliveryETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryATD", {
        get: function () { return this.deliveryATD; },
        set: function (newValue) { if (this.deliveryATD != newValue) {
            this.deliveryATD = newValue;
            this.MarkAsDirty("DeliveryATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryETA", {
        get: function () { return this.deliveryETA; },
        set: function (newValue) { if (this.deliveryETA != newValue) {
            this.deliveryETA = newValue;
            this.MarkAsDirty("DeliveryETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryATA", {
        get: function () { return this.deliveryATA; },
        set: function (newValue) { if (this.deliveryATA != newValue) {
            this.deliveryATA = newValue;
            this.MarkAsDirty("DeliveryATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryDeparture", {
        get: function () { return this.deliveryDeparture; },
        set: function (newValue) { if (this.deliveryDeparture != newValue) {
            this.deliveryDeparture = newValue;
            this.MarkAsDirty("DeliveryDeparture");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryArrival", {
        get: function () { return this.deliveryArrival; },
        set: function (newValue) { if (this.deliveryArrival != newValue) {
            this.deliveryArrival = newValue;
            this.MarkAsDirty("DeliveryArrival");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryFrom", {
        get: function () { return this.deliveryFrom; },
        set: function (newValue) { if (this.deliveryFrom != newValue) {
            this.deliveryFrom = newValue;
            this.MarkAsDirty("DeliveryFrom");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "DeliveryTo", {
        get: function () { return this.deliveryTo; },
        set: function (newValue) { if (this.deliveryTo != newValue) {
            this.deliveryTo = newValue;
            this.MarkAsDirty("DeliveryTo");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "IsEmptyContainerReturnFU", {
        get: function () { return this.isEmptyContainerReturnFU; },
        set: function (newValue) { if (this.isEmptyContainerReturnFU != newValue) {
            this.isEmptyContainerReturnFU = newValue;
            this.MarkAsDirty("IsEmptyContainerReturnFU");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnId", {
        get: function () { return this.emptyContainerReturnId; },
        set: function (newValue) { if (this.emptyContainerReturnId != newValue) {
            this.emptyContainerReturnId = newValue;
            this.MarkAsDirty("EmptyContainerReturnId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnETD", {
        get: function () { return this.emptyContainerReturnETD; },
        set: function (newValue) { if (this.emptyContainerReturnETD != newValue) {
            this.emptyContainerReturnETD = newValue;
            this.MarkAsDirty("EmptyContainerReturnETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnATD", {
        get: function () { return this.emptyContainerReturnATD; },
        set: function (newValue) { if (this.emptyContainerReturnATD != newValue) {
            this.emptyContainerReturnATD = newValue;
            this.MarkAsDirty("EmptyContainerReturnATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnETA", {
        get: function () { return this.emptyContainerReturnETA; },
        set: function (newValue) { if (this.emptyContainerReturnETA != newValue) {
            this.emptyContainerReturnETA = newValue;
            this.MarkAsDirty("EmptyContainerReturnETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnATA", {
        get: function () { return this.emptyContainerReturnATA; },
        set: function (newValue) { if (this.emptyContainerReturnATA != newValue) {
            this.emptyContainerReturnATA = newValue;
            this.MarkAsDirty("EmptyContainerReturnATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ReturnDeparture", {
        get: function () { return this.returnDeparture; },
        set: function (newValue) { if (this.returnDeparture != newValue) {
            this.returnDeparture = newValue;
            this.MarkAsDirty("ReturnDeparture");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ReturnArrival", {
        get: function () { return this.returnArrival; },
        set: function (newValue) { if (this.returnArrival != newValue) {
            this.returnArrival = newValue;
            this.MarkAsDirty("ReturnArrival");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnFrom", {
        get: function () { return this.emptyContainerReturnFrom; },
        set: function (newValue) { if (this.emptyContainerReturnFrom != newValue) {
            this.emptyContainerReturnFrom = newValue;
            this.MarkAsDirty("EmptyContainerReturnFrom");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "EmptyContainerReturnTo", {
        get: function () { return this.emptyContainerReturnTo; },
        set: function (newValue) { if (this.emptyContainerReturnTo != newValue) {
            this.emptyContainerReturnTo = newValue;
            this.MarkAsDirty("EmptyContainerReturnTo");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "VesselName", {
        get: function () { return this.vesselName; },
        set: function (newValue) { if (this.vesselName != newValue) {
            this.vesselName = newValue;
            this.MarkAsDirty("VesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContainerFollowUpPM.prototype, "ShipmentNotes", {
        get: function () { return this.shipmentNotes; },
        set: function (newValue) { if (this.shipmentNotes != newValue) {
            this.shipmentNotes = newValue;
            this.MarkAsDirty("ShipmentNotes");
        } },
        enumerable: true,
        configurable: true
    });
    ContainerFollowUpPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ContainerFollowUp");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ContainerFollowUpPM.prototype, "PropertyChanged", void 0);
    return ContainerFollowUpPM;
}());
exports.ContainerFollowUpPM = ContainerFollowUpPM;
//# sourceMappingURL=ContainerFollowUpPM.js.map