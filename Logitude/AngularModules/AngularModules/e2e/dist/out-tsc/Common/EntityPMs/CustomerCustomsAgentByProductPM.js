"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerCustomsAgentByProductPM = /** @class */ (function () {
    function CustomerCustomsAgentByProductPM(entityParentPM) {
        this.EntityParentPM = entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (value) { this.productTypeCode = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "CustomsAgentId", {
        get: function () { return this.customsAgentId; },
        set: function (value) { this.customsAgentId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) { this.customerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCustomsAgentByProductPM.prototype, "CustomsAgentName", {
        get: function () { return this.customsAgentName; },
        set: function (value) { this.customsAgentName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerCustomsAgentByProductPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return CustomerCustomsAgentByProductPM;
}());
exports.CustomerCustomsAgentByProductPM = CustomerCustomsAgentByProductPM;
//# sourceMappingURL=CustomerCustomsAgentByProductPM.js.map