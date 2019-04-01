

declare var window: any;
import { Component, OnInit, ViewChildren, QueryList} from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ShipmentPM } from '../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPickUpPM } from '../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { ShipmentDeliveryPM } from '../../../Shipment/EntityPMs/ShipmentDeliveryPM';

import {AppTool, DateTool, FormatTool} from '../../../Infrastructure/Tools';
import { ShipmentTool } from '../../../Shipment/Tools';
import {AirlineListService} from '../../../Common/Services/StandardLists/AirlineListService';
import { AgentSharedManifestList } from '../../../Common/EntityLists/AgentSharedManifestList';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { SharedAgentManifestService } from '../../../Shipment/Services/Others/SharedAgentManifestService';
import { ShipmentPMService } from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../Common/EntityLists/CardList';
import {AddressList} from '../../../Common/EntityLists/AddressList';  
import {AddressListService} from '../../../Common/Services/StandardLists/AddressListService';
import {IncotermListService} from '../../../Common/Services/StandardLists/IncotermListService';
import {MoveTypeListService} from '../../../Infrastructure/Services/StandardLists/MoveTypeListService';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';

import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import { SharedManifestTranslationPM } from '../../../Common/EntityPMs/SharedManifestTranslationPM';
import {IncotermList} from '../../../Common/EntityLists/IncotermList';
import {AirlineList} from '../../../Common/EntityLists/AirlineList';
import {MoveTypeList} from '../../../Infrastructure/EntityLists/MoveTypeList';
import {CurrencyList} from '../../../Common/EntityLists/CurrencyList';

import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {PortListService} from '../../../Common/Services/StandardLists/PortListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {PortList} from '../../../Common/EntityLists/PortList';
import {CountryListService} from '../../../Common/Services/StandardLists/CountryListService';
import { ShipmentPackagePM } from '../../../Shipment/EntityPMs/ShipmentPackagePM';
import {PackageTypeListService} from '../../../Common/Services/StandardLists/PackageTypeListService';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {PackageTypeList} from '../../../Common/EntityLists/PackageTypeList';
import {PackageTypePM} from '../../../Common/EntityPMs/PackageTypePM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {PartnerSL} from '../../../Common/DataContracts/PartnerSL';
import {HouseSL} from '../../../Common/DataContracts/HouseSL';
import { AgentSharedManifestPMService } from '../../../Common/Services/StandardPMs/AgentSharedManifestPMService';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';
import { ShipmentValidator } from '../../../Shipment/Validators/ShipmentValidator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';
import {AddressService} from '../../../Common/Services/ExtendedLists/AddressService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {VesselListService} from '../../../Common/Services/StandardLists/VesselListService';
import {VesselList} from '../../../Common/EntityLists/VesselList';
import {ImportEntityArgs} from '../../../Common/Components/Maintenance/TenantImportComponent';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import { ShipmentPickUpDeliverySL } from '../../../Common/DataContracts/ShipmentPickUpDeliverySL';
import {CitySelectionArgs} from '../../../Common/Args';

@Component({
    moduleId: module.id,
    selector: 'SharedManifestAdditionalComponent',
    templateUrl: './SharedManifestAdditionalComponent.html',
    providers: [SharedAgentManifestService, AgentSharedManifestPMService,EntityPMService],
})

