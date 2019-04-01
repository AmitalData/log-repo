declare var System: any;
declare var window: any;
import {DocumentTypeList} from '../../../Common/EntityLists/DocumentTypeList';
import {DocumentsFilingPM} from '../../../Common/EntityPMs/DocumentsFilingPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {DocumentTypeListExtendedService} from '../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {AppTool} from '../../../Infrastructure/Tools';
import {DocumentTypeListService} from '../../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentsFilingPMService} from '../../../Common/Services/StandardPMs/DocumentsFilingPMService';
import {ImageLibraryService} from '../../../Common/Services/Others/ImageLibraryService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DateTimeToDatePipe} from '../../../Controls/Pipes/DateTimeToDatePipe';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import {DocsInDataViewModel} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsIn/ViewModel/DocsInDataViewModel';

import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {GeneralDocumentFollowUpHelper} from '../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { CardPMService } from '"../../../Common/Services/StandardPMs/CardPMService';

@Component({
    moduleId: module.id,
    selector: "DocsInTabControl",
    templateUrl: './DocsInTabComponent.html',
    inputs: ['EntityPM' , 'EntityId', 'ChildEntityId', 'ObjectTableId', 'ChildObjectTableId', 'TransportModeId', 'ShipmentlevelCode', 'ChildEntityReference'],
    providers: [DocumentTypeListExtendedService, DocumentsFilingExtendedPMService, DocumentsFilingPMService, ServiceArgs, ImageLibraryService, DocumentTypeListService],
})

export class DocsInTabComponent extends BaseComponent implements OnInit {

    public DataContext: DocsInTabComponent=this;
    public ItemsSource: ObservableCollection;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public EntityId: string = "";
    public ChildEntityId: string = "";
    public ObjectTableId: string = "";
    public ChildObjectTableId: string = "";
    public TransportModeId: string = "";
    public ShipmentlevelCode: string = "";
    public ChildEntityReference: string = "";
    public Tenant: number;
    public IsClickToUpload: boolean = false;
    public DownloadAllVisibile: boolean = false;
    public HasDocuments: boolean = false;
    ObjectTableName: string;
    IsShowFollowColum: boolean;
    DocumentsList: DocsInDataViewModel[];
    StaticDocumentsList: DocsInDataViewModel[];

    public DocumentTypes: DocumentTypeList[];
    externalDocs: DocumentsFilingPM[];
    additional: DocumentsFilingPM = null;
    EntityPM: any;
    SelectedExternalViewModel: DocsInDataViewModel;
    DeleteAttachmentButtonEnable: boolean = false;
    IsStardLoadPage: boolean;
    public documentsFilingPMService: DocumentsFilingPMService;
    public UndoReceivedButtonEnable: boolean;
    public TabHeaderTextCode: string;
    AllDocumentTypeList: DocumentTypeList[] = [];

    IsLoadDocumentsFilingListsComplete: boolean = false;
    IsLoadDocumentTypeListsComplete: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeListService: DocumentTypeListService , public _imageLibraryService: ImageLibraryService,public entityArgs: EntityArgs, public _documentTypeListExtendedService: DocumentTypeListExtendedService, public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.TabHeaderTextCode = "DocsIn.O.DocsIn"; // entityArgs.ObjectTableName + ".TH.DocsIn";

        if (this.documentsFilingPMService == null) {
            this.documentsFilingPMService = new DocumentsFilingPMService();

        }
        this.Listen();
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    DownloadAllClick() {

        var service: CardPMService = new CardPMService();
        service.get(SessionLocator.LoggedUserPM.Id).subscribe(res => {
            if (!res.HasError) {

                var link = ServiceHelper.GetLogitudeURL() + "/WebPages/SharedDownloadPage.aspx?id=" + SessionLocator.Tenant + ":" + null + ":ship:" + this.EntityId + ":" + res.Result.PartnerTypeId + ":" + ServiceHelper.GetLDocumentDownloadToken();
                var win = window.open(link, '_blank');
                win.focus();
            }
        });
       

    }


    ngOnInit() {
        var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];
        if (table) {
            this.ObjectTableName = table.Name;
        }

