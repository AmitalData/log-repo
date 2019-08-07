"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var DocumentFilingMetaDataValuePM = /** @class */ (function () {
    function DocumentFilingMetaDataValuePM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(DocumentFilingMetaDataValuePM.prototype, "ChangeSetOp", {
        get: function () { return this.changeSetOp; },
        set: function (newValue) { this.changeSetOp = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingMetaDataValuePM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingMetaDataValuePM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingMetaDataValuePM.prototype, "DocumentsFilingId", {
        get: function () { return this.documentsFilingId; },
        set: function (newValue) { this.documentsFilingId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingMetaDataValuePM.prototype, "DocumentsMetaDataTypeId", {
        get: function () { return this.documentsMetaDataTypeId; },
        set: function (newValue) { this.documentsMetaDataTypeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingMetaDataValuePM.prototype, "MetaDataValue", {
        get: function () { return this.metaDataValue; },
        set: function (newValue) { this.metaDataValue = newValue; },
        enumerable: true,
        configurable: true
    });
    return DocumentFilingMetaDataValuePM;
}());
exports.DocumentFilingMetaDataValuePM = DocumentFilingMetaDataValuePM;
//# sourceMappingURL=DocumentFilingMetaDataValuePM.js.map