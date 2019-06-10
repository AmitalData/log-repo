import {Component, ViewChildren, QueryList}  from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ClientPM} from '../../../../Customs/EntityPMs/ClientPM';
import {ClientPMService} from '../../../../Customs/Services/StandardPMs/ClientPMService'
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {CreateClientRequestParams, ClientAdressParams, ClientAddressCommunicationType, ClientDrivingLicenseParams, ClientDrivingLicenseTypeParams} from '../../../../Customs/DataContract/RequestParams/CreateClientRequestParams';
import {INF_MSG_GenericResponseData} from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
declare var window: any;
import {ClientMessagesService} from '../../../../Customs/Services/WebServices/ClientMessagesService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from      '../../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    moduleId: module.id,
    templateUrl: './ClientEditComponent.html',
    providers: [EntityArgs],
})

export class ClientEditComponent extends BaseComponent{
    public TabsItemsSource: TabItem[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public CurrentEntity: ClientPM;
   clientPMService: ClientPMService = new ClientPMService();
   public entityResourceService: EntityResourceService = new EntityResourceService();
   public DataContext: any = this;
   isNewClient: boolean;
   isExternalId: boolean;
   requestParams: CreateClientRequestParams;
   responseData: INF_MSG_GenericResponseData;
   clientMessageService: ClientMessagesService = new ClientMessagesService();
   public ValidationErrorsList: string[] = [];

   constructor(public entityArgs: EntityArgs) {
       super();

       this.entityArgs.EntityArgEventEmitter.subscribe(
           theMessage => {
               if (theMessage == "ReloadEntity") {
                   this.clientPMService.get(this.CurrentEntity.Id).subscribe(response => {
                       var result = response.Result;
                       if (!AppTool.IsNullOrEmpty(result)) {
                           this.CurrentEntity = result;
                           this.entityArgs.EntityPM = this.CurrentEntity;
                       }
                   });
               }
           }
       );
   }

   SetWindowArgs(args: any) {

       this.CurrentEntity = args.CurrentEntity;
       this.isNewClient = args.isNewClient;
       this.isExternalId = args.IsExternalId;
       this.BuildTabs();
       this.RunComponent();
       this.entityArgs.EntityPM = this.CurrentEntity;
       this.entityArgs.ObjectTableName = "Customs.Client";

   }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }


    public get FacilitationTypeCode() { return this.CurrentEntity.FacilitationTypeCode; }
    public set FacilitationTypeCode(newValue: string) { this.CurrentEntity.FacilitationTypeCode = newValue; }



    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        this.TabsItemsSource.push(new TabItem("ADDRESSES", "General.O.Addresses"));
        this.TabsItemsSource.push(new TabItem("LICENSE", "General.O.DrivingLicense"));
        this.TabsItemsSource.push(new TabItem("COMMUNICATION", "General.O.Communications"));
        this.TabsItemsSource.push(new TabItem("EVENTS", "General.O.Events"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "General.O.RequestSheets"));
        this.TabsItemsSource.push(new TabItem("MOREDATA", "Customs.Client.TH.MoreData"));

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

        if (this.Retries < 3) {
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
    private ADDRESSES: any = null;
    private LICENSE: any = null;
    private COMMUNICATION: any = null;
    private EVENTS: any = null;
    private REQUESTSHEET: any = null;
    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "GENERAL": {
                        if (this.GENERAL == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/General/ClientGeneralTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.GENERAL = cmpRef.instance;
                                    this.GENERAL.InitTab(this.CurrentEntity, this.isNewClient);
                                });
                        }

