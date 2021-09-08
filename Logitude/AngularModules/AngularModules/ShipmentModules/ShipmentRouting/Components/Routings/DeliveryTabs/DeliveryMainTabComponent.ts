import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {PortList} from '../../../../../Common/EntityLists/PortList';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../../Common/EntityLists/AddressList';
import {CountryList} from '../../../../../Common/EntityLists/CountryList';
import {PortListService} from '../../../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../../Common/Services/StandardLists/AddressListService';
import {CountryListService} from '../../../../../Common/Services/StandardLists/CountryListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {CitySelectionArgs} from '../../../../../Common/Args';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';

@Component({
    
    templateUrl: './DeliveryMainTabComponent.html',
})

export class DeliveryMainTabComponent extends BaseComponent {
    public EntityPM: ShipmentDeliveryPM;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    constructor() {
        super();
        this.InitServices();
    }

    private myPortListService: PortListService;
    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    private myCountryListService: CountryListService;
    InitServices() {
        this.myPortListService = new PortListService();
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myCountryListService = new CountryListService();
    }

    public TransportModeId: string;
    public FullResponsibilityHelp: string;
    public IsConnectedToContainer: boolean = false;
    InitTab(myEntityPM: ShipmentDeliveryPM, myShipmentPM: ShipmentPM) {
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        this.TransportModeId = this.ShipmentPM.TransportModeId;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.FullResponsibilityHelp = TextCodeTranslator.Translate("ShipmentPickUpDelivery.FullResponsibilityHelpText");

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

        }

