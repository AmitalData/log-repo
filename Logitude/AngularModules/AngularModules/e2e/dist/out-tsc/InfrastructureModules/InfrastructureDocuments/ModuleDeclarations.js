"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DocumentCustomFieldsComponent_1 = require("./Components/DocumentComponent/DocumentCustomFieldsComponent");
var GeneratedDocumentCustomFieldComponent_1 = require("./Components/DocumentComponent/GeneratedDocumentCustomFieldComponent");
var PrintDocumentComponent_1 = require("./Components/DocumentComponent/PrintDocumentComponent");
var EditDocumentComponent_1 = require("./Components/DocumentComponent/EditDocumentComponent");
var HtmlDocumentPreviewComponent_1 = require("./Components/DocumentComponent/HtmlDocumentPreviewComponent");
var SaveAsTemplateComponent_1 = require("./Components/DocumentComponent/SaveAsTemplateComponent");
var SimplogInfoPopupComponent_1 = require("./Components/DocumentComponent/SimplogInfoPopupComponent");
var HeaderAndFooterComponent_1 = require("./Components/DocumentComponent/HeaderAndFooterComponent");
var DocumentObjectFieldsComponent_1 = require("./Components/DocumentComponent/DocumentObjectFieldsComponent");
var AttachDocsOutComponent_1 = require("./Components/DocumentComponent/AttachDocs/AttachDocsOutComponent");
var AttachmentDocsInComponent_1 = require("./Components/DocumentComponent/AttachDocs/AttachmentDocsInComponent");
var AttachmentUploaderComponent_1 = require("./Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
var SendDocumentComponent_1 = require("./Components/DocumentComponent/SendDocumentComponent");
var AddDocumentTypeFromLibraryComponent_1 = require("./Components/DocumentComponent/FromLibrary/AddDocumentTypeFromLibraryComponent");
var AddDocumentTypeTemplateFromLibraryComponent_1 = require("./Components/DocumentComponent/FromLibrary/AddDocumentTypeTemplateFromLibraryComponent");
var DocumentFilingBackupBatchesComponent_1 = require("./Components/DocumentsBackup/DocumentFilingBackupBatchesComponent");
var AddDocumentFilingBackupBatchComponent_1 = require("./Components/DocumentsBackup/AddDocumentFilingBackupBatchComponent");
var DocumentFilingBackupSettingComponent_1 = require("./Components/DocumentsBackup/DocumentFilingBackupSettingComponent");
var DocumentTypeCopiesComponent_1 = require("./Components/DocumentType/Tab/DocumentTypeCopiesComponent");
var AddEditDocumentTypeCustomFieldComponent_1 = require("./Components/DocumentType/Tab/AddEditDocumentTypeCustomFieldComponent");
var DocumentTypeCustomFieldsComponent_1 = require("./Components/DocumentType/Tab/DocumentTypeCustomFieldsComponent");
var PrintingOptionsComponent_1 = require("./Components/DocumentType/Tab/PrintingOptionsComponent");
var SharedLogisticsTabComponent_1 = require("./Components/DocumentType/Tab/SharedLogisticsTabComponent");
var LogBoxTabComponent_1 = require("./Components/DocumentType/Tab/LogBoxTabComponent");
var AdvanceDocumentTypeComponent_1 = require("./Components/DocumentType/AdvanceDocumentTypeComponent");
var AdvanceDocumentTypeTemplateComponent_1 = require("./Components/DocumentType/AdvanceDocumentTypeTemplateComponent");
var DocumentTypeGeneralTabComponent_1 = require("./Components/DocumentType/DocumentTypeGeneralTabComponent");
var DocumentTypeTemplateComponent_1 = require("./Components/DocumentType/DocumentTypeTemplateComponent");
var NewDocumentTypeComponent_1 = require("./Components/DocumentType/NewDocumentTypeComponent");
var NewReportTemplateComponent_1 = require("./Components/DocumentType/NewReportTemplateComponent");
var SendToContactsComponent_1 = require("./Components/SendMessageContacts/SendToContactsComponent");
var SharedDocumentsPermissionsComponent_1 = require("./Components/SharedDocument/SharedDocumentsPermissionsComponent");
var SharedDocumentComponent_1 = require("./Components/SharedDocument/SharedDocumentComponent");
var DocumentsFilingGeneralTabComponent_1 = require("./Components/DocumentsFiling/DocumentsFilingGeneralTabComponent");
exports.Components = [
    DocumentTypeCopiesComponent_1.DocumentTypeCopiesComponent,
    AddEditDocumentTypeCustomFieldComponent_1.AddEditDocumentTypeCustomFieldComponent,
    DocumentTypeCustomFieldsComponent_1.DocumentTypeCustomFieldsComponent,
    PrintingOptionsComponent_1.PrintingOptionsComponent,
    SharedLogisticsTabComponent_1.SharedLogisticsTabComponent,
    LogBoxTabComponent_1.LogBoxTabComponent,
    AdvanceDocumentTypeComponent_1.AdvanceDocumentTypeComponent,
    AdvanceDocumentTypeTemplateComponent_1.AdvanceDocumentTypeTemplateComponent,
    DocumentTypeGeneralTabComponent_1.DocumentTypeGeneralTabComponent,
    DocumentTypeTemplateComponent_1.DocumentTypeTemplateComponent,
    NewDocumentTypeComponent_1.NewDocumentTypeComponent,
    NewReportTemplateComponent_1.NewReportTemplateComponent,
    DocumentCustomFieldsComponent_1.DocumentCustomFieldsComponent,
    GeneratedDocumentCustomFieldComponent_1.GeneratedDocumentCustomFieldComponent,
    PrintDocumentComponent_1.PrintDocumentComponent,
    SendDocumentComponent_1.SendDocumentComponent,
    EditDocumentComponent_1.EditDocumentComponent,
    HtmlDocumentPreviewComponent_1.HtmlDocumentPreviewComponent,
    SaveAsTemplateComponent_1.SaveAsTemplateComponent,
    SimplogInfoPopupComponent_1.SimplogInfoPopupComponent,
    HeaderAndFooterComponent_1.HeaderAndFooterComponent,
    DocumentObjectFieldsComponent_1.DocumentObjectFieldsComponent,
    AttachDocsOutComponent_1.AttachDocsOutComponent,
    AttachmentDocsInComponent_1.AttachmentDocsInComponent,
    AttachmentUploaderComponent_1.AttachmentUploaderComponent,
    AddDocumentTypeFromLibraryComponent_1.AddDocumentTypeFromLibraryComponent,
    AddDocumentTypeTemplateFromLibraryComponent_1.AddDocumentTypeTemplateFromLibraryComponent,
    SendToContactsComponent_1.SendToContactsComponent,
    DocumentFilingBackupBatchesComponent_1.DocumentFilingBackupBatchesComponent,
    AddDocumentFilingBackupBatchComponent_1.AddDocumentFilingBackupBatchComponent,
    DocumentFilingBackupSettingComponent_1.DocumentFilingBackupSettingComponent,
    SharedDocumentsPermissionsComponent_1.SharedDocumentsPermissionsComponent,
    SharedDocumentComponent_1.SharedDocumentComponent,
    DocumentsFilingGeneralTabComponent_1.DocumentsFilingGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "DocumentTypeCopiesComponent": {
                myResult = DocumentTypeCopiesComponent_1.DocumentTypeCopiesComponent;
                break;
            }
            case "AddEditDocumentTypeCustomFieldComponent": {
                myResult = AddEditDocumentTypeCustomFieldComponent_1.AddEditDocumentTypeCustomFieldComponent;
                break;
            }
            case "DocumentTypeCustomFieldsComponent": {
                myResult = DocumentTypeCustomFieldsComponent_1.DocumentTypeCustomFieldsComponent;
                break;
            }
            case "PrintingOptionsComponent": {
                myResult = PrintingOptionsComponent_1.PrintingOptionsComponent;
                break;
            }
            case "SharedLogisticsTabComponent": {
                myResult = SharedLogisticsTabComponent_1.SharedLogisticsTabComponent;
                break;
            }
            case "LogBoxTabComponent": {
                myResult = LogBoxTabComponent_1.LogBoxTabComponent;
                break;
            }
            case "AdvanceDocumentTypeComponent": {
                myResult = AdvanceDocumentTypeComponent_1.AdvanceDocumentTypeComponent;
                break;
            }
            case "AdvanceDocumentTypeTemplateComponent": {
                myResult = AdvanceDocumentTypeTemplateComponent_1.AdvanceDocumentTypeTemplateComponent;
                break;
            }
            case "DocumentTypeGeneralTabComponent": {
                myResult = DocumentTypeGeneralTabComponent_1.DocumentTypeGeneralTabComponent;
                break;
            }
            case "DocumentTypeTemplateComponent": {
                myResult = DocumentTypeTemplateComponent_1.DocumentTypeTemplateComponent;
                break;
            }
            case "NewDocumentTypeComponent": {
                myResult = NewDocumentTypeComponent_1.NewDocumentTypeComponent;
                break;
            }
            case "NewReportTemplateComponent": {
                myResult = NewReportTemplateComponent_1.NewReportTemplateComponent;
                break;
            }
            case "DocumentCustomFieldsComponent": {
                myResult = DocumentCustomFieldsComponent_1.DocumentCustomFieldsComponent;
                break;
            }
            case "GeneratedDocumentCustomFieldComponent": {
                myResult = GeneratedDocumentCustomFieldComponent_1.GeneratedDocumentCustomFieldComponent;
                break;
            }
            case "PrintDocumentComponent": {
                myResult = PrintDocumentComponent_1.PrintDocumentComponent;
                break;
            }
            case "SendDocumentComponent": {
                myResult = SendDocumentComponent_1.SendDocumentComponent;
                break;
            }
            case "EditDocumentComponent": {
                myResult = EditDocumentComponent_1.EditDocumentComponent;
                break;
            }
            case "HtmlDocumentPreviewComponent": {
                myResult = HtmlDocumentPreviewComponent_1.HtmlDocumentPreviewComponent;
                break;
            }
            case "SaveAsTemplateComponent": {
                myResult = SaveAsTemplateComponent_1.SaveAsTemplateComponent;
                break;
            }
            case "SimplogInfoPopupComponent": {
                myResult = SimplogInfoPopupComponent_1.SimplogInfoPopupComponent;
                break;
            }
            case "HeaderAndFooterComponent": {
                myResult = HeaderAndFooterComponent_1.HeaderAndFooterComponent;
                break;
            }
            case "DocumentObjectFieldsComponent": {
                myResult = DocumentObjectFieldsComponent_1.DocumentObjectFieldsComponent;
                break;
            }
            case "AttachDocsOutComponent": {
                myResult = AttachDocsOutComponent_1.AttachDocsOutComponent;
                break;
            }
            case "AddDocumentTypeFromLibraryComponent": {
                myResult = AddDocumentTypeFromLibraryComponent_1.AddDocumentTypeFromLibraryComponent;
                break;
            }
            case "AddDocumentTypeTemplateFromLibraryComponent": {
                myResult = AddDocumentTypeTemplateFromLibraryComponent_1.AddDocumentTypeTemplateFromLibraryComponent;
                break;
            }
            case "AttachmentDocsInComponent": {
                myResult = AttachmentDocsInComponent_1.AttachmentDocsInComponent;
                break;
            }
            case "AttachmentUploaderComponent": {
                myResult = AttachmentUploaderComponent_1.AttachmentUploaderComponent;
                break;
            }
            case "SendToContactsComponent": {
                myResult = SendToContactsComponent_1.SendToContactsComponent;
                break;
            }
            case "DocumentFilingBackupBatchesComponent": {
                myResult = DocumentFilingBackupBatchesComponent_1.DocumentFilingBackupBatchesComponent;
                break;
            }
            case "AddDocumentFilingBackupBatchComponent": {
                myResult = AddDocumentFilingBackupBatchComponent_1.AddDocumentFilingBackupBatchComponent;
                break;
            }
            case "DocumentFilingBackupSettingComponent": {
                myResult = DocumentFilingBackupSettingComponent_1.DocumentFilingBackupSettingComponent;
                break;
            }
            case "SharedDocumentsPermissionsComponent": {
                myResult = SharedDocumentsPermissionsComponent_1.SharedDocumentsPermissionsComponent;
                break;
            }
            case "SharedDocumentComponent": {
                myResult = SharedDocumentComponent_1.SharedDocumentComponent;
                break;
            }
            case "DocumentsFilingGeneralTabComponent": {
                myResult = DocumentsFilingGeneralTabComponent_1.DocumentsFilingGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map