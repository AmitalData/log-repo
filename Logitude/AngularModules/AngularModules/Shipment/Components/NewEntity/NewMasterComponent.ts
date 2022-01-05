import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, FormatTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentTool} from '../../Tools';
import {ShipmentValidator} from '../../Validators/ShipmentValidator';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentOrderPackagePM} from '../../EntityPMs/ShipmentOrderPackagePM';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {MAWBStackPM} from '../../../Common/EntityPMs/MAWBStackPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {PortList} from '../../../Common/EntityLists/PortList';
import {CardList} from '../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../Common/EntityLists/AirlineList';
import {AddressList} from '../../../Common/EntityLists/AddressList';
import {PackageTypeList} from '../../../Common/EntityLists/PackageTypeList';
import {PortListService} from '../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../Common/Services/StandardLists/AirlineListService';
import {AddressListService} from '../../../Common/Services/StandardLists/AddressListService';
import {IncotermListService} from '../../../Common/Services/StandardLists/IncotermListService';
import {ShipmentPMService} from '../../Services/StandardPMs/ShipmentPMService';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ShipmentDomainService} from '../../Services/ShipmentDomainService';
import {AWBStackDomainService} from '../../../Common/Services/AWBStackDomainService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ChildDirective } from '../../../Infrastructure/Directives/ChildDirective';
import { ContactInputTemplateArgs } from '../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ShipmentSubTypeListService } from '../../services/standardlists/shipmentsubtypelistservice';
import { ShipmentSubTypeList } from '../../EntityLists/ShipmentSubTypeList';
import { FeatureToggleList } from '../../../Infrastructure/EntityLists/FeatureToggleList';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    templateUrl: './NewMasterComponent.html',
})

export class NewMasterComponent extends BaseComponent implements OnInit, AfterViewInit {
    public ShipmentTypeName: string = null;

    public TenantPM: TenantPM;
    public EntityPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "Master";
    public LabelColumnWidth: number = 115;
    public ControlColumnWidth: number = 220;
    public ValidationErrorsList: string[] = [];
    public SessionIndex: number;
    public OkButtonLabel: string;
    public AgentDependencyProperty1: string = "AG";
    public AgentDependencyProperty1IsList: boolean = false;
    @ViewChild(ChildDirective) Child: ChildDirective;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsChooseVesselVisible: boolean = false;
    constructor() {
        super();
      this.SessionIndex = this.CurrentSession.SessionIndex;
        this.InitializeServices();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = this.myShipmentPMService.GetNewEntityPM();
        this.EntityPM.ShipmentLevelCode = "C";
        this.OkButtonLabel = TextCodeTranslator.Translate("Shipment.B.Create");

        if (this.TenantPM.AllowCustomersInAgentsLOV) {
            this.AgentDependencyProperty1 = "CS,AG";
            this.AgentDependencyProperty1IsList = true;
        }
    }

    public SubTypeFeatureToggle: FeatureToggleList;
    public ScreenIsReady: boolean = false;
    ngOnInit() {
        var listservice: EntityListService = new EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then((res: any) => {
            res.subscribe((resp: any) => {
                this.SubTypeFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SUB")[0]; 
                this.ScreenIsReady = true;

                this.BuildFiltersLists();

                if (this.IsCopyFromShipment == false && this.IsBuildFromQuote == false) {
                    this.OnFiltersChanged();
                }

                this.LoadAllowedAirline();
                this.LoadShipmentSubTypes();
            });
        });

    }

