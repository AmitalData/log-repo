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
var ShipmentPMService_1 = require("../../Services/StandardPMs/ShipmentPMService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var PortListService_1 = require("../../../Common/Services/StandardLists/PortListService");
var Args_1 = require("../../Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_2 = require("../../../Infrastructure/Args");
var AddressPM_1 = require("../../../Common/EntityPMs/AddressPM");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_2 = require("../../Tools");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var ShipmenDirectionConvertComponent = /** @class */ (function (_super) {
    __extends(ShipmenDirectionConvertComponent, _super);
    function ShipmenDirectionConvertComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.EnabledOkButton = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CardDependencyProperty1 = "CS";
        _this.CardDependencyProperty1IsList = false;
        _this.IsCurrentInlandDomestic = false;
        _this.IsOldInlandDomestic = false;
        _this.DirectionsList = [];
        _this.IsPortsVisible = false;
        _this.ScreenOpacity = 0.7;
        _this.IsScreenEnabled = false;
        _this.isShipperMyCustomer = false;
        _this.isConsigneeMyCustomer = false;
        //Customer
        _this.CustomerDependencyProperty1 = "CS";
        _this.CustomerDependencyProperty1IsList = false;
        _this.IsCustomerRequired = false;
        //Ports
        _this.FromPortList = null;
        _this.ToPortList = null;
        _this.SessionIndex = SessionLocator_1.SessionLocator.Index;
        _this.myCardListService = new CardListService_1.CardListService();
        _this.myAddressListService = new AddressListService_1.AddressListService();
        _this.myPortListService = new PortListService_1.PortListService();
        return _this;
    }
    ShipmenDirectionConvertComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.EnabledOkButton = args.EnabledOkButton;
        this.ValidationErrorsList = args.ValidationErrorsList;
        this.oldShipmentDirection = this.EntityPM.DirectionId;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.IsOldInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        this.IsCurrentInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.CardDependencyProperty1 = "AG";
            this.CardDependencyProperty1IsList = false;
        }
        else {
            if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                this.CardDependencyProperty1 = "CS,AG";
                this.CardDependencyProperty1IsList = true;
            }
        }
        this.SetLabels();
        this.SetScreenEnabled();
        this.BuildDirectionsFilterList();
        this.Clone();
        var fromAddressId = this.EntityPM.ShipperAddressId;
        var toAddressId = this.EntityPM.ConsigneeAddressId;
        if (this.IsCurrentInlandDomestic) {
            fromAddressId = this.EntityPM.MainCarriageFromAddressId;
            toAddressId = this.EntityPM.MainCarriageToAddressId;
        }
        this.LoadAddress("S", fromAddressId);
        this.LoadAddress("C", toAddressId);
    };
    ShipmenDirectionConvertComponent.prototype.SetLabels = function () {
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCode = "Shipment.S.NewShipment.Gateway";
                this.ToTextCode = "Shipment.S.NewShipment.Destination";
                break;
            }
            case "O": {
                this.FromTextCode = "Shipment.S.NewShipment.LoadingPort";
                this.ToTextCode = "Shipment.S.NewShipment.DischargePort";
                break;
            }
            case "I": {
                this.FromTextCode = "Shipment.S.NewShipment.From";
                this.ToTextCode = "Shipment.S.NewShipment.To";
                break;
            }
            default: {
                this.FromTextCode = "Shipment.S.NewShipment.From";
                this.ToTextCode = "Shipment.S.NewShipment.To";
                break;
            }
        }
    };
    ShipmenDirectionConvertComponent.prototype.SetScreenEnabled = function () {
        var isScreenEnabled = false;
        if (this.oldShipmentDirection != this.DirectionId) {
            isScreenEnabled = true;
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
        // Ports        
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isScreenEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isScreenEnabled);
        this.SetUIProperties();
    };
    ShipmenDirectionConvertComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Shipper();
        this.SetUIProperties_Consignee();
        this.SetUIProperties_Ports();
    };
    ShipmenDirectionConvertComponent.prototype.SetUIProperties_Shipper = function () {
        var isFieldRequired = false;
        if (this.IsCurrentInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                isFieldRequired = true;
            }
        }
        this.UIProperties.SetRequired("ShipperId", this.ObjectTableName, isFieldRequired);
    };
    ShipmenDirectionConvertComponent.prototype.SetUIProperties_Consignee = function () {
        var isFieldRequired = false;
        if (this.IsCurrentInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isFieldRequired = true;
            }
        }
        this.UIProperties.SetRequired("ConsigneeId", this.ObjectTableName, isFieldRequired);
    };
    ShipmenDirectionConvertComponent.prototype.SetUIProperties_Ports = function () {
        var isFromRequired = false;
        var isToRequired = false;
        var isPortsVisible = false;
        if (this.IsOldInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                isFromRequired = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
                isToRequired = true;
            }
        }
        if (this.IsOldInlandDomestic || (this.DirectionId == "D" && this.DirectionId != this.oldShipmentDirection && this.EntityPM.TransportModeId != "I")) {
            isPortsVisible = true;
        }
        this.IsPortsVisible = isPortsVisible;
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, isToRequired);
    };
    // Direction
    ShipmenDirectionConvertComponent.prototype.BuildDirectionsFilterList = function () {
        this.DirectionsList = [];
        this.DirectionsList.push(new DirectionFilterItem("E", "Export"));
        this.DirectionsList.push(new DirectionFilterItem("I", "Import"));
        this.DirectionsList.push(new DirectionFilterItem("D", "Domestic"));
        this.DirectionsList.push(new DirectionFilterItem("R", "Drop"));
    };
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) {
            if (this.EntityPM.DirectionId != newValue) {
                this.EntityPM.DirectionId = newValue;
                this.OnDirectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmenDirectionConvertComponent.prototype.OnDirectionChanged = function () {
        this.IsCurrentInlandDomestic = this.EntityPM.TransportModeId == "I" && this.EntityPM.DirectionId == "D" ? true : false;
        this.SetScreenEnabled();
        this.SetUIProperties();
        this.SetPartners();
        this.LoadAddress("S", this.EntityPM.ShipperAddressId);
        this.LoadAddress("C", this.EntityPM.ConsigneeAddressId);
    };
    ShipmenDirectionConvertComponent.prototype.SetPartners = function () {
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
    };
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ShipperId != newValue) {
                this.EntityPM.ShipperId = newValue;
                this.EntityPM.MainCarriageFromPartnerId = newValue;
                if (this.ShipmentCustomerTypeCode == "SHI") {
                    this.CustomerId = newValue;
                }
                this.SetUIProperties_Shipper();
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
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperAddressId", {
        get: function () { return this.EntityPM.ShipperAddressId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperAddressId != newValue) {
                this.EntityPM.ShipperAddressId = newValue;
                this.EntityPM.MainCarriageFromAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ShipperAddressList = null;
                }
                else {
                    this.LoadAddress("S", newValue);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperAddressList", {
        get: function () { return this.myShipperAddressList; },
        set: function (newValue) {
            this.myShipperAddressList = newValue;
            if (this.IsCurrentInlandDomestic) {
                if (newValue != null) {
                    this.EntityPM.FromCountryId = newValue.CountryId;
                    this.EntityPM.FromCountryIsEC = newValue.CountryEC;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperContactId", {
        get: function () { return this.EntityPM.ShipperContactId; },
        set: function (newValue) {
            if (this.EntityPM.ShipperContactId != newValue) {
                this.EntityPM.ShipperContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperReference1", {
        get: function () { return this.EntityPM.ShipperReference1; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference1 != newValue) {
                this.EntityPM.ShipperReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperReference2", {
        get: function () { return this.EntityPM.ShipperReference2; },
        set: function (newValue) {
            if (this.EntityPM.ShipperReference2 != newValue) {
                this.EntityPM.ShipperReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipperName", {
        get: function () { return this.EntityPM.ShipperName; },
        set: function (newValue) {
            if (this.EntityPM.ShipperName != newValue) {
                this.EntityPM.ShipperName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeId", {
        get: function () { return this.EntityPM.ConsigneeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ConsigneeId != newValue) {
                this.EntityPM.ConsigneeId = newValue;
                this.EntityPM.MainCarriageToPartnerId = newValue;
                if (this.ShipmentCustomerTypeCode == "CON") {
                    this.CustomerId = newValue;
                }
                this.SetUIProperties_Consignee();
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
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeAddressId", {
        get: function () { return this.EntityPM.ConsigneeAddressId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeAddressId != newValue) {
                this.EntityPM.ConsigneeAddressId = newValue;
                this.EntityPM.MainCarriageToAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ConsigneeAddressList = null;
                }
                else {
                    this.LoadAddress("C", newValue);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeAddressList", {
        get: function () { return this.myConsigneeAddressList; },
        set: function (newValue) {
            this.myConsigneeAddressList = newValue;
            if (this.IsCurrentInlandDomestic) {
                if (newValue != null) {
                    this.EntityPM.ToCountryId = newValue.CountryId;
                    this.EntityPM.ToCountryIsEC = newValue.CountryEC;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeContactId", {
        get: function () { return this.EntityPM.ConsigneeContactId; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeContactId != newValue) {
                this.EntityPM.ConsigneeContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeReference1", {
        get: function () { return this.EntityPM.ConsigneeReference1; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference1 != newValue) {
                this.EntityPM.ConsigneeReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeReference2", {
        get: function () { return this.EntityPM.ConsigneeReference2; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeReference2 != newValue) {
                this.EntityPM.ConsigneeReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ConsigneeName", {
        get: function () { return this.EntityPM.ConsigneeName; },
        set: function (newValue) {
            if (this.EntityPM.ConsigneeName != newValue) {
                this.EntityPM.ConsigneeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "IsShipperMyCustomer", {
        get: function () { return this.isShipperMyCustomer; },
        set: function (value) {
            if (this.isShipperMyCustomer != value) {
                this.isShipperMyCustomer = value;
                if (value) {
                    if (this.ShipmentCustomerTypeCode == "SHI") {
                        this.CustomerId = this.ShipperId;
                    }
                }
                this.SetUIProperties_Shipper();
                this.SetUIProperties_Consignee();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "IsConsigneeMyCustomer", {
        get: function () { return this.isConsigneeMyCustomer; },
        set: function (value) {
            if (this.isConsigneeMyCustomer != value) {
                this.isConsigneeMyCustomer = value;
                if (value) {
                    if (this.ShipmentCustomerTypeCode == "CON") {
                        this.CustomerId = this.ConsigneeId;
                    }
                }
                this.SetUIProperties_Shipper();
                this.SetUIProperties_Consignee();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmenDirectionConvertComponent.prototype.SetCustomer = function (myCode) {
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
    ShipmenDirectionConvertComponent.prototype.ComputeCustomerDependency = function () {
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
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "ShipmentCustomerTypeCode", {
        get: function () { return this.EntityPM.ShipmentCustomerTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentCustomerTypeCode != newValue) {
                this.EntityPM.ShipmentCustomerTypeCode = newValue;
                this.CustomerId = null;
                this.SetCustomer(newValue);
                this.SetCustomerRequired();
                this.ComputeCustomerDependency();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "CustomerId", {
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
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "CustomerAddressId", {
        get: function () { return this.EntityPM.CustomerAddressId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerAddressId != newValue) {
                this.EntityPM.CustomerAddressId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "CustomerContactId", {
        get: function () { return this.EntityPM.CustomerContactId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerContactId != newValue) {
                this.EntityPM.CustomerContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmenDirectionConvertComponent.prototype.SetCustomerPartner = function () {
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
    ShipmenDirectionConvertComponent.prototype.SetCustomerRequired = function () {
        var isRequired = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerId) || Tools_1.AppTool.IsNullOrEmpty(this.ShipmentCustomerTypeCode)) {
            isRequired = true;
        }
        this.IsCustomerRequired = isRequired;
    };
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "MainCarriageFromPortId", {
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFromPortId != value) {
                this.EntityPM.MainCarriageFromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromPortList = null;
                    Tools_2.RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            _this.FromPortList = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        _this.FromPortList = myResponse2.Result;
                                        Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
                                    }
                                });
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmenDirectionConvertComponent.prototype, "MainCarriageToPortId", {
        get: function () { return this.EntityPM.MainCarriageToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageToPortId != value) {
                this.EntityPM.MainCarriageToPortId = value;
                this.EntityPM.ToPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToPortList = null;
                    Tools_2.RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            _this.ToPortList = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        _this.ToPortList = myResponse2.Result;
                                        Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
                                    }
                                });
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    //LoadAddress
    ShipmenDirectionConvertComponent.prototype.LoadAddress = function (myAddressCode, myAddressId) {
        var _this = this;
        switch (myAddressCode) {
            case "S": {
                if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
                    this.myAddressListService.getSingle(myAddressId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ShipperAddressList = myResponse.Result;
                        }
                    });
                }
                break;
            }
            case "C": {
                if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
                    this.myAddressListService.getSingle(myAddressId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.ConsigneeAddressList = myResponse.Result;
                        }
                    });
                }
                break;
            }
        }
    };
    // Add|Edit Address
    ShipmenDirectionConvertComponent.prototype.EditAddressClicked = function (myAddressCode) {
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
                    }
                }
            });
        }
    };
    ShipmenDirectionConvertComponent.prototype.AddAddressClicked = function (myAddressCode) {
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
                    }
                }
            });
        }
    };
    // Commands
    ShipmenDirectionConvertComponent.prototype.AddPartnerClicked = function (myPartnerCode) {
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
                    }
                    else if (myPartnerCode == "C") {
                        _this.ConsigneeId = comp.EntityPM.Id;
                        _this.ConsigneeName = comp.EntityPM.EnglishName;
                    }
                    else if (myPartnerCode == "T") {
                        _this.CustomerId = comp.EntityPM.Id;
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
    ShipmenDirectionConvertComponent.prototype.ComputeAddCustomerTitle = function () {
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
    ShipmenDirectionConvertComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('DirectionId');
        this.myCloner.AddField('ShipperId');
        this.myCloner.AddField('ShipperName');
        this.myCloner.AddField('ShipperAddressId');
        this.myCloner.AddField('ShipperContactId');
        this.myCloner.AddField('ShipperReference1');
        this.myCloner.AddField('ShipperReference2');
        this.myCloner.AddField('ConsigneeId');
        this.myCloner.AddField('ConsigneeName');
        this.myCloner.AddField('ConsigneeAddressId');
        this.myCloner.AddField('ConsigneeContactId');
        this.myCloner.AddField('ConsigneeReference1');
        this.myCloner.AddField('ConsigneeReference2');
        this.myCloner.AddField('CustomerId');
        this.myCloner.AddField('CustomerName');
        this.myCloner.AddField('ShipmentCustomerTypeCode');
        this.myCloner.AddField('MainCarriageFromPortId');
        this.myCloner.AddField('MainCarriageFromToId');
        this.myCloner.AddEntity(this.EntityPM);
    };
    ShipmenDirectionConvertComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    ShipmenDirectionConvertComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    ShipmenDirectionConvertComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var message = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.IsCurrentInlandDomestic) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
                errors.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ConsigneeId")));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperId)) {
                errors.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ShipperId")));
            }
            if (this.EntityPM.ShipmentLevelCode == "C") {
                errors.push("Master inland domestic are not allowed");
            }
            else if (this.EntityPM.ShipmentLevelCode == "H") {
                errors.push("House inland domestic shipments are not allowed");
            }
            if (this.EntityPM.ShipmentLevelCode != "C") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
                    if (this.EntityPM.FromCountryId != this.EntityPM.ToCountryId) {
                        if (this.EntityPM.FromCountryIsEC == false || this.EntityPM.ToCountryIsEC == false) {
                            errors.push("Both Addresses must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
        else {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId)) {
                var textCode = Tools_2.ShipmentTool.GetFromPortTextCode(this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode);
                errors.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(textCode)));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
                var textCode = Tools_2.ShipmentTool.GetToPortTextCode(this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode);
                errors.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(textCode)));
            }
            if (this.EntityPM.DirectionId == "D") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
                    if (this.EntityPM.FromCountryId != this.EntityPM.ToCountryId) {
                        if (this.EntityPM.FromCountryIsEC == false || this.EntityPM.ToCountryIsEC == false) {
                            errors.push("Both Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId) || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentCustomerTypeCode)) {
                errors.push(message.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.CustomerId")));
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.SetCustomerPartner();
            if (this.IsCurrentInlandDomestic) {
                if (this.ShipperAddressList != null) {
                    this.EntityPM.FromCountryId = this.ShipperAddressList.CountryId;
                    this.EntityPM.FromCountryIsEC = this.ShipperAddressList.CountryEC;
                }
                if (this.ConsigneeAddressList != null) {
                    this.EntityPM.ToCountryId = this.ConsigneeAddressList.CountryId;
                    this.EntityPM.ToCountryIsEC = this.ConsigneeAddressList.CountryEC;
                }
                this.EntityPM.MainCarriageFromPartnerId = this.ShipperId;
                this.EntityPM.MainCarriageFromAddressId = this.ShipperAddressId;
                this.EntityPM.MainCarriageToPartnerId = this.ConsigneeId;
                this.EntityPM.MainCarriageToAddressId = this.ConsigneeAddressId;
                this.EntityPM.IncludePickUp = false;
                this.EntityPM.IncludeDelivery = false;
                this.MainCarriageFromPortId = null;
                this.MainCarriageToPortId = null;
                this.EntityPM.MainCarriageFinalDestinationPortId = null;
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Title = "Convert Shipment Direction";
                confirmWindow.Width = 400;
                confirmWindow.Show("Origin and Destination ports will be replaced by the Shipper/Consignee Addresses, proceed?");
                confirmWindow.YesButtonText = "Yes";
                confirmWindow.NoButtonText = "No";
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CompleteConversion();
                    }
                });
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
                this.CompleteConversion();
            }
        }
    };
    ShipmenDirectionConvertComponent.prototype.CompleteConversion = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var oldDirectionName = this.DirectionsList.filter(function (d) { return d.Code == _this.oldShipmentDirection; })[0].Name;
        var currentDirectionName = this.DirectionsList.filter(function (d) { return d.Code == _this.DirectionId; })[0].Name;
        this.EntityPM.EventNote = "Converted from [" + oldDirectionName + "] to [" + currentDirectionName + "]";
        this.EntityPM.ShipmentDirectionConverted = true;
        var service = new ShipmentPMService_1.ShipmentPMService();
        service.update(this.EntityPM).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ShipmenDirectionConvertComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmenDirectionConvertComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ShipmenDirectionConvertComponent);
    return ShipmenDirectionConvertComponent;
}(BaseComponent_1.BaseComponent));
exports.ShipmenDirectionConvertComponent = ShipmenDirectionConvertComponent;
var ConvertDirectionArgs = /** @class */ (function () {
    function ConvertDirectionArgs() {
        this.EnabledOkButton = true;
        this.ValidationErrorsList = [];
    }
    return ConvertDirectionArgs;
}());
exports.ConvertDirectionArgs = ConvertDirectionArgs;
var DirectionFilterItem = /** @class */ (function () {
    function DirectionFilterItem(code, name, src) {
        if (src === void 0) { src = null; }
        this.Code = code;
        this.Name = name;
        this.SRC = src;
    }
    return DirectionFilterItem;
}());
exports.DirectionFilterItem = DirectionFilterItem;
//# sourceMappingURL=ShipmenDirectionConvertComponent.js.map