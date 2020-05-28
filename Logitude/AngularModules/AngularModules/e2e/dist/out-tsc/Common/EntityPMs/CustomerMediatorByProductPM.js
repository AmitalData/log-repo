"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerMediatorByProductPM = /** @class */ (function () {
    function CustomerMediatorByProductPM(entityParentPM) {
        this.EntityParentPM = entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (value) { this.productTypeCode = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "MediatorId", {
        get: function () { return this.mediatorId; },
        set: function (value) { this.mediatorId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) { this.customerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerMediatorByProductPM.prototype, "MediatorName", {
        get: function () { return this.mediatorName; },
        set: function (value) { this.mediatorName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerMediatorByProductPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return CustomerMediatorByProductPM;
}());
exports.CustomerMediatorByProductPM = CustomerMediatorByProductPM;
//# sourceMappingURL=CustomerMediatorByProductPM.js.map