export class SharedManifestAdditionalComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Shipment";
    public DataContext = this;
    private myAddressListService: AddressListService;
    private myCardListService: CardListService;
    private myIncotermListService: IncotermListService;
    private myAirlineListService: AirlineListService;
    private myPartnersDomainService: PartnersDomainService;
    private myPortListService: PortListService;
    private countryListService: CountryListService;
    private myVesselListService: VesselListService;
    private myMoveTypeListService: MoveTypeListService;
    private myCurrencyListService: CurrencyListService;
    IsRemovePackageAreaFromScreen: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ConsigneeDefaultValues: string;
    Notify1DefaultValues: string;
    ShiperDefaultValues: string;
    public AllPackageTypes: PackageTypeList[] = [];
    AgentSharedManifestList: any;
 
    IsNoNoOtherPartnersFound: boolean = false;
    HideGeneralDetailsArea: boolean = false;
    HideOtherPartnersArea: boolean = false;
    HidePackageTypeArea: boolean = false;
    HideAgentSideDataArea: boolean = false;
    HideOurSideDataArea: boolean = false;


    HideTransShipmentsDetailsArea: boolean = false;


    IsNoTransShipmentsDetailsFound: boolean = false;

    IsShowTransShipmentsDetails:boolean = false;


    IsNoGeneralDetailsFound: boolean = false;


    AgentSideData: AgentSide;
    CurrentEntity: AgentSharedManifestPM;
    ManifestSL: ManifestSL;
    public CarrierDependencyProperty1: string;
    IsNoAgentDataFound: boolean = false;
    HeaderDirectionId: string = "";
    HeaderShipperName: string = "";
    HeaderRoute: string = "";
    HeaderLongMaster: string = "";
    HeaderMaster: string = "";
    HeaderAgentRef: string = "";
    HeaderTransportModeId: string = "";
    IsNoPackagesFound: boolean = false;
    public EntityPM: ShipmentPM = new ShipmentPM();
    IsShowHouseListArea: boolean;
    IsEnableCreateHouse: boolean;
    private myShipmentPMService: ShipmentPMService;
    IsConsolShipment: boolean = false;
    IsHouseShipment: boolean = false;
    IsHideDescriptionOfGoods: boolean = false;
    IsHideIsDangerous: boolean = false;
    IsHideValueOfGoods: boolean = false;
    IsHideIncoterm: boolean = false;
    IsHideCarrier: boolean = false;
    IsHideTransshipment1Carrier: boolean = false;
    IsHideTransshipment2Carrier: boolean = false;
    IsHideTransshipment3Carrier: boolean = false;
    IsHideMoveType: boolean = false;
    IsHideValueOfGoodsCurrency: boolean = false;
    IsHideMainHarmonize: boolean;
    
    IsHideMainCarriageVessel: boolean = false;
    IsHideTransshipment1Vessel: boolean = false;
    IsHideTransshipment2Vessel: boolean = false;
    IsHideTransshipment3Vessel: boolean= false;


    IsHideMainInterline: boolean= false;

    public CardDependencyProperty1: string = "CS";
    CardDependencyProperty1IsList: boolean= false;
    LableCreateButton: string = "Create";
   

    MoveTypeCode: string;
    ValueOfGoodsCurrencyCode: string;

    private ConsigneeCode: string;
    private ShipperCode: string;
    private  Notify1Code: string;
    private IncotermCode: string;
    private MainCarriageCarrierCode;
    private Transshipment1CarrierCode;
    private Transshipment2CarrierCode;
    private Transshipment3CarrierCode;

    private Transshipment1VesselCode: string;
    private Transshipment2VesselCode: string;
    private Transshipment3VesselCode: string;
    IsNoAddtionalFound: boolean = false;



    HouseEntity: HouseSL;
    public ValidationErrorsList: string[] = [];
    public FromPortList: PortList = null;
    public ToPortList: PortList = null;

    IsLoadedShipperTranslation: boolean = false;
    IsLoadedConsigneeTranslation: boolean = false;
    IsLoadedShiperDefaultValues: boolean = false;
    IsLoadedConsigneeDefaultValues: boolean = false;
    IsLoadedIncotermTranslation: boolean = false;
    IsLoadedMoveTypeTranslation: boolean = false;
    IsLoadedValueOfGoodsCurrencyTranslation:boolean = false;
    IsLoadedCarrierTranslation: boolean = false;
    IsLoadedTransshipment1CarrierTranslation: boolean = false;
    IsLoadedTransshipment2CarrierTranslation: boolean = false;
    IsLoadedTransshipment3CarrierTranslation: boolean = false;

    IsLoadedMainCarriageVesselTranslation: boolean = false;
    IsLoadedTransshipment1VesselTranslation: boolean = false;
    IsLoadedTransshipment2VesselTranslation: boolean = false;
    IsLoadedTransshipment3VesselTranslation: boolean = false;

    IsLoadedMainCarriageInterlineTranslation: boolean = false;
    IsLoadedNotify1IdTranslation: boolean = false;
    IsLoadedNotify1DefaultValuesTranslation: boolean = false;

    IsLoadedPortsTranslation: boolean = false;
    IsLoadedPackageTranslation: boolean = false;


    HidePickupDetailsArea: boolean = false;
    HideDeliveryDetailsArea: boolean = false;
    
    IsNoPickupDetailsFound: boolean = false;
    IsNoDeliveryDetailsFound: boolean = false;
    myPackageTypeService: PackageTypeListService;
    IsLoadedToPickUpTranslation: boolean = false;
    IsLoadedFromPickUpTranslation: boolean = false;


    IsLoadedFromDeliveryTranslation: boolean = false;
    IsLoadedToDeliveryTranslation: boolean = false;

    OurSideShipmentPickUp: ShipmentPickUpDeliverySL;
    OurSideShipmentDelivery: ShipmentPickUpDeliverySL;











    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedAgentManifestService: SharedAgentManifestService, public _agentSharedManifestPMService: AgentSharedManifestPMService, public entityPMService:EntityPMService) {
        super();
        this.myPackageTypeService = new PackageTypeListService();
        this.myShipmentPMService = new ShipmentPMService();
        this.myCardListService = new CardListService();
        this.myAddressListService = new AddressListService();
        this.myAirlineListService = new AirlineListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.myIncotermListService = new IncotermListService();
        this.myPortListService = new PortListService();
        this.countryListService = new CountryListService();
        this.myVesselListService = new VesselListService();
        this.myMoveTypeListService = new MoveTypeListService();
        this.myCurrencyListService = new CurrencyListService();
        
    }

    ngOnInit() {

    }


    //  Properites

    private myShipperSalesmanId: string;
    private myConsigneeSalesmanId: string;
    SetSalesman() {
        this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.myShipperSalesmanId) ? this.EntityPM.CreatedByUserId : this.myShipperSalesmanId;
    }


    private shiperAddressList: AddressList = new AddressList();
    get ShipperAddressList() { return this.shiperAddressList; }
    set ShipperAddressList(newValue: AddressList) {
        this.shiperAddressList = newValue;

    }


    get ShipperAddressId() { return this.EntityPM.ShipperAddressId; }
    set ShipperAddressId(newValue: string) {
        if (this.EntityPM.ShipperAddressId != newValue) {
            this.EntityPM.ShipperAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        if (myResponse.Result) {
                            this.ShipperAddressList = myResponse.Result;
                        }
                    }
                });
            }
        }
    }

    get ShipperContactId() { return this.EntityPM.ShipperContactId; }
    set ShipperContactId(newValue: string) {
        if (this.EntityPM.ShipperContactId != newValue) {
            this.EntityPM.ShipperContactId = newValue;
        }
    }

    private ShipperPartnerTypeId: string;
    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperPartnerTypeId = null;
                this.ShipperContactId = null;
                this.ShipperCode = null;
                this.EntityPM.ShipperName = null;
                this.EntityPM.ShipperNote = null;
                this.EntityPM.ShipperReference1 = null;
                this.EntityPM.ShipperReference2 = null;
                this.EntityPM.ShipperMainAddressId = null;
                this.EntityPM.ShipperPickAddressId = null;
                this.EntityPM.KnownConsignorNumber = null;
                this.EntityPM.KCExpirationDate = null;
                this.ShipperAddressId = null;
                this.myShipperSalesmanId = null;
                this.SetSalesman();
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {

                            this.ShipperCode = myCardList.Code;
                            this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                            this.ShipperContactId = myCardList.PrimaryContactId;
                            this.EntityPM.ShipperName = myCardList.EnglishName;
                            this.EntityPM.ShipperNote = myCardList.Notes;
                            this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                            this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                            this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                            this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                            this.ShipperAddressId = myCardList.MainAddressId;
                            this.myShipperSalesmanId = myCardList.SalesmanUserId;

                            if (!this.IsConsolShipment && this.HouseEntity && this.HouseEntity.Shipper) {

                                if (this.HouseEntity.Shipper.Code == this.ShipperCode) {
                                    this.EntityPM.ShipperReference1 = this.HouseEntity.ShipperReference1;
                                    this.EntityPM.ShipperReference2 = this.HouseEntity.ShipperReference2;
                                }

                            } else if (this.ManifestSL.Shipper && this.ManifestSL.Shipper.Code == this.ShipperCode) {
                                this.EntityPM.ShipperReference1 = this.ManifestSL.ShipperReference1;
                                this.EntityPM.ShipperReference2 = this.ManifestSL.ShipperReference2;
                            }


                            this.SetSalesman();
                        }
                    }
                });
            }
        }
    }




    // Agent
    get AgentId() { return this.EntityPM.AgentId; }
    set AgentId(newValue: string) {
        if (this.EntityPM.AgentId != newValue) {
            this.EntityPM.AgentId = newValue;
        

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.AgentContactId = null;
                this.EntityPM.AgentName = null;
                this.EntityPM.AgentNote = null;
                this.EntityPM.AgentReference1 = null;
                this.EntityPM.AgentReference2 = null;
                this.AgentAddressId = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.AgentContactId = myCardList.PrimaryContactId;
                            this.EntityPM.AgentName = myCardList.EnglishName;
                            this.EntityPM.AgentNote = myCardList.Notes;
                            this.AgentAddressId = myCardList.MainAddressId;

                        }
                    }
                });
            }
        }
    }

    get AgentAddressId() { return this.EntityPM.AgentAddressId; }
    set AgentAddressId(newValue: string) {
        if (this.EntityPM.AgentAddressId != newValue) {
            this.EntityPM.AgentAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AgentAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        if (myResponse.Result) {
                            this.AgentAddressList = myResponse.Result;
                        }

                    }
                });
            }
        }
    }

    private myAgentAddressList: AddressList;
    get AgentAddressList() { return this.myAgentAddressList; }
    set AgentAddressList(newValue: AddressList) {
        this.myAgentAddressList = newValue;
    }

    get AgentContactId() { return this.EntityPM.AgentContactId; }
    set AgentContactId(newValue: string) {
        if (this.EntityPM.AgentContactId != newValue) {
            this.EntityPM.AgentContactId = newValue;
        }
    }

    get AgentReference1() { return this.EntityPM.AgentReference1; }
    set AgentReference1(newValue: string) {
        if (this.EntityPM.AgentReference1 != newValue) {
            this.EntityPM.AgentReference1 = newValue;
        }
    }

    get AgentReference2() { return this.EntityPM.AgentReference2; }
    set AgentReference2(newValue: string) {
        if (this.EntityPM.AgentReference2 != newValue) {
            this.EntityPM.AgentReference2 = newValue;
        }
    }

    get AgentName() { return this.EntityPM.AgentName; }
    set AgentName(newValue: string) {
        if (this.EntityPM.AgentName != newValue) {
            this.EntityPM.AgentName = newValue;
        }
    }






    // Consignee


    private consigneeAddressList: AddressList = new AddressList();
    get ConsigneeAddressList() { return this.consigneeAddressList; }
    set ConsigneeAddressList(newValue: AddressList) {
        this.consigneeAddressList = newValue;

    }



    private ConsigneePartnerTypeId: string;
    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ConsigneePartnerTypeId = null;
                this.ConsigneeCode = null;
                this.ConsigneeContactId = null;
                this.EntityPM.ConsigneeName = null;
                this.EntityPM.ConsigneeNote = null;
                this.EntityPM.ConsigneeReference1 = null;
                this.EntityPM.ConsigneeReference2 = null;
                this.EntityPM.ConsigneeMainAddressId = null;
                this.EntityPM.ConsigneePickAddressId = null;
                this.ConsigneeAddressId = null;
                this.myConsigneeSalesmanId = null;
                this.SetSalesman();
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.ConsigneeCode = myCardList.Code;
                            this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                            this.ConsigneeContactId = myCardList.PrimaryContactId;
                            this.EntityPM.ConsigneeName = myCardList.EnglishName;
                            this.EntityPM.ConsigneeNote = myCardList.Notes;
                            this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                            this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                            this.ConsigneeAddressId = myCardList.MainAddressId;
                            this.myConsigneeSalesmanId = myCardList.SalesmanUserId;

                            if (!this.IsConsolShipment && this.HouseEntity && this.HouseEntity.Consignee) {

                                if (this.HouseEntity.Consignee.Code == this.ConsigneeCode) {
                                    this.EntityPM.ConsigneeReference1 = this.HouseEntity.ConsigneeReference1;
                                    this.EntityPM.ConsigneeReference2 = this.HouseEntity.ConsigneeReference2;
                                }

                            } else if (this.ManifestSL.Consignee && this.ManifestSL.Consignee.Code == this.ConsigneeCode) {
                                this.EntityPM.ConsigneeReference1 = this.ManifestSL.ConsigneeReference1;
                                this.EntityPM.ConsigneeReference2 = this.ManifestSL.ConsigneeReference2;
                            }
                              

                            this.SetSalesman();
                        }
                    }
                });
            }
        }
    }

    get ConsigneeAddressId() { return this.EntityPM.ConsigneeAddressId; }
    set ConsigneeAddressId(newValue: string) {
        if (this.EntityPM.ConsigneeAddressId != newValue) {
            this.EntityPM.ConsigneeAddressId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ConsigneeAddressList = null;
            }

            else {
                this.myAddressListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        if (myResponse.Result) {
                            this.ConsigneeAddressList = myResponse.Result;
                        }
                    }
                });
            }
        }
    }


    get ConsigneeContactId() { return this.EntityPM.ConsigneeContactId; }
    set ConsigneeContactId(newValue: string) {
        if (this.EntityPM.ConsigneeContactId != newValue) {
            this.EntityPM.ConsigneeContactId = newValue;
        }
    }

    get ConsigneeReference1() { return this.EntityPM.ConsigneeReference1; }
    set ConsigneeReference1(newValue: string) {
        if (this.EntityPM.ConsigneeReference1 != newValue) {
            this.EntityPM.ConsigneeReference1 = newValue;
        }
    }

    get ConsigneeReference2() { return this.EntityPM.ConsigneeReference2; }
    set ConsigneeReference2(newValue: string) {
        if (this.EntityPM.ConsigneeReference2 != newValue) {
            this.EntityPM.ConsigneeReference2 = newValue;
        }
    }

    get ConsigneeName() { return this.EntityPM.ConsigneeName; }
    set ConsigneeName(newValue: string) {
        if (this.EntityPM.ConsigneeName != newValue) {
            this.EntityPM.ConsigneeName = newValue;
        }
    }



    





    // Notify1
    private isPartnerChanged_Notify1: boolean;
    get Notify1Id() { return this.EntityPM.Notify1Id; }
    set Notify1Id(newValue: string) {
        if (this.EntityPM.Notify1Id != newValue) {
            this.EntityPM.Notify1Id = newValue;
            this.isPartnerChanged_Notify1 = true;
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
            this.Notify1Code = null;
            

      
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
                        this.Notify1Code = myCard.Code;
                      
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
            myService.GetMainAddressByCardId(this.Notify1Id, SessionInfo.LoggedUserTenant).subscribe((myResponse: ServiceResponse) => {
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
        }

    }


      // Notify2
    private isPartnerChanged_Notify2: boolean;
    get Notify2Id() { return this.EntityPM.Notify2Id; }
    set Notify2Id(newValue: string) {
        if (this.EntityPM.Notify2Id != newValue) {
            this.EntityPM.Notify2Id = newValue;
            this.isPartnerChanged_Notify2 = true;
            this.GetNotify2Card();
        }
    }

    get Notify2AddressId() { return this.EntityPM.Notify2AddressId; }
    set Notify2AddressId(newValue: string) {
        if (this.EntityPM.Notify2AddressId != newValue) {
            this.EntityPM.Notify2AddressId = newValue;
            this.GetNotify2Address();
        }
    }

    private GetNotify2Card() {

        if (this.Notify2Id == null) {
            this.Notify2AddressId = null;
            this.Notify2AddressList = null;
            this.EntityPM.Notify2Name = null;
            this.EntityPM.Notify2Note = null;
            this.EntityPM.Notify2ContactId = null;

        }

        else {
            this.LoadNotify2Card();
        }
    }
    private LoadNotify2Card() {
        this.myCardListService.getSingle(this.Notify2Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.Notify2Name = myCard.EnglishName;
                        this.EntityPM.Notify2Note = myCard.Notes;
                        this.EntityPM.Notify2ContactId = myCard.PrimaryContactId;
                    }

                    if (this.isPartnerChanged_Notify2) {
                        this.GetNotify2MainAddress();
                    }

                    else {
                        this.GetNotify2Address();
                    }

                    this.isPartnerChanged_Notify2 = false;
                }
            }
        });
    }

    public Notify2AddressList: AddressList;
    private GetNotify2Address() {
        if (!AppTool.IsNullOrEmpty(this.Notify2AddressId)) {
            var myService: AddressListService = new AddressListService();

            myService.getSingle(this.Notify2AddressId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetNotify2Address(myResponse.Result);
                    }
                }
            });
        }
    }
    private GetNotify2MainAddress() {
        if (!AppTool.IsNullOrEmpty(this.Notify2Id)) {
            var myService: AddressService = new AddressService();
            myService.GetMainAddressByCardId(this.Notify2Id, SessionInfo.LoggedUserTenant).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.SetNotify2Address(myResponse.Result);
                    }
                }
            });
        }
    }

    private SetNotify2Address(list: AddressList) {
        this.Notify2AddressList = list;

        if (list == null) {
            if (this.EntityPM.Notify2AddressId != null) {
                this.EntityPM.Notify2AddressId = null;
            }
        }

        else {

            if (this.EntityPM.Notify2AddressId != list.Id) {
                this.EntityPM.Notify2AddressId = list.Id;
            }

       

        }


    }

    //IncotermId

    get IncotermId() { return this.EntityPM.IncotermId; }
    set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;

            if (newValue != null) {
                this.myIncotermListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myList: IncotermList = myResponse.Result;
                        if (myList) {
                            this.IncotermCode = myList.Code;
                            this.EntityPM.IncotermCode = myList.Code;
                            this.EntityPM.IncotermName = myList.Name;
                            this.FreightPrepaidCollectId = myList.Freight;
                            this.OtherPrepaidCollectId = myList.OtherCharges;
                        }
                    }
                });
            }
        }
    }



    get MoveTypeId() { return this.EntityPM.MoveTypeId; }
    set MoveTypeId(newValue: string) {
        if (this.EntityPM.MoveTypeId != newValue) {
            this.EntityPM.MoveTypeId = newValue;

            if (newValue != null) {
                this.myMoveTypeListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myList: MoveTypeList = myResponse.Result;
                        if (myList) {
                            this.MoveTypeCode = myList.Code;
                            this.EntityPM.MoveTypeCode = myList.Code;
                            this.EntityPM.MoveTypeName = myList.MoveTypeEnglishName;
   
                        }
                    }
                });
            }
        }
    }





    get ValueOfGoodsCurrencyId() { return this.EntityPM.ValueOfGoodsCurrencyId; }
    set ValueOfGoodsCurrencyId(newValue: string) {
        if (this.EntityPM.ValueOfGoodsCurrencyId != newValue) {
            this.EntityPM.ValueOfGoodsCurrencyId = newValue;
            if (newValue != null) {
                this.myCurrencyListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myList: CurrencyList = myResponse.Result;
                        if (myList) {
                            this.ValueOfGoodsCurrencyCode = myList.Code;
    

                        }
                    }
                });
            }
        }
    }



    get ValueOfGoods() { return this.EntityPM.ValueOfGoods; }
    set ValueOfGoods(newValue: number) {
        if (this.EntityPM.ValueOfGoods != newValue) {
            this.EntityPM.ValueOfGoods = newValue;
        }
    }

    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
        }
    }

    get MainHarmonize() { return this.EntityPM.MainHarmonize; }
    set MainHarmonize(newValue: string) {
        if (this.EntityPM.MainHarmonize != newValue) {
            this.EntityPM.MainHarmonize = newValue;
        }
    }


    

    //MainCarriageVesselId

    MainCarriageVesselCode: string;
    get MainCarriageVesselId() { return this.EntityPM.MainCarriageVesselId; }
    set MainCarriageVesselId(newValue: string) {
        if (this.EntityPM.MainCarriageVesselId != newValue) {
            this.EntityPM.MainCarriageVesselId = newValue;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageVesselId)) {

                if (!this.IsHideMainCarriageVessel) {

                    this.myVesselListService.getSingleFromCache(this.EntityPM.MainCarriageVesselId).subscribe((myResponse: ServiceResponse) => {
                        if (myResponse != null) {
                            if (!myResponse.HasError) {

                                var list: VesselList = myResponse.Result;
                                if (list) {
                                    this.MainCarriageVesselCode = myResponse.Result.Code;
                                    this.EntityPM.MainCarriageVesselName = myResponse.Result.Code;
                                }

                            }
                        }
                    });
                }
                else {

                    this.MainCarriageVesselCode = this.ManifestSL.MainCarriageVesselCode;
                    this.EntityPM.MainCarriageVesselName = this.ManifestSL.MainCarriageVesselName;
                }
            }

        }
    }

    get Transshipment1VesselId() { return this.EntityPM.Transshipment1VesselId; }
    set Transshipment1VesselId(value: string) {
        if (this.EntityPM.Transshipment1VesselId != value) {
            this.EntityPM.Transshipment1VesselId = value;

            if (!this.IsHideTransshipment1Vessel) {
                if (AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.Transshipment1VesselName = null;
                    this.Transshipment1VesselCode = null;
                }

                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: VesselList = myResponse.Result;
                            if (list) {
                                this.Transshipment1VesselCode = list.Code;
                                this.EntityPM.Transshipment1VesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
            else {
                this.Transshipment1VesselCode = this.ManifestSL.Transshipment1VesselCode;
                this.EntityPM.Transshipment1VesselName = this.ManifestSL.Transshipment1VesselName;
       

            }
        }
    }

    get Transshipment2VesselId() { return this.EntityPM.Transshipment2VesselId; }
    set Transshipment2VesselId(value: string) {
        if (this.EntityPM.Transshipment2VesselId != value) {
            this.EntityPM.Transshipment2VesselId = value;

            if (!this.IsHideTransshipment2Vessel) {
                if (AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.Transshipment2VesselName = null;
                    this.Transshipment2VesselCode = null;
                }

                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: VesselList = myResponse.Result;
                            if (list) {
                                this.Transshipment2VesselCode = list.Code;
                                this.EntityPM.Transshipment2VesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
            else {
                this.Transshipment2VesselCode = this.ManifestSL.Transshipment2VesselCode;
                this.EntityPM.Transshipment2VesselName = this.ManifestSL.Transshipment2VesselName;


            }
        }
    }


    get Transshipment3VesselId() { return this.EntityPM.Transshipment3VesselId; }
    set Transshipment3VesselId(value: string) {
        if (this.EntityPM.Transshipment3VesselId != value) {
            this.EntityPM.Transshipment3VesselId = value;

            if (!this.IsHideTransshipment3Vessel) {
                if (AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.Transshipment3VesselName = null;
                    this.Transshipment3VesselCode = null;
                }

                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: VesselList = myResponse.Result;
                            if (list) {
                                this.Transshipment3VesselCode = list.Code;
                                this.EntityPM.Transshipment3VesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
            else {
                this.Transshipment3VesselCode = this.ManifestSL.Transshipment3VesselCode;
                this.EntityPM.Transshipment3VesselName = this.ManifestSL.Transshipment3VesselName;


            }
        }
    }


    //IsInterlineAdded
    private isInterlineAdded: boolean = false;
    get IsInterlineAdded() { return this.isInterlineAdded; }
    set IsInterlineAdded(value: boolean) {
        if (this.isInterlineAdded != value) {
            this.isInterlineAdded = value;
        }
    }



    InterlineCode: string;
    get InterlineId() { return this.EntityPM.InterlineId; }
    set InterlineId(newValue: string) {
        if (this.EntityPM.InterlineId != newValue) {
            this.EntityPM.InterlineId = newValue;

            this.OnInterlineChanged();
        }
    }

 
    OnInterlineChanged() {
        var myAirlineId: string = this.InterlineId;
        if (AppTool.IsNullOrEmpty(myAirlineId)) {
            myAirlineId = this.MainCarriageCarrierId;
        }

        if (AppTool.IsNullOrEmpty(myAirlineId)) {
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;
            this.AirlinePrefix = null;
        }

        else {
            this.myAirlineListService.getSingleFromCache(myAirlineId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AirlineList = myResponse.Result;
                  
                    if (list != null) {
                        this.InterlineCode = list.Code;
                        this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                        this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;

                        var myPrefix: string = null;
                        if (!AppTool.IsNullOrEmpty(list.Prefix)) {
                            myPrefix = list.Prefix.toString().trim();
                            myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                        }

                        this.AirlinePrefix = myPrefix;
                        this.EntityPM.AirlinePrefix = myPrefix; 
                    }
                }
            });
        }
    }
   




    get FreightPrepaidCollectId() { return this.EntityPM.FreightPrepaidCollectId; }
    set FreightPrepaidCollectId(newValue: string) {
        if (this.EntityPM.FreightPrepaidCollectId != newValue) {
            this.EntityPM.FreightPrepaidCollectId = newValue;
        }
    }

    get OtherPrepaidCollectId() { return this.EntityPM.OtherPrepaidCollectId; }
    set OtherPrepaidCollectId(newValue: string) {
        if (this.EntityPM.OtherPrepaidCollectId != newValue) {
            this.EntityPM.OtherPrepaidCollectId = newValue;
        }
    }



    // Carrier
    get Transshipment1CarrierId() { return this.EntityPM.Transshipment1CarrierId; }
    set Transshipment1CarrierId(value: string) {
        if (this.EntityPM.Transshipment1CarrierId != value) {
            this.EntityPM.Transshipment1CarrierId = value;

         
            if (AppTool.IsNullOrEmpty(value)) {
               this.Transshipment1CarrierPrefix = null;
              //  this.Transshipment1CarrierNumber = null;
                this.Transshipment1CarrierCode = null;
                this.EntityPM.Transshipment1AdditionalMAWBOBLBL = null;
                //RoutingHelper.Transshipment1CarrierChanged(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            this.Transshipment1CarrierCode = list.Code;
                            this.EntityPM.Transshipment1CarrierCode = list.Code;
                            this.EntityPM.Transshipment1CarrierName = list.EnglishName;
                            this.EntityPM.Transshipment1CarrierWebSite = list.WebSite;

                            if (this.EntityPM.TransportModeId == "A") {
                                this.Transshipment1CarrierPrefix = list.Code;
                             
                            }

              
                        }
                    }
                });
            }
        }
    }

    get Transshipment2CarrierId() { return this.EntityPM.Transshipment2CarrierId; }
    set Transshipment2CarrierId(value: string) {
        if (this.EntityPM.Transshipment2CarrierId != value) {
            this.EntityPM.Transshipment2CarrierId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.Transshipment2CarrierPrefix = null;
                this.Transshipment2CarrierCode = null;
               // this.Transshipment2CarrierNumber = null;
               // this.Transshipment2AdditionalMAWBOBLBL = null;
             //   RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.Transshipment2CarrierCode = list.Code;
                            this.EntityPM.Transshipment2CarrierCode = list.Code;
                            this.EntityPM.Transshipment2CarrierName = list.EnglishName;
                            this.EntityPM.Transshipment2CarrierWebSite = list.WebSite;

                            if (this.EntityPM.TransportModeId == "A") {
                                this.Transshipment2CarrierPrefix = list.Code;
                            }

                           // RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, list);
                        }
                    }
                });
            }
        }
    }

    get Transshipment3CarrierId() { return this.EntityPM.Transshipment3CarrierId; }
    set Transshipment3CarrierId(value: string) {
        if (this.EntityPM.Transshipment3CarrierId != value) {
            this.EntityPM.Transshipment3CarrierId = value;


            if (AppTool.IsNullOrEmpty(value)) {
                this.Transshipment3CarrierPrefix = null;
                //  this.Transshipment3CarrierNumber = null;
                this.Transshipment3CarrierCode = null;
                this.EntityPM.Transshipment3AdditionalMAWBOBLBL = null;
                //RoutingHelper.Transshipment3CarrierChanged(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            this.Transshipment3CarrierCode = list.Code;
                            this.EntityPM.Transshipment3CarrierCode = list.Code;
                            this.EntityPM.Transshipment3CarrierName = list.EnglishName;
                            this.EntityPM.Transshipment3CarrierWebSite = list.WebSite;

                            if (this.EntityPM.TransportModeId == "A") {
                                this.Transshipment3CarrierPrefix = list.Code;

                            }


                        }
                    }
                });
            }
        }
    }


    // Carrier Prefix
    get Transshipment1CarrierPrefix() { return this.EntityPM.Transshipment1CarrierPrefix; }
    set Transshipment1CarrierPrefix(value: string) {
        if (this.EntityPM.Transshipment1CarrierPrefix != value) {
            this.EntityPM.Transshipment1CarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get Transshipment2CarrierPrefix() { return this.EntityPM.Transshipment2CarrierPrefix; }
    set Transshipment2CarrierPrefix(value: string) {
        if (this.EntityPM.Transshipment2CarrierPrefix != value) {
            this.EntityPM.Transshipment2CarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get Transshipment3CarrierPrefix() { return this.EntityPM.Transshipment3CarrierPrefix; }
    set Transshipment3CarrierPrefix(value: string) {
        if (this.EntityPM.Transshipment3CarrierPrefix != value) {
            this.EntityPM.Transshipment3CarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    
    //MainCarriageCarrierId

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;
            // this.SetUIProperties_MasterField();

            if (!newValue) {
                this.Master = null;
                this.AirlinePrefix = null;
                this.LongMaster = null;
                this.AccountNumber = null;
                this.EntityPM.MainCarriageCarrierCode = null;
                this.EntityPM.MainCarriageCarrierName = null;
                this.EntityPM.MainCarriageCarrierNumber = null;
                this.EntityPM.MainCarriageCarrierPrefix = null;
                this.EntityPM.CarrierIsCheckDigit = false;
                this.EntityPM.CarrierIsLimitedLength = false;
                ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myCardListResponse: ServiceResponse) => {
                    if (!myCardListResponse.HasError) {

                        var myCardList: CardList = myCardListResponse.Result;
                        if (myCardList) {
                            this.MainCarriageCarrierCode = myCardList.Code;
                            this.EntityPM.MainCarriageCarrierCode = myCardList.Code;
                            this.EntityPM.MainCarriageCarrierName = myCardList.EnglishName;
                            this.EntityPM.MainCarriageCarrierWebSite = myCardList.WebSite;
              
                            if (this.EntityPM.TransportModeId == "A") {
                                this.AccountNumber = myCardList.AirlineAccountNumber;

                                if (myCardList.Code != null) {
                                    if (myCardList.Code.length <= 2) {
                                        this.EntityPM.MainCarriageCarrierPrefix = myCardList.Code;
                                    }
                                }

                                // dont get from chach: if user choosed from tenant0 it wont get it
                                this.myAirlineListService.getSingle(newValue).subscribe((myAirlineListResponse: ServiceResponse) => {
                                    var myAirlineList: AirlineList = myAirlineListResponse.Result;

                                    if (myAirlineList != null) {
                                        this.EntityPM.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                        this.EntityPM.CarrierIsLimitedLength = myAirlineList.LimitedLength;
                                        this.EntityPM.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                        this.EntityPM.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;
                      
                                        var myPrefix: string = null;
                                        if (!AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                            myPrefix = myAirlineList.Prefix.toString().trim();
                                            myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                                        }

                                        this.AirlinePrefix = myPrefix;

                                        this.myPartnersDomainService.GetAirlineByCode(myAirlineList.Code, 0).subscribe((myResponse: ServiceResponse) => {
                                            if (!myResponse.HasError) {
                                                ShipmentTool.MapTenantZeroAirline(this.EntityPM, myResponse.Result);
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    }
                });
            }
        }
    }

    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(newValue: string) {
        if (this.EntityPM.AirlinePrefix != newValue) {
            this.EntityPM.AirlinePrefix = newValue;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get LongMaster() { return this.EntityPM.LongMaster; }
    set LongMaster(newValue: string) {
        if (this.EntityPM.LongMaster != newValue) {
            this.EntityPM.LongMaster = newValue;
            // this.ValidateMasterField();
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(newValue: string) {
        if (this.EntityPM.Master != newValue) {
            this.EntityPM.Master = newValue;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }


    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(newValue: string) {
        if (this.EntityPM.AccountNumber != newValue) {
            this.EntityPM.AccountNumber = newValue;
        }
    }


    SharedManifestStatus: string;
    IsInlandDomestic: boolean;
    




    SetWindowArgs(args: any) {

        this.CurrentEntity = args.CurrentEntity;
        this.ManifestSL = args.ManifestSL;
        this.HouseEntity = args.HouseEntity;
        this.AgentSharedManifestList = args.AgentSharedManifestList;
        this.SharedManifestStatus = args.SharedManifestStatus;


        if (this.ManifestSL && this.CurrentEntity) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");

            this.RunComponent();
            this.FullShipmentProperites();
            this.BuildAgentSide();
            this.BuildAgentShipmentPickUpDelivey();
            this.BuildOurSide();
            //this.BuildAdditionalFields();//islam
           
            this.StopBusyIndicator();
        }

    }


    AgentShipmentPickUpMainCarriageETD: Date;
    AgentShipmentPickUpMainCarriageETA: Date;
    AgentShipmentPickUpMainCarriageATD: Date;
    AgentShipmentPickUpMainCarriageATA: Date;

    AgentShipmentDeliveryMainCarriageETD: Date;
    AgentShipmentDeliveryMainCarriageETA: Date;

    AgentShipmentDeliveryMainCarriageATD: Date;
    AgentShipmentDeliveryMainCarriageATA: Date;



    BuildAgentShipmentPickUpDelivey() {
     
        if (this.ManifestSL.ShipmentPickUp) {
            this.AgentShipmentPickUpMainCarriageETD = this.ManifestSL.ShipmentPickUp.MainCarriageETD;
            this.AgentShipmentPickUpMainCarriageETA = this.ManifestSL.ShipmentPickUp.MainCarriageETA;
            this.AgentShipmentPickUpMainCarriageATD = this.ManifestSL.ShipmentPickUp.MainCarriageATD;
            this.AgentShipmentPickUpMainCarriageATA = this.ManifestSL.ShipmentPickUp.MainCarriageATA;

        }

        if (this.ManifestSL.ShipmentDelivery) {
            this.AgentShipmentDeliveryMainCarriageETD = this.ManifestSL.ShipmentDelivery.MainCarriageETD;
            this.AgentShipmentDeliveryMainCarriageETA = this.ManifestSL.ShipmentDelivery.MainCarriageETA;
            this.AgentShipmentDeliveryMainCarriageATD = this.ManifestSL.ShipmentDelivery.MainCarriageATD;
            this.AgentShipmentDeliveryMainCarriageATA = this.ManifestSL.ShipmentDelivery.MainCarriageATA;

        } 
    }

    FullShipmentProperites() {

        this.EntityPM = this.myShipmentPMService.GetNewEntityPM();
        if (this.EntityPM) {
       

            this.EntityPM.DirectionId = "I";
            this.EntityPM.TransportModeId = this.ManifestSL.TransportModeId;
            this.EntityPM.Master = this.ManifestSL.MasterNumber;
            this.EntityPM.LongMaster = this.ManifestSL.LongMaster;
            this.EntityPM.HAWBDate = this.HouseEntity != null ? this.HouseEntity.HAWBDate : this.ManifestSL.HAWBDate;
            this.EntityPM.IsDangerous = this.HouseEntity != null ? this.HouseEntity.IsDangerous : this.ManifestSL.IsDangerous;
            this.EntityPM.MainCarriageCarrierNumber = this.HouseEntity != null ? this.HouseEntity.MainCarriageCarrierNumber : this.ManifestSL.MainCarriageCarrierNumber;


            this.EntityPM.ShipperName = this.HouseEntity != null ? this.HouseEntity.ShipperName : this.ManifestSL.ShipperName;
            this.EntityPM.ConsigneeReference1 = this.HouseEntity != null ? this.HouseEntity.ConsigneeReference1 : this.ManifestSL.ConsigneeReference1;
            this.EntityPM.ConsigneeReference2 = this.HouseEntity != null ? this.HouseEntity.ConsigneeReference2 : this.ManifestSL.ConsigneeReference2;
            this.EntityPM.ShipperReference1 = this.HouseEntity != null ? this.HouseEntity.ShipperReference1 : this.ManifestSL.ShipperReference1;
            this.EntityPM.ShipperReference2 = this.HouseEntity != null ? this.HouseEntity.ShipperReference2 : this.ManifestSL.ShipperReference2;

            this.EntityPM.AgentSharedManifestRef = this.HouseEntity != null ? this.HouseEntity.SharedManifestRef : this.ManifestSL.SharedManifestRef;
            this.EntityPM.ShipmentLevelCode = this.HouseEntity != null ? "H" : this.ManifestSL.ShipmentLevelCode;
            this.EntityPM.House = this.HouseEntity != null ? this.HouseEntity.HouseNumber : this.ManifestSL.HouseNumber;
            this.EntityPM.MasterShipmentDataId = this.ManifestSL != null ? this.ManifestSL.EntityId:null ;
            this.EntityPM.ShipmentPackages = this.HouseEntity != null ? this.HouseEntity.ShipmentPackages : this.ManifestSL.ShipmentPackages;

    
            this.EntityPM.DescriptionOfGoods = this.HouseEntity != null ? this.HouseEntity.GeneralDescriptionOfGoods : this.ManifestSL.GeneralDescriptionOfGoods;
            this.EntityPM.ChargeableWeight = this.HouseEntity != null ? this.HouseEntity.ChargeableWeight : this.ManifestSL.ChargeableWeight;
            this.EntityPM.GrossWeight = this.HouseEntity != null ? this.HouseEntity.GrossWeight : this.ManifestSL.GrossWeight;
            this.EntityPM.PackagesQuantity = this.HouseEntity != null ? this.HouseEntity.PackagesQuantity : this.ManifestSL.PackagesQuantity;
            this.EntityPM.TEU = this.HouseEntity != null ? this.HouseEntity.TEU : this.ManifestSL.TEU;

            this.EntityPM.Volume = this.HouseEntity != null ? this.HouseEntity.Volume : this.ManifestSL.Volume;
            this.EntityPM.VolumetricWeight = this.HouseEntity != null ? this.HouseEntity.VolumetricWeight : this.ManifestSL.VolumetricWeight;
            this.EntityPM.NumberOfContainers = this.HouseEntity != null ? this.HouseEntity.NumberOfContainers : this.ManifestSL.NumberOfContainers;
            this.EntityPM.NumberOfPackages = this.HouseEntity != null ? this.HouseEntity.NumberOfPackages : this.ManifestSL.NumberOfPackages;
            this.EntityPM.GrossWeightUnitCode = this.HouseEntity != null ? this.HouseEntity.GrossWeightUnitCode : this.ManifestSL.GrossWeightUnitCode;

            this.EntityPM.ChargeableWeightUnitCode = this.HouseEntity != null ? this.HouseEntity.ChargeableWeightUnitCode : this.ManifestSL.ChargeableWeightUnitCode;
            this.EntityPM.DimensionsUnitCode = this.HouseEntity != null ? this.HouseEntity.DimensionsUnitCode : this.ManifestSL.DimensionsUnitCode;
            this.EntityPM.VolumeUnitCode = this.HouseEntity != null ? this.HouseEntity.VolumeUnitCode : this.ManifestSL.VolumeUnitCode;
            this.EntityPM.GrossWeightEdited = this.HouseEntity != null ? this.HouseEntity.GrossWeightEdited : this.ManifestSL.GrossWeightEdited;
            this.EntityPM.ShipmentTypeId = this.HouseEntity != null ? this.HouseEntity.ShipmentTypeId : this.ManifestSL.ShipmentTypeId;

            this.EntityPM.ShipmentTypeName = this.HouseEntity != null ? this.HouseEntity.ShipmentTypeName : this.ManifestSL.ShipmentTypeName;
            this.EntityPM.OrderGrossWeight = this.HouseEntity != null ? this.HouseEntity.OrderGrossWeight : this.ManifestSL.OrderGrossWeight;
            this.EntityPM.ValueOfGoods = this.HouseEntity != null ? this.HouseEntity.ValueOfGoods : this.ManifestSL.ValueOfGoods;

            this.EntityPM.MainHarmonize = this.HouseEntity != null ? this.HouseEntity.MainHarmonize : this.ManifestSL.MainHarmonize;

            if (this.ManifestSL) {
    
                this.EntityPM.TrailerNumber = this.ManifestSL.TrailerNumber;
                this.EntityPM.TruckNumber = this.ManifestSL.TruckNumber;
                this.EntityPM.MAWBOBLDate = this.ManifestSL.MAWBOBLDate;

                this.EntityPM.MainCarriageETD = this.ManifestSL.MainCarriageETD;
                this.EntityPM.MainCarriageATD = this.ManifestSL.MainCarriageATD;
                this.EntityPM.MainCarriageETA = this.ManifestSL.MainCarriageETA;
                this.EntityPM.Transshipment1AdditionalMAWBOBLBL = this.ManifestSL.Transshipment1MAWBOBL;
                this.EntityPM.Transshipment1ETD = this.ManifestSL.Transshipment1ETD;
                this.EntityPM.Transshipment1ATD = this.ManifestSL.Transshipment1ATD;
                this.EntityPM.Transshipment1ETA = this.ManifestSL.Transshipment1ETA;
                this.EntityPM.Transshipment2AdditionalMAWBOBLBL = this.ManifestSL.Transshipment2MAWBOBL;
                this.EntityPM.Transshipment2ETD = this.ManifestSL.Transshipment2ETD;
                this.EntityPM.Transshipment2ATD = this.ManifestSL.Transshipment2ATD;
                this.EntityPM.Transshipment2ETA = this.ManifestSL.Transshipment2ETA;
                this.EntityPM.Transshipment3AdditionalMAWBOBLBL = this.ManifestSL.Transshipment3MAWBOBL;
                this.EntityPM.Transshipment3ETD = this.ManifestSL.Transshipment3ETD;
                this.EntityPM.Transshipment3ATD = this.ManifestSL.Transshipment3ATD;
                this.EntityPM.Transshipment3ETA = this.ManifestSL.Transshipment3ETA;
                this.EntityPM.MainCarriageCarrierNumber = this.ManifestSL.MainCarriageCarrierNumber;
                this.EntityPM.Transshipment1CarrierNumber = this.ManifestSL.Transshipment1CarrierNumber;
                this.EntityPM.Transshipment2CarrierNumber = this.ManifestSL.Transshipment2CarrierNumber;
                this.EntityPM.Transshipment3CarrierNumber = this.ManifestSL.Transshipment3CarrierNumber;

            }
         
            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
            this.myPortListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PortList[] = myResponse.Result;
                    if (this.ManifestSL.MainCarriageFromPort != null) {
                        var fromPort: PortList = list.filter(d => d.Code == this.ManifestSL.MainCarriageFromPort.Code)[0];
                        if (fromPort) {
                            this.EntityPM.FromPortId = fromPort.Id;
                            this.EntityPM.MainCarriageFromPortId = fromPort.Id;
                            this.FromPortList = fromPort;
                        }
                    }
                    if (this.ManifestSL.MainCarriageToPort != null) {
                        var toPort: PortList = list.filter(d => d.Code == this.ManifestSL.MainCarriageToPort.Code)[0];
                        if (toPort) {
                            this.EntityPM.ToPortId = toPort.Id;
                            this.EntityPM.MainCarriageToPortId = toPort.Id;
                            this.ToPortList = toPort;
                        }
                    }

                    if (!this.IsHouseShipment) {

                        //Transshipment1

                        if (this.ManifestSL.Transshipment1FromPort != null) {
                            var transshipment1FromPortCode: PortList = list.filter(d => d.Code == this.ManifestSL.Transshipment1FromPort.Code)[0];
                            if (transshipment1FromPortCode) {
                                this.EntityPM.Transshipment1FromPortCode = transshipment1FromPortCode.Code;
                                this.EntityPM.Transshipment1FromPortName = transshipment1FromPortCode.EnglishName;
                                this.EntityPM.Transshipment1FromPortId = transshipment1FromPortCode.Id;
                                this.EntityPM.Transshipment1FromPortCountryCode = transshipment1FromPortCode.CountryCode;
                                this.EntityPM.Transshipment1FromPortCountryName = transshipment1FromPortCode.CountryName;
                            }
                        }
                        if (this.ManifestSL.Transshipment1ToPort != null) {
                            var transshipment1ToPortCode: PortList = list.filter(d => d.Code == this.ManifestSL.Transshipment1ToPort.Code)[0];
                            if (transshipment1ToPortCode) {
                                this.EntityPM.Transshipment1ToPortCode = transshipment1ToPortCode.Code;
                                this.EntityPM.Transshipment1ToPortName = transshipment1ToPortCode.EnglishName;
                                this.EntityPM.Transshipment1ToPortId = transshipment1ToPortCode.Id;
                                this.EntityPM.Transshipment1ToPortCountryCode = transshipment1ToPortCode.CountryCode;
                                this.EntityPM.Transshipment1ToPortCountryName = transshipment1ToPortCode.CountryName;
                            }
                        }


                         //Transshipment2

                        if (this.ManifestSL.Transshipment2FromPort != null) {
                            var transshipment2FromPortCode: PortList = list.filter(d => d.Code == this.ManifestSL.Transshipment2FromPort.Code)[0];
                            if (transshipment2FromPortCode) {
                                this.EntityPM.Transshipment2FromPortCode = transshipment2FromPortCode.Code;
                                this.EntityPM.Transshipment2FromPortName = transshipment2FromPortCode.EnglishName;
                                this.EntityPM.Transshipment2FromPortId = transshipment2FromPortCode.Id;
                                this.EntityPM.Transshipment2FromPortCountryCode = transshipment2FromPortCode.CountryCode;
                                this.EntityPM.Transshipment2FromPortCountryName = transshipment2FromPortCode.CountryName;
                            }
                        }
                        if (this.ManifestSL.Transshipment2ToPort != null) {
                            var transshipment2ToPortCode: PortList = list.filter(d => d.Code == this.ManifestSL.Transshipment2ToPort.Code)[0];
                            if (transshipment2ToPortCode) {
                                this.EntityPM.Transshipment2ToPortCode = transshipment2ToPortCode.Code;
                                this.EntityPM.Transshipment2ToPortName = transshipment2ToPortCode.EnglishName;
                                this.EntityPM.Transshipment2ToPortId = transshipment2ToPortCode.Id;
                                this.EntityPM.Transshipment2ToPortCountryCode = transshipment2ToPortCode.CountryCode;
                                this.EntityPM.Transshipment2ToPortCountryName = transshipment2ToPortCode.CountryName;
                            }
                        }



                         //Transshipment3

                        if (this.ManifestSL.Transshipment3FromPort != null) {
                            var transshipment3FromPortCode: PortList = list.filter(d => d.Code == this.ManifestSL.Transshipment3FromPort.Code)[0];
                            if (transshipment3FromPortCode) {
                                this.EntityPM.Transshipment3FromPortCode = transshipment3FromPortCode.Code;
                                this.EntityPM.Transshipment3FromPortName = transshipment3FromPortCode.EnglishName;
                                this.EntityPM.Transshipment3FromPortId = transshipment3FromPortCode.Id;
                                this.EntityPM.Transshipment3FromPortCountryCode = transshipment3FromPortCode.CountryCode;
                                this.EntityPM.Transshipment3FromPortCountryName = transshipment3FromPortCode.CountryName;
                            }
                        }
                        if (this.ManifestSL.Transshipment3ToPort != null) {
                            var transshipment3ToPortCode: PortList = list.filter(d => d.Code == this.ManifestSL.Transshipment3ToPort.Code)[0];
                            if (transshipment3ToPortCode) {
                                this.EntityPM.Transshipment3ToPortCode = transshipment3ToPortCode.Code;
                                this.EntityPM.Transshipment3ToPortName = transshipment3ToPortCode.EnglishName;
                                this.EntityPM.Transshipment3ToPortId = transshipment3ToPortCode.Id;
                                this.EntityPM.Transshipment3ToPortCountryCode = transshipment3ToPortCode.CountryCode;
                                this.EntityPM.Transshipment3ToPortCountryName = transshipment3ToPortCode.CountryName;
                            }
                        }



                    }

                }

                this.IsLoadedPortsTranslation = true;
                this.StopBusyIndicator();
            });

        }

        if (this.EntityPM && this.EntityPM.TransportModeId == "A") {
            this.IsRemovePackageAreaFromScreen = true;
        }

    }

    BuildAgentSide() {

        if (!this.HouseEntity) {
            this.AgentSideData = new AgentSide(this.ManifestSL , null);
        }
        else {
            this.AgentSideData = new AgentSide( null, this.HouseEntity);
         
        }

    }

    ShipmentPackages: ShipmentPackagePM[] = [];
    OurAgentPackagesType: PackageTypePM[] = [];

    IsHideRoutingLeg1: boolean = false;
    IsHideRoutingLeg2: boolean = false;
    IsHideRoutingLeg3: boolean = false;


    BuildOurSide() {


        switch (this.ManifestSL.TransportModeId) {
            case "A": { this.CarrierDependencyProperty1 = "AL"; break; }
            case "O": { this.CarrierDependencyProperty1 = "SL"; break; }
            case "I": { this.CarrierDependencyProperty1 = "TR"; break; }

        }

        if (this.HouseEntity) {
            this.IsHouseShipment = true;
            this.LableCreateButton = "Create House";
        } else {
            if (this.ManifestSL.ShipmentLevelCode == "C") {
                this.LableCreateButton = "Create Master";
                this.IsConsolShipment = true;
                this.ShipperId = this.CurrentEntity.AgentId;
                this.ConsigneeId = SessionLocator.TenantPM.AgentId;
                this.ConsigneeAddressId = SessionLocator.TenantPM.AddressId;
                this.ObjectTableName = "Master";

            } else {
                this.AgentId = this.CurrentEntity.AgentId;
                this.LableCreateButton = "Create Direct";
            }
        }



       //     LableCreateButton

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }



        if (!this.IsHouseShipment) this.IsShowTransShipmentsDetails = true;

        if (AppTool.IsNullOrEmpty(this.AgentSideData.IncotermCode)) this.IsHideIncoterm = true;
        if (AppTool.IsNullOrEmpty(this.AgentSideData.ValueOfGoods)) this.IsHideValueOfGoods = true;
        if (AppTool.IsNullOrEmpty(this.AgentSideData.DescriptionOfGoods)) this.IsHideDescriptionOfGoods = true;
        if (!this.AgentSideData.IsDangerous) this.IsHideIsDangerous = true;
        if (AppTool.IsNullOrEmpty(this.AgentSideData.MoveTypeCode)) this.IsHideMoveType = true;
        if (AppTool.IsNullOrEmpty(this.AgentSideData.ValueOfGoodsCurrencyCode)) this.IsHideValueOfGoodsCurrency = true;
        if (AppTool.IsNullOrEmpty(this.AgentSideData.MainHarmonize)) this.IsHideMainHarmonize = true;
        


        
        if (AppTool.IsNullOrEmpty(this.AgentSideData.CarrierCode) || this.EntityPM.ShipmentLevelCode == "H") this.IsHideCarrier = true;

        if (this.IsHouseShipment || this.EntityPM.TransportModeId != "O" || AppTool.IsNullOrEmpty(this.ManifestSL.MainCarriageVesselCode)) this.IsHideMainCarriageVessel = true;

        if (this.IsHouseShipment || this.EntityPM.TransportModeId != "A" || AppTool.IsNullOrEmpty(this.ManifestSL.InterlineCode)) this.IsHideMainInterline = true;


        if (this.IsHouseShipment || AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortCode)) this.IsHideRoutingLeg1 = true; 



        if (this.IsHouseShipment || AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortCode)) this.IsHideRoutingLeg2= true; 

        if (this.IsHouseShipment || AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3FromPortCode)) this.IsHideRoutingLeg3 = true; 

        
        
        if (!AppTool.IsNullOrEmpty(this.AgentSideData.IncotermId) && !this.AgentSideData.IncotermAddedManually) {
            this.IncotermId = this.AgentSideData.IncotermId;
            this.IncotermCode = this.AgentSideData.IncotermCode;
        }

        if (!AppTool.IsNullOrEmpty(this.AgentSideData.CarrierId) && !this.AgentSideData.CarrierAddedManually) {
            this.MainCarriageCarrierId = this.AgentSideData.CarrierId;
            this.MainCarriageCarrierCode = this.AgentSideData.CarrierCode;
        }

        if (!AppTool.IsNullOrEmpty(this.AgentSideData.MoveTypeId) && !this.AgentSideData.MoveTypeAddedManually) {
            this.MoveTypeId = this.AgentSideData.MoveTypeId;
            this.MoveTypeCode = this.AgentSideData.MoveTypeCode;
        }

        if (!AppTool.IsNullOrEmpty(this.AgentSideData.ValueOfGoodsCurrencyId) && !this.AgentSideData.ValueOfGoodsCurrencyAddedManually) {
            this.ValueOfGoodsCurrencyId = this.AgentSideData.ValueOfGoodsCurrencyId;
            this.ValueOfGoodsCurrencyCode = this.AgentSideData.ValueOfGoodsCurrencyCode;
        }

        //leg 1

        if (!this.IsHideRoutingLeg1) {
       
           if (!AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment1CarrierCode)) {
               if (!this.AgentSideData.Transshipment1CarrierAddedManually) {
                   this.Transshipment1CarrierId = this.ManifestSL.Transshipment1CarrierId;
               }
            } else this.IsHideTransshipment1Carrier = true;


           if (!AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment1VesselCode) && this.EntityPM.TransportModeId == "O") {

               if (!this.AgentSideData.Transshipment1VesselAddedManually) {
                   this.Transshipment1VesselId = this.ManifestSL.Transshipment1VesselId;
               }
            } else this.IsHideTransshipment1Vessel = true;


            if (this.IsHideTransshipment1Carrier && this.IsHideTransshipment1Vessel) this.IsHideRoutingLeg1 = true;
        }
        else {
            this.IsHideTransshipment1Vessel = true;
            this.IsHideTransshipment1Carrier = true;
        }


        //leg 2

        if (!this.IsHideRoutingLeg2) {

            if (!AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment2CarrierCode)) {
                if (!this.AgentSideData.Transshipment2CarrierAddedManually) {
                    this.Transshipment2CarrierId = this.ManifestSL.Transshipment2CarrierId;
                }
            } else this.IsHideTransshipment2Carrier = true;


            if (!AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment2VesselCode) && this.EntityPM.TransportModeId == "O") {

                if (!this.AgentSideData.Transshipment2VesselAddedManually) {
                    this.Transshipment2VesselId = this.ManifestSL.Transshipment2VesselId;
                }
            } else this.IsHideTransshipment2Vessel = true;


            if (this.IsHideTransshipment2Carrier && this.IsHideTransshipment2Vessel) this.IsHideRoutingLeg2 = true;
        }
        else {
            this.IsHideTransshipment2Vessel = true;
            this.IsHideTransshipment2Carrier = true;
        }




        //leg 3

        if (!this.IsHideRoutingLeg3) {

            if (!AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment3CarrierCode)) {
                if (!this.AgentSideData.Transshipment3CarrierAddedManually) {
                    this.Transshipment3CarrierId = this.ManifestSL.Transshipment3CarrierId;
                }
            } else this.IsHideTransshipment3Carrier = true;


            if (!AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment3VesselCode) && this.EntityPM.TransportModeId == "O") {

                if (!this.AgentSideData.Transshipment3VesselAddedManually) {
                    this.Transshipment3VesselId = this.ManifestSL.Transshipment3VesselId;
                }
            } else this.IsHideTransshipment3Vessel = true;


            if (this.IsHideTransshipment3Carrier && this.IsHideTransshipment3Vessel) this.IsHideRoutingLeg3 = true;
        }
        else {
            this.IsHideTransshipment3Vessel = true;
            this.IsHideTransshipment3Carrier = true;
        }


        if (this.IsHideRoutingLeg1 && this.IsHideRoutingLeg2 && this.IsHideRoutingLeg3) {
            this.IsNoTransShipmentsDetailsFound = true;
            this.HideTransShipmentsDetailsArea = true;
        }  



        if (!AppTool.IsNullOrEmpty(this.ManifestSL.MainCarriageVesselId) && !this.AgentSideData.MainCarriageVesselAddedManually) this.MainCarriageVesselId = this.ManifestSL.MainCarriageVesselId;
  
        if (!AppTool.IsNullOrEmpty(this.ManifestSL.InterlineId) && !this.AgentSideData.InterlineAddedManually) this.InterlineId = this.ManifestSL.InterlineId;
         




        // Shipper Translation
        if (this.AgentSideData.Shipper && !this.IsConsolShipment) {
            var ShipperTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.Shipper.Code && f.ObjectTableName == "ShipperCard")[0];
            if (ShipperTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(ShipperTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
            
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
        
                            this.ShipperId = myResult;
                        }
                    }


                    this.IsLoadedShipperTranslation = true;
                    this.StopBusyIndicator();


                });

            } else this.IsLoadedShipperTranslation = true;
        } else this.IsLoadedShipperTranslation = true;


        if (this.AgentSideData.Consignee && !this.IsConsolShipment) {
            var consigneeTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.Consignee.Code && f.ObjectTableName == "ConsigneeCard")[0];
            if (consigneeTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(consigneeTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
         
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.ConsigneeId = myResult;
                        }
                    }

                    this.IsLoadedConsigneeTranslation = true;
                    this.StopBusyIndicator();
                    

                });

            } else this.IsLoadedConsigneeTranslation = true;
        } else this.IsLoadedConsigneeTranslation = true;


        //Shipper DefaultValues

        if (this.AgentSideData.Shipper && !this.IsConsolShipment) {

            var englishName = !AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.EnglishName) ? this.AgentSideData.Shipper.EnglishName : "";
            var Address1 = !AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.Address1) ? this.AgentSideData.Shipper.Address1 : "";
            var Address2 = !AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.Address2) ? this.AgentSideData.Shipper.Address2 : "";
            var city = !AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.City) ? this.AgentSideData.Shipper.City : "";

            this.ShiperDefaultValues = englishName + "^";
            this.ShiperDefaultValues += (Address1 + "^");
            this.ShiperDefaultValues += (Address2 + "^");
            this.ShiperDefaultValues += (city + "^");
            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
            apiQueryFilters.addAdditionalFilter("Code", this.AgentSideData.Shipper.CountryCode, null, null, "Equals", true, true, true, "Text");
            this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
  
                if (!myResponse.HasError) {
                    var list: any[] = myResponse.Result;
                    var CountryList: any = list.filter(d => d.Code == this.AgentSideData.Shipper.CountryCode)[0];
                    if (CountryList) {
                        this.ShiperDefaultValues += (CountryList.Id);
                    }
                }
                this.IsLoadedShiperDefaultValues = true;
                this.StopBusyIndicator();
            });

        } else this.IsLoadedShiperDefaultValues = true;

        //Consignee  DefaultValues 
        if (this.AgentSideData.Consignee && !this.IsConsolShipment) {

            var englishName = !AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.EnglishName) ? this.AgentSideData.Consignee.EnglishName : "";
            var Address1 = !AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.Address1) ? this.AgentSideData.Consignee.Address1 : "";
            var Address2 = !AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.Address2) ? this.AgentSideData.Consignee.Address2 : "";
            var city = !AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.City) ? this.AgentSideData.Consignee.City : "";

            this.ConsigneeDefaultValues = englishName + "^";
            this.ConsigneeDefaultValues += (Address1 + "^");
            this.ConsigneeDefaultValues += (Address2 + "^");
            this.ConsigneeDefaultValues += (city + "^");
            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
            apiQueryFilters.addAdditionalFilter("Code", this.AgentSideData.Consignee.CountryCode, null, null, "Equals", true, true, true, "Text");
            this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
          
                if (!myResponse.HasError) {
                    var list: any[] = myResponse.Result;
                    var CountryList: any = list.filter(d => d.Code == this.AgentSideData.Consignee.CountryCode)[0];
                    if (CountryList) {
                        this.ConsigneeDefaultValues += (CountryList.Id);
                    }
                }

                this.IsLoadedConsigneeDefaultValues = true;
                this.StopBusyIndicator();
                
            });

        } else this.IsLoadedConsigneeDefaultValues = true;



        // Incoterm Translation

        if (!this.IsHideIncoterm && this.AgentSideData.IncotermAddedManually) {
            var incotermTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.IncotermCode && f.ObjectTableName == "Incoterm")[0];
            if (incotermTranslation) {
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", incotermTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myIncotermListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: IncotermList[] = myResponse.Result;
                        var IncotermList: IncotermList = list.filter(d => d.Code == incotermTranslation.MyCode && !d.InActive)[0];
                        if (IncotermList) this.IncotermId = IncotermList.Id;

                    }

                    this.FreightPrepaidCollectId = this.AgentSideData.FreightPrepaidCollectId;
                    this.OtherPrepaidCollectId = this.AgentSideData.OtherPrepaidCollectId;



                    this.IsLoadedIncotermTranslation = true;
                    this.StopBusyIndicator();
                });
            }
            else {
                this.FreightPrepaidCollectId = this.AgentSideData.FreightPrepaidCollectId;
                this.OtherPrepaidCollectId = this.AgentSideData.OtherPrepaidCollectId;
                this.IsLoadedIncotermTranslation = true;
            }
        }
        else {
            this.FreightPrepaidCollectId = this.AgentSideData.FreightPrepaidCollectId;
            this.OtherPrepaidCollectId = this.AgentSideData.OtherPrepaidCollectId;
            this.IsLoadedIncotermTranslation = true;
        }



        // Move Type Translation
        if (!this.IsHideMoveType && this.AgentSideData.MoveTypeAddedManually) {
            var moveTypeTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.MoveTypeCode && f.ObjectTableName == "MoveType")[0];
            if (moveTypeTranslation) {
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", moveTypeTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myMoveTypeListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: MoveTypeList[] = myResponse.Result;
                        var MoveTypeList: MoveTypeList = list.filter(d => d.Code == moveTypeTranslation.MyCode && !d.InActive)[0];
                        if (MoveTypeList) this.MoveTypeId = MoveTypeList.Id;
                    }

                    this.IsLoadedMoveTypeTranslation = true;
                    this.StopBusyIndicator();
                });
            }
            else {

                this.IsLoadedMoveTypeTranslation = true;
            }
        }
        else {

            this.IsLoadedMoveTypeTranslation = true;
        }

        //ValueOfGoodsCurrency Translation
        if (!this.IsHideValueOfGoodsCurrency && this.AgentSideData.ValueOfGoodsCurrencyAddedManually) {
            var valueOfGoodsCurrencyCodeTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.ValueOfGoodsCurrencyCode && f.ObjectTableName == "ValueOfGoodsCurrency")[0];
            if (valueOfGoodsCurrencyCodeTranslation) {
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", valueOfGoodsCurrencyCodeTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myCurrencyListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CurrencyList[] = myResponse.Result;
                        var currencyList: CurrencyList = list.filter(d => d.Code == valueOfGoodsCurrencyCodeTranslation.MyCode && !d.InActive)[0];
                        if (currencyList) this.ValueOfGoodsCurrencyId = currencyList.Id;
                    }

                    this.IsLoadedValueOfGoodsCurrencyTranslation = true;
                    this.StopBusyIndicator();
                });
            }
            else {

                this.IsLoadedValueOfGoodsCurrencyTranslation = true;
            }
        }
        else {

            this.IsLoadedValueOfGoodsCurrencyTranslation = true;
        }


      //_______________________________ Carrier Translation  Start ____________________________________________

        //1 MainCarriageCarrier
        if (!this.IsHideCarrier && this.AgentSideData.CarrierAddedManually) {
           
            var carrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.CarrierCode && f.ObjectTableName == "Carrier")[0];
            if (carrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(carrierTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
         
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.MainCarriageCarrierId = myResult;
                        }
                    }
                    this.IsLoadedCarrierTranslation = true;
                    this.StopBusyIndicator();

                });


            } else this.IsLoadedCarrierTranslation = true;
        } else this.IsLoadedCarrierTranslation = true;

        // 2 shipment1Carrier
        if (!this.IsHideTransshipment1Carrier && this.AgentSideData.Transshipment1CarrierAddedManually) {

            var shipment1CarrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.Transshipment1CarrierCode && f.ObjectTableName == "Transshipment1Carrier")[0];
            if (shipment1CarrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(shipment1CarrierTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
          
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.Transshipment1CarrierId = myResult;
                        }
                    }
                    this.IsLoadedTransshipment1CarrierTranslation = true;
                    this.StopBusyIndicator();

                });


            } else this.IsLoadedTransshipment1CarrierTranslation = true;
        } else this.IsLoadedTransshipment1CarrierTranslation = true;

        // 3 shipmen2Carrier
        if (!this.IsHideTransshipment2Carrier && this.AgentSideData.Transshipment2CarrierAddedManually) {

            var transshipment2CarrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.Transshipment2CarrierCode && f.ObjectTableName == "Transshipment2Carrier")[0];
            if (transshipment2CarrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(transshipment2CarrierTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
              
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.Transshipment2CarrierId = myResult;
                        }
                    }
                    this.IsLoadedTransshipment2CarrierTranslation = true;
                    this.StopBusyIndicator();

                });


            } else this.IsLoadedTransshipment2CarrierTranslation = true;
        } else this.IsLoadedTransshipment2CarrierTranslation = true;

          // 4 shipmen3Carrier
        if (!this.IsHideTransshipment3Carrier && this.AgentSideData.Transshipment3CarrierAddedManually) {

            var transshipment3CarrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.Transshipment3CarrierCode && f.ObjectTableName == "Transshipment3Carrier")[0];
            if (transshipment3CarrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(transshipment3CarrierTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;

                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.Transshipment3CarrierId = myResult;
                        }
                    }
                    this.IsLoadedTransshipment3CarrierTranslation = true;
                    this.StopBusyIndicator();

                });


            } else this.IsLoadedTransshipment3CarrierTranslation = true;
        } else this.IsLoadedTransshipment3CarrierTranslation = true;



       //_______________________________ End ____________________________________________


      
        //_______________________________ Vessel Translation  Start ____________________________________________

       // 1 MainCarriageVessel 
        if (!this.IsHideMainCarriageVessel && this.AgentSideData.MainCarriageVesselAddedManually) {

            var mainCarriageVesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.MainCarriageVesselCode && f.ObjectTableName == "MainCarriageVessel")[0];
            if (mainCarriageVesselTranslation) {
         
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", mainCarriageVesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");

                this.myVesselListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
   
                    if (!myResponse.HasError) {
                        var list: VesselList[] = myResponse.Result;
                        var VesselList: VesselList = list.filter(d => d.Code == mainCarriageVesselTranslation.MyCode && !d.InActive)[0];
                        if (VesselList) {
                            this.MainCarriageVesselId = VesselList.Id;
                        }

                    }
                    this.IsLoadedMainCarriageVesselTranslation = true;
                    this.StopBusyIndicator();


                });


            } else this.IsLoadedMainCarriageVesselTranslation = true;
        } else this.IsLoadedMainCarriageVesselTranslation = true;

        // 2 Transshipment1Vessel 
        if (!this.IsHideTransshipment1Vessel && this.AgentSideData.Transshipment1VesselAddedManually) {

            var transshipment1VesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.Transshipment1VesselCode && f.ObjectTableName == "Transshipment1Vessel")[0];
            if (transshipment1VesselTranslation) {
                var myService: VesselListService = new VesselListService();
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", transshipment1VesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");

                myService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {

                    if (!myResponse.HasError) {
                        var list: VesselList[] = myResponse.Result;
                        var VesselList: VesselList = list.filter(d => d.Code == transshipment1VesselTranslation.MyCode && !d.InActive)[0];
                        if (VesselList) {
                            this.Transshipment1VesselId = VesselList.Id;
                        }

                    }

                    this.IsLoadedTransshipment1VesselTranslation = true;
                    this.StopBusyIndicator();


                });

            } else this.IsLoadedTransshipment1VesselTranslation = true;
        } else this.IsLoadedTransshipment1VesselTranslation = true;

       // 3 Transshipment2Vessel 
        if (!this.IsHideTransshipment2Vessel && this.AgentSideData.Transshipment2VesselAddedManually ) {

            var transshipment2VesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.Transshipment2VesselCode && f.ObjectTableName == "Transshipment2Vessel")[0];
            if (transshipment2VesselTranslation) {
                var myService: VesselListService = new VesselListService();
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", transshipment2VesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");

                myService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList[] = myResponse.Result;
                        var VesselList: VesselList = list.filter(d => d.Code == transshipment2VesselTranslation.MyCode && !d.InActive)[0];
                        if (VesselList) {
                            this.Transshipment2VesselId = VesselList.Id;
                        }

                    }

                    this.IsLoadedTransshipment2VesselTranslation = true;
                    this.StopBusyIndicator();

       
                });


            } else this.IsLoadedTransshipment2VesselTranslation = true;
        } else this.IsLoadedTransshipment2VesselTranslation = true;


        // 4 Transshipment3Vessel 
        if (!this.IsHideTransshipment3Vessel && this.AgentSideData.Transshipment3VesselAddedManually) {

            var transshipment3VesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.Transshipment3VesselCode && f.ObjectTableName == "Transshipment3Vessel")[0];
            if (transshipment3VesselTranslation) {
                var myService: VesselListService = new VesselListService();
                var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", transshipment3VesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
    
                myService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList[] = myResponse.Result;
                        var VesselList: VesselList = list.filter(d => d.Code == transshipment3VesselTranslation.MyCode && !d.InActive)[0];
                        if (VesselList) {
                            this.Transshipment3VesselId = VesselList.Id;
                        }

                    }
                    this.IsLoadedTransshipment3VesselTranslation = true;
                    this.StopBusyIndicator();
                });


            } else this.IsLoadedTransshipment3VesselTranslation = true;
        } else this.IsLoadedTransshipment3VesselTranslation = true;

        //_______________________________ End ____________________________________________



        // Interline Translation
        if (!this.IsHideMainInterline && this.AgentSideData.InterlineAddedManually) {

            if (this.ManifestSL) {
                var MainCarriageInterlineTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.ManifestSL.InterlineCode && f.ObjectTableName == "MainCarriageInterline")[0];
                if (MainCarriageInterlineTranslation) {

                    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                    this.myAirlineListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
               
                        if (!myResponse.HasError) {
                            var list: AirlineList[] = myResponse.Result;
                            var airlineList: AirlineList = list.filter(d => d.Code == MainCarriageInterlineTranslation.MyCode && !d.InActive)[0];
                            if (airlineList) this.InterlineId = airlineList.Id;

                        }
              
                        this.IsLoadedMainCarriageInterlineTranslation = true;
                        this.StopBusyIndicator();

                    });

                } else this.IsLoadedMainCarriageInterlineTranslation = true;
            } else this.IsLoadedMainCarriageInterlineTranslation = true;
        } else this.IsLoadedMainCarriageInterlineTranslation = true;

        
        // Package Translation

        if (this.AgentSideData.PackageTypes && this.AgentSideData.PackageTypes.length > 0) {
          
            this.myPackageTypeService.getAllFromCache().subscribe((resp: any) => {
                if (!resp.HasError) {
                    this.AllPackageTypes = resp.Result;
                }

     

                this.AgentSideData.PackageTypes.forEach((item) => {
                    var packageTypePM: PackageTypePM = new PackageTypePM();
                    packageTypePM.ComputedLocalName = item.Code;
                    packageTypePM.Code = item.Code;
                    packageTypePM.EnglishName = item.EnglishName;
                    packageTypePM.IsContainer = item.IsContainer;
                    packageTypePM.Id = item.Id;


                    if (item.AddedManually) {
                        var packageitem = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == item.Code && f.ObjectTableName == "Package")[0];
                        if (packageitem) {
                            if (this.AllPackageTypes) {
                                var packageTranslate: PackageTypeList = this.AllPackageTypes.filter(d => d.Code == packageitem.MyCode && !d.InActive)[0];
                                if (packageTranslate) {
                                    packageTypePM.Id = packageTranslate.Id;
                                    packageTypePM.Code = packageTranslate.Code;
                                    packageTypePM.EnglishName = packageTranslate.EnglishName;
                                    packageTypePM.IsContainer = packageTranslate.IsContainer;
                                }
                            }
                        }
                    }

                    this.OurAgentPackagesType.push(packageTypePM);

                });


                if (!this.OurAgentPackagesType.filter(d => AppTool.IsNullOrEmpty(d.Id))[0]) {
                    this.HidePackageTypeArea = true;
                }

                this.IsLoadedPackageTranslation = true;
                this.StopBusyIndicator();

            });



        }
        else {

            this.HidePackageTypeArea = true;
            this.IsNoPackagesFound = true;
            this.IsLoadedPackageTranslation = true;

        }


        //OtherPartner Translation

        if (this.AgentSideData.Notify1 == null) {
            this.HideOtherPartnersArea = true;
            this.IsNoNoOtherPartnersFound = true;
            this.IsLoadedNotify1IdTranslation = true;
            this.IsLoadedNotify1DefaultValuesTranslation = true;
  
        }
        else {
            // Notify1 Translation
            if (this.AgentSideData.Notify1) {

                if (!AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Code)) {

                    var notify1Translation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == this.AgentSideData.Notify1.Code && f.ObjectTableName == "Notify1Card")[0];
                    if (notify1Translation) {

                        this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(notify1Translation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                            var pmResponse: ServiceResponse = res;
 
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {

                                    this.Notify1Id = myResult;
                                    this.HideOtherPartnersArea = true;

                                }
                            }
                   
                            this.IsLoadedNotify1IdTranslation = true;
                            this.StopBusyIndicator();

                        });

                    } else this.IsLoadedNotify1IdTranslation = true;
                    

                    var englishName = !AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.EnglishName) ? this.AgentSideData.Notify1.EnglishName : "";
                    var Address1 = !AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Address1) ? this.AgentSideData.Notify1.Address1 : "";
                    var Address2 = !AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Address2) ? this.AgentSideData.Notify1.Address2 : "";
                    var city = !AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.City) ? this.AgentSideData.Notify1.City : "";

                    this.Notify1DefaultValues = englishName + "^";
                    this.Notify1DefaultValues += (Address1 + "^");
                    this.Notify1DefaultValues += (Address2 + "^");
                    this.Notify1DefaultValues += (city + "^");


                    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                    apiQueryFilters.addAdditionalFilter("Code", this.AgentSideData.Notify1.CountryCode, null, null, "Equals", true, true, true, "Text");
                    this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: any[] = myResponse.Result;
                            var CountryList: any = list.filter(d => d.Code == this.AgentSideData.Notify1.CountryCode && !d.InActive)[0];
                            if (CountryList) {
                                this.Notify1DefaultValues += (CountryList.Id);
                            }
                        }
        
                        this.IsLoadedNotify1DefaultValuesTranslation = true;
                        this.StopBusyIndicator();
                    });

                }
                else {
                    this.HideOtherPartnersArea = true;
                    this.IsLoadedNotify1IdTranslation = true;
                    this.IsLoadedNotify1DefaultValuesTranslation = true;
                }
            }


        }
     

        if (this.IsHideCarrier &&  this.IsHideMainCarriageVessel && this.IsHideMainInterline && this.IsConsolShipment) {
            this.IsNoAgentDataFound = true;
            this.HideAgentSideDataArea = true;
            this.HideOurSideDataArea = true;
        }


        if (this.IsHideIncoterm && this.IsHideMoveType && this.IsHideIsDangerous && this.IsHideDescriptionOfGoods && this.IsHideValueOfGoods && this.IsHideValueOfGoodsCurrency && this.IsHideMainHarmonize) {
            this.IsNoGeneralDetailsFound = true;
            this.HideGeneralDetailsArea = true;
        } 


        this.BuildOurPickUpDeliverySide("PICK");
        this.BuildOurPickUpDeliverySide("DELV");
        
        this.ComputeHeightAgentDataTransLationArea();



        this.StopBusyIndicator();

    }

    
    HeightAgentDataTransLationArea: string = "335px";
    ComputeHeightAgentDataTransLationArea() {

        var height:number = 335;
        if (this.IsHideCarrier) height -=20;
        if (this.IsHideMainCarriageVessel && this.IsHideMainInterline) height -= 20;

        this.HeightAgentDataTransLationArea = height.toString() + "px";

    }


    IsLoadAdditionalScreen: boolean = false;
    IsLoadSharedManifestheaderScreen: boolean = false;


 

    //BuildAdditionalFields() {
    //    //SharedManifestAdditionalScreen
    //    var objecttable = window.ObjectTables.filter(t => t.Name === "Shipment")[0];
    //    var myScreen = window.Screens.filter((x: any) => x.ObjectTableId === objecttable.Id && x.Code == "SharedManifestAdditionalScreen" )[0];
    //    if (myScreen) {
    //        if (window.ScreenFields.filter(s => s.ScreenId === myScreen.Id).length > 0) {
    //            this.IsLoadAdditionalScreen = true;
    //            this.RunComponent();

    //        }
    //        else {
    //            this.IsNoAddtionalFound = true;
    //            this.HideAddtionalArea = true;
    //        }
    //    }
    //}

    SetDataOnFinish() {
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;
        this.SetPartnersOnFinish();
        this.SetCountryECOnFinish();


    }

    SetPartnersOnFinish() {
        if (this.IsConsolShipment) {
            this.EntityPM.AgentId = this.ShipperId;
            this.EntityPM.AgentName = this.EntityPM.ShipperName;
            this.EntityPM.AgentAddressId = this.ShipperAddressId;
            this.EntityPM.AgentContactId = this.ShipperContactId;

            if (this.ManifestSL) {
                 this.EntityPM.ShipperReference1 = this.ManifestSL.AgentReference1;
                  this.EntityPM.ShipperReference2 = this.ManifestSL.AgentReference2;
            }

        }
        else if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {

            this.EntityPM.CustomerId = null;
            this.EntityPM.CustomerName = null;
            this.EntityPM.CustomerNote = null;
            this.EntityPM.CustomerAddressId = null;
            this.EntityPM.CustomerContactId = null;
            this.EntityPM.CustomerReference1 = null;
            this.EntityPM.CustomerReference2 = null;
            this.EntityPM.ShipmentCustomerTypeCode = null;
            this.EntityPM.ShipmentCustomerTypeCode = "CON";
            this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
            this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
            this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
            this.EntityPM.CustomerAddressId = this.EntityPM.ConsigneeAddressId;
            this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
            this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
            this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;

            if (AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
                this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.myConsigneeSalesmanId) ? this.EntityPM.CreatedByUserId : this.myConsigneeSalesmanId;
            }

        }


        if (this.CurrentEntity) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
                this.EntityPM.AgentReference1 = this.CurrentEntity.AgentReference;
            }
        }



    }

    SetCountryECOnFinish() {
        if (this.FromPortList != null) {
            this.EntityPM.FromCountryId = this.FromPortList.CountryId;
            this.EntityPM.FromCountryIsEC = this.FromPortList.CountryEC;
        }

        if (this.ToPortList != null) {
            this.EntityPM.ToCountryId = this.ToPortList.CountryId;
            this.EntityPM.ToCountryIsEC = this.ToPortList.CountryEC;
        }
    }




    AddPartnerClicked(myPartnerCode: string) {

        var args = new NewEntityArgs();
        var logeWindow = new LogitudeWindow();
        var pathComponent = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";

      
            if (myPartnerCode == "C") {

                args.DefaultValues = this.ConsigneeDefaultValues;
                logeWindow.Title = "New Consignee";
                logeWindow.ComponentLoaded.subscribe(comp => {
                    logeWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.ConsigneeId = comp.EntityPM.Id;
                            this.ConsigneeName = comp.EntityPM.EnglishName;
                            this.myConsigneeSalesmanId = comp.EntityPM.SalesmanUserId;
                        }
                    });

                });


            }
            else if (myPartnerCode == "Notify1") {

                args.DefaultValues = this.Notify1DefaultValues;
                logeWindow.Title = "New Notify 1";
                logeWindow.ComponentLoaded.subscribe(comp => {
                    logeWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.Notify1Id = comp.EntityPM.Id;
                        }
                    });

                });


            }
            else {
                args.DefaultValues = this.ShiperDefaultValues;
                logeWindow.Title = "New Shipper";


                logeWindow.ComponentLoaded.subscribe(comp => {
                    logeWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.ShipperId = comp.EntityPM.Id;
                            this.myShipperSalesmanId = comp.EntityPM.SalesmanUserId;
                        }
                    });

                });

            }

            logeWindow.Width = 960;
            logeWindow.Height = 600;
            logeWindow.WindowArgs = args;
            logeWindow.Show(pathComponent);



    
      
    }

    AddShipmentPickUpDeliveryPartnerClicked(tableName: string, code: string, entityName:string) {
        var objectTable: any = window.ObjectTables.filter(d => d.Name.toLowerCase() === tableName.toLocaleLowerCase())[0];
        if (objectTable.IsNewWizard) {
            this.RunNewEntityWizard(objectTable.NewWizardComponentPath, objectTable.Name, code, entityName);
        }
    }






    private RunNewEntityWizard(newWizardComponentPath: string, originalTableName: string, code: string, entityName:string) {

        var componentPath: string = newWizardComponentPath;

        if (componentPath != null) {
            var ourSideShipmentPickUpDelivery: any = entityName == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;

            if (ourSideShipmentPickUpDelivery) {
                var args = new NewEntityArgs();
                args.DefaultValues = code == "F" ? ourSideShipmentPickUpDelivery.FromPartnerDefaultValues : ourSideShipmentPickUpDelivery.ToPartnerDefaultValues;
                logWindow.WindowArgs = args;
            }
  
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(originalTableName));
            logWindow.Title = windowTitle;
      
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event, code, entityName));
            logWindow.Show(componentPath);
        }

       
    }

    private OnNewEntityWindowClosed($event: any, code: string, entity: string) {
        console.log($event);
        if ($event && $event != "event") {
            var ourSideShipmentPickUpDelivery: any = entity == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
            if (ourSideShipmentPickUpDelivery) {
                if (code == "F") ourSideShipmentPickUpDelivery.FromPartnerCardId = $event;
                else ourSideShipmentPickUpDelivery.ToPartnerCardId = $event;
            }

        }
    }





    SelectCityCommand(code: string, entity: string ) {

        var mySourceCountryId: string;
        var ourSideShipmentPickUpDelivery: any = entity == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
        if (ourSideShipmentPickUpDelivery) {
            if (code == "F") mySourceCountryId = ourSideShipmentPickUpDelivery.FromAddressCountryId;
            else mySourceCountryId = ourSideShipmentPickUpDelivery.ToAddressCountryId;

        }


        var args = new CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {
                if (code == "F") {
                    ourSideShipmentPickUpDelivery.FromAddressCity = args.CityName;
                    ourSideShipmentPickUpDelivery.FromAddressCountryId = args.CountryId;

                } else{

                    ourSideShipmentPickUpDelivery.ToAddressCity = args.CityName;
                    ourSideShipmentPickUpDelivery.ToAddressCountryId = args.CountryId;
               
                }

            }
        });
    }


    AddCountryClick(code: string , entity:string) {
        var countryCode: string;
        var countryName: string;

        var shipmentPickUpDelivery: any = entity == "Pickup" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;
        if (shipmentPickUpDelivery) {
            if (code == "F") {
                countryCode = shipmentPickUpDelivery.FromAddressCountryCode;
                countryName = shipmentPickUpDelivery.FromAddressCountryName;
            } else {
                countryCode = shipmentPickUpDelivery.ToAddressCountryCode;
                countryName = shipmentPickUpDelivery.ToAddressCountryName;
            }

        }
            var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
            this.entityPMService.getNewEntity("Country").then(response => {
              
                var args = new EntityArgs();
                args.EntityPM = response;
                args.EntityPM.Code = countryCode;
                args.EntityPM.EnglishName = countryName;
                args.ObjectTableName = "Country";
                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("Country"));
                logWindow.WindowArgs = args;
                logWindow.Title = windowTitle;
                logWindow.Show(componentPath);

                logWindow.WindowClosed.subscribe($event => {
                    if ($event) {
                        var filters = new ApiQueryFilters();
                        filters.Tenant = SessionInfo.LoggedUserTenant;
                        filters.GetAll = true;
                        filters.ForceCacheRefresh = true;
                        var entityListService = new EntityListService();
                        entityListService.getAllFromCache("Country", filters).then((res: any) => {
                            res.subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError && myResponse.Result) {
                                    var country = myResponse.Result.filter(d => d.Id == $event)[0];
                                    if (country) {

                                        var ourSideShipmentPickUpDelivery: any = entity == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
                                        if (ourSideShipmentPickUpDelivery) {
                                            if (code == "F") {
                                                ourSideShipmentPickUpDelivery.FromAddressCountryCode = country.Code;
                                                ourSideShipmentPickUpDelivery.FromAddressCountryName = country.EnglishName;
                                                ourSideShipmentPickUpDelivery.FromAddressCountryId = country.Id;
                                                
                                            } else {
                                                ourSideShipmentPickUpDelivery.ToAddressCountryCode = country.Code;
                                                ourSideShipmentPickUpDelivery.ToAddressCountryName = country.EnglishName;
                                                ourSideShipmentPickUpDelivery.ToAddressCountryId = country.Id;
                                            }
                                        }
                                    }
                                }

                            });


                        });
                    }
                });


            });
      
    }






    CreateButtonClicked() {
        this.OnCreate();
        // this.CurrentSession.CloseCurrentWindow();
    }

    //-------------------------------------------------------------------------------

    IsRefeshPackageType: boolean = false;
    AddPackageType(item: PackageTypePM) {
    
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("PackageType").then(response => {
            var packagetype = this.AgentSideData.PackageTypes.filter(d => d.Code == item.ComputedLocalName)[0];
            var args = new EntityArgs();
            args.EntityPM = response;

            if (packagetype) {

                args.EntityPM.Code = packagetype.Code;
                args.EntityPM.EnglishName = packagetype.EnglishName;
                args.EntityPM.IsContainer = packagetype.IsContainer;
                args.EntityPM.IsAir = packagetype.IsAir;
                args.EntityPM.IsOcean = packagetype.IsOcean;
                args.EntityPM.IsInland = packagetype.IsInland;
                args.EntityPM.LocalName = packagetype.LocalName;
                args.EntityPM.Notes = packagetype.Notes;
                args.EntityPM.AddedManually = true;

                if (packagetype.Volume && packagetype.Volume > 0) args.EntityPM.Volume = packagetype.Volume;
                if (packagetype.TEU && packagetype.TEU > 0) args.EntityPM.TEU = packagetype.TEU;
                if (packagetype.ContainerSize && packagetype.ContainerSize > 0) args.EntityPM.ContainerSize = packagetype.ContainerSize;
          
            }


            args.ObjectTableName ="PackageType";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("PackageType"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);

            logWindow.WindowClosed.subscribe($event => {
                if ($event) {
                    var filters = new ApiQueryFilters();
                    filters.Tenant = SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService();
                    entityListService.getAllFromCache("PackageType", filters).then((res: any) => {
                        res.subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError && myResponse.Result) {
                                var packageType = myResponse.Result.filter(d => d.Id == $event)[0];
                                if (packageType) {

                                    item.Id = packageType.Id;
                                    item.Code = packageType.Code;
                                    item.EnglishName = packageType.EnglishName;
                                   // this.IsRefeshPackageType = !this.IsRefeshPackageType;
                                }
                            }
                      
                        });


                    });
                }
            });


        });
    }
  
    AddVesselClcik(fieldName:string) {

        var defultCode = "";
        var defultName = "";
        if (fieldName == "Transshipment1Vessel") {
            defultCode = this.ManifestSL.Transshipment1VesselCode;
            defultName = this.ManifestSL.Transshipment1VesselName;
        } else if (fieldName == "Transshipment2Vessel") {
            defultCode = this.ManifestSL.Transshipment2VesselCode;
            defultName = this.ManifestSL.Transshipment2VesselName;
        } else if (fieldName == "Transshipment3Vessel") {
            defultCode = this.ManifestSL.Transshipment3VesselCode;
            defultName = this.ManifestSL.Transshipment3VesselName;
        }

        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("Vessel").then(response => {
            var args = new EntityArgs();
            args.EntityPM = response;
            args.EntityPM.EnglishName = args.EntityPM.LocalName = defultName;
            args.EntityPM.Code = defultCode;

            args.ObjectTableName = "Vessel";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("Vessel"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);

            logWindow.WindowClosed.subscribe($event => {
                if ($event) {
                    var filters = new ApiQueryFilters();
                    filters.Tenant = SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService();
                    entityListService.getAllFromCache("Vessel", filters).then((res: any) => {
                        res.subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError && myResponse.Result) {
                                var vessel = myResponse.Result.filter(d => d.Id == $event)[0];
                                if (vessel) {
                                    if (fieldName == "Transshipment1Vessel") {
                                        this.Transshipment1VesselId = vessel.Id;
                                    } else if (fieldName == "Transshipment2Vessel") {
                                        this.Transshipment2VesselId = vessel.Id;
                                    } else if (fieldName == "Transshipment3Vessel") {
                                        this.Transshipment3VesselId = vessel.Id;
                                    }
                                }
                            }
                        });
                    });


                  
                }
            });

        });

    }

    AddCarrierClcik(fieldName: string) {
        var objecttableName = "";

        var defultCode = "";
        var defultName = "";
        if (fieldName == "Transshipment1Carrier") {
            defultCode = this.ManifestSL.Transshipment1CarrierCode;
            defultName = this.ManifestSL.Transshipment1CarrierName;
        } else if (fieldName == "Transshipment2Carrier") {
            defultCode = this.ManifestSL.Transshipment2CarrierCode;
            defultName = this.ManifestSL.Transshipment2CarrierName;
        } else if (fieldName == "Transshipment3Carrier") {
            defultCode = this.ManifestSL.Transshipment3CarrierCode;
            defultName = this.ManifestSL.Transshipment3CarrierName;
        }
        switch (this.ManifestSL.TransportModeId) {
            case "A": { objecttableName = "Airline"; break; }
            case "O": { objecttableName = "ShippingLine"; break; }
            case "I": { objecttableName = "Trucker"; break; }

        }
      
        var defaultValues = defultCode + "^" + defultName;
        var path: string = "./Common/Components/Partners/NewEntity/New" + objecttableName + "Component";

        var title = "Add " + objecttableName;
        if (objecttableName == "ShippingLine") {
            title = "New Shipping Line";
        }

        this._entityResourceService.getEntityResourceByTableName(objecttableName, 0).subscribe(response => {

            var windowArgs: any = {};
            windowArgs.DefaultValues = defaultValues;
            windowArgs.RequestPage = "SharedManifest"
            var windowTitle = title;//"New Shipping Line";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe($event => {
                if ($event) this.RefrshCarrier(fieldName, $event, objecttableName);
            });

        });
    }
    RefrshCarrier(fieldName: string, value: string, objectTableName: string) {
        var filters = new ApiQueryFilters();
        filters.Tenant = SessionInfo.LoggedUserTenant;
        filters.GetAll = true;
        filters.ForceCacheRefresh = true;
        var entityListService = new EntityListService();
        entityListService.getAllFromCache(objectTableName, filters).then((res: any) => {
            res.subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError && myResponse.Result) {
                    var Carrier = myResponse.Result.filter(d => d.Id == value)[0];
                    if (Carrier) {
                        if (fieldName == "Transshipment1Carrier") {
                            this.Transshipment1CarrierId = Carrier.Id;
                        } else if (fieldName == "Transshipment2Carrier") {
                            this.Transshipment2CarrierId = Carrier.Id;
                        } else if (fieldName == "Transshipment3Carrier") {
                            this.Transshipment3CarrierId = Carrier.Id;
                        }
                    }
                }
            });
        });


    }


    AddEntityClcik(objectTableName:string) {

        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(objectTableName).then(response => {
            var args = new EntityArgs();
            args.EntityPM = response;

            if (objectTableName == "Incoterm") {
                args.EntityPM.Code =  this.AgentSideData.IncotermCode;
                args.EntityPM.Name =  this.AgentSideData.IncotermName;
            }
            else if (objectTableName == "MoveType") {
                args.EntityPM.Code = this.AgentSideData.MoveTypeCode;
                args.EntityPM.MoveTypeEnglishName = args.EntityPM.MoveTypeLocalName = this.AgentSideData.MoveTypeName;
                args.EntityPM.TransportModeId = this.AgentSideData.MoveTypeTransportModeId;
                if (!AppTool.IsNullOrEmpty(args.EntityPM.TransportModeId)) {
                    if (args.EntityPM.TransportModeId == "O") args.EntityPM.IsOcean = true;
                    else if (args.EntityPM.TransportModeId == "I") args.EntityPM.IsInland = true;
                    else if (args.EntityPM.TransportModeId == "A") args.EntityPM.IsAir = true;
                }
            }

          

            args.ObjectTableName = objectTableName;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(objectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);

            logWindow.WindowClosed.subscribe($event => {
                if ($event) {
                    var filters = new ApiQueryFilters();
                    filters.Tenant = SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService();
                    entityListService.getAllFromCache(objectTableName, filters).then((res: any) => {
                        res.subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError && myResponse.Result) {
                                var entity = myResponse.Result.filter(d => d.Id == $event)[0];
                                if (entity) {

                                    if (objectTableName == "Incoterm") {
                                        this.IncotermCode = entity.Code;
                                        this.IncotermId = entity.Id;
                                    }
                                    else if (objectTableName == "MoveType") {

                                        this.MoveTypeCode = entity.Code;
                                        this.MoveTypeId = entity.Id;

                                    } 
                                }


                            }
                        });
                    });



                }
            });

        });

    }


    AddValueOfGoodsCurrencyClcik() {
        var componentPath = "./Common/Components/Maintenance/Currency/NewCurrencyComponent";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("Currency"));
        logWindow.Title = windowTitle;
        logWindow.Show(componentPath);
        logWindow.WindowClosed.subscribe($event => {
            if ($event) {
                var filters = new ApiQueryFilters();
                filters.Tenant = SessionInfo.LoggedUserTenant;
                filters.GetAll = true;
                filters.ForceCacheRefresh = true;
                var entityListService = new EntityListService();
                entityListService.getAllFromCache("Currency", filters).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError && myResponse.Result) {
                            var entity = myResponse.Result.filter(d => d.Id == $event)[0];
                            if (entity) {
                                this.ValueOfGoodsCurrencyCode = entity.Code;
                                this.ValueOfGoodsCurrencyId = entity.Id;
                            }

                        }
                    });
                });



            }
        });

    }

    PackageTypeValueChange(value: any, item: PackageTypePM) {
        if (value) {
            item.Id = value.Id;
            item.Code = value.Code;
            item.EnglishName = value.EnglishName;
            //item.IsContainer = value.IsContainer;
        } else {
            item.Id = "";
            item.Code =  "";
            item.EnglishName = "";

        }

 
    }



    ButtonHideOtherPartnersAreaClicked() {

        this.HideOtherPartnersArea = !this.HideOtherPartnersArea;
    }


    imgHidePickupDetailsAreaClicked() {
        this.HidePickupDetailsArea = !this.HidePickupDetailsArea;
    }

    imgHideDeliveryDetailsAreaClicked() {
        this.HideDeliveryDetailsArea = !this.HideDeliveryDetailsArea;
    }


    


    imgHideGeneralDetailsAreaClicked() {
        this.HideGeneralDetailsArea = !this.HideGeneralDetailsArea;
    }

    imgHidePackageTypeAreaClicked()
    {

        this.HidePackageTypeArea = !this.HidePackageTypeArea;

    }

    imgHideAddtionalClicked()
    {
        this.HideAddtionalArea = !this.HideAddtionalArea;
    }


    imgHideAgentSideDataAreaClicked() {
        this.HideAgentSideDataArea = !this.HideAgentSideDataArea;
    }


    imgHideOurSideDataAreaClicked() {
        this.HideOurSideDataArea = !this.HideOurSideDataArea;
    }


    ButtonHideTransShipmentsDetailsAreaClicked() {
        this.HideTransShipmentsDetailsArea = !this.HideTransShipmentsDetailsArea;
    }


    public HideAddtionalArea: boolean = false;


    OnCreate() {

        this.SetDataOnFinish();
        this.CurrentSession.StartBusyIndicator("Creating...");
        var validator = new ShipmentValidator();
        this.ValidationErrorsList = validator.Validate(this.EntityPM);




        var IsValidPackageTranslation: boolean = true;
        if (!this.IsRemovePackageAreaFromScreen) {
            if (this.OurAgentPackagesType && this.OurAgentPackagesType.length > 0) {
                var item = this.OurAgentPackagesType.filter(d => AppTool.IsNullOrEmpty(d.Id))[0];
                if (item) {
                    this.ValidationErrorsList.push("Please fill shipment packages translations ");
                }
            }
        }


        if (AppTool.IsNullOrEmpty(this.ShipperId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ShipperId")));
        }

      
        if (!this.IsHideCarrier && AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId))
        {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.MainCarriageCarrierId")));
        }


        if (!this.IsHideTransshipment1Carrier && AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Transshipment1CarrierId")));
        }


        if (!this.IsHideTransshipment2Carrier && AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Transshipment2CarrierId")));
        }

        if (!this.IsHideTransshipment3Carrier && AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3CarrierId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Transshipment3CarrierId")));
        }

        if (!this.IsHideMainCarriageVessel && AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageVesselId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.MainCarriageVesselId")));
        }

        if (!this.IsHideTransshipment1Vessel && AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1VesselId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Transshipment1VesselId")));
        }

        if (!this.IsHideTransshipment2Vessel && AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2VesselId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Transshipment2VesselId")));
        }

        if (!this.IsHideTransshipment3Vessel && AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3VesselId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Transshipment3VesselId")));
        }

        if (!this.IsHideMainInterline && AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.InterlineId")));
        }

        
        

        if (!this.IsHideIncoterm && AppTool.IsNullOrEmpty(this.EntityPM.IncotermId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.IncotermId")));
        }


        if (!this.IsHideMoveType && AppTool.IsNullOrEmpty(this.EntityPM.MoveTypeId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.MoveTypeId")));
        }

        if (!this.IsHideValueOfGoodsCurrency && AppTool.IsNullOrEmpty(this.EntityPM.ValueOfGoodsCurrencyId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ValueOfGoodsCurrencyId")));
        }



        if (this.AgentSideData.Notify1 != null && !AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Code) && AppTool.IsNullOrEmpty(this.EntityPM.Notify1Id)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Notify1Id")));
        }

        //if (this.AgentSideData.Notify2 != null && !AppTool.IsNullOrEmpty(this.AgentSideData.Notify2.Code) &&  AppTool.IsNullOrEmpty(this.EntityPM.Notify2Id)) {
        //    var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //    this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Notify2Id")));
        //}

        
        if (AppTool.IsNullOrEmpty(this.ConsigneeId) && this.EntityPM.ShipmentLevelCode =="C") {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
        }


        this.AddShipmentPickUpDeliveryPM("PICK");
        this.AddShipmentPickUpDeliveryPM("DELV");

        if (this.ValidationErrorsList.length != 0) {

            this.CurrentSession.StopBusyIndicator();
            return;
        }


        this.AddShipmentPickUpDeliveryTransLation("PICK");
        this.AddShipmentPickUpDeliveryTransLation("DELV");

        //IncotermCode
        if (this.AgentSideData.IncotermCode && !this.IsHideIncoterm && this.IncotermCode && this.AgentSideData.IncotermAddedManually) {
            this.InsertAndUpdateTransLation(this.AgentSideData.IncotermCode, "Incoterm", this.IncotermCode);
        }

        //MoveTypeCode
        if (this.AgentSideData.MoveTypeCode && !this.IsHideMoveType && this.MoveTypeCode && this.AgentSideData.MoveTypeAddedManually) {
            this.InsertAndUpdateTransLation(this.AgentSideData.MoveTypeCode, "MoveType", this.MoveTypeCode);
        }

        //ValueOfGoodsCurrencyCode
        if (this.AgentSideData.ValueOfGoodsCurrencyCode && !this.IsHideValueOfGoodsCurrency && this.ValueOfGoodsCurrencyCode && this.AgentSideData.ValueOfGoodsCurrencyAddedManually) {
            this.InsertAndUpdateTransLation(this.AgentSideData.ValueOfGoodsCurrencyCode, "ValueOfGoodsCurrency", this.ValueOfGoodsCurrencyCode);
        }

        //CarrierCode
        if (this.AgentSideData.CarrierCode && !this.IsHideCarrier && this.MainCarriageCarrierCode && this.ManifestSL.CarrierAddedManually) {
            this.InsertAndUpdateTransLation(this.AgentSideData.CarrierCode, "Carrier", this.MainCarriageCarrierCode);

        }

        //Transshipment1CarrierCode
        if (this.ManifestSL.Transshipment1CarrierCode && !this.IsHideTransshipment1Carrier && this.Transshipment1CarrierCode && this.AgentSideData.Transshipment1CarrierAddedManually) {

            this.InsertAndUpdateTransLation(this.ManifestSL.Transshipment1CarrierCode, "Transshipment1Carrier", this.Transshipment1CarrierCode);
          
        }

        //Transshipment2CarrierCode
        if (this.ManifestSL.Transshipment2CarrierCode && !this.IsHideTransshipment2Carrier && this.Transshipment2CarrierCode && this.AgentSideData.Transshipment2CarrierAddedManually) {
            this.InsertAndUpdateTransLation(this.ManifestSL.Transshipment2CarrierCode, "Transshipment2Carrier", this.Transshipment2CarrierCode);

        }

        //Transshipment3CarrierCode
        if (this.ManifestSL.Transshipment3CarrierCode && !this.IsHideTransshipment3Carrier && this.Transshipment3CarrierCode && this.AgentSideData.Transshipment3CarrierAddedManually) {
            this.InsertAndUpdateTransLation(this.ManifestSL.Transshipment3CarrierCode, "Transshipment3Carrier", this.Transshipment3CarrierCode);
     
        }



        //MainCarriageVesselCode
        if (this.ManifestSL && this.ManifestSL.MainCarriageVesselCode && !this.IsHideMainCarriageVessel && this.MainCarriageVesselCode && this.AgentSideData.MainCarriageVesselAddedManually) {

            this.InsertAndUpdateTransLation(this.ManifestSL.MainCarriageVesselCode, "MainCarriageVessel", this.MainCarriageVesselCode);

          
        }

        //Transshipment1VesselCode
        if (this.ManifestSL && this.ManifestSL.Transshipment1VesselCode && !this.IsHideTransshipment1Vessel && this.Transshipment1VesselCode && this.AgentSideData.Transshipment1VesselAddedManually) {

            this.InsertAndUpdateTransLation(this.ManifestSL.Transshipment1VesselCode, "Transshipment1Vessel", this.Transshipment1VesselCode);

        }

        //Transshipment2VesselCode
        if (this.ManifestSL && this.ManifestSL.Transshipment2VesselCode && !this.IsHideTransshipment2Vessel && this.Transshipment2VesselCode && this.AgentSideData.Transshipment2VesselAddedManually) {
            this.InsertAndUpdateTransLation(this.ManifestSL.Transshipment2VesselCode, "Transshipment2Vessel", this.Transshipment2VesselCode);


        }

        //Transshipment3VesselCode
        if (this.ManifestSL && this.ManifestSL.Transshipment3VesselCode && !this.IsHideTransshipment3Vessel && this.Transshipment3VesselCode && this.AgentSideData.Transshipment3VesselAddedManually) {
            this.InsertAndUpdateTransLation(this.ManifestSL.Transshipment3VesselCode, "Transshipment3Vessel", this.Transshipment3VesselCode);


        }



        //IsHideInterlineCode
        if (this.ManifestSL && this.ManifestSL.InterlineCode && !this.IsHideMainInterline && this.InterlineCode && this.AgentSideData.InterlineAddedManually) {

            this.InsertAndUpdateTransLation(this.ManifestSL.InterlineCode, "MainCarriageInterline", this.InterlineCode);

      
        }




        //Shipper
        if (!this.IsConsolShipment && this.AgentSideData.Shipper && this.AgentSideData.Shipper.Code && this.ShipperCode) {

            this.InsertAndUpdateTransLation(this.AgentSideData.Shipper.Code, "ShipperCard", this.ShipperCode);

        }

        //Consignee
        if (!this.IsConsolShipment && this.AgentSideData.Consignee && !AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.Code) && !AppTool.IsNullOrEmpty(this.ConsigneeCode)) {

            this.InsertAndUpdateTransLation(this.AgentSideData.Consignee.Code, "ConsigneeCard", this.ConsigneeCode);

        }
        

        //Notify1
        if (this.AgentSideData.Notify1 && !AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Code) && !AppTool.IsNullOrEmpty(this.Notify1Code)) {
            this.InsertAndUpdateTransLation(this.AgentSideData.Notify1.Code, "Notify1Card", this.Notify1Code);

        }


   

        
        //PackageTypes

        if (this.AgentSideData.PackageTypes && this.AgentSideData.PackageTypes.filter((d => d.AddedManually)).length > 0) {
            this.AgentSideData.PackageTypes.filter((d => d.AddedManually)).forEach((PackageType) => {
                var newTranslatePackage = this.OurAgentPackagesType.filter(d => d.ComputedLocalName == PackageType.Code)[0];

                if (newTranslatePackage) {
                    var trans = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == PackageType.Code && f.ObjectTableName == "Package")[0];
                    if (!trans) {
                        var newTrans: SharedManifestTranslationPM = new SharedManifestTranslationPM();
                        newTrans.ChangeSetOp = "Insert";
                        newTrans.AgentCode = PackageType.Code;
                        newTrans.MyCode = newTranslatePackage.Code;
                        newTrans.ObjectTableName = "Package";
                        newTrans.Tenant = SessionInfo.LoggedUserTenant;
                        this.CurrentEntity.SharedManifestTranslations.push(newTrans);
                    }
                    else {

                        if (trans.ChangeSetOp != "Insert") {
                            trans.ChangeSetOp = "Update";
                        }
                        trans.MyCode = newTranslatePackage.Code;
                    }





                    this.EntityPM.ShipmentPackages.filter(d => d.PackageTypeCode == PackageType.Code && d.IsPackageAddedManually).forEach((item) => {
                        item.PackageTypeId = newTranslatePackage.Id;
                        item.PackageTypeCode = newTranslatePackage.Code;
                        item.PackageTypeName = newTranslatePackage.EnglishName;
                        item.IsContainer = newTranslatePackage.IsContainer;
                    });

                    this.EntityPM.ShipmentPackages.filter(d => d.InsideShipmentPackages != null && d.InsideShipmentPackages.length > 0).forEach((shipmentPackage) => {

                            shipmentPackage.InsideShipmentPackages.filter(d => d.PackageTypeCode == PackageType.Code && d.IsPackageAddedManually).forEach((item) => {
                                item.PackageTypeId = newTranslatePackage.Id;
                                item.PackageTypeCode = newTranslatePackage.Code;
                                item.PackageTypeName = newTranslatePackage.EnglishName;
                                item.IsContainer = newTranslatePackage.IsContainer;
                    });
                    });

                }

            });


        }


        this.EntityPM.IsCreatedFromAgentSharedManifest = true;
        this.myShipmentPMService.insert(this.EntityPM).subscribe(res => {
       
            var shipResponse: ServiceResponse = res;
            if (!shipResponse.HasError) {

                if (!AppTool.IsNullOrEmpty(this.SharedManifestStatus)) {
                    this.CurrentEntity.StatusCode = this.SharedManifestStatus;
                }
         

                this._agentSharedManifestPMService.update(this.CurrentEntity).subscribe(response => {
                    this.CurrentSession.StopBusyIndicator();
                  
                    if (!response.HasError) {
                        
                        if (this.HouseEntity) this.HouseEntity.EntityId = shipResponse.Result.Id;
                        else {
                            this.ManifestSL.EntityId = shipResponse.Result.Id;
                            ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Accept Manifests");
                        }

                        this.CurrentSession.CurrentWindow.Close(shipResponse.Result.Id);
                        if (!this.HouseEntity) {
                            var messageWindow: MessageWindow = new MessageWindow();
                            messageWindow.Title = "Shipment Creation";
                            messageWindow.Show("Your Shipment was successfully created.");

                        }
                    }
                    else {

                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                if (shipResponse.ErrorsArray && shipResponse.ErrorsArray.length > 0) {
                    shipResponse.ErrorsArray.forEach((item) => {
                        this.ValidationErrorsList.push(item);
                    });
                }
            }
        });



    

    }


    AddShipmentPickUpDeliveryPM(pickUpDeliveryTypeCode:string) {
        if (this.ManifestSL && !this.IsHouseShipment) {
            var agentshipmentPickUpDelivery: any = pickUpDeliveryTypeCode == "PICK" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;


            if (agentshipmentPickUpDelivery) {
                var shipmentPickUpDeliveryPM: any = pickUpDeliveryTypeCode == "PICK" ? new ShipmentPickUpPM(null) : new ShipmentDeliveryPM(null);

                var ourSideShipmentPickUpDelivery: any = pickUpDeliveryTypeCode == "PICK" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;

                var entityName = pickUpDeliveryTypeCode == "PICK" ? "Pickup" : "Delivery";

                shipmentPickUpDeliveryPM.PickUpDeliveryFromTypeCode = ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode;
                shipmentPickUpDeliveryPM.PickUpDeliveryToTypeCode = ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode;
                shipmentPickUpDeliveryPM.ETA = ourSideShipmentPickUpDelivery.MainCarriageETA;
                shipmentPickUpDeliveryPM.ETD = ourSideShipmentPickUpDelivery.MainCarriageETD;
                shipmentPickUpDeliveryPM.ATA = ourSideShipmentPickUpDelivery.MainCarriageATA;
                shipmentPickUpDeliveryPM.ATD = ourSideShipmentPickUpDelivery.MainCarriageATD;
                shipmentPickUpDeliveryPM.TransportModeCode = ourSideShipmentPickUpDelivery.SelectedTransportMode ? ourSideShipmentPickUpDelivery.SelectedTransportMode.Code : null;
                shipmentPickUpDeliveryPM.PickUpDeliveryTypeCode = pickUpDeliveryTypeCode;
                //From
                if (shipmentPickUpDeliveryPM.PickUpDeliveryFromTypeCode == "PORT") {
                    var fromPort = agentshipmentPickUpDelivery.FromPort;

                    if (fromPort) {
                        shipmentPickUpDeliveryPM.FromPortId = ourSideShipmentPickUpDelivery.FromPortId;
                        shipmentPickUpDeliveryPM.FromPortCode = fromPort.Code;
                        shipmentPickUpDeliveryPM.FromPortCountryCode = fromPort.CountryCode;
                    }


                }
                else if (shipmentPickUpDeliveryPM.PickUpDeliveryFromTypeCode == "PART") {
                    shipmentPickUpDeliveryPM.FromPartnerCardId = ourSideShipmentPickUpDelivery.FromPartnerCardId;
                    shipmentPickUpDeliveryPM.FromAddressId = ourSideShipmentPickUpDelivery.FromAddressId;


                    if (AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromPartnerCardId)) {
                        this.ValidationErrorsList.push(entityName + " From Partner field is required");
                    }

                }
                else if (shipmentPickUpDeliveryPM.PickUpDeliveryFromTypeCode == "CASL") {
                    shipmentPickUpDeliveryPM.FromAddressZipCode = ourSideShipmentPickUpDelivery.FromAddressZipCode;
                    shipmentPickUpDeliveryPM.FromAddressCity = ourSideShipmentPickUpDelivery.FromAddressCity;
                    shipmentPickUpDeliveryPM.FromAddressCountryId = ourSideShipmentPickUpDelivery.FromAddressCountryId;
                    shipmentPickUpDeliveryPM.FromAddressCountryCode = ourSideShipmentPickUpDelivery.FromAddressCountryCode;

                    if (AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromAddressCountryId)) {
                        this.ValidationErrorsList.push(entityName + " From Country field is required");
                    }
                    if (AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromAddressZipCode) && AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromAddressCity)) {
                        this.ValidationErrorsList.push(entityName + " From City or From Zip Code field is required");
                    }

                }

                //To
                if (shipmentPickUpDeliveryPM.PickUpDeliveryToTypeCode == "PORT") {
                    var toPort = agentshipmentPickUpDelivery.ToPort;
                    if (toPort) {
                        shipmentPickUpDeliveryPM.ToPortId = ourSideShipmentPickUpDelivery.ToPortId;
                        shipmentPickUpDeliveryPM.ToPortCode = toPort.Code;
                        shipmentPickUpDeliveryPM.ToPortCountryCode = toPort.CountryCode;
                    }

                }
                else if (shipmentPickUpDeliveryPM.PickUpDeliveryToTypeCode == "PART") {
                    shipmentPickUpDeliveryPM.ToPartnerCardId = ourSideShipmentPickUpDelivery.ToPartnerCardId;
                    shipmentPickUpDeliveryPM.ToAddressId = ourSideShipmentPickUpDelivery.ToAddressId;
                    if (AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToPartnerCardId)) {
                        this.ValidationErrorsList.push(entityName + " To Partner field is required");
                    }


                }
                else if (shipmentPickUpDeliveryPM.PickUpDeliveryToTypeCode == "CASL") {
                    shipmentPickUpDeliveryPM.ToAddressZipCode = ourSideShipmentPickUpDelivery.ToAddressZipCode;
                    shipmentPickUpDeliveryPM.ToAddressCity = ourSideShipmentPickUpDelivery.ToAddressCity;
                    shipmentPickUpDeliveryPM.ToAddressCountryId = ourSideShipmentPickUpDelivery.ToAddressCountryId;
                    shipmentPickUpDeliveryPM.ToAddressCountryCode = ourSideShipmentPickUpDelivery.ToAddressCountryCode;

                    if (AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToAddressCountryId)) {
                        this.ValidationErrorsList.push(entityName + " To Country field is required");
                    }
                    if (AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToAddressZipCode) && AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToAddressCity)) {
                        this.ValidationErrorsList.push(entityName + " To City or To Zip Code field is required");
                    }

                }

                if (this.ValidationErrorsList.length == 0) {

                    if (pickUpDeliveryTypeCode == "PICK") {
                        this.EntityPM.ShipmentPickUps = [];
                        this.EntityPM.AddPickUp(shipmentPickUpDeliveryPM);
                    } else {

                        this.EntityPM.ShipmentDeliveries = [];
                        this.EntityPM.AddDelivery(shipmentPickUpDeliveryPM);
                    }
                }

            }

        }
    }

    AddShipmentPickUpDeliveryTransLation(pickUpDeliveryTypeCode:string) {
        if (this.ManifestSL && !this.IsHouseShipment) {
            var agentshipmentPickUpDelivery: any = pickUpDeliveryTypeCode == "PICK" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;
            if (agentshipmentPickUpDelivery) {
                var ourSideShipmentPickUpDelivery: any = pickUpDeliveryTypeCode == "PICK" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
                var entityName = pickUpDeliveryTypeCode == "PICK" ? "PickUp" : "Delivery";

                if (ourSideShipmentPickUpDelivery) {
                    if (ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode == "PART") {
                        if (ourSideShipmentPickUpDelivery.FromPartnerCardCode) this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.FromPartner.Code, ("Shipment" + entityName + "FromPartner"), ourSideShipmentPickUpDelivery.FromPartnerCardCode);
                    }
                    else if (ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode == "CASL") {
                        if (ourSideShipmentPickUpDelivery.FromAddressCountryCode) this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.FromAddressCountryCode, ("Shipment" + entityName + "FromCountry"), ourSideShipmentPickUpDelivery.FromAddressCountryCode);
                    }

                    if (ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode == "PART") {
                        if (ourSideShipmentPickUpDelivery.ToPartnerCardCode) this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.ToPartner.Code, ("Shipment" + entityName + "ToPartner"), ourSideShipmentPickUpDelivery.ToPartnerCardCode);
                    } else if (ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode == "CASL") {
                        if (ourSideShipmentPickUpDelivery.ToAddressCountryCode) this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.ToAddressCountryCode, ("Shipment" + entityName + "ToCountry"), ourSideShipmentPickUpDelivery.ToAddressCountryCode);
                    }
                }

            }
        }
    }

    BuildOurPickUpDeliverySide(pickUpDeliveryTypeCode:string) {


      


       var isNoFound: boolean = false;
        if (this.ManifestSL && !this.IsHouseShipment) {
            var agentshipmentPickUpDelivery: any = pickUpDeliveryTypeCode == "PICK" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;

            if (agentshipmentPickUpDelivery) {
                var ourSideShipmentPickUpDelivery: any = pickUpDeliveryTypeCode == "PICK" ? this.OurSideShipmentPickUp = new ShipmentPickUpDeliverySL() : this.OurSideShipmentDelivery = new ShipmentPickUpDeliverySL();
                var entityName = pickUpDeliveryTypeCode == "PICK" ? "PickUp" : "Delivery";

                ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode = agentshipmentPickUpDelivery.PickUpDeliveryFromTypeCode;
                ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode = agentshipmentPickUpDelivery.PickUpDeliveryToTypeCode;
                ourSideShipmentPickUpDelivery.MainCarriageATA = agentshipmentPickUpDelivery.MainCarriageATA;
                ourSideShipmentPickUpDelivery.MainCarriageATD = agentshipmentPickUpDelivery.MainCarriageATD;
                ourSideShipmentPickUpDelivery.MainCarriageETA = agentshipmentPickUpDelivery.MainCarriageETA;
                ourSideShipmentPickUpDelivery.MainCarriageETD = agentshipmentPickUpDelivery.MainCarriageETD;
                ourSideShipmentPickUpDelivery.SelectedTransportMode = ourSideShipmentPickUpDelivery.TransportModeLists.filter(d => d.Code == agentshipmentPickUpDelivery.TransportModeCode)[0];

                var isNeedTranslations: boolean = false;

                //From
                if (agentshipmentPickUpDelivery.FromPort) {
                    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                    this.myPortListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: PortList[] = myResponse.Result;
                            var fromPort: PortList = list.filter(d => d.Code == agentshipmentPickUpDelivery.FromPort.Code)[0];
                            if (fromPort) {
                                ourSideShipmentPickUpDelivery.FromPortId = fromPort.Id;

                            }
                        }
                        if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
                        else this.IsLoadedFromDeliveryTranslation = true;

                        this.StopBusyIndicator();
                    });

                }
                else if (agentshipmentPickUpDelivery.FromPartner) {
                    // Shipper Translation
                    var shipmentPickUpFromPartnerTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == agentshipmentPickUpDelivery.FromPartner.Code && f.ObjectTableName == "Shipment" + entityName + "FromPartner")[0];
                    if (shipmentPickUpFromPartnerTranslation) {
                        this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(shipmentPickUpFromPartnerTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                            var pmResponse: ServiceResponse = res;

                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {

                                    ourSideShipmentPickUpDelivery.FromPartnerCardId = myResult;
                                } else isNeedTranslations = true;
                            }

                            if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
                            else this.IsLoadedFromDeliveryTranslation = true;

                            this.StopBusyIndicator();
                        });

                    }
                    else {

                        if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
                        else this.IsLoadedFromDeliveryTranslation = true;

                        isNeedTranslations = true;
                        this.StopBusyIndicator();
                    }


                    var englishName = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.EnglishName) ? agentshipmentPickUpDelivery.FromPartner.EnglishName : "";
                    var Address1 = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.Address1) ? agentshipmentPickUpDelivery.FromPartner.Address1 : "";
                    var Address2 = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.Address2) ? agentshipmentPickUpDelivery.FromPartner.Address2 : "";
                    var city = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.City) ? agentshipmentPickUpDelivery.FromPartner.City : "";

                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues = englishName + "^";
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (Address1 + "^");
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (Address2 + "^");
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (city + "^");
                    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                    apiQueryFilters.addAdditionalFilter("Code", agentshipmentPickUpDelivery.FromPartner.CountryCode, null, null, "Equals", true, true, true, "Text");
                    this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: any[] = myResponse.Result;
                            var CountryList: any = list.filter(d => d.Code == agentshipmentPickUpDelivery.FromPartner.CountryCode && !d.InActive)[0];
                            if (CountryList) {
                                ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (CountryList.Id);
                            }
                        }

                    });

                }
                else if (agentshipmentPickUpDelivery.PickUpDeliveryFromTypeCode == "CASL") {

                    if (agentshipmentPickUpDelivery.FromAddressCountryCode) {

                        var shipmentPickUpFromCountryTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == agentshipmentPickUpDelivery.FromAddressCountryCode && f.ObjectTableName == "Shipment" + entityName + "FromCountry")[0];
                        if (shipmentPickUpFromCountryTranslation) {
                            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                            apiQueryFilters.GetAll = true;
                            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                            apiQueryFilters.addAdditionalFilter("Code", shipmentPickUpFromCountryTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                            this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    var list: any[] = myResponse.Result;
                                    var CountryList: any = list.filter(d => d.Code == shipmentPickUpFromCountryTranslation.MyCode && !d.InActive)[0];
                                    if (CountryList) {
                                        ourSideShipmentPickUpDelivery.FromAddressCountryId = CountryList.Id;
                                        ourSideShipmentPickUpDelivery.FromAddressCountryName = CountryList.EnglishName;
                                        ourSideShipmentPickUpDelivery.FromAddressCountryCode = CountryList.Code;

                                    } else isNeedTranslations = true;
                                }

                                if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
                                else this.IsLoadedFromDeliveryTranslation = true;

                                this.StopBusyIndicator();
                            });

                        } else {
                            if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
                            else this.IsLoadedFromDeliveryTranslation = true;
                            isNeedTranslations = true;
                            this.StopBusyIndicator();
                        }

                    }
                    if (agentshipmentPickUpDelivery.FromAddressCity) isNeedTranslations = true;

                    ourSideShipmentPickUpDelivery.FromAddressZipCode = agentshipmentPickUpDelivery.FromAddressZipCode;
                    ourSideShipmentPickUpDelivery.FromAddressCity = agentshipmentPickUpDelivery.FromAddressCity;

                }
                else {
                    if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
                    else this.IsLoadedFromDeliveryTranslation = true;
                }


                //To
                if (agentshipmentPickUpDelivery.ToPort) {
                    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                    this.myPortListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: PortList[] = myResponse.Result;
                            var ToPort: PortList = list.filter(d => d.Code == agentshipmentPickUpDelivery.ToPort.Code)[0];
                            if (ToPort) {
                                ourSideShipmentPickUpDelivery.ToPortId = ToPort.Id;

                            }
                        }
                        if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
                        else this.IsLoadedToDeliveryTranslation = true;

                        this.StopBusyIndicator();
                    });

                }
                else if (agentshipmentPickUpDelivery.ToPartner) {
                    // Shipper Translation
                    var shipmentPickUpToPartnerTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == agentshipmentPickUpDelivery.ToPartner.Code && f.ObjectTableName == "Shipment" + entityName + "ToPartner")[0];
                    if (shipmentPickUpToPartnerTranslation) {
                        this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(shipmentPickUpToPartnerTranslation.MyCode, SessionInfo.LoggedUserTenant).subscribe(res => {
                            var pmResponse: ServiceResponse = res;

                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {

                                    ourSideShipmentPickUpDelivery.ToPartnerCardId = myResult;
                                } else isNeedTranslations = true;
                            }

                            if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
                            else this.IsLoadedToDeliveryTranslation = true;

                            this.StopBusyIndicator();
                        });

                    }
                    else {

                        if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
                        else this.IsLoadedToDeliveryTranslation = true;

                        isNeedTranslations = true;
                        this.StopBusyIndicator();
                    }


                    var englishName = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.EnglishName) ? agentshipmentPickUpDelivery.ToPartner.EnglishName : "";
                    var Address1 = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.Address1) ? agentshipmentPickUpDelivery.ToPartner.Address1 : "";
                    var Address2 = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.Address2) ? agentshipmentPickUpDelivery.ToPartner.Address2 : "";
                    var city = !AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.City) ? agentshipmentPickUpDelivery.ToPartner.City : "";

                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues = englishName + "^";
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (Address1 + "^");
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (Address2 + "^");
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (city + "^");
                    var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                    apiQueryFilters.addAdditionalFilter("Code", agentshipmentPickUpDelivery.ToPartner.CountryCode, null, null, "Equals", true, true, true, "Text");
                    this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var list: any[] = myResponse.Result;
                            var CountryList: any = list.filter(d => d.Code == agentshipmentPickUpDelivery.ToPartner.CountryCode && !d.InActive)[0];
                            if (CountryList) {
                                ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (CountryList.Id);
                            }
                        }

                    });

                }
                else if (agentshipmentPickUpDelivery.PickUpDeliveryToTypeCode == "CASL") {

                    if (agentshipmentPickUpDelivery.ToAddressCountryCode) {

                        var shipmentPickUpToCountryTranslation = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == agentshipmentPickUpDelivery.ToAddressCountryCode && f.ObjectTableName == "Shipment" + entityName + "ToCountry")[0];
                        if (shipmentPickUpToCountryTranslation) {
                            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
                            apiQueryFilters.GetAll = true;
                            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
                            apiQueryFilters.addAdditionalFilter("Code", shipmentPickUpToCountryTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                            this.countryListService.getAllFromCache(apiQueryFilters).subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    var list: any[] = myResponse.Result;
                                    var CountryList: any = list.filter(d => d.Code == shipmentPickUpToCountryTranslation.MyCode && !d.InActive)[0];
                                    if (CountryList) {
                                        ourSideShipmentPickUpDelivery.ToAddressCountryId = CountryList.Id;
                                        ourSideShipmentPickUpDelivery.ToAddressCountryName = CountryList.EnglishName;
                                        ourSideShipmentPickUpDelivery.ToAddressCountryCode = CountryList.Code;

                                    } else isNeedTranslations = true;
                                }

                                if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
                                else this.IsLoadedToDeliveryTranslation = true;

                                this.StopBusyIndicator();
                            });

                        } else {
                            if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
                            else this.IsLoadedToDeliveryTranslation = true;
                            isNeedTranslations = true;
                            this.StopBusyIndicator();
                        }

                    }
                    if (agentshipmentPickUpDelivery.ToAddressCity) isNeedTranslations = true;

                    ourSideShipmentPickUpDelivery.ToAddressZipCode = agentshipmentPickUpDelivery.ToAddressZipCode;
                    ourSideShipmentPickUpDelivery.ToAddressCity = agentshipmentPickUpDelivery.ToAddressCity;

                }
                else {
                    if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
                    else this.IsLoadedToDeliveryTranslation = true;
                }
        
                if (!isNeedTranslations) {
                    if (pickUpDeliveryTypeCode == "PICK") this.IsNoPickupDetailsFound = true;
                    else this.IsNoDeliveryDetailsFound = true;
                }
            }
            else isNoFound = true;
        }
        else isNoFound = true;




        if (isNoFound) {

            if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedFromPickUpTranslation = true;
            else this.IsLoadedFromDeliveryTranslation = true;


            if (pickUpDeliveryTypeCode == "PICK") this.IsLoadedToPickUpTranslation = true;
            else this.IsLoadedToDeliveryTranslation = true;


            if (pickUpDeliveryTypeCode == "PICK") this.HidePickupDetailsArea = true;
            else this.HideDeliveryDetailsArea = true;

            if (pickUpDeliveryTypeCode == "PICK") this.IsNoPickupDetailsFound = true;
            else this.IsNoDeliveryDetailsFound = true;

            this.StopBusyIndicator();
        }

    }

    
    InsertAndUpdateTransLation(agentCode: string, tableName: string, myCode:string) {
      
        var trans = this.CurrentEntity.SharedManifestTranslations.filter(f => f.AgentCode == agentCode && f.ObjectTableName == tableName)[0];
        if (!trans) {
            var newTrans: SharedManifestTranslationPM = new SharedManifestTranslationPM();
            newTrans.ChangeSetOp = "Insert";
            newTrans.AgentCode = agentCode;
            newTrans.MyCode = myCode;
            newTrans.ObjectTableName = tableName;
            newTrans.Tenant = SessionInfo.LoggedUserTenant;
            this.CurrentEntity.SharedManifestTranslations.push(newTrans);
        }
        else {
            trans.ChangeSetOp = "Update";
            trans.MyCode = myCode;
        }
    }




    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private timerToken: any;
    private Retries: number = 0;
    private GeneratedComponent: any;

    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.LoadChildComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    
    LoadChildComponent() {
        if (this.IsLoadAdditionalScreen) {
            let myGeneratedComponentLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "GECO")[0];
            if (myGeneratedComponentLocation != null) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', myGeneratedComponentLocation.viewContainerRef)
                    .then(cmpRef => {
                        this.GeneratedComponent = cmpRef.instance;

                        var screenCode = "NewShipment";
                        cmpRef.instance.LabelWidth = 110;
                        cmpRef.instance.Run(this.EntityPM, "Shipment", "SharedManifestAdditionalScreen");

                        cmpRef.instance.LoadCompleted.subscribe(s => {

                        });
                    });
                this.IsLoadAdditionalScreen = false;
            }
        }

        if (!this.IsLoadSharedManifestheaderScreen) {
            let mySharedManifestHeaderLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == "SHCO")[0];
            if (mySharedManifestHeaderLocation != null) {
                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent', mySharedManifestHeaderLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.Run(this.CurrentEntity, this.AgentSharedManifestList);


                    });
                this.IsLoadSharedManifestheaderScreen = true;
            }
        }
    }

    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }


    StopBusyIndicator() {



        if (this.IsLoadedCarrierTranslation && this.IsLoadedIncotermTranslation && this.IsLoadedShipperTranslation && this.IsLoadedConsigneeTranslation && this.IsLoadedShiperDefaultValues && this.IsLoadedConsigneeDefaultValues && this.IsLoadedTransshipment1CarrierTranslation && this.IsLoadedTransshipment2CarrierTranslation && this.IsLoadedTransshipment3CarrierTranslation && this.IsLoadedMainCarriageVesselTranslation && this.IsLoadedTransshipment1VesselTranslation && this.IsLoadedTransshipment2VesselTranslation && this.IsLoadedTransshipment3VesselTranslation && this.IsLoadedMainCarriageInterlineTranslation && this.IsLoadedNotify1IdTranslation && this.IsLoadedNotify1DefaultValuesTranslation && this.IsLoadedPortsTranslation && this.IsLoadedPackageTranslation && this.IsLoadedMoveTypeTranslation && this.IsLoadedValueOfGoodsCurrencyTranslation && this.IsLoadedFromPickUpTranslation && this.IsLoadedToPickUpTranslation && this.IsLoadedFromDeliveryTranslation && this.IsLoadedToDeliveryTranslation) {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();

            if ((!AppTool.IsNullOrEmpty(this.IncotermId) || this.IsHideIncoterm) && (!AppTool.IsNullOrEmpty(this.MoveTypeId) || this.IsHideMoveType) && (!AppTool.IsNullOrEmpty(this.ValueOfGoodsCurrencyId) || this.IsHideValueOfGoodsCurrency)) {
                this.HideGeneralDetailsArea = true;
            }


            if ((!AppTool.IsNullOrEmpty(this.Transshipment1CarrierId) || this.IsHideTransshipment1Carrier) && (!AppTool.IsNullOrEmpty(this.Transshipment2CarrierId) || this.IsHideTransshipment2Carrier) && (!AppTool.IsNullOrEmpty(this.Transshipment3CarrierId) || this.IsHideTransshipment3Carrier) && (!AppTool.IsNullOrEmpty(this.Transshipment1VesselId) || this.IsHideTransshipment1Vessel) && (!AppTool.IsNullOrEmpty(this.Transshipment2VesselId) || this.IsHideTransshipment2Vessel) && (!AppTool.IsNullOrEmpty(this.Transshipment3VesselId) || this.IsHideTransshipment3Vessel)) {
                this.HideTransShipmentsDetailsArea = true;
            }

        }

    }



}


