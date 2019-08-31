"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var UserRolesPM = /** @class */ (function () {
    function UserRolesPM(_entityParentPM) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(UserRolesPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "Exists", {
        get: function () { return this.exists; },
        set: function (newValue) { this.exists = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "Added", {
        get: function () { return this.added; },
        set: function (newValue) { this.added = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesPM.prototype, "Removed", {
        get: function () { return this.removed; },
        set: function (newValue) { this.removed = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    UserRolesPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
    };
    return UserRolesPM;
}());
exports.UserRolesPM = UserRolesPM;
//# sourceMappingURL=UserRolesPM.js.map