"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var UserPermittedProductPM = /** @class */ (function () {
    function UserPermittedProductPM(entityParentPM) {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(UserPermittedProductPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermittedProductPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermittedProductPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermittedProductPM.prototype, "UserId", {
        get: function () { return this.userId; },
        set: function (newValue) { this.userId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermittedProductPM.prototype, "ProductTypeCode", {
        get: function () { return this.productTypeCode; },
        set: function (newValue) { this.productTypeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPermittedProductPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    UserPermittedProductPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return UserPermittedProductPM;
}());
exports.UserPermittedProductPM = UserPermittedProductPM;
//# sourceMappingURL=UserPermittedProductPM.js.map