    private GeneratedComponent: any;
    ngAfterViewInit() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.Child.Location)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = "NewMaster";
                cmpRef.instance.LabelWidth = 110;
                cmpRef.instance.Run(this.EntityPM, "Shipment", screenCode, false, false, this.ObjectTableName);
            });
    }

    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsScreenEnabled);
        }
    }

    SourceEntityPM: ShipmentPM;
    public IsBuildFromQuote: boolean = false;
    public IsCopyFromShipment: boolean = false;
    public IsMasterCreatedFromHouse: boolean = false;
    SetWindowArgs(args: any) {
        if (args.IsNew == null) {
            this.SourceEntityPM = args.Shipment;
            this.IsCopyFromShipment = args.IsCopyFromShipment;
            this.IsBuildFromQuote = args.IsBuildFromQuote;
            this.IsMasterCreatedFromHouse = args.IsMasterCreatedFromHouse;

            this.BuildFiltersLists();
            this.SetUIProperties();
            this.CopyEntityData();
        }
    }

    private myPortListService: PortListService;
    private myCardListService: CardListService;
    private myAirlineListService: AirlineListService;
    private myAddressListService: AddressListService;
    private myIncotermListService: IncotermListService;
    private myPartnersDomainService: PartnersDomainService;
    private myShipmentPMService: ShipmentPMService;
    private myShipmentSubTypeListService: ShipmentSubTypeListService;
    InitializeServices() {
        this.myPortListService = new PortListService();
        this.myCardListService = new CardListService();
        this.myAirlineListService = new AirlineListService();
        this.myAddressListService = new AddressListService();
        this.myIncotermListService = new IncotermListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.myShipmentPMService = new ShipmentPMService();
        this.myShipmentSubTypeListService = new ShipmentSubTypeListService();
    }
    LoadAllowedAirline() {
        if (SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    if (this.EntityPM.TransportModeId == "A") {
                        if (this.EntityPM.ShipmentLevelCode != "H") {

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

    private allShipmentSubTypes: ShipmentSubTypeList[] = [];
    LoadShipmentSubTypes() {
        this.allShipmentSubTypes = [];

        this.myShipmentSubTypeListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allShipmentSubTypes = myResponse.Result;
                this.allShipmentSubTypes = this.allShipmentSubTypes.filter(d => !d.Inactive);
            }
        });
    }

    public DirectionsList: FilterClass[] = [];
    public TransportModesList: FilterClass[] = [];
    public ShipmentTypesList: FilterClass[] = [];
    public ShipmentLevelsList: FilterClass[] = [];
    public ShipmentSubTypesList: FilterClass[] = [];
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

        var isAirExportOnly: boolean = FeatureLocator.HasFeaturePermession(this.ObjectTableName, "AIREXPORTONLY");
        var isAllTransportModes: boolean = FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ALLTRANSPORTMODES");

        if (isAirExportOnly && !isAllTransportModes) {
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

            if (this.DirectionId != "D") {
                this.TransportModesList.push(transport_I);
            }
        }

        this.BuildShipmentTypes();
    }
    BuildShipmentTypes() {
        this.ShipmentTypesList = [];

        if (this.DirectionId && this.TransportModeId) {
            switch (this.TransportModeId) {
                case "O": {
                    var item1 = new FilterClass("FCLD", "FCL Consol", "./Images/CellIcons/Container.png");
                    item1.HasHelp = true;
                    item1.HelpText = "You can consolidate only files type FCL - Like FCL direct just you can consolidate few files.";

                    var item2 = new FilterClass("LCLD", "LCL Consol", "./Images/CellIcons/Package.png");
                    item2.HasHelp = true;
                    item2.HelpText = "You can consolidate only files type LCL/bulk - Like LCL direct just you can consolidate few files .";

                    var item3 = new FilterClass("MyGO", "Groupage", "./Images/CellIcons/Groupage.png");
                    item3.HasHelp = true;
                    item3.HelpText = "The Master will be FCL the house will be LCL, you must stuff ALL the LCL files inside the container or containers that exist in the Master.";

                    this.ShipmentTypesList.push(item1);
                    this.ShipmentTypesList.push(item2);
                    this.ShipmentTypesList.push(item3);
                    break;
                }

                case "I": {
                    this.ShipmentTypesList.push(new FilterClass("FTL", "FTL", "./Images/CellIcons/Container.png"));
                    this.ShipmentTypesList.push(new FilterClass("LTL", "LTL", "./Images/CellIcons/Package.png"));
                    this.ShipmentTypesList.push(new FilterClass("MyGI", "Groupage", "./Images/CellIcons/Groupage.png"));
                    break;
                }
            }
        }
    }
    BuildShipmentSubTypes() {
        this.ShipmentSubTypesList = [];

        this.allShipmentSubTypes.filter(d => d.ShipmentTypeCode == this.ShipmentTypeId  || d.ShipmentTypeCode == null).forEach(item => {
            this.ShipmentSubTypesList.push(new FilterClass(item.Id, item.Name));
        });

        this.SetDefaultSubType();
    }
    private SetDefaultSubType() {
        var subTypeCode: string = null;
        switch (this.ShipmentTypeId) {
            case "Air":
                {
                    subTypeCode = "Air";
                    break;
                }

            case "FCLD":
                {
                    subTypeCode = "FCL";
                    break;
                }

            case "LCLD":
                {
                    subTypeCode = "LCL";
                    break;
                }

            case "FTL":
                {
                    subTypeCode = "FTL";
                    break;
                }

            case "LTL":
                {
                    subTypeCode = "LTL";
                    break;
                }

            case "MyGI":
                {
                    subTypeCode = "MyGI";
                    break;
                }

            case "MyGO":
                {
                    subTypeCode = "MyGO";
                    break;
                }
        }

        var subType: ShipmentSubTypeList = this.allShipmentSubTypes.filter(d => d.Code == subTypeCode)[0];
        if (subType) {
            this.ShipmentSubTypeId = subType.Id;
        }

        else {
            var defaultSubType: ShipmentSubTypeList = this.allShipmentSubTypes.filter(d => d.ShipmentTypeCode == this.ShipmentTypeId)[0];
            if (defaultSubType) {
                this.ShipmentSubTypeId = defaultSubType.Id;
            }
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

        // Agent
        this.UIProperties.SetEnabled("AgentId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("AgentAddressId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("AgentContactId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("AgentReference1", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("AgentReference2", this.ObjectTableName, isScreenEnabled);

        // General
        if (this.IsMasterCreatedFromHouse) {
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
        this.UIProperties.SetEnabled("MAWBOBLDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageVesselName", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("FreightPrepaidCollectId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("OtherPrepaidCollectId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isScreenEnabled);

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
        if (!this.IsMasterCreatedFromHouse) {
            this.IsDirectionListEnabled = true;
            this.IsTransportModesListEnabled = AppTool.IsNullOrEmpty(this.DirectionId) ? false : true;
            this.IsMasterTypesListEnabled = true;
            this.IsShipmentSubTypesListEnabled = true;
        }

        this.SetScreenEnabled();
        this.SetUIProperties();
        this.SetUnits();
        this.SetLabels();
        this.SetPartners();
        this.SetPrepaidCollect();
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

    private isMasterTypesListEnabled: boolean = false;
    get IsMasterTypesListEnabled() { return this.isMasterTypesListEnabled; }
    set IsMasterTypesListEnabled(value: boolean) {
        if (this.isMasterTypesListEnabled != value) {
            this.isMasterTypesListEnabled = value;
        }
    }

    private isShipmentSubTypesListEnabled: boolean = false;
    get IsShipmentSubTypesListEnabled() { return this.isShipmentSubTypesListEnabled; }
    set IsShipmentSubTypesListEnabled(value: boolean) {
        if (this.isShipmentSubTypesListEnabled != value) {
            this.isShipmentSubTypesListEnabled = value;
        }
    }

    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId != newValue) {
            this.EntityPM.DirectionId = newValue;
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            this.SetTransportModes();
            this.OnFiltersChanged();
        }
    }

    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
            this.MainCarriageFromPortId = null;
            this.MainCarriageToPortId = null;
            this.MainCarriageCarrierId = null;

            if (newValue == "A") {
                this.ShipmentTypeId = "Air";
            }

            else {
                this.ShipmentTypeId = null;
            }

            this.SetOrderDetails();
            this.OnFiltersChanged();
            this.BuildShipmentTypes();
            this.BuildShipmentSubTypes();
            this.LoadAllowedAirline();
            this.ValidateMasterField();
        }
    }

    get ShipmentTypeId() { return this.EntityPM.ShipmentTypeId; }
    set ShipmentTypeId(newValue: string) {
        if (this.EntityPM.ShipmentTypeId != newValue) {
            this.EntityPM.ShipmentTypeId = newValue;
            this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

            this.BuildShipmentSubTypes();
            this.SetOrderDetails();
            this.OnFiltersChanged();
        }
    }

    get ShipmentLevelCode() { return this.EntityPM.ShipmentLevelCode; }
    set ShipmentLevelCode(newValue: string) {
        if (this.EntityPM.ShipmentLevelCode != newValue) {
            this.EntityPM.ShipmentLevelCode = newValue;
            this.SetUIProperties();
            this.SetScreenEnabled();
            this.SetOrderDetails();
            this.ValidateMasterField();
        }
    }

    get ShipmentSubTypeId() { return this.EntityPM.ShipmentSubTypeId; }
    set ShipmentSubTypeId(newValue: string) {
        if (this.EntityPM.ShipmentSubTypeId != newValue) {
            this.EntityPM.ShipmentSubTypeId = newValue;
        }
    }

    public FromTextCode: string;
    public ToTextCode: string;
    public CarrierTextCode: string;
    public CarrierNumberTextCode: string;
    public CarrierDependencyProperty1: string;
    public MasterTextCode: string;
    public MasterDateTextCode: string;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    SetLabels() {
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCode = "Master.S.NewMaster.Gateway";
                this.ToTextCode = "Master.S.NewMaster.Destination";
                this.CarrierTextCode = "Master.S.NewMaster.Airline";
                this.CarrierNumberTextCode = "Master.S.NewMaster.FlightNo";
                this.CarrierDependencyProperty1 = "AL";
                this.MasterTextCode = "Master.S.NewMaster.MAWB";
                this.MasterDateTextCode = "Master.S.NewMaster.MAWBDate";
                break;
            }

            case "O": {
                this.FromTextCode = "Master.S.NewMaster.LoadingPort";
                this.ToTextCode = "Master.S.NewMaster.DischargePort";
                this.CarrierTextCode = "Master.S.NewMaster.Shippingline";
                this.CarrierNumberTextCode = "Master.S.NewMaster.VoyageNo";
                this.CarrierDependencyProperty1 = "SL";
                this.MasterTextCode = "Master.S.NewMaster.OBL";
                this.MasterDateTextCode = "Master.S.NewMaster.OBLDate";
                break;
            }

            case "I": {
                this.FromTextCode = "Master.S.NewMaster.From";
                this.ToTextCode = "Master.S.NewMaster.To";
                this.CarrierTextCode = "Master.S.NewMaster.Trucker";
                this.CarrierNumberTextCode = "Master.S.NewMaster.TruckerNo";
                this.CarrierDependencyProperty1 = "TR";
                this.MasterTextCode = "Master.S.NewMaster.CMR/RWB#";
                this.MasterDateTextCode = "Master.S.NewMaster.CMR/RWBDate";
                break;
            }

            default: {
                this.FromTextCode = "Master.S.NewMaster.From";
                this.ToTextCode = "Master.S.NewMaster.To";
                this.CarrierTextCode = "Master.S.NewMaster.Carrier";
                this.CarrierNumberTextCode = "Master.S.NewMaster.No";
                this.MasterTextCode = "Master.S.NewMaster.MAWB";
                this.MasterDateTextCode = "Master.S.NewMaster.MAWBDate";
                break;
            }
        }

        this.VolumeLabel = TextCodeTranslator.Translate("Master.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Master.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);

        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Master.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }

        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Master.F.ChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    }
    SetUnits() {
        var myDimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        var myVolumeUnitCode = this.TenantPM.VolumeUnitCode;
        var myGrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        var myChargeableWeightUnitCode = AppTool.GetChargeableWeightUnitCode(this.TransportModeId);

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


            this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;


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
        }

        this.ComputeOrderVolumetricWeight();
        this.ComputeChargeableWeight();
    }
    SetPartners() {

    }
    SetPrepaidCollect() {
        switch (this.DirectionId) {
            case "E":
            case "R": {
                this.FreightPrepaidCollectId = this.TenantPM.MasterExportFreightPrepaidCollectId;
                this.OtherPrepaidCollectId = this.TenantPM.MasterExportOtherPrepaidCollectId;
                break;
            }

            case "I": {
                this.FreightPrepaidCollectId = this.TenantPM.MasterImportFreightPrepaidCollectId;
                this.OtherPrepaidCollectId = this.TenantPM.MasterImportOtherPrepaidCollectId;
                break;
            }

            case "D": {
                this.FreightPrepaidCollectId = "P";
                this.OtherPrepaidCollectId = "P";
                break;
            }
        }
    }
    SetOrderDetails() {
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

    SetUIProperties() {
        this.IsChooseVesselVisible = false;
        if (this.TransportModeId == "O") {
            this.IsChooseVesselVisible = true;
        }

        this.SetUIProperties_Agent();
        this.SetUIProperties_Ports();
        this.SetUIProperties_MasterField();

        if (this.IsLCLEntity) {
            this.SetUIProperties_OrderDetails();
        }

        else {
            this.SetUIProperties_Containers();
        }
    }
    SetUIProperties_Agent() {
        var isFieldRequired: boolean = false;

        if (AppTool.IsNullOrEmpty(this.AgentId)) {
            isFieldRequired = true;
        }

        this.UIProperties.SetRequired("AgentId", this.ObjectTableName, isFieldRequired);
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
        var isFieldEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            isFieldEnabled = true;
        }

        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isFieldEnabled);
    }
    SetUIProperties_OrderDetails() {
        var isFieldsEnabled = this.EntityPM.ShipmentOrderPackages.length == 0 ? true : false;
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isFieldsEnabled);

        var isNoOfPackagesRequired = false;
        if (!AppTool.IsNullOrZero(this.OrderGrossWeight) || !AppTool.IsNullOrZero(this.BookingVolume) || !AppTool.IsNullOrZero(this.OrderChargeableWeight)) {
            if (AppTool.IsNullOrZero(this.BookingNumberOfPackages)) {
                isNoOfPackagesRequired = true;
            }
        }

        this.UIProperties.SetRequired("BookingNumberOfPackages", this.ObjectTableName, isNoOfPackagesRequired);
    }
    SetUIProperties_Containers() {
        this.UIProperties.SetEnabled("PackageTypeId1", this.ObjectTableName, this.Quantity1 > 0);
        this.UIProperties.SetEnabled("PackageTypeId2", this.ObjectTableName, this.Quantity2 > 0);
        this.UIProperties.SetEnabled("PackageTypeId3", this.ObjectTableName, this.Quantity3 > 0);
        this.UIProperties.SetEnabled("PackageTypeId4", this.ObjectTableName, this.Quantity4 > 0);
        this.UIProperties.SetEnabled("PackageTypeId5", this.ObjectTableName, this.Quantity5 > 0);

        if (AppTool.IsNullOrZero(this.Quantity1)) {
            this.PackageTypeId1 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity2)) {
            this.PackageTypeId2 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity3)) {
            this.PackageTypeId3 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity4)) {
            this.PackageTypeId4 = null;
        }

        if (AppTool.IsNullOrZero(this.Quantity5)) {
            this.PackageTypeId5 = null;
        }
    }

    // Agent
    get AgentId() { return this.EntityPM.AgentId; }
    set AgentId(newValue: string) {
        if (this.EntityPM.AgentId != newValue) {
            this.EntityPM.AgentId = newValue;
            this.SetUIProperties_Agent();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.AgentContactId = null;
                this.EntityPM.AgentName = null;
                this.EntityPM.AgentNote = null;
                this.EntityPM.AgentReference1 = null;
                this.EntityPM.AgentReference2 = null;
                this.AgentAddressId = null;
                this.IsCustomerCreditLimitEnabled = false;
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
                            this.IsCustomerCreditLimitEnabled = myCardList.IsCreditLimitEnabled; 
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
                        this.AgentAddressList = myResponse.Result;
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

    // General
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
            this.myShipmentDomainService.ValidateShipmentMasterFieldExistance(this.EntityPM)
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

    get MainCarriageVesselName() { return this.EntityPM.MainCarriageVesselName; }
    set MainCarriageVesselName(newValue: string) {
        if (this.EntityPM.MainCarriageVesselName != newValue) {
            this.EntityPM.MainCarriageVesselName = newValue;
            this.MainCarriageVesselId = null;
        }
    }

    ChooseVesselClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, IdProperty: "MainCarriageVesselId", NameProperty: "MainCarriageVesselName" };
        logitudeWindow.Title = "Vessel Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/ChooseVesselComponent");
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
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

    // Expected Order Details
    get OrderGrossWeight() { return this.EntityPM.OrderGrossWeight; }
    set OrderGrossWeight(newValue: number) {
        if (this.EntityPM.OrderGrossWeight != newValue) {
            this.EntityPM.OrderGrossWeight = AppTool.Round(newValue, 3);
            this.ComputeChargeableWeight();
            this.SetUIProperties_OrderDetails();
        }
    }

    get BookingVolume() { return this.EntityPM.BookingVolume; }
    set BookingVolume(newValue: number) {
        if (this.EntityPM.BookingVolume != newValue) {
            this.EntityPM.BookingVolume = AppTool.Round(newValue, 3);
            this.ComputeOrderVolumetricWeight();
            this.SetUIProperties_OrderDetails();
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

            this.SetUIProperties_OrderDetails();

            if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                this.EntityPM.OrderVolumetricWeight = myResult;
                this.EntityPM.OrderGrossWeight = AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, myResult);
                this.EntityPM.BookingVolume = AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.OrderVolumetricWeight, this.EntityPM.Ratio);
            }
        }
    }

    get BookingNumberOfPackages() { return this.EntityPM.BookingNumberOfPackages; }
    set BookingNumberOfPackages(newValue: number) {
        if (this.EntityPM.BookingNumberOfPackages != newValue) {
            this.EntityPM.BookingNumberOfPackages = newValue;

            this.SetUIProperties_OrderDetails();
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
            this.SetUIProperties_Containers();
        }
    }

    get Quantity2() { return this.EntityPM.Quantity2; }
    set Quantity2(newValue: number) {
        if (this.EntityPM.Quantity2 != newValue) {
            this.EntityPM.Quantity2 = newValue;
            this.SetUIProperties_Containers();
        }
    }

    get Quantity3() { return this.EntityPM.Quantity3; }
    set Quantity3(newValue: number) {
        if (this.EntityPM.Quantity3 != newValue) {
            this.EntityPM.Quantity3 = newValue;
            this.SetUIProperties_Containers();
        }
    }

    get Quantity4() { return this.EntityPM.Quantity4; }
    set Quantity4(newValue: number) {
        if (this.EntityPM.Quantity4 != newValue) {
            this.EntityPM.Quantity4 = newValue;
            this.SetUIProperties_Containers();
        }
    }

    get Quantity5() { return this.EntityPM.Quantity5; }
    set Quantity5(newValue: number) {
        if (this.EntityPM.Quantity5 != newValue) {
            this.EntityPM.Quantity5 = newValue;
            this.SetUIProperties_Containers();
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

    // Copy
    public CopyCheckBoxTop: number = 5;
    public IsCopyOtherPartnersVisible: boolean = false;
    CopyEntityData() {
        if (this.IsBuildFromQuote || this.IsCopyFromShipment) {
            this.EntityPM.IsBuildFromQuote = this.IsBuildFromQuote;
            this.EntityPM.IsCopyFromShipment = this.IsCopyFromShipment;

            this.DirectionId = this.SourceEntityPM.DirectionId;
            this.TransportModeId = this.SourceEntityPM.TransportModeId;
            this.ShipmentTypeId = this.SourceEntityPM.ShipmentTypeId;

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

                if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeId)) {
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
            if (!AppTool.IsNullOrEmpty(this.SourceEntityPM.AgentId)) {
                this.IsCopyAgent = true;
                this.IsCopyAgentEnabled = true;
            }
        }

        else if (this.IsBuildFromQuote) {
            //this.IsCopyShipper = true;
            //this.IsCopyConsignee = true;
            //this.IsCopyAgent = true;
            //this.IsCopyCustomAgentExport = true;
            //this.IsCopyCustomAgentImport = true;
            //this.IsCopyNotify1 = true;
            //this.IsCopyNotify2 = true;
            //this.IsCopyShipperNotExporter = true;
            //this.IsCopyConsigneeNotImporter = true;
            //this.IsCopyFreightForwarder = true;
            //this.IsCopyConsolidator = true;
        }
    }

    public IsCopyAgentEnabled: boolean = false;

    private isCopyAgent: boolean = false;
    get IsCopyAgent() { return this.isCopyAgent; }
    set IsCopyAgent(value: boolean) {
        if (this.isCopyAgent != value) {
            this.isCopyAgent = value;

            this.EntityPM.AgentId = !value ? null : this.SourceEntityPM.AgentId;
            this.EntityPM.AgentName = !value ? null : this.SourceEntityPM.AgentName;
            this.EntityPM.AgentNote = !value ? null : this.SourceEntityPM.AgentNote;
            this.EntityPM.AgentAddressId = !value ? null : this.SourceEntityPM.AgentAddressId;
            this.EntityPM.AgentContactId = !value ? null : this.SourceEntityPM.AgentContactId;
            //this.EntityPM.AgentReference1 = !value ? null : this.SourceEntityPM.AgentReference1;
            //this.EntityPM.AgentReference2 = !value ? null : this.SourceEntityPM.AgentReference2;
            this.SetUIProperties_Agent();
        }
    }

    private isCopyShipper: boolean = false;
    get IsCopyShipper() { return this.isCopyShipper; }
    set IsCopyShipper(value) {
        if (this.isCopyShipper != value) {
            this.isCopyShipper = value;

            this.EntityPM.ShipperId = !value ? null : this.SourceEntityPM.ShipperId;
            this.EntityPM.ShipperName = this.SourceEntityPM.ShipperName;
            this.EntityPM.ShipperNote = this.SourceEntityPM.ShipperNote;
            this.EntityPM.ShipperAddressId = !value ? null : this.SourceEntityPM.ShipperAddressId;
            this.EntityPM.ShipperContactId = !value ? null : this.SourceEntityPM.ShipperContactId;
            //this.EntityPM.ShipperReference1 = !value ? null : this.SourceEntityPM.ShipperReference1;
            //this.EntityPM.ShipperReference2 = !value ? null : this.SourceEntityPM.ShipperReference2;
        }
    }

    private isCopyConsignee: boolean = false;
    get IsCopyConsignee() { return this.isCopyConsignee; }
    set IsCopyConsignee(value) {
        if (this.isCopyConsignee != value) {
            this.isCopyConsignee = value;

            this.EntityPM.ConsigneeId = !value ? null : this.SourceEntityPM.ConsigneeId;
            this.EntityPM.ShipperName = !value ? null : this.SourceEntityPM.ShipperName;
            this.EntityPM.ShipperNote = !value ? null : this.SourceEntityPM.ShipperNote;
            this.EntityPM.ConsigneeAddressId = !value ? null : this.SourceEntityPM.ConsigneeAddressId;
            this.EntityPM.ConsigneeContactId = !value ? null : this.SourceEntityPM.ConsigneeContactId;
            //this.EntityPM.ConsigneeReference1 = !value ? null : this.SourceEntityPM.ConsigneeReference1;
            //this.EntityPM.ConsigneeReference2 = !value ? null : this.SourceEntityPM.ConsigneeReference2;            
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

            this.EntityPM.ConsolidatorId = !value ? null : this.SourceEntityPM.ConsolidatorId;
            this.EntityPM.ConsolidatorName = !value ? null : this.SourceEntityPM.ConsolidatorName;
            this.EntityPM.ConsolidatorNote = !value ? null : this.SourceEntityPM.ConsolidatorNote;
            this.EntityPM.ConsolidatorAddressId = !value ? null : this.SourceEntityPM.ConsolidatorAddressId;
            this.EntityPM.ConsolidatorContactId = !value ? null : this.SourceEntityPM.ConsolidatorContactId;
            //this.EntityPM.ConsolidatorReference = !value ? null : this.SourceEntityPM.ConsolidatorReference;
        }
    }

    private isCopyFlights: boolean = false;
    get IsCopyFlights() { return this.isCopyFlights; }
    set IsCopyFlights(value) {
        if (this.isCopyFlights != value) {
            this.isCopyFlights = value;
        }
    }

    private isCopyPreCarriage: boolean = false;
    get IsCopyPreCarriage() { return this.isCopyPreCarriage; }
    set IsCopyPreCarriage(value) {
        if (this.isCopyPreCarriage != value) {
            this.isCopyPreCarriage = value;

            if (value == true) {
                this.CopyPreCarriageValues();
            }

            else {
                this.SetPreCarriageValuesToNull();

            }
        }
    }

    private isCopyOnCarriage: boolean = false;
    get IsCopyOnCarriage() { return this.isCopyOnCarriage; }
    set IsCopyOnCarriage(value) {
        if (this.isCopyOnCarriage != value) {
            this.isCopyOnCarriage = value;

            if (value == true) {
                this.CopyOnCarriageValues();
            }

            else {
                this.SetOnCarriageValuesToNull();

            }
        }
    }

    private CopyPreCarriageValues() {
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
    private CopyOnCarriageValues() {
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

    private SetPreCarriageValuesToNull() {
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
    private SetOnCarriageValuesToNull() {
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
                this.EntityPM.GrossWeightPerStorageDays = this.SourceEntityPM.GrossWeightPerStorageDays;
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
                this.EntityPM.GrossWeightPerStorageDays = null;
            }

            this.SetUIProperties_OrderDetails();
        }
    }

    // Commands
    CancelButtonClicked() {
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = false;
            confirmWindow.YesButtonText = "Don't Save";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show("You are about to cancel Shipment and all data will be lost");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CloseWizardWindow();
                }
                else if (confirmWindow.No) {
                    //nth
                }
            });
        }
        else {
            this.CloseWizardWindow();
        }
    }
    private CloseWizardWindow() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        this.CurrentSession.StartBusyIndicator("Creating...");

        this.SetDataOnFinish();

        var validator = new ShipmentValidator();
        this.ValidationErrorsList = validator.Validate(this.EntityPM);

        if (!AppTool.IsNullOrZero(this.OrderGrossWeight) || !AppTool.IsNullOrZero(this.BookingVolume) || !AppTool.IsNullOrZero(this.OrderChargeableWeight)) {
            if (AppTool.IsNullOrZero(this.BookingNumberOfPackages)) {
                this.ValidationErrorsList.push("Number of Packages is required");
            }
        }

        if (AppTool.IsNullOrEmpty(this.AgentId)) {
            var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Master.F.AgentId")));
        }

        if (this.ValidationErrorsList.length == 0) {

            if (this.IsValidatingMasterField) {

                if (!this.IsMasterFieldValid) {
                    this.ValidationErrorsList.push(this.MasterFieldValidityMessage);
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
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;

        if (this.IsCopyFlights) {
            ShipmentTool.CopyFlights(this.EntityPM, this.SourceEntityPM);
        }

        this.SetPartnersOnFinish();
        this.SetCountryECOnFinish();
        this.SetOrderPackagesOnFinish();
        this.SetPickupDeliveryOnFinish();
        this.SetInlandDomesticOnFinish();
    }
    SetPartnersOnFinish() {
        if (!AppTool.IsNullOrEmpty(this.AgentId)) {
            if (this.IsCopyFromShipment == false) {
                if (this.DirectionId == "E") {
                    this.EntityPM.ConsigneeId = this.AgentId;
                    this.EntityPM.ConsigneeName = this.AgentName;
                    this.EntityPM.ConsigneeAddressId = this.AgentAddressId;
                    this.EntityPM.ConsigneeContactId = this.AgentContactId;
                    this.EntityPM.ConsigneeReference1 = this.AgentReference1;
                    this.EntityPM.ConsigneeReference2 = this.AgentReference2;
                    this.EntityPM.ShipperId = SessionLocator.TenantPM.AgentId;
                    this.EntityPM.ShipperAddressId = SessionLocator.TenantPM.AddressId;
                }

                else {
                    this.EntityPM.ShipperId = this.AgentId;
                    this.EntityPM.ShipperName = this.AgentName;
                    this.EntityPM.ShipperAddressId = this.AgentAddressId;
                    this.EntityPM.ShipperContactId = this.AgentContactId;
                    this.EntityPM.ShipperReference1 = this.AgentReference1;
                    this.EntityPM.ShipperReference2 = this.AgentReference2;
                    this.EntityPM.ConsigneeId = SessionLocator.TenantPM.AgentId;
                    this.EntityPM.ConsigneeAddressId = SessionLocator.TenantPM.AddressId;
                }
            }

            else {
                if (this.DirectionId == "E") {
                    if (this.EntityPM.ConsigneeId == null) {
                        this.EntityPM.ConsigneeId = this.AgentId;
                        this.EntityPM.ConsigneeName = this.AgentName;
                        this.EntityPM.ConsigneeAddressId = this.AgentAddressId;
                        this.EntityPM.ConsigneeContactId = this.AgentContactId;
                        this.EntityPM.ConsigneeReference1 = this.AgentReference1;
                        this.EntityPM.ConsigneeReference2 = this.AgentReference2;
                        this.EntityPM.ShipperId = SessionLocator.TenantPM.AgentId;
                        this.EntityPM.ShipperAddressId = SessionLocator.TenantPM.AddressId;
                    }
                }

                else {
                    if (this.EntityPM.ShipperId == null) {
                        this.EntityPM.ShipperId = this.AgentId;
                        this.EntityPM.ShipperName = this.AgentName;
                        this.EntityPM.ShipperAddressId = this.AgentAddressId;
                        this.EntityPM.ShipperContactId = this.AgentContactId;
                        this.EntityPM.ShipperReference1 = this.AgentReference1;
                        this.EntityPM.ShipperReference2 = this.AgentReference2;
                        this.EntityPM.ConsigneeId = SessionLocator.TenantPM.AgentId;
                        this.EntityPM.ConsigneeAddressId = SessionLocator.TenantPM.AddressId;
                    }
                }
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
        //this.EntityPM.ShipmentPickUps = [];
        //this.EntityPM.ShipmentDeliveries = [];

        //if (this.IncludePickUp) {
        //    var typeCode = "PART";
        //    if (AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
        //        typeCode = "CASL";
        //    }

        //    var newPickUp = new ShipmentPickUpPM(this.EntityPM);
        //    newPickUp.Tenant = SessionLocator.Tenant;
        //    newPickUp.PickUpDeliveryTypeCode = "PICK";
        //    newPickUp.PickUpDeliveryFromTypeCode = typeCode;
        //    newPickUp.FromAddressCity = this.FromAddressCity;
        //    newPickUp.FromAddressCountryId = this.FromAddressCountryId;
        //    newPickUp.FromAddressZipCode = this.FromAddressZipCode;
        //    newPickUp.FromPartnerCardId = this.ShipperId;
        //    newPickUp.FromAddressId = this.PickUpAddressId;
        //    newPickUp.PickUpDeliveryToTypeCode = "PORT";
        //    newPickUp.ToPortId = this.MainCarriageFromPortId;
        //    this.EntityPM.AddPickUp(newPickUp);
        //}

        //if (this.IncludeDelivery) {
        //    var typeCode = "PART";
        //    if (AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
        //        typeCode = "CASL";
        //    }

        //    var newDelivery = new ShipmentDeliveryPM(this.EntityPM);
        //    newDelivery.Tenant = SessionLocator.Tenant;
        //    newDelivery.PickUpDeliveryTypeCode = "DELV";
        //    newDelivery.PickUpDeliveryFromTypeCode = "PORT";
        //    newDelivery.FromPortId = this.MainCarriageToPortId;
        //    newDelivery.PickUpDeliveryToTypeCode = typeCode;
        //    newDelivery.ToAddressCity = this.ToAddressCity;
        //    newDelivery.ToAddressCountryId = this.ToAddressCountryId;
        //    newDelivery.ToAddressZipCode = this.ToAddressZipCode;
        //    newDelivery.ToPartnerCardId = this.ConsigneeId;
        //    newDelivery.ToAddressId = this.DeliveryAddressId;
        //    this.EntityPM.AddDelivery(newDelivery);
        //}
    }

    SetInlandDomesticOnFinish() {
        if (this.IsInlandDomestic) {
            this.MainCarriageFromPortId = null;
            this.MainCarriageToPortId = null;
            this.EntityPM.MainCarriageFinalDestinationPortId = null;
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
        var isCheckCreditLimit = this.IsCheckCreditLimit();
        if (isCheckCreditLimit) {
            this.ValidateCreditLimit();
        }
        else {
            this.CompleteSubmitCreatingShipment();
        }
    }
    private IsCustomerCreditLimitEnabled = false;
    IsCheckCreditLimit() {
        var isCheck = false;
        var HasCreditLimitFeature: boolean = false;
        var IsCreditLimitActivated: boolean = false;
        var IsCreditLimitHasAction: boolean = false;
        HasCreditLimitFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        if (HasCreditLimitFeature) {
            IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            IsCreditLimitHasAction = (ObjectsLocator.CreditLimitSettingPM.ShipmentCreationBlock == false && ObjectsLocator.CreditLimitSettingPM.ShipmentCreationWarning == true) ? true : false;
        }

        if (HasCreditLimitFeature && IsCreditLimitActivated && IsCreditLimitHasAction && this.IsCustomerCreditLimitEnabled) {
            isCheck = true;
        }
        return isCheck;
    }

    ValidateCreditLimit() {
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myShipmentDomainService == null) {
            this.myShipmentDomainService = new ShipmentDomainService();
        }
        this.myShipmentDomainService.GetCustomerCreditLimitDetails(this.AgentId, this.EntityPM.QuoteId, this.IsBuildFromQuote).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                var warnings: string = myResponse.Result;
                if (!AppTool.IsNullOrEmpty(warnings)) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 200;
                    confirmWindow.ShowCancelButton = false;
                    confirmWindow.YesButtonText = "Continue";
                    confirmWindow.NoButtonText = "Cancel";
                    confirmWindow.Title = "Credit limit";
                    confirmWindow.Show(warnings);
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.CompleteSubmitCreatingShipment();
                        }
                        else if (confirmWindow.No) {
                            //nth
                        }
                    });
                }
                else {
                    this.CompleteSubmitCreatingShipment();
                }
            }
        });
    }

    CompleteSubmitCreatingShipment() {
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
    AddContact(partnerId: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplateArgs();
        args.CustomerId = partnerId;
        args.CardDependencyProperty1 ="AG";
        args.CustomerLable = "Agent";
        args.ComponentName = "Partners";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewContactWindowClosed($event));
    }

    OnNewContactWindowClosed(arg: any) {
        if (arg != 'cancel') {
            this.AgentContactId = arg;           
        }
    }
}

class FilterClass {
    public Code: string;
    public Name: string;
    public SRC: string;
    public HasHelp: boolean = false;
    public HelpText: string = null;
    constructor(code: string, name: string, src: string = null) {
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
}
