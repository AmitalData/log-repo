import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ClientPMService} from '../../../../../Customs/Services/StandardPMs/ClientPMService';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import {AddAddressContactForClientRequestParams, ClientAddress, ClientsAddressCommunicationResult, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/AddAddressContactForClientRequestParams';
import { CustomMessageProgressComponent } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ClientMessagesService} from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ClientAddressPM } from 'Customs/Entitypms/ClientAddressPM';
import { ClientPM } from 'Customs/Entitypms/ClientPM';
import { ClientsAddressCommTypePM } from 'Customs/Entitypms/ClientsAddressCommTypePM';

@Component({ 
    
    templateUrl: './AddEditAddressComponent.html',
})
export class AddEditAddressComponent extends BaseComponent{
  public IsDisplayOnly: boolean = false;
  public CustomSendOptionsButtonCanForcePersonalSign: any;

    public ObjectTableName: string = "Customs.ClientAddress";
    public DataContext = this;
    LayoutDirection: string = 'ltr';
    entityPM: ClientAddressPM;
    clientPM: ClientPM;
    CommunicationsList: ObservableCollection;
   clientPMService: ClientPMService = new ClientPMService();
   isNew: boolean;
   requestParams: AddAddressContactForClientRequestParams;
   responseData: INF_MSG_GenericResponseData;
   clientMessagesService: ClientMessagesService = new ClientMessagesService();
   isNewClient: boolean;

   public ValidationErrorsList: string[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
      

     
        
    }

    SetWindowArgs(args: any) {
        this.entityPM = args.clientAddressPM;
        this.clientPM = args.clientPM;
        this.isNew = args.IsNew;
        this.isNewClient = args.isNewClient;
        this.BuildTabs();
        this.CommunicationsList = new ObservableCollection([]);
        this.BuildCommunicationsList();

    }

