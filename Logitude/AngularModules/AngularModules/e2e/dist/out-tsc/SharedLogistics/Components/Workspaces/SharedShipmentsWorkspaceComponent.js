"use strict";
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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SharedLogisticsService_1 = require("../../Services/Others/SharedLogisticsService");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../Infrastructure/Tools");
var SharedShipmentsWorkspaceComponent = /** @class */ (function () {
    function SharedShipmentsWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.IsResourcesReady = false;
        this.mySelectedTransportFilter = "All";
        this.mySelectedDirectionFilter = "All";
        this.myService = new SharedLogisticsService_1.SharedLogisticsService();
    }
    SharedShipmentsWorkspaceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(function (response) {
            _this.IsResourcesReady = true;
            _this.SetSelectedItem();
            _this.LoadShipments();
        });
    };
    SharedShipmentsWorkspaceComponent.prototype.SetSelectedItem = function () {
        this.SelectedItem = "PROG";
    };
    Object.defineProperty(SharedShipmentsWorkspaceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedShipmentsWorkspaceComponent.prototype.RefreshButtonClicked = function () {
    };
    Object.defineProperty(SharedShipmentsWorkspaceComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (value) {
            if (this.mySelectedTransportFilter != value) {
                this.mySelectedTransportFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedShipmentsWorkspaceComponent.prototype, "SelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (value) {
            if (this.mySelectedDirectionFilter != value) {
                this.mySelectedDirectionFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedShipmentsWorkspaceComponent.prototype.LoadShipments = function () {
        var _this = this;
        this.InProgressShipmentsList = [];
        var filters = new SharedLogisticsService_1.ShipmentFilters();
        filters.PartnerId = SessionInfo_1.SessionInfo.LoggedUserCardId;
        filters.PartnerType = SessionInfo_1.SessionInfo.LoggedUserCardType;
        filters.DirectionId = this.SelectedDirectionFilter;
        filters.TransportModeId = this.SelectedTransportFilter;
        //filters.ShipmentLevelCode = ($.trim($.SelectedShipmentLevel) == "" || $.trim($.SelectedShipmentLevel) == "All") ? null : $.SelectedShipmentLevel;
        //filters.SearchField = ($.trim($.SearchText_SHI) == "" || $.trim($.SearchText_SHI) == $.watermark_SHI) ? null : $.trim($.SearchText_SHI);
        filters.PageSize = 1000000;
        filters.PageIndex = 0;
        if (this.SelectedItem == "PROG") {
            filters.IsOperationalClosed = false;
        }
        else {
            filters.IsOperationalClosed = null;
        }
        this.myService.GetSharedShipments(filters).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myList = myResponse.Result;
                    myList.forEach(function (item) {
                        _this.InProgressShipmentsList.push(new ShipmentEntity(item));
                    });
                    _this.InProgressCount = _this.InProgressShipmentsList.length;
                    if (_this.InProgressShipmentsList.length == 0) {
                        //no data
                    }
                }
            }
        });
    };
    SharedShipmentsWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SharedShipmentsWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SharedShipmentsWorkspaceComponent);
    return SharedShipmentsWorkspaceComponent;
}());
exports.SharedShipmentsWorkspaceComponent = SharedShipmentsWorkspaceComponent;
var ShipmentEntity = /** @class */ (function () {
    function ShipmentEntity(entity) {
        this.myEntity = entity;
    }
    Object.defineProperty(ShipmentEntity.prototype, "DirectionId", {
        get: function () { return this.myEntity.DirectionId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "TransportModeId", {
        get: function () { return this.myEntity.TransportModeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ShipmentNumber", {
        get: function () { return this.myEntity.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "StatusName", {
        get: function () { return this.myEntity.StatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "IncotermCode", {
        get: function () { return this.myEntity.IncotermCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ShipmentType", {
        get: function () { return this.myEntity.ShipmentType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "DescriptionOfGoods", {
        get: function () { return this.myEntity.DescriptionOfGoods; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "PartnerName", {
        get: function () {
            var result = "";
            if (this.myEntity.ShipmentLevelCode == "C") {
                result = this.myEntity.AgentName;
            }
            else {
                if (this.myEntity.DirectionId == "I") {
                    result = this.myEntity.Shipper;
                }
                else {
                    result = this.myEntity.Consignee;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromCountyCode", {
        get: function () {
            var result = "";
            if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
                result = this.myEntity.MainCarriageFromCountryCode;
            }
            else {
                result = this.myEntity.FromCountryCode;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromPortCode", {
        get: function () {
            var result = "";
            if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
                result = "";
            }
            else {
                result = this.myEntity.FromPort;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromPortName", {
        get: function () {
            var result = "";
            if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
                result = this.myEntity.MainCarriageFromCity;
            }
            else {
                result = this.myEntity.FromPortName;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToCountyCode", {
        get: function () {
            var result = "";
            if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
                result = this.myEntity.MainCarriageToCountryCode;
            }
            else {
                result = this.myEntity.ToCountryCode;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToPortCode", {
        get: function () {
            var result = "";
            if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
                result = "";
            }
            else {
                result = this.myEntity.ToPort;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToPortName", {
        get: function () {
            var result = "";
            if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
                result = this.myEntity.MainCarriageToCity;
            }
            else {
                result = this.myEntity.ToPortName;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "StatusNameAndLocation", {
        get: function () {
            var result = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.myEntity.StatusName)) {
                result = this.myEntity.StatusName;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.myEntity.StatusLocation)) {
                result += " (" + this.myEntity.StatusLocation + ")";
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "MyReference", {
        get: function () {
            var result = "";
            if (this.myEntity.ShipmentLevelCode == "C") {
                result = this.myEntity.AgentReference1;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.myEntity.AgentReference2)) {
                    result = (result == "") ? this.myEntity.AgentReference2 : result + ", " + this.myEntity.AgentReference2;
                }
            }
            else {
                result = this.myEntity.CustomerReference1;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.myEntity.CustomerReference2)) {
                    result = (result == "") ? this.myEntity.CustomerReference2 : result + ", " + this.myEntity.CustomerReference2;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromDate", {
        get: function () {
            var result = "";
            var myATD = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageATD);
            var myETD = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageETD);
            if (myATD != null) {
                result = myATD.ShortDateString;
            }
            else {
                if (myETD != null) {
                    result = myETD.ShortDateString;
                }
                else {
                    result = "No Date";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromDateColor", {
        get: function () {
            var result = "";
            if (this.myEntity.MainCarriageATD != null) {
                result = "#282E30";
            }
            else {
                if (this.myEntity.MainCarriageETD != null) {
                    result = "#282E30";
                }
                else {
                    result = "Silver";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromDateType", {
        get: function () {
            var result = "";
            if (this.myEntity.MainCarriageATD != null) {
                result = "(Actual)";
            }
            else {
                if (this.myEntity.MainCarriageETD != null) {
                    result = "(Estimate)";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromDateTypeColor", {
        get: function () {
            var result = "";
            if (this.myEntity.MainCarriageATD != null) {
                result = "#009161";
            }
            else {
                if (this.myEntity.MainCarriageETD != null) {
                    result = "#6E7172";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToDate", {
        get: function () {
            var result = "";
            var myATA = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationATA);
            var myETA = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationETA);
            if (myATA != null) {
                result = myATA.ShortDateString;
            }
            else {
                if (myETA != null) {
                    result = myETA.ShortDateString;
                }
                else {
                    result = "No Date";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToDateColor", {
        get: function () {
            var result = "";
            if (this.myEntity.MainCarriageFinalDestinationATA != null) {
                result = "#282E30";
            }
            else {
                if (this.myEntity.MainCarriageFinalDestinationETA != null) {
                    result = "#282E30";
                }
                else {
                    result = "Silver";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToDateType", {
        get: function () {
            var result = "";
            if (this.myEntity.MainCarriageFinalDestinationATA != null) {
                result = "(Actual)";
            }
            else {
                if (this.myEntity.MainCarriageFinalDestinationETA != null) {
                    result = "(Estimate)";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToDateTypeColor", {
        get: function () {
            var result = "";
            if (this.myEntity.MainCarriageFinalDestinationATA != null) {
                result = "#009161";
            }
            else {
                if (this.myEntity.MainCarriageFinalDestinationETA != null) {
                    result = "#6E7172";
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "StatusDate", {
        get: function () {
            var result = "";
            var date = Tools_1.DateTool.GetDateFormats(this.myEntity.StatusDate);
            if (date != null) {
                result = date.ShortDateString;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ReferenceLabel", {
        get: function () {
            var result = "";
            if (this.myEntity.ShipmentLevelCode == "H") {
                result = "House:";
            }
            else {
                result = "Master:";
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "Reference", {
        get: function () {
            var result = "";
            if (this.myEntity.ShipmentLevelCode == "H") {
                result = this.myEntity.House;
            }
            else {
                result = this.myEntity.LongMaster;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "FromTime", {
        get: function () {
            var result = "";
            var myATD = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageATD);
            var myETD = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageETD);
            if (myATD != null) {
                result = myATD.ShortTimeString;
            }
            else {
                if (myETD != null) {
                    result = myETD.ShortTimeString;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentEntity.prototype, "ToTime", {
        get: function () {
            var result = "";
            var myATA = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationATA);
            var myETA = Tools_1.DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationETA);
            if (myATA != null) {
                result = myATA.ShortTimeString;
            }
            else {
                if (myETA != null) {
                    result = myETA.ShortTimeString;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    return ShipmentEntity;
}());
exports.ShipmentEntity = ShipmentEntity;
//# sourceMappingURL=SharedShipmentsWorkspaceComponent.js.map