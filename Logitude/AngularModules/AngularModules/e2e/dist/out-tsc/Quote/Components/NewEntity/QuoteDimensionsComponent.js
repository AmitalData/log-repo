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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var QuotePackagePM_1 = require("../../EntityPMs/QuotePackagePM");
var PackageTypeListService_1 = require("../../../Common/Services/StandardLists/PackageTypeListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var QuoteDimensionsComponent = /** @class */ (function () {
    function QuoteDimensionsComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.DataContext = this;
        this.ObjectTableName = "Quote";
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
    }
    QuoteDimensionsComponent.prototype.SetWindowArgs = function (entityPM) {
        var _this = this;
        this.EntityPM = entityPM;
        this.IsPackageTypeVisible = this.EntityPM.TransportModeId == "A" ? false : true;
        this.entityResourceService.getEntityResourceByTableName("QuotePackage", 0).subscribe(function (response1) {
            _this.IsResourcesReady = true;
            _this.SaveData();
            _this.SetLabels();
            _this.BuildData();
        });
    };
    QuoteDimensionsComponent.prototype.SaveData = function () {
        var _this = this;
        this.EntityPM.QuotePackages.forEach(function (item) {
            var newItem = new QuotePackagePM_1.QuotePackagePM(null);
            newItem.Id = item.Id;
            newItem.GrossWeight = item.GrossWeight;
            newItem.Height = item.Height;
            newItem.Length = item.Length;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Quantity = item.Quantity;
            newItem.QuoteId = item.QuoteId;
            newItem.Tenant = item.Tenant;
            newItem.Volume = item.Volume;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.Width = item.Width;
            _this.savedPackages.push(newItem);
        });
        this.savedVolume = this.EntityPM.Volume;
        this.savedGrossWeight = this.EntityPM.GrossWeight;
        this.savedChargeableWeight = this.EntityPM.ChargeableWeight;
        this.savedVolumetricWeight = this.EntityPM.VolumetricWeight;
        this.savedNumberOfPackages = this.EntityPM.NumberOfPackages;
    };
    QuoteDimensionsComponent.prototype.SetLabels = function () {
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Volume").replace('%VolumeCode', this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.GrossWeight").replace('%GrossWeightCode', this.EntityPM.GrossWeightUnitCode);
        this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.VolumetricWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
    };
    QuoteDimensionsComponent.prototype.BuildData = function () {
        var _this = this;
        this.ItemsSource = [];
        var list = new Array();
        this.EntityPM.QuotePackages.forEach(function (item) {
            list.push(item);
        });
        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {
                var item = new QuotePackagePM_1.QuotePackagePM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.QuoteId = this.EntityPM.Id;
                list.push(item);
            }
        }
        list.forEach(function (item) {
            var itemViewModel = new DimensionsPackageItem(item, _this);
            _this.ItemsSource.push(itemViewModel);
        });
    };
    Object.defineProperty(QuoteDimensionsComponent.prototype, "NumberOfPackages", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.NumberOfPackages) ? 0 : this.EntityPM.NumberOfPackages; },
        set: function (newVaule) {
            this.EntityPM.NumberOfPackages = newVaule;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDimensionsComponent.prototype, "GrossWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeGrossWeigh_Kg_Ton();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDimensionsComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeightUnitCode != newValue) {
                this.EntityPM.GrossWeightUnitCode = newValue;
                this.ComputeGrossWeigh_Kg_Ton();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDimensionsComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
                this.EntityPM.ChargeableWeightUnitCode = newValue;
                this.ComputeChargeableWeight_Kg();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteDimensionsComponent.prototype.ComputeGrossWeigh_Kg_Ton = function () {
        var weigh_Kg = null;
        var weigh_Ton = null;
        if (this.GrossWeight != null) {
            var factorOfConvert = 1;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                switch (this.GrossWeightUnitCode.toUpperCase()) {
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
            weigh_Kg = this.GrossWeight * factorOfConvert;
        }
        if (weigh_Kg != null) {
            weigh_Kg = Tools_1.AppTool.Round(weigh_Kg, 3);
            weigh_Ton = weigh_Kg / 1000;
        }
        if (weigh_Ton != null) {
            weigh_Ton = Tools_1.AppTool.Round(weigh_Ton, 3);
        }
        this.EntityPM.GrossWeightInKG = weigh_Kg;
        this.EntityPM.GrossWeightPerTon = weigh_Ton;
    };
    QuoteDimensionsComponent.prototype.ComputeChargeableWeight_Kg = function () {
        var weigh_Kg = null;
        var weigh_Ton = null;
        if (this.ChargeableWeight != null) {
            var factorOfConvert = 1;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ChargeableWeightUnitCode)) {
                switch (this.ChargeableWeightUnitCode.toUpperCase()) {
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
            weigh_Kg = this.ChargeableWeight * factorOfConvert;
        }
        if (weigh_Kg != null) {
            weigh_Kg = Tools_1.AppTool.Round(weigh_Kg, 3);
        }
        this.EntityPM.ChargeableWeightInKG = weigh_Kg;
    };
    Object.defineProperty(QuoteDimensionsComponent.prototype, "Volume", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolume_CBM();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteDimensionsComponent.prototype.ComputeVolume_CBM = function () {
        var volume_CBM = null;
        if (this.Volume != null) {
            var factorOfConvert = 1;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                switch (this.EntityPM.VolumeUnitCode.toUpperCase()) {
                    case "CBM": {
                        factorOfConvert = 1;
                        break;
                    }
                    case "CBI": {
                        factorOfConvert = 61024;
                        break;
                    } // 1m³ = 61024in³
                    case "CBF": {
                        factorOfConvert = 35.315;
                        break;
                    } // 1m³ = 35.315ft³
                }
            }
            volume_CBM = this.Volume / factorOfConvert;
        }
        if (volume_CBM != null) {
            volume_CBM = Tools_1.AppTool.Round(volume_CBM, 3);
        }
        this.EntityPM.VolumeInCBM = volume_CBM;
    };
    Object.defineProperty(QuoteDimensionsComponent.prototype, "VolumetricWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.VolumetricWeight) ? 0 : this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteDimensionsComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                this.EntityPM.ChargeableWeight = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeChargeableWeight_Kg();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteDimensionsComponent.prototype.ComputeTotals = function () {
        if (this.EntityPM.QuotePackages.length == 0) {
            this.Volume = null;
            this.GrossWeight = null;
            this.ChargeableWeight = null;
            this.VolumetricWeight = null;
            this.NumberOfPackages = null;
        }
        else {
            var myNumberOfPackages = 0;
            var myVolume = 0;
            var myGrossWeight = 0;
            var myVolumetricWeight = 0;
            this.EntityPM.QuotePackages.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
                    myNumberOfPackages += item.Quantity;
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
        this.NumberOfPackages = myNumberOfPackages;
        this.Volume = myVolume;
        this.VolumetricWeight = myVolumetricWeight;
        this.GrossWeight = myGrossWeight;
        this.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.GrossWeight, this.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    };
    QuoteDimensionsComponent.prototype.AddPackageClicked = function () {
        var _this = this;
        var itemPM = new QuotePackagePM_1.QuotePackagePM(null);
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.QuoteId = this.EntityPM.Id;
        var logeWindow = new LogitudeWindow_1.LogitudeWindow();
        logeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.AddPackage");
        logeWindow.WindowArgs = new DimensionsPackageItem(itemPM, this);
        logeWindow.Show("./Quote/Components/NewEntity/NewQuoteAddEditDimensionsComponent");
        logeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.ComputeTotals();
            }
        });
    };
    QuoteDimensionsComponent.prototype.DeletePackage = function (itemViewModel) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var itemIndex = _this.ItemsSource.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    _this.ItemsSource.splice(itemIndex, 1);
                }
                _this.EntityPM.RemoveQuotePackagePM(itemViewModel.EntityPM);
                _this.ComputeTotals();
            }
        });
    };
    QuoteDimensionsComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        this.EntityPM.QuotePackages = [];
        this.savedPackages.forEach(function (item) {
            _this.EntityPM.AddQuotePackagePM(item);
        });
        this.EntityPM.Volume = this.savedVolume;
        this.EntityPM.GrossWeight = this.savedGrossWeight;
        this.EntityPM.ChargeableWeight = this.savedChargeableWeight;
        this.EntityPM.VolumetricWeight = this.savedVolumetricWeight;
        this.EntityPM.NumberOfPackages = this.savedNumberOfPackages;
        this.CurrentSession.CloseCurrentWindow();
    };
    QuoteDimensionsComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    QuoteDimensionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuoteDimensionsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], QuoteDimensionsComponent);
    return QuoteDimensionsComponent;
}());
exports.QuoteDimensionsComponent = QuoteDimensionsComponent;
var DimensionsPackageItem = /** @class */ (function (_super) {
    __extends(DimensionsPackageItem, _super);
    function DimensionsPackageItem(entityPM, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "QuotePackage";
        _this.IsWindowMode = false;
        _this.IsPackageTypeVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = entityPM;
        _this.QuotePM = fatherComponent.EntityPM;
        _this.IsPackageTypeVisible = fatherComponent.IsPackageTypeVisible;
        _this.SetUIProperties();
        return _this;
    }
    DimensionsPackageItem.prototype.SetUIProperties = function () {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        if (this.Quantity > 0) {
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
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);
        if (this.IsPackageTypeVisible) {
            this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, Tools_1.AppTool.IsNullOrZero(this.GrossWeight) ? true : false);
        }
    };
    Object.defineProperty(DimensionsPackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != newValue) {
                this.EntityPM.PackageTypeId = newValue;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.EntityPM.PackageTypeName = null;
                }
                else {
                    var packageTypeService = new PackageTypeListService_1.PackageTypeListService();
                    packageTypeService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myPackageTypeList = myResponse.Result;
                            if (myPackageTypeList != null) {
                                _this.EntityPM.PackageTypeName = myPackageTypeList.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DimensionsPackageItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DimensionsPackageItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 0);
                var itemIndex = this.QuotePM.QuotePackages.indexOf(this.EntityPM);
                if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                    this.Height = null;
                    this.Length = null;
                    this.Width = null;
                    this.Volume = null;
                    this.GrossWeight = null;
                    this.VolumetricWeight = null;
                    if (!this.IsWindowMode) {
                        if (itemIndex > -1) {
                            this.QuotePM.RemoveQuotePackagePM(this.EntityPM);
                        }
                    }
                }
                else {
                    if (!this.IsWindowMode) {
                        if (itemIndex == -1) {
                            this.QuotePM.AddQuotePackagePM(this.EntityPM);
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
    Object.defineProperty(DimensionsPackageItem.prototype, "Length", {
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
    Object.defineProperty(DimensionsPackageItem.prototype, "Width", {
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
    Object.defineProperty(DimensionsPackageItem.prototype, "Height", {
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
    Object.defineProperty(DimensionsPackageItem.prototype, "Dimensions", {
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
    Object.defineProperty(DimensionsPackageItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 2);
                this.ComputeVolumetricWeight();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DimensionsPackageItem.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 2);
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DimensionsPackageItem.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 2);
            if (this.EntityPM.GrossWeight != myValue) {
                this.EntityPM.GrossWeight = myValue;
                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    DimensionsPackageItem.prototype.OnGrossWeightLostFocus = function (input) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.QuotePM.ChargeableWeightUnitCode, this.QuotePM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.QuotePM.Ratio);
                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        }
    };
    DimensionsPackageItem.prototype.ComputeVolume = function () {
        if (this.QuotePM.Ratio == null) {
            this.QuotePM.Ratio = Tools_1.AppTool.GetRatio(this.QuotePM.DirectionId, this.QuotePM.TransportModeId, this.QuotePM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.QuotePM.Ratio, this.QuotePM.DimensionsUnitCode, this.QuotePM.VolumeUnitCode, this.QuotePM.GrossWeightUnitCode);
    };
    DimensionsPackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.QuotePM.Ratio == null) {
            this.QuotePM.Ratio = Tools_1.AppTool.GetRatio(this.QuotePM.DirectionId, this.QuotePM.TransportModeId, this.QuotePM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.QuotePM.Ratio, this.QuotePM.DimensionsUnitCode, this.QuotePM.VolumeUnitCode, this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode);
    };
    return DimensionsPackageItem;
}(BaseComponent_1.BaseComponent));
exports.DimensionsPackageItem = DimensionsPackageItem;
//# sourceMappingURL=QuoteDimensionsComponent.js.map