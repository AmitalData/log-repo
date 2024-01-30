import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomsDocumentPointerService } from '../../../../../Customs/Services/Others/CustomsDocumentPointerService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
declare var window: any;
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../../../Infrastructure/DataContracts/ImageParameter';
import { Guid } from '../../../../../Infrastructure/Utilities/Guid';
import { CertificateOfOriginPMService } from 'Customs/Services/StandardPMs/CertificateOfOriginPMService';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { CertificateOfOriginListService } from 'Customs/Services/StandardLists/CertificateOfOriginListService';
import { SendRequestVIA } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';
import { CertificateOfOriginComponent } from './CertificateOfOriginTabs/CertificateOfOriginComponent';

declare var attachmentUploader, ResultAsArray: any;

@Component({
    selector: 'DigitalCertificateOfOriginTabComponent',

    templateUrl: './DigitalCertificateOfOriginTabComponent.html',
    providers: [DeclarationExtendedListService]
})

export class DigitalCertificateOfOriginTabComponent extends BaseRequestsSheetMassaging implements OnInit {
    public onQueryChangeEvent: any;
    public EntityPM: DeclarationPM;

    public ObjectTableName: string = null;
    public DataContext: any = this;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public declarationPMService: DeclarationPMService = new DeclarationPMService();
    customsDocumentPointerService: CustomsDocumentPointerService
    public UploadFileId: string = Guid.NewRandomString();
    filterImageParameter: ImageParameter;
    certificateOfOriginPMService: CertificateOfOriginPMService;
    certificateOfOriginListService: CertificateOfOriginListService = new CertificateOfOriginListService();
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();


