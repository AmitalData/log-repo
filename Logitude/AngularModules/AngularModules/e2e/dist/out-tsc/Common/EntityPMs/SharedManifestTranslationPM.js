"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var SharedManifestTranslationPM = /** @class */ (function () {
    function SharedManifestTranslationPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(SharedManifestTranslationPM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "ObjectTableName", {
        get: function () { return this.objectTableName; },
        set: function (newValue) { this.objectTableName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "MyCode", {
        get: function () { return this.myCode; },
        set: function (newValue) { this.myCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "AgentCode", {
        get: function () { return this.agentCode; },
        set: function (newValue) { this.agentCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "AgentId", {
        get: function () { return this.agentId; },
        set: function (newValue) { this.agentId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "CreatedByUserId", {
        get: function () { return this.createdByUserId; },
        set: function (newValue) { this.createdByUserId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "CreateDate", {
        get: function () { return this.createDate; },
        set: function (newValue) { this.createDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "UpdatedByUserId", {
        get: function () { return this.updatedByUserId; },
        set: function (newValue) { this.updatedByUserId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestTranslationPM.prototype, "UpdateDate", {
        get: function () { return this.updateDate; },
        set: function (newValue) { this.updateDate = newValue; },
        enumerable: true,
        configurable: true
    });
    SharedManifestTranslationPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return SharedManifestTranslationPM;
}());
exports.SharedManifestTranslationPM = SharedManifestTranslationPM;
//# sourceMappingURL=SharedManifestTranslationPM.js.map