                        break;
                    }

                    case "ADDRESSES": {
                        if (this.ADDRESSES == null) {
                            this.entityResourceService.getEntityResourceByTableName("Address").subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/ClientAddressesTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.ADDRESSES = cmpRef.instance;
                                    this.ADDRESSES.InitTab(this.CurrentEntity, this.isNewClient);
                                });
                            });
                        }
                        break;

                    }

                    case "LICENSE": {
                        if (this.LICENSE == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.ClientDrivingLicense").subscribe(response => {
                                this.entityResourceService.getEntityResourceByTableName("Customs.ClientDrivingLicenseType").subscribe(response => {
                                SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/License/ClientDrivingLicenseTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.LICENSE = cmpRef.instance;
                                        this.LICENSE.InitTab(this.CurrentEntity, this.isNewClient);
                                        });
                                });
                            });
                        }
                        break;

                    }

                    case "COMMUNICATION": {
                        if (this.COMMUNICATION == null) {
                         
                                this.entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(response => {
                                    SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.COMMUNICATION = cmpRef.instance;
                                            this.COMMUNICATION.IsTitleHidden = false;
                                        });
                                });
                          
                        }
                        break;

                    }

                    case "EVENTS": {
                        if (this.EVENTS == null) {
                                     SessionLocator.DynamicLoader.Load("./Common/Components/Events/EventsTabComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.EVENTS = cmpRef.instance;
                                        this.EVENTS.IsTitleHidden = false;
                                    });
                    
                           
                        }
                        break;

                    }

                    case "REQUESTSHEET": {
                        if (this.REQUESTSHEET == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
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


                }
            }
        }
    }

    OkButtonClicked() {
        
        if (this.isNewClient) {
            this.clientPMService.insert(this.CurrentEntity).subscribe(response => {
                var result = response.Result;
                SessionLocator.CurrentSession.CloseCurrentWindow();

            });
        }

        else {
            this.clientPMService.update(this.CurrentEntity).subscribe(response => {
                var result = response.Result;
               // SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
                SessionLocator.CurrentSession.CloseCurrentWindow();

            });

        }
        
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);

        var errors: string[] = [];
        Validator.TryValidateObject(this.CurrentEntity, "Customs.Client", errors);

        for (let item of this.CurrentEntity.ClientAddresses) {
            Validator.TryValidateObject(item, "Customs.ClientAddress", errors);
        }

        if (this.CurrentEntity.ClientAddresses == null || (this.CurrentEntity.ClientAddresses != null && this.CurrentEntity.ClientAddresses.length == 0)) {
            errors.push("חובה להזין לפחות כתובת אחת ללקוח");
        }

        if (this.CurrentEntity.ClientDrivingLicenses != null && this.CurrentEntity.ClientDrivingLicenses.length > 0){
            for (let item of this.CurrentEntity.ClientDrivingLicenses) {
                if (item.ClientDrivingLicenseTypes == null || (item.ClientDrivingLicenseTypes != null && item.ClientDrivingLicenseTypes.length == 0)) {
                    errors.push("חובה להזין לפחות סוג רישיון אחד לכל רישיון");
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            SessionLocator.CurrentSession.StartBusyIndicator("");

            var currRequestParams = new CreateClientRequestParams();
            currRequestParams.LoggingEnabled = true;
            currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
            currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
            currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
            currRequestParams.Tenant = SessionLocator.Tenant;
            currRequestParams.LoggingEntityId = this.CurrentEntity.Id;
                currRequestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.Client')[0].Id;
                currRequestParams.LoggingEntityReference = this.CurrentEntity.Code;
                currRequestParams.IsFakeResponse = true;
                currRequestParams.RequestName = "Add Client Request";
                currRequestParams.ResponseName = "Add Client Response";
                currRequestParams.IsExternalId = this.isExternalId;
                currRequestParams.ClientTypeSpecificCode = this.CurrentEntity.ClientTypeSpecificCode;
                currRequestParams.IsActive = this.CurrentEntity.IsActive;
                currRequestParams.DunsNumber = this.CurrentEntity.DunsNumber;
                currRequestParams.EnglishBirthPlace = this.CurrentEntity.EnglishBirthPlace;
                currRequestParams.EnglishCorporationName = this.CurrentEntity.EnglishCorporationName;
                currRequestParams.EnglishFatherName = this.CurrentEntity.EnglishFatherName;
                currRequestParams.EnglishFirstName = this.CurrentEntity.EnglishFirstName;
                currRequestParams.EnglishLastName = this.CurrentEntity.EnglishLastName;
                currRequestParams.FullName = this.CurrentEntity.FullName;
                currRequestParams.GenderCode = this.CurrentEntity.GenderCode;
                currRequestParams.LocalCorporationName = this.CurrentEntity.LocalCorporationName;
                currRequestParams.LocalFirstName = this.CurrentEntity.LocalFirstName;
                currRequestParams.LocalLastName = this.CurrentEntity.LocalLastName;
                currRequestParams.PassportCountryCode = this.CurrentEntity.PassportCountryCode;
                currRequestParams.PassportExpirationDate = this.CurrentEntity.PassportExpirationDate;
                currRequestParams.PassportFirstName = this.CurrentEntity.PassportFirstName;
                currRequestParams.PassportIssueDate = this.CurrentEntity.PassportIssueDate;
                currRequestParams.PassportLastName = this.CurrentEntity.PassportLastName;
                currRequestParams.PassportNumber = this.CurrentEntity.PassportNumber;
            currRequestParams.PassportTypeCode = this.CurrentEntity.PassportTypeCode;
            currRequestParams.NationalIdentificationNumber = this.CurrentEntity.NationalIdentificationNumber;

                currRequestParams.BirthDate = this.CurrentEntity.BirthDate;
                currRequestParams.IsImporter = this.CurrentEntity.IsImporter;
                currRequestParams.IsExporter = this.CurrentEntity.IsExporter;
                currRequestParams.ConcurrencyGUID = this.CurrentEntity.ConcurrencyGUID;

                currRequestParams.ClientAddresses = [];
                for (var clientAddress of this.CurrentEntity.ClientAddresses) {

                    var addressParams: ClientAdressParams = new ClientAdressParams();
                    addressParams.AddressPurposeCode = clientAddress.AddressPurposeCode;
                    addressParams.AddressTypeCode = clientAddress.AddressTypeCode;
                    addressParams.AuthorizedSignerPermit1 = clientAddress.AuthorizedSignerPermit1;
                    addressParams.AuthorizedSignerPermit2 = clientAddress.AuthorizedSignerPermit2;
                    addressParams.AuthorizedSignerPermit3 = clientAddress.AuthorizedSignerPermit3;
                    addressParams.BranchName = clientAddress.BranchName;
                    addressParams.ContactFirstName = clientAddress.ContactFirstName;
                    addressParams.ContactIdentifier = clientAddress.ContactIdentifier;
                    addressParams.ContactLastName = clientAddress.ContactLastName;
                    addressParams.ContactRoleTypeCode = clientAddress.ContactRoleTypeCode;
                    addressParams.ContactStateCode = clientAddress.ContactStateCode;
                    addressParams.EnglishCityName = clientAddress.EnglishCityName;
                    addressParams.EnglishCountryCode = clientAddress.EnglishCountryCode;
                    addressParams.EnglishMainAddressLine = clientAddress.EnglishMainAddressLine;
                    addressParams.EnglishPostalCode = clientAddress.EnglishPostalCode;
                    addressParams.IsHebrewAddress = clientAddress.IsHebrewAddress;
                    addressParams.LocalApartment = clientAddress.LocalApartment;
                    addressParams.EnglishSubCountryCode = clientAddress.EnglishSubCountryCode;
                    addressParams.IsPalestinianCity = clientAddress.IsPalestinianCity;
                    addressParams.LocalCityCode = clientAddress.LocalCityCode;
                    addressParams.LocalEntrance = clientAddress.LocalEntrance;
                    addressParams.LocalHouseLetter = clientAddress.LocalHouseLetter;
                    addressParams.LocalHouseNumber = clientAddress.LocalHouseNumber;
                    addressParams.LocalPOBox = clientAddress.LocalPOBox;
                    addressParams.LocalPostalCode = clientAddress.LocalPostalCode;
                    addressParams.LocalSecondLine = clientAddress.LocalSecondLine;
                    addressParams.LocalStreetName = clientAddress.LocalStreetName;
                    addressParams.CustomAddressCode = clientAddress.CustomAddressCode;
                    addressParams.AddressId = clientAddress.AddressId;

                 
                    addressParams.ClientAddressCommunicationType = [];
                    for (var communicationItem of clientAddress.ClientsAddressCommTypes) {
                        var clientsAddressCommunication = new ClientAddressCommunicationType();
                      
                     
                        clientsAddressCommunication.CommunicationAddress = communicationItem.CommunicationAddress;
                        clientsAddressCommunication.CommunicationTypeCode = communicationItem.CommunicationTypeCode;
                        clientsAddressCommunication.CommunicationTypeName = communicationItem.CommunicationTypeName;
                        addressParams.ClientAddressCommunicationType.push(clientsAddressCommunication);
                    }

                    currRequestParams.ClientAddresses.push(addressParams);
                }

                currRequestParams.ClientDrivingLicenses = [];
                for (var clientDrivingLicenseItem of this.CurrentEntity.ClientDrivingLicenses) {
                    var clientDrivingLicenseParams: ClientDrivingLicenseParams = new ClientDrivingLicenseParams();
                    clientDrivingLicenseParams.DrivingLicenseNumber = clientDrivingLicenseItem.DrivingLicenseNumber;
                    clientDrivingLicenseParams.DriverLicenseValidityDate = clientDrivingLicenseItem.DriverLicenseValidityDate;
                    clientDrivingLicenseParams.DrivingLicenseCountryID = clientDrivingLicenseItem.DrivingLicenseCountryID;

                    clientDrivingLicenseParams.ClientDrivingLicenseTypes = [];
                    for (var clientDrivingLicenseTypeItem of clientDrivingLicenseItem.ClientDrivingLicenseTypes) {
                        var clientDrivingLicenseTypeParams: ClientDrivingLicenseTypeParams = new ClientDrivingLicenseTypeParams();
                        clientDrivingLicenseTypeParams.DriversLicenseTypeCode = clientDrivingLicenseTypeItem.DriversLicenseTypeCode;
                        clientDrivingLicenseParams.ClientDrivingLicenseTypes.push(clientDrivingLicenseTypeParams);
                    }
                    currRequestParams.ClientDrivingLicenses.push(clientDrivingLicenseParams);
                }

                CustomMessageProgressComponent
                    .ShowProgressBar(currRequestParams.PBId,
                    "שליחת מסר הקמת ספק", false)
                    .then((res) => {
                        this.responseData = res;
                        this.OnMassageDisplayMethod();
                    }
                    ).catch((err) => {
                        //this.ValidationErrorsList = [];
                        //     this.ValidationErrorsList.push(err);
                    });


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
        SessionLocator.CurrentSession.CloseCurrentWindow();
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