    TabsSource: any[] = [];
    SelectedTab: string = ";"
    isHebrewSelected: boolean = false;
    isEnglishSelected: boolean = false;
    BuildTabs() {
        if (this.IsHebrewAddress || this.isNew) {
            this.SelectedTab = "Hebrew";
            this.isHebrewSelected = true;
          
        }
        else {
            this.SelectedTab = "English";
            this.isEnglishSelected = true;
        }

        //this.SelectedTab = "Hebrew";
        this.TabsSource.push({ Name: "Hebrew", isSelected: this.isHebrewSelected, Header: TextCodeTranslator.Translate("Customs.Client.O.Hebrew") });
        this.TabsSource.push({ Name: "English", isSelected: this.isEnglishSelected, Header: TextCodeTranslator.Translate("Customs.Client.O.English") });
      
    }
    SelectionChanged(tab: any) {

        this.TabsSource.forEach(item => { // reset selection
            item.isSelected = false;
        });

        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab); return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    }


    AddButonClicked() {
       
        var newCommunicationPM = new ClientsAddressCommTypePM(this.clientPM);
        newCommunicationPM.Tenant = SessionLocator.Tenant;
        newCommunicationPM.ClientId = this.clientPM.Id;

        if (!this.entityPM.ClientsAddressCommTypes.includes(newCommunicationPM)) {
            this.entityPM.AddClientsAddressCommType(newCommunicationPM);
                this.CommunicationsList.Insert(new CommunicationItemModel(newCommunicationPM,this));
            }
       
    }

    BuildCommunicationsList() {

        for (let item of this.entityPM.ClientsAddressCommTypes){

        this.CommunicationsList.Insert(new CommunicationItemModel(item,this));
        }
    }

    RemoveRow(item: CommunicationItemModel) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.Vendor.O.DeleteCommunication"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CommunicationsList.Remove(item);
                    this.entityPM.RemoveClientsAddressCommType(item.CommunicationPM);
                }

            });

        }
    }



  //#region Properties


    public get BranchName() { return this.entityPM.BranchName; }
    public set BranchName(newValue: string) { this.entityPM.BranchName = newValue; }

    public get AddressTypeCode() { return this.entityPM.AddressTypeCode; }
    public set AddressTypeCode(newValue: string) { this.entityPM.AddressTypeCode = newValue; }

    public get AddressTypeName() { return this.entityPM.AddressTypeName; }
    public set AddressTypeName(newValue: string) { this.entityPM.AddressTypeName = newValue; }

    public get LocalCityCode() { return this.entityPM.LocalCityCode; }
    public set LocalCityCode(newValue: string) { this.entityPM.LocalCityCode = newValue; }

    public get LocalCityName() { return this.entityPM.LocalCityName; }
    public set LocalCityName(newValue: string) { this.entityPM.LocalCityName = newValue; }

    public get LocalStreetName() { return this.entityPM.LocalStreetName; }
    public set LocalStreetName(newValue: string) { this.entityPM.LocalStreetName = newValue; }

    public get LocalHouseNumber() { return this.entityPM.LocalHouseNumber; }
    public set LocalHouseNumber(newValue: string) { this.entityPM.LocalHouseNumber = newValue; }

    public get LocalPOBox() { return this.entityPM.LocalPOBox; }
    public set LocalPOBox(newValue: string) { this.entityPM.LocalPOBox = newValue; }
 
    public get AddressPurposeCode() { return this.entityPM.AddressPurposeCode; }
    public set AddressPurposeCode(newValue: string) { this.entityPM.AddressPurposeCode = newValue; }

    public get LocalSecondLine() { return this.entityPM.LocalSecondLine; }
    public set LocalSecondLine(newValue: string) { this.entityPM.LocalSecondLine = newValue; }

    public get LocalHouseLetter() { return this.entityPM.LocalHouseLetter; }
    public set LocalHouseLetter(newValue: string) { this.entityPM.LocalHouseLetter = newValue; }

    public get LocalPostalCode() { return this.entityPM.LocalPostalCode; }
    public set LocalPostalCode(newValue: string) { this.entityPM.LocalPostalCode = newValue; }

    public get EnglishCountryCode() { return this.entityPM.EnglishCountryCode; }
    public set EnglishCountryCode(newValue: string) { this.entityPM.EnglishCountryCode = newValue; }

    
    public get EnglishCityName() { return this.entityPM.EnglishCityName; }
    public set EnglishCityName(newValue: string) { this.entityPM.EnglishCityName = newValue; }

    public get EnglishPostalCode() { return this.entityPM.EnglishPostalCode; }
    public set EnglishPostalCode(newValue: string) { this.entityPM.EnglishPostalCode = newValue; }

    public get EnglishSubCountryCode() { return this.entityPM.EnglishSubCountryCode; }
    public set EnglishSubCountryCode(newValue: string) { this.entityPM.EnglishSubCountryCode = newValue; }
 
    
    public get EnglishMainAddressLine() { return this.entityPM.EnglishMainAddressLine; }
    public set EnglishMainAddressLine(newValue: string) { this.entityPM.EnglishMainAddressLine = newValue; }

    public get ContactIdentifier() { return this.entityPM.ContactIdentifier; }
    public set ContactIdentifier(newValue: string) { this.entityPM.ContactIdentifier = newValue; }
  
    public get ContactFirstName() { return this.entityPM.ContactFirstName; }
    public set ContactFirstName(newValue: string) { this.entityPM.ContactFirstName = newValue; }


    public get AuthorizedSignerPermit1() { return this.entityPM.AuthorizedSignerPermit1; }
    public set AuthorizedSignerPermit1(newValue: string) { this.entityPM.AuthorizedSignerPermit1 = newValue; }

    public get AuthorizedSignerPermit2() { return this.entityPM.AuthorizedSignerPermit2; }
    public set AuthorizedSignerPermit2(newValue: string) { this.entityPM.AuthorizedSignerPermit2 = newValue; }

    public get AuthorizedSignerPermit3() { return this.entityPM.AuthorizedSignerPermit3; }
    public set AuthorizedSignerPermit3(newValue: string) { this.entityPM.AuthorizedSignerPermit3 = newValue; }

    public get ContactRoleTypeCode() { return this.entityPM.ContactRoleTypeCode; }
    public set ContactRoleTypeCode(newValue: string) { this.entityPM.ContactRoleTypeCode = newValue; }

    public get ContactLastName() { return this.entityPM.ContactLastName; }
    public set ContactLastName(newValue: string) { this.entityPM.ContactLastName = newValue; }

    //public get IsHebrewAddress() { return this.entityPM.IsHebrewAddress; }
    //public set IsHebrewAddress(newValue: boolean) { this.entityPM.IsHebrewAddress = newValue; }

    //public get IsPalestinianCity() { return this.entityPM.IsPalestinianCity; }
    //public set IsPalestinianCity(newValue: boolean) { this.entityPM.IsPalestinianCity = newValue; }

    
   

