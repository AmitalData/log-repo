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
var ShipmentPickUpPM_1 = require("../../EntityPMs/ShipmentPickUpPM");
var ShipmentDeliveryPM_1 = require("../../EntityPMs/ShipmentDeliveryPM");
var ShipmentOrderPackagePM_1 = require("../../EntityPMs/ShipmentOrderPackagePM");
var AddressPM_1 = require("../../../Common/EntityPMs/AddressPM");
var PortListService_1 = require("../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../Common/Services/StandardLists/AirlineListService");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var IncotermListService_1 = require("../../../Common/Services/StandardLists/IncotermListService");
var ShipmentPMService_1 = require("../../Services/StandardPMs/ShipmentPMService");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var Args_1 = require("../../Args");
var Args_2 = require("../../../Infrastructure/Args");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ShipmentDomainService_1 = require("../../Services/ShipmentDomainService");
var AWBStackDomainService_1 = require("../../../Common/Services/AWBStackDomainService");
var Args_3 = require("../../../Common/Args");
var ShippingLinePMService_1 = require("../../../Common/Services/StandardPMs/ShippingLinePMService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var NewShipmentComponent = /** @class */ (function (_super) {
    __extends(NewShipmentComponent, _super);
    function NewShipmentComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Shipment";
        _this.LabelColumnWidth = 115;
        _this.ControlColumnWidth = 220;
        _this.CardDependencyProperty1 = "CS";
        _this.CardDependencyProperty1IsList = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ScreenIsReady = false;
        _this.IsShipmentLevelFixed = false;
        _this.IsBuildFromQuote = false;
        _this.IsCopyFromShipment = false;
        _this.IsCreatedFromMasterHouses = false;
        _this.IsCreatedFromCustomerOverview = false;
        _this.ShowShipmentLevels = true;
        _this.DirectionsList = [];
        _this.TransportModesList = [];
        _this.ShipmentTypesList = [];
        _this.ShipmentLevelsList = [];
        _this.ScreenOpacity = 0.7;
        _this.IsScreenEnabled = false;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.IsInlandDomestic = false;
        _this.isDirectionListEnabled = false;
        _this.isTransportModesListEnabled = false;
        _this.isShipmentTypesListEnabled = false;
        _this.isShipmentLevelsListEnabled = false;
        _this.ShipmentTypeName = null;
        _this.isShipperMyCustomer = false;
        _this.isConsigneeMyCustomer = false;
        //Customer
        _this.CustomerDependencyProperty1 = "CS";
        _this.CustomerDependencyProperty1IsList = false;
        _this.IsCustomerRequired = false;
        _this.isFirstTimeFromQuotePickup = true;
        _this.isFirstTimeFromQuoteDelivery = true;
        // Main Carriage
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
        _this.CopyCheckBoxTop = 5;
        _this.IsCopyOtherPartnersVisible = false;
        _this.IsCopyShipperEnabled = false;
        _this.IsCopyConsigneeEnabled = false;
        _this.isCopyShipper = false;
        _this.isCopyConsignee = false;
        _this.isCopyAgent = false;
        _this.isCopyCustomAgentImport = false;
        _this.isCopyCustomAgentExport = false;
        _this.isCopyNotify1 = false;
        _this.isCopyNotify2 = false;
        _this.isCopyShipperNotExporter = false;
        _this.isCopyConsigneeNotImporter = false;
        _this.isCopyFreightForwarder = false;
        _this.isCopyConsolidator = false;
        _this.isCopyPreCarriage = false;
        _this.isCopyOnCarriage = false;
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
        _this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Create");
        if (_this.TenantPM.AllowAgentInCustomersLOV) {
            _this.CardDependencyProperty1 = "CS,AG";
            _this.CardDependencyProperty1IsList = true;
        }
        return _this;
    }
    NewShipmentComponent.prototype.ngOnInit = function () {
        var _this = this;
        var listservice = new EntityListService_1.EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then(function (res) {
            res.subscribe(function (resp) {
                _this.BuildFiltersLists();
                if (_this.IsCopyFromShipment == false && _this.IsBuildFromQuote == false) {
                    _this.OnFiltersChanged();
                }
                _this.BuildAdditionalFields();
                _this.LoadAllowedAirline();
                _this.ScreenIsReady = true;
            });
        });
    };
    NewShipmentComponent.prototype.InitializeServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myAirlineListService = new AirlineListService_1.AirlineListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.myIncotermListService = new IncotermListService_1.IncotermListService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        this.myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        this.myShippingLineService = new ShippingLinePMService_1.ShippingLinePMService();
    };
    NewShipmentComponent.prototype.LoadAllowedAirline = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    if (this.EntityPM.TransportModeId == "A") {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            if (!this.IsBuildFromQuote && !this.IsCopyFromShipment) {
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
        }
    };
    NewShipmentComponent.prototype.SetWindowArgs = function (args) {
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
    };
    NewShipmentComponent.prototype.BuildFiltersLists = function () {
        this.DirectionsList = [];
        this.TransportModesList = [];
        var direct_E = new FilterClass("E", "Export");
        var direct_I = new FilterClass("I", "Import");
        var direct_D = new FilterClass("D", "Domestic");
        var direct_R = new FilterClass("R", "Drop");
        var transport_A = new FilterClass("A", "Air");
        var transport_O = new FilterClass("O", "Ocean");
        var transport_I = new FilterClass("I", "Inland");
        var isAirExportOnly = false;
        if (this.IsCreatedFromCustomerOverview) {
            var myCodes = [];
            myCodes.push("EAWB");
            myCodes.push("BUBK");
            if (FeatureLocator_1.FeatureLocator.IsPackageOneOf(myCodes)) {
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
    };
    NewShipmentComponent.prototype.BuildShipmentTypes = function () {
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
    };
    NewShipmentComponent.prototype.BuildShipmentLevels = function () {
        this.ShipmentLevelsList = [];
        if (this.ShowShipmentLevels) {
            this.ShipmentLevelsList.push(new FilterClass("D", "Direct"));
            this.ShipmentLevelsList.push(new FilterClass("H", "House"));
        }
    };
    NewShipmentComponent.prototype.SetScreenEnabled = function () {
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
    };
    NewShipmentComponent.prototype.OnFiltersChanged = function () {
        if (!this.IsCreatedFromMasterHouses) {
            this.IsDirectionListEnabled = true;
            this.IsTransportModesListEnabled = Tools_1.AppTool.IsNullOrEmpty(this.DirectionId) ? false : true;
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
    };
    Object.defineProperty(NewShipmentComponent.prototype, "IsDirectionListEnabled", {
        get: function () { return this.isDirectionListEnabled; },
        set: function (value) {
            if (this.isDirectionListEnabled != value) {
                this.isDirectionListEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsTransportModesListEnabled", {
        get: function () { return this.isTransportModesListEnabled; },
        set: function (value) {
            if (this.isTransportModesListEnabled != value) {
                this.isTransportModesListEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsShipmentTypesListEnabled", {
        get: function () { return this.isShipmentTypesListEnabled; },
        set: function (value) {
            if (this.isShipmentTypesListEnabled != value) {
                this.isShipmentTypesListEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsShipmentLevelsListEnabled", {
        get: function () { return this.isShipmentLevelsListEnabled; },
        set: function (value) {
            if (this.isShipmentLevelsListEnabled != value) {
                this.isShipmentLevelsListEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) {
            if (this.EntityPM.DirectionId != newValue) {
                this.EntityPM.DirectionId = newValue;
                this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
                this.OnFiltersChanged();
                this.SetTransportModes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) {
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
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsInlandDomestic = this.TransportModeId == "I" && this.DirectionId == "D" ? true : false;
                this.DelOrderDetails();
                this.OnFiltersChanged();
                this.BuildShipmentTypes();
                this.LoadAllowedAirline();
                this.ValidateMasterField();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipmentTypeId", {
        get: function () { return this.EntityPM.ShipmentTypeId; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentTypeId != newValue) {
                this.EntityPM.ShipmentTypeId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipmentTypeName = null;
                }
                else {
                    var item = this.ShipmentTypesList.filter(function (f) { return f.Code == newValue; })[0];
                    if (item) {
                        this.ShipmentTypeName = item.Name;
                    }
                }
                this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                this.DelOrderDetails();
                this.OnFiltersChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipmentLevelCode", {
        get: function () { return this.EntityPM.ShipmentLevelCode; },
        set: function (newValue) {
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
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.SetLabels = function () {
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
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
        else {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        }
    };
    NewShipmentComponent.prototype.SetUnits = function () {
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
            this.ComputeOrderVolumetricWeight();
            this.ComputeChargeableWeight();
        }
    };
    NewShipmentComponent.prototype.SetPartners = function () {
        if (!this.IsBuildFromQuote && !this.IsCopyFromShipment) {
            this.IsShipperMyCustomer = false;
            this.IsConsigneeMyCustomer = false;
            var myCRMCustomerId = null;
            switch (this.DirectionId) {
                case "I": {
                    this.ShipmentCustomerTypeCode = "CON";
                    this.IsConsigneeMyCustomer = true;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myCRMCustomerId)) {
                        this.ConsigneeId = myCRMCustomerId;
                        if (this.ShipperId == myCRMCustomerId) {
                            this.ShipperId = null;
                        }
                    }
                    break;
                }
                default: {
                    this.ShipmentCustomerTypeCode = "SHI";
                    this.IsShipperMyCustomer = true;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myCRMCustomerId)) {
                        this.ShipperId = myCRMCustomerId;
                        if (this.ConsigneeId == myCRMCustomerId) {
                            this.ConsigneeId = null;
                        }
                    }
                    break;
                }
            }
        }
        if (this.IsCopyFromShipment) {
            this.CopyPartners();
        }
    };
    NewShipmentComponent.prototype.SetPrepaidCollect = function () {
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
                myOtherPrepaidCollectId = "P";
                break;
            }
        }
        if (this.IsCopyFromShipment || this.IsBuildFromQuote) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.FreightPrepaidCollectId)) {
                this.FreightPrepaidCollectId = myFreightPrepaidCollectId;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.OtherPrepaidCollectId)) {
                this.OtherPrepaidCollectId = myOtherPrepaidCollectId;
            }
        }
        else {
            this.FreightPrepaidCollectId = myFreightPrepaidCollectId;
            this.OtherPrepaidCollectId = myOtherPrepaidCollectId;
        }
    };
    NewShipmentComponent.prototype.DelOrderDetails = function () {
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
    NewShipmentComponent.prototype.SetTransportModes = function () {
        if (this.ShipmentLevelCode == "H") {
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
        }
    };
    NewShipmentComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Filters();
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Customer();
        this.SetUIProperties_Ports();
        this.SetUIProperties_MasterField();
        this.SetUIProperties_HouseField();
        this.SetUIProperties_VesselField();
        this.SetUIProperties_OrderDetails();
    };
    NewShipmentComponent.prototype.SetUIProperties_Filters = function () {
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
    };
    NewShipmentComponent.prototype.SetUIProperties_Shipper = function () {
        var isFieldRequired = false;
        if (this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                isFieldRequired = true;
            }
        }
        //if (AppTool.IsNullOrEmpty(this.ShipperId)) {
        //    if (this.IsShipperMyCustomer) {
        //        isFieldRequired = true;
        //    }
        //    else if (this.DirectionId == "E" || this.DirectionId == "D" || this.DirectionId == "R" || this.DirectionId == null) {
        //        isFieldRequired = true;
        //    }
        //}
        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, isFieldRequired);
    };
    NewShipmentComponent.prototype.SetUIProperties_Consignee = function () {
        var isFieldRequired = false;
        if (this.IsInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isFieldRequired = true;
            }
        }
        //if (AppTool.IsNullOrEmpty(this.ConsigneeId)) {
        //    if (this.IsConsigneeMyCustomer) {
        //        isFieldRequired = true;
        //    }
        //    else if (this.EntityPM.DirectionId == "I") {
        //        isFieldRequired = true;
        //    }
        //    else if (this.IsInlandDomestic) {
        //        isFieldRequired = true;
        //    }
        //}
        this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, isFieldRequired);
    };
    NewShipmentComponent.prototype.SetUIProperties_Customer = function () {
        //var isFieldEnabled: boolean = true;
        //if (this.ShipmentCustomerTypeCode == "SHI") {
        //    if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
        //        isFieldEnabled = false;
        //    }
        //}
        //else if (this.ShipmentCustomerTypeCode == "CON") {
        //    if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
        //        isFieldEnabled = false;
        //    }
        //}
        //this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, isFieldEnabled);
    };
    NewShipmentComponent.prototype.SetUIProperties_Ports = function () {
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
    NewShipmentComponent.prototype.SetUIProperties_MasterField = function () {
        var isFieldVisible = false;
        var isFieldEnabled = false;
        if (this.ShipmentLevelCode != "H") {
            if (!this.IsInlandDomestic) {
                isFieldVisible = true;
            }
        }
        if (this.IsScreenEnabled) {
            if (this.TransportModeId == "A") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                    isFieldEnabled = true;
                }
            }
            else {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetVisibility("Master", this.ObjectTableName, isFieldVisible);
    };
    NewShipmentComponent.prototype.SetUIProperties_HouseField = function () {
        var isFieldVisible = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId)) {
            if (this.DirectionId == "E" || this.DirectionId == "D" || this.DirectionId == "R") {
                var settingsCode = "HAWBCounter" + this.TransportModeId + "_E_D";
                var settings = SessionLocator_1.SessionLocator.TenantSettings.filter(function (f) { return f.SettingCode == settingsCode; })[0];
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
    };
    NewShipmentComponent.prototype.SetUIProperties_VesselField = function () {
        var isFieldVisible = false;
        var isFieldEnabled = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentLevelCode)) {
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
    };
    NewShipmentComponent.prototype.SetUIProperties_OrderDetails = function () {
        var isFieldsEnabled = false;
        if (this.IsScreenEnabled) {
            isFieldsEnabled = this.EntityPM.ShipmentOrderPackages.length == 0 ? true : false;
        }
        this.UIProperties.SetEnabled("OrderGrossWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingVolume", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("OrderChargeableWeight", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("BookingNumberOfPackages", this.ObjectTableName, isFieldsEnabled);
    };
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                if (this.ShipmentCustomerTypeCode == "SHI") {
                    this.CustomerId = newValue;
                }
                this.SetUIProperties_Shipper();
                this.SetUIProperties_Customer();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
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
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                                _this.ShipperContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.ShipperName = myCardList.EnglishName;
                                _this.EntityPM.ShipperNote = myCardList.Notes;
                                _this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                                _this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                                _this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                                _this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                                _this.ShipperAddressId = myCardList.MainAddressId;
                                _this.ShipperIsCustomer = myCardList.IsCustomer;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperAddressId", {
        get: function () { return this.EntityPM.ShipperAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ShipperAddressId != newValue) {
                this.EntityPM.ShipperAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipperAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ShipperAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperAddressList", {
        get: function () { return this.myShipperAddressList; },
        set: function (newValue) {
            this.myShipperAddressList = newValue;
            this.UpdatePickUpAddressFields();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperContactId", {
        get: function () { return this.EntityPM.ShipperContactId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperContactId != newValue) {
                this.EntityPM.ShipperContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperReference1", {
        get: function () { return this.EntityPM.ShipperReference1; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference1 != newValue) {
                this.EntityPM.ShipperReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperReference2", {
        get: function () { return this.EntityPM.ShipperReference2; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference2 != newValue) {
                this.EntityPM.ShipperReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ShipperName", {
        get: function () { return this.EntityPM.ShipperName; },
        set: function (newValue) {
            if (this.EntityPM.ShipperName != newValue) {
                this.EntityPM.ShipperName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeId", {
        get: function () { return this.EntityPM.ConsigneeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                if (this.ShipmentCustomerTypeCode == "CON") {
                    this.CustomerId = newValue;
                }
                this.SetUIProperties_Consignee();
                this.SetUIProperties_Customer();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
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
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                                _this.ConsigneeContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.ConsigneeName = myCardList.EnglishName;
                                _this.EntityPM.ConsigneeNote = myCardList.Notes;
                                _this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                                _this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                                _this.ConsigneeAddressId = myCardList.MainAddressId;
                                _this.ConsigneeIsCustomer = myCardList.IsCustomer;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeAddressId", {
        get: function () { return this.EntityPM.ConsigneeAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ConsigneeAddressId != newValue) {
                this.EntityPM.ConsigneeAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ConsigneeAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ConsigneeAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeAddressList", {
        get: function () { return this.myConsigneeAddressList; },
        set: function (newValue) {
            this.myConsigneeAddressList = newValue;
            this.UpdateDeliveryAddressFields();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeContactId", {
        get: function () { return this.EntityPM.ConsigneeContactId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeContactId != newValue) {
                this.EntityPM.ConsigneeContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeReference1", {
        get: function () { return this.EntityPM.ConsigneeReference1; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference1 != newValue) {
                this.EntityPM.ConsigneeReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeReference2", {
        get: function () { return this.EntityPM.ConsigneeReference2; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference2 != newValue) {
                this.EntityPM.ConsigneeReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ConsigneeName", {
        get: function () { return this.EntityPM.ConsigneeName; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeName != newValue) {
                this.EntityPM.ConsigneeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsShipperMyCustomer", {
        get: function () { return this.isShipperMyCustomer; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsConsigneeMyCustomer", {
        get: function () { return this.isConsigneeMyCustomer; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.SetSalesman = function () {
        this.EntityPM.SalesmanUserId = Tools_1.AppTool.IsNullOrEmpty(this.myCustomerSalesmanId) ? this.EntityPM.CreatedByUserId : this.myCustomerSalesmanId;
        this.EntityPM.AccountManagerUserId = this.myCustomerAccountManagerUserId != null ? this.myCustomerAccountManagerUserId : SessionLocator_1.SessionLocator.LoggedUserId;
    };
    NewShipmentComponent.prototype.SetCustomer = function (myCode) {
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
    };
    NewShipmentComponent.prototype.ComputeCustomerDependency = function () {
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
                        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                            this.CustomerDependencyProperty1 = "CS,AG";
                            this.CustomerDependencyProperty1IsList = true;
                        }
                    }
                    break;
                }
            case "AGT":
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
            case "CSD":
                {
                    this.CustomerDependencyProperty1 = "AG,CS";
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
    };
    Object.defineProperty(NewShipmentComponent.prototype, "ShipmentCustomerTypeCode", {
        get: function () { return this.EntityPM.ShipmentCustomerTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentCustomerTypeCode != newValue) {
                this.EntityPM.ShipmentCustomerTypeCode = newValue;
                this.CustomerId = null;
                this.SetCustomer(newValue);
                this.SetCustomerRequired();
                this.ComputeCustomerDependency();
                this.SetUIProperties_Customer();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.CustomerId != newValue) {
                this.EntityPM.CustomerId = newValue;
                this.SetCustomerRequired();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.CustomerContactId = null;
                    this.EntityPM.CustomerName = null;
                    this.EntityPM.CustomerNote = null;
                    this.CustomerAddressId = null;
                    this.myCustomerSalesmanId = null;
                    this.myCustomerAccountManagerUserId = null;
                    this.SetSalesman();
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.CustomerContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.CustomerName = myCardList.EnglishName;
                                _this.EntityPM.CustomerNote = myCardList.Notes;
                                _this.CustomerAddressId = myCardList.MainAddressId;
                                _this.myCustomerSalesmanId = myCardList.SalesmanUserId;
                                _this.myCustomerAccountManagerUserId = myCardList.AccountManagerUserId;
                                _this.SetSalesman();
                                _this.SetCustomerPartner();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "CustomerAddressId", {
        get: function () { return this.EntityPM.CustomerAddressId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerAddressId != newValue) {
                this.EntityPM.CustomerAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "CustomerContactId", {
        get: function () { return this.EntityPM.CustomerContactId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerContactId != newValue) {
                this.EntityPM.CustomerContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.SetCustomerPartner = function () {
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
    };
    NewShipmentComponent.prototype.SetCustomerRequired = function () {
        var isRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerId) || Tools_1.AppTool.IsNullOrEmpty(this.ShipmentCustomerTypeCode)) {
            isRequired = true;
        }
        this.IsCustomerRequired = isRequired;
    };
    // Add|Edit Partner
    NewShipmentComponent.prototype.AddPartnerClicked = function (myPartnerCode) {
        var _this = this;
        var myComponentPath = null;
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
                var windowArgs = new Args_1.AddEditPartnerArgs();
                windowArgs.EntityPM = this.EntityPM;
                windowArgs.IsNewEntity = true;
                windowArgs.PartnerTypeCode = "AGT";
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = myTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBAddEditPartnerComponent');
                logWindow.ComponentLoaded.subscribe(function (cmp) {
                    logWindow.WindowClosed.subscribe(function ($event) {
                        if (cmp.IsUpdatingPartner) {
                            var myPartnerId = cmp.CurrentPartnerId;
                            var myAddressId = cmp.CurrentAddressId;
                            if (_this.EntityPM.IssuingCarrierAgentId != myPartnerId) {
                                _this.EntityPM.IssuingCarrierAgentId = myPartnerId;
                            }
                            else {
                                _this.EntityPM.IssuingCarrierAddressId = myAddressId;
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
        var args = new Args_2.NewEntityArgs();
        if (!isCustomer) {
            args.Perspective = "ShippersAndConsignees";
        }
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        logeWindow.Width = 960;
        logeWindow.Height = 600;
        logeWindow.Title = title;
        logeWindow.WindowArgs = args;
        logeWindow.Show(myComponentPath);
        logeWindow.ComponentLoaded.subscribe(function (comp) {
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (myPartnerCode == "S") {
                        _this.ShipperId = comp.EntityPM.Id;
                        _this.ShipperName = comp.EntityPM.EnglishName;
                        _this.myCustomerSalesmanId = comp.EntityPM.SalesmanUserId;
                        _this.myCustomerAccountManagerUserId = comp.EntityPM.AccountManagerUserId;
                    }
                    else if (myPartnerCode == "C") {
                        _this.ConsigneeId = comp.EntityPM.Id;
                        _this.ConsigneeName = comp.EntityPM.EnglishName;
                        _this.myCustomerSalesmanId = comp.EntityPM.SalesmanUserId;
                        _this.myCustomerAccountManagerUserId = comp.EntityPM.AccountManagerUserId;
                    }
                    else if (myPartnerCode == "T") {
                        _this.CustomerId = comp.EntityPM.Id;
                        _this.myCustomerSalesmanId = comp.EntityPM.SalesmanUserId;
                        _this.myCustomerAccountManagerUserId = comp.EntityPM.AccountManagerUserId;
                        if (_this.ShipmentCustomerTypeCode == "SHI") {
                            _this.ShipperId = comp.EntityPM.Id;
                            _this.ShipperName = comp.EntityPM.EnglishName;
                        }
                        else if (_this.ShipmentCustomerTypeCode == "CON") {
                            _this.ConsigneeId = comp.EntityPM.Id;
                            _this.ConsigneeName = comp.EntityPM.EnglishName;
                        }
                    }
                }
            });
        });
    };
    NewShipmentComponent.prototype.ComputeAddCustomerTitle = function () {
        var myResult = "";
        switch (this.ShipmentCustomerTypeCode) {
            case "AGT":
                {
                    myResult = "Agent";
                    break;
                }
            case "CAE":
                {
                    myResult = "Custom's Agent Export";
                    break;
                }
            case "CAI":
                {
                    myResult = "Custom's Agent Import";
                    break;
                }
            case "CCP":
                {
                    myResult = "Custom Clearance Point";
                    break;
                }
            case "CNI":
                {
                    myResult = "Consignee Not Importer";
                    break;
                }
            case "COL":
                {
                    myResult = "Coloader";
                    break;
                }
            case "CON":
                {
                    myResult = "Consignee";
                    break;
                }
            case "CSD":
                {
                    myResult = "Consolidator";
                    break;
                }
            case "FOR":
                {
                    myResult = "Freight Forwarder";
                    break;
                }
            case "IGT":
                {
                    myResult = "Issuing Carrier Agent";
                    break;
                }
            case "NT1":
                {
                    myResult = "Notify 1";
                    break;
                }
            case "NT2":
                {
                    myResult = "Notify 2";
                    break;
                }
            case "REA":
                {
                    myResult = "Releasing Agent";
                    break;
                }
            case "SHI":
                {
                    myResult = "Shipper";
                    break;
                }
            case "SNE":
                {
                    myResult = "Shipper Not Exporter";
                    break;
                }
        }
        return myResult;
    };
    Object.defineProperty(NewShipmentComponent.prototype, "IncludePickUp", {
        // Pickup
        get: function () { return this.EntityPM.IncludePickUp; },
        set: function (newValue) {
            if (this.EntityPM.IncludePickUp != newValue) {
                this.EntityPM.IncludePickUp = newValue;
                this.UpdatePickUpAddressFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "PickUpAddressId", {
        get: function () { return this.EntityPM.PickUpAddressId; },
        set: function (newValue) {
            if (this.EntityPM.PickUpAddressId != newValue) {
                this.EntityPM.PickUpAddressId = newValue;
                this.LoadPickupAddress();
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.FromAddressCity = null;
                    this.FromAddressZipCode = null;
                    this.FromAddressCountryId = null;
                }
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.LoadPickupAddress = function () {
        var _this = this;
        if (this.PickUpAddressId == null) {
            this.PickupAddressList = null;
        }
        else {
            this.myAddressListService.getSingle(this.PickUpAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.PickupAddressList = myResponse.Result;
                }
            });
        }
    };
    Object.defineProperty(NewShipmentComponent.prototype, "FromAddressCity", {
        get: function () { return this.EntityPM.FromAddressCity; },
        set: function (newValue) {
            if (this.EntityPM.FromAddressCity != newValue) {
                this.EntityPM.FromAddressCity = newValue;
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "FromAddressZipCode", {
        get: function () { return this.EntityPM.FromAddressZipCode; },
        set: function (newValue) {
            if (this.EntityPM.FromAddressZipCode != newValue) {
                this.EntityPM.FromAddressZipCode = newValue;
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "FromAddressCountryId", {
        get: function () { return this.EntityPM.FromAddressCountryId; },
        set: function (newValue) {
            if (this.EntityPM.FromAddressCountryId != newValue) {
                this.EntityPM.FromAddressCountryId = newValue;
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.UpdatePickUpAddressFields = function () {
        if (!this.IncludePickUp) {
            this.EntityPM.PickUpAddressId = null;
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.PickupAddressList = null;
        }
        else if (this.EntityPM.IsBuildFromQuote && this.isFirstTimeFromQuotePickup) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
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
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperPickAddressId)) {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperPickAddressId;
            }
            else {
                this.EntityPM.PickUpAddressId = this.EntityPM.ShipperMainAddressId;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                this.EntityPM.FromAddressCity = null;
                this.EntityPM.FromAddressZipCode = null;
                this.EntityPM.FromAddressCountryId = null;
            }
            this.LoadPickupAddress();
        }
        this.SetUIProperties_PickupFields();
    };
    NewShipmentComponent.prototype.SetUIProperties_PickupFields = function () {
        var isCityRequired = false;
        var isCountryRequired = false;
        if (this.IncludePickUp) {
            var validateFields = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                validateFields = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                validateFields = true;
            }
            if (validateFields) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.FromAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.FromAddressZipCode)) {
                    isCityRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.FromAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, isCountryRequired);
    };
    Object.defineProperty(NewShipmentComponent.prototype, "IncludeDelivery", {
        // Delivery
        get: function () { return this.EntityPM.IncludeDelivery; },
        set: function (newValue) {
            if (this.EntityPM.IncludeDelivery != newValue) {
                this.EntityPM.IncludeDelivery = newValue;
                this.UpdateDeliveryAddressFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "DeliveryAddressId", {
        get: function () { return this.EntityPM.DeliveryAddressId; },
        set: function (newValue) {
            if (this.EntityPM.DeliveryAddressId != newValue) {
                this.EntityPM.DeliveryAddressId = newValue;
                this.LoadDeliveryAddress();
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ToAddressCity = null;
                    this.ToAddressZipCode = null;
                    this.ToAddressCountryId = null;
                }
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.LoadDeliveryAddress = function () {
        var _this = this;
        if (this.DeliveryAddressId == null) {
            this.DeliveryAddressList = null;
        }
        else {
            this.myAddressListService.getSingle(this.DeliveryAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.DeliveryAddressList = myResponse.Result;
                }
            });
        }
    };
    Object.defineProperty(NewShipmentComponent.prototype, "ToAddressCity", {
        get: function () { return this.EntityPM.ToAddressCity; },
        set: function (newValue) {
            if (this.EntityPM.ToAddressCity != newValue) {
                this.EntityPM.ToAddressCity = newValue;
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ToAddressZipCode", {
        get: function () { return this.EntityPM.ToAddressZipCode; },
        set: function (newValue) {
            if (this.EntityPM.ToAddressZipCode != newValue) {
                this.EntityPM.ToAddressZipCode = newValue;
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "ToAddressCountryId", {
        get: function () { return this.EntityPM.ToAddressCountryId; },
        set: function (newValue) {
            if (this.EntityPM.ToAddressCountryId != newValue) {
                this.EntityPM.ToAddressCountryId = newValue;
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.UpdateDeliveryAddressFields = function () {
        if (!this.IncludeDelivery) {
            this.EntityPM.DeliveryAddressId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.DeliveryAddressList = null;
        }
        else if (this.EntityPM.IsBuildFromQuote && this.isFirstTimeFromQuoteDelivery) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
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
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePickAddressId)) {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneePickAddressId;
            }
            else {
                this.EntityPM.DeliveryAddressId = this.EntityPM.ConsigneeMainAddressId;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                this.EntityPM.ToAddressCity = null;
                this.EntityPM.ToAddressZipCode = null;
                this.EntityPM.ToAddressCountryId = null;
            }
            this.LoadDeliveryAddress();
        }
        this.SetUIProperties_DeliveryFields();
    };
    NewShipmentComponent.prototype.SetUIProperties_DeliveryFields = function () {
        var isCityRequired = false;
        var isCountryRequired = false;
        if (this.IncludeDelivery) {
            var validateFields = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                validateFields = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId) && Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                validateFields = true;
            }
            if (validateFields) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ToAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.ToAddressZipCode)) {
                    isCityRequired = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.ToAddressCountryId)) {
                    isCountryRequired = true;
                }
            }
        }
        this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, isCountryRequired);
    };
    // Select City
    NewShipmentComponent.prototype.SelectCityCommand = function (myAddressCode) {
        var _this = this;
        var mySourceCountryId = myAddressCode == "P" ? this.FromAddressCountryId : this.ToAddressCountryId;
        var args = new Args_3.CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                if (myAddressCode == "P") {
                    _this.FromAddressCity = args.CityName;
                    _this.FromAddressCountryId = args.CountryId;
                }
                else {
                    _this.ToAddressCity = args.CityName;
                    _this.ToAddressCountryId = args.CountryId;
                }
            }
        });
    };
    // Add|Edit Address
    NewShipmentComponent.prototype.EditAddressClicked = function (myAddressCode) {
        var _this = this;
        var myAddressId = null;
        var myPartnerTypeId = null;
        var isCustomer;
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
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "S": {
                            _this.ShipperAddressId = null;
                            _this.ShipperAddressId = myAddressId;
                            break;
                        }
                        case "C": {
                            _this.ConsigneeAddressId = null;
                            _this.ConsigneeAddressId = myAddressId;
                            break;
                        }
                        case "P": {
                            if (_this.PickUpAddressId == _this.ShipperAddressId) {
                                _this.ShipperAddressId = null;
                                _this.ShipperAddressId = myAddressId;
                            }
                            else {
                                _this.PickUpAddressId = null;
                                _this.PickUpAddressId = myAddressId;
                            }
                            break;
                        }
                        case "D": {
                            if (_this.DeliveryAddressId == _this.ConsigneeAddressId) {
                                _this.ConsigneeAddressId = null;
                                _this.ConsigneeAddressId = myAddressId;
                            }
                            else {
                                _this.DeliveryAddressId = null;
                                _this.DeliveryAddressId = myAddressId;
                            }
                            break;
                        }
                    }
                }
            });
        }
    };
    NewShipmentComponent.prototype.AddAddressClicked = function (myAddressCode) {
        var _this = this;
        var entityPM = null;
        var myPartnerTypeId = null;
        var isCustomer;
        switch (myAddressCode) {
            case "S": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ShipperId;
                myPartnerTypeId = this.ShipperPartnerTypeId;
                isCustomer = this.ShipperIsCustomer;
                break;
            }
            case "C": {
                entityPM = new AddressPM_1.AddressPM();
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.AddressTypeId = "O";
                entityPM.CardId = this.ConsigneeId;
                myPartnerTypeId = this.ConsigneePartnerTypeId;
                isCustomer = this.ConsigneeIsCustomer;
                break;
            }
            case "P": {
                if (this.IncludePickUp) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                        entityPM = new AddressPM_1.AddressPM();
                        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                        entityPM = new AddressPM_1.AddressPM();
                        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "S": {
                            _this.ShipperAddressId = null;
                            _this.ShipperAddressId = entityPM.Id;
                            break;
                        }
                        case "C": {
                            _this.ConsigneeAddressId = null;
                            _this.ConsigneeAddressId = entityPM.Id;
                            break;
                        }
                        case "P": {
                            _this.PickUpAddressId = null;
                            _this.PickUpAddressId = entityPM.Id;
                            break;
                        }
                        case "D": {
                            _this.DeliveryAddressId = null;
                            _this.DeliveryAddressId = entityPM.Id;
                            break;
                        }
                    }
                }
            });
        }
    };
    Object.defineProperty(NewShipmentComponent.prototype, "MainCarriageFromPortId", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "MainCarriageToPortId", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            var _this = this;
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
                                _this.EntityPM.ReleasingAgentId = newValue;
                                _this.EntityPM.ReleasingAgentContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.ReleasingAgentName = myCardList.EnglishName;
                                _this.EntityPM.ReleasingAgentNote = myCardList.Notes;
                                _this.EntityPM.ReleasingAgentAddressId = myCardList.MainAddressId;
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
                                else if (_this.EntityPM.TransportModeId == "O") {
                                    _this.myShippingLineService.get(_this.MainCarriageCarrierId).subscribe(function (myResponse) {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                var line = myResponse.Result;
                                                var agentId = line.ShippingAgentId;
                                                if (agentId != null) {
                                                    _this.myCardListService.getSingle(agentId).subscribe(function (myResponse) {
                                                        if (myResponse != null) {
                                                            if (!myResponse.HasError) {
                                                                var agent = myResponse.Result;
                                                                _this.EntityPM.ReleasingAgentId = agent.Id;
                                                                _this.EntityPM.ReleasingAgentContactId = agent.PrimaryContactId;
                                                                _this.EntityPM.ReleasingAgentName = agent.EnglishName;
                                                                _this.EntityPM.ReleasingAgentNote = agent.Notes;
                                                                _this.EntityPM.ReleasingAgentAddressId = agent.MainAddressId;
                                                            }
                                                        }
                                                    });
                                                }
                                                else {
                                                    _this.EntityPM.ReleasingAgentId = line.Id;
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
                this.EntityPM.MainCarriageCarrierNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "AirlinePrefix", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "Master", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "LongMaster", {
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
    NewShipmentComponent.prototype.ValidateMasterField = function () {
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
    NewShipmentComponent.prototype.ValidateMasterFieldIsUsed = function () {
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
    Object.defineProperty(NewShipmentComponent.prototype, "MAWBOBLDate", {
        get: function () { return this.EntityPM.MAWBOBLDate; },
        set: function (newValue) {
            if (this.EntityPM.MAWBOBLDate != newValue) {
                this.EntityPM.MAWBOBLDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) {
            if (this.EntityPM.AccountNumber != newValue) {
                this.EntityPM.AccountNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "MainCarriageVesselId", {
        get: function () { return this.EntityPM.MainCarriageVesselId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageVesselId != newValue) {
                this.EntityPM.MainCarriageVesselId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IncotermId", {
        // General
        get: function () { return this.EntityPM.IncotermId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.IncotermId != newValue) {
                this.EntityPM.IncotermId = newValue;
                if (newValue != null) {
                    this.myIncotermListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myList = myResponse.Result;
                            if (myList) {
                                _this.FreightPrepaidCollectId = myList.Freight;
                                _this.OtherPrepaidCollectId = myList.OtherCharges;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "MoveTypeId", {
        get: function () { return this.EntityPM.MoveTypeId; },
        set: function (newValue) {
            if (this.EntityPM.MoveTypeId != newValue) {
                this.EntityPM.MoveTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "SalesmanUserId", {
        get: function () { return this.EntityPM.SalesmanUserId; },
        set: function (newValue) {
            if (this.EntityPM.SalesmanUserId != newValue) {
                this.EntityPM.SalesmanUserId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.EntityPM.FreightPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.FreightPrepaidCollectId != newValue) {
                this.EntityPM.FreightPrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.EntityPM.OtherPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.OtherPrepaidCollectId != newValue) {
                this.EntityPM.OtherPrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "House", {
        get: function () { return this.EntityPM.House; },
        set: function (newValue) {
            if (this.EntityPM.House != newValue) {
                this.EntityPM.House = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Expected Order Details
    NewShipmentComponent.prototype.FillDimensionsClicked = function () {
        var _this = this;
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        logeWindow.Width = 850;
        logeWindow.Title = "Fill Dimensions";
        logeWindow.WindowArgs = this.EntityPM;
        logeWindow.Show("./Shipment/Components/NewEntity/WizardDimensionsComponent");
        logeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.SetUIProperties_OrderDetails();
            }
        });
    };
    Object.defineProperty(NewShipmentComponent.prototype, "OrderGrossWeight", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "BookingVolume", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "OrderVolumetricWeight", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "OrderChargeableWeight", {
        get: function () { return this.EntityPM.OrderChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderChargeableWeight != newValue) {
                var myResult = Tools_1.AppTool.Round(newValue, 3);
                this.EntityPM.OrderChargeableWeight = myResult;
                if (this.OrderGrossWeight == null && this.OrderVolumetricWeight == null) {
                    this.EntityPM.OrderVolumetricWeight = myResult;
                    this.EntityPM.OrderGrossWeight = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, myResult);
                    this.EntityPM.BookingVolume = Tools_1.AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, myResult, this.EntityPM.Ratio);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "BookingNumberOfPackages", {
        get: function () { return this.EntityPM.BookingNumberOfPackages; },
        set: function (newValue) {
            if (this.EntityPM.BookingNumberOfPackages != newValue) {
                this.EntityPM.BookingNumberOfPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "OrderIsDangerouseGoods", {
        get: function () { return this.EntityPM.OrderIsDangerouseGoods; },
        set: function (newValue) {
            if (this.EntityPM.OrderIsDangerouseGoods != newValue) {
                this.EntityPM.OrderIsDangerouseGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.ComputeOrderVolumetricWeight = function () {
        var myResult = null;
        if (this.BookingVolume != null) {
            myResult = Tools_1.AppTool.GetWeightFromVolume(this.EntityPM.VolumeUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.BookingVolume, this.EntityPM.Ratio);
        }
        else if (this.OrderGrossWeight != null) {
            myResult = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.OrderGrossWeight);
        }
        this.OrderVolumetricWeight = myResult;
    };
    NewShipmentComponent.prototype.ComputeChargeableWeight = function () {
        this.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.OrderGrossWeight, this.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    };
    Object.defineProperty(NewShipmentComponent.prototype, "Quantity1", {
        get: function () { return this.EntityPM.Quantity1; },
        set: function (newValue) {
            if (this.EntityPM.Quantity1 != newValue) {
                this.EntityPM.Quantity1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "Quantity2", {
        get: function () { return this.EntityPM.Quantity2; },
        set: function (newValue) {
            if (this.EntityPM.Quantity2 != newValue) {
                this.EntityPM.Quantity2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "Quantity3", {
        get: function () { return this.EntityPM.Quantity3; },
        set: function (newValue) {
            if (this.EntityPM.Quantity3 != newValue) {
                this.EntityPM.Quantity3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "Quantity4", {
        get: function () { return this.EntityPM.Quantity4; },
        set: function (newValue) {
            if (this.EntityPM.Quantity4 != newValue) {
                this.EntityPM.Quantity4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "Quantity5", {
        get: function () { return this.EntityPM.Quantity5; },
        set: function (newValue) {
            if (this.EntityPM.Quantity5 != newValue) {
                this.EntityPM.Quantity5 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "PackageTypeId1", {
        get: function () { return this.EntityPM.PackageTypeId1; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId1 != newValue) {
                this.EntityPM.PackageTypeId1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "PackageTypeId2", {
        get: function () { return this.EntityPM.PackageTypeId2; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId2 != newValue) {
                this.EntityPM.PackageTypeId2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "PackageTypeId3", {
        get: function () { return this.EntityPM.PackageTypeId3; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId3 != newValue) {
                this.EntityPM.PackageTypeId3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "PackageTypeId4", {
        get: function () { return this.EntityPM.PackageTypeId4; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId4 != newValue) {
                this.EntityPM.PackageTypeId4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "PackageTypeId5", {
        get: function () { return this.EntityPM.PackageTypeId5; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeId5 != newValue) {
                this.EntityPM.PackageTypeId5 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewShipmentComponent.prototype.BuildAdditionalFields = function () {
        this.RunComponent();
    };
    NewShipmentComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewShipmentComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            var screenCode = "NewShipment";
            cmpRef.instance.LabelWidth = 110;
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, screenCode);
        });
    };
    NewShipmentComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewShipmentComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsScreenEnabled);
        }
    };
    NewShipmentComponent.prototype.CopyEntityData = function () {
        if (this.IsBuildFromQuote || this.IsCopyFromShipment) {
            this.EntityPM.IsBuildFromQuote = this.IsBuildFromQuote;
            this.EntityPM.IsCopyFromShipment = this.IsCopyFromShipment;
            this.DirectionId = this.SourceEntityPM.DirectionId;
            this.TransportModeId = this.SourceEntityPM.TransportModeId;
            this.ShipmentTypeId = this.SourceEntityPM.ShipmentTypeId;
            this.ShipmentCustomerTypeCode = this.SourceEntityPM.ShipmentCustomerTypeCode;
            this.CustomerId = this.SourceEntityPM.CustomerId;
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
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.AgentId)) {
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
    NewShipmentComponent.prototype.CopyRoutings = function () {
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
    NewShipmentComponent.prototype.CopyPartners = function () {
        if (this.IsCopyFromShipment) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.ShipperId)) {
                this.IsCopyShipper = true;
                this.IsCopyShipperEnabled = true;
                //if (this.SourceEntityPM.ShipperId == this.SourceEntityPM.CustomerId) {
                //    this.IsShipperMyCustomer = true;
                //    this.ShipmentCustomerTypeCode = "SHI";
                //}
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SourceEntityPM.ConsigneeId)) {
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
    };
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyShipper", {
        get: function () { return this.isCopyShipper; },
        set: function (value) {
            var _this = this;
            if (this.isCopyShipper != value) {
                this.isCopyShipper = value;
                this.EntityPM.ShipperId = !value ? null : this.SourceEntityPM.ShipperId;
                //this.EntityPM.ShipperReference1 = !value ? null : this.SourceEntityPM.ShipperReference1;
                //this.EntityPM.ShipperReference2 = !value ? null : this.SourceEntityPM.ShipperReference2;
                this.ShipperAddressId = !value ? null : this.SourceEntityPM.ShipperAddressId;
                this.ShipperContactId = !value ? null : this.SourceEntityPM.ShipperContactId;
                this.SetUIProperties_Shipper();
                if (this.EntityPM.ShipperId) {
                    this.myCardListService.getSingle(this.EntityPM.ShipperId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                                _this.EntityPM.ShipperName = myCardList.EnglishName;
                                _this.EntityPM.ShipperNote = myCardList.Notes;
                                _this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                                _this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                                _this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                                _this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                                if (Tools_1.AppTool.IsNullOrEmpty(_this.SourceEntityPM.ShipperContactId)) {
                                    _this.ShipperContactId = null;
                                    _this.ShipperContactId = myCardList.PrimaryContactId;
                                }
                                if (Tools_1.AppTool.IsNullOrEmpty(_this.SourceEntityPM.ShipperAddressId)) {
                                    _this.ShipperAddressId = null;
                                    _this.ShipperAddressId = myCardList.MainAddressId;
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyConsignee", {
        get: function () { return this.isCopyConsignee; },
        set: function (value) {
            var _this = this;
            if (this.isCopyConsignee != value) {
                this.isCopyConsignee = value;
                this.EntityPM.ConsigneeId = !value ? null : this.SourceEntityPM.ConsigneeId;
                //this.EntityPM.ConsigneeReference1 = !value ? null : this.SourceEntityPM.ConsigneeReference1;
                //this.EntityPM.ConsigneeReference2 = !value ? null : this.SourceEntityPM.ConsigneeReference2;
                this.ConsigneeAddressId = !value ? null : this.SourceEntityPM.ConsigneeAddressId;
                this.ConsigneeContactId = !value ? null : this.SourceEntityPM.ConsigneeContactId;
                this.SetUIProperties_Consignee();
                if (this.EntityPM.ConsigneeId) {
                    this.myCardListService.getSingle(this.EntityPM.ConsigneeId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                                _this.EntityPM.ConsigneeName = myCardList.EnglishName;
                                _this.EntityPM.ConsigneeNote = myCardList.Notes;
                                _this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                                _this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                                if (Tools_1.AppTool.IsNullOrEmpty(_this.SourceEntityPM.ConsigneeContactId)) {
                                    _this.ConsigneeContactId = null;
                                    _this.ConsigneeContactId = myCardList.PrimaryContactId;
                                }
                                if (Tools_1.AppTool.IsNullOrEmpty(_this.SourceEntityPM.ConsigneeAddressId)) {
                                    _this.ConsigneeAddressId = null;
                                    _this.ConsigneeAddressId = myCardList.MainAddressId;
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyAgent", {
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
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyCustomAgentImport", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyCustomAgentExport", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyNotify1", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyNotify2", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyShipperNotExporter", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyConsigneeNotImporter", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyFreightForwarder", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyConsolidator", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyPreCarriage", {
        get: function () { return this.isCopyPreCarriage; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyOnCarriage", {
        get: function () { return this.isCopyOnCarriage; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyFlights", {
        get: function () { return this.isCopyFlights; },
        set: function (value) {
            if (this.isCopyFlights != value) {
                this.isCopyFlights = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyDescription", {
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
    Object.defineProperty(NewShipmentComponent.prototype, "IsCopyPackages", {
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
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewShipmentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewShipmentComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;
        this.SetPartnersOnFinish();
        this.SetCountryECOnFinish();
        this.SetInlandDomesticOnFinish();
        var validator = new ShipmentValidator_1.ShipmentValidator();
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
    };
    NewShipmentComponent.prototype.SetDataOnFinish = function () {
        this.SetOrderPackagesOnFinish();
        this.SetPickupDeliveryOnFinish();
        if (this.IsCopyFlights) {
            Tools_2.ShipmentTool.CopyFlights(this.EntityPM, this.SourceEntityPM);
        }
    };
    NewShipmentComponent.prototype.SetPartnersOnFinish = function () {
        this.SetCustomerPartner();
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
            this.EntityPM.SalesmanUserId = Tools_1.AppTool.IsNullOrEmpty(this.myCustomerSalesmanId) ? this.EntityPM.CreatedByUserId : this.myCustomerSalesmanId;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AccountManagerUserId)) {
            this.EntityPM.AccountManagerUserId = Tools_1.AppTool.IsNullOrEmpty(this.myCustomerAccountManagerUserId) ? this.EntityPM.CreatedByUserId : this.myCustomerAccountManagerUserId;
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
    };
    NewShipmentComponent.prototype.SetCountryECOnFinish = function () {
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
    };
    NewShipmentComponent.prototype.SetInlandDomesticOnFinish = function () {
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
    };
    NewShipmentComponent.prototype.SetOrderPackagesOnFinish = function () {
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
    NewShipmentComponent.prototype.SetPickupDeliveryOnFinish = function () {
        this.EntityPM.ShipmentPickUps = [];
        this.EntityPM.ShipmentDeliveries = [];
        if (this.IncludePickUp) {
            var typeCode = "PART";
            if (Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                typeCode = "CASL";
            }
            var newPickUp = new ShipmentPickUpPM_1.ShipmentPickUpPM(this.EntityPM);
            newPickUp.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
            if (Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                typeCode = "CASL";
            }
            var newDelivery = new ShipmentDeliveryPM_1.ShipmentDeliveryPM(this.EntityPM);
            newDelivery.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
    };
    NewShipmentComponent.prototype.ValidateMasterStack = function () {
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
                                        _this.CurrentSession.StopBusyIndicator();
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
    NewShipmentComponent.prototype.SetMAWBAirline = function () {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    };
    NewShipmentComponent.prototype.SubmitCreatingShipment = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.SetDataOnFinish();
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
    ], NewShipmentComponent.prototype, "viewContainerRef", void 0);
    NewShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewShipmentComponent);
    return NewShipmentComponent;
}(BaseComponent_1.BaseComponent));
exports.NewShipmentComponent = NewShipmentComponent;
var FilterClass = /** @class */ (function () {
    function FilterClass(code, name, src) {
        if (src === void 0) { src = null; }
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
    return FilterClass;
}());
exports.FilterClass = FilterClass;
//# sourceMappingURL=NewShipmentComponent.js.map