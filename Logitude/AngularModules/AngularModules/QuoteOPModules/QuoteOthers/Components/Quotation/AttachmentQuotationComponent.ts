
import {Component, OnInit, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import { DocumentsFilingPM } from '../../../../Common/EntityPMs/DocumentsFilingPM';
import { AppTool } from '../../../../Infrastructure/Tools';
///import {AttachmentsArgs} from '../../../../CRMModules/CRMTickets/Components/EditTabs/MainTab/SendEmailComponent';

@Component({
    
    selector: 'AttachmentQuotationComponent',
    templateUrl: './AttachmentQuotationComponent.html',
})

export class AttachmentQuotationComponent implements OnInit {
    OnCloseAttachmentDocsInEvent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    QuotationAttachmentsLists: AttachmentsArgs[];
    SelectedQuotationAttachmentsList: AttachmentsArgs;
    SelectedQuotationAttachmentsLists: AttachmentsArgs[];
    TriggerViewModel: any;
    
    constructor() {
        this.SelectedQuotationAttachmentsLists = [];
    }

    ngOnInit(


    ) {


    }


    SetWindowArgs(args: any) {
        this.QuotationAttachmentsLists = args.QuotationAttachmentsLists;

        this.TriggerViewModel = args.TriggerViewModel
    }






    CloseButtonClicked() {

        this.CurrentSession.CurrentWindow.Close("");

    }


    SaveButtonClicked() {
        if (this.TriggerViewModel) {
            if (!this.TriggerViewModel.AttachmentsList) this.TriggerViewModel.AttachmentsList = [];
            this.SelectedQuotationAttachmentsLists.forEach((doc) => {
                if (!this.TriggerViewModel.AttachmentsList.filter(d => d.DocumentId == doc.DocumentId)[0]) {
                    this.TriggerViewModel.AttachmentsList.push(doc);
                }
            });
        }

        this.CloseButtonClicked();
      
    }


    IsSelect: boolean;
    public CheckboxClick(item: AttachmentsArgs) {

        var selectitem = this.QuotationAttachmentsLists.filter(d => d.DocumentId == item.DocumentId)[0];
        if (this.SelectedQuotationAttachmentsLists.filter(d => d.DocumentId == selectitem.DocumentId)[0]) {
            this.SelectedQuotationAttachmentsLists = this.SelectedQuotationAttachmentsLists.filter(d => d.DocumentId != selectitem.DocumentId);
        } else {
            this.SelectedQuotationAttachmentsLists.push(selectitem);
        }

    }



    View(item: AttachmentsArgs) {

        if (item != null) {
            DownloadManager.DownloadPage(item.DocumentId);

        }


    }



}


export class AttachmentsArgs {
    public DocumentFilingPM: DocumentsFilingPM;
    constructor(documentFilingPM: DocumentsFilingPM) {
        this.DocumentFilingPM = documentFilingPM;
    }

    public IsQuotationAttachment: boolean = false;

    private fileName: string = null;
    get FileName() {
        if (this.DocumentFilingPM != null) this.fileName = this.DocumentFilingPM.FileName;
        return this.fileName;
    }
    set FileName(value: string) {
        this.fileName = value;
    }


    private fileExtension: string = null;
    get FileExtension() {
        if (this.DocumentFilingPM != null) this.fileExtension = this.DocumentFilingPM.FileExtension;
        return this.fileExtension;
    }
    set FileExtension(value: string) {
        this.fileExtension = value;
    }



    private tenant: number = null;
    get Tenant() {
        if (this.DocumentFilingPM != null) this.tenant = this.DocumentFilingPM.Tenant;
        return this.tenant;
    }
    set Tenant(value: number) {
        this.tenant = value;
    }


    private documentFilingId: string = null;
    get DocumentFilingId() {
        if (this.DocumentFilingPM != null) this.documentFilingId = this.DocumentFilingPM.Id;
        return this.documentFilingId;
    }
    set DocumentFilingId(value: string) {
        this.documentFilingId = value;
    }



    private entityId: string = null;
    get EntityId() {
        if (this.DocumentFilingPM != null) this.entityId = this.DocumentFilingPM.EntityId;
        return this.entityId;
    }
    set EntityId(value: string) {
        this.entityId = value;
    }



    private objectTableId: string = null;
    get ObjectTableId() {
        if (this.DocumentFilingPM != null) this.objectTableId = this.DocumentFilingPM.ObjectTableId;
        return this.objectTableId;
    }
    set ObjectTableId(value: string) {
        this.objectTableId = value;
    }

    private createdByUserId: string = null;
    get CreatedByUserId() {
        if (this.DocumentFilingPM != null) this.createdByUserId = this.DocumentFilingPM.CreatedByUserId;
        return this.createdByUserId;
    }
    set CreatedByUserId(value: string) {
        this.createdByUserId = value;
    }

    private ownerId: string = null;
    get OwnerId() {
        if (this.DocumentFilingPM != null) this.ownerId = this.DocumentFilingPM.OwnerId;
        return this.ownerId;
    }
    set OwnerId(value: string) {
        this.ownerId = value;
    }


    private updatedByUserId: string = null;
    get UpdatedByUserId() {
        if (this.DocumentFilingPM != null) this.updatedByUserId = this.DocumentFilingPM.UpdatedByUserId;
        return this.updatedByUserId;
    }
    set UpdatedByUserId(value: string) {
        this.updatedByUserId = value;
    }

    private updateDate: Date = null;
    get UpdateDate() {
        if (this.DocumentFilingPM != null) this.updateDate = this.DocumentFilingPM.UpdateDate;
        return this.updateDate;
    }
    set UpdateDate(value: Date) {
        this.updateDate = value;
    }


    private fileSize: number = null;
    get FileSize() {
        if (this.DocumentFilingPM != null) this.fileSize = this.DocumentFilingPM.FileSize;
        return this.fileSize;
    }
    set FileSize(value: number) {
        this.fileSize = value;
    }

    private createDate: Date = null;
    get CreateDate() {
        if (this.DocumentFilingPM != null) this.createDate = this.DocumentFilingPM.CreateDate;
        return this.createDate;
    }
    set CreateDate(value: Date) {
        this.createDate = value;
    }

    private documentId: string = null;
    get DocumentId() {
        if (this.DocumentFilingPM != null) this.documentId = this.DocumentFilingPM.DocumentId;
        return this.documentId;
    }
    set DocumentId(value: string) {
        this.documentId = value;
    }

    ViewAttachment() {
        if (this.DocumentFilingPM) {
            var documentSecurity = this.DocumentFilingPM.SecurityId;
            var link = "/WebPages/CorrespondenceDownloadpage.aspx?id=" + documentSecurity + "~" + this.Tenant;
            window.open(ServiceHelper.GetLogitudeURL() + link);
        }
        else if (!AppTool.IsNullOrEmpty(this.DocumentId)) {
            DownloadManager.DownloadPage(this.DocumentId);
        }
    }
}
