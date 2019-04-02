import {Component, AfterViewInit, ChangeDetectorRef, OnInit, Input, Output}  from '@angular/core';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ImageLibraryService} from '../../../Common/Services/Others/ImageLibraryService';
import {DocumentsFilingPM}  from '../../../Common/EntityPMs/DocumentsFilingPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {DownloadManager} from '../../../Infrastructure/Utilities/DownloadManager';
@Component({
    moduleId: module.id,
    templateUrl: './DocumentsFilingTemplateComponent.html',
})

export class DocumentsFilingTemplateComponent {

    _ImageLibraryService: ImageLibraryService;
    public rowData: any;
    public FieldName: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        
    }
    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.FieldName = fieldName;
        this.cd.detectChanges();
    }
    FirePreventSelect() {
        this.CurrentSession.PseventRowSelectEvent.emit("document");
    }
    DownloadDocumentFile(documentFiling: DocumentsFilingPM) {
        this.FirePreventSelect();
        this._ImageLibraryService = new ImageLibraryService();
        this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.FileExtension, documentFiling.Folder, SessionLocator.Tenant).subscribe(res => {


                var documentName = documentFiling.DocumentId;


                DownloadManager.DownloadPage(documentName);

            });
        

    }
}
