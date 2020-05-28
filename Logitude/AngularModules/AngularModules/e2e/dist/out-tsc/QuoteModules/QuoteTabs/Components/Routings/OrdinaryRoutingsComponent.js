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
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var QuoteUtilities_1 = require("../../../../Quote/Utilities/QuoteUtilities");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AddressPM_1 = require("../../../../Common/EntityPMs/AddressPM");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../../Common/Args");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var OrdinaryRoutingsComponent = /** @class */ (function (_super) {
    __extends(OrdinaryRoutingsComponent, _super);
    function OrdinaryRoutingsComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "Quote";
        _this.DataContext = _this;
        _this.IsSubjectVisible = false;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsQuoteEditEnabled = false;
        _this.IsPickupAddressSubFieldsVisibile = false;
        _this.IsDeliveryAddressSubFieldsVisibile = false;
        _this.IsSubjectVisible = SessionLocator_1.SessionLocator.TenantPM.IsQuoteSubjectEdited ? true : false;
        _this.InitializeServices();
        _this.Listen();
        return _this;
    }
    OrdinaryRoutingsComponent.prototype.InitTab = function (entityPM, tableName) {
        this.EntityPM = entityPM;
        this.LoadPickupDeliveryData();
        this.SetLabels();
        this.SetUIProperties();
    };
    OrdinaryRoutingsComponent.prototype.InitializeServices = function () {
        this.myAddressListService = new AddressListService_1.AddressListService();
    };
    OrdinaryRoutingsComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "QTRT") {
                    _this.LoadPickupDeliveryData();
                    _this.SetUIProperties();
                }
            });
        }
    };
    OrdinaryRoutingsComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    OrdinaryRoutingsComponent.prototype.SetUIProperties = function () {
        this.IsQuoteEditEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.SetUIProperties_PickupFields();
        this.SetUIProperties_DeliveryFields();
        this.SetUIProperties_EntityClosed();
    };
    OrdinaryRoutingsComponent.prototype.SetUIProperties_EntityClosed = function () {
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("IncludePickUp", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromAddressCity", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromAddressZipCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("FromAddressCountryId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("PickUpAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("IncludeDelivery", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToAddressCity", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToAddressZipCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ToAddressCountryId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("DeliveryAddressId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, this.IsQuoteEditEnabled);
    };
    OrdinaryRoutingsComponent.prototype.LoadPickupDeliveryData = function () {
        if (this.IncludePickUp) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                    this.LoadPickupAddress();
                }
            }
        }
        if (this.IncludeDelivery) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                    this.LoadDeliveryAddress();
                }
            }
        }
    };
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IsSubjectEdited", {
        // Subject
        get: function () { return this.EntityPM.IsSubjectEdited; },
        set: function (value) {
            if (this.EntityPM.IsSubjectEdited != value) {
                this.EntityPM.IsSubjectEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (value) {
            if (this.EntityPM.Subject != value) {
                this.EntityPM.Subject = value;
                this.IsSubjectEdited = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdinaryRoutingsComponent.prototype.ResetSubjectEdited = function () {
        this.IsSubjectEdited = false;
        this.GetSubjectField();
    };
    OrdinaryRoutingsComponent.prototype.GetSubjectField = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantPM.IsQuoteSubjectEdited) {
            var myQuoteDomainService = new QuoteDomainService_1.QuoteDomainService();
            myQuoteDomainService.ComputeQuoteAutomaticSubject(this.EntityPM).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.EntityPM.Subject = myResponse.Result;
                }
            });
        }
    };
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ETD", {
        get: function () { return this.EntityPM.ETD; },
        set: function (newValue) {
            if (this.EntityPM.ETD != newValue) {
                this.EntityPM.ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ETA", {
        get: function () { return this.EntityPM.ETA; },
        set: function (newValue) {
            if (this.EntityPM.ETA != newValue) {
                this.EntityPM.ETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IncludePickUp", {
        // PickUp
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "PickUpAddressId", {
        get: function () { return this.EntityPM.PickUpAddressId; },
        set: function (newValue) {
            if (this.EntityPM.PickUpAddressId != newValue) {
                this.EntityPM.PickUpAddressId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.FromAddressCity = null;
                    this.FromAddressZipCode = null;
                    this.FromAddressCountryId = null;
                }
                this.LoadPickupAddress();
                this.SetUIProperties_PickupFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "FromAddressZipCode", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "FromAddressCity", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "FromAddressCountryId", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ShipperId", {
        get: function () { return this.EntityPM.ShipperId; },
        enumerable: true,
        configurable: true
    });
    OrdinaryRoutingsComponent.prototype.LoadPickupAddress = function () {
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
    OrdinaryRoutingsComponent.prototype.UpdatePickUpAddressFields = function () {
        if (!this.IncludePickUp) {
            this.EntityPM.PickUpAddressId = null;
            this.EntityPM.FromAddressCity = null;
            this.EntityPM.FromAddressZipCode = null;
            this.EntityPM.FromAddressCountryId = null;
            this.PickupAddressList = null;
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IsEditPickUpAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsQuoteEditEnabled) {
                if (this.IncludePickUp) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId) && !Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                        myResult = true;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IsAddPickUpAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsQuoteEditEnabled) {
                if (this.IncludePickUp) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                        myResult = true;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    OrdinaryRoutingsComponent.prototype.SetUIProperties_PickupFields = function () {
        var isCityRequired = false;
        var isCountryRequired = false;
        var isPickupAddressSubFieldsVisibile = false;
        if (this.IncludePickUp) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipperId)) {
                isPickupAddressSubFieldsVisibile = true;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.PickUpAddressId)) {
                isPickupAddressSubFieldsVisibile = true;
            }
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
        this.IsPickupAddressSubFieldsVisibile = isPickupAddressSubFieldsVisibile;
        this.UIProperties.SetRequired("FromAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("FromAddressCountryId", this.ObjectTableName, isCountryRequired);
    };
    OrdinaryRoutingsComponent.prototype.SetLabels = function () {
        this.FromLabel = "Quote.S.NewQuote.From";
        this.ToLabel = "Quote.S.NewQuote.To";
        this.CarrierLabel = "Quote.S.NewQuote.Carrier";
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.FromLabel = "Quote.S.NewQuote.Gateway";
                this.ToLabel = "Quote.S.NewQuote.Destination";
                this.CarrierLabel = "Quote.S.NewQuote.Airline";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }
            case "O": {
                this.FromLabel = "Quote.S.NewQuote.LoadingPort";
                this.ToLabel = "Quote.S.NewQuote.DischargePort";
                this.CarrierLabel = "Quote.S.NewQuote.Shippingline";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }
            case "I": {
                this.FromLabel = "Quote.S.NewQuote.From";
                this.ToLabel = "Quote.S.NewQuote.To";
                this.CarrierLabel = "Quote.S.NewQuote.Trucker";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }
    };
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.FromPortId != newValue) {
                this.EntityPM.FromPortId = newValue;
                if (newValue == null) {
                    this.EntityPM.FromCountryId = null;
                }
                else {
                    var myService = new PortListService_1.PortListService();
                    myService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var port = myResponse.Result;
                            if (port != null) {
                                _this.EntityPM.FromCountryId = port.CountryId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ToPortId != newValue) {
                this.EntityPM.ToPortId = newValue;
                if (newValue == null) {
                    this.EntityPM.ToCountryId = null;
                }
                else {
                    var myService = new PortListService_1.PortListService();
                    myService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var port = myResponse.Result;
                            if (port != null) {
                                _this.EntityPM.ToCountryId = port.CountryId;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierId != newValue) {
                this.EntityPM.MainCarriageCarrierId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IncludeDelivery", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "DeliveryAddressId", {
        get: function () { return this.EntityPM.DeliveryAddressId; },
        set: function (newValue) {
            if (this.EntityPM.DeliveryAddressId != newValue) {
                this.EntityPM.DeliveryAddressId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ToAddressCity = null;
                    this.ToAddressZipCode = null;
                    this.ToAddressCountryId = null;
                }
                this.LoadDeliveryAddress();
                this.SetUIProperties_DeliveryFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ToAddressZipCode", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ToAddressCity", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ToAddressCountryId", {
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "ConsigneeId", {
        get: function () { return this.EntityPM.ConsigneeId; },
        enumerable: true,
        configurable: true
    });
    OrdinaryRoutingsComponent.prototype.LoadDeliveryAddress = function () {
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
    OrdinaryRoutingsComponent.prototype.UpdateDeliveryAddressFields = function () {
        if (!this.IncludeDelivery) {
            this.EntityPM.DeliveryAddressId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.DeliveryAddressList = null;
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
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IsEditDeliveryAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsQuoteEditEnabled) {
                if (this.IncludeDelivery) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId) && !Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                        myResult = true;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdinaryRoutingsComponent.prototype, "IsAddDeliveryAddressEnabled", {
        get: function () {
            var myResult = false;
            if (this.IsQuoteEditEnabled) {
                if (this.IncludeDelivery) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                        myResult = true;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    OrdinaryRoutingsComponent.prototype.SetUIProperties_DeliveryFields = function () {
        var isCityRequired = false;
        var isCountryRequired = false;
        var isDeliveryAddressSubFieldsVisibile = false;
        if (this.IncludeDelivery) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.ConsigneeId)) {
                isDeliveryAddressSubFieldsVisibile = true;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.DeliveryAddressId)) {
                isDeliveryAddressSubFieldsVisibile = true;
            }
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
        this.IsDeliveryAddressSubFieldsVisibile = isDeliveryAddressSubFieldsVisibile;
        this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, isCityRequired);
        this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, isCountryRequired);
    };
    // Commands
    OrdinaryRoutingsComponent.prototype.SelectCityClicked = function (selectCityTypeCode) {
        var _this = this;
        var mySourceCountryId = selectCityTypeCode == "P" ? this.FromAddressCountryId : this.ToAddressCountryId;
        var args = new Args_1.CitySelectionArgs(this.ToAddressCountryId);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if (args.IsCitySelected) {
                if (selectCityTypeCode == "P") {
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
    OrdinaryRoutingsComponent.prototype.EditAddressClicked = function (myAddressCode) {
        var _this = this;
        var myAddressId = null;
        var myPartnerId = null;
        switch (myAddressCode) {
            case "P": {
                if (this.IncludePickUp) {
                    myPartnerId = this.ShipperId;
                    myAddressId = this.PickUpAddressId;
                }
                break;
            }
            case "D": {
                if (this.IncludeDelivery) {
                    myPartnerId = this.ConsigneeId;
                    myAddressId = this.DeliveryAddressId;
                }
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId) && !Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.Routings.EditAddress");
            logeWindow.WindowArgs = { EntityId: myAddressId, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
                        case "P": {
                            _this.LoadPickupAddress();
                            break;
                        }
                        case "D": {
                            _this.LoadDeliveryAddress();
                            break;
                        }
                    }
                }
            });
        }
    };
    OrdinaryRoutingsComponent.prototype.AddAddressClicked = function (myAddressCode) {
        var _this = this;
        var entityPM = null;
        var myPartnerId = null;
        switch (myAddressCode) {
            case "P": {
                if (this.IncludePickUp) {
                    myPartnerId = this.ShipperId;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
                        entityPM = new AddressPM_1.AddressPM();
                        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = myPartnerId;
                    }
                }
                break;
            }
            case "D": {
                if (this.IncludeDelivery) {
                    myPartnerId = this.ConsigneeId;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
                        entityPM = new AddressPM_1.AddressPM();
                        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        entityPM.AddressTypeId = "O";
                        entityPM.CardId = myPartnerId;
                    }
                }
                break;
            }
        }
        if (entityPM != null && !Tools_1.AppTool.IsNullOrEmpty(myPartnerId)) {
            var logeWindow = new LogitudeWindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.Routings.AddAddress");
            logeWindow.WindowArgs = { EntityPM: entityPM, CardId: myPartnerId };
            logeWindow.Show("./QuoteModules/QuoteTabs/Components/Routings/RoutingsAddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    switch (myAddressCode) {
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
    OrdinaryRoutingsComponent = __decorate([
        core_1.Component({
            selector: 'OrdinaryRoutingsComponent',
            moduleId: module.id,
            templateUrl: './OrdinaryRoutingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OrdinaryRoutingsComponent);
    return OrdinaryRoutingsComponent;
}(BaseComponent_1.BaseComponent));
exports.OrdinaryRoutingsComponent = OrdinaryRoutingsComponent;
//# sourceMappingURL=OrdinaryRoutingsComponent.js.map