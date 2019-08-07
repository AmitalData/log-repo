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
var ShipmentOrderPackagePM_1 = require("../../EntityPMs/ShipmentOrderPackagePM");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var PackageTypeListService_1 = require("../../../Common/Services/StandardLists/PackageTypeListService");
var WizardDimensionsComponent = /** @class */ (function () {
    function WizardDimensionsComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.DataContext = this;
        this.ObjectTableName = "Shipment";
        this.ItemsSource = [];
        this.ValidationErrorsList = [];
        this.IsResourcesReady = false;
        this.IsPackageTypeVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.savedPackages = [];
        this.savedVolume = null;
        this.savedGrossWeight = null;
        this.savedChargeableWeight = null;
        this.savedVolumetricWeight = null;
        this.savedNumberOfPackages = null;
        this.VolumeLabel = null;
        this.GrossWeightLabel = null;
        this.ChargeableWeightLabel = null;
        this.VolumetricWeightLabel = null;
        this.DimensionsColumnHeader = null;
        this.VolumeColumnHeader = null;
        this.VolumetricWeightColumnHeader = null;
        this.WeightColumnHeader = null;
    }
    WizardDimensionsComponent.prototype.SetWindowArgs = function (entityPM) {
        var _this = this;
        this.EntityPM = entityPM;
        this.IsPackageTypeVisible = this.EntityPM.TransportModeId == "A" ? false : true;
        this.entityResourceService.getEntityResourceByTableName("ShipmentOrderPackage").subscribe(function (response) {
            _this.IsResourcesReady = true;
            _this.SaveData();
            _this.SetLabels();
            _this.BuildData();
        });
    };
    WizardDimensionsComponent.prototype.SaveData = function () {
        var _this = this;
        this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
            var newItem = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(null);
            newItem.Id = item.Id;
            newItem.GrossWeight = item.GrossWeight;
            newItem.Height = item.Height;
            newItem.Length = item.Length;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Quantity = item.Quantity;
            newItem.ShipmentId = item.ShipmentId;
            newItem.Tenant = item.Tenant;
            newItem.Volume = item.Volume;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.Width = item.Width;
            newItem.ContainerTypeId = item.ContainerTypeId;
            newItem.IsContainer = item.IsContainer;
            _this.savedPackages.push(newItem);
        });
        this.savedVolume = this.EntityPM.BookingVolume;
        this.savedGrossWeight = this.EntityPM.OrderGrossWeight;
        this.savedChargeableWeight = this.EntityPM.OrderChargeableWeight;
        this.savedVolumetricWeight = this.EntityPM.OrderVolumetricWeight;
        this.savedNumberOfPackages = this.EntityPM.BookingNumberOfPackages;
    };
    WizardDimensionsComponent.prototype.SetLabels = function () {
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.BookingVolume.Short").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OrderGrossWeight.Short").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OrderChargeableWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OrderVolumetricWeight.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);
    };
    WizardDimensionsComponent.prototype.BuildData = function () {
        var _this = this;
        this.ItemsSource = [];
        var list = [];
        this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
            list.push(item);
        });
        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {
                var item = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                list.push(item);
            }
        }
        list.forEach(function (item) {
            _this.ItemsSource.push(new WizardDimensionItem(item, _this));
        });
    };
    Object.defineProperty(WizardDimensionsComponent.prototype, "BookingVolume", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingVolume) ? 0 : this.EntityPM.BookingVolume; },
        set: function (newValue) {
            if (this.EntityPM.BookingVolume != newValue) {
                this.EntityPM.BookingVolume = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionsComponent.prototype, "OrderGrossWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OrderGrossWeight) ? 0 : this.EntityPM.OrderGrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderGrossWeight != newValue) {
                this.EntityPM.OrderGrossWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionsComponent.prototype, "OrderChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OrderChargeableWeight) ? 0 : this.EntityPM.OrderChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderChargeableWeight != newValue) {
                this.EntityPM.OrderChargeableWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionsComponent.prototype, "OrderVolumetricWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OrderVolumetricWeight) ? 0 : this.EntityPM.OrderVolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.OrderVolumetricWeight != newValue) {
                this.EntityPM.OrderVolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionsComponent.prototype, "BookingNumberOfPackages", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingNumberOfPackages) ? 0 : this.EntityPM.BookingNumberOfPackages; },
        set: function (newValue) {
            if (this.EntityPM.BookingNumberOfPackages != newValue) {
                this.EntityPM.BookingNumberOfPackages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    WizardDimensionsComponent.prototype.ComputeTotals = function () {
        if (this.EntityPM.ShipmentOrderPackages.length == 0) {
            this.BookingVolume = null;
            this.OrderGrossWeight = null;
            this.OrderChargeableWeight = null;
            this.OrderVolumetricWeight = null;
            this.BookingNumberOfPackages = null;
        }
        else {
            var myQuantity = 0;
            var myVolume = 0;
            var myGrossWeight = 0;
            var myVolumetricWeight = 0;
            this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
                    myQuantity += item.Quantity;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
                    myVolumetricWeight += item.VolumetricWeight;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    myGrossWeight += item.GrossWeight;
                }
            });
        }
        this.BookingNumberOfPackages = myQuantity;
        this.BookingVolume = myVolume;
        this.OrderVolumetricWeight = myVolumetricWeight;
        this.OrderGrossWeight = Tools_1.AppTool.Round(myGrossWeight, 3);
        this.OrderChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.OrderGrossWeight, this.OrderVolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    };
    WizardDimensionsComponent.prototype.AddPackageClicked = function () {
        var _this = this;
        var itemPM = new ShipmentOrderPackagePM_1.ShipmentOrderPackagePM(null);
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.ShipmentId = this.EntityPM.Id;
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        logeWindow.Title = "Add line";
        logeWindow.WindowArgs = new WizardDimensionItem(itemPM, this);
        logeWindow.Show("./Shipment/Components/NewEntity/WizardAddEditDimensionsComponent");
        logeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.ComputeTotals();
            }
        });
    };
    WizardDimensionsComponent.prototype.DeletePackage = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var itemIndex = _this.ItemsSource.indexOf(item);
                if (itemIndex > -1) {
                    _this.ItemsSource.splice(itemIndex, 1);
                }
                _this.EntityPM.RemoveOrderPackage(item.EntityPM);
                _this.ComputeTotals();
            }
        });
    };
    WizardDimensionsComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        this.EntityPM.ShipmentOrderPackages = [];
        this.savedPackages.forEach(function (item) {
            _this.EntityPM.AddOrderPackage(item);
        });
        this.EntityPM.BookingVolume = this.savedVolume;
        this.EntityPM.OrderGrossWeight = this.savedGrossWeight;
        this.EntityPM.OrderChargeableWeight = this.savedChargeableWeight;
        this.EntityPM.OrderVolumetricWeight = this.savedVolumetricWeight;
        this.EntityPM.BookingNumberOfPackages = this.savedNumberOfPackages;
        this.CurrentSession.CloseCurrentWindow();
    };
    WizardDimensionsComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    WizardDimensionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WizardDimensionsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], WizardDimensionsComponent);
    return WizardDimensionsComponent;
}());
exports.WizardDimensionsComponent = WizardDimensionsComponent;
var WizardDimensionItem = /** @class */ (function (_super) {
    __extends(WizardDimensionItem, _super);
    function WizardDimensionItem(item, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "ShipmentOrderPackage";
        _this.IsWindowMode = false;
        _this.IsPackageTypeVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.EntityPM = item;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsPackageTypeVisible = fatherComponent.IsPackageTypeVisible;
        _this.SetUIProperties();
        return _this;
    }
    WizardDimensionItem.prototype.SetUIProperties = function () {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        this.IsEditingEnabled = true;
        if (this.IsEditingEnabled) {
            if (this.Quantity > 0 || this.hasValue) {
                isFieldEnabled = true;
                isVolumeEnabled = true;
                isDimensionEnabled = true;
                if (this.Height != null || this.Width != null || this.Length != null) {
                    isVolumeEnabled = false;
                }
                else if (this.Volume != null) {
                    isDimensionEnabled = false;
                }
            }
        }
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);
        if (this.IsPackageTypeVisible) {
            this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, Tools_1.AppTool.IsNullOrZero(this.GrossWeight) ? true : false);
        }
    };
    WizardDimensionItem.prototype.HasValue = function (hasValue) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    };
    Object.defineProperty(WizardDimensionItem.prototype, "PackageTypeId", {
        // Properties
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
    Object.defineProperty(WizardDimensionItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 0);
                var itemIndex = this.ShipmentPM.ShipmentOrderPackages.indexOf(this.EntityPM);
                if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                    this.Height = null;
                    this.Length = null;
                    this.Width = null;
                    this.Volume = null;
                    this.GrossWeight = null;
                    this.VolumetricWeight = null;
                    if (!this.IsWindowMode) {
                        if (itemIndex > -1) {
                            this.ShipmentPM.RemoveOrderPackage(this.EntityPM);
                        }
                    }
                }
                else {
                    if (!this.IsWindowMode) {
                        if (itemIndex == -1) {
                            this.ShipmentPM.AddOrderPackage(this.EntityPM);
                        }
                    }
                }
                this.SetUIProperties();
                this.ComputeVolume();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "Length", {
        get: function () { return this.EntityPM.Length; },
        set: function (newValue) {
            if (this.EntityPM.Length != newValue) {
                this.EntityPM.Length = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "Width", {
        get: function () { return this.EntityPM.Width; },
        set: function (newValue) {
            if (this.EntityPM.Width != newValue) {
                this.EntityPM.Width = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "Height", {
        get: function () { return this.EntityPM.Height; },
        set: function (newValue) {
            if (this.EntityPM.Height != newValue) {
                this.EntityPM.Height = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "Dimensions", {
        get: function () {
            var myDimensions;
            if (this.Length == null && this.Width == null && this.Height == null) {
                myDimensions = " - - ";
            }
            else {
                var myLength = 0;
                var myWidth = 0;
                var myHeight = 0;
                if (this.Length != null) {
                    myLength = this.Length;
                }
                if (this.Width != null) {
                    myWidth = this.Width;
                }
                if (this.Height != null) {
                    myHeight = this.Height;
                }
                myDimensions = myLength + "-" + myWidth + "-" + myHeight;
            }
            return myDimensions;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeVolumetricWeight();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WizardDimensionItem.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.GrossWeight != myValue) {
                this.EntityPM.GrossWeight = myValue;
                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    WizardDimensionItem.prototype.OnGrossWeightLostFocus = function (input1) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);
                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        }
    };
    WizardDimensionItem.prototype.ComputeVolume = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    };
    WizardDimensionItem.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    return WizardDimensionItem;
}(BaseComponent_1.BaseComponent));
exports.WizardDimensionItem = WizardDimensionItem;
//# sourceMappingURL=WizardDimensionsComponent.js.map