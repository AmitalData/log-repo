import { Component, OnInit, ViewChild, ViewContainerRef, OnDestroy} from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, FormatTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentTool} from '../../Tools';
import {ShipmentValidator} from '../../Validators/ShipmentValidator';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../EntityPMs/ShipmentPickUpPM';
import {ShipmentDeliveryPM} from '../../EntityPMs/ShipmentDeliveryPM';
import {ShipmentOrderPackagePM} from '../../EntityPMs/ShipmentOrderPackagePM';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {MAWBStackPM} from '../../../Common/EntityPMs/MAWBStackPM';
import {AddressPM} from '../../../Common/EntityPMs/AddressPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {PortList} from '../../../Common/EntityLists/PortList';
import {CardList} from '../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../Common/EntityLists/AirlineList';
import {AirlinePM} from '../../../Common/EntityPMs/AirlinePM';
import {AddressList} from '../../../Common/EntityLists/AddressList';      
import {IncotermList} from '../../../Common/EntityLists/IncotermList';
import {PackageTypeList} from '../../../Common/EntityLists/PackageTypeList';
import {PortListService} from '../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../Common/Services/StandardLists/AirlineListService';
import {AddressListService} from '../../../Common/Services/StandardLists/AddressListService';
import {IncotermListService} from '../../../Common/Services/StandardLists/IncotermListService';
import {ShipmentPMService} from '../../Services/StandardPMs/ShipmentPMService';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {NewShipmentComponentArgs, AddEditPartnerArgs} from '../../Args';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentDomainService} from '../../Services/ShipmentDomainService';
import {AWBStackDomainService} from '../../../Common/Services/AWBStackDomainService';
import {CitySelectionArgs} from '../../../Common/Args';
import {ShippingLinePM} from '../../../Common/EntityPMs/ShippingLinePM';
import {ShippingLinePMService} from '../../../Common/Services/StandardPMs/ShippingLinePMService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';

@Component({
    moduleId: module.id,
    templateUrl: './NewShipmentComponent.html',
})

