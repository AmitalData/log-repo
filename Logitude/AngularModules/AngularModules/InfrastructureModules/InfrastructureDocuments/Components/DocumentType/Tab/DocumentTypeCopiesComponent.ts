import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentCopiesViewModel} from '../../DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'DocumentTypeCopiesTab',
    templateUrl: './DocumentTypeCopiesComponent.html',
})

export class DocumentTypeCopiesComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    SelectedDocumentCopiesViewModel: DocumentCopiesViewModel;
    public DocumentTypeCopiesLists: DocumentCopiesViewModel[];
    public IsVisibile: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs) {
        super();
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeCopy", 0).subscribe((response:any) => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) { this.Run(); }
        });
    }

    Run() {
        if (!this.EntityPM.DocumentTypeCopies) return;

        this.DocumentTypeCopiesLists = [];
        this.EntityPM.DocumentTypeCopies.forEach((copy) => {
            this.DocumentTypeCopiesLists.push(new DocumentCopiesViewModel(copy, null, null, null, null, null, null, null));
        });
    }

    CheckboxIsSelectedByDefaultClick(selectedItem: DocumentCopiesViewModel, value: any) {
        if (selectedItem == null) return;

        selectedItem.IsSelectedByDefault = value;
        if (selectedItem.CurrentDocumentTypeCopy) {
            selectedItem.CurrentDocumentTypeCopy.IsSelectedByDefault = value;
        }
    }

    EditClicked(selectedItem: DocumentCopiesViewModel) {
        if (selectedItem == null) return;
        if (!selectedItem.CurrentDocumentTypeCopy) return;

        let logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 150;
        logWindow.Title = "Document Type Copy";
        logWindow.WindowArgs = { Item: selectedItem };
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/Tab/DocumentTypeCopyDetailsComponent');
        logWindow.WindowClosed.subscribe((message: any) => {
            if (message != null) {
                this.SaveChanges(selectedItem, message);
            }
        });
    }

    SaveChanges(selectedItem: DocumentCopiesViewModel, message: any) {
        selectedItem.Name = message;
        selectedItem.CurrentDocumentTypeCopy.Name = message;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
}
