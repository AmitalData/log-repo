"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var DocumentsFilingMetaDataValuePM = /** @class */ (function () {
    function DocumentsFilingMetaDataValuePM(entityParentPM) {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "EntityParentPM", {
        get: function () { return this.entityParentPM; },
        set: function (newValue) { this.entityParentPM = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "DocumentsFilingId", {
        get: function () { return this.documentsFilingId; },
        set: function (newValue) { this.documentsFilingId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "DocumentsMetaDataTypeId", {
        get: function () { return this.documentsMetaDataTypeId; },
        set: function (newValue) { this.documentsMetaDataTypeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "DocumentsMetaDataTypeCode", {
        get: function () { return this.documentsMetaDataTypeCode; },
        set: function (newValue) { this.documentsMetaDataTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsFilingMetaDataValuePM.prototype, "MetaDataValue", {
        get: function () { return this.metaDataValue; },
        set: function (newValue) { this.metaDataValue = newValue; },
        enumerable: true,
        configurable: true
    });
    DocumentsFilingMetaDataValuePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    };
    return DocumentsFilingMetaDataValuePM;
}());
exports.DocumentsFilingMetaDataValuePM = DocumentsFilingMetaDataValuePM;
//# sourceMappingURL=DocumentsFilingMetaDataValuePM.js.map