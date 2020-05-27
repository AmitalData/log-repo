"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerSalesmanByProductPM = /** @class */ (function () {
    function CustomerSalesmanByProductPM(entityParentPM) {
        3;
        this.EntityParentPM = entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (value) { this.productTypeCode = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "SalesmanUserId", {
        get: function () { return this.salesmanUserId; },
        set: function (value) { this.salesmanUserId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) { this.customerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesmanByProductPM.prototype, "SalesmanUserName", {
        get: function () { return this.salesmanUserName; },
        set: function (value) { this.salesmanUserName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerSalesmanByProductPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return CustomerSalesmanByProductPM;
}());
exports.CustomerSalesmanByProductPM = CustomerSalesmanByProductPM;
//# sourceMappingURL=CustomerSalesmanByProductPM.js.map