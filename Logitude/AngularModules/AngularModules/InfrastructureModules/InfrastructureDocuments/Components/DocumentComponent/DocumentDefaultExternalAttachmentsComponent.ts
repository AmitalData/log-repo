import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentTypeTemplatePM } from '../../../../Common/EntityPMs/DocumentTypeTemplatePM'; 
import { DocumentPM } from '../../../../Common/EntityPMs/DocumentPM';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentFile } from '../../../../Common/DataContracts/DocumentFile';
import { DocumentFileService } from '../../../../Common/Services/DocumentServices/DocumentFileService';
import { DocumentTypeTemplatePMService } from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService'; 
import { AppTool } from '../../../../Infrastructure/Tools';
declare var querySelection, resultToUnitArray: any;


@Component({

    selector: 'DocumentDefaultExternalAttachmentsComponent',
    templateUrl: './DocumentDefaultExternalAttachmentsComponent.html',
})

export class DocumentDefaultExternalAttachmentsComponent implements OnInit {
    
    DocumentDefaultExternalAttachments: DocumentDefaultExternalAttachments[] = [];
    DocumentTypeTemplatePM: DocumentTypeTemplatePM; 
    DocumentTypeTemplateId: string;
    AttachedExternalDocumentsIds: string;
    TemplateExternalAttachmentsIds: string[] = [];

    ExternalDocumentId: string = Guid.NewRandomString();
    FileName: string;
    Extension: string;

    private DocumentsTotalSize = 0;
    private  DocumentsMaximumSize = 20;

    private documentTypeTemplatePMService: DocumentTypeTemplatePMService = new DocumentTypeTemplatePMService();
       
    private documentFileService: DocumentFileService = new DocumentFileService();
    private CurrentSession = SessionLocator.SelectedSession;
      
    constructor() { 
    }

    ngOnInit() {
    }

    SetWindowArgs(args: any) {  
        this.DocumentTypeTemplatePM = args.DocumentTypeTemplatePM;
        this.DocumentTypeTemplateId = this.DocumentTypeTemplatePM.Id;
        this.GetAttachedExternalDocumentsIdsByTemplateId(); 
           
    } 

    private GetAttachedExternalDocumentsIdsByTemplateId() {
        this.InitializeAttachedExternalDocumentsIds();

        if (this.hasExternalAttachments()) 
            this.LoadDocumentsList();
       
    }

    private LoadDocumentsList() {
        this.GetTemplateExternalAttachmentsIds(this.AttachedExternalDocumentsIds);
        this.GetDocumentDefaultExternalAttachment(this.AttachedExternalDocumentsIds);
    }

    private InitializeAttachedExternalDocumentsIds() {

        this.AttachedExternalDocumentsIds = this.DocumentTypeTemplatePM.AttachedExternalDocumentsIds;

        if (!this.AttachedExternalDocumentsIds) {
            this.AttachedExternalDocumentsIds = "";
        }
    }

    private hasExternalAttachments() {
        return this.AttachedExternalDocumentsIds.length > 0;
    }


    GetDocumentDefaultExternalAttachment(TemplateExternalAttachmentsIds: string) {
        this.StartBusyIndicator();
        this.documentFileService.GetDocumentsByIdsList(TemplateExternalAttachmentsIds).subscribe((res: any) => {
                let response: ServiceResponse = res;
                if (this.validResponse(response)) {
                    this.BuildDocumentsList(response.Result) 
                } else {
                    this.HandleServiceError(response)
                }
            });
         
    }
    BuildDocumentsList(Result: any) {
        this.StopBusyIndicator();
        Result.forEach(item => {  
            this.DocumentDefaultExternalAttachments.push(new DocumentDefaultExternalAttachments(item));
            this.DocumentsTotalSize = this.DocumentsTotalSize + item.FileSize;
        }) 
    }

     
    private validResponse(response: ServiceResponse) {
        return !response.HasError && response.Result;
    }

    OpenUpLoadDocumentFile() {
        document.getElementById(this.ExternalDocumentId).click();

    }

    UpLoadDocumentFile(event: any) { 
        const file = querySelection(this.ExternalDocumentId); 

        if (!file) { 
            return;
        }
         
        const DocumentsListSize = this.GetByteFileSize(this.DocumentsTotalSize);
        const FileSize = this.GetByteFileSize(file.size);

        if (this.ValidateFileSize(DocumentsListSize, FileSize)) {
            this.ShowMessage("The maximum size of documents you can attach is 20 MB. Please send the documents in separated emails");
            return;
        }
        
        this.Extension = file.name.split('.')[1];
        this.FileName = file.name.split('.')[0];  

        if (this.Extension) { 
        this.ConvertArrayBufferToBase64(file, this); 
        }

    }
    private ValidateFileSize(DocumentsListSize: number, FileSize: number) {
        return DocumentsListSize + FileSize >= this.DocumentsMaximumSize;
    }

    GetByteFileSize(size: any) {
        const Byte = 1024;
        let fileSize = 0;

        if (size != null) {
            fileSize = size / (Byte * Byte);
        }
        return fileSize;
    }
      
