"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerAccountManagerByProductPM = /** @class */ (function () {
    function CustomerAccountManagerByProductPM(entityParentPM) {
        this.EntityParentPM = entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (value) { this.productTypeCode = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "AccountManagerId", {
        get: function () { return this.accountManagerId; },
        set: function (value) { this.accountManagerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) { this.customerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAccountManagerByProductPM.prototype, "AccountManagerName", {
        get: function () { return this.accountManagerName; },
        set: function (value) { this.accountManagerName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerAccountManagerByProductPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return CustomerAccountManagerByProductPM;
}());
exports.CustomerAccountManagerByProductPM = CustomerAccountManagerByProductPM;
//# sourceMappingURL=CustomerAccountManagerByProductPM.js.map