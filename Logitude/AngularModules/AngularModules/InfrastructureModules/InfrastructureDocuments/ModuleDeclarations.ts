
import {DocumentCustomFieldsComponent} from './Components/DocumentComponent/DocumentCustomFieldsComponent';
import {GeneratedDocumentCustomFieldComponent} from './Components/DocumentComponent/GeneratedDocumentCustomFieldComponent'
import {PrintDocumentComponent} from './Components/DocumentComponent/PrintDocumentComponent';
import {EditDocumentComponent} from './Components/DocumentComponent/EditDocumentComponent';
import {HtmlDocumentPreviewComponent} from './Components/DocumentComponent/HtmlDocumentPreviewComponent';
import {SaveAsTemplateComponent} from './Components/DocumentComponent/SaveAsTemplateComponent';
import {SimplogInfoPopupComponent} from './Components/DocumentComponent/SimplogInfoPopupComponent';
import {HeaderAndFooterComponent} from './Components/DocumentComponent/HeaderAndFooterComponent';
import {DocumentObjectFieldsComponent} from './Components/DocumentComponent/DocumentObjectFieldsComponent';
import {AttachDocsOutComponent} from './Components/DocumentComponent/AttachDocs/AttachDocsOutComponent';
import {AttachmentDocsInComponent} from './Components/DocumentComponent/AttachDocs/AttachmentDocsInComponent';
import {AttachmentUploaderComponent} from './Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent';
import {SendDocumentComponent} from './Components/DocumentComponent/SendDocumentComponent';
import {AddDocumentTypeFromLibraryComponent} from './Components/DocumentComponent/FromLibrary/AddDocumentTypeFromLibraryComponent';
import {AddDocumentTypeTemplateFromLibraryComponent} from './Components/DocumentComponent/FromLibrary/AddDocumentTypeTemplateFromLibraryComponent';
import { DocumentFilingBackupBatchesComponent} from './Components/DocumentsBackup/DocumentFilingBackupBatchesComponent';
import { AddDocumentFilingBackupBatchComponent} from './Components/DocumentsBackup/AddDocumentFilingBackupBatchComponent';
import { DocumentFilingBackupSettingComponent} from './Components/DocumentsBackup/DocumentFilingBackupSettingComponent';
import {DocumentTypeCopiesComponent} from './Components/DocumentType/Tab/DocumentTypeCopiesComponent';
import {DocumentTypeCopyDetailsComponent} from './Components/DocumentType/Tab/DocumentTypeCopyDetailsComponent';
import {AddEditDocumentTypeCustomFieldComponent} from './Components/DocumentType/Tab/AddEditDocumentTypeCustomFieldComponent';
import {DocumentTypeCustomFieldsComponent} from './Components/DocumentType/Tab/DocumentTypeCustomFieldsComponent';
import {PrintingOptionsComponent} from './Components/DocumentType/Tab/PrintingOptionsComponent';
import {SharedLogisticsTabComponent} from './Components/DocumentType/Tab/SharedLogisticsTabComponent';
import {LogBoxTabComponent} from './Components/DocumentType/Tab/LogBoxTabComponent';
import {AdvanceDocumentTypeComponent} from './Components/DocumentType/AdvanceDocumentTypeComponent';
import {AdvanceDocumentTypeTemplateComponent} from './Components/DocumentType/AdvanceDocumentTypeTemplateComponent';
import {DocumentTypeGeneralTabComponent} from './Components/DocumentType/DocumentTypeGeneralTabComponent';
import {DocumentTypeTemplateComponent} from './Components/DocumentType/DocumentTypeTemplateComponent';
import {NewDocumentTypeComponent} from './Components/DocumentType/NewDocumentTypeComponent';
import {NewReportTemplateComponent} from './Components/DocumentType/NewReportTemplateComponent';
import {SendToContactsComponent} from './Components/SendMessageContacts/SendToContactsComponent';
import {SharedDocumentsPermissionsComponent} from './Components/SharedDocument/SharedDocumentsPermissionsComponent';
import {SharedDocumentComponent} from './Components/SharedDocument/SharedDocumentComponent';
import { DocumentsFilingGeneralTabComponent } from './Components/DocumentsFiling/DocumentsFilingGeneralTabComponent';
import { DocumentDefultAttachmentsComponent } from './Components/DocumentComponent/DocumentDefultAttachmentsComponent';
import { DocumentDefaultExternalAttachmentsComponent } from './Components/DocumentComponent/DocumentDefaultExternalAttachmentsComponent';








