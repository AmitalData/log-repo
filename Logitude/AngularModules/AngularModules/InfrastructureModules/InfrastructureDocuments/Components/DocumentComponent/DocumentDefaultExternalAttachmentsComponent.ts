
import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { DocumentTypeCopyList } from '../../../../Common/EntityLists/DocumentTypeCopyList';
import { DocumentTypeList } from '../../../../Common/EntityLists/DocumentTypeList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentTypeListService } from '../../../../Common/Services/StandardLists/DocumentTypeListService';
import { DocumentTypeListExtendedService } from '../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';

import { DocumentDefultAttachment } from '../../../../Common/DataContracts/DocumentDefultAttachment';
import { DocumentTypeTemplatePM } from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import { DocumentExtendedService } from '../../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import { DocumentPM } from '../../../../Common/EntityPMs/DocumentPM';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';



@Component({

    selector: 'DocumentDefaultExternalAttachmentsComponent',
    templateUrl: './DocumentDefaultExternalAttachmentsComponent.html',
})

export class DocumentDefaultExternalAttachmentsComponent implements OnInit {
    
    DocumentDefaultExternalAttachments: DocumentDefaultExternalAttachments[] = [];

    DocumentTypeTemplatePM: DocumentTypeTemplatePM;
    ObjectTableId: string;
    DocumentTypeTemplateId: string;
    AttachedExternalDocumentsIds: string;
    TemplateExternalAttachmentsIds: string[] = [];

    private documentExtendedService: DocumentExtendedService = new DocumentExtendedService();
    //DocumentExtended

    private CurrentSession = SessionLocator.SelectedSession;
     
    IsReady: boolean = false;
    constructor() { 
    }

    ngOnInit(


    ) {


    }

    SetWindowArgs(args: any) {
       // this.CurrentSession.StartBusyIndicator("Loading...");

        this.DocumentTypeTemplatePM = args.DocumentTypeTemplatePM;
        this.DocumentTypeTemplateId = this.DocumentTypeTemplatePM.Id;
        this.AttachedExternalDocumentsIds = this.DocumentTypeTemplatePM.AttachedExternalDocumentsIds;
        this.GetTemplateExternalAttachmentsIds(this.AttachedExternalDocumentsIds)
        this.GetDocumentDefaultExternalAttachment(this.TemplateExternalAttachmentsIds);
        this.ObjectTableId = args.ObjectTableId;
      //  this.DocumentDefaultExternalAttachments = this.DocumentTypeTemplatePM.DocumentDefultAttachments;
          
       // this.LoadData();

    }
    GetDocumentDefaultExternalAttachment(TemplateExternalAttachmentsIds: string[]) {
         
        TemplateExternalAttachmentsIds.forEach((item) => {
            // this.TermsofUsePMLists.push(new TermsofUsePMViewModel(item));
            this.documentExtendedService.GetDocumentById(item, this.DocumentTypeTemplatePM.Tenant).subscribe((res: any) => {

                var response: ServiceResponse = res; 
                if (!response.HasError) {
                    var result = response.Result;
                    if (result) {
                        this.DocumentDefaultExternalAttachments.push(new DocumentDefaultExternalAttachments(result));
                    }
                }
                //else {
                //    this.HandleServiceError(response)
                //}
            });

        });

      

    }


    ViewFile(item: DocumentPM) {

        var documentName = item.Id
        DownloadManager.DownloadPage(documentName);

    }


    GetTemplateExternalAttachmentsIds(AttachedExternalDocumentsIds: string): any {
        this.TemplateExternalAttachmentsIds = AttachedExternalDocumentsIds.split(',');
        console.log("length is " + this.TemplateExternalAttachmentsIds.length);
    }

     
    public LoadData() {

        this.LoadDocumentOut();
        this.LoadDocumentIn();

    }



    LoadDocumentOut() {
        var _documentTypeListService: DocumentTypeListExtendedService = new DocumentTypeListExtendedService();
        _documentTypeListService.GetDocumentTypeCopyLists(this.ObjectTableId).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var documentTypeCopyLists: DocumentTypeCopyList[] = pmResponse.Result;
                documentTypeCopyLists.forEach((copy) => {
                   
                 //   this.DocOutAttachmentLists.push(item);
                });
            }

            //if (this.DocOutAttachmentLists.length == 0) this.IsNotDocumentOutFound = true;

           
            this.LoadComplete();

        });
    }

    IsNotDocumentOutFound: boolean = false;
    IsNotDocumentInFound: boolean = false;

    LoadDocumentIn() {

        var _documentTypeListService: DocumentTypeListService = new DocumentTypeListService();
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
        _documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {

                var documentTypeList: DocumentTypeList[] = pmResponse.Result;
                documentTypeList.filter(d => d.ObjectTableId == this.ObjectTableId && d.IsDocIn == true).forEach((doc) => {
                   
                 //   this.DocInAttachmentLists.push(item);
                });
            }
          //  if (this.DocInAttachmentLists.length == 0) this.IsNotDocumentInFound = true;


            
            this.LoadComplete();
        });
    }



    LoadComplete() {

    //   if (this.IsLoadDocumentOutDocument && this.IsLoadDocumentInDocument) {
     //       this.CurrentSession.StopBusyIndicator();
      //      this.IsReady = true;
      //  }
    }


    CloseButtonClicked() {

        this.CurrentSession.CurrentWindow.Close("");
    }


    GetDocumentDefultAttachment(doc: DocumentDefaultExternalAttachments) {

        var item = new DocumentDefultAttachment();
       // i//tem.DocumentTypeId = doc.DocumentTypeId; 
        //item.DocumentTypeName = doc.DocumentTypeName;
        return item;
    }


    SaveButtonClicked() {

        this.DocumentTypeTemplatePM.IsDefultAttachmentsXMLChanged = true;
        this.DocumentTypeTemplatePM.DocumentDefultAttachments = this.BuildDocumentDefultAttachmentLists();
        this.CloseButtonClicked();

    }


    private BuildDocumentDefultAttachmentLists() {
        var documentDefultAttachments = new Array<DocumentDefultAttachment>();
      //  this.DocOutAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
           // documentDefultAttachments.push(this.GetDocumentDefultAttachment(doc));
       // });
       // this.DocInAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
       //     documentDefultAttachments.push(this.GetDocumentDefultAttachment(doc));
      //  });
        return documentDefultAttachments;
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




