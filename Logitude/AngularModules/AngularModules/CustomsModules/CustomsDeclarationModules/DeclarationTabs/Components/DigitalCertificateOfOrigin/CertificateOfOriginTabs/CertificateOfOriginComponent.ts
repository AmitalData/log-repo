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


@Component({

    templateUrl: './CertificateOfOriginComponent.html',
    providers: [EntityArgs],
})

export class CertificateOfOriginComponent extends BaseComponent {
    public right: any;
    public CustomSendOptionsButtonCanForcePersonalSign: any;

    public TabsItemsSource: TabItem[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: CertificateOfOriginPM;
    certificateOfOriginPMService: CertificateOfOriginPMService = new CertificateOfOriginPMService();

    
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: any = this;

    isExternalId: boolean;
    requestParams: CreateClientRequestParams;
    responseData: INF_MSG_GenericResponseData;
    clientMessageService: ClientMessagesService = new ClientMessagesService();
    public ValidationErrorsList: string[] = [];
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
        debugger
        
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

        console.log(this.EntityPM);
        debugger
        
        this.EntityPM.IsUnitedInvoices ?  this.EntityPM.IsUnitedInvoices : this.EntityPM.IsUnitedInvoices = false;
        
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        
        if (this.IsNewOrEdit == StatusCertificateOfOrigin.IsNew) {
            this.CurrentSession.CloseCurrentWindow();
            this.certificateOfOriginPMService.insert(this.EntityPM).subscribe((response: any) => {
                var result = response.Result;
                debugger
                
            });
        }
        
        else if (this.IsNewOrEdit == StatusCertificateOfOrigin.IsEdit){
            this.isEntityChange = true;
            
            this.certificateOfOriginPMService.update(this.EntityPM).subscribe((response: any) => {
                var result = response.Result;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CurrentSession.CloseCurrentWindow();

            });

        }
        // if (this.ClientItemsList != null) {
        //     this.ClientItemsList.forEach(element => {
        //         this.clientItemPMService.update(element.ClientItemPM).subscribe((response: any) => {
        //         });
        //     });

        // }

    }


   
    OnMassageDisplayMethod() {
        if (this.requestParams == null) {
            this.requestParams = new CreateClientRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData();
        }
    }
    IsSendDocumentEnabled:boolean = true;
   
  


  
    // ShowClientIndication() {
       
    //     if (this.CurrentEntity.ClientIndications == null || this.CurrentEntity.ClientIndications.length == 0) {
    //         return;
    //     }

    SendButtonClicked(customSendOptionsArgs:any){
        debugger




    }
    CancelButtonClicked() {
        this.EntityPM.RejectChanges();
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

