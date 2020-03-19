
import {Component, OnInit, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {AttachmentsArgs} from '../../../../CRMModules/CRMTickets/Components/EditTabs/MainTab/SendEmailComponent';

@Component({
    moduleId: module.id,
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
