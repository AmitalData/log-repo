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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ShipmentPMCustomCode_1 = require("../EntityPMCustomCode/ShipmentPMCustomCode");
var core_1 = require("@angular/core");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var CustomFieldClass_1 = require("../../Infrastructure/DataContracts/CustomFieldClass");
var ShipmentPM = /** @class */ (function () {
    function ShipmentPM() {
        //for (var property in this) {
        //    if (this.hasOwnProperty(property)) {
        //        this[property] = null;
        //    }
        //}
        this.PropertyChanged = new core_1.EventEmitter();
        this.UIProperties = new UIProperties_1.UIProperties(this);
        this.IsDirty = false;
    }
    Object.defineProperty(ShipmentPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { if (this.id != newValue) {
            this.id = newValue;
            this.MarkAsDirty("Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsHybrid", {
        get: function () { return this.isHybrid; },
        set: function (newValue) { if (this.isHybrid != newValue) {
            this.isHybrid = newValue;
            this.MarkAsDirty("IsHybrid");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BaseShipmentNumber", {
        get: function () { return this.baseShipmentNumber; },
        set: function (newValue) { if (this.baseShipmentNumber != newValue) {
            this.baseShipmentNumber = newValue;
            this.MarkAsDirty("BaseShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConcurrencyGUID", {
        get: function () { return this.concurrencyGUID; },
        set: function (newValue) { if (this.concurrencyGUID != newValue) {
            this.concurrencyGUID = newValue;
            this.MarkAsDirty("ConcurrencyGUID");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CASSCode", {
        get: function () { return this.cASSCode; },
        set: function (newValue) { if (this.cASSCode != newValue) {
            this.cASSCode = newValue;
            this.MarkAsDirty("CASSCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DeliveryOrder", {
        get: function () { return this.deliveryOrder; },
        set: function (newValue) { if (this.deliveryOrder != newValue) {
            this.deliveryOrder = newValue;
            this.MarkAsDirty("DeliveryOrder");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ImportManifest", {
        get: function () { return this.importManifest; },
        set: function (newValue) { if (this.importManifest != newValue) {
            this.importManifest = newValue;
            this.MarkAsDirty("ImportManifest");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightLocationId", {
        get: function () { return this.freightLocationId; },
        set: function (newValue) { if (this.freightLocationId != newValue) {
            this.freightLocationId = newValue;
            this.MarkAsDirty("FreightLocationId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TransportDocumentNumber", {
        get: function () { return this.transportDocumentNumber; },
        set: function (newValue) { if (this.transportDocumentNumber != newValue) {
            this.transportDocumentNumber = newValue;
            this.MarkAsDirty("TransportDocumentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierTransportDocumentNumber", {
        get: function () { return this.carrierTransportDocumentNumber; },
        set: function (newValue) { if (this.carrierTransportDocumentNumber != newValue) {
            this.carrierTransportDocumentNumber = newValue;
            this.MarkAsDirty("CarrierTransportDocumentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OpenReceivablesInLocalCurrency", {
        get: function () { return this.openReceivablesInLocalCurrency; },
        set: function (newValue) { if (this.openReceivablesInLocalCurrency != newValue) {
            this.openReceivablesInLocalCurrency = newValue;
            this.MarkAsDirty("OpenReceivablesInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountedReceivablesInLocalCurrency", {
        get: function () { return this.accountedReceivablesInLocalCurrency; },
        set: function (newValue) { if (this.accountedReceivablesInLocalCurrency != newValue) {
            this.accountedReceivablesInLocalCurrency = newValue;
            this.MarkAsDirty("AccountedReceivablesInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProfitInLocalCurrency", {
        get: function () { return this.profitInLocalCurrency; },
        set: function (newValue) { if (this.profitInLocalCurrency != newValue) {
            this.profitInLocalCurrency = newValue;
            this.MarkAsDirty("ProfitInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "EstimateProfitInLocalCurrency", {
        get: function () { return this.estimateProfitInLocalCurrency; },
        set: function (newValue) { if (this.estimateProfitInLocalCurrency != newValue) {
            this.estimateProfitInLocalCurrency = newValue;
            this.MarkAsDirty("EstimateProfitInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OpenReceivablesInProfitCurrency", {
        get: function () { return this.openReceivablesInProfitCurrency; },
        set: function (newValue) { if (this.openReceivablesInProfitCurrency != newValue) {
            this.openReceivablesInProfitCurrency = newValue;
            this.MarkAsDirty("OpenReceivablesInProfitCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountedReceivablesInProfitCurrency", {
        get: function () { return this.accountedReceivablesInProfitCurrency; },
        set: function (newValue) { if (this.accountedReceivablesInProfitCurrency != newValue) {
            this.accountedReceivablesInProfitCurrency = newValue;
            this.MarkAsDirty("AccountedReceivablesInProfitCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProfitInProfitCurrency", {
        get: function () { return this.profitInProfitCurrency; },
        set: function (newValue) { if (this.profitInProfitCurrency != newValue) {
            this.profitInProfitCurrency = newValue;
            this.MarkAsDirty("ProfitInProfitCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "EstimateProfitInProfitCurrency", {
        get: function () { return this.estimateProfitInProfitCurrency; },
        set: function (newValue) { if (this.estimateProfitInProfitCurrency != newValue) {
            this.estimateProfitInProfitCurrency = newValue;
            this.MarkAsDirty("EstimateProfitInProfitCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OpenPayablesInLocalCurrency", {
        get: function () { return this.openPayablesInLocalCurrency; },
        set: function (newValue) { if (this.openPayablesInLocalCurrency != newValue) {
            this.openPayablesInLocalCurrency = newValue;
            this.MarkAsDirty("OpenPayablesInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountedPayablesInLocalCurrency", {
        get: function () { return this.accountedPayablesInLocalCurrency; },
        set: function (newValue) { if (this.accountedPayablesInLocalCurrency != newValue) {
            this.accountedPayablesInLocalCurrency = newValue;
            this.MarkAsDirty("AccountedPayablesInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OpenPayablesInProfitCurrency", {
        get: function () { return this.openPayablesInProfitCurrency; },
        set: function (newValue) { if (this.openPayablesInProfitCurrency != newValue) {
            this.openPayablesInProfitCurrency = newValue;
            this.MarkAsDirty("OpenPayablesInProfitCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountedPayablesInProfitCurrency", {
        get: function () { return this.accountedPayablesInProfitCurrency; },
        set: function (newValue) { if (this.accountedPayablesInProfitCurrency != newValue) {
            this.accountedPayablesInProfitCurrency = newValue;
            this.MarkAsDirty("AccountedPayablesInProfitCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CountryForStatisticsId", {
        get: function () { return this.countryForStatisticsId; },
        set: function (newValue) { if (this.countryForStatisticsId != newValue) {
            this.countryForStatisticsId = newValue;
            this.MarkAsDirty("CountryForStatisticsId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierId", {
        get: function () { return this.mainCarriageCarrierId; },
        set: function (newValue) { if (this.mainCarriageCarrierId != newValue) {
            this.mainCarriageCarrierId = newValue;
            this.MarkAsDirty("MainCarriageCarrierId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierName", {
        get: function () { return this.mainCarriageCarrierName; },
        set: function (newValue) { if (this.mainCarriageCarrierName != newValue) {
            this.mainCarriageCarrierName = newValue;
            this.MarkAsDirty("MainCarriageCarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierCode", {
        get: function () { return this.mainCarriageCarrierCode; },
        set: function (newValue) { if (this.mainCarriageCarrierCode != newValue) {
            this.mainCarriageCarrierCode = newValue;
            this.MarkAsDirty("MainCarriageCarrierCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.mainCarriageCarrierNumber; },
        set: function (newValue) { if (this.mainCarriageCarrierNumber != newValue) {
            this.mainCarriageCarrierNumber = newValue;
            this.MarkAsDirty("MainCarriageCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierAddressId", {
        get: function () { return this.mainCarriageCarrierAddressId; },
        set: function (newValue) { if (this.mainCarriageCarrierAddressId != newValue) {
            this.mainCarriageCarrierAddressId = newValue;
            this.MarkAsDirty("MainCarriageCarrierAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierWebSite", {
        get: function () { return this.mainCarriageCarrierWebSite; },
        set: function (newValue) { if (this.mainCarriageCarrierWebSite != newValue) {
            this.mainCarriageCarrierWebSite = newValue;
            this.MarkAsDirty("MainCarriageCarrierWebSite");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsFSRSent", {
        get: function () { return this.isFSRSent; },
        set: function (newValue) { if (this.isFSRSent != newValue) {
            this.isFSRSent = newValue;
            this.MarkAsDirty("IsFSRSent");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastFSRStatusRequestDate", {
        get: function () { return this.lastFSRStatusRequestDate; },
        set: function (newValue) { if (this.lastFSRStatusRequestDate != newValue) {
            this.lastFSRStatusRequestDate = newValue;
            this.MarkAsDirty("LastFSRStatusRequestDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FHLStatusDate", {
        get: function () { return this.fHLStatusDate; },
        set: function (newValue) { if (this.fHLStatusDate != newValue) {
            this.fHLStatusDate = newValue;
            this.MarkAsDirty("FHLStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FWBStatusDate", {
        get: function () { return this.fWBStatusDate; },
        set: function (newValue) { if (this.fWBStatusDate != newValue) {
            this.fWBStatusDate = newValue;
            this.MarkAsDirty("FWBStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierLastStatusCode", {
        get: function () { return this.carrierLastStatusCode; },
        set: function (newValue) { if (this.carrierLastStatusCode != newValue) {
            this.carrierLastStatusCode = newValue;
            this.MarkAsDirty("CarrierLastStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierLastStatusName", {
        get: function () { return this.carrierLastStatusName; },
        set: function (newValue) { if (this.carrierLastStatusName != newValue) {
            this.carrierLastStatusName = newValue;
            this.MarkAsDirty("CarrierLastStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierLastStatusDate", {
        get: function () { return this.carrierLastStatusDate; },
        set: function (newValue) { if (this.carrierLastStatusDate != newValue) {
            this.carrierLastStatusDate = newValue;
            this.MarkAsDirty("CarrierLastStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FNAReason", {
        get: function () { return this.fNAReason; },
        set: function (newValue) { if (this.fNAReason != newValue) {
            this.fNAReason = newValue;
            this.MarkAsDirty("FNAReason");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId1", {
        get: function () { return this.aWBSpecialHandlingCodeId1; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId1 != newValue) {
            this.aWBSpecialHandlingCodeId1 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId2", {
        get: function () { return this.aWBSpecialHandlingCodeId2; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId2 != newValue) {
            this.aWBSpecialHandlingCodeId2 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId3", {
        get: function () { return this.aWBSpecialHandlingCodeId3; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId3 != newValue) {
            this.aWBSpecialHandlingCodeId3 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId4", {
        get: function () { return this.aWBSpecialHandlingCodeId4; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId4 != newValue) {
            this.aWBSpecialHandlingCodeId4 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId4");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId5", {
        get: function () { return this.aWBSpecialHandlingCodeId5; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId5 != newValue) {
            this.aWBSpecialHandlingCodeId5 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId5");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId6", {
        get: function () { return this.aWBSpecialHandlingCodeId6; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId6 != newValue) {
            this.aWBSpecialHandlingCodeId6 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId6");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId7", {
        get: function () { return this.aWBSpecialHandlingCodeId7; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId7 != newValue) {
            this.aWBSpecialHandlingCodeId7 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId7");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId8", {
        get: function () { return this.aWBSpecialHandlingCodeId8; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId8 != newValue) {
            this.aWBSpecialHandlingCodeId8 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId8");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSpecialHandlingCodeId9", {
        get: function () { return this.aWBSpecialHandlingCodeId9; },
        set: function (newValue) { if (this.aWBSpecialHandlingCodeId9 != newValue) {
            this.aWBSpecialHandlingCodeId9 = newValue;
            this.MarkAsDirty("AWBSpecialHandlingCodeId9");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBChargeRate", {
        get: function () { return this.aWBChargeRate; },
        set: function (newValue) { if (this.aWBChargeRate != newValue) {
            this.aWBChargeRate = newValue;
            this.MarkAsDirty("AWBChargeRate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBChargeAmount", {
        get: function () { return this.aWBChargeAmount; },
        set: function (newValue) { if (this.aWBChargeAmount != newValue) {
            this.aWBChargeAmount = newValue;
            this.MarkAsDirty("AWBChargeAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBCommodityItemNumber", {
        get: function () { return this.aWBCommodityItemNumber; },
        set: function (newValue) { if (this.aWBCommodityItemNumber != newValue) {
            this.aWBCommodityItemNumber = newValue;
            this.MarkAsDirty("AWBCommodityItemNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PPCC", {
        get: function () { return this.pPCC; },
        set: function (newValue) { if (this.pPCC != newValue) {
            this.pPCC = newValue;
            this.MarkAsDirty("PPCC");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FlightDate", {
        get: function () { return this.flightDate; },
        set: function (newValue) { if (this.flightDate != newValue) {
            this.flightDate = newValue;
            this.MarkAsDirty("FlightDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsFlightDateActual", {
        get: function () { return this.isFlightDateActual; },
        set: function (newValue) { if (this.isFlightDateActual != newValue) {
            this.isFlightDateActual = newValue;
            this.MarkAsDirty("IsFlightDateActual");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConnectedShipments", {
        get: function () { return this.connectedShipments; },
        set: function (newValue) { if (this.connectedShipments != newValue) {
            this.connectedShipments = newValue;
            this.MarkAsDirty("ConnectedShipments");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MasterShipmentNumber", {
        get: function () { return this.masterShipmentNumber; },
        set: function (newValue) { if (this.masterShipmentNumber != newValue) {
            this.masterShipmentNumber = newValue;
            this.MarkAsDirty("MasterShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ChargeableWeightInKG", {
        get: function () { return this.chargeableWeightInKG; },
        set: function (newValue) { if (this.chargeableWeightInKG != newValue) {
            this.chargeableWeightInKG = newValue;
            this.MarkAsDirty("ChargeableWeightInKG");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "GrossWeightInKG", {
        get: function () { return this.grossWeightInKG; },
        set: function (newValue) { if (this.grossWeightInKG != newValue) {
            this.grossWeightInKG = newValue;
            this.MarkAsDirty("GrossWeightInKG");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ChargeableWeight", {
        get: function () { return this.chargeableWeight; },
        set: function (newValue) { if (this.chargeableWeight != newValue) {
            this.chargeableWeight = newValue;
            this.MarkAsDirty("ChargeableWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "GrossWeight", {
        get: function () { return this.grossWeight; },
        set: function (newValue) { if (this.grossWeight != newValue) {
            this.grossWeight = newValue;
            this.MarkAsDirty("GrossWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CurrentUserId", {
        get: function () { return this.currentUserId; },
        set: function (newValue) { if (this.currentUserId != newValue) {
            this.currentUserId = newValue;
            this.MarkAsDirty("CurrentUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BasketId", {
        get: function () { return this.basketId; },
        set: function (newValue) { if (this.basketId != newValue) {
            this.basketId = newValue;
            this.MarkAsDirty("BasketId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentNumber", {
        get: function () { return this.shipmentNumber; },
        set: function (newValue) { if (this.shipmentNumber != newValue) {
            this.shipmentNumber = newValue;
            this.MarkAsDirty("ShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DirectionId", {
        get: function () { return this.directionId; },
        set: function (newValue) { if (this.directionId != newValue) {
            this.directionId = newValue;
            this.MarkAsDirty("DirectionId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DirectionName", {
        get: function () { return this.directionName; },
        set: function (newValue) { if (this.directionName != newValue) {
            this.directionName = newValue;
            this.MarkAsDirty("DirectionName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TransportModeId", {
        get: function () { return this.transportModeId; },
        set: function (newValue) { if (this.transportModeId != newValue) {
            this.transportModeId = newValue;
            this.MarkAsDirty("TransportModeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TransportModeName", {
        get: function () { return this.transportModeName; },
        set: function (newValue) { if (this.transportModeName != newValue) {
            this.transportModeName = newValue;
            this.MarkAsDirty("TransportModeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentTypeId", {
        get: function () { return this.shipmentTypeId; },
        set: function (newValue) { if (this.shipmentTypeId != newValue) {
            this.shipmentTypeId = newValue;
            this.MarkAsDirty("ShipmentTypeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentTypeName", {
        get: function () { return this.shipmentTypeName; },
        set: function (newValue) { if (this.shipmentTypeName != newValue) {
            this.shipmentTypeName = newValue;
            this.MarkAsDirty("ShipmentTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "House", {
        get: function () { return this.house; },
        set: function (newValue) { if (this.house != newValue) {
            this.house = newValue;
            this.MarkAsDirty("House");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CreateDateTime", {
        get: function () { return this.createDateTime; },
        set: function (newValue) { if (this.createDateTime != newValue) {
            this.createDateTime = newValue;
            this.MarkAsDirty("CreateDateTime");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MAWBTakenFromStack", {
        get: function () { return this.mAWBTakenFromStack; },
        set: function (newValue) { if (this.mAWBTakenFromStack != newValue) {
            this.mAWBTakenFromStack = newValue;
            this.MarkAsDirty("MAWBTakenFromStack");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MAWBReturnedToStack", {
        get: function () { return this.mAWBReturnedToStack; },
        set: function (newValue) { if (this.mAWBReturnedToStack != newValue) {
            this.mAWBReturnedToStack = newValue;
            this.MarkAsDirty("MAWBReturnedToStack");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BranchId", {
        get: function () { return this.branchId; },
        set: function (newValue) { if (this.branchId != newValue) {
            this.branchId = newValue;
            this.MarkAsDirty("BranchId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BranchName", {
        get: function () { return this.branchName; },
        set: function (newValue) { if (this.branchName != newValue) {
            this.branchName = newValue;
            this.MarkAsDirty("BranchName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BranchAddress", {
        get: function () { return this.branchAddress; },
        set: function (newValue) { if (this.branchAddress != newValue) {
            this.branchAddress = newValue;
            this.MarkAsDirty("BranchAddress");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IncotermId", {
        get: function () { return this.incotermId; },
        set: function (newValue) { if (this.incotermId != newValue) {
            this.incotermId = newValue;
            this.MarkAsDirty("IncotermId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IncotermCode", {
        get: function () { return this.incotermCode; },
        set: function (newValue) { if (this.incotermCode != newValue) {
            this.incotermCode = newValue;
            this.MarkAsDirty("IncotermCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IncotermName", {
        get: function () { return this.incotermName; },
        set: function (newValue) { if (this.incotermName != newValue) {
            this.incotermName = newValue;
            this.MarkAsDirty("IncotermName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Routing", {
        get: function () { return this.routing; },
        set: function (newValue) { if (this.routing != newValue) {
            this.routing = newValue;
            this.MarkAsDirty("Routing");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SalesmanUserId", {
        get: function () { return this.salesmanUserId; },
        set: function (newValue) { if (this.salesmanUserId != newValue) {
            this.salesmanUserId = newValue;
            this.MarkAsDirty("SalesmanUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SalesmanUserName", {
        get: function () { return this.salesmanUserName; },
        set: function (newValue) { if (this.salesmanUserName != newValue) {
            this.salesmanUserName = newValue;
            this.MarkAsDirty("SalesmanUserName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CreatedByUserId", {
        get: function () { return this.createdByUserId; },
        set: function (newValue) { if (this.createdByUserId != newValue) {
            this.createdByUserId = newValue;
            this.MarkAsDirty("CreatedByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DepartmentId", {
        get: function () { return this.departmentId; },
        set: function (newValue) { if (this.departmentId != newValue) {
            this.departmentId = newValue;
            this.MarkAsDirty("DepartmentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (newValue) { if (this.notes != newValue) {
            this.notes = newValue;
            this.MarkAsDirty("Notes");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DescriptionOfGoods", {
        get: function () { return this.descriptionOfGoods; },
        set: function (newValue) { if (this.descriptionOfGoods != newValue) {
            this.descriptionOfGoods = newValue;
            this.MarkAsDirty("DescriptionOfGoods");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastModified", {
        get: function () { return this.lastModified; },
        set: function (newValue) { if (this.lastModified != newValue) {
            this.lastModified = newValue;
            this.MarkAsDirty("LastModified");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "HAWBDate", {
        get: function () { return this.hAWBDate; },
        set: function (newValue) { if (this.hAWBDate != newValue) {
            this.hAWBDate = newValue;
            this.MarkAsDirty("HAWBDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MAWBStackNumber", {
        get: function () { return this.mAWBStackNumber; },
        set: function (newValue) { if (this.mAWBStackNumber != newValue) {
            this.mAWBStackNumber = newValue;
            this.MarkAsDirty("MAWBStackNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsOperationalClosed", {
        get: function () { return this.isOperationalClosed; },
        set: function (newValue) { if (this.isOperationalClosed != newValue) {
            this.isOperationalClosed = newValue;
            this.MarkAsDirty("IsOperationalClosed");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field1", {
        get: function () { if (!this.field1) {
            this.field1 = new CustomFieldClass_1.CustomFieldClass(null, "Field1", "Shipment");
        } return this.field1; },
        set: function (newValue) { this.field1 = newValue; this.MarkAsDirty("Field1"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field2", {
        get: function () { if (!this.field2) {
            this.field2 = new CustomFieldClass_1.CustomFieldClass(null, "Field2", "Shipment");
        } return this.field2; },
        set: function (newValue) { this.field2 = newValue; this.MarkAsDirty("Field2"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field3", {
        get: function () { if (!this.field3) {
            this.field3 = new CustomFieldClass_1.CustomFieldClass(null, "Field3", "Shipment");
        } return this.field3; },
        set: function (newValue) { this.field3 = newValue; this.MarkAsDirty("Field3"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field4", {
        get: function () { if (!this.field4) {
            this.field4 = new CustomFieldClass_1.CustomFieldClass(null, "Field4", "Shipment");
        } return this.field4; },
        set: function (newValue) { this.field4 = newValue; this.MarkAsDirty("Field4"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field5", {
        get: function () { if (!this.field5) {
            this.field5 = new CustomFieldClass_1.CustomFieldClass(null, "Field5", "Shipment");
        } return this.field5; },
        set: function (newValue) { this.field5 = newValue; this.MarkAsDirty("Field5"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field6", {
        get: function () { if (!this.field6) {
            this.field6 = new CustomFieldClass_1.CustomFieldClass(null, "Field6", "Shipment");
        } return this.field6; },
        set: function (newValue) { this.field6 = newValue; this.MarkAsDirty("Field6"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field7", {
        get: function () { if (!this.field7) {
            this.field7 = new CustomFieldClass_1.CustomFieldClass(null, "Field7", "Shipment");
        } return this.field7; },
        set: function (newValue) { this.field7 = newValue; this.MarkAsDirty("Field7"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field8", {
        get: function () { if (!this.field8) {
            this.field8 = new CustomFieldClass_1.CustomFieldClass(null, "Field8", "Shipment");
        } return this.field8; },
        set: function (newValue) { this.field8 = newValue; this.MarkAsDirty("Field8"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field9", {
        get: function () { if (!this.field9) {
            this.field9 = new CustomFieldClass_1.CustomFieldClass(null, "Field9", "Shipment");
        } return this.field9; },
        set: function (newValue) { this.field9 = newValue; this.MarkAsDirty("Field9"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field10", {
        get: function () { if (!this.field10) {
            this.field10 = new CustomFieldClass_1.CustomFieldClass(null, "Field10", "Shipment");
        } return this.field10; },
        set: function (newValue) { this.field10 = newValue; this.MarkAsDirty("Field10"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field11", {
        get: function () { if (!this.field11) {
            this.field11 = new CustomFieldClass_1.CustomFieldClass(null, "Field11", "Shipment");
        } return this.field11; },
        set: function (newValue) { this.field11 = newValue; this.MarkAsDirty("Field11"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field12", {
        get: function () { if (!this.field12) {
            this.field12 = new CustomFieldClass_1.CustomFieldClass(null, "Field12", "Shipment");
        } return this.field12; },
        set: function (newValue) { this.field12 = newValue; this.MarkAsDirty("Field12"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field13", {
        get: function () { if (!this.field13) {
            this.field13 = new CustomFieldClass_1.CustomFieldClass(null, "Field13", "Shipment");
        } return this.field13; },
        set: function (newValue) { this.field13 = newValue; this.MarkAsDirty("Field13"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field14", {
        get: function () { if (!this.field14) {
            this.field14 = new CustomFieldClass_1.CustomFieldClass(null, "Field14", "Shipment");
        } return this.field14; },
        set: function (newValue) { this.field14 = newValue; this.MarkAsDirty("Field14"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field15", {
        get: function () { if (!this.field15) {
            this.field15 = new CustomFieldClass_1.CustomFieldClass(null, "Field15", "Shipment");
        } return this.field15; },
        set: function (newValue) { this.field15 = newValue; this.MarkAsDirty("Field15"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field16", {
        get: function () { if (!this.field16) {
            this.field16 = new CustomFieldClass_1.CustomFieldClass(null, "Field16", "Shipment");
        } return this.field16; },
        set: function (newValue) { this.field16 = newValue; this.MarkAsDirty("Field16"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field17", {
        get: function () { if (!this.field17) {
            this.field17 = new CustomFieldClass_1.CustomFieldClass(null, "Field17", "Shipment");
        } return this.field17; },
        set: function (newValue) { this.field17 = newValue; this.MarkAsDirty("Field17"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field18", {
        get: function () { if (!this.field18) {
            this.field18 = new CustomFieldClass_1.CustomFieldClass(null, "Field18", "Shipment");
        } return this.field18; },
        set: function (newValue) { this.field18 = newValue; this.MarkAsDirty("Field18"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field19", {
        get: function () { if (!this.field19) {
            this.field19 = new CustomFieldClass_1.CustomFieldClass(null, "Field19", "Shipment");
        } return this.field19; },
        set: function (newValue) { this.field19 = newValue; this.MarkAsDirty("Field19"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field20", {
        get: function () { if (!this.field20) {
            this.field20 = new CustomFieldClass_1.CustomFieldClass(null, "Field20", "Shipment");
        } return this.field20; },
        set: function (newValue) { this.field20 = newValue; this.MarkAsDirty("Field20"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field21", {
        get: function () { if (!this.field21) {
            this.field21 = new CustomFieldClass_1.CustomFieldClass(null, "Field21", "Shipment");
        } return this.field21; },
        set: function (newValue) { this.field21 = newValue; this.MarkAsDirty("Field21"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field22", {
        get: function () { if (!this.field22) {
            this.field22 = new CustomFieldClass_1.CustomFieldClass(null, "Field22", "Shipment");
        } return this.field22; },
        set: function (newValue) { this.field22 = newValue; this.MarkAsDirty("Field22"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field23", {
        get: function () { if (!this.field23) {
            this.field23 = new CustomFieldClass_1.CustomFieldClass(null, "Field23", "Shipment");
        } return this.field23; },
        set: function (newValue) { this.field23 = newValue; this.MarkAsDirty("Field23"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field24", {
        get: function () { if (!this.field24) {
            this.field24 = new CustomFieldClass_1.CustomFieldClass(null, "Field24", "Shipment");
        } return this.field24; },
        set: function (newValue) { this.field24 = newValue; this.MarkAsDirty("Field24"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field25", {
        get: function () { if (!this.field25) {
            this.field25 = new CustomFieldClass_1.CustomFieldClass(null, "Field25", "Shipment");
        } return this.field25; },
        set: function (newValue) { this.field25 = newValue; this.MarkAsDirty("Field25"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field26", {
        get: function () { if (!this.field26) {
            this.field26 = new CustomFieldClass_1.CustomFieldClass(null, "Field26", "Shipment");
        } return this.field6; },
        set: function (newValue) { this.field26 = newValue; this.MarkAsDirty("Field26"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field27", {
        get: function () { if (!this.field27) {
            this.field27 = new CustomFieldClass_1.CustomFieldClass(null, "Field27", "Shipment");
        } return this.field27; },
        set: function (newValue) { this.field27 = newValue; this.MarkAsDirty("Field27"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field28", {
        get: function () { if (!this.field28) {
            this.field28 = new CustomFieldClass_1.CustomFieldClass(null, "Field28", "Shipment");
        } return this.field28; },
        set: function (newValue) { this.field28 = newValue; this.MarkAsDirty("Field28"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field29", {
        get: function () { if (!this.field29) {
            this.field29 = new CustomFieldClass_1.CustomFieldClass(null, "Field29", "Shipment");
        } return this.field29; },
        set: function (newValue) { this.field29 = newValue; this.MarkAsDirty("Field29"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field30", {
        get: function () { if (!this.field30) {
            this.field30 = new CustomFieldClass_1.CustomFieldClass(null, "Field30", "Shipment");
        } return this.field30; },
        set: function (newValue) { this.field30 = newValue; this.MarkAsDirty("Field30"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field31", {
        get: function () { if (!this.field31) {
            this.field31 = new CustomFieldClass_1.CustomFieldClass(null, "Field31", "Shipment");
        } return this.field31; },
        set: function (newValue) { this.field31 = newValue; this.MarkAsDirty("Field31"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field32", {
        get: function () { if (!this.field32) {
            this.field32 = new CustomFieldClass_1.CustomFieldClass(null, "Field32", "Shipment");
        } return this.field32; },
        set: function (newValue) { this.field32 = newValue; this.MarkAsDirty("Field32"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field33", {
        get: function () { if (!this.field33) {
            this.field33 = new CustomFieldClass_1.CustomFieldClass(null, "Field33", "Shipment");
        } return this.field33; },
        set: function (newValue) { this.field33 = newValue; this.MarkAsDirty("Field33"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field34", {
        get: function () { if (!this.field34) {
            this.field34 = new CustomFieldClass_1.CustomFieldClass(null, "Field34", "Shipment");
        } return this.field34; },
        set: function (newValue) { this.field34 = newValue; this.MarkAsDirty("Field34"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field35", {
        get: function () { if (!this.field35) {
            this.field35 = new CustomFieldClass_1.CustomFieldClass(null, "Field35", "Shipment");
        } return this.field35; },
        set: function (newValue) { this.field35 = newValue; this.MarkAsDirty("Field35"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field36", {
        get: function () { if (!this.field36) {
            this.field36 = new CustomFieldClass_1.CustomFieldClass(null, "Field36", "Shipment");
        } return this.field36; },
        set: function (newValue) { this.field36 = newValue; this.MarkAsDirty("Field36"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field37", {
        get: function () { if (!this.field37) {
            this.field37 = new CustomFieldClass_1.CustomFieldClass(null, "Field37", "Shipment");
        } return this.field37; },
        set: function (newValue) { this.field37 = newValue; this.MarkAsDirty("Field37"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field38", {
        get: function () { if (!this.field38) {
            this.field38 = new CustomFieldClass_1.CustomFieldClass(null, "Field38", "Shipment");
        } return this.field38; },
        set: function (newValue) { this.field38 = newValue; this.MarkAsDirty("Field38"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field39", {
        get: function () { if (!this.field39) {
            this.field39 = new CustomFieldClass_1.CustomFieldClass(null, "Field39", "Shipment");
        } return this.field39; },
        set: function (newValue) { this.field39 = newValue; this.MarkAsDirty("Field39"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Field40", {
        get: function () { if (!this.field40) {
            this.field40 = new CustomFieldClass_1.CustomFieldClass(null, "Field40", "Shipment");
        } return this.field40; },
        set: function (newValue) { this.field40 = newValue; this.MarkAsDirty("Field40"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SearchFields", {
        get: function () { return this.searchFields; },
        set: function (newValue) { if (this.searchFields != newValue) {
            this.searchFields = newValue;
            this.MarkAsDirty("SearchFields");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsSecured", {
        get: function () { return this.isSecured; },
        set: function (newValue) { if (this.isSecured != newValue) {
            this.isSecured = newValue;
            this.MarkAsDirty("IsSecured");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Master", {
        get: function () { return this.master; },
        set: function (newValue) { if (this.master != newValue) {
            this.master = newValue;
            this.MarkAsDirty("Master");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentTypeViewField", {
        get: function () { return this.shipmentTypeViewField; },
        set: function (newValue) { if (this.shipmentTypeViewField != newValue) {
            this.shipmentTypeViewField = newValue;
            this.MarkAsDirty("ShipmentTypeViewField");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LongMaster", {
        get: function () { return this.longMaster; },
        set: function (newValue) { if (this.longMaster != newValue) {
            this.longMaster = newValue;
            this.MarkAsDirty("LongMaster");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.freightPrepaidCollectId; },
        set: function (newValue) { if (this.freightPrepaidCollectId != newValue) {
            this.freightPrepaidCollectId = newValue;
            this.MarkAsDirty("FreightPrepaidCollectId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.otherPrepaidCollectId; },
        set: function (newValue) { if (this.otherPrepaidCollectId != newValue) {
            this.otherPrepaidCollectId = newValue;
            this.MarkAsDirty("OtherPrepaidCollectId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "GrossWeightUnitCode", {
        get: function () { return this.grossWeightUnitCode; },
        set: function (newValue) { if (this.grossWeightUnitCode != newValue) {
            this.grossWeightUnitCode = newValue;
            this.MarkAsDirty("GrossWeightUnitCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.chargeableWeightUnitCode; },
        set: function (newValue) { if (this.chargeableWeightUnitCode != newValue) {
            this.chargeableWeightUnitCode = newValue;
            this.MarkAsDirty("ChargeableWeightUnitCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DimensionsUnitCode", {
        get: function () { return this.dimensionsUnitCode; },
        set: function (newValue) { if (this.dimensionsUnitCode != newValue) {
            this.dimensionsUnitCode = newValue;
            this.MarkAsDirty("DimensionsUnitCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "RateClassCode", {
        get: function () { return this.rateClassCode; },
        set: function (newValue) { if (this.rateClassCode != newValue) {
            this.rateClassCode = newValue;
            this.MarkAsDirty("RateClassCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "VolumetricWeight", {
        get: function () { return this.volumetricWeight; },
        set: function (newValue) { if (this.volumetricWeight != newValue) {
            this.volumetricWeight = newValue;
            this.MarkAsDirty("VolumetricWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "VolumeInCBM", {
        get: function () { return this.volumeInCBM; },
        set: function (newValue) { if (this.volumeInCBM != newValue) {
            this.volumeInCBM = newValue;
            this.MarkAsDirty("VolumeInCBM");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Volume", {
        get: function () { return this.volume; },
        set: function (newValue) { if (this.volume != newValue) {
            this.volume = newValue;
            this.MarkAsDirty("Volume");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackagesQuantity", {
        get: function () { return this.packagesQuantity; },
        set: function (newValue) { if (this.packagesQuantity != newValue) {
            this.packagesQuantity = newValue;
            this.MarkAsDirty("PackagesQuantity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NumberOfPackages", {
        get: function () { return this.numberOfPackages; },
        set: function (newValue) { if (this.numberOfPackages != newValue) {
            this.numberOfPackages = newValue;
            this.MarkAsDirty("NumberOfPackages");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NumberOfContainers", {
        get: function () { return this.numberOfContainers; },
        set: function (newValue) { if (this.numberOfContainers != newValue) {
            this.numberOfContainers = newValue;
            this.MarkAsDirty("NumberOfContainers");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Ratio", {
        get: function () { return this.ratio; },
        set: function (newValue) { if (this.ratio != newValue) {
            this.ratio = newValue;
            this.MarkAsDirty("Ratio");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DimFactor", {
        get: function () { return this.dimFactor; },
        set: function (newValue) { if (this.dimFactor != newValue) {
            this.dimFactor = newValue;
            this.MarkAsDirty("DimFactor");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MAWBOBLDate", {
        get: function () { return this.mAWBOBLDate; },
        set: function (newValue) { if (this.mAWBOBLDate != newValue) {
            this.mAWBOBLDate = newValue;
            this.MarkAsDirty("MAWBOBLDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "GrossWeightEdited", {
        get: function () { return this.grossWeightEdited; },
        set: function (newValue) { if (this.grossWeightEdited != newValue) {
            this.grossWeightEdited = newValue;
            this.MarkAsDirty("GrossWeightEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OrderGrossWeightEdited", {
        get: function () { return this.orderGrossWeightEdited; },
        set: function (newValue) { if (this.orderGrossWeightEdited != newValue) {
            this.orderGrossWeightEdited = newValue;
            this.MarkAsDirty("OrderGrossWeightEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ChargeableWeightEdited", {
        get: function () { return this.chargeableWeightEdited; },
        set: function (newValue) { if (this.chargeableWeightEdited != newValue) {
            this.chargeableWeightEdited = newValue;
            this.MarkAsDirty("ChargeableWeightEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OrderChargeableWeightEdited", {
        get: function () { return this.orderChargeableWeightEdited; },
        set: function (newValue) { if (this.orderChargeableWeightEdited != newValue) {
            this.orderChargeableWeightEdited = newValue;
            this.MarkAsDirty("OrderChargeableWeightEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "VolumeUnitCode", {
        get: function () { return this.volumeUnitCode; },
        set: function (newValue) { if (this.volumeUnitCode != newValue) {
            this.volumeUnitCode = newValue;
            this.MarkAsDirty("VolumeUnitCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "StatusId", {
        get: function () { return this.statusId; },
        set: function (newValue) { if (this.statusId != newValue) {
            this.statusId = newValue;
            this.MarkAsDirty("StatusId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "StatusName", {
        get: function () { return this.statusName; },
        set: function (newValue) { if (this.statusName != newValue) {
            this.statusName = newValue;
            this.MarkAsDirty("StatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "StatusDate", {
        get: function () { return this.statusDate; },
        set: function (newValue) { if (this.statusDate != newValue) {
            this.statusDate = newValue;
            this.MarkAsDirty("StatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "StatusLocation", {
        get: function () { return this.statusLocation; },
        set: function (newValue) { if (this.statusLocation != newValue) {
            this.statusLocation = newValue;
            this.MarkAsDirty("StatusLocation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "StatusWeight", {
        get: function () { return this.statusWeight; },
        set: function (newValue) { if (this.statusWeight != newValue) {
            this.statusWeight = newValue;
            this.MarkAsDirty("StatusWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "QuoteId", {
        get: function () { return this.quoteId; },
        set: function (newValue) { if (this.quoteId != newValue) {
            this.quoteId = newValue;
            this.MarkAsDirty("QuoteId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingId", {
        get: function () { return this.bookingId; },
        set: function (newValue) { if (this.bookingId != newValue) {
            this.bookingId = newValue;
            this.MarkAsDirty("BookingId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingNumber", {
        get: function () { return this.bookingNumber; },
        set: function (newValue) { if (this.bookingNumber != newValue) {
            this.bookingNumber = newValue;
            this.MarkAsDirty("BookingNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TotalContainers", {
        get: function () { return this.totalContainers; },
        set: function (newValue) { if (this.totalContainers != newValue) {
            this.totalContainers = newValue;
            this.MarkAsDirty("TotalContainers");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentType", {
        get: function () { return this.shipmentType; },
        set: function (newValue) { if (this.shipmentType != newValue) {
            this.shipmentType = newValue;
            this.MarkAsDirty("ShipmentType");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FollowUpType", {
        get: function () { return this.followUpType; },
        set: function (newValue) { if (this.followUpType != newValue) {
            this.followUpType = newValue;
            this.MarkAsDirty("FollowUpType");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FollowUpId", {
        get: function () { return this.followUpId; },
        set: function (newValue) { if (this.followUpId != newValue) {
            this.followUpId = newValue;
            this.MarkAsDirty("FollowUpId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FollowUpDate", {
        get: function () { return this.followUpDate; },
        set: function (newValue) { if (this.followUpDate != newValue) {
            this.followUpDate = newValue;
            this.MarkAsDirty("FollowUpDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPMId", {
        get: function () { return this.shipmentPMId; },
        set: function (newValue) { if (this.shipmentPMId != newValue) {
            this.shipmentPMId = newValue;
            this.MarkAsDirty("ShipmentPMId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastUpdate", {
        get: function () { return this.lastUpdate; },
        set: function (newValue) { if (this.lastUpdate != newValue) {
            this.lastUpdate = newValue;
            this.MarkAsDirty("LastUpdate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NewMessage", {
        get: function () { return this.newMessage; },
        set: function (newValue) { if (this.newMessage != newValue) {
            this.newMessage = newValue;
            this.MarkAsDirty("NewMessage");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FollowUpNotes", {
        get: function () { return this.followUpNotes; },
        set: function (newValue) { if (this.followUpNotes != newValue) {
            this.followUpNotes = newValue;
            this.MarkAsDirty("FollowUpNotes");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsAnyConversation", {
        get: function () { return this.isAnyConversation; },
        set: function (newValue) { if (this.isAnyConversation != newValue) {
            this.isAnyConversation = newValue;
            this.MarkAsDirty("IsAnyConversation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NumberOfShipments", {
        get: function () { return this.numberOfShipments; },
        set: function (newValue) { if (this.numberOfShipments != newValue) {
            this.numberOfShipments = newValue;
            this.MarkAsDirty("NumberOfShipments");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationPortId", {
        get: function () { return this.mainCarriageFinalDestinationPortId; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationPortId != newValue) {
            this.mainCarriageFinalDestinationPortId = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationPortCode", {
        get: function () { return this.mainCarriageFinalDestinationPortCode; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationPortCode != newValue) {
            this.mainCarriageFinalDestinationPortCode = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationPortName", {
        get: function () { return this.mainCarriageFinalDestinationPortName; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationPortName != newValue) {
            this.mainCarriageFinalDestinationPortName = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationPortCountryCode", {
        get: function () { return this.mainCarriageFinalDestinationPortCountryCode; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationPortCountryCode != newValue) {
            this.mainCarriageFinalDestinationPortCountryCode = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationPortCountryName", {
        get: function () { return this.mainCarriageFinalDestinationPortCountryName; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationPortCountryName != newValue) {
            this.mainCarriageFinalDestinationPortCountryName = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainHarmonize", {
        get: function () { return this.mainHarmonize; },
        set: function (newValue) { if (this.mainHarmonize != newValue) {
            this.mainHarmonize = newValue;
            this.MarkAsDirty("MainHarmonize");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsDangerous", {
        get: function () { return this.isDangerous; },
        set: function (newValue) { if (this.isDangerous != newValue) {
            this.isDangerous = newValue;
            this.MarkAsDirty("IsDangerous");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DangerousClassNumber", {
        get: function () { return this.dangerousClassNumber; },
        set: function (newValue) { if (this.dangerousClassNumber != newValue) {
            this.dangerousClassNumber = newValue;
            this.MarkAsDirty("DangerousClassNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DangerousUnNumber", {
        get: function () { return this.dangerousUnNumber; },
        set: function (newValue) { if (this.dangerousUnNumber != newValue) {
            this.dangerousUnNumber = newValue;
            this.MarkAsDirty("DangerousUnNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DangerousPackagingGroup", {
        get: function () { return this.dangerousPackagingGroup; },
        set: function (newValue) { if (this.dangerousPackagingGroup != newValue) {
            this.dangerousPackagingGroup = newValue;
            this.MarkAsDirty("DangerousPackagingGroup");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DangerousIMDGCode", {
        get: function () { return this.dangerousIMDGCode; },
        set: function (newValue) { if (this.dangerousIMDGCode != newValue) {
            this.dangerousIMDGCode = newValue;
            this.MarkAsDirty("DangerousIMDGCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DangerousFlashPoint", {
        get: function () { return this.dangerousFlashPoint; },
        set: function (newValue) { if (this.dangerousFlashPoint != newValue) {
            this.dangerousFlashPoint = newValue;
            this.MarkAsDirty("DangerousFlashPoint");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DangerousMaterialDescription", {
        get: function () { return this.dangerousMaterialDescription; },
        set: function (newValue) { if (this.dangerousMaterialDescription != newValue) {
            this.dangerousMaterialDescription = newValue;
            this.MarkAsDirty("DangerousMaterialDescription");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LTCWEdited", {
        get: function () { return this.lTCWEdited; },
        set: function (newValue) { if (this.lTCWEdited != newValue) {
            this.lTCWEdited = newValue;
            this.MarkAsDirty("LTCWEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPickUpIndex", {
        get: function () { return this.shipmentPickUpIndex; },
        set: function (newValue) { if (this.shipmentPickUpIndex != newValue) {
            this.shipmentPickUpIndex = newValue;
            this.MarkAsDirty("ShipmentPickUpIndex");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentDeliveryIndex", {
        get: function () { return this.shipmentDeliveryIndex; },
        set: function (newValue) { if (this.shipmentDeliveryIndex != newValue) {
            this.shipmentDeliveryIndex = newValue;
            this.MarkAsDirty("ShipmentDeliveryIndex");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentContainerReturnIndex", {
        get: function () { return this.shipmentContainerReturnIndex; },
        set: function (newValue) { if (this.shipmentContainerReturnIndex != newValue) {
            this.shipmentContainerReturnIndex = newValue;
            this.MarkAsDirty("ShipmentContainerReturnIndex");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPayableStatusCode", {
        get: function () { return this.shipmentPayableStatusCode; },
        set: function (newValue) { if (this.shipmentPayableStatusCode != newValue) {
            this.shipmentPayableStatusCode = newValue;
            this.MarkAsDirty("ShipmentPayableStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentReceivableStatusCode", {
        get: function () { return this.shipmentReceivableStatusCode; },
        set: function (newValue) { if (this.shipmentReceivableStatusCode != newValue) {
            this.shipmentReceivableStatusCode = newValue;
            this.MarkAsDirty("ShipmentReceivableStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentReceivableStatusName", {
        get: function () { return this.shipmentReceivableStatusName; },
        set: function (newValue) { if (this.shipmentReceivableStatusName != newValue) {
            this.shipmentReceivableStatusName = newValue;
            this.MarkAsDirty("ShipmentReceivableStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPayableStatusName", {
        get: function () { return this.shipmentPayableStatusName; },
        set: function (newValue) { if (this.shipmentPayableStatusName != newValue) {
            this.shipmentPayableStatusName = newValue;
            this.MarkAsDirty("ShipmentPayableStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsCancelled", {
        get: function () { return this.isCancelled; },
        set: function (newValue) { if (this.isCancelled != newValue) {
            this.isCancelled = newValue;
            this.MarkAsDirty("IsCancelled");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CancelledDate", {
        get: function () { return this.cancelledDate; },
        set: function (newValue) { if (this.cancelledDate != newValue) {
            this.cancelledDate = newValue;
            this.MarkAsDirty("CancelledDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsAccountingClosed", {
        get: function () { return this.isAccountingClosed; },
        set: function (newValue) { if (this.isAccountingClosed != newValue) {
            this.isAccountingClosed = newValue;
            this.MarkAsDirty("IsAccountingClosed");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccessDate", {
        get: function () { return this.accessDate; },
        set: function (newValue) { if (this.accessDate != newValue) {
            this.accessDate = newValue;
            this.MarkAsDirty("AccessDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "UpdatedByUserId", {
        get: function () { return this.updatedByUserId; },
        set: function (newValue) { if (this.updatedByUserId != newValue) {
            this.updatedByUserId = newValue;
            this.MarkAsDirty("UpdatedByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastUpdateDate", {
        get: function () { return this.lastUpdateDate; },
        set: function (newValue) { if (this.lastUpdateDate != newValue) {
            this.lastUpdateDate = newValue;
            this.MarkAsDirty("LastUpdateDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProfitCurrencyId", {
        get: function () { return this.profitCurrencyId; },
        set: function (newValue) { if (this.profitCurrencyId != newValue) {
            this.profitCurrencyId = newValue;
            this.MarkAsDirty("ProfitCurrencyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProfitCurrencyCode", {
        get: function () { return this.profitCurrencyCode; },
        set: function (newValue) { if (this.profitCurrencyCode != newValue) {
            this.profitCurrencyCode = newValue;
            this.MarkAsDirty("ProfitCurrencyCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProfitExchangeRate", {
        get: function () { return this.profitExchangeRate; },
        set: function (newValue) { if (this.profitExchangeRate != newValue) {
            this.profitExchangeRate = newValue;
            this.MarkAsDirty("ProfitExchangeRate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "UpdatedByUserName", {
        get: function () { return this.updatedByUserName; },
        set: function (newValue) { if (this.updatedByUserName != newValue) {
            this.updatedByUserName = newValue;
            this.MarkAsDirty("UpdatedByUserName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "EventNote", {
        get: function () { return this.eventNote; },
        set: function (newValue) { if (this.eventNote != newValue) {
            this.eventNote = newValue;
            this.MarkAsDirty("EventNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NextLegCode", {
        get: function () { return this.nextLegCode; },
        set: function (newValue) { if (this.nextLegCode != newValue) {
            this.nextLegCode = newValue;
            this.MarkAsDirty("NextLegCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NextLegName", {
        get: function () { return this.nextLegName; },
        set: function (newValue) { if (this.nextLegName != newValue) {
            this.nextLegName = newValue;
            this.MarkAsDirty("NextLegName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NextETD", {
        get: function () { return this.nextETD; },
        set: function (newValue) { if (this.nextETD != newValue) {
            this.nextETD = newValue;
            this.MarkAsDirty("NextETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NextETA", {
        get: function () { return this.nextETA; },
        set: function (newValue) { if (this.nextETA != newValue) {
            this.nextETA = newValue;
            this.MarkAsDirty("NextETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentLevelCode", {
        get: function () { return this.shipmentLevelCode; },
        set: function (newValue) { if (this.shipmentLevelCode != newValue) {
            this.shipmentLevelCode = newValue;
            this.MarkAsDirty("ShipmentLevelCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentLevelName", {
        get: function () { return this.shipmentLevelName; },
        set: function (newValue) { if (this.shipmentLevelName != newValue) {
            this.shipmentLevelName = newValue;
            this.MarkAsDirty("ShipmentLevelName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MasterShipmentDataId", {
        get: function () { return this.masterShipmentDataId; },
        set: function (newValue) { if (this.masterShipmentDataId != newValue) {
            this.masterShipmentDataId = newValue;
            this.MarkAsDirty("MasterShipmentDataId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NumberOfFollowUps", {
        get: function () { return this.numberOfFollowUps; },
        set: function (newValue) { if (this.numberOfFollowUps != newValue) {
            this.numberOfFollowUps = newValue;
            this.MarkAsDirty("NumberOfFollowUps");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "QuoteNumber", {
        get: function () { return this.quoteNumber; },
        set: function (newValue) { if (this.quoteNumber != newValue) {
            this.quoteNumber = newValue;
            this.MarkAsDirty("QuoteNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentCustomerTypeCode", {
        get: function () { return this.shipmentCustomerTypeCode; },
        set: function (newValue) { if (this.shipmentCustomerTypeCode != newValue) {
            this.shipmentCustomerTypeCode = newValue;
            this.MarkAsDirty("ShipmentCustomerTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeAddressOneTime", {
        get: function () { return this.consigneeAddressOneTime; },
        set: function (newValue) { if (this.consigneeAddressOneTime != newValue) {
            this.consigneeAddressOneTime = newValue;
            this.MarkAsDirty("ConsigneeAddressOneTime");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperAddressOneTime", {
        get: function () { return this.shipperAddressOneTime; },
        set: function (newValue) { if (this.shipperAddressOneTime != newValue) {
            this.shipperAddressOneTime = newValue;
            this.MarkAsDirty("ShipperAddressOneTime");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (newValue) { if (this.customerId != newValue) {
            this.customerId = newValue;
            this.MarkAsDirty("CustomerId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerAddressId", {
        get: function () { return this.customerAddressId; },
        set: function (newValue) { if (this.customerAddressId != newValue) {
            this.customerAddressId = newValue;
            this.MarkAsDirty("CustomerAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerContactId", {
        get: function () { return this.customerContactId; },
        set: function (newValue) { if (this.customerContactId != newValue) {
            this.customerContactId = newValue;
            this.MarkAsDirty("CustomerContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerReference1", {
        get: function () { return this.customerReference1; },
        set: function (newValue) { if (this.customerReference1 != newValue) {
            this.customerReference1 = newValue;
            this.MarkAsDirty("CustomerReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerReference2", {
        get: function () { return this.customerReference2; },
        set: function (newValue) { if (this.customerReference2 != newValue) {
            this.customerReference2 = newValue;
            this.MarkAsDirty("CustomerReference2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerName", {
        get: function () { return this.customerName; },
        set: function (newValue) { if (this.customerName != newValue) {
            this.customerName = newValue;
            this.MarkAsDirty("CustomerName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerNote", {
        get: function () { return this.customerNote; },
        set: function (newValue) { if (this.customerNote != newValue) {
            this.customerNote = newValue;
            this.MarkAsDirty("CustomerNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreelancerId", {
        get: function () { return this.freelancerId; },
        set: function (newValue) { if (this.freelancerId != newValue) {
            this.freelancerId = newValue;
            this.MarkAsDirty("FreelancerId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreelancerAddressId", {
        get: function () { return this.freelancerAddressId; },
        set: function (newValue) { if (this.freelancerAddressId != newValue) {
            this.freelancerAddressId = newValue;
            this.MarkAsDirty("FreelancerAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreelancerContactId", {
        get: function () { return this.freelancerContactId; },
        set: function (newValue) { if (this.freelancerContactId != newValue) {
            this.freelancerContactId = newValue;
            this.MarkAsDirty("FreelancerContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreelancerName", {
        get: function () { return this.freelancerName; },
        set: function (newValue) { if (this.freelancerName != newValue) {
            this.freelancerName = newValue;
            this.MarkAsDirty("FreelancerName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierAgentId", {
        get: function () { return this.issuingCarrierAgentId; },
        set: function (newValue) { if (this.issuingCarrierAgentId != newValue) {
            this.issuingCarrierAgentId = newValue;
            this.MarkAsDirty("IssuingCarrierAgentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierAddressId", {
        get: function () { return this.issuingCarrierAddressId; },
        set: function (newValue) { if (this.issuingCarrierAddressId != newValue) {
            this.issuingCarrierAddressId = newValue;
            this.MarkAsDirty("IssuingCarrierAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierAgentName", {
        get: function () { return this.issuingCarrierAgentName; },
        set: function (newValue) { if (this.issuingCarrierAgentName != newValue) {
            this.issuingCarrierAgentName = newValue;
            this.MarkAsDirty("IssuingCarrierAgentName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierAgentNote", {
        get: function () { return this.issuingCarrierAgentNote; },
        set: function (newValue) { if (this.issuingCarrierAgentNote != newValue) {
            this.issuingCarrierAgentNote = newValue;
            this.MarkAsDirty("IssuingCarrierAgentNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierIATACode", {
        get: function () { return this.issuingCarrierIATACode; },
        set: function (newValue) { if (this.issuingCarrierIATACode != newValue) {
            this.issuingCarrierIATACode = newValue;
            this.MarkAsDirty("IssuingCarrierIATACode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightForwarderId", {
        get: function () { return this.freightForwarderId; },
        set: function (newValue) { if (this.freightForwarderId != newValue) {
            this.freightForwarderId = newValue;
            this.MarkAsDirty("FreightForwarderId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightForwarderAddressId", {
        get: function () { return this.freightForwarderAddressId; },
        set: function (newValue) { if (this.freightForwarderAddressId != newValue) {
            this.freightForwarderAddressId = newValue;
            this.MarkAsDirty("FreightForwarderAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightForwarderContactId", {
        get: function () { return this.freightForwarderContactId; },
        set: function (newValue) { if (this.freightForwarderContactId != newValue) {
            this.freightForwarderContactId = newValue;
            this.MarkAsDirty("FreightForwarderContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightForwarderReference", {
        get: function () { return this.freightForwarderReference; },
        set: function (newValue) { if (this.freightForwarderReference != newValue) {
            this.freightForwarderReference = newValue;
            this.MarkAsDirty("FreightForwarderReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightForwarderName", {
        get: function () { return this.freightForwarderName; },
        set: function (newValue) { if (this.freightForwarderName != newValue) {
            this.freightForwarderName = newValue;
            this.MarkAsDirty("FreightForwarderName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightForwarderNote", {
        get: function () { return this.freightForwarderNote; },
        set: function (newValue) { if (this.freightForwarderNote != newValue) {
            this.freightForwarderNote = newValue;
            this.MarkAsDirty("FreightForwarderNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperId", {
        get: function () { return this.shipperId; },
        set: function (newValue) { if (this.shipperId != newValue) {
            this.shipperId = newValue;
            this.MarkAsDirty("ShipperId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperAddressId", {
        get: function () { return this.shipperAddressId; },
        set: function (newValue) { if (this.shipperAddressId != newValue) {
            this.shipperAddressId = newValue;
            this.MarkAsDirty("ShipperAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperContactId", {
        get: function () { return this.shipperContactId; },
        set: function (newValue) { if (this.shipperContactId != newValue) {
            this.shipperContactId = newValue;
            this.MarkAsDirty("ShipperContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperReference1", {
        get: function () { return this.shipperReference1; },
        set: function (newValue) { if (this.shipperReference1 != newValue) {
            this.shipperReference1 = newValue;
            this.MarkAsDirty("ShipperReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperReference2", {
        get: function () { return this.shipperReference2; },
        set: function (newValue) { if (this.shipperReference2 != newValue) {
            this.shipperReference2 = newValue;
            this.MarkAsDirty("ShipperReference2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperName", {
        get: function () { return this.shipperName; },
        set: function (newValue) { if (this.shipperName != newValue) {
            this.shipperName = newValue;
            this.MarkAsDirty("ShipperName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNote", {
        get: function () { return this.shipperNote; },
        set: function (newValue) { if (this.shipperNote != newValue) {
            this.shipperNote = newValue;
            this.MarkAsDirty("ShipperNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperAddressText", {
        get: function () { return this.shipperAddressText; },
        set: function (newValue) { if (this.shipperAddressText != newValue) {
            this.shipperAddressText = newValue;
            this.MarkAsDirty("ShipperAddressText");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperAddressCountryCode", {
        get: function () { return this.shipperAddressCountryCode; },
        set: function (newValue) { if (this.shipperAddressCountryCode != newValue) {
            this.shipperAddressCountryCode = newValue;
            this.MarkAsDirty("ShipperAddressCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeId", {
        get: function () { return this.consigneeId; },
        set: function (newValue) { if (this.consigneeId != newValue) {
            this.consigneeId = newValue;
            this.MarkAsDirty("ConsigneeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeAddressId", {
        get: function () { return this.consigneeAddressId; },
        set: function (newValue) { if (this.consigneeAddressId != newValue) {
            this.consigneeAddressId = newValue;
            this.MarkAsDirty("ConsigneeAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeContactId", {
        get: function () { return this.consigneeContactId; },
        set: function (newValue) { if (this.consigneeContactId != newValue) {
            this.consigneeContactId = newValue;
            this.MarkAsDirty("ConsigneeContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeReference1", {
        get: function () { return this.consigneeReference1; },
        set: function (newValue) { if (this.consigneeReference1 != newValue) {
            this.consigneeReference1 = newValue;
            this.MarkAsDirty("ConsigneeReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeReference2", {
        get: function () { return this.consigneeReference2; },
        set: function (newValue) { if (this.consigneeReference2 != newValue) {
            this.consigneeReference2 = newValue;
            this.MarkAsDirty("ConsigneeReference2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeName", {
        get: function () { return this.consigneeName; },
        set: function (newValue) { if (this.consigneeName != newValue) {
            this.consigneeName = newValue;
            this.MarkAsDirty("ConsigneeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNote", {
        get: function () { return this.consigneeNote; },
        set: function (newValue) { if (this.consigneeNote != newValue) {
            this.consigneeNote = newValue;
            this.MarkAsDirty("ConsigneeNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeAddressText", {
        get: function () { return this.consigneeAddressText; },
        set: function (newValue) { if (this.consigneeAddressText != newValue) {
            this.consigneeAddressText = newValue;
            this.MarkAsDirty("ConsigneeAddressText");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeAddressCountryCode", {
        get: function () { return this.consigneeAddressCountryCode; },
        set: function (newValue) { if (this.consigneeAddressCountryCode != newValue) {
            this.consigneeAddressCountryCode = newValue;
            this.MarkAsDirty("ConsigneeAddressCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentId", {
        get: function () { return this.agentId; },
        set: function (newValue) { if (this.agentId != newValue) {
            this.agentId = newValue;
            this.MarkAsDirty("AgentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentAddressId", {
        get: function () { return this.agentAddressId; },
        set: function (newValue) { if (this.agentAddressId != newValue) {
            this.agentAddressId = newValue;
            this.MarkAsDirty("AgentAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentContactId", {
        get: function () { return this.agentContactId; },
        set: function (newValue) { if (this.agentContactId != newValue) {
            this.agentContactId = newValue;
            this.MarkAsDirty("AgentContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentReference1", {
        get: function () { return this.agentReference1; },
        set: function (newValue) { if (this.agentReference1 != newValue) {
            this.agentReference1 = newValue;
            this.MarkAsDirty("AgentReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentReference2", {
        get: function () { return this.agentReference2; },
        set: function (newValue) { if (this.agentReference2 != newValue) {
            this.agentReference2 = newValue;
            this.MarkAsDirty("AgentReference2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentName", {
        get: function () { return this.agentName; },
        set: function (newValue) { if (this.agentName != newValue) {
            this.agentName = newValue;
            this.MarkAsDirty("AgentName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentNote", {
        get: function () { return this.agentNote; },
        set: function (newValue) { if (this.agentNote != newValue) {
            this.agentNote = newValue;
            this.MarkAsDirty("AgentNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentAddressText", {
        get: function () { return this.agentAddressText; },
        set: function (newValue) { if (this.agentAddressText != newValue) {
            this.agentAddressText = newValue;
            this.MarkAsDirty("AgentAddressText");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentAddressCountryCode", {
        get: function () { return this.agentAddressCountryCode; },
        set: function (newValue) { if (this.agentAddressCountryCode != newValue) {
            this.agentAddressCountryCode = newValue;
            this.MarkAsDirty("AgentAddressCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentExportId", {
        get: function () { return this.customAgentExportId; },
        set: function (newValue) { if (this.customAgentExportId != newValue) {
            this.customAgentExportId = newValue;
            this.MarkAsDirty("CustomAgentExportId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentExportAddressId", {
        get: function () { return this.customAgentExportAddressId; },
        set: function (newValue) { if (this.customAgentExportAddressId != newValue) {
            this.customAgentExportAddressId = newValue;
            this.MarkAsDirty("CustomAgentExportAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentExportContactId", {
        get: function () { return this.customAgentExportContactId; },
        set: function (newValue) { if (this.customAgentExportContactId != newValue) {
            this.customAgentExportContactId = newValue;
            this.MarkAsDirty("CustomAgentExportContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentExportReference", {
        get: function () { return this.customAgentExportReference; },
        set: function (newValue) { if (this.customAgentExportReference != newValue) {
            this.customAgentExportReference = newValue;
            this.MarkAsDirty("CustomAgentExportReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentExportName", {
        get: function () { return this.customAgentExportName; },
        set: function (newValue) { if (this.customAgentExportName != newValue) {
            this.customAgentExportName = newValue;
            this.MarkAsDirty("CustomAgentExportName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentExportNote", {
        get: function () { return this.customAgentExportNote; },
        set: function (newValue) { if (this.customAgentExportNote != newValue) {
            this.customAgentExportNote = newValue;
            this.MarkAsDirty("CustomAgentExportNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentImportId", {
        get: function () { return this.customAgentImportId; },
        set: function (newValue) { if (this.customAgentImportId != newValue) {
            this.customAgentImportId = newValue;
            this.MarkAsDirty("CustomAgentImportId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentImportAddressId", {
        get: function () { return this.customAgentImportAddressId; },
        set: function (newValue) { if (this.customAgentImportAddressId != newValue) {
            this.customAgentImportAddressId = newValue;
            this.MarkAsDirty("CustomAgentImportAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentImportContactId", {
        get: function () { return this.customAgentImportContactId; },
        set: function (newValue) { if (this.customAgentImportContactId != newValue) {
            this.customAgentImportContactId = newValue;
            this.MarkAsDirty("CustomAgentImportContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentImportReference", {
        get: function () { return this.customAgentImportReference; },
        set: function (newValue) { if (this.customAgentImportReference != newValue) {
            this.customAgentImportReference = newValue;
            this.MarkAsDirty("CustomAgentImportReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentImportName", {
        get: function () { return this.customAgentImportName; },
        set: function (newValue) { if (this.customAgentImportName != newValue) {
            this.customAgentImportName = newValue;
            this.MarkAsDirty("CustomAgentImportName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomAgentImportNote", {
        get: function () { return this.customAgentImportNote; },
        set: function (newValue) { if (this.customAgentImportNote != newValue) {
            this.customAgentImportNote = newValue;
            this.MarkAsDirty("CustomAgentImportNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1Id", {
        get: function () { return this.notify1Id; },
        set: function (newValue) { if (this.notify1Id != newValue) {
            this.notify1Id = newValue;
            this.MarkAsDirty("Notify1Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1AddressId", {
        get: function () { return this.notify1AddressId; },
        set: function (newValue) { if (this.notify1AddressId != newValue) {
            this.notify1AddressId = newValue;
            this.MarkAsDirty("Notify1AddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1ContactId", {
        get: function () { return this.notify1ContactId; },
        set: function (newValue) { if (this.notify1ContactId != newValue) {
            this.notify1ContactId = newValue;
            this.MarkAsDirty("Notify1ContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1Name", {
        get: function () { return this.notify1Name; },
        set: function (newValue) { if (this.notify1Name != newValue) {
            this.notify1Name = newValue;
            this.MarkAsDirty("Notify1Name");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1Note", {
        get: function () { return this.notify1Note; },
        set: function (newValue) { if (this.notify1Note != newValue) {
            this.notify1Note = newValue;
            this.MarkAsDirty("Notify1Note");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify2Id", {
        get: function () { return this.notify2Id; },
        set: function (newValue) { if (this.notify2Id != newValue) {
            this.notify2Id = newValue;
            this.MarkAsDirty("Notify2Id");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify2AddressId", {
        get: function () { return this.notify2AddressId; },
        set: function (newValue) { if (this.notify2AddressId != newValue) {
            this.notify2AddressId = newValue;
            this.MarkAsDirty("Notify2AddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify2ContactId", {
        get: function () { return this.notify2ContactId; },
        set: function (newValue) { if (this.notify2ContactId != newValue) {
            this.notify2ContactId = newValue;
            this.MarkAsDirty("Notify2ContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify2Name", {
        get: function () { return this.notify2Name; },
        set: function (newValue) { if (this.notify2Name != newValue) {
            this.notify2Name = newValue;
            this.MarkAsDirty("Notify2Name");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify2Note", {
        get: function () { return this.notify2Note; },
        set: function (newValue) { if (this.notify2Note != newValue) {
            this.notify2Note = newValue;
            this.MarkAsDirty("Notify2Note");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNotExporterId", {
        get: function () { return this.shipperNotExporterId; },
        set: function (newValue) { if (this.shipperNotExporterId != newValue) {
            this.shipperNotExporterId = newValue;
            this.MarkAsDirty("ShipperNotExporterId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNotExporterAddressId", {
        get: function () { return this.shipperNotExporterAddressId; },
        set: function (newValue) { if (this.shipperNotExporterAddressId != newValue) {
            this.shipperNotExporterAddressId = newValue;
            this.MarkAsDirty("ShipperNotExporterAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNotExporterContactId", {
        get: function () { return this.shipperNotExporterContactId; },
        set: function (newValue) { if (this.shipperNotExporterContactId != newValue) {
            this.shipperNotExporterContactId = newValue;
            this.MarkAsDirty("ShipperNotExporterContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNotExporterName", {
        get: function () { return this.shipperNotExporterName; },
        set: function (newValue) { if (this.shipperNotExporterName != newValue) {
            this.shipperNotExporterName = newValue;
            this.MarkAsDirty("ShipperNotExporterName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNotExporterNote", {
        get: function () { return this.shipperNotExporterNote; },
        set: function (newValue) { if (this.shipperNotExporterNote != newValue) {
            this.shipperNotExporterNote = newValue;
            this.MarkAsDirty("ShipperNotExporterNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNotImporterId", {
        get: function () { return this.consigneeNotImporterId; },
        set: function (newValue) { if (this.consigneeNotImporterId != newValue) {
            this.consigneeNotImporterId = newValue;
            this.MarkAsDirty("ConsigneeNotImporterId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNotImporterAddressId", {
        get: function () { return this.consigneeNotImporterAddressId; },
        set: function (newValue) { if (this.consigneeNotImporterAddressId != newValue) {
            this.consigneeNotImporterAddressId = newValue;
            this.MarkAsDirty("ConsigneeNotImporterAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNotImporterContactId", {
        get: function () { return this.consigneeNotImporterContactId; },
        set: function (newValue) { if (this.consigneeNotImporterContactId != newValue) {
            this.consigneeNotImporterContactId = newValue;
            this.MarkAsDirty("ConsigneeNotImporterContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNotImporterName", {
        get: function () { return this.consigneeNotImporterName; },
        set: function (newValue) { if (this.consigneeNotImporterName != newValue) {
            this.consigneeNotImporterName = newValue;
            this.MarkAsDirty("ConsigneeNotImporterName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNotImporterNote", {
        get: function () { return this.consigneeNotImporterNote; },
        set: function (newValue) { if (this.consigneeNotImporterNote != newValue) {
            this.consigneeNotImporterNote = newValue;
            this.MarkAsDirty("ConsigneeNotImporterNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomClearancePointId", {
        get: function () { return this.customClearancePointId; },
        set: function (newValue) { if (this.customClearancePointId != newValue) {
            this.customClearancePointId = newValue;
            this.MarkAsDirty("CustomClearancePointId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomClearancePointAddressId", {
        get: function () { return this.customClearancePointAddressId; },
        set: function (newValue) { if (this.customClearancePointAddressId != newValue) {
            this.customClearancePointAddressId = newValue;
            this.MarkAsDirty("CustomClearancePointAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomClearancePointContactId", {
        get: function () { return this.customClearancePointContactId; },
        set: function (newValue) { if (this.customClearancePointContactId != newValue) {
            this.customClearancePointContactId = newValue;
            this.MarkAsDirty("CustomClearancePointContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomClearancePointReference1", {
        get: function () { return this.customClearancePointReference1; },
        set: function (newValue) { if (this.customClearancePointReference1 != newValue) {
            this.customClearancePointReference1 = newValue;
            this.MarkAsDirty("CustomClearancePointReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomClearancePointName", {
        get: function () { return this.customClearancePointName; },
        set: function (newValue) { if (this.customClearancePointName != newValue) {
            this.customClearancePointName = newValue;
            this.MarkAsDirty("CustomClearancePointName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomClearancePointNote", {
        get: function () { return this.customClearancePointNote; },
        set: function (newValue) { if (this.customClearancePointNote != newValue) {
            this.customClearancePointNote = newValue;
            this.MarkAsDirty("CustomClearancePointNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderId", {
        get: function () { return this.coloaderId; },
        set: function (newValue) { if (this.coloaderId != newValue) {
            this.coloaderId = newValue;
            this.MarkAsDirty("ColoaderId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderAddressId", {
        get: function () { return this.coloaderAddressId; },
        set: function (newValue) { if (this.coloaderAddressId != newValue) {
            this.coloaderAddressId = newValue;
            this.MarkAsDirty("ColoaderAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderContactId", {
        get: function () { return this.coloaderContactId; },
        set: function (newValue) { if (this.coloaderContactId != newValue) {
            this.coloaderContactId = newValue;
            this.MarkAsDirty("ColoaderContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderReference1", {
        get: function () { return this.coloaderReference1; },
        set: function (newValue) { if (this.coloaderReference1 != newValue) {
            this.coloaderReference1 = newValue;
            this.MarkAsDirty("ColoaderReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderName", {
        get: function () { return this.coloaderName; },
        set: function (newValue) { if (this.coloaderName != newValue) {
            this.coloaderName = newValue;
            this.MarkAsDirty("ColoaderName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderNote", {
        get: function () { return this.coloaderNote; },
        set: function (newValue) { if (this.coloaderNote != newValue) {
            this.coloaderNote = newValue;
            this.MarkAsDirty("ColoaderNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsolidatorId", {
        get: function () { return this.consolidatorId; },
        set: function (newValue) { if (this.consolidatorId != newValue) {
            this.consolidatorId = newValue;
            this.MarkAsDirty("ConsolidatorId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsolidatorAddressId", {
        get: function () { return this.consolidatorAddressId; },
        set: function (newValue) { if (this.consolidatorAddressId != newValue) {
            this.consolidatorAddressId = newValue;
            this.MarkAsDirty("ConsolidatorAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsolidatorContactId", {
        get: function () { return this.consolidatorContactId; },
        set: function (newValue) { if (this.consolidatorContactId != newValue) {
            this.consolidatorContactId = newValue;
            this.MarkAsDirty("ConsolidatorContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsolidatorReference", {
        get: function () { return this.consolidatorReference; },
        set: function (newValue) { if (this.consolidatorReference != newValue) {
            this.consolidatorReference = newValue;
            this.MarkAsDirty("ConsolidatorReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsolidatorName", {
        get: function () { return this.consolidatorName; },
        set: function (newValue) { if (this.consolidatorName != newValue) {
            this.consolidatorName = newValue;
            this.MarkAsDirty("ConsolidatorName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsolidatorNote", {
        get: function () { return this.consolidatorNote; },
        set: function (newValue) { if (this.consolidatorNote != newValue) {
            this.consolidatorNote = newValue;
            this.MarkAsDirty("ConsolidatorNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ForwarderShipmentNumber", {
        get: function () { return this.forwarderShipmentNumber; },
        set: function (newValue) { if (this.forwarderShipmentNumber != newValue) {
            this.forwarderShipmentNumber = newValue;
            this.MarkAsDirty("ForwarderShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ComputedForwarderShipmentNumber", {
        get: function () { return this.computedForwarderShipmentNumber; },
        set: function (newValue) { if (this.computedForwarderShipmentNumber != newValue) {
            this.computedForwarderShipmentNumber = newValue;
            this.MarkAsDirty("ComputedForwarderShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerShipmentNumber", {
        get: function () { return this.customerShipmentNumber; },
        set: function (newValue) { if (this.customerShipmentNumber != newValue) {
            this.customerShipmentNumber = newValue;
            this.MarkAsDirty("CustomerShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomsDeclarationNumber", {
        get: function () { return this.customsDeclarationNumber; },
        set: function (newValue) { if (this.customsDeclarationNumber != newValue) {
            this.customsDeclarationNumber = newValue;
            this.MarkAsDirty("CustomsDeclarationNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentId", {
        get: function () { return this.releasingAgentId; },
        set: function (newValue) { if (this.releasingAgentId != newValue) {
            this.releasingAgentId = newValue;
            this.MarkAsDirty("ReleasingAgentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentAddressId", {
        get: function () { return this.releasingAgentAddressId; },
        set: function (newValue) { if (this.releasingAgentAddressId != newValue) {
            this.releasingAgentAddressId = newValue;
            this.MarkAsDirty("ReleasingAgentAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentContactId", {
        get: function () { return this.releasingAgentContactId; },
        set: function (newValue) { if (this.releasingAgentContactId != newValue) {
            this.releasingAgentContactId = newValue;
            this.MarkAsDirty("ReleasingAgentContactId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentReference1", {
        get: function () { return this.releasingAgentReference1; },
        set: function (newValue) { if (this.releasingAgentReference1 != newValue) {
            this.releasingAgentReference1 = newValue;
            this.MarkAsDirty("ReleasingAgentReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentReference2", {
        get: function () { return this.releasingAgentReference2; },
        set: function (newValue) { if (this.releasingAgentReference2 != newValue) {
            this.releasingAgentReference2 = newValue;
            this.MarkAsDirty("ReleasingAgentReference2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentName", {
        get: function () { return this.releasingAgentName; },
        set: function (newValue) { if (this.releasingAgentName != newValue) {
            this.releasingAgentName = newValue;
            this.MarkAsDirty("ReleasingAgentName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReleasingAgentNote", {
        get: function () { return this.releasingAgentNote; },
        set: function (newValue) { if (this.releasingAgentNote != newValue) {
            this.releasingAgentNote = newValue;
            this.MarkAsDirty("ReleasingAgentNote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NoFreightFile", {
        get: function () { return this.noFreightFile; },
        set: function (newValue) { if (this.noFreightFile != newValue) {
            this.noFreightFile = newValue;
            this.MarkAsDirty("NoFreightFile");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsExceptionResolved", {
        get: function () { return this.isExceptionResolved; },
        set: function (newValue) { if (this.isExceptionResolved != newValue) {
            this.isExceptionResolved = newValue;
            this.MarkAsDirty("IsExceptionResolved");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AirlinePrefix", {
        get: function () { return this.airlinePrefix; },
        set: function (newValue) { if (this.airlinePrefix != newValue) {
            this.airlinePrefix = newValue;
            this.MarkAsDirty("AirlinePrefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageCarrierPrefix", {
        get: function () { return this.mainCarriageCarrierPrefix; },
        set: function (newValue) { if (this.mainCarriageCarrierPrefix != newValue) {
            this.mainCarriageCarrierPrefix = newValue;
            this.MarkAsDirty("MainCarriageCarrierPrefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1CarrierPrefix", {
        get: function () { return this.transshipment1CarrierPrefix; },
        set: function (newValue) { if (this.transshipment1CarrierPrefix != newValue) {
            this.transshipment1CarrierPrefix = newValue;
            this.MarkAsDirty("Transshipment1CarrierPrefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2CarrierPrefix", {
        get: function () { return this.transshipment2CarrierPrefix; },
        set: function (newValue) { if (this.transshipment2CarrierPrefix != newValue) {
            this.transshipment2CarrierPrefix = newValue;
            this.MarkAsDirty("Transshipment2CarrierPrefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3CarrierPrefix", {
        get: function () { return this.transshipment3CarrierPrefix; },
        set: function (newValue) { if (this.transshipment3CarrierPrefix != newValue) {
            this.transshipment3CarrierPrefix = newValue;
            this.MarkAsDirty("Transshipment3CarrierPrefix");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPortId", {
        get: function () { return this.fromPortId; },
        set: function (newValue) { if (this.fromPortId != newValue) {
            this.fromPortId = newValue;
            this.MarkAsDirty("FromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPortId", {
        get: function () { return this.toPortId; },
        set: function (newValue) { if (this.toPortId != newValue) {
            this.toPortId = newValue;
            this.MarkAsDirty("ToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageTransportModeId", {
        get: function () { return this.preCarriageTransportModeId; },
        set: function (newValue) { if (this.preCarriageTransportModeId != newValue) {
            this.preCarriageTransportModeId = newValue;
            this.MarkAsDirty("PreCarriageTransportModeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageFromPortId", {
        get: function () { return this.preCarriageFromPortId; },
        set: function (newValue) { if (this.preCarriageFromPortId != newValue) {
            this.preCarriageFromPortId = newValue;
            this.MarkAsDirty("PreCarriageFromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageToPortId", {
        get: function () { return this.preCarriageToPortId; },
        set: function (newValue) { if (this.preCarriageToPortId != newValue) {
            this.preCarriageToPortId = newValue;
            this.MarkAsDirty("PreCarriageToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageCarrierId", {
        get: function () { return this.preCarriageCarrierId; },
        set: function (newValue) { if (this.preCarriageCarrierId != newValue) {
            this.preCarriageCarrierId = newValue;
            this.MarkAsDirty("PreCarriageCarrierId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageCarrierNumber", {
        get: function () { return this.preCarriageCarrierNumber; },
        set: function (newValue) { if (this.preCarriageCarrierNumber != newValue) {
            this.preCarriageCarrierNumber = newValue;
            this.MarkAsDirty("PreCarriageCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageCarrierName", {
        get: function () { return this.preCarriageCarrierName; },
        set: function (newValue) { if (this.preCarriageCarrierName != newValue) {
            this.preCarriageCarrierName = newValue;
            this.MarkAsDirty("PreCarriageCarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageCarrierCode", {
        get: function () { return this.preCarriageCarrierCode; },
        set: function (newValue) { if (this.preCarriageCarrierCode != newValue) {
            this.preCarriageCarrierCode = newValue;
            this.MarkAsDirty("PreCarriageCarrierCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageFromPortCode", {
        get: function () { return this.preCarriageFromPortCode; },
        set: function (newValue) { if (this.preCarriageFromPortCode != newValue) {
            this.preCarriageFromPortCode = newValue;
            this.MarkAsDirty("PreCarriageFromPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageFromPortName", {
        get: function () { return this.preCarriageFromPortName; },
        set: function (newValue) { if (this.preCarriageFromPortName != newValue) {
            this.preCarriageFromPortName = newValue;
            this.MarkAsDirty("PreCarriageFromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageFromPortCountryCode", {
        get: function () { return this.preCarriageFromPortCountryCode; },
        set: function (newValue) { if (this.preCarriageFromPortCountryCode != newValue) {
            this.preCarriageFromPortCountryCode = newValue;
            this.MarkAsDirty("PreCarriageFromPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageFromPortCountryName", {
        get: function () { return this.preCarriageFromPortCountryName; },
        set: function (newValue) { if (this.preCarriageFromPortCountryName != newValue) {
            this.preCarriageFromPortCountryName = newValue;
            this.MarkAsDirty("PreCarriageFromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageToPortCode", {
        get: function () { return this.preCarriageToPortCode; },
        set: function (newValue) { if (this.preCarriageToPortCode != newValue) {
            this.preCarriageToPortCode = newValue;
            this.MarkAsDirty("PreCarriageToPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageToPortName", {
        get: function () { return this.preCarriageToPortName; },
        set: function (newValue) { if (this.preCarriageToPortName != newValue) {
            this.preCarriageToPortName = newValue;
            this.MarkAsDirty("PreCarriageToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageToPortCountryCode", {
        get: function () { return this.preCarriageToPortCountryCode; },
        set: function (newValue) { if (this.preCarriageToPortCountryCode != newValue) {
            this.preCarriageToPortCountryCode = newValue;
            this.MarkAsDirty("PreCarriageToPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageToPortCountryName", {
        get: function () { return this.preCarriageToPortCountryName; },
        set: function (newValue) { if (this.preCarriageToPortCountryName != newValue) {
            this.preCarriageToPortCountryName = newValue;
            this.MarkAsDirty("PreCarriageToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageETD", {
        get: function () { return this.preCarriageETD; },
        set: function (newValue) { if (this.preCarriageETD != newValue) {
            this.preCarriageETD = newValue;
            this.MarkAsDirty("PreCarriageETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageATD", {
        get: function () { return this.preCarriageATD; },
        set: function (newValue) { if (this.preCarriageATD != newValue) {
            this.preCarriageATD = newValue;
            this.MarkAsDirty("PreCarriageATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageETA", {
        get: function () { return this.preCarriageETA; },
        set: function (newValue) { if (this.preCarriageETA != newValue) {
            this.preCarriageETA = newValue;
            this.MarkAsDirty("PreCarriageETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageATA", {
        get: function () { return this.preCarriageATA; },
        set: function (newValue) { if (this.preCarriageATA != newValue) {
            this.preCarriageATA = newValue;
            this.MarkAsDirty("PreCarriageATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageCarrierWebSite", {
        get: function () { return this.preCarriageCarrierWebSite; },
        set: function (newValue) { if (this.preCarriageCarrierWebSite != newValue) {
            this.preCarriageCarrierWebSite = newValue;
            this.MarkAsDirty("PreCarriageCarrierWebSite");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageTransportModeId", {
        get: function () { return this.onCarriageTransportModeId; },
        set: function (newValue) { if (this.onCarriageTransportModeId != newValue) {
            this.onCarriageTransportModeId = newValue;
            this.MarkAsDirty("OnCarriageTransportModeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageFromPortId", {
        get: function () { return this.onCarriageFromPortId; },
        set: function (newValue) { if (this.onCarriageFromPortId != newValue) {
            this.onCarriageFromPortId = newValue;
            this.MarkAsDirty("OnCarriageFromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageToPortId", {
        get: function () { return this.onCarriageToPortId; },
        set: function (newValue) { if (this.onCarriageToPortId != newValue) {
            this.onCarriageToPortId = newValue;
            this.MarkAsDirty("OnCarriageToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageCarrierId", {
        get: function () { return this.onCarriageCarrierId; },
        set: function (newValue) { if (this.onCarriageCarrierId != newValue) {
            this.onCarriageCarrierId = newValue;
            this.MarkAsDirty("OnCarriageCarrierId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageCarrierNumber", {
        get: function () { return this.onCarriageCarrierNumber; },
        set: function (newValue) { if (this.onCarriageCarrierNumber != newValue) {
            this.onCarriageCarrierNumber = newValue;
            this.MarkAsDirty("OnCarriageCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageCarrierName", {
        get: function () { return this.onCarriageCarrierName; },
        set: function (newValue) { if (this.onCarriageCarrierName != newValue) {
            this.onCarriageCarrierName = newValue;
            this.MarkAsDirty("OnCarriageCarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageCarrierCode", {
        get: function () { return this.onCarriageCarrierCode; },
        set: function (newValue) { if (this.onCarriageCarrierCode != newValue) {
            this.onCarriageCarrierCode = newValue;
            this.MarkAsDirty("OnCarriageCarrierCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageFromPortCode", {
        get: function () { return this.onCarriageFromPortCode; },
        set: function (newValue) { if (this.onCarriageFromPortCode != newValue) {
            this.onCarriageFromPortCode = newValue;
            this.MarkAsDirty("OnCarriageFromPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageFromPortName", {
        get: function () { return this.onCarriageFromPortName; },
        set: function (newValue) { if (this.onCarriageFromPortName != newValue) {
            this.onCarriageFromPortName = newValue;
            this.MarkAsDirty("OnCarriageFromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageFromPortCountryCode", {
        get: function () { return this.onCarriageFromPortCountryCode; },
        set: function (newValue) { if (this.onCarriageFromPortCountryCode != newValue) {
            this.onCarriageFromPortCountryCode = newValue;
            this.MarkAsDirty("OnCarriageFromPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageFromPortCountryName", {
        get: function () { return this.onCarriageFromPortCountryName; },
        set: function (newValue) { if (this.onCarriageFromPortCountryName != newValue) {
            this.onCarriageFromPortCountryName = newValue;
            this.MarkAsDirty("OnCarriageFromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageToPortCode", {
        get: function () { return this.onCarriageToPortCode; },
        set: function (newValue) { if (this.onCarriageToPortCode != newValue) {
            this.onCarriageToPortCode = newValue;
            this.MarkAsDirty("OnCarriageToPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageToPortName", {
        get: function () { return this.onCarriageToPortName; },
        set: function (newValue) { if (this.onCarriageToPortName != newValue) {
            this.onCarriageToPortName = newValue;
            this.MarkAsDirty("OnCarriageToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageToPortCountryCode", {
        get: function () { return this.onCarriageToPortCountryCode; },
        set: function (newValue) { if (this.onCarriageToPortCountryCode != newValue) {
            this.onCarriageToPortCountryCode = newValue;
            this.MarkAsDirty("OnCarriageToPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageToPortCountryName", {
        get: function () { return this.onCarriageToPortCountryName; },
        set: function (newValue) { if (this.onCarriageToPortCountryName != newValue) {
            this.onCarriageToPortCountryName = newValue;
            this.MarkAsDirty("OnCarriageToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageETD", {
        get: function () { return this.onCarriageETD; },
        set: function (newValue) { if (this.onCarriageETD != newValue) {
            this.onCarriageETD = newValue;
            this.MarkAsDirty("OnCarriageETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageATD", {
        get: function () { return this.onCarriageATD; },
        set: function (newValue) { if (this.onCarriageATD != newValue) {
            this.onCarriageATD = newValue;
            this.MarkAsDirty("OnCarriageATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageETA", {
        get: function () { return this.onCarriageETA; },
        set: function (newValue) { if (this.onCarriageETA != newValue) {
            this.onCarriageETA = newValue;
            this.MarkAsDirty("OnCarriageETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageATA", {
        get: function () { return this.onCarriageATA; },
        set: function (newValue) { if (this.onCarriageATA != newValue) {
            this.onCarriageATA = newValue;
            this.MarkAsDirty("OnCarriageATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageCarrierWebSite", {
        get: function () { return this.onCarriageCarrierWebSite; },
        set: function (newValue) { if (this.onCarriageCarrierWebSite != newValue) {
            this.onCarriageCarrierWebSite = newValue;
            this.MarkAsDirty("OnCarriageCarrierWebSite");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageTransportModeId", {
        get: function () { return this.mainCarriageTransportModeId; },
        set: function (newValue) { if (this.mainCarriageTransportModeId != newValue) {
            this.mainCarriageTransportModeId = newValue;
            this.MarkAsDirty("MainCarriageTransportModeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromPortId", {
        get: function () { return this.mainCarriageFromPortId; },
        set: function (newValue) { if (this.mainCarriageFromPortId != newValue) {
            this.mainCarriageFromPortId = newValue;
            this.MarkAsDirty("MainCarriageFromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToPortId", {
        get: function () { return this.mainCarriageToPortId; },
        set: function (newValue) { if (this.mainCarriageToPortId != newValue) {
            this.mainCarriageToPortId = newValue;
            this.MarkAsDirty("MainCarriageToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromPortCode", {
        get: function () { return this.mainCarriageFromPortCode; },
        set: function (newValue) { if (this.mainCarriageFromPortCode != newValue) {
            this.mainCarriageFromPortCode = newValue;
            this.MarkAsDirty("MainCarriageFromPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromPortName", {
        get: function () { return this.mainCarriageFromPortName; },
        set: function (newValue) { if (this.mainCarriageFromPortName != newValue) {
            this.mainCarriageFromPortName = newValue;
            this.MarkAsDirty("MainCarriageFromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromPortCountryName", {
        get: function () { return this.mainCarriageFromPortCountryName; },
        set: function (newValue) { if (this.mainCarriageFromPortCountryName != newValue) {
            this.mainCarriageFromPortCountryName = newValue;
            this.MarkAsDirty("MainCarriageFromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromPortCountryCode", {
        get: function () { return this.mainCarriageFromPortCountryCode; },
        set: function (newValue) { if (this.mainCarriageFromPortCountryCode != newValue) {
            this.mainCarriageFromPortCountryCode = newValue;
            this.MarkAsDirty("MainCarriageFromPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToPortCode", {
        get: function () { return this.mainCarriageToPortCode; },
        set: function (newValue) { if (this.mainCarriageToPortCode != newValue) {
            this.mainCarriageToPortCode = newValue;
            this.MarkAsDirty("MainCarriageToPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToPortName", {
        get: function () { return this.mainCarriageToPortName; },
        set: function (newValue) { if (this.mainCarriageToPortName != newValue) {
            this.mainCarriageToPortName = newValue;
            this.MarkAsDirty("MainCarriageToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToPortCountryCode", {
        get: function () { return this.mainCarriageToPortCountryCode; },
        set: function (newValue) { if (this.mainCarriageToPortCountryCode != newValue) {
            this.mainCarriageToPortCountryCode = newValue;
            this.MarkAsDirty("MainCarriageToPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToPortCountryName", {
        get: function () { return this.mainCarriageToPortCountryName; },
        set: function (newValue) { if (this.mainCarriageToPortCountryName != newValue) {
            this.mainCarriageToPortCountryName = newValue;
            this.MarkAsDirty("MainCarriageToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageVesselId", {
        get: function () { return this.mainCarriageVesselId; },
        set: function (newValue) { if (this.mainCarriageVesselId != newValue) {
            this.mainCarriageVesselId = newValue;
            this.MarkAsDirty("MainCarriageVesselId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageVesselId", {
        get: function () { return this.preCarriageVesselId; },
        set: function (newValue) { if (this.preCarriageVesselId != newValue) {
            this.preCarriageVesselId = newValue;
            this.MarkAsDirty("PreCarriageVesselId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageVesselId", {
        get: function () { return this.onCarriageVesselId; },
        set: function (newValue) { if (this.onCarriageVesselId != newValue) {
            this.onCarriageVesselId = newValue;
            this.MarkAsDirty("OnCarriageVesselId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1VesselId", {
        get: function () { return this.transshipment1VesselId; },
        set: function (newValue) { if (this.transshipment1VesselId != newValue) {
            this.transshipment1VesselId = newValue;
            this.MarkAsDirty("Transshipment1VesselId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2VesselId", {
        get: function () { return this.transshipment2VesselId; },
        set: function (newValue) { if (this.transshipment2VesselId != newValue) {
            this.transshipment2VesselId = newValue;
            this.MarkAsDirty("Transshipment2VesselId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3VesselId", {
        get: function () { return this.transshipment3VesselId; },
        set: function (newValue) { if (this.transshipment3VesselId != newValue) {
            this.transshipment3VesselId = newValue;
            this.MarkAsDirty("Transshipment3VesselId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageVesselName", {
        get: function () { return this.mainCarriageVesselName; },
        set: function (newValue) { if (this.mainCarriageVesselName != newValue) {
            this.mainCarriageVesselName = newValue;
            this.MarkAsDirty("MainCarriageVesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PreCarriageVesselName", {
        get: function () { return this.preCarriageVesselName; },
        set: function (newValue) { if (this.preCarriageVesselName != newValue) {
            this.preCarriageVesselName = newValue;
            this.MarkAsDirty("PreCarriageVesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageVesselName", {
        get: function () { return this.onCarriageVesselName; },
        set: function (newValue) { if (this.onCarriageVesselName != newValue) {
            this.onCarriageVesselName = newValue;
            this.MarkAsDirty("OnCarriageVesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1VesselName", {
        get: function () { return this.transshipment1VesselName; },
        set: function (newValue) { if (this.transshipment1VesselName != newValue) {
            this.transshipment1VesselName = newValue;
            this.MarkAsDirty("Transshipment1VesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2VesselName", {
        get: function () { return this.transshipment2VesselName; },
        set: function (newValue) { if (this.transshipment2VesselName != newValue) {
            this.transshipment2VesselName = newValue;
            this.MarkAsDirty("Transshipment2VesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3VesselName", {
        get: function () { return this.transshipment3VesselName; },
        set: function (newValue) { if (this.transshipment3VesselName != newValue) {
            this.transshipment3VesselName = newValue;
            this.MarkAsDirty("Transshipment3VesselName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageIsFromStack", {
        get: function () { return this.mainCarriageIsFromStack; },
        set: function (newValue) { if (this.mainCarriageIsFromStack != newValue) {
            this.mainCarriageIsFromStack = newValue;
            this.MarkAsDirty("MainCarriageIsFromStack");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageATD", {
        get: function () { return this.mainCarriageATD; },
        set: function (newValue) { if (this.mainCarriageATD != newValue) {
            this.mainCarriageATD = newValue;
            this.MarkAsDirty("MainCarriageATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageATA", {
        get: function () { return this.mainCarriageATA; },
        set: function (newValue) { if (this.mainCarriageATA != newValue) {
            this.mainCarriageATA = newValue;
            this.MarkAsDirty("MainCarriageATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageETD", {
        get: function () { return this.mainCarriageETD; },
        set: function (newValue) { if (this.mainCarriageETD != newValue) {
            this.mainCarriageETD = newValue;
            this.MarkAsDirty("MainCarriageETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageETA", {
        get: function () { return this.mainCarriageETA; },
        set: function (newValue) { if (this.mainCarriageETA != newValue) {
            this.mainCarriageETA = newValue;
            this.MarkAsDirty("MainCarriageETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1FromPortId", {
        get: function () { return this.transshipment1FromPortId; },
        set: function (newValue) { if (this.transshipment1FromPortId != newValue) {
            this.transshipment1FromPortId = newValue;
            this.MarkAsDirty("Transshipment1FromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ToPortId", {
        get: function () { return this.transshipment1ToPortId; },
        set: function (newValue) { if (this.transshipment1ToPortId != newValue) {
            this.transshipment1ToPortId = newValue;
            this.MarkAsDirty("Transshipment1ToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ATD", {
        get: function () { return this.transshipment1ATD; },
        set: function (newValue) { if (this.transshipment1ATD != newValue) {
            this.transshipment1ATD = newValue;
            this.MarkAsDirty("Transshipment1ATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ATA", {
        get: function () { return this.transshipment1ATA; },
        set: function (newValue) { if (this.transshipment1ATA != newValue) {
            this.transshipment1ATA = newValue;
            this.MarkAsDirty("Transshipment1ATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ETD", {
        get: function () { return this.transshipment1ETD; },
        set: function (newValue) { if (this.transshipment1ETD != newValue) {
            this.transshipment1ETD = newValue;
            this.MarkAsDirty("Transshipment1ETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ETA", {
        get: function () { return this.transshipment1ETA; },
        set: function (newValue) { if (this.transshipment1ETA != newValue) {
            this.transshipment1ETA = newValue;
            this.MarkAsDirty("Transshipment1ETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1CarrierNumber", {
        get: function () { return this.transshipment1CarrierNumber; },
        set: function (newValue) { if (this.transshipment1CarrierNumber != newValue) {
            this.transshipment1CarrierNumber = newValue;
            this.MarkAsDirty("Transshipment1CarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1CarrierId", {
        get: function () { return this.transshipment1CarrierId; },
        set: function (newValue) { if (this.transshipment1CarrierId != newValue) {
            this.transshipment1CarrierId = newValue;
            this.MarkAsDirty("Transshipment1CarrierId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1CarrierName", {
        get: function () { return this.transshipment1CarrierName; },
        set: function (newValue) { if (this.transshipment1CarrierName != newValue) {
            this.transshipment1CarrierName = newValue;
            this.MarkAsDirty("Transshipment1CarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1CarrierCode", {
        get: function () { return this.transshipment1CarrierCode; },
        set: function (newValue) { if (this.transshipment1CarrierCode != newValue) {
            this.transshipment1CarrierCode = newValue;
            this.MarkAsDirty("Transshipment1CarrierCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1FromPortCode", {
        get: function () { return this.transshipment1FromPortCode; },
        set: function (newValue) { if (this.transshipment1FromPortCode != newValue) {
            this.transshipment1FromPortCode = newValue;
            this.MarkAsDirty("Transshipment1FromPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1FromPortName", {
        get: function () { return this.transshipment1FromPortName; },
        set: function (newValue) { if (this.transshipment1FromPortName != newValue) {
            this.transshipment1FromPortName = newValue;
            this.MarkAsDirty("Transshipment1FromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1FromPortCountryCode", {
        get: function () { return this.transshipment1FromPortCountryCode; },
        set: function (newValue) { if (this.transshipment1FromPortCountryCode != newValue) {
            this.transshipment1FromPortCountryCode = newValue;
            this.MarkAsDirty("Transshipment1FromPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1FromPortCountryName", {
        get: function () { return this.transshipment1FromPortCountryName; },
        set: function (newValue) { if (this.transshipment1FromPortCountryName != newValue) {
            this.transshipment1FromPortCountryName = newValue;
            this.MarkAsDirty("Transshipment1FromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ToPortCode", {
        get: function () { return this.transshipment1ToPortCode; },
        set: function (newValue) { if (this.transshipment1ToPortCode != newValue) {
            this.transshipment1ToPortCode = newValue;
            this.MarkAsDirty("Transshipment1ToPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ToPortName", {
        get: function () { return this.transshipment1ToPortName; },
        set: function (newValue) { if (this.transshipment1ToPortName != newValue) {
            this.transshipment1ToPortName = newValue;
            this.MarkAsDirty("Transshipment1ToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ToPortCountryCode", {
        get: function () { return this.transshipment1ToPortCountryCode; },
        set: function (newValue) { if (this.transshipment1ToPortCountryCode != newValue) {
            this.transshipment1ToPortCountryCode = newValue;
            this.MarkAsDirty("Transshipment1ToPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1ToPortCountryName", {
        get: function () { return this.transshipment1ToPortCountryName; },
        set: function (newValue) { if (this.transshipment1ToPortCountryName != newValue) {
            this.transshipment1ToPortCountryName = newValue;
            this.MarkAsDirty("Transshipment1ToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1CarrierWebSite", {
        get: function () { return this.transshipment1CarrierWebSite; },
        set: function (newValue) { if (this.transshipment1CarrierWebSite != newValue) {
            this.transshipment1CarrierWebSite = newValue;
            this.MarkAsDirty("Transshipment1CarrierWebSite");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2CarrierWebSite", {
        get: function () { return this.transshipment2CarrierWebSite; },
        set: function (newValue) { if (this.transshipment2CarrierWebSite != newValue) {
            this.transshipment2CarrierWebSite = newValue;
            this.MarkAsDirty("Transshipment2CarrierWebSite");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3CarrierWebSite", {
        get: function () { return this.transshipment3CarrierWebSite; },
        set: function (newValue) { if (this.transshipment3CarrierWebSite != newValue) {
            this.transshipment3CarrierWebSite = newValue;
            this.MarkAsDirty("Transshipment3CarrierWebSite");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2FromPortId", {
        get: function () { return this.transshipment2FromPortId; },
        set: function (newValue) { if (this.transshipment2FromPortId != newValue) {
            this.transshipment2FromPortId = newValue;
            this.MarkAsDirty("Transshipment2FromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ToPortId", {
        get: function () { return this.transshipment2ToPortId; },
        set: function (newValue) { if (this.transshipment2ToPortId != newValue) {
            this.transshipment2ToPortId = newValue;
            this.MarkAsDirty("Transshipment2ToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ATD", {
        get: function () { return this.transshipment2ATD; },
        set: function (newValue) { if (this.transshipment2ATD != newValue) {
            this.transshipment2ATD = newValue;
            this.MarkAsDirty("Transshipment2ATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ATA", {
        get: function () { return this.transshipment2ATA; },
        set: function (newValue) { if (this.transshipment2ATA != newValue) {
            this.transshipment2ATA = newValue;
            this.MarkAsDirty("Transshipment2ATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ETD", {
        get: function () { return this.transshipment2ETD; },
        set: function (newValue) { if (this.transshipment2ETD != newValue) {
            this.transshipment2ETD = newValue;
            this.MarkAsDirty("Transshipment2ETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ETA", {
        get: function () { return this.transshipment2ETA; },
        set: function (newValue) { if (this.transshipment2ETA != newValue) {
            this.transshipment2ETA = newValue;
            this.MarkAsDirty("Transshipment2ETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2CarrierNumber", {
        get: function () { return this.transshipment2CarrierNumber; },
        set: function (newValue) { if (this.transshipment2CarrierNumber != newValue) {
            this.transshipment2CarrierNumber = newValue;
            this.MarkAsDirty("Transshipment2CarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2CarrierId", {
        get: function () { return this.transshipment2CarrierId; },
        set: function (newValue) { if (this.transshipment2CarrierId != newValue) {
            this.transshipment2CarrierId = newValue;
            this.MarkAsDirty("Transshipment2CarrierId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2CarrierName", {
        get: function () { return this.transshipment2CarrierName; },
        set: function (newValue) { if (this.transshipment2CarrierName != newValue) {
            this.transshipment2CarrierName = newValue;
            this.MarkAsDirty("Transshipment2CarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2CarrierCode", {
        get: function () { return this.transshipment2CarrierCode; },
        set: function (newValue) { if (this.transshipment2CarrierCode != newValue) {
            this.transshipment2CarrierCode = newValue;
            this.MarkAsDirty("Transshipment2CarrierCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2FromPortCode", {
        get: function () { return this.transshipment2FromPortCode; },
        set: function (newValue) { if (this.transshipment2FromPortCode != newValue) {
            this.transshipment2FromPortCode = newValue;
            this.MarkAsDirty("Transshipment2FromPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2FromPortName", {
        get: function () { return this.transshipment2FromPortName; },
        set: function (newValue) { if (this.transshipment2FromPortName != newValue) {
            this.transshipment2FromPortName = newValue;
            this.MarkAsDirty("Transshipment2FromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2FromPortCountryCode", {
        get: function () { return this.transshipment2FromPortCountryCode; },
        set: function (newValue) { if (this.transshipment2FromPortCountryCode != newValue) {
            this.transshipment2FromPortCountryCode = newValue;
            this.MarkAsDirty("Transshipment2FromPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2FromPortCountryName", {
        get: function () { return this.transshipment2FromPortCountryName; },
        set: function (newValue) { if (this.transshipment2FromPortCountryName != newValue) {
            this.transshipment2FromPortCountryName = newValue;
            this.MarkAsDirty("Transshipment2FromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ToPortCode", {
        get: function () { return this.transshipment2ToPortCode; },
        set: function (newValue) { if (this.transshipment2ToPortCode != newValue) {
            this.transshipment2ToPortCode = newValue;
            this.MarkAsDirty("Transshipment2ToPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ToPortName", {
        get: function () { return this.transshipment2ToPortName; },
        set: function (newValue) { if (this.transshipment2ToPortName != newValue) {
            this.transshipment2ToPortName = newValue;
            this.MarkAsDirty("Transshipment2ToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ToPortCountryCode", {
        get: function () { return this.transshipment2ToPortCountryCode; },
        set: function (newValue) { if (this.transshipment2ToPortCountryCode != newValue) {
            this.transshipment2ToPortCountryCode = newValue;
            this.MarkAsDirty("Transshipment2ToPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2ToPortCountryName", {
        get: function () { return this.transshipment2ToPortCountryName; },
        set: function (newValue) { if (this.transshipment2ToPortCountryName != newValue) {
            this.transshipment2ToPortCountryName = newValue;
            this.MarkAsDirty("Transshipment2ToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3FromPortId", {
        get: function () { return this.transshipment3FromPortId; },
        set: function (newValue) { if (this.transshipment3FromPortId != newValue) {
            this.transshipment3FromPortId = newValue;
            this.MarkAsDirty("Transshipment3FromPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ToPortId", {
        get: function () { return this.transshipment3ToPortId; },
        set: function (newValue) { if (this.transshipment3ToPortId != newValue) {
            this.transshipment3ToPortId = newValue;
            this.MarkAsDirty("Transshipment3ToPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ATD", {
        get: function () { return this.transshipment3ATD; },
        set: function (newValue) { if (this.transshipment3ATD != newValue) {
            this.transshipment3ATD = newValue;
            this.MarkAsDirty("Transshipment3ATD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ATA", {
        get: function () { return this.transshipment3ATA; },
        set: function (newValue) { if (this.transshipment3ATA != newValue) {
            this.transshipment3ATA = newValue;
            this.MarkAsDirty("Transshipment3ATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ETD", {
        get: function () { return this.transshipment3ETD; },
        set: function (newValue) { if (this.transshipment3ETD != newValue) {
            this.transshipment3ETD = newValue;
            this.MarkAsDirty("Transshipment3ETD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ETA", {
        get: function () { return this.transshipment3ETA; },
        set: function (newValue) { if (this.transshipment3ETA != newValue) {
            this.transshipment3ETA = newValue;
            this.MarkAsDirty("Transshipment3ETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3CarrierNumber", {
        get: function () { return this.transshipment3CarrierNumber; },
        set: function (newValue) { if (this.transshipment3CarrierNumber != newValue) {
            this.transshipment3CarrierNumber = newValue;
            this.MarkAsDirty("Transshipment3CarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3CarrierId", {
        get: function () { return this.transshipment3CarrierId; },
        set: function (newValue) { if (this.transshipment3CarrierId != newValue) {
            this.transshipment3CarrierId = newValue;
            this.MarkAsDirty("Transshipment3CarrierId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3CarrierName", {
        get: function () { return this.transshipment3CarrierName; },
        set: function (newValue) { if (this.transshipment3CarrierName != newValue) {
            this.transshipment3CarrierName = newValue;
            this.MarkAsDirty("Transshipment3CarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3CarrierCode", {
        get: function () { return this.transshipment3CarrierCode; },
        set: function (newValue) { if (this.transshipment3CarrierCode != newValue) {
            this.transshipment3CarrierCode = newValue;
            this.MarkAsDirty("Transshipment3CarrierCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3FromPortCode", {
        get: function () { return this.transshipment3FromPortCode; },
        set: function (newValue) { if (this.transshipment3FromPortCode != newValue) {
            this.transshipment3FromPortCode = newValue;
            this.MarkAsDirty("Transshipment3FromPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3FromPortName", {
        get: function () { return this.transshipment3FromPortName; },
        set: function (newValue) { if (this.transshipment3FromPortName != newValue) {
            this.transshipment3FromPortName = newValue;
            this.MarkAsDirty("Transshipment3FromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3FromPortCountryCode", {
        get: function () { return this.transshipment3FromPortCountryCode; },
        set: function (newValue) { if (this.transshipment3FromPortCountryCode != newValue) {
            this.transshipment3FromPortCountryCode = newValue;
            this.MarkAsDirty("Transshipment3FromPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3FromPortCountryName", {
        get: function () { return this.transshipment3FromPortCountryName; },
        set: function (newValue) { if (this.transshipment3FromPortCountryName != newValue) {
            this.transshipment3FromPortCountryName = newValue;
            this.MarkAsDirty("Transshipment3FromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ToPortCode", {
        get: function () { return this.transshipment3ToPortCode; },
        set: function (newValue) { if (this.transshipment3ToPortCode != newValue) {
            this.transshipment3ToPortCode = newValue;
            this.MarkAsDirty("Transshipment3ToPortCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ToPortName", {
        get: function () { return this.transshipment3ToPortName; },
        set: function (newValue) { if (this.transshipment3ToPortName != newValue) {
            this.transshipment3ToPortName = newValue;
            this.MarkAsDirty("Transshipment3ToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ToPortCountryCode", {
        get: function () { return this.transshipment3ToPortCountryCode; },
        set: function (newValue) { if (this.transshipment3ToPortCountryCode != newValue) {
            this.transshipment3ToPortCountryCode = newValue;
            this.MarkAsDirty("Transshipment3ToPortCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3ToPortCountryName", {
        get: function () { return this.transshipment3ToPortCountryName; },
        set: function (newValue) { if (this.transshipment3ToPortCountryName != newValue) {
            this.transshipment3ToPortCountryName = newValue;
            this.MarkAsDirty("Transshipment3ToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromCountryIsEC", {
        get: function () { return this.fromCountryIsEC; },
        set: function (newValue) { if (this.fromCountryIsEC != newValue) {
            this.fromCountryIsEC = newValue;
            this.MarkAsDirty("FromCountryIsEC");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToCountryIsEC", {
        get: function () { return this.toCountryIsEC; },
        set: function (newValue) { if (this.toCountryIsEC != newValue) {
            this.toCountryIsEC = newValue;
            this.MarkAsDirty("ToCountryIsEC");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FinalDistenationPortId", {
        get: function () { return this.finalDistenationPortId; },
        set: function (newValue) { if (this.finalDistenationPortId != newValue) {
            this.finalDistenationPortId = newValue;
            this.MarkAsDirty("FinalDistenationPortId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1AdditionalMAWBOBLBL", {
        get: function () { return this.transshipment1AdditionalMAWBOBLBL; },
        set: function (newValue) { if (this.transshipment1AdditionalMAWBOBLBL != newValue) {
            this.transshipment1AdditionalMAWBOBLBL = newValue;
            this.MarkAsDirty("Transshipment1AdditionalMAWBOBLBL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2AdditionalMAWBOBLBL", {
        get: function () { return this.transshipment2AdditionalMAWBOBLBL; },
        set: function (newValue) { if (this.transshipment2AdditionalMAWBOBLBL != newValue) {
            this.transshipment2AdditionalMAWBOBLBL = newValue;
            this.MarkAsDirty("Transshipment2AdditionalMAWBOBLBL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3AdditionalMAWBOBLBL", {
        get: function () { return this.transshipment3AdditionalMAWBOBLBL; },
        set: function (newValue) { if (this.transshipment3AdditionalMAWBOBLBL != newValue) {
            this.transshipment3AdditionalMAWBOBLBL = newValue;
            this.MarkAsDirty("Transshipment3AdditionalMAWBOBLBL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPort", {
        get: function () { return this.fromPort; },
        set: function (newValue) { if (this.fromPort != newValue) {
            this.fromPort = newValue;
            this.MarkAsDirty("FromPort");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPortName", {
        get: function () { return this.fromPortName; },
        set: function (newValue) { if (this.fromPortName != newValue) {
            this.fromPortName = newValue;
            this.MarkAsDirty("FromPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPortCountry", {
        get: function () { return this.fromPortCountry; },
        set: function (newValue) { if (this.fromPortCountry != newValue) {
            this.fromPortCountry = newValue;
            this.MarkAsDirty("FromPortCountry");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPortCountryName", {
        get: function () { return this.fromPortCountryName; },
        set: function (newValue) { if (this.fromPortCountryName != newValue) {
            this.fromPortCountryName = newValue;
            this.MarkAsDirty("FromPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPort", {
        get: function () { return this.toPort; },
        set: function (newValue) { if (this.toPort != newValue) {
            this.toPort = newValue;
            this.MarkAsDirty("ToPort");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPortName", {
        get: function () { return this.toPortName; },
        set: function (newValue) { if (this.toPortName != newValue) {
            this.toPortName = newValue;
            this.MarkAsDirty("ToPortName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPortCountry", {
        get: function () { return this.toPortCountry; },
        set: function (newValue) { if (this.toPortCountry != newValue) {
            this.toPortCountry = newValue;
            this.MarkAsDirty("ToPortCountry");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPortCountryName", {
        get: function () { return this.toPortCountryName; },
        set: function (newValue) { if (this.toPortCountryName != newValue) {
            this.toPortCountryName = newValue;
            this.MarkAsDirty("ToPortCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OrderGrossWeight", {
        get: function () { return this.orderGrossWeight; },
        set: function (newValue) { if (this.orderGrossWeight != newValue) {
            this.orderGrossWeight = newValue;
            this.MarkAsDirty("OrderGrossWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingVolume", {
        get: function () { return this.bookingVolume; },
        set: function (newValue) { if (this.bookingVolume != newValue) {
            this.bookingVolume = newValue;
            this.MarkAsDirty("BookingVolume");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingNumberOfPackages", {
        get: function () { return this.bookingNumberOfPackages; },
        set: function (newValue) { if (this.bookingNumberOfPackages != newValue) {
            this.bookingNumberOfPackages = newValue;
            this.MarkAsDirty("BookingNumberOfPackages");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OrderIsDangerouseGoods", {
        get: function () { return this.orderIsDangerouseGoods; },
        set: function (newValue) { if (this.orderIsDangerouseGoods != newValue) {
            this.orderIsDangerouseGoods = newValue;
            this.MarkAsDirty("OrderIsDangerouseGoods");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingConfirmationNumber", {
        get: function () { return this.bookingConfirmationNumber; },
        set: function (newValue) { if (this.bookingConfirmationNumber != newValue) {
            this.bookingConfirmationNumber = newValue;
            this.MarkAsDirty("BookingConfirmationNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingConfirmedBy", {
        get: function () { return this.bookingConfirmedBy; },
        set: function (newValue) { if (this.bookingConfirmedBy != newValue) {
            this.bookingConfirmedBy = newValue;
            this.MarkAsDirty("BookingConfirmedBy");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BookingConfirmationNotes", {
        get: function () { return this.bookingConfirmationNotes; },
        set: function (newValue) { if (this.bookingConfirmationNotes != newValue) {
            this.bookingConfirmationNotes = newValue;
            this.MarkAsDirty("BookingConfirmationNotes");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OrderVolumetricWeight", {
        get: function () { return this.orderVolumetricWeight; },
        set: function (newValue) { if (this.orderVolumetricWeight != newValue) {
            this.orderVolumetricWeight = newValue;
            this.MarkAsDirty("OrderVolumetricWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OrderChargeableWeight", {
        get: function () { return this.orderChargeableWeight; },
        set: function (newValue) { if (this.orderChargeableWeight != newValue) {
            this.orderChargeableWeight = newValue;
            this.MarkAsDirty("OrderChargeableWeight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CutoffDate", {
        get: function () { return this.cutoffDate; },
        set: function (newValue) { if (this.cutoffDate != newValue) {
            this.cutoffDate = newValue;
            this.MarkAsDirty("CutoffDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPrint", {
        get: function () { return this.aWBPrint; },
        set: function (newValue) { if (this.aWBPrint != newValue) {
            this.aWBPrint = newValue;
            this.MarkAsDirty("AWBPrint");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FWBStatusCode", {
        get: function () { return this.fWBStatusCode; },
        set: function (newValue) { if (this.fWBStatusCode != newValue) {
            this.fWBStatusCode = newValue;
            this.MarkAsDirty("FWBStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FWBStatusName", {
        get: function () { return this.fWBStatusName; },
        set: function (newValue) { if (this.fWBStatusName != newValue) {
            this.fWBStatusName = newValue;
            this.MarkAsDirty("FWBStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FHLStatusCode", {
        get: function () { return this.fHLStatusCode; },
        set: function (newValue) { if (this.fHLStatusCode != newValue) {
            this.fHLStatusCode = newValue;
            this.MarkAsDirty("FHLStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FHLStatusName", {
        get: function () { return this.fHLStatusName; },
        set: function (newValue) { if (this.fHLStatusName != newValue) {
            this.fHLStatusName = newValue;
            this.MarkAsDirty("FHLStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBCurrencyId", {
        get: function () { return this.aWBCurrencyId; },
        set: function (newValue) { if (this.aWBCurrencyId != newValue) {
            this.aWBCurrencyId = newValue;
            this.MarkAsDirty("AWBCurrencyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBCurrencyCode", {
        get: function () { return this.aWBCurrencyCode; },
        set: function (newValue) { if (this.aWBCurrencyCode != newValue) {
            this.aWBCurrencyCode = newValue;
            this.MarkAsDirty("AWBCurrencyCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBFreightAmountPrepaid", {
        get: function () { return this.aWBFreightAmountPrepaid; },
        set: function (newValue) { if (this.aWBFreightAmountPrepaid != newValue) {
            this.aWBFreightAmountPrepaid = newValue;
            this.MarkAsDirty("AWBFreightAmountPrepaid");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBFreightAmountCollect", {
        get: function () { return this.aWBFreightAmountCollect; },
        set: function (newValue) { if (this.aWBFreightAmountCollect != newValue) {
            this.aWBFreightAmountCollect = newValue;
            this.MarkAsDirty("AWBFreightAmountCollect");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBCarrierTarrifReference", {
        get: function () { return this.aWBCarrierTarrifReference; },
        set: function (newValue) { if (this.aWBCarrierTarrifReference != newValue) {
            this.aWBCarrierTarrifReference = newValue;
            this.MarkAsDirty("AWBCarrierTarrifReference");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBDeclaredValueForCarriage", {
        get: function () { return this.aWBDeclaredValueForCarriage; },
        set: function (newValue) { if (this.aWBDeclaredValueForCarriage != newValue) {
            this.aWBDeclaredValueForCarriage = newValue;
            this.MarkAsDirty("AWBDeclaredValueForCarriage");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBDeclaredValueForCustoms", {
        get: function () { return this.aWBDeclaredValueForCustoms; },
        set: function (newValue) { if (this.aWBDeclaredValueForCustoms != newValue) {
            this.aWBDeclaredValueForCustoms = newValue;
            this.MarkAsDirty("AWBDeclaredValueForCustoms");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBAccountingInformation", {
        get: function () { return this.aWBAccountingInformation; },
        set: function (newValue) { if (this.aWBAccountingInformation != newValue) {
            this.aWBAccountingInformation = newValue;
            this.MarkAsDirty("AWBAccountingInformation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBInsurrenceValue", {
        get: function () { return this.aWBInsurrenceValue; },
        set: function (newValue) { if (this.aWBInsurrenceValue != newValue) {
            this.aWBInsurrenceValue = newValue;
            this.MarkAsDirty("AWBInsurrenceValue");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBHandlingInformation", {
        get: function () { return this.aWBHandlingInformation; },
        set: function (newValue) { if (this.aWBHandlingInformation != newValue) {
            this.aWBHandlingInformation = newValue;
            this.MarkAsDirty("AWBHandlingInformation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SCI", {
        get: function () { return this.sCI; },
        set: function (newValue) { if (this.sCI != newValue) {
            this.sCI = newValue;
            this.MarkAsDirty("SCI");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBComments", {
        get: function () { return this.aWBComments; },
        set: function (newValue) { if (this.aWBComments != newValue) {
            this.aWBComments = newValue;
            this.MarkAsDirty("AWBComments");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPrintingComments", {
        get: function () { return this.aWBPrintingComments; },
        set: function (newValue) { if (this.aWBPrintingComments != newValue) {
            this.aWBPrintingComments = newValue;
            this.MarkAsDirty("AWBPrintingComments");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBSignature", {
        get: function () { return this.aWBSignature; },
        set: function (newValue) { if (this.aWBSignature != newValue) {
            this.aWBSignature = newValue;
            this.MarkAsDirty("AWBSignature");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPlace", {
        get: function () { return this.aWBPlace; },
        set: function (newValue) { if (this.aWBPlace != newValue) {
            this.aWBPlace = newValue;
            this.MarkAsDirty("AWBPlace");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBChargesCodeCode", {
        get: function () { return this.aWBChargesCodeCode; },
        set: function (newValue) { if (this.aWBChargesCodeCode != newValue) {
            this.aWBChargesCodeCode = newValue;
            this.MarkAsDirty("AWBChargesCodeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineId", {
        get: function () { return this.tenantZeroAirlineId; },
        set: function (newValue) { if (this.tenantZeroAirlineId != newValue) {
            this.tenantZeroAirlineId = newValue;
            this.MarkAsDirty("TenantZeroAirlineId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineTTY", {
        get: function () { return this.tenantZeroAirlineTTY; },
        set: function (newValue) { if (this.tenantZeroAirlineTTY != newValue) {
            this.tenantZeroAirlineTTY = newValue;
            this.MarkAsDirty("TenantZeroAirlineTTY");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlinePIMA", {
        get: function () { return this.tenantZeroAirlinePIMA; },
        set: function (newValue) { if (this.tenantZeroAirlinePIMA != newValue) {
            this.tenantZeroAirlinePIMA = newValue;
            this.MarkAsDirty("TenantZeroAirlinePIMA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineChampFWB", {
        get: function () { return this.tenantZeroAirlineChampFWB; },
        set: function (newValue) { if (this.tenantZeroAirlineChampFWB != newValue) {
            this.tenantZeroAirlineChampFWB = newValue;
            this.MarkAsDirty("TenantZeroAirlineChampFWB");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineChampFHL", {
        get: function () { return this.tenantZeroAirlineChampFHL; },
        set: function (newValue) { if (this.tenantZeroAirlineChampFHL != newValue) {
            this.tenantZeroAirlineChampFHL = newValue;
            this.MarkAsDirty("TenantZeroAirlineChampFHL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineChampFSU", {
        get: function () { return this.tenantZeroAirlineChampFSU; },
        set: function (newValue) { if (this.tenantZeroAirlineChampFSU != newValue) {
            this.tenantZeroAirlineChampFSU = newValue;
            this.MarkAsDirty("TenantZeroAirlineChampFSU");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineChampFSRFSA", {
        get: function () { return this.tenantZeroAirlineChampFSRFSA; },
        set: function (newValue) { if (this.tenantZeroAirlineChampFSRFSA != newValue) {
            this.tenantZeroAirlineChampFSRFSA = newValue;
            this.MarkAsDirty("TenantZeroAirlineChampFSRFSA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineChampFVRFVA", {
        get: function () { return this.tenantZeroAirlineChampFVRFVA; },
        set: function (newValue) { if (this.tenantZeroAirlineChampFVRFVA != newValue) {
            this.tenantZeroAirlineChampFVRFVA = newValue;
            this.MarkAsDirty("TenantZeroAirlineChampFVRFVA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierIsChampRegistered", {
        get: function () { return this.carrierIsChampRegistered; },
        set: function (newValue) { if (this.carrierIsChampRegistered != newValue) {
            this.carrierIsChampRegistered = newValue;
            this.MarkAsDirty("CarrierIsChampRegistered");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineChampNeedsRegistration", {
        get: function () { return this.tenantZeroAirlineChampNeedsRegistration; },
        set: function (newValue) { if (this.tenantZeroAirlineChampNeedsRegistration != newValue) {
            this.tenantZeroAirlineChampNeedsRegistration = newValue;
            this.MarkAsDirty("TenantZeroAirlineChampNeedsRegistration");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineGLSHKFWB", {
        get: function () { return this.tenantZeroAirlineGLSHKFWB; },
        set: function (newValue) { if (this.tenantZeroAirlineGLSHKFWB != newValue) {
            this.tenantZeroAirlineGLSHKFWB = newValue;
            this.MarkAsDirty("TenantZeroAirlineGLSHKFWB");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineGLSHKFHL", {
        get: function () { return this.tenantZeroAirlineGLSHKFHL; },
        set: function (newValue) { if (this.tenantZeroAirlineGLSHKFHL != newValue) {
            this.tenantZeroAirlineGLSHKFHL = newValue;
            this.MarkAsDirty("TenantZeroAirlineGLSHKFHL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineGLSHKFSU", {
        get: function () { return this.tenantZeroAirlineGLSHKFSU; },
        set: function (newValue) { if (this.tenantZeroAirlineGLSHKFSU != newValue) {
            this.tenantZeroAirlineGLSHKFSU = newValue;
            this.MarkAsDirty("TenantZeroAirlineGLSHKFSU");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineGLSHKFSRFSA", {
        get: function () { return this.tenantZeroAirlineGLSHKFSRFSA; },
        set: function (newValue) { if (this.tenantZeroAirlineGLSHKFSRFSA != newValue) {
            this.tenantZeroAirlineGLSHKFSRFSA = newValue;
            this.MarkAsDirty("TenantZeroAirlineGLSHKFSRFSA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineGLSHKFVRFVA", {
        get: function () { return this.tenantZeroAirlineGLSHKFVRFVA; },
        set: function (newValue) { if (this.tenantZeroAirlineGLSHKFVRFVA != newValue) {
            this.tenantZeroAirlineGLSHKFVRFVA = newValue;
            this.MarkAsDirty("TenantZeroAirlineGLSHKFVRFVA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierIsGLSHKRegistered", {
        get: function () { return this.carrierIsGLSHKRegistered; },
        set: function (newValue) { if (this.carrierIsGLSHKRegistered != newValue) {
            this.carrierIsGLSHKRegistered = newValue;
            this.MarkAsDirty("CarrierIsGLSHKRegistered");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TenantZeroAirlineGLSHKNeedsRegistration", {
        get: function () { return this.tenantZeroAirlineGLSHKNeedsRegistration; },
        set: function (newValue) { if (this.tenantZeroAirlineGLSHKNeedsRegistration != newValue) {
            this.tenantZeroAirlineGLSHKNeedsRegistration = newValue;
            this.MarkAsDirty("TenantZeroAirlineGLSHKNeedsRegistration");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierIsCheckDigit", {
        get: function () { return this.carrierIsCheckDigit; },
        set: function (newValue) { if (this.carrierIsCheckDigit != newValue) {
            this.carrierIsCheckDigit = newValue;
            this.MarkAsDirty("CarrierIsCheckDigit");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CarrierIsLimitedLength", {
        get: function () { return this.carrierIsLimitedLength; },
        set: function (newValue) { if (this.carrierIsLimitedLength != newValue) {
            this.carrierIsLimitedLength = newValue;
            this.MarkAsDirty("CarrierIsLimitedLength");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LocalCustomsTransmissionsStatusCode", {
        get: function () { return this.localCustomsTransmissionsStatusCode; },
        set: function (newValue) { if (this.localCustomsTransmissionsStatusCode != newValue) {
            this.localCustomsTransmissionsStatusCode = newValue;
            this.MarkAsDirty("LocalCustomsTransmissionsStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LocalCustomsTransmissionsStatusName", {
        get: function () { return this.localCustomsTransmissionsStatusName; },
        set: function (newValue) { if (this.localCustomsTransmissionsStatusName != newValue) {
            this.localCustomsTransmissionsStatusName = newValue;
            this.MarkAsDirty("LocalCustomsTransmissionsStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LocalCustomsTransmissionsStatusError", {
        get: function () { return this.localCustomsTransmissionsStatusError; },
        set: function (newValue) { if (this.localCustomsTransmissionsStatusError != newValue) {
            this.localCustomsTransmissionsStatusError = newValue;
            this.MarkAsDirty("LocalCustomsTransmissionsStatusError");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LocalCustomsTransmissionsStatusDate", {
        get: function () { return this.localCustomsTransmissionsStatusDate; },
        set: function (newValue) { if (this.localCustomsTransmissionsStatusDate != newValue) {
            this.localCustomsTransmissionsStatusDate = newValue;
            this.MarkAsDirty("LocalCustomsTransmissionsStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LocalCustomsSentByUserId", {
        get: function () { return this.localCustomsSentByUserId; },
        set: function (newValue) { if (this.localCustomsSentByUserId != newValue) {
            this.localCustomsSentByUserId = newValue;
            this.MarkAsDirty("LocalCustomsSentByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LocalCustomsSentByUserName", {
        get: function () { return this.localCustomsSentByUserName; },
        set: function (newValue) { if (this.localCustomsSentByUserName != newValue) {
            this.localCustomsSentByUserName = newValue;
            this.MarkAsDirty("LocalCustomsSentByUserName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IncludesCustoms", {
        get: function () { return this.includesCustoms; },
        set: function (newValue) { if (this.includesCustoms != newValue) {
            this.includesCustoms = newValue;
            this.MarkAsDirty("IncludesCustoms");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsUpdateByAutomation", {
        get: function () { return this.isUpdateByAutomation; },
        set: function (newValue) { if (this.isUpdateByAutomation != newValue) {
            this.isUpdateByAutomation = newValue;
            this.MarkAsDirty("IsUpdateByAutomation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DeclarationNumber", {
        get: function () { return this.declarationNumber; },
        set: function (newValue) { if (this.declarationNumber != newValue) {
            this.declarationNumber = newValue;
            this.MarkAsDirty("DeclarationNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DeclarationDate", {
        get: function () { return this.declarationDate; },
        set: function (newValue) { if (this.declarationDate != newValue) {
            this.declarationDate = newValue;
            this.MarkAsDirty("DeclarationDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomsClearanceDate", {
        get: function () { return this.customsClearanceDate; },
        set: function (newValue) { if (this.customsClearanceDate != newValue) {
            this.customsClearanceDate = newValue;
            this.MarkAsDirty("CustomsClearanceDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProductCode", {
        get: function () { return this.productCode; },
        set: function (newValue) { if (this.productCode != newValue) {
            this.productCode = newValue;
            this.MarkAsDirty("ProductCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SecurityKey", {
        get: function () { return this.securityKey; },
        set: function (newValue) { if (this.securityKey != newValue) {
            this.securityKey = newValue;
            this.MarkAsDirty("SecurityKey");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TEU", {
        get: function () { return this.tEU; },
        set: function (newValue) { if (this.tEU != newValue) {
            this.tEU = newValue;
            this.MarkAsDirty("TEU");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConvertFromHouseToDirect", {
        get: function () { return this.convertFromHouseToDirect; },
        set: function (newValue) { if (this.convertFromHouseToDirect != newValue) {
            this.convertFromHouseToDirect = newValue;
            this.MarkAsDirty("ConvertFromHouseToDirect");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConvertFromDirectToHouse", {
        get: function () { return this.convertFromDirectToHouse; },
        set: function (newValue) { if (this.convertFromDirectToHouse != newValue) {
            this.convertFromDirectToHouse = newValue;
            this.MarkAsDirty("ConvertFromDirectToHouse");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsRefreshShipmentFollowUps", {
        get: function () { return this.isRefreshShipmentFollowUps; },
        set: function (newValue) { if (this.isRefreshShipmentFollowUps != newValue) {
            this.isRefreshShipmentFollowUps = newValue;
            this.MarkAsDirty("IsRefreshShipmentFollowUps");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerRankName", {
        get: function () { return this.customerRankName; },
        set: function (newValue) { if (this.customerRankName != newValue) {
            this.customerRankName = newValue;
            this.MarkAsDirty("CustomerRankName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FinalArrivalDate", {
        get: function () { return this.finalArrivalDate; },
        set: function (newValue) { if (this.finalArrivalDate != newValue) {
            this.finalArrivalDate = newValue;
            this.MarkAsDirty("FinalArrivalDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "EstimatedFinalArrivalDate", {
        get: function () { return this.estimatedFinalArrivalDate; },
        set: function (newValue) { if (this.estimatedFinalArrivalDate != newValue) {
            this.estimatedFinalArrivalDate = newValue;
            this.MarkAsDirty("EstimatedFinalArrivalDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ActualFinalArrivalDate", {
        get: function () { return this.actualFinalArrivalDate; },
        set: function (newValue) { if (this.actualFinalArrivalDate != newValue) {
            this.actualFinalArrivalDate = newValue;
            this.MarkAsDirty("ActualFinalArrivalDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ForeignPartnerCountryCode", {
        get: function () { return this.foreignPartnerCountryCode; },
        set: function (newValue) { if (this.foreignPartnerCountryCode != newValue) {
            this.foreignPartnerCountryCode = newValue;
            this.MarkAsDirty("ForeignPartnerCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastStatusLogDate", {
        get: function () { return this.lastStatusLogDate; },
        set: function (newValue) { if (this.lastStatusLogDate != newValue) {
            this.lastStatusLogDate = newValue;
            this.MarkAsDirty("LastStatusLogDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFullCarrierNumber", {
        get: function () { return this.mainCarriageFullCarrierNumber; },
        set: function (newValue) { if (this.mainCarriageFullCarrierNumber != newValue) {
            this.mainCarriageFullCarrierNumber = newValue;
            this.MarkAsDirty("MainCarriageFullCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1FullCarrierNumber", {
        get: function () { return this.transshipment1FullCarrierNumber; },
        set: function (newValue) { if (this.transshipment1FullCarrierNumber != newValue) {
            this.transshipment1FullCarrierNumber = newValue;
            this.MarkAsDirty("Transshipment1FullCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2FullCarrierNumber", {
        get: function () { return this.transshipment2FullCarrierNumber; },
        set: function (newValue) { if (this.transshipment2FullCarrierNumber != newValue) {
            this.transshipment2FullCarrierNumber = newValue;
            this.MarkAsDirty("Transshipment2FullCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3FullCarrierNumber", {
        get: function () { return this.transshipment3FullCarrierNumber; },
        set: function (newValue) { if (this.transshipment3FullCarrierNumber != newValue) {
            this.transshipment3FullCarrierNumber = newValue;
            this.MarkAsDirty("Transshipment3FullCarrierNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ExceptionDescription", {
        get: function () { return this.exceptionDescription; },
        set: function (newValue) { if (this.exceptionDescription != newValue) {
            this.exceptionDescription = newValue;
            this.MarkAsDirty("ExceptionDescription");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ExceptionResolvedDescription", {
        get: function () { return this.exceptionResolvedDescription; },
        set: function (newValue) { if (this.exceptionResolvedDescription != newValue) {
            this.exceptionResolvedDescription = newValue;
            this.MarkAsDirty("ExceptionResolvedDescription");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastExceptionDescription", {
        get: function () { return this.lastExceptionDescription; },
        set: function (newValue) { if (this.lastExceptionDescription != newValue) {
            this.lastExceptionDescription = newValue;
            this.MarkAsDirty("LastExceptionDescription");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ExceptionDate", {
        get: function () { return this.exceptionDate; },
        set: function (newValue) { if (this.exceptionDate != newValue) {
            this.exceptionDate = newValue;
            this.MarkAsDirty("ExceptionDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "HasException", {
        get: function () { return this.hasException; },
        set: function (newValue) { if (this.hasException != newValue) {
            this.hasException = newValue;
            this.MarkAsDirty("HasException");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "HasExceptionMessage", {
        get: function () { return this.hasExceptionMessage; },
        set: function (newValue) { if (this.hasExceptionMessage != newValue) {
            this.hasExceptionMessage = newValue;
            this.MarkAsDirty("HasExceptionMessage");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromLocation", {
        get: function () { return this.fromLocation; },
        set: function (newValue) { if (this.fromLocation != newValue) {
            this.fromLocation = newValue;
            this.MarkAsDirty("FromLocation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToLocation", {
        get: function () { return this.toLocation; },
        set: function (newValue) { if (this.toLocation != newValue) {
            this.toLocation = newValue;
            this.MarkAsDirty("ToLocation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MoveTypeId", {
        get: function () { return this.moveTypeId; },
        set: function (newValue) { if (this.moveTypeId != newValue) {
            this.moveTypeId = newValue;
            this.MarkAsDirty("MoveTypeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MoveTypeCode", {
        get: function () { return this.moveTypeCode; },
        set: function (newValue) { if (this.moveTypeCode != newValue) {
            this.moveTypeCode = newValue;
            this.MarkAsDirty("MoveTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MoveTypeName", {
        get: function () { return this.moveTypeName; },
        set: function (newValue) { if (this.moveTypeName != newValue) {
            this.moveTypeName = newValue;
            this.MarkAsDirty("MoveTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsCreatedFromAgentSharedManifest", {
        get: function () { return this.isCreatedFromAgentSharedManifest; },
        set: function (newValue) { if (this.isCreatedFromAgentSharedManifest != newValue) {
            this.isCreatedFromAgentSharedManifest = newValue;
            this.MarkAsDirty("IsCreatedFromAgentSharedManifest");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AMSBL", {
        get: function () { return this.aMSBL; },
        set: function (newValue) { if (this.aMSBL != newValue) {
            this.aMSBL = newValue;
            this.MarkAsDirty("AMSBL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomFileId", {
        get: function () { return this.customFileId; },
        set: function (newValue) { if (this.customFileId != newValue) {
            this.customFileId = newValue;
            this.MarkAsDirty("CustomFileId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomFileNumber", {
        get: function () { return this.customFileNumber; },
        set: function (newValue) { if (this.customFileNumber != newValue) {
            this.customFileNumber = newValue;
            this.MarkAsDirty("CustomFileNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageSTD", {
        get: function () { return this.mainCarriageSTD; },
        set: function (newValue) { if (this.mainCarriageSTD != newValue) {
            this.mainCarriageSTD = newValue;
            this.MarkAsDirty("MainCarriageSTD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageSTA", {
        get: function () { return this.mainCarriageSTA; },
        set: function (newValue) { if (this.mainCarriageSTA != newValue) {
            this.mainCarriageSTA = newValue;
            this.MarkAsDirty("MainCarriageSTA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1STD", {
        get: function () { return this.transshipment1STD; },
        set: function (newValue) { if (this.transshipment1STD != newValue) {
            this.transshipment1STD = newValue;
            this.MarkAsDirty("Transshipment1STD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment1STA", {
        get: function () { return this.transshipment1STA; },
        set: function (newValue) { if (this.transshipment1STA != newValue) {
            this.transshipment1STA = newValue;
            this.MarkAsDirty("Transshipment1STA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2STD", {
        get: function () { return this.transshipment2STD; },
        set: function (newValue) { if (this.transshipment2STD != newValue) {
            this.transshipment2STD = newValue;
            this.MarkAsDirty("Transshipment2STD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment2STA", {
        get: function () { return this.transshipment2STA; },
        set: function (newValue) { if (this.transshipment2STA != newValue) {
            this.transshipment2STA = newValue;
            this.MarkAsDirty("Transshipment2STA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3STD", {
        get: function () { return this.transshipment3STD; },
        set: function (newValue) { if (this.transshipment3STD != newValue) {
            this.transshipment3STD = newValue;
            this.MarkAsDirty("Transshipment3STD");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Transshipment3STA", {
        get: function () { return this.transshipment3STA; },
        set: function (newValue) { if (this.transshipment3STA != newValue) {
            this.transshipment3STA = newValue;
            this.MarkAsDirty("Transshipment3STA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsMultipleCommodities", {
        get: function () { return this.isMultipleCommodities; },
        set: function (newValue) { if (this.isMultipleCommodities != newValue) {
            this.isMultipleCommodities = newValue;
            this.MarkAsDirty("IsMultipleCommodities");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NominatedHandlingPartyId", {
        get: function () { return this.nominatedHandlingPartyId; },
        set: function (newValue) { if (this.nominatedHandlingPartyId != newValue) {
            this.nominatedHandlingPartyId = newValue;
            this.MarkAsDirty("NominatedHandlingPartyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantIdCode1", {
        get: function () { return this.otherParticipantIdCode1; },
        set: function (newValue) { if (this.otherParticipantIdCode1 != newValue) {
            this.otherParticipantIdCode1 = newValue;
            this.MarkAsDirty("OtherParticipantIdCode1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantIdCode2", {
        get: function () { return this.otherParticipantIdCode2; },
        set: function (newValue) { if (this.otherParticipantIdCode2 != newValue) {
            this.otherParticipantIdCode2 = newValue;
            this.MarkAsDirty("OtherParticipantIdCode2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantIdCode3", {
        get: function () { return this.otherParticipantIdCode3; },
        set: function (newValue) { if (this.otherParticipantIdCode3 != newValue) {
            this.otherParticipantIdCode3 = newValue;
            this.MarkAsDirty("OtherParticipantIdCode3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationCode1", {
        get: function () { return this.otherParticipantInformationCode1; },
        set: function (newValue) { if (this.otherParticipantInformationCode1 != newValue) {
            this.otherParticipantInformationCode1 = newValue;
            this.MarkAsDirty("OtherParticipantInformationCode1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationCode2", {
        get: function () { return this.otherParticipantInformationCode2; },
        set: function (newValue) { if (this.otherParticipantInformationCode2 != newValue) {
            this.otherParticipantInformationCode2 = newValue;
            this.MarkAsDirty("OtherParticipantInformationCode2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationCode3", {
        get: function () { return this.otherParticipantInformationCode3; },
        set: function (newValue) { if (this.otherParticipantInformationCode3 != newValue) {
            this.otherParticipantInformationCode3 = newValue;
            this.MarkAsDirty("OtherParticipantInformationCode3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationPortCode1", {
        get: function () { return this.otherParticipantInformationPortCode1; },
        set: function (newValue) { if (this.otherParticipantInformationPortCode1 != newValue) {
            this.otherParticipantInformationPortCode1 = newValue;
            this.MarkAsDirty("OtherParticipantInformationPortCode1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationPortCode2", {
        get: function () { return this.otherParticipantInformationPortCode2; },
        set: function (newValue) { if (this.otherParticipantInformationPortCode2 != newValue) {
            this.otherParticipantInformationPortCode2 = newValue;
            this.MarkAsDirty("OtherParticipantInformationPortCode2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationPortCode3", {
        get: function () { return this.otherParticipantInformationPortCode3; },
        set: function (newValue) { if (this.otherParticipantInformationPortCode3 != newValue) {
            this.otherParticipantInformationPortCode3 = newValue;
            this.MarkAsDirty("OtherParticipantInformationPortCode3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationName1", {
        get: function () { return this.otherParticipantInformationName1; },
        set: function (newValue) { if (this.otherParticipantInformationName1 != newValue) {
            this.otherParticipantInformationName1 = newValue;
            this.MarkAsDirty("OtherParticipantInformationName1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationName2", {
        get: function () { return this.otherParticipantInformationName2; },
        set: function (newValue) { if (this.otherParticipantInformationName2 != newValue) {
            this.otherParticipantInformationName2 = newValue;
            this.MarkAsDirty("OtherParticipantInformationName2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationName3", {
        get: function () { return this.otherParticipantInformationName3; },
        set: function (newValue) { if (this.otherParticipantInformationName3 != newValue) {
            this.otherParticipantInformationName3 = newValue;
            this.MarkAsDirty("OtherParticipantInformationName3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationReference1", {
        get: function () { return this.otherParticipantInformationReference1; },
        set: function (newValue) { if (this.otherParticipantInformationReference1 != newValue) {
            this.otherParticipantInformationReference1 = newValue;
            this.MarkAsDirty("OtherParticipantInformationReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationReference2", {
        get: function () { return this.otherParticipantInformationReference2; },
        set: function (newValue) { if (this.otherParticipantInformationReference2 != newValue) {
            this.otherParticipantInformationReference2 = newValue;
            this.MarkAsDirty("OtherParticipantInformationReference2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OtherParticipantInformationReference3", {
        get: function () { return this.otherParticipantInformationReference3; },
        set: function (newValue) { if (this.otherParticipantInformationReference3 != newValue) {
            this.otherParticipantInformationReference3 = newValue;
            this.MarkAsDirty("OtherParticipantInformationReference3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformation1", {
        get: function () { return this.accountingInformation1; },
        set: function (newValue) { if (this.accountingInformation1 != newValue) {
            this.accountingInformation1 = newValue;
            this.MarkAsDirty("AccountingInformation1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformation2", {
        get: function () { return this.accountingInformation2; },
        set: function (newValue) { if (this.accountingInformation2 != newValue) {
            this.accountingInformation2 = newValue;
            this.MarkAsDirty("AccountingInformation2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformation3", {
        get: function () { return this.accountingInformation3; },
        set: function (newValue) { if (this.accountingInformation3 != newValue) {
            this.accountingInformation3 = newValue;
            this.MarkAsDirty("AccountingInformation3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformation4", {
        get: function () { return this.accountingInformation4; },
        set: function (newValue) { if (this.accountingInformation4 != newValue) {
            this.accountingInformation4 = newValue;
            this.MarkAsDirty("AccountingInformation4");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformation5", {
        get: function () { return this.accountingInformation5; },
        set: function (newValue) { if (this.accountingInformation5 != newValue) {
            this.accountingInformation5 = newValue;
            this.MarkAsDirty("AccountingInformation5");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformation6", {
        get: function () { return this.accountingInformation6; },
        set: function (newValue) { if (this.accountingInformation6 != newValue) {
            this.accountingInformation6 = newValue;
            this.MarkAsDirty("AccountingInformation6");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformationIdentifierCode1", {
        get: function () { return this.accountingInformationIdentifierCode1; },
        set: function (newValue) { if (this.accountingInformationIdentifierCode1 != newValue) {
            this.accountingInformationIdentifierCode1 = newValue;
            this.MarkAsDirty("AccountingInformationIdentifierCode1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformationIdentifierCode2", {
        get: function () { return this.accountingInformationIdentifierCode2; },
        set: function (newValue) { if (this.accountingInformationIdentifierCode2 != newValue) {
            this.accountingInformationIdentifierCode2 = newValue;
            this.MarkAsDirty("AccountingInformationIdentifierCode2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformationIdentifierCode3", {
        get: function () { return this.accountingInformationIdentifierCode3; },
        set: function (newValue) { if (this.accountingInformationIdentifierCode3 != newValue) {
            this.accountingInformationIdentifierCode3 = newValue;
            this.MarkAsDirty("AccountingInformationIdentifierCode3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformationIdentifierCode4", {
        get: function () { return this.accountingInformationIdentifierCode4; },
        set: function (newValue) { if (this.accountingInformationIdentifierCode4 != newValue) {
            this.accountingInformationIdentifierCode4 = newValue;
            this.MarkAsDirty("AccountingInformationIdentifierCode4");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformationIdentifierCode5", {
        get: function () { return this.accountingInformationIdentifierCode5; },
        set: function (newValue) { if (this.accountingInformationIdentifierCode5 != newValue) {
            this.accountingInformationIdentifierCode5 = newValue;
            this.MarkAsDirty("AccountingInformationIdentifierCode5");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingInformationIdentifierCode6", {
        get: function () { return this.accountingInformationIdentifierCode6; },
        set: function (newValue) { if (this.accountingInformationIdentifierCode6 != newValue) {
            this.accountingInformationIdentifierCode6 = newValue;
            this.MarkAsDirty("AccountingInformationIdentifierCode6");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ReferenceNumber", {
        get: function () { return this.referenceNumber; },
        set: function (newValue) { if (this.referenceNumber != newValue) {
            this.referenceNumber = newValue;
            this.MarkAsDirty("ReferenceNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SupplementaryShipmentInformation1", {
        get: function () { return this.supplementaryShipmentInformation1; },
        set: function (newValue) { if (this.supplementaryShipmentInformation1 != newValue) {
            this.supplementaryShipmentInformation1 = newValue;
            this.MarkAsDirty("SupplementaryShipmentInformation1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SupplementaryShipmentInformation2", {
        get: function () { return this.supplementaryShipmentInformation2; },
        set: function (newValue) { if (this.supplementaryShipmentInformation2 != newValue) {
            this.supplementaryShipmentInformation2 = newValue;
            this.MarkAsDirty("SupplementaryShipmentInformation2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastSentByUserId", {
        get: function () { return this.lastSentByUserId; },
        set: function (newValue) { if (this.lastSentByUserId != newValue) {
            this.lastSentByUserId = newValue;
            this.MarkAsDirty("LastSentByUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ValueOfGoods", {
        get: function () { return this.valueOfGoods; },
        set: function (newValue) { if (this.valueOfGoods != newValue) {
            this.valueOfGoods = newValue;
            this.MarkAsDirty("ValueOfGoods");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ValueOfGoodsCurrencyId", {
        get: function () { return this.valueOfGoodsCurrencyId; },
        set: function (newValue) { if (this.valueOfGoodsCurrencyId != newValue) {
            this.valueOfGoodsCurrencyId = newValue;
            this.MarkAsDirty("ValueOfGoodsCurrencyId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OperationalDate", {
        get: function () { return this.operationalDate; },
        set: function (newValue) { if (this.operationalDate != newValue) {
            this.operationalDate = newValue;
            this.MarkAsDirty("OperationalDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FBLIsFromStock", {
        get: function () { return this.fBLIsFromStock; },
        set: function (newValue) { if (this.fBLIsFromStock != newValue) {
            this.fBLIsFromStock = newValue;
            this.MarkAsDirty("FBLIsFromStock");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FBLReturnedToStock", {
        get: function () { return this.fBLReturnedToStock; },
        set: function (newValue) { if (this.fBLReturnedToStock != newValue) {
            this.fBLReturnedToStock = newValue;
            this.MarkAsDirty("FBLReturnedToStock");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FBLTakenFromStock", {
        get: function () { return this.fBLTakenFromStock; },
        set: function (newValue) { if (this.fBLTakenFromStock != newValue) {
            this.fBLTakenFromStock = newValue;
            this.MarkAsDirty("FBLTakenFromStock");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FBLStockNumber", {
        get: function () { return this.fBLStockNumber; },
        set: function (newValue) { if (this.fBLStockNumber != newValue) {
            this.fBLStockNumber = newValue;
            this.MarkAsDirty("FBLStockNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FBLReturnedToStockWithCancel", {
        get: function () { return this.fBLReturnedToStockWithCancel; },
        set: function (newValue) { if (this.fBLReturnedToStockWithCancel != newValue) {
            this.fBLReturnedToStockWithCancel = newValue;
            this.MarkAsDirty("FBLReturnedToStockWithCancel");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromPartnerId", {
        get: function () { return this.mainCarriageFromPartnerId; },
        set: function (newValue) { if (this.mainCarriageFromPartnerId != newValue) {
            this.mainCarriageFromPartnerId = newValue;
            this.MarkAsDirty("MainCarriageFromPartnerId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFromAddressId", {
        get: function () { return this.mainCarriageFromAddressId; },
        set: function (newValue) { if (this.mainCarriageFromAddressId != newValue) {
            this.mainCarriageFromAddressId = newValue;
            this.MarkAsDirty("MainCarriageFromAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToPartnerId", {
        get: function () { return this.mainCarriageToPartnerId; },
        set: function (newValue) { if (this.mainCarriageToPartnerId != newValue) {
            this.mainCarriageToPartnerId = newValue;
            this.MarkAsDirty("MainCarriageToPartnerId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageToAddressId", {
        get: function () { return this.mainCarriageToAddressId; },
        set: function (newValue) { if (this.mainCarriageToAddressId != newValue) {
            this.mainCarriageToAddressId = newValue;
            this.MarkAsDirty("MainCarriageToAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Driver", {
        get: function () { return this.driver; },
        set: function (newValue) { if (this.driver != newValue) {
            this.driver = newValue;
            this.MarkAsDirty("Driver");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TruckNumber", {
        get: function () { return this.truckNumber; },
        set: function (newValue) { if (this.truckNumber != newValue) {
            this.truckNumber = newValue;
            this.MarkAsDirty("TruckNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TrailerNumber", {
        get: function () { return this.trailerNumber; },
        set: function (newValue) { if (this.trailerNumber != newValue) {
            this.trailerNumber = newValue;
            this.MarkAsDirty("TrailerNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AsAgreedFreight", {
        get: function () { return this.asAgreedFreight; },
        set: function (newValue) { if (this.asAgreedFreight != newValue) {
            this.asAgreedFreight = newValue;
            this.MarkAsDirty("AsAgreedFreight");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AsAgreedOtherCharges", {
        get: function () { return this.asAgreedOtherCharges; },
        set: function (newValue) { if (this.asAgreedOtherCharges != newValue) {
            this.asAgreedOtherCharges = newValue;
            this.MarkAsDirty("AsAgreedOtherCharges");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ARInvoiceIssued", {
        get: function () { return this.aRInvoiceIssued; },
        set: function (newValue) { if (this.aRInvoiceIssued != newValue) {
            this.aRInvoiceIssued = newValue;
            this.MarkAsDirty("ARInvoiceIssued");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CreditNoteIssued", {
        get: function () { return this.creditNoteIssued; },
        set: function (newValue) { if (this.creditNoteIssued != newValue) {
            this.creditNoteIssued = newValue;
            this.MarkAsDirty("CreditNoteIssued");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountNumber", {
        get: function () { return this.accountNumber; },
        set: function (newValue) { if (this.accountNumber != newValue) {
            this.accountNumber = newValue;
            this.MarkAsDirty("AccountNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CargonautFHLStatusCode", {
        get: function () { return this.cargonautFHLStatusCode; },
        set: function (newValue) { if (this.cargonautFHLStatusCode != newValue) {
            this.cargonautFHLStatusCode = newValue;
            this.MarkAsDirty("CargonautFHLStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CargonautFHLStatusName", {
        get: function () { return this.cargonautFHLStatusName; },
        set: function (newValue) { if (this.cargonautFHLStatusName != newValue) {
            this.cargonautFHLStatusName = newValue;
            this.MarkAsDirty("CargonautFHLStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CargonautFHLStatusDate", {
        get: function () { return this.cargonautFHLStatusDate; },
        set: function (newValue) { if (this.cargonautFHLStatusDate != newValue) {
            this.cargonautFHLStatusDate = newValue;
            this.MarkAsDirty("CargonautFHLStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CargonautFWBStatusCode", {
        get: function () { return this.cargonautFWBStatusCode; },
        set: function (newValue) { if (this.cargonautFWBStatusCode != newValue) {
            this.cargonautFWBStatusCode = newValue;
            this.MarkAsDirty("CargonautFWBStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CargonautFWBStatusName", {
        get: function () { return this.cargonautFWBStatusName; },
        set: function (newValue) { if (this.cargonautFWBStatusName != newValue) {
            this.cargonautFWBStatusName = newValue;
            this.MarkAsDirty("CargonautFWBStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CargonautFWBStatusDate", {
        get: function () { return this.cargonautFWBStatusDate; },
        set: function (newValue) { if (this.cargonautFWBStatusDate != newValue) {
            this.cargonautFWBStatusDate = newValue;
            this.MarkAsDirty("CargonautFWBStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MarkFollowUpsAsDone", {
        get: function () { return this.markFollowUpsAsDone; },
        set: function (newValue) { if (this.markFollowUpsAsDone != newValue) {
            this.markFollowUpsAsDone = newValue;
            this.MarkAsDirty("MarkFollowUpsAsDone");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CalculateProfit", {
        get: function () { return this.calculateProfit; },
        set: function (newValue) { if (this.calculateProfit != newValue) {
            this.calculateProfit = newValue;
            this.MarkAsDirty("CalculateProfit");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CalculateStatus", {
        get: function () { return this.calculateStatus; },
        set: function (newValue) { if (this.calculateStatus != newValue) {
            this.calculateStatus = newValue;
            this.MarkAsDirty("CalculateStatus");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ComputedStatusId", {
        get: function () { return this.computedStatusId; },
        set: function (newValue) { if (this.computedStatusId != newValue) {
            this.computedStatusId = newValue;
            this.MarkAsDirty("ComputedStatusId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ComputedStatusDate", {
        get: function () { return this.computedStatusDate; },
        set: function (newValue) { if (this.computedStatusDate != newValue) {
            this.computedStatusDate = newValue;
            this.MarkAsDirty("ComputedStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ComputedStatusName", {
        get: function () { return this.computedStatusName; },
        set: function (newValue) { if (this.computedStatusName != newValue) {
            this.computedStatusName = newValue;
            this.MarkAsDirty("ComputedStatusName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomFilePocoId", {
        get: function () { return this.customFilePocoId; },
        set: function (newValue) { if (this.customFilePocoId != newValue) {
            this.customFilePocoId = newValue;
            this.MarkAsDirty("CustomFilePocoId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CalculatePayables", {
        get: function () { return this.calculatePayables; },
        set: function (newValue) { if (this.calculatePayables != newValue) {
            this.calculatePayables = newValue;
            this.MarkAsDirty("CalculatePayables");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CalculateReceivables", {
        get: function () { return this.calculateReceivables; },
        set: function (newValue) { if (this.calculateReceivables != newValue) {
            this.calculateReceivables = newValue;
            this.MarkAsDirty("CalculateReceivables");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsAddingStackEvents", {
        get: function () { return this.isAddingStackEvents; },
        set: function (newValue) { if (this.isAddingStackEvents != newValue) {
            this.isAddingStackEvents = newValue;
            this.MarkAsDirty("IsAddingStackEvents");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsRemovingStackEvents", {
        get: function () { return this.isRemovingStackEvents; },
        set: function (newValue) { if (this.isRemovingStackEvents != newValue) {
            this.isRemovingStackEvents = newValue;
            this.MarkAsDirty("IsRemovingStackEvents");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "StackAirlineId", {
        get: function () { return this.stackAirlineId; },
        set: function (newValue) { if (this.stackAirlineId != newValue) {
            this.stackAirlineId = newValue;
            this.MarkAsDirty("StackAirlineId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPartnerCity", {
        get: function () { return this.fromPartnerCity; },
        set: function (newValue) { if (this.fromPartnerCity != newValue) {
            this.fromPartnerCity = newValue;
            this.MarkAsDirty("FromPartnerCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPartnerCountryCode", {
        get: function () { return this.fromPartnerCountryCode; },
        set: function (newValue) { if (this.fromPartnerCountryCode != newValue) {
            this.fromPartnerCountryCode = newValue;
            this.MarkAsDirty("FromPartnerCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromPartnerCountryName", {
        get: function () { return this.fromPartnerCountryName; },
        set: function (newValue) { if (this.fromPartnerCountryName != newValue) {
            this.fromPartnerCountryName = newValue;
            this.MarkAsDirty("FromPartnerCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPartnerCity", {
        get: function () { return this.toPartnerCity; },
        set: function (newValue) { if (this.toPartnerCity != newValue) {
            this.toPartnerCity = newValue;
            this.MarkAsDirty("ToPartnerCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPartnerCountryCode", {
        get: function () { return this.toPartnerCountryCode; },
        set: function (newValue) { if (this.toPartnerCountryCode != newValue) {
            this.toPartnerCountryCode = newValue;
            this.MarkAsDirty("ToPartnerCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToPartnerCountryName", {
        get: function () { return this.toPartnerCountryName; },
        set: function (newValue) { if (this.toPartnerCountryName != newValue) {
            this.toPartnerCountryName = newValue;
            this.MarkAsDirty("ToPartnerCountryName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsSendFSRCreatingShipment", {
        get: function () { return this.isSendFSRCreatingShipment; },
        set: function (newValue) { if (this.isSendFSRCreatingShipment != newValue) {
            this.isSendFSRCreatingShipment = newValue;
            this.MarkAsDirty("IsSendFSRCreatingShipment");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToCountryId", {
        get: function () { return this.toCountryId; },
        set: function (newValue) { if (this.toCountryId != newValue) {
            this.toCountryId = newValue;
            this.MarkAsDirty("ToCountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromCountryId", {
        get: function () { return this.fromCountryId; },
        set: function (newValue) { if (this.fromCountryId != newValue) {
            this.fromCountryId = newValue;
            this.MarkAsDirty("FromCountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CopyFromShipmentId", {
        get: function () { return this.copyFromShipmentId; },
        set: function (newValue) { if (this.copyFromShipmentId != newValue) {
            this.copyFromShipmentId = newValue;
            this.MarkAsDirty("CopyFromShipmentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsCopyFromShipment", {
        get: function () { return this.isCopyFromShipment; },
        set: function (newValue) { if (this.isCopyFromShipment != newValue) {
            this.isCopyFromShipment = newValue;
            this.MarkAsDirty("IsCopyFromShipment");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsBuildFromQuote", {
        get: function () { return this.isBuildFromQuote; },
        set: function (newValue) { if (this.isBuildFromQuote != newValue) {
            this.isBuildFromQuote = newValue;
            this.MarkAsDirty("IsBuildFromQuote");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsBuildFromBooking", {
        get: function () { return this.isBuildFromBooking; },
        set: function (newValue) { if (this.isBuildFromBooking != newValue) {
            this.isBuildFromBooking = newValue;
            this.MarkAsDirty("IsBuildFromBooking");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperMainAddressId", {
        get: function () { return this.shipperMainAddressId; },
        set: function (newValue) { if (this.shipperMainAddressId != newValue) {
            this.shipperMainAddressId = newValue;
            this.MarkAsDirty("ShipperMainAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperPickAddressId", {
        get: function () { return this.shipperPickAddressId; },
        set: function (newValue) { if (this.shipperPickAddressId != newValue) {
            this.shipperPickAddressId = newValue;
            this.MarkAsDirty("ShipperPickAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeMainAddressId", {
        get: function () { return this.consigneeMainAddressId; },
        set: function (newValue) { if (this.consigneeMainAddressId != newValue) {
            this.consigneeMainAddressId = newValue;
            this.MarkAsDirty("ConsigneeMainAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneePickAddressId", {
        get: function () { return this.consigneePickAddressId; },
        set: function (newValue) { if (this.consigneePickAddressId != newValue) {
            this.consigneePickAddressId = newValue;
            this.MarkAsDirty("ConsigneePickAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "HasPreCarriage", {
        get: function () { return this.hasPreCarriage; },
        set: function (newValue) { if (this.hasPreCarriage != newValue) {
            this.hasPreCarriage = newValue;
            this.MarkAsDirty("HasPreCarriage");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "HasOnCarriage", {
        get: function () { return this.hasOnCarriage; },
        set: function (newValue) { if (this.hasOnCarriage != newValue) {
            this.hasOnCarriage = newValue;
            this.MarkAsDirty("HasOnCarriage");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IncludePickUp", {
        get: function () { return this.includePickUp; },
        set: function (newValue) { if (this.includePickUp != newValue) {
            this.includePickUp = newValue;
            this.MarkAsDirty("IncludePickUp");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromAddressCity", {
        get: function () { return this.fromAddressCity; },
        set: function (newValue) { if (this.fromAddressCity != newValue) {
            this.fromAddressCity = newValue;
            this.MarkAsDirty("FromAddressCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromAddressZipCode", {
        get: function () { return this.fromAddressZipCode; },
        set: function (newValue) { if (this.fromAddressZipCode != newValue) {
            this.fromAddressZipCode = newValue;
            this.MarkAsDirty("FromAddressZipCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromAddressCountryId", {
        get: function () { return this.fromAddressCountryId; },
        set: function (newValue) { if (this.fromAddressCountryId != newValue) {
            this.fromAddressCountryId = newValue;
            this.MarkAsDirty("FromAddressCountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PickUpAddressId", {
        get: function () { return this.pickUpAddressId; },
        set: function (newValue) { if (this.pickUpAddressId != newValue) {
            this.pickUpAddressId = newValue;
            this.MarkAsDirty("PickUpAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomConnectToShipment", {
        get: function () { return this.customConnectToShipment; },
        set: function (newValue) { if (this.customConnectToShipment != newValue) {
            this.customConnectToShipment = newValue;
            this.MarkAsDirty("CustomConnectToShipment");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IncludeDelivery", {
        get: function () { return this.includeDelivery; },
        set: function (newValue) { if (this.includeDelivery != newValue) {
            this.includeDelivery = newValue;
            this.MarkAsDirty("IncludeDelivery");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToAddressCity", {
        get: function () { return this.toAddressCity; },
        set: function (newValue) { if (this.toAddressCity != newValue) {
            this.toAddressCity = newValue;
            this.MarkAsDirty("ToAddressCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToAddressZipCode", {
        get: function () { return this.toAddressZipCode; },
        set: function (newValue) { if (this.toAddressZipCode != newValue) {
            this.toAddressZipCode = newValue;
            this.MarkAsDirty("ToAddressZipCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToAddressCountryId", {
        get: function () { return this.toAddressCountryId; },
        set: function (newValue) { if (this.toAddressCountryId != newValue) {
            this.toAddressCountryId = newValue;
            this.MarkAsDirty("ToAddressCountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DeliveryAddressId", {
        get: function () { return this.deliveryAddressId; },
        set: function (newValue) { if (this.deliveryAddressId != newValue) {
            this.deliveryAddressId = newValue;
            this.MarkAsDirty("DeliveryAddressId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SpecialServicesTypeId", {
        get: function () { return this.specialServicesTypeId; },
        set: function (newValue) { if (this.specialServicesTypeId != newValue) {
            this.specialServicesTypeId = newValue;
            this.MarkAsDirty("SpecialServicesTypeId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SpecialServicesTypeName", {
        get: function () { return this.specialServicesTypeName; },
        set: function (newValue) { if (this.specialServicesTypeName != newValue) {
            this.specialServicesTypeName = newValue;
            this.MarkAsDirty("SpecialServicesTypeName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Quantity1", {
        get: function () { return this.quantity1; },
        set: function (newValue) { if (this.quantity1 != newValue) {
            this.quantity1 = newValue;
            this.MarkAsDirty("Quantity1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Quantity2", {
        get: function () { return this.quantity2; },
        set: function (newValue) { if (this.quantity2 != newValue) {
            this.quantity2 = newValue;
            this.MarkAsDirty("Quantity2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Quantity3", {
        get: function () { return this.quantity3; },
        set: function (newValue) { if (this.quantity3 != newValue) {
            this.quantity3 = newValue;
            this.MarkAsDirty("Quantity3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Quantity4", {
        get: function () { return this.quantity4; },
        set: function (newValue) { if (this.quantity4 != newValue) {
            this.quantity4 = newValue;
            this.MarkAsDirty("Quantity4");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Quantity5", {
        get: function () { return this.quantity5; },
        set: function (newValue) { if (this.quantity5 != newValue) {
            this.quantity5 = newValue;
            this.MarkAsDirty("Quantity5");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackageTypeId1", {
        get: function () { return this.packageTypeId1; },
        set: function (newValue) { if (this.packageTypeId1 != newValue) {
            this.packageTypeId1 = newValue;
            this.MarkAsDirty("PackageTypeId1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackageTypeId2", {
        get: function () { return this.packageTypeId2; },
        set: function (newValue) { if (this.packageTypeId2 != newValue) {
            this.packageTypeId2 = newValue;
            this.MarkAsDirty("PackageTypeId2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackageTypeId3", {
        get: function () { return this.packageTypeId3; },
        set: function (newValue) { if (this.packageTypeId3 != newValue) {
            this.packageTypeId3 = newValue;
            this.MarkAsDirty("PackageTypeId3");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackageTypeId4", {
        get: function () { return this.packageTypeId4; },
        set: function (newValue) { if (this.packageTypeId4 != newValue) {
            this.packageTypeId4 = newValue;
            this.MarkAsDirty("PackageTypeId4");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackageTypeId5", {
        get: function () { return this.packageTypeId5; },
        set: function (newValue) { if (this.packageTypeId5 != newValue) {
            this.packageTypeId5 = newValue;
            this.MarkAsDirty("PackageTypeId5");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NewConcurrencyGUID", {
        get: function () { return this.newConcurrencyGUID; },
        set: function (newValue) { if (this.newConcurrencyGUID != newValue) {
            this.newConcurrencyGUID = newValue;
            this.MarkAsDirty("NewConcurrencyGUID");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConnectedShipmentsPayablesCount", {
        get: function () { return this.connectedShipmentsPayablesCount; },
        set: function (newValue) { if (this.connectedShipmentsPayablesCount != newValue) {
            this.connectedShipmentsPayablesCount = newValue;
            this.MarkAsDirty("ConnectedShipmentsPayablesCount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConnectedShipmentsReceivablesCount", {
        get: function () { return this.connectedShipmentsReceivablesCount; },
        set: function (newValue) { if (this.connectedShipmentsReceivablesCount != newValue) {
            this.connectedShipmentsReceivablesCount = newValue;
            this.MarkAsDirty("ConnectedShipmentsReceivablesCount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NumberOfInsidePackages", {
        get: function () { return this.numberOfInsidePackages; },
        set: function (newValue) { if (this.numberOfInsidePackages != newValue) {
            this.numberOfInsidePackages = newValue;
            this.MarkAsDirty("NumberOfInsidePackages");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "NumberOfInsidePackagesDetails", {
        get: function () { return this.numberOfInsidePackagesDetails; },
        set: function (newValue) { if (this.numberOfInsidePackagesDetails != newValue) {
            this.numberOfInsidePackagesDetails = newValue;
            this.MarkAsDirty("NumberOfInsidePackagesDetails");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountManagerUserId", {
        get: function () { return this.accountManagerUserId; },
        set: function (newValue) { if (this.accountManagerUserId != newValue) {
            this.accountManagerUserId = newValue;
            this.MarkAsDirty("AccountManagerUserId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountManagerUserName", {
        get: function () { return this.accountManagerUserName; },
        set: function (newValue) { if (this.accountManagerUserName != newValue) {
            this.accountManagerUserName = newValue;
            this.MarkAsDirty("AccountManagerUserName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ManifestReason", {
        get: function () { return this.manifestReason; },
        set: function (newValue) { if (this.manifestReason != newValue) {
            this.manifestReason = newValue;
            this.MarkAsDirty("ManifestReason");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ManifestStatusCode", {
        get: function () { return this.manifestStatusCode; },
        set: function (newValue) { if (this.manifestStatusCode != newValue) {
            this.manifestStatusCode = newValue;
            this.MarkAsDirty("ManifestStatusCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsKnownCargo", {
        get: function () { return this.isKnownCargo; },
        set: function (newValue) { if (this.isKnownCargo != newValue) {
            this.isKnownCargo = newValue;
            this.MarkAsDirty("IsKnownCargo");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "RegulatedAgentRANumber", {
        get: function () { return this.regulatedAgentRANumber; },
        set: function (newValue) { if (this.regulatedAgentRANumber != newValue) {
            this.regulatedAgentRANumber = newValue;
            this.MarkAsDirty("RegulatedAgentRANumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "KnownConsignorNumber", {
        get: function () { return this.knownConsignorNumber; },
        set: function (newValue) { if (this.knownConsignorNumber != newValue) {
            this.knownConsignorNumber = newValue;
            this.MarkAsDirty("KnownConsignorNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "KCExpirationDate", {
        get: function () { return this.kCExpirationDate; },
        set: function (newValue) { if (this.kCExpirationDate != newValue) {
            this.kCExpirationDate = newValue;
            this.MarkAsDirty("KCExpirationDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ColoaderRANumber", {
        get: function () { return this.coloaderRANumber; },
        set: function (newValue) { if (this.coloaderRANumber != newValue) {
            this.coloaderRANumber = newValue;
            this.MarkAsDirty("ColoaderRANumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPrintingSecurityStatusId", {
        get: function () { return this.aWBPrintingSecurityStatusId; },
        set: function (newValue) { if (this.aWBPrintingSecurityStatusId != newValue) {
            this.aWBPrintingSecurityStatusId = newValue;
            this.MarkAsDirty("AWBPrintingSecurityStatusId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPrintingRANumber", {
        get: function () { return this.aWBPrintingRANumber; },
        set: function (newValue) { if (this.aWBPrintingRANumber != newValue) {
            this.aWBPrintingRANumber = newValue;
            this.MarkAsDirty("AWBPrintingRANumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AdditionalHandlingInfo", {
        get: function () { return this.additionalHandlingInfo; },
        set: function (newValue) { if (this.additionalHandlingInfo != newValue) {
            this.additionalHandlingInfo = newValue;
            this.MarkAsDirty("AdditionalHandlingInfo");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPrintingSecurityStatusEdited", {
        get: function () { return this.aWBPrintingSecurityStatusEdited; },
        set: function (newValue) { if (this.aWBPrintingSecurityStatusEdited != newValue) {
            this.aWBPrintingSecurityStatusEdited = newValue;
            this.MarkAsDirty("AWBPrintingSecurityStatusEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBPrintingRANumberEdited", {
        get: function () { return this.aWBPrintingRANumberEdited; },
        set: function (newValue) { if (this.aWBPrintingRANumberEdited != newValue) {
            this.aWBPrintingRANumberEdited = newValue;
            this.MarkAsDirty("AWBPrintingRANumberEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AdditionalHandlingInfoEdited", {
        get: function () { return this.additionalHandlingInfoEdited; },
        set: function (newValue) { if (this.additionalHandlingInfoEdited != newValue) {
            this.additionalHandlingInfoEdited = newValue;
            this.MarkAsDirty("AdditionalHandlingInfoEdited");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ViaColoader", {
        get: function () { return this.viaColoader; },
        set: function (newValue) { if (this.viaColoader != newValue) {
            this.viaColoader = newValue;
            this.MarkAsDirty("ViaColoader");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierReference1", {
        get: function () { return this.issuingCarrierReference1; },
        set: function (newValue) { if (this.issuingCarrierReference1 != newValue) {
            this.issuingCarrierReference1 = newValue;
            this.MarkAsDirty("IssuingCarrierReference1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsMissingDocument", {
        get: function () { return this.isMissingDocument; },
        set: function (newValue) { if (this.isMissingDocument != newValue) {
            this.isMissingDocument = newValue;
            this.MarkAsDirty("IsMissingDocument");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DocumentsSearchFields", {
        get: function () { return this.documentsSearchFields; },
        set: function (newValue) { if (this.documentsSearchFields != newValue) {
            this.documentsSearchFields = newValue;
            this.MarkAsDirty("DocumentsSearchFields");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "InterlineId", {
        get: function () { return this.interlineId; },
        set: function (newValue) { if (this.interlineId != newValue) {
            this.interlineId = newValue;
            this.MarkAsDirty("InterlineId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperAddress1", {
        get: function () { return this.shipperAddress1; },
        set: function (newValue) { if (this.shipperAddress1 != newValue) {
            this.shipperAddress1 = newValue;
            this.MarkAsDirty("ShipperAddress1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperAddress2", {
        get: function () { return this.shipperAddress2; },
        set: function (newValue) { if (this.shipperAddress2 != newValue) {
            this.shipperAddress2 = newValue;
            this.MarkAsDirty("ShipperAddress2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperZipCode", {
        get: function () { return this.shipperZipCode; },
        set: function (newValue) { if (this.shipperZipCode != newValue) {
            this.shipperZipCode = newValue;
            this.MarkAsDirty("ShipperZipCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperStateId", {
        get: function () { return this.shipperStateId; },
        set: function (newValue) { if (this.shipperStateId != newValue) {
            this.shipperStateId = newValue;
            this.MarkAsDirty("ShipperStateId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperCountryId", {
        get: function () { return this.shipperCountryId; },
        set: function (newValue) { if (this.shipperCountryId != newValue) {
            this.shipperCountryId = newValue;
            this.MarkAsDirty("ShipperCountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperCity", {
        get: function () { return this.shipperCity; },
        set: function (newValue) { if (this.shipperCity != newValue) {
            this.shipperCity = newValue;
            this.MarkAsDirty("ShipperCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperPhoneNumber", {
        get: function () { return this.shipperPhoneNumber; },
        set: function (newValue) { if (this.shipperPhoneNumber != newValue) {
            this.shipperPhoneNumber = newValue;
            this.MarkAsDirty("ShipperPhoneNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperFaxNumber", {
        get: function () { return this.shipperFaxNumber; },
        set: function (newValue) { if (this.shipperFaxNumber != newValue) {
            this.shipperFaxNumber = newValue;
            this.MarkAsDirty("ShipperFaxNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeAddress1", {
        get: function () { return this.consigneeAddress1; },
        set: function (newValue) { if (this.consigneeAddress1 != newValue) {
            this.consigneeAddress1 = newValue;
            this.MarkAsDirty("ConsigneeAddress1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeAddress2", {
        get: function () { return this.consigneeAddress2; },
        set: function (newValue) { if (this.consigneeAddress2 != newValue) {
            this.consigneeAddress2 = newValue;
            this.MarkAsDirty("ConsigneeAddress2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeZipCode", {
        get: function () { return this.consigneeZipCode; },
        set: function (newValue) { if (this.consigneeZipCode != newValue) {
            this.consigneeZipCode = newValue;
            this.MarkAsDirty("ConsigneeZipCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeStateId", {
        get: function () { return this.consigneeStateId; },
        set: function (newValue) { if (this.consigneeStateId != newValue) {
            this.consigneeStateId = newValue;
            this.MarkAsDirty("ConsigneeStateId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeCountryId", {
        get: function () { return this.consigneeCountryId; },
        set: function (newValue) { if (this.consigneeCountryId != newValue) {
            this.consigneeCountryId = newValue;
            this.MarkAsDirty("ConsigneeCountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeCity", {
        get: function () { return this.consigneeCity; },
        set: function (newValue) { if (this.consigneeCity != newValue) {
            this.consigneeCity = newValue;
            this.MarkAsDirty("ConsigneeCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneePhoneNumber", {
        get: function () { return this.consigneePhoneNumber; },
        set: function (newValue) { if (this.consigneePhoneNumber != newValue) {
            this.consigneePhoneNumber = newValue;
            this.MarkAsDirty("ConsigneePhoneNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeFaxNumber", {
        get: function () { return this.consigneeFaxNumber; },
        set: function (newValue) { if (this.consigneeFaxNumber != newValue) {
            this.consigneeFaxNumber = newValue;
            this.MarkAsDirty("ConsigneeFaxNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1Address1", {
        get: function () { return this.notify1Address1; },
        set: function (newValue) { if (this.notify1Address1 != newValue) {
            this.notify1Address1 = newValue;
            this.MarkAsDirty("Notify1Address1");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1Address2", {
        get: function () { return this.notify1Address2; },
        set: function (newValue) { if (this.notify1Address2 != newValue) {
            this.notify1Address2 = newValue;
            this.MarkAsDirty("Notify1Address2");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1ZipCode", {
        get: function () { return this.notify1ZipCode; },
        set: function (newValue) { if (this.notify1ZipCode != newValue) {
            this.notify1ZipCode = newValue;
            this.MarkAsDirty("Notify1ZipCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1StateId", {
        get: function () { return this.notify1StateId; },
        set: function (newValue) { if (this.notify1StateId != newValue) {
            this.notify1StateId = newValue;
            this.MarkAsDirty("Notify1StateId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1CountryId", {
        get: function () { return this.notify1CountryId; },
        set: function (newValue) { if (this.notify1CountryId != newValue) {
            this.notify1CountryId = newValue;
            this.MarkAsDirty("Notify1CountryId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1City", {
        get: function () { return this.notify1City; },
        set: function (newValue) { if (this.notify1City != newValue) {
            this.notify1City = newValue;
            this.MarkAsDirty("Notify1City");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1PhoneNumber", {
        get: function () { return this.notify1PhoneNumber; },
        set: function (newValue) { if (this.notify1PhoneNumber != newValue) {
            this.notify1PhoneNumber = newValue;
            this.MarkAsDirty("Notify1PhoneNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1FaxNumber", {
        get: function () { return this.notify1FaxNumber; },
        set: function (newValue) { if (this.notify1FaxNumber != newValue) {
            this.notify1FaxNumber = newValue;
            this.MarkAsDirty("Notify1FaxNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IssuingCarrierCity", {
        get: function () { return this.issuingCarrierCity; },
        set: function (newValue) { if (this.issuingCarrierCity != newValue) {
            this.issuingCarrierCity = newValue;
            this.MarkAsDirty("IssuingCarrierCity");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MAWBReturnedToStackWithCancel", {
        get: function () { return this.mAWBReturnedToStackWithCancel; },
        set: function (newValue) { if (this.mAWBReturnedToStackWithCancel != newValue) {
            this.mAWBReturnedToStackWithCancel = newValue;
            this.MarkAsDirty("MAWBReturnedToStackWithCancel");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MAWBStackAirlineId", {
        get: function () { return this.mAWBStackAirlineId; },
        set: function (newValue) { if (this.mAWBStackAirlineId != newValue) {
            this.mAWBStackAirlineId = newValue;
            this.MarkAsDirty("MAWBStackAirlineId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DontAddToImportersQueue", {
        get: function () { return this.dontAddToImportersQueue; },
        set: function (newValue) { if (this.dontAddToImportersQueue != newValue) {
            this.dontAddToImportersQueue = newValue;
            this.MarkAsDirty("DontAddToImportersQueue");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DontAddToForwarderQueue", {
        get: function () { return this.dontAddToForwarderQueue; },
        set: function (newValue) { if (this.dontAddToForwarderQueue != newValue) {
            this.dontAddToForwarderQueue = newValue;
            this.MarkAsDirty("DontAddToForwarderQueue");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ForwarderPartnerId", {
        get: function () { return this.forwarderPartnerId; },
        set: function (newValue) { if (this.forwarderPartnerId != newValue) {
            this.forwarderPartnerId = newValue;
            this.MarkAsDirty("ForwarderPartnerId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ForwardingPartnerId", {
        get: function () { return this.forwardingPartnerId; },
        set: function (newValue) { if (this.forwardingPartnerId != newValue) {
            this.forwardingPartnerId = newValue;
            this.MarkAsDirty("ForwardingPartnerId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OperationalCloseDate", {
        get: function () { return this.operationalCloseDate; },
        set: function (newValue) { if (this.operationalCloseDate != newValue) {
            this.operationalCloseDate = newValue;
            this.MarkAsDirty("OperationalCloseDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FirstOperationalCloseDate", {
        get: function () { return this.firstOperationalCloseDate; },
        set: function (newValue) { if (this.firstOperationalCloseDate != newValue) {
            this.firstOperationalCloseDate = newValue;
            this.MarkAsDirty("FirstOperationalCloseDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FirstAccountingCloseDate", {
        get: function () { return this.firstAccountingCloseDate; },
        set: function (newValue) { if (this.firstAccountingCloseDate != newValue) {
            this.firstAccountingCloseDate = newValue;
            this.MarkAsDirty("FirstAccountingCloseDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AccountingCloseDate", {
        get: function () { return this.accountingCloseDate; },
        set: function (newValue) { if (this.accountingCloseDate != newValue) {
            this.accountingCloseDate = newValue;
            this.MarkAsDirty("AccountingCloseDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FromCountryCode", {
        get: function () { return this.fromCountryCode; },
        set: function (newValue) { if (this.fromCountryCode != newValue) {
            this.fromCountryCode = newValue;
            this.MarkAsDirty("FromCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ToCountryCode", {
        get: function () { return this.toCountryCode; },
        set: function (newValue) { if (this.toCountryCode != newValue) {
            this.toCountryCode = newValue;
            this.MarkAsDirty("ToCountryCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConvertToCustomFile", {
        get: function () { return this.convertToCustomFile; },
        set: function (newValue) { if (this.convertToCustomFile != newValue) {
            this.convertToCustomFile = newValue;
            this.MarkAsDirty("ConvertToCustomFile");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "CustomerTenantNumber", {
        get: function () { return this.customerTenantNumber; },
        set: function (newValue) { if (this.customerTenantNumber != newValue) {
            this.customerTenantNumber = newValue;
            this.MarkAsDirty("CustomerTenantNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationETA", {
        get: function () { return this.mainCarriageFinalDestinationETA; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationETA != newValue) {
            this.mainCarriageFinalDestinationETA = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationETA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "MainCarriageFinalDestinationATA", {
        get: function () { return this.mainCarriageFinalDestinationATA; },
        set: function (newValue) { if (this.mainCarriageFinalDestinationATA != newValue) {
            this.mainCarriageFinalDestinationATA = newValue;
            this.MarkAsDirty("MainCarriageFinalDestinationATA");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DepartureArrivalFromDate", {
        get: function () { return this.departureArrivalFromDate; },
        set: function (newValue) { if (this.departureArrivalFromDate != newValue) {
            this.departureArrivalFromDate = newValue;
            this.MarkAsDirty("DepartureArrivalFromDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DepartureArrivalToDate", {
        get: function () { return this.departureArrivalToDate; },
        set: function (newValue) { if (this.departureArrivalToDate != newValue) {
            this.departureArrivalToDate = newValue;
            this.MarkAsDirty("DepartureArrivalToDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FirstPickupLocation", {
        get: function () { return this.firstPickupLocation; },
        set: function (newValue) { if (this.firstPickupLocation != newValue) {
            this.firstPickupLocation = newValue;
            this.MarkAsDirty("FirstPickupLocation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FinalDeliveryLocation", {
        get: function () { return this.finalDeliveryLocation; },
        set: function (newValue) { if (this.finalDeliveryLocation != newValue) {
            this.finalDeliveryLocation = newValue;
            this.MarkAsDirty("FinalDeliveryLocation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsImporterShipment", {
        get: function () { return this.isImporterShipment; },
        set: function (newValue) { if (this.isImporterShipment != newValue) {
            this.isImporterShipment = newValue;
            this.MarkAsDirty("IsImporterShipment");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsUpdatedByAnalyzer", {
        get: function () { return this.isUpdatedByAnalyzer; },
        set: function (newValue) { if (this.isUpdatedByAnalyzer != newValue) {
            this.isUpdatedByAnalyzer = newValue;
            this.MarkAsDirty("IsUpdatedByAnalyzer");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsCreatedFromCustomerOverview", {
        get: function () { return this.isCreatedFromCustomerOverview; },
        set: function (newValue) { if (this.isCreatedFromCustomerOverview != newValue) {
            this.isCreatedFromCustomerOverview = newValue;
            this.MarkAsDirty("IsCreatedFromCustomerOverview");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DeclarationXMLData", {
        get: function () { return this.declarationXMLData; },
        set: function (newValue) { if (this.declarationXMLData != newValue) {
            this.declarationXMLData = newValue;
            this.MarkAsDirty("DeclarationXMLData");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsImporterApprovalRequired", {
        get: function () { return this.isImporterApprovalRequired; },
        set: function (newValue) { if (this.isImporterApprovalRequired != newValue) {
            this.isImporterApprovalRequired = newValue;
            this.MarkAsDirty("IsImporterApprovalRequired");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DocsSentToAgent", {
        get: function () { return this.docsSentToAgent; },
        set: function (newValue) { if (this.docsSentToAgent != newValue) {
            this.docsSentToAgent = newValue;
            this.MarkAsDirty("DocsSentToAgent");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "VersionApproved", {
        get: function () { return this.versionApproved; },
        set: function (newValue) { if (this.versionApproved != newValue) {
            this.versionApproved = newValue;
            this.MarkAsDirty("VersionApproved");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ApproveDateTime", {
        get: function () { return this.approveDateTime; },
        set: function (newValue) { if (this.approveDateTime != newValue) {
            this.approveDateTime = newValue;
            this.MarkAsDirty("ApproveDateTime");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsNewARInvoiceBlocked", {
        get: function () { return this.isNewARInvoiceBlocked; },
        set: function (newValue) { if (this.isNewARInvoiceBlocked != newValue) {
            this.isNewARInvoiceBlocked = newValue;
            this.MarkAsDirty("IsNewARInvoiceBlocked");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentAddtionalDataXML", {
        get: function () { return this.shipmentAddtionalDataXML; },
        set: function (newValue) { if (this.shipmentAddtionalDataXML != newValue) {
            this.shipmentAddtionalDataXML = newValue;
            this.MarkAsDirty("ShipmentAddtionalDataXML");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OriginShipmentId", {
        get: function () { return this.originShipmentId; },
        set: function (newValue) { if (this.originShipmentId != newValue) {
            this.originShipmentId = newValue;
            this.MarkAsDirty("OriginShipmentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProrateReceivables", {
        get: function () { return this.prorateReceivables; },
        set: function (newValue) { if (this.prorateReceivables != newValue) {
            this.prorateReceivables = newValue;
            this.MarkAsDirty("ProrateReceivables");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightRelease", {
        get: function () { return this.freightRelease; },
        set: function (newValue) { if (this.freightRelease != newValue) {
            this.freightRelease = newValue;
            this.MarkAsDirty("FreightRelease");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "TerminalAvailable", {
        get: function () { return this.terminalAvailable; },
        set: function (newValue) { if (this.terminalAvailable != newValue) {
            this.terminalAvailable = newValue;
            this.MarkAsDirty("TerminalAvailable");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ISFNumber", {
        get: function () { return this.iSFNumber; },
        set: function (newValue) { if (this.iSFNumber != newValue) {
            this.iSFNumber = newValue;
            this.MarkAsDirty("ISFNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ISFDate", {
        get: function () { return this.iSFDate; },
        set: function (newValue) { if (this.iSFDate != newValue) {
            this.iSFDate = newValue;
            this.MarkAsDirty("ISFDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ITNumber", {
        get: function () { return this.iTNumber; },
        set: function (newValue) { if (this.iTNumber != newValue) {
            this.iTNumber = newValue;
            this.MarkAsDirty("ITNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ITDate", {
        get: function () { return this.iTDate; },
        set: function (newValue) { if (this.iTDate != newValue) {
            this.iTDate = newValue;
            this.MarkAsDirty("ITDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DocumentsClosingDate", {
        get: function () { return this.documentsClosingDate; },
        set: function (newValue) { if (this.documentsClosingDate != newValue) {
            this.documentsClosingDate = newValue;
            this.MarkAsDirty("DocumentsClosingDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OBLTypeCode", {
        get: function () { return this.oBLTypeCode; },
        set: function (newValue) { if (this.oBLTypeCode != newValue) {
            this.oBLTypeCode = newValue;
            this.MarkAsDirty("OBLTypeCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ENSNumber", {
        get: function () { return this.eNSNumber; },
        set: function (newValue) { if (this.eNSNumber != newValue) {
            this.eNSNumber = newValue;
            this.MarkAsDirty("ENSNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ENSDate", {
        get: function () { return this.eNSDate; },
        set: function (newValue) { if (this.eNSDate != newValue) {
            this.eNSDate = newValue;
            this.MarkAsDirty("ENSDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "RegistryDate", {
        get: function () { return this.registryDate; },
        set: function (newValue) { if (this.registryDate != newValue) {
            this.registryDate = newValue;
            this.MarkAsDirty("RegistryDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegExpectedEntryDate", {
        get: function () { return this.warehouseLegExpectedEntryDate; },
        set: function (newValue) { this.warehouseLegExpectedEntryDate = newValue; this.MarkAsDirty("WarehouseLegExpectedEntryDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegActualEntryDate", {
        get: function () { return this.warehouseLegActualEntryDate; },
        set: function (newValue) { this.warehouseLegActualEntryDate = newValue; this.MarkAsDirty("WarehouseLegActualEntryDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegExpectedReleaseDate", {
        get: function () { return this.warehouseLegExpectedReleaseDate; },
        set: function (newValue) { this.warehouseLegExpectedReleaseDate = newValue; this.MarkAsDirty("WarehouseLegExpectedReleaseDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegActualReleaseDate", {
        get: function () { return this.warehouseLegActualReleaseDate; },
        set: function (newValue) { this.warehouseLegActualReleaseDate = newValue; this.MarkAsDirty("WarehouseLegActualReleaseDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegLastFreeDate", {
        get: function () { return this.warehouseLegLastFreeDate; },
        set: function (newValue) { this.warehouseLegLastFreeDate = newValue; this.MarkAsDirty("WarehouseLegLastFreeDate"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegWarehouseId", {
        get: function () { return this.warehouseLegWarehouseId; },
        set: function (newValue) { this.warehouseLegWarehouseId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegAddressId", {
        get: function () { return this.warehouseLegAddressId; },
        set: function (newValue) { this.warehouseLegAddressId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegRemarks", {
        get: function () { return this.warehouseLegRemarks; },
        set: function (newValue) { this.warehouseLegRemarks = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegTerminalCode", {
        get: function () { return this.warehouseLegTerminalCode; },
        set: function (newValue) { this.warehouseLegTerminalCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegReference", {
        get: function () { return this.warehouseLegReference; },
        set: function (newValue) { this.warehouseLegReference = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegAddressCountryName", {
        get: function () { return this.warehouseLegAddressCountryName; },
        set: function (newValue) { this.warehouseLegAddressCountryName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegAddressCountryCode", {
        get: function () { return this.warehouseLegAddressCountryCode; },
        set: function (newValue) { this.warehouseLegAddressCountryCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsAssembly", {
        get: function () { return this.isAssembly; },
        set: function (newValue) { if (this.isAssembly != newValue) {
            this.isAssembly = newValue;
            this.MarkAsDirty("IsAssembly");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastSharedEventId", {
        get: function () { return this.lastSharedEventId; },
        set: function (newValue) { if (this.lastSharedEventId != newValue) {
            this.lastSharedEventId = newValue;
            this.MarkAsDirty("LastSharedEventId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastSharedEventLocation", {
        get: function () { return this.lastSharedEventLocation; },
        set: function (newValue) { if (this.lastSharedEventLocation != newValue) {
            this.lastSharedEventLocation = newValue;
            this.MarkAsDirty("LastSharedEventLocation");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastSharedEventNotes", {
        get: function () { return this.lastSharedEventNotes; },
        set: function (newValue) { if (this.lastSharedEventNotes != newValue) {
            this.lastSharedEventNotes = newValue;
            this.MarkAsDirty("LastSharedEventNotes");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastSharedEventDate", {
        get: function () { return this.lastSharedEventDate; },
        set: function (newValue) { if (this.lastSharedEventDate != newValue) {
            this.lastSharedEventDate = newValue;
            this.MarkAsDirty("LastSharedEventDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "LastSharedEventName", {
        get: function () { return this.lastSharedEventName; },
        set: function (newValue) { if (this.lastSharedEventName != newValue) {
            this.lastSharedEventName = newValue;
            this.MarkAsDirty("LastSharedEventName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "GrossWeightPerTon", {
        get: function () { return this.grossWeightPerTon; },
        set: function (newValue) { if (this.grossWeightPerTon != newValue) {
            this.grossWeightPerTon = newValue;
            this.MarkAsDirty("GrossWeightPerTon");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsRefreshFollowUp", {
        get: function () { return this.isRefreshFollowUp; },
        set: function (newValue) { if (this.isRefreshFollowUp != newValue) {
            this.isRefreshFollowUp = newValue;
            ;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "IsManifestSentToAgent", {
        get: function () { return this.isManifestSentToAgent; },
        set: function (newValue) { if (this.isManifestSentToAgent != newValue) {
            this.isManifestSentToAgent = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AgentSharedManifestRef", {
        get: function () { return this.agentSharedManifestRef; },
        set: function (newValue) { if (this.agentSharedManifestRef != newValue) {
            this.agentSharedManifestRef = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegTerminalName", {
        get: function () { return this.warehouseLegTerminalName; },
        set: function (newValue) { this.warehouseLegTerminalName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegEntryDate", {
        get: function () { return this.warehouseLegEntryDate; },
        set: function (newValue) { this.warehouseLegEntryDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegReleaseDate", {
        get: function () { return this.warehouseLegReleaseDate; },
        set: function (newValue) { this.warehouseLegReleaseDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ManifestLastSharingDate", {
        get: function () { return this.manifestLastSharingDate; },
        set: function (newValue) { if (this.manifestLastSharingDate != newValue) {
            this.manifestLastSharingDate = newValue;
            this.MarkAsDirty("ManifestLastSharingDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegCutOffDate", {
        get: function () { return this.warehouseLegCutOffDate; },
        set: function (newValue) { this.warehouseLegCutOffDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "WarehouseLegVGMCutOffDate", {
        get: function () { return this.warehouseLegVGMCutOffDate; },
        set: function (newValue) { this.warehouseLegVGMCutOffDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AMSClosingDate", {
        get: function () { return this.aMSClosingDate; },
        set: function (newValue) { this.aMSClosingDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "UpdatedByPartner", {
        get: function () { return this.updatedByPartner; },
        set: function (newValue) { this.updatedByPartner = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SendUpdatesToAgentEnabled", {
        get: function () { return this.sendUpdatesToAgentEnabled; },
        set: function (newValue) { if (this.sendUpdatesToAgentEnabled != newValue) {
            this.sendUpdatesToAgentEnabled = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "UpdateSendUpdatesToAgentEnabledField", {
        get: function () { return this.updateSendUpdatesToAgentEnabledField; },
        set: function (newValue) { if (this.updateSendUpdatesToAgentEnabledField != newValue) {
            this.updateSendUpdatesToAgentEnabledField = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRASIError", {
        get: function () { return this.iNTTRASIError; },
        set: function (newValue) { this.iNTTRASIError = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRASIStatusCode", {
        get: function () { return this.iNTTRASIStatusCode; },
        set: function (newValue) { this.iNTTRASIStatusCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRASIStatusName", {
        get: function () { return this.iNTTRASIStatusName; },
        set: function (newValue) { this.iNTTRASIStatusName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRASIStatusDate", {
        get: function () { return this.iNTTRASIStatusDate; },
        set: function (newValue) { this.iNTTRASIStatusDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "EmergencyContactId", {
        get: function () { return this.emergencyContactId; },
        set: function (newValue) { this.emergencyContactId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRAContractNumber", {
        get: function () { return this.iNTTRAContractNumber; },
        set: function (newValue) { this.iNTTRAContractNumber = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRAInstructions", {
        get: function () { return this.iNTTRAInstructions; },
        set: function (newValue) { this.iNTTRAInstructions = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRAComments", {
        get: function () { return this.iNTTRAComments; },
        set: function (newValue) { this.iNTTRAComments = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OnCarriageAdditionalTransportModeCode", {
        get: function () { return this.onCarriageAdditionalTransportModeCode; },
        set: function (newValue) { this.onCarriageAdditionalTransportModeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRADocumentQTY", {
        get: function () { return this.iNTTRADocumentQTY; },
        set: function (newValue) { this.iNTTRADocumentQTY = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SIHasAttachList", {
        get: function () { return this.sIHasAttachList; },
        set: function (newValue) { this.sIHasAttachList = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRAIsFreighted", {
        get: function () { return this.iNTTRAIsFreighted; },
        set: function (newValue) { this.iNTTRAIsFreighted = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRADocumentTypeCode", {
        get: function () { return this.iNTTRADocumentTypeCode; },
        set: function (newValue) { this.iNTTRADocumentTypeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "SplitOnCarriage", {
        get: function () { return this.splitOnCarriage; },
        set: function (newValue) { if (this.splitOnCarriage != newValue) {
            this.splitOnCarriage = newValue;
            this.MarkAsDirty();
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "INTTRALastStatusDate", {
        get: function () { return this.iNTTRALastStatusDate; },
        set: function (newValue) { if (this.iNTTRALastStatusDate != newValue) {
            this.iNTTRALastStatusDate = newValue;
            this.MarkAsDirty("INTTRALastStatusDate");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ProjectNumber", {
        get: function () { return this.projectNumber; },
        set: function (newValue) { if (this.projectNumber != newValue) {
            this.projectNumber = newValue;
            this.MarkAsDirty("ProjectNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify1Reference", {
        get: function () { return this.notify1Reference; },
        set: function (newValue) {
            if (this.notify1Reference != newValue) {
                this.notify1Reference = newValue;
                this.MarkAsDirty("Notify1Reference");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "Notify2Reference", {
        get: function () { return this.notify2Reference; },
        set: function (newValue) {
            if (this.notify2Reference != newValue) {
                this.notify2Reference = newValue;
                this.MarkAsDirty("Notify2Reference");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipperNotExporterReference", {
        get: function () { return this.shipperNotExporterReference; },
        set: function (newValue) {
            if (this.shipperNotExporterReference != newValue) {
                this.shipperNotExporterReference = newValue;
                this.MarkAsDirty("ShipperNotExporterReference");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConsigneeNotImporterReference", {
        get: function () { return this.consigneeNotImporterReference; },
        set: function (newValue) {
            if (this.consigneeNotImporterReference != newValue) {
                this.consigneeNotImporterReference = newValue;
                this.MarkAsDirty("ConsigneeNotImporterReference");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "BasicFreightId", {
        get: function () { return this.basicFreightId; },
        set: function (newValue) {
            if (this.basicFreightId != newValue) {
                this.basicFreightId = newValue;
                this.MarkAsDirty("BasicFreightId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DestinationPortChargesId", {
        get: function () { return this.destinationPortChargesId; },
        set: function (newValue) {
            if (this.destinationPortChargesId != newValue) {
                this.destinationPortChargesId = newValue;
                this.MarkAsDirty("DestinationPortChargesId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "DestinationHaulageChargesId", {
        get: function () { return this.destinationHaulageChargesId; },
        set: function (newValue) {
            if (this.destinationHaulageChargesId != newValue) {
                this.destinationHaulageChargesId = newValue;
                this.MarkAsDirty("DestinationHaulageChargesId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AdditionalChargesId", {
        get: function () { return this.additionalChargesId; },
        set: function (newValue) {
            if (this.additionalChargesId != newValue) {
                this.additionalChargesId = newValue;
                this.MarkAsDirty("AdditionalChargesId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightPayerId", {
        get: function () { return this.freightPayerId; },
        set: function (newValue) {
            if (this.freightPayerId != newValue) {
                this.freightPayerId = newValue;
                this.MarkAsDirty("FreightPayerId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "FreightPayerAddressId", {
        get: function () { return this.freightPayerAddressId; },
        set: function (newValue) {
            if (this.freightPayerAddressId != newValue) {
                this.freightPayerAddressId = newValue;
                this.MarkAsDirty("FreightPayerAddressId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "HasContainerException", {
        get: function () { return this.hasContainerException; },
        set: function (newValue) {
            if (this.hasContainerException != newValue) {
                this.hasContainerException = newValue;
                this.MarkAsDirty("HasContainerException");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OriginMainCarriageFromPortId", {
        get: function () { return this.originMainCarriageFromPortId; },
        set: function (newValue) {
            if (this.originMainCarriageFromPortId != newValue) {
                this.originMainCarriageFromPortId = newValue;
                this.MarkAsDirty("OriginMainCarriageFromPortId");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "OriginFinalDestinationPortId", {
        get: function () { return this.originFinalDestinationPortId; },
        set: function (newValue) {
            if (this.originFinalDestinationPortId != newValue) {
                this.originFinalDestinationPortId = newValue;
                this.MarkAsDirty("HasContainerException");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ARInvoices", {
        get: function () { return this.aRInvoices; },
        set: function (newValue) {
            if (this.aRInvoices != newValue) {
                this.aRInvoices = newValue;
                this.MarkAsDirty("ARInvoices");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConvertShipmentToLCL", {
        get: function () { return this.convertShipmentToLCL; },
        set: function (newValue) { if (this.convertShipmentToLCL != newValue) {
            this.convertShipmentToLCL = newValue;
            this.MarkAsDirty("ConvertShipmentToLCL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ConvertShipmentToFCL", {
        get: function () { return this.convertShipmentToFCL; },
        set: function (newValue) { if (this.convertShipmentToFCL != newValue) {
            this.convertShipmentToFCL = newValue;
            this.MarkAsDirty("ConvertShipmentToFCL");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "ShipmentDirectionConverted", {
        get: function () { return this.shipmentDirectionConverted; },
        set: function (newValue) { if (this.shipmentDirectionConverted != newValue) {
            this.shipmentDirectionConverted = newValue;
            this.MarkAsDirty("ShipmentDirectionConverted");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "PackagesDeleted", {
        get: function () { return this.packagesDeleted; },
        set: function (newValue) { if (this.packagesDeleted != newValue) {
            this.packagesDeleted = newValue;
            this.MarkAsDirty("PackagesDeleted");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPM.prototype, "AWBOCIPMs", {
        get: function () {
            if (this.aWBOCIPMs == null) {
                this.aWBOCIPMs = [];
            }
            return this.aWBOCIPMs;
        },
        set: function (newValue) {
            if (this.aWBOCIPMs != newValue) {
                this.aWBOCIPMs = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddOCI = function (item) {
        if (item != null) {
            var index = this.AWBOCIPMs.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.AWBOCIPMs.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveOCI = function (item) {
        if (item != null) {
            var index = this.AWBOCIPMs.indexOf(item);
            if (index > -1) {
                this.AWBOCIPMs.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPackages", {
        get: function () {
            if (this.shipmentPackages == null) {
                this.shipmentPackages = [];
            }
            return this.shipmentPackages;
        },
        set: function (newValue) {
            if (this.shipmentPackages != newValue) {
                this.shipmentPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddPackage = function (item) {
        if (item != null) {
            var index = this.ShipmentPackages.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPackages.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemovePackage = function (item) {
        if (item != null) {
            var index = this.ShipmentPackages.indexOf(item);
            if (index > -1) {
                this.ShipmentPackages.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentCommodities", {
        get: function () {
            if (this.shipmentCommodities == null) {
                this.shipmentCommodities = [];
            }
            return this.shipmentCommodities;
        },
        set: function (newValue) {
            if (this.shipmentCommodities != newValue) {
                this.shipmentCommodities = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddCommodity = function (item) {
        if (item != null) {
            var index = this.ShipmentCommodities.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentCommodities.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveCommodity = function (item) {
        if (item != null) {
            var index = this.ShipmentCommodities.indexOf(item);
            if (index > -1) {
                this.ShipmentCommodities.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentOrderPackages", {
        get: function () {
            if (this.shipmentOrderPackages == null) {
                this.shipmentOrderPackages = [];
            }
            return this.shipmentOrderPackages;
        },
        set: function (newValue) {
            if (this.shipmentOrderPackages != newValue) {
                this.shipmentOrderPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddOrderPackage = function (item) {
        if (item != null) {
            var index = this.ShipmentOrderPackages.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentOrderPackages.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveOrderPackage = function (item) {
        if (item != null) {
            var index = this.ShipmentOrderPackages.indexOf(item);
            if (index > -1) {
                this.ShipmentOrderPackages.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPayables", {
        get: function () {
            if (this.shipmentPayables == null) {
                this.shipmentPayables = [];
            }
            return this.shipmentPayables;
        },
        set: function (newValue) {
            if (this.shipmentPayables != newValue) {
                this.shipmentPayables = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddPayable = function (item) {
        if (item != null) {
            var index = this.ShipmentPayables.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPayables.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemovePayable = function (item) {
        if (item != null) {
            var index = this.ShipmentPayables.indexOf(item);
            if (index > -1) {
                this.ShipmentPayables.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentReceivables", {
        get: function () {
            if (this.shipmentReceivables == null) {
                this.shipmentReceivables = [];
            }
            return this.shipmentReceivables;
        },
        set: function (newValue) {
            if (this.shipmentReceivables != newValue) {
                this.shipmentReceivables = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddReceivable = function (item) {
        if (item != null) {
            var index = this.ShipmentReceivables.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentReceivables.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveReceivable = function (item) {
        if (item != null) {
            var index = this.ShipmentReceivables.indexOf(item);
            if (index > -1) {
                this.ShipmentReceivables.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentAWBPrintOnlies", {
        get: function () {
            if (this.shipmentAWBPrintOnlies == null) {
                this.shipmentAWBPrintOnlies = [];
            }
            return this.shipmentAWBPrintOnlies;
        },
        set: function (newValue) {
            if (this.shipmentAWBPrintOnlies != newValue) {
                this.shipmentAWBPrintOnlies = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddAWBPrintOnly = function (item) {
        if (item != null) {
            var index = this.ShipmentAWBPrintOnlies.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentAWBPrintOnlies.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveAWBPrintOnly = function (item) {
        if (item != null) {
            var index = this.ShipmentAWBPrintOnlies.indexOf(item);
            if (index > -1) {
                this.ShipmentAWBPrintOnlies.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentConsoleShipments", {
        get: function () {
            if (this.shipmentConsoleShipments == null) {
                this.shipmentConsoleShipments = [];
            }
            return this.shipmentConsoleShipments;
        },
        set: function (newValue) {
            if (this.shipmentConsoleShipments != newValue) {
                this.shipmentConsoleShipments = newValue;
                //this.MarkAsDirty();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddConsoleShipment = function (item) {
        if (item != null) {
            var index = this.ShipmentConsoleShipments.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentConsoleShipments.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveConsoleShipment = function (item) {
        if (item != null) {
            var index = this.ShipmentConsoleShipments.indexOf(item);
            if (index > -1) {
                this.ShipmentConsoleShipments.splice(index, 1);
                this.MasterShipmentDataId = null;
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentPickUps", {
        get: function () {
            if (this.shipmentPickUps == null) {
                this.shipmentPickUps = [];
            }
            return this.shipmentPickUps;
        },
        set: function (newValue) {
            if (this.shipmentPickUps != newValue) {
                this.shipmentPickUps = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddPickUp = function (item) {
        if (item != null) {
            var index = this.ShipmentPickUps.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPickUps.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemovePickUp = function (item) {
        if (item != null) {
            var index = this.ShipmentPickUps.indexOf(item);
            if (index > -1) {
                this.ShipmentPickUps.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentDeliveries", {
        get: function () {
            if (this.shipmentDeliveries == null) {
                this.shipmentDeliveries = [];
            }
            return this.shipmentDeliveries;
        },
        set: function (newValue) {
            if (this.shipmentDeliveries != newValue) {
                this.shipmentDeliveries = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddDelivery = function (item) {
        if (item != null) {
            var index = this.ShipmentDeliveries.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentDeliveries.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveDelivery = function (item) {
        if (item != null) {
            var index = this.ShipmentDeliveries.indexOf(item);
            if (index > -1) {
                this.ShipmentDeliveries.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "FollowUps", {
        get: function () {
            if (this.followUps == null) {
                this.followUps = [];
            }
            return this.followUps;
        },
        set: function (newValue) {
            if (this.followUps != newValue) {
                this.followUps = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddShipmentFollowUp = function (item) {
        if (item != null) {
            var index = this.FollowUps.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.FollowUps.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveShipmentFollowUp = function (item) {
        if (item != null) {
            var index = this.FollowUps.indexOf(item);
            if (index > -1) {
                this.FollowUps.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    Object.defineProperty(ShipmentPM.prototype, "ShipmentAssemblies", {
        get: function () {
            if (this.shipmentAssemblies == null) {
                this.shipmentAssemblies = [];
            }
            return this.shipmentAssemblies;
        },
        set: function (newValue) {
            if (this.shipmentAssemblies != newValue) {
                this.shipmentAssemblies = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPM.prototype.AddAssembly = function (item) {
        if (item != null) {
            var index = this.ShipmentAssemblies.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentAssemblies.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.RemoveAssembly = function (item) {
        if (item != null) {
            var index = this.ShipmentAssemblies.indexOf(item);
            if (index > -1) {
                this.ShipmentAssemblies.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    ShipmentPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ShipmentPMCustomCode_1.ShipmentPMCustomCode.ApplyEntityChanged(propertyName, this);
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Shipment");
        }
    };
    ShipmentPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ShipmentPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ShipmentPM.prototype, "PropertyChanged", void 0);
    return ShipmentPM;
}());
exports.ShipmentPM = ShipmentPM;
//# sourceMappingURL=ShipmentPM.js.map