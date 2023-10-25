declare var window: any;
import { Component, Input, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { DocumentsFilingPM } from '../../../Common/EntityPMs/DocumentsFilingPM';
import { CustomsDocumentPM } from '../../../Customs/EntityPMs/CustomsDocumentPM';
import { CustomsDocumentsTicketPM } from '../../../Customs/EntityPMs/CustomsDocumentsTicketPM';
import { CustomsDocumentTicketViewModel } from './CustomsDocumentTicketViewModel';
import { RelatedDocumentViewModel } from './RelatedDocumentViewModel';
import { CustDocsTicketWebService } from '../../../Customs/Services/WebServices/CustDocsTicketWebService';
import { CustDocMetaDataValuesWebService } from '../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService';
import { CustomsDocumentMetaDataValuePM } from '../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import { CustomsDocumentsDataProvider } from './CustomsDocumentsDataProvider';
import { ICustomsDocumentsController } from './ICustomsDocumentsController';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ImageLibraryService } from '../../../Common/Services/Others/ImageLibraryService';
import { CustDocRelatedDocsWebService } from '../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { CustomsDocumentPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentPMService';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { CustomsSettingExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { CustomsDocumentsTicketPMService } from 'Customs/Services/StandardPMs/CustomsDocumentsTicketPMService';
import { CustomDocumentTypeMetaDataList } from 'Customs/EntityLists/CustomDocumentTypeMetaDataList';
import { OcrDocumentExtendedListService } from 'Customs/Services/ExtendedLists/OcrDocumentExtendedListService';
import { TruckerList } from 'Common/EntityLists/TruckerList';

@Component({

    templateUrl: './CustomsDocumentsComponent.html',
})

export class CustomsDocumentsComponent
    extends BaseComponent
    implements OnDestroy {
    //****************Inputs****************//
    @Input() ChildEntityId1: string = null;
    @Input() ChildEntityId2: string = null;
    @Input() ChildEntityId3: string = null;
    @Input() ParentEntityCode: string = null;
    //*************properties***************//
    public EntityPM: any;
    public ClosingData: any;

    public ObjectTableName: string;
    public DataContext: any = this;
    public CurrentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public CustomsDocumentsTicketViewModels: CustomsDocumentTicketViewModel[];
    public StaticCustomsDocumentsTicketViewModels: CustomsDocumentTicketViewModel[];
    public RelatedDocuments: RelatedDocumentViewModel[];
    public MetadataValues: CustomsDocumentMetaDataValuePM[];
    public CustomsDocumentsTickets: CustomsDocumentsTicketPM[];
    private customsDocumentsDataProvider: CustomsDocumentsDataProvider;
    private customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
    private customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
    public BuildHeader: boolean = false;
    public FilterSelectedValue: string;
    public DocTypesFilterItems: ApiQueryFilters;
    private customDocumentTypeCode: string;
    public reload_cancelDoc: boolean = false;
    get CustomDocumentTypeCode() { return this.customDocumentTypeCode; }
    set CustomDocumentTypeCode(value: string) {
        if (this.customDocumentTypeCode != value) {
            this.customDocumentTypeCode = value;
            this.FilterCustomsDocumentsTickets();
        }
    }
    public AllTicketsCount: string;
    public NotUploadedCount: string;
    public UploadedCount: string;
    public RequiredDocsCount: string;
    public iCustomsDocumentsController: ICustomsDocumentsController;
    _ImageLibraryService: ImageLibraryService;
    private custDocRelatedDocsWebService: CustDocRelatedDocsWebService;
    public DisplayOnlyMessage: string = "";
    IsRelatedDocsVisible: boolean = true;
    IsWindowMode: boolean = false;
    DontLoadTickets: boolean = false;
    PreventEdit: boolean = false;
    public SelectedDocumentId: string = null;
    DocumentRequestCodeIcon: string = "";
    IsDocumentRequestCodeButton: boolean = false;
    IsDocumentRequestCodeSendDigital: boolean = false;
    DocumentRequestCodeText: string = "";
    ParentEntityCode_args: string = "";
    DontClear: boolean = false;
    bulkUploadDocumentsPermission: boolean = true;

    public customs: string = "עמילות";
    public forwarding: string = "שילוח";
    IsClose: boolean = false;
    //************************************//
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        if (entityArgs.EntityParentPM != null) {
            this.ParentEntityCode_args = entityArgs.EntityParentPM;
        }
        if (entityArgs.IsFromStandAloneScreen)
            this.DontClear = true;
        if (entityArgs.EntityPM && !entityArgs.SkipCtor) {
            this.Start(entityArgs.EntityPM, entityArgs.ObjectTableName, entityArgs.EntityParentPM, entityArgs.IsFromStandAloneScreen ,null,this.IsClose);
        }

        this.bulkUploadDocumentsPermission = FeatureLocator.HasFeaturePermession("Customs.Declaration", "BULKUPLOADDOCUMENTS");
    }
    ngOnDestroy() {
    
        console.log("CustomsDocumentsComponent:ngOnDestroy");
        this.entityArgs = null;
        if (this.CustomsDocumentsTicketViewModels == null) return;
        this.CustomsDocumentsTicketViewModels.forEach((item) => { item.DataContext = null; })
        this.CustomsDocumentsTicketViewModels = null;
    }
    Start(entityPM: any, objectTableName: string, _ParentEntityCode_args: string, IsFromStandAloneScreen: boolean, closingData: any,IsClose:boolean=false) {
        if (_ParentEntityCode_args != null) {
            this.ParentEntityCode_args = _ParentEntityCode_args;
        }
        if (IsFromStandAloneScreen) this.DontClear = true;
        this.EntityPM = entityPM;
        this.ClosingData = closingData;

        if (this.EntityPM.Direction == 'E') {
            
                this.customs =  TextCodeTranslator.Translate('Customs.CustomsDocument.O.CustomFile');
                this.forwarding =  TextCodeTranslator.Translate('Customs.CustomsDocument.O.ExportFile');      
                         

            this.DocumentFilterSelectedValue = "all";
            this.IsClose = IsClose;
        }
        this.ObjectTableName = objectTableName;
        if (!AppTool.IsNullOrEmpty(this.ParentEntityCode_args)) {
            this.ParentEntityCode = this.ParentEntityCode_args;
        }
        else {
            this.ParentEntityCode = this.ObjectTableName.split('.')[1];

        }
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocument").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocumentsTicket").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocumentPointer").subscribe((response: any) => {
                        var timeout = 0;
                        if (!AppTool.IsNullOrEmpty(this.ParentEntityCode_args)) {
                            this.InsureCustomsDocumentsController(true);
                            timeout = 50;
                        }
                        else
                            this.InsureCustomsDocumentsController();



                        //if (AppTool.IsNullOrEmpty(this.customsDocumentsDataProvider)) {
                        //    this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM);
                        //}
                        //if (AppTool.IsNullOrEmpty(this.iCustomsDocumentsController)) {
                        //    this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
                        //}
                        this.IsRelatedDocsVisible = this.iCustomsDocumentsController.IsRelatedDocumentsVisible();
                        this.DisplayOnlyCheck();

                        setTimeout(() => {
                            this.InitiateComponent();
                            this.Listen();
                            this.BuildHeader = true;
                            this.FilterSelectedValue = 'alltickets';
                            this._ImageLibraryService = new ImageLibraryService();
                            this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
                            this.GetDocumentRequestDefaults(this.EntityPM.CustomerCode);
                        }, timeout);


                    });
                });
            });
        });
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.DisplayOnlyCheck();
                        this.InsureCustomsDocumentsController();
                        //this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM);
                        //this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
                        if (this.RefreshDocsScreen) {
                            this.RefreshButtonClicked(this.SelectedDocumentId);
                            this.RefreshDocsScreen = false;
                        }
                        //if (!this.DontLoadTickets) {
                        //    this.InitiateComponent();
                        //    this.FilterSelectedValue = 'alltickets';
                        //}
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCCD") {
                            this.DisplayOnlyCheck();
                            this.RefreshButtonClicked(this.SelectedDocumentId);
                        }
                    }
                })
            );
        }
    }
    InsureCustomsDocumentsController(reload = false) {
        if (AppTool.IsNullOrEmpty(this.customsDocumentsDataProvider)) {
            this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM, null, null, this.ParentEntityCode);
        }
        if (AppTool.IsNullOrEmpty(this.iCustomsDocumentsController)) {
            this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
        }
        else if (reload) {
            this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.EntityPM, null, null, this.ParentEntityCode);

            this.iCustomsDocumentsController = this.customsDocumentsDataProvider.GetCustomsDocumentsController();
        }


    }
    InitiateComponent(selectedDocId: string = null) {

        this.CurrentSession.StartBusyIndicatorLoading();
        this.CustomsDocumentsTickets = [];
        this.MetadataValues = [];
        this.CustomsDocumentsTicketViewModels = [];
        this.StaticCustomsDocumentsTicketViewModels = [];
        var custDocsTicketWebService: CustDocsTicketWebService = new CustDocsTicketWebService();
        var custDocsMetadataWebService: CustDocMetaDataValuesWebService = new CustDocMetaDataValuesWebService();
        //******Getting customs documents ticket for the entity*****//
        // this.DisplayOnlyCheck();
        custDocsTicketWebService.GetCustomsDocumentsTicketsByEntityIdAndChilds(this.EntityPM.Id, null, null, null, this.ParentEntityCode, this.EntityPM.TransportModeId == "A").subscribe((response: ServiceResponse) => {
            this.CustomsDocumentsTickets = response.Result;
            var customsDocTickets: string = "";
            var tickets = this.CustomsDocumentsTickets.filter(d => !AppTool.IsNullOrEmpty(d.DocumentTypeCode));
            var codeValues = "";
            this.DocTypesFilterItems = new ApiQueryFilters();
            for (var j = 0; j < tickets.length; j++) {
                codeValues = codeValues + tickets[j].DocumentTypeCode + ',';
            }
            if (codeValues != null) {
                codeValues = codeValues.substr(0, codeValues.length - 1);
            }
            this.DocTypesFilterItems.addAdditionalFilter("Code", codeValues, null, null, "InListExact", false, false, false, "string", false, true);

            for (var i = 0; i < this.CustomsDocumentsTickets.length; i++) {
                customsDocTickets = customsDocTickets + "," + this.CustomsDocumentsTickets[i].DocumentsFilingId;
            }
            customsDocTickets = customsDocTickets.substr(1, customsDocTickets.length - 1);

            custDocsMetadataWebService.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocTickets).subscribe((response2: ServiceResponse) => {
                
                this.MetadataValues = response2.Result;

                this.GetRelatedDocuments(selectedDocId, true); 
                //this.FillCustomsDocumentsTickets(this.CustomsDocumentsTickets, selectedDocId);
                //this.GetAutoGeneratedTickets(selectedDocId);
                // this.CustomsDocumentsTicketViewModels = [];

                this.CurrentSession.StopBusyIndicator();
            });
        });
    }

    FillCustomsDocumentsTickets(tickets: CustomsDocumentsTicketPM[], selectedDocId: string = null, reload = false) {
        
        if (this.CustomsDocumentsTicketViewModels == null) {
            this.CustomsDocumentsTicketViewModels = [];
        }
        //
        if (this.ParentEntityCode_args == "DeclarationCancellation") {
            if (this.reload_cancelDoc && !this.RefreshDocsScreen) {
                this.reload_cancelDoc = false; return;
            }

            this.reload_cancelDoc = true;
        }



        if (!AppTool.IsNullOrEmpty(this.ParentEntityCode_args)) {
            if (this.ParentEntityCode_args != "ExportDeclarationClosingData" || !this.DontClear) {
                this.CustomsDocumentsTicketViewModels = [];
                this.StaticCustomsDocumentsTicketViewModels = [];
            }
        }
       for (var i = 0; i < tickets.length; i++) {
            var customsDocumentsTicketViewModel: CustomsDocumentTicketViewModel = new CustomsDocumentTicketViewModel(tickets[i], this.MetadataValues, false, this.IsDisplayOnly,
                this.EntityPM, this.ObjectTableName, this.iCustomsDocumentsController);
            customsDocumentsTicketViewModel.DataContext = this;
            this.CustomsDocumentsTicketViewModels.push(customsDocumentsTicketViewModel);
            this.StaticCustomsDocumentsTicketViewModels.push(customsDocumentsTicketViewModel);
        }



        //this.DontClear = false;


        console.log(this.CustomsDocumentsTicketViewModels);
        this.SetFilterCounts();
        if (selectedDocId) {
            var ticket: CustomsDocumentTicketViewModel = this.CustomsDocumentsTicketViewModels.filter(d => d.Id == selectedDocId)[0];
            this.EditCustomsDocumentsTicket(ticket); 
            this.SelectedDocumentId = null;
        }
    }

    GetRelatedDocuments(selectedDocId : string = null, isFirst: boolean = false) {
        if (this.iCustomsDocumentsController.IsRelatedDocumentsVisible()) {
            this.RelatedDocuments = [];
            this.customsDocumentsDataProvider.GetCustomsDocumentsRelatedDocuments(this.DocumentFilterSelectedValue).subscribe((response: ServiceResponse) => {
                var relatedDocs: DocumentsFilingPM[];
                relatedDocs = response.Result;
                for (var i = 0; i < relatedDocs.length; i++) {
                    var ticket = this.CustomsDocumentsTickets.filter(d => d.DocumentsFilingId == relatedDocs[i].Id)[0];
                    var values: CustomsDocumentMetaDataValuePM[] = this.MetadataValues.filter(d => d.CustomsDocumentId == relatedDocs[i].Id);

                    if (!ticket) {
                        var relatedDocViewModel = new RelatedDocumentViewModel(relatedDocs[i], values, this.IsDisplayOnly);
                        this.RelatedDocuments.push(relatedDocViewModel);
                    }
                }
                if(isFirst)
                    this.GetAutoGeneratedTickets(selectedDocId , this.RelatedDocuments);
            });
        }
        else if(isFirst){
            this.GetAutoGeneratedTickets(selectedDocId);
        }
    }

    FilterItemClicked(key: string) {
        this.FilterSelectedValue = key;
        this.FilterCustomsDocumentsTickets();
    }

    DocumentFilterSelectedValue: string = "customs";
    DocumentFilterItemClicked(value: string) {
        this.DocumentFilterSelectedValue = value;
        this.GetRelatedDocuments();
    }
    IsRefreshButtonDisabled: boolean = false;
    RefreshButtonClicked(selectedDocId: string = null) {
        this.IsRefreshButtonDisabled = true;
        let t = setTimeout(() => { this.IsRefreshButtonDisabled = false; clearTimeout(t); }, 1000);
        this.InitiateComponent(selectedDocId);
    }

    FilterCustomsDocumentsTickets() {

        this.CustomsDocumentsTicketViewModels = [];
        if (!AppTool.IsNullOrEmpty(this.CustomDocumentTypeCode)) {
            this.CustomsDocumentsTicketViewModels = this.StaticCustomsDocumentsTicketViewModels.filter(d => d.DocumentTypeCode == this.CustomDocumentTypeCode);
        }
        else {
            this.CustomsDocumentsTicketViewModels = this.StaticCustomsDocumentsTicketViewModels;
        }
        this.SetFilterCounts();
        switch (this.FilterSelectedValue) {
            case 'alltickets': {
                break;
            }
            case 'notuploaded': {
                this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.filter(d => AppTool.IsNullOrEmpty(d.CustomsDocId));
                break;
            }
            case 'uploaded': {
                this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.filter(d => !AppTool.IsNullOrEmpty(d.CustomsDocId));

                break;
            }
            case 'requireddocs': {
                this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.filter(d => !AppTool.IsNullOrEmpty(d.RequestedDocumentId));
                break;
            }
        }
        this.SortCustomsDocumentTickets();
    }

    SetFilterCounts() {
        if (AppTool.IsNullOrEmpty(this.CustomDocumentTypeCode)) {
            this.AllTicketsCount = this.StaticCustomsDocumentsTicketViewModels.length + "";
            this.NotUploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => AppTool.IsNullOrEmpty(d.CustomsDocId)).length + "";
            this.UploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => !AppTool.IsNullOrEmpty(d.CustomsDocId)).length + "";
            this.RequiredDocsCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => !AppTool.IsNullOrEmpty(d.RequestedDocumentId)).length + "";

        }
        else {
            this.AllTicketsCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => d.DocumentTypeCode == this.CustomDocumentTypeCode).length + "";
            this.NotUploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => d.DocumentTypeCode == this.CustomDocumentTypeCode && AppTool.IsNullOrEmpty(d.CustomsDocId)).length + "";
            this.UploadedCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => d.DocumentTypeCode == this.CustomDocumentTypeCode && !AppTool.IsNullOrEmpty(d.CustomsDocId)).length + "";
            this.RequiredDocsCount = this.StaticCustomsDocumentsTicketViewModels.filter(d => d.DocumentTypeCode == this.CustomDocumentTypeCode && !AppTool.IsNullOrEmpty(d.RequestedDocumentId)).length + "";

        }

    }

    GetAutoGeneratedTickets(selectedDocId: string , RelatedDocuments : RelatedDocumentViewModel[] = null) {
           this.iCustomsDocumentsController.GetAutoGeneratedTickets(this.CustomsDocumentsTicketViewModels, this.CustomsDocumentsTickets , RelatedDocuments)
            .subscribe((resp: any) => { // this will push part of the generated tickets.
                var sub = this.iCustomsDocumentsController
                    .GetCustomsInterfaceSettingsDocumentTypesCompleted
                    .subscribe((response: any) => { // added this to add the tickets from CustomsInterfaceSettings
                        var tickets: CustomsDocumentTicketViewModel[] = response.Result; // this is the full result of tickets.
                        sub.unsubscribe();
                        if (!AppTool.IsNullOrEmpty(tickets)) {
                            tickets.forEach((autoGeneratedTicketVM) => {
                                var pm = this.CustomsDocumentsTickets.filter(r => r.DocumentTypeCode == autoGeneratedTicketVM.DocumentTypeCode)[0];
                                //  let task39629SuppressUnique: boolean = true;///CALL#309883 לם נפתח טיקט לכל ח-ן ספק +CALL#309868;310765, 311721
                                //  if (task39629SuppressUnique  || AppTool.IsNullOrEmpty(vm)) {
                                //      ticket.DataContext = this;
                                //      ticket.isDisplayOnly = this.IsDisplayOnly;
                                //      this.CustomsDocumentsTicketViewModels.push(ticket);
                                //      this.StaticCustomsDocumentsTicketViewModels.push(ticket);
                                //}


                                let toAdd: boolean = true;;
                                if (this.iCustomsDocumentsController.CheckIfDuplicateTicket()) {
                                    if (/*autoGeneratedTicketVM.DocumentTypeCode == "380" &&*/ autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers.length == 1) {

                                        this.CustomsDocumentsTickets
                                            .filter(r => r.DocumentTypeCode == autoGeneratedTicketVM.DocumentTypeCode)
                                            .forEach(ticket1 => {
                                                var pointer = ticket1.CustomsDocumentPointers
                                                    .filter(currPointer =>
                                                        currPointer.ParentEntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].ParentEntityId &&
                                                    (currPointer.ParentEntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].ParentEntityCode
                                                        || (ticket1.DocumentTypeCode == '419' && autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].ParentEntityCode == 'ExportDeclarationClosingData')) &&
                                                        currPointer.Child1EntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child1EntityCode &&
                                                        currPointer.Child2EntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child2EntityCode &&
                                                        currPointer.Child3EntityCode == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child3EntityCode &&
                                                        currPointer.Child1EntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child1EntityId &&
                                                        currPointer.Child2EntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child2EntityId &&
                                                        currPointer.Child3EntityId == autoGeneratedTicketVM.customsDocumentsTicketPM.CustomsDocumentPointers[0].Child3EntityId
                                                    )[0];
                                                if (!AppTool.IsNullOrEmpty(pointer)) {
                                                    toAdd = false;
                                                }

                                            });

                                    }
                                }


                                if (!AppTool.IsNullOrEmpty(pm) && autoGeneratedTicketVM.FromCompanyDocumentType2Add) {// already Exist DocumentTypeCode and from   GetCustomsInterfaceSettingsDocumentTypesCompleted
                                    console.log("Task 42286: CALL#315874 טיקטים כפולים לסוגי מסמך");
                                    toAdd = false;
                                }
                               
                                if (toAdd || autoGeneratedTicketVM.IsOcrRelatedDocument) {
                                    autoGeneratedTicketVM.DataContext = this;
                                    autoGeneratedTicketVM.isDisplayOnly = this.IsDisplayOnly;
                                    this.CustomsDocumentsTicketViewModels.push(autoGeneratedTicketVM);
                                    this.StaticCustomsDocumentsTicketViewModels.push(autoGeneratedTicketVM);
                                    //this.CustomsDocumentsTickets.push(ticket);
                                }


                            });
                        }

                        this.FillCustomsDocumentsTickets(this.CustomsDocumentsTickets, selectedDocId);

                        this.iCustomsDocumentsController.FillDefaultMetaData(this.CustomsDocumentsTicketViewModels);
                        this.iCustomsDocumentsController.FillDefaultMetaData(this.StaticCustomsDocumentsTicketViewModels);
                        this.SortCustomsDocumentTickets();
                        this.SetFilterCounts();
                    });

                this.iCustomsDocumentsController.GetCustomsInterfaceSettingsDocumentTypes(this.EntityPM);

            });
    }

    DownloadDocumentFile(documentsFilingId: string) {

        this.custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe((resp: ServiceResponse) => {
            var documentFiling = resp.Result;
            this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.Extension, documentFiling.Folder, SessionLocator.Tenant).subscribe((res: any) => {

 
                var documentName = documentFiling.DocumentId;

                var token = ServiceHelper.GetLDocumentDownloadToken();
                let uri = ServiceHelper.GetLogitudeURL() + "WebPages/Downloadpage.aspx?id=" + documentName + "&tempId=" + token;
                //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
                //    return;
                //}

                DownloadManager.DownloadPage(documentName);

            });
        });

    }



    SortCustomsDocumentTickets() {
        this.CustomsDocumentsTicketViewModels = this.CustomsDocumentsTicketViewModels.sort((a, b) => {
            return (a.DocumentTypeCode === b.DocumentTypeCode) ? 0 : (a.DocumentTypeCode < b.DocumentTypeCode) ? -1 : 1
        });
    }

    DisplayOnlyCheck() {
        this.iCustomsDocumentsController.DisplayOnlyCheck().subscribe((resp: ServiceResponse) => {

            this.IsDisplayOnly = resp.Result.IsDisplayOnly;
            this.DisplayOnlyMessage = resp.Result.DisplayOnlyMessage;
        });
    }



    AddCustomsDocumentsTicket() {
        this.CurrentSession.StartBusyIndicatorLoading();
        var windowArgs: any = {};
        windowArgs.CustomsDocumentsTicket = new CustomsDocumentsTicketPM();
        windowArgs.CustomsDocumentsTicket.Tenant = SessionLocator.Tenant;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        windowArgs.IsNewState = true;
        var entityInfo = this.iCustomsDocumentsController.GetParentAndChildrenEntityCodesAndIds();
        windowArgs.ParentEntityId = entityInfo.ParentEntityId;
        windowArgs.ParentEntityCode = entityInfo.ParentEntityCode;
        windowArgs.Child1EntityId = entityInfo.Child1EntityId;
        windowArgs.Child1EntityCode = entityInfo.Child1EntityCode;
        windowArgs.iCustomsDocumentsController = this.iCustomsDocumentsController;
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.AddCustomsDocument");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));

        logWindow.Show('./CustomsModules/CustomsDocuments/Components/AddEditCustomsDocumentComponent');
        this.CurrentSession.StopBusyIndicator();
    }
    RefreshDocsScreen = false;
    OnAddEditWindowClosed(event) {
        if (event == 'ok') {
            this.RefreshDocsScreen = true;
            //  this.RefreshButtonClicked();
            this.RefreshEntity();
        }
    }

    RefreshEntity() {
        
        var refreshFrom = this.iCustomsDocumentsController.GetRefreshFrom();
        if (refreshFrom == "e") {
            if (this.CurrentSession.CurrentEditComponent) {
                this.RefreshDocsScreen = true;
                this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
            else {
                this.RefreshButtonClicked(this.SelectedDocumentId);
            }
        }
        else if (refreshFrom == "d") {
            this.RefreshButtonClicked(this.SelectedDocumentId);
        }
    }

    private isRequireDocumentTicket: string = null;

    EditCustomsDocumentsTicket(customsDocumentsTicket: CustomsDocumentTicketViewModel) {
        
        if(customsDocumentsTicket.RequestedDocumentId)
            this.isRequireDocumentTicket = customsDocumentsTicket.RequestedDocumentId; 
        else
            this.isRequireDocumentTicket = null;

        if (!customsDocumentsTicket.PreventEdit) {
            if (customsDocumentsTicket.DocumentsFilingId) {
                this.CurrentSession.StartBusyIndicatorLoading();
                var documentsFilingId = encodeURIComponent(customsDocumentsTicket.DocumentsFilingId);
                this.iCustomsDocumentsController.CheckRequestsInProgress(documentsFilingId).subscribe((response: ServiceResponse) => {
                    var isThereRequests = response.Result.IsDisplayOnly;
                    var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
                    customsDocumentPMService.get(documentsFilingId).subscribe((resp: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();
                        if (!resp.HasError) {
                            var customsDoc = resp.Result;
                            if(CustomsDocumentTicketViewModel.IsOcrDocument){

                                this.UpsertSupplierInvioceByOcr(customsDoc.DocumentsFilingId, isThereRequests, customsDocumentsTicket.customsDocumentsTicketPM, customsDoc);
                                CustomsDocumentTicketViewModel.IsOcrDocument = false;
                            }
                            else{

                                this.ApplyEditCustomsDocumentTicket(isThereRequests, customsDocumentsTicket.customsDocumentsTicketPM, customsDoc);

                            }
                        }
                    });

                });
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.ApplyEditCustomsDocumentTicket(false, customsDocumentsTicket.customsDocumentsTicketPM, null);
            }                       
        }

    }

    ApplyEditCustomsDocumentTicket(isThereRequests: boolean, customsDocumentsTicket: CustomsDocumentsTicketPM, customsDocument: CustomsDocumentPM) {
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        }
        var windowArgs: any = {};
        if (customsDocumentsTicket) {
            windowArgs.CustomsDocumentsTicket = customsDocumentsTicket;
            windowArgs.CustomsDocumentsTicket.Tenant = SessionLocator.Tenant;
            if (!customsDocumentsTicket.Id) {
                windowArgs.IsNewState = true;
            }
            else {
                windowArgs.IsNewState = false;
            }
        }          
        windowArgs.ClosingData=this.ClosingData;
        windowArgs.CustomsDocument = customsDocument;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly && isThereRequests;
        windowArgs.IsEntityDisplayOnly = this.IsDisplayOnly;
        windowArgs.IsCustomsDocumentInRequest = isThereRequests;
        var entityInfo = this.iCustomsDocumentsController.GetParentAndChildrenEntityCodesAndIds();
        windowArgs.iCustomsDocumentsController = this.iCustomsDocumentsController;
        windowArgs.ParentEntityId = entityInfo.ParentEntityId;
        windowArgs.ParentEntityCode = entityInfo.ParentEntityCode;
        windowArgs.Child1EntityId = entityInfo.Child1EntityId;
        windowArgs.Child1EntityCode = entityInfo.Child1EntityCode;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.RequestedDocumentId = this.isRequireDocumentTicket;
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditDocumentMetaData");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));
        
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/AddEditCustomsDocumentComponent');
    }

    UpsertSupplierInvioceByOcr(documentFilingId : string, isThereRequests: boolean, customsDocumentsTicket: CustomsDocumentsTicketPM, customsDocument: CustomsDocumentPM) {
        
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        this.custDocRelatedDocsWebService.UpdateSupplierInvioceByOcr(this.EntityPM.Id, documentFilingId).subscribe((response: ServiceResponse) => {
            if (response.HasError) {
                this.CurrentSession.StopBusyIndicator();
                let messageWindow = new MessageWindow();
                messageWindow.Show(response.ErrorsArray[0]); 
                return;
            }
            
            else 
            { 
                this.RefreshEntity();
                var confirmWindow = new ConfirmWindow();
                confirmWindow.ShowNoButton = false;
                confirmWindow.ShowInfoImage = true;
                confirmWindow.Title = TextCodeTranslator.Translate(response.Result);
                confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.Show(response.Result);

                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.ApplyEditCustomsDocumentTicket(isThereRequests, customsDocumentsTicket, customsDocument);

                    }

                });

            }
            
        });
    }

    private IsCustomDocumentItemEnabled(documentStatusCode: string) {
        var MyEditControlViewModelController = this.CurrentSession.CurrentEditComponent.EditComponentController;
        if (MyEditControlViewModelController.InDisplayMode
            || documentStatusCode == "8"
            || this.IsUnifaceLock()) {
            return false;
        }
        return true;
    }

    IsUnifaceLock() {

    }

    EditRelatedDocument(relatedDocumentViewModel: RelatedDocumentViewModel) {
        if (!this.PreventEdit) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var documentsFilingId = relatedDocumentViewModel.Id;
            documentsFilingId = encodeURIComponent(documentsFilingId);
            var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
            customsDocumentPMService.get(documentsFilingId).subscribe((resp: ServiceResponse) => {
                if (!resp.HasError) {
                    relatedDocumentViewModel.CustomDocument = resp.Result;
                    if (relatedDocumentViewModel.CustomDocument) {
                        this.CurrentSession.StopBusyIndicator();
                        this.ApplyEditCustomsDocumentTicket(false, null, relatedDocumentViewModel.CustomDocument);
                    }
                    else {
                        //DocumentsFilingId = CurrentDocument.Id, Tenant = TenantContext.Current.Id, DeclarationId = CurrentDocument.EntityId,
                        //    ExternalEntityName = CurrentDocument.ExternalEntityName,
                        //    ExternalEntityReference = CurrentDocument.ExternalEntityReference,
                        //    DocumentId = CurrentDocument.DocumentId,
                        //    Extension = CurrentDocument.FileExtension,
                        //    CurrentEntityId = CurrentDocument.EntityId,
                        //    FileSize = CurrentDocument.FileSize,
                        var customsDocumentPM: CustomsDocumentPM = new CustomsDocumentPM();
                        customsDocumentPM.DocumentsFilingId = relatedDocumentViewModel.Id;
                        customsDocumentPM.Tenant = SessionLocator.Tenant;
                        customsDocumentPM.DeclarationId = this.EntityPM.Id;
                        customsDocumentPM.CurrentEntityId = this.EntityPM.Id;
                        customsDocumentPM.Extension = relatedDocumentViewModel.Extension;
                        customsDocumentPM.FileSize = relatedDocumentViewModel.FileSize;
                        customsDocumentPM.DocumentId = relatedDocumentViewModel.documentsFilingPM.DocumentId;
                        customsDocumentPM.ExternalEntityName = relatedDocumentViewModel.documentsFilingPM.ExternalEntityName;
                        customsDocumentPM.ExternalEntityReference = relatedDocumentViewModel.documentsFilingPM.ExternalEntityReference;

                        relatedDocumentViewModel.CustomDocument = customsDocumentPM;
                        var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
                        customsDocumentPMService.insert(customsDocumentPM).subscribe((resp: ServiceResponse) => {
                            if (!resp.HasError) {
                                this.CurrentSession.StopBusyIndicator();
                                this.ApplyEditCustomsDocumentTicket(false, null, relatedDocumentViewModel.CustomDocument);
                            }
                            else {
                                this.CurrentSession.StopBusyIndicator();

                                if (this.CurrentSession.CurrentEditComponent) {
                                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = resp.ErrorsArray;
                                }
                                else {
                                    var messageWindow = new MessageWindow();
                                    messageWindow.Width = 400;
                                    messageWindow.Height = 200;
                                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                                    if (resp.ErrorsArray && resp.ErrorsArray.length > 0) {
                                        messageWindow.Show(resp.ErrorsArray[0]);
                                    }
                                    else {
                                        messageWindow.Show("Server Error");
                                    }
                                    messageWindow.WindowClosed.subscribe((event: any) => {

                                        messageWindow.Close();

                                    });
                                }
                            }
                            //this.CurrentSession.StartBusyIndicatorSaving();
                            //                                this.CurrentSession.StopBusyIndicator();

                        });
                    }
                }
            });
        }
    }

    SetWindowArgs(windowArgs) {
        this.IsWindowMode = true;
        this.Start(windowArgs.EntityPM, windowArgs.ObjectTableName, windowArgs.EntityParentPM, windowArgs.IsFromStandAloneScreen, windowArgs.ClosingData,windowArgs.IsClose);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    PreventEditRelatedDoc() {
        this.PreventEdit = true;
    }

    AllowEditRelatedDoc() {
        this.PreventEdit = false;
    }

    public ListOfStatusCode2Show: string[] = ["1", "2"];
    ShowCustomAnswerClicked(event, customsDocumentsTicket: CustomsDocumentTicketViewModel) {
        event.stopPropagation();
        //console.log(customsDocumentsTicket);

        //if (AppTool.IsNullOrEmpty(customsDocumentsTicket.customsDocumentsTicketPM.DocumentStatusCode) ||
        //    AppTool.IsNullOrEmpty(customsDocumentsTicket.customsDocumentsTicketPM.DocumentsFilingId) ||
        //    //entityPm.DocumentStatusCode!= "1" 
        //    this.ListOfStatusCode2Show.indexOf(customsDocumentsTicket.customsDocumentsTicketPM.DocumentStatusCode)==-1
        if (customsDocumentsTicket.ApprovedImageVisibility || customsDocumentsTicket.DeniedImageVisibility) {
        } else {
            if (!customsDocumentsTicket.HaveCustomAnswer) {
                this.EditCustomsDocumentsTicket(customsDocumentsTicket);
                return;
            }
        }




        this.CurrentSession.StartBusyIndicatorLoading();
        let objecttable: any = window.ObjectTables.filter(d => d.Name == "Customs.CustomsDocument")[0];


        var myCommunicationLogStepListService = new CommunicationLogStepListService();
        //logId=1-212245&tenant=1


        myCommunicationLogStepListService
            .GetRequestComminicationIdByEntityId2(SessionLocator.Tenant, "2715", "30", objecttable.Id, customsDocumentsTicket.customsDocumentsTicketPM.DocumentsFilingId)
            .subscribe((rsp: any) => {
                var myCustomsRequestsSheet = rsp.Result;
                this.CurrentSession.StopBusyIndicator();
                if (myCustomsRequestsSheet) {
                    let customsRequestMenuService = new CustomsRequestMenuService();
                    customsRequestMenuService.ShowModalByIdAndIntreface(
                        myCustomsRequestsSheet.RequestComminicationId, "2715", "קלוט צרופה");
                } else {

                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 200;
                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    messageWindow.Show("שליחת המסמך למכס נכשל ( פירוט נוסף בגיליון הבקשות )");

                }
            });


    }

    DocumentRequestbuttonclicked() {

        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
                (myUnifreightMessageM: UnifreightMessageM) => {
                    if (
                        myUnifreightMessageM.LogitudeViewModel == "CustomsDocumentsComponent" &&
                        (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                        myUnifreightMessageM.LogitudeEntityNumber == this.EntityPM.Id) {
                        sub.unsubscribe();
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        this.UnifreightDocumentRequestCallbackAction(myUnifreightMessageM);
                    }
                });
        SessionLocator.SelectedSession.StartBusyIndicator("Loading ...");

        AmitalGatewayUtil.Instance
            .ShowDocumentsSharing(
                this.EntityPM.CustomFileNo,
                this.EntityPM.Id,
                "CustomsDocumentsComponent",
                this.EntityPM.CustomerCode);
    }

    private UnifreightDocumentRequestCallbackAction(unifreightMessageM: UnifreightMessageM) { }

    private GetDocumentRequestDefaults(CustomerCode: string) {
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_SHARE_DESPO", "NON", "NON", SessionLocator.Tenant)
            .subscribe((response: any) => {
                this.IsDocumentRequestCodeButton = false;
                this.IsDocumentRequestCodeSendDigital = false;
                if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {


                    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "GGG_BOX_ACTIVAT", "NON", CustomerCode, SessionLocator.Tenant)
                        .subscribe((response: any) => {
                            this.IsDocumentRequestCodeButton = false;
                            this.IsDocumentRequestCodeSendDigital = false;
                            this.DocumentRequestCodeIcon = "LOGBOX";
                            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                                this.IsDocumentRequestCodeButton = true;
                            }
                            myCustomsSettingExtendedListService.GetDefault("ISRAEL", "GGG_LBL_ACTIVAT", "NON", CustomerCode, SessionLocator.Tenant)
                                .subscribe((res: any) => {
                                    if (!res.HasError && res.Result != null && res.Result.DefaultValue == "Y") {
                                        this.IsDocumentRequestCodeButton = true;
                                        this.IsDocumentRequestCodeSendDigital = true;
                                    }
                                    if (this.IsDocumentRequestCodeSendDigital) {
                                        this.DocumentRequestCodeIcon = "DEFAULT";
                                    }
                                });
                        });
                }
            });
    }


    confirmCheckOrginalDocWindow() {
        var myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CheckOrginalDoc") || 'האם לסמן מסמך מקורי');

        return new Promise<boolean>((resolve, reject) =>
            myConfirmWindow.WindowClosed.subscribe(event =>
                resolve(myConfirmWindow.Yes)))
    }


    async BulkUploadDocuments() {
        let documnetUpload: CustomsDocumentTicketViewModel[] = this.getDocumentReadyToUpload(true);

        if (documnetUpload.length === 0) {

            const msg: MessageWindow = new MessageWindow();
            msg.Show('אין מסמכים לשליחה למכס');
            await new Promise<void>(resolve => msg.WindowClosed.subscribe(() => resolve()));
            return;
        }

        const orginalDocIsCheck: boolean = await this.checkOrginalDoc(documnetUpload);

        if (!orginalDocIsCheck)
            documnetUpload = this.getDocumentReadyToUpload(false);

        if (documnetUpload.length !== this.CustomsDocumentsTicketViewModels.filter(x => !AppTool.IsNullOrEmpty(x.DocumentsFilingId) && AppTool.IsNullOrEmpty(x.CustomsDocId)).length) {
            const msg: MessageWindow = new MessageWindow();
            msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.HaveMandatory") || 'יש מסמכים עם חוסר בנתוני Metadata ולכן מסמכים אילו לא יעלו למכס');
            await new Promise<void>(resolve => msg.WindowClosed.subscribe(() => resolve()));
        }

        this.CurrentSession.StartBusyIndicatorLoading();

        await Promise.all(documnetUpload.map(async (docTicket: CustomsDocumentTicketViewModel) => {
            const doc: CustomsDocumentPM = await this.getCustomDocument(docTicket.DocumentsFilingId);
            doc.IsSendToQueue = true;
            doc.IsMetaDataReady = true;
            await this.SendCustomsDocumentMethod(doc);
        }));

        this.RefreshEntity();

        this.CurrentSession.StopBusyIndicator();

    }

    private getDocumentReadyToUpload(exclude87: boolean): CustomsDocumentTicketViewModel[] {
        return this.CustomsDocumentsTicketViewModels.filter((customDocument: CustomsDocumentTicketViewModel) => {
            return customDocument.DocumentsFilingId && AppTool.IsNullOrEmpty(customDocument.CustomsDocId) &&
                customDocument.CustomsDocumentMetaDataValuePMs &&

                customDocument.CustomDocumentTypeMetaDataLists.filter(x => x.DocumentTypeCode == customDocument.DocumentTypeCode).every((value: CustomDocumentTypeMetaDataList) => {
                    const metaDataType: CustomsDocumentMetaDataValuePM = customDocument.CustomsDocumentMetaDataValuePMs.find(metaDataType => value?.MetaDataTypeCode == metaDataType?.MetaDataTypeCode);
                    return !value?.Mandatory ||
                        (value?.Mandatory && value?.MetaDataTypeCode === '87' && exclude87) ||
                        (metaDataType?.MetaDataValue || metaDataType?.MetaDataValue == '');
                });
        });
    }

    private async checkOrginalDoc(documnetUpload: CustomsDocumentTicketViewModel[]) {
        let documentsFilingIds: string[] = documnetUpload.reduce((res: string[], customDocument: CustomsDocumentTicketViewModel) => {
            const haveIsOrginalDoc: boolean = customDocument.CustomDocumentTypeMetaDataLists.filter(x => x.DocumentTypeCode == customDocument.DocumentTypeCode).some(type => type.MetaDataTypeCode === '87' && type.Mandatory);
            const value: CustomsDocumentMetaDataValuePM = customDocument.CustomsDocumentMetaDataValuePMs.find(d => d?.MetaDataTypeCode === '87');
            if (haveIsOrginalDoc && !value?.MetaDataValue)
                res.push(customDocument.DocumentsFilingId)

            return res;
        }, []);

        if (documentsFilingIds.length === 0) return true;

        const isApprove: boolean = await this.confirmCheckOrginalDocWindow();

        if (isApprove)
            await this.updateCustomsDocuments(documentsFilingIds);

        return isApprove;
    }


    private async updateCustomsDocuments(documentsFilingIds: string[]): Promise<void> {
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));

        await Promise.all(
            documentsFilingIds.map(async documentsFilingId => {
                const doc: CustomsDocumentPM = await this.getCustomDocument(documentsFilingId);
                let value: CustomsDocumentMetaDataValuePM = doc.CustomsDocumentMetaDataValues.find(x => x.MetaDataTypeCode === '87');
                if (!value) {
                    value = new CustomsDocumentMetaDataValuePM(null);
                    value.ChangeSetOp = 'Insert';
                    value.MetaDataTypeCode = '87';
                    value.Tenant = this.EntityPM.Tenant;
                    value.CustomsDocumentId = documentsFilingId;

                    doc.AddCustomsDocumentMetaDataValue(value);
                }
                value.MetaDataValue = 'True'

                return new Promise<void>((resolve, reject) =>
                    new CustomsDocumentPMService().update(doc).subscribe(() => resolve()));
            })
        )

        SessionLocator.SelectedSession.StopBusyIndicator();
    }


    private getCustomDocument(_documentsFilingId: string): Promise<CustomsDocumentPM> {
        const documentsFilingId: string = encodeURIComponent(_documentsFilingId);

        return new Promise<CustomsDocumentPM>((resolve, reject) =>
            this.iCustomsDocumentsController.CheckRequestsInProgress(documentsFilingId).subscribe((response: ServiceResponse) =>
                this.customsDocumentPMService.get(documentsFilingId).subscribe((resp: ServiceResponse) =>
                    resolve(resp.Result)
                )));
    }


    private SendCustomsDocumentMethod(customsDocument: CustomsDocumentPM): Promise<void> {
        return new Promise<void>((resolve, reject) =>
            this.customsDocumentPMService.update(customsDocument).subscribe((docRes: ServiceResponse) =>
                resolve()
            ));
    }

    SubmitTicketChanges(CustomsDocumentsTicket: CustomsDocumentsTicketPM) {
        this.customsDocumentsTicketPMService.update(CustomsDocumentsTicket).subscribe((ticketRes: ServiceResponse) => { });
    }
}

export class RelatedEntityParams {
    public ParentEntityId: string;
    public ChildEntity1Id: string;
    public ChildEntity2Id: string;
    public ChildEntity3Id: string;
    public ParentEntityCode: string;
    public ChildEntity1Code: string;
    public ChildEntity2Code: string;
    public ChildEntity3Code: string;

}
