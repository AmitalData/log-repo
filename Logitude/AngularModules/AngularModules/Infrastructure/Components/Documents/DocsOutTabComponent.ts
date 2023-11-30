declare var System: any;
declare var window: any;
import {Component, OnInit, OnDestroy, ElementRef, EventEmitter, Output}  from '@angular/core';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {DocumentTypeList} from '../../../Common/EntityLists/DocumentTypeList';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DocumentOutPM} from '../../../Common/EntityPMs/DocumentOutPM';
import {DocumentOutCopyPM} from '../../../Common/EntityPMs/DocumentOutCopyPM';
import {DateTimeToDatePipe} from '../../../Controls/Pipes/DateTimeToDatePipe';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CommunicationLogExtendedPMService} from '../../../Common/Services/ExtendedPMs/CommunicationLogExtendedPMService';
import {EventTypeExtendedPMService} from '../../../Infrastructure/Services/ExtendedPMs/EventTypeExtendedPMService';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {DocumentOutPMService} from '../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {GeneralDocumentFollowUpHelper} from '../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper';
import {DocumentTypeListService} from '../../../Common/Services/StandardLists/DocumentTypeListService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {DocsOutDataViewModel} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import {CommunicationLogPMViewModel} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/CommunicationLogPMViewModel';
import {DocumentsFilingPM} from '../../../Common/EntityPMs/DocumentsFilingPM';
import {EventTypePM} from '../../../Infrastructure/EntityPMs/EventTypePM';
import {ShipmentFollowUpPM} from '../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {WindowArgs} from '../../../Infrastructure/DataContracts/WindowArgs';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';    
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { CardPMService } from '"../../../Common/Services/StandardPMs/CardPMService';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { ARInvoicePM } from '../../../Invoice/EntityPMs/ARInvoicePM';

@Component({
    
    selector: "DocsOutControl",
    templateUrl: './DocsOutTabComponent.html',
    inputs: ['EntityPM', 'EntityId', 'ObjectTableId', 'TransportModeId', 'ShipmentlevelCode', 'ChildEntityReference', 'ChildObjectTableId', 'ChildEntityId', 'EntityReference', 'ChildrenObjectTableIds', 'InitializeDocsOutForAnotherObjectTable', 'IsCustomFilter', 'CustomFilterValue', 'CustomFilterOperation' , 'ShowMessageDocument'],
    providers: [ DocumentOutPMService, DocumentsFilingExtendedPMService, CommunicationLogExtendedPMService, EventTypeExtendedPMService, DocumentTypeListService],   
})

export class DocsOutTabComponent implements OnInit, OnDestroy {
    public ComponentName: string = "DocsOut";
    @Output() LoadFirstObjectCompleted = new EventEmitter();
    public ObjectTableId: string = "";
    public EntityId: string = "";
    public TransportModeId: string = "";
    public ShipmentlevelCode: string = "";
    public ChildrenObjectTableIds: string = "";
    public ChildEntityReference: string = "";
    public ChildObjectTableId: string = "";
    public ChildEntityId: string = "";
    public EntityReference: string = "";
    public ObjectTableName: string = "";
    public DownloadAllVisibile: boolean = false;
    public HasDocuments: boolean = false;
    public ShowMessageDocument: boolean = false;
    //test
    EntityPM: any;
    public CustomFilterOperation: string = "";
    public CustomFilterValue: string = "";
    public IsCustomFilter: boolean = false;

    public IsShowFollowColum: boolean = false;
    IsStardLoadPage: boolean;
    public FollowUpDocumentTypesLists: DocumentTypeList[];

    public DocumentOuts: DocumentOutPM[];

    public DocumentTypes: DocumentTypeList[];
    AllDocumentTypeList :DocumentTypeList[];



