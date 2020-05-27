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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var PackageTypeListService_1 = require("../../../../../Common/Services/StandardLists/PackageTypeListService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var GroupageComponent_1 = require("./GroupageComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var GroupageContainerComponent = /** @class */ (function (_super) {
    __extends(GroupageContainerComponent, _super);
    function GroupageContainerComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.ShipmentPM = null;
        _this.FatherComponent = null;
        _this.ObjectTableName = "ShipmentPackage";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isContainerNumberExists = false;
        return _this;
    }
    GroupageContainerComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.FatherComponent = args['FatherComponent'];
        this.ShipmentListItem = args['ShipmentListItem'];
        this.ShipmentPM = this.FatherComponent.EntityPM;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ContainerNumber)) {
            this.isContainerNumberExists = true;
        }
        this.SetLabels();
        this.SetUIProperties();
    };
    GroupageContainerComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    };
    GroupageContainerComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("ContainerNumber", this.ObjectTableName, !this.isContainerNumberExists);
        this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
        this.UIProperties.SetRequired("Weight", this.ObjectTableName, Tools_1.AppTool.IsNullOrZero(this.Weight) ? true : false);
    };
    Object.defineProperty(GroupageContainerComponent.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != value) {
                this.EntityPM.PackageTypeId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.PackageTypeName = null;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PackageTypeName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageContainerComponent.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageContainerComponent.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        set: function (value) {
            if (this.EntityPM.ContainerNumber != value) {
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.ContainerNumber = value;
                }
                else {
                    this.EntityPM.ContainerNumber = value.toUpperCase();
                }
                this.ValidateContainerNumber(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    GroupageContainerComponent.prototype.ContainerNumberLostFocus = function (input) {
        this.ValidateContainerNumber(input);
    };
    GroupageContainerComponent.prototype.ValidateContainerNumber = function (input) {
        var warnings = [];
        var error = Tools_1.FormatTool.ValidateContainerNumber(input);
        if (!Tools_1.AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);
        }
        this.WarningErrorsList = warnings;
    };
    Object.defineProperty(GroupageContainerComponent.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (value) {
            if (this.EntityPM.Weight != value) {
                this.EntityPM.Weight = Tools_1.AppTool.Round(value, 3);
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageContainerComponent.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (value) {
            if (this.EntityPM.Volume != value) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(value, 3);
                this.ComputeVolumetricWeight();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageContainerComponent.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (value) {
            if (this.EntityPM.VolumetricWeight != value) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(value, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageContainerComponent.prototype, "Tare", {
        get: function () { return this.EntityPM.Tare; },
        set: function (value) {
            if (this.EntityPM.Tare != value) {
                this.EntityPM.Tare = Tools_1.AppTool.Round(value, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageContainerComponent.prototype, "ShipperSeal", {
        get: function () { return this.EntityPM.ShipperSeal; },
        set: function (value) {
            if (this.EntityPM.ShipperSeal != value) {
                this.EntityPM.ShipperSeal = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    GroupageContainerComponent.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.EntityPM.Quantity, this.EntityPM.Width, this.EntityPM.Height, this.EntityPM.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    GroupageContainerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    GroupageContainerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            errors.push("Container Type is required");
        }
        if (Tools_1.AppTool.IsNullOrZero(this.Weight)) {
            errors.push("Gross Weight is required");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ContainerNumber)) {
            if (this.FatherComponent.MyGroupagePackages.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.ContainerNumber) && d.ContainerNumber.toUpperCase() == _this.ContainerNumber.toUpperCase(); }).length > 0) {
                errors.push("This container number is already added");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var indexOfItem = this.FatherComponent.ShipmentsPackages.indexOf(this.ShipmentListItem);
            if (indexOfItem > -1) {
                this.FatherComponent.ShipmentsPackages.splice(indexOfItem, 1);
            }
            var newItem = new GroupageComponent_1.GroupageListItem(this.EntityPM, this.FatherComponent, true);
            this.FatherComponent.MyGroupagePackages.push(newItem);
            this.FatherComponent.BuildToggleItems();
            newItem.UpdateItem();
            newItem.ComputeFromInsidePackages();
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    GroupageContainerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GroupageContainerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GroupageContainerComponent);
    return GroupageContainerComponent;
}(BaseComponent_1.BaseComponent));
exports.GroupageContainerComponent = GroupageContainerComponent;
//# sourceMappingURL=GroupageContainerComponent.js.map