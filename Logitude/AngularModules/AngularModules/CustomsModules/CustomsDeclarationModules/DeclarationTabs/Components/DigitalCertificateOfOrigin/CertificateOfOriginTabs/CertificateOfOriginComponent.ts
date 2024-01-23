import { Component, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { CreateClientRequestParams, ClientAdressParams, ClientAddressCommunicationType, ClientDrivingLicenseParams, ClientDrivingLicenseTypeParams } from 'Customs/DataContract/RequestParams/CreateClientRequestParams';

declare var window: any;
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ClientMessagesService } from 'Customs/Services/WebServices/ClientMessagesService';
import { INF_MSG_GenericResponseData } from 'Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { Validator } from 'Infrastructure/Validators/Validator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AppTool } from 'Infrastructure/Tools';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { CertificateOfOriginPMService } from 'Customs/Services/StandardPMs/CertificateOfOriginPMService';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { StatusCertificateOfOrigin } from '../DigitalCertificateOfOriginTabComponent';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { SendRequestVIA } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CertificateOfOriginListService } from 'Customs/Services/StandardLists/CertificateOfOriginListService';


@Component({

    templateUrl: './CertificateOfOriginComponent.html',
    providers: [EntityArgs],
})

export class CertificateOfOriginComponent extends BaseRequestsSheetMassaging  {
    public right: any;

