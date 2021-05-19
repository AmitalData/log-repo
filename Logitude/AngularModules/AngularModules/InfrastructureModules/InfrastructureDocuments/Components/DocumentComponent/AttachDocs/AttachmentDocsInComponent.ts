import {Component, OnInit, EventEmitter}  from '@angular/core';
import {DocumentsFilingPM} from '../../../../../Common/EntityPMs/DocumentsFilingPM';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentOutCopyViewModel} from '../DocsOut/ViewModel/DocumentOutCopyViewModel';
import {AttachmentsList} from '../DocsOut/Filters/AttachmentsList';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {DownloadManager} from '../../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    
    selector: 'AttachDocsIn',
    templateUrl: './AttachmentDocsInComponent.html',
})

export class AttachmentDocsInComponent implements OnInit {
    public DocumentsFilingList: DocumentsFilingPM[];
    public SelectDocumentsFilingPM: DocumentsFilingPM[];
    public SelectDocumentsFilingViewModel: DocumentsFilingPM;
    OnCloseAttachmentDocsInEvent = new EventEmitter();
    public AttachmentsLists: AttachmentsList[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


    }

    ngOnInit(


    ) {





    }


    SetWindowArgs(args: any) {

        this.DocumentsFilingList = args.DocumentsFilingList;
        this.OnCloseAttachmentDocsInEvent = args.OnCloseAttachmentDocsInEvent;
        this.SelectDocumentsFilingPM = new Array<DocumentsFilingPM>();
    }






    CloseButtonClicked() {

        this.CurrentSession.CurrentWindow.Close("");

    }


    SaveButtonClicked() {

        this.AttachmentsLists = new Array<AttachmentsList>();

        this.SelectDocumentsFilingPM.forEach((doc) => {

            var item = new AttachmentsList();
            item.Id = doc.DocumentId;
            item.DocumentFilingId = doc.Id;
            item.Tenant = doc.Tenant;
            item.FileSize = doc.FileSize;
            item.FileExtension = doc.FileExtension;
            item.DocumentTypeCopyNameWithDocumentTypeName = doc.FileName; 
            item.ShowRemoveLink = true;
            item.DirectionCode =  "I";
            this.AttachmentsLists.push(item);

        }); 

        this.OnCloseAttachmentDocsInEvent.emit(this.AttachmentsLists);
        this.CloseButtonClicked();

    }


    IsSelect: boolean;
    public CheckboxClick(item: DocumentsFilingPM) {

        var selectitem = this.SelectDocumentsFilingPM.filter(d=> d.Id == item.Id)[0];
        if (!item.IsAttachSelect) {
            if (selectitem == null) {
                this.SelectDocumentsFilingPM.push(item);
            }
            item.IsAttachSelect = true;

        }
        else {
            if (selectitem != null) {
                this.SelectDocumentsFilingPM = this.SelectDocumentsFilingPM.filter(d=> d.Id != selectitem.Id);
            }
            item.IsAttachSelect = false;
        }





    }



    View(item: DocumentsFilingPM) {

        if (item != null) {
            DownloadManager.DownloadPage("",item.SecurityId);

        }


    }



}
