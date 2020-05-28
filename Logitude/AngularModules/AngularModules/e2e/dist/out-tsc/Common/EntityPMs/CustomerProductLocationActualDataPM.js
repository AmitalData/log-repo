"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerProductLocationActualDataPM = /** @class */ (function () {
    function CustomerProductLocationActualDataPM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "CountryName", {
        get: function () { return this.countryName; },
        set: function (newValue) { this.countryName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "CountryCode", {
        get: function () { return this.countryCode; },
        set: function (newValue) { this.countryCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "Month", {
        get: function () { return this.month; },
        set: function (newValue) { this.month = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "CountryId", {
        get: function () { return this.countryId; },
        set: function (newValue) { this.countryId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "ChargeableWeight", {
        get: function () { return this.chargeableWeight; },
        set: function (newValue) { this.chargeableWeight = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "Revenue", {
        get: function () { return this.revenue; },
        set: function (newValue) { this.revenue = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "NumberOfShipments", {
        get: function () { return this.numberOfShipments; },
        set: function (newValue) { this.numberOfShipments = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (newValue) { this.customerId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "Year", {
        get: function () { return this.year; },
        set: function (newValue) { this.year = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerProductLocationActualDataPM.prototype, "TEU", {
        get: function () { return this.tEU; },
        set: function (newValue) { this.tEU = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerProductLocationActualDataPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return CustomerProductLocationActualDataPM;
}());
exports.CustomerProductLocationActualDataPM = CustomerProductLocationActualDataPM;
//# sourceMappingURL=CustomerProductLocationActualDataPM.js.map