        else {
            if (this.ShipmentPM.ShipmentPackages.filter(f => f.DeliveryId == this.EntityPM.Id).length > 0) {
                this.IsConnectedToContainer = true;
            }

            if (this.ShipmentPM.ShipmentPackages.filter(f => f.EmptyContainerReturnId == this.EntityPM.Id).length > 0) {
                this.IsConnectedToContainer = true;
            }
        }

        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = true;
    public IsEmptyContainerVisible: boolean = false;
    SetUIProperties() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
            this.IsEditingEnabled = false;
        }

        else {
            this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        }        

        this.UIProperties.SetEnabled("FromAddress", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ToAddress", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FullResponsibility", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CarrierNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Driver", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("TruckNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("TrailerNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("TransportModeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ATA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("EmptyDeliveryContainerPartnerId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("EmptyDeliveryDepotReference", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_From();
        this.SetUIProperties_To();
        this.SetUIProperties_EmptyContainer();
        this.SetUIProperties_ValidDatesFields();
    }
    SetUIProperties_From() {
        switch (this.FromTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("FromPartnerCardId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromPartnerCardId) ? true : false);
                this.UIProperties.SetEnabled("FromPartnerCardId", this.ObjectTableName, this.IsEditingEnabled);

                var isAddressIdEnabled = false;
                if (this.IsEditingEnabled) {
                    if (!AppTool.IsNullOrEmpty(this.FromPartnerCardId)) {
                        isAddressIdEnabled = true;
                    }
                }

                this.UIProperties.SetEnabled("FromAddressId", this.ObjectTableName, isAddressIdEnabled);
                break;
            }

            case "PORT": {
                this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromPortId) ? true : false);
                this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, this.IsEditingEnabled);
                break;
            }

            case "CASL": {
                this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromAddressCity) && AppTool.IsNullOrEmpty(this.FromAddressZipCode) ? true : false);
                this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromAddressCountryId) ? true : false);

                this.UIProperties.SetEnabled("FromAddressZipCode", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, this.IsEditingEnabled);
                break;
            }
        }
    }
    SetUIProperties_To() {
        switch (this.ToTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("ToPartnerCardId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPartnerCardId) ? true : false);
                this.UIProperties.SetEnabled("ToPartnerCardId", this.ObjectTableName, this.IsEditingEnabled);

                var isAddressIdEnabled = false;
                if (this.IsEditingEnabled) {
                    if (!AppTool.IsNullOrEmpty(this.ToPartnerCardId)) {
                        isAddressIdEnabled = true;
                    }
                }

                this.UIProperties.SetEnabled("ToAddressId", this.ObjectTableName, isAddressIdEnabled);
                break;
            }

            case "PORT": {
                this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
                this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, this.IsEditingEnabled);
                break;
            }

            case "CASL": {
                this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToAddressCity) && AppTool.IsNullOrEmpty(this.ToAddressZipCode) ? true : false);
                this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToAddressCountryId) ? true : false);

                this.UIProperties.SetEnabled("ToAddressZipCode", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, this.IsEditingEnabled);
                break;
            }
        }
    }
    SetUIProperties_EmptyContainer() {

        var isEmptyContainerVisible = false;

        if (this.IsFCLEntity) {
            if (this.ShipmentPM.DirectionId == "I") {
                isEmptyContainerVisible = true;
            }
        }

        this.IsEmptyContainerVisible = isEmptyContainerVisible;
    }
    SetUIProperties_ValidDatesFields() {

        this.UIProperties.SetValidity("ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("ATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.ATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATD"));
            this.UIProperties.SetValidity("ATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.ATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATA"));
            this.UIProperties.SetValidity("ATA", this.ObjectTableName, false, errorMessage);
        }
    }

    get IsFromRequired() {
        var myResult: boolean = false;

        if (this.FullResponsibility) {
            switch (this.FromTypeCode) {
                case "PART": {
                    if (AppTool.IsNullOrEmpty(this.FromPartnerCardId)) {
                        myResult = true;
                    }
                    break;
                }

                case "PORT": {
                    if (AppTool.IsNullOrEmpty(this.FromPortId)) {
                        myResult = true;
                    }
                    break;
                }

                case "CASL": {
                    if (AppTool.IsNullOrEmpty(this.FromAddressCity) || AppTool.IsNullOrEmpty(this.FromAddressCountryId)) {
                        myResult = true;
                    }
                    break;
                }
            }
        }

        return myResult;
    }

    get IsToRequired() {
        var myResult: boolean = false;

        if (this.FullResponsibility) {
            switch (this.ToTypeCode) {
                case "PART": {
                    if (AppTool.IsNullOrEmpty(this.ToPartnerCardId)) {
                        myResult = true;
                    }
                    break;
                }

                case "PORT": {
                    if (AppTool.IsNullOrEmpty(this.ToPortId)) {
                        myResult = true;
                    }
                    break;
                }

                case "CASL": {
                    if (AppTool.IsNullOrEmpty(this.ToAddressCity) || AppTool.IsNullOrEmpty(this.ToAddressCountryId)) {
                        myResult = true;
                    }
                    break;
                }
            }
        }

        return myResult;
    }

    // On Open Edit Mood
    OnEditMoodScreen() {
        if (!AppTool.IsNullOrEmpty(this.FromAddressId)) {
            this.myAddressListService.getSingle(this.FromAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.FromAddressList = myResponse.Result;
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(this.ToAddressId)) {
            this.myAddressListService.getSingle(this.ToAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ToAddressList = myResponse.Result;
                }
            });
        }
    }

    get FullResponsibility() { return this.EntityPM.FullResponsibility; }
    set FullResponsibility(value: boolean) {
        if (this.EntityPM.FullResponsibility != value) {
            this.EntityPM.FullResponsibility = value;
            this.SetUIProperties_From();
        }
    }

    // From
    get FromTypeCode() { return this.EntityPM.PickUpDeliveryFromTypeCode; }
    set FromTypeCode(value: string) {
        if (this.EntityPM.PickUpDeliveryFromTypeCode != value) {
            this.EntityPM.PickUpDeliveryFromTypeCode = value;

            this.FromPartnerCardId = null;
            this.FromAddressId = null;
            this.FromPortId = null;
            this.FromAddressCity = null;
            this.FromAddressZipCode = null;
            this.FromAddressCountryId = null;
            this.FromAddress = null;
            this.fromAddressList = null;
            this.SetUIProperties_From();
        }
    }

    get FromPartnerCardId() { return this.EntityPM.FromPartnerCardId; }
    set FromPartnerCardId(value: string) {
        if (this.EntityPM.FromPartnerCardId != value) {
            this.EntityPM.FromPartnerCardId = value;
            this.SetUIProperties_From();

            if (AppTool.IsNullOrEmpty(value)) {
                this.FromAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            if (!AppTool.IsNullOrEmpty(list.PickAddressId)) {
                                this.FromAddressId = list.PickAddressId;
                            }

                            else {
                                this.FromAddressId = list.MainAddressId;
                            }
                        }
                    }
                });
            }
        }
    }

    get FromAddressId() { return this.EntityPM.FromAddressId; }
    set FromAddressId(value: string) {
        if (this.EntityPM.FromAddressId != value) {
            this.EntityPM.FromAddressId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.FromAddressList = null;
                this.EntityPM.FromAddressCity_Dummy = null;
                this.EntityPM.FromAddressCountryCode = null;
                this.EntityPM.FromAddressCountryName = null;
            }

            else {
                this.myAddressListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: AddressList = myResponse.Result;
                        if (list) {
                            this.FromAddressList = list;
                            this.EntityPM.FromAddressCity_Dummy = list.City;
                            this.EntityPM.FromAddressCountryCode = list.CountryCode;
                            this.EntityPM.FromAddressCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(value: string) {
        if (this.EntityPM.FromPortId != value) {
            this.EntityPM.FromPortId = value;
            this.SetUIProperties_From();

            if (AppTool.IsNullOrEmpty(value)) {
                this.FromAddress = null;
                this.EntityPM.FromPortCode = null;
                this.EntityPM.FromPortName = null;
                this.EntityPM.FromPortCountryCode = null;
                this.EntityPM.FromPortCountryName = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.FromAddress = "Port Of: " + list.EnglishName;
                            this.EntityPM.FromPortCode = list.Code;
                            this.EntityPM.FromPortName = list.EnglishName;
                            this.EntityPM.FromPortCountryCode = list.CountryCode;
                            this.EntityPM.FromPortCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    get FromAddressCity() { return this.EntityPM.FromAddressCity; }
    set FromAddressCity(value: string) {
        if (this.EntityPM.FromAddressCity != value) {
            this.EntityPM.FromAddressCity = value;
            this.EntityPM.FromAddressCity_Dummy = value;
            this.SetUIProperties_From();
        }
    }

    get FromAddressZipCode() { return this.EntityPM.FromAddressZipCode; }
    set FromAddressZipCode(value: string) {
        if (this.EntityPM.FromAddressZipCode != value) {
            this.EntityPM.FromAddressZipCode = value;
            this.SetUIProperties_From();
        }
    }

    get FromAddressCountryId() { return this.EntityPM.FromAddressCountryId; }
    set FromAddressCountryId(value: string) {
        if (this.EntityPM.FromAddressCountryId != value) {
            this.EntityPM.FromAddressCountryId = value;
            this.SetUIProperties_From();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.FromAddressCountryCode = null;
                this.EntityPM.FromAddressCountryName = null;
            }

            else {
                this.myCountryListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.FromAddressCountryCode = list.Code;
                            this.EntityPM.FromAddressCountryName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get FromAddress() { return this.EntityPM.FromAddress; }
    set FromAddress(value: string) {
        if (this.EntityPM.FromAddress != value) {
            this.EntityPM.FromAddress = value;
        }
    }

    private fromAddressList: AddressList;
    get FromAddressList() { return this.fromAddressList; }
    set FromAddressList(newValue: AddressList) {
        this.fromAddressList = newValue;
    }

    // To
    get ToTypeCode() { return this.EntityPM.PickUpDeliveryToTypeCode; }
    set ToTypeCode(value: string) {
        if (this.EntityPM.PickUpDeliveryToTypeCode != value) {
            this.EntityPM.PickUpDeliveryToTypeCode = value;

            this.EntityPM.ToPartnerCardId = null;
            this.EntityPM.ToAddressId = null;
            this.EntityPM.ToPortId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.EntityPM.ToAddress = null;
            this.ToAddressList = null;
            this.SetUIProperties_To();
        }
    }

    get ToPartnerCardId() { return this.EntityPM.ToPartnerCardId; }
    set ToPartnerCardId(value: string) {
        if (this.EntityPM.ToPartnerCardId != value) {
            this.EntityPM.ToPartnerCardId = value;
            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            if (!AppTool.IsNullOrEmpty(list.PickAddressId)) {
                                this.ToAddressId = list.PickAddressId;
                            }

                            else {
                                this.ToAddressId = list.MainAddressId;
                            }
                        }
                    }
                });
            }
        }
    }

    get ToAddressId() { return this.EntityPM.ToAddressId; }
    set ToAddressId(value: string) {
        if (this.EntityPM.ToAddressId != value) {
            this.EntityPM.ToAddressId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddressList = null;
                this.EntityPM.ToAddressCity_Dummy = null;
                this.EntityPM.ToAddressCountryCode = null;
                this.EntityPM.ToAddressCountryName = null;
            }

            else {
                this.myAddressListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: AddressList = myResponse.Result;
                        if (list) {
                            this.ToAddressList = list;
                            this.EntityPM.ToAddressCity_Dummy = list.City;
                            this.EntityPM.ToAddressCountryCode = list.CountryCode;
                            this.EntityPM.ToAddressCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.EntityPM.ToPortId != value) {
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddress = null;
                this.EntityPM.ToPortCode = null;
                this.EntityPM.ToPortName = null;
                this.EntityPM.ToPortCountryCode = null;
                this.EntityPM.ToPortCountryName = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.ToAddress = "Port Of: " + list.EnglishName;
                            this.EntityPM.ToPortCode = list.Code;
                            this.EntityPM.ToPortName = list.EnglishName;
                            this.EntityPM.ToPortCountryCode = list.CountryCode;
                            this.EntityPM.ToPortCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    get ToAddressCity() { return this.EntityPM.ToAddressCity; }
    set ToAddressCity(value: string) {
        if (this.EntityPM.ToAddressCity != value) {
            this.EntityPM.ToAddressCity = value;
            this.EntityPM.ToAddressCity_Dummy = value;
            this.SetUIProperties_To();
        }
    }

    get ToAddressZipCode() { return this.EntityPM.ToAddressZipCode; }
    set ToAddressZipCode(value: string) {
        if (this.EntityPM.ToAddressZipCode != value) {
            this.EntityPM.ToAddressZipCode = value;
            this.SetUIProperties_To();
        }
    }

    get ToAddressCountryId() { return this.EntityPM.ToAddressCountryId; }
    set ToAddressCountryId(value: string) {
        if (this.EntityPM.ToAddressCountryId != value) {
            this.EntityPM.ToAddressCountryId = value;
            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.ToAddressCountryCode = null;
                this.EntityPM.ToAddressCountryName = null;
            }

            else {
                this.myCountryListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.ToAddressCountryCode = list.Code;
                            this.EntityPM.ToAddressCountryName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get ToAddress() { return this.EntityPM.ToAddress; }
    set ToAddress(value: string) {
        if (this.EntityPM.ToAddress != value) {
            this.EntityPM.ToAddress = value;
        }
    }

    private toAddressList: AddressList;
    get ToAddressList() { return this.toAddressList; }
    set ToAddressList(newValue: AddressList) {
        this.toAddressList = newValue;
    }

    // Select City
    SelectCityCommand(myAddressCode: string) {

        var mySourceCountryId: string = myAddressCode == "F" ? this.FromAddressCountryId : this.ToAddressCountryId;

        var args = new CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                if (myAddressCode == "F") {
                    this.FromAddressCity = args.CityName;
                    this.FromAddressCountryId = args.CountryId;
                }

                else {
                    this.ToAddressCity = args.CityName;
                    this.ToAddressCountryId = args.CountryId;
                }
            }
        });
    }

    // Trucker
    get CarrierId() { return this.EntityPM.CarrierId; }
    set CarrierId(value: string) {
        if (this.EntityPM.CarrierId != value) {
            this.EntityPM.CarrierId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.CarrierCode = null;
                this.CarrierName = null;
                this.CarrierWebSite = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.CarrierCode = list.Code;
                            this.CarrierName = list.EnglishName;
                            this.CarrierWebSite = list.WebSite;
                        }
                    }
                });
            }
        }
    }

    get CarrierCode() { return this.EntityPM.CarrierCode; }
    set CarrierCode(value: string) {
        if (this.EntityPM.CarrierCode != value) {
            this.EntityPM.CarrierCode = value;
        }
    }

    get CarrierName() { return this.EntityPM.CarrierName; }
    set CarrierName(value: string) {
        if (this.EntityPM.CarrierName != value) {
            this.EntityPM.CarrierName = value;
        }
    }

    get CarrierWebSite() { return this.EntityPM.CarrierWebSite; }
    set CarrierWebSite(value: string) {
        if (this.EntityPM.CarrierWebSite != value) {
            this.EntityPM.CarrierWebSite = value;
        }
    }

    get CarrierNumber() { return this.EntityPM.CarrierNumber; }
    set CarrierNumber(value: string) {
        if (this.EntityPM.CarrierNumber != value) {
            this.EntityPM.CarrierNumber = value;
        }
    }

    get Driver() { return this.EntityPM.Driver; }
    set Driver(value: string) {
        if (this.EntityPM.Driver != value) {
            this.EntityPM.Driver = value;
        }
    }

    get TruckNumber() { return this.EntityPM.TruckNumber; }
    set TruckNumber(value: string) {
        if (this.EntityPM.TruckNumber != value) {
            this.EntityPM.TruckNumber = value;
        }
    }

    get TrailerNumber() { return this.EntityPM.TrailerNumber; }
    set TrailerNumber(value: string) {
        if (this.EntityPM.TrailerNumber != value) {
            this.EntityPM.TrailerNumber = value;
        }
    }

    get TransportModeCode() { return this.EntityPM.TransportModeCode; }
    set TransportModeCode(value: string) {
        if (this.EntityPM.TransportModeCode != value) {
            this.EntityPM.TransportModeCode = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }
    private ConvertStringToDate(dateValue: any) {
        return DateTool.GetDateParts(dateValue).DateObject;
    }

    get ETD() { return this.EntityPM.ETD; }
    set ETD(value: Date) {
        if (this.EntityPM.ETD != value) {
            if (this.EntityPM.ETD != null && !(this.EntityPM.ETD instanceof Date)) {
                this.EntityPM.ETD = this.ConvertStringToDate(this.EntityPM.ETD);
            }
            this.EntityPM.ETD = value;
        }
    }

    get ETA() { return this.EntityPM.ETA; }
    set ETA(value: Date) {
        if (this.EntityPM.ETA != value) {
            this.EntityPM.ETA = value;
        }
    }

    get ATD() { return this.EntityPM.ATD; }
    set ATD(value: Date) {
        if (this.EntityPM.ATD != value) {
            if (this.EntityPM.ATD != null && !(this.EntityPM.ATD instanceof Date)) {
                this.EntityPM.ATD = this.ConvertStringToDate(this.EntityPM.ATD);
            }
            this.EntityPM.ATD = value;
            this.SetUIProperties_ValidDatesFields();
        }
    }

    get ATA() { return this.EntityPM.ATA; }
    set ATA(value: Date) {
        if (this.EntityPM.ATA != value) {
            this.EntityPM.ATA = value;
            this.SetUIProperties_ValidDatesFields();
        }
    }

    get EmptyDeliveryContainerPartnerId() { return this.EntityPM.EmptyDeliveryContainerPartnerId; }
    set EmptyDeliveryContainerPartnerId(value: string) {
        if (this.EntityPM.EmptyDeliveryContainerPartnerId != value) {
            this.EntityPM.EmptyDeliveryContainerPartnerId = value;
        }
    }

    get EmptyDeliveryDepotReference() { return this.EntityPM.EmptyDeliveryDepotReference; }
    set EmptyDeliveryDepotReference(value: string) {
        if (this.EntityPM.EmptyDeliveryDepotReference != value) {
            this.EntityPM.EmptyDeliveryDepotReference = value;
        }
    }

    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "ETD": { this.ATD = DateTool.GetDateParts(this.ETD).DateObject; break; }
            case "ETA": { this.ATA = DateTool.GetDateParts(this.ETA).DateObject; break; }
        }
    }
}
