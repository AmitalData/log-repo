"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ObjectFieldValidationPM = /** @class */ (function () {
    function ObjectFieldValidationPM(entityParentPM) {
        this.EntityParentPM = entityParentPM;
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(ObjectFieldValidationPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldValidationPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldValidationPM.prototype, "SearchFields", {
        get: function () { return this.searchFields; },
        set: function (newValue) { this.searchFields = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldValidationPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldValidationPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; },
        enumerable: true,
        configurable: true
    });
    ObjectFieldValidationPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return ObjectFieldValidationPM;
}());
exports.ObjectFieldValidationPM = ObjectFieldValidationPM;
//# sourceMappingURL=ObjectFieldValidationPM.js.map