export const Components =
    [
        DocumentTypeCopiesComponent,
        DocumentTypeCopyDetailsComponent,
        AddEditDocumentTypeCustomFieldComponent,
        DocumentTypeCustomFieldsComponent,
        PrintingOptionsComponent,
        SharedLogisticsTabComponent,
        LogBoxTabComponent,
        AdvanceDocumentTypeComponent,
        AdvanceDocumentTypeTemplateComponent,
        DocumentTypeGeneralTabComponent,
        DocumentTypeTemplateComponent,
        NewDocumentTypeComponent,
        NewReportTemplateComponent,
        DocumentCustomFieldsComponent,
        GeneratedDocumentCustomFieldComponent,
        PrintDocumentComponent,
        SendDocumentComponent,
        EditDocumentComponent,
        HtmlDocumentPreviewComponent,
        SaveAsTemplateComponent,
        SimplogInfoPopupComponent,
        HeaderAndFooterComponent,
        DocumentObjectFieldsComponent,
        AttachDocsOutComponent,
        AttachmentDocsInComponent,
        AttachmentUploaderComponent,
        AddDocumentTypeFromLibraryComponent,
        AddDocumentTypeTemplateFromLibraryComponent,
        SendToContactsComponent,
        DocumentFilingBackupBatchesComponent,
        AddDocumentFilingBackupBatchComponent,
        DocumentFilingBackupSettingComponent,
        SharedDocumentsPermissionsComponent,
        SharedDocumentComponent,
        DocumentsFilingGeneralTabComponent,
        DocumentDefultAttachmentsComponent,
        DocumentDefaultExternalAttachmentsComponent

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DocumentTypeCopiesComponent": { myResult = DocumentTypeCopiesComponent; break; }
            case "DocumentTypeCopyDetailsComponent": { myResult = DocumentTypeCopyDetailsComponent; break; }
            case "AddEditDocumentTypeCustomFieldComponent": { myResult = AddEditDocumentTypeCustomFieldComponent; break; }
            case "DocumentTypeCustomFieldsComponent": { myResult = DocumentTypeCustomFieldsComponent; break; }
            case "PrintingOptionsComponent": { myResult = PrintingOptionsComponent; break; }
            case "SharedLogisticsTabComponent": { myResult = SharedLogisticsTabComponent; break; }
            case "LogBoxTabComponent": { myResult = LogBoxTabComponent; break; }
            case "AdvanceDocumentTypeComponent": { myResult = AdvanceDocumentTypeComponent; break; }
            case "AdvanceDocumentTypeTemplateComponent": { myResult = AdvanceDocumentTypeTemplateComponent; break; }
            case "DocumentTypeGeneralTabComponent": { myResult = DocumentTypeGeneralTabComponent; break; }
            case "DocumentTypeTemplateComponent": { myResult = DocumentTypeTemplateComponent; break; }
            case "NewDocumentTypeComponent": { myResult = NewDocumentTypeComponent; break; }
            case "NewReportTemplateComponent": { myResult = NewReportTemplateComponent; break; }
            case "DocumentCustomFieldsComponent": { myResult = DocumentCustomFieldsComponent; break; }
            case "GeneratedDocumentCustomFieldComponent": { myResult = GeneratedDocumentCustomFieldComponent; break; }
            case "PrintDocumentComponent": { myResult = PrintDocumentComponent; break; }
            case "SendDocumentComponent": { myResult = SendDocumentComponent; break; }
            case "EditDocumentComponent": { myResult = EditDocumentComponent; break; }
            case "HtmlDocumentPreviewComponent": { myResult = HtmlDocumentPreviewComponent; break; }
            case "SaveAsTemplateComponent": { myResult = SaveAsTemplateComponent; break; }
            case "SimplogInfoPopupComponent": { myResult = SimplogInfoPopupComponent; break; }
            case "HeaderAndFooterComponent": { myResult = HeaderAndFooterComponent; break; }
            case "DocumentObjectFieldsComponent": { myResult = DocumentObjectFieldsComponent; break; }
            case "AttachDocsOutComponent": { myResult = AttachDocsOutComponent; break; }
            case "AddDocumentTypeFromLibraryComponent": { myResult = AddDocumentTypeFromLibraryComponent; break; }
            case "AddDocumentTypeTemplateFromLibraryComponent": { myResult = AddDocumentTypeTemplateFromLibraryComponent; break; }
            case "AttachmentDocsInComponent": { myResult = AttachmentDocsInComponent; break; }
            case "AttachmentUploaderComponent": { myResult = AttachmentUploaderComponent; break; }
            case "SendToContactsComponent": { myResult = SendToContactsComponent; break; }
            case "DocumentFilingBackupBatchesComponent": { myResult = DocumentFilingBackupBatchesComponent; break; }
            case "AddDocumentFilingBackupBatchComponent": { myResult = AddDocumentFilingBackupBatchComponent; break; }
            case "DocumentFilingBackupSettingComponent": { myResult = DocumentFilingBackupSettingComponent; break; }    
            case "SharedDocumentsPermissionsComponent": { myResult = SharedDocumentsPermissionsComponent; break; }
            case "SharedDocumentComponent": { myResult = SharedDocumentComponent; break; }
            case "DocumentsFilingGeneralTabComponent": { myResult = DocumentsFilingGeneralTabComponent; break; }
            case "DocumentDefultAttachmentsComponent": { myResult = DocumentDefultAttachmentsComponent; break; }
            case "DocumentDefaultExternalAttachmentsComponent": { myResult = DocumentDefaultExternalAttachmentsComponent; break; }
                 
                



        }

        return myResult;
    }
}
