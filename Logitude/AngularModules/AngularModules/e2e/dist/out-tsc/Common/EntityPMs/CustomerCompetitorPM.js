"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var CustomerCompetitorPM = /** @class */ (function () {
    function CustomerCompetitorPM(entityParentPM) {
        this.EntityParentPM = entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(CustomerCompetitorPM.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) { this.customerId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorId", {
        get: function () { return this.competitorId; },
        set: function (value) { this.competitorId = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CustomerName", {
        get: function () { return this.customerName; },
        set: function (value) { this.customerName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorName", {
        get: function () { return this.competitorName; },
        set: function (value) { this.competitorName = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorWebsite", {
        get: function () { return this.competitorWebsite; },
        set: function (value) { this.competitorWebsite = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorStrengths", {
        get: function () { return this.competitorStrengths; },
        set: function (value) { this.competitorStrengths = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorWeaknesses", {
        get: function () { return this.competitorWeaknesses; },
        set: function (value) { this.competitorWeaknesses = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorOpportunity", {
        get: function () { return this.competitorOpportunity; },
        set: function (value) { this.competitorOpportunity = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "CompetitorThreat", {
        get: function () { return this.competitorThreat; },
        set: function (value) { this.competitorThreat = value; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerCompetitorPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    CustomerCompetitorPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    return CustomerCompetitorPM;
}());
exports.CustomerCompetitorPM = CustomerCompetitorPM;
//# sourceMappingURL=CustomerCompetitorPM.js.map