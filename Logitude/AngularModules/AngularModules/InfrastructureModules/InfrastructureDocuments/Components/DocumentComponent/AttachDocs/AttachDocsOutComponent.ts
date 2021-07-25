import {Component, OnInit}  from '@angular/core';
import {DocumentOutCopyPM} from '../../../../../Common/EntityPMs/DocumentOutCopyPM';
import {DocumentOutCopyViewModel} from '../DocsOut/ViewModel/DocumentOutCopyViewModel';
import {AttachmentsList} from '../DocsOut/Filters/AttachmentsList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';

@Component({
    
    selector: 'AttachDocsOut',
    templateUrl: './AttachDocsOutComponent.html',
})

export class AttachDocsOutComponent implements OnInit {
    public DocumentCopiesList: DocumentOutCopyViewModel[];

    public SelectedDocumentCopiesList: DocumentOutCopyViewModel;
     public SelectedDocumentsList: DocumentOutCopyViewModel[];
     public AttachmentsLists: AttachmentsList[];
     DataContext: any;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


    }

    ngOnInit(


    ) {





    }


    SetDataContext(dataContext: any) {

        this.DataContext = dataContext;
        this.DocumentCopiesList = [];
        this.DataContext.DocumentCopiesList.forEach((item) => {


            item.Key = Guid.newGuid();
            this.DocumentCopiesList.push(item);
        });

        this.DocumentCopiesList.sort((a, b) => {
            if (a.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase() < b.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase()) {
                return -1;
            }
            else if (a.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase() > b.DocumentTypeCopyNameWithDocumentTypeName.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });


        this.SelectedDocumentsList = new Array<DocumentOutCopyViewModel>();
    }






    CloseButtonClicked() {
        this.DataContext.ReloadFroalaEditor();
        if (this.DataContext.AttachmentsLists.length != null && this.DataContext.AttachmentsLists.length > 0) {
            this.DataContext.IsShowAttachmentList = true;
        }
        this.DataContext.IsEnableLinkDocOout = true;
        this.CurrentSession.CurrentWindow.Close("");
    }



    SaveButtonClicked() {

     
        console.log(this.SelectedDocumentsList)
       
        this.AttachmentsLists = new Array<AttachmentsList>();
      

        this.SelectedDocumentsList.forEach((doc) => {

            var item = new AttachmentsList();
            item.Id = doc.Id;
            item.Tenant = doc.Tenant;
            item.FileSize = doc.FileSize;
            item.DocumentTypeCopyNameWithDocumentTypeName = doc.DocumentTypeCopyNameWithDocumentTypeName;
            item.ShowRemoveLink = true;
            item.DirectionCode = "O";

            this.AttachmentsLists.push(item);
           
        });
   
        console.log(this.AttachmentsLists);
        this.DataContext.BliudAttachmentList(this.AttachmentsLists);
        this.DataContext.IsEnableLinkDocOout = true;
        this.CloseButtonClicked();
    }


    IsSelect: boolean;
    public CheckboxClick(item: DocumentOutCopyViewModel) {

        var selectitem = this.SelectedDocumentsList.filter(d=> d.Id == item.Id)[0];
        if (!item.IsAttachSelect) {
            if (selectitem == null) {
                this.SelectedDocumentsList.push(item);
            }
            item.IsAttachSelect = true;

        }
        else {
            if (selectitem != null) {
                this.SelectedDocumentsList = this.SelectedDocumentsList.filter(d=> d.Id != selectitem.Id);
            }
            item.IsAttachSelect = false;
        }


      
        
    }

}