export class NewShipmentComponent extends BaseComponent implements OnInit, OnDestroy {
    public TenantPM: TenantPM;
    public EntityPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "Shipment";
    public LabelColumnWidth: number = 115;
    public ControlColumnWidth: number = 220;
    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
    public ValidationErrorsList: string[] = [];
    public OkButtonLabel: string;
    public SessionIndex: number;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    private PropertyChangedEvent: any = null;
    constructor() {
        super();
        this.SessionIndex = SessionLocator.Index;
        this.InitializeServices();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = this.myShipmentPMService.GetNewEntityPM();
        this.OkButtonLabel = TextCodeTranslator.Translate("Shipment.B.Create");
         
        if (this.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }
    }
    public ScreenIsReady: boolean = false;
    ngOnInit() {
        var listservice: EntityListService = new EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then((res: any) => {
            res.subscribe(resp => {
                this.BuildFiltersLists();

                if (this.IsCopyFromShipment == false && this.IsBuildFromQuote == false) {
                    this.OnFiltersChanged();
                }

                this.BuildAdditionalFields();
                this.LoadAllowedAirline();
                this.ScreenIsReady = true;
                this.ListenToPropertyChanged();
            });
        });

    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.PropertyChangedEvent);
    }
    
    private ListenToPropertyChanged() {
        if (this.PropertyChangedEvent) {
            AppTool.KillEventEmitter(this.PropertyChangedEvent);
            this.PropertyChangedEvent = null;
        }

        this.PropertyChangedEvent = this.EntityPM.PropertyChanged.subscribe(s => {
            if (s) {
                if (s.PropertyName == "ShipmentCustomerTypeCode") {
                    this.CustomerId = null;
                    this.SetCustomer(this.EntityPM.ShipmentCustomerTypeCode);
                    this.SetCustomerRequired();
                    this.ComputeCustomerDependency();
                }
            }
        });
    }

    private myPortListService: PortListService;
    private myCardListService: CardListService;
    private myAirlineListService: AirlineListService;
    private myAddressListService: AddressListService;
    private myIncotermListService: IncotermListService;
    private myPartnersDomainService: PartnersDomainService;
    private myShipmentPMService: ShipmentPMService;
    private myShippingLineService: ShippingLinePMService;
    InitializeServices() {
        this.myPortListService = new PortListService();
        this.myCardListService = new CardListService();
        this.myAirlineListService = new AirlineListService();
        this.myAddressListService = new AddressListService();
        this.myIncotermListService = new IncotermListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.myShipmentPMService = new ShipmentPMService();
        this.myShippingLineService = new ShippingLinePMService();
    }
    LoadAllowedAirline() {
        if (SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    if (this.EntityPM.TransportModeId == "A") {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            if (!this.IsBuildFromQuote && !this.IsCopyFromShipment) {
                                this.myPartnersDomainService.GetAllowedAirlineId().subscribe((myResponse: ServiceResponse) => {
                                    if (myResponse != null) {
                                        if (myResponse.HasError) {
                                            this.ValidationErrorsList = myResponse.ErrorsArray;
                                        }

                                        else {
                                            var allowedAirlineId: string = myResponse.Result;
                                            if (!AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                                this.MainCarriageCarrierId = allowedAirlineId;
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            }
        }
    }

    public SourceEntityPM: ShipmentPM;
    public IsShipmentLevelFixed: boolean = false;
    public IsBuildFromQuote: boolean = false;
    public IsCopyFromShipment: boolean = false;
    public IsCreatedFromMasterHouses: boolean = false;
    public IsCreatedFromCustomerOverview: boolean = false;
    public ShowShipmentLevels: boolean = true;
    SetWindowArgs(args: any) {
        if (args.IsNew == null) {
            this.SourceEntityPM = args.Shipment;
            this.EntityPM.ShipmentLevelCode = args.ShipmentLevelCode;
            this.IsShipmentLevelFixed = args.IsShipmentLevelFixed;
            this.IsBuildFromQuote = args.IsBuildFromQuote;
            this.IsCopyFromShipment = args.IsCopyFromShipment;
            this.IsCreatedFromMasterHouses = args.IsCreatedFromMasterHouses;
            this.IsCreatedFromCustomerOverview = args.IsCreatedFromCustomerOverview;
            this.ShowShipmentLevels = false;
            this.BuildFiltersLists();
            this.SetUIProperties();
            this.CopyEntityData();
        }
    }

    public DirectionsList: FilterClass[] = [];
    public TransportModesList: FilterClass[] = [];
    public ShipmentTypesList: FilterClass[] = [];
    public ShipmentLevelsList: FilterClass[] = [];
    BuildFiltersLists() {
        this.DirectionsList = [];
        this.TransportModesList = [];        

        var direct_E = new FilterClass("E", "Export");
        var direct_I = new FilterClass("I", "Import");
        var direct_D = new FilterClass("D", "Domestic");
        var direct_R = new FilterClass("R", "Drop");
        var transport_A = new FilterClass("A", "Air");
        var transport_O = new FilterClass("O", "Ocean");
        var transport_I = new FilterClass("I", "Inland");

        var isAirExportOnly: boolean = false;        
        if (this.IsCreatedFromCustomerOverview) {
            var myCodes: string[] = [];
            myCodes.push("EAWB");
            myCodes.push("BUBK");

            if (FeatureLocator.IsPackageOneOf(myCodes)) {
                isAirExportOnly = true;
            }
        }

        if (isAirExportOnly) {
            this.DirectionsList.push(direct_E);
            this.TransportModesList.push(transport_A);
            this.DirectionId = "E";
            this.TransportModeId = "A";
            this.OnFiltersChanged();
        }

        else {
            this.DirectionsList.push(direct_E);
            this.DirectionsList.push(direct_I);
            this.DirectionsList.push(direct_D);
            this.DirectionsList.push(direct_R);
            this.TransportModesList.push(transport_A);
            this.TransportModesList.push(transport_O);
            this.TransportModesList.push(transport_I);
        }

        this.BuildShipmentTypes();
        this.BuildShipmentLevels();
    }
    BuildShipmentTypes() {
        this.ShipmentTypesList = [];
        
        if (this.DirectionId && this.TransportModeId) {
            switch (this.TransportModeId) {
                case "O": {
                    this.ShipmentTypesList.push(new FilterClass("FCLD", "FCL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LCLD", "LCL", "./Images/CellIcons/Package.png"));
                    break;
                }

                case "I": {
                    this.ShipmentTypesList.push(new FilterClass("FTL", "FTL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LTL", "LTL", "./Images/CellIcons/Package.png"));
                    break;
                }
            }
        }
    }
    BuildShipmentLevels() {
        this.ShipmentLevelsList = [];

        if (this.ShowShipmentLevels) {
            this.ShipmentLevelsList.push(new FilterClass("D", "Direct"));
            this.ShipmentLevelsList.push(new FilterClass("H", "House"));
        }
    }

    public ScreenOpacity: number = 0.7;
    public IsScreenEnabled: boolean = false;
    SetScreenEnabled() {        

        var isScreenEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.DirectionId) && !AppTool.IsNullOrEmpty(this.TransportModeId) && !AppTool.IsNullOrEmpty(this.ShipmentLevelCode)) {
            if (this.TransportModeId == "A") {
                isScreenEnabled = true;
            }

            else if (!AppTool.IsNullOrEmpty(this.ShipmentTypeId)) {
                isScreenEnabled = true;
            }
        }

        this.IsScreenEnabled = isScreenEnabled;
        this.ScreenOpacity = isScreenEnabled ? 1 : 0.7;

        // Shipper
        this.UIProperties.SetEnabled("ShipperId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipperReference2", this.ObjectTableName, isScreenEnabled);

        // Consignee
        this.UIProperties.SetEnabled("ConsigneeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ConsigneeReference2", this.ObjectTableName, isScreenEnabled);

        //Customer
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ShipmentCustomerTypeCode", this.ObjectTableName, isScreenEnabled);

        // Pickup
        this.UIProperties.SetEnabled("IncludePickUp", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PickUpAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressZipCode", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, isScreenEnabled);

        // Delivery
        this.UIProperties.SetEnabled("IncludeDelivery", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("DeliveryAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressZipCode", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, isScreenEnabled);

        // MainCarriage
        if (this.IsCreatedFromMasterHouses) {
            this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isScreenEnabled);
            this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isScreenEnabled);
        }

        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, isScreenEnabled);        

        // General
        this.UIProperties.SetEnabled("IncotermId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MoveTypeId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FreightPrepaidCollectId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("OtherPrepaidCollectId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("House", this.ObjectTableName, isScreenEnabled);

        // Expected Order
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("OrderIsDangerouseGoods", this.ObjectTableName, isScreenEnabled);        
        this.UIProperties.SetEnabled("Quantity1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Quantity2", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Quantity3", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Quantity4", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Quantity5", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageTypeId1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageTypeId2", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageTypeId3", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageTypeId4", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("PackageTypeId5", this.ObjectTableName, isScreenEnabled);

        this.SetUIProperties_GeneratedComponent();
    }

    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public IsInlandDomestic: boolean = false;
    OnFiltersChanged() {
        
        if (!this.IsCreatedFromMasterHouses) {
            this.IsDirectionListEnabled = true;
            this.IsTransportModesListEnabled = AppTool.IsNullOrEmpty(this.DirectionId) ? false : true;
            this.IsShipmentTypesListEnabled = true;
        }

        this.SetScreenEnabled();
        this.SetUIProperties();        
        this.SetUnits();
        this.SetLabels();
        this.SetPartners();
        this.SetPrepaidCollect();

        if (this.IsInlandDomestic) {
            if (!this.IsShipmentLevelFixed) {
                this.ShipmentLevelCode = "D";
            }

            this.ShowShipmentLevels = false;
        }

        else {
            if (!this.IsShipmentLevelFixed) {
                this.ShowShipmentLevels = true;
            }
        }
    }

    private isDirectionListEnabled: boolean = false;
    get IsDirectionListEnabled() { return this.isDirectionListEnabled; }
    set IsDirectionListEnabled(value: boolean) {
        if (this.isDirectionListEnabled != value) {
            this.isDirectionListEnabled = value;
        }
    }

    private isTransportModesListEnabled: boolean = false;
    get IsTransportModesListEnabled() { return this.isTransportModesListEnabled; }
    set IsTransportModesListEnabled(value: boolean) {
        if (this.isTransportModesListEnabled != value) {
            this.isTransportModesListEnabled = value;
        }
    }

    private isShipmentTypesListEnabled: boolean = false;
    get IsShipmentTypesListEnabled() { return this.isShipmentTypesListEnabled; }
    set IsShipmentTypesListEnabled(value: boolean) {
        if (this.isShipmentTypesListEnabled != value) {
            this.isShipmentTypesListEnabled = value;
        }
    }

    private isShipmentLevelsListEnabled: boolean = false;
    get IsShipmentLevelsListEnabled() { return this.isShipmentLevelsListEnabled; }
    set IsShipmentLevelsListEnabled(value: boolean) {
        if (this.isShipmentLevelsListEnabled != value) {
            this.isShipmentLevelsListEnabled = value;
        }
    }

    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId != newValue) {
            this.EntityPM.DirectionId = newValue;
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            this.OnFiltersChanged();
            this.SetTransportModes();            
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;

            this.MainCarriageFromPortId = null;
            this.MainCarriageToPortId = null;
            this.MainCarriageCarrierId = null;
            this.EntityPM.ShipmentTypeId = null;
            this.MoveTypeId = null;

            if (!this.IsShipmentLevelFixed) {
                this.EntityPM.ShipmentLevelCode = null;
            }
            
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;

            this.DelOrderDetails();
            this.OnFiltersChanged();
            this.BuildShipmentTypes();
            this.LoadAllowedAirline();
            this.ValidateMasterField();            
        }
    }

    public ShipmentTypeName: string = null;
    get ShipmentTypeId() { return this.EntityPM.ShipmentTypeId; }
    set ShipmentTypeId(newValue: string) {
        if (this.EntityPM.ShipmentTypeId != newValue) {
            this.EntityPM.ShipmentTypeId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipmentTypeName = null;
            }

            else {
                var item = this.ShipmentTypesList.filter(f => f.Code == newValue)[0];
                if (item) {
                    this.ShipmentTypeName = item.Name;
                }
            }

            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.DelOrderDetails();
            this.OnFiltersChanged();
        }
    }

    get ShipmentLevelCode() { return this.EntityPM.ShipmentLevelCode; }
    set ShipmentLevelCode(newValue: string) {
        if (this.EntityPM.ShipmentLevelCode != newValue) {
            this.EntityPM.ShipmentLevelCode = newValue;
            
            if (newValue == "H") {
                this.Master = null;
                this.MainCarriageCarrierId = null;
                this.MainCarriageCarrierNumber = null;
            }

            this.SetScreenEnabled();
            this.SetUIProperties();            
            this.DelOrderDetails();
            this.ValidateMasterField();
            this.SetTransportModes();
        }
    }

    public FromTextCode: string;
    public ToTextCode: string;
    public CarrierTextCode: string;
    public CarrierNumberTextCode: string;
    public CarrierDependencyProperty1: string;
    public MasterTextCode: string;
    public HouseTextCode: string;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    SetLabels() {
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCode = "Shipment.S.NewShipment.Gateway";
                this.ToTextCode = "Shipment.S.NewShipment.Destination";
                this.CarrierTextCode = "Shipment.S.NewShipment.Airline";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.FlightNo";
                this.CarrierDependencyProperty1 = "AL";
                this.MasterTextCode = "Shipment.S.NewShipment.MAWB";
                this.HouseTextCode = "Shipment.S.NewShipment.HAWB";
                break;
            }

            case "O": {
                this.FromTextCode = "Shipment.S.NewShipment.LoadingPort";
                this.ToTextCode = "Shipment.S.NewShipment.DischargePort";
                this.CarrierTextCode = "Shipment.S.NewShipment.Shippingline";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.VoyageNo";
                this.CarrierDependencyProperty1 = "SL";
                this.MasterTextCode = "Shipment.S.NewShipment.OBL";
                this.HouseTextCode = "Shipment.S.NewShipment.FBL";
                break;
            }

            case "I": {
                this.FromTextCode = "Shipment.S.NewShipment.From";
                this.ToTextCode = "Shipment.S.NewShipment.To";
                this.CarrierTextCode = "Shipment.S.NewShipment.Trucker";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.TruckerNo";
                this.CarrierDependencyProperty1 = "TR";
                this.MasterTextCode = "Shipment.S.NewShipment.CMR/RWB#";
                this.HouseTextCode = "Shipment.F.House";
                break;
            }

            default: {
                this.FromTextCode = "Shipment.S.NewShipment.From";
                this.ToTextCode = "Shipment.S.NewShipment.To";
                this.CarrierTextCode = "Shipment.S.NewShipment.Carrier";
                this.CarrierNumberTextCode = "Shipment.S.NewShipment.No";
                this.MasterTextCode = "Shipment.S.NewShipment.MAWB";
                this.HouseTextCode = "Shipment.S.NewShipment.HAWB";
                break;
            }
        }

        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }

        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    }
    SetUnits() {
        var myDimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        var myVolumeUnitCode = this.TenantPM.VolumeUnitCode;
        var myGrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        var myChargeableWeightUnitCode = AppTool.GetChargeableWeightUnitCode(this.TransportModeId, this.ShipmentTypeId);

        if (this.DirectionId == "D") {
            if (!AppTool.IsNullOrEmpty(this.TenantPM.CountryCode)) {
                if (this.TenantPM.CountryCode.toUpperCase() == "US") {
                    myDimensionsUnitCode = "Inc";
                    myVolumeUnitCode = "CBI";
                    myGrossWeightUnitCode = "LB";
                    myChargeableWeightUnitCode = "LB";
                }
            }
        }

        if (this.EntityPM.IsCopyFromShipment || this.EntityPM.IsBuildFromQuote) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
                this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.GrossWeightUnitCode)) {
                this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeightUnitCode)) {
                this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
            }

            if (this.EntityPM.Ratio == null) {
                this.EntityPM.Ratio = AppTool.GetRatio(this.DirectionId, this.TransportModeId, this.ShipmentTypeId, this.TenantPM.CountryCode);
            }

            if (this.EntityPM.DimFactor == null) {
                this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
            }
        }

        else {
            this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
            this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
            this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
            this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
            this.EntityPM.Ratio = AppTool.GetRatio(this.DirectionId, this.TransportModeId, this.ShipmentTypeId, this.TenantPM.CountryCode);
            this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
            this.ComputeOrderVolumetricWeight();
            this.ComputeChargeableWeight();
        }
    }
    SetPartners() {
        if (this.IsCopyFromShipment) {
            this.CopyPartners();
        }
    }
    SetPrepaidCollect() {
        var myFreightPrepaidCollectId = null;
        var myOtherPrepaidCollectId = null;

        switch (this.DirectionId) {
            case "E":
            case "R": {
                myFreightPrepaidCollectId = this.TenantPM.ExportFreightPrepaidCollectId;
                myOtherPrepaidCollectId = this.TenantPM.ExportOtherPrepaidCollectId;
                break;
            }

            case "I": {
                myFreightPrepaidCollectId = this.TenantPM.ImportFreightPrepaidCollectId;
                myOtherPrepaidCollectId = this.TenantPM.ImportOtherPrepaidCollectId;
                break;
            }

            case "D": {
                myFreightPrepaidCollectId = "P";
                myOtherPrepaidCollectId= "P";
                break;
            }
        }

        if (this.IsCopyFromShipment || this.IsBuildFromQuote) {
            if (AppTool.IsNullOrEmpty(this.FreightPrepaidCollectId)) {
                this.FreightPrepaidCollectId = myFreightPrepaidCollectId;
            }

            if (AppTool.IsNullOrEmpty(this.OtherPrepaidCollectId)) {
                this.OtherPrepaidCollectId = myOtherPrepaidCollectId;
            }
        }

        else {
            this.FreightPrepaidCollectId = myFreightPrepaidCollectId;
            this.OtherPrepaidCollectId = myOtherPrepaidCollectId;
        }
    } 
    DelOrderDetails() {
        if (!this.IsCopyFromShipment && !this.IsBuildFromQuote) {
            this.Quantity1 = null;
            this.Quantity2 = null;
            this.Quantity3 = null;
            this.Quantity4 = null;
            this.Quantity5 = null;
            this.PackageTypeId1 = null;
            this.PackageTypeId2 = null;
            this.PackageTypeId3 = null;
            this.PackageTypeId4 = null;
            this.PackageTypeId5 = null;
            this.OrderGrossWeight = null;
            this.BookingVolume = null;
            this.OrderVolumetricWeight = null;
            this.OrderChargeableWeight = null;
            this.BookingNumberOfPackages = null;
        }
    }
    SetTransportModes() {
        if (this.ShipmentLevelCode == "H") {
            if (this.DirectionId == "D") {
                if (this.TransportModeId == "I") {
                    this.TransportModeId = null;
                }

                var itemIndex: number = this.TransportModesList.findIndex(f => f.Code == "I");
                if (itemIndex > -1) {
                    this.TransportModesList.splice(itemIndex, 1);
                }
            }

            else {
                var itemIndex: number = this.TransportModesList.findIndex(f => f.Code == "I");
                if (itemIndex == -1) {
                    this.TransportModesList.push(new FilterClass("I", "Inland"));
                }
            }
        }
    }
  
    SetUIProperties() {
        this.SetUIProperties_Filters();
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Ports();
        this.SetUIProperties_MasterField();
        this.SetUIProperties_HouseField();
        this.SetUIProperties_VesselField();
        this.SetUIProperties_OrderDetails();
    }
    SetUIProperties_Filters() {

        var isLevelsEnabled = false;

        if (!this.IsShipmentLevelFixed) {
            if (this.TransportModeId == "A") {
                isLevelsEnabled = true;
            }

            else if (this.ShipmentTypeId != null) {
                isLevelsEnabled = true;
            }
        }

        this.IsShipmentLevelsListEnabled = isLevelsEnabled;
    }
    SetUIProperties_Shipper() {
        var isFieldRequired: boolean = false;

        if (this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.ShipperId)) {
                isFieldRequired = true;
            }
        }
        
        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, isFieldRequired);
    }
    SetUIProperties_Consignee() {
        var isFieldRequired: boolean = false;

        if (this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isFieldRequired = true;
            }
        }
        
        this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, isFieldRequired);
    }
    
    SetUIProperties_Ports() {
        var isFromRequired: boolean = false;
        var isToRequired: boolean = false;

        if (!this.IsInlandDomestic) {
            if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                isFromRequired = true;
            }

            if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
                isToRequired = true;
            }
        }

        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, isToRequired);
    }
    SetUIProperties_MasterField() {
        var isFieldVisible = false;
        var isFieldEnabled = false;

        if (this.ShipmentLevelCode != "H") {
            if (!this.IsInlandDomestic) {
                isFieldVisible = true;
            }
        }

        if (this.IsScreenEnabled) {
            if (this.TransportModeId == "A") {
                if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                    isFieldEnabled = true;
                }
            }
            else {
                isFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetVisibility("Master", this.ObjectTableName, isFieldVisible);
    }
    SetUIProperties_HouseField() {
        var isFieldVisible = true;

        if (!AppTool.IsNullOrEmpty(this.TransportModeId)) {
            if (this.DirectionId == "E" || this.DirectionId == "D" || this.DirectionId == "R") {
                var settingsCode = "HAWBCounter" + this.TransportModeId + "_E_D";

                var settings: any = SessionLocator.TenantSettings.filter(f => f.SettingCode == settingsCode)[0];

                if (settings != null) {
                    var getHouseField = false;

                    if (this.ShipmentLevelCode == "H") {
                        getHouseField = true;
                    }

                    else if (this.ShipmentLevelCode == "D") {
                        if (!settings.DontIncludeDirects) {
                            getHouseField = true;
                        }
                    }

                    if (getHouseField) {
                        if (settings.SettingValue == "Shipment" || settings.SettingValue == "Counter") {
                            isFieldVisible = false;
                        }
                    }
                }
            }
        }

        this.UIProperties.SetVisibility("House", this.ObjectTableName, isFieldVisible);
    }
    SetUIProperties_VesselField() {
        var isFieldVisible = false;
        var isFieldEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.ShipmentLevelCode)) {
            if (this.ShipmentLevelCode != "H") {
                if (this.TransportModeId == "O") {
                    isFieldVisible = true;

                    if (this.IsScreenEnabled) {
                        if (this.ShipmentTypeId == "FCLD") {
                            isFieldEnabled = true;
                        }
                    }
                }
            }
        }

        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetVisibility("MainCarriageVesselId", this.ObjectTableName, isFieldVisible);
    }
    SetUIProperties_OrderDetails() {
        var isFieldsEnabled = false;

        if (this.IsScreenEnabled) {
            isFieldsEnabled = this.EntityPM.ShipmentOrderPackages.length == 0 ? true : false;
        }

        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isFieldsEnabled);
    }

    // Shipper
    private ShipperPartnerTypeId: string;
    private ShipperIsCustomer: boolean;
    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(newValue: string) {
        if (this.EntityPM.ShipperId != newValue) {
            this.EntityPM.ShipperId = newValue;

            if (this.ShipmentCustomerTypeCode == "SHI") {
                this.CustomerId = newValue;
            }

            this.SetUIProperties_Shipper();            

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ShipperPartnerTypeId = null;
                this.ShipperContactId = null;
                this.EntityPM.ShipperName = null;
                this.EntityPM.ShipperNote = null;
                this.EntityPM.ShipperReference1 = null;
                this.EntityPM.ShipperReference2 = null;
                this.EntityPM.ShipperMainAddressId = null;
                this.EntityPM.ShipperPickAddressId = null;
                this.EntityPM.KnownConsignorNumber = null;
                this.EntityPM.KCExpirationDate = null;
                this.ShipperAddressId = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                            this.ShipperContactId = myCardList.PrimaryContactId;
                            this.EntityPM.ShipperName = myCardList.EnglishName;
                            this.EntityPM.ShipperNote = myCardList.Notes;
                            this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                            this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                            this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                            this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                            this.ShipperAddressId = myCardList.MainAddressId;
                            this.ShipperIsCustomer = myCardList.IsCustomer;
                        }
                    }
                });
            }
        }
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
                        this.ShipperAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    private myShipperAddressList: AddressList;
    get ShipperAddressList() { return this.myShipperAddressList; }
    set ShipperAddressList(newValue: AddressList) {
        this.myShipperAddressList = newValue;
        this.UpdatePickUpAddressFields();
    }

    get ShipperContactId() { return this.EntityPM.ShipperContactId; }
    set ShipperContactId(newValue: string) {
        if (this.EntityPM.ShipperContactId != newValue) {
            this.EntityPM.ShipperContactId = newValue;
        }
    }

    get ShipperReference1() { return this.EntityPM.ShipperReference1; }
    set ShipperReference1(newValue: string) {
        if (this.EntityPM.ShipperReference1 != newValue) {
            this.EntityPM.ShipperReference1 = newValue;
        }
    }

    get ShipperReference2() { return this.EntityPM.ShipperReference2; }
    set ShipperReference2(newValue: string) {
        if (this.EntityPM.ShipperReference2 != newValue) {
            this.EntityPM.ShipperReference2 = newValue;
        }
    }

    get ShipperName() { return this.EntityPM.ShipperName; }
    set ShipperName(newValue: string) {
        if (this.EntityPM.ShipperName != newValue) {
            this.EntityPM.ShipperName = newValue;
        }
    }

    // Consignee
    private ConsigneePartnerTypeId: string;
    private ConsigneeIsCustomer: boolean;
    get ConsigneeId() { return this.EntityPM.ConsigneeId; }
    set ConsigneeId(newValue: string) {
        if (this.EntityPM.ConsigneeId != newValue) {
            this.EntityPM.ConsigneeId = newValue;

            if (this.ShipmentCustomerTypeCode == "CON") {
                this.CustomerId = newValue;
            }

            this.SetUIProperties_Consignee();            

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ConsigneePartnerTypeId = null;
                this.ConsigneeContactId = null;
                this.EntityPM.ConsigneeName = null;
                this.EntityPM.ConsigneeNote = null;
                this.EntityPM.ConsigneeReference1 = null;
                this.EntityPM.ConsigneeReference2 = null;
                this.EntityPM.ConsigneeMainAddressId = null;
                this.EntityPM.ConsigneePickAddressId = null;
                this.ConsigneeAddressId = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                            this.ConsigneeContactId = myCardList.PrimaryContactId;
                            this.EntityPM.ConsigneeName = myCardList.EnglishName;
                            this.EntityPM.ConsigneeNote = myCardList.Notes;
                            this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                            this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                            this.ConsigneeAddressId = myCardList.MainAddressId;
                            this.ConsigneeIsCustomer = myCardList.IsCustomer;
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
                        this.ConsigneeAddressList = myResponse.Result;
                    }
                });
            }
        }
    }

    private myConsigneeAddressList: AddressList;
    get ConsigneeAddressList() { return this.myConsigneeAddressList; }
    set ConsigneeAddressList(newValue: AddressList) {
        this.myConsigneeAddressList = newValue;
        this.UpdateDeliveryAddressFields();
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

    private isShipperMyCustomer: boolean = false;
    get IsShipperMyCustomer() { return this.isShipperMyCustomer; }
    set IsShipperMyCustomer(value: boolean) {
        if (this.isShipperMyCustomer != value) {
            this.isShipperMyCustomer = value;

            if (value) {
                if (this.ShipmentCustomerTypeCode == "SHI") {
                    this.CustomerId = this.ShipperId;
                }
            }

            this.SetSalesman();
            this.SetUIProperties_Shipper();
            this.SetUIProperties_Consignee();
        }
    }

    private isConsigneeMyCustomer: boolean = false;
    get IsConsigneeMyCustomer() { return this.isConsigneeMyCustomer; }
    set IsConsigneeMyCustomer(value: boolean) {
        if (this.isConsigneeMyCustomer != value) {
            this.isConsigneeMyCustomer = value;

            if (value) {
                if (this.ShipmentCustomerTypeCode == "CON") {
                    this.CustomerId = this.ConsigneeId;
                }
            }

            this.SetSalesman();
            this.SetUIProperties_Shipper();
            this.SetUIProperties_Consignee();
        }
    }

    private myCustomerSalesmanId: string;
    private myCustomerAccountManagerUserId: string;
    SetSalesman() {
        this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.myCustomerSalesmanId) ? this.EntityPM.CreatedByUserId : this.myCustomerSalesmanId;
        this.EntityPM.AccountManagerUserId = this.myCustomerAccountManagerUserId != null ? this.myCustomerAccountManagerUserId : SessionLocator.LoggedUserId;        
    }
    SetCustomer(myCode: string) {
        if (myCode == 'SHI') {
            this.IsShipperMyCustomer = true;
            this.IsConsigneeMyCustomer = false;
        }

        else if (myCode == 'CON') {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = true;
        }

        else {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = false;
        }
    }

    //Customer
    public CustomerDependencyProperty1: string = "CS";
    public CustomerDependencyProperty1IsList: boolean = false;
    private ComputeCustomerDependency() {
        switch (this.ShipmentCustomerTypeCode) {
            case "SHI":
            case "CON":
                {
                    this.CustomerDependencyProperty1 = "CS";
                    this.CustomerDependencyProperty1IsList = false;

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        this.CustomerDependencyProperty1 = "AG";
                        this.CustomerDependencyProperty1IsList = false;
                    }

                    else {
                        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            this.CustomerDependencyProperty1 = "CS,AG";
                            this.CustomerDependencyProperty1IsList = true;
                        }
                    }

                    break;
                }

            case "AGT":
                {
                    if (SessionLocator.TenantPM.AllowCustomersInAgentsLOV) {
                        this.CustomerDependencyProperty1 = "AG,CS";
                        this.CustomerDependencyProperty1IsList = true;
                    }

                    else {
                        this.CustomerDependencyProperty1 = "AG";
                        this.CustomerDependencyProperty1IsList = false;
                    }

                    break;
                }

            case "IGT":
            case "FOR":
            case "COL":
                {
                    this.CustomerDependencyProperty1 = "AG";
                    this.CustomerDependencyProperty1IsList = false;
                    break;
                }

            case "CAE":
            case "CAI":
                {
                    this.CustomerDependencyProperty1 = "CG";
                    this.CustomerDependencyProperty1IsList = false;
                    break;
                }

            case "NT1":
            case "NT2":
            case "REA":
                {
                    this.CustomerDependencyProperty1 = "AG,AL,CG,CS,SG,SL,TR,VD,WH";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }

            case "SNE":
            case "CNI":
                {
                    this.CustomerDependencyProperty1 = "AG,CS";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }
            case "CSD":
                {
                    this.CustomerDependencyProperty1 = "AG,CS,SG";
                    this.CustomerDependencyProperty1IsList = true;
                    break;
                }

            case "CCP": {
                this.CustomerDependencyProperty1 = "WH";
                this.CustomerDependencyProperty1IsList = false;
                break;
            }

            case "OTH": {
                this.CustomerDependencyProperty1 = "CS";
                this.CustomerDependencyProperty1IsList = false;
                break;
            }
        }
    }

    get ShipmentCustomerTypeCode() { return this.EntityPM.ShipmentCustomerTypeCode; }
    set ShipmentCustomerTypeCode(newValue: string) {
        if (this.EntityPM.ShipmentCustomerTypeCode != newValue) {
            this.EntityPM.ShipmentCustomerTypeCode = newValue;

            this.CustomerId = null;

            this.SetCustomer(newValue);
            this.SetCustomerRequired();
            this.ComputeCustomerDependency();
        }
    }

    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;

            this.SetCustomerRequired();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.CustomerContactId = null;
                this.EntityPM.CustomerName = null;
                this.EntityPM.CustomerNote = null;
                this.CustomerAddressId = null;
                this.myCustomerSalesmanId = null;
                this.myCustomerAccountManagerUserId = null;
                this.SetSalesman();
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {                            
                            this.CustomerContactId = myCardList.PrimaryContactId;
                            this.EntityPM.CustomerName = myCardList.EnglishName;
                            this.EntityPM.CustomerNote = myCardList.Notes;
                            this.CustomerAddressId = myCardList.MainAddressId;
                            this.myCustomerSalesmanId = myCardList.SalesmanUserId;
                            this.myCustomerAccountManagerUserId = myCardList.AccountManagerUserId;
                            this.SetSalesman();
                            this.SetCustomerPartner();                     
                        }
                    }
                });
            }
        }
    }

    get CustomerAddressId() { return this.EntityPM.CustomerAddressId; }
    set CustomerAddressId(newValue: string) {
        if (this.EntityPM.CustomerAddressId != newValue) {
            this.EntityPM.CustomerAddressId = newValue;            
        }
    }

    get CustomerContactId() { return this.EntityPM.CustomerContactId; }
    set CustomerContactId(newValue: string) {
        if (this.EntityPM.CustomerContactId != newValue) {
            this.EntityPM.CustomerContactId = newValue;
        }
    }

    private SetCustomerPartner() {
        switch (this.ShipmentCustomerTypeCode) {
            case "SHI": {
                this.ShipperId = this.CustomerId;
                break;
            }

            case "CON": {
                this.ConsigneeId = this.CustomerId;
                break;
            }

            case "AGT": {
                this.EntityPM.AgentId = this.CustomerId;
                this.EntityPM.AgentAddressId = this.CustomerAddressId;
                this.EntityPM.AgentContactId = this.CustomerContactId;
                break;
            }

            case "IGT": {
                this.EntityPM.IssuingCarrierAgentId = this.CustomerId;
                this.EntityPM.IssuingCarrierAddressId = this.CustomerAddressId;
                break;
            }

            case "FOR": {
                this.EntityPM.FreightForwarderId = this.CustomerId;
                this.EntityPM.FreightForwarderAddressId = this.CustomerAddressId;
                this.EntityPM.FreightForwarderContactId = this.CustomerContactId;
                break;
            }

            case "COL": {
                this.EntityPM.ColoaderId = this.CustomerId;
                this.EntityPM.ColoaderAddressId = this.CustomerAddressId;
                this.EntityPM.ColoaderContactId = this.CustomerContactId;
                break;
            }

            case "CAE": {
                this.EntityPM.CustomAgentExportId = this.CustomerId;
                this.EntityPM.CustomAgentExportAddressId = this.CustomerAddressId;
                this.EntityPM.CustomAgentExportContactId = this.CustomerContactId;
                break;
            }

            case "CAI": {
                this.EntityPM.CustomAgentImportId = this.CustomerId;
                this.EntityPM.CustomAgentImportAddressId = this.CustomerAddressId;
                this.EntityPM.CustomAgentImportContactId = this.CustomerContactId;
                break;
            }

            case "NT1": {
                this.EntityPM.Notify1Id = this.CustomerId;
                this.EntityPM.Notify1AddressId = this.CustomerAddressId;
                this.EntityPM.Notify1ContactId = this.CustomerContactId;
                break;
            }

            case "NT2": {
                this.EntityPM.Notify2Id = this.CustomerId;
                this.EntityPM.Notify2AddressId = this.CustomerAddressId;
                this.EntityPM.Notify2ContactId = this.CustomerContactId;
                break;
            }

            case "REA": {
                this.EntityPM.ReleasingAgentId = this.CustomerId;
                this.EntityPM.ReleasingAgentAddressId = this.CustomerAddressId;
                this.EntityPM.ReleasingAgentContactId = this.CustomerContactId;
                break;
            }

            case "SNE": {
                this.EntityPM.ShipperNotExporterId = this.CustomerId;
                this.EntityPM.ShipperNotExporterAddressId = this.CustomerAddressId;
                this.EntityPM.ShipperNotExporterContactId = this.CustomerContactId;
                break;
            }

            case "CNI": {
                this.EntityPM.ConsigneeNotImporterId = this.CustomerId;
                this.EntityPM.ConsigneeNotImporterAddressId = this.CustomerAddressId;
                this.EntityPM.ConsigneeNotImporterContactId = this.CustomerContactId;
                break;
            }

            case "CSD": {
                this.EntityPM.ConsolidatorId = this.CustomerId;
                this.EntityPM.ConsolidatorAddressId = this.CustomerAddressId;
                this.EntityPM.ConsolidatorContactId = this.CustomerContactId;
                break;
            }

            case "CCP": {
                this.EntityPM.CustomClearancePointId = this.CustomerId;
                this.EntityPM.CustomClearancePointAddressId = this.CustomerAddressId;
                this.EntityPM.CustomClearancePointContactId = this.CustomerContactId;
                break;
            }

            case "OTH": {
                
                break;
            }
        }
    }

    public IsCustomerRequired: boolean = false;
    private SetCustomerRequired() {
        var isRequired: boolean = false

        if (AppTool.IsNullOrEmpty(this.CustomerId) || AppTool.IsNullOrEmpty(this.ShipmentCustomerTypeCode)) {
            isRequired = true;
        }

        this.IsCustomerRequired = isRequired;
    }

    // Add|Edit Partner
    AddPartnerClicked(myPartnerCode: string) {
        var myComponentPath: string = null;
        var title = "";
        var isCustomer = false;
        
        if (myPartnerCode == "S") {
            title = "New Shipper";
            isCustomer = this.IsShipperMyCustomer;
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        }

        else if (myPartnerCode == "C") {
            title = "New Consignee";
            isCustomer = this.IsConsigneeMyCustomer;
            myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        }

        else if (myPartnerCode == "T") {
            if (this.ShipmentCustomerTypeCode == "IGT") {
                var myTitle = "Add Issuing Carrier's Agent";

                var windowArgs = new AddEditPartnerArgs();
                windowArgs.EntityPM = this.EntityPM;
                windowArgs.IsNewEntity = true;
                windowArgs.PartnerTypeCode = "AGT";

                var logWindow = new LogitudeWindow();
                logWindow.Title = myTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');

                logWindow.ComponentLoaded.subscribe(cmp => {
                    logWindow.WindowClosed.subscribe(($event: any) => {

                        if (cmp.IsUpdatingPartner) {

                            var myPartnerId = cmp.CurrentPartnerId;
                            var myAddressId = cmp.CurrentAddressId;

                            if (this.EntityPM.IssuingCarrierAgentId != myPartnerId) {
                                this.EntityPM.IssuingCarrierAgentId = myPartnerId;
                            }

                            else {
                                this.EntityPM.IssuingCarrierAddressId = myAddressId;
                            }
                        }
                    });
                });
            }

            else {
                title = "New " + this.ComputeAddCustomerTitle();

                if (this.CustomerDependencyProperty1 == "AG") {
                    myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
                }

                else if (this.CustomerDependencyProperty1 == "WH") {
                    myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewWarehouseComponent";
                }

                else if (this.ShipmentCustomerTypeCode == "CAE" || this.ShipmentCustomerTypeCode == "CAI") {
                    myComponentPath = "./CommonModules/CommonPartners/Components/NewEntity/NewCustomAgentComponent";
                }

                else {
                    myComponentPath = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";

                    if (this.ShipmentCustomerTypeCode == "SHI") {
                        isCustomer = this.IsShipperMyCustomer;
                    }

                    else if (this.ShipmentCustomerTypeCode == "CON") {
                        isCustomer = this.IsConsigneeMyCustomer;
                    }
                }
            }         
        }

        var args = new NewEntityArgs();
        if (!isCustomer) {
            args.Perspective = "ShippersAndConsignees";
        }

        var logeWindow = new LogitudeWindow();
        logeWindow.Width = 960;
        logeWindow.Height = 600;
        logeWindow.Title = title;
        logeWindow.WindowArgs = args;
        logeWindow.Show(myComponentPath);

        logeWindow.ComponentLoaded.subscribe(comp => {
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {

                    if (myPartnerCode == "S") {
                        this.ShipperId = comp.EntityPM.Id;
                        this.ShipperName = comp.EntityPM.EnglishName;
                        this.myCustomerSalesmanId = comp.EntityPM.SalesmanUserId;
                        this.myCustomerAccountManagerUserId = comp.EntityPM.AccountManagerUserId;
                    }

                    else if (myPartnerCode == "C") {
                        this.ConsigneeId = comp.EntityPM.Id;
                        this.ConsigneeName = comp.EntityPM.EnglishName;
                        this.myCustomerSalesmanId = comp.EntityPM.SalesmanUserId;
                        this.myCustomerAccountManagerUserId = comp.EntityPM.AccountManagerUserId;
                    }

                    else if (myPartnerCode == "T") {        
                        this.CustomerId = comp.EntityPM.Id;
                        this.myCustomerSalesmanId = comp.EntityPM.SalesmanUserId;
                        this.myCustomerAccountManagerUserId = comp.EntityPM.AccountManagerUserId;

                        if (this.ShipmentCustomerTypeCode == "SHI") {
                            this.ShipperId = comp.EntityPM.Id;
                            this.ShipperName = comp.EntityPM.EnglishName;
                        }

                        else if (this.ShipmentCustomerTypeCode == "CON") {
                            this.ConsigneeId = comp.EntityPM.Id;
                            this.ConsigneeName = comp.EntityPM.EnglishName;
                        }
                    }
                }
            });
        });
    }

    private ComputeAddCustomerTitle(): string {
        var myResult: string = "";

        switch (this.ShipmentCustomerTypeCode) {
            case "AGT":
                {
                    myResult = "Agent";
                    break
                }

            case "CAE":
                {
                    myResult = "Custom's Agent Export";
                    break
                }

            case "CAI":
                {
                    myResult = "Custom's Agent Import";
                    break
                }

            case "CCP":
                {
                    myResult = "Custom Clearance Point";
                    break
                }

            case "CNI":
                {
                    myResult = "Consignee Not Importer";
                    break
                }

            case "COL":
                {
                    myResult = "Coloader";
                    break
                }

            case "CON":
                {
                    myResult = "Consignee";
                    break
                }

            case "CSD":
                {
                    myResult = "Consolidator";
                    break
                }

            case "FOR":
                {
                    myResult = "Freight Forwarder";
                    break
                }

            case "IGT":
                {
                    myResult = "Issuing Carrier Agent";
                    break
                }

            case "NT1":
                {
                    myResult = "Notify 1";
                    break
                }

            case "NT2":
                {
                    myResult = "Notify 2";
                    break
                }

            case "REA":
                {
                    myResult = "Releasing Agent";
                    break
                }

            case "SHI":
                {
                    myResult = "Shipper";
                    break
                }

            case "SNE":
                {
                    myResult = "Shipper Not Exporter";
                    break
                }
        }

        return myResult;
    }

    // Pickup
    get IncludePickUp() { return this.EntityPM.IncludePickUp; }
    set IncludePickUp(newValue: boolean) {
        if (this.EntityPM.IncludePickUp != newValue) {
            this.EntityPM.IncludePickUp = newValue;
            this.UpdatePickUpAddressFields();
        }
    }

    get PickUpAddressId() { return this.EntityPM.PickUpAddressId; }
    set PickUpAddressId(newValue: string) {
        if (this.EntityPM.PickUpAddressId != newValue) {
            this.EntityPM.PickUpAddressId = newValue;

            this.LoadPickupAddress();

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.FromAddressCity = null;
                this.FromAddressZipCode = null;
                this.FromAddressCountryId = null;
            }

            this.SetUIProperties_PickupFields();
        }
    }

    public PickupAddressList: AddressList;
    LoadPickupAddress() {
        if (this.PickUpAddressId == null) {
            this.PickupAddressList = null;
        }

        else {
            this.myAddressListService.getSingle(this.PickUpAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.PickupAddressList = myResponse.Result;
                }
            });
        }
    }

    get FromAddressCity() { return this.EntityPM.FromAddressCity; }
    set FromAddressCity(newValue: string) {
        if (this.EntityPM.FromAddressCity != newValue) {
            this.EntityPM.FromAddressCity = newValue;
            this.SetUIProperties_PickupFields();
        }
    }

    get FromAddressZipCode() { return this.EntityPM.FromAddressZipCode; }
    set FromAddressZipCode(newValue: string) {
        if (this.EntityPM.FromAddressZipCode != newValue) {
            this.EntityPM.FromAddressZipCode = newValue;
            this.SetUIProperties_PickupFields();
        }
    }

    get FromAddressCountryId() { return this.EntityPM.FromAddressCountryId; }
    set FromAddressCountryId(newValue: string) {
        if (this.EntityPM.FromAddressCountryId != newValue) {
            this.EntityPM.FromAddressCountryId = newValue;
            this.SetUIProperties_PickupFields();
        }
    }

    private isFirstTimeFromQuotePickup: boolean = true;
    UpdatePickUpAddressFields() {

        if (!this.IncludePickUp) {
            this.EntityPM.PickUpAddressId = null;
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.PickupAddressList = null;
        }

        else if (this.EntityPM.IsBuildFromQuote && this.isFirstTimeFromQuotePickup) {
            if (AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = this.SourceEntityPM.FromAddressCity;
                this.EntityPM.FromAddressZipCode = this.SourceEntityPM.FromAddressZipCode;
                this.EntityPM.FromAddressCountryId = this.SourceEntityPM.FromAddressCountryId;
            }

            else if (this.ShipperAddressList != null) {
                this.isFirstTimeFromQuotePickup = false;
                this.PickupAddressList = this.ShipperAddressList;
                this.EntityPM.PickUpAddressId = this.ShipperAddressList.Id;
            }
        }

        else {
            this.PickupAddressList = null;
            this.EntityPM.PickUpAddressId = null;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.ShipperPickAddressId)) {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperPickAddressId;
            }

            else {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperMainAddressId;
            }

            if (!AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }

            this.LoadPickupAddress();
        }

        this.SetUIProperties_PickupFields();
    }
    SetUIProperties_PickupFields() {
        var isCityRequired: boolean = false;
        var isCountryRequired: boolean = false;

        if (this.IncludePickUp) {

            var validateFields: boolean = false;

            if (AppTool.IsNullOrEmpty(this.ShipperId)) {
                validateFields = true;
            }

            else if (!AppTool.IsNullOrEmpty(this.ShipperId) && AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                validateFields = true;
            }

            if (validateFields) {
                if (AppTool.IsNullOrEmpty(this.FromAddressCity) && AppTool.IsNullOrEmpty(this.FromAddressZipCode)) {
                    isCityRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.FromAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, isCountryRequired);
    }

    // Delivery
    get IncludeDelivery() { return this.EntityPM.IncludeDelivery; }
    set IncludeDelivery(newValue: boolean) {
        if (this.EntityPM.IncludeDelivery != newValue) {
            this.EntityPM.IncludeDelivery = newValue;
            this.UpdateDeliveryAddressFields();
        }
    }

    get DeliveryAddressId() { return this.EntityPM.DeliveryAddressId; }
    set DeliveryAddressId(newValue: string) {
        if (this.EntityPM.DeliveryAddressId != newValue) {
            this.EntityPM.DeliveryAddressId = newValue;

            this.LoadDeliveryAddress();

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.ToAddressCity = null;
                this.ToAddressZipCode = null;
                this.ToAddressCountryId = null;
            }

            this.SetUIProperties_DeliveryFields();
        }
    }

    public DeliveryAddressList: AddressList;
    LoadDeliveryAddress() {
        if (this.DeliveryAddressId == null) {
            this.DeliveryAddressList = null;
        }

        else {
            this.myAddressListService.getSingle(this.DeliveryAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.DeliveryAddressList = myResponse.Result;
                }
            });
        }
    }

    get ToAddressCity() { return this.EntityPM.ToAddressCity; }
    set ToAddressCity(newValue: string) {
        if (this.EntityPM.ToAddressCity != newValue) {
            this.EntityPM.ToAddressCity = newValue;
            this.SetUIProperties_DeliveryFields();
        }
    }

    get ToAddressZipCode() { return this.EntityPM.ToAddressZipCode; }
    set ToAddressZipCode(newValue: string) {
        if (this.EntityPM.ToAddressZipCode != newValue) {
            this.EntityPM.ToAddressZipCode = newValue;
            this.SetUIProperties_DeliveryFields();
        }
    }

    get ToAddressCountryId() { return this.EntityPM.ToAddressCountryId; }
    set ToAddressCountryId(newValue: string) {
        if (this.EntityPM.ToAddressCountryId != newValue) {
            this.EntityPM.ToAddressCountryId = newValue;
            this.SetUIProperties_DeliveryFields();
        }
    }

    private isFirstTimeFromQuoteDelivery: boolean = true;
    UpdateDeliveryAddressFields() {
        if (!this.IncludeDelivery) {
            this.EntityPM.DeliveryAddressId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.DeliveryAddressList = null;
        }

        else if (this.EntityPM.IsBuildFromQuote && this.isFirstTimeFromQuoteDelivery) {
            if (AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.ToAddressCity = this.SourceEntityPM.ToAddressCity;
                this.EntityPM.ToAddressZipCode = this.SourceEntityPM.ToAddressZipCode;
                this.EntityPM.ToAddressCountryId = this.SourceEntityPM.ToAddressCountryId;
            }

            else if (this.ConsigneeAddressList != null) {
                this.isFirstTimeFromQuoteDelivery = false;
                this.DeliveryAddressList = this.ConsigneeAddressList;
                this.EntityPM.DeliveryAddressId = this.ConsigneeAddressList.Id;
            }
        }

        else {
            this.DeliveryAddressList = null;
            this.EntityPM.DeliveryAddressId = null;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePickAddressId)) {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneePickAddressId;
            }

            else {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneeMainAddressId;
            }

            if (!AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }

            this.LoadDeliveryAddress();
        }

        this.SetUIProperties_DeliveryFields();
    }
    SetUIProperties_DeliveryFields() {
        var isCityRequired: boolean = false;
        var isCountryRequired: boolean = false;

        if (this.IncludeDelivery) {
            var validateFields: boolean = false;

            if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                validateFields = true;
            }

            else if (!AppTool.IsNullOrEmpty(this.ConsigneeId) && AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                validateFields = true;
            }

            if (validateFields) {
                if (AppTool.IsNullOrEmpty(this.ToAddressCity) && AppTool.IsNullOrEmpty(this.ToAddressZipCode)) {
                    isCityRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.ToAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, isCountryRequired);
    }

    // Select City
    SelectCityCommand(myAddressCode: string) {

        var mySourceCountryId: string = myAddressCode == "P" ? this.FromAddressCountryId : this.ToAddressCountryId;

        var args = new CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                if (myAddressCode == "P") {
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

    // Add|Edit Address
    EditAddressClicked(myAddressCode: string) {
        var myAddressId: string = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;

        switch (myAddressCode) {
            case "S": {
                myAddressId = this.ShipperAddressId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.ShipperIsCustomer;
                break;
            }

            case "C": {
                myAddressId = this.ConsigneeAddressId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = this.ConsigneeIsCustomer;
                break;
            }

            case "P": {
                if (this.IncludePickUp) {
                    myAddressId = this.PickUpAddressId;
                    myPartnerTypeId = this.ShipperPartnerTypeId;
                    isCustomer = this.ShipperIsCustomer;
                }

                break;
            }

            case "D": {
                if (this.IncludeDelivery) {
                    myAddressId = this.DeliveryAddressId;
                    myPartnerTypeId = this.ConsigneePartnerTypeId;
                    isCustomer = this.ConsigneeIsCustomer;
                }

                break;
            }
        }

        if (!AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "S": {
                            this.ShipperAddressId = null;
                            this.ShipperAddressId = myAddressId;
                            break;
                        }

                        case "C": {
                            this.ConsigneeAddressId = null;
                            this.ConsigneeAddressId = myAddressId;
                            break;
                        }

                        case "P": {
                            if (this.PickUpAddressId == this.ShipperAddressId) {
                                this.ShipperAddressId = null;
                                this.ShipperAddressId = myAddressId;
                            }

                            else {
                                this.PickUpAddressId = null;
                                this.PickUpAddressId = myAddressId;
                            }

                            break;
                        }

                        case "D": {
                            if (this.DeliveryAddressId == this.ConsigneeAddressId) {
                                this.ConsigneeAddressId = null;
                                this.ConsigneeAddressId = myAddressId;
                            }

                            else {
                                this.DeliveryAddressId = null;
                                this.DeliveryAddressId = myAddressId;
                            }

                            break;
                        }
                    }
                }
            });
        }
    }
    AddAddressClicked(myAddressCode: string) {
        var entityPM: AddressPM = null;
        var myPartnerTypeId: string = null;
        var isCustomer: boolean;

        switch (myAddressCode) {
            case "S": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ShipperId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.ShipperIsCustomer;
                break;
            }

            case "C": {
                entityPM = new AddressPM();
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ConsigneeId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = this.ConsigneeIsCustomer;
                break;
            }

            case "P": {
                if (this.IncludePickUp) {
                    if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
                        entityPM = new AddressPM();
                        entityPM.Tenant = SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = this.ShipperId;
                        myPartnerTypeId = this.ShipperPartnerTypeId;
                        isCustomer = this.ShipperIsCustomer;
                    }
                }

                break;
            }

            case "D": {
                if (this.IncludeDelivery) {
                    if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                        entityPM = new AddressPM();
                        entityPM.Tenant = SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = this.ConsigneeId;
                        myPartnerTypeId = this.ConsigneePartnerTypeId;
                        isCustomer = this.ConsigneeIsCustomer;
                    }
                }

                break;
            }
        }

        if (entityPM != null) {
            var logeWindow = new LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    switch (myAddressCode) {
                        case "S": {
                            this.ShipperAddressId = null;
                            this.ShipperAddressId = entityPM.Id;
                            break;
                        }

                        case "C": {
                            this.ConsigneeAddressId = null;
                            this.ConsigneeAddressId = entityPM.Id;
                            break;
                        }

                        case "P": {
                            this.PickUpAddressId = null;
                            this.PickUpAddressId = entityPM.Id;
                            break;
                        }

                        case "D": {
                            this.DeliveryAddressId = null;
                            this.DeliveryAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    }

    // Main Carriage
    public FromPortList: PortList = null;
    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(value: string) {
        if (this.EntityPM.MainCarriageFromPortId != value) {
            this.EntityPM.MainCarriageFromPortId = value;
            this.EntityPM.FromPortId = value;
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                this.FromPortList = null;
            }

            else {
                this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.FromPortList = myResponse.Result;
                    }
                });
            }
        }
    }

    public ToPortList: PortList = null;
    get MainCarriageToPortId() { return this.EntityPM.MainCarriageToPortId; }
    set MainCarriageToPortId(value: string) {
        if (this.EntityPM.MainCarriageToPortId != value) {
            this.EntityPM.MainCarriageToPortId = value;
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToPortList = null;
            }

            else {
                this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.ToPortList = myResponse.Result;
                    }
                });
            }
        }
    }

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;
            this.SetUIProperties_MasterField();

            if (!newValue) {
                if (this.TransportModeId == "A") {
                    this.Master = null;
                }

                this.AirlinePrefix = null;
                this.LongMaster = null;
                this.AccountNumber = null;
                this.EntityPM.MainCarriageCarrierCode = null;
                this.EntityPM.MainCarriageCarrierName = null;
                this.EntityPM.MainCarriageCarrierNumber = null;
                this.EntityPM.MainCarriageCarrierPrefix = null;
                this.EntityPM.CarrierIsCheckDigit = false;
                this.EntityPM.CarrierIsLimitedLength = false;
                this.EntityPM.ReleasingAgentContactId = null;
                this.EntityPM.ReleasingAgentName = null;
                this.EntityPM.ReleasingAgentNote = null;
                this.EntityPM.ReleasingAgentAddressId = null;
                this.EntityPM.ReleasingAgentId = null;
                ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myCardListResponse: ServiceResponse) => {
                    if (!myCardListResponse.HasError) {

                        var myCardList: CardList = myCardListResponse.Result;
                        if (myCardList) {

                            this.EntityPM.MainCarriageCarrierCode = myCardList.Code;
                            this.EntityPM.MainCarriageCarrierName = myCardList.EnglishName;
                            this.EntityPM.MainCarriageCarrierWebSite = myCardList.WebSite;

                            this.EntityPM.ReleasingAgentId = newValue;
                            this.EntityPM.ReleasingAgentContactId = myCardList.PrimaryContactId;
                            this.EntityPM.ReleasingAgentName = myCardList.EnglishName;
                            this.EntityPM.ReleasingAgentNote = myCardList.Notes;
                            this.EntityPM.ReleasingAgentAddressId = myCardList.MainAddressId;

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
                            else if (this.EntityPM.TransportModeId == "O") {
                                this.myShippingLineService.get(this.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
                                    if (myResponse != null) {
                                        if (!myResponse.HasError) {
                                            var line: ShippingLinePM = myResponse.Result;
                                            var agentId = line.ShippingAgentId;
                                            if (agentId != null) {
                                                this.myCardListService.getSingle(agentId).subscribe((myResponse: ServiceResponse) => {
                                                    if (myResponse != null) {
                                                        if (!myResponse.HasError) {
                                                            var agent: CardList = myResponse.Result;
                                                            this.EntityPM.ReleasingAgentId = agent.Id;
                                                            this.EntityPM.ReleasingAgentContactId = agent.PrimaryContactId;
                                                            this.EntityPM.ReleasingAgentName = agent.EnglishName;
                                                            this.EntityPM.ReleasingAgentNote = agent.Notes;
                                                            this.EntityPM.ReleasingAgentAddressId = agent.MainAddressId;
                                                        }
                                                    }
                                                });
                                            }
                                            else {
                                                this.EntityPM.ReleasingAgentId = line.Id;
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                });
            }
        }
    }

    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    set MainCarriageCarrierNumber(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
            this.EntityPM.MainCarriageCarrierNumber = newValue;
        }
    }

    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(newValue: string) {
        if (this.EntityPM.AirlinePrefix != newValue) {
            this.EntityPM.AirlinePrefix = newValue;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(newValue: string) {
        if (this.EntityPM.Master != newValue) {
            this.EntityPM.Master = newValue;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get LongMaster() { return this.EntityPM.LongMaster; }
    set LongMaster(newValue: string) {
        if (this.EntityPM.LongMaster != newValue) {
            this.EntityPM.LongMaster = newValue;
            this.ValidateMasterField();
        }
    }

    public IsMasterFieldValid: boolean = true;
    public IsValidatingMasterField: boolean = false;
    public MasterFieldValidityMessage: string = null;
    private myShipmentDomainService: ShipmentDomainService;
    private ValidateMasterField() {

        this.IsMasterFieldValid = true;
        this.IsValidatingMasterField = false;
        this.MasterFieldValidityMessage = null;

        if (this.ShipmentLevelCode != "H") {
            if (this.TransportModeId == "A") {
                if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
                        this.IsValidatingMasterField = true;
                    }
                }
            }
        }

        if (this.IsValidatingMasterField) {
            if (AppTool.IsNullOrEmpty(this.Master)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
            }

            else {
                var myResult: string = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);

                if (!AppTool.IsNullOrEmpty(myResult)) {
                    this.IsMasterFieldValid = false;
                    this.MasterFieldValidityMessage = myResult;
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
                }

                else {

                    this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");

                    if (!AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
                        this.ValidateMasterFieldIsUsed();
                    }
                }
            }
        }
    }
    private ValidateMasterFieldIsUsed() {
        if (this.myShipmentDomainService == null) {
            this.myShipmentDomainService = new ShipmentDomainService();
        }

        if (!AppTool.IsNullOrEmpty(this.Master)) {
            this.myShipmentDomainService.ValidateShipmentMasterFieldExistance(this.EntityPM.Id, this.EntityPM.BookingId, this.EntityPM.Master, this.EntityPM.AirlinePrefix, this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode, this.EntityPM.IsCancelled)
                .subscribe((myResult: any) => {

                    if (AppTool.IsNullOrEmpty(myResult)) {
                        this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                    }

                    else {
                        this.IsMasterFieldValid = false;
                        this.MasterFieldValidityMessage = myResult;
                        this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
                    }
                });
        }
    }

    get MAWBOBLDate() { return this.EntityPM.MAWBOBLDate; }
    set MAWBOBLDate(newValue: Date) {
        if (this.EntityPM.MAWBOBLDate != newValue) {
            this.EntityPM.MAWBOBLDate = newValue;
        }
    }

    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(newValue: string) {
        if (this.EntityPM.AccountNumber != newValue) {
            this.EntityPM.AccountNumber = newValue;
        }
    }

    get MainCarriageVesselId() { return this.EntityPM.MainCarriageVesselId; }
    set MainCarriageVesselId(newValue: string) {
        if (this.EntityPM.MainCarriageVesselId != newValue) {
            this.EntityPM.MainCarriageVesselId = newValue;
        }
    }

    // General
    get IncotermId() { return this.EntityPM.IncotermId; }
    set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;

            if (newValue != null) {
                this.myIncotermListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myList: IncotermList = myResponse.Result;
                        if (myList) {
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
        }
    }

    get SalesmanUserId() { return this.EntityPM.SalesmanUserId; }
    set SalesmanUserId(newValue: string) {
        if (this.EntityPM.SalesmanUserId != newValue) {
            this.EntityPM.SalesmanUserId = newValue;
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

    get House() { return this.EntityPM.House; }
    set House(newValue: string) {
        if (this.EntityPM.House != newValue) {
            this.EntityPM.House = newValue;
        }
    }

    // Expected Order Details
    FillDimensionsClicked() {
        var logeWindow = new LogitudeWindow();
        logeWindow.Width = 850;
        logeWindow.Title = "Fill Dimensions";
        logeWindow.WindowArgs = this.EntityPM;
        logeWindow.Show("./Shipment/Components/NewEntity/WizardDimensionsComponent");
        logeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.SetUIProperties_OrderDetails();
            }
        });
    }

    get OrderGrossWeight() { return this.EntityPM.OrderGrossWeight; }
    set OrderGrossWeight(newValue: number) {
        if (this.EntityPM.OrderGrossWeight != newValue) {
            this.EntityPM.OrderGrossWeight = AppTool.Round(newValue, 3);
            this.ComputeChargeableWeight();
        }
    }

    get BookingVolume() { return this.EntityPM.BookingVolume; }
    set BookingVolume(newValue: number) {
        if (this.EntityPM.BookingVolume != newValue) {
            this.EntityPM.BookingVolume = AppTool.Round(newValue, 3);
            this.ComputeOrderVolumetricWeight();
        }
    }

    get OrderVolumetricWeight() { return this.EntityPM.OrderVolumetricWeight; }
    set OrderVolumetricWeight(newValue: number) {
        if (this.EntityPM.OrderVolumetricWeight != newValue) {
            this.EntityPM.OrderVolumetricWeight = AppTool.Round(newValue, 3);
            this.ComputeChargeableWeight();
        }
    }

    get OrderChargeableWeight() { return this.EntityPM.OrderChargeableWeight; }
    set OrderChargeableWeight(newValue: number) {
        if (this.EntityPM.OrderChargeableWeight != newValue) {
            var myResult: number = AppTool.Round(newValue, 3);
            this.EntityPM.OrderChargeableWeight = myResult;

            if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                this.EntityPM.OrderVolumetricWeight = myResult;
                this.EntityPM.OrderGrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, myResult);
                this.EntityPM.BookingVolume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, myResult, this.EntityPM.Ratio);
            }
        }
    }

    get BookingNumberOfPackages() { return this.EntityPM.BookingNumberOfPackages; }
    set BookingNumberOfPackages(newValue: number) {
        if (this.EntityPM.BookingNumberOfPackages != newValue) {
            this.EntityPM.BookingNumberOfPackages = newValue;
        }
    }   

    get OrderIsDangerouseGoods() { return this.EntityPM.OrderIsDangerouseGoods; }
    set OrderIsDangerouseGoods(newValue: boolean) {
        if (this.EntityPM.OrderIsDangerouseGoods != newValue) {
            this.EntityPM.OrderIsDangerouseGoods = newValue;
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
        }
    }

    private ComputeOrderVolumetricWeight() {
        var myResult: number = null;

        if (this.BookingVolume != null) {
            myResult = AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.BookingVolume, this.EntityPM.Ratio);
        }

        else if (this.OrderGrossWeight != null) {
            myResult = AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.OrderGrossWeight);
        }

        this.OrderVolumetricWeight = myResult;
    }
    private ComputeChargeableWeight() {
        this.OrderChargeableWeight = AppTool.CalculateChargeableWeight(this.OrderGrossWeight, this.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }

    get Quantity1() { return this.EntityPM.Quantity1; }
    set Quantity1(newValue: number) {
        if (this.EntityPM.Quantity1 != newValue) {
            this.EntityPM.Quantity1 = newValue;
        }
    }  

    get Quantity2() { return this.EntityPM.Quantity2; }
    set Quantity2(newValue: number) {
        if (this.EntityPM.Quantity2 != newValue) {
            this.EntityPM.Quantity2 = newValue;
        }
    }  

    get Quantity3() { return this.EntityPM.Quantity3; }
    set Quantity3(newValue: number) {
        if (this.EntityPM.Quantity3 != newValue) {
            this.EntityPM.Quantity3 = newValue;
        }
    } 

    get Quantity4() { return this.EntityPM.Quantity4; }
    set Quantity4(newValue: number) {
        if (this.EntityPM.Quantity4 != newValue) {
            this.EntityPM.Quantity4 = newValue;
        }
    } 

    get Quantity5() { return this.EntityPM.Quantity5; }
    set Quantity5(newValue: number) {
        if (this.EntityPM.Quantity5 != newValue) {
            this.EntityPM.Quantity5 = newValue;
        }
    } 

    public PackageTypeList1: PackageTypeList = null;
    get PackageTypeId1() { return this.EntityPM.PackageTypeId1; }
    set PackageTypeId1(newValue: string) {
        if (this.EntityPM.PackageTypeId1 != newValue) {
            this.EntityPM.PackageTypeId1 = newValue;
        }
    } 

    public PackageTypeList2: PackageTypeList = null;
    get PackageTypeId2() { return this.EntityPM.PackageTypeId2; }
    set PackageTypeId2(newValue: string) {
        if (this.EntityPM.PackageTypeId2 != newValue) {
            this.EntityPM.PackageTypeId2 = newValue;
        }
    } 

    public PackageTypeList3: PackageTypeList = null;
    get PackageTypeId3() { return this.EntityPM.PackageTypeId3; }
    set PackageTypeId3(newValue: string) {
        if (this.EntityPM.PackageTypeId3 != newValue) {
            this.EntityPM.PackageTypeId3 = newValue;
        }
    } 

    public PackageTypeList4: PackageTypeList = null;
    get PackageTypeId4() { return this.EntityPM.PackageTypeId4; }
    set PackageTypeId4(newValue: string) {
        if (this.EntityPM.PackageTypeId4 != newValue) {
            this.EntityPM.PackageTypeId4 = newValue;
        }
    } 

    public PackageTypeList5: PackageTypeList = null;
    get PackageTypeId5() { return this.EntityPM.PackageTypeId5; }
    set PackageTypeId5(newValue: string) {
        if (this.EntityPM.PackageTypeId5 != newValue) {
            this.EntityPM.PackageTypeId5 = newValue;
        }
    } 

    // Additional Fields
    private timerToken: any;
    private Retries: number = 0;
    private GeneratedComponent: any;
    BuildAdditionalFields() {
        this.RunComponent();
    }
    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {

                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = "NewShipment";
                cmpRef.instance.LabelWidth = 110;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
            });
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
    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsScreenEnabled);
        }
    }

    public CopyCheckBoxTop: number = 5;
    public IsCopyOtherPartnersVisible: boolean = false;
    CopyEntityData() {
        if (this.IsBuildFromQuote || this.IsCopyFromShipment) {
            this.EntityPM.IsBuildFromQuote = this.IsBuildFromQuote;
            this.EntityPM.IsCopyFromShipment = this.IsCopyFromShipment;

            this.DirectionId = this.SourceEntityPM.DirectionId;
            this.TransportModeId = this.SourceEntityPM.TransportModeId;
            this.ShipmentTypeId = this.SourceEntityPM.ShipmentTypeId;
            this.ShipmentCustomerTypeCode = this.SourceEntityPM.ShipmentCustomerTypeCode;
            this.CustomerId = this.SourceEntityPM.CustomerId;

            ShipmentTool.CopyShipment(this.EntityPM, this.SourceEntityPM);

            this.OnFiltersChanged();

            this.CopyRoutings();
            this.CopyPartners();

            if (this.IsCopyFromShipment) {
                this.OkButtonLabel = TextCodeTranslator.Translate("Shipment.B.Copy");

                this.EntityPM.OriginShipmentId = this.SourceEntityPM.Id;
                this.EntityPM.IncotermId = this.SourceEntityPM.IncotermId;
                this.EntityPM.FreightPrepaidCollectId = this.SourceEntityPM.FreightPrepaidCollectId;
                this.EntityPM.OtherPrepaidCollectId = this.SourceEntityPM.OtherPrepaidCollectId;
                this.EntityPM.BaseShipmentNumber = this.SourceEntityPM.ShipmentNumber;
                this.IsCopyDescription = AppTool.IsNullOrEmpty(this.SourceEntityPM.DescriptionOfGoods) ? false : true;
                this.IsCopyDescriptionEnabled = AppTool.IsNullOrEmpty(this.SourceEntityPM.DescriptionOfGoods) ? false : true;
                this.IsCopyPackagesEnabled = this.SourceEntityPM.ShipmentPackages.length > 0 ? true : false;

                if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.AgentId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.CustomAgentImportId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.CustomAgentExportId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.Notify1Id)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.Notify2Id)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperNotExporterId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeNotImporterId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.FreightForwarderId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
            }

            else if (this.IsBuildFromQuote) {
                this.EntityPM.QuoteId = this.SourceEntityPM.QuoteId;
                this.EntityPM.QuoteNumber = this.SourceEntityPM.QuoteNumber;
            }
        }
    }
    CopyRoutings() {
        if (this.IsCopyFromShipment) {
            this.EntityPM.AirlinePrefix = this.SourceEntityPM.AirlinePrefix;
            this.EntityPM.MainCarriageCarrierId = this.SourceEntityPM.MainCarriageCarrierId;
            this.EntityPM.MainCarriageCarrierNumber = this.SourceEntityPM.MainCarriageCarrierNumber;
            this.EntityPM.MainCarriageCarrierCode = this.SourceEntityPM.MainCarriageCarrierCode;
            this.EntityPM.MainCarriageCarrierPrefix = this.SourceEntityPM.MainCarriageCarrierPrefix;
            this.EntityPM.FromPortId = this.SourceEntityPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageFromPortId = this.SourceEntityPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageFromPortCode = this.SourceEntityPM.MainCarriageFromPortCode;
            this.EntityPM.MainCarriageFromPortName = this.SourceEntityPM.MainCarriageFromPortName;
            this.EntityPM.MainCarriageFromPortCountryCode = this.SourceEntityPM.MainCarriageFromPortCountryCode;
            this.EntityPM.MainCarriageFromPortCountryName = this.SourceEntityPM.MainCarriageFromPortCountryName;
            this.EntityPM.ToPortId = this.SourceEntityPM.MainCarriageToPortId;
            this.EntityPM.MainCarriageToPortId = this.SourceEntityPM.MainCarriageToPortId;
            this.EntityPM.MainCarriageToPortCode = this.SourceEntityPM.MainCarriageToPortCode;
            this.EntityPM.MainCarriageToPortName = this.SourceEntityPM.MainCarriageToPortName;
            this.EntityPM.MainCarriageToPortCountryCode = this.SourceEntityPM.MainCarriageToPortCountryCode;
            this.EntityPM.MainCarriageToPortCountryName = this.SourceEntityPM.MainCarriageToPortCountryName;
            this.EntityPM.FinalDistenationPortId = this.SourceEntityPM.FinalDistenationPortId;
            this.EntityPM.MainCarriageFinalDestinationPortId = this.SourceEntityPM.MainCarriageFinalDestinationPortId;  
        }

        if (this.IsBuildFromQuote) {
            this.EntityPM.MainCarriageFromPartnerId = this.SourceEntityPM.MainCarriageFromPartnerId;
            this.EntityPM.MainCarriageFromAddressId = this.SourceEntityPM.MainCarriageFromAddressId;
            this.EntityPM.MainCarriageToPartnerId = this.SourceEntityPM.MainCarriageToPartnerId;
            this.EntityPM.MainCarriageToAddressId = this.SourceEntityPM.MainCarriageToAddressId;
            this.EntityPM.MainCarriageFromPortId = this.SourceEntityPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageToPortId = this.SourceEntityPM.MainCarriageToPortId;
            this.EntityPM.FromPortId = this.SourceEntityPM.FromPortId;
            this.EntityPM.ToPortId = this.SourceEntityPM.ToPortId;
            this.EntityPM.MainCarriageCarrierId = this.SourceEntityPM.MainCarriageCarrierId;
            this.EntityPM.MainCarriageFinalDestinationPortId = this.SourceEntityPM.MainCarriageFinalDestinationPortId;
        }
    }
    CopyPartners() {
        if (this.IsCopyFromShipment) {

            if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperId)) {
                this.IsCopyShipper = true;
                this.IsCopyShipperEnabled = true;

                //if (this.SourceEntityPM.ShipperId == this.SourceEntityPM.CustomerId) {
                //    this.IsShipperMyCustomer = true;
                //    this.ShipmentCustomerTypeCode = "SHI";
                //}
            }

            if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeId)) {
                this.IsCopyConsignee = true;
                this.IsCopyConsigneeEnabled = true;

                //if (this.SourceEntityPM.ConsigneeId == this.SourceEntityPM.CustomerId) {
                //    this.IsConsigneeMyCustomer = true;
                //    this.ShipmentCustomerTypeCode = "CON";
                //}
            }
        }

        else if (this.IsBuildFromQuote) {
            this.IsCopyShipper = true;
            this.IsCopyConsignee = true;
            this.IsCopyAgent = true;
            this.IsCopyCustomAgentExport = true;
            this.IsCopyCustomAgentImport = true;
            this.IsCopyNotify1 = true;
            this.IsCopyNotify2 = true;
            this.IsCopyShipperNotExporter = true;
            this.IsCopyConsigneeNotImporter = true;
            this.IsCopyFreightForwarder = true;
            this.IsCopyConsolidator = true;

            //if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperId)) {
            //    if (this.SourceEntityPM.ShipperId == this.SourceEntityPM.CustomerId) {
            //        this.ShipmentCustomerTypeCode = "SHI";
            //        this.IsShipperMyCustomer = true;                    
            //    }
            //}

            //if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeId)) {
            //    if (this.SourceEntityPM.ConsigneeId == this.SourceEntityPM.CustomerId) {
            //        this.ShipmentCustomerTypeCode = "CON";
            //        this.IsConsigneeMyCustomer = true;                    
            //    }
            //}
        }
    }

    public IsCopyShipperEnabled: boolean = false;
    public IsCopyConsigneeEnabled: boolean = false;

    private isCopyShipper: boolean = false;
    get IsCopyShipper() { return this.isCopyShipper; }
    set IsCopyShipper(value) {
        if (this.isCopyShipper != value) {
            this.isCopyShipper = value;

            this.EntityPM.ShipperId = !value ? null : this.SourceEntityPM.ShipperId;
            //this.EntityPM.ShipperReference1 = !value ? null : this.SourceEntityPM.ShipperReference1;
            //this.EntityPM.ShipperReference2 = !value ? null : this.SourceEntityPM.ShipperReference2;
            this.ShipperAddressId = !value ? null : this.SourceEntityPM.ShipperAddressId;
            this.ShipperContactId = !value ? null : this.SourceEntityPM.ShipperContactId;

            this.SetUIProperties_Shipper();

            if (this.EntityPM.ShipperId) {
                this.myCardListService.getSingle(this.EntityPM.ShipperId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.ShipperPartnerTypeId = myCardList.PartnerTypeId;                            
                            this.EntityPM.ShipperName = myCardList.EnglishName;
                            this.EntityPM.ShipperNote = myCardList.Notes;
                            this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                            this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                            this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                            this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;

                            if (AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperContactId)) {
                                this.ShipperContactId = null;
                                this.ShipperContactId = myCardList.PrimaryContactId;
                            }

                            if (AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperAddressId)) {
                                this.ShipperAddressId = null;
                                this.ShipperAddressId = myCardList.MainAddressId;
                            }
                        }
                    }
                });
            }
        }
    }

    private isCopyConsignee: boolean = false;
    get IsCopyConsignee() { return this.isCopyConsignee; }
    set IsCopyConsignee(value) {
        if (this.isCopyConsignee != value) {
            this.isCopyConsignee = value;

            this.EntityPM.ConsigneeId = !value ? null : this.SourceEntityPM.ConsigneeId;
            //this.EntityPM.ConsigneeReference1 = !value ? null : this.SourceEntityPM.ConsigneeReference1;
            //this.EntityPM.ConsigneeReference2 = !value ? null : this.SourceEntityPM.ConsigneeReference2;
            this.ConsigneeAddressId = !value ? null : this.SourceEntityPM.ConsigneeAddressId;
            this.ConsigneeContactId = !value ? null : this.SourceEntityPM.ConsigneeContactId;

            this.SetUIProperties_Consignee();

            if (this.EntityPM.ConsigneeId) {
                this.myCardListService.getSingle(this.EntityPM.ConsigneeId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myCardList: CardList = myResponse.Result;
                        if (myCardList) {
                            this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                            this.EntityPM.ConsigneeName = myCardList.EnglishName;
                            this.EntityPM.ConsigneeNote = myCardList.Notes;
                            this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                            this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;

                            if (AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeContactId)) {
                                this.ConsigneeContactId = null;
                                this.ConsigneeContactId = myCardList.PrimaryContactId;
                            }

                            if (AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeAddressId)) {
                                this.ConsigneeAddressId = null;
                                this.ConsigneeAddressId = myCardList.MainAddressId;
                            }
                        }
                    }
                });
            }
        }
    }

    private isCopyAgent: boolean = false;
    get IsCopyAgent() { return this.isCopyAgent; }
    set IsCopyAgent(value: boolean) {
        if (this.isCopyAgent != value){
            this.isCopyAgent = value;

            this.EntityPM.AgentId = !value ? null : this.SourceEntityPM.AgentId;
            this.EntityPM.AgentName = !value ? null : this.SourceEntityPM.AgentName;
            this.EntityPM.AgentNote = !value ? null : this.SourceEntityPM.AgentNote;
            this.EntityPM.AgentAddressId = !value ? null : this.SourceEntityPM.AgentAddressId;
            this.EntityPM.AgentContactId = !value ? null : this.SourceEntityPM.AgentContactId;
            //this.EntityPM.AgentReference1 = !value ? null : this.SourceEntityPM.AgentReference1;
            //this.EntityPM.AgentReference2 = !value ? null : this.SourceEntityPM.AgentReference2;
        }
    }

    private isCopyCustomAgentImport: boolean = false;
    get IsCopyCustomAgentImport() { return this.isCopyCustomAgentImport; }
    set IsCopyCustomAgentImport(value) {
        if (this.isCopyCustomAgentImport != value) {
            this.isCopyCustomAgentImport = value;

            this.EntityPM.CustomAgentImportId = !value ? null : this.SourceEntityPM.CustomAgentImportId;
            this.EntityPM.CustomAgentImportName = !value ? null : this.SourceEntityPM.CustomAgentImportName;
            this.EntityPM.CustomAgentImportNote = !value ? null : this.SourceEntityPM.CustomAgentImportNote;
            this.EntityPM.CustomAgentImportAddressId = !value ? null : this.SourceEntityPM.CustomAgentImportAddressId;
            this.EntityPM.CustomAgentImportContactId = !value ? null : this.SourceEntityPM.CustomAgentImportContactId;
            //this.EntityPM.CustomAgentImportReference = !value ? null : this.SourceEntityPM.CustomAgentImportReference;
        }
    }

    private isCopyCustomAgentExport: boolean = false;
    get IsCopyCustomAgentExport() { return this.isCopyCustomAgentExport; }
    set IsCopyCustomAgentExport(value) {
        if (this.isCopyCustomAgentExport != value) {
            this.isCopyCustomAgentExport = value;

            this.EntityPM.CustomAgentExportId = !value ? null : this.SourceEntityPM.CustomAgentExportId;
            this.EntityPM.CustomAgentExportName = !value ? null : this.SourceEntityPM.CustomAgentExportName;
            this.EntityPM.CustomAgentExportNote = !value ? null : this.SourceEntityPM.CustomAgentExportNote;
            this.EntityPM.CustomAgentExportAddressId = !value ? null : this.SourceEntityPM.CustomAgentExportAddressId;
            this.EntityPM.CustomAgentExportContactId = !value ? null : this.SourceEntityPM.CustomAgentExportContactId;
            //this.EntityPM.CustomAgentExportReference = !value ? null : this.SourceEntityPM.CustomAgentExportReference;
        }
    }

    private isCopyNotify1: boolean = false;
    get IsCopyNotify1() { return this.isCopyNotify1; }
    set IsCopyNotify1(value) {
        if (this.isCopyNotify1 != value) {
            this.isCopyNotify1 = value;

            this.EntityPM.Notify1Id = !value ? null : this.SourceEntityPM.Notify1Id;
            this.EntityPM.Notify1Name = !value ? null : this.SourceEntityPM.Notify1Name;
            this.EntityPM.Notify1Note = !value ? null : this.SourceEntityPM.Notify1Note;
            this.EntityPM.Notify1AddressId = !value ? null : this.SourceEntityPM.Notify1AddressId;
            this.EntityPM.Notify1ContactId = !value ? null : this.SourceEntityPM.Notify1ContactId;
        }
    }

    private isCopyNotify2: boolean = false;
    get IsCopyNotify2() { return this.isCopyNotify2; }
    set IsCopyNotify2(value) {
        if (this.isCopyNotify2 != value) {
            this.isCopyNotify2 = value;

            this.EntityPM.Notify2Id = !value ? null : this.SourceEntityPM.Notify2Id;
            this.EntityPM.Notify2Name = !value ? null : this.SourceEntityPM.Notify2Name;
            this.EntityPM.Notify2Note = !value ? null : this.SourceEntityPM.Notify2Note;
            this.EntityPM.Notify2AddressId = !value ? null : this.SourceEntityPM.Notify2AddressId;
            this.EntityPM.Notify2ContactId = !value ? null : this.SourceEntityPM.Notify2ContactId;
        }
    }

    private isCopyShipperNotExporter: boolean = false;
    get IsCopyShipperNotExporter() { return this.isCopyShipperNotExporter; }
    set IsCopyShipperNotExporter(value) {
        if (this.isCopyShipperNotExporter != value) {
            this.isCopyShipperNotExporter = value;

            this.EntityPM.ShipperNotExporterId = !value ? null : this.SourceEntityPM.ShipperNotExporterId;
            this.EntityPM.ShipperNotExporterName = !value ? null : this.SourceEntityPM.ShipperNotExporterName;
            this.EntityPM.ShipperNotExporterNote = !value ? null : this.SourceEntityPM.ShipperNotExporterNote;
            this.EntityPM.ShipperNotExporterAddressId = !value ? null : this.SourceEntityPM.ShipperNotExporterAddressId;
            this.EntityPM.ShipperNotExporterContactId = !value ? null : this.SourceEntityPM.ShipperNotExporterContactId;
        }
    }

    private isCopyConsigneeNotImporter: boolean = false;
    get IsCopyConsigneeNotImporter() { return this.isCopyConsigneeNotImporter; }
    set IsCopyConsigneeNotImporter(value) {
        if (this.isCopyConsigneeNotImporter != value) {
            this.isCopyConsigneeNotImporter = value;

            this.EntityPM.ConsigneeNotImporterId = !value ? null : this.SourceEntityPM.ConsigneeNotImporterId;
            this.EntityPM.ConsigneeNotImporterName = !value ? null : this.SourceEntityPM.ConsigneeNotImporterName;
            this.EntityPM.ConsigneeNotImporterNote = !value ? null : this.SourceEntityPM.ConsigneeNotImporterNote;
            this.EntityPM.ConsigneeNotImporterAddressId = !value ? null : this.SourceEntityPM.ConsigneeNotImporterAddressId;
            this.EntityPM.ConsigneeNotImporterContactId = !value ? null : this.SourceEntityPM.ConsigneeNotImporterContactId;
        }
    }

    private isCopyFreightForwarder: boolean = false;
    get IsCopyFreightForwarder() { return this.isCopyFreightForwarder; }
    set IsCopyFreightForwarder(value) {
        if (this.isCopyFreightForwarder != value) {
            this.isCopyFreightForwarder = value;

            this.EntityPM.FreightForwarderId = !value ? null : this.SourceEntityPM.FreightForwarderId;
            this.EntityPM.FreightForwarderName = !value ? null : this.SourceEntityPM.FreightForwarderName;
            this.EntityPM.FreightForwarderNote = !value ? null : this.SourceEntityPM.FreightForwarderNote;
            this.EntityPM.FreightForwarderAddressId = !value ? null : this.SourceEntityPM.FreightForwarderAddressId;
            this.EntityPM.FreightForwarderContactId = !value ? null : this.SourceEntityPM.FreightForwarderContactId;
            //this.EntityPM.FreightForwarderReference = !value ? null : this.SourceEntityPM.FreightForwarderReference;
        }
    }

    private isCopyConsolidator: boolean = false;
    get IsCopyConsolidator() { return this.isCopyConsolidator; }
    set IsCopyConsolidator(value) {
        if (this.isCopyConsolidator != value) {
            this.isCopyConsolidator = value;

            this.EntityPM.ConsolidatorId = !value ? null :this.SourceEntityPM.ConsolidatorId;
            this.EntityPM.ConsolidatorName = !value ? null : this.SourceEntityPM.ConsolidatorName;
            this.EntityPM.ConsolidatorNote = !value ? null : this.SourceEntityPM.ConsolidatorNote;
            this.EntityPM.ConsolidatorAddressId = !value ? null : this.SourceEntityPM.ConsolidatorAddressId;
            this.EntityPM.ConsolidatorContactId = !value ? null : this.SourceEntityPM.ConsolidatorContactId;
            //this.EntityPM.ConsolidatorReference = !value ? null : this.SourceEntityPM.ConsolidatorReference;
        }
    }

    private isCopyPreCarriage: boolean = false;
    get IsCopyPreCarriage() { return this.isCopyPreCarriage; }
    set IsCopyPreCarriage(value) {
        if (this.isCopyPreCarriage != value) {
            this.isCopyPreCarriage = value;

            if (value == true) {
                this.EntityPM.PreCarriageFromPortId = this.SourceEntityPM.PreCarriageFromPortId;
                this.EntityPM.PreCarriageFromPortCode = this.SourceEntityPM.PreCarriageFromPortCode;
                this.EntityPM.PreCarriageFromPortName = this.SourceEntityPM.PreCarriageFromPortName;
                this.EntityPM.PreCarriageFromPortCountryCode = this.SourceEntityPM.PreCarriageFromPortCountryCode;
                this.EntityPM.PreCarriageFromPortCountryName = this.SourceEntityPM.PreCarriageFromPortCountryName;
                this.EntityPM.PreCarriageToPortId = this.SourceEntityPM.PreCarriageToPortId;
                this.EntityPM.PreCarriageToPortCode = this.SourceEntityPM.PreCarriageToPortCode;
                this.EntityPM.PreCarriageToPortName = this.SourceEntityPM.PreCarriageToPortName;
                this.EntityPM.PreCarriageToPortCountryCode = this.SourceEntityPM.PreCarriageToPortCountryCode;
                this.EntityPM.PreCarriageToPortCountryName = this.SourceEntityPM.PreCarriageToPortCountryName;
                this.EntityPM.PreCarriageCarrierId = this.SourceEntityPM.PreCarriageCarrierId;
                this.EntityPM.PreCarriageCarrierCode = this.SourceEntityPM.PreCarriageCarrierCode;
                this.EntityPM.PreCarriageCarrierName = this.SourceEntityPM.PreCarriageCarrierName;
                this.EntityPM.PreCarriageCarrierNumber = this.SourceEntityPM.PreCarriageCarrierNumber;
                this.EntityPM.PreCarriageCarrierWebSite = this.SourceEntityPM.PreCarriageCarrierWebSite;
                this.EntityPM.PreCarriageTransportModeId = this.SourceEntityPM.PreCarriageTransportModeId;
                this.EntityPM.PreCarriageVesselId = this.SourceEntityPM.PreCarriageVesselId;
                this.EntityPM.PreCarriageVesselName = this.SourceEntityPM.PreCarriageVesselName;
                this.EntityPM.PreCarriageETD = this.SourceEntityPM.PreCarriageETD;
                this.EntityPM.PreCarriageATD = this.SourceEntityPM.PreCarriageATD;
                this.EntityPM.PreCarriageETA = this.SourceEntityPM.PreCarriageETA;
                this.EntityPM.PreCarriageATA = this.SourceEntityPM.PreCarriageATA;
            }

            else {
                this.EntityPM.PreCarriageFromPortId = null;
                this.EntityPM.PreCarriageFromPortCode = null;
                this.EntityPM.PreCarriageFromPortName = null;
                this.EntityPM.PreCarriageFromPortCountryCode = null;
                this.EntityPM.PreCarriageFromPortCountryName = null;
                this.EntityPM.PreCarriageToPortId = null;
                this.EntityPM.PreCarriageToPortCode = null;
                this.EntityPM.PreCarriageToPortName = null;
                this.EntityPM.PreCarriageToPortCountryCode = null;
                this.EntityPM.PreCarriageToPortCountryName = null;
                this.EntityPM.PreCarriageCarrierId = null;
                this.EntityPM.PreCarriageCarrierCode = null;
                this.EntityPM.PreCarriageCarrierName = null;
                this.EntityPM.PreCarriageCarrierNumber = null;
                this.EntityPM.PreCarriageCarrierWebSite = null;
                this.EntityPM.PreCarriageTransportModeId = null;
                this.EntityPM.PreCarriageVesselId = null;
                this.EntityPM.PreCarriageVesselName = null;
                this.EntityPM.PreCarriageETD = null;
                this.EntityPM.PreCarriageATD = null;
                this.EntityPM.PreCarriageETA = null;
                this.EntityPM.PreCarriageATA = null;
            }
        }
    }

    private isCopyOnCarriage: boolean = false;
    get IsCopyOnCarriage() { return this.isCopyOnCarriage; }
    set IsCopyOnCarriage(value) {
        if (this.isCopyOnCarriage != value) {
            this.isCopyOnCarriage = value;

            if (value == true) {
                this.EntityPM.OnCarriageFromPortId = this.SourceEntityPM.OnCarriageFromPortId;
                this.EntityPM.OnCarriageFromPortCode = this.SourceEntityPM.OnCarriageFromPortCode;
                this.EntityPM.OnCarriageFromPortName = this.SourceEntityPM.OnCarriageFromPortName;
                this.EntityPM.OnCarriageFromPortCountryCode = this.SourceEntityPM.OnCarriageFromPortCountryCode;
                this.EntityPM.OnCarriageFromPortCountryName = this.SourceEntityPM.OnCarriageFromPortCountryName;
                this.EntityPM.OnCarriageToPortId = this.SourceEntityPM.OnCarriageToPortId;
                this.EntityPM.OnCarriageToPortCode = this.SourceEntityPM.OnCarriageToPortCode;
                this.EntityPM.OnCarriageToPortName = this.SourceEntityPM.OnCarriageToPortName;
                this.EntityPM.OnCarriageToPortCountryCode = this.SourceEntityPM.OnCarriageToPortCountryCode;
                this.EntityPM.OnCarriageToPortCountryName = this.SourceEntityPM.OnCarriageToPortCountryName;
                this.EntityPM.OnCarriageCarrierId = this.SourceEntityPM.OnCarriageCarrierId;
                this.EntityPM.OnCarriageCarrierCode = this.SourceEntityPM.OnCarriageCarrierCode;
                this.EntityPM.OnCarriageCarrierName = this.SourceEntityPM.OnCarriageCarrierName;
                this.EntityPM.OnCarriageCarrierNumber = this.SourceEntityPM.OnCarriageCarrierNumber;
                this.EntityPM.OnCarriageCarrierWebSite = this.SourceEntityPM.OnCarriageCarrierWebSite;
                this.EntityPM.OnCarriageTransportModeId = this.SourceEntityPM.OnCarriageTransportModeId;
                this.EntityPM.OnCarriageVesselId = this.SourceEntityPM.OnCarriageVesselId;
                this.EntityPM.OnCarriageVesselName = this.SourceEntityPM.OnCarriageVesselName;
                this.EntityPM.OnCarriageETD = this.SourceEntityPM.OnCarriageETD;
                this.EntityPM.OnCarriageATD = this.SourceEntityPM.OnCarriageATD;
                this.EntityPM.OnCarriageETA = this.SourceEntityPM.OnCarriageETA;
                this.EntityPM.OnCarriageATA = this.SourceEntityPM.OnCarriageATA;
            }

            else {
                this.EntityPM.OnCarriageFromPortId = null;
                this.EntityPM.OnCarriageFromPortCode = null;
                this.EntityPM.OnCarriageFromPortName = null;
                this.EntityPM.OnCarriageFromPortCountryCode = null;
                this.EntityPM.OnCarriageFromPortCountryName = null;
                this.EntityPM.OnCarriageToPortId = null;
                this.EntityPM.OnCarriageToPortCode = null;
                this.EntityPM.OnCarriageToPortName = null;
                this.EntityPM.OnCarriageToPortCountryCode = null;
                this.EntityPM.OnCarriageToPortCountryName = null;
                this.EntityPM.OnCarriageCarrierId = null;
                this.EntityPM.OnCarriageCarrierCode = null;
                this.EntityPM.OnCarriageCarrierName = null;
                this.EntityPM.OnCarriageCarrierNumber = null;
                this.EntityPM.OnCarriageCarrierWebSite = null;
                this.EntityPM.OnCarriageTransportModeId = null;
                this.EntityPM.OnCarriageVesselId = null;
                this.EntityPM.OnCarriageVesselName = null;
                this.EntityPM.OnCarriageETD = null;
                this.EntityPM.OnCarriageATD = null;
                this.EntityPM.OnCarriageETA = null;
                this.EntityPM.OnCarriageATA = null;
            }
        }
    }

    private isCopyFlights: boolean = false;
    get IsCopyFlights() { return this.isCopyFlights; }
    set IsCopyFlights(value) {
        if (this.isCopyFlights != value) {
            this.isCopyFlights = value;
        }
    }

    public IsCopyDescriptionEnabled: boolean = false;
    private isCopyDescription: boolean = false;
    get IsCopyDescription() { return this.isCopyDescription; }
    set IsCopyDescription(value) {
        if (this.isCopyDescription != value) {
            this.isCopyDescription = value;
            this.DescriptionOfGoods = !value ? null : this.SourceEntityPM.DescriptionOfGoods;
        }
    }

    public IsCopyPackagesEnabled: boolean = false;
    private isCopyPackages: boolean = false;
    get IsCopyPackages() { return this.isCopyPackages; }
    set IsCopyPackages(value) {
        if (this.isCopyPackages != value) {
            this.isCopyPackages = value;

            if (value) {
                ShipmentTool.CopyShipmentPackages(this.EntityPM, this.SourceEntityPM, false);

                this.EntityPM.BookingVolume = this.SourceEntityPM.BookingVolume;
                this.EntityPM.OrderVolumetricWeight = this.SourceEntityPM.OrderVolumetricWeight;
                this.EntityPM.OrderGrossWeight = this.SourceEntityPM.OrderGrossWeight;
                this.EntityPM.OrderChargeableWeight = this.SourceEntityPM.OrderChargeableWeight;
                this.EntityPM.BookingNumberOfPackages = this.SourceEntityPM.BookingNumberOfPackages;

                this.EntityPM.GrossWeight = this.SourceEntityPM.GrossWeight;
                this.EntityPM.GrossWeightInKG = this.SourceEntityPM.GrossWeightInKG;
                this.EntityPM.Volume = this.SourceEntityPM.Volume;
                this.EntityPM.VolumeInCBM = this.SourceEntityPM.VolumeInCBM;
                this.EntityPM.ChargeableWeight = this.SourceEntityPM.ChargeableWeight;
                this.EntityPM.ChargeableWeightInKG = this.SourceEntityPM.ChargeableWeightInKG;
                this.EntityPM.VolumetricWeight = this.SourceEntityPM.VolumetricWeight;
                this.EntityPM.NumberOfContainers = this.SourceEntityPM.NumberOfContainers;
                this.EntityPM.NumberOfPackages = this.SourceEntityPM.NumberOfPackages;
                this.EntityPM.TEU = this.SourceEntityPM.TEU;
                this.EntityPM.GrossWeightPerTon = this.SourceEntityPM.GrossWeightPerTon;

                this.EntityPM.GrossWeightEdited = this.SourceEntityPM.GrossWeightEdited;
                this.EntityPM.ChargeableWeightEdited = this.SourceEntityPM.ChargeableWeightEdited;
                this.EntityPM.OrderGrossWeightEdited = this.SourceEntityPM.OrderGrossWeightEdited;
                this.EntityPM.OrderChargeableWeightEdited = this.SourceEntityPM.OrderChargeableWeightEdited;
            }

            else {
                this.EntityPM.ShipmentPackages = [];
                this.EntityPM.ShipmentOrderPackages = [];

                this.EntityPM.BookingVolume = null;
                this.EntityPM.OrderVolumetricWeight = null;
                this.EntityPM.OrderGrossWeight = null;
                this.EntityPM.OrderChargeableWeight = null;
                this.EntityPM.BookingNumberOfPackages = null;

                this.EntityPM.GrossWeight = null;
                this.EntityPM.GrossWeightInKG = null;
                this.EntityPM.Volume = null;
                this.EntityPM.VolumeInCBM = null;
                this.EntityPM.ChargeableWeight = null;
                this.EntityPM.ChargeableWeightInKG = null;
                this.EntityPM.VolumetricWeight = null;
                this.EntityPM.NumberOfContainers = null;
                this.EntityPM.NumberOfPackages = null;
                this.EntityPM.TEU = null;
                this.EntityPM.GrossWeightPerTon = null;

                this.EntityPM.GrossWeightEdited = false;
                this.EntityPM.ChargeableWeightEdited = false;
                this.EntityPM.OrderGrossWeightEdited = false;
                this.EntityPM.OrderChargeableWeightEdited = false;
            }

            this.SetUIProperties_OrderDetails();
        }
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        this.CurrentSession.StartBusyIndicator("Creating...");

        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;
        this.SetPartnersOnFinish();
        this.SetCountryECOnFinish();
        this.SetInlandDomesticOnFinish();

        var validator = new ShipmentValidator();
        this.ValidationErrorsList = validator.Validate(this.EntityPM);

        if (this.ValidationErrorsList.length == 0) {

            if (this.IsValidatingMasterField) {

                if (!this.IsMasterFieldValid) {
                    this.ValidationErrorsList.push(this.MasterFieldValidityMessage);
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.ValidateMasterStack();
                }
            }

            else {
                this.SubmitCreatingShipment();
            }
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }

    SetDataOnFinish() {
        this.SetOrderPackagesOnFinish();        
        this.SetPickupDeliveryOnFinish();

        if (this.IsCopyFlights) {
            ShipmentTool.CopyFlights(this.EntityPM, this.SourceEntityPM);
        }
    }
    SetPartnersOnFinish() {
        this.SetCustomerPartner();

        if (AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
            this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.myCustomerSalesmanId) ? this.EntityPM.CreatedByUserId : this.myCustomerSalesmanId;
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.AccountManagerUserId)) {
            this.EntityPM.AccountManagerUserId = AppTool.IsNullOrEmpty(this.myCustomerAccountManagerUserId) ? this.EntityPM.CreatedByUserId : this.myCustomerAccountManagerUserId;
        }

        //this.EntityPM.CustomerId = null;
        //this.EntityPM.CustomerName = null;
        //this.EntityPM.CustomerNote = null;
        //this.EntityPM.CustomerAddressId = null;
        //this.EntityPM.CustomerContactId = null;
        //this.EntityPM.CustomerReference1 = null;
        //this.EntityPM.CustomerReference2 = null;
        //this.EntityPM.ShipmentCustomerTypeCode = null;

        // if (this.IsShipperMyCustomer) {
        //    this.EntityPM.ShipmentCustomerTypeCode = "SHI";
        //    this.EntityPM.CustomerId = this.EntityPM.ShipperId;
        //    this.EntityPM.CustomerName = this.EntityPM.ShipperName;
        //    this.EntityPM.CustomerNote = this.EntityPM.ShipperNote;
        //    this.EntityPM.CustomerAddressId = this.EntityPM.ShipperAddressId;
        //    this.EntityPM.CustomerContactId = this.EntityPM.ShipperContactId;
        //    this.EntityPM.CustomerReference1 = this.EntityPM.ShipperReference1;
        //    this.EntityPM.CustomerReference2 = this.EntityPM.ShipperReference2;

        //    if (AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
        //        this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.myShipperSalesmanId) ? this.EntityPM.CreatedByUserId : this.myShipperSalesmanId;
        //    }
        //}

        //else if (this.IsConsigneeMyCustomer) {
        //    this.EntityPM.ShipmentCustomerTypeCode = "CON";
        //    this.EntityPM.CustomerId = this.EntityPM.ConsigneeId;
        //    this.EntityPM.CustomerName = this.EntityPM.ConsigneeName;
        //    this.EntityPM.CustomerNote = this.EntityPM.ConsigneeNote;
        //    this.EntityPM.CustomerAddressId = this.EntityPM.ConsigneeAddressId;
        //    this.EntityPM.CustomerContactId = this.EntityPM.ConsigneeContactId;
        //   this.EntityPM.CustomerReference1 = this.EntityPM.ConsigneeReference1;
        //    this.EntityPM.CustomerReference2 = this.EntityPM.ConsigneeReference2;

        //    if (AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
        //        this.EntityPM.SalesmanUserId = AppTool.IsNullOrEmpty(this.myConsigneeSalesmanId) ? this.EntityPM.CreatedByUserId : this.myConsigneeSalesmanId;
        //    }
        //}
    }
    SetCountryECOnFinish() {
        if (this.IsInlandDomestic) {
            if (this.ShipperAddressList != null) {
                this.EntityPM.FromCountryId = this.ShipperAddressList.CountryId;
                this.EntityPM.FromCountryIsEC = this.ShipperAddressList.CountryEC;
            }

            if (this.ConsigneeAddressList != null) {
                this.EntityPM.ToCountryId = this.ConsigneeAddressList.CountryId;
                this.EntityPM.ToCountryIsEC = this.ConsigneeAddressList.CountryEC;
            }
        }

        else {
            if (this.FromPortList != null) {
                this.EntityPM.FromCountryId = this.FromPortList.CountryId;
                this.EntityPM.FromCountryIsEC = this.FromPortList.CountryEC;
            }

            if (this.ToPortList != null) {
                this.EntityPM.ToCountryId = this.ToPortList.CountryId;
                this.EntityPM.ToCountryIsEC = this.ToPortList.CountryEC;
            }
        }
    }
    SetInlandDomesticOnFinish() {
        if (this.IsInlandDomestic) {
            this.IncludePickUp = false;
            this.IncludeDelivery = false;
            this.MainCarriageFromPortId = null;
            this.MainCarriageToPortId = null;
            this.EntityPM.MainCarriageFinalDestinationPortId = null;
            this.EntityPM.MainCarriageFromPartnerId = this.ShipperId;
            this.EntityPM.MainCarriageFromAddressId = this.ShipperAddressId;
            this.EntityPM.MainCarriageToPartnerId = this.ConsigneeId;
            this.EntityPM.MainCarriageToAddressId = this.ConsigneeAddressId;
        }
    }
    SetOrderPackagesOnFinish() {
        if (this.IsFCLEntity) {
            var numberOfPackages = 0;
            this.EntityPM.ShipmentOrderPackages = [];

            if (this.PackageTypeList1 != null && this.Quantity1 > 0) {
                var item = new ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList1.IsContainer;
                item.Quantity = this.Quantity1;
                item.PackageTypeId = this.PackageTypeId1;
                item.Tenant = SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity1;
            }

            if (this.PackageTypeList2 != null && this.Quantity2 > 0) {
                var item = new ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList2.IsContainer;
                item.Quantity = this.Quantity2;
                item.PackageTypeId = this.PackageTypeId2;
                item.Tenant = SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity2;
            }

            if (this.PackageTypeList3 != null && this.Quantity3 > 0) {
                var item = new ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList3.IsContainer;
                item.Quantity = this.Quantity3;
                item.PackageTypeId = this.PackageTypeId3;
                item.Tenant = SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity3;
            }

            if (this.PackageTypeList4 != null && this.Quantity4 > 0) {
                var item = new ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList4.IsContainer;
                item.Quantity = this.Quantity4;
                item.PackageTypeId = this.PackageTypeId4;
                item.Tenant = SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity4;
            }

            if (this.PackageTypeList5 != null && this.Quantity5 > 0) {
                var item = new ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList5.IsContainer;
                item.Quantity = this.Quantity5;
                item.PackageTypeId = this.PackageTypeId5;
                item.Tenant = SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity5;
            }

            if (numberOfPackages > 0) {
                this.EntityPM.BookingNumberOfPackages = numberOfPackages;
            }
        }
    }
    SetPickupDeliveryOnFinish() {
        this.EntityPM.ShipmentPickUps = [];
        this.EntityPM.ShipmentDeliveries = [];

        if (this.IncludePickUp) {
            var typeCode = "PART";
            if (AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                typeCode = "CASL";
            }

            var newPickUp = new ShipmentPickUpPM(this.EntityPM);
            newPickUp.Tenant = SessionLocator.Tenant;
            newPickUp.PickUpDeliveryTypeCode = "PICK";
            newPickUp.PickUpDeliveryFromTypeCode = typeCode;
            newPickUp.FromAddressCity = this.FromAddressCity;
            newPickUp.FromAddressCountryId = this.FromAddressCountryId;
            newPickUp.FromAddressZipCode = this.FromAddressZipCode;
            newPickUp.FromPartnerCardId = this.ShipperId;
            newPickUp.FromAddressId = this.PickUpAddressId;
            newPickUp.PickUpDeliveryToTypeCode = "PORT";
            newPickUp.ToPortId = this.MainCarriageFromPortId;
            this.EntityPM.AddPickUp(newPickUp);
        }

        if (this.IncludeDelivery) {
            var typeCode = "PART";
            if (AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                typeCode = "CASL";
            }

            var newDelivery = new ShipmentDeliveryPM(this.EntityPM);
            newDelivery.Tenant = SessionLocator.Tenant;
            newDelivery.PickUpDeliveryTypeCode = "DELV";
            newDelivery.PickUpDeliveryFromTypeCode = "PORT";
            newDelivery.FromPortId = this.MainCarriageToPortId;
            newDelivery.PickUpDeliveryToTypeCode = typeCode;
            newDelivery.ToAddressCity = this.ToAddressCity;
            newDelivery.ToAddressCountryId = this.ToAddressCountryId;
            newDelivery.ToAddressZipCode = this.ToAddressZipCode;
            newDelivery.ToPartnerCardId = this.ConsigneeId;
            newDelivery.ToAddressId = this.DeliveryAddressId;
            this.EntityPM.AddDelivery(newDelivery);
        }
    }
   
    private isGetFromStock: boolean = false;
    private StackDomainService: AWBStackDomainService;
    ValidateMasterStack() {

        var isLoadingStack = false;

        if (!AppTool.IsNullOrEmpty(this.Master)) {
            if (this.Master.length == 8 && !this.EntityPM.MainCarriageIsFromStack && !this.EntityPM.MAWBTakenFromStack) {
                if (FormatTool.IsNumeric(this.Master)) {

                    isLoadingStack = true;

                    if (this.StackDomainService == null) {
                        this.StackDomainService = new AWBStackDomainService();
                    }

                    this.StackDomainService.GetMAWBStackPMByNumber(+this.Master).subscribe((myResponse: ServiceResponse) => {
                        var isCreating = false;

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            this.CurrentSession.StopBusyIndicator();
                        }

                        else {
                            var myStackPM: MAWBStackPM = myResponse.Result;

                            if (myStackPM == null) {
                                isCreating = true;
                            }

                            else {
                                var myAirlineId: string = this.MainCarriageCarrierId;
                                if (!AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
                                    myAirlineId = this.EntityPM.InterlineId;
                                }

                                if (myStackPM.AirlineId == myAirlineId) {
                                    if (!AppTool.IsNullOrEmpty(myStackPM.AssignedToId) && myStackPM.AssignedToId != this.EntityPM.ShipperId) {

                                        this.CurrentSession.StopBusyIndicator();

                                        var messageWindow = new MessageWindow();
                                        messageWindow.Width = 450;
                                        messageWindow.Height = 190;
                                        messageWindow.Title = "Invalid Master";
                                        messageWindow.Show("AWB is assigned to another shipper");
                                        this.Master = null;
                                    }

                                    else {
                                        this.CurrentSession.StopBusyIndicator();

                                        var confirmWindow = new ConfirmWindow();
                                        confirmWindow.Title = "AWB exists in the stock";
                                        confirmWindow.Show("Do you want to get this awb from stock?");
                                        confirmWindow.WindowClosed.subscribe((event: any) => {
                                            if (confirmWindow.Yes) {
                                                this.EntityPM.MAWBTakenFromStack = true;
                                                this.EntityPM.MAWBStackNumber = AppTool.PadLeft(myStackPM.Number.toString(), 8, '0');
                                                this.MAWBOBLDate = DateTool.GetCurrentDateTimeAsUtc();
                                                this.SetMAWBAirline();
                                                this.isGetFromStock = true;

                                                this.SubmitCreatingShipment();
                                            }

                                            else {
                                                this.Master = null;
                                                this.CurrentSession.StopBusyIndicator();
                                            }
                                        });
                                    }
                                }

                                else {
                                    isCreating = true;
                                }
                            }
                        }

                        if (isCreating) {
                            this.SubmitCreatingShipment();
                        }
                    });
                }
            }
        }

        if (!isLoadingStack) {
            this.SubmitCreatingShipment();
        }
    }
    SetMAWBAirline() {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    }

    SubmitCreatingShipment() {
        this.CurrentSession.StartBusyIndicator("Creating...");

        this.SetDataOnFinish();

        this.myShipmentPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.EntityPM = myResponse.Result;

                if (this.EntityPM) {
                    var transMode = (this.EntityPM.TransportModeId == "A" ? "Air" : (this.EntityPM.TransportModeId == "O" ? "Ocean" : (this.EntityPM.TransportModeId == "I" ? "Inland" : "")));
                    var activity = "New " + transMode + (!AppTool.IsNullOrEmpty(this.EntityPM.ShipmentType) ? " " + this.EntityPM.ShipmentType : "") + " " + this.ObjectTableName;
                    ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, activity);
                }
              

                this.CurrentSession.CloseCurrentWindowEmit('OK');

                if (this.IsBuildFromQuote || this.IsCopyFromShipment) {

                    var myBackButtonLabel: string = null;
                    var myBackSessionTextCode: string = null;

                    if (this.IsBuildFromQuote) {
                        myBackButtonLabel = "Quote: " + this.SourceEntityPM.QuoteNumber;
                        myBackSessionTextCode = "General.MH.Quotes";
                    }

                    else {
                        myBackButtonLabel = "Shipment: " + this.SourceEntityPM.ShipmentNumber;
                        myBackSessionTextCode = "General.MH.Operations";
                    }

                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: this.EntityPM.Id, ObjectTableName: 'Shipment', BackButtonLabel: myBackButtonLabel });

                            this.CurrentSession.ChangeSessionHeader({ MenuTextCode: "General.MH.Operations" });                            

                            cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                this.CurrentSession.ChangeSessionHeader({ MenuTextCode: myBackSessionTextCode });  

                                if (this.IsBuildFromQuote) {
                                    this.CurrentSession.FireEvent("LoadConnectedShipments");
                                }             
                            });
                        });
                }
            }
        });
    }
}

export class FilterClass {
    public Code: string;
    public Name: string;
    public SRC: string;
    constructor(code: string, name: string, src: string = null) {
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
}
