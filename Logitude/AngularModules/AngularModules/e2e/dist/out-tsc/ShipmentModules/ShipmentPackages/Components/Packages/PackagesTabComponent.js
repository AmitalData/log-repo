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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ShipmentPackagePM_1 = require("../../../../Shipment/EntityPMs/ShipmentPackagePM");
var ShipmentPackageItemPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPackageItemPM");
var InsideShipmentPackagePM_1 = require("../../../../Shipment/EntityPMs/InsideShipmentPackagePM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var PackageTypeListService_1 = require("../../../../Common/Services/StandardLists/PackageTypeListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ShipmentDeliveryPM_1 = require("../../../../Shipment/EntityPMs/ShipmentDeliveryPM");
var ShipmentPickUpDeliveryPackagePM_1 = require("../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var WarehouseReleasePackageListExtendedService_1 = require("../../../../Warehouse/Services/ExtendedLists/WarehouseReleasePackageListExtendedService");
var PickUpDeliveryPackageHarmonizePM_1 = require("../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var PackagesTabComponent = /** @class */ (function (_super) {
    __extends(PackagesTabComponent, _super);
    function PackagesTabComponent(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = null;
        _this.DataContext = _this;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.IsResourcesReady = false;
        _this.IsContainersFUVisible = false;
        _this.IsCommodityNameVisible = false;
        _this.IsCommodityNumberVisible = false;
        _this.IsShippingInstructionsVisible = false;
        _this.IsDeletePackagesButtonVisible = false;
        _this.IsDownloadUploadPackagesVisible = false;
        _this.ReloadDetails = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.CrossDockReleasesEvent = null;
        _this.firstDigit = ",";
        _this.secondDigit = ".";
        _this.ChargeableWeightPasted = false;
        _this.GrossWeightPasted = false;
        _this.IsGroupageEntity = false;
        _this.IsInsideButtonVisible = false;
        _this.AllPackageTypes = [];
        // SetUIProperties
        _this.IsEditingEnabled = true;
        _this.IsTotalsFieldEnabled = true;
        _this.IsAddInsideButtonEnabled = false;
        _this.DimensionsDependencyProperty1 = null;
        _this.DimensionsDependencyProperty1IsList = false;
        // Measurments
        _this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
        _this.IsMeasurmentsHidden = true;
        // Generate
        _this.IsGenerateControlVisible = false;
        _this.IsGenerateButtonVisible = false;
        _this.IsGenerateButtonEnabled = false;
        _this.IsBuildButtonVisible = false;
        _this.IsBuildButtonEnabled = false;
        _this.IsNoPackagesLoadedTextVisible = false;
        _this.IsRebuildButtonVisible = false;
        _this.IsGeneratePackagesfromCrossDockReleasesButtonVisible = false;
        _this.IsGeneratePackagesfromCrossDockReleasesButtonEnabled = false;
        // Commands
        _this.SelectedRow = null;
        _this.dowonload = false;
        _this.saveAfterDeletePackages = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ObjectTableName = entityArgs.ObjectTableName;
        _this.DirectionId = _this.EntityPM.DirectionId;
        _this.TransportModeId = _this.EntityPM.TransportModeId;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.Listen();
        _this.setDigits();
        return _this;
    }
    PackagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "AWBWizardClosed") {
                    _this.SetUIProperties();
                    _this.SetGenerateData();
                    _this.BuildItemsSource();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.SetGenerateData();
                    _this.BuildItemsSource();
                    if (_this.dowonload) {
                        _this.dowonload = false;
                        _this.DownloadPackages();
                    }
                    if (_this.saveAfterDeletePackages) {
                        _this.saveAfterDeletePackages = false;
                        _this.CreatePackagesFromExcel();
                    }
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                    _this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                    _this.DirectionId = _this.EntityPM.DirectionId;
                    _this.OnResourcesReady();
                    //this.SetUIProperties();
                    //this.SetGenerateData();
                    //this.BuildItemsSource();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHPK" || tabCode == "JHPK") {
                    _this.SetUIProperties();
                }
            });
            this.CrossDockReleasesEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "CrossDockReleases") {
                    _this.SetGenerateData();
                }
            });
        }
    };
    PackagesTabComponent.prototype.ChargeableWeightPaste = function ($event) {
        this.ChargeableWeightPasted = true;
    };
    PackagesTabComponent.prototype.GrossWeightPaste = function ($event) {
        this.GrossWeightPasted = true;
    };
    PackagesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.CrossDockReleasesEvent);
    };
    PackagesTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "DELETEPACKAGES")) {
                this.IsDeletePackagesButtonVisible = true;
            }
            if (this.IsFCLEntity && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "DOWNUPLPACAKGES")) {
                this.IsDownloadUploadPackagesVisible = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "COMMODITYNUMBER")) {
                this.IsCommodityNumberVisible = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "COMMODITYNAME")) {
                this.IsCommodityNameVisible = true;
            }
            if (this.IsFCLEntity) {
                this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (res1) {
                    _this.entityResourceService.getEntityResourceByTableName("ShipmentPackageItem").subscribe(function (res2) {
                        _this.entityResourceService.getEntityResourceByTableName("InsideShipmentPackage").subscribe(function (res3) {
                            _this.IsResourcesReady = true;
                            _this.OnResourcesReady();
                        });
                    });
                });
            }
            else {
                this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (res1) {
                    _this.entityResourceService.getEntityResourceByTableName("ShipmentPackageItem").subscribe(function (res2) {
                        _this.IsResourcesReady = true;
                        _this.OnResourcesReady();
                    });
                });
            }
        }
    };
    PackagesTabComponent.prototype.OnResourcesReady = function () {
        var _this = this;
        var isInsideButtonVisible = true;
        if (this.IsFCLEntity) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentTypeId)) {
                var myShipmentTypeId = this.EntityPM.ShipmentTypeId.toUpperCase();
                if (myShipmentTypeId.indexOf("MYG") > -1) {
                    this.IsGroupageEntity = true;
                    isInsideButtonVisible = false;
                }
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Area.ContainersFU")) {
                this.IsContainersFUVisible = true;
            }
        }
        else {
            isInsideButtonVisible = false;
        }
        this.IsInsideButtonVisible = isInsideButtonVisible;
        this.SetLabels();
        this.SetUIProperties();
        this.SetGenerateData();
        this.BuildItemsSource();
        this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "UpdatePackagesTab") {
                _this.SetGenerateData();
            }
        });
        if (this.IsEditingEnabled) {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            myService.getAllFromCache().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.AllPackageTypes = myResponse.Result;
                }
            });
        }
        if (this.IsFCLEntity) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ShippingInstructions")) {
                if (this.EntityPM.TransportModeId == "O" && (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "I")) {
                    if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                        this.IsShippingInstructionsVisible = true;
                    }
                    else if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null) {
                        this.IsShippingInstructionsVisible = true;
                    }
                }
            }
        }
    };
    PackagesTabComponent.prototype.SetLabels = function () {
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeightUnitCode");
        }
        else {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.WtMsrUnitCode.Short");
        }
        if (this.IsLCLEntity) {
            this.AddButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.AddPackage");
            this.PackageTypeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
            this.QuantityLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.NumberOfPackages");
        }
        else {
            this.AddButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.AddContainer");
            this.PackageTypeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.ContainerTypeId");
            this.QuantityLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.NumberOfContainers");
        }
        this.SetAttachedLabels();
    };
    PackagesTabComponent.prototype.SetAttachedLabels = function () {
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.ChargeableWeightUnitCode);
        }
        else {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.WtMsr.Short").replace('%ChargWeightCode', this.ChargeableWeightUnitCode);
        }
        this.VolumeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Volume").replace('%VolumeCode', this.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.GrossWeight").replace('%GrossWeightCode', this.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.VolumetricWeight").replace('%ChargWeightCode', this.ChargeableWeightUnitCode);
    };
    PackagesTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, this.IsEditingEnabled);
        var isTotalsFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.ShipmentPackages.length > 0) {
                isTotalsFieldEnabled = true;
            }
        }
        this.IsTotalsFieldEnabled = isTotalsFieldEnabled;
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("AWBCommodityItemNumber", this.ObjectTableName, isTotalsFieldEnabled);
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_DimensionsUnitCode();
    };
    PackagesTabComponent.prototype.SetUIProperties_InsideButton = function () {
        var isButtonEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsFCLEntity) {
                if (this.SelectedRow != null) {
                    if (!this.SelectedRow.IsConnectedToRouting) {
                        isButtonEnabled = true;
                    }
                }
            }
        }
        this.IsAddInsideButtonEnabled = isButtonEnabled;
    };
    PackagesTabComponent.prototype.SetUIProperties_DimensionsUnitCode = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }
        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }
        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    };
    PackagesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var itemsCollection = [];
        this.EntityPM.ShipmentPackages.forEach(function (item) {
            itemsCollection.push(new ShipmentPackageItem(item, _this));
        });
        this.ItemsSource.InsertCollection(itemsCollection);
        this.SetGenerateData();
    };
    PackagesTabComponent.prototype.MeasurmentsSettingsClicked = function () {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;
        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.HideMeasurmentsSettings");
        }
        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Packages.MeasurmentsSettings");
        }
    };
    Object.defineProperty(PackagesTabComponent.prototype, "VolumeUnitCode", {
        get: function () { return this.EntityPM.VolumeUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.VolumeUnitCode != newValue) {
                this.EntityPM.VolumeUnitCode = newValue;
                this.EntityPM.DimensionsUnitCode = Tools_1.AppTool.GetDimentionsCodeFromVolumeCode(newValue);
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.SetUIProperties_DimensionsUnitCode();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "DimensionsUnitCode", {
        get: function () { return this.EntityPM.DimensionsUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.DimensionsUnitCode != newValue) {
                this.EntityPM.DimensionsUnitCode = newValue;
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeightUnitCode != newValue) {
                this.EntityPM.GrossWeightUnitCode = newValue;
                this.OnMeasurmentsSettingsChanged();
                this.ComputeGrossWeigh_Kg_Ton();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
                this.EntityPM.ChargeableWeightUnitCode = newValue;
                this.ComputeDimFactor();
                this.OnMeasurmentsSettingsChanged();
                this.ComputeChargeableWeight_Kg();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "Ratio", {
        get: function () { return this.EntityPM.Ratio; },
        set: function (newValue) {
            if (this.EntityPM.Ratio != newValue) {
                this.EntityPM.Ratio = newValue;
                this.ComputeDimFactor();
                Tools_2.ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "DimFactor", {
        get: function () { return this.EntityPM.DimFactor; },
        set: function (newValue) {
            if (this.EntityPM.DimFactor != newValue) {
                this.EntityPM.DimFactor = newValue;
                this.EntityPM.Ratio = Tools_1.AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
                Tools_2.ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    PackagesTabComponent.prototype.ComputeDimFactor = function () {
        this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    };
    PackagesTabComponent.prototype.ComputeGrossWeigh_Kg_Ton = function () {
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
    PackagesTabComponent.prototype.ComputeChargeableWeight_Kg = function () {
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
    PackagesTabComponent.prototype.OnMeasurmentsSettingsChanged = function () {
        this.SetAttachedLabels();
        Tools_2.ShipmentTool.RecalculateShipmentFields(this.EntityPM);
    };
    PackagesTabComponent.prototype.SetUIProperties_DimFactor = function () {
        var isDimFactorVisibile = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }
        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    };
    Object.defineProperty(PackagesTabComponent.prototype, "TEU", {
        // Summary
        get: function () { return this.EntityPM.TEU == null ? 0 : this.EntityPM.TEU; },
        set: function (newValue) {
            if (this.EntityPM.TEU != newValue) {
                this.EntityPM.TEU = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume == null ? 0 : this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeVolume_CBM();
            }
        },
        enumerable: true,
        configurable: true
    });
    PackagesTabComponent.prototype.ComputeVolume_CBM = function () {
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
    Object.defineProperty(PackagesTabComponent.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight == null ? 0 : this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "NumberOfPackages", {
        get: function () {
            var myResult = 0;
            if (this.IsFCLEntity) {
                myResult = this.EntityPM.NumberOfContainers == null ? 0 : this.EntityPM.NumberOfContainers;
            }
            else {
                myResult = this.EntityPM.NumberOfPackages == null ? 0 : this.EntityPM.NumberOfPackages;
            }
            return myResult;
        },
        set: function (value) {
            if (this.IsFCLEntity) {
                if (this.EntityPM.NumberOfContainers != value) {
                    this.EntityPM.NumberOfContainers = value;
                }
            }
            else {
                if (this.EntityPM.NumberOfPackages != value) {
                    this.EntityPM.NumberOfPackages = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeGrossWeigh_Kg_Ton();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "ChargeableWeight", {
        get: function () { return this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                this.EntityPM.ChargeableWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeChargeableWeight_Kg();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "GrossWeightEdited", {
        get: function () { return this.EntityPM.GrossWeightEdited; },
        set: function (value) {
            if (this.EntityPM.GrossWeightEdited != value) {
                this.EntityPM.GrossWeightEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "ChargeableWeightEdited", {
        get: function () { return this.EntityPM.ChargeableWeightEdited; },
        set: function (value) {
            if (this.EntityPM.ChargeableWeightEdited != value) {
                this.EntityPM.ChargeableWeightEdited = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "AWBCommodityItemNumber", {
        get: function () { return this.EntityPM.AWBCommodityItemNumber; },
        set: function (newValue) {
            if (this.EntityPM.AWBCommodityItemNumber != newValue) {
                this.EntityPM.AWBCommodityItemNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "IsDangerous", {
        get: function () { return this.EntityPM.IsDangerous; },
        set: function (newValue) {
            if (this.EntityPM.IsDangerous != newValue) {
                this.EntityPM.IsDangerous = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PackagesTabComponent.prototype.setDigits = function () {
        this.firstDigit = ",";
        this.secondDigit = ".";
        //switch (SessionLocator.TenantPM.NumberFormatCode) {
        //    case "CD": {
        //        this.firstDigit = ",";
        //        this.secondDigit = ".";
        //        break;
        //    }
        //    case "DC": {
        //        this.firstDigit = ".";
        //        this.secondDigit = ",";
        //        break;
        //    }
        //    case "AD": {
        //        this.firstDigit = "'";
        //        this.secondDigit = ".";
        //        break;
        //    }
        //    default:
        //        {
        //            this.firstDigit = ",";
        //            this.secondDigit = ".";
        //            break;
        //        }
        //}
    };
    PackagesTabComponent.prototype.GrossWeightLostFocus = function (input) {
        var valueComputed = 0;
        var valueInserted = 0;
        this.EntityPM.ShipmentPackages.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.Weight)) {
                valueComputed += item.Weight;
            }
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            if (this.firstDigit == ".") {
                if (!this.GrossWeightPasted) {
                    input = input.replace(/\./g, '');
                }
                input = input.replace(/,/g, ".");
            }
            else if (this.firstDigit == "'") {
                input = input.replace(/'/g, '');
            }
            else {
                input = Tools_1.AppTool.Replace(input, ",", "");
            }
            valueInserted = Number(input);
        }
        if (!valueComputed) {
            valueComputed = 0;
        }
        if (!valueInserted) {
            valueInserted = 0;
        }
        if (this.GrossWeight != valueInserted) {
            this.GrossWeightEdited = !(valueComputed == valueInserted);
            this.GrossWeight = valueInserted;
            this.ComputeTotals();
        }
    };
    PackagesTabComponent.prototype.ChargeableWeightLostFocus = function (input) {
        var valueComputed = 0;
        var valueInserted = 0;
        valueComputed = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
            if (this.firstDigit == ".") {
                if (!this.ChargeableWeightPasted) {
                    input = input.replace(/\./g, '');
                }
                input = input.replace(/,/g, ".");
            }
            else if (this.firstDigit == "'") {
                input = input.replace(/'/g, '');
            }
            else {
                input = Tools_1.AppTool.Replace(input, ",", "");
            }
            valueInserted = Number(input);
            valueInserted = Tools_1.AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        }
        if (!valueComputed) {
            valueComputed = 0;
        }
        if (!valueInserted) {
            valueInserted = 0;
        }
        if (this.ChargeableWeight != valueInserted) {
            this.ChargeableWeightEdited = !(valueComputed == valueInserted);
            this.ChargeableWeight = Tools_1.AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            this.ComputeTotals();
        }
    };
    PackagesTabComponent.prototype.ResetGrossWeightEdited = function () {
        this.GrossWeightEdited = false;
        this.ComputeTotals();
    };
    PackagesTabComponent.prototype.ResetChargeableWeightEdited = function () {
        this.ChargeableWeightEdited = false;
        this.ComputeTotals();
    };
    PackagesTabComponent.prototype.ResetTotalEditedValues = function () {
        this.GrossWeightEdited = false;
        this.ChargeableWeightEdited = false;
    };
    PackagesTabComponent.prototype.ComputeTotals = function () {
        var _this = this;
        if (this.EntityPM.ShipmentPackages.length == 0) {
            this.TEU = null;
            this.NumberOfPackages = null;
            this.GrossWeight = null;
            this.Volume = null;
            this.VolumetricWeight = null;
            this.ChargeableWeight = null;
            this.AWBCommodityItemNumber = null;
            this.GrossWeightEdited = false;
            this.ChargeableWeightEdited = false;
        }
        else {
            var myTEU = 0;
            var myQuantity = 0;
            var myVolume = 0;
            var myGrossWeight = 0;
            var myVolumetricWeight = 0;
            this.EntityPM.ShipmentPackages.forEach(function (item) {
                if (item.Quantity != null) {
                    myQuantity += item.Quantity;
                }
                if (item.Volume != null) {
                    myVolume += item.Volume;
                }
                if (item.VolumetricWeight != null) {
                    myVolumetricWeight += item.VolumetricWeight;
                }
                if (item.Weight != null) {
                    myGrossWeight += item.Weight;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.PackageTypeId)) {
                    var myPackageType = _this.AllPackageTypes.filter(function (f) { return f.Id == item.PackageTypeId; })[0];
                    if (myPackageType) {
                        if (myPackageType.TEU) {
                            myTEU += myPackageType.TEU;
                        }
                    }
                }
            });
            this.TEU = myTEU;
            this.NumberOfPackages = myQuantity;
            this.Volume = Tools_1.AppTool.Round(myVolume, 3);
            this.VolumetricWeight = Tools_1.AppTool.Round(myVolumetricWeight, 3);
            if (!this.GrossWeightEdited) {
                this.GrossWeight = Tools_1.AppTool.Round(myGrossWeight, 3);
            }
            if (!this.ChargeableWeightEdited) {
                this.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }
        this.SetUIProperties();
        Tools_2.ShipmentTool.OnShipmentQuantitiesChanged(this.EntityPM);
    };
    PackagesTabComponent.prototype.GeneratePackagesfromCrossDockReleasesButtonClicked = function () {
        var _this = this;
        if (!this.IsLCLEntity) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Only Container Packages can be added to your shipment packages");
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.YesButtonText = "Add";
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.GetWarehouseReleasePackageLists(true);
                }
            });
        }
        else
            this.GetWarehouseReleasePackageLists();
    };
    PackagesTabComponent.prototype.GetWarehouseReleasePackageLists = function (iscontainer) {
        var _this = this;
        if (iscontainer === void 0) { iscontainer = false; }
        var warehouseReleasePackageListExtendedService = new WarehouseReleasePackageListExtendedService_1.WarehouseReleasePackageListExtendedService();
        warehouseReleasePackageListExtendedService.getWarehouseReleasePackageListsByShipmentId(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var releasePackages = myResponse.Result;
                if (releasePackages && releasePackages.length > 0) {
                    if (iscontainer)
                        releasePackages = releasePackages.filter(function (d) { return d.IsContainer; });
                    if (releasePackages.length > 0) {
                        _this.GeneratePackagesFromWarehouseReleasesPackages(releasePackages);
                    }
                }
            }
        });
    };
    PackagesTabComponent.prototype.SetGenerateData = function () {
        var _this = this;
        this.IsGenerateControlVisible = this.EntityPM.ShipmentPackages.length == 0 ? true : false;
        if (this.IsGenerateControlVisible) {
            var count = 0;
            if (this.EntityPM.BookingNumberOfPackages) {
                count = this.EntityPM.BookingNumberOfPackages;
            }
            if (this.EntityPM.ShipmentLevelCode == "C") {
                this.BuildButtonLabel = "Build From " + this.EntityPM.ShipmentConsoleShipments.length + " Shipments";
                this.IsBuildButtonVisible = this.EntityPM.ShipmentConsoleShipments.length > 0 ? true : false;
                this.IsBuildButtonEnabled = true;
            }
            else {
                this.GenerateButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.GenerateFromOrderPackages").replace("%Number", count.toString());
                this.IsGenerateButtonVisible = true;
                this.IsGenerateButtonEnabled = this.EntityPM.BookingNumberOfPackages > 0 ? true : false;
            }
        }
        var isRebuildButtonVisible = false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.ShipmentPackages.length > 0) {
                    isRebuildButtonVisible = true;
                }
            }
        }
        this.IsRebuildButtonVisible = isRebuildButtonVisible;
        if (!this.IsEditingEnabled) {
            this.IsGenerateButtonEnabled = false;
            this.IsGeneratePackagesfromCrossDockReleasesButtonEnabled = false;
        }
        //Cross Dock
        this.IsGeneratePackagesfromCrossDockReleasesButtonVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CROSSDOCKS")) {
            if (this.IsGenerateButtonVisible && this.IsEditingEnabled) {
                if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                    this.GenerateCrossDockReleasesButtonLabel = "Generate from Cross Dock Releases Packages";
                    this.IsGeneratePackagesfromCrossDockReleasesButtonVisible = true;
                    var count = 0;
                    var warehouseReleasePackageListExtendedService = new WarehouseReleasePackageListExtendedService_1.WarehouseReleasePackageListExtendedService();
                    warehouseReleasePackageListExtendedService.getCheckIfShipmentHasReleasePackages(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var result = myResponse.Result;
                            if (result == true) {
                                _this.IsGeneratePackagesfromCrossDockReleasesButtonEnabled = true;
                            }
                        }
                    });
                }
            }
        }
    };
    PackagesTabComponent.prototype.GenerateButtonClicked = function () {
        if (this.IsLCLEntity) {
            this.Generate_LCL();
        }
        else {
            this.Generate_FCL();
        }
        this.RefreshPackages();
    };
    PackagesTabComponent.prototype.Generate_LCL = function () {
        var _this = this;
        if (this.EntityPM.ShipmentOrderPackages.length > 0) {
            this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
                var itemPM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                itemPM.Tenant = _this.EntityPM.Tenant;
                itemPM.ShipmentId = _this.EntityPM.Id;
                itemPM.Quantity = item.Quantity;
                itemPM.Weight = item.GrossWeight;
                itemPM.Volume = item.Volume;
                itemPM.VolumetricWeight = item.VolumetricWeight;
                itemPM.PackageTypeId = item.PackageTypeId;
                itemPM.PackageTypeName = item.PackageTypeName;
                itemPM.Width = item.Width;
                itemPM.Length = item.Length;
                itemPM.Height = item.Height;
                _this.EntityPM.AddPackage(itemPM);
            });
        }
        else if (this.EntityPM.BookingNumberOfPackages > 0) {
            var itemPM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
            itemPM.Tenant = this.EntityPM.Tenant;
            itemPM.ShipmentId = this.EntityPM.Id;
            itemPM.Quantity = this.EntityPM.BookingNumberOfPackages;
            itemPM.Weight = this.EntityPM.OrderGrossWeight;
            itemPM.Volume = this.EntityPM.BookingVolume;
            itemPM.VolumetricWeight = this.EntityPM.OrderVolumetricWeight;
            this.EntityPM.AddPackage(itemPM);
        }
    };
    PackagesTabComponent.prototype.Generate_FCL = function () {
        var _this = this;
        this.EntityPM.ShipmentOrderPackages.forEach(function (item) {
            var count = item.Quantity;
            var i = 1;
            while (i <= count) {
                var itemPM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                itemPM.Tenant = _this.EntityPM.Tenant;
                itemPM.ShipmentId = _this.EntityPM.Id;
                itemPM.Quantity = 1;
                itemPM.Weight = item.GrossWeight;
                itemPM.Volume = item.Volume;
                itemPM.VolumetricWeight = item.VolumetricWeight;
                itemPM.PackageTypeId = item.PackageTypeId;
                itemPM.PackageTypeName = item.PackageTypeName;
                itemPM.IsContainer = item.IsContainer;
                _this.EntityPM.AddPackage(itemPM);
                i++;
            }
        });
    };
    PackagesTabComponent.prototype.GeneratePackagesFromWarehouseReleasesPackages = function (allPackages) {
        var _this = this;
        if (allPackages) {
            allPackages.forEach(function (item) {
                var newPackage = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                newPackage.ShipperSeal = item.ShipperSeal;
                newPackage.Tenant = item.Tenant;
                newPackage.ShipmentId = _this.EntityPM.Id;
                newPackage.ContainerNumber = item.ContainerNumber;
                newPackage.Description = item.Description;
                newPackage.Harmonize = item.Harmonize;
                newPackage.Height = item.Height;
                newPackage.IsContainer = item.IsContainer;
                newPackage.Length = item.Length;
                newPackage.PackageTypeId = item.PackageTypeId;
                newPackage.PackageTypeName = item.PackageTypeName;
                newPackage.Quantity = item.Quantity;
                newPackage.Volume = item.Volume;
                newPackage.Weight = item.Weight;
                newPackage.Width = item.Width;
                _this.EntityPM.AddPackage(newPackage);
            });
            this.RefreshPackages();
        }
    };
    PackagesTabComponent.prototype.RefreshPackages = function () {
        this.BuildItemsSource();
        this.ComputeTotals();
        this.EntityPM.IsDangerous = this.EntityPM.OrderIsDangerouseGoods;
        if (this.IsLCLEntity) {
            if (this.ItemsSource.Length > 0) {
                if (this.EntityPM.OrderGrossWeight != null) {
                    if (this.EntityPM.OrderGrossWeight != this.GrossWeight) {
                        this.GrossWeight = this.EntityPM.OrderGrossWeight;
                        this.GrossWeightEdited = true;
                    }
                }
                if (this.EntityPM.OrderChargeableWeight != null) {
                    if (this.EntityPM.OrderChargeableWeight != this.ChargeableWeight) {
                        this.ChargeableWeight = this.EntityPM.OrderChargeableWeight;
                        this.ChargeableWeightEdited = true;
                    }
                }
            }
        }
    };
    PackagesTabComponent.prototype.BuildButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        myDomainService.GetShipmentConsolidationPackages(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var allPackages = myResponse.Result;
                    if (allPackages.length == 0) {
                        _this.IsNoPackagesLoadedTextVisible = true;
                    }
                    else {
                        _this.IsNoPackagesLoadedTextVisible = false;
                        if (_this.IsGroupageEntity) {
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.WindowArgs = { FatherComponent: _this, AllPackages: allPackages };
                            logWindow.Title = "Build Master Packages";
                            logWindow.IsFillScreen = true;
                            logWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Groupage/GroupageComponent");
                            logWindow.WindowClosed.subscribe(function (s) {
                                if (s) {
                                    _this.BuildItemsSource();
                                    _this.ComputeTotals();
                                }
                            });
                        }
                        else {
                            _this.BuildPackagesFromList(allPackages);
                        }
                    }
                }
            }
        });
    };
    PackagesTabComponent.prototype.RebuildButtonClicked = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Rebuild Packages?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.EntityPM.ShipmentPackages.length > 0) {
                    _this.EntityPM.ShipmentPackages = [];
                    _this.EntityPM.IsDirty = true;
                    _this.BuildItemsSource();
                    _this.ComputeTotals();
                }
                //this.EntityPM.ShipmentPackages.filter(f => f.OriginalShipmentPackageId != null).forEach(item => {
                //    this.EntityPM.RemovePackage(item);
                //});
                //this.EntityPM.ShipmentPackages.forEach(item => {
                //    item.InsideShipmentPackages.filter(f => f.OriginalShipmentPackageId != null).forEach(itemInside => {
                //        item.RemoveInsideShipmentPackagePM(itemInside);
                //    });
                //});
                _this.BuildButtonClicked();
            }
        });
    };
    PackagesTabComponent.prototype.BuildPackagesFromList = function (allPackages) {
        var _this = this;
        if (this.TransportModeId == "A") {
            allPackages.forEach(function (item) {
                var matchedItem = _this.EntityPM.ShipmentPackages.filter(function (f) { return f.Height == item.Height && f.Width == item.Width && f.Length == item.Length && f.PackageTypeId == item.PackageTypeId; })[0];
                if (matchedItem != null) {
                    // Quantity
                    if (Tools_1.AppTool.IsNullOrEmpty(matchedItem.Quantity)) {
                        matchedItem.Quantity = item.Quantity;
                    }
                    else {
                        matchedItem.Quantity = matchedItem.Quantity + item.Quantity;
                    }
                    // Weight
                    if (Tools_1.AppTool.IsNullOrEmpty(matchedItem.Weight)) {
                        matchedItem.Weight = item.Weight;
                    }
                    else {
                        matchedItem.Weight = matchedItem.Weight + item.Weight;
                    }
                    // Volume
                    if (Tools_1.AppTool.IsNullOrEmpty(matchedItem.Width) || Tools_1.AppTool.IsNullOrEmpty(matchedItem.Height) || Tools_1.AppTool.IsNullOrEmpty(matchedItem.Length)) {
                        matchedItem.Volume = (matchedItem.Weight * _this.EntityPM.Ratio) / 1000;
                        matchedItem.VolumetricWeight = matchedItem.Weight;
                    }
                    else {
                        matchedItem.Volume = (matchedItem.Width * matchedItem.Height * matchedItem.Length * matchedItem.Quantity) / 1000000;
                        matchedItem.VolumetricWeight = (matchedItem.Volume * 1000) / _this.EntityPM.Ratio;
                    }
                }
                else {
                    var newPackage = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                    newPackage.ShipmentId = _this.EntityPM.Id;
                    newPackage.ClassNumber = item.ClassNumber;
                    newPackage.ContainerNumber = item.ContainerNumber;
                    newPackage.Description = item.Description;
                    newPackage.FlashPoint = item.FlashPoint;
                    newPackage.Harmonize = item.Harmonize;
                    newPackage.Height = item.Height;
                    newPackage.IMDGCode = item.IMDGCode;
                    newPackage.FlashPointTemperatureUnitCode = item.FlashPointTemperatureUnitCode;
                    newPackage.IsContainer = item.IsContainer;
                    newPackage.IsDangerous = item.IsDangerous;
                    newPackage.Length = item.Length;
                    newPackage.MarksAndNumbers = item.MarksAndNumbers;
                    newPackage.MaterialDescription = item.MaterialDescription;
                    newPackage.PackageTypeId = item.PackageTypeId;
                    newPackage.PackageTypeName = item.PackageTypeName;
                    newPackage.PackagingGroup = item.PackagingGroup;
                    newPackage.Quantity = item.Quantity;
                    newPackage.ShipperSeal = item.ShipperSeal;
                    newPackage.CarrierSeal = item.CarrierSeal;
                    newPackage.SOC = item.SOC;
                    newPackage.Tare = item.Tare;
                    newPackage.Temperature = item.Temperature;
                    newPackage.Tenant = item.Tenant;
                    newPackage.UnNumber = item.UnNumber;
                    newPackage.Ventilation = item.Ventilation;
                    newPackage.Volume = item.Volume;
                    newPackage.VolumetricWeight = item.VolumetricWeight;
                    newPackage.Weight = item.Weight;
                    newPackage.Width = item.Width;
                    newPackage.OriginalShipmentPackageId = item.Id;
                    newPackage.Reference1 = item.Reference1;
                    newPackage.Reference2 = item.Reference2;
                    newPackage.Reference3 = item.Reference3;
                    newPackage.Reference4 = item.Reference4;
                    newPackage.CommodityNumber = item.CommodityNumber;
                    newPackage.CommodityName = item.CommodityName;
                    _this.EntityPM.AddPackage(newPackage);
                }
            });
        }
        else {
            allPackages.forEach(function (item) {
                var newPackage = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                newPackage.ShipmentId = _this.EntityPM.Id;
                newPackage.ClassNumber = item.ClassNumber;
                newPackage.ContainerNumber = item.ContainerNumber;
                newPackage.Description = item.Description;
                newPackage.FlashPoint = item.FlashPoint;
                newPackage.Harmonize = item.Harmonize;
                newPackage.Height = item.Height;
                newPackage.IMDGCode = item.IMDGCode;
                newPackage.FlashPointTemperatureUnitCode = item.FlashPointTemperatureUnitCode;
                newPackage.IsContainer = item.IsContainer;
                newPackage.IsDangerous = item.IsDangerous;
                newPackage.Length = item.Length;
                newPackage.MarksAndNumbers = item.MarksAndNumbers;
                newPackage.MaterialDescription = item.MaterialDescription;
                newPackage.PackageTypeId = item.PackageTypeId;
                newPackage.PackageTypeName = item.PackageTypeName;
                newPackage.PackagingGroup = item.PackagingGroup;
                newPackage.Quantity = item.Quantity;
                newPackage.ShipperSeal = item.ShipperSeal;
                newPackage.CarrierSeal = item.CarrierSeal;
                newPackage.SOC = item.SOC;
                newPackage.Tare = item.Tare;
                newPackage.Temperature = item.Temperature;
                newPackage.Tenant = item.Tenant;
                newPackage.UnNumber = item.UnNumber;
                newPackage.Ventilation = item.Ventilation;
                newPackage.Volume = item.Volume;
                newPackage.VolumetricWeight = item.VolumetricWeight;
                newPackage.Weight = item.Weight;
                newPackage.Width = item.Width;
                newPackage.OriginalShipmentPackageId = item.Id;
                newPackage.Reference1 = item.Reference1;
                newPackage.Reference2 = item.Reference2;
                newPackage.Reference3 = item.Reference3;
                newPackage.Reference4 = item.Reference4;
                newPackage.CommodityNumber = item.CommodityNumber;
                newPackage.CommodityName = item.CommodityName;
                item.InsideShipmentPackages.forEach(function (itemInside) {
                    var newInsidePackage = new InsideShipmentPackagePM_1.InsideShipmentPackagePM(null);
                    newInsidePackage.Quantity = itemInside.Quantity;
                    newInsidePackage.Height = itemInside.Height;
                    newInsidePackage.Length = itemInside.Length;
                    newInsidePackage.Width = itemInside.Width;
                    newInsidePackage.Weight = itemInside.Weight;
                    newInsidePackage.PackageTypeId = itemInside.PackageTypeId;
                    newInsidePackage.PackageTypeName = itemInside.PackageTypeName;
                    newInsidePackage.Volume = itemInside.Volume;
                    newInsidePackage.VolumetricWeight = itemInside.VolumetricWeight;
                    newInsidePackage.Tenant = itemInside.Tenant;
                    newInsidePackage.Description = itemInside.Description;
                    newInsidePackage.OriginalInsideShipmentPackageId = itemInside.Id;
                    newInsidePackage.Reference1 = itemInside.Reference1;
                    newInsidePackage.Reference2 = itemInside.Reference2;
                    newInsidePackage.Reference3 = itemInside.Reference3;
                    newInsidePackage.Reference4 = itemInside.Reference4;
                    newInsidePackage.CommodityNumber = itemInside.CommodityNumber;
                    newInsidePackage.CommodityName = itemInside.CommodityName;
                    newPackage.AddInsideShipmentPackagePM(newInsidePackage);
                });
                _this.EntityPM.AddPackage(newPackage);
            });
        }
        this.BuildItemsSource();
        this.ComputeTotals();
    };
    PackagesTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
        this.SetUIProperties_InsideButton();
    };
    PackagesTabComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            var isExpandaple = false;
            var item = myRow.rowData;
            if (item) {
                item.Row = myRow;
                if (item.InsideItemsSource.length > 0) {
                    isExpandaple = true;
                }
            }
            myRow.SetExpandaple(isExpandaple);
        }
    };
    PackagesTabComponent.prototype.AddPackageClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var itemPM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
        itemPM.NonActiveContainer = false;
        if (this.IsLCLEntity) {
            itemPM.IsContainer = false;
            itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.AddPackage");
        }
        else {
            itemPM.Quantity = 1;
            itemPM.IsContainer = true;
            itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            itemPM.TemperatureUnitCode = SessionLocator_1.SessionLocator.TenantPM.TemperatureUnitCode;
            itemPM.FlashPointTemperatureUnitCode = SessionLocator_1.SessionLocator.TenantPM.TemperatureUnitCode;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.AddContainer");
        }
        var myPath;
        if (this.TransportModeId == "A") {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditAirPackageComponent";
            logWindow.Height = 550;
        }
        else {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent";
            logWindow.Width = 940;
            logWindow.Height = 610;
        }
        var itemComponent = new ShipmentPackageItem(itemPM, this, true);
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    };
    PackagesTabComponent.prototype.EditPackageClicked = function (itemComponent) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        if (this.IsLCLEntity) {
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.EditPackage");
        }
        else {
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.EditContainer");
        }
        var myPath;
        if (this.TransportModeId == "A") {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditAirPackageComponent";
            itemComponent.CopyPackageItems();
            itemComponent.BuildPackageItems();
        }
        else {
            myPath = "./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent";
            logWindow.Width = 940;
            logWindow.Height = 610;
        }
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    };
    PackagesTabComponent.prototype.DeletePackageClicked = function (itemComponent) {
        var _this = this;
        var message = this.IsLCLEntity ? TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage") : "Delete This Container";
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(message);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (itemComponent.EntityPM.ShipmentPackageItems != null) {
                    itemComponent.EntityPM.ShipmentPackageItems.forEach(function (itemPackage) {
                        itemComponent.EntityPM.RemoveShipmentPackageItemPM(itemPackage);
                    });
                }
                if (itemComponent.EntityPM.InsideShipmentPackages != null) {
                    itemComponent.EntityPM.InsideShipmentPackages.forEach(function (itemInside) {
                        itemComponent.EntityPM.RemoveInsideShipmentPackagePM(itemInside);
                    });
                }
                _this.EntityPM.RemovePackage(itemComponent.EntityPM);
                _this.ItemsSource.Remove(itemComponent);
                _this.ComputeTotals();
                _this.SetUIProperties();
                _this.SetGenerateData();
            }
        });
    };
    PackagesTabComponent.prototype.AddInsideButtonClicked = function () {
        if (this.SelectedRow != null) {
            var itemPM = new InsideShipmentPackagePM_1.InsideShipmentPackagePM(null);
            itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            itemPM.ShipmentPackageId = this.SelectedRow.EntityPM.Id;
            var itemComponent = new InsideShipmentPackageItem(itemPM, this.SelectedRow, true);
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.DataContext = itemComponent;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("InsideShipmentPackage.O.AddInsidePackage");
            logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditInsidePackageComponent");
        }
    };
    PackagesTabComponent.prototype.EditInsideButtonClicked = function (itemComponent) {
        if (itemComponent) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.DataContext = itemComponent;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("InsideShipmentPackage.O.EditInsidePackage");
            logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditInsidePackageComponent");
        }
    };
    PackagesTabComponent.prototype.DeleteInsideButtonClicked = function (itemComponent) {
        var _this = this;
        if (itemComponent) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    itemComponent.ShipmentPackagePM.RemoveInsideShipmentPackagePM(itemComponent.EntityPM);
                    itemComponent.fatherComponent.BuildInsideItemsSource();
                    itemComponent.fatherComponent.ComputeFromInsidePackages();
                    if (itemComponent.fatherComponent.Row) {
                        var isExpandaple = false;
                        if (itemComponent.fatherComponent.InsideItemsSource.length > 0) {
                            isExpandaple = true;
                        }
                        itemComponent.fatherComponent.Row.SetExpandaple(isExpandaple);
                    }
                    if (itemComponent.ShipmentPackagePM.InsideShipmentPackages.length == 0) {
                        _this.BuildItemsSource();
                    }
                }
            });
        }
    };
    PackagesTabComponent.prototype.ChooseCommodityClicked = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'AWBCommodityItemNumber' };
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
        logitudeWindow.WindowClosed.subscribe(function (d) {
            _this.SetUIProperties();
        });
    };
    PackagesTabComponent.prototype.EditDangerouseClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.EditDangerousGoods");
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBDangerousPackageComponent");
    };
    PackagesTabComponent.prototype.SetMouseHoverRow = function (item, isRowHover) {
        if (item) {
            item.IsRowHover = isRowHover;
        }
    };
    PackagesTabComponent.prototype.ViewStatusesClicked = function (item) {
        if (item) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 950;
            logitudeWindow.Height = 595;
            logitudeWindow.IsFillScreen = true;
            logitudeWindow.Title = "Container Statuses";
            logitudeWindow.WindowArgs = { ShipmentId: this.EntityPM.Id, ContainerId: item.EntityPM.Id };
            logitudeWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/LastStatusComponent");
        }
    };
    PackagesTabComponent.prototype.DeletePackagesButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.ShipmentPackages.length > 0) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Are you sure you want to delete all packages?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.StartDelete();
                }
            });
        }
    };
    PackagesTabComponent.prototype.StartDelete = function () {
        var numberOfPackages = this.EntityPM.ShipmentPackages.length;
        for (var i = this.EntityPM.ShipmentPackages.length - 1; i >= 0; i--) {
            var shipmentPackage = this.EntityPM.ShipmentPackages[i];
            //Package items
            if (shipmentPackage.ShipmentPackageItems != null && shipmentPackage.ShipmentPackageItems.length > 0) {
                for (var j = shipmentPackage.ShipmentPackageItems.length - 1; j >= 0; j--) {
                    shipmentPackage.RemoveShipmentPackageItemPM(shipmentPackage.ShipmentPackageItems[j]);
                }
            }
            // Inside packages
            if (shipmentPackage.InsideShipmentPackages != null && shipmentPackage.InsideShipmentPackages.length > 0) {
                for (var k = shipmentPackage.InsideShipmentPackages.length - 1; k >= 0; k--) {
                    shipmentPackage.RemoveInsideShipmentPackagePM(shipmentPackage.InsideShipmentPackages[k]);
                }
            }
            // package harmonize
            if (shipmentPackage.ShipmentPackageHarmonizes != null && shipmentPackage.ShipmentPackageHarmonizes.length > 0) {
                for (var m = shipmentPackage.ShipmentPackageHarmonizes.length - 1; m >= 0; m--) {
                    shipmentPackage.RemoveShipmentPackageHarmonizePM(shipmentPackage.ShipmentPackageHarmonizes[m]);
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPackage.DeliveryId)) {
                var delivery = this.EntityPM.ShipmentDeliveries.filter(function (f) { return f.Id == shipmentPackage.DeliveryId; })[0];
                if (delivery) {
                    if (delivery.ShipmentPickUpDeliveryPackages != null && delivery.ShipmentPickUpDeliveryPackages.length > 0) {
                        for (var n = delivery.ShipmentPickUpDeliveryPackages.length - 1; n >= 0; n--) {
                            var deliveryPackage = delivery.ShipmentPickUpDeliveryPackages[n];
                            if (deliveryPackage) {
                                if (deliveryPackage.PickUpDeliveryPackageHarmonizes != null && deliveryPackage.PickUpDeliveryPackageHarmonizes.length > 0) {
                                    for (var f = deliveryPackage.PickUpDeliveryPackageHarmonizes.length - 1; f >= 0; f--) {
                                        deliveryPackage.RemovePickUpDeliveryPackageHarmonizePM(deliveryPackage.PickUpDeliveryPackageHarmonizes[f]);
                                    }
                                }
                                delivery.RemovePackage(deliveryPackage);
                            }
                        }
                    }
                    this.EntityPM.RemoveDelivery(delivery);
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPackage.EmptyContainerReturnId)) {
                var emptyContainer = this.EntityPM.ShipmentDeliveries.filter(function (f) { return f.Id == shipmentPackage.EmptyContainerReturnId; })[0];
                if (emptyContainer) {
                    if (emptyContainer.ShipmentPickUpDeliveryPackages != null && emptyContainer.ShipmentPickUpDeliveryPackages.length > 0) {
                        for (var n = emptyContainer.ShipmentPickUpDeliveryPackages.length - 1; n >= 0; n--) {
                            var deliveryPackage = emptyContainer.ShipmentPickUpDeliveryPackages[n];
                            if (deliveryPackage) {
                                if (deliveryPackage.PickUpDeliveryPackageHarmonizes != null && deliveryPackage.PickUpDeliveryPackageHarmonizes.length > 0) {
                                    for (var f = deliveryPackage.PickUpDeliveryPackageHarmonizes.length - 1; f >= 0; f--) {
                                        deliveryPackage.RemovePickUpDeliveryPackageHarmonizePM(deliveryPackage.PickUpDeliveryPackageHarmonizes[f]);
                                    }
                                }
                                emptyContainer.RemovePackage(deliveryPackage);
                            }
                        }
                    }
                    this.EntityPM.RemoveDelivery(emptyContainer);
                }
            }
            this.EntityPM.RemovePackage(shipmentPackage);
        }
        this.EntityPM.PackagesDeleted = true;
        this.EntityPM.EventNote = numberOfPackages + " packages deleted";
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        this.ComputeTotals();
        this.SetUIProperties();
        this.SetGenerateData();
    };
    PackagesTabComponent.prototype.DownloadClicked = function () {
        this.dowonload = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    PackagesTabComponent.prototype.DownloadPackages = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Downloading Packages";
        logWindow.Width = 500;
        logWindow.Height = 200;
        logWindow.Show('./ShipmentModules/ShipmentPackages/Components/Packages/DownloadPackagesFileComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            comp.Download(_this.EntityPM.Id, _this.EntityPM.ShipmentNumber);
        });
    };
    PackagesTabComponent.prototype.OnFileChanged = function (fileEvent) {
        var file = fileEvent.target.files[0];
        if (file) {
            var extension = file.name.split('.')[1];
            if (extension.includes("xls")) {
                this.SelectExcelFile(fileEvent);
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }
    };
    PackagesTabComponent.prototype.SelectExcelFile = function (fileEvent) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var file = fileEvent.target.files[0];
        if (file && file.size > 0) {
            var documentExtendedService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
            documentExtendedService.GetFileSizeAndUnit(file.size).subscribe(function (response) {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        _this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    };
    PackagesTabComponent.prototype.StartUploadingExcelFile = function (file) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    };
    PackagesTabComponent.prototype.ConvertArrayBufferToBase64 = function (file, context) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            var filter = new ShipmentDomainService_1.ExcelPackageFilter();
            filter.FileData = window.btoa(binary);
            filter.ShipmentId = context.EntityPM.Id;
            context.SendExcelToServer(filter);
        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    };
    PackagesTabComponent.prototype.SendExcelToServer = function (filter) {
        var _this = this;
        var myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        myDomainService.PostUploadExcelFile(filter).subscribe(function (response) {
            if (!response.HasError) {
                _this.packages = response.Result;
                if (_this.packages.filter(function (d) { return d.HasErrors; }).length > 0) {
                    _this.CurrentSession.StopBusyIndicator();
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("File contains errors, please validate the data and try again");
                }
                else {
                    if (_this.EntityPM.ShipmentPackages.length > 0) {
                        _this.CurrentSession.StopBusyIndicator();
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Show("Uploading packages will result in deleting existing packages and all its data");
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.saveAfterDeletePackages = true;
                                _this.StartDelete();
                                _this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                        });
                    }
                    else {
                        _this.CreatePackagesFromExcel();
                    }
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    PackagesTabComponent.prototype.CreatePackagesFromExcel = function () {
        var _this = this;
        this.packages.forEach(function (item) {
            var shipmentPackage = new ShipmentPackagePM_1.ShipmentPackagePM(null);
            shipmentPackage.Quantity = 1;
            shipmentPackage.IsContainer = true;
            shipmentPackage.Tenant = SessionLocator_1.SessionLocator.Tenant;
            shipmentPackage.TemperatureUnitCode = SessionLocator_1.SessionLocator.TenantPM.TemperatureUnitCode;
            shipmentPackage.FlashPointTemperatureUnitCode = SessionLocator_1.SessionLocator.TenantPM.TemperatureUnitCode;
            shipmentPackage.PackageTypeId = item.ContainerTypeId;
            shipmentPackage.PackageTypeCode = item.ContainerTypeCode;
            shipmentPackage.PackageTypeName = item.ContainerTypeName;
            shipmentPackage.ContainerNumber = item.ContainerNumber;
            shipmentPackage.Volume = item.Volume;
            shipmentPackage.Weight = item.GrossWeight;
            shipmentPackage.Tare = item.Tare;
            shipmentPackage.ShipperSeal = item.ShipperSeal;
            shipmentPackage.CarrierSeal = item.CarrierSeal;
            shipmentPackage.MarksAndNumbers = item.MarksAndNumbers;
            shipmentPackage.Description = item.Description;
            shipmentPackage.IsContainerRefrigerated = item.IsRefrigerated;
            var ratio = _this.EntityPM.Ratio;
            if (Tools_1.AppTool.IsNullOrZero(ratio)) {
                ratio = Tools_1.AppTool.GetRatio(_this.EntityPM.DirectionId, _this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
            }
            if (Tools_1.AppTool.IsNullOrZero(shipmentPackage.Volume)) {
                shipmentPackage.Volume = Tools_1.AppTool.ComputePackageVolume(shipmentPackage.Quantity, shipmentPackage.Width, shipmentPackage.Height, shipmentPackage.Length, shipmentPackage.Weight, ratio, _this.EntityPM.DimensionsUnitCode, _this.EntityPM.VolumeUnitCode, _this.EntityPM.GrossWeightUnitCode);
            }
            shipmentPackage.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(shipmentPackage.Quantity, shipmentPackage.Width, shipmentPackage.Height, shipmentPackage.Length, shipmentPackage.Volume, shipmentPackage.Weight, ratio, _this.EntityPM.DimensionsUnitCode, _this.EntityPM.VolumeUnitCode, _this.EntityPM.GrossWeightUnitCode, _this.EntityPM.ChargeableWeightUnitCode);
            if (item.IsRefrigerated == false) {
                shipmentPackage.NonActiveContainer = false;
            }
            _this.EntityPM.AddPackage(shipmentPackage);
        });
        this.BuildItemsSource();
        this.ResetTotalEditedValues();
        this.ComputeTotals();
        this.CurrentSession.StopBusyIndicator();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], PackagesTabComponent.prototype, "ReloadDetails", void 0);
    PackagesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PackagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PackagesTabComponent);
    return PackagesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PackagesTabComponent = PackagesTabComponent;
var ShipmentPackageItem = /** @class */ (function (_super) {
    __extends(ShipmentPackageItem, _super);
    function ShipmentPackageItem(entity, fatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "ShipmentPackage";
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.IsNewEntity = false;
        _this.InsideItemsSource = [];
        _this.IsRowHover = false;
        _this.IsCommodityNameVisible = false;
        _this.IsCommodityNumberVisible = false;
        _this.IsVehicleDetails = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = false;
        _this.IsVolumeEnabled = false;
        _this.IsBuildButtonEnabled = false;
        _this.IsEditingFieldsEnabled = false;
        _this.IsConnectedToRouting = false;
        _this.IsDeliveryConnectedWithMultiContainers = false;
        _this.WarningErrorsList = [];
        _this.ContainerNumberWarning = null;
        _this.selectedMethod = null;
        _this.MethodsList = [];
        // Inside Packages
        _this.RowDetailsHeights = 0;
        _this.maxPackageItemsLineNumber = 0;
        _this.savedItems = [];
        _this.SaveCompletedEvent = null;
        _this.EntityPM = entity;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsLCLEntity = fatherComponent.IsLCLEntity;
        _this.IsFCLEntity = fatherComponent.IsFCLEntity;
        _this.IsCommodityNumberVisible = fatherComponent.IsCommodityNumberVisible;
        _this.IsCommodityNameVisible = fatherComponent.IsCommodityNameVisible;
        _this.IsNewEntity = isNew;
        _this.SetUIProperties();
        _this.BuildInsideItemsSource();
        _this.ValidateContainerNumber(_this.ContainerNumber);
        _this.BuildPackageItems();
        _this.maxPackageItemsLineNumber = Tools_1.ArrayTool.Max(_this.PackageItemsList.Collection, "LineNumber");
        if (_this.IsNewEntity) {
            _this.SetUIProperties_Cars(false);
            _this.IsVehicleDetails = false;
        }
        return _this;
    }
    ShipmentPackageItem.prototype.SetUIProperties = function () {
        var _this = this;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        var isConnectedToRouting = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DeliveryId) || !Tools_1.AppTool.IsNullOrEmpty(this.EmptyContainerReturnId)) {
            isConnectedToRouting = true;
        }
        var isEditingFieldsEnabled = true;
        if (!this.IsEditingEnabled) {
            isEditingFieldsEnabled = false;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
            var allPackages = this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.DeliveryId; });
            if (allPackages.length > 1) {
                this.IsDeliveryConnectedWithMultiContainers = true;
            }
        }
        this.IsEditingFieldsEnabled = isEditingFieldsEnabled;
        this.IsConnectedToRouting = isConnectedToRouting;
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.SetUIProperties_Package();
        this.SetUIProperties_Container();
        this.SetUIProperties_Dangerous();
        this.SetUIProperties_BuildButton();
        this.SetUIProperties_Harmonize();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.SetUIProperties_Cars(_this.IsEditingEnabled && list.IsVehicle);
                        _this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
    };
    ShipmentPackageItem.prototype.SetUIProperties_Package = function () {
        if (this.ShipmentPM.TransportModeId == "A") {
            var isVolumeEnabled = false;
            var isDimensionEnabled = false;
            var isGrossWeightEnabled = false;
            if (this.IsEditingFieldsEnabled) {
                if (this.Quantity > 0) {
                    isVolumeEnabled = true;
                    isDimensionEnabled = true;
                    isGrossWeightEnabled = true;
                    if (this.Height != null || this.Width != null || this.Length != null) {
                        isVolumeEnabled = false;
                    }
                    else if (this.Volume != null) {
                        isDimensionEnabled = false;
                    }
                }
            }
            this.IsVolumeEnabled = isVolumeEnabled;
            this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
            this.UIProperties.SetEnabled("CommodityNumber", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference1", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference2", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference3", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference4", this.ObjectTableName, this.IsEditingFieldsEnabled);
        }
    };
    ShipmentPackageItem.prototype.SetUIProperties_Container = function () {
        if (this.ShipmentPM.TransportModeId != "A") {
            this.UIProperties.SetRequired('Weight', this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Weight) ? true : false);
            this.UIProperties.SetRequired('PackageTypeId', this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("ContainerNumber", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingFieldsEnabled);
            var isVolumeEnabled = false;
            var isDimensionEnabled = false;
            var isGrossWeightEnabled = false;
            if (this.IsEditingFieldsEnabled) {
                if (this.IsFCLEntity) {
                    isVolumeEnabled = true;
                    isDimensionEnabled = true;
                    isGrossWeightEnabled = true;
                }
                else {
                    if (this.Quantity > 0) {
                        isVolumeEnabled = true;
                        isDimensionEnabled = true;
                        isGrossWeightEnabled = true;
                        if (this.Height != null || this.Width != null || this.Length != null) {
                            isVolumeEnabled = false;
                        }
                        else if (this.Volume != null) {
                            isDimensionEnabled = false;
                        }
                    }
                }
            }
            this.IsVolumeEnabled = isVolumeEnabled;
            this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
            this.UIProperties.SetEnabled("ShipperSeal", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Tare", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("MarksAndNumbers", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference1", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference2", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference3", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("Reference4", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("CommodityNumber", this.ObjectTableName, this.IsEditingFieldsEnabled);
            var isFCLFieldEnabled = false;
            if (this.IsEditingFieldsEnabled) {
                if (this.IsFCLEntity) {
                    isFCLFieldEnabled = true;
                }
            }
            this.UIProperties.SetEnabled("VGM", this.ObjectTableName, isFCLFieldEnabled);
            this.UIProperties.SetEnabled("CarrierSeal", this.ObjectTableName, isFCLFieldEnabled);
            this.UIProperties.SetEnabled("Ventilation", this.ObjectTableName, isFCLFieldEnabled);
            var isTemperatureEnabled = false;
            if (isFCLFieldEnabled) {
                if (this.NonActiveContainer == false) {
                    isTemperatureEnabled = true;
                }
            }
            this.UIProperties.SetEnabled("Temperature", this.ObjectTableName, isTemperatureEnabled);
            this.SetUIProperties_ContainerFU();
            this.SetUIProperties_NonActiveContainer();
        }
    };
    ShipmentPackageItem.prototype.SetUIProperties_Dangerous = function () {
        if (this.ShipmentPM.TransportModeId != "A") {
            var isFieldEnabled = false;
            if (this.IsEditingFieldsEnabled) {
                if (this.EntityPM.IsDangerous) {
                    isFieldEnabled = true;
                }
            }
            this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, this.IsEditingFieldsEnabled);
            this.UIProperties.SetEnabled("ClassNumber", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("UnNumber", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("PackagingGroup", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("IMDGCode", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("FlashPoint", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("MaterialDescription", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("FlashPointTemperatureUnitCode", this.ObjectTableName, isFieldEnabled);
        }
    };
    ShipmentPackageItem.prototype.SetUIProperties_BuildButton = function () {
        if (this.ShipmentPM.TransportModeId != "A") {
            var isEnabled = false;
            if (this.IsEditingFieldsEnabled) {
                if (this.PackageTypeId != null) {
                    isEnabled = true;
                }
            }
            this.IsBuildButtonEnabled = isEnabled;
        }
    };
    ShipmentPackageItem.prototype.SetUIProperties_ContainerFU = function () {
        var isDeliveryFieldsEnabled = false;
        var isDeliveryPlacesEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsDeliveryFU) {
                if (this.IsDeliveryConnectedWithMultiContainers == false) {
                    isDeliveryFieldsEnabled = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.DeliveryId)) {
                        isDeliveryPlacesEnabled = true;
                    }
                }
            }
        }
        this.UIProperties.SetEnabled("IsDeliveryFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DeliveryETD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATD", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryETA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryATA", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryTransportModeCode", this.ObjectTableName, isDeliveryFieldsEnabled);
        this.UIProperties.SetEnabled("DeliveryFrom", this.ObjectTableName, isDeliveryPlacesEnabled);
        this.UIProperties.SetEnabled("DeliveryTo", this.ObjectTableName, isDeliveryPlacesEnabled);
        var isEmptyContainerReturnFieldsEnabled = false;
        var isEmptyContainerReturnPlacesEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsEmptyContainerReturnFU) {
                isEmptyContainerReturnFieldsEnabled = true;
                if (Tools_1.AppTool.IsNullOrEmpty(this.EmptyContainerReturnId)) {
                    isEmptyContainerReturnPlacesEnabled = true;
                }
            }
        }
        this.UIProperties.SetEnabled("IsEmptyContainerReturnFU", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATD", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnETA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnATA", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("ECRTransportModeCode", this.ObjectTableName, isEmptyContainerReturnFieldsEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnFrom", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
        this.UIProperties.SetEnabled("EmptyContainerReturnTo", this.ObjectTableName, isEmptyContainerReturnPlacesEnabled);
    };
    ShipmentPackageItem.prototype.SetUIProperties_NonActiveContainer = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.IsContainer && this.IsContainerRefrigerated) {
                isFieldEnabled = true;
            }
        }
        this.UIProperties.SetEnabled("NonActiveContainer", this.ObjectTableName, isFieldEnabled);
    };
    ShipmentPackageItem.prototype.SetUIProperties_Harmonize = function () {
        var isFieldEnabled = true;
        if (this.IsEditingEnabled) {
            isFieldEnabled = true;
            if (this.IsMultiHarmonize == true) {
                isFieldEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Harmonize", this.ObjectTableName, isFieldEnabled);
    };
    ShipmentPackageItem.prototype.SetUIProperties_Cars = function (isEnabled) {
        if (this.IsEditingEnabled && !isEnabled) {
            this.Make = null;
            this.Model = null;
            this.Color = null;
            this.Year = null;
            this.CountryId = null;
            this.ChassisNumber = null;
            this.RegistrationNumber = null;
        }
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
    };
    Object.defineProperty(ShipmentPackageItem.prototype, "PackageTypeTextCode", {
        get: function () { return this.IsLCLEntity ? "ShipmentPackage.F.PackageTypeId" : "ShipmentPackage.F.ContainerTypeId"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != value) {
                this.EntityPM.PackageTypeId = value;
                this.SetUIProperties_BuildButton();
                if (this.ShipmentPM.TransportModeId != "A") {
                    this.UIProperties.SetRequired('PackageTypeId', this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
                }
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.PackageTypeName = null;
                    this.EntityPM.PackageTypeCode = null;
                    this.IsContainer = false;
                    this.IsContainerRefrigerated = false;
                    this.NonActiveContainer = false;
                    this.SetUIProperties_NonActiveContainer();
                    this.SetUIProperties_Cars(false);
                    this.IsVehicleDetails = false;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PackageTypeName = list.EnglishName;
                                _this.EntityPM.PackageTypeCode = list.Code;
                                _this.IsContainer = list.IsContainer;
                                _this.IsContainerRefrigerated = list.IsRefrigerated;
                                if (list.IsRefrigerated == false) {
                                    _this.NonActiveContainer = false;
                                }
                                if (_this.IsNewEntity && _this.IsContainer) {
                                    _this.Quantity = 1;
                                }
                                _this.SetUIProperties_NonActiveContainer();
                                _this.SetUIProperties_Cars(list.IsVehicle);
                                _this.IsVehicleDetails = list.IsVehicle;
                            }
                            else {
                                _this.SetUIProperties_Cars(false);
                                _this.IsVehicleDetails = false;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "IsContainer", {
        get: function () { return this.EntityPM.IsContainer; },
        set: function (newValue) {
            if (this.EntityPM.IsContainer != newValue) {
                this.EntityPM.IsContainer = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "IsContainerRefrigerated", {
        get: function () { return this.EntityPM.IsContainerRefrigerated; },
        set: function (newValue) {
            if (this.EntityPM.IsContainerRefrigerated != newValue) {
                this.EntityPM.IsContainerRefrigerated = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "NonActiveContainer", {
        get: function () { return this.EntityPM.NonActiveContainer; },
        set: function (value) {
            if (this.EntityPM.NonActiveContainer != value) {
                this.EntityPM.NonActiveContainer = value;
                if (value) {
                    this.Temperature = 999;
                }
                this.SetUIProperties_Container();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        set: function (newValue) {
            if (this.EntityPM.ContainerNumber != newValue) {
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.EntityPM.ContainerNumber = newValue;
                }
                else {
                    this.EntityPM.ContainerNumber = newValue.toUpperCase();
                }
                this.ValidateContainerNumber(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPackageItem.prototype.ContainerNumberLostFocus = function (input) {
        this.ValidateContainerNumber(input);
    };
    ShipmentPackageItem.prototype.ValidateContainerNumber = function (input) {
        var warnings = [];
        var error = Tools_1.FormatTool.ValidateContainerNumber(input);
        if (!Tools_1.AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);
        }
        this.WarningErrorsList = warnings;
        this.ContainerNumberWarning = error;
    };
    Object.defineProperty(ShipmentPackageItem.prototype, "Quantity", {
        // Dimensions
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 0);
                this.ComputeVolume();
                this.fatherComponent.ComputeTotals();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Length", {
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
    Object.defineProperty(ShipmentPackageItem.prototype, "Width", {
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
    Object.defineProperty(ShipmentPackageItem.prototype, "Height", {
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
    Object.defineProperty(ShipmentPackageItem.prototype, "Dimensions", {
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
    Object.defineProperty(ShipmentPackageItem.prototype, "Volume", {
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
    Object.defineProperty(ShipmentPackageItem.prototype, "VolumetricWeight", {
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
    Object.defineProperty(ShipmentPackageItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.Weight != myValue) {
                this.EntityPM.Weight = myValue;
                if (this.ShipmentPM.TransportModeId != "A") {
                    this.UIProperties.SetRequired('Weight', this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Weight) ? true : false);
                }
                if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                    this.fatherComponent.ResetTotalEditedValues();
                    this.fatherComponent.ComputeTotals();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPackageItem.prototype.OnGrossWeightLostFocus = function (input1) {
        if (this.fatherComponent.IsLCLEntity) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
                if (this.Width == null || this.Height == null || this.Length == null) {
                    this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                    this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);
                    this.SetUIProperties();
                    if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                        this.fatherComponent.ResetTotalEditedValues();
                        this.fatherComponent.ComputeTotals();
                    }
                }
            }
        }
    };
    ShipmentPackageItem.prototype.ComputeVolume = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    };
    ShipmentPackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    Object.defineProperty(ShipmentPackageItem.prototype, "IsDangerous", {
        // IsDangerous
        get: function () { return this.EntityPM.IsDangerous; },
        set: function (newValue) {
            if (this.EntityPM.IsDangerous != newValue) {
                this.EntityPM.IsDangerous = newValue;
                this.ClassNumber = null;
                this.UnNumber = null;
                this.PackagingGroup = null;
                this.IMDGCode = null;
                this.FlashPoint = null;
                this.MaterialDescription = null;
                this.EntityPM.CeficClass = null;
                this.EntityPM.KelmerCode = null;
                this.EntityPM.ProperShippingName = null;
                this.EntityPM.EMS = null;
                this.EntityPM.MarinePollutant = false;
                this.SetUIProperties_Dangerous();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "ClassNumber", {
        get: function () { return this.EntityPM.ClassNumber; },
        set: function (newValue) {
            if (this.EntityPM.ClassNumber != newValue) {
                this.EntityPM.ClassNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "UnNumber", {
        get: function () { return this.EntityPM.UnNumber; },
        set: function (newValue) {
            if (this.EntityPM.UnNumber != newValue) {
                this.EntityPM.UnNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "PackagingGroup", {
        get: function () { return this.EntityPM.PackagingGroup; },
        set: function (newValue) {
            if (this.EntityPM.PackagingGroup != newValue) {
                this.EntityPM.PackagingGroup = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "IMDGCode", {
        get: function () { return this.EntityPM.IMDGCode; },
        set: function (newValue) {
            if (this.EntityPM.IMDGCode != newValue) {
                this.EntityPM.IMDGCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "FlashPoint", {
        get: function () { return this.EntityPM.FlashPoint; },
        set: function (newValue) {
            if (this.EntityPM.FlashPoint != newValue) {
                this.EntityPM.FlashPoint = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "FlashPointTemperatureUnitCode", {
        get: function () { return this.EntityPM.FlashPointTemperatureUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.FlashPointTemperatureUnitCode != newValue) {
                this.EntityPM.FlashPointTemperatureUnitCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "MaterialDescription", {
        get: function () { return this.EntityPM.MaterialDescription; },
        set: function (newValue) {
            if (this.EntityPM.MaterialDescription != newValue) {
                this.EntityPM.MaterialDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "ShipperSeal", {
        // Other Properties
        get: function () { return this.EntityPM.ShipperSeal; },
        set: function (newValue) {
            if (this.EntityPM.ShipperSeal != newValue) {
                this.EntityPM.ShipperSeal = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "CarrierSeal", {
        get: function () { return this.EntityPM.CarrierSeal; },
        set: function (newValue) {
            if (this.EntityPM.CarrierSeal != newValue) {
                this.EntityPM.CarrierSeal = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Tare", {
        get: function () { return this.EntityPM.Tare; },
        set: function (newValue) {
            if (this.EntityPM.Tare != newValue) {
                this.EntityPM.Tare = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Harmonize", {
        get: function () { return this.EntityPM.Harmonize; },
        set: function (newValue) {
            if (this.EntityPM.Harmonize != newValue) {
                this.EntityPM.Harmonize = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "IsMultiHarmonize", {
        get: function () { return this.EntityPM.IsMultiHarmonize; },
        set: function (newValue) {
            if (this.EntityPM.IsMultiHarmonize != newValue) {
                this.EntityPM.IsMultiHarmonize = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Temperature", {
        get: function () { return this.EntityPM.Temperature; },
        set: function (newValue) {
            if (this.EntityPM.Temperature != newValue) {
                this.EntityPM.Temperature = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Ventilation", {
        get: function () { return this.EntityPM.Ventilation; },
        set: function (newValue) {
            if (this.EntityPM.Ventilation != newValue) {
                this.EntityPM.Ventilation = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "MarksAndNumbers", {
        get: function () { return this.EntityPM.MarksAndNumbers; },
        set: function (newValue) {
            if (this.EntityPM.MarksAndNumbers != newValue) {
                this.EntityPM.MarksAndNumbers = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "SOC", {
        get: function () { return this.EntityPM.SOC; },
        set: function (newValue) {
            if (this.EntityPM.SOC != newValue) {
                this.EntityPM.SOC = Tools_1.AppTool.Round(newValue, 0);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "VGM", {
        get: function () { return this.EntityPM.VGM; },
        set: function (newValue) {
            if (this.EntityPM.VGM != newValue) {
                this.EntityPM.VGM = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "MethodUsed", {
        get: function () { return this.EntityPM.MethodUsed; },
        set: function (newValue) {
            if (this.EntityPM.MethodUsed != newValue) {
                this.EntityPM.MethodUsed = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "SelectedMethod", {
        get: function () { return this.selectedMethod; },
        set: function (value) {
            if (this.selectedMethod != value) {
                this.selectedMethod = value;
                if (value) {
                    this.MethodUsed = value.Name;
                }
                else {
                    this.MethodUsed = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    Object.defineProperty(ShipmentPackageItem.prototype, "Reference1", {
        get: function () { return this.EntityPM.Reference1; },
        set: function (newValue) {
            if (this.EntityPM.Reference1 != newValue) {
                this.EntityPM.Reference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Reference2", {
        get: function () { return this.EntityPM.Reference2; },
        set: function (newValue) {
            if (this.EntityPM.Reference2 != newValue) {
                this.EntityPM.Reference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Reference3", {
        get: function () { return this.EntityPM.Reference3; },
        set: function (newValue) {
            if (this.EntityPM.Reference3 != newValue) {
                this.EntityPM.Reference3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Reference4", {
        get: function () { return this.EntityPM.Reference4; },
        set: function (newValue) {
            if (this.EntityPM.Reference4 != newValue) {
                this.EntityPM.Reference4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "CommodityNumber", {
        get: function () { return this.EntityPM.CommodityNumber; },
        set: function (newValue) {
            if (this.EntityPM.CommodityNumber != newValue) {
                this.EntityPM.CommodityNumber = newValue;
                this.CommodityName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "CommodityName", {
        get: function () { return this.EntityPM.CommodityName; },
        set: function (newValue) {
            if (this.EntityPM.CommodityName != newValue) {
                this.EntityPM.CommodityName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "LastStatusName", {
        get: function () { return this.EntityPM.LastStatusName; },
        enumerable: true,
        configurable: true
    });
    ShipmentPackageItem.prototype.FillMethodsList = function () {
        var _this = this;
        this.MethodsList = [];
        //this.MethodsList.push({ Code: "0", Name: null });
        this.MethodsList.push({ Code: "1", Name: "Method 1" });
        this.MethodsList.push({ Code: "2", Name: "Method 2" });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MethodUsed)) {
            this.selectedMethod = this.MethodsList.filter(function (f) { return f.Name == _this.MethodUsed; })[0];
        }
    };
    ShipmentPackageItem.prototype.BuildButtonClicked = function () {
        var myResult = "";
        var importer = "";
        var importerRef1 = "";
        if (this.ShipmentPM.DirectionId == "E") {
            importer = this.ShipmentPM.ConsigneeName;
            importerRef1 = this.ShipmentPM.ConsigneeReference1;
        }
        else if (this.ShipmentPM.DirectionId == "I") {
            importer = this.ShipmentPM.ShipperName;
            importerRef1 = this.ShipmentPM.ShipperReference1;
        }
        if (this.EntityPM.IsContainer) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ContainerNumber)) {
                myResult = myResult + this.ContainerNumber + "\n";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipperSeal)) {
                myResult = myResult + "Shipper Seal: " + this.ShipperSeal + "\n";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CarrierSeal)) {
                myResult = myResult + "Carrier Seal: " + this.EntityPM.CarrierSeal + "\n";
            }
            if (this.Tare != null) {
                myResult = myResult + "Tare: " + this.Tare.toString() + "\n";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(importer)) {
                myResult = myResult + importer;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(importer)) {
                myResult = myResult + importer + "\n";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(importerRef1)) {
                myResult = myResult + "PO: " + importerRef1;
            }
        }
        this.MarksAndNumbers = myResult;
    };
    ShipmentPackageItem.prototype.BuildInsideItemsSource = function () {
        var _this = this;
        this.InsideItemsSource = [];
        this.EntityPM.InsideShipmentPackages.forEach(function (item) {
            _this.InsideItemsSource.push(new InsideShipmentPackageItem(item, _this));
        });
        this.RowDetailsHeights = (this.InsideItemsSource.length * 26) + 20 + 28;
        this.fatherComponent.ReloadDetails.emit("");
    };
    ShipmentPackageItem.prototype.ComputeFromInsidePackages = function () {
        var myWeight = 0;
        var myVolume = 0;
        this.InsideItemsSource.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.Weight)) {
                myWeight += item.Weight;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.Volume)) {
                myVolume += item.Volume;
            }
        });
        this.Weight = myWeight;
        this.Volume = myVolume;
    };
    // Package Items
    ShipmentPackageItem.prototype.BuildPackageItems = function () {
        var _this = this;
        if (this.PackageItemsList == null) {
            this.PackageItemsList = new ObservableCollection_1.ObservableCollection([]);
        }
        else {
            this.PackageItemsList.Collection.forEach(function (item) {
                _this.PackageItemsList.Clear();
            });
        }
        var itemsCollection = [];
        this.EntityPM.ShipmentPackageItems.forEach(function (item) {
            itemsCollection.push(new PackageItem(item, _this, false));
        });
        this.PackageItemsList.InsertCollection(itemsCollection);
    };
    ShipmentPackageItem.prototype.CopyPackageItems = function () {
        var _this = this;
        this.savedItems = [];
        if (this.EntityPM.ShipmentPackageItems.length > 0) {
            // this.maxPackageItemsLineNumber = this.EntityPM.ShipmentPackageItems.Max(m => m.LineNumber);
            this.EntityPM.ShipmentPackageItems.forEach(function (item) {
                _this.maxPackageItemsLineNumber = 0;
                if (item.LineNumber > _this.maxPackageItemsLineNumber) {
                    _this.maxPackageItemsLineNumber = item.LineNumber;
                }
                var packageItem = new ShipmentPackageItemPM_1.ShipmentPackageItemPM(null);
                packageItem.PackageId = item.PackageId;
                packageItem.Tenant = item.Tenant;
                packageItem.Quantity = item.Quantity;
                packageItem.LineNumber = item.LineNumber;
                packageItem.Description = item.Description;
                packageItem.GoodsValue = item.GoodsValue;
                _this.savedItems.push(packageItem);
            });
        }
    };
    ShipmentPackageItem.prototype.ResetPackageItems = function () {
        var _this = this;
        if (this.savedItems != null) {
            var items = this.EntityPM.ShipmentPackageItems;
            items.forEach(function (item) {
                var savedItem = _this.savedItems.filter(function (d) { return d.LineNumber == item.LineNumber; })[0];
                if (savedItem == null) {
                    if (_this.EntityPM.ShipmentPackageItems.indexOf(item) != -1) {
                        _this.EntityPM.RemoveShipmentPackageItemPM(item);
                    }
                }
                else {
                    item.Quantity = savedItem.Quantity;
                    item.Description = savedItem.Description;
                    item.GoodsValue = savedItem.GoodsValue;
                }
            });
            this.savedItems.forEach(function (item) {
                var list = _this.EntityPM.ShipmentPackageItems.filter(function (d) { return d.LineNumber == item.LineNumber; });
                if (list == null) {
                    _this.EntityPM.ShipmentPackageItems.push(item);
                }
            });
        }
    };
    ShipmentPackageItem.prototype.AddPackageItemMethod = function () {
        this.maxPackageItemsLineNumber += 1;
        var item = new ShipmentPackageItemPM_1.ShipmentPackageItemPM(null);
        item.Tenant = SessionLocator_1.SessionLocator.Tenant;
        item.PackageId = this.EntityPM.Id;
        item.LineNumber = this.maxPackageItemsLineNumber;
        this.PackageItemsList.Insert(new PackageItem(item, this, true));
        //this.CurrentSession.LogitudeGridHelper.ResetRowIndex();
    };
    ShipmentPackageItem.prototype.OnRowEnded = function ($event) {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //alert("Oh Yea !! " + $event + " " + this.PackageItemsList.Length);
        //if (errors != null && errors.length == 0) {
        if (($event) == this.PackageItemsList.Length) {
            this.AddPackageItemMethod();
            //this.CurrentSession.ResetRowIndex();
        }
        //}
    };
    ShipmentPackageItem.prototype.Ondblclick = function () {
        if (this.PackageItemsList.Length == 0) {
            this.AddPackageItemMethod();
        }
    };
    Object.defineProperty(ShipmentPackageItem.prototype, "IsDeliveryFU", {
        // Container Follow U\p
        get: function () { return this.EntityPM.IsDeliveryFU; },
        set: function (value) {
            if (this.EntityPM.IsDeliveryFU != value) {
                this.EntityPM.IsDeliveryFU = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryId", {
        get: function () { return this.EntityPM.DeliveryId; },
        set: function (value) {
            if (this.EntityPM.DeliveryId != value) {
                this.EntityPM.DeliveryId = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryFrom", {
        get: function () { return this.EntityPM.DeliveryFrom; },
        set: function (value) {
            if (this.EntityPM.DeliveryFrom != value) {
                this.EntityPM.DeliveryFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryTo", {
        get: function () { return this.EntityPM.DeliveryTo; },
        set: function (value) {
            if (this.EntityPM.DeliveryTo != value) {
                this.EntityPM.DeliveryTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryTransportModeCode", {
        get: function () { return this.EntityPM.DeliveryTransportModeCode; },
        set: function (value) {
            if (this.EntityPM.DeliveryTransportModeCode != value) {
                this.EntityPM.DeliveryTransportModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryETD", {
        get: function () { return this.EntityPM.DeliveryETD; },
        set: function (value) {
            if (this.EntityPM.DeliveryETD != value) {
                this.EntityPM.DeliveryETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryATD", {
        get: function () { return this.EntityPM.DeliveryATD; },
        set: function (value) {
            if (this.EntityPM.DeliveryATD != value) {
                this.EntityPM.DeliveryATD = value;
                this.SetUIProperties_ValidateActualDates_D();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryETA", {
        get: function () { return this.EntityPM.DeliveryETA; },
        set: function (value) {
            if (this.EntityPM.DeliveryETA != value) {
                this.EntityPM.DeliveryETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "DeliveryATA", {
        get: function () { return this.EntityPM.DeliveryATA; },
        set: function (value) {
            if (this.EntityPM.DeliveryATA != value) {
                this.EntityPM.DeliveryATA = value;
                this.SetUIProperties_ValidateActualDates_D();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "IsEmptyContainerReturnFU", {
        get: function () { return this.EntityPM.IsEmptyContainerReturnFU; },
        set: function (value) {
            if (this.EntityPM.IsEmptyContainerReturnFU != value) {
                this.EntityPM.IsEmptyContainerReturnFU = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnId", {
        get: function () { return this.EntityPM.EmptyContainerReturnId; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnId != value) {
                this.EntityPM.EmptyContainerReturnId = value;
                this.SetUIProperties_ContainerFU();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnFrom", {
        get: function () { return this.EntityPM.EmptyContainerReturnFrom; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnFrom != value) {
                this.EntityPM.EmptyContainerReturnFrom = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnTo", {
        get: function () { return this.EntityPM.EmptyContainerReturnTo; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnTo != value) {
                this.EntityPM.EmptyContainerReturnTo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "ECRTransportModeCode", {
        get: function () { return this.EntityPM.ECRTransportModeCode; },
        set: function (value) {
            if (this.EntityPM.ECRTransportModeCode != value) {
                this.EntityPM.ECRTransportModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnETD", {
        get: function () { return this.EntityPM.EmptyContainerReturnETD; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnETD != value) {
                this.EntityPM.EmptyContainerReturnETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnATD", {
        get: function () { return this.EntityPM.EmptyContainerReturnATD; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnATD != value) {
                this.EntityPM.EmptyContainerReturnATD = value;
                this.SetUIProperties_ValidateActualDates_R();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnETA", {
        get: function () { return this.EntityPM.EmptyContainerReturnETA; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnETA != value) {
                this.EntityPM.EmptyContainerReturnETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "EmptyContainerReturnATA", {
        get: function () { return this.EntityPM.EmptyContainerReturnATA; },
        set: function (value) {
            if (this.EntityPM.EmptyContainerReturnATA != value) {
                this.EntityPM.EmptyContainerReturnATA = value;
                this.SetUIProperties_ValidateActualDates_R();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Make", {
        get: function () { return this.EntityPM.Make; },
        set: function (newValue) {
            if (this.EntityPM.Make != newValue) {
                this.EntityPM.Make = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Model", {
        get: function () { return this.EntityPM.Model; },
        set: function (newValue) {
            if (this.EntityPM.Model != newValue) {
                this.EntityPM.Model = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Year", {
        get: function () { return this.EntityPM.Year; },
        set: function (newValue) {
            if (this.EntityPM.Year != newValue) {
                this.EntityPM.Year = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "Color", {
        get: function () { return this.EntityPM.Color; },
        set: function (newValue) {
            if (this.EntityPM.Color != newValue) {
                this.EntityPM.Color = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "ChassisNumber", {
        get: function () { return this.EntityPM.ChassisNumber; },
        set: function (newValue) {
            if (this.EntityPM.ChassisNumber != newValue) {
                this.EntityPM.ChassisNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "RegistrationNumber", {
        get: function () { return this.EntityPM.RegistrationNumber; },
        set: function (newValue) {
            if (this.EntityPM.RegistrationNumber != newValue) {
                this.EntityPM.RegistrationNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPackageItem.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPackageItem.prototype.SetUIProperties_ValidateActualDates_D = function () {
        this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.DeliveryATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD"));
            this.UIProperties.SetValidity("DeliveryATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.DeliveryATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA"));
            this.UIProperties.SetValidity("DeliveryATA", this.ObjectTableName, false, errorMessage);
        }
    };
    ShipmentPackageItem.prototype.SetUIProperties_ValidateActualDates_R = function () {
        this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.EmptyContainerReturnATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD"));
            this.UIProperties.SetValidity("EmptyContainerReturnATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.EmptyContainerReturnATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA"));
            this.UIProperties.SetValidity("EmptyContainerReturnATA", this.ObjectTableName, false, errorMessage);
        }
    };
    ShipmentPackageItem.prototype.DeliveryIconClicked = function () {
        var typeCode = "D";
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.SaveCommandCode = typeCode;
            this.EntityPM.DummyIdGuid = Tools_1.AppTool.GetNewGuid();
            this.SaveChanges();
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.DeliveryId) || this.IsDeliveryFU) {
                this.ShowFollowupWindow(typeCode, false);
            }
            else {
                this.ShowActionsWindow(typeCode);
            }
        }
    };
    ShipmentPackageItem.prototype.EmptyContainerIconClicked = function () {
        var typeCode = "R";
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.SaveCommandCode = typeCode;
            this.EntityPM.DummyIdGuid = Tools_1.AppTool.GetNewGuid();
            this.SaveChanges();
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EmptyContainerReturnId) || this.IsEmptyContainerReturnFU) {
                this.ShowFollowupWindow(typeCode, false);
            }
            else {
                this.ShowActionsWindow(typeCode);
            }
        }
    };
    ShipmentPackageItem.prototype.SaveChanges = function () {
        var _this = this;
        if (!this.SaveCompletedEvent) {
            this.SaveCompletedEvent = this.fatherComponent.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.ShipmentPM = _this.fatherComponent.entityArgs.EntityPM;
                    _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.DummyIdGuid == _this.EntityPM.DummyIdGuid; })[0];
                    _this.ShowActionsWindow(_this.SaveCommandCode);
                }
                Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                _this.SaveCompletedEvent = null;
            });
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    ShipmentPackageItem.prototype.ShowActionsWindow = function (typeCode) {
        var _this = this;
        var windowTitle = typeCode == "D" ? "Container Delivery Actions" : "Empty Container Return Actions";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { Code: typeCode };
        logWindow.ShowCloseButton = true;
        logWindow.Title = windowTitle;
        logWindow.Width = 400;
        logWindow.Height = 150;
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupActionsComponent");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                switch (s) {
                    case "Follow": {
                        _this.ShowFollowupWindow(typeCode, true);
                        break;
                    }
                    case "Routing": {
                        _this.AddRouting(typeCode);
                        break;
                    }
                }
            }
        });
    };
    ShipmentPackageItem.prototype.ShowFollowupWindow = function (typeCode, isNewFollowup) {
        var windowTitle;
        switch (typeCode) {
            case "D": {
                windowTitle = isNewFollowup ? "Add Delivery Follow up" : "Edit Delivery Follow up";
                //if (isNewFollowup) {
                //    this.IsDeliveryFU = true;
                //}
                break;
            }
            case "R": {
                windowTitle = isNewFollowup ? "Add Empty Container Return Follow up" : "Edit Empty Container Return Follow up";
                //if (isNewFollowup) {
                //    this.IsEmptyContainerReturnFU = true;
                //}
                break;
            }
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { DataContext: this, Code: typeCode, IsNewFollowup: isNewFollowup };
        logWindow.Title = windowTitle;
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWindowComponent");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
            }
        });
    };
    ShipmentPackageItem.prototype.AddRouting = function (typeCode) {
        var _this = this;
        var myDeliveryIndex = 1;
        var myWindowTitle = null;
        var myPickUpDeliveryTypeCode = null;
        switch (typeCode) {
            case "R": {
                myPickUpDeliveryTypeCode = "EMPT";
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddEmptyCR");
                if (this.ShipmentPM.ShipmentContainerReturnIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentContainerReturnIndex + 1;
                }
                break;
            }
            default: {
                myPickUpDeliveryTypeCode = "DELV";
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");
                if (this.ShipmentPM.ShipmentDeliveryIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentDeliveryIndex + 1;
                }
                break;
            }
        }
        var newDeliveryPM = new ShipmentDeliveryPM_1.ShipmentDeliveryPM(null);
        newDeliveryPM.FullResponsibility = true;
        newDeliveryPM.Tenant = this.ShipmentPM.Tenant;
        newDeliveryPM.ShipmentId = this.ShipmentPM.Id;
        newDeliveryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        newDeliveryPM.PickUpDeliveryNumber = this.ShipmentPM.ShipmentNumber + "/" + myDeliveryIndex;
        newDeliveryPM.PickUpDeliveryTypeCode = myPickUpDeliveryTypeCode;
        newDeliveryPM.ConnectedPackageId = this.EntityPM.Id;
        switch (typeCode) {
            case "D": {
                newDeliveryPM.ETD = this.DeliveryETD;
                newDeliveryPM.ATD = this.DeliveryATD;
                newDeliveryPM.ETA = this.DeliveryETA;
                newDeliveryPM.ATA = this.DeliveryATA;
                break;
            }
            case "R": {
                newDeliveryPM.ETD = this.EmptyContainerReturnETD;
                newDeliveryPM.ATD = this.EmptyContainerReturnATD;
                newDeliveryPM.ETA = this.EmptyContainerReturnETA;
                newDeliveryPM.ATA = this.EmptyContainerReturnATA;
                break;
            }
        }
        var newDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(null);
        newDeliveryPackagePM.Tenant = this.EntityPM.Tenant;
        newDeliveryPackagePM.ContainerNumber = this.EntityPM.ContainerNumber;
        newDeliveryPackagePM.Description = this.EntityPM.Description;
        newDeliveryPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
        newDeliveryPackagePM.PackageTypeName = this.EntityPM.PackageTypeName;
        newDeliveryPackagePM.Quantity = this.EntityPM.Quantity;
        newDeliveryPackagePM.Volume = this.EntityPM.Volume;
        newDeliveryPackagePM.Weight = this.EntityPM.Weight;
        newDeliveryPackagePM.ShipperSeal = this.EntityPM.ShipperSeal;
        newDeliveryPackagePM.Width = this.EntityPM.Width;
        newDeliveryPackagePM.Height = this.EntityPM.Height;
        newDeliveryPackagePM.Length = this.EntityPM.Length;
        newDeliveryPackagePM.Harmonize = this.EntityPM.Harmonize;
        newDeliveryPackagePM.OriginalShipmentPackageId = this.EntityPM.Id;
        newDeliveryPackagePM.IsMultiHarmonize = this.EntityPM.IsMultiHarmonize;
        this.EntityPM.ShipmentPackageHarmonizes.forEach(function (harmonizeItem) {
            var harmonize = new PickUpDeliveryPackageHarmonizePM_1.PickUpDeliveryPackageHarmonizePM(null);
            harmonize.Harmonize = harmonizeItem.Harmonize;
            harmonize.Tenant = harmonizeItem.Tenant;
            newDeliveryPackagePM.AddPickUpDeliveryPackageHarmonizePM(harmonize);
        });
        newDeliveryPM.AddPackage(newDeliveryPackagePM);
        var isCreatingContainerDelivery = false;
        if (typeCode == "D") {
            isCreatingContainerDelivery = true;
        }
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: newDeliveryPM, IsNewEntity: true, ContainerReturnDeliveryId: this.DeliveryId, IsCreatingContainerDelivery: isCreatingContainerDelivery };
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                // after save must refresh entities
                _this.ShipmentPM = _this.fatherComponent.EntityPM;
                if (!_this.IsNewEntity) {
                    _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.Id == _this.EntityPM.Id; })[0];
                }
                _this.fatherComponent.SetUIProperties_InsideButton();
                _this.SetUIProperties();
                _this.InsideItemsSource.forEach(function (item) {
                    item.SetUIProperties();
                });
            }
        });
        logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
    };
    ShipmentPackageItem.prototype.EditRouting = function (typeCode) {
        var _this = this;
        var myDeliveryId = null;
        var myWindowTitle = null;
        var myEditedDelivery = null;
        switch (typeCode) {
            case "R": {
                myDeliveryId = this.EmptyContainerReturnId;
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditEmptyCR");
                break;
            }
            default: {
                myDeliveryId = this.DeliveryId;
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditDelivery");
                break;
            }
        }
        var myEditedDelivery = this.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.Id == myDeliveryId; })[0];
        if (myEditedDelivery) {
            var windowTitle = myWindowTitle + ": " + myEditedDelivery.PickUpDeliveryNumber;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: myEditedDelivery, IsNewEntity: false };
            logitudeWindow.Width = 950;
            logitudeWindow.Height = 595;
            logitudeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    // after save must refresh entities
                    _this.ShipmentPM = _this.fatherComponent.EntityPM;
                    if (!_this.IsNewEntity) {
                        _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.Id == _this.EntityPM.Id; })[0];
                    }
                }
            });
            logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
        }
    };
    ShipmentPackageItem.prototype.ChooseCommodityClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'CommodityNumber', NameProperty: 'CommodityName' };
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
    };
    ShipmentPackageItem.prototype.AdvancedDangerousClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 400;
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Title = "Dangerous Goods Advanced";
        logitudeWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AdvancedDangerousGoodsComponent");
    };
    Object.defineProperty(ShipmentPackageItem.prototype, "HasContainerException", {
        get: function () { return this.EntityPM.HasContainerException; },
        set: function (value) {
            if (this.EntityPM.HasContainerException != value) {
                this.EntityPM.HasContainerException = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return ShipmentPackageItem;
}(BaseComponent_1.BaseComponent));
exports.ShipmentPackageItem = ShipmentPackageItem;
var InsideShipmentPackageItem = /** @class */ (function (_super) {
    __extends(InsideShipmentPackageItem, _super);
    function InsideShipmentPackageItem(entity, fatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "InsideShipmentPackage";
        _this.IsNewEntity = false;
        _this.IsVehicleDetails = false;
        _this.IsEditingEnabled = false;
        _this.EntityPM = entity;
        _this.ShipmentPM = fatherComponent.ShipmentPM;
        _this.ShipmentPackagePM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.CountryListService = new CountryListService_1.CountryListService();
        _this.SetUIProperties();
        if (_this.IsNewEntity) {
            _this.SetUIPropertiesOfCars(false);
            _this.IsVehicleDetails = false;
        }
        return _this;
    }
    InsideShipmentPackageItem.prototype.SetUIProperties = function () {
        var _this = this;
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        if (this.fatherComponent.IsConnectedToRouting) {
            this.IsEditingEnabled = false;
        }
        if (this.IsEditingEnabled) {
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
        }
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Reference1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Reference2", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Reference3", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Reference4", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CommodityNumber", this.ObjectTableName, this.IsEditingEnabled);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.SetUIPropertiesOfCars(_this.IsEditingEnabled && list.IsVehicle);
                        _this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
    };
    InsideShipmentPackageItem.prototype.SetUIPropertiesOfCars = function (isEnabled) {
        if (this.IsEditingEnabled && !isEnabled) {
            this.Make = null;
            this.Model = null;
            this.Color = null;
            this.Year = null;
            this.CountryId = null;
            this.ChassisNumber = null;
            this.RegistrationNumber = null;
        }
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
    };
    Object.defineProperty(InsideShipmentPackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != newValue) {
                this.EntityPM.PackageTypeId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.PackageTypeName = null;
                    this.SetUIPropertiesOfCars(false);
                    this.IsVehicleDetails = false;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PackageTypeName = list.EnglishName;
                                _this.SetUIPropertiesOfCars(list.IsVehicle);
                                _this.IsVehicleDetails = list.IsVehicle;
                            }
                            else {
                                _this.SetUIPropertiesOfCars(false);
                                _this.IsVehicleDetails = false;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 0);
                this.ComputeVolume();
                //this.fatherComponent.ComputeTotals();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Length", {
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
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Width", {
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
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Height", {
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
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Dimensions", {
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
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeVolumetricWeight();
                this.SetUIProperties();
                if (!this.IsNewEntity) {
                    this.fatherComponent.ComputeFromInsidePackages();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
                //this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.Weight != myValue) {
                this.EntityPM.Weight = myValue;
                if (!this.IsNewEntity) {
                    this.fatherComponent.ComputeFromInsidePackages();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    InsideShipmentPackageItem.prototype.OnGrossWeightLostFocus = function (input1) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);
                this.SetUIProperties();
                if (!this.IsNewEntity) {
                    this.fatherComponent.ComputeFromInsidePackages();
                }
            }
        }
    };
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Reference1", {
        get: function () { return this.EntityPM.Reference1; },
        set: function (newValue) {
            if (this.EntityPM.Reference1 != newValue) {
                this.EntityPM.Reference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Reference2", {
        get: function () { return this.EntityPM.Reference2; },
        set: function (newValue) {
            if (this.EntityPM.Reference2 != newValue) {
                this.EntityPM.Reference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Reference3", {
        get: function () { return this.EntityPM.Reference3; },
        set: function (newValue) {
            if (this.EntityPM.Reference3 != newValue) {
                this.EntityPM.Reference3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Reference4", {
        get: function () { return this.EntityPM.Reference4; },
        set: function (newValue) {
            if (this.EntityPM.Reference4 != newValue) {
                this.EntityPM.Reference4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "CommodityNumber", {
        get: function () { return this.EntityPM.CommodityNumber; },
        set: function (newValue) {
            if (this.EntityPM.CommodityNumber != newValue) {
                this.EntityPM.CommodityNumber = newValue;
                this.CommodityName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "CommodityName", {
        get: function () { return this.EntityPM.CommodityName; },
        set: function (newValue) {
            if (this.EntityPM.CommodityName != newValue) {
                this.EntityPM.CommodityName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    InsideShipmentPackageItem.prototype.ComputeVolume = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    };
    InsideShipmentPackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Make", {
        get: function () { return this.EntityPM.Make; },
        set: function (newValue) {
            if (this.EntityPM.Make != newValue) {
                this.EntityPM.Make = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Model", {
        get: function () { return this.EntityPM.Model; },
        set: function (newValue) {
            if (this.EntityPM.Model != newValue) {
                this.EntityPM.Model = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Year", {
        get: function () { return this.EntityPM.Year; },
        set: function (newValue) {
            if (this.EntityPM.Year != newValue) {
                this.EntityPM.Year = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "Color", {
        get: function () { return this.EntityPM.Color; },
        set: function (newValue) {
            if (this.EntityPM.Color != newValue) {
                this.EntityPM.Color = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "ChassisNumber", {
        get: function () { return this.EntityPM.ChassisNumber; },
        set: function (newValue) {
            if (this.EntityPM.ChassisNumber != newValue) {
                this.EntityPM.ChassisNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "RegistrationNumber", {
        get: function () { return this.EntityPM.RegistrationNumber; },
        set: function (newValue) {
            if (this.EntityPM.RegistrationNumber != newValue) {
                this.EntityPM.RegistrationNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideShipmentPackageItem.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
                this.CountryListService.getSingle(this.EntityPM.CountryId).subscribe(function (result) {
                    var country = result.Result;
                    if (country != null) {
                        _this.EntityPM.CountryCode = country.Code;
                        _this.EntityPM.CountryName = country.EnglishName;
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    InsideShipmentPackageItem.prototype.ChooseCommodityClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'CommodityNumber', NameProperty: 'CommodityName' };
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
    };
    return InsideShipmentPackageItem;
}(BaseComponent_1.BaseComponent));
exports.InsideShipmentPackageItem = InsideShipmentPackageItem;
var PackageItem = /** @class */ (function (_super) {
    __extends(PackageItem, _super);
    function PackageItem(entity, fatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "ShipmentPackageItem";
        _this.IsNewEntity = false;
        _this.EntityPM = entity;
        _this.IsNewEntity = isNew;
        _this.SetButtonHandler();
        return _this;
    }
    PackageItem.prototype.SetButtonHandler = function () {
    };
    Object.defineProperty(PackageItem.prototype, "Description", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.Description;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackageItem.prototype, "Quantity", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.Quantity;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackageItem.prototype, "GoodsValue", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.GoodsValue;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.EntityPM.GoodsValue != newValue) {
                this.EntityPM.GoodsValue = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PackageItem.prototype.RemoveLine = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this item ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.fatherComponent.EntityPM.ShipmentPackageItems.indexOf(_this.EntityPM) != -1) {
                    _this.fatherComponent.EntityPM.RemoveShipmentPackageItemPM(_this.EntityPM);
                }
                if (_this.fatherComponent.PackageItemsList.Collection.indexOf(_this) != -1) {
                    _this.fatherComponent.PackageItemsList.Remove(_this);
                }
            }
        });
    };
    return PackageItem;
}(BaseComponent_1.BaseComponent));
exports.PackageItem = PackageItem;
var MethodItem = /** @class */ (function () {
    function MethodItem() {
    }
    return MethodItem;
}());
//# sourceMappingURL=PackagesTabComponent.js.map