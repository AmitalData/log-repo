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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ShipmentPM_1 = require("../../../Shipment/EntityPMs/ShipmentPM");
var ShipmentPickUpPM_1 = require("../../../Shipment/EntityPMs/ShipmentPickUpPM");
var ShipmentDeliveryPM_1 = require("../../../Shipment/EntityPMs/ShipmentDeliveryPM");
var Tools_1 = require("../../../Infrastructure/Tools");
var Tools_2 = require("../../../Shipment/Tools");
var AirlineListService_1 = require("../../../Common/Services/StandardLists/AirlineListService");
var SharedAgentManifestService_1 = require("../../../Shipment/Services/Others/SharedAgentManifestService");
var ShipmentPMService_1 = require("../../../Shipment/Services/StandardPMs/ShipmentPMService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AddressList_1 = require("../../../Common/EntityLists/AddressList");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var IncotermListService_1 = require("../../../Common/Services/StandardLists/IncotermListService");
var MoveTypeListService_1 = require("../../../Infrastructure/Services/StandardLists/MoveTypeListService");
var CurrencyListService_1 = require("../../../Common/Services/StandardLists/CurrencyListService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Args_1 = require("../../../Infrastructure/Args");
var SharedManifestTranslationPM_1 = require("../../../Common/EntityPMs/SharedManifestTranslationPM");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var PortListService_1 = require("../../../Common/Services/StandardLists/PortListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var CountryListService_1 = require("../../../Common/Services/StandardLists/CountryListService");
var PackageTypeListService_1 = require("../../../Common/Services/StandardLists/PackageTypeListService");
var EntityPMService_1 = require("../../../Infrastructure/Services/EntityPMService");
var PackageTypePM_1 = require("../../../Common/EntityPMs/PackageTypePM");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var AgentSharedManifestPMService_1 = require("../../../Common/Services/StandardPMs/AgentSharedManifestPMService");
var ShipmentValidator_1 = require("../../../Shipment/Validators/ShipmentValidator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var AddressService_1 = require("../../../Common/Services/ExtendedLists/AddressService");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var VesselListService_1 = require("../../../Common/Services/StandardLists/VesselListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var ShipmentPickUpDeliverySL_1 = require("../../../Common/DataContracts/ShipmentPickUpDeliverySL");
var Args_2 = require("../../../Common/Args");
var SharedManifestAdditionalComponent = /** @class */ (function (_super) {
    __extends(SharedManifestAdditionalComponent, _super);
    function SharedManifestAdditionalComponent(_sharedAgentManifestService, _agentSharedManifestPMService, entityPMService) {
        var _this = _super.call(this) || this;
        _this._sharedAgentManifestService = _sharedAgentManifestService;
        _this._agentSharedManifestPMService = _agentSharedManifestPMService;
        _this.entityPMService = entityPMService;
        _this.ObjectTableName = "Shipment";
        _this.DataContext = _this;
        _this.IsRemovePackageAreaFromScreen = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.AllPackageTypes = [];
        _this.IsNoNoOtherPartnersFound = false;
        _this.HideGeneralDetailsArea = false;
        _this.HideOtherPartnersArea = false;
        _this.HidePackageTypeArea = false;
        _this.HideAgentSideDataArea = false;
        _this.HideOurSideDataArea = false;
        _this.HideTransShipmentsDetailsArea = false;
        _this.IsNoTransShipmentsDetailsFound = false;
        _this.IsShowTransShipmentsDetails = false;
        _this.IsNoGeneralDetailsFound = false;
        _this.IsNoAgentDataFound = false;
        _this.HeaderDirectionId = "";
        _this.HeaderShipperName = "";
        _this.HeaderRoute = "";
        _this.HeaderLongMaster = "";
        _this.HeaderMaster = "";
        _this.HeaderAgentRef = "";
        _this.HeaderTransportModeId = "";
        _this.IsNoPackagesFound = false;
        _this.EntityPM = new ShipmentPM_1.ShipmentPM();
        _this.IsConsolShipment = false;
        _this.IsHouseShipment = false;
        _this.IsHideDescriptionOfGoods = false;
        _this.IsHideIsDangerous = false;
        _this.IsHideValueOfGoods = false;
        _this.IsHideIncoterm = false;
        _this.IsHideCarrier = false;
        _this.IsHideTransshipment1Carrier = false;
        _this.IsHideTransshipment2Carrier = false;
        _this.IsHideTransshipment3Carrier = false;
        _this.IsHideMoveType = false;
        _this.IsHideValueOfGoodsCurrency = false;
        _this.IsHideMainCarriageVessel = false;
        _this.IsHideTransshipment1Vessel = false;
        _this.IsHideTransshipment2Vessel = false;
        _this.IsHideTransshipment3Vessel = false;
        _this.IsHideMainInterline = false;
        _this.CardDependencyProperty1 = "CS";
        _this.CardDependencyProperty1IsList = false;
        _this.LableCreateButton = "Create";
        _this.IsNoAddtionalFound = false;
        _this.ValidationErrorsList = [];
        _this.FromPortList = null;
        _this.ToPortList = null;
        _this.IsLoadedShipperTranslation = false;
        _this.IsLoadedConsigneeTranslation = false;
        _this.IsLoadedShiperDefaultValues = false;
        _this.IsLoadedConsigneeDefaultValues = false;
        _this.IsLoadedIncotermTranslation = false;
        _this.IsLoadedMoveTypeTranslation = false;
        _this.IsLoadedValueOfGoodsCurrencyTranslation = false;
        _this.IsLoadedCarrierTranslation = false;
        _this.IsLoadedTransshipment1CarrierTranslation = false;
        _this.IsLoadedTransshipment2CarrierTranslation = false;
        _this.IsLoadedTransshipment3CarrierTranslation = false;
        _this.IsLoadedMainCarriageVesselTranslation = false;
        _this.IsLoadedTransshipment1VesselTranslation = false;
        _this.IsLoadedTransshipment2VesselTranslation = false;
        _this.IsLoadedTransshipment3VesselTranslation = false;
        _this.IsLoadedMainCarriageInterlineTranslation = false;
        _this.IsLoadedNotify1IdTranslation = false;
        _this.IsLoadedNotify1DefaultValuesTranslation = false;
        _this.IsLoadedPortsTranslation = false;
        _this.IsLoadedPackageTranslation = false;
        _this.HidePickupDetailsArea = false;
        _this.HideDeliveryDetailsArea = false;
        _this.IsNoPickupDetailsFound = false;
        _this.IsNoDeliveryDetailsFound = false;
        _this.IsLoadedToPickUpTranslation = false;
        _this.IsLoadedFromPickUpTranslation = false;
        _this.IsLoadedFromDeliveryTranslation = false;
        _this.IsLoadedToDeliveryTranslation = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.shiperAddressList = new AddressList_1.AddressList();
        // Consignee
        _this.consigneeAddressList = new AddressList_1.AddressList();
        //IsInterlineAdded
        _this.isInterlineAdded = false;
        _this.ShipmentPackages = [];
        _this.OurAgentPackagesType = [];
        _this.IsHideRoutingLeg1 = false;
        _this.IsHideRoutingLeg2 = false;
        _this.IsHideRoutingLeg3 = false;
        _this.HeightAgentDataTransLationArea = "335px";
        _this.IsLoadAdditionalScreen = false;
        _this.IsLoadSharedManifestheaderScreen = false;
        //-------------------------------------------------------------------------------
        _this.IsRefeshPackageType = false;
        _this.HideAddtionalArea = false;
        _this.IsHaveFromPickUpTranslation = false;
        _this.IsHaveToPickUpTranslation = false;
        _this.IsHaveFromDeliveryTranslation = false;
        _this.IsHaveToDeliveryTranslation = false;
        _this.Retries = 0;
        _this.myPackageTypeService = new PackageTypeListService_1.PackageTypeListService();
        _this.myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        _this.myCardListService = new CardListService_1.CardListService();
        _this.myAddressListService = new AddressListService_1.AddressListService();
        _this.myAirlineListService = new AirlineListService_1.AirlineListService();
        _this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.myIncotermListService = new IncotermListService_1.IncotermListService();
        _this.myPortListService = new PortListService_1.PortListService();
        _this.countryListService = new CountryListService_1.CountryListService();
        _this.myVesselListService = new VesselListService_1.VesselListService();
        _this.myMoveTypeListService = new MoveTypeListService_1.MoveTypeListService();
        _this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        return _this;
    }
    SharedManifestAdditionalComponent.prototype.ngOnInit = function () {
    };
    SharedManifestAdditionalComponent.prototype.SetSalesman = function () {
        this.EntityPM.SalesmanUserId = Tools_1.AppTool.IsNullOrEmpty(this.myShipperSalesmanId) ? this.EntityPM.CreatedByUserId : this.myShipperSalesmanId;
    };
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ShipperAddressList", {
        get: function () { return this.shiperAddressList; },
        set: function (newValue) {
            this.shiperAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ShipperAddressId", {
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
                            if (myResponse.Result) {
                                _this.ShipperAddressList = myResponse.Result;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ShipperContactId", {
        get: function () { return this.EntityPM.ShipperContactId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperContactId != newValue) {
                this.EntityPM.ShipperContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
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
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ShipperCode = myCardList.Code;
                                _this.ShipperPartnerTypeId = myCardList.PartnerTypeId;
                                _this.ShipperContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.ShipperName = myCardList.EnglishName;
                                _this.EntityPM.ShipperNote = myCardList.Notes;
                                _this.EntityPM.ShipperMainAddressId = myCardList.MainAddressId;
                                _this.EntityPM.ShipperPickAddressId = myCardList.PickAddressId;
                                _this.EntityPM.KnownConsignorNumber = myCardList.KnownConsignor;
                                _this.EntityPM.KCExpirationDate = myCardList.KCExpirationDate;
                                _this.ShipperAddressId = myCardList.MainAddressId;
                                _this.myShipperSalesmanId = myCardList.SalesmanUserId;
                                if (!_this.IsConsolShipment && _this.HouseEntity && _this.HouseEntity.Shipper) {
                                    if (_this.HouseEntity.Shipper.Code == _this.ShipperCode) {
                                        _this.EntityPM.ShipperReference1 = _this.HouseEntity.ShipperReference1;
                                        _this.EntityPM.ShipperReference2 = _this.HouseEntity.ShipperReference2;
                                    }
                                }
                                else if (_this.ManifestSL.Shipper && _this.ManifestSL.Shipper.Code == _this.ShipperCode) {
                                    _this.EntityPM.ShipperReference1 = _this.ManifestSL.ShipperReference1;
                                    _this.EntityPM.ShipperReference2 = _this.ManifestSL.ShipperReference2;
                                }
                                _this.SetSalesman();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentId", {
        // Agent
        get: function () { return this.EntityPM.AgentId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.AgentId != newValue) {
                this.EntityPM.AgentId = newValue;
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentAddressId", {
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
                            if (myResponse.Result) {
                                _this.AgentAddressList = myResponse.Result;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentAddressList", {
        get: function () { return this.myAgentAddressList; },
        set: function (newValue) {
            this.myAgentAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentContactId", {
        get: function () { return this.EntityPM.AgentContactId; },
        set: function (newValue) {
            if (this.EntityPM.AgentContactId != newValue) {
                this.EntityPM.AgentContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentReference1", {
        get: function () { return this.EntityPM.AgentReference1; },
        set: function (newValue) {
            if (this.EntityPM.AgentReference1 != newValue) {
                this.EntityPM.AgentReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentReference2", {
        get: function () { return this.EntityPM.AgentReference2; },
        set: function (newValue) {
            if (this.EntityPM.AgentReference2 != newValue) {
                this.EntityPM.AgentReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AgentName", {
        get: function () { return this.EntityPM.AgentName; },
        set: function (newValue) {
            if (this.EntityPM.AgentName != newValue) {
                this.EntityPM.AgentName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeAddressList", {
        get: function () { return this.consigneeAddressList; },
        set: function (newValue) {
            this.consigneeAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeId", {
        get: function () { return this.EntityPM.ConsigneeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
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
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myCardList = myResponse.Result;
                            if (myCardList) {
                                _this.ConsigneeCode = myCardList.Code;
                                _this.ConsigneePartnerTypeId = myCardList.PartnerTypeId;
                                _this.ConsigneeContactId = myCardList.PrimaryContactId;
                                _this.EntityPM.ConsigneeName = myCardList.EnglishName;
                                _this.EntityPM.ConsigneeNote = myCardList.Notes;
                                _this.EntityPM.ConsigneeMainAddressId = myCardList.MainAddressId;
                                _this.EntityPM.ConsigneePickAddressId = myCardList.PickAddressId;
                                _this.ConsigneeAddressId = myCardList.MainAddressId;
                                _this.myConsigneeSalesmanId = myCardList.SalesmanUserId;
                                if (!_this.IsConsolShipment && _this.HouseEntity && _this.HouseEntity.Consignee) {
                                    if (_this.HouseEntity.Consignee.Code == _this.ConsigneeCode) {
                                        _this.EntityPM.ConsigneeReference1 = _this.HouseEntity.ConsigneeReference1;
                                        _this.EntityPM.ConsigneeReference2 = _this.HouseEntity.ConsigneeReference2;
                                    }
                                }
                                else if (_this.ManifestSL.Consignee && _this.ManifestSL.Consignee.Code == _this.ConsigneeCode) {
                                    _this.EntityPM.ConsigneeReference1 = _this.ManifestSL.ConsigneeReference1;
                                    _this.EntityPM.ConsigneeReference2 = _this.ManifestSL.ConsigneeReference2;
                                }
                                _this.SetSalesman();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeAddressId", {
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
                            if (myResponse.Result) {
                                _this.ConsigneeAddressList = myResponse.Result;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeContactId", {
        get: function () { return this.EntityPM.ConsigneeContactId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeContactId != newValue) {
                this.EntityPM.ConsigneeContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeReference1", {
        get: function () { return this.EntityPM.ConsigneeReference1; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference1 != newValue) {
                this.EntityPM.ConsigneeReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeReference2", {
        get: function () { return this.EntityPM.ConsigneeReference2; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference2 != newValue) {
                this.EntityPM.ConsigneeReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ConsigneeName", {
        get: function () { return this.EntityPM.ConsigneeName; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeName != newValue) {
                this.EntityPM.ConsigneeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Notify1Id", {
        get: function () { return this.EntityPM.Notify1Id; },
        set: function (newValue) {
            if (this.EntityPM.Notify1Id != newValue) {
                this.EntityPM.Notify1Id = newValue;
                this.isPartnerChanged_Notify1 = true;
                this.GetNotify1Card();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Notify1AddressId", {
        get: function () { return this.EntityPM.Notify1AddressId; },
        set: function (newValue) {
            if (this.EntityPM.Notify1AddressId != newValue) {
                this.EntityPM.Notify1AddressId = newValue;
                this.GetNotify1Address();
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedManifestAdditionalComponent.prototype.GetNotify1Card = function () {
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
    };
    SharedManifestAdditionalComponent.prototype.LoadNotify1Card = function () {
        var _this = this;
        this.myCardListService.getSingle(this.Notify1Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.Notify1Name = myCard.EnglishName;
                        _this.EntityPM.Notify1Note = myCard.Notes;
                        _this.EntityPM.Notify1ContactId = myCard.PrimaryContactId;
                        _this.Notify1Code = myCard.Code;
                    }
                    if (_this.isPartnerChanged_Notify1) {
                        _this.GetNotify1MainAddress();
                    }
                    else {
                        _this.GetNotify1Address();
                    }
                    _this.isPartnerChanged_Notify1 = false;
                }
            }
        });
    };
    SharedManifestAdditionalComponent.prototype.GetNotify1Address = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify1AddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.Notify1AddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetNotify1Address(myResponse.Result);
                    }
                }
            });
        }
    };
    SharedManifestAdditionalComponent.prototype.GetNotify1MainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify1Id)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.Notify1Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetNotify1Address(myResponse.Result);
                    }
                }
            });
        }
    };
    SharedManifestAdditionalComponent.prototype.SetNotify1Address = function (list) {
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
    };
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Notify2Id", {
        get: function () { return this.EntityPM.Notify2Id; },
        set: function (newValue) {
            if (this.EntityPM.Notify2Id != newValue) {
                this.EntityPM.Notify2Id = newValue;
                this.isPartnerChanged_Notify2 = true;
                this.GetNotify2Card();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Notify2AddressId", {
        get: function () { return this.EntityPM.Notify2AddressId; },
        set: function (newValue) {
            if (this.EntityPM.Notify2AddressId != newValue) {
                this.EntityPM.Notify2AddressId = newValue;
                this.GetNotify2Address();
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedManifestAdditionalComponent.prototype.GetNotify2Card = function () {
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
    };
    SharedManifestAdditionalComponent.prototype.LoadNotify2Card = function () {
        var _this = this;
        this.myCardListService.getSingle(this.Notify2Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myCard = myResponse.Result;
                    if (myCard != null) {
                        _this.EntityPM.Notify2Name = myCard.EnglishName;
                        _this.EntityPM.Notify2Note = myCard.Notes;
                        _this.EntityPM.Notify2ContactId = myCard.PrimaryContactId;
                    }
                    if (_this.isPartnerChanged_Notify2) {
                        _this.GetNotify2MainAddress();
                    }
                    else {
                        _this.GetNotify2Address();
                    }
                    _this.isPartnerChanged_Notify2 = false;
                }
            }
        });
    };
    SharedManifestAdditionalComponent.prototype.GetNotify2Address = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify2AddressId)) {
            var myService = new AddressListService_1.AddressListService();
            myService.getSingle(this.Notify2AddressId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetNotify2Address(myResponse.Result);
                    }
                }
            });
        }
    };
    SharedManifestAdditionalComponent.prototype.GetNotify2MainAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notify2Id)) {
            var myService = new AddressService_1.AddressService();
            myService.GetMainAddressByCardId(this.Notify2Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.SetNotify2Address(myResponse.Result);
                    }
                }
            });
        }
    };
    SharedManifestAdditionalComponent.prototype.SetNotify2Address = function (list) {
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
    };
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "IncotermId", {
        //IncotermId
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
                                _this.IncotermCode = myList.Code;
                                _this.EntityPM.IncotermCode = myList.Code;
                                _this.EntityPM.IncotermName = myList.Name;
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "MoveTypeId", {
        get: function () { return this.EntityPM.MoveTypeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.MoveTypeId != newValue) {
                this.EntityPM.MoveTypeId = newValue;
                if (newValue != null) {
                    this.myMoveTypeListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myList = myResponse.Result;
                            if (myList) {
                                _this.MoveTypeCode = myList.Code;
                                _this.EntityPM.MoveTypeCode = myList.Code;
                                _this.EntityPM.MoveTypeName = myList.MoveTypeEnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ValueOfGoodsCurrencyId", {
        get: function () { return this.EntityPM.ValueOfGoodsCurrencyId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ValueOfGoodsCurrencyId != newValue) {
                this.EntityPM.ValueOfGoodsCurrencyId = newValue;
                if (newValue != null) {
                    this.myCurrencyListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myList = myResponse.Result;
                            if (myList) {
                                _this.ValueOfGoodsCurrencyCode = myList.Code;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "ValueOfGoods", {
        get: function () { return this.EntityPM.ValueOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.ValueOfGoods != newValue) {
                this.EntityPM.ValueOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "IsDangerous", {
        get: function () { return this.EntityPM.IsDangerous; },
        set: function (newValue) {
            if (this.EntityPM.IsDangerous != newValue) {
                this.EntityPM.IsDangerous = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "MainHarmonize", {
        get: function () { return this.EntityPM.MainHarmonize; },
        set: function (newValue) {
            if (this.EntityPM.MainHarmonize != newValue) {
                this.EntityPM.MainHarmonize = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "MainCarriageVesselId", {
        get: function () { return this.EntityPM.MainCarriageVesselId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.MainCarriageVesselId != newValue) {
                this.EntityPM.MainCarriageVesselId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageVesselId)) {
                    if (!this.IsHideMainCarriageVessel) {
                        this.myVesselListService.getSingleFromCache(this.EntityPM.MainCarriageVesselId).subscribe(function (myResponse) {
                            if (myResponse != null) {
                                if (!myResponse.HasError) {
                                    var list = myResponse.Result;
                                    if (list) {
                                        _this.MainCarriageVesselCode = myResponse.Result.Code;
                                        _this.EntityPM.MainCarriageVesselName = myResponse.Result.Code;
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment1VesselId", {
        get: function () { return this.EntityPM.Transshipment1VesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment1VesselId != value) {
                this.EntityPM.Transshipment1VesselId = value;
                if (!this.IsHideTransshipment1Vessel) {
                    if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.EntityPM.Transshipment1VesselName = null;
                        this.Transshipment1VesselCode = null;
                    }
                    else {
                        this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    _this.Transshipment1VesselCode = list.Code;
                                    _this.EntityPM.Transshipment1VesselName = list.EnglishName;
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment2VesselId", {
        get: function () { return this.EntityPM.Transshipment2VesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment2VesselId != value) {
                this.EntityPM.Transshipment2VesselId = value;
                if (!this.IsHideTransshipment2Vessel) {
                    if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.EntityPM.Transshipment2VesselName = null;
                        this.Transshipment2VesselCode = null;
                    }
                    else {
                        this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    _this.Transshipment2VesselCode = list.Code;
                                    _this.EntityPM.Transshipment2VesselName = list.EnglishName;
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment3VesselId", {
        get: function () { return this.EntityPM.Transshipment3VesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment3VesselId != value) {
                this.EntityPM.Transshipment3VesselId = value;
                if (!this.IsHideTransshipment3Vessel) {
                    if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.EntityPM.Transshipment3VesselName = null;
                        this.Transshipment3VesselCode = null;
                    }
                    else {
                        this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    _this.Transshipment3VesselCode = list.Code;
                                    _this.EntityPM.Transshipment3VesselName = list.EnglishName;
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "IsInterlineAdded", {
        get: function () { return this.isInterlineAdded; },
        set: function (value) {
            if (this.isInterlineAdded != value) {
                this.isInterlineAdded = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "InterlineId", {
        get: function () { return this.EntityPM.InterlineId; },
        set: function (newValue) {
            if (this.EntityPM.InterlineId != newValue) {
                this.EntityPM.InterlineId = newValue;
                this.OnInterlineChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedManifestAdditionalComponent.prototype.OnInterlineChanged = function () {
        var _this = this;
        var myAirlineId = this.InterlineId;
        if (Tools_1.AppTool.IsNullOrEmpty(myAirlineId)) {
            myAirlineId = this.MainCarriageCarrierId;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myAirlineId)) {
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;
            this.AirlinePrefix = null;
        }
        else {
            this.myAirlineListService.getSingleFromCache(myAirlineId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.InterlineCode = list.Code;
                        _this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                        _this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;
                        var myPrefix = null;
                        if (!Tools_1.AppTool.IsNullOrEmpty(list.Prefix)) {
                            myPrefix = list.Prefix.toString().trim();
                            myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                        }
                        _this.AirlinePrefix = myPrefix;
                        _this.EntityPM.AirlinePrefix = myPrefix;
                    }
                }
            });
        }
    };
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.EntityPM.FreightPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.FreightPrepaidCollectId != newValue) {
                this.EntityPM.FreightPrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.EntityPM.OtherPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.OtherPrepaidCollectId != newValue) {
                this.EntityPM.OtherPrepaidCollectId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment1CarrierId", {
        // Carrier
        get: function () { return this.EntityPM.Transshipment1CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment1CarrierId != value) {
                this.EntityPM.Transshipment1CarrierId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Transshipment1CarrierPrefix = null;
                    //  this.Transshipment1CarrierNumber = null;
                    this.Transshipment1CarrierCode = null;
                    this.EntityPM.Transshipment1AdditionalMAWBOBLBL = null;
                    //RoutingHelper.Transshipment1CarrierChanged(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.Transshipment1CarrierCode = list.Code;
                                _this.EntityPM.Transshipment1CarrierCode = list.Code;
                                _this.EntityPM.Transshipment1CarrierName = list.EnglishName;
                                _this.EntityPM.Transshipment1CarrierWebSite = list.WebSite;
                                if (_this.EntityPM.TransportModeId == "A") {
                                    _this.Transshipment1CarrierPrefix = list.Code;
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment2CarrierId", {
        get: function () { return this.EntityPM.Transshipment2CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment2CarrierId != value) {
                this.EntityPM.Transshipment2CarrierId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Transshipment2CarrierPrefix = null;
                    this.Transshipment2CarrierCode = null;
                    // this.Transshipment2CarrierNumber = null;
                    // this.Transshipment2AdditionalMAWBOBLBL = null;
                    //   RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.Transshipment2CarrierCode = list.Code;
                                _this.EntityPM.Transshipment2CarrierCode = list.Code;
                                _this.EntityPM.Transshipment2CarrierName = list.EnglishName;
                                _this.EntityPM.Transshipment2CarrierWebSite = list.WebSite;
                                if (_this.EntityPM.TransportModeId == "A") {
                                    _this.Transshipment2CarrierPrefix = list.Code;
                                }
                                // RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, list);
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment3CarrierId", {
        get: function () { return this.EntityPM.Transshipment3CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment3CarrierId != value) {
                this.EntityPM.Transshipment3CarrierId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Transshipment3CarrierPrefix = null;
                    //  this.Transshipment3CarrierNumber = null;
                    this.Transshipment3CarrierCode = null;
                    this.EntityPM.Transshipment3AdditionalMAWBOBLBL = null;
                    //RoutingHelper.Transshipment3CarrierChanged(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.Transshipment3CarrierCode = list.Code;
                                _this.EntityPM.Transshipment3CarrierCode = list.Code;
                                _this.EntityPM.Transshipment3CarrierName = list.EnglishName;
                                _this.EntityPM.Transshipment3CarrierWebSite = list.WebSite;
                                if (_this.EntityPM.TransportModeId == "A") {
                                    _this.Transshipment3CarrierPrefix = list.Code;
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment1CarrierPrefix", {
        // Carrier Prefix
        get: function () { return this.EntityPM.Transshipment1CarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.Transshipment1CarrierPrefix != value) {
                this.EntityPM.Transshipment1CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment2CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment2CarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.Transshipment2CarrierPrefix != value) {
                this.EntityPM.Transshipment2CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Transshipment3CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment3CarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.Transshipment3CarrierPrefix != value) {
                this.EntityPM.Transshipment3CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "MainCarriageCarrierId", {
        //MainCarriageCarrierId
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            var _this = this;
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
                    Tools_2.ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myCardListResponse) {
                        if (!myCardListResponse.HasError) {
                            var myCardList = myCardListResponse.Result;
                            if (myCardList) {
                                _this.MainCarriageCarrierCode = myCardList.Code;
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AirlinePrefix", {
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        set: function (newValue) {
            if (this.EntityPM.LongMaster != newValue) {
                this.EntityPM.LongMaster = newValue;
                // this.ValidateMasterField();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "Master", {
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
    Object.defineProperty(SharedManifestAdditionalComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) {
            if (this.EntityPM.AccountNumber != newValue) {
                this.EntityPM.AccountNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedManifestAdditionalComponent.prototype.SetWindowArgs = function (args) {
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
    };
    SharedManifestAdditionalComponent.prototype.BuildAgentShipmentPickUpDelivey = function () {
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
    };
    SharedManifestAdditionalComponent.prototype.FullShipmentProperites = function () {
        var _this = this;
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
            this.EntityPM.MasterShipmentDataId = this.ManifestSL != null ? this.ManifestSL.EntityId : null;
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
            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.myPortListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (_this.ManifestSL.MainCarriageFromPort != null) {
                        var fromPort = list.filter(function (d) { return d.Code == _this.ManifestSL.MainCarriageFromPort.Code; })[0];
                        if (fromPort) {
                            _this.EntityPM.FromPortId = fromPort.Id;
                            _this.EntityPM.MainCarriageFromPortId = fromPort.Id;
                            _this.FromPortList = fromPort;
                        }
                    }
                    if (_this.ManifestSL.MainCarriageToPort != null) {
                        var toPort = list.filter(function (d) { return d.Code == _this.ManifestSL.MainCarriageToPort.Code; })[0];
                        if (toPort) {
                            _this.EntityPM.ToPortId = toPort.Id;
                            _this.EntityPM.MainCarriageToPortId = toPort.Id;
                            _this.ToPortList = toPort;
                        }
                    }
                    if (!_this.IsHouseShipment) {
                        //Transshipment1
                        if (_this.ManifestSL.Transshipment1FromPort != null) {
                            var transshipment1FromPortCode = list.filter(function (d) { return d.Code == _this.ManifestSL.Transshipment1FromPort.Code; })[0];
                            if (transshipment1FromPortCode) {
                                _this.EntityPM.Transshipment1FromPortCode = transshipment1FromPortCode.Code;
                                _this.EntityPM.Transshipment1FromPortName = transshipment1FromPortCode.EnglishName;
                                _this.EntityPM.Transshipment1FromPortId = transshipment1FromPortCode.Id;
                                _this.EntityPM.Transshipment1FromPortCountryCode = transshipment1FromPortCode.CountryCode;
                                _this.EntityPM.Transshipment1FromPortCountryName = transshipment1FromPortCode.CountryName;
                            }
                        }
                        if (_this.ManifestSL.Transshipment1ToPort != null) {
                            var transshipment1ToPortCode = list.filter(function (d) { return d.Code == _this.ManifestSL.Transshipment1ToPort.Code; })[0];
                            if (transshipment1ToPortCode) {
                                _this.EntityPM.Transshipment1ToPortCode = transshipment1ToPortCode.Code;
                                _this.EntityPM.Transshipment1ToPortName = transshipment1ToPortCode.EnglishName;
                                _this.EntityPM.Transshipment1ToPortId = transshipment1ToPortCode.Id;
                                _this.EntityPM.Transshipment1ToPortCountryCode = transshipment1ToPortCode.CountryCode;
                                _this.EntityPM.Transshipment1ToPortCountryName = transshipment1ToPortCode.CountryName;
                            }
                        }
                        //Transshipment2
                        if (_this.ManifestSL.Transshipment2FromPort != null) {
                            var transshipment2FromPortCode = list.filter(function (d) { return d.Code == _this.ManifestSL.Transshipment2FromPort.Code; })[0];
                            if (transshipment2FromPortCode) {
                                _this.EntityPM.Transshipment2FromPortCode = transshipment2FromPortCode.Code;
                                _this.EntityPM.Transshipment2FromPortName = transshipment2FromPortCode.EnglishName;
                                _this.EntityPM.Transshipment2FromPortId = transshipment2FromPortCode.Id;
                                _this.EntityPM.Transshipment2FromPortCountryCode = transshipment2FromPortCode.CountryCode;
                                _this.EntityPM.Transshipment2FromPortCountryName = transshipment2FromPortCode.CountryName;
                            }
                        }
                        if (_this.ManifestSL.Transshipment2ToPort != null) {
                            var transshipment2ToPortCode = list.filter(function (d) { return d.Code == _this.ManifestSL.Transshipment2ToPort.Code; })[0];
                            if (transshipment2ToPortCode) {
                                _this.EntityPM.Transshipment2ToPortCode = transshipment2ToPortCode.Code;
                                _this.EntityPM.Transshipment2ToPortName = transshipment2ToPortCode.EnglishName;
                                _this.EntityPM.Transshipment2ToPortId = transshipment2ToPortCode.Id;
                                _this.EntityPM.Transshipment2ToPortCountryCode = transshipment2ToPortCode.CountryCode;
                                _this.EntityPM.Transshipment2ToPortCountryName = transshipment2ToPortCode.CountryName;
                            }
                        }
                        //Transshipment3
                        if (_this.ManifestSL.Transshipment3FromPort != null) {
                            var transshipment3FromPortCode = list.filter(function (d) { return d.Code == _this.ManifestSL.Transshipment3FromPort.Code; })[0];
                            if (transshipment3FromPortCode) {
                                _this.EntityPM.Transshipment3FromPortCode = transshipment3FromPortCode.Code;
                                _this.EntityPM.Transshipment3FromPortName = transshipment3FromPortCode.EnglishName;
                                _this.EntityPM.Transshipment3FromPortId = transshipment3FromPortCode.Id;
                                _this.EntityPM.Transshipment3FromPortCountryCode = transshipment3FromPortCode.CountryCode;
                                _this.EntityPM.Transshipment3FromPortCountryName = transshipment3FromPortCode.CountryName;
                            }
                        }
                        if (_this.ManifestSL.Transshipment3ToPort != null) {
                            var transshipment3ToPortCode = list.filter(function (d) { return d.Code == _this.ManifestSL.Transshipment3ToPort.Code; })[0];
                            if (transshipment3ToPortCode) {
                                _this.EntityPM.Transshipment3ToPortCode = transshipment3ToPortCode.Code;
                                _this.EntityPM.Transshipment3ToPortName = transshipment3ToPortCode.EnglishName;
                                _this.EntityPM.Transshipment3ToPortId = transshipment3ToPortCode.Id;
                                _this.EntityPM.Transshipment3ToPortCountryCode = transshipment3ToPortCode.CountryCode;
                                _this.EntityPM.Transshipment3ToPortCountryName = transshipment3ToPortCode.CountryName;
                            }
                        }
                    }
                }
                _this.IsLoadedPortsTranslation = true;
                _this.StopBusyIndicator();
            });
        }
        if (this.EntityPM && this.EntityPM.TransportModeId == "A") {
            this.IsRemovePackageAreaFromScreen = true;
        }
    };
    SharedManifestAdditionalComponent.prototype.BuildAgentSide = function () {
        if (!this.HouseEntity) {
            this.AgentSideData = new AgentSide(this.ManifestSL, null);
        }
        else {
            this.AgentSideData = new AgentSide(null, this.HouseEntity);
        }
    };
    SharedManifestAdditionalComponent.prototype.BuildOurSide = function () {
        var _this = this;
        switch (this.ManifestSL.TransportModeId) {
            case "A": {
                this.CarrierDependencyProperty1 = "AL";
                break;
            }
            case "O": {
                this.CarrierDependencyProperty1 = "SL";
                break;
            }
            case "I": {
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }
        if (this.HouseEntity) {
            this.IsHouseShipment = true;
            this.LableCreateButton = "Create House";
        }
        else {
            if (this.ManifestSL.ShipmentLevelCode == "C") {
                this.LableCreateButton = "Create Master";
                this.IsConsolShipment = true;
                this.ShipperId = this.CurrentEntity.AgentId;
                this.ConsigneeId = SessionLocator_1.SessionLocator.TenantPM.AgentId;
                this.ConsigneeAddressId = SessionLocator_1.SessionLocator.TenantPM.AddressId;
                this.ObjectTableName = "Master";
            }
            else {
                this.AgentId = this.CurrentEntity.AgentId;
                this.LableCreateButton = "Create Direct";
            }
        }
        //     LableCreateButton
        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }
        if (!this.IsHouseShipment)
            this.IsShowTransShipmentsDetails = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.IncotermCode))
            this.IsHideIncoterm = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.ValueOfGoods))
            this.IsHideValueOfGoods = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.DescriptionOfGoods))
            this.IsHideDescriptionOfGoods = true;
        if (!this.AgentSideData.IsDangerous)
            this.IsHideIsDangerous = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.MoveTypeCode))
            this.IsHideMoveType = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.ValueOfGoodsCurrencyCode))
            this.IsHideValueOfGoodsCurrency = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.MainHarmonize))
            this.IsHideMainHarmonize = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.CarrierCode) || this.EntityPM.ShipmentLevelCode == "H")
            this.IsHideCarrier = true;
        if (this.IsHouseShipment || this.EntityPM.TransportModeId != "O" || Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.MainCarriageVesselCode))
            this.IsHideMainCarriageVessel = true;
        if (this.IsHouseShipment || this.EntityPM.TransportModeId != "A" || Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.InterlineCode))
            this.IsHideMainInterline = true;
        if (this.IsHouseShipment || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortCode))
            this.IsHideRoutingLeg1 = true;
        if (this.IsHouseShipment || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortCode))
            this.IsHideRoutingLeg2 = true;
        if (this.IsHouseShipment || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3FromPortCode))
            this.IsHideRoutingLeg3 = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.IncotermId) && !this.AgentSideData.IncotermAddedManually) {
            this.IncotermId = this.AgentSideData.IncotermId;
            this.IncotermCode = this.AgentSideData.IncotermCode;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.CarrierId) && !this.AgentSideData.CarrierAddedManually) {
            this.MainCarriageCarrierId = this.AgentSideData.CarrierId;
            this.MainCarriageCarrierCode = this.AgentSideData.CarrierCode;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.MoveTypeId) && !this.AgentSideData.MoveTypeAddedManually) {
            this.MoveTypeId = this.AgentSideData.MoveTypeId;
            this.MoveTypeCode = this.AgentSideData.MoveTypeCode;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.ValueOfGoodsCurrencyId) && !this.AgentSideData.ValueOfGoodsCurrencyAddedManually) {
            this.ValueOfGoodsCurrencyId = this.AgentSideData.ValueOfGoodsCurrencyId;
            this.ValueOfGoodsCurrencyCode = this.AgentSideData.ValueOfGoodsCurrencyCode;
        }
        //leg 1
        if (!this.IsHideRoutingLeg1) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment1CarrierCode)) {
                if (!this.AgentSideData.Transshipment1CarrierAddedManually) {
                    this.Transshipment1CarrierId = this.ManifestSL.Transshipment1CarrierId;
                }
            }
            else
                this.IsHideTransshipment1Carrier = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment1VesselCode) && this.EntityPM.TransportModeId == "O") {
                if (!this.AgentSideData.Transshipment1VesselAddedManually) {
                    this.Transshipment1VesselId = this.ManifestSL.Transshipment1VesselId;
                }
            }
            else
                this.IsHideTransshipment1Vessel = true;
            if (this.IsHideTransshipment1Carrier && this.IsHideTransshipment1Vessel)
                this.IsHideRoutingLeg1 = true;
        }
        else {
            this.IsHideTransshipment1Vessel = true;
            this.IsHideTransshipment1Carrier = true;
        }
        //leg 2
        if (!this.IsHideRoutingLeg2) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment2CarrierCode)) {
                if (!this.AgentSideData.Transshipment2CarrierAddedManually) {
                    this.Transshipment2CarrierId = this.ManifestSL.Transshipment2CarrierId;
                }
            }
            else
                this.IsHideTransshipment2Carrier = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment2VesselCode) && this.EntityPM.TransportModeId == "O") {
                if (!this.AgentSideData.Transshipment2VesselAddedManually) {
                    this.Transshipment2VesselId = this.ManifestSL.Transshipment2VesselId;
                }
            }
            else
                this.IsHideTransshipment2Vessel = true;
            if (this.IsHideTransshipment2Carrier && this.IsHideTransshipment2Vessel)
                this.IsHideRoutingLeg2 = true;
        }
        else {
            this.IsHideTransshipment2Vessel = true;
            this.IsHideTransshipment2Carrier = true;
        }
        //leg 3
        if (!this.IsHideRoutingLeg3) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment3CarrierCode)) {
                if (!this.AgentSideData.Transshipment3CarrierAddedManually) {
                    this.Transshipment3CarrierId = this.ManifestSL.Transshipment3CarrierId;
                }
            }
            else
                this.IsHideTransshipment3Carrier = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.Transshipment3VesselCode) && this.EntityPM.TransportModeId == "O") {
                if (!this.AgentSideData.Transshipment3VesselAddedManually) {
                    this.Transshipment3VesselId = this.ManifestSL.Transshipment3VesselId;
                }
            }
            else
                this.IsHideTransshipment3Vessel = true;
            if (this.IsHideTransshipment3Carrier && this.IsHideTransshipment3Vessel)
                this.IsHideRoutingLeg3 = true;
        }
        else {
            this.IsHideTransshipment3Vessel = true;
            this.IsHideTransshipment3Carrier = true;
        }
        if (this.IsHideRoutingLeg1 && this.IsHideRoutingLeg2 && this.IsHideRoutingLeg3) {
            this.IsNoTransShipmentsDetailsFound = true;
            this.HideTransShipmentsDetailsArea = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.MainCarriageVesselId) && !this.AgentSideData.MainCarriageVesselAddedManually)
            this.MainCarriageVesselId = this.ManifestSL.MainCarriageVesselId;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.InterlineId) && !this.AgentSideData.InterlineAddedManually)
            this.InterlineId = this.ManifestSL.InterlineId;
        // Shipper Translation
        if (this.AgentSideData.Shipper && !this.IsConsolShipment) {
            var ShipperTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.Shipper.Code && f.ObjectTableName == "ShipperCard"; })[0];
            if (ShipperTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(ShipperTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.ShipperId = myResult;
                        }
                    }
                    _this.IsLoadedShipperTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedShipperTranslation = true;
        }
        else
            this.IsLoadedShipperTranslation = true;
        if (this.AgentSideData.Consignee && !this.IsConsolShipment) {
            var consigneeTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.Consignee.Code && f.ObjectTableName == "ConsigneeCard"; })[0];
            if (consigneeTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(consigneeTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.ConsigneeId = myResult;
                        }
                    }
                    _this.IsLoadedConsigneeTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedConsigneeTranslation = true;
        }
        else
            this.IsLoadedConsigneeTranslation = true;
        //Shipper DefaultValues
        if (this.AgentSideData.Shipper && !this.IsConsolShipment) {
            var englishName = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.EnglishName) ? this.AgentSideData.Shipper.EnglishName : "";
            var Address1 = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.Address1) ? this.AgentSideData.Shipper.Address1 : "";
            var Address2 = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.Address2) ? this.AgentSideData.Shipper.Address2 : "";
            var city = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Shipper.City) ? this.AgentSideData.Shipper.City : "";
            this.ShiperDefaultValues = englishName + "^";
            this.ShiperDefaultValues += (Address1 + "^");
            this.ShiperDefaultValues += (Address2 + "^");
            this.ShiperDefaultValues += (city + "^");
            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            apiQueryFilters.addAdditionalFilter("Code", this.AgentSideData.Shipper.CountryCode, null, null, "Equals", true, true, true, "Text");
            this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    var CountryList = list.filter(function (d) { return d.Code == _this.AgentSideData.Shipper.CountryCode; })[0];
                    if (CountryList) {
                        _this.ShiperDefaultValues += (CountryList.Id);
                    }
                }
                _this.IsLoadedShiperDefaultValues = true;
                _this.StopBusyIndicator();
            });
        }
        else
            this.IsLoadedShiperDefaultValues = true;
        //Consignee  DefaultValues 
        if (this.AgentSideData.Consignee && !this.IsConsolShipment) {
            var englishName = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.EnglishName) ? this.AgentSideData.Consignee.EnglishName : "";
            var Address1 = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.Address1) ? this.AgentSideData.Consignee.Address1 : "";
            var Address2 = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.Address2) ? this.AgentSideData.Consignee.Address2 : "";
            var city = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.City) ? this.AgentSideData.Consignee.City : "";
            this.ConsigneeDefaultValues = englishName + "^";
            this.ConsigneeDefaultValues += (Address1 + "^");
            this.ConsigneeDefaultValues += (Address2 + "^");
            this.ConsigneeDefaultValues += (city + "^");
            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            apiQueryFilters.addAdditionalFilter("Code", this.AgentSideData.Consignee.CountryCode, null, null, "Equals", true, true, true, "Text");
            this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    var CountryList = list.filter(function (d) { return d.Code == _this.AgentSideData.Consignee.CountryCode; })[0];
                    if (CountryList) {
                        _this.ConsigneeDefaultValues += (CountryList.Id);
                    }
                }
                _this.IsLoadedConsigneeDefaultValues = true;
                _this.StopBusyIndicator();
            });
        }
        else
            this.IsLoadedConsigneeDefaultValues = true;
        // Incoterm Translation
        if (!this.IsHideIncoterm && this.AgentSideData.IncotermAddedManually) {
            var incotermTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.IncotermCode && f.ObjectTableName == "Incoterm"; })[0];
            if (incotermTranslation) {
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", incotermTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myIncotermListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var IncotermList = list.filter(function (d) { return d.Code == incotermTranslation.MyCode && !d.InActive; })[0];
                        if (IncotermList)
                            _this.IncotermId = IncotermList.Id;
                    }
                    _this.FreightPrepaidCollectId = _this.AgentSideData.FreightPrepaidCollectId;
                    _this.OtherPrepaidCollectId = _this.AgentSideData.OtherPrepaidCollectId;
                    _this.IsLoadedIncotermTranslation = true;
                    _this.StopBusyIndicator();
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
            var moveTypeTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.MoveTypeCode && f.ObjectTableName == "MoveType"; })[0];
            if (moveTypeTranslation) {
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", moveTypeTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myMoveTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var MoveTypeList = list.filter(function (d) { return d.Code == moveTypeTranslation.MyCode && !d.InActive; })[0];
                        if (MoveTypeList)
                            _this.MoveTypeId = MoveTypeList.Id;
                    }
                    _this.IsLoadedMoveTypeTranslation = true;
                    _this.StopBusyIndicator();
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
            var valueOfGoodsCurrencyCodeTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.ValueOfGoodsCurrencyCode && f.ObjectTableName == "ValueOfGoodsCurrency"; })[0];
            if (valueOfGoodsCurrencyCodeTranslation) {
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", valueOfGoodsCurrencyCodeTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myCurrencyListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var currencyList = list.filter(function (d) { return d.Code == valueOfGoodsCurrencyCodeTranslation.MyCode && !d.InActive; })[0];
                        if (currencyList)
                            _this.ValueOfGoodsCurrencyId = currencyList.Id;
                    }
                    _this.IsLoadedValueOfGoodsCurrencyTranslation = true;
                    _this.StopBusyIndicator();
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
            var carrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.CarrierCode && f.ObjectTableName == "Carrier"; })[0];
            if (carrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(carrierTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.MainCarriageCarrierId = myResult;
                        }
                    }
                    _this.IsLoadedCarrierTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedCarrierTranslation = true;
        }
        else
            this.IsLoadedCarrierTranslation = true;
        // 2 shipment1Carrier
        if (!this.IsHideTransshipment1Carrier && this.AgentSideData.Transshipment1CarrierAddedManually) {
            var shipment1CarrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.Transshipment1CarrierCode && f.ObjectTableName == "Transshipment1Carrier"; })[0];
            if (shipment1CarrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(shipment1CarrierTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.Transshipment1CarrierId = myResult;
                        }
                    }
                    _this.IsLoadedTransshipment1CarrierTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedTransshipment1CarrierTranslation = true;
        }
        else
            this.IsLoadedTransshipment1CarrierTranslation = true;
        // 3 shipmen2Carrier
        if (!this.IsHideTransshipment2Carrier && this.AgentSideData.Transshipment2CarrierAddedManually) {
            var transshipment2CarrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.Transshipment2CarrierCode && f.ObjectTableName == "Transshipment2Carrier"; })[0];
            if (transshipment2CarrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(transshipment2CarrierTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.Transshipment2CarrierId = myResult;
                        }
                    }
                    _this.IsLoadedTransshipment2CarrierTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedTransshipment2CarrierTranslation = true;
        }
        else
            this.IsLoadedTransshipment2CarrierTranslation = true;
        // 4 shipmen3Carrier
        if (!this.IsHideTransshipment3Carrier && this.AgentSideData.Transshipment3CarrierAddedManually) {
            var transshipment3CarrierTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.Transshipment3CarrierCode && f.ObjectTableName == "Transshipment3Carrier"; })[0];
            if (transshipment3CarrierTranslation) {
                this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(transshipment3CarrierTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            _this.Transshipment3CarrierId = myResult;
                        }
                    }
                    _this.IsLoadedTransshipment3CarrierTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedTransshipment3CarrierTranslation = true;
        }
        else
            this.IsLoadedTransshipment3CarrierTranslation = true;
        //_______________________________ End ____________________________________________
        //_______________________________ Vessel Translation  Start ____________________________________________
        // 1 MainCarriageVessel 
        if (!this.IsHideMainCarriageVessel && this.AgentSideData.MainCarriageVesselAddedManually) {
            var mainCarriageVesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.MainCarriageVesselCode && f.ObjectTableName == "MainCarriageVessel"; })[0];
            if (mainCarriageVesselTranslation) {
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", mainCarriageVesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                this.myVesselListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var VesselList = list.filter(function (d) { return d.Code == mainCarriageVesselTranslation.MyCode && !d.InActive; })[0];
                        if (VesselList) {
                            _this.MainCarriageVesselId = VesselList.Id;
                        }
                    }
                    _this.IsLoadedMainCarriageVesselTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedMainCarriageVesselTranslation = true;
        }
        else
            this.IsLoadedMainCarriageVesselTranslation = true;
        // 2 Transshipment1Vessel 
        if (!this.IsHideTransshipment1Vessel && this.AgentSideData.Transshipment1VesselAddedManually) {
            var transshipment1VesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.Transshipment1VesselCode && f.ObjectTableName == "Transshipment1Vessel"; })[0];
            if (transshipment1VesselTranslation) {
                var myService = new VesselListService_1.VesselListService();
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", transshipment1VesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                myService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var VesselList = list.filter(function (d) { return d.Code == transshipment1VesselTranslation.MyCode && !d.InActive; })[0];
                        if (VesselList) {
                            _this.Transshipment1VesselId = VesselList.Id;
                        }
                    }
                    _this.IsLoadedTransshipment1VesselTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedTransshipment1VesselTranslation = true;
        }
        else
            this.IsLoadedTransshipment1VesselTranslation = true;
        // 3 Transshipment2Vessel 
        if (!this.IsHideTransshipment2Vessel && this.AgentSideData.Transshipment2VesselAddedManually) {
            var transshipment2VesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.Transshipment2VesselCode && f.ObjectTableName == "Transshipment2Vessel"; })[0];
            if (transshipment2VesselTranslation) {
                var myService = new VesselListService_1.VesselListService();
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", transshipment2VesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                myService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var VesselList = list.filter(function (d) { return d.Code == transshipment2VesselTranslation.MyCode && !d.InActive; })[0];
                        if (VesselList) {
                            _this.Transshipment2VesselId = VesselList.Id;
                        }
                    }
                    _this.IsLoadedTransshipment2VesselTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedTransshipment2VesselTranslation = true;
        }
        else
            this.IsLoadedTransshipment2VesselTranslation = true;
        // 4 Transshipment3Vessel 
        if (!this.IsHideTransshipment3Vessel && this.AgentSideData.Transshipment3VesselAddedManually) {
            var transshipment3VesselTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.Transshipment3VesselCode && f.ObjectTableName == "Transshipment3Vessel"; })[0];
            if (transshipment3VesselTranslation) {
                var myService = new VesselListService_1.VesselListService();
                var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                apiQueryFilters.GetAll = true;
                apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                apiQueryFilters.addAdditionalFilter("Code", transshipment3VesselTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                myService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var VesselList = list.filter(function (d) { return d.Code == transshipment3VesselTranslation.MyCode && !d.InActive; })[0];
                        if (VesselList) {
                            _this.Transshipment3VesselId = VesselList.Id;
                        }
                    }
                    _this.IsLoadedTransshipment3VesselTranslation = true;
                    _this.StopBusyIndicator();
                });
            }
            else
                this.IsLoadedTransshipment3VesselTranslation = true;
        }
        else
            this.IsLoadedTransshipment3VesselTranslation = true;
        //_______________________________ End ____________________________________________
        // Interline Translation
        if (!this.IsHideMainInterline && this.AgentSideData.InterlineAddedManually) {
            if (this.ManifestSL) {
                var MainCarriageInterlineTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.ManifestSL.InterlineCode && f.ObjectTableName == "MainCarriageInterline"; })[0];
                if (MainCarriageInterlineTranslation) {
                    var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    this.myAirlineListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            var airlineList = list.filter(function (d) { return d.Code == MainCarriageInterlineTranslation.MyCode && !d.InActive; })[0];
                            if (airlineList)
                                _this.InterlineId = airlineList.Id;
                        }
                        _this.IsLoadedMainCarriageInterlineTranslation = true;
                        _this.StopBusyIndicator();
                    });
                }
                else
                    this.IsLoadedMainCarriageInterlineTranslation = true;
            }
            else
                this.IsLoadedMainCarriageInterlineTranslation = true;
        }
        else
            this.IsLoadedMainCarriageInterlineTranslation = true;
        // Package Translation
        if (this.AgentSideData.PackageTypes && this.AgentSideData.PackageTypes.length > 0) {
            this.myPackageTypeService.getAllFromCache().subscribe(function (resp) {
                if (!resp.HasError) {
                    _this.AllPackageTypes = resp.Result;
                }
                _this.AgentSideData.PackageTypes.forEach(function (item) {
                    var packageTypePM = new PackageTypePM_1.PackageTypePM();
                    packageTypePM.ComputedLocalName = item.Code;
                    packageTypePM.Code = item.Code;
                    packageTypePM.EnglishName = item.EnglishName;
                    packageTypePM.IsContainer = item.IsContainer;
                    packageTypePM.Id = item.Id;
                    if (item.AddedManually) {
                        var packageitem = _this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == item.Code && f.ObjectTableName == "Package"; })[0];
                        if (packageitem) {
                            if (_this.AllPackageTypes) {
                                var packageTranslate = _this.AllPackageTypes.filter(function (d) { return d.Code == packageitem.MyCode && !d.InActive; })[0];
                                if (packageTranslate) {
                                    packageTypePM.Id = packageTranslate.Id;
                                    packageTypePM.Code = packageTranslate.Code;
                                    packageTypePM.EnglishName = packageTranslate.EnglishName;
                                    packageTypePM.IsContainer = packageTranslate.IsContainer;
                                }
                            }
                        }
                    }
                    _this.OurAgentPackagesType.push(packageTypePM);
                });
                if (!_this.OurAgentPackagesType.filter(function (d) { return Tools_1.AppTool.IsNullOrEmpty(d.Id); })[0]) {
                    _this.HidePackageTypeArea = true;
                }
                _this.IsLoadedPackageTranslation = true;
                _this.StopBusyIndicator();
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
                if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Code)) {
                    var notify1Translation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == _this.AgentSideData.Notify1.Code && f.ObjectTableName == "Notify1Card"; })[0];
                    if (notify1Translation) {
                        this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(notify1Translation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {
                                    _this.Notify1Id = myResult;
                                    _this.HideOtherPartnersArea = true;
                                }
                            }
                            _this.IsLoadedNotify1IdTranslation = true;
                            _this.StopBusyIndicator();
                        });
                    }
                    else
                        this.IsLoadedNotify1IdTranslation = true;
                    var englishName = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.EnglishName) ? this.AgentSideData.Notify1.EnglishName : "";
                    var Address1 = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Address1) ? this.AgentSideData.Notify1.Address1 : "";
                    var Address2 = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Address2) ? this.AgentSideData.Notify1.Address2 : "";
                    var city = !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.City) ? this.AgentSideData.Notify1.City : "";
                    this.Notify1DefaultValues = englishName + "^";
                    this.Notify1DefaultValues += (Address1 + "^");
                    this.Notify1DefaultValues += (Address2 + "^");
                    this.Notify1DefaultValues += (city + "^");
                    var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    apiQueryFilters.addAdditionalFilter("Code", this.AgentSideData.Notify1.CountryCode, null, null, "Equals", true, true, true, "Text");
                    this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            var CountryList = list.filter(function (d) { return d.Code == _this.AgentSideData.Notify1.CountryCode && !d.InActive; })[0];
                            if (CountryList) {
                                _this.Notify1DefaultValues += (CountryList.Id);
                            }
                        }
                        _this.IsLoadedNotify1DefaultValuesTranslation = true;
                        _this.StopBusyIndicator();
                    });
                }
                else {
                    this.HideOtherPartnersArea = true;
                    this.IsLoadedNotify1IdTranslation = true;
                    this.IsLoadedNotify1DefaultValuesTranslation = true;
                }
            }
        }
        if (this.IsHideCarrier && this.IsHideMainCarriageVessel && this.IsHideMainInterline && this.IsConsolShipment) {
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
    };
    SharedManifestAdditionalComponent.prototype.ComputeHeightAgentDataTransLationArea = function () {
        var height = 335;
        if (this.IsHideCarrier)
            height -= 20;
        if (this.IsHideMainCarriageVessel && this.IsHideMainInterline)
            height -= 20;
        this.HeightAgentDataTransLationArea = height.toString() + "px";
    };
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
    SharedManifestAdditionalComponent.prototype.SetDataOnFinish = function () {
        this.EntityPM.MainCarriageFinalDestinationPortId = this.EntityPM.MainCarriageToPortId;
        this.SetPartnersOnFinish();
        this.SetCountryECOnFinish();
    };
    SharedManifestAdditionalComponent.prototype.SetPartnersOnFinish = function () {
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
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
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
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SalesmanUserId)) {
                this.EntityPM.SalesmanUserId = Tools_1.AppTool.IsNullOrEmpty(this.myConsigneeSalesmanId) ? this.EntityPM.CreatedByUserId : this.myConsigneeSalesmanId;
            }
        }
        if (this.CurrentEntity) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
                this.EntityPM.AgentReference1 = this.CurrentEntity.AgentReference;
            }
        }
    };
    SharedManifestAdditionalComponent.prototype.SetCountryECOnFinish = function () {
        if (this.FromPortList != null) {
            this.EntityPM.FromCountryId = this.FromPortList.CountryId;
            this.EntityPM.FromCountryIsEC = this.FromPortList.CountryEC;
        }
        if (this.ToPortList != null) {
            this.EntityPM.ToCountryId = this.ToPortList.CountryId;
            this.EntityPM.ToCountryIsEC = this.ToPortList.CountryEC;
        }
    };
    SharedManifestAdditionalComponent.prototype.AddPartnerClicked = function (myPartnerCode) {
        var _this = this;
        var args = new Args_1.NewEntityArgs();
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        var pathComponent = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
        if (myPartnerCode == "C") {
            args.DefaultValues = this.ConsigneeDefaultValues;
            logeWindow.Title = "New Consignee";
            logeWindow.ComponentLoaded.subscribe(function (comp) {
                logeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.ConsigneeId = comp.EntityPM.Id;
                        _this.ConsigneeName = comp.EntityPM.EnglishName;
                        _this.myConsigneeSalesmanId = comp.EntityPM.SalesmanUserId;
                    }
                });
            });
        }
        else if (myPartnerCode == "Notify1") {
            args.DefaultValues = this.Notify1DefaultValues;
            logeWindow.Title = "New Notify 1";
            logeWindow.ComponentLoaded.subscribe(function (comp) {
                logeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.Notify1Id = comp.EntityPM.Id;
                    }
                });
            });
        }
        else {
            args.DefaultValues = this.ShiperDefaultValues;
            logeWindow.Title = "New Shipper";
            logeWindow.ComponentLoaded.subscribe(function (comp) {
                logeWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.ShipperId = comp.EntityPM.Id;
                        _this.myShipperSalesmanId = comp.EntityPM.SalesmanUserId;
                    }
                });
            });
        }
        logeWindow.Width = 960;
        logeWindow.Height = 600;
        logeWindow.WindowArgs = args;
        logeWindow.Show(pathComponent);
    };
    SharedManifestAdditionalComponent.prototype.AddShipmentPickUpDeliveryPartnerClicked = function (tableName, code, entityName) {
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name.toLowerCase() === tableName.toLocaleLowerCase(); })[0];
        if (objectTable.IsNewWizard) {
            this.RunNewEntityWizard(objectTable.NewWizardComponentPath, objectTable.Name, code, entityName);
        }
    };
    SharedManifestAdditionalComponent.prototype.RunNewEntityWizard = function (newWizardComponentPath, originalTableName, code, entityName) {
        var _this = this;
        var componentPath = newWizardComponentPath;
        if (componentPath != null) {
            var ourSideShipmentPickUpDelivery = entityName == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            if (ourSideShipmentPickUpDelivery) {
                var args = new Args_1.NewEntityArgs();
                args.DefaultValues = code == "F" ? ourSideShipmentPickUpDelivery.FromPartnerDefaultValues : ourSideShipmentPickUpDelivery.ToPartnerDefaultValues;
                logWindow.WindowArgs = args;
            }
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(originalTableName));
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event, code, entityName); });
            logWindow.Show(componentPath);
        }
    };
    SharedManifestAdditionalComponent.prototype.OnNewEntityWindowClosed = function ($event, code, entity) {
        console.log($event);
        if ($event && $event != "event") {
            var ourSideShipmentPickUpDelivery = entity == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
            if (ourSideShipmentPickUpDelivery) {
                if (code == "F")
                    ourSideShipmentPickUpDelivery.FromPartnerCardId = $event;
                else
                    ourSideShipmentPickUpDelivery.ToPartnerCardId = $event;
            }
        }
    };
    SharedManifestAdditionalComponent.prototype.SelectCityCommand = function (code, entity) {
        var mySourceCountryId;
        var ourSideShipmentPickUpDelivery = entity == "Pickup" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
        if (ourSideShipmentPickUpDelivery) {
            if (code == "F")
                mySourceCountryId = ourSideShipmentPickUpDelivery.FromAddressCountryId;
            else
                mySourceCountryId = ourSideShipmentPickUpDelivery.ToAddressCountryId;
        }
        var args = new Args_2.CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                if (code == "F") {
                    ourSideShipmentPickUpDelivery.FromAddressCity = args.CityName;
                    ourSideShipmentPickUpDelivery.FromAddressCountryId = args.CountryId;
                }
                else {
                    ourSideShipmentPickUpDelivery.ToAddressCity = args.CityName;
                    ourSideShipmentPickUpDelivery.ToAddressCountryId = args.CountryId;
                }
            }
        });
    };
    SharedManifestAdditionalComponent.prototype.AddCountryClick = function (code, entity) {
        var _this = this;
        var countryCode;
        var countryName;
        var shipmentPickUpDelivery = entity == "Pickup" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;
        if (shipmentPickUpDelivery) {
            if (code == "F") {
                countryCode = shipmentPickUpDelivery.FromAddressCountryCode;
                countryName = shipmentPickUpDelivery.FromAddressCountryName;
            }
            else {
                countryCode = shipmentPickUpDelivery.ToAddressCountryCode;
                countryName = shipmentPickUpDelivery.ToAddressCountryName;
            }
        }
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("Country").then(function (response) {
            var args = new EntityArgs_1.EntityArgs();
            args.EntityPM = response;
            args.EntityPM.Code = countryCode;
            args.EntityPM.EnglishName = countryName;
            args.ObjectTableName = "Country";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("Country"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event) {
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService_1.EntityListService();
                    entityListService.getAllFromCache("Country", filters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (!myResponse.HasError && myResponse.Result) {
                                var country = myResponse.Result.filter(function (d) { return d.Id == $event; })[0];
                                if (country) {
                                    var ourSideShipmentPickUpDelivery = entity == "Pickup" ? _this.OurSideShipmentPickUp : _this.OurSideShipmentDelivery;
                                    if (ourSideShipmentPickUpDelivery) {
                                        if (code == "F") {
                                            ourSideShipmentPickUpDelivery.FromAddressCountryCode = country.Code;
                                            ourSideShipmentPickUpDelivery.FromAddressCountryName = country.EnglishName;
                                            ourSideShipmentPickUpDelivery.FromAddressCountryId = country.Id;
                                        }
                                        else {
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
    };
    SharedManifestAdditionalComponent.prototype.CreateButtonClicked = function () {
        this.OnCreate();
        // this.CurrentSession.CloseCurrentWindow();
    };
    SharedManifestAdditionalComponent.prototype.AddPackageType = function (item) {
        var _this = this;
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("PackageType").then(function (response) {
            var packagetype = _this.AgentSideData.PackageTypes.filter(function (d) { return d.Code == item.ComputedLocalName; })[0];
            var args = new EntityArgs_1.EntityArgs();
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
                if (packagetype.Volume && packagetype.Volume > 0)
                    args.EntityPM.Volume = packagetype.Volume;
                if (packagetype.TEU && packagetype.TEU > 0)
                    args.EntityPM.TEU = packagetype.TEU;
                if (packagetype.ContainerSize && packagetype.ContainerSize > 0)
                    args.EntityPM.ContainerSize = packagetype.ContainerSize;
            }
            args.ObjectTableName = "PackageType";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("PackageType"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event) {
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService_1.EntityListService();
                    entityListService.getAllFromCache("PackageType", filters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (!myResponse.HasError && myResponse.Result) {
                                var packageType = myResponse.Result.filter(function (d) { return d.Id == $event; })[0];
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
    };
    SharedManifestAdditionalComponent.prototype.AddVesselClcik = function (fieldName) {
        var _this = this;
        var defultCode = "";
        var defultName = "";
        if (fieldName == "Transshipment1Vessel") {
            defultCode = this.ManifestSL.Transshipment1VesselCode;
            defultName = this.ManifestSL.Transshipment1VesselName;
        }
        else if (fieldName == "Transshipment2Vessel") {
            defultCode = this.ManifestSL.Transshipment2VesselCode;
            defultName = this.ManifestSL.Transshipment2VesselName;
        }
        else if (fieldName == "Transshipment3Vessel") {
            defultCode = this.ManifestSL.Transshipment3VesselCode;
            defultName = this.ManifestSL.Transshipment3VesselName;
        }
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("Vessel").then(function (response) {
            var args = new EntityArgs_1.EntityArgs();
            args.EntityPM = response;
            args.EntityPM.EnglishName = args.EntityPM.LocalName = defultName;
            args.EntityPM.Code = defultCode;
            args.ObjectTableName = "Vessel";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("Vessel"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event) {
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService_1.EntityListService();
                    entityListService.getAllFromCache("Vessel", filters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (!myResponse.HasError && myResponse.Result) {
                                var vessel = myResponse.Result.filter(function (d) { return d.Id == $event; })[0];
                                if (vessel) {
                                    if (fieldName == "Transshipment1Vessel") {
                                        _this.Transshipment1VesselId = vessel.Id;
                                    }
                                    else if (fieldName == "Transshipment2Vessel") {
                                        _this.Transshipment2VesselId = vessel.Id;
                                    }
                                    else if (fieldName == "Transshipment3Vessel") {
                                        _this.Transshipment3VesselId = vessel.Id;
                                    }
                                }
                            }
                        });
                    });
                }
            });
        });
    };
    SharedManifestAdditionalComponent.prototype.AddCarrierClcik = function (fieldName) {
        var _this = this;
        var objecttableName = "";
        var defultCode = "";
        var defultName = "";
        if (fieldName == "Transshipment1Carrier") {
            defultCode = this.ManifestSL.Transshipment1CarrierCode;
            defultName = this.ManifestSL.Transshipment1CarrierName;
        }
        else if (fieldName == "Transshipment2Carrier") {
            defultCode = this.ManifestSL.Transshipment2CarrierCode;
            defultName = this.ManifestSL.Transshipment2CarrierName;
        }
        else if (fieldName == "Transshipment3Carrier") {
            defultCode = this.ManifestSL.Transshipment3CarrierCode;
            defultName = this.ManifestSL.Transshipment3CarrierName;
        }
        switch (this.ManifestSL.TransportModeId) {
            case "A": {
                objecttableName = "Airline";
                break;
            }
            case "O": {
                objecttableName = "ShippingLine";
                break;
            }
            case "I": {
                objecttableName = "Trucker";
                break;
            }
        }
        var defaultValues = defultCode + "^" + defultName;
        var path = "./Common/Components/Partners/NewEntity/New" + objecttableName + "Component";
        var title = "Add " + objecttableName;
        if (objecttableName == "ShippingLine") {
            title = "New Shipping Line";
        }
        this._entityResourceService.getEntityResourceByTableName(objecttableName, 0).subscribe(function (response) {
            var windowArgs = {};
            windowArgs.DefaultValues = defaultValues;
            windowArgs.RequestPage = "SharedManifest";
            var windowTitle = title; //"New Shipping Line";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event)
                    _this.RefrshCarrier(fieldName, $event, objecttableName);
            });
        });
    };
    SharedManifestAdditionalComponent.prototype.RefrshCarrier = function (fieldName, value, objectTableName) {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        filters.GetAll = true;
        filters.ForceCacheRefresh = true;
        var entityListService = new EntityListService_1.EntityListService();
        entityListService.getAllFromCache(objectTableName, filters).then(function (res) {
            res.subscribe(function (myResponse) {
                if (!myResponse.HasError && myResponse.Result) {
                    var Carrier = myResponse.Result.filter(function (d) { return d.Id == value; })[0];
                    if (Carrier) {
                        if (fieldName == "Transshipment1Carrier") {
                            _this.Transshipment1CarrierId = Carrier.Id;
                        }
                        else if (fieldName == "Transshipment2Carrier") {
                            _this.Transshipment2CarrierId = Carrier.Id;
                        }
                        else if (fieldName == "Transshipment3Carrier") {
                            _this.Transshipment3CarrierId = Carrier.Id;
                        }
                    }
                }
            });
        });
    };
    SharedManifestAdditionalComponent.prototype.AddEntityClcik = function (objectTableName) {
        var _this = this;
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(objectTableName).then(function (response) {
            var args = new EntityArgs_1.EntityArgs();
            args.EntityPM = response;
            if (objectTableName == "Incoterm") {
                args.EntityPM.Code = _this.AgentSideData.IncotermCode;
                args.EntityPM.Name = _this.AgentSideData.IncotermName;
            }
            else if (objectTableName == "MoveType") {
                args.EntityPM.Code = _this.AgentSideData.MoveTypeCode;
                args.EntityPM.MoveTypeEnglishName = args.EntityPM.MoveTypeLocalName = _this.AgentSideData.MoveTypeName;
                args.EntityPM.TransportModeId = _this.AgentSideData.MoveTypeTransportModeId;
                if (!Tools_1.AppTool.IsNullOrEmpty(args.EntityPM.TransportModeId)) {
                    if (args.EntityPM.TransportModeId == "O")
                        args.EntityPM.IsOcean = true;
                    else if (args.EntityPM.TransportModeId == "I")
                        args.EntityPM.IsInland = true;
                    else if (args.EntityPM.TransportModeId == "A")
                        args.EntityPM.IsAir = true;
                }
            }
            args.ObjectTableName = objectTableName;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(objectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.Show(componentPath);
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event) {
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    var entityListService = new EntityListService_1.EntityListService();
                    entityListService.getAllFromCache(objectTableName, filters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (!myResponse.HasError && myResponse.Result) {
                                var entity = myResponse.Result.filter(function (d) { return d.Id == $event; })[0];
                                if (entity) {
                                    if (objectTableName == "Incoterm") {
                                        _this.IncotermCode = entity.Code;
                                        _this.IncotermId = entity.Id;
                                    }
                                    else if (objectTableName == "MoveType") {
                                        _this.MoveTypeCode = entity.Code;
                                        _this.MoveTypeId = entity.Id;
                                    }
                                }
                            }
                        });
                    });
                }
            });
        });
    };
    SharedManifestAdditionalComponent.prototype.AddValueOfGoodsCurrencyClcik = function () {
        var _this = this;
        var componentPath = "./Common/Components/Maintenance/Currency/NewCurrencyComponent";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("Currency"));
        logWindow.Title = windowTitle;
        logWindow.Show(componentPath);
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                var filters = new ApiQueryFilters_1.ApiQueryFilters();
                filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                filters.GetAll = true;
                filters.ForceCacheRefresh = true;
                var entityListService = new EntityListService_1.EntityListService();
                entityListService.getAllFromCache("Currency", filters).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (!myResponse.HasError && myResponse.Result) {
                            var entity = myResponse.Result.filter(function (d) { return d.Id == $event; })[0];
                            if (entity) {
                                _this.ValueOfGoodsCurrencyCode = entity.Code;
                                _this.ValueOfGoodsCurrencyId = entity.Id;
                            }
                        }
                    });
                });
            }
        });
    };
    SharedManifestAdditionalComponent.prototype.PackageTypeValueChange = function (value, item) {
        if (value) {
            item.Id = value.Id;
            item.Code = value.Code;
            item.EnglishName = value.EnglishName;
            //item.IsContainer = value.IsContainer;
        }
        else {
            item.Id = "";
            item.Code = "";
            item.EnglishName = "";
        }
    };
    SharedManifestAdditionalComponent.prototype.ButtonHideOtherPartnersAreaClicked = function () {
        this.HideOtherPartnersArea = !this.HideOtherPartnersArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHidePickupDetailsAreaClicked = function () {
        this.HidePickupDetailsArea = !this.HidePickupDetailsArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHideDeliveryDetailsAreaClicked = function () {
        this.HideDeliveryDetailsArea = !this.HideDeliveryDetailsArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHideGeneralDetailsAreaClicked = function () {
        this.HideGeneralDetailsArea = !this.HideGeneralDetailsArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHidePackageTypeAreaClicked = function () {
        this.HidePackageTypeArea = !this.HidePackageTypeArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHideAddtionalClicked = function () {
        this.HideAddtionalArea = !this.HideAddtionalArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHideAgentSideDataAreaClicked = function () {
        this.HideAgentSideDataArea = !this.HideAgentSideDataArea;
    };
    SharedManifestAdditionalComponent.prototype.imgHideOurSideDataAreaClicked = function () {
        this.HideOurSideDataArea = !this.HideOurSideDataArea;
    };
    SharedManifestAdditionalComponent.prototype.ButtonHideTransShipmentsDetailsAreaClicked = function () {
        this.HideTransShipmentsDetailsArea = !this.HideTransShipmentsDetailsArea;
    };
    SharedManifestAdditionalComponent.prototype.OnCreate = function () {
        var _this = this;
        this.SetDataOnFinish();
        this.CurrentSession.StartBusyIndicator("Creating...");
        var validator = new ShipmentValidator_1.ShipmentValidator();
        this.ValidationErrorsList = validator.Validate(this.EntityPM);
        var IsValidPackageTranslation = true;
        if (!this.IsRemovePackageAreaFromScreen) {
            if (this.OurAgentPackagesType && this.OurAgentPackagesType.length > 0) {
                var item = this.OurAgentPackagesType.filter(function (d) { return Tools_1.AppTool.IsNullOrEmpty(d.Id); })[0];
                if (item) {
                    this.ValidationErrorsList.push("Please fill shipment packages translations ");
                }
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ShipperId")));
        }
        if (!this.IsHideCarrier && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.MainCarriageCarrierId")));
        }
        if (!this.IsHideTransshipment1Carrier && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Transshipment1CarrierId")));
        }
        if (!this.IsHideTransshipment2Carrier && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Transshipment2CarrierId")));
        }
        if (!this.IsHideTransshipment3Carrier && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3CarrierId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Transshipment3CarrierId")));
        }
        if (!this.IsHideMainCarriageVessel && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageVesselId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.MainCarriageVesselId")));
        }
        if (!this.IsHideTransshipment1Vessel && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1VesselId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Transshipment1VesselId")));
        }
        if (!this.IsHideTransshipment2Vessel && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2VesselId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Transshipment2VesselId")));
        }
        if (!this.IsHideTransshipment3Vessel && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3VesselId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Transshipment3VesselId")));
        }
        if (!this.IsHideMainInterline && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.InterlineId")));
        }
        if (!this.IsHideIncoterm && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IncotermId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.IncotermId")));
        }
        if (!this.IsHideMoveType && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MoveTypeId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.MoveTypeId")));
        }
        if (!this.IsHideValueOfGoodsCurrency && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ValueOfGoodsCurrencyId)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ValueOfGoodsCurrencyId")));
        }
        if (this.AgentSideData.Notify1 != null && !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Code) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1Id)) {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Notify1Id")));
        }
        //if (this.AgentSideData.Notify2 != null && !AppTool.IsNullOrEmpty(this.AgentSideData.Notify2.Code) &&  AppTool.IsNullOrEmpty(this.EntityPM.Notify2Id)) {
        //    var message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //    this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.Notify2Id")));
        //}
        if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId) && this.EntityPM.ShipmentLevelCode == "C") {
            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.ValidationErrorsList.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
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
        if (!this.IsConsolShipment && this.AgentSideData.Consignee && !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Consignee.Code) && !Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeCode)) {
            this.InsertAndUpdateTransLation(this.AgentSideData.Consignee.Code, "ConsigneeCard", this.ConsigneeCode);
        }
        //Notify1
        if (this.AgentSideData.Notify1 && !Tools_1.AppTool.IsNullOrEmpty(this.AgentSideData.Notify1.Code) && !Tools_1.AppTool.IsNullOrEmpty(this.Notify1Code)) {
            this.InsertAndUpdateTransLation(this.AgentSideData.Notify1.Code, "Notify1Card", this.Notify1Code);
        }
        //PackageTypes
        if (this.AgentSideData.PackageTypes && this.AgentSideData.PackageTypes.filter((function (d) { return d.AddedManually; })).length > 0) {
            this.AgentSideData.PackageTypes.filter((function (d) { return d.AddedManually; })).forEach(function (PackageType) {
                var newTranslatePackage = _this.OurAgentPackagesType.filter(function (d) { return d.ComputedLocalName == PackageType.Code; })[0];
                if (newTranslatePackage) {
                    var trans = _this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == PackageType.Code && f.ObjectTableName == "Package"; })[0];
                    if (!trans) {
                        var newTrans = new SharedManifestTranslationPM_1.SharedManifestTranslationPM();
                        newTrans.ChangeSetOp = "Insert";
                        newTrans.AgentCode = PackageType.Code;
                        newTrans.MyCode = newTranslatePackage.Code;
                        newTrans.ObjectTableName = "Package";
                        newTrans.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                        _this.CurrentEntity.SharedManifestTranslations.push(newTrans);
                    }
                    else {
                        if (trans.ChangeSetOp != "Insert") {
                            trans.ChangeSetOp = "Update";
                        }
                        trans.MyCode = newTranslatePackage.Code;
                    }
                    _this.EntityPM.ShipmentPackages.filter(function (d) { return d.PackageTypeCode == PackageType.Code && d.IsPackageAddedManually; }).forEach(function (item) {
                        item.PackageTypeId = newTranslatePackage.Id;
                        item.PackageTypeCode = newTranslatePackage.Code;
                        item.PackageTypeName = newTranslatePackage.EnglishName;
                        item.IsContainer = newTranslatePackage.IsContainer;
                    });
                    _this.EntityPM.ShipmentPackages.filter(function (d) { return d.InsideShipmentPackages != null && d.InsideShipmentPackages.length > 0; }).forEach(function (shipmentPackage) {
                        shipmentPackage.InsideShipmentPackages.filter(function (d) { return d.PackageTypeCode == PackageType.Code && d.IsPackageAddedManually; }).forEach(function (item) {
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
        this.myShipmentPMService.insert(this.EntityPM).subscribe(function (res) {
            var shipResponse = res;
            if (!shipResponse.HasError) {
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.SharedManifestStatus)) {
                    _this.CurrentEntity.StatusCode = _this.SharedManifestStatus;
                }
                _this._agentSharedManifestPMService.update(_this.CurrentEntity).subscribe(function (response) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!response.HasError) {
                        if (_this.HouseEntity)
                            _this.HouseEntity.EntityId = shipResponse.Result.Id;
                        else {
                            _this.ManifestSL.EntityId = shipResponse.Result.Id;
                            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Accept Manifests");
                        }
                        _this.CurrentSession.CurrentWindow.Close(shipResponse.Result.Id);
                        if (!_this.HouseEntity) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Title = "Shipment Creation";
                            messageWindow.Show("Your Shipment was successfully created.");
                        }
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                if (shipResponse.ErrorsArray && shipResponse.ErrorsArray.length > 0) {
                    shipResponse.ErrorsArray.forEach(function (item) {
                        _this.ValidationErrorsList.push(item);
                    });
                }
            }
        });
    };
    SharedManifestAdditionalComponent.prototype.AddShipmentPickUpDeliveryPM = function (pickUpDeliveryTypeCode) {
        if (this.ManifestSL && !this.IsHouseShipment) {
            var agentshipmentPickUpDelivery = pickUpDeliveryTypeCode == "PICK" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;
            if (agentshipmentPickUpDelivery) {
                var shipmentPickUpDeliveryPM = pickUpDeliveryTypeCode == "PICK" ? new ShipmentPickUpPM_1.ShipmentPickUpPM(null) : new ShipmentDeliveryPM_1.ShipmentDeliveryPM(null);
                var ourSideShipmentPickUpDelivery = pickUpDeliveryTypeCode == "PICK" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
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
                    if (Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromPartnerCardId)) {
                        this.ValidationErrorsList.push(entityName + " From Partner field is required");
                    }
                }
                else if (shipmentPickUpDeliveryPM.PickUpDeliveryFromTypeCode == "CASL") {
                    shipmentPickUpDeliveryPM.FromAddressZipCode = ourSideShipmentPickUpDelivery.FromAddressZipCode;
                    shipmentPickUpDeliveryPM.FromAddressCity = ourSideShipmentPickUpDelivery.FromAddressCity;
                    shipmentPickUpDeliveryPM.FromAddressCountryId = ourSideShipmentPickUpDelivery.FromAddressCountryId;
                    shipmentPickUpDeliveryPM.FromAddressCountryCode = ourSideShipmentPickUpDelivery.FromAddressCountryCode;
                    if (Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromAddressCountryId)) {
                        this.ValidationErrorsList.push(entityName + " From Country field is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromAddressZipCode) && Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.FromAddressCity)) {
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
                    if (Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToPartnerCardId)) {
                        this.ValidationErrorsList.push(entityName + " To Partner field is required");
                    }
                }
                else if (shipmentPickUpDeliveryPM.PickUpDeliveryToTypeCode == "CASL") {
                    shipmentPickUpDeliveryPM.ToAddressZipCode = ourSideShipmentPickUpDelivery.ToAddressZipCode;
                    shipmentPickUpDeliveryPM.ToAddressCity = ourSideShipmentPickUpDelivery.ToAddressCity;
                    shipmentPickUpDeliveryPM.ToAddressCountryId = ourSideShipmentPickUpDelivery.ToAddressCountryId;
                    shipmentPickUpDeliveryPM.ToAddressCountryCode = ourSideShipmentPickUpDelivery.ToAddressCountryCode;
                    if (Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToAddressCountryId)) {
                        this.ValidationErrorsList.push(entityName + " To Country field is required");
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToAddressZipCode) && Tools_1.AppTool.IsNullOrEmpty(shipmentPickUpDeliveryPM.ToAddressCity)) {
                        this.ValidationErrorsList.push(entityName + " To City or To Zip Code field is required");
                    }
                }
                if (this.ValidationErrorsList.length == 0) {
                    if (pickUpDeliveryTypeCode == "PICK") {
                        this.EntityPM.ShipmentPickUps = [];
                        this.EntityPM.AddPickUp(shipmentPickUpDeliveryPM);
                    }
                    else {
                        this.EntityPM.ShipmentDeliveries = [];
                        this.EntityPM.AddDelivery(shipmentPickUpDeliveryPM);
                    }
                }
            }
        }
    };
    SharedManifestAdditionalComponent.prototype.AddShipmentPickUpDeliveryTransLation = function (pickUpDeliveryTypeCode) {
        if (this.ManifestSL && !this.IsHouseShipment) {
            var agentshipmentPickUpDelivery = pickUpDeliveryTypeCode == "PICK" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;
            if (agentshipmentPickUpDelivery) {
                var ourSideShipmentPickUpDelivery = pickUpDeliveryTypeCode == "PICK" ? this.OurSideShipmentPickUp : this.OurSideShipmentDelivery;
                var entityName = pickUpDeliveryTypeCode == "PICK" ? "PickUp" : "Delivery";
                if (ourSideShipmentPickUpDelivery) {
                    if (ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode == "PART") {
                        if (ourSideShipmentPickUpDelivery.FromPartnerCardCode)
                            this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.FromPartner.Code, ("Shipment" + entityName + "FromPartner"), ourSideShipmentPickUpDelivery.FromPartnerCardCode);
                    }
                    else if (ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode == "CASL") {
                        if (ourSideShipmentPickUpDelivery.FromAddressCountryCode)
                            this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.FromAddressCountryCode, ("Shipment" + entityName + "FromCountry"), ourSideShipmentPickUpDelivery.FromAddressCountryCode);
                    }
                    if (ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode == "PART") {
                        if (ourSideShipmentPickUpDelivery.ToPartnerCardCode)
                            this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.ToPartner.Code, ("Shipment" + entityName + "ToPartner"), ourSideShipmentPickUpDelivery.ToPartnerCardCode);
                    }
                    else if (ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode == "CASL") {
                        if (ourSideShipmentPickUpDelivery.ToAddressCountryCode)
                            this.InsertAndUpdateTransLation(agentshipmentPickUpDelivery.ToAddressCountryCode, ("Shipment" + entityName + "ToCountry"), ourSideShipmentPickUpDelivery.ToAddressCountryCode);
                    }
                }
            }
        }
    };
    SharedManifestAdditionalComponent.prototype.BuildOurPickUpDeliverySide = function (pickUpDeliveryTypeCode) {
        var _this = this;
        var isNoFound = false;
        if (this.ManifestSL && !this.IsHouseShipment) {
            var agentshipmentPickUpDelivery = pickUpDeliveryTypeCode == "PICK" ? this.ManifestSL.ShipmentPickUp : this.ManifestSL.ShipmentDelivery;
            if (agentshipmentPickUpDelivery) {
                var ourSideShipmentPickUpDelivery = pickUpDeliveryTypeCode == "PICK" ? this.OurSideShipmentPickUp = new ShipmentPickUpDeliverySL_1.ShipmentPickUpDeliverySL() : this.OurSideShipmentDelivery = new ShipmentPickUpDeliverySL_1.ShipmentPickUpDeliverySL();
                var entityName = pickUpDeliveryTypeCode == "PICK" ? "PickUp" : "Delivery";
                ourSideShipmentPickUpDelivery.PickUpDeliveryFromTypeCode = agentshipmentPickUpDelivery.PickUpDeliveryFromTypeCode;
                ourSideShipmentPickUpDelivery.PickUpDeliveryToTypeCode = agentshipmentPickUpDelivery.PickUpDeliveryToTypeCode;
                ourSideShipmentPickUpDelivery.MainCarriageATA = agentshipmentPickUpDelivery.MainCarriageATA;
                ourSideShipmentPickUpDelivery.MainCarriageATD = agentshipmentPickUpDelivery.MainCarriageATD;
                ourSideShipmentPickUpDelivery.MainCarriageETA = agentshipmentPickUpDelivery.MainCarriageETA;
                ourSideShipmentPickUpDelivery.MainCarriageETD = agentshipmentPickUpDelivery.MainCarriageETD;
                ourSideShipmentPickUpDelivery.SelectedTransportMode = ourSideShipmentPickUpDelivery.TransportModeLists.filter(function (d) { return d.Code == agentshipmentPickUpDelivery.TransportModeCode; })[0];
                var isNeedTranslations = false;
                //From
                if (agentshipmentPickUpDelivery.FromPort) {
                    var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    this.myPortListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            var fromPort = list.filter(function (d) { return d.Code == agentshipmentPickUpDelivery.FromPort.Code; })[0];
                            if (fromPort) {
                                ourSideShipmentPickUpDelivery.FromPortId = fromPort.Id;
                                _this.SetPickUpDeliveryAreaVisibility(pickUpDeliveryTypeCode);
                            }
                        }
                        if (pickUpDeliveryTypeCode == "PICK")
                            _this.IsLoadedFromPickUpTranslation = true;
                        else
                            _this.IsLoadedFromDeliveryTranslation = true;
                        _this.StopBusyIndicator();
                    });
                }
                else if (agentshipmentPickUpDelivery.FromPartner) {
                    // Shipper Translation
                    var shipmentPickUpFromPartnerTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == agentshipmentPickUpDelivery.FromPartner.Code && f.ObjectTableName == "Shipment" + entityName + "FromPartner"; })[0];
                    if (shipmentPickUpFromPartnerTranslation) {
                        this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(shipmentPickUpFromPartnerTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {
                                    ourSideShipmentPickUpDelivery.FromPartnerCardId = myResult;
                                    _this.SetPickUpDeliveryAreaVisibility(pickUpDeliveryTypeCode);
                                }
                            }
                            if (pickUpDeliveryTypeCode == "PICK")
                                _this.IsLoadedFromPickUpTranslation = true;
                            else
                                _this.IsLoadedFromDeliveryTranslation = true;
                            _this.StopBusyIndicator();
                        });
                    }
                    else {
                        if (pickUpDeliveryTypeCode == "PICK")
                            this.IsLoadedFromPickUpTranslation = true;
                        else
                            this.IsLoadedFromDeliveryTranslation = true;
                        this.StopBusyIndicator();
                    }
                    var englishName = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.EnglishName) ? agentshipmentPickUpDelivery.FromPartner.EnglishName : "";
                    var Address1 = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.Address1) ? agentshipmentPickUpDelivery.FromPartner.Address1 : "";
                    var Address2 = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.Address2) ? agentshipmentPickUpDelivery.FromPartner.Address2 : "";
                    var city = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.FromPartner.City) ? agentshipmentPickUpDelivery.FromPartner.City : "";
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues = englishName + "^";
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (Address1 + "^");
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (Address2 + "^");
                    ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (city + "^");
                    var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    apiQueryFilters.addAdditionalFilter("Code", agentshipmentPickUpDelivery.FromPartner.CountryCode, null, null, "Equals", true, true, true, "Text");
                    this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            var CountryList = list.filter(function (d) { return d.Code == agentshipmentPickUpDelivery.FromPartner.CountryCode && !d.InActive; })[0];
                            if (CountryList) {
                                ourSideShipmentPickUpDelivery.FromPartnerDefaultValues += (CountryList.Id);
                            }
                        }
                    });
                }
                else if (agentshipmentPickUpDelivery.PickUpDeliveryFromTypeCode == "CASL") {
                    if (agentshipmentPickUpDelivery.FromAddressCountryCode) {
                        var shipmentPickUpFromCountryTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == agentshipmentPickUpDelivery.FromAddressCountryCode && f.ObjectTableName == "Shipment" + entityName + "FromCountry"; })[0];
                        if (shipmentPickUpFromCountryTranslation) {
                            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                            apiQueryFilters.GetAll = true;
                            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                            apiQueryFilters.addAdditionalFilter("Code", shipmentPickUpFromCountryTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                            this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    var list = myResponse.Result;
                                    var CountryList = list.filter(function (d) { return d.Code == shipmentPickUpFromCountryTranslation.MyCode && !d.InActive; })[0];
                                    if (CountryList) {
                                        ourSideShipmentPickUpDelivery.FromAddressCountryId = CountryList.Id;
                                        ourSideShipmentPickUpDelivery.FromAddressCountryName = CountryList.EnglishName;
                                        ourSideShipmentPickUpDelivery.FromAddressCountryCode = CountryList.Code;
                                        if (!agentshipmentPickUpDelivery.FromAddressCity)
                                            _this.SetPickUpDeliveryAreaVisibility(pickUpDeliveryTypeCode);
                                    }
                                }
                                if (pickUpDeliveryTypeCode == "PICK")
                                    _this.IsLoadedFromPickUpTranslation = true;
                                else
                                    _this.IsLoadedFromDeliveryTranslation = true;
                                _this.StopBusyIndicator();
                            });
                        }
                        else {
                            if (pickUpDeliveryTypeCode == "PICK")
                                this.IsLoadedFromPickUpTranslation = true;
                            else
                                this.IsLoadedFromDeliveryTranslation = true;
                            this.StopBusyIndicator();
                        }
                    }
                    ourSideShipmentPickUpDelivery.FromAddressZipCode = agentshipmentPickUpDelivery.FromAddressZipCode;
                    ourSideShipmentPickUpDelivery.FromAddressCity = agentshipmentPickUpDelivery.FromAddressCity;
                }
                else {
                    if (pickUpDeliveryTypeCode == "PICK")
                        this.IsLoadedFromPickUpTranslation = true;
                    else
                        this.IsLoadedFromDeliveryTranslation = true;
                }
                //To
                if (agentshipmentPickUpDelivery.ToPort) {
                    var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    this.myPortListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            var ToPort = list.filter(function (d) { return d.Code == agentshipmentPickUpDelivery.ToPort.Code; })[0];
                            if (ToPort) {
                                _this.SetPickUpDeliveryAreaVisibility(pickUpDeliveryTypeCode, false);
                                ourSideShipmentPickUpDelivery.ToPortId = ToPort.Id;
                            }
                        }
                        if (pickUpDeliveryTypeCode == "PICK")
                            _this.IsLoadedToPickUpTranslation = true;
                        else
                            _this.IsLoadedToDeliveryTranslation = true;
                        _this.StopBusyIndicator();
                    });
                }
                else if (agentshipmentPickUpDelivery.ToPartner) {
                    // Shipper Translation
                    var shipmentPickUpToPartnerTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == agentshipmentPickUpDelivery.ToPartner.Code && f.ObjectTableName == "Shipment" + entityName + "ToPartner"; })[0];
                    if (shipmentPickUpToPartnerTranslation) {
                        this._sharedAgentManifestService.getSharedAgentManifestTransLateIdByCode(shipmentPickUpToPartnerTranslation.MyCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                            var pmResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {
                                    ourSideShipmentPickUpDelivery.ToPartnerCardId = myResult;
                                    _this.SetPickUpDeliveryAreaVisibility(pickUpDeliveryTypeCode, false);
                                }
                            }
                            if (pickUpDeliveryTypeCode == "PICK")
                                _this.IsLoadedToPickUpTranslation = true;
                            else
                                _this.IsLoadedToDeliveryTranslation = true;
                            _this.StopBusyIndicator();
                        });
                    }
                    else {
                        if (pickUpDeliveryTypeCode == "PICK")
                            this.IsLoadedToPickUpTranslation = true;
                        else
                            this.IsLoadedToDeliveryTranslation = true;
                        this.StopBusyIndicator();
                    }
                    var englishName = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.EnglishName) ? agentshipmentPickUpDelivery.ToPartner.EnglishName : "";
                    var Address1 = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.Address1) ? agentshipmentPickUpDelivery.ToPartner.Address1 : "";
                    var Address2 = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.Address2) ? agentshipmentPickUpDelivery.ToPartner.Address2 : "";
                    var city = !Tools_1.AppTool.IsNullOrEmpty(agentshipmentPickUpDelivery.ToPartner.City) ? agentshipmentPickUpDelivery.ToPartner.City : "";
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues = englishName + "^";
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (Address1 + "^");
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (Address2 + "^");
                    ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (city + "^");
                    var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    apiQueryFilters.GetAll = true;
                    apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    apiQueryFilters.addAdditionalFilter("Code", agentshipmentPickUpDelivery.ToPartner.CountryCode, null, null, "Equals", true, true, true, "Text");
                    this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            var CountryList = list.filter(function (d) { return d.Code == agentshipmentPickUpDelivery.ToPartner.CountryCode && !d.InActive; })[0];
                            if (CountryList) {
                                ourSideShipmentPickUpDelivery.ToPartnerDefaultValues += (CountryList.Id);
                            }
                        }
                    });
                }
                else if (agentshipmentPickUpDelivery.PickUpDeliveryToTypeCode == "CASL") {
                    if (agentshipmentPickUpDelivery.ToAddressCity)
                        isNeedTranslations = true;
                    if (agentshipmentPickUpDelivery.ToAddressCountryCode) {
                        var shipmentPickUpToCountryTranslation = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == agentshipmentPickUpDelivery.ToAddressCountryCode && f.ObjectTableName == "Shipment" + entityName + "ToCountry"; })[0];
                        if (shipmentPickUpToCountryTranslation) {
                            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
                            apiQueryFilters.GetAll = true;
                            apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                            apiQueryFilters.addAdditionalFilter("Code", shipmentPickUpToCountryTranslation.MyCode, null, null, "Equals", true, true, true, "Text");
                            this.countryListService.getAllFromCache(apiQueryFilters).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    var list = myResponse.Result;
                                    var CountryList = list.filter(function (d) { return d.Code == shipmentPickUpToCountryTranslation.MyCode && !d.InActive; })[0];
                                    if (CountryList) {
                                        ourSideShipmentPickUpDelivery.ToAddressCountryId = CountryList.Id;
                                        ourSideShipmentPickUpDelivery.ToAddressCountryName = CountryList.EnglishName;
                                        ourSideShipmentPickUpDelivery.ToAddressCountryCode = CountryList.Code;
                                        if (!agentshipmentPickUpDelivery.ToAddressCity)
                                            _this.SetPickUpDeliveryAreaVisibility(pickUpDeliveryTypeCode, false);
                                    }
                                }
                                if (pickUpDeliveryTypeCode == "PICK")
                                    _this.IsLoadedToPickUpTranslation = true;
                                else
                                    _this.IsLoadedToDeliveryTranslation = true;
                                _this.StopBusyIndicator();
                            });
                        }
                        else {
                            if (pickUpDeliveryTypeCode == "PICK")
                                this.IsLoadedToPickUpTranslation = true;
                            else
                                this.IsLoadedToDeliveryTranslation = true;
                            this.StopBusyIndicator();
                        }
                    }
                    ourSideShipmentPickUpDelivery.ToAddressZipCode = agentshipmentPickUpDelivery.ToAddressZipCode;
                    ourSideShipmentPickUpDelivery.ToAddressCity = agentshipmentPickUpDelivery.ToAddressCity;
                }
                else {
                    if (pickUpDeliveryTypeCode == "PICK")
                        this.IsLoadedToPickUpTranslation = true;
                    else
                        this.IsLoadedToDeliveryTranslation = true;
                }
            }
            else
                isNoFound = true;
        }
        else
            isNoFound = true;
        if (isNoFound) {
            if (pickUpDeliveryTypeCode == "PICK")
                this.IsLoadedFromPickUpTranslation = true;
            else
                this.IsLoadedFromDeliveryTranslation = true;
            if (pickUpDeliveryTypeCode == "PICK")
                this.IsLoadedToPickUpTranslation = true;
            else
                this.IsLoadedToDeliveryTranslation = true;
            if (pickUpDeliveryTypeCode == "PICK")
                this.HidePickupDetailsArea = true;
            else
                this.HideDeliveryDetailsArea = true;
            if (pickUpDeliveryTypeCode == "PICK")
                this.IsNoPickupDetailsFound = true;
            else
                this.IsNoDeliveryDetailsFound = true;
            this.StopBusyIndicator();
        }
    };
    SharedManifestAdditionalComponent.prototype.SetPickUpDeliveryAreaVisibility = function (pickUpDeliveryTypeCode, isFrom) {
        if (isFrom === void 0) { isFrom = true; }
        if (pickUpDeliveryTypeCode == "PICK") {
            if (isFrom)
                this.IsHaveFromPickUpTranslation = true;
            else
                this.IsHaveToPickUpTranslation = true;
        }
        else {
            if (isFrom)
                this.IsHaveFromDeliveryTranslation = true;
            else
                this.IsHaveToDeliveryTranslation = true;
        }
    };
    SharedManifestAdditionalComponent.prototype.InsertAndUpdateTransLation = function (agentCode, tableName, myCode) {
        var trans = this.CurrentEntity.SharedManifestTranslations.filter(function (f) { return f.AgentCode == agentCode && f.ObjectTableName == tableName; })[0];
        if (!trans) {
            var newTrans = new SharedManifestTranslationPM_1.SharedManifestTranslationPM();
            newTrans.ChangeSetOp = "Insert";
            newTrans.AgentCode = agentCode;
            newTrans.MyCode = myCode;
            newTrans.ObjectTableName = tableName;
            newTrans.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.CurrentEntity.SharedManifestTranslations.push(newTrans);
        }
        else {
            trans.ChangeSetOp = "Update";
            trans.MyCode = myCode;
        }
    };
    SharedManifestAdditionalComponent.prototype.RunComponent = function () {
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
    };
    SharedManifestAdditionalComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        if (this.IsLoadAdditionalScreen) {
            var myGeneratedComponentLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == "GECO"; })[0];
            if (myGeneratedComponentLocation != null) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', myGeneratedComponentLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    _this.GeneratedComponent = cmpRef.instance;
                    var screenCode = "NewShipment";
                    cmpRef.instance.LabelWidth = 110;
                    cmpRef.instance.Run(_this.EntityPM, "Shipment", "SharedManifestAdditionalScreen");
                    cmpRef.instance.LoadCompleted.subscribe(function (s) {
                    });
                });
                this.IsLoadAdditionalScreen = false;
            }
        }
        if (!this.IsLoadSharedManifestheaderScreen) {
            var mySharedManifestHeaderLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == "SHCO"; })[0];
            if (mySharedManifestHeaderLocation != null) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent', mySharedManifestHeaderLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.Run(_this.CurrentEntity, _this.AgentSharedManifestList);
                });
                this.IsLoadSharedManifestheaderScreen = true;
            }
        }
    };
    SharedManifestAdditionalComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    SharedManifestAdditionalComponent.prototype.StopBusyIndicator = function () {
        if (this.IsLoadedCarrierTranslation && this.IsLoadedIncotermTranslation && this.IsLoadedShipperTranslation && this.IsLoadedConsigneeTranslation && this.IsLoadedShiperDefaultValues && this.IsLoadedConsigneeDefaultValues && this.IsLoadedTransshipment1CarrierTranslation && this.IsLoadedTransshipment2CarrierTranslation && this.IsLoadedTransshipment3CarrierTranslation && this.IsLoadedMainCarriageVesselTranslation && this.IsLoadedTransshipment1VesselTranslation && this.IsLoadedTransshipment2VesselTranslation && this.IsLoadedTransshipment3VesselTranslation && this.IsLoadedMainCarriageInterlineTranslation && this.IsLoadedNotify1IdTranslation && this.IsLoadedNotify1DefaultValuesTranslation && this.IsLoadedPortsTranslation && this.IsLoadedPackageTranslation && this.IsLoadedMoveTypeTranslation && this.IsLoadedValueOfGoodsCurrencyTranslation && this.IsLoadedFromPickUpTranslation && this.IsLoadedToPickUpTranslation && this.IsLoadedFromDeliveryTranslation && this.IsLoadedToDeliveryTranslation) {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if ((!Tools_1.AppTool.IsNullOrEmpty(this.IncotermId) || this.IsHideIncoterm) && (!Tools_1.AppTool.IsNullOrEmpty(this.MoveTypeId) || this.IsHideMoveType) && (!Tools_1.AppTool.IsNullOrEmpty(this.ValueOfGoodsCurrencyId) || this.IsHideValueOfGoodsCurrency)) {
                this.HideGeneralDetailsArea = true;
            }
            if ((!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1CarrierId) || this.IsHideTransshipment1Carrier) && (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2CarrierId) || this.IsHideTransshipment2Carrier) && (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment3CarrierId) || this.IsHideTransshipment3Carrier) && (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1VesselId) || this.IsHideTransshipment1Vessel) && (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2VesselId) || this.IsHideTransshipment2Vessel) && (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment3VesselId) || this.IsHideTransshipment3Vessel)) {
                this.HideTransShipmentsDetailsArea = true;
            }
            if (this.IsLoadedFromPickUpTranslation && this.IsLoadedToPickUpTranslation) {
                if (this.IsHaveFromPickUpTranslation && this.IsHaveToPickUpTranslation) {
                    this.HidePickupDetailsArea = true;
                }
            }
            if (this.IsLoadedFromDeliveryTranslation && this.IsLoadedToDeliveryTranslation) {
                if (this.IsHaveFromDeliveryTranslation && this.IsHaveToDeliveryTranslation) {
                    this.HideDeliveryDetailsArea = true;
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], SharedManifestAdditionalComponent.prototype, "AllLocations", void 0);
    SharedManifestAdditionalComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedManifestAdditionalComponent',
            templateUrl: './SharedManifestAdditionalComponent.html',
            providers: [SharedAgentManifestService_1.SharedAgentManifestService, AgentSharedManifestPMService_1.AgentSharedManifestPMService, EntityPMService_1.EntityPMService],
        }),
        __metadata("design:paramtypes", [SharedAgentManifestService_1.SharedAgentManifestService, AgentSharedManifestPMService_1.AgentSharedManifestPMService, EntityPMService_1.EntityPMService])
    ], SharedManifestAdditionalComponent);
    return SharedManifestAdditionalComponent;
}(BaseComponent_1.BaseComponent));
exports.SharedManifestAdditionalComponent = SharedManifestAdditionalComponent;
var AgentSide = /** @class */ (function (_super) {
    __extends(AgentSide, _super);
    function AgentSide(manifestSL, houseSL) {
        var _this = _super.call(this) || this;
        _this.PackageTypes = [];
        _this.Shipper = houseSL != null ? houseSL.Shipper : manifestSL.Shipper;
        _this.Consignee = houseSL != null ? houseSL.Consignee : manifestSL.Consignee;
        _this.IncotermCode = houseSL != null ? houseSL.IncotermCode : manifestSL.IncotermCode;
        _this.IncotermName = houseSL != null ? !Tools_1.AppTool.IsNullOrEmpty(houseSL.IncotermName) ? houseSL.IncotermName : houseSL.IncotermCode : !Tools_1.AppTool.IsNullOrEmpty(manifestSL.IncotermName) ? manifestSL.IncotermName : manifestSL.IncotermCode;
        _this.IncotermId = houseSL != null ? houseSL.IncotermId : manifestSL.IncotermId;
        _this.CarrierCode = houseSL != null ? houseSL.CarrierCode : manifestSL.CarrierCode;
        _this.CarrierName = houseSL != null ? !Tools_1.AppTool.IsNullOrEmpty(houseSL.CarrierName) ? houseSL.CarrierName : houseSL.CarrierCode : !Tools_1.AppTool.IsNullOrEmpty(manifestSL.CarrierName) ? manifestSL.CarrierName : manifestSL.CarrierCode;
        _this.CarrierId = houseSL != null ? houseSL.CarrierId : manifestSL.CarrierId;
        _this.FreightPrepaidCollectId = houseSL != null ? houseSL.FreightPrepaidCollectId : manifestSL.FreightPrepaidCollectId;
        _this.Notify1 = houseSL != null ? houseSL.Notify1 : manifestSL.Notify1;
        _this.Notify2 = houseSL != null ? houseSL.Notify2 : manifestSL.Notify2;
        _this.OtherPrepaidCollectId = houseSL != null ? houseSL.OtherPrepaidCollectId : manifestSL.OtherPrepaidCollectId;
        _this.AgentName = houseSL != null ? houseSL.AgentName : manifestSL.AgentName;
        _this.ValueOfGoods = houseSL != null ? houseSL.ValueOfGoods : manifestSL.ValueOfGoods;
        _this.IsDangerous = houseSL != null ? houseSL.IsDangerous : manifestSL.IsDangerous;
        _this.DescriptionOfGoods = houseSL != null ? houseSL.GeneralDescriptionOfGoods : manifestSL.GeneralDescriptionOfGoods;
        _this.MoveTypeCode = houseSL != null ? houseSL.MoveTypeCode : manifestSL.MoveTypeCode;
        _this.MoveTypeName = houseSL != null ? !Tools_1.AppTool.IsNullOrEmpty(houseSL.MoveTypeName) ? houseSL.MoveTypeName : houseSL.MoveTypeCode : !Tools_1.AppTool.IsNullOrEmpty(manifestSL.MoveTypeName) ? manifestSL.MoveTypeName : manifestSL.MoveTypeCode;
        _this.MoveTypeId = houseSL != null ? houseSL.MoveTypeId : manifestSL.MoveTypeId;
        var shipmentPackagePM = houseSL != null ? houseSL.ShipmentPackages : manifestSL.ShipmentPackages;
        _this.InterlineAddedManually = houseSL != null ? false : manifestSL.InterlineAddedManually;
        _this.MoveTypeAddedManually = houseSL != null ? houseSL.MoveTypeAddedManually : manifestSL.MoveTypeAddedManually;
        _this.IncotermAddedManually = houseSL != null ? houseSL.IncotermAddedManually : manifestSL.IncotermAddedManually;
        _this.CarrierAddedManually = houseSL != null ? houseSL.CarrierAddedManually : manifestSL.CarrierAddedManually;
        _this.Transshipment1CarrierAddedManually = houseSL != null ? false : manifestSL.Transshipment1CarrierAddedManually;
        _this.Transshipment2CarrierAddedManually = houseSL != null ? false : manifestSL.Transshipment2CarrierAddedManually;
        _this.Transshipment3CarrierAddedManually = houseSL != null ? false : manifestSL.Transshipment3CarrierAddedManually;
        _this.MainCarriageVesselAddedManually = houseSL != null ? false : manifestSL.MainCarriageVesselAddedManually;
        _this.Transshipment1VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment1VesselAddedManually;
        _this.Transshipment2VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment2VesselAddedManually;
        _this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        _this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        _this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        _this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        _this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        _this.Transshipment3VesselAddedManually = houseSL != null ? false : manifestSL.Transshipment3VesselAddedManually;
        _this.ValueOfGoodsCurrencyCode = houseSL != null ? houseSL.ValueOfGoodsCurrencyCode : manifestSL.ValueOfGoodsCurrencyCode;
        _this.ValueOfGoodsCurrencyName = houseSL != null ? !Tools_1.AppTool.IsNullOrEmpty(houseSL.ValueOfGoodsCurrencyName) ? houseSL.ValueOfGoodsCurrencyName : houseSL.ValueOfGoodsCurrencyCode : !Tools_1.AppTool.IsNullOrEmpty(manifestSL.ValueOfGoodsCurrencyName) ? manifestSL.ValueOfGoodsCurrencyName : manifestSL.ValueOfGoodsCurrencyCode;
        _this.ValueOfGoodsCurrencyId = houseSL != null ? houseSL.ValueOfGoodsCurrencyId : manifestSL.ValueOfGoodsCurrencyId;
        _this.ValueOfGoodsCurrencyAddedManually = houseSL != null ? houseSL.ValueOfGoodsCurrencyAddedManually : manifestSL.ValueOfGoodsCurrencyAddedManually;
        _this.MoveTypeTransportModeId = houseSL != null ? houseSL.MoveTypeTransportModeId : manifestSL.MoveTypeTransportModeId;
        _this.MainHarmonize = houseSL != null ? houseSL.MainHarmonize : manifestSL.MainHarmonize;
        if (shipmentPackagePM && shipmentPackagePM.length > 0) {
            _this.PackageTypes = [];
            shipmentPackagePM.forEach(function (shipmentPackage) {
                var item = _this.PackageTypes.filter(function (d) { return d.Code == shipmentPackage.PackageTypeCode; })[0];
                if (!item) {
                    var packageTypePM = new PackageTypePM_1.PackageTypePM();
                    packageTypePM.Id = shipmentPackage.PackageTypeId;
                    packageTypePM.Code = shipmentPackage.PackageTypeCode;
                    packageTypePM.EnglishName = shipmentPackage.PackageTypeName;
                    packageTypePM.IsContainer = shipmentPackage.IsContainer;
                    packageTypePM.IsAir = shipmentPackage.PackageTypeIsAir;
                    packageTypePM.IsOcean = shipmentPackage.PackageTypeIsOcean;
                    packageTypePM.IsInland = shipmentPackage.PackageTypeIsInland;
                    packageTypePM.LocalName = shipmentPackage.PackageTypeLocalName;
                    packageTypePM.Volume = shipmentPackage.PackageTypeVolume;
                    packageTypePM.TEU = shipmentPackage.TEU;
                    packageTypePM.Notes = shipmentPackage.PackageTypeNote;
                    packageTypePM.ContainerSize = shipmentPackage.ContainerSize;
                    packageTypePM.AddedManually = shipmentPackage.IsPackageAddedManually;
                    _this.PackageTypes.push(packageTypePM);
                }
            });
            shipmentPackagePM.filter(function (d) { return d.InsideShipmentPackages != null && d.InsideShipmentPackages.length > 0; }).forEach(function (shipmentPackage) {
                shipmentPackage.InsideShipmentPackages.forEach(function (insideShipmentPackages) {
                    var item = _this.PackageTypes.filter(function (d) { return d.Code == insideShipmentPackages.PackageTypeCode; })[0];
                    if (!item) {
                        var packageTypePM = new PackageTypePM_1.PackageTypePM();
                        packageTypePM.Id = insideShipmentPackages.PackageTypeId;
                        packageTypePM.Code = insideShipmentPackages.PackageTypeCode;
                        packageTypePM.EnglishName = insideShipmentPackages.PackageTypeName;
                        packageTypePM.IsContainer = insideShipmentPackages.IsContainer;
                        packageTypePM.IsAir = insideShipmentPackages.PackageTypeIsAir;
                        packageTypePM.IsOcean = insideShipmentPackages.PackageTypeIsOcean;
                        packageTypePM.IsInland = insideShipmentPackages.PackageTypeIsInland;
                        packageTypePM.LocalName = insideShipmentPackages.PackageTypeLocalName;
                        packageTypePM.Volume = insideShipmentPackages.PackageTypeVolume;
                        packageTypePM.TEU = insideShipmentPackages.TEU;
                        packageTypePM.Notes = insideShipmentPackages.PackageTypeNote;
                        packageTypePM.ContainerSize = insideShipmentPackages.ContainerSize;
                        packageTypePM.AddedManually = insideShipmentPackages.IsPackageAddedManually;
                        _this.PackageTypes.push(packageTypePM);
                    }
                });
            });
        }
        return _this;
    }
    return AgentSide;
}(BaseComponent_1.BaseComponent));
exports.AgentSide = AgentSide;
//# sourceMappingURL=SharedManifestAdditionalComponent.js.map