//#endregion



    get IsHebrewAddress() { return this.entityPM.IsHebrewAddress; }
    set IsHebrewAddress(newValue: boolean) {
        if (this.entityPM.IsHebrewAddress != newValue) {
            this.entityPM.IsHebrewAddress = newValue;
            if (newValue) {
                this.IsPalestinianCity = false;
            }
           
        }
    }

    UseHebrewAddress(newValue: boolean) {
        this.IsHebrewAddress = newValue;
     
    }

    get IsPalestinianCity() { return this.entityPM.IsPalestinianCity; }
    set IsPalestinianCity(newValue: boolean) {
        if (this.entityPM.IsPalestinianCity != newValue) {
            this.entityPM.IsPalestinianCity = newValue;
            if (newValue) {
                this.IsHebrewAddress = false;
            }

        }
    }

    UsePalestinianAddress(newValue: boolean) {
        this.IsPalestinianCity = newValue;

    }


    CancelButtonClicked() {

        this.entityPM.RejectChanges();
        //this.CurrentSession.CloseCurrentWindow();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }


    OkButtonClicked() {

        this.clientPM.AddClientAddress(this.entityPM);
      
        this.CurrentSession.CloseCurrentWindow();
    }
 

    public operationType: OperationTypes;
    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
      //  alert(customSendOptionsArgs);

        var errors: string[] = [];
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        for (let item of this.entityPM.ClientsAddressCommTypes) {

            Validator.TryValidateObject(item, this.ObjectTableName, errors);
        }

       // this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.operationType = OperationTypes.Update;
            if (this.isNew) {
                //  trigger.addressesList.Add(this);
                this.operationType = OperationTypes.Add;
            }
            if (!this.isNewClient) {
                this.CurrentSession.StartBusyIndicator("");

                
                var currRequestParams = new AddAddressContactForClientRequestParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
                currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
                currRequestParams.Tenant = SessionLocator.Tenant;
                currRequestParams.AddressCode = new ClientAddress();
                currRequestParams.AddressCode.AddressId = this.entityPM.AddressId;
                currRequestParams.AddressCode.AddressContactState = this.entityPM.ContactStateCode;
                currRequestParams.AddressCode.AddressTypeCode = this.entityPM.AddressTypeCode;
                currRequestParams.AddressCode.AddressPurposeCode = this.entityPM.AddressPurposeCode;
                currRequestParams.AddressCode.IsPalestinianCity = this.entityPM.IsPalestinianCity;
                currRequestParams.AddressCode.IsHebrewAddress = this.entityPM.IsHebrewAddress;
                currRequestParams.AddressCode.BranchName = this.entityPM.BranchName;
                currRequestParams.AddressCode.ContactIdentifier = this.entityPM.ContactIdentifier;
                currRequestParams.AddressCode.ContactFirstName = this.entityPM.ContactFirstName;
                currRequestParams.AddressCode.ContactLastName = this.entityPM.ContactLastName;
                currRequestParams.AddressCode.ContactRoleTypeCode = this.entityPM.ContactRoleTypeCode;
                currRequestParams.AddressCode.AuthorizedSignerPermit1 = this.entityPM.AuthorizedSignerPermit1;
                currRequestParams.AddressCode.AuthorizedSignerPermit2 = this.entityPM.AuthorizedSignerPermit2;
                currRequestParams.AddressCode.AuthorizedSignerPermit3 = this.entityPM.AuthorizedSignerPermit3;
                currRequestParams.AddressCode.LocalCityCode = this.entityPM.LocalCityCode;
                currRequestParams.AddressCode.LocalSecondLine = this.entityPM.LocalSecondLine;
                currRequestParams.AddressCode.LocalStreetName = this.entityPM.LocalStreetName;
                currRequestParams.AddressCode.LocalHouseLetter = this.entityPM.LocalHouseLetter;
                currRequestParams.AddressCode.LocalEntrance = this.entityPM.LocalEntrance;
                currRequestParams.AddressCode.EnglishCountryCode = this.entityPM.EnglishCountryCode;
                currRequestParams.AddressCode.EnglishSubCountryCode = this.entityPM.EnglishSubCountryCode;
                currRequestParams.AddressCode.EnglishCityName = this.entityPM.EnglishCityName;
                currRequestParams.AddressCode.EnglishMainAddressLine = this.entityPM.EnglishMainAddressLine;
                currRequestParams.AddressCode.EnglishPostalCode = this.entityPM.EnglishPostalCode;
                if (this.entityPM.LocalApartment)
                    currRequestParams.AddressCode.LocalApartment = this.entityPM.LocalApartment.toString();
                currRequestParams.AddressCode.LocalPOBox = this.entityPM.LocalPOBox;
                currRequestParams.AddressCode.LocalPostalCode = this.entityPM.LocalPostalCode;
                currRequestParams.AddressCode.LocalHouseNumber = this.entityPM.LocalHouseNumber;
                currRequestParams.AddressCode.CustomAddressCode = this.entityPM.CustomAddressCode;
                currRequestParams.LoggingEnabled = true,
                    currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;

                currRequestParams.Tenant = this.entityPM.Tenant;
                currRequestParams.RequestName = "Add/Update/Delete Address Contact Request";
                currRequestParams.ResponseName = "Add/Update/Delete Address Contact Request";
                currRequestParams.ClientId = this.clientPM.Id;
                currRequestParams.ExternalId = this.clientPM.Code;
                currRequestParams.PassportNumber = this.clientPM.PassportNumber;
                currRequestParams.PassportTypeCode = this.clientPM.PassportTypeCode;
                currRequestParams.PassportCountryCode = this.clientPM.PassportCountryCode;        
                currRequestParams.OperationType = this.operationType;
   
                currRequestParams.AddressCode.ClientsAddressCommunication = [];
                for (var communicationItem of this.entityPM.ClientsAddressCommTypes) {
                    var clientsAddressCommunication = new ClientsAddressCommunicationResult();
                    clientsAddressCommunication.CommunicationAddress = communicationItem.CommunicationAddress;
                    clientsAddressCommunication.CommunicationType = communicationItem.CommunicationTypeCode;
                    clientsAddressCommunication.CommunicationTypeName = communicationItem.CommunicationTypeName;
                    currRequestParams.AddressCode.ClientsAddressCommunication.push(clientsAddressCommunication);
                }

                CustomMessageProgressComponent
                    .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                    "שליחת מסר הוספה/עדכון/מחיקה כתובת לקוח", false)
                    .then((res) => {
                        this.responseData = res;
                        this.OnMassageDisplayMethod();
                    }
                ).catch((err) => {
                    //this.ValidationErrorsList = [];
                    //     this.ValidationErrorsList.push(err);
                    });


                this.clientMessagesService.PostUpdateDeleteClientAddressContactRequest(currRequestParams)
                    .subscribe((myServiceResponse: ServiceResponse) => {
                        if (this.isNew) {
                            this.clientPM.AddClientAddress(this.entityPM);
                        }
                        if (myServiceResponse.Result.Succeeded == true && myServiceResponse.Result.HasException == false) {
                            this.CurrentSession.CloseCurrentWindowEmit("ReloadEntity");
                        }
                        else {
                            this.CurrentSession.CloseCurrentWindow();
                        }
                    });
            }       

         //   this.isNew = false;
        }
            
    }

    OnMassageDisplayMethod() {
        if (this.requestParams == null) {
            this.requestParams = new AddAddressContactForClientRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData();
        }
    }

   
   
}


