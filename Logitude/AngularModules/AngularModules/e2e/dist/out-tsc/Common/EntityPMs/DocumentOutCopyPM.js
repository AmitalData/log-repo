"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var DocumentOutCopyPM = /** @class */ (function () {
    function DocumentOutCopyPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(DocumentOutCopyPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "DocumentId", {
        get: function () { return this.documentId; },
        set: function (newValue) { this.documentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "DocumentOutId", {
        get: function () { return this.documentOutId; },
        set: function (newValue) { this.documentOutId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "DocumentTypeCopyId", {
        get: function () { return this.documentTypeCopyId; },
        set: function (newValue) { this.documentTypeCopyId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "DocoumentTypeCopyName", {
        get: function () { return this.docoumentTypeCopyName; },
        set: function (newValue) { this.docoumentTypeCopyName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "DocumentTypeCopyNameWithDocumentTypeName", {
        get: function () { return this.documentTypeCopyNameWithDocumentTypeName; },
        set: function (newValue) { this.documentTypeCopyNameWithDocumentTypeName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "LastPrintedByUserId", {
        get: function () { return this.lastPrintedByUserId; },
        set: function (newValue) { this.lastPrintedByUserId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "LastPrintDate", {
        get: function () { return this.lastPrintDate; },
        set: function (newValue) { this.lastPrintDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "FileSize", {
        get: function () { return this.fileSize; },
        set: function (newValue) { this.fileSize = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "FileName", {
        get: function () { return this.fileName; },
        set: function (newValue) { this.fileName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "LastPrintedByUserName", {
        get: function () { return this.lastPrintedByUserName; },
        set: function (newValue) { this.lastPrintedByUserName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "IsAttachSelect", {
        get: function () { return this.isAttachSelect; },
        set: function (newValue) { this.isAttachSelect = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutCopyPM.prototype, "CalculatedFileName", {
        get: function () { return this.calculatedFileName; },
        set: function (newValue) { this.calculatedFileName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    DocumentOutCopyPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return DocumentOutCopyPM;
}());
exports.DocumentOutCopyPM = DocumentOutCopyPM;
//# sourceMappingURL=DocumentOutCopyPM.js.map