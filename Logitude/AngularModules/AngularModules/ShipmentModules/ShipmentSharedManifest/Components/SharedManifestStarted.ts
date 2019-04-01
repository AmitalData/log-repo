

import { Component, OnInit } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SharedAgentManifestService } from '../../../Shipment/Services/Others/SharedAgentManifestService';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import { CardList } from '../../../Common/EntityLists/CardList';
import { AgentSharedLogisticsKeyPMService } from '../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService';
import { AgentSharedLogisticsKey } from '../../../Common/EntityPMs/AgentSharedLogisticsKey';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {UIProperty, UIProperties}  from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';

import {LogitudeWindow} from '../../../Controls/Windows/logitudewindow';

import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {AddressList} from '../../../Common/EntityLists/AddressList';
import {ContactList} from '../../../Common/EntityLists/ContactList';
import {AddressListService} from '../../../Common/Services/StandardLists/AddressListService';
import {ContactListService} from '../../../Common/Services/StandardLists/ContactListService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AddressPM} from '../../../Common/EntityPMs/AddressPM';
@Component({
    moduleId: module.id,
    selector: 'SharedManifestStarted',
    templateUrl: './SharedManifestStarted.html',
    providers: [SharedAgentManifestService, AgentSharedLogisticsKeyPMService],
})
export class SharedManifestStarted {
    //private myCardListService: CardListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedAgentManifestService: SharedAgentManifestService, public _agentSharedLogisticsKeyPMService: AgentSharedLogisticsKeyPMService) {
        this.Listen();
    }

    LableShareBoutton: string = "Share";


    public ValidationErrorsList: string[] = [];
    EntityPM: ShipmentPM;
    HasError: boolean = false;
    IsSuccessfullySharedManifest: boolean = false;

    IsEnableButtonSharedManifest: boolean = false;


    FromSharedManifestEditAgentComponent: boolean = false;
    IsShareUpdatedAgent: boolean = false;
    IsShowUpdateAgentArea: boolean = false;

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.FromSharedManifestEditAgentComponent = args.FromSharedManifestEditAgentComponent;
        this.IsShareUpdatedAgent = args.IsShareUpdatedAgent;

        if (this.IsShareUpdatedAgent) {
            this.LableShareBoutton = "Next";
        }

        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
            this.ValidationErrorsList.push("Please define shipment agent in the Partners tab");
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
            this.ValidationErrorsList.push("The consignee partner is missing");
        }

        if (this.EntityPM.TransportModeId == "A" && AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            this.ValidationErrorsList.push("The master number is missing");
        }

      
        if (this.ValidationErrorsList.length == 0) {

            if (!this.FromSharedManifestEditAgentComponent) {
                this.CurrentSession.StartBusyIndicator("Loading...");
                this._agentSharedLogisticsKeyPMService.GetSingleByAgentId(this.EntityPM.AgentId).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {

                        var agentSharedKey: AgentSharedLogisticsKey = myResponse.Result;
                        if (agentSharedKey) {
                            switch (agentSharedKey.StatusCode) {
                                case "A":

                                    if (this.EntityPM.ShipmentLevelCode == "C") {
                                        this.CurrentSession.StartBusyIndicator("Loading...");
                                        this._sharedAgentManifestService.GetCheckIfMasterShipmentHaveHouseWithOtherAgent(this.EntityPM.Id, this.EntityPM.AgentId, this.EntityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
                                            this.CurrentSession.StopBusyIndicator();
                                            if (!myResponse.HasError) {
                                                if (myResponse.Result == true) {
                                                    this.ValidationErrorsList.push("One of the houses has agent different from the master shipment");

                                                } else {
                                                    this.IsEnableButtonSharedManifest = true;
                                                    if (this.EntityPM.IsManifestSentToAgent) this.UpdateAgent();

                                                }
                                            }
                                        });
                                    }
                                    else {
                                        this.IsEnableButtonSharedManifest = true;
                                        if (this.EntityPM.IsManifestSentToAgent) this.UpdateAgent();
                                    }
                                    break;

                                case "W":

                                    this.ValidationErrorsList.push("Waiting for the Agent’s approval to enable sharing");
                                    this.HasError = true;
                                    break;

                                case "I":
                                    this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                                    this.HasError = true;
                                    break;


                            }
                        }
                        else {
                            this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                            this.HasError = true;
                        }
                    }
                    else {
                        this.ValidationErrorsList.push(myResponse.ErrorsArray[0]);
                        this.HasError = true;
                    }

                });
            }
            else  this.UpdateAgent();
        }
        else this.HasError = true;


    }


    isSharingManifesRequested: boolean = false;
     SharingManifesButtonClick() {

        if (this.EntityPM.IsDirty) {
            this.isSharingManifesRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartSharingManifest();
        }



    };


    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    if (this.isSharingManifesRequested) {
                        this.StartSharingManifest();
                    }
                    
                    this.isSharingManifesRequested = false;
                } 


            });
        }
    }
    

  
    UpdateAgent() {

        if (!this.FromSharedManifestEditAgentComponent) {
            this.InitializeServices();

            var partnerItem: PartnerItem = new PartnerItem(this, "AGENT");
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "Share Updated Agent";
            logitudeWindow.DataContext = partnerItem;
            logitudeWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestEditAgentComponent");
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s == "OK") {
                    this.StartUpdateAgentManifest();
                }
            });

            this.CloseButtonClicked();
        }
        else {
            this.IsShowUpdateAgentArea = true;
            this.CurrentSession.CurrentWindow.Title = "Share Manifest with Updated Agent";
        }

    }


    StartUpdateAgentManifest() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var logWindow = new LogitudeWindow();
        windowArgs.FromSharedManifestEditAgentComponent = true;
        logWindow.Title = "Share Updated Agent";
        windowArgs.IsShareUpdatedAgent = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestStarted");
    }




    StartSharingManifest(isUpdateAgent: boolean = false) {

    
            this.CurrentSession.StartBusyIndicator("Sharing Manifest...");
            this._sharedAgentManifestService.ShareAgentManifest(this.EntityPM.Id, isUpdateAgent).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.IsEnableButtonSharedManifest = false;
                if (!myResponse.HasError) {
                    this.IsSuccessfullySharedManifest = true;
                    this.EntityPM.IsManifestSentToAgent = true;

                    this.IsShowUpdateAgentArea = false;
                    this.IsShareUpdatedAgent = false;
                    var activity: string = !isUpdateAgent ? "Share Manifests" : "Share Updated Agent";
                    ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", activity);

                } else {

                    if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                        myResponse.ErrorsArray.forEach((item) => {
                            this.ValidationErrorsList.push(item);
                        });

                    }
                    this.HasError = true;
                }


            });
        
    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();

    }



    IsEditingEnabled: boolean = true;
    public CardListService: CardListService;
    public AddressListService: AddressListService;
    public ContactListService: ContactListService;
    InitializeServices() {
        this.CardListService = new CardListService();
        this.AddressListService = new AddressListService();
        this.ContactListService = new ContactListService();
    }

    public AllAddresses: AddressList[] = [];
    public AllContacts: ContactList[] = [];



}

