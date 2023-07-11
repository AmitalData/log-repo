import {Component, ViewChildren, QueryList, ChangeDetectorRef}  from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
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
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { ClientPMService } from 'Customs/Services/StandardPMs/ClientPMService';
import { ClientsTapagPM } from 'Customs/EntityPMs/ClientsTapagPM';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';


@Component({
    
    templateUrl: './ClientEditComponent.html',
    providers: [EntityArgs],
})

export class ClientEditComponent extends BaseComponent{
  public IsDisplayOnly: boolean = false;
  public right: any;
  public CustomSendOptionsButtonCanForcePersonalSign: any;

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
   private CurrentSession = SessionLocator.SelectedSession;
   public isEntityChange: boolean = false;

   ClientsTapagList: ClientsTapag[] = [];
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


    addTapagEnabled: boolean;
    public get AddTapagEnabled() { return this.CurrentEntity ? this.addTapagEnabled : null; }
    public set AddTapagEnabled(newValue: boolean) {
        this.addTapagEnabled = newValue;
    }

    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        this.TabsItemsSource.push(new TabItem("ADDRESSES", "General.O.Addresses"));
        this.TabsItemsSource.push(new TabItem("LICENSE", "General.O.DrivingLicense"));
        this.TabsItemsSource.push(new TabItem("COMMUNICATION", "General.O.Communications"));
        this.TabsItemsSource.push(new TabItem("EVENTS", "General.O.Events"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "General.O.RequestSheets"));
        this.TabsItemsSource.push(new TabItem("MOREDATA", "Customs.Client.TH.MoreData"));
        this.TabsItemsSource.push(new TabItem("CLIENTPOA", "General.O.ClientPoas"));
        this.BuildClientsTapagList();

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
    private ADDRESSES: any = null;
    private CLIENTPOA: any = null;
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
                            this.entityResourceService.getEntityResourceByTableName("Address").subscribe((response:any) => {
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
                            this.entityResourceService.getEntityResourceByTableName("Customs.ClientDrivingLicense").subscribe((response:any) => {
                                this.entityResourceService.getEntityResourceByTableName("Customs.ClientDrivingLicenseType").subscribe((response:any) => {
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
                         
                                this.entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe((response:any) => {
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
                            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
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

                    case "CLIENTPOA": {
                        if (this.CLIENTPOA == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.ClientsPoa").subscribe((response: any) => {
                                SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/ClientPoa/ClientPoaTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.CLIENTPOA = cmpRef.instance;
                                        this.CLIENTPOA.InitTab(this.CurrentEntity);
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
        this.isEntityChange = true;
        if (this.isNewClient) {
            this.clientPMService.insert(this.CurrentEntity).subscribe((response:any) => {
                var result = response.Result;
                this.CurrentSession.CloseCurrentWindow();

            });
        }

        else {
            this.clientPMService.update(this.CurrentEntity).subscribe((response:any) => {
                var result = response.Result;
               // this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CurrentSession.CloseCurrentWindow();

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

            this.CurrentSession.StartBusyIndicator("");

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
                    .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
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
        this.CurrentSession.CloseCurrentWindow();
    }


    AddTapagNumberButtonClicked() {
        
        
        if (this.AddTapagEnabled && !this.IsDisplayOnly) {

            var item: ClientsTapagPM = new ClientsTapagPM(this.CurrentEntity);
            item.Tenant = this.CurrentEntity.Tenant;
            item.ClientId = this.CurrentEntity.Id;

            if (!this.CurrentEntity.ClientsTapags.includes(item)) {
                this.CurrentEntity.AddClientsTapag(item);
            }

            this.AddTapagEnabled = false;
            this.BuildClientsTapagList();
        }

    }

    BuildClientsTapagList() {

        this.ClientsTapagList = [];

        this.AddTapagEnabled = true;
        for (var i = 0; i < this.CurrentEntity.ClientsTapags.length; i++) {
            var viewModel: ClientsTapag = new ClientsTapag(this.CurrentEntity.ClientsTapags[i], this);
           viewModel.ClientsTapagNumber = i + 1;
            if (viewModel.TapagNumber == null) {
                this.AddTapagEnabled = false;
            }
            this.ClientsTapagList.push(viewModel);


        }

        if (this.ClientsTapagList.length == 0) {
            var item = new ClientsTapagPM(this.EntityPM);
            item.Tenant = this.CurrentEntity.Tenant;
            item.ClientId = this.CurrentEntity.Id;
            var viewModel: ClientsTapag = new ClientsTapag(item, this);
            viewModel.ClientsTapagNumber
            this.ClientsTapagList.push(viewModel);
            this.CurrentEntity.AddClientsTapag(item);


            this.AddTapagEnabled = false;
        }


    }
    ShowClientIndication() {
       
        if (this.CurrentEntity.ClientIndications == null || this.CurrentEntity.ClientIndications.length == 0) {
            return;
        }

        var windowArgs: any = {};
        windowArgs.CustomerIndicationList =  this.CurrentEntity.ClientIndications;;
        windowArgs.IsClientIndication = true
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 470;
        logitudeWindow.Height = 520;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = TextCodeTranslator.Translate("Customs.ClientIndication.O.IndicationClient"); 
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/CustomerIndicationComponent');
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




export class ClientsTapag extends BaseComponent {
    public EntityPM: ClientsTapagPM;
    private Parent: ClientEditComponent;
    ObjectTableName: string = "Customs.ClientsTapag";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ClientsTapagPM, parent: ClientEditComponent) {
        super();
        this.EntityPM = item;
        this.Parent = parent;
        //this.UIProperties.SetEnabled("TapagNumber", this.ObjectTableName, !this.Parent.IsDisplayOnly);

    }

    //#region Properties
    deleteTapagNumberVisible: boolean = false;
    public get DeleteTapagNumberVisible() { return this.deleteTapagNumberVisible; }
    public set DeleteTapagNumberVisible(newValue: boolean) { this.deleteTapagNumberVisible = newValue; }

    clientsTapagNumber: number = 1;
    public get ClientsTapagNumber() { return this.clientsTapagNumber; }
    public set ClientsTapagNumber(newValue: number) { this.clientsTapagNumber = newValue; }

    public get TapagNumber() { return this.EntityPM.TapagNumber; }
    public set TapagNumber(newValue: string) {
        
        this.EntityPM.TapagNumber = newValue;
        if (newValue != null) {
            if (this.Parent.ClientsTapagList.length == 1) {
                this.Parent.CurrentEntity.AddClientsTapag(this.EntityPM);
            }
            this.Parent.AddTapagEnabled = true;
        }
        else {
            this.Parent.AddTapagEnabled = false;
        }

    }
    //#endregion

    OnMouseOver() {
        if (this.ClientsTapagNumber > 1) {
            this.DeleteTapagNumberVisible = true;
        }
    } 

    OnMouseLeave() {
        if (!this.overCloseButton) {
            this.DeleteTapagNumberVisible = false;
        }
    }

    // close button
    overCloseButton: boolean = false;
    OnIconButtonMouseOver() {
        this.overCloseButton = true;
    }

    OnIconButtonMouseLeave() {
        this.overCloseButton = false;
    }
 

    DeleteTapagNumberButtonClicked() {

        var msg = TextCodeTranslator.Translate("Customs.ClientsTapag.O.DeleteTapagNumber");
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 150;
        confirmWindow.Show(msg);
        confirmWindow.WindowClosed.subscribe((event: any) => {

            if (confirmWindow.Yes) { // YES
                this.Parent.CurrentEntity.RemoveClientsTapag(this.EntityPM);
                this.Parent.BuildClientsTapagList();
            }
        });
    }  
}