    public ItemsSource: ObservableCollection;
    public CertificateOfOrigins: CertificateOfOriginPM[];
    public CertificateOfOriginItems: ObservableCollection;
    public itemsList: CertificateOfOriginPM[];
    public IsVisible = false;
    public MultiUpdate = false;
    private _entityListService: EntityListService;
    public IsDisplayOnly: boolean = false;
    public ShowStorageStatusMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    LayoutDirection: string = 'ltr';
    NumberOfLoadedItems: number = 500;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    IsDisplayMessage: boolean;

    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef, public declarationExtendedListService: DeclarationExtendedListService) {
        super();

        this.ItemsSource = new ObservableCollection([]);
        this.CertificateOfOriginItems = new ObservableCollection([]);

        this.Listen();
        this._entityListService = new EntityListService();

        this.certificateOfOriginPMService = new CertificateOfOriginPMService();

    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.CertificateOfOrigin").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.CertificateOfOriginInvoice").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.CertificateOfOriginItem").subscribe((response: any) => {
                        var multiUpdateFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "MultiUpdate");
                        if (multiUpdateFeature) {
                            this.MultiUpdate = true;
                        }
                        this.IsVisible = true;
                        this.certificateOfOriginPMService = new CertificateOfOriginPMService();
                        this.ObjectTableName = this.entityArgs.ObjectTableName;
                        this.ReloadMyScreen();
                    });
                });
            });
        });
    }


    public CurrentEditComponentId: string;
    private Listen() {

        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.ReloadMyScreen();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        setTimeout(() => {
                            this.ReloadMyScreen();
                        });
                    }

                })
            );
        }
    }

    ReloadMyScreen() { ///DSV - After Sending to Customs - Enter and Getting Optimistic Concurancy error"
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.getCertificateOfOrigins();
        this.DisplayOnlyCheck();
    }

    isNew = StatusCertificateOfOrigin.IsNew;
    isEdit = StatusCertificateOfOrigin.IsEdit;
    selectedCertificateOfOrigin = new CertificateOfOriginPM();
    AddNewCertificateOfOrigin(isNewOrEditCertificateOfOrigin: StatusCertificateOfOrigin) {
        // initilize new certificatgetCertificateOfOriginse:
        const newCertificateOfOriginPM = new CertificateOfOriginPM();
        newCertificateOfOriginPM.DeclarationId = this.EntityPM.Id;
        newCertificateOfOriginPM.Tenant = this.EntityPM.Tenant;

        // on click item get one CertificateOfOrigin
        var args: any = {
            Decalaration: this.EntityPM,
            CertificateOfOrigin: isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsEdit ? this.selectedCertificateOfOrigin : newCertificateOfOriginPM,
            IsNewOrEdit: isNewOrEditCertificateOfOrigin
        };

        if(isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsNew){
            this.openLogWindow(isNewOrEditCertificateOfOrigin,args);
            return;
        }

        this.getCertificateOfOriginByIDAndopenLogWindow(args.CertificateOfOrigin.Id, this.EntityPM.Id,isNewOrEditCertificateOfOrigin,args);
    }

    ViewInitCompleted($event) {
        this.SelectedRow = this.ItemsSource.Collection[0];
        this.OnRowSelected(this.SelectedRow);
    }


    async SendMsgCooStatusCode(item) {
        var requestParams= new CertificateOfOriginRequestRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator.LoggedUserId;
        requestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.CertificateOfOrigin')[0].Id;
        requestParams.LoggingEntityId = item.Id;
        requestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        requestParams.Tenant = SessionLocator.Tenant;
        requestParams.CertificateOfOriginId = item.Id;
        requestParams.DeclarationId = this.EntityPM.Id;
        requestParams.CustomFileNo = this.EntityPM.CustomFileNo;
        requestParams.RequestReasonCode = 13;



        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,requestParams.PBId,
                "שליחת שאילתא לסטטוס תעודה", true)
            .then((res) => {

                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {

                this.ValidationErrorsList.push(err);
            });


        this.certificateOfOriginWebService.PostCertificateOfOriginRequest(requestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {

            });
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new CertificateOfOriginRequestRequestParams();
        }
        this.RefreshScreen();
    }

    RefreshScreen() {
        if (this.ResponseData == null) {
            return;
        }
        this.ReloadMyScreen();
    }

    ShowCertificatePDF(item) {
        this.certificateOfOriginWebService.GetCertificateOfOriginDocumentDeclarationId(this.EntityPM.Id, item.Id)
            .subscribe((myResponse: ServiceResponse) => {
                var myRes = myResponse.Result;
                if (!AppTool.IsNullOrEmpty(myRes.DocumentDeclarationId)) {
                    DownloadManager.DownloadPage(myRes.DocumentDeclarationId);
                }
            });
    }

    getCertificateOfOrigins() {
        this.certificateOfOriginWebService.GetCertificateOfOriginByID(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(myResult => {

            if (myResult == null) {
                this.CertificateOfOrigins = [];
            }
            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError && myResponse.Result) {

                    this.CertificateOfOrigins = myResponse.Result;
                    this.CertificateOfOrigins.sort((a, b) => parseInt(a.Counter) - parseInt(b.Counter));

                    this.ItemsSource.Clear();
                    // change the counter from server
                    let counter = 0;
                    this.CertificateOfOrigins.forEach(certificateOfOrigin => {
                        certificateOfOrigin.ListCounter = ++counter;
                        this.ItemsSource.Insert(certificateOfOrigin, true);
                    });
                }
            }
        });
    }
    
    getCertificateOfOriginByIDAndopenLogWindow(certificateOfOriginId, declarationId,isNewOrEditCertificateOfOrigin,args) {
        this.certificateOfOriginWebService.GetCertificateOfOriginByIDIncludeChildrens(certificateOfOriginId, declarationId, this.EntityPM.Tenant).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError && myResponse.Result) {
                this.selectedCertificateOfOrigin = myResponse.Result;
                args.CertificateOfOrigin = myResponse.Result;
                this.openLogWindow(isNewOrEditCertificateOfOrigin,args);
            }
        });
    }

    openLogWindow(isNewOrEditCertificateOfOrigin,args){
        if(this.isOpen) return;
        this.isOpen = true;
        var logWindow = new LogitudeWindow();
                logWindow.Width = 1030;
                logWindow.Height = 725;
                // Main Title
                let title = TextCodeTranslator.Translate("Customs.Declaration.TH.CertificateOfOrigin");
                logWindow.Title = isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsEdit && !AppTool.IsNullOrEmpty(this.selectedCertificateOfOrigin.COONumber) ? title += `: ${this.selectedCertificateOfOrigin.COONumber}` : title;
        
                // Side Title
                let CertificateOfOriginStatus = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.CooStatusCode");
                logWindow.SubTitle = isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsEdit && !AppTool.IsNullOrEmpty(this.selectedCertificateOfOrigin.CooStatusCodeName) ? CertificateOfOriginStatus += `: ${this.selectedCertificateOfOrigin.CooStatusCodeName}` : null;
        
                logWindow.WindowArgs = args;
                logWindow.ShowCloseButton = true;
                logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/CertificateOfOriginComponent');
                logWindow.WindowClosed.subscribe(($event: any) => {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.ReloadMyScreen();
                    this.isOpen = false;
                });
    }

    public SelectedRow: CertificateOfOriginPM = null;
    public SelectedRowB4Refresh: CertificateOfOriginPM = null;
    OnRowSelected(itemComponent: CertificateOfOriginPM) {
        this.SelectedRow = itemComponent;
        this.SelectedRowB4Refresh = this.SelectedRow;
        this.filterAgrs = new ApiQueryFilters();

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    filterAgrs: ApiQueryFilters;
    isOpen:boolean;
    EditButtonClicked(item: CertificateOfOriginPM) {
        this.selectedCertificateOfOrigin = item;
        this.AddNewCertificateOfOrigin(this.isEdit);
    }

    // #101512 copy CertificateOfOrigin
    CopyOfCertificate(item: CertificateOfOriginPM) {

        this.certificateOfOriginWebService.GetCertificateOfOriginByIDIncludeChildrens(item.Id,  this.EntityPM.Id, this.EntityPM.Tenant).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError && myResponse.Result) {
                this.selectedCertificateOfOrigin = myResponse.Result;
            
                let newCertificateOfOriginPM = new CertificateOfOriginPM();
                newCertificateOfOriginPM = this.selectedCertificateOfOrigin;
                newCertificateOfOriginPM.Id = null;
                newCertificateOfOriginPM.Counter = null;
                newCertificateOfOriginPM.COONumber = null;
                newCertificateOfOriginPM.CooStatusCode = null;
                newCertificateOfOriginPM.CooStatusCodeName = null;
                
                newCertificateOfOriginPM.IsUnitedInvoices ? newCertificateOfOriginPM.IsUnitedInvoices : newCertificateOfOriginPM.IsUnitedInvoices = false;
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        
                this.certificateOfOriginPMService.insert(newCertificateOfOriginPM).subscribe((response: any) => {
                    if (!response.HasError) {
                        var result = response.Result;
                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                        // this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.CurrentSession.StopBusyIndicator();
            
                        // open the copied certificate on edit mode:
                        this.selectedCertificateOfOrigin = result;
                        this.AddNewCertificateOfOrigin(this.isEdit);
                    }
                });
            }
        });
    }

    DeleteButtonClicked(item: CertificateOfOriginPM) {
        if (!item) return;
        this.selectedCertificateOfOrigin = item;
        if (!this.selectedCertificateOfOrigin.IsSubmitted) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 300;
            // TODO: change to text code
            // let deleteCertificate = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.DeleteCertificate");
            // confirmWindow.Show("האם למחוק את התעודה?");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.DeleteCertificate"));
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.DeleteSelected(this.selectedCertificateOfOrigin);
                }
                else if (confirmWindow.No) {
                    this.selectedCertificateOfOrigin = null;
                }
            });
        }
    }

    lastDeletedItem: CertificateOfOriginPM;
    DeleteSelected(item: CertificateOfOriginPM) {
        this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item;
        this.certificateOfOriginWebService.delete(item.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSource.Remove(item);
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.EntityPM.AmendmentMessage != null && this.EntityPM.AmendmentMessage != "") {
            {
                this.IsDisplayMessage = true;

                this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
                if (this.EntityPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.EntityPM.IsAmendmentDisplayOnly;
            }
        }
        else if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            return;
        }
    }
}


export enum StatusCertificateOfOrigin {
    IsNew = 'IsNew',
    IsEdit = 'IsEdit',
}
