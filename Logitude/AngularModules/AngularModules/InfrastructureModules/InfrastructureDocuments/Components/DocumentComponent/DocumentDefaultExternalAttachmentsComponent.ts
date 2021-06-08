import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentTypeTemplatePM } from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import { DocumentExtendedService } from '../../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import { DocumentPM } from '../../../../Common/EntityPMs/DocumentPM';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentFile } from '../../../../Common/DataContracts/DocumentFile';
import { DocumentFileService } from '../../../../Common/Services/DocumentServices/DocumentFileService';
import { DocumentTypeTemplatePMService } from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService'; 
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

    private documentExtendedService: DocumentExtendedService = new DocumentExtendedService();
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
        this.AttachedExternalDocumentsIds = this.DocumentTypeTemplatePM.AttachedExternalDocumentsIds;

        if (this.hasExternalAttachments()) {

            this.GetTemplateExternalAttachmentsIds(this.AttachedExternalDocumentsIds)
        }

        this.GetDocumentDefaultExternalAttachment(this.TemplateExternalAttachmentsIds); 
           
    }
    private hasExternalAttachments() {
        return this.AttachedExternalDocumentsIds && this.AttachedExternalDocumentsIds.length > 0;
    }


    GetDocumentDefaultExternalAttachment(TemplateExternalAttachmentsIds: string[]) {

        if (this.TemplateExternalAttachmentsIds.length > 0) {

            TemplateExternalAttachmentsIds.forEach((item) => {
                this.documentExtendedService.GetDocumentById(item, this.DocumentTypeTemplatePM.Tenant).subscribe((res: any) => {

                    var response: ServiceResponse = res;
                    if (!response.HasError) {
                        var result = response.Result;
                        if (result) {
                            this.DocumentDefaultExternalAttachments.push(new DocumentDefaultExternalAttachments(result));
                        }
                     }
                    else {
                        this.HandleServiceError(response)
                    }
                });
            });
        } 

    }

     
    OpenUpLoadDocumentFile() {
        document.getElementById(this.ExternalDocumentId).click();

    }

    UpLoadDocumentFile(event: any) {

        var file = querySelection(this.ExternalDocumentId);

        if (file) {

            this.Extension = file.name.split('.')[1];
            this.FileName = file.name.split('.')[0];

            
            if (this.Extension) { 
                this.ConvertArrayBufferToBase64(file, this);
            }

        }

    }

    ConvertArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.createDocumentFile(window.btoa(binary));
        };

        reader.onerror = function (e) {

        };
        reader.readAsArrayBuffer(file);


    } 
  
    createDocumentFile(file: any) {

        var documentFile = new DocumentFile();
        documentFile.FileData = file;
        documentFile.CreateDate = new Date(); 
        documentFile.Tenant = "" + this.DocumentTypeTemplatePM.Tenant;
        documentFile.FileName = this.FileName;
        documentFile.Extension = this.Extension;
        documentFile.Folder = "others";

        this.InsertDocument(documentFile);

    }

    InsertDocument(documentFile: DocumentFile) {
        this.documentFileService.insert(documentFile).subscribe((res: any) => {
            var response: ServiceResponse = res;
            if (!response.HasError) {
                var result = response.Result;
                if (result) {
                    this.DocumentDefaultExternalAttachments.push(new DocumentDefaultExternalAttachments(result));
                    this.UpdateDocumentDefaultExternalIdsField(result.Id);
                }
            }
            else {
                this.HandleServiceError(response)
            }
        });
    }

    UpdateDocumentDefaultExternalIdsField(Id: any) {
        if (this.AttachedExternalDocumentsIds) { 
            this.AttachedExternalDocumentsIds = this.AttachedExternalDocumentsIds + "," + Id; 
        } else{
            this.AttachedExternalDocumentsIds = Id;
        }

        this.GetTemplateExternalAttachmentsIds(this.AttachedExternalDocumentsIds);
        this.DocumentTypeTemplatePM.AttachedExternalDocumentsIds = this.AttachedExternalDocumentsIds;
        this.UpdateDocumentTypeTemplate(this.DocumentTypeTemplatePM);

    }

    UpdateDocumentTypeTemplate(documentTypeTemplatePM: DocumentTypeTemplatePM) {  

        this.documentTypeTemplatePMService.update(documentTypeTemplatePM).subscribe((res: any) => {
            var response: ServiceResponse = res;
            if (!response.HasError) {
                var result = response.Result;
                if (!result) { 
                    this.HandleServiceError(response) 
                }
            }
        });
    }

    HandleServiceError(serviceResponse: ServiceResponse) {
        if (!serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length == 0) {
            return;
        }
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(serviceResponse.ErrorsArray[0]); 
    }


    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }


    Remove(item: DocumentPM) {

        this.documentFileService.DeleteDocumentFile(item.Id, this.DocumentTypeTemplatePM.Tenant).subscribe((res: any) => {

                var response: ServiceResponse = res;
                if (!response.HasError) {
                    var result = response.Result;
                    if (result) {
                        this.RemoveDocuemnt(item);
                    }
                }
                else {
                    this.HandleServiceError(response)
                }
            });

    }

    RemoveDocuemnt(documentPM: DocumentPM) {
         
        this.RemoveFromExternalAttachemnts(documentPM);
        this.RemoveFromAttachmentsIdsIds(documentPM);
         
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
        this.AttachedExternalDocumentsIds = "";

        for (var i = 0; i < this.TemplateExternalAttachmentsIds.length; i++) {
            if (i == 0) {
                this.AttachedExternalDocumentsIds = TemplateExternalAttachmentsIds[i]; 
            } else {
                    this.AttachedExternalDocumentsIds = this.AttachedExternalDocumentsIds + "," + TemplateExternalAttachmentsIds[i];
                   
                } 
        } 
    }

    ViewFile(item: DocumentPM) {
        var documentName = item.Id
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
    constructor(item: DocumentPM) {
        this.Id = item.Id;
        this.Name = item.CalculatedFileName;
        this.Extension = item.Extension;

    }
}
 

