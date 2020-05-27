"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_2 = require("../../Tools");
var ShipmentValidator_1 = require("../../Validators/ShipmentValidator");
var ShipmentOrderPackagePM_1 = require("../../EntityPMs/ShipmentOrderPackagePM");
var PortListService_1 = require("../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../Common/Services/StandardLists/AirlineListService");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var IncotermListService_1 = require("../../../Common/Services/StandardLists/IncotermListService");
var ShipmentPMService_1 = require("../../Services/StandardPMs/ShipmentPMService");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ShipmentDomainService_1 = require("../../Services/ShipmentDomainService");
var AWBStackDomainService_1 = require("../../../Common/Services/AWBStackDomainService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var NewMasterComponent = /** @class */ (function (_super) {
    __extends(NewMasterComponent, _super);
    function NewMasterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Master";
        _this.LabelColumnWidth = 115;
        _this.ControlColumnWidth = 220;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsBuildFromQuote = false;
        _this.IsCopyFromShipment = false;
        _this.DirectionsList = [];
        _this.TransportModesList = [];
        _this.ShipmentTypesList = [];
        _this.ShipmentLevelsList = [];
        _this.ScreenOpacity = 0.7;
        _this.IsScreenEnabled = false;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.IsInlandDomestic = false;
        // General
        _this.FromPortList = null;
        _this.ToPortList = null;
        _this.IsMasterFieldValid = true;
        _this.IsValidatingMasterField = false;
        _this.MasterFieldValidityMessage = null;
        _this.PackageTypeList1 = null;
        _this.PackageTypeList2 = null;
        _this.PackageTypeList3 = null;
        _this.PackageTypeList4 = null;
        _this.PackageTypeList5 = null;
        _this.Retries = 0;
        // Copy
        _this.CopyCheckBoxTop = 5;
        _this.IsCopyOtherPartnersVisible = false;
        _this.IsCopyAgentEnabled = false;
        _this.isCopyAgent = false;
        _this.isCopyShipper = false;
        _this.isCopyConsignee = false;
        _this.isCopyCustomAgentImport = false;
        _this.isCopyCustomAgentExport = false;
        _this.isCopyNotify1 = false;
        _this.isCopyNotify2 = false;
        _this.isCopyShipperNotExporter = false;
        _this.isCopyConsigneeNotImporter = false;
        _this.isCopyFreightForwarder = false;
        _this.isCopyConsolidator = false;
        _this.isCopyFlights = false;
        _this.IsCopyDescriptionEnabled = false;
        _this.isCopyDescription = false;
        _this.IsCopyPackagesEnabled = false;
        _this.isCopyPackages = false;
        _this.isGetFromStock = false;
        _this.SessionIndex = SessionLocator_1.SessionLocator.Index;
        _this.InitializeServices();
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.EntityPM = _this.myShipmentPMService.GetNewEntityPM();
        _this.EntityPM.ShipmentLevelCode = "C";
        _this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Create");
        _this.BuildAdditionalFields();
        return _this;
    }
    NewMasterComponent.prototype.ngOnInit = function () {
        var _this = this;
        var listservice = new EntityListService_1.EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then(function (res) {
            res.subscribe(function (resp) {
                _this.BuildFiltersLists();
                if (_this.IsCopyFromShipment == false && _this.IsBuildFromQuote == false) {
                    _this.OnFiltersChanged();
                }
                _this.LoadAllowedAirline();
                //this.ScreenIsReady = true;
            });
        });
    };
    NewMasterComponent.prototype.SetWindowArgs = function (args) {
        if (args.IsNew == null) {
            this.SourceEntityPM = args.Shipment;
            this.IsCopyFromShipment = args.IsCopyFromShipment;
            this.IsBuildFromQuote = args.IsBuildFromQuote;
            this.BuildFiltersLists();
            this.SetUIProperties();
            this.CopyEntityData();
        }
    };
    NewMasterComponent.prototype.InitializeServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myAirlineListService = new AirlineListService_1.AirlineListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.myIncotermListService = new IncotermListService_1.IncotermListService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        this.myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
    };
    NewMasterComponent.prototype.LoadAllowedAirline = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    if (this.EntityPM.TransportModeId == "A") {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            this.myPartnersDomainService.GetAllowedAirlineId().subscribe(function (myResponse) {
                                if (myResponse != null) {
                                    if (myResponse.HasError) {
                                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                                    }
                                    else {
                                        var allowedAirlineId = myResponse.Result;
                                        if (!Tools_1.AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                            _this.MainCarriageCarrierId = allowedAirlineId;
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    };
    NewMasterComponent.prototype.BuildFiltersLists = function () {
        this.DirectionsList = [];
        this.TransportModesList = [];
        var direct_E = new FilterClass("E", "Export");
        var direct_I = new FilterClass("I", "Import");
        var direct_D = new FilterClass("D", "Domestic");
        var direct_R = new FilterClass("R", "Drop");
        var transport_A = new FilterClass("A", "Air");
        var transport_O = new FilterClass("O", "Ocean");
        var transport_I = new FilterClass("I", "Inland");
        var isAirExportOnly = FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "AIREXPORTONLY");
        var isAllTransportModes = FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ALLTRANSPORTMODES");
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
    };
    NewMasterComponent.prototype.BuildShipmentTypes = function () {
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
    };
    NewMasterComponent.prototype.SetScreenEnabled = function () {
        var isScreenEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DirectionId) && !Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId) && !Tools_1.AppTool.IsNullOrEmpty(this.ShipmentLevelCode)) {
            if (this.TransportModeId == "A") {
                isScreenEnabled = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentTypeId)) {
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
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MAWBOBLDate", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, isScreenEnabled);
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
    };
    NewMasterComponent.prototype.OnFiltersChanged = function () {
        this.SetScreenEnabled();
        this.SetUIProperties();
        this.SetUnits();
        this.SetLabels();
        this.SetPartners();
        this.SetPrepaidCollect();
    };
    Object.defineProperty(NewMasterComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) {
            if (this.EntityPM.DirectionId != newValue) {
                this.EntityPM.DirectionId = newValue;
                this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
                this.SetTransportModes();
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) {
            if (this.EntityPM.TransportModeId != newValue) {
                this.EntityPM.TransportModeId = newValue;
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
                this.MainCarriageFromPortId = null;
                this.MainCarriageToPortId = null;
                this.MainCarriageCarrierId = null;
                this.ShipmentTypeId = null;
                this.SetOrderDetails();
                this.OnFiltersChanged();
                this.BuildShipmentTypes();
                this.LoadAllowedAirline();
                this.ValidateMasterField();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "ShipmentTypeId", {
        get: function () { return this.EntityPM.ShipmentTypeId; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentTypeId != newValue) {
                this.EntityPM.ShipmentTypeId = newValue;
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.SetOrderDetails();
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "ShipmentLevelCode", {
        get: function () { return this.EntityPM.ShipmentLevelCode; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentLevelCode != newValue) {
                this.EntityPM.ShipmentLevelCode = newValue;
                this.SetUIProperties();
                this.SetScreenEnabled();
                this.SetOrderDetails();
                this.ValidateMasterField();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewMasterComponent.prototype.SetLabels = function () {
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
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
        else {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Master.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    };
    NewMasterComponent.prototype.SetUnits = function () {
        var myDimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        var myVolumeUnitCode = this.TenantPM.VolumeUnitCode;
        var myGrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        var myChargeableWeightUnitCode = Tools_1.AppTool.GetChargeableWeightUnitCode(this.TransportModeId, this.ShipmentTypeId);
        if (this.DirectionId == "D") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TenantPM.CountryCode)) {
                if (this.TenantPM.CountryCode.toUpperCase() == "US") {
                    myDimensionsUnitCode = "Inc";
                    myVolumeUnitCode = "CBI";
                    myGrossWeightUnitCode = "LB";
                    myChargeableWeightUnitCode = "LB";
                }
            }
        }
        if (this.EntityPM.IsCopyFromShipment || this.EntityPM.IsBuildFromQuote) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
                this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.GrossWeightUnitCode)) {
                this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeightUnitCode)) {
                this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
            }
            if (this.EntityPM.Ratio == null) {
                this.EntityPM.Ratio = Tools_1.AppTool.GetRatio(this.DirectionId, this.TransportModeId, this.ShipmentTypeId, this.TenantPM.CountryCode);
            }
            if (this.EntityPM.DimFactor == null) {
                this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
            }
        }
        else {
            this.EntityPM.VolumeUnitCode = myVolumeUnitCode;
            this.EntityPM.DimensionsUnitCode = myDimensionsUnitCode;
            this.EntityPM.GrossWeightUnitCode = myGrossWeightUnitCode;
            this.EntityPM.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
            this.EntityPM.Ratio = Tools_1.AppTool.GetRatio(this.DirectionId, this.TransportModeId, this.ShipmentTypeId, this.TenantPM.CountryCode);
            this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
        }
        this.ComputeOrderVolumetricWeight();
        this.ComputeChargeableWeight();
    };
    NewMasterComponent.prototype.SetPartners = function () {
    };
    NewMasterComponent.prototype.SetPrepaidCollect = function () {
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
    };
    NewMasterComponent.prototype.SetOrderDetails = function () {
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
    };
    NewMasterComponent.prototype.SetTransportModes = function () {
        if (this.DirectionId == "D") {
            if (this.TransportModeId == "I") {
                this.TransportModeId = null;
            }
            var itemIndex = this.TransportModesList.findIndex(function (f) { return f.Code == "I"; });
            if (itemIndex > -1) {
                this.TransportModesList.splice(itemIndex, 1);
            }
        }
        else {
            var itemIndex = this.TransportModesList.findIndex(function (f) { return f.Code == "I"; });
            if (itemIndex == -1) {
                this.TransportModesList.push(new FilterClass("I", "Inland"));
            }
        }
    };
    NewMasterComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Agent();
        this.SetUIProperties_Ports();
        this.SetUIProperties_MasterField();
        this.SetUIProperties_OrderDetails();
    };
    NewMasterComponent.prototype.SetUIProperties_Agent = function () {
        var isFieldRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentId)) {
            isFieldRequired = true;
        }
        this.UIProperties.SetRequired("AgentId", this.ObjectTableName, isFieldRequired);
    };
    NewMasterComponent.prototype.SetUIProperties_Ports = function () {
        var isFromRequired = false;
        var isToRequired = false;
        if (!this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                isFromRequired = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
                isToRequired = true;
            }
        }
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, isToRequired);
    };
    NewMasterComponent.prototype.SetUIProperties_MasterField = function () {
        var isFieldEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            isFieldEnabled = true;
        }
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isFieldEnabled);
    };
    NewMasterComponent.prototype.SetUIProperties_OrderDetails = function () {
        var isFieldsEnabled = this.EntityPM.ShipmentOrderPackages.length == 0 ? true : false;
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isFieldsEnabled);
    };
    Object.defineProperty(NewMasterComponent.prototype, "AgentId", {
        // Agent
        get: function () { return this.EntityPM.AgentId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.AgentId != newValue) {
                this.EntityPM.AgentId = newValue;
                this.SetUIProperties_Agent();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.EntityPM.AgentContactId = null;
                    this.EntityPM.AgentName = null;
                    this.EntityPM.AgentNote = null;
                    this.EntityPM.AgentReference1 = null;
                    this.EntityPM.AgentReference2 = null;
                    this.AgentAddressId = null;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.AgentContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.AgentName = myCardList.EnglishName;
                                _this.EntityPM.AgentNote = myCardList.Notes;
                                _this.AgentAddressId = myCardList.MainAddressId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AgentAddressId", {
        get: function () { return this.EntityPM.AgentAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.AgentAddressId != newValue) {
                this.EntityPM.AgentAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.AgentAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.AgentAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AgentAddressList", {
        get: function () { return this.myAgentAddressList; },
        set: function (newValue) {
            this.myAgentAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AgentContactId", {
        get: function () { return this.EntityPM.AgentContactId; },
        set: function (newValue) {
            if (this.EntityPM.AgentContactId != newValue) {
                this.EntityPM.AgentContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AgentReference1", {
        get: function () { return this.EntityPM.AgentReference1; },
        set: function (newValue) {
            if (this.EntityPM.AgentReference1 != newValue) {
                this.EntityPM.AgentReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AgentReference2", {
        get: function () { return this.EntityPM.AgentReference2; },
        set: function (newValue) {
            if (this.EntityPM.AgentReference2 != newValue) {
                this.EntityPM.AgentReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AgentName", {
        get: function () { return this.EntityPM.AgentName; },
        set: function (newValue) {
            if (this.EntityPM.AgentName != newValue) {
                this.EntityPM.AgentName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "MainCarriageFromPortId", {
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFromPortId != value) {
                this.EntityPM.MainCarriageFromPortId = value;
                this.EntityPM.FromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.FromPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "MainCarriageToPortId", {
        get: function () { return this.EntityPM.MainCarriageToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageToPortId != value) {
                this.EntityPM.MainCarriageToPortId = value;
                this.EntityPM.ToPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToPortList = null;
                }
                else {
                    this.myPortListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ToPortList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            var _this = this;
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
                    Tools_2.ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myCardListResponse) {
                        if (!myCardListResponse.HasError) {
                            var myCardList = myCardListResponse.Result;
                            if (myCardList) {
                                _this.EntityPM.MainCarriageCarrierCode = myCardList.Code;
                                _this.EntityPM.MainCarriageCarrierName = myCardList.EnglishName;
                                _this.EntityPM.MainCarriageCarrierWebSite = myCardList.WebSite;
                                if (_this.EntityPM.TransportModeId == "A") {
                                    _this.AccountNumber = myCardList.AirlineAccountNumber;
                                    if (myCardList.Code != null) {
                                        if (myCardList.Code.length <= 2) {
                                            _this.EntityPM.MainCarriageCarrierPrefix = myCardList.Code;
                                        }
                                    }
                                    // dont get from chach: if user choosed from tenant0 it wont get it
                                    _this.myAirlineListService.getSingle(newValue).subscribe(function (myAirlineListResponse) {
                                        var myAirlineList = myAirlineListResponse.Result;
                                        if (myAirlineList != null) {
                                            _this.EntityPM.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                            _this.EntityPM.CarrierIsLimitedLength = myAirlineList.LimitedLength;
                                            _this.EntityPM.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                            _this.EntityPM.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;
                                            var myPrefix = null;
                                            if (!Tools_1.AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                myPrefix = myAirlineList.Prefix.toString().trim();
                                                myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                                            }
                                            _this.AirlinePrefix = myPrefix;
                                            _this.myPartnersDomainService.GetAirlineByCode(myAirlineList.Code, 0).subscribe(function (myResponse) {
                                                if (!myResponse.HasError) {
                                                    Tools_2.ShipmentTool.MapTenantZeroAirline(_this.EntityPM, myResponse.Result);
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
                this.EntityPM.MainCarriageCarrierNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AirlinePrefix", {
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (newValue) {
            if (this.EntityPM.AirlinePrefix != newValue) {
                this.EntityPM.AirlinePrefix = newValue;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (newValue) {
            if (this.EntityPM.Master != newValue) {
                this.EntityPM.Master = newValue;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        set: function (newValue) {
            if (this.EntityPM.LongMaster != newValue) {
                this.EntityPM.LongMaster = newValue;
                this.ValidateMasterField();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewMasterComponent.prototype.ValidateMasterField = function () {
        this.IsMasterFieldValid = true;
        this.IsValidatingMasterField = false;
        this.MasterFieldValidityMessage = null;
        if (this.ShipmentLevelCode != "H") {
            if (this.TransportModeId == "A") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
                        this.IsValidatingMasterField = true;
                    }
                }
            }
        }
        if (this.IsValidatingMasterField) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
            }
            else {
                var myResult = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);
                if (!Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    this.IsMasterFieldValid = false;
                    this.MasterFieldValidityMessage = myResult;
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
                }
                else {
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
                        this.ValidateMasterFieldIsUsed();
                    }
                }
            }
        }
    };
    NewMasterComponent.prototype.ValidateMasterFieldIsUsed = function () {
        var _this = this;
        if (this.myShipmentDomainService == null) {
            this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            this.myShipmentDomainService.ValidateShipmentMasterFieldExistance(this.EntityPM.Id, this.EntityPM.BookingId, this.EntityPM.Master, this.EntityPM.AirlinePrefix, this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode, this.EntityPM.IsCancelled)
                .subscribe(function (myResult) {
                if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    _this.UIProperties.SetValidity("Master", _this.ObjectTableName, true, "");
                }
                else {
                    _this.IsMasterFieldValid = false;
                    _this.MasterFieldValidityMessage = myResult;
                    _this.UIProperties.SetValidity("Master", _this.ObjectTableName, false, myResult);
                }
            });
        }
    };
    Object.defineProperty(NewMasterComponent.prototype, "MAWBOBLDate", {
        get: function () { return this.EntityPM.MAWBOBLDate; },
        set: function (newValue) {
            if (this.EntityPM.MAWBOBLDate != newValue) {
                this.EntityPM.MAWBOBLDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) {
            if (this.EntityPM.AccountNumber != newValue) {
                this.EntityPM.AccountNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "MainCarriageVesselId", {
        get: function () { return this.EntityPM.MainCarriageVesselId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageVesselId != newValue) {
                this.EntityPM.MainCarriageVesselId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.EntityPM.FreightPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.FreightPrepaidCollectId != newValue) {
                this.EntityPM.FreightPrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.EntityPM.OtherPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.OtherPrepaidCollectId != newValue) {
                this.EntityPM.OtherPrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "OrderGrossWeight", {
        // Expected Order Details
        get: function () { return this.EntityPM.OrderGrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderGrossWeight != newValue) {
                this.EntityPM.OrderGrossWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeChargeableWeight();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "BookingVolume", {
        get: function () { return this.EntityPM.BookingVolume; },
        set: function (newValue) {
            if (this.EntityPM.BookingVolume != newValue) {
                this.EntityPM.BookingVolume = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeOrderVolumetricWeight();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "OrderVolumetricWeight", {
        get: function () { return this.EntityPM.OrderVolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderVolumetricWeight != newValue) {
                this.EntityPM.OrderVolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeChargeableWeight();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "OrderChargeableWeight", {
        get: function () { return this.EntityPM.OrderChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderChargeableWeight != newValue) {
                var myResult = Tools_1.AppTool.Round(newValue, 3);
                this.EntityPM.OrderChargeableWeight = myResult;
                if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                    this.EntityPM.OrderVolumetricWeight = myResult;
                    this.EntityPM.OrderGrossWeight = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, myResult);
                    this.EntityPM.BookingVolume = Tools_1.AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.OrderVolumetricWeight, this.EntityPM.Ratio);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "BookingNumberOfPackages", {
        get: function () { return this.EntityPM.BookingNumberOfPackages; },
        set: function (newValue) {
            if (this.EntityPM.BookingNumberOfPackages != newValue) {
                this.EntityPM.BookingNumberOfPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "OrderIsDangerouseGoods", {
        get: function () { return this.EntityPM.OrderIsDangerouseGoods; },
        set: function (newValue) {
            if (this.EntityPM.OrderIsDangerouseGoods != newValue) {
                this.EntityPM.OrderIsDangerouseGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewMasterComponent.prototype.ComputeOrderVolumetricWeight = function () {
        var myResult = null;
        if (this.BookingVolume != null) {
            myResult = Tools_1.AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.BookingVolume, this.EntityPM.Ratio);
        }
        else if (this.OrderGrossWeight != null) {
            myResult = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.OrderGrossWeight);
        }
        this.OrderVolumetricWeight = myResult;
    };
    NewMasterComponent.prototype.ComputeChargeableWeight = function () {
        this.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.OrderGrossWeight, this.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    };
    Object.defineProperty(NewMasterComponent.prototype, "Quantity1", {
        get: function () { return this.EntityPM.Quantity1; },
        set: function (newValue) {
            if (this.EntityPM.Quantity1 != newValue) {
                this.EntityPM.Quantity1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "Quantity2", {
        get: function () { return this.EntityPM.Quantity2; },
        set: function (newValue) {
            if (this.EntityPM.Quantity2 != newValue) {
                this.EntityPM.Quantity2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "Quantity3", {
        get: function () { return this.EntityPM.Quantity3; },
        set: function (newValue) {
            if (this.EntityPM.Quantity3 != newValue) {
                this.EntityPM.Quantity3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "Quantity4", {
        get: function () { return this.EntityPM.Quantity4; },
        set: function (newValue) {
            if (this.EntityPM.Quantity4 != newValue) {
                this.EntityPM.Quantity4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "Quantity5", {
        get: function () { return this.EntityPM.Quantity5; },
        set: function (newValue) {
            if (this.EntityPM.Quantity5 != newValue) {
                this.EntityPM.Quantity5 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "PackageTypeId1", {
        get: function () { return this.EntityPM.PackageTypeId1; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId1 != newValue) {
                this.EntityPM.PackageTypeId1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "PackageTypeId2", {
        get: function () { return this.EntityPM.PackageTypeId2; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId2 != newValue) {
                this.EntityPM.PackageTypeId2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "PackageTypeId3", {
        get: function () { return this.EntityPM.PackageTypeId3; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId3 != newValue) {
                this.EntityPM.PackageTypeId3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "PackageTypeId4", {
        get: function () { return this.EntityPM.PackageTypeId4; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId4 != newValue) {
                this.EntityPM.PackageTypeId4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "PackageTypeId5", {
        get: function () { return this.EntityPM.PackageTypeId5; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId5 != newValue) {
                this.EntityPM.PackageTypeId5 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewMasterComponent.prototype.BuildAdditionalFields = function () {
        this.RunComponent();
    };
    NewMasterComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewMasterComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            var screenCode = "NewMaster";
            cmpRef.instance.LabelWidth = 110;
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, screenCode);
        });
    };
    NewMasterComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewMasterComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsScreenEnabled);
        }
    };
    NewMasterComponent.prototype.CopyEntityData = function () {
        if (this.IsBuildFromQuote || this.IsCopyFromShipment) {
            this.EntityPM.IsBuildFromQuote = this.IsBuildFromQuote;
            this.EntityPM.IsCopyFromShipment = this.IsCopyFromShipment;
            this.DirectionId = this.SourceEntityPM.DirectionId;
            this.TransportModeId = this.SourceEntityPM.TransportModeId;
            this.ShipmentTypeId = this.SourceEntityPM.ShipmentTypeId;
            Tools_2.ShipmentTool.CopyShipment(this.EntityPM, this.SourceEntityPM);
            this.OnFiltersChanged();
            this.CopyRoutings();
            this.CopyPartners();
            if (this.IsCopyFromShipment) {
                this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Copy");
                this.EntityPM.OriginShipmentId = this.SourceEntityPM.Id;
                this.EntityPM.IncotermId = this.SourceEntityPM.IncotermId;
                this.EntityPM.FreightPrepaidCollectId = this.SourceEntityPM.FreightPrepaidCollectId;
                this.EntityPM.OtherPrepaidCollectId = this.SourceEntityPM.OtherPrepaidCollectId;
                this.EntityPM.BaseShipmentNumber = this.SourceEntityPM.ShipmentNumber;
                this.IsCopyDescription = Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.DescriptionOfGoods) ? false : true;
                this.IsCopyDescriptionEnabled = Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.DescriptionOfGoods) ? false : true;
                this.IsCopyPackagesEnabled = this.SourceEntityPM.ShipmentPackages.length > 0 ? true : false;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.CustomAgentImportId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.CustomAgentExportId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.Notify1Id)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.Notify2Id)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperNotExporterId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeNotImporterId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.FreightForwarderId)) {
                    this.IsCopyOtherPartnersVisible = true;
                }
            }
            else if (this.IsBuildFromQuote) {
                this.EntityPM.QuoteId = this.SourceEntityPM.QuoteId;
                this.EntityPM.QuoteNumber = this.SourceEntityPM.QuoteNumber;
            }
        }
    };
    NewMasterComponent.prototype.CopyRoutings = function () {
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
    };
    NewMasterComponent.prototype.CopyPartners = function () {
        if (this.IsCopyFromShipment) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.AgentId)) {
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
    };
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyAgent", {
        get: function () { return this.isCopyAgent; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyShipper", {
        get: function () { return this.isCopyShipper; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyConsignee", {
        get: function () { return this.isCopyConsignee; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyCustomAgentImport", {
        get: function () { return this.isCopyCustomAgentImport; },
        set: function (value) {
            if (this.isCopyCustomAgentImport != value) {
                this.isCopyCustomAgentImport = value;
                this.EntityPM.CustomAgentImportId = !value ? null : this.SourceEntityPM.CustomAgentImportId;
                this.EntityPM.CustomAgentImportName = !value ? null : this.SourceEntityPM.CustomAgentImportName;
                this.EntityPM.CustomAgentImportNote = !value ? null : this.SourceEntityPM.CustomAgentImportNote;
                this.EntityPM.CustomAgentImportAddressId = !value ? null : this.SourceEntityPM.CustomAgentImportAddressId;
                this.EntityPM.CustomAgentImportContactId = !value ? null : this.SourceEntityPM.CustomAgentImportContactId;
                //this.EntityPM.CustomAgentImportReference = !value ? null : this.SourceEntityPM.CustomAgentImportReference;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyCustomAgentExport", {
        get: function () { return this.isCopyCustomAgentExport; },
        set: function (value) {
            if (this.isCopyCustomAgentExport != value) {
                this.isCopyCustomAgentExport = value;
                this.EntityPM.CustomAgentExportId = !value ? null : this.SourceEntityPM.CustomAgentExportId;
                this.EntityPM.CustomAgentExportName = !value ? null : this.SourceEntityPM.CustomAgentExportName;
                this.EntityPM.CustomAgentExportNote = !value ? null : this.SourceEntityPM.CustomAgentExportNote;
                this.EntityPM.CustomAgentExportAddressId = !value ? null : this.SourceEntityPM.CustomAgentExportAddressId;
                this.EntityPM.CustomAgentExportContactId = !value ? null : this.SourceEntityPM.CustomAgentExportContactId;
                //this.EntityPM.CustomAgentExportReference = !value ? null : this.SourceEntityPM.CustomAgentExportReference;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyNotify1", {
        get: function () { return this.isCopyNotify1; },
        set: function (value) {
            if (this.isCopyNotify1 != value) {
                this.isCopyNotify1 = value;
                this.EntityPM.Notify1Id = !value ? null : this.SourceEntityPM.Notify1Id;
                this.EntityPM.Notify1Name = !value ? null : this.SourceEntityPM.Notify1Name;
                this.EntityPM.Notify1Note = !value ? null : this.SourceEntityPM.Notify1Note;
                this.EntityPM.Notify1AddressId = !value ? null : this.SourceEntityPM.Notify1AddressId;
                this.EntityPM.Notify1ContactId = !value ? null : this.SourceEntityPM.Notify1ContactId;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyNotify2", {
        get: function () { return this.isCopyNotify2; },
        set: function (value) {
            if (this.isCopyNotify2 != value) {
                this.isCopyNotify2 = value;
                this.EntityPM.Notify2Id = !value ? null : this.SourceEntityPM.Notify2Id;
                this.EntityPM.Notify2Name = !value ? null : this.SourceEntityPM.Notify2Name;
                this.EntityPM.Notify2Note = !value ? null : this.SourceEntityPM.Notify2Note;
                this.EntityPM.Notify2AddressId = !value ? null : this.SourceEntityPM.Notify2AddressId;
                this.EntityPM.Notify2ContactId = !value ? null : this.SourceEntityPM.Notify2ContactId;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyShipperNotExporter", {
        get: function () { return this.isCopyShipperNotExporter; },
        set: function (value) {
            if (this.isCopyShipperNotExporter != value) {
                this.isCopyShipperNotExporter = value;
                this.EntityPM.ShipperNotExporterId = !value ? null : this.SourceEntityPM.ShipperNotExporterId;
                this.EntityPM.ShipperNotExporterName = !value ? null : this.SourceEntityPM.ShipperNotExporterName;
                this.EntityPM.ShipperNotExporterNote = !value ? null : this.SourceEntityPM.ShipperNotExporterNote;
                this.EntityPM.ShipperNotExporterAddressId = !value ? null : this.SourceEntityPM.ShipperNotExporterAddressId;
                this.EntityPM.ShipperNotExporterContactId = !value ? null : this.SourceEntityPM.ShipperNotExporterContactId;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyConsigneeNotImporter", {
        get: function () { return this.isCopyConsigneeNotImporter; },
        set: function (value) {
            if (this.isCopyConsigneeNotImporter != value) {
                this.isCopyConsigneeNotImporter = value;
                this.EntityPM.ConsigneeNotImporterId = !value ? null : this.SourceEntityPM.ConsigneeNotImporterId;
                this.EntityPM.ConsigneeNotImporterName = !value ? null : this.SourceEntityPM.ConsigneeNotImporterName;
                this.EntityPM.ConsigneeNotImporterNote = !value ? null : this.SourceEntityPM.ConsigneeNotImporterNote;
                this.EntityPM.ConsigneeNotImporterAddressId = !value ? null : this.SourceEntityPM.ConsigneeNotImporterAddressId;
                this.EntityPM.ConsigneeNotImporterContactId = !value ? null : this.SourceEntityPM.ConsigneeNotImporterContactId;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyFreightForwarder", {
        get: function () { return this.isCopyFreightForwarder; },
        set: function (value) {
            if (this.isCopyFreightForwarder != value) {
                this.isCopyFreightForwarder = value;
                this.EntityPM.FreightForwarderId = !value ? null : this.SourceEntityPM.FreightForwarderId;
                this.EntityPM.FreightForwarderName = !value ? null : this.SourceEntityPM.FreightForwarderName;
                this.EntityPM.FreightForwarderNote = !value ? null : this.SourceEntityPM.FreightForwarderNote;
                this.EntityPM.FreightForwarderAddressId = !value ? null : this.SourceEntityPM.FreightForwarderAddressId;
                this.EntityPM.FreightForwarderContactId = !value ? null : this.SourceEntityPM.FreightForwarderContactId;
                //this.EntityPM.FreightForwarderReference = !value ? null : this.SourceEntityPM.FreightForwarderReference;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyConsolidator", {
        get: function () { return this.isCopyConsolidator; },
        set: function (value) {
            if (this.isCopyConsolidator != value) {
                this.isCopyConsolidator = value;
                this.EntityPM.ConsolidatorId = !value ? null : this.SourceEntityPM.ConsolidatorId;
                this.EntityPM.ConsolidatorName = !value ? null : this.SourceEntityPM.ConsolidatorName;
                this.EntityPM.ConsolidatorNote = !value ? null : this.SourceEntityPM.ConsolidatorNote;
                this.EntityPM.ConsolidatorAddressId = !value ? null : this.SourceEntityPM.ConsolidatorAddressId;
                this.EntityPM.ConsolidatorContactId = !value ? null : this.SourceEntityPM.ConsolidatorContactId;
                //this.EntityPM.ConsolidatorReference = !value ? null : this.SourceEntityPM.ConsolidatorReference;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyFlights", {
        get: function () { return this.isCopyFlights; },
        set: function (value) {
            if (this.isCopyFlights != value) {
                this.isCopyFlights = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyDescription", {
        get: function () { return this.isCopyDescription; },
        set: function (value) {
            if (this.isCopyDescription != value) {
                this.isCopyDescription = value;
                this.DescriptionOfGoods = !value ? null : this.SourceEntityPM.DescriptionOfGoods;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMasterComponent.prototype, "IsCopyPackages", {
        get: function () { return this.isCopyPackages; },
        set: function (value) {
            if (this.isCopyPackages != value) {
                this.isCopyPackages = value;
                if (value) {
                    Tools_2.ShipmentTool.CopyShipmentPackages(this.EntityPM, this.SourceEntityPM, false);
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
                }
                this.SetUIProperties_OrderDetails();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewMasterComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewMasterComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.SetDataOnFinish();
        var validator = new ShipmentValidator_1.ShipmentValidator();
        this.ValidationErrorsList = validator.Validate(this.EntityPM);
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Master.F.AgentId")));
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
    };
    NewMasterComponent.prototype.SetDataOnFinish = function () {
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;
        if (this.IsCopyFlights) {
            Tools_2.ShipmentTool.CopyFlights(this.EntityPM, this.SourceEntityPM);
        }
        this.SetPartnersOnFinish();
        this.SetCountryECOnFinish();
        this.SetOrderPackagesOnFinish();
        this.SetPickupDeliveryOnFinish();
        this.SetInlandDomesticOnFinish();
    };
    NewMasterComponent.prototype.SetPartnersOnFinish = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentId)) {
            if (this.IsCopyFromShipment == false) {
                if (this.DirectionId == "E") {
                    this.EntityPM.ConsigneeId = this.AgentId;
                    this.EntityPM.ConsigneeName = this.AgentName;
                    this.EntityPM.ConsigneeAddressId = this.AgentAddressId;
                    this.EntityPM.ConsigneeContactId = this.AgentContactId;
                    this.EntityPM.ConsigneeReference1 = this.AgentReference1;
                    this.EntityPM.ConsigneeReference2 = this.AgentReference2;
                    this.EntityPM.ShipperId = SessionLocator_1.SessionLocator.TenantPM.AgentId;
                    this.EntityPM.ShipperAddressId = SessionLocator_1.SessionLocator.TenantPM.AddressId;
                }
                else {
                    this.EntityPM.ShipperId = this.AgentId;
                    this.EntityPM.ShipperName = this.AgentName;
                    this.EntityPM.ShipperAddressId = this.AgentAddressId;
                    this.EntityPM.ShipperContactId = this.AgentContactId;
                    this.EntityPM.ShipperReference1 = this.AgentReference1;
                    this.EntityPM.ShipperReference2 = this.AgentReference2;
                    this.EntityPM.ConsigneeId = SessionLocator_1.SessionLocator.TenantPM.AgentId;
                    this.EntityPM.ConsigneeAddressId = SessionLocator_1.SessionLocator.TenantPM.AddressId;
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
                        this.EntityPM.ShipperId = SessionLocator_1.SessionLocator.TenantPM.AgentId;
                        this.EntityPM.ShipperAddressId = SessionLocator_1.SessionLocator.TenantPM.AddressId;
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
                        this.EntityPM.ConsigneeId = SessionLocator_1.SessionLocator.TenantPM.AgentId;
                        this.EntityPM.ConsigneeAddressId = SessionLocator_1.SessionLocator.TenantPM.AddressId;
                    }
                }
            }
        }
    };
    NewMasterComponent.prototype.SetCountryECOnFinish = function () {
        if (this.FromPortList != null) {
            this.EntityPM.FromCountryId = this.FromPortList.CountryId;
            this.EntityPM.FromCountryIsEC = this.FromPortList.CountryEC;
        }
        if (this.ToPortList != null) {
            this.EntityPM.ToCountryId = this.ToPortList.CountryId;
            this.EntityPM.ToCountryIsEC = this.ToPortList.CountryEC;
        }
    };
    NewMasterComponent.prototype.SetOrderPackagesOnFinish = function () {
        if (this.IsFCLEntity) {
            var numberOfPackages = 0;
            this.EntityPM.ShipmentOrderPackages = [];
            if (this.PackageTypeList1 != null && this.Quantity1 > 0) {
                var item = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList1.IsContainer;
                item.Quantity = this.Quantity1;
                item.PackageTypeId = this.PackageTypeId1;
                item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity1;
            }
            if (this.PackageTypeList2 != null && this.Quantity2 > 0) {
                var item = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList2.IsContainer;
                item.Quantity = this.Quantity2;
                item.PackageTypeId = this.PackageTypeId2;
                item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity2;
            }
            if (this.PackageTypeList3 != null && this.Quantity3 > 0) {
                var item = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList3.IsContainer;
                item.Quantity = this.Quantity3;
                item.PackageTypeId = this.PackageTypeId3;
                item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity3;
            }
            if (this.PackageTypeList4 != null && this.Quantity4 > 0) {
                var item = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList4.IsContainer;
                item.Quantity = this.Quantity4;
                item.PackageTypeId = this.PackageTypeId4;
                item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity4;
            }
            if (this.PackageTypeList5 != null && this.Quantity5 > 0) {
                var item = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(this.EntityPM);
                item.IsContainer = this.PackageTypeList5.IsContainer;
                item.Quantity = this.Quantity5;
                item.PackageTypeId = this.PackageTypeId5;
                item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                this.EntityPM.AddOrderPackage(item);
                numberOfPackages = numberOfPackages + this.Quantity5;
            }
            if (numberOfPackages > 0) {
                this.EntityPM.BookingNumberOfPackages = numberOfPackages;
            }
        }
    };
    NewMasterComponent.prototype.SetPickupDeliveryOnFinish = function () {
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
    };
    NewMasterComponent.prototype.SetInlandDomesticOnFinish = function () {
        if (this.IsInlandDomestic) {
            this.MainCarriageFromPortId = null;
            this.MainCarriageToPortId = null;
            this.EntityPM.MainCarriageFinalDestinationPortId = null;
        }
    };
    NewMasterComponent.prototype.ValidateMasterStack = function () {
        var _this = this;
        var isLoadingStack = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            if (this.Master.length == 8 && !this.EntityPM.MainCarriageIsFromStack && !this.EntityPM.MAWBTakenFromStack) {
                if (Tools_1.FormatTool.IsNumeric(this.Master)) {
                    isLoadingStack = true;
                    if (this.StackDomainService == null) {
                        this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
                    }
                    this.StackDomainService.GetMAWBStackPMByNumber(+this.Master).subscribe(function (myResponse) {
                        var isCreating = false;
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                            _this.CurrentSession.StopBusyIndicator();
                        }
                        else {
                            var myStackPM = myResponse.Result;
                            if (myStackPM == null) {
                                isCreating = true;
                            }
                            else {
                                var myAirlineId = _this.MainCarriageCarrierId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InterlineId)) {
                                    myAirlineId = _this.EntityPM.InterlineId;
                                }
                                if (myStackPM.AirlineId == myAirlineId) {
                                    if (!Tools_1.AppTool.IsNullOrEmpty(myStackPM.AssignedToId) && myStackPM.AssignedToId != _this.EntityPM.ShipperId) {
                                        _this.CurrentSession.StopBusyIndicator();
                                        var messageWindow = new MessageWindow_1.MessageWindow();
                                        messageWindow.Width = 450;
                                        messageWindow.Height = 190;
                                        messageWindow.Title = "Invalid Master";
                                        messageWindow.Show("AWB is assigned to another shipper");
                                        _this.Master = null;
                                    }
                                    else {
                                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                        confirmWindow.Title = "AWB exists in the stock";
                                        confirmWindow.Show("Do you want to get this awb from stock?");
                                        confirmWindow.WindowClosed.subscribe(function (event) {
                                            if (confirmWindow.Yes) {
                                                _this.EntityPM.MAWBTakenFromStack = true;
                                                _this.EntityPM.MAWBStackNumber = Tools_1.AppTool.PadLeft(myStackPM.Number.toString(), 8, '0');
                                                _this.MAWBOBLDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                                                _this.SetMAWBAirline();
                                                _this.isGetFromStock = true;
                                                _this.SubmitCreatingShipment();
                                            }
                                            else {
                                                _this.Master = null;
                                                _this.CurrentSession.StopBusyIndicator();
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
                            _this.SubmitCreatingShipment();
                        }
                    });
                }
            }
        }
        if (!isLoadingStack) {
            this.SubmitCreatingShipment();
        }
    };
    NewMasterComponent.prototype.SetMAWBAirline = function () {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    };
    NewMasterComponent.prototype.SubmitCreatingShipment = function () {
        var _this = this;
        this.myShipmentPMService.insert(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.EntityPM = myResponse.Result;
                if (_this.EntityPM) {
                    var transMode = (_this.EntityPM.TransportModeId == "A" ? "Air" : (_this.EntityPM.TransportModeId == "O" ? "Ocean" : (_this.EntityPM.TransportModeId == "I" ? "Inland" : "")));
                    var activity = "New " + transMode + (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.ShipmentType) ? " " + _this.EntityPM.ShipmentType : "") + " " + _this.ObjectTableName;
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(_this.ObjectTableName, activity);
                }
                _this.CurrentSession.CloseCurrentWindowEmit('OK');
                if (_this.IsBuildFromQuote || _this.IsCopyFromShipment) {
                    var myBackButtonLabel = null;
                    var myBackSessionTextCode = null;
                    if (_this.IsBuildFromQuote) {
                        myBackButtonLabel = "Quote: " + _this.SourceEntityPM.QuoteNumber;
                        myBackSessionTextCode = "General.MH.Quotes";
                    }
                    else {
                        myBackButtonLabel = "Shipment: " + _this.SourceEntityPM.ShipmentNumber;
                        myBackSessionTextCode = "General.MH.Operations";
                    }
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: _this.EntityPM.Id, ObjectTableName: 'Shipment', BackButtonLabel: myBackButtonLabel });
                        _this.CurrentSession.ChangeSessionHeader({ MenuTextCode: "General.MH.Operations" });
                        cmpRef.instance.BackCompleted.subscribe(function ($event) {
                            _this.CurrentSession.ChangeSessionHeader({ MenuTextCode: myBackSessionTextCode });
                            if (_this.IsBuildFromQuote) {
                                _this.CurrentSession.FireEvent("LoadConnectedShipments");
                            }
                        });
                    });
                }
            }
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewMasterComponent.prototype, "viewContainerRef", void 0);
    NewMasterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewMasterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewMasterComponent);
    return NewMasterComponent;
}(BaseComponent_1.BaseComponent));
exports.NewMasterComponent = NewMasterComponent;
var FilterClass = /** @class */ (function () {
    function FilterClass(code, name, src) {
        if (src === void 0) { src = null; }
        this.HasHelp = false;
        this.HelpText = null;
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
    return FilterClass;
}());
//# sourceMappingURL=NewMasterComponent.js.map