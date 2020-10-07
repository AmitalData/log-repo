/// <reference path="../../../../common/entitylists/documenttypecopylist.ts" />
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

@Component({

    selector: 'DocumentDefultAttachmentsComponent',
    templateUrl: './DocumentDefultAttachmentsComponent.html',
})

export class DocumentDefultAttachmentsComponent implements OnInit {
    public DocumentTypeCopyLists: DocumentTypeCopyList[];
    public DocumentTypeLists: DocumentTypeList[];
    DataContext: any;

    DocOutAttachmentLists: DocumentDefultAttachmentClass[] = [];
    DocInAttachmentLists: DocumentDefultAttachmentClass[] = [];





    ObjectTableId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.DocOutAttachmentLists = [];
        this.DocInAttachmentLists = [];

    }

    ngOnInit(


    ) {





    }


    SetWindowArgs(args: any) {

        this.ObjectTableId = args.ObjectTableId;
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

                documentTypeCopyLists.forEach((doc) => {
                    this.DocOutAttachmentLists.push(new DocumentDefultAttachmentClass(doc.Id, "DocOut", doc.Name));
                });
            }
        });
    }

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
                    this.DocInAttachmentLists.push(new DocumentDefultAttachmentClass(doc.Id, "DocIn", doc.Name));
                });
            }
        });
    }

    CloseButtonClicked() {
   
        this.CurrentSession.CurrentWindow.Close("");
    }



    SaveButtonClicked() {
        let attachmentLists: DocumentDefultAttachmentClass[] = [];

        this.DocOutAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
            attachmentLists.push(doc);
        });


        this.DocInAttachmentLists.filter(d => d.IsChecked == true).forEach((doc) => {
            attachmentLists.push(doc);
        });





        this.CloseButtonClicked();
    }


    //IsSelect: boolean;
    //public CheckboxClick(item: DocumentOutCopyViewModel) {

    //    var selectitem = this.SelectedDocumentsList.filter(d => d.Id == item.Id)[0];
    //    if (!item.IsAttachSelect) {
    //        if (selectitem == null) {
    //            this.SelectedDocumentsList.push(item);
    //        }
    //        item.IsAttachSelect = true;

    //    }
    //    else {
    //        if (selectitem != null) {
    //            this.SelectedDocumentsList = this.SelectedDocumentsList.filter(d => d.Id != selectitem.Id);
    //        }
    //        item.IsAttachSelect = false;
    //    }




    //}

}



export class DocumentDefultAttachmentClass {

    constructor(id:string , type:string , name:string) {
        this.Id = id;
        this.Type = type;

        this.Name = name;

    }


    Id: string;
    Type: string;
    Name: string;
    IsChecked: boolean;
}