    public ItemsSource: DocsOutDataViewModel[];
    public StaticDocumentsList: DocsOutDataViewModel[];
    public CommunicationLogs: CommunicationLogPMViewModel[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public SelectedInternalDocument: DocsOutDataViewModel;
    public DocumentOutCopyId: string;
    public IsShowAddFromLibraryLink: boolean = false;

    public DocumentInPMs: DocumentsFilingPM[];
    InitializeDocsOutForAnotherObjectTable = new EventEmitter();
    SearchText: string;
    public TabHeaderTextCode: string;
    isPrintRequested: boolean = false;
    isSendRequested: boolean = false;

    IsLoadDocumentOutListsComplete: boolean = false;
    IsLoadDocumenTypeLists: boolean = false;
    IsLoadCommunicationLogsListsComplete: boolean = false;
    IsLoadFollowUpDocumentTypeListsComplete: boolean = false;
    IsOpenSendComponent: boolean = false;
    public DisableSendOriginalCopy: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(
        public entityArgs: EntityArgs,
        public _documentOutPMService: DocumentOutPMService,
        public _communicationLogExtendedPMService: CommunicationLogExtendedPMService,
        public _eventTypeExtendedPMService: EventTypeExtendedPMService,
        public _documentTypeListService: DocumentTypeListService,
        public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService,
        public _elementRef: ElementRef
    ) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DocumentTypes = [];
        this.Listen();
    }

    DownloadAllClick() {

        ServiceLocator.SendTotangoUserActivity("Shipment", "Docs Out Downloaded");

        var service: CardPMService = new CardPMService();
        service.get(SessionLocator.LoggedUserPM.Id).subscribe((res:any) => {
            if (!res.HasError) {

                var link = ServiceHelper.GetLogitudeURL() + "/WebPages/SharedDownloadPage.aspx?id=" + SessionLocator.Tenant + ":" + null + ":ship:" + this.EntityId + ":O:" + ServiceHelper.GetLDocumentDownloadToken();
                var win = window.open(link, '_blank');
                win.focus();
            }
        });


    }


    CheckHasDocuments() {
        this.HasDocuments = false;
        if (this.StaticDocumentsList) {
            this.StaticDocumentsList.forEach(item => {
                if (item.HasFile) {
                    this.HasDocuments = true;
                }
            });
        }
    }
    

    ngOnInit() {
        this.TabHeaderTextCode = "DocsOut.O.DocsOut";// this.ObjectTableName + ".TH.DocsOut";

        var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];
        if (table) {
            this.ObjectTableName = table.Name;
        }

        else this.ObjectTableName = "Shipment";


        if (FeatureLocator.HasFeaturePermession("Shipment", "DOCSOUTDOWNLOADDOCUMENTS") && this.ObjectTableName == "Shipment") {
            this.DownloadAllVisibile = true;
        }




        // Ayman:
        // we need this for Translation
        // Please don't remove it
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.DocsOut";