export class PartnerItem extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Code: string;
    public ObjectTableName: string = "Shipment";
    constructor(public fatherComponent: SharedManifestStarted, typeCode: string) {
        super();
        this.EntityPM = fatherComponent.EntityPM;
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.Code = typeCode;
        this.InitializeProperties();
        this.GetPartnerAddress();
        this.GetPartnerContact();
    }


    public IsDisableNextButton: boolean = false;
    public IsEditingEnabled: boolean = true;
    SetUIProperties() {

        var isPartnerFilled = this.PartnerId == null ? false : true;
        var isFieldsEnabled = false;
        if (this.IsEditingEnabled) {
            if (isPartnerFilled) {
                isFieldsEnabled = true;
            }
        }

        this.UIProperties.SetRequired(this.PartnerIdProperty, this.ObjectTableName, !isPartnerFilled);
        this.UIProperties.SetEnabled(this.PartnerIdProperty, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.PartnerAddressIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.PartnerContactIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.Reference1Property, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.Reference2Property, this.ObjectTableName, this.IsEditingEnabled);
    }

    public FullCode: string;
    public PartnerIndex: number;
    public PartnerTypeName: string;
    InitializeProperties() {
        this.PartnerIndex = 3; this.FullCode = "Agent";
        this.PartnerTypeName = TextCodeTranslator.Translate("Shipment.F." + this.FullCode + "Id");
    }

    get CardDependencyProperty1() {
        var myResult: string = "AG";

        return myResult;
    }
    get CardDependencyProperty1IsList() {
        var myResult: boolean = false;

        switch (this.Code) {
            case "SHIPR":
            case "CONSI":
            case "CSTMR":
                {
                    if (this.EntityPM.ShipmentLevelCode != "C") {
                        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            myResult = true;
                        }
                    }

                    break
                }

            case "REAGT":
            case "NOTF1":
            case "NOTF2":
            case "SHPNT":
            case "CONNT":
            case "CONSL":
                {
                    myResult = true;
                    break;
                }

            default: {
                myResult = false;
                break;
            }
        }

        return myResult;
    }

    get Name() {
        switch (this.Code) {
            case "AGENT": { return this.EntityPM.AgentName; }
            default: { return null; }
        }
    }
    set Name(newValue: string) {
        if (this.EntityPM.AgentName != newValue) {
            this.EntityPM.AgentName = newValue
        }

    }

    get Note() {
        return this.EntityPM.AgentNote;
    }
    set Note(newValue: string) {
        if (this.EntityPM.AgentNote != newValue) {
            this.EntityPM.AgentNote = newValue
        }
    }

    get IsCustomer() {
        return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
    }


    // PartnerId
    public PartnerCardList: CardList = null;

    get PartnerIdProperty() {
        return "AgentId"; 
    }

    get PartnerId() {
        return this.AgentId
    }
    set PartnerId(newValue: string) {
        this.AgentId = newValue;
    }



  
    get AgentId() {
        return this.EntityPM.AgentId;
    }
    set AgentId(newValue: string) {
        if (this.EntityPM.AgentId != newValue) {
            this.EntityPM.AgentId = newValue;
            this.IsDisableNextButton = true;
            this.fatherComponent._sharedAgentManifestService.GetIsAgentSharedManifests(this.EntityPM.AgentId, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var isDisableNextButton: boolean = myResponse.Result ? true : false;
                    this.IsDisableNextButton = isDisableNextButton;
                }
            });

            
            this.GetPartnerCard();
        }
    }




    get PartnerAddressIdProperty() {
       
            return "AgentAddressId";
    }

    get AddressId() {
        return this.AgentAddressId;
    }
    set AddressId(newValue: string) {
        this.AgentAddressId = newValue;
    }

  
    get AgentAddressId() {
        return this.EntityPM.AgentAddressId;
    }
    set AgentAddressId(newValue: string) {
        if (this.EntityPM.AgentAddressId != newValue) {
            this.EntityPM.AgentAddressId = newValue;
            this.GetPartnerAddress();
        }
    }



    // ContactId
    get PartnerContactIdProperty() {
        return "AgentContactId";
    }

    get ContactId() {
        return this.AgentContactId;
    }
    set ContactId(newValue: string) {
        this.AgentContactId = newValue; 
    }

  

    get AgentContactId() {
        return this.EntityPM.AgentContactId;
    }
    set AgentContactId(newValue: string) {
        if (this.EntityPM.AgentContactId != newValue) {
            this.EntityPM.AgentContactId = newValue;
            this.GetPartnerContact();
        }
    }



    // Reference1
    get HasReference1() {
        var myResult: boolean = true;

        return myResult;
    }
    get Reference1Property() {
        return "AgentReference1";
    }
    get Reference1() {
        return this.AgentReference1;
    }
    set Reference1(newValue: string) {
        this.AgentReference1 = newValue;
    }


    get AgentReference1() {
        return this.EntityPM.AgentReference1;
    }
    set AgentReference1(newValue: string) {
        if (this.EntityPM.AgentReference1 != newValue) {
            this.EntityPM.AgentReference1 = newValue;
        }
    }

  





    // Reference2
    get HasReference2() {
        var myResult: boolean = true;

        return myResult;
    }
    get Reference2Property() {
        return "AgentReference2";
    }
    get Reference2() {
        return this.AgentReference2;
    }
    set Reference2(newValue: string) {
        this.AgentReference2 = newValue;
    }

   
    get AgentReference2() {
        return this.EntityPM.AgentReference2;
    }
    set AgentReference2(newValue: string) {
        if (this.EntityPM.AgentReference2 != newValue) {
            this.EntityPM.AgentReference2 = newValue;
        }
    }


    public IsReseting: boolean = false;
    private isAddressLoaded = false;
    private isContactLoaded = false;
    public ShowNoTemplateText = false;
    public AddressCityText: string = null;
    public PartnerAddressList: AddressList;
    public PartnerContactList: ContactList;
    GetPartnerCard() {

        this.SetUIProperties();

        var myCardId: string = this.PartnerId;

        if (AppTool.IsNullOrEmpty(myCardId)) {
            this.Name = null;
            this.Note = null;
            this.PartnerCardList = null;
            this.AddressId = null;
            this.ContactId = null;

            if (this.Code == "SHIPR") {
                if (this.EntityPM.KnownConsignorNumber != null) {
                    this.EntityPM.KnownConsignorNumber = null;
                }

                if (this.EntityPM.KCExpirationDate != null) {
                    this.EntityPM.KCExpirationDate = null;
                }
            }
        }

        else {
            var myService = this.fatherComponent.CardListService;
            myService.getSingle(myCardId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.PartnerCardList = myResponse.Result;

                        if (this.PartnerCardList != null) {

                            this.Name = this.PartnerCardList.EnglishName;
                            this.Note = this.PartnerCardList.Notes;

                           

                            if (this.Code == "SHIPR") {

                                if (this.EntityPM.KnownConsignorNumber != this.PartnerCardList.KnownConsignor) {
                                    this.EntityPM.KnownConsignorNumber = this.PartnerCardList.KnownConsignor;
                                }

                                if (this.EntityPM.KCExpirationDate != this.PartnerCardList.KCExpirationDate) {
                                    this.EntityPM.KCExpirationDate = this.PartnerCardList.KCExpirationDate;
                                }
                            }

                            if (!this.IsReseting) {
                                this.AddressId = this.PartnerCardList.MainAddressId;
                                this.ContactId = this.PartnerCardList.PrimaryContactId;
                            }
                        }
                    }
                }
            });
        }
    }
    GetPartnerAddress() {

        this.isAddressLoaded = false;
        this.PartnerAddressList = null;
        this.ShowNoTemplateText = false;
        this.AddressCityText = null;

        var myAddressId: string = this.AddressId;

        if (myAddressId != null) {

            var list: AddressList = this.fatherComponent.AllAddresses.filter(f => f.Id == myAddressId)[0];
            if (list) {
                this.PartnerAddressList = list;
                this.isAddressLoaded = true;
                this.BuildAddressCityText();
                this.OnLoadCompleted();
            }

            else {
                var myService = this.fatherComponent.AddressListService;
                myService.getSingle(myAddressId).subscribe((myResponse: ServiceResponse) => {

                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.PartnerAddressList = myResponse.Result;
                            this.BuildAddressCityText();

                            if (this.PartnerAddressList) {
                                this.fatherComponent.AllAddresses.push(this.PartnerAddressList);
                            }
                        }
                    }

                    this.isAddressLoaded = true;
                    this.OnLoadCompleted();
                });
            }
        }

        else {
            this.isAddressLoaded = true;
            this.OnLoadCompleted();
        }
    }
    GetPartnerContact() {

        this.isContactLoaded = false;
        this.PartnerContactList = null;
        this.ShowNoTemplateText = false;

        var myContactId: string = this.ContactId;

        if (myContactId != null) {

            var list: ContactList = this.fatherComponent.AllContacts.filter(f => f.Id == myContactId)[0];
            if (list) {
                this.PartnerContactList = list;
                this.isContactLoaded = true;
                this.OnLoadCompleted();
            }

            var myService = this.fatherComponent.ContactListService;
            myService.getSingle(myContactId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.PartnerContactList = myResponse.Result;

                        if (this.PartnerContactList) {
                            this.fatherComponent.AllContacts.push(this.PartnerContactList);
                        }
                    }
                }

                this.isContactLoaded = true;
                this.OnLoadCompleted();
            });
        }

        else {
            this.isContactLoaded = true;
            this.OnLoadCompleted();
        }
    }
    OnLoadCompleted() {
        if (this.isAddressLoaded && this.isContactLoaded) {
            if (this.PartnerAddressList == null && this.PartnerContactList == null) {
                this.ShowNoTemplateText = true;
            }

            else {
                this.ShowNoTemplateText = false;
            }
        }
    }
    BuildAddressCityText() {
        var myResult = null;

        if (this.PartnerAddressList) {

            if (!AppTool.IsNullOrEmpty(this.PartnerAddressList.City)) {
                myResult = this.PartnerAddressList.City;
            }

            if (!AppTool.IsNullOrEmpty(this.PartnerAddressList.StateName)) {
                myResult = AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.StateName : myResult + ", " + this.PartnerAddressList.StateName;
            }

            if (!AppTool.IsNullOrEmpty(this.PartnerAddressList.ZipCode)) {
                myResult = AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.ZipCode : myResult + ", " + this.PartnerAddressList.ZipCode;
            }
        }

        this.AddressCityText = myResult;
    }

    // Add|Edit Partner
    private isEditButtonClicked: boolean = false;
    AddPartnerClicked() {
     

        var myPerspective: string = null;
        var myComponentPath: string = null;

        if (this.CardDependencyProperty1 == "AG") {
            myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
        }

        if (!this.IsCustomer) {
            myPerspective = "ShippersAndConsignees";
        }

        if (myComponentPath != null) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = "New " + this.PartnerTypeName;

            if (!AppTool.IsNullOrEmpty(myPerspective)) {
                var args = new NewEntityArgs();
                args.Perspective = myPerspective;
                logWindow.WindowArgs = args;
            }

            logWindow.Show(myComponentPath);

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.PartnerId = comp.EntityPM.Id;
                    }
                });
            });
        }
    }
  
    EditPartnerClicked() {

            if (!this.isEditButtonClicked) {
                if (!AppTool.IsNullOrEmpty(this.PartnerId)) {
                    this.isEditButtonClicked = true;

                    var myService = this.fatherComponent.CardListService;
                    myService.getSingle(this.PartnerId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: CardList = myResponse.Result;
                            if (list) {
                                var objectTableName: string = "Agent";


                                if (objectTableName != null) {
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Title = "Edit " + this.PartnerTypeName;
                                    logWindow.IsFillScreen = true;
                                    logWindow.ShowEditComponent(this.PartnerId, objectTableName);

                                    logWindow.ComponentLoaded.subscribe(comp => {
                                        logWindow.WindowClosed.subscribe(s => {

                                            this.Name = comp.EntityPM.EnglishName;
                                            this.Note = comp.EntityPM.Notes;

                                            if (this.Code == "SHIPR") {

                                                if (this.EntityPM.KnownConsignorNumber != comp.EntityPM.KnownConsignor) {
                                                    this.EntityPM.KnownConsignorNumber = comp.EntityPM.KnownConsignor;
                                                }

                                                if (this.EntityPM.KCExpirationDate != comp.EntityPM.KCExpirationDate) {
                                                    this.EntityPM.KCExpirationDate = comp.EntityPM.KCExpirationDate;
                                                }
                                            }

                                            this.GetPartnerAddress();
                                            this.GetPartnerContact();
                                            this.isEditButtonClicked = false;
                                        });
                                    });
                                }

                                else {
                                    this.isEditButtonClicked = false;
                                }
                            }

                            else {
                                this.isEditButtonClicked = false;
                            }
                        }

                        else {
                            this.isEditButtonClicked = false;
                        }
                    });
                }
            }
        
    }

    // Add|Edit Address
    AddAddressClicked() {

        var entityPM: AddressPM = new AddressPM();
        entityPM.Tenant = SessionLocator.Tenant;
        entityPM.AddressTypeId = "O";
        entityPM.CardId = this.PartnerId;

        var myPartnerTypeId: string = null;
        var isCustomer: boolean;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }

        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.AddressId = null;
                    this.AddressId = entityPM.Id;
                }
            });
        }
    }
    EditAddressClicked() {
        var myAddressId = this.AddressId;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {

                    var list = this.fatherComponent.AllAddresses.filter(f => f.Id == myAddressId)[0];
                    if (list) {
                        var indexOfList = this.fatherComponent.AllAddresses.indexOf(list);
                        this.fatherComponent.AllAddresses.splice(indexOfList, 1);
                    }

                    this.AddressId = null;
                    this.AddressId = myAddressId;
                }
            });
        }
    }

    private isPartnerWindowOpened: boolean;

}