        else this.ObjectTableName = "Shipment";


        if (FeatureLocator.HasFeaturePermession("Shipment", "DOCSINDOWNLOADDOCUMENTS") && this.ObjectTableName == "Shipment") {
            this.DownloadAllVisibile = true;
        }


        // Ayman:
        // we need this for Translation
        // Please don't remove it
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.DocsIn";

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("DocsIn").subscribe(response => {
                this.IsStardLoadPage = true;


                this.Tenant = SessionInfo.LoggedUserTenant;
                if (this.EntityPM && this.EntityPM.FollowUps) {
                    switch (this.ObjectTableName.toLowerCase()) {
                        case "shipment":
                        case "master":
                            {
                                this.IsShowFollowColum = FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups");
                                break;
                            }

                        case "quote":
                            {
                                this.IsShowFollowColum = FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups");
                                break;
                            }
                    }
                }
                

                this.LoadData();
            });

        });


    }


    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    private RefreshDocInEvent: any = null;


    private Listen() {

        if (!this.RefreshDocInEvent) {
            this.RefreshDocInEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "RefreshDocIn") {
                    this.RefreshButtonClicked();
                } 
            });
        }

        if (this.entityArgs.EditComponent) {

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHDI") {
                    this.additional = null;
                    this.LoadAllDocumentTypeList();
                }
            });
        }
    }


    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        //AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }


    onSearchTextChangeEvent(search) {

        if (search) {
            if (search != "Search" && this.StaticDocumentsList) {
                this.DocumentsList = this.StaticDocumentsList.filter(d => d.Name && d.Name && d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1);
                this.DocumentsList = this.SortItemSource(this.DocumentsList);

            }
        }
        else this.DocumentsList = this.StaticDocumentsList;

        this.BuildItemsSource();
    }

    LoadData() {
    
        this.IsLoadDocumentsFilingListsComplete = false;
        this.IsLoadDocumentTypeListsComplete = false;

        this.LoadAllDocumentTypeList();
        this.LoadDocumentsFilingPM(null);
    }

    RefreshButtonClicked() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.LoadData();
    }
    //LoadDocumentTypeLists() {

    //    var objecttableid: string = "";

    //    if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) && this.ChildObjectTableId) objecttableid = this.ChildObjectTableId;

    //    else objecttableid = this.ObjectTableId;
    //    this._documentTypeListExtendedService.getDocumentTypeListsByEnityIdAndTenant(this.TransportModeId, this.ShipmentlevelCode, objecttableid, this.Tenant).subscribe(res => {

    //        var pmResponse: ServiceResponse = res;
    //        if (!pmResponse.HasError) {
    //            var myResult = pmResponse.Result;
    //            if (myResult) {
    //                this.DocumentTypes = myResult;
    //            }
    //        }

    //        this.IsLoadDocumentTypeListsComplete = true;
    //        this.LoadComplete();
    //    });

    //}

    LoadAllDocumentTypeList() {

        this.DocumentTypes = [];
        this.AllDocumentTypeList = [];
        this.IsLoadDocumentTypeListsComplete = false;
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = this.Tenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.AllDocumentTypeList = pmResponse.Result;
            }
            this.FillDocumentTypes();

            this.IsLoadDocumentTypeListsComplete = true;
            this.LoadComplete();
            this.CheckHasDocuments();
        });


    }

    LoadDocumentsFilingPM(theAdditional: DocumentsFilingPM) {
        this.IsLoadDocumentsFilingListsComplete = false;
        this.StaticDocumentsList = [];
        this.additional = theAdditional;

        var getDocsIn = true;
        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) && AppTool.IsNullOrEmpty(this.ChildEntityId)) {
            getDocsIn = false;
        }

        if (getDocsIn) {
            this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(this.EntityId, this.ChildEntityId, this.ObjectTableId, "I", SessionInfo.LoggedUserTenant, false).subscribe(res => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.externalDocs = myResult;
                    }
                }

                this.IsLoadDocumentsFilingListsComplete = true;
                this.LoadComplete();
            });
        }
        else {
            this.IsLoadDocumentsFilingListsComplete = true;
            this.LoadComplete();
        }

    }

    FillDocInList() {
        if (!this.StaticDocumentsList) {
            this.StaticDocumentsList = [];
        }
   
        if (this.DocumentTypes != null) {
            this.DocumentTypes.forEach((docType) => {
                var exists = this.StaticDocumentsList.filter(d => d.Id == docType.Id)[0];
                if (!exists) {
                    var docVeiwModel = new DocsInDataViewModel(null, this, docType, this.EntityId, this.ChildEntityId, this.ChildEntityReference, this.externalDocs, this.ObjectTableId);
                    this.StaticDocumentsList.push(docVeiwModel);
                }

            });
        }

        if (this.EntityPM && this.EntityPM.FollowUps && this.EntityPM.FollowUps.length > 0 && this.AllDocumentTypeList) {
            this.EntityPM.FollowUps.filter(d => d.Area == "DocIn" && !d.Done && !AppTool.IsNullOrEmpty(d.DocumentTypeId)).forEach((follow) => {
                var docType: DocumentTypeList = this.AllDocumentTypeList.filter(d => d.Id == follow.DocumentTypeId && d.IsDocIn)[0];
                if (docType) {
                    var exists = null;
                    if (!AppTool.IsNullOrEmpty(follow.ExternalDocumentId)) {
                        exists = this.StaticDocumentsList.filter(d => d.Id == docType.Id && d.CurrentDocument && follow.ExternalDocumentId == d.CurrentDocument.Id)[0];
                    }
                    if (!exists) {
                        exists = this.StaticDocumentsList.filter(d => d.Id == docType.Id)[0];
                    }
                  
                    if (!exists) {
                        var docVeiwModel = new DocsInDataViewModel(null, this, docType, this.EntityId, this.ChildEntityId, this.ChildEntityReference, this.externalDocs, this.ObjectTableId);
                        docVeiwModel.HasFollowUp = true;
                        this.StaticDocumentsList.push(docVeiwModel);
                    }
                    else {

                        if (!AppTool.IsNullOrEmpty(follow.ExternalDocumentId)) {

                            exists.HasFollowUp = true;
                        }
                        else {

                            var doc = this.StaticDocumentsList.filter(d => d.DocumentTypeId == exists.DocumentTypeId && d.CurrentDocument != null).sort((a, b) => (b.CurrentDocument.CreateDate > a.CurrentDocument.CreateDate) ? -1 : ((a.CurrentDocument.CreateDate < b.CurrentDocument.CreateDate) ? 1 : 0))[0];
                            if (doc) {
                                doc.HasFollowUp = true;
                            }
                            else exists.HasFollowUp = true;
                        }

                    }
                }

            });
            this.CheckHasDocuments();

           
        }

    
        this.DocumentsList = this.SortItemSource(this.StaticDocumentsList);

        this.SelectedExternalViewModel = this.DocumentsList[0];

        if (this.SelectedExternalViewModel) {
            if (!this.SelectedExternalViewModel.Received) {
                this.SelectedExternalViewModel.SetReceivedButtonVisibility = true;
            }
            else {
                this.SelectedExternalViewModel.SetReceivedButtonVisibility = false;
            }
            if (!this.SelectedExternalViewModel.DocumentHasFile) {
                this.SelectedExternalViewModel.SetAttachedButtonVisibility = true;
                this.SelectedExternalViewModel.DownloadButtonVisibility = false;
            }
            else {
                this.SelectedExternalViewModel.SetAttachedButtonVisibility = false;
                this.SelectedExternalViewModel.DownloadButtonVisibility = true;
            }

        }



        if (this.additional != null) {
            var additionalView = this.StaticDocumentsList.filter(d => d.ExternalDocumentId == this.additional.Id)[0];
            if (additionalView != null) {
         
                additionalView.UploadButtonClicked();
            }
        }
        this.CurrentSession.StopBusyIndicator();

        this.BuildItemsSource();
    }

    FillDocumentTypes() {

        if (this.AllDocumentTypeList) {
                var objecttableid: string = "";
                if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) && this.ChildObjectTableId) objecttableid = this.ChildObjectTableId;
                else objecttableid = this.ObjectTableId;
       
                this.DocumentTypes = this.AllDocumentTypeList.filter(d => d.ObjectTableId == objecttableid && d.IsDocIn && !d.InActive);
                if (!AppTool.IsNullOrEmpty(this.TransportModeId)) {
                switch (this.TransportModeId) {
                    case "A":
                        {

                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsAir == true);
                            break;
                        }
                    case "O":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsOcean == true);
                            break;
                        }
                    case "I":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsInland == true);
                            break;
                        }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.ShipmentlevelCode)) {
                switch (this.ShipmentlevelCode) {
                    case "C":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsMaster == true);
                            break;
                        }

                    case "M":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsMaster == true);
                            break;
                        }

                    case "D":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsDirect == true);
                            break;
                        }
                    case "H":
                        {
                            this.DocumentTypes = this.DocumentTypes.filter(a => a.IsHouse == true);
                            break;
                        }
                }

            }
        }


    }


    SortItemSource(ItemsSource: any) {

        ItemsSource.sort((a, b) => {
            if (a.Name.toLowerCase() < b.Name.toLowerCase()) {
                return -1;
            }
            else if (a.Name.toLowerCase() > b.Name.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });
        return ItemsSource;
    }

    OnMouseOver(item: DocsInDataViewModel) {

        var selectedId: string = this.SelectedExternalViewModel ? this.SelectedExternalViewModel.Id:null;

        this.DocumentsList.forEach((item) => {
            if (item.Id != selectedId) {
                item.DownloadButtonVisibility = false;
                item.SetAttachedButtonVisibility = false;
                item.SetReceivedButtonVisibility = false;
            }

            else {
                if (item.DocumentId) {
                    if (this.SelectedExternalViewModel && this.SelectedExternalViewModel.DocumentId) {
                        if (item.CurrentDocument && this.SelectedExternalViewModel && this.SelectedExternalViewModel.CurrentDocument){
                            if (item.CurrentDocument.DocumentId != this.SelectedExternalViewModel.CurrentDocument.DocumentId) {
                                item.DownloadButtonVisibility = false;
                                item.SetAttachedButtonVisibility = false;
                                item.SetReceivedButtonVisibility = false;
                            }
                        }
                    }
                }

            }

        });


        if (!item.Received) {
            item.SetReceivedButtonVisibility = true;
        }
        else {
            item.SetReceivedButtonVisibility = false;
        }
        if (!item.DocumentHasFile) {
            item.SetAttachedButtonVisibility = true;
            item.DownloadButtonVisibility = false;
        }
        else {
            item.SetAttachedButtonVisibility = false;
            item.DownloadButtonVisibility = true;
        }

    }


    OnMouseleave(item: DocsInDataViewModel) {
        var selectedId: string = this.SelectedExternalViewModel ? this.SelectedExternalViewModel.Id : null;
        this.DocumentsList.forEach((item) => {
            if (item.Id != selectedId) {
                item.DownloadButtonVisibility = false;
                item.SetAttachedButtonVisibility = false;
                item.SetReceivedButtonVisibility = false;
            }

            else {
                if (item.DocumentId) {
                    if (this.SelectedExternalViewModel && this.SelectedExternalViewModel.DocumentId) {
                        if (item.CurrentDocument && this.SelectedExternalViewModel.CurrentDocument) {
                            if (item.CurrentDocument.DocumentId != this.SelectedExternalViewModel.CurrentDocument.DocumentId) {
                                item.DownloadButtonVisibility = false;
                                item.SetAttachedButtonVisibility = false;
                                item.SetReceivedButtonVisibility = false;
                            }
                        }
                    }
                }

            }

        });

    }

    OnFollowMouseOver(item: DocsInDataViewModel) {

        this.DocumentsList.forEach((item) => {
            item.ShowFollowUp = false;

        });
        item.ShowFollowUp = true;

    }

    OnFollowMouseleave(item: DocsInDataViewModel) {

        this.DocumentsList.forEach((item) => {
            item.ShowFollowUp = false;
        });


    }



    AddFollowUp(item: DocsInDataViewModel) {

        var generalFollowUpHelper: GeneralDocumentFollowUpHelper = new GeneralDocumentFollowUpHelper(this.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildEntityReference, "DocIn", item, this.EntityPM);
        generalFollowUpHelper.AddFollowUp();


    }


    BuildItemsSource() {

        var itemsCollection: DocsInDataViewModel[] = [];
        this.HasDocuments = false;

        this.DocumentsList.forEach((item) => {
            if (item.DataContext.DocumentHasFile) {
                this.HasDocuments = true;
            }
            itemsCollection.push(item);
        })


        this.ItemsSource.Clear();
               
        this.ItemsSource.AppendCollection(itemsCollection);

    }




    OnSelectedDocumentInList(item: DocsInDataViewModel) {

        this.SelectedExternalViewModel = item;

        if (this.SelectedExternalViewModel.DocumentHasFile) {
            this.DeleteAttachmentButtonEnable = true;
        }
        else {
            this.DeleteAttachmentButtonEnable = false;
        }


        if (this.SelectedExternalViewModel.Received && !this.SelectedExternalViewModel.DocumentHasFile) {
            this.UndoReceivedButtonEnable = true;
        }
        else {
            this.UndoReceivedButtonEnable = false;
        }

        this.DocumentsList.forEach((item) => {

            if (item.Id != this.SelectedExternalViewModel.Id) {
                item.DownloadButtonVisibility = false;
                item.SetAttachedButtonVisibility = false;
                item.SetReceivedButtonVisibility = false;
            }

        });





    }

    IsDeleteAttachment: boolean;
    DeleteAttachmentButtonClicked() {

        if (this.SelectedExternalViewModel != null && this.SelectedExternalViewModel.CurrentDocument) {
            this._documentsFilingExtendedPMService.GetDocumentById(this.SelectedExternalViewModel.CurrentDocument.DocumentId, SessionInfo.LoggedUserTenant).subscribe(res => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var document = pmResponse.Result;
                    if (document) {
                        if (!this.IsDeleteAttachment) {
                            this.IsDeleteAttachment = true;
                            this.CurrentSession.StartBusyIndicator("Saving...");
                            this._imageLibraryService.RemoveFile(document.Id, document.Tenant).subscribe(result => {
                                this.externalDocs = this.externalDocs.filter(d => d.Id != this.SelectedExternalViewModel.ExternalDocumentId); 

                                this.SelectedExternalViewModel.RemoveDocument();
                                this.DeleteAttachmentButtonEnable = false;
                                this.UndoReceivedButtonEnable = true;
                                this.IsDeleteAttachment = false;
                                this.CheckHasDocuments();
                                this.CurrentSession.StopBusyIndicator();

                            });
                        }


                    }

                }



            });
        }
    }

    CheckHasDocuments() {
        this.HasDocuments = false;
        if (this.DocumentsList) {
            this.DocumentsList.forEach(item => {
                if (item.DataContext.DocumentHasFile) {
                    this.HasDocuments = true;
                }
            });
        }
    }



    UndoReceivedButtonClicked() {
        if (this.SelectedExternalViewModel != null) {

            this.SelectedExternalViewModel.UndoReceived();

        }

    }



    LoadComplete() {

        if (this.IsLoadDocumentsFilingListsComplete && this.IsLoadDocumentsFilingListsComplete ) {
            this.StaticDocumentsList = [];
            if (this.externalDocs != null) {
                this.externalDocs.forEach((docin) => {
                    if (this.AllDocumentTypeList != null) {
                        var docType = this.AllDocumentTypeList.filter(d => d.Id == docin.DocumentTypeId && d.InActive == false)[0];
                        if (docType) {
                            var docVeiwModel = new DocsInDataViewModel(docin, this, docType, this.EntityId, docin.ChildEntityId, docin.ChildEntityReference, this.externalDocs, this.ObjectTableId);
                            this.StaticDocumentsList.push(docVeiwModel);
                        }
                    }
                });

                this.FillDocInList();

            }

            this.CurrentSession.StopBusyIndicator();
        }
    }

}
