import { Component, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { CreateClientRequestParams, ClientAdressParams, ClientAddressCommunicationType, ClientDrivingLicenseParams, ClientDrivingLicenseTypeParams } from 'Customs/DataContract/RequestParams/CreateClientRequestParams';

declare var window: any;
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { ClientPMService } from 'Customs/Services/StandardPMs/ClientPMService';
import { ClientsTapagPM } from 'Customs/EntityPMs/ClientsTapagPM';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ClientItemPMService } from 'Customs/Services/StandardPMs/ClientItemPMService';
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


@Component({

    templateUrl: './CertificateOfOriginComponent.html',
    providers: [EntityArgs],
})

export class CertificateOfOriginComponent extends BaseComponent {
    public IsDisplayOnly: boolean = false;
    public right: any;
    public CustomSendOptionsButtonCanForcePersonalSign: any;

    public TabsItemsSource: TabItem[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public CurrentEntity: CertificateOfOriginPM;
    clientPMService: CertificateOfOriginPMService = new CertificateOfOriginPMService();
    // clientItemPMService: ClientItemPMService = new ClientItemPMService();

    public entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: any = this;
    isNewClient: boolean;
    isExternalId: boolean;
    requestParams: CreateClientRequestParams;
    responseData: INF_MSG_GenericResponseData;
    clientMessageService: ClientMessagesService = new ClientMessagesService();
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public isEntityChange: boolean = false;

    // ClientsTapagList: ClientsTapag[] = [];
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
                         this.clientPMService.get(this.CurrentEntity.Id).subscribe((response: any) => {
                             var result = response.Result;
                             if (!AppTool.IsNullOrEmpty(result)) {
                                 this.CurrentEntity = result;
                                 this.entityArgs.EntityPM = this.CurrentEntity;
                             }

                         });
                     }
                 }

             );
             this.isLoad=true;
        });
    }


    DecalarationData:DeclarationPM;
    SetWindowArgs(args: any) {

        this.CurrentEntity = args.CertificateOfOrigin;
        this.DecalarationData = args.Decalaration;

       
        
        this.BuildTabs();
        this.RunComponent();
        this.entityArgs.EntityPM = this.CurrentEntity;
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
    public get AddTapagEnabled() { return this.CurrentEntity ? this.addTapagEnabled : null; }
    public set AddTapagEnabled(newValue: boolean) {
        this.addTapagEnabled = newValue;
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
                                    this.GENERAL.InitTab(this.CurrentEntity,this.DecalarationData, false);
                                });
                        }
                        break;
                    }
            
                    case "MOREDATA": {
                        if (this.MOREDATA == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/MoreData/CertificateOfOriginMoreDetailsTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.MOREDATA = cmpRef.instance;
                                    this.MOREDATA.InitTab(this.CurrentEntity,this.DecalarationData, false);
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

    OkButtonClicked() {
        this.isEntityChange = true;
        if (this.isNewClient) {
            this.clientPMService.insert(this.CurrentEntity).subscribe((response: any) => {
                var result = response.Result;
                this.CurrentSession.CloseCurrentWindow();

            });
        }

        else {
            this.clientPMService.update(this.CurrentEntity).subscribe((response: any) => {
                var result = response.Result;
                // this.CurrentSession.CurrentEditComponent.SaveChanges();
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


    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);

        var errors: string[] = [];
        Validator.TryValidateObject(this.CurrentEntity, "Customs.Client", errors);

        // for (let item of this.CurrentEntity.ClientAddresses) {
        //     Validator.TryValidateObject(item, "Customs.ClientAddress", errors);
        // }

        // if (this.CurrentEntity.ClientAddresses == null || (this.CurrentEntity.ClientAddresses != null && this.CurrentEntity.ClientAddresses.length == 0)) {
        //     errors.push("חובה להזין לפחות כתובת םחת ללקוח");
        // }

       
        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicator("");

            var currRequestParams = new CreateClientRequestParams();
            currRequestParams.LoggingEnabled = true;
            currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
            currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
            currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
            currRequestParams.Tenant = SessionLocator.Tenant;
            currRequestParams.LoggingEntityId = this.CurrentEntity.Id;
            currRequestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.Client')[0].Id;
            currRequestParams.LoggingEntityReference = this.CurrentEntity.Id;
            currRequestParams.IsFakeResponse = true;
            currRequestParams.RequestName = "Add Client Request";
            currRequestParams.ResponseName = "Add Client Response";
            currRequestParams.IsExternalId = this.isExternalId;
            // currRequestParams.ClientTypeSpecificCode = this.CurrentEntity.ClientTypeSpecificCode;
            // currRequestParams.IsActive = this.CurrentEntity.IsActive;
            // currRequestParams.DunsNumber = this.CurrentEntity.DunsNumber;
            // currRequestParams.EnglishBirthPlace = this.CurrentEntity.EnglishBirthPlace;
            // currRequestParams.EnglishCorporationName = this.CurrentEntity.EnglishCorporationName;
            // currRequestParams.EnglishFatherName = this.CurrentEntity.EnglishFatherName;
            // currRequestParams.EnglishFirstName = this.CurrentEntity.EnglishFirstName;
            // currRequestParams.EnglishLastName = this.CurrentEntity.EnglishLastName;
            // currRequestParams.FullName = this.CurrentEntity.FullName;
            // currRequestParams.GenderCode = this.CurrentEntity.GenderCode;
            // currRequestParams.LocalCorporationName = this.CurrentEntity.LocalCorporationName;
            // currRequestParams.LocalFirstName = this.CurrentEntity.LocalFirstName;
            // currRequestParams.LocalLastName = this.CurrentEntity.LocalLastName;
            // currRequestParams.PassportCountryCode = this.CurrentEntity.PassportCountryCode;
            // currRequestParams.PassportExpirationDate = this.CurrentEntity.PassportExpirationDate;
            // currRequestParams.PassportFirstName = this.CurrentEntity.PassportFirstName;
            // currRequestParams.PassportIssueDate = this.CurrentEntity.PassportIssueDate;
            // currRequestParams.PassportLastName = this.CurrentEntity.PassportLastName;
            // currRequestParams.PassportNumber = this.CurrentEntity.PassportNumber;
            // currRequestParams.PassportTypeCode = this.CurrentEntity.PassportTypeCode;
            // currRequestParams.NationalIdentificationNumber = this.CurrentEntity.NationalIdentificationNumber;

            // currRequestParams.BirthDate = this.CurrentEntity.BirthDate;
            // currRequestParams.IsImporter = this.CurrentEntity.IsImporter;
            // currRequestParams.IsExporter = this.CurrentEntity.IsExporter;
            // currRequestParams.ConcurrencyGUID = this.CurrentEntity.ConcurrencyGUID;

           

            // CustomMessageProgressComponent
            //     .ShowProgressBar(this.CurrentSession, currRequestParams.PBId,
            //         "שליחת מסר הקמת ספק", false)
            //     .then((res) => {
            //         this.responseData = res;
            //         this.OnMassageDisplayMethod();
            //     }
            //     ).catch((err) => {
            //         //this.ValidationErrorsList = [];
            //         //     this.ValidationErrorsList.push(err);
            //     });


            this.clientMessageService.CreateClientRequest(currRequestParams)
                .subscribe((myServiceResponse: ServiceResponse) => {
                });

        }

    }

    OnMassageDisplayMethod() {
        if (this.requestParams == null) {
            this.requestParams = new CreateClientRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData();
        }
    }


    CancelButtonClicked() {
        this.CurrentEntity.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }


  
    // ShowClientIndication() {
       
    //     if (this.CurrentEntity.ClientIndications == null || this.CurrentEntity.ClientIndications.length == 0) {
    //         return;
    //     }

    //     var windowArgs: any = {};
    //     windowArgs.CustomerIndicationList =  this.CurrentEntity.ClientIndications;;
    //     windowArgs.IsClientIndication = true
    //     var logitudeWindow = new LogitudeWindow();
    //     logitudeWindow.Width = 470;
    //     logitudeWindow.Height = 520;
    //     logitudeWindow.IsShowCloseButton = false;
    //     logitudeWindow.Title = TextCodeTranslator.Translate("Customs.ClientIndication.O.IndicationClient"); 
    //     logitudeWindow.WindowArgs = windowArgs;
    //     logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/CustomerIndicationComponent');
    // }

}



class TabItem {
    public code: string;
    public textCode: string;
    constructor(Code: string, TextCode: string) {
        this.code = Code;

        this.textCode = TextCode;
    }
}




// export class ClientsTapag extends BaseComponent {
//     public EntityPM: ClientsTapagPM;
//     private Parent: CertificateOfOriginComponent;
//     ObjectTableName: string = "Customs.ClientsTapag";
//     private CurrentSession = SessionLocator.SelectedSession;
//     constructor(item: ClientsTapagPM, parent: CertificateOfOriginComponent) {
//         super();
//         this.EntityPM = item;
//         this.Parent = parent;
//         //this.UIProperties.SetEnabled("TapagNumber", this.ObjectTableName, !this.Parent.IsDisplayOnly);

//     }

//     //#region Properties
//     deleteTapagNumberVisible: boolean = false;
//     public get DeleteTapagNumberVisible() { return this.deleteTapagNumberVisible; }
//     public set DeleteTapagNumberVisible(newValue: boolean) { this.deleteTapagNumberVisible = newValue; }

//     clientsTapagNumber: number = 1;
//     public get ClientsTapagNumber() { return this.clientsTapagNumber; }
//     public set ClientsTapagNumber(newValue: number) { this.clientsTapagNumber = newValue; }

//     public get TapagNumber() { return this.EntityPM.TapagNumber; }
//     public set TapagNumber(newValue: string) {

//         this.EntityPM.TapagNumber = newValue;
//         if (newValue != null) {
//             if (this.Parent.ClientsTapagList.length == 1) {
//                 this.Parent.CurrentEntity.AddClientsTapag(this.EntityPM);
//             }
//             this.Parent.AddTapagEnabled = true;
//         }
//         else {
//             this.Parent.AddTapagEnabled = false;
//         }

//     }
//     //#endregion

//     OnMouseOver() {
//         if (this.ClientsTapagNumber > 1) {
//             this.DeleteTapagNumberVisible = true;
//         }
//     }

//     OnMouseLeave() {
//         if (!this.overCloseButton) {
//             this.DeleteTapagNumberVisible = false;
//         }
//     }

//     // close button
//     overCloseButton: boolean = false;
//     OnIconButtonMouseOver() {
//         this.overCloseButton = true;
//     }

//     OnIconButtonMouseLeave() {
//         this.overCloseButton = false;
//     }


//     DeleteTapagNumberButtonClicked() {

//         var msg = TextCodeTranslator.Translate("Customs.ClientsTapag.O.DeleteTapagNumber");
//         var confirmWindow = new ConfirmWindow();
//         confirmWindow.Width = 400;
//         confirmWindow.Height = 150;
//         confirmWindow.Show(msg);
//         confirmWindow.WindowClosed.subscribe((event: any) => {

//             if (confirmWindow.Yes) { // YES
//                 this.Parent.CurrentEntity.RemoveClientsTapag(this.EntityPM);
//                 this.Parent.BuildClientsTapagList();
//             }
//         });
//     }  
// }