    public TabsItemsSource: TabItem[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: CertificateOfOriginPM;
    certificateOfOriginPMService: CertificateOfOriginPMService = new CertificateOfOriginPMService();
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    certificateOfOriginListService: CertificateOfOriginListService = new CertificateOfOriginListService();


    public entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: any = this;

    isExternalId: boolean;
    requestParams: CreateClientRequestParams;
    responseData: INF_MSG_GenericResponseData;
    clientMessageService: ClientMessagesService = new ClientMessagesService();
    private CurrentSession = SessionLocator.SelectedSession;
    public isEntityChange: boolean = false;
    public DecalarationData:DeclarationPM;
    public IsNewOrEdit:StatusCertificateOfOrigin;
    isDispalyOnlyStatusList: number[] = [4, 8];


    tapagNumberName = '';
    public isLoad:boolean = false;
    constructor(
        public entityArgs: EntityArgs,
    ) {
        super();
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientsTapag").subscribe((response: any) => {
            this.tapagNumberName = 'TapagNumber';
        });
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientIndication").subscribe((response: any) => {

            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {

                    if (theMessage == "ReloadEntity") {
                        this.certificateOfOriginPMService.get(this.EntityPM.Id).subscribe((response: any) => {
                            var result = response.Result;
                            if (!AppTool.IsNullOrEmpty(result)) {
                                this.EntityPM = result;
                                this.entityArgs.EntityPM = this.EntityPM;
                            }

                        });
                    }
                }

            );
             this.isLoad=true;
        });
    }



    SetWindowArgs(args: any) {

        this.EntityPM = args.CertificateOfOrigin;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;

        this.BuildTabs();
        this.RunComponent();
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.CertificateOfOrigin";


    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }



    addTapagEnabled: boolean;
    public get AddTapagEnabled() { return this.EntityPM ? this.addTapagEnabled : null; }
    public set AddTapagEnabled(newValue: boolean) {
        this.addTapagEnabled = newValue;
    }
    public get IsDisplayOnly() {
        return this.isDispalyOnlyStatusList.includes(Number(this.EntityPM?.CooStatusCode))
    }
    BuildTabs() {
        this.TabsItemsSource = [];

        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        this.TabsItemsSource.push(new TabItem("MOREDATA", "Customs.Declaration.O.MoreData"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "Customs.Declaration.TH.RequestSheet"));
        this.TabsItemsSource.push(new TabItem("ANSWERTOCERTIFICATE", "Customs.CertificateOfOrigin.O.AnswerToCertificate"));


        // this.BuildClientsTapagList();
        this.selectedTabCode = "GENERAL";
    }

    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    private isViewInited = false;
    InitializeComponent() {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    }

    private GENERAL: any = null;
    private MOREDATA: any = null;
    private REQUESTSHEET: any = null;
    private ANSWERTOCERTIFICATE: any = null;

    public ClientItemsList = null;
    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {
                    case "GENERAL": {
                        if (this.GENERAL == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/General/CertificateOfOriginGeneralTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.GENERAL = cmpRef.instance;
                                    this.GENERAL.InitTab(this.EntityPM,this.DecalarationData, this.IsNewOrEdit);
                                });
                        }
                        break;
                    }

                    case "MOREDATA": {
                        if (this.MOREDATA == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/MoreData/CertificateOfOriginMoreDetailsTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.MOREDATA = cmpRef.instance;
                                    this.MOREDATA.InitTab(this.EntityPM,this.DecalarationData, this.IsNewOrEdit); 
                                });
                        }
                        break;
                    }

                    case "REQUESTSHEET": {
                        if (this.REQUESTSHEET == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                                SessionLocator.DynamicLoader.Load("./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.REQUESTSHEET = cmpRef.instance;
                                        this.REQUESTSHEET.IsTitleHidden = false;
                                        this.REQUESTSHEET.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                                        this.REQUESTSHEET.MyRequestOnly = false;
                                        this.REQUESTSHEET.SetEntityArgs(this.entityArgs);
                                    });

                            });
                        }
                        break;
                    }

                    case "ANSWERTOCERTIFICATE": {
                        // Add logic for ANSWERTOCERTIFICATE case here
                        break;
                    }

                    // Add more cases as needed for other options

                }
            }


        }
    }

    SaveButtonClicked() {
        if (!this.EntityPM.CooTypeCode || !this.EntityPM.RequestReasonCode) {// manddatory fields
            this.GENERAL.CheckMandatoryFields();
            return;
        }

        this.EntityPM.IsUnitedInvoices ?  this.EntityPM.IsUnitedInvoices : this.EntityPM.IsUnitedInvoices = false;

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        if (this.IsNewOrEdit == StatusCertificateOfOrigin.IsNew) {
            this.certificateOfOriginPMService.insert(this.EntityPM).subscribe((response: any) => {
                var result = response.Result;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
                
                this.IsNewOrEdit = StatusCertificateOfOrigin.IsEdit;
            });
        }
        
        else if (this.IsNewOrEdit == StatusCertificateOfOrigin.IsEdit){
            this.isEntityChange = true;
            
            this.certificateOfOriginPMService.update(this.EntityPM).subscribe((response: any) => {
                var result = response.Result;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    
    async SendButtonClicked(customSendOptionsArgs:any){
        if(!this.EntityPM.CooTypeCode || !this.EntityPM.RequestReasonCode) {// manddatory fields
            this.GENERAL.CheckMandatoryFields();
            return;
        }

        var requestParams= new CertificateOfOriginRequestRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator.LoggedUserId;
        requestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.CertificateOfOrigin')[0].Id;
        requestParams.LoggingEntityId = this.EntityPM.Id;
        requestParams.RequestVIA = customSendOptionsArgs.SendRequestVIA;
        requestParams.Tenant = SessionLocator.Tenant;
        requestParams.CertificateOfOriginId = this.EntityPM.Id;
        requestParams.DeclarationId = this.DecalarationData.Id;
        requestParams.CustomFileNo = this.DecalarationData.CustomFileNo;
        requestParams.RequestReasonCode = Number(this.EntityPM.RequestReasonCode);
        
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
        this.certificateOfOriginListService.getSingle(this.ResponseData.AppicationId).subscribe(myResult => {   
           
               
                if (!myResult.HasError && myResult.Result) {

                    this.EntityPM =  myResult.Result;       
                    
                    
                }
           
           
        });
      
    }
    
    CancelButtonClicked() {
        // this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
}

class TabItem {
    public code: string;
    public textCode: string;
    constructor(Code: string, TextCode: string) {
        this.code = Code;

        this.textCode = TextCode;
    }
}

