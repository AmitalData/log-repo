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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SplitPartialPackageComponent = /** @class */ (function (_super) {
    __extends(SplitPartialPackageComponent, _super);
    function SplitPartialPackageComponent() {
        var _this = _super.call(this) || this;
        _this.ItemPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "ShipmentPackage";
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.TransportModeId = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsPackageItemsMessageVisible = false;
        _this._quantity = null;
        _this._volume = null;
        _this._weight = null;
        return _this;
    }
    SplitPartialPackageComponent.prototype.SetWindowArgs = function (args) {
        this.ItemPM = args['Item'];
        if (this.ItemPM) {
            this.EntityPM = this.ItemPM.EntityPM;
            this.ShipmentPM = this.ItemPM.fatherComponent.EntityPM;
            this.IsLCLEntity = this.ItemPM.fatherComponent.IsLCLEntity;
            this.IsFCLEntity = this.ItemPM.fatherComponent.IsFCLEntity;
            this.TransportModeId = this.ShipmentPM.TransportModeId;
            this.Quantity = this.ItemPM.Quantity;
            this.Volume = this.ItemPM.Volume;
            this.Weight = this.ItemPM.Weight;
            this.SetLabels();
            this.SetUIProperties();
        }
    };
    SplitPartialPackageComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.ShipmentPM.ChargeableWeightUnitCode);
    };
    SplitPartialPackageComponent.prototype.SetUIProperties = function () {
        //var isQuantityValid: boolean = true;
        if (this.TransportModeId != "A") {
            this.UIProperties.SetRequired('Weight', this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Weight) ? true : false);
        }
        if (this.EntityPM.ShipmentPackageItems.length > 0) {
            this.IsPackageItemsMessageVisible = true;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Volume)) {
            this.UIProperties.SetEnabled('Volume', this.ObjectTableName, false);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Weight)) {
            this.UIProperties.SetEnabled('Weight', this.ObjectTableName, false);
        }
    };
    Object.defineProperty(SplitPartialPackageComponent.prototype, "Quantity", {
        get: function () { return this._quantity; },
        set: function (value) {
            if (this._quantity != value) {
                this._quantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitPartialPackageComponent.prototype, "Volume", {
        get: function () { return this._volume; },
        set: function (value) {
            if (this._volume != value) {
                this._volume = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitPartialPackageComponent.prototype, "Weight", {
        get: function () { return this._weight; },
        set: function (value) {
            if (this._weight != value) {
                this._weight = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    SplitPartialPackageComponent.prototype.OnGrossWeightLostFocus = function (input1) {
    };
    SplitPartialPackageComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SplitPartialPackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        // Quantity
        if (Tools_1.AppTool.IsNullOrEmpty(this.Quantity)) {
            errors.push("Quantity field is required");
        }
        else {
            if (this.Quantity >= this.ItemPM.Quantity) {
                errors.push("Quantity should be less than " + this.ItemPM.Quantity);
            }
        }
        // Volume
        if (this.ItemPM.Volume != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Volume)) {
                errors.push("Volume field is required");
            }
            else {
                if (this.Volume >= this.ItemPM.Volume) {
                    errors.push("Volume should be less than " + this.ItemPM.Volume);
                }
            }
        }
        // Weight
        if (this.ItemPM.Weight != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Weight)) {
                if (this.TransportModeId != "A") {
                    errors.push("Weight field is required");
                }
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemPM.Weight)) {
                    if (this.Weight >= this.ItemPM.Weight) {
                        errors.push("Weight should be less than " + this.ItemPM.Weight);
                    }
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    };
    SplitPartialPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SplitPartialPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SplitPartialPackageComponent);
    return SplitPartialPackageComponent;
}(BaseComponent_1.BaseComponent));
exports.SplitPartialPackageComponent = SplitPartialPackageComponent;
//# sourceMappingURL=SplitPartialPackageComponent.js.map