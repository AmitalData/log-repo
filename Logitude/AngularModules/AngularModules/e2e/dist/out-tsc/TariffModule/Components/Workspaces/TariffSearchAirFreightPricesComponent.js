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
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var TariffDomainService_1 = require("../../../TariffModule/Services/TariffDomainService");
var TariffSearchAirFreightPricesComponent = /** @class */ (function (_super) {
    __extends(TariffSearchAirFreightPricesComponent, _super);
    function TariffSearchAirFreightPricesComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "TariffLine";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AvailableTariffs = [];
        _this.weightCode = "KG";
        _this.myDomainService = new TariffDomainService_1.TariffDomainService();
        _this.SetUIProperties();
        _this.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        return _this;
    }
    Object.defineProperty(TariffSearchAirFreightPricesComponent.prototype, "OriginPortId", {
        get: function () {
            return this.originPortId;
        },
        set: function (value) {
            if (this.originPortId != value) {
                this.originPortId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffSearchAirFreightPricesComponent.prototype, "DestinationPortId", {
        get: function () {
            return this.destinationPortId;
        },
        set: function (value) {
            if (this.destinationPortId != value) {
                this.destinationPortId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffSearchAirFreightPricesComponent.prototype, "Date", {
        get: function () {
            return this.date;
        },
        set: function (value) {
            if (this.date != value) {
                this.date = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffSearchAirFreightPricesComponent.prototype, "WeightCode", {
        get: function () {
            return this.weightCode;
        },
        set: function (value) {
            if (this.weightCode != value) {
                this.weightCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffSearchAirFreightPricesComponent.prototype, "Weight", {
        get: function () {
            return this.weight;
        },
        set: function (value) {
            if (this.weight != value) {
                this.weight = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffSearchAirFreightPricesComponent.prototype.SearchButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId)) {
            this.ValidationErrorsList.push("From port is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId)) {
            this.ValidationErrorsList.push("To port is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Date)) {
            this.ValidationErrorsList.push("Date is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Weight)) {
            this.ValidationErrorsList.push("Weight is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.WeightCode)) {
            this.ValidationErrorsList.push("Weight unit is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var computedWeight = this.ComputeWeightInKG(this.Weight);
            this.myDomainService.GetAvailableAirlineFreightTariffs(this.OriginPortId, this.DestinationPortId, this.Date, computedWeight).subscribe(function (res) {
                if (!res.HasError) {
                    if (res.Result) {
                        _this.AvailableTariffs = res.Result;
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    TariffSearchAirFreightPricesComponent.prototype.ComputeWeightInKG = function (weight) {
        var weigh_Kg = null;
        var weigh_Ton = null;
        if (this.Weight != null) {
            var factorOfConvert = 1;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.WeightCode)) {
                switch (this.WeightCode.toUpperCase()) {
                    case "KG": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "LB": {
                        factorOfConvert = 0.45359237;
                        break;
                    }
                    case "MT": {
                        factorOfConvert = 1000;
                        break;
                    }
                }
            }
            weigh_Kg = this.Weight * factorOfConvert;
        }
        if (weigh_Kg != null) {
            weigh_Kg = Tools_1.AppTool.Round(weigh_Kg, 3);
        }
        return weigh_Kg;
    };
    TariffSearchAirFreightPricesComponent.prototype.PriceClick = function (item) {
        if (item) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: item.Id,
                    ObjectTableName: "Tariff",
                    SelectedTabCode: item.VersionId,
                });
            });
        }
    };
    TariffSearchAirFreightPricesComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId));
        this.UIProperties.SetRequired("Date", null, Tools_1.AppTool.IsNullOrEmpty(this.Date));
        this.UIProperties.SetRequired("Weight", null, Tools_1.AppTool.IsNullOrEmpty(this.Weight));
    };
    TariffSearchAirFreightPricesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TariffSearchAirFreightPricesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TariffSearchAirFreightPricesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TariffSearchAirFreightPricesComponent);
    return TariffSearchAirFreightPricesComponent;
}(BaseComponent_1.BaseComponent));
exports.TariffSearchAirFreightPricesComponent = TariffSearchAirFreightPricesComponent;
//# sourceMappingURL=TariffSearchAirFreightPricesComponent.js.map