        if (this.InitializeDocsOutForAnotherObjectTable) {
            this.InitializeDocsOutForAnotherObjectTable.subscribe(($event: any) => {

                this.InitializeDocsOutForAnotherObjectTableMethod($event);
            });

        }

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("DocsOut").subscribe((response:any) => {
                this.IsStardLoadPage = true;
                this.Load();
            });

        });

    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.DocOutChangedEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        
    }
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private DocOutChangedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent) {

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "SHDO") {
                    var followUpDocumenttypeIds: string[] = [];
                    if (this.EntityPM.FollowUps) {
                        this.EntityPM.FollowUps.forEach((item) => {
                            if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                                if (followUpDocumenttypeIds.indexOf(item.DocumentTypeId) == -1) {
                                    followUpDocumenttypeIds.push(item.DocumentTypeId);
                                }
                            }

                        });
                        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                        apiQueryFilters.GetAll = true;
                        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {

                            var pmResponse: ServiceResponse = res;
                            if (!pmResponse.HasError) {
                                this.AllDocumentTypeList = pmResponse.Result;
                                this.FollowUpDocumentTypesLists = this.AllDocumentTypeList.filter(d => followUpDocumenttypeIds.indexOf(d.Id) > -1 && d.IsDocOut); 
                                this.StaticDocumentsList.forEach((doc) => {
                                    doc.HasFollowUp = false;
                                });
                                this.SetFollowUpList();
                            }

                        });

                       


                    }
                }
            });
        }


        if (this.CurrentSession.CurrentEditComponent != null) {

            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        //this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        if (this.isPrintRequested) {
                            this.InitializePrinting();
                        }

                        if (this.isSendRequested) {
                            this.InitializeSending("NONE");
                        }
                    }

                    this.isPrintRequested = false;
                    this.isSendRequested = false;
                });
            } 

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    
                    }
                });
            }








        }










        if (!this.DocOutChangedEvent) {
            this.DocOutChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "RefreshDocumentOutPrint") {
                    this.InitializeDocsOutControl();
                } else if (s == "RefreshDocumentOutSend") {
                    this.LoadDocumentsFilingsWithDocuments(true);
                }
            });
        }
    }

    

    InitializeDocsOutControl() {

        this.IsLoadCommunicationLogsListsComplete = false;
        this.IsLoadDocumentOutListsComplete = false;
        this.IsLoadDocumenTypeLists = false;
        this.IsLoadFollowUpDocumentTypeListsComplete = false;

        this.DocumentOuts = [];
        this.DocumentInPMs = [];
        this.DocumentTypes = [];
        this.FollowUpDocumentTypesLists = [];
        this.CommunicationLogs = [];
        this.ItemsSource = new Array<DocsOutDataViewModel>();
        this.StaticDocumentsList = new Array<DocsOutDataViewModel>();
        this.SelectedInternalDocument = null;
 

        this.LoadDocumentOutLists();
        this.LoadCommunicationLogs();
        this.LoadAllDocumentTypeList();
        this.LoadFollowUpDocumentTypeLists();
       
    }

    InitializeDocsOutForAnotherObjectTableMethod(tableId: any) {

        this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, tableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
            var documentids: string = "";
            var documentOuts: any[] = [];

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                documentOuts = pmResponse.Result;
                if (documentOuts) {
                    documentOuts.forEach((item) => {
                        var docType = this.DocumentTypes.filter(d => d.Id == item.DocumentTypeId)[0];
                        if (docType) {
                            if (!this.StaticDocumentsList) {
                                this.StaticDocumentsList = new Array<DocsOutDataViewModel>();
                            }
                           // var exists = this.StaticDocumentsList.filter(d => d.Id == item.DocumentTypeId)[0];

                           // if (!exists) {
                               var exists = new DocsOutDataViewModel(null, this.EntityId, item.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, item.ChildEntityReference, documentOuts, this.CommunicationLogs, this, this.EntityPM,  "", docType);
                                this.StaticDocumentsList.push(exists);

                                var isfollow: boolean = false;
                                if (this.EntityPM.FollowUps) {
                                    var followUp: any = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == docType.Id && d.Area == "DocOut" && !d.Done)[0];
                                    if (followUp) {
                                        isfollow = true;
                                        exists.HasFollowUp = true;
                                    }
                                }


                           // }

                        }

                        else {

                            documentids += (item.DocumentTypeId + ";");
                        }

                    });

                    //if (!AppTool.IsNullOrEmpty(documentids)) {
                    //    this._documentTypePMService.getDocumentTypesByIds(documentids, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                    //        var pmResponse: ServiceResponse = res;
                    //        if (!pmResponse.HasError) {
                    //            var myResult = pmResponse.Result;
                    //            if (myResult) {
                    //                myResult.forEach((docType) => {
                    //                    var docOut: any = documentOuts.filter(d => d.DocumentTypeId == docType.Id)[0];
                    //                    if (docOut) {
                    //                        var exists = this.StaticDocumentsList.filter(d => d.Id == docOut.DocumentTypeId)[0];
                    //                        if (!exists) {
                    //                            exists = new DocsOutDataViewModel(docType, this.EntityId, docOut.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, docOut.ChildEntityReference, documentOuts, this.CommunicationLogs, this, this.EntityPM);
                    //                            this.StaticDocumentsList.push(exists);

                    //                            var isfollow: boolean = false;
                    //                            if (this.EntityPM.FollowUps) {
                    //                                var followUp: any = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == docType.Id && d.Area == "DocOut" && !d.Done)[0];
                    //                                if (followUp) {
                    //                                    isfollow = true;
                    //                                    exists.HasFollowUp = true;
                    //                                }
                    //                            }
                    //                        }
                    //                    }

                    //                    if (this.DocumentTypes.indexOf(docType) == -1) {
                    //                        this.DocumentTypes.push(docType);
                    //                    }
                    //                });

                    //            }
                    //        }
                     
                    //    });

                    //}
                    
                }
            }

            this.DocumentInPMs = new Array<DocumentsFilingPM>();
            this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(this.EntityId, this.ChildEntityId, tableId, "I", SessionInfo.LoggedUserTenant, false).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    this.DocumentInPMs.concat(myResult); 

                }
            });


        });

    }


    //Load

    Load() {

        if (FeatureLocator.HasFeaturePermession("DocumentType", "FROMLIBRARY")) {
            this.IsShowAddFromLibraryLink = true;
        }
        else {
            this.IsShowAddFromLibraryLink = false;
        }

        if (this.entityArgs && this.entityArgs.ObjectTableName == "ARInvoice" && SessionLocator.AccountingSettingPM.BlockSendInvoiceOriginalCopy) {
            this.DisableSendOriginalCopy = true;
        }
    
    
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
       
        this.InitializeDocsOutControl();



    }
    LoadDocumentsFilingsWithDocuments(isLoadOnlay: boolean = false) {
        this.DocumentInPMs = new Array<DocumentsFilingPM>();
        this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(this.EntityId, this.ChildEntityId, this.ObjectTableId, "I", SessionInfo.LoggedUserTenant, true).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.DocumentInPMs = myResult;
                if (!isLoadOnlay) {
                    this.LoadFirstObjectCompleted.emit("Ready");
                }
                this.CheckHasDocuments();
            }
        });
    }

    LoadAllDocumentTypeList() {

        this.IsLoadDocumenTypeLists = false;
        if (!AppTool.IsNullOrEmpty(this.CustomFilterValue)) {
            this.CustomFilterValue = this.CustomFilterValue.toUpperCase();
        }

        this.AllDocumentTypeList = [];
        this.DocumentTypes = [];
        this.DocumentOuts = [];

        if (this.StaticDocumentsList == null) {
            this.StaticDocumentsList = new Array<DocsOutDataViewModel>();
        }

        var objecttableid: string = "";
        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) && this.ChildObjectTableId) objecttableid = this.ChildObjectTableId;

        else objecttableid = this.ObjectTableId;


        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.AllDocumentTypeList = pmResponse.Result;
            }

        var childrenIds: any[] = !AppTool.IsNullOrEmpty(this.ChildrenObjectTableIds) ? this.ChildrenObjectTableIds.split(',') : [];
        this.DocumentTypes = this.AllDocumentTypeList.filter(a => a.Tenant == SessionInfo.LoggedUserTenant && (a.ObjectTableId == objecttableid || childrenIds.indexOf(a.ObjectTableId)!=-1 ) && a.IsDocOut);
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
            this.CheckHasDocuments();
        }
        this.IsLoadDocumenTypeLists = true;
         this.LoadComplete();
        });
    }

    RefreshButtonClicked() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.Load();
    }

    LoadDocumentOutLists() {

        this.IsLoadDocumentOutListsComplete = false;

        var getDocsOut: boolean = true;
        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) && AppTool.IsNullOrEmpty(this.ChildEntityId)) {
            getDocsOut = false;
        }

        if (getDocsOut) {
            this._documentOutPMService.getDocumentOutsByEntityIdAndObjectTable(this.EntityId, this.ChildEntityId, this.ObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.DocumentOuts = myResult;

                    }
                }
                this.IsLoadDocumentOutListsComplete = true;
                this.LoadComplete();

            });
        }
        else {
            this.IsLoadDocumentOutListsComplete = true;
            this.LoadComplete();
        }

    }

    LoadComplete() {
        if (this.IsLoadCommunicationLogsListsComplete && this.IsLoadDocumentOutListsComplete && this.IsLoadDocumenTypeLists && this.IsLoadFollowUpDocumentTypeListsComplete) {

            if (this.DocumentTypes) {
                if (this.IsCustomFilter) {
                    if (this.CustomFilterOperation) {

                        if (this.CustomFilterOperation == "Equal") {


                            if (this.ShowMessageDocument) {
                                this.DocumentTypes = this.DocumentTypes.filter(d => d.Code == this.CustomFilterValue || d.TemplateFormatCode == 'M');

                            }

                        }
                        else if (this.CustomFilterOperation == "NotEqual") {
                            this.DocumentTypes = this.DocumentTypes.filter(d => d.Code != this.CustomFilterValue);
                        }
                    }
                }
            }
            this.BuildItemsSource();

            this.LoadDocumentsFilingsWithDocuments();
        }
    }

    LoadFollowUpDocumentTypeLists() {
        this.IsLoadFollowUpDocumentTypeListsComplete = false;

        var followUpDocumenttypeIds: string[] = [];
        if (this.EntityPM && this.EntityPM.FollowUps) {
            this.EntityPM.FollowUps.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    if (followUpDocumenttypeIds.indexOf(item.DocumentTypeId) == -1) {
                        followUpDocumenttypeIds.push(item.DocumentTypeId);
                    }
                }

            });
            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
            this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    this.AllDocumentTypeList = pmResponse.Result;
                    this.FollowUpDocumentTypesLists = this.AllDocumentTypeList.filter(d => followUpDocumenttypeIds.indexOf(d.Id) > -1 && d.IsDocOut);
                }

                this.IsLoadFollowUpDocumentTypeListsComplete = true;
                this.LoadComplete();

            });

        } else {
            this.IsLoadFollowUpDocumentTypeListsComplete = true;
            this.LoadComplete();
        }


    }

    LoadCommunicationLogs() {
        this.IsLoadCommunicationLogsListsComplete = false;
        this.CommunicationLogs = new Array<CommunicationLogPMViewModel>();
        this._communicationLogExtendedPMService.getCommunicationLogPMsByEntityId(this.EntityId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    myResult.forEach((item) => {
                        this.CommunicationLogs.push(new CommunicationLogPMViewModel(item));
                    });
                }

            }
            this.IsLoadCommunicationLogsListsComplete = true;
            this.LoadComplete();
            
        });

    }


    onSearchTextChangeEvent(search) {

        if (search) {
            if (search != "Search" && this.StaticDocumentsList) {
                this.ItemsSource = this.StaticDocumentsList.filter(d => d.Name && d.Name.toUpperCase().indexOf(search.toUpperCase()) > -1);
                this.SortItemSource();
            }
        }
        else this.ItemsSource = this.StaticDocumentsList;
    }

    BuildItemsSource() {
      var  documentidsList: string[]= [];
        if (this.DocumentOuts && this.DocumentTypes ) {
            this.DocumentOuts.forEach((item) => {
                var docType = this.DocumentTypes.filter(d => d.Id == item.DocumentTypeId)[0];
                if (docType) {
                    var exists = this.StaticDocumentsList.filter(d => d.Id == item.Id)[0] != null ? true : false;
                    if (!exists) {
                        this.StaticDocumentsList.push(new DocsOutDataViewModel(null, this.EntityId, item.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, item.ChildEntityReference, this.DocumentOuts, this.CommunicationLogs, this, this.EntityPM, "", docType));
                    }
                }
                else {
                    documentidsList.push(item.DocumentTypeId);
                }

            })
            this.CheckHasDocuments();
        }


        this.AllDocumentTypeList.filter(d => documentidsList.indexOf(d.Id) > -1).forEach((item) => {
            if (this.DocumentTypes.indexOf(item) == -1) {
                this.DocumentTypes.push(item);
            }

        });

        this.FillDocumentTypes();

        this.SortItemSource();


            //this._documentTypePMService.getDocumentTypesByIds(documentids, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
            //    var pmResponse: ServiceResponse = res;
            //    if (!pmResponse.HasError) {
            //        var myResult = pmResponse.Result;
            //        if (myResult) {
            //            myResult.forEach((item) => {
            //                if (this.DocumentTypes.indexOf(item) == -1) {
            //                    this.DocumentTypes.push(item);
            //                }
            //            });

            //        }
            //    }
            //    this.FillDocumentTypes();
            //});

        
    }



    SortItemSource() {

        this.ItemsSource = this.ItemsSource.sort();
        this.ItemsSource.sort((a, b) => {
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



    }

    FillDocumentTypes() {

        if (this.StaticDocumentsList == null) {
            this.StaticDocumentsList = new Array<DocsOutDataViewModel>();
        }
        
        if (this.DocumentTypes) {
            this.DocumentTypes.filter(d => d.ObjectTableId == this.ObjectTableId || d.ObjectTableId == this.ChildObjectTableId).forEach((item) => {
                var exists = this.StaticDocumentsList.filter(d => d.Id == item.Id)[0] != null ? true : false;
                if (!exists) {
                    if (item != null) {

                        if (this.CheckIfHasDocumentOut(item) || item.InActive == false) {

                            var docVeiwModel = new DocsOutDataViewModel(null, this.EntityId, this.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, this.ChildEntityReference, this.DocumentOuts, this.CommunicationLogs, this, this.EntityPM, "", item);

                            if (!docVeiwModel.DocumentTypeList.IsReadOnly || (docVeiwModel.DocumentTypeList.IsReadOnly && docVeiwModel.Exists)) {
                                this.StaticDocumentsList.push(docVeiwModel);
                            }

                        }
                    }
                }

            });
        }



        this.SetFollowUpList();

        this.CurrentSession.StopBusyIndicator();
    }




    SetFollowUpList() {
        if (this.EntityPM && this.EntityPM.FollowUps && this.EntityPM.FollowUps.length > 0 && this.FollowUpDocumentTypesLists) {
            this.EntityPM.FollowUps.filter(d => d.Area == "DocOut" && !d.Done && !AppTool.IsNullOrEmpty(d.DocumentTypeId)).forEach((follow) => {
                var docType: DocumentTypeList = this.FollowUpDocumentTypesLists.filter(d => d.Id == follow.DocumentTypeId && d.IsDocOut)[0];
                if (docType) {
                    var exists = this.StaticDocumentsList.filter(d => d.Id == docType.Id)[0];
                    if (!exists) {
                        var docVeiwModel = new DocsOutDataViewModel(null, this.EntityId, this.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, this.ChildEntityReference, this.DocumentOuts, this.CommunicationLogs, this, this.EntityPM, "", docType);
                        docVeiwModel.HasFollowUp = true;
                        this.StaticDocumentsList.push(docVeiwModel);
                    }
                    else {
                        exists.HasFollowUp = true;
                    }
                }

            });


        }




        if (this.ObjectTableName == "Quote") {

            if (!this.EntityPM.IsQuoteDataExternal || !this.EntityPM.IsQuoteDocumentExternal) {
                this.StaticDocumentsList = this.StaticDocumentsList.filter(d => d.DocumentTypeCode != "QUOTE");

            }
        }


        this.ItemsSource = new Array<DocsOutDataViewModel>();
        this.ItemsSource = this.StaticDocumentsList;

        this.ItemsSource.sort((a, b) => {
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






        if (this.ItemsSource && this.ItemsSource.length > 0) {


            if (this.SelectedInternalDocument) {
                this.SelectedInternalDocument = this.ItemsSource.filter(d => d.DocumentTypeId == this.SelectedInternalDocument.DocumentTypeId)[0];
            }

            if (!this.SelectedInternalDocument) {
                this.SelectedInternalDocument = this.ItemsSource[0];
            }

            if (this.SelectedInternalDocument.DocumentTypeList.TemplateFormatCode == "M") {

                this.SelectedInternalDocument.VisibleSendButton = true;
            }
            else {
                this.SelectedInternalDocument.VisiblePrintButton = true;
            }

        }


    }



    CheckIfHasDocumentOut(docType: DocumentTypeList) {
        var hasdoc = false;

        if (docType != null) {
            if (!AppTool.IsNullOrEmpty(this.ChildEntityId)) {
                hasdoc = this.DocumentOuts.filter(d => d.DocumentTypeId == docType.Id && d.ChildEntityId == this.ChildEntityId)[0] ? true : false;
            }
            else {
                hasdoc = this.DocumentOuts.filter(d => d.DocumentTypeId == docType.Id)[0] ? true : false;
            }
        }
        return hasdoc;
    }

    OnSelectedDocumentOutList(item: DocsOutDataViewModel) {
        this.SelectedInternalDocument = item;

        this.ItemsSource.forEach((item) => {
            item.VisiblePrintButton = false;
            item.VisibleSendButton = false;

        });



        if (item.DocumentTypeList.TemplateFormatCode == "M") {
            item.VisibleSendButton = true;

        }
        else {
            item.VisiblePrintButton = true;
        }



    }

    CreateInternalDocument(m: string) {
        this._documentOutPMService.getCreateDocumentOut(this.SelectedInternalDocument.DocumentTypeId, this.EntityId, this.ChildEntityId, this.SelectedInternalDocument.ChildReference, this.ObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    this.SelectedInternalDocument.CurrentDocument = myResult;
                    this.ShowSendControl(m);
                }

            }




        });

    }



    //MouseEvent
    OnmMouseOver(item: DocsOutDataViewModel) {

        this.ItemsSource.forEach((item) => {
            item.VisiblePrintButton = false;
            item.VisibleSendButton = false;

        });

        if (item.DocumentTypeList.TemplateFormatCode == "M") {
            item.VisibleSendButton = true;
        }
        else { item.VisiblePrintButton = true; }

        if (this.SelectedInternalDocument.DocumentTypeList.TemplateFormatCode == "M") { this.SelectedInternalDocument.VisibleSendButton = true; }
        else { this.SelectedInternalDocument.VisiblePrintButton = true; }



    }
    OnmMouseleave(item: DocsOutDataViewModel) {

        this.ItemsSource.forEach((item) => {
            item.VisiblePrintButton = false;
            item.VisibleSendButton = false;
        });

        if (this.SelectedInternalDocument.DocumentTypeList.TemplateFormatCode == "M") { this.SelectedInternalDocument.VisibleSendButton = true; }
        else { this.SelectedInternalDocument.VisiblePrintButton = true; }

    }



    //FollowUp
    OnFollowMouseOver(item: DocsOutDataViewModel) {

        this.ItemsSource.forEach((item) => {
            item.ShowFollowUp = false;

        });
        item.ShowFollowUp = true;

    }
    OnFollowMouseleave(item: DocsOutDataViewModel) {

        this.ItemsSource.forEach((item) => {

            item.ShowFollowUp = false;
        });


    }
    AddFollowUp(item: DocsOutDataViewModel) {
        var generalFollowUpHelper: GeneralDocumentFollowUpHelper = new GeneralDocumentFollowUpHelper(this.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildEntityReference, "DocOut", item, this.EntityPM);
        generalFollowUpHelper.AddFollowUp();

    }


    //Send

    IsSendClose: boolean = false;
    SendButtonClick(item: DocsOutDataViewModel) {
        if (!this.IsOpenSendComponent) {
            this.IsOpenSendComponent = true;
            this.SelectedInternalDocument = item;
            if (this.EntityPM && this.EntityPM.IsDirty) {
                this.isSendRequested = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.InitializeSending("NONE");
            }
        }
 
     

    }
    InitializeSending(m: string) {


        if (m == "NONE") {
            if (this.SelectedInternalDocument.CurrentDocument != null) {
                if (this.SelectedInternalDocument.CurrentDocument.TemplateType == "P") {
                    if (this.SelectedInternalDocument.CurrentDocument.DocumentOutCopies.length != 0) {
                        m = this.SelectedInternalDocument.CurrentDocument.DocumentOutCopies[0].Id;
                    }
                }
                this.ShowSendControl(m);
            }

            else {

                this.CreateInternalDocument(m);

            }

        }


    }


    ShowSendControl(documentOutCopyId: string, title: string = "Send Message") {
        this.IsOpenSendComponent = false;
        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.SelectedInternalDocument.DocumentTypeName + " Sending");
        this.IsSendClose = false;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1000) sendWindowWidth = 1000;
        if (sendWindowHeight < 600) sendWindowHeight = 600;
      
        if (this.SelectedInternalDocument != null) {
            if (this.SelectedInternalDocument.CurrentDocument != null) this.DocumentOutCopyId = documentOutCopyId;
            else this.SelectedInternalDocument.Exists = true;
        }


        this.SelectedInternalDocument.EntityId = this.EntityId;
        this.SelectedInternalDocument.documentOutCopyId = this.DocumentOutCopyId;
        this.SelectedInternalDocument.DocsOutItemsList = this.ItemsSource;
        this.SelectedInternalDocument.IsSend = true;


        this.SelectedInternalDocument.PageRequestSendComponent = "DocOut";


        this.SelectedInternalDocument.ModeSendDocument = title == "Document Editor" ? "preview" : "Edit";

        this.SelectedInternalDocument.EventRefreshName = "CommunicationRefresh";
        var logWindow = new LogitudeWindow();
        logWindow.Width = this.SelectedInternalDocument.WindowWidth = sendWindowWidth;
        logWindow.Height = this.SelectedInternalDocument.WindowHeight = sendWindowHeight;
        logWindow.Title = title;
        logWindow.DataContext = this.SelectedInternalDocument;
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (this.SelectedInternalDocument.ModeSendDocument == "Edit") {
                if ($event == "SendEnd") {
                    if (!this.IsSendClose) {
                        this.IsSendClose = true;
                        this.LoadDocumentsFilingsWithDocuments(true);
                    }
                }
            }
        });


    }



    //Print
    PrintButtonClick(item: DocsOutDataViewModel) {
        this.SelectedInternalDocument = item;

        let validationErrors = this.ValidateEntityPM();
        if (validationErrors.length > 0) {
            this.ShowMessageWindow(validationErrors.toString());
            return;
        }

        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.isPrintRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.InitializePrinting();
        }



    };

    private ValidateEntityPM() {
        let errors = [];
        this.ValidateARInvoicePM(errors);

        return errors;
    }

    private ValidateARInvoicePM(errors) {
        if (!SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "INU")[0]) return;
        if (!this.EntityPM) return;
        if (!(this.EntityPM instanceof ARInvoicePM)) return;
        let sATTransferingStatusCode = "TG";
        if (this.EntityPM.SATTransferStatusCode != sATTransferingStatusCode) return;

        errors.push("You are not allowed to build the document while invoice status is Transferring to SAT");
    }

    public ShowMessageWindow(message: string) {
        let messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message ? message : "Error");
    }

    InitializePrinting() {

    
        var buildingDocumentText: string = TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument");
        if (AppTool.IsNullOrEmpty(buildingDocumentText)) buildingDocumentText = "Building document...";
 

        if (this.SelectedInternalDocument != null) {
            if (this.SelectedInternalDocument.CurrentDocument != null) {
                this.ShowPrintControl();
            }

            else {

                this.CurrentSession.StartBusyIndicator(buildingDocumentText);
                this._documentOutPMService.getCreateDocumentOut(this.SelectedInternalDocument.DocumentTypeId, this.EntityId, this.ChildEntityId, this.ChildEntityReference, this.ObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var documentout = pmResponse.Result;
                        if (documentout) {
                            this.SelectedInternalDocument.CurrentDocument = documentout;
                            this.SelectedInternalDocument.Exists = true;
                            if (documentout.IssuedByUserName && documentout.IssuedDate && documentout.Issued) {
                                this.SelectedInternalDocument.HasFile = true;
                                this.SelectedInternalDocument.IssuedDate = documentout.IssuedDate;
                                this.SelectedInternalDocument.IssuedByUserName = documentout.IssuedByUserName;
                            }


                            this.CurrentSession.StopBusyIndicator();
                            this.ShowPrintControl();
                        }
                    }


                });
            }
        }


    }
    ShowPrintControl() {

     
        var logWindow = new LogitudeWindow();
        logWindow.Width = 760;
        logWindow.Height = 552;
        this.SelectedInternalDocument.EntityId = this.EntityId;
        this.SelectedInternalDocument.DocsOutItemsList = this.ItemsSource;
        this.SelectedInternalDocument.PageRequestSendComponent = "DocOut";
        this.SelectedInternalDocument.EventRefreshName = "CommunicationRefresh";
        logWindow.DataContext = this.SelectedInternalDocument;
        logWindow.Title = "Print " + this.SelectedInternalDocument.DocumentTypeList.Name;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CheckHasDocuments();
            if (this.CurrentSession.CurrentEditComponent) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
            
        });



    }
    


    ViewCommunicationLog(docsOutDataViewModel: DocsOutDataViewModel, communicationLog: CommunicationLogPMViewModel) {
        this.SelectedInternalDocument = docsOutDataViewModel;
        this.SelectedInternalDocument.SelectedCommunicationLogViewMode = communicationLog;
        this.ShowSendControl("", "Document Editor");
    }

    LinkAddDocumentFromLibraryClcik() {

        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.EntityId = this.EntityId;
        windowArgs.TransportModeId = this.TransportModeId;
        windowArgs.ShipmentlevelCode = this.ShipmentlevelCode;
        windowArgs.ChildEntityId = this.ChildObjectTableId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.PageRequest = "DocOut";


        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 550;
        logWindow.Title = "New Documents";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/FromLibrary/AddDocumentTypeFromLibraryComponent");

    }

}
