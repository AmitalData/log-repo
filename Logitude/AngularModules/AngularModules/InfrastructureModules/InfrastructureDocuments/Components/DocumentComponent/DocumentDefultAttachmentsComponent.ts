
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



@Component({

    selector: 'DocumentDefultAttachmentsComponent',
    templateUrl: './DocumentDefultAttachmentsComponent.html',
})

export class DocumentDefultAttachmentsComponent implements OnInit {
    public DocumentTypeCopyLists: DocumentTypeCopyList[];
    public DocumentTypeLists: DocumentTypeList[];
    DataContext: any;
    DocOutAttachmentLists: DocumentDefultAttachmentItem[] = [];
    DocInAttachmentLists: DocumentDefultAttachmentItem[] = [];
    DocumentDefultAttachments: DocumentDefultAttachment[] = [];

    DocumentTypeTemplatePM: DocumentTypeTemplatePM;
    ObjectTableId: string;
    private CurrentSession = SessionLocator.SelectedSession;

    IsLoadDocumentInDocument: boolean = false;
    IsLoadDocumentOutDocument: boolean = false;
    IsReady: boolean = false;
    constructor() {
        this.DocOutAttachmentLists = [];
        this.DocInAttachmentLists = [];
    }

    ngOnInit(


    ) {


    }

    SetWindowArgs(args: any) {
        this.CurrentSession.StartBusyIndicator("Loading...");

        this.DocumentTypeTemplatePM = args.DocumentTypeTemplatePM;
        this.ObjectTableId = args.ObjectTableId;
        this.DocumentDefultAttachments = this.DocumentTypeTemplatePM.DocumentDefultAttachments;
        this.LoadData();

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
                    var item = new DocumentDefultAttachmentItem(copy.DocumentTypeId, "DocOut", copy.Name, copy.Id)
                    if (this.DocumentDefultAttachments.filter(d => d.DocumentTypeId == item.DocumentTypeId && d.Type == "DocOut")[0]) {
                        item.IsChecked = true;
                    }
                  this.DocOutAttachmentLists.push(item);
                });
            }

            if (this.DocOutAttachmentLists.length == 0) this.IsNotDocumentOutFound = true;

            this.IsLoadDocumentOutDocument = true;
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
                    var item = new DocumentDefultAttachmentItem(doc.Id, "DocIn", doc.Name);
                    if (this.DocumentDefultAttachments.filter(d => d.DocumentTypeId == item.DocumentTypeId && d.Type == "DocIn")[0]) {
                        item.IsChecked = true;
                    }
                   this.DocInAttachmentLists.push(item);
                });
            }
            if (this.DocInAttachmentLists.length == 0) this.IsNotDocumentInFound = true;


            this.IsLoadDocumentInDocument = true;
            this.LoadComplete();
        });
    }



    LoadComplete() {

        if (this.IsLoadDocumentOutDocument && this.IsLoadDocumentInDocument) {
            this.CurrentSession.StopBusyIndicator();
            this.IsReady = true;
        }
    }


    CloseButtonClicked() {
   
        this.CurrentSession.CurrentWindow.Close("");
    }


    GetDocumentDefultAttachment(doc: DocumentDefultAttachmentItem) {

        var item = new DocumentDefultAttachment();
        item.DocumentTypeId = doc.DocumentTypeId;
        item.DocumentTypeCopyId = doc.DocumentTypeCopyId;

        item.Type = doc.Type;
        item.DocumentTypeName = doc.DocumentTypeName;
        return item;
    }


    SaveButtonClicked() {

        this.DocumentTypeTemplatePM.IsDefultAttachmentsXMLChanged = true;
        this.DocumentTypeTemplatePM.DocumentDefultAttachments = this.BuildDocumentDefultAttachmentLists();
        this.CloseButtonClicked();

    }


    private BuildDocumentDefultAttachmentLists() {
        var documentDefultAttachments = new Array<DocumentDefultAttachment>();
        this.DocOutAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
            documentDefultAttachments.push(this.GetDocumentDefultAttachment(doc));
        });
        this.DocInAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
            documentDefultAttachments.push(this.GetDocumentDefultAttachment(doc));
        });
        return documentDefultAttachments;
    }
}


export class DocumentDefultAttachmentItem {
    public DocumentTypeId: string;
    public DocumentTypeName: string;
    public DocumentTypeCopyId: string;
    public Type: string;
    public IsChecked: boolean;
    constructor(id: string,  type: string, name: string , copyId:string = null) {
        this.DocumentTypeId = id;
        this.DocumentTypeCopyId = copyId;

        this.Type = type;
        this.DocumentTypeName = name;

    }



}