export class AgentSide extends BaseComponent {
    Shipper: PartnerSL;
    Consignee: PartnerSL;
    Notify1: PartnerSL;
    Notify2: PartnerSL;
    IncotermCode: string;
    IncotermName: string;
    CarrierCode: string;
    CarrierName: string;
    FreightPrepaidCollectId: string;
    OtherPrepaidCollectId: string;
    AgentName: string;
    PackageTypes: PackageTypePM[] = [];
    IncotermId: string;

    CarrierId: string;
    CarrierAddedManually: boolean;
    ValueOfGoods: number;
    IsDangerous: boolean;
    DescriptionOfGoods: string;

    MainHarmonize: string;



    public MainCarriageVesselAddedManually: boolean;
    public Transshipment1VesselAddedManually: boolean;
    public Transshipment2VesselAddedManually: boolean;
    public Transshipment3VesselAddedManually: boolean;


    public Transshipment1CarrierAddedManually: boolean;
    public Transshipment2CarrierAddedManually: boolean;
    public Transshipment3CarrierAddedManually: boolean;



    IncotermAddedManually: boolean;


    public InterlineAddedManually: boolean;


    public ValueOfGoodsCurrencyId: string;
    public ValueOfGoodsCurrencyName: string;
    public ValueOfGoodsCurrencyCode: string;
    public ValueOfGoodsCurrencyAddedManually: boolean;

