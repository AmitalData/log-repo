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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ShipmentPackagePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentPackagePM");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ShipmentDomainService_1 = require("../../../../../Shipment/Services/ShipmentDomainService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var AWBPackagesTabComponent = /** @class */ (function (_super) {
    __extends(AWBPackagesTabComponent, _super);
    function AWBPackagesTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.TabSummaryAreaHeight = 150;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.firstDigit = ",";
        _this.secondDigit = ".";
        _this.ChargeableWeightPasted = false;
        _this.GrossWeightPasted = false;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.IsTotalsFieldEnabled = false;
        // Validate
        _this.ShowWarning_GrossWeight = false;
        _this.ShowWarning_ChargeableWeight = false;
        _this.ShowWarning_AWBCommodityItemNumber = false;
        _this.ShowWarning_DescriptionOfGoods = false;
        // Rebuild
        _this.IsRebuildButtonVisible = false;
        // Generate
        _this.IsGeneratingVisible = false;
        _this.IsBuildFromShipmentsVisible = false;
        _this.IsNoPackagesLoadedTextVisible = false;
        _this.BuildFromShipmentsLabel = null;
        _this.DomainService = new ShipmentDomainService_1.ShipmentDomainService();
        _this.setDigits();
        return _this;
    }
    AWBPackagesTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.ShipmentLevelCode = this.Wizard.ShipmentLevelCode;
        this.SetLabels();
        this.BuildData();
        this.Listen();
        this.Validate();
        this.SetUIProperties();
    };
    AWBPackagesTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.SetUIProperties();
        this.SetRebuildButton();
        this.SetGenerateButton();
    };
    AWBPackagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.BuildData();
                    _this.RefreshTab();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.BuildData();
                    _this.RefreshTab();
                }
            });
        }
    };
    AWBPackagesTabComponent.prototype.ChargeableWeightPaste = function ($event) {
        this.ChargeableWeightPasted = true;
    };
    AWBPackagesTabComponent.prototype.GrossWeightPaste = function ($event) {
        this.GrossWeightPasted = true;
    };
    AWBPackagesTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isFieldEnabled = this.IsEditingEnabled;
        var isFieldVisible = !this.IsMultipleCommodities;
        var tabSummaryAreaHeight = 150;
        if (isFieldEnabled) {
            isFieldEnabled = false;
            if (this.EntityPM.IsMultipleCommodities) {
                tabSummaryAreaHeight = 120;
                if (this.EntityPM.ShipmentCommodities != null) {
                    if (this.EntityPM.ShipmentCommodities.length > 0) {
                        isFieldEnabled = true;
                    }
                }
            }
            else {
                tabSummaryAreaHeight = 150;
                if (this.EntityPM.ShipmentPackages != null) {
                    if (this.EntityPM.ShipmentPackages.length > 0) {
                        isFieldEnabled = true;
                    }
                }
            }
        }
        this.TabSummaryAreaHeight = tabSummaryAreaHeight;
        this.IsTotalsFieldEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("AWBCommodityItemNumber", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetVisibility("AWBCommodityItemNumber", this.ObjectTableName, isFieldVisible);
        this.UIProperties.SetVisibility("DescriptionOfGoods", this.ObjectTableName, isFieldVisible);
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    AWBPackagesTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_PAC();
        this.Wizard.ValidateScreen_FRE();
        this.Wizard.ValidateScreen_GEN();
    };
    AWBPackagesTabComponent.prototype.Validate = function () {
        if (!this.Wizard.IsImportWizard) {
            var isShowWarning_GrossWeight = false;
            var isShowWarning_ChargeableWeight = false;
            var isShowWarning_AWBCommodityItemNumber = false;
            var isShowWarning_DescriptionOfGoods = false;
            if (Tools_1.AppTool.IsNullOrZero(this.GrossWeight)) {
                isShowWarning_GrossWeight = true;
            }
            if (this.Wizard.IsFWB) {
                if (Tools_1.AppTool.IsNullOrZero(this.ChargeableWeight)) {
                    isShowWarning_ChargeableWeight = true;
                }
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
                    isShowWarning_DescriptionOfGoods = true;
                }
            }
            if (!this.IsMultipleCommodities) {
                if (this.Wizard.IsFWB) {
                    if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.MainCarriageCarrierCode == "AR") {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
                            isShowWarning_DescriptionOfGoods = true;
                        }
                    }
                    if (!Tools_1.FormatTool.Validate_CommodityNo(this.AWBCommodityItemNumber)) {
                        isShowWarning_AWBCommodityItemNumber = true;
                    }
                    if (!isShowWarning_AWBCommodityItemNumber) {
                        var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBCommodityItemNumber"; })[0];
                        if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBCommodityItemNumber)) {
                            isShowWarning_AWBCommodityItemNumber = true;
                        }
                    }
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
                        isShowWarning_DescriptionOfGoods = true;
                    }
                }
                if (!isShowWarning_DescriptionOfGoods) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "DescriptionOfGoods"; })[0];
                    if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.DescriptionOfGoods)) {
                        isShowWarning_DescriptionOfGoods = true;
                    }
                }
            }
        }
        this.ShowWarning_GrossWeight = isShowWarning_GrossWeight;
        this.ShowWarning_ChargeableWeight = isShowWarning_ChargeableWeight;
        this.ShowWarning_AWBCommodityItemNumber = isShowWarning_AWBCommodityItemNumber;
        this.ShowWarning_DescriptionOfGoods = isShowWarning_DescriptionOfGoods;
    };
    AWBPackagesTabComponent.prototype.SetLabels = function () {
        this.VolumeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.Volume").replace('%VolumeCode', this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.GrossWeight").replace('%GrossWeightCode', this.EntityPM.GrossWeightUnitCode);
        this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.VolumetricWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
    };
    AWBPackagesTabComponent.prototype.SetRebuildButton = function () {
        var isRebuildButtonVisible = false;
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0 && this.EntityPM.ShipmentPackages.length > 0) {
            isRebuildButtonVisible = true;
        }
        this.IsRebuildButtonVisible = isRebuildButtonVisible;
    };
    AWBPackagesTabComponent.prototype.RebuildClicked = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Rebuild Packages?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.EntityPM.ShipmentPackages.length > 0) {
                    _this.EntityPM.ShipmentPackages = [];
                    _this.EntityPM.IsDirty = true;
                    _this.ItemsSource = [];
                    _this.ComputeTotals();
                    _this.SetUIProperties();
                    _this.Validate();
                    _this.FireWizardEvent();
                    _this.SetRebuildButton();
                    _this.SetGenerateButton();
                }
                //var list = this.EntityPM.ShipmentPackages.filter(f => f.OriginalShipmentPackageId != null);
                //list.forEach(item => {
                //    this.EntityPM.RemovePackage(item);
                //});
                _this.GenerateClicked();
            }
        });
    };
    AWBPackagesTabComponent.prototype.SetGenerateButton = function () {
        var isGeneratingVisible = false;
        var isBuildFromShipmentsVisible = false;
        if (this.IsMultipleCommodities == false) {
            if (this.ItemsSource.length == 0) {
                isGeneratingVisible = true;
            }
            if (this.EntityPM.ShipmentLevelCode == "C") {
                if (this.EntityPM.ShipmentConsoleShipments.length > 0) {
                    isBuildFromShipmentsVisible = true;
                }
            }
        }
        this.BuildFromShipmentsLabel = "Build From " + this.EntityPM.ShipmentConsoleShipments.length + " Shipments";
        this.IsGeneratingVisible = isGeneratingVisible;
        this.IsBuildFromShipmentsVisible = isBuildFromShipmentsVisible;
    };
    AWBPackagesTabComponent.prototype.GenerateClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DomainService.GetShipmentConsolidationPackages(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var allPackages = myResponse.Result;
                    if (allPackages.length == 0) {
                        _this.IsNoPackagesLoadedTextVisible = true;
                    }
                    else {
                        _this.IsNoPackagesLoadedTextVisible = false;
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
                                var newPackage = new ShipmentPackagePM_1.ShipmentPackagePM(_this.EntityPM);
                                newPackage.ShipmentId = _this.EntityPM.Id;
                                newPackage.ClassNumber = item.ClassNumber;
                                newPackage.ContainerNumber = item.ContainerNumber;
                                newPackage.Description = item.Description;
                                newPackage.FlashPoint = item.FlashPoint;
                                newPackage.Harmonize = item.Harmonize;
                                newPackage.Height = item.Height;
                                newPackage.IMDGCode = item.IMDGCode;
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
                                _this.EntityPM.AddPackage(newPackage);
                            }
                        });
                    }
                    _this.BuildData();
                    _this.ComputeTotals();
                }
            }
        });
    };
    // Single | Multiple Commodities
    AWBPackagesTabComponent.prototype.SetMultipleCommodities = function (newValue) {
        this.IsMultipleCommodities = newValue;
    };
    Object.defineProperty(AWBPackagesTabComponent.prototype, "IsMultipleCommodities", {
        get: function () { return this.EntityPM.IsMultipleCommodities; },
        set: function (newValue) {
            if (this.EntityPM.IsMultipleCommodities != newValue) {
                this.EntityPM.IsMultipleCommodities = newValue;
                this.SetUIProperties();
                this.ChangeDataStructure();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPackagesTabComponent.prototype.ChangeDataStructure = function () {
        this.Validate();
        this.FireWizardEvent();
    };
    AWBPackagesTabComponent.prototype.BuildData = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.IsMultipleCommodities) {
        }
        else {
        }
        var list = new Array();
        this.EntityPM.ShipmentPackages.forEach(function (item) {
            list.push(item);
        });
        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (list.length < 5) {
                for (var i = list.length; i < 5; i++) {
                    var item = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                    item.Tenant = this.EntityPM.Tenant;
                    item.ShipmentId = this.EntityPM.Id;
                    item.IsAWBWizardDefault = true;
                    list.push(item);
                }
            }
        }
        list. /*sort((a, b) => { return (a === b) ? 0 : a ? 1 : 1 }).*/forEach(function (item) {
            var itemViewModel = new AWBWizardPackageItem(item, false, _this);
            _this.ItemsSource.push(itemViewModel);
            itemViewModel.SetUIProperties();
        });
        this.SetRebuildButton();
        this.SetGenerateButton();
    };
    Object.defineProperty(AWBPackagesTabComponent.prototype, "AWBCommodityItemNumber", {
        // Properties
        get: function () { return this.EntityPM.AWBCommodityItemNumber; },
        set: function (newValue) {
            if (this.EntityPM.AWBCommodityItemNumber != newValue) {
                this.EntityPM.AWBCommodityItemNumber = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "DescriptionOfGoods", {
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "IsDangerous", {
        get: function () { return this.EntityPM.IsDangerous; },
        set: function (newValue) {
            if (this.EntityPM.IsDangerous != newValue) {
                this.EntityPM.IsDangerous = newValue;
                this.Validate();
                this.FireWizardEvent();
                if (newValue == false) {
                    this.EntityPM.DangerousClassNumber = null;
                    this.EntityPM.DangerousUnNumber = null;
                    this.EntityPM.DangerousPackagingGroup = null;
                    this.EntityPM.DangerousIMDGCode = null;
                    this.EntityPM.DangerousFlashPoint = null;
                    this.EntityPM.DangerousMaterialDescription = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "Volume", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "VolumetricWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VolumetricWeight) ? 0 : this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "NumberOfPackages", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.NumberOfPackages) ? 0 : this.EntityPM.NumberOfPackages; },
        set: function (newVaule) {
            if (this.EntityPM.NumberOfPackages != newVaule) {
                this.EntityPM.NumberOfPackages = newVaule;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "GrossWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = Tools_1.AppTool.Round(newValue, 3);
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                this.EntityPM.ChargeableWeight = Tools_1.AppTool.Round(newValue, 3);
                this.Validate();
                this.FireWizardEvent();
                this.ComputeAWBChargeAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBPackagesTabComponent.prototype, "AWBChargeAmount", {
        get: function () { return this.EntityPM.AWBChargeAmount; },
        set: function (newValue) {
            if (this.EntityPM.AWBChargeAmount != newValue) {
                this.EntityPM.AWBChargeAmount = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeAWBFrieghtAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBPackagesTabComponent.prototype.ComputeAWBChargeAmount = function () {
        this.AWBChargeAmount = Tools_2.ShipmentTool.ComputeAWBChargeAmount(this.EntityPM.RateClassCode, this.EntityPM.AWBChargeRate, this.EntityPM.ChargeableWeight);
    };
    AWBPackagesTabComponent.prototype.ComputeAWBFrieghtAmount = function () {
        var computedAmount = this.AWBChargeAmount;
        var totaAmount = this.EntityPM.AWBFreightAmountPrepaid + this.EntityPM.AWBFreightAmountCollect;
        var recompute = true;
        var isPrepaidHasAmount = (this.EntityPM.AWBFreightAmountPrepaid != 0 && this.EntityPM.AWBFreightAmountPrepaid != null);
        var isCollectHasAmount = (this.EntityPM.AWBFreightAmountCollect != 0 && this.EntityPM.AWBFreightAmountCollect != null);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FreightPrepaidCollectId)) {
            if (isPrepaidHasAmount && isCollectHasAmount && (computedAmount == totaAmount)) {
                recompute = false;
            }
        }
        if (recompute) {
            if (this.EntityPM.FreightPrepaidCollectId == "P") {
                this.EntityPM.AWBFreightAmountCollect = 0;
                this.EntityPM.AWBFreightAmountPrepaid = computedAmount == null ? 0 : computedAmount;
            }
            else if (this.EntityPM.FreightPrepaidCollectId == "C") {
                this.EntityPM.AWBFreightAmountPrepaid = 0;
                this.EntityPM.AWBFreightAmountCollect = computedAmount == null ? 0 : computedAmount;
            }
        }
        //this.FireAWBErrorsEvent();
    };
    AWBPackagesTabComponent.prototype.setDigits = function () {
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
    AWBPackagesTabComponent.prototype.GrossWeightLostFocus = function (input) {
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
        valueComputed = valueComputed == 0 ? null : valueComputed;
        valueInserted = valueInserted == 0 ? null : valueInserted;
        this.EntityPM.GrossWeightEdited = !(valueComputed == valueInserted);
        this.GrossWeight = valueInserted;
        this.ComputeTotals();
    };
    AWBPackagesTabComponent.prototype.ChargeableWeightLostFocus = function (input) {
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
        }
        valueComputed = valueComputed == 0 ? null : valueComputed;
        valueInserted = valueInserted == 0 ? null : valueInserted;
        this.EntityPM.ChargeableWeightEdited = !(valueComputed == valueInserted);
        this.ChargeableWeight = Tools_1.AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        this.ComputeTotals();
    };
    AWBPackagesTabComponent.prototype.ResetGrossWeightEdited = function () {
        this.EntityPM.GrossWeightEdited = false;
        this.ComputeTotals();
    };
    AWBPackagesTabComponent.prototype.ResetChargeableWeightEdited = function () {
        this.EntityPM.ChargeableWeightEdited = false;
        this.ComputeTotals();
    };
    AWBPackagesTabComponent.prototype.ResetTotalEditedValues = function () {
        this.EntityPM.GrossWeightEdited = false;
        this.EntityPM.ChargeableWeightEdited = false;
    };
    AWBPackagesTabComponent.prototype.ComputeTotals = function () {
        if (this.EntityPM.ShipmentPackages.length == 0) {
            this.EntityPM.NumberOfPackages = null;
            this.EntityPM.GrossWeight = null;
            this.EntityPM.Volume = null;
            this.EntityPM.VolumetricWeight = null;
            this.EntityPM.ChargeableWeight = null;
            this.EntityPM.AWBCommodityItemNumber = null;
            this.EntityPM.GrossWeightEdited = false;
            this.EntityPM.ChargeableWeightEdited = false;
        }
        else {
            var myQuantity = 0;
            var myVolume = 0;
            var myGrossWeight = 0;
            var myVolumetricWeight = 0;
            this.EntityPM.ShipmentPackages.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
                    myQuantity += item.Quantity;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
                    myVolumetricWeight += item.VolumetricWeight;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Weight)) {
                    myGrossWeight += item.Weight;
                }
            });
        }
        this.NumberOfPackages = myQuantity;
        this.Volume = myVolume;
        this.VolumetricWeight = myVolumetricWeight;
        if (!this.EntityPM.GrossWeightEdited) {
            this.GrossWeight = Tools_1.AppTool.Round(myGrossWeight, 3);
        }
        if (!this.EntityPM.ChargeableWeightEdited) {
            this.EntityPM.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
        }
        this.SetUIProperties();
        this.Validate();
        this.FireWizardEvent();
        this.ComputeAWBChargeAmount();
    };
    AWBPackagesTabComponent.prototype.AddPackage = function () {
        var itemPM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        var itemViewModel = new AWBWizardPackageItem(itemPM, true, this);
        this.RunPackageWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.AddPackage"));
    };
    AWBPackagesTabComponent.prototype.EditPackage = function (itemComponent) {
        this.RunPackageWindow(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.EditPackage"));
    };
    AWBPackagesTabComponent.prototype.DeletePackage = function (itemComponent) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (itemComponent.EntityPM.ShipmentPackageItems != null) {
                    itemComponent.EntityPM.ShipmentPackageItems = [];
                }
                if (itemComponent.EntityPM.InsideShipmentPackages != null) {
                    itemComponent.EntityPM.InsideShipmentPackages = [];
                }
                _this.EntityPM.RemovePackage(itemComponent.EntityPM);
                var itemIndex = _this.ItemsSource.indexOf(itemComponent);
                if (itemIndex > -1) {
                    _this.ItemsSource.splice(itemIndex, 1);
                }
                _this.ComputeTotals();
                _this.SetUIProperties();
                _this.Validate();
                _this.FireWizardEvent();
                _this.SetRebuildButton();
                _this.SetGenerateButton();
            }
        });
    };
    AWBPackagesTabComponent.prototype.RunPackageWindow = function (itemComponent, windowTitle) {
        this._entityResourceService.getEntityResourceByTableName(itemComponent.ObjectTableName).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.DataContext = itemComponent;
            logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBAddEditPackageComponent");
        });
    };
    AWBPackagesTabComponent.prototype.ChooseCommodityClicked = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'AWBCommodityItemNumber' };
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
        logitudeWindow.WindowClosed.subscribe(function (s) {
            _this.SetUIProperties();
            _this.Validate();
            _this.FireWizardEvent();
        });
    };
    AWBPackagesTabComponent.prototype.EditDangerouse = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.EditDangerousGoods");
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBDangerousPackageComponent");
    };
    AWBPackagesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AWBPackagesTabComponent',
            templateUrl: './AWBPackagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBPackagesTabComponent);
    return AWBPackagesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AWBPackagesTabComponent = AWBPackagesTabComponent;
var AWBWizardPackageItem = /** @class */ (function (_super) {
    __extends(AWBWizardPackageItem, _super);
    function AWBWizardPackageItem(entityPM, isNew, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "ShipmentPackage";
        _this.IsNewEntity = false;
        _this.IsWindowMode = false;
        _this.IsEditingEnabled = false;
        _this.EntityPM = entityPM;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.SetUIProperties();
        return _this;
    }
    AWBWizardPackageItem.prototype.SetUIProperties = function () {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
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
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);
    };
    AWBWizardPackageItem.prototype.HasValue = function (hasValue) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    };
    Object.defineProperty(AWBWizardPackageItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 0);
                var itemIndex = this.ShipmentPM.ShipmentPackages.indexOf(this.EntityPM);
                if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                    this.Height = null;
                    this.Length = null;
                    this.Width = null;
                    this.Volume = null;
                    this.VolumetricWeight = null;
                    this.Weight = null;
                    if (!this.IsWindowMode) {
                        if (itemIndex > -1) {
                            this.ShipmentPM.RemovePackage(this.EntityPM);
                        }
                    }
                }
                else {
                    if (!this.IsWindowMode) {
                        if (itemIndex == -1) {
                            this.ShipmentPM.AddPackage(this.EntityPM);
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "Length", {
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "Width", {
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "Height", {
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "Dimensions", {
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "Volume", {
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "VolumetricWeight", {
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
    Object.defineProperty(AWBWizardPackageItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.Weight != myValue) {
                this.EntityPM.Weight = myValue;
                this.fatherComponent.ResetTotalEditedValues();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardPackageItem.prototype.OnGrossWeightLostFocus = function (input1) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);
                this.SetUIProperties();
                this.fatherComponent.ResetTotalEditedValues();
                this.fatherComponent.ComputeTotals();
            }
        }
    };
    AWBWizardPackageItem.prototype.ComputeVolume = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    };
    AWBWizardPackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    return AWBWizardPackageItem;
}(BaseComponent_1.BaseComponent));
exports.AWBWizardPackageItem = AWBWizardPackageItem;
//# sourceMappingURL=AWBPackagesTabComponent.js.map