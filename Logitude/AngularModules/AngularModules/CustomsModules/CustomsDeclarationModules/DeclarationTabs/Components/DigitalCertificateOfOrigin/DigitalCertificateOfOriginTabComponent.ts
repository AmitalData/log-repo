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
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
declare var window: any;
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../../../Infrastructure/DataContracts/ImageParameter';
import { Guid } from '../../../../../Infrastructure/Utilities/Guid';
import { variable } from '@angular/compiler/src/output/output_ast';
import { CertificateOfOriginPMService } from 'Customs/Services/StandardPMs/CertificateOfOriginPMService';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { CertificateOfOriginListService } from 'Customs/Services/StandardLists/CertificateOfOriginListService';
import { CustomSendOptionsArgs, SendRequestVIA } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';

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
    public IsOcr = false;

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

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEIN") {
                            if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR")) {
                                if (this.EntityPM.IsDirty) {
                                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                                    const unsub = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                        this.ReloadMyScreen();
                                        unsub.unsubscribe();

                                    });
                                }
                                else {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    this.ReloadMyScreen();
                                }

                            }
                            else
                                this.ReloadMyScreen();
                        }
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

    isNew =  StatusCertificateOfOrigin.IsNew;
    isEdit =  StatusCertificateOfOrigin.IsEdit;
    selectedCertificateOfOrigin= new CertificateOfOriginPM();
    AddNewCertificateOfOrigin(isNewOrEditCertificateOfOrigin:StatusCertificateOfOrigin) {
        // initilize new certificatgetCertificateOfOriginse:
        const newCertificateOfOriginPM = new CertificateOfOriginPM();
        newCertificateOfOriginPM.DeclarationId = this.EntityPM.Id;
        newCertificateOfOriginPM.Tenant = this.EntityPM.Tenant;
        
        // on click item get one CertificateOfOrigin
        var args: any = {
            Decalaration: this.EntityPM,
            CertificateOfOrigin:  isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsEdit ? this.selectedCertificateOfOrigin : newCertificateOfOriginPM ,
            IsNewOrEdit : isNewOrEditCertificateOfOrigin
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1030;
        logWindow.Height = 725;
        
        // Main Title
        let title = TextCodeTranslator.Translate("Customs.Declaration.TH.CertificateOfOrigin");      
        logWindow.Title = isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsEdit && !AppTool.IsNullOrEmpty(this.selectedCertificateOfOrigin.COONumber) ? title += `: ${this.selectedCertificateOfOrigin.COONumber }` : title;

        // Side Title
        let CertificateOfOriginStatus = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.CooStatusCode");
        logWindow.SubTitle = isNewOrEditCertificateOfOrigin == StatusCertificateOfOrigin.IsEdit && !AppTool.IsNullOrEmpty(this.selectedCertificateOfOrigin.CooStatusCodeName) ? CertificateOfOriginStatus += `: ${this.selectedCertificateOfOrigin.CooStatusCodeName}` : null;

        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/CertificateOfOriginComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            this.ReloadMyScreen();
        });
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

    }

    // #101512 copy CertificateOfOrigin
    CopyOfCertificate() {
     
    }
    ShowCertificatePDF(item) {
        this.certificateOfOriginWebService.GetCertificateOfOriginDocumentDeclarationId(this.EntityPM.Id,item.Id)
        .subscribe((myResponse: ServiceResponse) => {
            var myRes = myResponse.Result;
            if (!AppTool.IsNullOrEmpty(myRes.DocumentDeclarationId)) {
                DownloadManager.DownloadPage(myRes.DocumentDeclarationId);
            }
        });
    }
 
    getCertificateOfOrigins() {
        this.certificateOfOriginWebService.GetCertificateOfOriginByID(this.EntityPM.Id,this.EntityPM.Tenant).subscribe(myResult => {   
            
            if (myResult == null) {
                this.CertificateOfOrigins =  [];
            }
            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError && myResponse.Result) {

                    this.CertificateOfOrigins =  myResponse.Result;       
                    this.ItemsSource.Clear();    
                    // change the counter from server
                    let counter = 0;
                    this.CertificateOfOrigins.forEach(certificateOfOrigin=>{
                        certificateOfOrigin.ListCounter = ++counter;
                        this.ItemsSource.Insert(certificateOfOrigin , true);
                    });       
                }
            }
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
    public CertificateOfOriginComprehensiveUpdate: CertificateOfOriginPM[] = [];
    IsComprehensiveUpdateChecked(checked: boolean, item: CertificateOfOriginPM) {

        if (checked) {
            this.CertificateOfOriginComprehensiveUpdate.push(item);
        }
       
    }

    EditButtonClicked(item: CertificateOfOriginPM) {
        this.selectedCertificateOfOrigin = item;
        this.AddNewCertificateOfOrigin(this.isEdit)
    }


    DeleteButtonClicked(item: CertificateOfOriginPM) {
        if(!item) return;
        this.selectedCertificateOfOrigin = item;
        if(!this.selectedCertificateOfOrigin.IsSubmitted){
            
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 300;
            // TODO: change to text code
            confirmWindow.Show("האם למחוק את התעודה?");
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.DeleteSelected(this.selectedCertificateOfOrigin)
                    }
                    else if (confirmWindow.No) {
                        this.selectedCertificateOfOrigin = null;
                    }
            });
        }
        else{
            // TODO: Display some msg? 
        }



        
    }

    lastDeletedItem: CertificateOfOriginPM;
    DeleteSelected(item: CertificateOfOriginPM) {
        this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item;

        var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            SaveCompletedEvent.unsubscribe();
            this.certificateOfOriginWebService.delete(item.Id).subscribe((myResponse: ServiceResponse) => {
                
                if (!myResponse.HasError) {
                    this.ItemsSource.Remove(item);
                    this.ReloadMyScreen();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges();
     
       

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
            // SetEnabled
            return;
        }       
    }

 
}


export enum StatusCertificateOfOrigin {
    IsNew = 'IsNew',
    IsEdit = 'IsEdit',
}