    ConvertArrayBufferToBase64(file: any, viewmodel: any) {

        this.StartBusyIndicator();
        const reader: FileReader = new FileReader();

        reader.onload = function (e) {
            let binary = '';
            const bytes = new Uint8Array(resultToUnitArray(e));
            let len = bytes.byteLength;
            for (let i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.createDocumentFile(window.btoa(binary));
        };  
        reader.onerror = function (e) { 
        };
        reader.readAsArrayBuffer(file);

    } 
  
    createDocumentFile(file: any) {

        let documentFile = this.GetNewDocument(file); 
        this.InsertDocument(documentFile);

    }

    private GetNewDocument(file: any) {
        let documentFile = new DocumentFile();
        documentFile.FileData = file;
        documentFile.CreateDate = new Date();
        documentFile.Tenant = "" + this.DocumentTypeTemplatePM.Tenant;
        documentFile.FileName = this.FileName;
        documentFile.Extension = this.Extension;
        documentFile.Folder = "others";
        return documentFile;
    }

    InsertDocument(documentFile: DocumentFile) {
        this.documentFileService.insert(documentFile).subscribe((res: any) => {
            let response: ServiceResponse = res;
            if (this.validResponse(response)) {
                this.UpdateDocumentsList(response.Result);
            } else {
                this.HandleServiceError(response)
            } 
        });
    }

    private UpdateDocumentsList(result: any) {
        this.StopBusyIndicator();
        this.DocumentDefaultExternalAttachments.push(new DocumentDefaultExternalAttachments(result));
        this.UpdateDocumentDefaultExternalIdsField(result.Id);
    }

    UpdateDocumentDefaultExternalIdsField(Id: any) {

        this.GetNewAttchmentsIds(Id); 
        this.GetTemplateExternalAttachmentsIds(this.AttachedExternalDocumentsIds);
        this.DocumentTypeTemplatePM.AttachedExternalDocumentsIds = this.AttachedExternalDocumentsIds;
        this.UpdateDocumentTypeTemplate(this.DocumentTypeTemplatePM);

    }

    private GetNewAttchmentsIds(Id: any) {
        if (AppTool.IsNullOrEmpty(this.AttachedExternalDocumentsIds)) {
            this.AttachedExternalDocumentsIds = Id;
        } else {
            this.AttachedExternalDocumentsIds = this.AttachedExternalDocumentsIds + "," + Id;
        }
    }

    UpdateDocumentTypeTemplate(documentTypeTemplatePM: DocumentTypeTemplatePM) {  

        this.documentTypeTemplatePMService.update(documentTypeTemplatePM).subscribe((res: any) => {
            let response: ServiceResponse = res;
            if (!this.validResponse(response))
                 this.HandleServiceError(response);  
        });
    }

    HandleServiceError(serviceResponse: ServiceResponse) { 
        this.StopBusyIndicator();
        if (!serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length == 0) {
            this.StopBusyIndicator();
            return;
        } 
        this.ShowMessage(serviceResponse.ErrorsArray[0]);
    }

    public ShowMessage(message: string) {
        let messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    private StartBusyIndicator() {
        this.CurrentSession.StartBusyIndicator("Loading...");
    }
    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }

    Remove(item: DocumentPM) {
        this.StartBusyIndicator();
        this.documentFileService.DeleteDocumentFile(item.Id, this.DocumentTypeTemplatePM.Tenant).subscribe((res: any) => { 
            let response: ServiceResponse = res;
            if (this.validResponse(response)) {
                this.RemoveDocuemnt(item);
            } else {
                this.HandleServiceError(response)
            } 
        }); 
    }

    RemoveDocuemnt(documentPM: DocumentPM) {
        this.DocumentsTotalSize = this.DocumentsTotalSize - documentPM.FileSize;
        this.RemoveFromExternalAttachemnts(documentPM);
        this.RemoveFromAttachmentsIdsIds(documentPM);
        this.StopBusyIndicator();
        this.Update();
    }

    private Update() {
        this.CalculateDefaultExternalAttachmentsIds(this.TemplateExternalAttachmentsIds);
        this.SetDefalutExternalAttachmentsIds(this.AttachedExternalDocumentsIds);
        this.UpdateDocumentTypeTemplate(this.DocumentTypeTemplatePM);
    }

    private RemoveFromAttachmentsIdsIds(documentPM: DocumentPM) {
        this.TemplateExternalAttachmentsIds.forEach((value, index) => {
            if (value == documentPM.Id)
                this.TemplateExternalAttachmentsIds.splice(index, 1);
        });
    }

    private RemoveFromExternalAttachemnts(item: DocumentPM) {
        this.DocumentDefaultExternalAttachments.forEach((value, index) => {
            if (value.Id == item.Id)
                this.DocumentDefaultExternalAttachments.splice(index, 1);
        });
    }

    SetDefalutExternalAttachmentsIds(AttachedExternalDocumentsIds: string) {
        this.DocumentTypeTemplatePM.AttachedExternalDocumentsIds = AttachedExternalDocumentsIds;
    }

    CalculateDefaultExternalAttachmentsIds(TemplateExternalAttachmentsIds: string[]) { 
        this.AttachedExternalDocumentsIds = TemplateExternalAttachmentsIds.toString(); 
    }

    ViewFile(item: DocumentPM) {
        let documentName = item.Id
        DownloadManager.DownloadPage(documentName); 
    } 

    GetTemplateExternalAttachmentsIds(AttachedExternalDocumentsIds: string): any {
        if (this.TemplateExternalAttachmentsIds != null || this.TemplateExternalAttachmentsIds.length != 0) {
            this.TemplateExternalAttachmentsIds = AttachedExternalDocumentsIds.split(',');
        }  
    } 

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    } 
}


export class DocumentDefaultExternalAttachments {
    public Name: string;
    public Extension: string;
    public Id: string;
    public FileSize: any;
    constructor(item: DocumentPM) {
        this.Id = item.Id;
        this.Name = item.CalculatedFileName;
        this.Extension = item.Extension;
        this.FileSize = item.FileSize;
    }
}
 