    MoveTypeId: string;
    MoveTypeName: string;
    MoveTypeCode: string;
    MoveTypeAddedManually: boolean;
    public MoveTypeTransportModeId: string;

    constructor(manifestSL: ManifestSL , houseSL: HouseSL ) {

        super();
        this.Shipper = houseSL != null ? houseSL.Shipper : manifestSL.Shipper ;
        this.Consignee = houseSL != null ? houseSL.Consignee : manifestSL.Consignee;
        this.IncotermCode = houseSL != null ? houseSL.IncotermCode : manifestSL.IncotermCode;
        this.IncotermName = houseSL != null ? !AppTool.IsNullOrEmpty(houseSL.IncotermName) ? houseSL.IncotermName : houseSL.IncotermCode : !AppTool.IsNullOrEmpty(manifestSL.IncotermName) ? manifestSL.IncotermName : manifestSL.IncotermCode;
        this.IncotermId = houseSL != null ? houseSL.IncotermId : manifestSL.IncotermId;

        this.CarrierCode = houseSL != null ? houseSL.CarrierCode : manifestSL.CarrierCode;
        this.CarrierName = houseSL != null ? !AppTool.IsNullOrEmpty(houseSL.CarrierName) ? houseSL.CarrierName : houseSL.CarrierCode : !AppTool.IsNullOrEmpty(manifestSL.CarrierName) ? manifestSL.CarrierName : manifestSL.CarrierCode;
        this.CarrierId = houseSL != null ? houseSL.CarrierId : manifestSL.CarrierId;

        this.FreightPrepaidCollectId = houseSL != null ? houseSL.FreightPrepaidCollectId : manifestSL.FreightPrepaidCollectId;
        this.Notify1 = houseSL != null ? houseSL.Notify1 : manifestSL.Notify1;
        this.Notify2 = houseSL != null ? houseSL.Notify2 : manifestSL.Notify2;
        this.OtherPrepaidCollectId = houseSL != null ? houseSL.OtherPrepaidCollectId : manifestSL.OtherPrepaidCollectId;
        this.AgentName = houseSL != null ? houseSL.AgentName : manifestSL.AgentName;
        this.ValueOfGoods = houseSL != null ? houseSL.ValueOfGoods : manifestSL.ValueOfGoods;
        this.IsDangerous = houseSL != null ? houseSL.IsDangerous : manifestSL.IsDangerous;
        this.DescriptionOfGoods = houseSL != null ? houseSL.GeneralDescriptionOfGoods : manifestSL.GeneralDescriptionOfGoods;

        this.MoveTypeCode = houseSL != null ? houseSL.MoveTypeCode : manifestSL.MoveTypeCode;
        this.MoveTypeName = houseSL != null ? !AppTool.IsNullOrEmpty(houseSL.MoveTypeName) ? houseSL.MoveTypeName : houseSL.MoveTypeCode : !AppTool.IsNullOrEmpty(manifestSL.MoveTypeName) ? manifestSL.MoveTypeName : manifestSL.MoveTypeCode;
        this.MoveTypeId = houseSL != null ? houseSL.MoveTypeId : manifestSL.MoveTypeId;
    
        
        var shipmentPackagePM: ShipmentPackagePM[] = houseSL != null ? houseSL.ShipmentPackages : manifestSL.ShipmentPackages;

        this.InterlineAddedManually = houseSL != null ? false : manifestSL.InterlineAddedManually;
        this.MoveTypeAddedManually = houseSL != null ? houseSL.MoveTypeAddedManually : manifestSL.MoveTypeAddedManually;
        this.IncotermAddedManually = houseSL != null ? houseSL.IncotermAddedManually : manifestSL.IncotermAddedManually;



      
        this.CarrierAddedManually = houseSL != null ? houseSL.CarrierAddedManually : manifestSL.CarrierAddedManually;
        this.Transshipment1CarrierAddedManually = houseSL != null ? false : manifestSL.Transshipment1CarrierAddedManually;
        this.Transshipment2CarrierAddedManually = houseSL != null ? false : manifestSL.Transshipment2CarrierAddedManually;
        this.Transshipment3CarrierAddedManually = houseSL != null ? false : manifestSL.Transshipment3CarrierAddedManually;

        this.MainCarriageVesselAddedManually = houseSL != null ? false : manifestSL.MainCarriageVesselAddedManually;
        this.Transshipment1VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment1VesselAddedManually;
        this.Transshipment2VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment2VesselAddedManually;
        this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;

        
        this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;


        this.ValueOfGoodsCurrencyCode = houseSL != null ? houseSL.ValueOfGoodsCurrencyCode : manifestSL.ValueOfGoodsCurrencyCode;
        this.ValueOfGoodsCurrencyName = houseSL != null ? !AppTool.IsNullOrEmpty(houseSL.ValueOfGoodsCurrencyName) ? houseSL.ValueOfGoodsCurrencyName : houseSL.ValueOfGoodsCurrencyCode : !AppTool.IsNullOrEmpty(manifestSL.ValueOfGoodsCurrencyName) ? manifestSL.ValueOfGoodsCurrencyName : manifestSL.ValueOfGoodsCurrencyCode;
        this.ValueOfGoodsCurrencyId = houseSL != null ? houseSL.ValueOfGoodsCurrencyId : manifestSL.ValueOfGoodsCurrencyId;

        this.ValueOfGoodsCurrencyAddedManually = houseSL != null ? houseSL.ValueOfGoodsCurrencyAddedManually : manifestSL.ValueOfGoodsCurrencyAddedManually;
        this.MoveTypeTransportModeId = houseSL != null ? houseSL.MoveTypeTransportModeId : manifestSL.MoveTypeTransportModeId;

        this.MainHarmonize = houseSL != null ? houseSL.MainHarmonize : manifestSL.MainHarmonize;




        if (shipmentPackagePM && shipmentPackagePM.length > 0) {
            this.PackageTypes = [];
            shipmentPackagePM.forEach((shipmentPackage) => {
                var item = this.PackageTypes.filter(d => d.Code == shipmentPackage.PackageTypeCode)[0];
                if (!item) {
                    var packageTypePM: PackageTypePM = new PackageTypePM();
                    packageTypePM.Id = shipmentPackage.PackageTypeId;
                    packageTypePM.Code = shipmentPackage.PackageTypeCode;
                    packageTypePM.EnglishName = shipmentPackage.PackageTypeName;
                    packageTypePM.IsContainer = shipmentPackage.IsContainer;
                    packageTypePM.IsAir = shipmentPackage.PackageTypeIsAir;
                    packageTypePM.IsOcean = shipmentPackage.PackageTypeIsOcean
                    packageTypePM.IsInland = shipmentPackage.PackageTypeIsInland
                    packageTypePM.LocalName = shipmentPackage.PackageTypeLocalName
                    packageTypePM.Volume = shipmentPackage.PackageTypeVolume
                    packageTypePM.TEU = shipmentPackage.TEU;
                    packageTypePM.Notes = shipmentPackage.PackageTypeNote;
                    packageTypePM.ContainerSize = shipmentPackage.ContainerSize;
                    packageTypePM.AddedManually = shipmentPackage.IsPackageAddedManually;
                    this.PackageTypes.push(packageTypePM);
                }
            });

            shipmentPackagePM.filter(d => d.InsideShipmentPackages != null && d.InsideShipmentPackages.length > 0).forEach((shipmentPackage) => {
                shipmentPackage.InsideShipmentPackages.forEach((insideShipmentPackages) => {

                    var item = this.PackageTypes.filter(d => d.Code == insideShipmentPackages.PackageTypeCode)[0];
                    if (!item) {
                        var packageTypePM: PackageTypePM = new PackageTypePM();
                        packageTypePM.Id = insideShipmentPackages.PackageTypeId;
                        packageTypePM.Code = insideShipmentPackages.PackageTypeCode;
                        packageTypePM.EnglishName = insideShipmentPackages.PackageTypeName;
                        packageTypePM.IsContainer = insideShipmentPackages.IsContainer;
                        packageTypePM.IsAir = insideShipmentPackages.PackageTypeIsAir;
                        packageTypePM.IsOcean = insideShipmentPackages.PackageTypeIsOcean
                        packageTypePM.IsInland = insideShipmentPackages.PackageTypeIsInland
                        packageTypePM.LocalName = insideShipmentPackages.PackageTypeLocalName
                        packageTypePM.Volume = insideShipmentPackages.PackageTypeVolume
                        packageTypePM.TEU = insideShipmentPackages.TEU;
                        packageTypePM.Notes = insideShipmentPackages.PackageTypeNote;
                        packageTypePM.ContainerSize = insideShipmentPackages.ContainerSize;
                        packageTypePM.AddedManually = insideShipmentPackages.IsPackageAddedManually;
                        this.PackageTypes.push(packageTypePM);
                    }

                });
            });
        }


    }
}






