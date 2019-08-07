"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DocumentTypeTemplateViewModel = /** @class */ (function () {
    function DocumentTypeTemplateViewModel(documentTypeTemplate) {
        this.CC = "";
        this.From = "";
        this.ReplyTo = "";
        this.TemplateSubject = "";
        this.TemplateCc = "";
        this.description = "";
        this.inActive = false;
        this.isDefault = false;
        this.Entity = documentTypeTemplate;
        this.ActiveId = documentTypeTemplate.Id + "Active";
        this.OriginalTemplate = documentTypeTemplate.OriginalTemplateName;
        this.IsEnabledAddDocumentTemplate = true;
        this.Id = documentTypeTemplate.Id;
        this.Tenant = documentTypeTemplate.Tenant;
        //this.TemplateBody = documentTypeTemplate.TemplateBody;
        this.TemplateType = documentTypeTemplate.TemplateType;
        this.LastUpdatedByUserId = documentTypeTemplate.LastUpdatedByUserId;
        this.DocumentTypeId = documentTypeTemplate.DocumentTypeId;
        this.Subject = documentTypeTemplate.Subject;
        this.LastUpdateDate = documentTypeTemplate.LastUpdateDate;
        this.InActive = documentTypeTemplate.InActive;
        this.EditorTool = documentTypeTemplate.EditorTool;
        this.VerticalShift = documentTypeTemplate.VerticalShift;
        this.HorizontalShift = documentTypeTemplate.HorizontalShift;
        this.OriginalTemplateId = documentTypeTemplate.OriginalTemplateId;
        this.Description = documentTypeTemplate.Description;
        this.Language = documentTypeTemplate.Language;
        this.InternalRemarks = documentTypeTemplate.InternalRemarks;
        this.CountryCode = documentTypeTemplate.CountryCode;
        this.IsEnabledForCustomers = documentTypeTemplate.IsEnabledForCustomers;
        this.IsCopiedAtSignup = documentTypeTemplate.IsCopiedAtSignup;
        this.LastUpdateByUserName = documentTypeTemplate.LastUpdateByUserName;
        this.IsDefault = documentTypeTemplate.IsDefault;
        this.OriginalTemplateName = documentTypeTemplate.OriginalTemplateName;
        this.DocumentTypeCode = documentTypeTemplate.DocumentTypeCode;
        this.IsHideDocumentName = documentTypeTemplate.IsHideDocumentName;
        this.TemplateBodyHtml = documentTypeTemplate.TemplateBodyHtml;
        this.TemplateHeaderHtml = documentTypeTemplate.TemplateHeaderHtml;
        this.TemplateFooterHtml = documentTypeTemplate.TemplateFooterHtml;
        this.CountryName = documentTypeTemplate.CountryName;
        this.ObjectTableId = documentTypeTemplate.ObjectTableId;
        this.TemplateHeaderHeight = documentTypeTemplate.TemplateHeaderHeight;
        this.TemplateFooterHeight = documentTypeTemplate.TemplateFooterHeight;
        this.TemplateTechnologyCode = documentTypeTemplate.TemplateTechnologyCode;
        this.From = documentTypeTemplate.From;
        this.ReplyTo = documentTypeTemplate.ReplyTo;
        this.CC = documentTypeTemplate.CC;
        if (documentTypeTemplate.InActive) {
            this.LableSetactive = "Mark as active";
        }
        else {
            this.LableSetactive = "Mark as inactive";
        }
        if (!documentTypeTemplate.IsHideDocumentName) {
            this.DocumentTypeName = documentTypeTemplate.DocumentTypeName;
        }
        else {
            this.DocumentTypeName = "";
        }
        // this.IsHaveJsonString = documentTypeTemplateList.IsHaveJsonString;
    }
    Object.defineProperty(DocumentTypeTemplateViewModel.prototype, "Description", {
        get: function () {
            return this.description;
        },
        set: function (newValue) {
            if (this.description != newValue) {
                this.description = newValue;
                this.Entity.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypeTemplateViewModel.prototype, "InActive", {
        get: function () { return this.inActive; },
        set: function (newValue) {
            if (this.inActive != newValue) {
                this.inActive = newValue;
                this.Entity.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentTypeTemplateViewModel.prototype, "IsDefault", {
        get: function () { return this.isDefault; },
        set: function (newValue) {
            if (this.isDefault != newValue) {
                this.isDefault = newValue;
                this.Entity.IsDefault = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return DocumentTypeTemplateViewModel;
}());
exports.DocumentTypeTemplateViewModel = DocumentTypeTemplateViewModel;
//# sourceMappingURL=DocumentTypeTemplateViewModel.js.map