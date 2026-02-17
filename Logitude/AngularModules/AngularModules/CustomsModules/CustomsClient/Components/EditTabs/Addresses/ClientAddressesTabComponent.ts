import {Component}  from '@angular/core';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ClientAddressPM} from '../../../../../Customs/EntityPMs/ClientAddressPM';
import {ClientPM} from '../../../../../Customs/EntityPMs/ClientPM';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomMessageProgressComponent } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import {ClientMessagesService} from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { ClaimGeneralTabComponent } from '../../../../CustomsClaim/Components/EditTabs/General/ClaimGeneralTabComponent';
import {AddAddressContactForClientRequestParams, ClientAddress, ClientsAddressCommunicationResult, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/AddAddressContactForClientRequestParams';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './ClientAddressesTabComponent.html',
})

export class ClientAddressesTabComponent extends BaseComponent{

    public entityResourceService: EntityResourceService = new EntityResourceService();

    entityPM: ClientPM;
    isNewClient: boolean;
    clientMessageService: ClientMessagesService = new ClientMessagesService();
    responseData: INF_MSG_GenericResponseData;
    requestParams: AddAddressContactForClientRequestParams;

    private Mode: string = "";
    private newAddressButtonVisibility: boolean = true;
    private editButtonVisibility: boolean = true;
    Parent: ClaimGeneralTabComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _EntityArgs: EntityArgs) {
        super();
    }

    InitTab(EntityPM: ClientPM, IsNew: boolean) {

        this.entityPM = EntityPM;
        this.isNewClient = IsNew;
        
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.Mode = "Claims";
            this.entityPM = args.EntityPM;
            this.Parent = args.Parent;
            this.NewAddressButtonVisibility = false;
            this.EditButtonVisibility = false;
        }
    }

    public get NewAddressButtonVisibility() { return this.newAddressButtonVisibility; }
    public set NewAddressButtonVisibility(newValue: boolean) { this.newAddressButtonVisibility = newValue; }

    public get EditButtonVisibility() { return this.editButtonVisibility; }
    public set EditButtonVisibility(newValue: boolean) { this.editButtonVisibility = newValue; }

    NewAddressButtonClicked() {
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientAddress").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.ClientsAddressCommType").subscribe(response => {

               // this.entityPM.ClientAddresses[0].IsHebrewAddress                
            var item = new ClientAddressPM(this.entityPM);
            item.Tenant = this.entityPM.Tenant; 
            item.ClientId = this.entityPM.Id;
            item.ContactFirstName = this.entityPM.EnglishFirstName;

            var windowArgs: any = {};
            windowArgs.clientAddressPM = item;
            windowArgs.clientPM = this.entityPM;
            windowArgs.IsNew = true;
            windowArgs.isNewClient = this.isNewClient;
            var windowTitle = TextCodeTranslator.Translate("General.O.AddAddress"); //"New Address";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 700;
       
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            // logWindow.WindowClosed.subscribe(($event: any) => this.SetCertificateStatusVisibility());
            logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/AddEditAddressComponent');
        });
            });
      
    }

    EditAddress(address: ClientAddressPM) {
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientAddress").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.ClientsAddressCommType").subscribe(response => {    

                var windowArgs: any = {};
                windowArgs.clientAddressPM = address;
                windowArgs.clientPM = this.entityPM;
                windowArgs.IsNew = false;
                windowArgs.isNewClient = this.isNewClient;
                var windowTitle = TextCodeTranslator.Translate("General.O.AddAddress"); //"New Address";
                var logWindow = new LogitudeWindow();
                logWindow.Width = 800;
                logWindow.Height = 700;

                logWindow.Title = windowTitle;
                logWindow.ShowCloseButton = true;
                logWindow.WindowArgs = windowArgs;
                //logWindow.WindowClosed.subscribe(($event: any) => this.ReloadEntityPM());
                logWindow.WindowClosed.subscribe((arg: any) => {
                    if (arg == "ReloadEntity") {
                        this._EntityArgs.SendMessage("ReloadEntity");
                    }
                });
                logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/AddEditAddressComponent');
            });
        });

    }

    public operationType: any;
    DeleteAddress(address: ClientAddressPM) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
            confirmWindow.Width = 300;


            confirmWindow.Show("האם למחוק את הכתובת?");
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.entityPM.RemoveClientAddress(address);
                    this.operationType = OperationTypes.Delete;
                    this.SendAddUpdateDeleteClientAddressContactRequest(address);
                    confirmWindow.Close();
                }

            });
       
    }

    ReloadEntityPM() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    SendAddUpdateDeleteClientAddressContactRequest(address: ClientAddressPM) {
                this.CurrentSession.StartBusyIndicator("");


                var currRequestParams = new AddAddressContactForClientRequestParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
              
                currRequestParams.Tenant = SessionLocator.Tenant;
                currRequestParams.AddressCode = new ClientAddress();
                currRequestParams.AddressCode.AddressId = address.AddressId;
                currRequestParams.AddressCode.AddressContactState = address.ContactStateCode;
                currRequestParams.AddressCode.AddressTypeCode = address.AddressTypeCode;
                currRequestParams.AddressCode.AddressPurposeCode = address.AddressPurposeCode;
                currRequestParams.AddressCode.IsPalestinianCity = address.IsPalestinianCity;
                currRequestParams.AddressCode.IsHebrewAddress = address.IsHebrewAddress;
                currRequestParams.AddressCode.BranchName = address.BranchName;
                currRequestParams.AddressCode.ContactIdentifier = address.ContactIdentifier;
                currRequestParams.AddressCode.ContactFirstName = address.ContactFirstName;
                currRequestParams.AddressCode.ContactLastName = address.ContactLastName;
                currRequestParams.AddressCode.ContactRoleTypeCode = address.ContactRoleTypeCode;
                currRequestParams.AddressCode.AuthorizedSignerPermit1 = address.AuthorizedSignerPermit1;
                currRequestParams.AddressCode.AuthorizedSignerPermit2 = address.AuthorizedSignerPermit2;
                currRequestParams.AddressCode.AuthorizedSignerPermit3 = address.AuthorizedSignerPermit3;
                currRequestParams.AddressCode.LocalCityCode = address.LocalCityCode;
                currRequestParams.AddressCode.LocalSecondLine = address.LocalSecondLine;
                currRequestParams.AddressCode.LocalStreetName = address.LocalStreetName;
                currRequestParams.AddressCode.LocalHouseLetter = address.LocalHouseLetter;
                currRequestParams.AddressCode.LocalEntrance = address.LocalEntrance;
                currRequestParams.AddressCode.EnglishCountryCode = address.EnglishCountryCode;
                currRequestParams.AddressCode.EnglishSubCountryCode = address.EnglishSubCountryCode;
                currRequestParams.AddressCode.EnglishCityName = address.EnglishCityName;
                currRequestParams.AddressCode.EnglishMainAddressLine = address.EnglishMainAddressLine;
                currRequestParams.AddressCode.EnglishPostalCode = address.EnglishPostalCode;
                if (address.LocalApartment)
                    currRequestParams.AddressCode.LocalApartment = address.LocalApartment.toString();
                currRequestParams.AddressCode.LocalPOBox = address.LocalPOBox;
                currRequestParams.AddressCode.LocalPostalCode = address.LocalPostalCode;
                currRequestParams.AddressCode.LocalHouseNumber = address.LocalHouseNumber;
                currRequestParams.AddressCode.CustomAddressCode = address.CustomAddressCode;
                currRequestParams.LoggingEnabled = true,
                    currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;

                currRequestParams.Tenant = address.Tenant;
                currRequestParams.RequestName = "Add/Update/Delete Address Contact Request";
                currRequestParams.ResponseName = "Add/Update/Delete Address Contact Request";
                currRequestParams.ClientId = this.entityPM.Id;
                currRequestParams.ExternalId = this.entityPM.Code;
                currRequestParams.PassportNumber = this.entityPM.PassportNumber;
                currRequestParams.PassportTypeCode = this.entityPM.PassportTypeCode;
                currRequestParams.PassportCountryCode = this.entityPM.PassportCountryCode;

                currRequestParams.OperationType = this.operationType;


                currRequestParams.AddressCode.ClientsAddressCommunication = [];
                for (var communicationItem of address.ClientsAddressCommTypes) {
                    var clientsAddressCommunication = new ClientsAddressCommunicationResult();
                    clientsAddressCommunication.CommunicationAddress = communicationItem.CommunicationAddress;
                    clientsAddressCommunication.CommunicationType = communicationItem.CommunicationTypeCode;
                    clientsAddressCommunication.CommunicationTypeName = communicationItem.CommunicationTypeName;
                    currRequestParams.AddressCode.ClientsAddressCommunication.push(clientsAddressCommunication);
                }

                CustomMessageProgressComponent
                    .ShowProgressBar(currRequestParams.PBId,
                    "שליחת מסר הוספה/עדכון/מחיקה כתובת לקוח", true)
                    .then((res) => {
                        this.responseData = res;
                        this.OnMassageDisplayMethod();
                    }
                    ).catch((err) => {
                        //this.ValidationErrorsList = [];
                        //     this.ValidationErrorsList.push(err);
                    });


                this.clientMessageService.PostUpdateDeleteClientAddressContactRequest(currRequestParams)
                    .subscribe((myServiceResponse: ServiceResponse) => {
                        //     this.clientPM.AddClientAddress(this.entityPM);
                    });

            }

    OnMassageDisplayMethod() {
                if (this.requestParams == null) {
                    this.requestParams = new AddAddressContactForClientRequestParams();
                }
                if (this.responseData == null) {
                    this.responseData = new INF_MSG_GenericResponseData();
                }
    }

    ChooseAddressButtonClicked(address: ClientAddressPM) {
        this.CurrentSession.CloseCurrentWindow();
        this.Parent.SelectAddresseCompleted(address);
    }

    OnRowDoubleClick(item: ClientAddressPM) {
        if (this.Mode == "Claims") {
            this.CurrentSession.CloseCurrentWindow();
            this.Parent.SelectAddresseCompleted(item);
        }
    }
}


