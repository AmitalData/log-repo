import {Component, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../../Common/EntityLists/AddressList';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../../Common/Services/StandardLists/AddressListService';
import {AddressService} from '../../../../../Common/Services/ExtendedLists/AddressService';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool, FormatTool} from '../../../../../Infrastructure/Tools';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {AddEditPartnerArgs} from '../../../../../Shipment/Args';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';

@Component({
    moduleId: module.id,
    selector: 'PartnersTabComponent',
    templateUrl: './AWBPartnersTabComponent.html',
})

export class AWBPartnersTabComponent extends BaseComponent
{
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: AWBPartnersTabComponent = this;
    public ObjectTableName: string;
    public LabelColumnWidth: number = 85;
    public PartnerBoxHeight: number = 200;
    private IsFirstTime: boolean = true;
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

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetWarningInfo();
        this.SetLOVDependency();
        this.SetUIProperties();
        this.InitializePartners();
        this.Validate();
    }

    RefreshTab() {
        this.Validate();
        this.SetUIProperties();
    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                }
            });
        }
    }

    public IsEditingEnabled: boolean = false;
    public IsEditSHIEnabled: boolean = false;
    public IsEditCONEnabled: boolean = false;
    public IsEditNTFEnabled: boolean = false;
    public ViaColoaderIsVisible: boolean = false;
    public ViaColoaderHeader: string = TextCodeTranslator.Translate("Shipment.F.IssuingCarrierAgentId");
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Notify1();
        this.SetUIProperties_IssuingCarrier();
    }
    private SetUIProperties_Shipper() {
        var isFieldFilled = this.ShipperId == null ? false : true;
        this.IsEditSHIEnabled = this.IsEditingEnabled;
        if (this.IsEditSHIEnabled) {
            this.IsEditSHIEnabled = isFieldFilled;
        }

        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, !isFieldFilled);
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ShipperReference1", this.ObjectTableName, this.IsEditingEnabled);
    }
    private SetUIProperties_Consignee() {
        var isFieldFilled = this.ConsigneeId == null ? false : true;
        this.IsEditCONEnabled = this.IsEditingEnabled;
        if (this.IsEditCONEnabled) {
            this.IsEditCONEnabled = isFieldFilled;
        }

        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference1", this.ObjectTableName, this.IsEditingEnabled);
    }
    private SetUIProperties_Notify1() {
        var isFieldFilled = this.Notify1Id == null ? false : true;
        this.IsEditNTFEnabled = this.IsEditingEnabled;
        if (this.IsEditNTFEnabled) {
            this.IsEditNTFEnabled = isFieldFilled;
        }

        this.UIProperties.SetEnabled("Notify1Id", this.ObjectTableName, this.IsEditingEnabled);
    }
    private SetUIProperties_IssuingCarrier() {
        var isAgentFieldEnabled = this.IsEditingEnabled;
        if (isAgentFieldEnabled) {
            isAgentFieldEnabled = this.EntityPM.ViaColoader ? true : false;
        }

        if (SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
            this.ViaColoaderIsVisible = true;
            this.ViaColoaderHeader = TextCodeTranslator.Translate("Shipment.S.Partners.IssuingCarrierColoader");
        }

        else {
            if (SessionLocator.TenantManagementJS.PackageCode != "BUBK" && SessionLocator.TenantManagementJS.PackageCode != "EAWB" && SessionLocator.TenantManagementJS.PackageCode != "EACR") {
                this.ViaColoaderIsVisible = true;
                this.ViaColoaderHeader = TextCodeTranslator.Translate("Shipment.S.Partners.IssuingCarrierColoader");
            }
        }

        this.UIProperties.SetEnabled("IssuingCarrierAgentId", this.ObjectTableName, isAgentFieldEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierAddressId", this.ObjectTableName, isAgentFieldEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierIATACode", this.ObjectTableName, isAgentFieldEnabled);
        this.UIProperties.SetEnabled("CASSCode", this.ObjectTableName, isAgentFieldEnabled);
        this.UIProperties.SetEnabled("IssuingCarrierReference1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ViaColoader", this.ObjectTableName, this.IsEditingEnabled);
    }   

    public IsBookingConnectWarningVisible: boolean = false;
    private SetWarningInfo() {
        var myResult = false;

        if (FeatureLocator.HasFeaturePermession("Booking", "Module")) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (this.EntityPM.ShipmentLevelCode != "H" && AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                    myResult = true;
                }
            }
        }

        this.IsBookingConnectWarningVisible = myResult;
        this.PartnerBoxHeight = myResult == true ? 200 : 213;
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

        if (!AppTool.IsNullOrEmpty(this.Notify1Id)) {
            if (!AppTool.IsNullOrEmpty(this.Notify1AddressId)) {
                this.GetNotify1Address();
            }

            else {
                this.GetNotify1MainAddress();
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

        this.SetDefaultIssuingCarrierAgent();
    }
    private isSetDefaultTenantAgent: boolean = false;
    private SetDefaultIssuingCarrierAgent() {
        if (!this.EntityPM.IsCopyFromShipment) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                this.isSetDefaultTenantAgent = true;

                if (AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                    this.CASSCode = this.Wizard.TenantPM.CASSCode;
                    this.IssuingCarrierIATACode = this.Wizard.TenantPM.IATA;
                }

                this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;

                if (!this.ViaColoader) {
                    this.IssuingCarrierAgentId = this.Wizard.TenantPM.AgentId;

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                            this.ShipperId = this.IssuingCarrierAgentId;
                        }

                        else {
                            if (AppTool.IsNullOrEmpty(this.EntityPM.ShipperId)) {
                                this.ShipperId = this.IssuingCarrierAgentId;
                            }
                        }
                    }
                }
            }

            else {
                if (this.IssuingCarrierAgentId == this.Wizard.TenantPM.AgentId) {
                    this.isSetDefaultTenantAgent = true;
                }
            }
        }
    }

    public PartnerDependencyProperty1: string;
    public PartnerDependencyProperty1IsList: boolean;
    private SetLOVDependency() {
        var myDependency = "CS";
        var myDependencyIsList = false;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            myDependency = "AG";
            myDependencyIsList = false;
        }

        else {
            if (this.Wizard.TenantPM.AllowAgentInCustomersLOV) {
                myDependency = "CS,AG";
                myDependencyIsList = true;
            }
        }

        this.PartnerDependencyProperty1 = myDependency;
        this.PartnerDependencyProperty1IsList = myDependencyIsList;
    }   

    public ShipperWarning: string = "";
    public ConsigneeWarning: string = "";
    public Notify1Warning: string = "";
    public AgentWarning: string = "";
    public ShowWarningConsigneeId: boolean = false;
    public ShowWarningShipperAddressId: boolean = false;
    public ShowWarningConsigneeAddressId: boolean = false;
    public ShowWarningNotify1AddressId: boolean = false;

    public ShowWarningAgentId: boolean = false;
    public ShowWarningAgentAddressId: boolean = false;
    public ShowWarningAgentIATA: boolean = false;
    public ShowWarningAgentCASS: boolean = false;
    public ShowWarningAgentReference: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_PAR();
    }
    private Validate() {
        this.Validate_SHI();
        this.Validate_CON();
        this.Validate_AGT();
        this.Validate_NTF();        
    }
    private Validate_SHI() {

        var warningMessage: string = "";
        var showAddressWarning = false;

        if (this.ShipperId == null) {
            warningMessage = "Shipper is required";
        }

        else {
            if (this.ShipperAddressId == null) {
                warningMessage = "Address is required";
            }

            else {
                var myAddressList = this.ShipperAddressList;

                if (myAddressList != null) {
                    var myAddress1: string = AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                    var myAddress2: string = AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                    var myZipCode: string = AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                    var myCity: string = AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                    var myFaxNumber: string = AppTool.IsNullOrEmpty(myAddressList.FaxNumber) ? null : myAddressList.FaxNumber.trim();
                    var myPhoneNumber: string = AppTool.IsNullOrEmpty(myAddressList.PhoneNumber) ? null : myAddressList.PhoneNumber.trim();

                    if (!FormatTool.IsTextFormatted(myAddress1)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                    }

                    if (!FormatTool.IsTextFormatted(myAddress2)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                    }

                    if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
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

                    if (AppTool.IsNullOrEmpty(myZipCode)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                    }

                    else if (!FormatTool.IsTextFormatted(myZipCode)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                    }

                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (AppTool.IsNullOrEmpty(myFaxNumber) && AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Phone Or Fax" : warningMessage + ",Phone Or Fax";
                        }
                    }
                }

                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }

            if (!AppTool.IsNullOrEmpty(warningMessage)) {
                showAddressWarning = true;
            }
        }

        this.ShipperWarning = warningMessage;
        this.ShowWarningShipperAddressId = showAddressWarning;
    }
    private Validate_CON() {
        this.ShowWarningConsigneeId = this.ConsigneeId == null ? true : false;

        var warningMessage: string = "";
        var showAddressWarning = false;

        if (this.ConsigneeId == null) {
            warningMessage = "Consignee is required";
        }

        else {
            if (this.ConsigneeAddressId == null) {
                warningMessage = "Address is required";
            }

            else {
                var myAddressList = this.ConsigneeAddressList;

                if (myAddressList != null) {
                    var myAddress1: string = AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                    var myAddress2: string = AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                    var myZipCode: string = AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                    var myCity: string = AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                    var myFaxNumber: string = AppTool.IsNullOrEmpty(myAddressList.FaxNumber) ? null : myAddressList.FaxNumber.trim();
                    var myPhoneNumber: string = AppTool.IsNullOrEmpty(myAddressList.PhoneNumber) ? null : myAddressList.PhoneNumber.trim();

                    if (!FormatTool.IsTextFormatted(myAddress1)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                    }

                    if (!FormatTool.IsTextFormatted(myAddress2)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                    }

                    if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
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

                    if (AppTool.IsNullOrEmpty(myZipCode)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                    }

                    else if (!FormatTool.IsTextFormatted(myZipCode)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                    }

                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (AppTool.IsNullOrEmpty(myFaxNumber) && AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Phone Or Fax" : warningMessage + ",Phone Or Fax";
                        }
                    }
                }

                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }

            if (!AppTool.IsNullOrEmpty(warningMessage)) {
                showAddressWarning = true;
            }
        }

        this.ConsigneeWarning = warningMessage;
        this.ShowWarningConsigneeAddressId = showAddressWarning;
    }
    private Validate_AGT() {
        if (this.Wizard.IsFWB) {
           
            this.ShowWarningAgentId = this.IssuingCarrierAgentId == null ? true : false;
            this.ShowWarningAgentAddressId = false;

            this.ShowWarningAgentReference = false;
            if (this.IssuingCarrierAgentId != null) {
                if (this.ViaColoader) {
                    if (this.Wizard.CCSTypeCode == "GLSHK") {
                        if (AppTool.IsNullOrEmpty(this.IssuingCarrierReference1)) {
                            this.ShowWarningAgentReference = true;
                        }
                    }
                }
            }

            this.ShowWarningAgentIATA = false;
            if (!AppTool.IsNullOrEmpty(this.IssuingCarrierIATACode)) {
                if (!FormatTool.Validate_IATACode(this.IssuingCarrierIATACode)) {
                    this.ShowWarningAgentIATA = true;
                }
            }

            this.ShowWarningAgentCASS = false;
            if (!AppTool.IsNullOrEmpty(this.CASSCode)) {
                if (!FormatTool.Validate_CASSCode(this.CASSCode)) {
                    this.ShowWarningAgentCASS = true;
                }
            }

            var warningMessage: string = "";
 
            if (this.IssuingCarrierAgentId == null) {
                warningMessage = "Issuing Carrier Agent is required";
            }

            else {
                if (this.IssuingCarrierAddressId == null) {
                    warningMessage = "Address is required";
                }

                else {
                    if (this.IssuingCarrierAddressList != null) {

                        var myCity = AppTool.IsNullOrEmpty(this.IssuingCarrierAddressList.City) ? null : this.IssuingCarrierAddressList.City.trim();

                        if (AppTool.IsNullOrEmpty(myCity)) {
                            warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                        }

                        else if (!FormatTool.IsTextFormatted(myCity)) {
                            warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "City" : warningMessage + ",City";
                        }
                    }

                    if (!AppTool.IsNullOrEmpty(warningMessage)) {
                        warningMessage += " is required";
                    }
                }

                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    this.ShowWarningAgentAddressId = true;
                }
            }

            this.AgentWarning = warningMessage;
        }
    }
    private Validate_NTF() {

        var warningMessage: string = "";
        var showAddressWarning = false;

        if (this.Notify1Id != null) {

            if (this.Notify1AddressId == null) {
                warningMessage = "Address is required";
            }

            else {
                var myAddressList = this.Notify1AddressList;
                if (myAddressList != null) {
                    var myAddress1: string = AppTool.IsNullOrEmpty(myAddressList.Address1) ? null : myAddressList.Address1.trim();
                    var myAddress2: string = AppTool.IsNullOrEmpty(myAddressList.Address2) ? null : myAddressList.Address2.trim();
                    var myZipCode: string = AppTool.IsNullOrEmpty(myAddressList.ZipCode) ? null : myAddressList.ZipCode.trim();
                    var myCity: string = AppTool.IsNullOrEmpty(myAddressList.City) ? null : myAddressList.City.trim();
                    var myFaxNumber: string = AppTool.IsNullOrEmpty(myAddressList.FaxNumber) ? null : myAddressList.FaxNumber.trim();
                    var myPhoneNumber: string = AppTool.IsNullOrEmpty(myAddressList.PhoneNumber) ? null : myAddressList.PhoneNumber.trim();

                    if (!FormatTool.IsTextFormatted(myAddress1)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1" : warningMessage + ",Address1";
                    }

                    if (!FormatTool.IsTextFormatted(myAddress2)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address2" : warningMessage + ",Address2";
                    }

                    if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Address1 or Address2" : warningMessage + ",Address1 or Address2";
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

                    if (AppTool.IsNullOrEmpty(myZipCode)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                    }

                    else if (!FormatTool.IsTextFormatted(myZipCode)) {
                        warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Zip Code" : warningMessage + ",Zip Code";
                    }

                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (AppTool.IsNullOrEmpty(myFaxNumber) && AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            warningMessage = AppTool.IsNullOrEmpty(warningMessage) ? "Phone Or Fax" : warningMessage + ",Phone Or Fax";
                        }
                    }
                }

                if (!AppTool.IsNullOrEmpty(warningMessage)) {
                    warningMessage += " is required";
                }
            }

            if (!AppTool.IsNullOrEmpty(warningMessage)) {
                showAddressWarning = true;
            }
        }

        this.Notify1Warning = warningMessage;
        this.ShowWarningNotify1AddressId = showAddressWarning;
    }

    // Regualted Agent Field Changed
    private RAFieldChanged() {
        if (this.Wizard.IsTabVisible_RAD) {
            ShipmentTool.OnRegulatedAgentFieldChanged(this.EntityPM);
        }
    }

    // Shipper
    private isPartnerChanged_Shipper: boolean;
    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;
            this.isPartnerChanged_Shipper = true;
            this.SetUIProperties_Shipper();
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

    get ShipperReference1() { return this.EntityPM.ShipperReference1; }
    set ShipperReference1(newValue: string) {
        if (this.EntityPM.ShipperReference1 != newValue) {
            this.EntityPM.ShipperReference1 = newValue;
        }
    }

    private GetShipperCard() {
        
        this.EntityPM.SalesmanUserId = this.EntityPM.CreatedByUserId;
        this.EntityPM.AccountManagerUserId = this.EntityPM.CreatedByUserId;

        if (this.ShipperId == null) {
            this.ShipperAddressId = null;
            this.ShipperReference1 = null;
            this.ShipperAddressList = null;
            this.EntityPM.ShipperName = null;
            this.EntityPM.ShipperNote = null;
            this.EntityPM.ShipperContactId = null;
            this.EntityPM.KnownConsignorNumber = null;
            this.EntityPM.KCExpirationDate = null;
            this.Validate_SHI();
            this.RAFieldChanged();
            this.FireWizardEvent();            
        }

        else {
            this.LoadShipperCard();
        }
    }
    private LoadShipperCard() {

        this.myCardListService.getSingle(this.ShipperId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.ShipperName = myCard.EnglishName;
                        this.EntityPM.ShipperNote = myCard.Notes;
                        this.EntityPM.ShipperContactId = myCard.PrimaryContactId;
                        this.EntityPM.KnownConsignorNumber = myCard.KnownConsignor;
                        this.EntityPM.KCExpirationDate = myCard.KCExpirationDate;
                        this.RAFieldChanged();

                        if (myCard.SalesmanUserId != null) {
                            this.EntityPM.SalesmanUserId = myCard.SalesmanUserId;
                        }

                        if (myCard.AccountManagerUserId != null) {
                            this.EntityPM.AccountManagerUserId = myCard.AccountManagerUserId;
                        }
                    }

                    if (this.isPartnerChanged_Shipper) {
                        this.GetShipperMainAddress();
                    }

                    else {
                        this.GetShipperAddress();
                    }

                    this.isPartnerChanged_Shipper = false;
                }
            }
        });
    }

    public ShipperAddressList: AddressList;
    private GetShipperAddress() {
        if (!AppTool.IsNullOrEmpty(this.ShipperAddressId)) {
            var myService: AddressListService = new AddressListService();
            
            myService.getSingle(this.ShipperAddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetShipperAddress(myResponse.Result);
                    }
                }
            });
        }
    }
    private GetShipperMainAddress() {
        if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
            var myService: AddressService = new AddressService();

            myService.GetMainAddressByCardId(this.ShipperId, this.Wizard.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetShipperAddress(myResponse.Result);
                    }
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

            if (this.EntityPM.ShipperFaxNumber != list.FaxNumber) {
                this.EntityPM.ShipperFaxNumber = list.FaxNumber;
            }

            if (this.EntityPM.ShipperPhoneNumber != list.PhoneNumber) {
                this.EntityPM.ShipperPhoneNumber = list.PhoneNumber;
            }
        }

        this.Validate_SHI();
        this.FireWizardEvent();
        this.SetMyCustomer();
    }

    private SetMyCustomer() {
        if (this.EntityPM.ShipmentLevelCode != "C") {

            if (this.Wizard.IsNewEntity) {
                if (this.EntityPM.ShipmentCustomerTypeCode != "SHI") {
                    this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                }
            }

            if (this.EntityPM.ShipmentCustomerTypeCode == "SHI") {
                if (this.EntityPM.CustomerId != this.EntityPM.ShipperId) {
                    this.EntityPM.CustomerId = this.EntityPM.ShipperId;
                }

                if (this.EntityPM.CustomerName != this.EntityPM.ShipperName) {
                    this.EntityPM.CustomerName = this.EntityPM.ShipperName;
                }

                if (this.EntityPM.CustomerNote != this.EntityPM.ShipperNote) {
                    this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
                }

                if (this.EntityPM.CustomerAddressId != this.EntityPM.ShipperAddressId) {
                    this.EntityPM.CustomerAddressId = this.EntityPM.ShipperAddressId;
                }

                if (this.EntityPM.CustomerContactId != this.EntityPM.ShipperContactId) {
                    this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
                }
            }
        }
    }

    // Consignee
    private isPartnerChanged_Consignee: boolean;
    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;
            this.isPartnerChanged_Consignee = true;
            this.SetUIProperties_Consignee();
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

    get ConsigneeReference1() { return this.EntityPM.ConsigneeReference1; }
    set ConsigneeReference1(newValue: string) {
        if (this.EntityPM.ConsigneeReference1 != newValue) {
            this.EntityPM.ConsigneeReference1 = newValue;
        }
    }

    private GetConsigneeCard() {
        
        if (this.ConsigneeId == null) {
            this.ConsigneeAddressId = null;
            this.ConsigneeReference1 = null;
            this.ConsigneeAddressList = null;
            this.EntityPM.ConsigneeName = null;
            this.EntityPM.ConsigneeNote = null;
            this.EntityPM.ConsigneeContactId = null;
            this.Validate_CON();
            this.FireWizardEvent();
        }

        else {
            this.LoadConsigneeCard();
        }
    }
    private LoadConsigneeCard() {
        this.myCardListService.getSingle(this.ConsigneeId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.ConsigneeName = myCard.EnglishName;
                        this.EntityPM.ConsigneeNote = myCard.Notes;
                        this.EntityPM.ConsigneeContactId = myCard.PrimaryContactId;
                    }

                    if (this.isPartnerChanged_Consignee) {
                        this.GetConsigneeMainAddress();
                    }

                    else {
                        this.GetConsigneeAddress();
                    }

                    this.isPartnerChanged_Consignee = false;
                }
            }
        });
    }

    public ConsigneeAddressList: AddressList;
    private GetConsigneeAddress() {
        if (!AppTool.IsNullOrEmpty(this.ConsigneeAddressId)) {
            var myService: AddressListService = new AddressListService();
            
            myService.getSingle(this.ConsigneeAddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetConsigneeAddress(myResponse.Result);
                    }
                }
            });
        }
    }
    private GetConsigneeMainAddress() {
        if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            var myService: AddressService = new AddressService();
            myService.GetMainAddressByCardId(this.ConsigneeId, this.Wizard.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetConsigneeAddress(myResponse.Result);
                    }
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

            if (this.EntityPM.ConsigneeFaxNumber != list.FaxNumber) {
                this.EntityPM.ConsigneeFaxNumber = list.FaxNumber;
            }

            if (this.EntityPM.ConsigneePhoneNumber != list.PhoneNumber) {
                this.EntityPM.ConsigneePhoneNumber = list.PhoneNumber;
            }
        }

        this.Validate_CON();
        this.FireWizardEvent();
    }

    // Notify1
    private isPartnerChanged_Notify1: boolean;
    get Notify1Id() { return this.EntityPM.Notify1Id; }
    set Notify1Id(newValue: string) {
        if (this.EntityPM.Notify1Id != newValue) {
            this.EntityPM.Notify1Id = newValue;
            this.isPartnerChanged_Notify1 = true;
            this.SetUIProperties_Notify1();
            this.GetNotify1Card();
        }
    }

    get Notify1AddressId() { return this.EntityPM.Notify1AddressId; }
    set Notify1AddressId(newValue: string) {
        if (this.EntityPM.Notify1AddressId != newValue) {
            this.EntityPM.Notify1AddressId = newValue;
            this.GetNotify1Address();
        }
    }

    private GetNotify1Card() {        

        if (this.Notify1Id == null) {
            this.Notify1AddressId = null;
            this.Notify1AddressList = null;
            this.EntityPM.Notify1Name = null;
            this.EntityPM.Notify1Note = null;
            this.EntityPM.Notify1ContactId = null;
            this.Validate_NTF();
            this.FireWizardEvent();
        }

        else {
            this.LoadNotify1Card();
        }
    }
    private LoadNotify1Card() {
        this.myCardListService.getSingle(this.Notify1Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.Notify1Name = myCard.EnglishName;
                        this.EntityPM.Notify1Note = myCard.Notes;
                        this.EntityPM.Notify1ContactId = myCard.PrimaryContactId;
                    }

                    if (this.isPartnerChanged_Notify1) {
                        this.GetNotify1MainAddress();
                    }

                    else {
                        this.GetNotify1Address();
                    }

                    this.isPartnerChanged_Notify1 = false;
                }
            }
        });
    }

    public Notify1AddressList: AddressList;
    private GetNotify1Address() {
        if (!AppTool.IsNullOrEmpty(this.Notify1AddressId)) {
            var myService: AddressListService = new AddressListService();
            
            myService.getSingle(this.Notify1AddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetNotify1Address(myResponse.Result);
                    }
                }
            });
        }
    }
    private GetNotify1MainAddress() {
        if (!AppTool.IsNullOrEmpty(this.Notify1Id)) {
            var myService: AddressService = new AddressService();
            myService.GetMainAddressByCardId(this.Notify1Id, this.Wizard.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetNotify1Address(myResponse.Result);
                    }
                }
            });
        }
    }
    private SetNotify1Address(list: AddressList) {
        this.Notify1AddressList = list;

        if (list == null) {
            if (this.EntityPM.Notify1AddressId != null) {
                this.EntityPM.Notify1AddressId = null;
            }
        }

        else {

            if (this.EntityPM.Notify1AddressId != list.Id) {
                this.EntityPM.Notify1AddressId = list.Id;
            }

            if (this.EntityPM.Notify1Address1 != list.Address1) {
                this.EntityPM.Notify1Address1 = list.Address1;
            }

            if (this.EntityPM.Notify1Address2 != list.Address2) {
                this.EntityPM.Notify1Address2 = list.Address2;
            }

            if (this.EntityPM.Notify1City != list.City) {
                this.EntityPM.Notify1City = list.City;
            }

            if (this.EntityPM.Notify1CountryId != list.CountryId) {
                this.EntityPM.Notify1CountryId = list.CountryId;
            }

            if (this.EntityPM.Notify1StateId != list.StateId) {
                this.EntityPM.Notify1StateId = list.StateId;
            }

            if (this.EntityPM.Notify1ZipCode != list.ZipCode) {
                this.EntityPM.Notify1ZipCode = list.ZipCode;
            }

            if (this.EntityPM.Notify1FaxNumber != list.FaxNumber) {
                this.EntityPM.Notify1FaxNumber = list.FaxNumber;
            }

            if (this.EntityPM.Notify1PhoneNumber != list.PhoneNumber) {
                this.EntityPM.Notify1PhoneNumber = list.PhoneNumber;
            }
        }

        this.Validate_NTF();
        this.FireWizardEvent();
    }

    // IssuingCarrier
    private isPartnerChanged_Issuing: boolean;
    get IssuingCarrierAgentId() { return this.EntityPM.IssuingCarrierAgentId; }
    set IssuingCarrierAgentId(newValue: string) {
        if (this.EntityPM.IssuingCarrierAgentId != newValue) {
            this.EntityPM.IssuingCarrierAgentId = newValue;
            this.isPartnerChanged_Issuing = true;
            this.SetUIProperties_IssuingCarrier();
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

    get IssuingCarrierReference1() { return this.EntityPM.IssuingCarrierReference1; }
    set IssuingCarrierReference1(newValue: string) {
        if (this.EntityPM.IssuingCarrierReference1 != newValue) {
            this.EntityPM.IssuingCarrierReference1 = newValue;
            this.Validate_AGT();
            this.FireWizardEvent();
        }
    }

    get IssuingCarrierIATACode() { return this.EntityPM.IssuingCarrierIATACode; }
    set IssuingCarrierIATACode(newValue: string) {
        if (this.EntityPM.IssuingCarrierIATACode != newValue) {
            this.EntityPM.IssuingCarrierIATACode = newValue;
            this.Validate_AGT();
            this.FireWizardEvent();
        }
    }

    get CASSCode() { return this.EntityPM.CASSCode; }
    set CASSCode(newValue: string) {
        if (this.EntityPM.CASSCode != newValue) {
            this.EntityPM.CASSCode = newValue;
            this.Validate_AGT();
            this.FireWizardEvent();
        }
    }

    get ColoaderRANumber() { return this.EntityPM.ColoaderRANumber; }
    set ColoaderRANumber(newValue: string) {
        if (this.EntityPM.ColoaderRANumber != newValue) {
            this.EntityPM.ColoaderRANumber = newValue;
            this.RAFieldChanged();
        }
    }

    get RegulatedAgentRANumber() { return this.EntityPM.RegulatedAgentRANumber; }
    set RegulatedAgentRANumber(newValue: string) {
        if (this.EntityPM.RegulatedAgentRANumber != newValue) {
            this.EntityPM.RegulatedAgentRANumber = newValue;
            this.RAFieldChanged();
        }
    }

    private GetIssuingCarrierCard() {        

        if (this.IssuingCarrierAgentId == null) {
            this.IssuingCarrierAddressId = null;
            this.IssuingCarrierAddressList = null;
            this.EntityPM.IssuingCarrierAgentName = null;
            this.EntityPM.IssuingCarrierAgentNote = null;
            this.Validate_AGT();
            this.FireWizardEvent();
        }

        else {
            this.LoadIssuingCarrierCard();
        }
    }
    private LoadIssuingCarrierCard() {
        this.myCardListService.getSingle(this.IssuingCarrierAgentId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.IssuingCarrierAgentName = myCard.EnglishName;
                        this.EntityPM.IssuingCarrierAgentNote = myCard.Notes;

                        if (this.isSetDefaultTenantAgent) {
                            this.CASSCode = this.Wizard.TenantPM.CASSCode;
                            this.IssuingCarrierIATACode = this.Wizard.TenantPM.IATA;
                            this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;
                        }

                        else {
                            this.CASSCode = myCard.CASSCode;
                            this.IssuingCarrierIATACode = myCard.IATACode;
                            this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;
                        }

                        if (this.ViaColoader) {
                            this.EntityPM.ColoaderId = this.EntityPM.IssuingCarrierAgentId;
                            this.EntityPM.ColoaderName = this.EntityPM.IssuingCarrierAgentName;
                            this.EntityPM.ColoaderNote = this.EntityPM.IssuingCarrierAgentNote;
                            this.EntityPM.ColoaderContactId = myCard.PrimaryContactId;
                            this.ColoaderRANumber = myCard.RegulatedAgentCode;
                        }
                    }

                    if (this.isPartnerChanged_Issuing) {
                        this.GetIssuingCarrierMainAddress();
                    }

                    else {
                        this.GetIssuingCarrierAddress();
                    }

                    this.isPartnerChanged_Issuing = false;
                }
            }
        });
    }

    public IssuingCarrierAddressList: AddressList;
    private GetIssuingCarrierAddress() {
        if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAddressId)) {
            var myService: AddressListService = new AddressListService();
            
            myService.getSingle(this.IssuingCarrierAddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetIssuingCarrierAddress(myResponse.Result);
                    }
                }
            });
        }
    }
    private GetIssuingCarrierMainAddress() {
        if (!AppTool.IsNullOrEmpty(this.IssuingCarrierAgentId)) {
            var myService: AddressService = new AddressService();
            myService.GetMainAddressByCardId(this.IssuingCarrierAgentId, this.Wizard.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetIssuingCarrierAddress(myResponse.Result);
                    }
                }
            });
        }
    }
    private SetIssuingCarrierAddress(list: AddressList) {
        this.IssuingCarrierAddressList = list;

        if (list == null) {
            if (this.EntityPM.IssuingCarrierAddressId != null) {
                this.EntityPM.IssuingCarrierAddressId = null;
            }

            if (this.EntityPM.IssuingCarrierCity != null) {
                this.EntityPM.IssuingCarrierCity = null;
            }
        }

        else {
            if (this.EntityPM.IssuingCarrierAddressId != list.Id) {
                this.EntityPM.IssuingCarrierAddressId = list.Id;
            }

            if (this.EntityPM.IssuingCarrierCity != list.City) {
                this.EntityPM.IssuingCarrierCity = list.City;
            }
        }

        if (this.ViaColoader) {
            this.EntityPM.ColoaderAddressId = this.EntityPM.IssuingCarrierAddressId;
        }

        this.Validate_AGT();
        this.FireWizardEvent();
    }

    // ViaColoader
    get ViaColoader() { return this.EntityPM.ViaColoader; }
    set ViaColoader(newValue: boolean) {
        if (this.EntityPM.ViaColoader != newValue) {
            this.EntityPM.ViaColoader = newValue;
            this.Validate_AGT();
            this.FireWizardEvent();
            this.SetUIProperties_IssuingCarrier();

            if (newValue) {
                this.isSetDefaultTenantAgent = false;
                this.IssuingCarrierAgentId = null;
                this.IssuingCarrierAddressId = null;
                this.IssuingCarrierIATACode = null;
                this.CASSCode = null;
                this.EntityPM.CASSCode = null;
            }

            else {
                var confirmWindow = new ConfirmWindow();

                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.isSetDefaultTenantAgent = true;
                        this.IssuingCarrierAgentId = this.Wizard.TenantPM.AgentId;
                        this.ColoaderRANumber = null;
                        this.CASSCode = this.Wizard.TenantPM.CASSCode;
                        this.IssuingCarrierIATACode = this.Wizard.TenantPM.IATA;
                        this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;
                    }

                    else {
                        this.EntityPM.ViaColoader = true
                        this.SetUIProperties_IssuingCarrier();
                    }
                });

                confirmWindow.Show("Restore the default issuing carrier's agent ?");
            }
        }
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
                case "NTF": { myTitle += "Notify"; break; }
                case "AGT": { myTitle += "Issuing Carrier's Agent"; break; }
            }

            var windowArgs = new AddEditPartnerArgs();
            windowArgs.EntityPM = this.EntityPM;
            windowArgs.IsNewEntity = isNewPartner;
            windowArgs.PartnerTypeCode = myPartnerTypeCode;

            var logWindow = new LogitudeWindow();
            logWindow.Title = myTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');

            logWindow.ComponentLoaded.subscribe(cmp => {
                logWindow.WindowClosed.subscribe(($event: any) => {
                    this.isPartnerWindowOpened = false;

                    if (cmp.IsUpdatingPartner) {
                        this.UpdatePartner(cmp.PartnerTypeCode, cmp.CurrentPartnerId, cmp.CurrentAddressId);
                    }
                });
            });
        }
    }
    UpdatePartner(myPartnerTypeCode: string, myPartnerId: string, myAddressId: string) {
        if (!AppTool.IsNullOrEmpty(myPartnerId)) {

            this.myCardListService.getSingle(myPartnerId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var iCardList: CardList = myResponse.Result;

                        if (iCardList) {
                            switch (myPartnerTypeCode) {
                                case "SHI":
                                    {
                                        if (this.EntityPM.ShipperId != myPartnerId) {
                                            this.EntityPM.ShipperId = myPartnerId;
                                        }

                                        if (this.EntityPM.ShipperName != iCardList.EnglishName) {
                                            this.EntityPM.ShipperName = iCardList.EnglishName;
                                        }

                                        if (this.EntityPM.ShipperNote != iCardList.Notes) {
                                            this.EntityPM.ShipperNote = iCardList.Notes;
                                        }

                                        if (this.EntityPM.ShipperContactId != iCardList.PrimaryContactId) {
                                            this.EntityPM.ShipperContactId = iCardList.PrimaryContactId;
                                        }

                                        if (this.EntityPM.KnownConsignorNumber != iCardList.KnownConsignor) {
                                            this.EntityPM.KnownConsignorNumber = iCardList.KnownConsignor;
                                        }

                                        if (this.EntityPM.KCExpirationDate != iCardList.KCExpirationDate) {
                                            this.EntityPM.KCExpirationDate = iCardList.KCExpirationDate;
                                        }

                                        this.RAFieldChanged();

                                        if (iCardList.SalesmanUserId != null) {
                                            if (this.EntityPM.SalesmanUserId != iCardList.SalesmanUserId) {
                                                this.EntityPM.SalesmanUserId = iCardList.SalesmanUserId;
                                            }
                                        }

                                        if (iCardList.AccountManagerUserId != null) {
                                            if (this.EntityPM.AccountManagerUserId != iCardList.AccountManagerUserId) {
                                                this.EntityPM.AccountManagerUserId = iCardList.AccountManagerUserId;
                                            }
                                        }

                                        this.SetUIProperties_Shipper();

                                        break;
                                    }

                                case "CON":
                                    {
                                        if (this.EntityPM.ConsigneeId != myPartnerId) {
                                            this.EntityPM.ConsigneeId = myPartnerId;
                                        }

                                        if (this.EntityPM.ConsigneeName != iCardList.EnglishName) {
                                            this.EntityPM.ConsigneeName = iCardList.EnglishName;
                                        }

                                        if (this.EntityPM.ConsigneeNote != iCardList.Notes) {
                                            this.EntityPM.ConsigneeNote = iCardList.Notes;
                                        }

                                        if (this.EntityPM.ConsigneeContactId != iCardList.PrimaryContactId) {
                                            this.EntityPM.ConsigneeContactId = iCardList.PrimaryContactId;
                                        }

                                        this.SetUIProperties_Consignee();

                                        break;
                                    }

                                case "NTF":
                                    {
                                        if (this.EntityPM.Notify1Id != myPartnerId) {
                                            this.EntityPM.Notify1Id = myPartnerId;
                                        }

                                        if (this.EntityPM.Notify1Name != iCardList.EnglishName) {
                                            this.EntityPM.Notify1Name = iCardList.EnglishName;
                                        }

                                        if (this.EntityPM.Notify1Note != iCardList.Notes) {
                                            this.EntityPM.Notify1Note = iCardList.Notes;
                                        }

                                        if (this.EntityPM.Notify1ContactId != iCardList.PrimaryContactId) {
                                            this.EntityPM.Notify1ContactId = iCardList.PrimaryContactId;
                                        }

                                        this.SetUIProperties_Notify1();

                                        break;
                                    }

                                case "ISS":
                                    {
                                        if (this.EntityPM.IssuingCarrierAgentId != myPartnerId) {
                                            this.EntityPM.IssuingCarrierAgentId = myPartnerId;
                                        }

                                        if (this.EntityPM.IssuingCarrierAgentName != iCardList.EnglishName) {
                                            this.EntityPM.IssuingCarrierAgentName = iCardList.EnglishName;
                                        }

                                        if (this.EntityPM.IssuingCarrierAgentNote != iCardList.Notes) {
                                            this.EntityPM.IssuingCarrierAgentNote = iCardList.Notes;
                                        }

                                        if (this.isSetDefaultTenantAgent) {
                                            if (this.CASSCode != this.Wizard.TenantPM.CASSCode) {
                                                this.CASSCode = this.Wizard.TenantPM.CASSCode;
                                            }

                                            if (this.IssuingCarrierIATACode != this.Wizard.TenantPM.IATA) {
                                                this.IssuingCarrierIATACode = this.Wizard.TenantPM.IATA;
                                            }

                                            if (this.RegulatedAgentRANumber != this.Wizard.TenantPM.RegulatedAgentNumber) {
                                                this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;
                                            }
                                        }

                                        else {
                                            if (this.CASSCode != iCardList.CASSCode) {
                                                this.CASSCode = iCardList.CASSCode;
                                            }

                                            if (this.IssuingCarrierIATACode != iCardList.IATACode) {
                                                this.IssuingCarrierIATACode = iCardList.IATACode;
                                            }

                                            if (this.RegulatedAgentRANumber != this.Wizard.TenantPM.RegulatedAgentNumber) {
                                                this.RegulatedAgentRANumber = this.Wizard.TenantPM.RegulatedAgentNumber;
                                            }
                                        }

                                        if (this.ViaColoader) {
                                            if (this.EntityPM.ColoaderId != this.EntityPM.IssuingCarrierAgentId) {
                                                this.EntityPM.ColoaderId = this.EntityPM.IssuingCarrierAgentId;
                                            }

                                            if (this.EntityPM.ColoaderName != this.EntityPM.IssuingCarrierAgentName) {
                                                this.EntityPM.ColoaderName = this.EntityPM.IssuingCarrierAgentName;
                                            }

                                            if (this.EntityPM.ColoaderNote != this.EntityPM.IssuingCarrierAgentNote) {
                                                this.EntityPM.ColoaderNote = this.EntityPM.IssuingCarrierAgentNote;
                                            }

                                            if (this.EntityPM.ColoaderContactId != iCardList.PrimaryContactId) {
                                                this.EntityPM.ColoaderContactId = iCardList.PrimaryContactId;
                                            }

                                            if (this.ColoaderRANumber != iCardList.RegulatedAgentCode) {
                                                this.ColoaderRANumber = iCardList.RegulatedAgentCode;
                                            }
                                        }

                                        this.SetUIProperties_IssuingCarrier();

                                        break;
                                    }
                            }

                            var iAddressList: AddressList = null;
                            if (AppTool.IsNullOrEmpty(myAddressId)) {
                                switch (myPartnerTypeCode) {
                                    case "SHI": { this.SetShipperAddress(null); break; }
                                    case "CON": { this.SetConsigneeAddress(null); break; }
                                    case "NTF": { this.SetNotify1Address(null); break; }
                                    case "ISS": { this.SetIssuingCarrierAddress(null); break; }
                                }
                            }

                            else {
                                var myService: AddressListService = new AddressListService();

                                myService.getSingle(myAddressId).subscribe((myResponse2: ServiceResponse) => {
                                    if (myResponse2 != null) {
                                        if (!myResponse2.HasError) {
                                            var iAddressList: AddressList = myResponse2.Result;

                                            switch (myPartnerTypeCode) {
                                                case "SHI": { this.SetShipperAddress(iAddressList); break; }
                                                case "CON": { this.SetConsigneeAddress(iAddressList); break; }
                                                case "NTF": { this.SetNotify1Address(iAddressList); break; }
                                                case "ISS": { this.SetIssuingCarrierAddress(iAddressList); break; }
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            });
        }
    }

}
