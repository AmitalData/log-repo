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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var PortListService_1 = require("../../../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../../Common/Services/StandardLists/AddressListService");
var CountryListService_1 = require("../../../../../Common/Services/StandardLists/CountryListService");
var Args_1 = require("../../../../../Common/Args");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeliveryMainTabComponent = /** @class */ (function (_super) {
    __extends(DeliveryMainTabComponent, _super);
    function DeliveryMainTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.ObjectTableName = "ShipmentPickUpDelivery";
        _this.IsConnectedToContainer = false;
        _this.IsEditingEnabled = true;
        _this.IsEmptyContainerVisible = false;
        _this.InitServices();
        return _this;
    }
    DeliveryMainTabComponent.prototype.InitServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myAddressListService = new AddressListService_1.AddressListService();
        this.myCountryListService = new CountryListService_1.CountryListService();
    };
    DeliveryMainTabComponent.prototype.InitTab = function (myEntityPM, myShipmentPM) {
        var _this = this;
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        this.TransportModeId = this.ShipmentPM.TransportModeId;
        this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.FullResponsibilityHelp = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDelivery.FullResponsibilityHelpText");
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        }
        else {
            if (this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.Id; }).length > 0) {
                this.IsConnectedToContainer = true;
            }
            if (this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.EmptyContainerReturnId == _this.EntityPM.Id; }).length > 0) {
                this.IsConnectedToContainer = true;
            }
        }
        this.SetUIProperties();
    };
    DeliveryMainTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        //if (this.IsEditingEnabled) {
        //    if (this.IsConnectedToContainer) {
        //        this.IsEditingEnabled = false;
        //    }
        //}
        this.UIProperties.SetEnabled("FromAddress", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ToAddress", this.ObjectTableName, false);
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
    };
    DeliveryMainTabComponent.prototype.SetUIProperties_From = function () {
        switch (this.FromTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("FromPartnerCardId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.FromPartnerCardId) ? true : false);
                this.UIProperties.SetEnabled("FromPartnerCardId", this.ObjectTableName, this.IsEditingEnabled);
                var isAddressIdEnabled = false;
                if (this.IsEditingEnabled) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPartnerCardId)) {
                        isAddressIdEnabled = true;
                    }
                }
                this.UIProperties.SetEnabled("FromAddressId", this.ObjectTableName, isAddressIdEnabled);
            }
            case "PORT": {
                this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.FromPortId) ? true : false);
                this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, this.IsEditingEnabled);
            }
            case "CASL": {
                this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.FromAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.FromAddressZipCode) ? true : false);
                this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.FromAddressCountryId) ? true : false);
                this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, this.IsEditingEnabled);
            }
        }
    };
    DeliveryMainTabComponent.prototype.SetUIProperties_To = function () {
        switch (this.ToTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("ToPartnerCardId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ToPartnerCardId) ? true : false);
                this.UIProperties.SetEnabled("ToPartnerCardId", this.ObjectTableName, this.IsEditingEnabled);
                var isAddressIdEnabled = false;
                if (this.IsEditingEnabled) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPartnerCardId)) {
                        isAddressIdEnabled = true;
                    }
                }
                this.UIProperties.SetEnabled("ToAddressId", this.ObjectTableName, isAddressIdEnabled);
            }
            case "PORT": {
                this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
                this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, this.IsEditingEnabled);
            }
            case "CASL": {
                this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ToAddressCity) && Tools_1.AppTool.IsNullOrEmpty(this.ToAddressZipCode) ? true : false);
                this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ToAddressCountryId) ? true : false);
                this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, this.IsEditingEnabled);
            }
        }
    };
    DeliveryMainTabComponent.prototype.SetUIProperties_EmptyContainer = function () {
        var isEmptyContainerVisible = false;
        if (this.IsFCLEntity) {
            if (this.ShipmentPM.DirectionId == "I") {
                isEmptyContainerVisible = true;
            }
        }
        this.IsEmptyContainerVisible = isEmptyContainerVisible;
    };
    DeliveryMainTabComponent.prototype.SetUIProperties_ValidDatesFields = function () {
        this.UIProperties.SetValidity("ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("ATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.ATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATD"));
            this.UIProperties.SetValidity("ATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.ATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDelivery.F.ATA"));
            this.UIProperties.SetValidity("ATA", this.ObjectTableName, false, errorMessage);
        }
    };
    // On Open Edit Mood
    DeliveryMainTabComponent.prototype.OnEditMoodScreen = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromAddressId)) {
            this.myAddressListService.getSingle(this.FromAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.FromAddressList = myResponse.Result;
                }
            });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ToAddressId)) {
            this.myAddressListService.getSingle(this.ToAddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.ToAddressList = myResponse.Result;
                }
            });
        }
    };
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FullResponsibility", {
        get: function () { return this.EntityPM.FullResponsibility; },
        set: function (value) {
            if (this.EntityPM.FullResponsibility != value) {
                this.EntityPM.FullResponsibility = value;
                this.SetUIProperties_From();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromTypeCode", {
        // From
        get: function () { return this.EntityPM.PickUpDeliveryFromTypeCode; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromPartnerCardId", {
        get: function () { return this.EntityPM.FromPartnerCardId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromPartnerCardId != value) {
                this.EntityPM.FromPartnerCardId = value;
                this.SetUIProperties_From();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromAddressId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.PickAddressId)) {
                                    _this.FromAddressId = list.PickAddressId;
                                }
                                else {
                                    _this.FromAddressId = list.MainAddressId;
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
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromAddressId", {
        get: function () { return this.EntityPM.FromAddressId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromAddressId != value) {
                this.EntityPM.FromAddressId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromAddressList = null;
                    this.EntityPM.FromAddressCity_Dummy = null;
                    this.EntityPM.FromAddressCountryCode = null;
                    this.EntityPM.FromAddressCountryName = null;
                }
                else {
                    this.myAddressListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.FromAddressList = list;
                                _this.EntityPM.FromAddressCity_Dummy = list.City;
                                _this.EntityPM.FromAddressCountryCode = list.CountryCode;
                                _this.EntityPM.FromAddressCountryName = list.CountryName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromPortId != value) {
                this.EntityPM.FromPortId = value;
                this.SetUIProperties_From();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.FromAddress = null;
                    this.EntityPM.FromPortCode = null;
                    this.EntityPM.FromPortName = null;
                    this.EntityPM.FromPortCountryCode = null;
                    this.EntityPM.FromPortCountryName = null;
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.FromAddress = "Port Of: " + list.EnglishName;
                                _this.EntityPM.FromPortCode = list.Code;
                                _this.EntityPM.FromPortName = list.EnglishName;
                                _this.EntityPM.FromPortCountryCode = list.CountryCode;
                                _this.EntityPM.FromPortCountryName = list.CountryName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromAddressCity", {
        get: function () { return this.EntityPM.FromAddressCity; },
        set: function (value) {
            if (this.EntityPM.FromAddressCity != value) {
                this.EntityPM.FromAddressCity = value;
                this.EntityPM.FromAddressCity_Dummy = value;
                this.SetUIProperties_From();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromAddressZipCode", {
        get: function () { return this.EntityPM.FromAddressZipCode; },
        set: function (value) {
            if (this.EntityPM.FromAddressZipCode != value) {
                this.EntityPM.FromAddressZipCode = value;
                this.SetUIProperties_From();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromAddressCountryId", {
        get: function () { return this.EntityPM.FromAddressCountryId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.FromAddressCountryId != value) {
                this.EntityPM.FromAddressCountryId = value;
                this.SetUIProperties_From();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.FromAddressCountryCode = null;
                    this.EntityPM.FromAddressCountryName = null;
                }
                else {
                    this.myCountryListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.FromAddressCountryCode = list.Code;
                                _this.EntityPM.FromAddressCountryName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromAddress", {
        get: function () { return this.EntityPM.FromAddress; },
        set: function (value) {
            if (this.EntityPM.FromAddress != value) {
                this.EntityPM.FromAddress = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "FromAddressList", {
        get: function () { return this.fromAddressList; },
        set: function (newValue) {
            this.fromAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToTypeCode", {
        // To
        get: function () { return this.EntityPM.PickUpDeliveryToTypeCode; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToPartnerCardId", {
        get: function () { return this.EntityPM.ToPartnerCardId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToPartnerCardId != value) {
                this.EntityPM.ToPartnerCardId = value;
                this.SetUIProperties_To();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToAddressId = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(list.PickAddressId)) {
                                    _this.ToAddressId = list.PickAddressId;
                                }
                                else {
                                    _this.ToAddressId = list.MainAddressId;
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
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToAddressId", {
        get: function () { return this.EntityPM.ToAddressId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToAddressId != value) {
                this.EntityPM.ToAddressId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToAddressList = null;
                    this.EntityPM.ToAddressCity_Dummy = null;
                    this.EntityPM.ToAddressCountryCode = null;
                    this.EntityPM.ToAddressCountryName = null;
                }
                else {
                    this.myAddressListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.ToAddressList = list;
                                _this.EntityPM.ToAddressCity_Dummy = list.City;
                                _this.EntityPM.ToAddressCountryCode = list.CountryCode;
                                _this.EntityPM.ToAddressCountryName = list.CountryName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToPortId != value) {
                this.EntityPM.ToPortId = value;
                this.SetUIProperties_To();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ToAddress = null;
                    this.EntityPM.ToPortCode = null;
                    this.EntityPM.ToPortName = null;
                    this.EntityPM.ToPortCountryCode = null;
                    this.EntityPM.ToPortCountryName = null;
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.ToAddress = "Port Of: " + list.EnglishName;
                                _this.EntityPM.ToPortCode = list.Code;
                                _this.EntityPM.ToPortName = list.EnglishName;
                                _this.EntityPM.ToPortCountryCode = list.CountryCode;
                                _this.EntityPM.ToPortCountryName = list.CountryName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToAddressCity", {
        get: function () { return this.EntityPM.ToAddressCity; },
        set: function (value) {
            if (this.EntityPM.ToAddressCity != value) {
                this.EntityPM.ToAddressCity = value;
                this.EntityPM.ToAddressCity_Dummy = value;
                this.SetUIProperties_To();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToAddressZipCode", {
        get: function () { return this.EntityPM.ToAddressZipCode; },
        set: function (value) {
            if (this.EntityPM.ToAddressZipCode != value) {
                this.EntityPM.ToAddressZipCode = value;
                this.SetUIProperties_To();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToAddressCountryId", {
        get: function () { return this.EntityPM.ToAddressCountryId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ToAddressCountryId != value) {
                this.EntityPM.ToAddressCountryId = value;
                this.SetUIProperties_To();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.ToAddressCountryCode = null;
                    this.EntityPM.ToAddressCountryName = null;
                }
                else {
                    this.myCountryListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.ToAddressCountryCode = list.Code;
                                _this.EntityPM.ToAddressCountryName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToAddress", {
        get: function () { return this.EntityPM.ToAddress; },
        set: function (value) {
            if (this.EntityPM.ToAddress != value) {
                this.EntityPM.ToAddress = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ToAddressList", {
        get: function () { return this.toAddressList; },
        set: function (newValue) {
            this.toAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    // Select City
    DeliveryMainTabComponent.prototype.SelectCityCommand = function (myAddressCode) {
        var _this = this;
        var mySourceCountryId = myAddressCode == "F" ? this.FromAddressCountryId : this.ToAddressCountryId;
        var args = new Args_1.CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                if (myAddressCode == "F") {
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
    Object.defineProperty(DeliveryMainTabComponent.prototype, "CarrierId", {
        // Trucker
        get: function () { return this.EntityPM.CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.CarrierId != value) {
                this.EntityPM.CarrierId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.CarrierCode = null;
                    this.CarrierName = null;
                    this.CarrierWebSite = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.CarrierCode = list.Code;
                                _this.CarrierName = list.EnglishName;
                                _this.CarrierWebSite = list.WebSite;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "CarrierCode", {
        get: function () { return this.EntityPM.CarrierCode; },
        set: function (value) {
            if (this.EntityPM.CarrierCode != value) {
                this.EntityPM.CarrierCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "CarrierName", {
        get: function () { return this.EntityPM.CarrierName; },
        set: function (value) {
            if (this.EntityPM.CarrierName != value) {
                this.EntityPM.CarrierName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "CarrierWebSite", {
        get: function () { return this.EntityPM.CarrierWebSite; },
        set: function (value) {
            if (this.EntityPM.CarrierWebSite != value) {
                this.EntityPM.CarrierWebSite = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "CarrierNumber", {
        get: function () { return this.EntityPM.CarrierNumber; },
        set: function (value) {
            if (this.EntityPM.CarrierNumber != value) {
                this.EntityPM.CarrierNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "Driver", {
        get: function () { return this.EntityPM.Driver; },
        set: function (value) {
            if (this.EntityPM.Driver != value) {
                this.EntityPM.Driver = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "TruckNumber", {
        get: function () { return this.EntityPM.TruckNumber; },
        set: function (value) {
            if (this.EntityPM.TruckNumber != value) {
                this.EntityPM.TruckNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "TrailerNumber", {
        get: function () { return this.EntityPM.TrailerNumber; },
        set: function (value) {
            if (this.EntityPM.TrailerNumber != value) {
                this.EntityPM.TrailerNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "TransportModeCode", {
        get: function () { return this.EntityPM.TransportModeCode; },
        set: function (value) {
            if (this.EntityPM.TransportModeCode != value) {
                this.EntityPM.TransportModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ETD", {
        get: function () { return this.EntityPM.ETD; },
        set: function (value) {
            if (this.EntityPM.ETD != value) {
                this.EntityPM.ETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ETA", {
        get: function () { return this.EntityPM.ETA; },
        set: function (value) {
            if (this.EntityPM.ETA != value) {
                this.EntityPM.ETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ATD", {
        get: function () { return this.EntityPM.ATD; },
        set: function (value) {
            if (this.EntityPM.ATD != value) {
                this.EntityPM.ATD = value;
                this.SetUIProperties_ValidDatesFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "ATA", {
        get: function () { return this.EntityPM.ATA; },
        set: function (value) {
            if (this.EntityPM.ATA != value) {
                this.EntityPM.ATA = value;
                this.SetUIProperties_ValidDatesFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "EmptyDeliveryContainerPartnerId", {
        get: function () { return this.EntityPM.EmptyDeliveryContainerPartnerId; },
        set: function (value) {
            if (this.EntityPM.EmptyDeliveryContainerPartnerId != value) {
                this.EntityPM.EmptyDeliveryContainerPartnerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryMainTabComponent.prototype, "EmptyDeliveryDepotReference", {
        get: function () { return this.EntityPM.EmptyDeliveryDepotReference; },
        set: function (value) {
            if (this.EntityPM.EmptyDeliveryDepotReference != value) {
                this.EntityPM.EmptyDeliveryDepotReference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DeliveryMainTabComponent.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "ETD": {
                this.ATD = Tools_1.DateTool.GetDateParts(this.ETD).DateObject;
                break;
            }
            case "ETA": {
                this.ATA = Tools_1.DateTool.GetDateParts(this.ETA).DateObject;
                break;
            }
        }
    };
    DeliveryMainTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeliveryMainTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeliveryMainTabComponent);
    return DeliveryMainTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DeliveryMainTabComponent = DeliveryMainTabComponent;
//# sourceMappingURL=DeliveryMainTabComponent.js.map