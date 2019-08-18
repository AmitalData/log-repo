"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerForwarderByProductPM = /** @class */ (function () {
    function CustomerForwarderByProductPM(entityParentPM) {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (value) { this.productTypeCode = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "ForwarderId", {
        get: function () { return this.forwarderId; },
        set: function (value) { this.forwarderId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) { this.customerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerForwarderByProductPM.prototype, "ForwarderName", {
        get: function () { return this.forwarderName; },
        set: function (value) { this.forwarderName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerForwarderByProductPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    return CustomerForwarderByProductPM;
}());
exports.CustomerForwarderByProductPM = CustomerForwarderByProductPM;
//# sourceMappingURL=CustomerForwarderByProductPM.js.map