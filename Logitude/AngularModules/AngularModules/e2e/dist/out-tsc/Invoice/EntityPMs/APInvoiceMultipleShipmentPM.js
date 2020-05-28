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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var PropertyChangedArgs_1 = require("../../Infrastructure/EventEmitterArgs/PropertyChangedArgs");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var APInvoiceMultipleShipmentPM = /** @class */ (function () {
    function APInvoiceMultipleShipmentPM(_entityParentPM) {
        this.PropertyChanged = new core_1.EventEmitter();
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { if (this.tenant != newValue) {
            this.tenant = newValue;
            this.MarkAsDirty("Tenant");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "APInvoiceId", {
        get: function () { return this.aPInvoiceId; },
        set: function (newValue) { if (this.aPInvoiceId != newValue) {
            this.aPInvoiceId = newValue;
            this.MarkAsDirty("APInvoiceId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "ShipmentId", {
        get: function () { return this.shipmentId; },
        set: function (newValue) { if (this.shipmentId != newValue) {
            this.shipmentId = newValue;
            this.MarkAsDirty("ShipmentId");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "ShipmentNumber", {
        get: function () { return this.shipmentNumber; },
        set: function (newValue) { if (this.shipmentNumber != newValue) {
            this.shipmentNumber = newValue;
            this.MarkAsDirty("ShipmentNumber");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "ShipmentLevelCode", {
        get: function () { return this.shipmentLevelCode; },
        set: function (newValue) { if (this.shipmentLevelCode != newValue) {
            this.shipmentLevelCode = newValue;
            this.MarkAsDirty("ShipmentLevelCode");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "House", {
        get: function () { return this.house; },
        set: function (newValue) { if (this.house != newValue) {
            this.house = newValue;
            this.MarkAsDirty("House");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "Master", {
        get: function () { return this.master; },
        set: function (newValue) { if (this.master != newValue) {
            this.master = newValue;
            this.MarkAsDirty("Master");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "LongMaster", {
        get: function () { return this.longMaster; },
        set: function (newValue) { if (this.longMaster != newValue) {
            this.longMaster = newValue;
            this.MarkAsDirty("LongMaster");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "MainCarriageCarrierName", {
        get: function () { return this.mainCarriageCarrierName; },
        set: function (newValue) { if (this.mainCarriageCarrierName != newValue) {
            this.mainCarriageCarrierName = newValue;
            this.MarkAsDirty("MainCarriageCarrierName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "PartnerType", {
        get: function () { return this.partnerType; },
        set: function (newValue) { if (this.partnerType != newValue) {
            this.partnerType = newValue;
            this.MarkAsDirty("PartnerType");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "PartnerName", {
        get: function () { return this.partnerName; },
        set: function (newValue) { if (this.partnerName != newValue) {
            this.partnerName = newValue;
            this.MarkAsDirty("PartnerName");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.subTotalInLocalCurrency; },
        set: function (newValue) { if (this.subTotalInLocalCurrency != newValue) {
            this.subTotalInLocalCurrency = newValue;
            this.MarkAsDirty("SubTotalInLocalCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.subTotalInInvoiceCurrency; },
        set: function (newValue) { if (this.subTotalInInvoiceCurrency != newValue) {
            this.subTotalInInvoiceCurrency = newValue;
            this.MarkAsDirty("SubTotalInInvoiceCurrency");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "ExpectedAmount", {
        get: function () { return this.expectedAmount; },
        set: function (newValue) { if (this.expectedAmount != newValue) {
            this.expectedAmount = newValue;
            this.MarkAsDirty("ExpectedAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "AccountedAmount", {
        get: function () { return this.accountedAmount; },
        set: function (newValue) { if (this.accountedAmount != newValue) {
            this.accountedAmount = newValue;
            this.MarkAsDirty("AccountedAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "OpenAmount", {
        get: function () { return this.openAmount; },
        set: function (newValue) { if (this.openAmount != newValue) {
            this.openAmount = newValue;
            this.MarkAsDirty("OpenAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "TotalAmount", {
        get: function () { return this.totalAmount; },
        set: function (newValue) { if (this.totalAmount != newValue) {
            this.totalAmount = newValue;
            this.MarkAsDirty("TotalAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "TotalVATAmount", {
        get: function () { return this.totalVATAmount; },
        set: function (newValue) { if (this.totalVATAmount != newValue) {
            this.totalVATAmount = newValue;
            this.MarkAsDirty("TotalVATAmount");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { if (this.indexOrder != newValue) {
            this.indexOrder = newValue;
            this.MarkAsDirty("IndexOrder");
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "TotalVatsList", {
        get: function () {
            if (this.totalVatsList == null) {
                this.totalVatsList = [];
            }
            return this.totalVatsList;
        },
        set: function (newValue) {
            if (this.totalVatsList != newValue) {
                this.totalVatsList = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleShipmentPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    APInvoiceMultipleShipmentPM.prototype.MarkAsDirty = function (propertyName) {
        if (propertyName === void 0) { propertyName = null; }
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs_1.PropertyChangedArgs(propertyName, this));
            ServiceLocator_1.ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "APInvoiceMultipleShipment");
        }
    };
    APInvoiceMultipleShipmentPM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    APInvoiceMultipleShipmentPM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], APInvoiceMultipleShipmentPM.prototype, "PropertyChanged", void 0);
    return APInvoiceMultipleShipmentPM;
}());
exports.APInvoiceMultipleShipmentPM = APInvoiceMultipleShipmentPM;
//# sourceMappingURL=APInvoiceMultipleShipmentPM.js.map