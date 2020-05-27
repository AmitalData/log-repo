"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var DocumentOutPM = /** @class */ (function () {
    function DocumentOutPM() {
        this.UIProperties = new UIProperties_1.UIProperties;
        this.IsDirty = false;
    }
    Object.defineProperty(DocumentOutPM.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "EntityId", {
        get: function () { return this.entityId; },
        set: function (newValue) { this.entityId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "SecurityId", {
        get: function () { return this.securityId; },
        set: function (newValue) { this.securityId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "IsChangeIssuedDate", {
        get: function () { return this.isChangeIssuedDate; },
        set: function (newValue) { this.isChangeIssuedDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "IssuedByUserId", {
        get: function () { return this.issuedByUserId; },
        set: function (newValue) { this.issuedByUserId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (newValue) { this.notes = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTypeId", {
        get: function () { return this.documentTypeId; },
        set: function (newValue) { this.documentTypeId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "Issued", {
        get: function () { return this.issued; },
        set: function (newValue) { this.issued = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "IssuedByUserName", {
        get: function () { return this.issuedByUserName; },
        set: function (newValue) { this.issuedByUserName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "ChildEntityId", {
        get: function () { return this.childEntityId; },
        set: function (newValue) { this.childEntityId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "ChildEntityReference", {
        get: function () { return this.childEntityReference; },
        set: function (newValue) { this.childEntityReference = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTypeName", {
        get: function () { return this.documentTypeName; },
        set: function (newValue) { this.documentTypeName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTypeCode", {
        get: function () { return this.documentTypeCode; },
        set: function (newValue) { this.documentTypeCode = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "IssuedDate", {
        get: function () { return this.issuedDate; },
        set: function (newValue) { this.issuedDate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTemplateEditorTool", {
        get: function () { return this.documentTemplateEditorTool; },
        set: function (newValue) { this.documentTemplateEditorTool = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTypeDefaultEditorTool", {
        get: function () { return this.documentTypeDefaultEditorTool; },
        set: function (newValue) { this.documentTypeDefaultEditorTool = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "FollowUpId", {
        get: function () { return this.followUpId; },
        set: function (newValue) { this.followUpId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentOutCopies", {
        get: function () { return this.documentOutCopies; },
        set: function (newValue) { this.documentOutCopies = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "FollowUpCount", {
        get: function () { return this.followUpCount; },
        set: function (newValue) { this.followUpCount = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "Extension", {
        get: function () { return this.extension; },
        set: function (newValue) { this.extension = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "ReportTemplate", {
        get: function () { return this.reportTemplate; },
        set: function (newValue) { this.reportTemplate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "HasFollowUp", {
        get: function () { return this.hasFollowUp; },
        set: function (newValue) { this.hasFollowUp = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "HTMLTemplate", {
        get: function () { return this.hTMLTemplate; },
        set: function (newValue) { this.hTMLTemplate = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "TemplateType", {
        get: function () { return this.templateType; },
        set: function (newValue) { this.templateType = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTypeSubject", {
        get: function () { return this.documentTypeSubject; },
        set: function (newValue) { this.documentTypeSubject = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "EditableFields", {
        get: function () { return this.editableFields; },
        set: function (newValue) { this.editableFields = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTemplateId", {
        get: function () { return this.documentTemplateId; },
        set: function (newValue) { this.documentTemplateId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "EmailTemplateId", {
        get: function () { return this.emailTemplateId; },
        set: function (newValue) { this.emailTemplateId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "XamlDocumentId", {
        get: function () { return this.xamlDocumentId; },
        set: function (newValue) { this.xamlDocumentId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "DocumentTypeObjectTableId", {
        get: function () { return this.documentTypeObjectTableId; },
        set: function (newValue) { this.documentTypeObjectTableId = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "NeedsRebuild", {
        get: function () { return this.needsRebuild; },
        set: function (newValue) { this.needsRebuild = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "IsBlobExist", {
        get: function () { return this.isBlobExist; },
        set: function (newValue) { this.isBlobExist = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "FileSize", {
        get: function () { return this.fileSize; },
        set: function (newValue) { this.fileSize = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "FileName", {
        get: function () { return this.fileName; },
        set: function (newValue) { this.fileName = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentOutPM.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; this.MarkAsDirty(); },
        enumerable: true,
        configurable: true
    });
    DocumentOutPM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    return DocumentOutPM;
}());
exports.DocumentOutPM = DocumentOutPM;
//# sourceMappingURL=DocumentOutPM.js.map