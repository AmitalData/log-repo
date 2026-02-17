import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {AddressService} from '../../../../Common/Services/ExtendedLists/AddressService';
import {AWBUtilities} from '../../../Utilities/AWBUtilities';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {BookingPM} from '../../../EntityPMs/BookingPM';
import {BookingWizardComponent} from '../BookingWizardComponent';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AddEditPartnerArgs} from '../../../Args';
import {BookingTool} from '../../../Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'PartnersTabComponent',
    moduleId: module.id,
    templateUrl: './PartnersTabComponent.html',
})

export class PartnersTabComponent extends BaseComponent {
    public Wizard: BookingWizardComponent;
    public EntityPM: BookingPM;
    public DataContext: PartnersTabComponent = this;
    public ObjectTableName: string;
    public LabelColumnWidth: number = 85;
    private myCardListService: CardListService;
    constructor() {
        super();
        this.InitializeServices();  
    }

    private InitializeServices() {
        if (this.myCardListService == null) {
            this.myCardListService = new CardListService();
        }
    }
    
    InitTab(wizard: BookingWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetLOVDependency();
        this.SetUIProperties();
        this.InitializePartners();
        this.Validate();
    }

    RefreshTab() {
        this.Validate();
    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.RefreshTab();
                    this.SetUIProperties();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.RefreshTab();
                    this.SetUIProperties();
                }
            });
        }
    }

    private InitializePartners() {

        if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
            if (!AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
                this.GetShipperAddress();
            }

            else {
                this.GetShipperMainAddress();
            }
        }

        if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            if (!AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
                this.GetConsigneeAddress();
            }

            else {
                this.GetConsigneeMainAddress();
            }
        }
        
        if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
            if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAddressId)) {
                this.GetIssuingCarrierAddress();
            }

            else {
                this.GetIssuingCarrierMainAddress();
            }
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id) && !this.EntityPM.IsCopyMode) {
            this.SetDefaultIssuingCarrierAgent();
        }
    }
    private isSetDefaultTenantAgent: boolean = false;
    private SetDefaultIssuingCarrierAgent() {

        this.CASSCode = InfraSettings.TenantPM.CASSCode;
        this.IssuingCarrierIATACode = InfraSettings.TenantPM.IATA;
        this.IssuingCarrierAgentId = InfraSettings.TenantPM.AgentId;

        if (this.EntityPM.BookingLevelCode == "C") {
            this.ShipperId = this.IssuingCarrierAgentId;
        }
    }
    
    public PartnerDependencyProperty1: string;
    public PartnerDependencyProperty1IsList: boolean;
    private SetLOVDependency() {
        var myDependency = "CS,AG";
        var myDependencyIsList = true;        
        
        this.PartnerDependencyProperty1 = myDependency;
        this.PartnerDependencyProperty1IsList = myDependencyIsList;
    }

    public IsEditingEnabled: boolean = false;
    public IsEditSHIEnabled: boolean = false;
    public IsEditCONEnabled: boolean = false;
    public IsEditAGTEnabled: boolean = false;

    public SetUIProperties() {
        this.IsEditingEnabled = BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);

        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierAgentId", this.ObjectTableName, this.IsEditingEnabled);

        var isShipperFieldEnabled = false;
        var isConsigneeFieldEnabled = false;
        var isIssuingFieldEnabled = false;

        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                isShipperFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isConsigneeFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
                isIssuingFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("ShipperAddressId", this.ObjectTableName, isShipperFieldEnabled);
        this.UIProperties.SetEnabled("ShipperReference", this.ObjectTableName, isShipperFieldEnabled);
        this.IsEditSHIEnabled = isShipperFieldEnabled;

        this.UIProperties.SetEnabled("ConsigneeAddressId", this.ObjectTableName, isConsigneeFieldEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference", this.ObjectTableName, isConsigneeFieldEnabled);
        this.IsEditCONEnabled = isConsigneeFieldEnabled;

        this.UIProperties.SetEnabled("IssuingCarrierAddressId", this.ObjectTableName, isIssuingFieldEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierIATACode", this.ObjectTableName, isIssuingFieldEnabled);
        this.UIProperties.SetEnabled("CASSCode", this.ObjectTableName, isIssuingFieldEnabled);
        this.IsEditAGTEnabled = isIssuingFieldEnabled;        
    }

    public ShipperWarning: string = "";
    public ConsigneeWarning: string = "";
    public AgentWarning: string = "";

    public ShowWarningShipperId: boolean = false;
    public ShowWarningConsigneeId: boolean = false;
    public ShowWarningAgentId: boolean = false;
    
    public ShowWarningAgentIATA: boolean = false;
    public ShowWarningAgentCASS: boolean = false;
    
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_PAR();
        this.Validate();
    }
    private Validate() {
        this.Validate_SHI();
        this.Validate_CON();
        this.Validate_AGT();
    }
    private Validate_SHI() {
        var warningMessage: string = "";
        
        if (this.ShipperId != null) {
            if (this.ShipperAddressList == null) {
                warningMessage = "Address is required";
            }

            else {
                var myAddressList = this.ShipperAddressList;
                var myAddress1: string = AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                var myAddress2: string = AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                var myCity: string = AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();

                if (!FormatTool.IsTextFormatted(myAddress1)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                }

                if (!FormatTool.IsTextFormatted(myAddress2)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                }

                if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
                }

                if (AppTool.IsNullOrEmpty(myAddressList.CountryId)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Country" : warningMessage + ",Country";
                }

                if (AppTool.IsNullOrEmpty(myAddressList.StateCode)) {
                    if (this.Wizard.AllStates.filter(d => d.CountryId == myAddressList.CountryId).length > 0) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "State" : warningMessage + ",State";
                    }
                }

                if (AppTool.IsNullOrEmpty(myCity)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }

                else if (!FormatTool.IsTextFormatted(myCity)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                
                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }
        }

        this.ShipperWarning = warningMessage;
    }
    private Validate_CON() {
        var warningMessage: string = "";
        
        if (this.ConsigneeId != null) {
            if (this.ConsigneeAddressList == null) {
                warningMessage = "Address is required";
            }

            else {
                var myAddressList = this.ConsigneeAddressList;
                var myAddress1: string = AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                var myAddress2: string = AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                var myZipCode: string = AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                var myCity: string = AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();

                if (!AWBUtilities.IsText(myAddress1)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                }

                if (!AWBUtilities.IsText(myAddress2)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                }

                if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
                }

                if (AppTool.IsNullOrEmpty(myAddressList.CountryId)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Country" : warningMessage + ",Country";
                }

                if (AppTool.IsNullOrEmpty(myAddressList.StateCode)) {
                    if (this.Wizard.AllStates.filter(d => d.CountryId == myAddressList.CountryId).length > 0) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "State" : warningMessage + ",State";
                    }
                }

                if (AppTool.IsNullOrEmpty(myCity)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }

                else if (!AWBUtilities.IsText(myCity)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }
                
                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }
        }

        this.ConsigneeWarning = warningMessage;
    }
    private Validate_AGT() {
        this.ShowWarningAgentId = this.IssuingCarrierAgentId == null ? true : false;      
        this.ShowWarningAgentIATA = false;

        if (!AppTool.IsNullOrEmpty(this.IssuingCarrierIATACode)) {
            if (!AWBUtilities.FormateValidate_IATACode(this.IssuingCarrierIATACode)) {
                this.ShowWarningAgentIATA = true;
            }
        }

        this.ShowWarningAgentCASS = false;
        if (!AppTool.IsNullOrEmpty(this.CASSCode)) {
            if (!AWBUtilities.FormateValidate_CASSCode(this.CASSCode)) {
                this.ShowWarningAgentCASS = true;
            }
        }

        var warningMessage: string = "";

        if (this.IssuingCarrierAgentId == null) {
            warningMessage = "Issuing Carrier Agent is required";
        }

        else {
            if (this.IssuingCarrierAddressList == null) {
                warningMessage = "Address is required";
            }

            else {
                var myCity = AppTool.IsNullOrEmpty(this.IssuingCarrierAddressList.City) ? null : this.IssuingCarrierAddressList.City.trim();

                if (AppTool.IsNullOrEmpty(myCity)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }

                else if (!AWBUtilities.IsText(myCity)) {
                    warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                }

                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }
        }

        this.AgentWarning = warningMessage;
    }

    // Shipper
    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
            this.SetUIProperties();
            this.GetShipperCard();
        }
    }

    get ShipperAddressId() { return this.EntityPM.ShipperAddressId; }
    set ShipperAddressId(newValue: string) {
        if (this.EntityPM.ShipperAddressId != newValue) {
            this.EntityPM.ShipperAddressId = newValue;
            this.GetShipperAddress();
        }
    }

    get ShipperReference() { return this.EntityPM.ShipperReference; }
    set ShipperReference(newValue: string) {
        if (this.EntityPM.ShipperReference != newValue) {
            this.EntityPM.ShipperReference = newValue;
        }
    }

    private GetShipperCard() {
        if (this.ShipperId == null) {
            this.ShipperAddressId = null;
            this.ShipperReference = null;
            this.ShipperAddressList = null;
            this.EntityPM.ShipperName = null;
            this.FireWizardEvent();
        }

        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {

                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.ShipperName = myCard.EnglishName;
                        this.GetShipperMainAddress();
                    }

                    else {
                        this.LoadShipperCard();
                    }
                }
            });
        }
    }
    private LoadShipperCard() {
        this.myCardListService.getSingle(this.ShipperId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {

                var myCard: CardList = myResponse.Result;

                if (myCard != null) {
                    this.EntityPM.ShipperName = myCard.EnglishName;
                }

                this.GetShipperMainAddress();
            }
        });
    }

    public ShipperAddressList: AddressList;
    private GetShipperAddress() {
        if (!AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
            var myService: AddressListService = new AddressListService();
            myService.getSingle(this.ShipperAddressId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myAddress: AddressList = myResponse.Result;
                    this.SetShipperAddress(myAddress);
                }
            });
        }
    }
    private GetShipperMainAddress() {
        if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
            var myService: AddressService = new AddressService();

            myService.GetMainAddressByCardId(this.ShipperId, this.Wizard.TenantPM.Id).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myAddress: AddressList = myResponse.Result;
                    this.SetShipperAddress(myAddress);
                }
            });
        }
    }
    private SetShipperAddress(list: AddressList) {
        this.ShipperAddressList = list;

        if (list == null) {
            if (this.EntityPM.ShipperAddressId != null) {
                this.EntityPM.ShipperAddressId = null;
            }
        }

        else {

            if (this.ShipperAddressId != list.Id) {
                this.ShipperAddressId = list.Id;
            }
            
            if (this.EntityPM.ShipperAddressId != list.Id) {
                this.EntityPM.ShipperAddressId = list.Id;
            }

            if (this.EntityPM.ShipperAddress1 != list.Address1) {
                this.EntityPM.ShipperAddress1 = list.Address1;
            }

            if (this.EntityPM.ShipperAddress2 != list.Address2) {
                this.EntityPM.ShipperAddress2 = list.Address2;
            }

            if (this.EntityPM.ShipperCity != list.City) {
                this.EntityPM.ShipperCity = list.City;
            }

            if (this.EntityPM.ShipperCountryId != list.CountryId) {
                this.EntityPM.ShipperCountryId = list.CountryId;
            }

            if (this.EntityPM.ShipperStateId != list.StateId) {
                this.EntityPM.ShipperStateId = list.StateId;
            }

            if (this.EntityPM.ShipperZipCode != list.ZipCode) {
                this.EntityPM.ShipperZipCode = list.ZipCode;
            }
        }

        this.FireWizardEvent();
    }
    
    // Consignee
    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;
            this.SetUIProperties();
            this.GetConsigneeCard();
        }
    }

    get ConsigneeAddressId() { return this.EntityPM.ConsigneeAddressId; }
    set ConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeAddressId != newValue) {
            this.EntityPM.ConsigneeAddressId = newValue;
            this.GetConsigneeAddress();
        }
    }

    get ConsigneeReference() { return this.EntityPM.ConsigneeReference; }
    set ConsigneeReference(newValue: string) {
        if (this.EntityPM.ConsigneeReference != newValue) {
            this.EntityPM.ConsigneeReference = newValue;
        }
    }

    private GetConsigneeCard() {
        if (this.ConsigneeId == null) {
            this.ConsigneeAddressId = null;
            this.ConsigneeReference = null;
            this.ConsigneeAddressList = null;
            this.EntityPM.ConsigneeName = null;
            this.FireWizardEvent();
        }

        else {
            this.myCardListService.getSingle(this.ConsigneeId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                if (myCard != null) {
                    this.EntityPM.ConsigneeName = myCard.EnglishName;
                    this.GetConsigneeMainAddress();
                }
                else {
                        this.LoadConsigneeCard();
                }
                }
            });
        }
    }
    private LoadConsigneeCard() {
        this.myCardListService.getSingle(this.ConsigneeId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {

                var myCard: CardList = myResponse.Result;

                if (myCard != null) {
                    this.EntityPM.ConsigneeName = myCard.EnglishName;
                }

                    this.GetConsigneeMainAddress();
                }
        });
    }

    public ConsigneeAddressList: AddressList;
    private GetConsigneeAddress() {
        if (!AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
            var myService: AddressListService = new AddressListService();
            myService.getSingle(this.ConsigneeAddressId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myAddress: AddressList = myResponse.Result;
                    this.SetConsigneeAddress(myAddress);
                }
            });
        }
    }
    private GetConsigneeMainAddress() {
        if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            var myService: AddressService = new AddressService();
            myService.GetMainAddressByCardId(this.ConsigneeId, this.Wizard.TenantPM.Id).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myAddress: AddressList = myResponse.Result;
                    this.SetConsigneeAddress(myAddress);
                }
            });
        }
    }
    private SetConsigneeAddress(list: AddressList) {
        this.ConsigneeAddressList = list;

        if (list == null) {
            if (this.EntityPM.ConsigneeAddressId != null) {
                this.EntityPM.ConsigneeAddressId = null;
            }
        }

        else {

            if (this.EntityPM.ConsigneeAddressId != list.Id) {
                this.EntityPM.ConsigneeAddressId = list.Id;
            }

            if (this.EntityPM.ConsigneeAddress1 != list.Address1) {
                this.EntityPM.ConsigneeAddress1 = list.Address1;
            }

            if (this.EntityPM.ConsigneeAddress2 != list.Address2) {
                this.EntityPM.ConsigneeAddress2 = list.Address2;
            }

            if (this.EntityPM.ConsigneeCity != list.City) {
                this.EntityPM.ConsigneeCity = list.City;
            }

            if (this.EntityPM.ConsigneeCountryId != list.CountryId) {
                this.EntityPM.ConsigneeCountryId = list.CountryId;
            }

            if (this.EntityPM.ConsigneeStateId != list.StateId) {
                this.EntityPM.ConsigneeStateId = list.StateId;
            }

            if (this.EntityPM.ConsigneeZipCode != list.ZipCode) {
                this.EntityPM.ConsigneeZipCode = list.ZipCode;
            }
        }

        this.FireWizardEvent();
    }

    // IssuingCarrier
    get IssuingCarrierAgentId() { return this.EntityPM.IssuingCarrierAgentId; }
    set IssuingCarrierAgentId(newValue: string) {
        if (this.EntityPM.IssuingCarrierAgentId != newValue) {
            this.EntityPM.IssuingCarrierAgentId = newValue;
            this.SetUIProperties();
            this.GetIssuingCarrierCard();
        }
    }

    get IssuingCarrierAddressId() { return this.EntityPM.IssuingCarrierAddressId; }
    set IssuingCarrierAddressId(newValue: string) {
        if (this.EntityPM.IssuingCarrierAddressId != newValue) {
            this.EntityPM.IssuingCarrierAddressId = newValue;
            this.GetIssuingCarrierAddress();
        }
    }
    
    get IssuingCarrierIATACode() { return this.EntityPM.IssuingCarrierIATACode; }
    set IssuingCarrierIATACode(newValue: string) {
        if (this.EntityPM.IssuingCarrierIATACode != newValue) {
            this.EntityPM.IssuingCarrierIATACode = newValue;
            this.FireWizardEvent();
        }
    }

    get CASSCode() { return this.EntityPM.CASSCode; }
    set CASSCode(newValue: string) {
        if (this.EntityPM.CASSCode != newValue) {
            this.EntityPM.CASSCode = newValue;
            this.FireWizardEvent();
        }
    }
    
    private GetIssuingCarrierCard() {
        if (this.IssuingCarrierAgentId == null) {
            this.IssuingCarrierAddressId = null;
            this.IssuingCarrierAddressList = null;
            this.EntityPM.IssuingCarrierAgentName = null;
            this.FireWizardEvent();
        }

        else {            
            this.myCardListService.getSingle(this.IssuingCarrierAgentId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                            this.CASSCode = myCard.CASSCode;
                            this.IssuingCarrierIATACode = myCard.IATACode;

                        this.GetIssuingCarrierMainAddress();
                    }

                    else {
                        this.LoadIssuingCarrierCard();
                    }
                }
            });
        }
    }
    private LoadIssuingCarrierCard() {
        this.myCardListService.getSingle(this.IssuingCarrierAgentId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                var myCard: CardList = myResponse.Result;

                if (myCard != null) {
                    this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                        this.CASSCode = myCard.CASSCode;
                        this.IssuingCarrierIATACode = myCard.IATACode;
                    }

                    this.GetIssuingCarrierMainAddress();
                }
        });
    }

    public IssuingCarrierAddressList: AddressList;
    private GetIssuingCarrierAddress() {
        if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAddressId)) {
            var myService: AddressListService = new AddressListService();
            myService.getSingle(this.IssuingCarrierAddressId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    var myAddress: AddressList = myResponse.Result;
                    this.SetIssuingCarrierAddress(myAddress);
                }
            });
        }
    }
    private GetIssuingCarrierMainAddress() {
        if (!this.isSetDefaultTenantAgent) {
            if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
                var myService: AddressService = new AddressService();
                myService.GetMainAddressByCardId(this.IssuingCarrierAgentId, this.Wizard.TenantPM.Id).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;

                    if (!myResponse.HasError) {
                        var myAddress: AddressList = myResponse.Result;
                        this.SetIssuingCarrierAddress(myAddress);
                    }
                });
            }
        }
    }
    private SetIssuingCarrierAddress(list: AddressList) {
        this.IssuingCarrierAddressList = list;

        if (list == null) {
            if (this.EntityPM.IssuingCarrierAddressId != null) {
                this.EntityPM.IssuingCarrierAddressId = null;
            }
        }

        else {
            if (this.EntityPM.IssuingCarrierAddressId != list.Id) {
                this.EntityPM.IssuingCarrierAddressId = list.Id;
            }
        }
        
        this.FireWizardEvent();
    }

    // Add|Edit
    private isPartnerWindowOpened: boolean;
    Add(myPartnerTypeCode: string) {
        this.AddEdit(myPartnerTypeCode, true);
    }
    Edit(myPartnerTypeCode: string) {
        this.AddEdit(myPartnerTypeCode, false);
    }
    AddEdit(myPartnerTypeCode: string, isNewPartner: boolean) {
        if (!this.isPartnerWindowOpened) {
            this.isPartnerWindowOpened = true;

            var myTitle = isNewPartner ? "Add " : "Edit ";

            switch (myPartnerTypeCode) {
                case "SHI": { myTitle += "Shipper"; break; }
                case "CON": { myTitle += "Consignee"; break; }
                case "AGT": { myTitle += "Issuing Carrier's Agent"; break; }
            }

            var windowArgs = new AddEditPartnerArgs();
            windowArgs.EntityPM = this.EntityPM;
            windowArgs.IsNewEntity = isNewPartner;
            windowArgs.PartnerTypeCode = myPartnerTypeCode;
            windowArgs.FatherComponent = this;

            var logWindow = new LogitudeWindow();
            logWindow.Title = myTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Booking/Components/BookingWizard/Partners/AddEditPartnerComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.isPartnerWindowOpened = false;
            });
        }
    }
    public UpdatePartner(myPartnerTypeCode: string, myPartnerId: string, myAddressId: string) {
        switch (myPartnerTypeCode) {
            case "SHI":
                {
                    if (this.ShipperId != myPartnerId) {
                        this.ShipperId = myPartnerId;
                    }

                    else {
                        this.EntityPM.ShipperAddressId = myAddressId;
                        this.LoadShipperCard();
                    }

                    break;
                }

            case "CON":
                {
                    if (this.ConsigneeId != myPartnerId) {
                        this.ConsigneeId = myPartnerId;
                    }

                    else {
                        this.ConsigneeId = null;
                        this.ConsigneeId = myPartnerId;

                        this.EntityPM.ConsigneeAddressId = myAddressId;
                        this.LoadConsigneeCard();
                    }

                    break;
                }
                
            case "AGT":
                {
                    if (this.IssuingCarrierAgentId != myPartnerId) {
                        this.IssuingCarrierAgentId = myPartnerId;
                    }

                    else {
                        this.EntityPM.IssuingCarrierAddressId = myAddressId;
                        this.LoadIssuingCarrierCard();
                    }

                    break;
                }
        }
    }
}