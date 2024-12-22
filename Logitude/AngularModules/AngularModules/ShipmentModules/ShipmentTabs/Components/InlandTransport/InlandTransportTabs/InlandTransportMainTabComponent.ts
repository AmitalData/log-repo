import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../../Common/EntityLists/AddressList';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {AddressListService} from '../../../../../Common/Services/StandardLists/AddressListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {CitySelectionArgs} from '../../../../../Common/Args';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './InlandTransportMainTabComponent.html',
})

export class InlandTransportMainTabComponent extends BaseComponent {
    public EntityPM: ShipmentDeliveryPM;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public TabTitle: string;

    constructor() {
        super();
        this.InitServices();
        this.RunComponent();
    }


    RunComponent() {
        this.RunComponentTimer();
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

    private myCardListService: CardListService;
    private myAddressListService: AddressListService;
    InitServices() {
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
    }

    InitTab(myEntityPM: ShipmentDeliveryPM, myShipmentPM: ShipmentPM) {
        this.EntityPM = myEntityPM;
        this.EntityPM.EntityParentPM = myShipmentPM;
        this.ShipmentPM = myShipmentPM;

        if (this.EntityPM.PickUpDeliveryNumber) {
            this.TabTitle = this.EntityPM.PickUpDeliveryNumber + "_" + this.ShipmentPM.CustomerName + " :" + TextCodeTranslator.Translate("ShipmentPickUpDelivery.O.ShipmentCertificateNumber");
        }
    }

    get ResponsibilityCode() { return this.EntityPM.ResponsibilityCode; }
    set ResponsibilityCode(value: string) {
        if (this.EntityPM.ResponsibilityCode != value) {
            this.EntityPM.ResponsibilityCode = value;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(value: number) {
        if (this.EntityPM.Quantity != value) {
            this.EntityPM.Quantity = value;
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(value: number) {
        if (this.EntityPM.GrossWeight != value) {
            this.EntityPM.GrossWeight = value;
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = value;
        }
    }

    get Commodity() { return this.EntityPM.Commodity; }
    set Commodity(value: string) {
        if (this.EntityPM.Commodity != value) {
            this.EntityPM.Commodity = value;
        }
    }
    get TruckerChargeableWeight() { return this.EntityPM.TruckerChargeableWeight; }
    set TruckerChargeableWeight(value: number) {
        if (this.EntityPM.TruckerChargeableWeight != value) {
            this.EntityPM.TruckerChargeableWeight = value;
        }
    }
    get CustomerChargeableWeight() { return this.EntityPM.CustomerChargeableWeight; }
    set CustomerChargeableWeight(value: number) {
        if (this.EntityPM.CustomerChargeableWeight != value) {
            this.EntityPM.CustomerChargeableWeight = value;
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(value: string) {
        if (this.EntityPM.DescriptionOfGoods != value) {
            this.EntityPM.DescriptionOfGoods = value;
        }
    }

    get PackageTypeCode() { return this.EntityPM.PackageTypeCode; }
    set PackageTypeCode(value: string) {
        if (this.EntityPM.PackageTypeCode != value) {
            this.EntityPM.PackageTypeCode = value;
        }
    }

    get FromAddressId() { return this.EntityPM.FromAddressId; }
    set FromAddressId(value: string) {
        if (this.EntityPM.FromAddressId != value) {
            this.EntityPM.FromAddressId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.FromAddressCountryCode = null;
                this.EntityPM.FromAddressCountryName = null;
            }

            else {
                this.myAddressListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: AddressList = myResponse.Result;
                        if (list) {
                            this.EntityPM.FromAddressCountryCode = list.CountryCode;
                            this.EntityPM.FromAddressCountryName = list.CountryName;
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
        }
    }

    get FromAddressCityId() { return this.EntityPM.FromAddressCityId; }
    set FromAddressCityId(value: string) {
        if (this.EntityPM.FromAddressCityId != value) {
            this.EntityPM.FromAddressCityId = value;
        }
    }

    get ToAddressId() { return this.EntityPM.ToAddressId; }
    set ToAddressId(value: string) {
        if (this.EntityPM.ToAddressId != value) {
            this.EntityPM.ToAddressId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.ToAddress = null;
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressCityId = null;
            }
            else {
                this.myAddressListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: AddressList = myResponse.Result;
                        if (list) {
                            this.ToAddressCity = list.City;
                            this.ToAddressCityId = list.CityId;
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
        }
    }

    get ToAddressCityId() { return this.EntityPM.ToAddressCityId; }
    set ToAddressCityId(value: string) {
        if (this.EntityPM.ToAddressCityId != value) {
            this.EntityPM.ToAddressCityId = value;
        }
    }

    get DeliveryContact() { return this.EntityPM.DeliveryContact; }
    set DeliveryContact(value: string) {
        if (this.EntityPM.DeliveryContact != value) {
            this.EntityPM.DeliveryContact = value;
        }
    }

    get ToAddress() { return this.EntityPM.ToAddress; }
    set ToAddress(value: string) {
        if (this.EntityPM.ToAddress != value) {
            this.EntityPM.ToAddress = value;
        }
    }

    // Select City
    SelectCityCommand(myAddressCode: "from"|"to") {

        var args = new CitySelectionArgs(null);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                if (myAddressCode == "from") {
                    this.FromAddressCityId = args.CityId;
                    this.FromAddressCity = args.CityLocalName;
                }

                else {
                    this.ToAddressCityId = args.CityId;
                    this.ToAddressCity = args.CityLocalName;
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
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.CarrierCode = list.Code;
                            this.CarrierName = list.EnglishName;
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

    get CarrierNumber() { return this.EntityPM.CarrierNumber; }
    set CarrierNumber(value: string) {
        if (this.EntityPM.CarrierNumber != value) {
            this.EntityPM.CarrierNumber = value;
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

    OnEditMoodScreen() {
    }
}
