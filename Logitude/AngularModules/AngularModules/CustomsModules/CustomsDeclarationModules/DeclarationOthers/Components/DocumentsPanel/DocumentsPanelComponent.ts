declare var window: any;
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM'
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator'
import { ServiceHelper } from '../../../../../Infrastructure/Utilities/ServiceHelper'
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools'
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ControlsIdCounter } from '../../../../../Infrastructure/Utilities/ControlsIdCounter';

import { DocumentsFilingPM } from '../../../../../Common/EntityPMs/DocumentsFilingPM';
import { RelatedDocumentViewModel } from '../../../../CustomsDocuments/Components/RelatedDocumentViewModel';

import { CustDocRelatedDocsWebService } from '../../../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';

import { ImageLibraryService } from '../../../../../Common/Services/Others/ImageLibraryService';
import { CustomDocumentViewerService } from '../../../../../Customs/Services/WebServices/CustomDocumentViewerService';
import { CustomsDocumentMetaDataValuePM } from '../../../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import { CustDocMetaDataValuesWebService } from '../../../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsDocumentsDataProvider } from 'CustomsModules/CustomsDocuments/Components/CustomsDocumentsDataProvider';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';

@Component({

    selector: 'DocumentsPanelComponent',
    templateUrl: "DocumentsPanelComponent.html",
})

export class DocumentsPanelComponent {

    EntityPM: DeclarationPM;
    ObjectTable: ObjectTablePM;
    public customs: string = "עמילות";
    public forwarding: string = "שילוח";
    private custDocRelatedDocsWebService: CustDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
    private _ImageLibraryService: ImageLibraryService = new ImageLibraryService();
    private _CustomDocumentViewerService: CustomDocumentViewerService = new CustomDocumentViewerService();
    private custDocsMetadataWebService: CustDocMetaDataValuesWebService = new CustDocMetaDataValuesWebService();
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    constructor() {
        var counter = ControlsIdCounter.GetNextControlIdCounter("DocumentListWrapperId");
        this.DocumentListWrapperId = "DocumentListWrapperId" + counter;
    }


    Run(args: any) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
        if (this.EntityPM.Direction == 'E') {
            this._entityResourceService.getEntityResourceByTableName("Customs.CustomsDocument", 0).subscribe((response: any) => {
                this.customs = TextCodeTranslator.Translate('Customs.CustomsDocument.O.CustomFile');
                this.forwarding = TextCodeTranslator.Translate('Customs.CustomsDocument.O.ExportFile');
                this.DocumentFilterSelectedValue = "all";
            });
            this._entityResourceService.getEntityResourceByTableName("Customs.OcrDocument").subscribe((response: any) => {});

        }
    }

    //#region Documents DDL
    IsDocsPanelVisible: boolean = false;
    DocumentListWrapperId: string;
    DocumentsIconClicked() {
        this.IsDocsPanelVisible = !this.IsDocsPanelVisible;
        if (this.IsDocsPanelVisible == true)
            this.LoadDocuments();
    }
    //#endregion

    //#region Document List
    public RelatedDocuments: RelatedDocumentViewModel[];
    private customsDocumentsDataProvider: CustomsDocumentsDataProvider;

    LoadDocuments() {
        var objecttable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(objecttable?.Name, this.EntityPM, null, null, null);

        this.CurrentSession.StartBusyIndicatorLoading();

        this.customsDocumentsDataProvider.GetCustomsDocumentsRelatedDocuments(this.DocumentFilterSelectedValue)

            .subscribe((response: ServiceResponse) => {
                console.log("[response] GetDocumentsFilingsForRelatedDocuments:", response);
                this.CurrentSession.StopBusyIndicator();

                if (!AppTool.IsNullOrEmpty(response)) {

                    this.RelatedDocuments = [];
                    var relatedDocs: DocumentsFilingPM[];
                    relatedDocs = response.Result;
                    for (var i = 0; i < relatedDocs.length; i++) {
                        //var ticket = this.CustomsDocumentsTickets.filter(d => d.DocumentsFilingId == relatedDocs[i].Id)[0];
                        //var values: CustomsDocumentMetaDataValuePM[] = this.MetadataValues.filter(d => d.CustomsDocumentId == relatedDocs[i].Id);
                        var values = null;

                        //if (!ticket) {
                        var relatedDocViewModel = new RelatedDocumentViewModel(relatedDocs[i], values, true);
                        this.RelatedDocuments.push(relatedDocViewModel);
                        //}
                    }
                    //Load first document
                    //this.TicketItemClicked(this.RelatedDocuments[0]);
                    //this.IsDocsPanelVisible = true;

                }
            });
    }
    DownloadDocumentFile(documentsFilingId: string) {

        this.custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe((resp: ServiceResponse) => {
            var documentFiling = resp.Result;
            this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.Extension, documentFiling.Folder, SessionLocator.Tenant).subscribe((res: any) => {


                var documentName = SessionLocator.Tenant + "_" + documentFiling.DocumentId;


                var token = ServiceHelper.GetLDocumentDownloadToken();
                let uri = ServiceHelper.GetLogitudeURL() + "WebPages/Downloadpage.aspx?id=" + documentName + "&tempId=" + token;
                // if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                //     AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
                //     return;
                // }
                // var win = window.open(uri);
                DownloadManager.DownloadPage(documentName);
            });
        });

    }
    //#endregion

    DocumentFilterSelectedValue: string = "customs";
    DocumentFilterItemClicked(value: string) {
        this.DocumentFilterSelectedValue = value;
        this.LoadDocuments();
        //this.GetRelatedDocuments();
    }
}
