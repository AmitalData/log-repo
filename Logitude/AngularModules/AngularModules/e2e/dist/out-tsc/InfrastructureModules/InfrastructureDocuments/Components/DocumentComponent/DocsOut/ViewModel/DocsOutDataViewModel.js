"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var GeneralDocumentFollowUpHelper_1 = require("../../../../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper");
var DocsOutDataViewModel = /** @class */ (function () {
    function DocsOutDataViewModel(docType, entityId, childEntityId, currentObjectTableId, childObjectTableId, childReference, internalDocuments, communicationLogLists, docsOutTabComponent, entityPM, objectTableName, documentTypeList) {
        if (objectTableName === void 0) { objectTableName = null; }
        if (documentTypeList === void 0) { documentTypeList = null; }
        var _this = this;
        this.IsCrm = false;
        this.IsNotFromDocsOutListOpenPrintControl = false;
        this.ToSpecificeEmail = "";
        this.HasTree = false;
        this.IsViewTree = false;
        this.DocumentTypeCode = "";
        this.CommunicationLogObsListHeight = "65px";
        this.DocsOutTabComponent = docsOutTabComponent;
        this.DocumentTypePM = docType;
        this.DocumentTypeList = documentTypeList;
        this.EntityPM = entityPM;
        this.EntityId = !Tools_1.AppTool.IsNullOrEmpty(entityId) ? entityId : "";
        this.ChildEntityId = !Tools_1.AppTool.IsNullOrEmpty(childEntityId) ? childEntityId : "";
        this.ChildObjectTableId = !Tools_1.AppTool.IsNullOrEmpty(childObjectTableId) ? childObjectTableId : "";
        this.ChildReference = !Tools_1.AppTool.IsNullOrEmpty(childReference) ? childReference : "";
        this.CurrentObjectTableId = !Tools_1.AppTool.IsNullOrEmpty(currentObjectTableId) ? currentObjectTableId : "";
        this.DocumentType = this.DocumentTypePM ? this.DocumentTypePM : this.DocumentTypeList;
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.CurrentObjectTableId; })[0];
        if (objectTableName) {
            table = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
        }
        if (table) {
            this.ObjectTableName = table.Name;
            this.CurrentObjectTableId = table.Id;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ChildObjectTableId; })[0];
            if (table)
                this.ChildObjectTableName = table.Name;
        }
        this.generalDocumentFollowUpHelper = new GeneralDocumentFollowUpHelper_1.GeneralDocumentFollowUpHelper(this.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildReference, "DocOut", this, entityPM);
        this.CommunicationLogPMs = communicationLogLists;
        if (internalDocuments != null) {
            if (childEntityId)
                this.CurrentDocument = internalDocuments.filter(function (d) { return d.DocumentTypeId == _this.DocumentType.Id && d.ChildEntityId == childEntityId; })[0];
            else
                this.CurrentDocument = internalDocuments.filter(function (d) { return d.DocumentTypeId == _this.DocumentType.Id; })[0];
        }
        if (this.CurrentDocument != null) {
            if (this.CommunicationLogPMs) {
                this.CommunicationLogObsList = this.CommunicationLogPMs.filter(function (d) { return d.CurrentEntityPm.DocumentOutId == _this.CurrentDocument.Id; });
                if (this.CommunicationLogObsList && this.CommunicationLogObsList.length > 0)
                    this.HasTree = true;
            }
            if (this.CurrentDocument.DocumentOutCopies.length > 0 && this.DocumentType.TemplateFormatCode == "P")
                this.HasFile = true;
            else
                this.HasFile = false;
            this.IssuedByUserName = this.CurrentDocument.IssuedByUserName;
            this.IssuedDate = this.CurrentDocument.IssuedDate;
            this.Exists = true;
        }
        else
            this.Exists = false;
        if (this.DocumentType) {
            this.Name = this.DocumentType.Name;
            this.Id = this.DocumentType.Id;
            this.DocumentTypeId = this.DocumentType.Id;
            this.DocumentTypeName = this.DocumentType.Name;
            this.TemplateType = this.DocumentType.TemplateFormatCode;
            this.DocumentTypeCode = this.DocumentType.Code;
            if (this.DocumentType.TemplateFormatCode == "M") {
                this.IsSendButtonsVisible = true;
                this.IsBuildViewButtonsVisible = false;
            }
            else {
                this.IsSendButtonsVisible = false;
                this.IsBuildViewButtonsVisible = true;
            }
        }
    }
    DocsOutDataViewModel.prototype.ViewTree = function () {
        if (this.IsViewTree)
            this.IsViewTree = false;
        else {
            this.SetCommunicationLogListHeight();
            this.IsViewTree = true;
        }
    };
    DocsOutDataViewModel.prototype.SetCommunicationLogListHeight = function () {
        if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 1) {
            this.CommunicationLogObsListHeight = "65px";
        }
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 2) {
            this.CommunicationLogObsListHeight = "90px";
        }
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 3) {
            this.CommunicationLogObsListHeight = "120px";
        }
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 4) {
            this.CommunicationLogObsListHeight = "144px";
        }
        else if (this.CommunicationLogObsList && this.CommunicationLogObsList.length == 5 || this.CommunicationLogObsList.length > 5) {
            this.CommunicationLogObsListHeight = "170px";
        }
    };
    Object.defineProperty(DocsOutDataViewModel.prototype, "InternalDocumentId", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.Id;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.CurrentDocument && this.CurrentDocument.Id != newValue) {
                this.CurrentDocument.Id = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsOutDataViewModel.prototype, "Issued", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.Issued;
            }
            else
                return false;
        },
        set: function (newValue) {
            if (this.CurrentDocument) {
                this.CurrentDocument.Issued = newValue;
                if (newValue) {
                    this.generalDocumentFollowUpHelper.MarkFollowUpAsDone();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    DocsOutDataViewModel.prototype.OnSelectedChangeCommunicationLogObsList = function (communicationLog) {
        this.SelectedCommunicationLogViewMode = communicationLog;
    };
    return DocsOutDataViewModel;
}());
exports.DocsOutDataViewModel = DocsOutDataViewModel;
//# sourceMappingURL=DocsOutDataViewModel.js.map