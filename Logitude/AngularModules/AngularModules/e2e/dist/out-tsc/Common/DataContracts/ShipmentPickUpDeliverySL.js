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
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../Infrastructure/Tools");
var CardListService_1 = require("../../Common/Services/StandardLists/CardListService");
var EntityListService_1 = require("../../Infrastructure/Services/EntityListService");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var ShipmentPickUpDeliverySL = /** @class */ (function (_super) {
    __extends(ShipmentPickUpDeliverySL, _super);
    function ShipmentPickUpDeliverySL() {
        var _this = _super.call(this) || this;
        _this.myCardListService = new CardListService_1.CardListService();
        _this.myEntityListService = new EntityListService_1.EntityListService();
        _this.TransportModeLists = [];
        _this.TransportModeLists.push(new ShipmentPickUpDeliveryTransportMode("BYTR", "By Truck"));
        _this.TransportModeLists.push(new ShipmentPickUpDeliveryTransportMode("BYRA", "By Rail"));
        return _this;
    }
    Object.defineProperty(ShipmentPickUpDeliverySL.prototype, "FromAddressCountryId", {
        get: function () { return this.fromAddressCountryId; },
        set: function (newValue) {
            var _this = this;
            if (this.fromAddressCountryId != newValue) {
                this.fromAddressCountryId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.FromAddressCountryCode = null;
                    this.FromAddressCountryName = null;
                }
                else {
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    this.myEntityListService.getAllFromCache("Country", filters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (!myResponse.HasError && myResponse.Result) {
                                var country = myResponse.Result.filter(function (d) { return d.Id == _this.fromAddressCountryId; })[0];
                                if (country) {
                                    _this.FromAddressCountryCode = country.Code;
                                    _this.FromAddressCountryName = country.EnglishName;
                                }
                            }
                        });
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliverySL.prototype, "ToAddressCountryId", {
        get: function () { return this.toAddressCountryId; },
        set: function (newValue) {
            var _this = this;
            if (this.toAddressCountryId != newValue) {
                this.toAddressCountryId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ToAddressCountryCode = null;
                    this.ToAddressCountryName = null;
                }
                else {
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    filters.GetAll = true;
                    filters.ForceCacheRefresh = true;
                    this.myEntityListService.getAllFromCache("Country", filters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (!myResponse.HasError && myResponse.Result) {
                                var country = myResponse.Result.filter(function (d) { return d.Id == _this.toAddressCountryId; })[0];
                                if (country) {
                                    _this.ToAddressCountryCode = country.Code;
                                    _this.ToAddressCountryName = country.EnglishName;
                                }
                            }
                        });
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPickUpDeliverySL.prototype, "FromPartnerCardId", {
        get: function () { return this.fromPartnerCardId; },
        set: function (newValue) {
            var _this = this;
            if (this.fromPartnerCardId != newValue) {
                this.fromPartnerCardId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.FromPartnerCardCode = null;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.FromPartnerCardCode = list.Code;
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
    Object.defineProperty(ShipmentPickUpDeliverySL.prototype, "ToPartnerCardId", {
        get: function () { return this.toPartnerCardId; },
        set: function (newValue) {
            var _this = this;
            if (this.toPartnerCardId != newValue) {
                this.toPartnerCardId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.ToPartnerCardCode = null;
                }
                else {
                    this.myCardListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.ToPartnerCardCode = list.Code;
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
    return ShipmentPickUpDeliverySL;
}(BaseComponent_1.BaseComponent));
exports.ShipmentPickUpDeliverySL = ShipmentPickUpDeliverySL;
var ShipmentPickUpDeliveryTransportMode = /** @class */ (function () {
    function ShipmentPickUpDeliveryTransportMode(code, name) {
        this.Code = code;
        this.Name = name;
    }
    return ShipmentPickUpDeliveryTransportMode;
}());
exports.ShipmentPickUpDeliveryTransportMode = ShipmentPickUpDeliveryTransportMode;
//# sourceMappingURL=ShipmentPickUpDeliverySL.js.map