export class CommunicationItemModel extends BaseComponent {
    public CommunicationPM: ClientsAddressCommTypePM = null;
    public ObjectTableName = "Customs.ClientsAddressCommType";
    public DataContext = this;
    Parent: AddEditAddressComponent;
    constructor(private communicationPM: ClientsAddressCommTypePM, parent: AddEditAddressComponent) {
        super();
        this.CommunicationPM = communicationPM;
        this.Parent = parent;
    }

    //#region Properties

    get CommunicationAddress() { return this.CommunicationPM.CommunicationAddress; }
    set CommunicationAddress(value: string) {
        if (this.CommunicationPM.CommunicationAddress != value) {
            this.CommunicationPM.CommunicationAddress = value;

        }
    }

    get CommunicationTypeCode() { return this.CommunicationPM.CommunicationTypeCode; }
    set CommunicationTypeCode(value: string) {
        if (this.CommunicationPM.CommunicationTypeCode != value) {
            this.CommunicationPM.CommunicationTypeCode = value;

        }
    }

    get CommunicationTypeName() { return this.CommunicationPM.CommunicationTypeName; }
    set CommunicationTypeName(value: string) {
        if (this.CommunicationPM.CommunicationTypeName != value) {
            this.CommunicationPM.CommunicationTypeName = value;

        }
    }


    //#endregion


    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

 
}
