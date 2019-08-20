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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var QuotePackagePM_1 = require("../../../../Quote/EntityPMs/QuotePackagePM");
var QuoteUtilities_1 = require("../../../../Quote/Utilities/QuoteUtilities");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var PackageTypeListService_1 = require("../../../../Common/Services/StandardLists/PackageTypeListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var Tools_2 = require("../../../../Quote/Tools");
var PackagesTabComponent = /** @class */ (function (_super) {
    __extends(PackagesTabComponent, _super);
    function PackagesTabComponent(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Quote";
        _this.IsResourcesReady = false;
        _this.QuoteIsFCL = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = true;
        _this.IsAddButtonEnabled = false;
        _this.IsDimFactorVisible = false;
        _this.IsQuantitiesVisible = false;
        _this.Delete1IsVisible = false;
        _this.Delete2IsVisible = false;
        _this.Delete3IsVisible = false;
        _this.Delete4IsVisible = false;
        _this.Delete5IsVisible = false;
        _this.DimensionsDependencyProperty1 = null;
        _this.DimensionsDependencyProperty1IsList = false;
        ///////// Measurments ///////////
        _this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Details.MeasurmentsSettings");
        _this.IsMeasurmentsHidden = true;
        _this.SelectedRow = null;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.Listen();
        return _this;
    }
    PackagesTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.QuoteIsFCL = !QuoteUtilities_1.QuoteUtilities.IsLCLQuote(this.EntityPM);
            this.entityResourceService.getEntityResourceByTableName("QuotePackage").subscribe(function (res) {
                _this.IsResourcesReady = true;
            });
            this.SetLabels();
            this.SetUIProperties();
            this.BuildItemsSource();
        }
    };
    PackagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.QuoteIsFCL = !QuoteUtilities_1.QuoteUtilities.IsLCLQuote(_this.EntityPM);
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.QuoteIsFCL = !QuoteUtilities_1.QuoteUtilities.IsLCLQuote(_this.EntityPM);
                    _this.SetLabels();
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "QTPK") {
                    _this.SetUIProperties_DimFactor();
                    _this.SetUIProperties_Expected_Details();
                    _this.SetUIProperties_DimensionsUnitCode();
                }
            });
        }
    };
    PackagesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    PackagesTabComponent.prototype.SetUIProperties = function () {
        this.IsAddButtonEnabled = false;
        this.IsEditingEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        if (this.IsEditingEnabled) {
            if (this.EntityPM.QuoteTypeCode == "A") {
                this.IsAddButtonEnabled = true;
            }
        }
        if (this.QuoteIsFCL) {
            var isQuantitiesVisible = (this.EntityPM.QuoteTypeCode == "A");
            this.IsQuantitiesVisible = isQuantitiesVisible;
            this.UIProperties.SetVisibility("PackageType1Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType2Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType3Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType4Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.UIProperties.SetVisibility("PackageType5Quantity", this.ObjectTableName, isQuantitiesVisible);
            this.SetUIProperties_Expected_Details();
        }
        this.SetUIProperties_EntityClosed();
        this.SetUIProperties_Totals();
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_DimensionsUnitCode();
    };
    PackagesTabComponent.prototype.SetUIProperties_EntityClosed = function () {
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, this.IsEditingEnabled);
    };
    PackagesTabComponent.prototype.SetUIProperties_Expected_Details = function () {
        this.UIProperties.SetEnabled("PackageType1Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType2Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType3Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType4Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PackageType5Quantity", this.ObjectTableName, this.IsEditingEnabled);
        if (this.EntityPM.QuoteTypeCode == "P") {
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, this.IsEditingEnabled);
        }
        else {
            var package1ControlIsEnabled = true;
            var package2ControlIsEnabled = true;
            var package3ControlIsEnabled = true;
            var package4ControlIsEnabled = true;
            var package5ControlIsEnabled = true;
            //1
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType1Quantity)) {
                package1ControlIsEnabled = false;
            }
            else if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType1UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
                package1ControlIsEnabled = false;
            }
            //2
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType2Quantity)) {
                package2ControlIsEnabled = false;
            }
            else if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType2UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
                package2ControlIsEnabled = false;
            }
            //3
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType3Quantity)) {
                package3ControlIsEnabled = false;
            }
            else if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType3UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
                package3ControlIsEnabled = false;
            }
            //4
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType4Quantity)) {
                package4ControlIsEnabled = false;
            }
            else if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType4UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
                package4ControlIsEnabled = false;
            }
            //5
            if (Tools_1.AppTool.IsNullOrZero(this.PackageType5Quantity)) {
                package5ControlIsEnabled = false;
            }
            else if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType5UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id != null)) {
                package5ControlIsEnabled = false;
            }
            this.UIProperties.SetEnabled("PackageType1Id", this.ObjectTableName, package1ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType2Id", this.ObjectTableName, package2ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType3Id", this.ObjectTableName, package3ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType4Id", this.ObjectTableName, package4ControlIsEnabled && this.IsEditingEnabled);
            this.UIProperties.SetEnabled("PackageType5Id", this.ObjectTableName, package5ControlIsEnabled && this.IsEditingEnabled);
        }
        if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType1UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            this.Delete1IsVisible = true;
        }
        else {
            this.Delete1IsVisible = false;
        }
        //2
        if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType2UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            this.Delete2IsVisible = true;
        }
        else {
            this.Delete2IsVisible = false;
        }
        //3
        if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType3UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            this.Delete3IsVisible = true;
        }
        else {
            this.Delete3IsVisible = false;
        }
        //4
        if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType4UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            this.Delete4IsVisible = true;
        }
        else {
            this.Delete4IsVisible = false;
        }
        //5
        if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostContainerType5UnitPrice != null; }).length > 0 && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id != null)) {
            this.Delete5IsVisible = true;
        }
        else {
            this.Delete5IsVisible = false;
        }
    };
    PackagesTabComponent.prototype.SetUIProperties_Totals = function () {
        var isTotalsFieldEnabled = false;
        var isTotalsEditedFieldEnabled = false;
        if (this.IsEditingEnabled) {
            isTotalsFieldEnabled = true;
            isTotalsEditedFieldEnabled = true;
            if (this.EntityPM.QuotePackages.length > 0) {
                isTotalsFieldEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isTotalsFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isTotalsEditedFieldEnabled);
    };
    PackagesTabComponent.prototype.SetUIProperties_DimFactor = function () {
        var isDimFactorVisibile = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DimensionsUnitCode)) {
            if (this.EntityPM.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }
        this.IsDimFactorVisible = isDimFactorVisibile;
        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
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
    PackagesTabComponent.prototype.MeasurmentsSettingsClicked = function () {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;
        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Details.MeasurmentsSettings");
        }
        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Details.HideMeasurmentsSettings");
        }
    };
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
                this.ChargeableWeight_Kg();
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
    Object.defineProperty(PackagesTabComponent.prototype, "Ratio", {
        get: function () { return this.EntityPM.Ratio; },
        set: function (newValue) {
            if (this.EntityPM.Ratio != newValue) {
                this.EntityPM.Ratio = newValue;
                this.ComputeDimFactor();
                QuoteUtilities_1.QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
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
                QuoteUtilities_1.QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    PackagesTabComponent.prototype.ComputeDimFactor = function () {
        this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    };
    PackagesTabComponent.prototype.OnMeasurmentsSettingsChanged = function () {
        this.SetLabels();
        QuoteUtilities_1.QuoteUtilities.RecalculateQuoteFields(this.EntityPM);
    };
    PackagesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource.Clear();
        this.EntityPM.QuotePackages.forEach(function (item) {
            _this.ItemsSource.Insert(new QuotePackageItem(item, _this, false));
        });
        this.SetUIProperties_Totals();
    };
    PackagesTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType1Quantity", {
        // FCL
        get: function () { return this.EntityPM.PackageType1Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType1Quantity != newValue) {
                this.EntityPM.PackageType1Quantity = newValue;
                if (this.EntityPM.QuoteTypeCode != "P") {
                    if (Tools_1.AppTool.IsNullOrZero(newValue)) {
                        this.PackageType1Id = null;
                    }
                }
                this.SetUIProperties_Expected_Details();
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType2Quantity", {
        get: function () { return this.EntityPM.PackageType2Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType2Quantity != newValue) {
                this.EntityPM.PackageType2Quantity = newValue;
                if (this.EntityPM.QuoteTypeCode != "P") {
                    if (Tools_1.AppTool.IsNullOrZero(newValue)) {
                        this.PackageType2Id = null;
                    }
                }
                this.SetUIProperties_Expected_Details();
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType3Quantity", {
        get: function () { return this.EntityPM.PackageType3Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType3Quantity != newValue) {
                this.EntityPM.PackageType3Quantity = newValue;
                if (this.EntityPM.QuoteTypeCode != "P") {
                    if (Tools_1.AppTool.IsNullOrZero(newValue)) {
                        this.PackageType3Id = null;
                    }
                }
                this.SetUIProperties_Expected_Details();
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType4Quantity", {
        get: function () { return this.EntityPM.PackageType4Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType4Quantity != newValue) {
                this.EntityPM.PackageType4Quantity = newValue;
                if (this.EntityPM.QuoteTypeCode != "P") {
                    if (Tools_1.AppTool.IsNullOrZero(newValue)) {
                        this.PackageType4Id = null;
                    }
                }
                this.SetUIProperties_Expected_Details();
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType5Quantity", {
        get: function () { return this.EntityPM.PackageType5Quantity; },
        set: function (newValue) {
            if (this.EntityPM.PackageType5Quantity != newValue) {
                this.EntityPM.PackageType5Quantity = newValue;
                if (this.EntityPM.QuoteTypeCode != "P") {
                    if (Tools_1.AppTool.IsNullOrZero(newValue)) {
                        this.PackageType5Id = null;
                    }
                }
                this.SetUIProperties_Expected_Details();
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType1Id", {
        get: function () { return this.EntityPM.PackageType1Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType1Id != newValue) {
                this.EntityPM.PackageType1Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
                this.SetUIProperties_Expected_Details();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType2Id", {
        get: function () { return this.EntityPM.PackageType2Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType2Id != newValue) {
                this.EntityPM.PackageType2Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
                this.SetUIProperties_Expected_Details();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType3Id", {
        get: function () { return this.EntityPM.PackageType3Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType3Id != newValue) {
                this.EntityPM.PackageType3Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
                this.SetUIProperties_Expected_Details();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType4Id", {
        get: function () { return this.EntityPM.PackageType4Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType4Id != newValue) {
                this.EntityPM.PackageType4Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
                this.SetUIProperties_Expected_Details();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "PackageType5Id", {
        get: function () { return this.EntityPM.PackageType5Id; },
        set: function (newValue) {
            if (this.EntityPM.PackageType5Id != newValue) {
                this.EntityPM.PackageType5Id = newValue;
                this.UpdateCharges();
                this.EntityPM.TEU = QuoteUtilities_1.QuoteUtilities.ComputeQuoteTEU(this.EntityPM);
                this.SetUIProperties_Expected_Details();
            }
        },
        enumerable: true,
        configurable: true
    });
    PackagesTabComponent.prototype.UpdateCharges = function () {
        var _this = this;
        var sum = 0;
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
        sum = sum + this.PackageType1Quantity;
        //}
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
        sum = sum + this.PackageType2Quantity;
        //}
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
        sum = sum + this.PackageType3Quantity;
        //}
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
        sum = sum + this.PackageType4Quantity;
        //}
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
        sum = sum + this.PackageType5Quantity;
        //}
        if (this.QuoteIsFCL) {
            this.EntityPM.NumberOfContainers = sum;
        }
        else {
            this.NumberOfPackages = sum;
        }
        this.EntityPM.QuoteCharges.forEach(function (item) {
            if (Tools_1.AppTool.IsNullOrEmpty(_this.PackageType1Id)) {
                item.CostContainerType1UnitPrice = null;
                item.SaleContainerType1UnitPrice = null;
                item.CostUnitPrice1InSaleCurrency = null;
                item.ContainerType1MarkUpValue = 0;
                item.ContainerType1MarkUpTypeCode = "F";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.PackageType2Id)) {
                item.CostContainerType2UnitPrice = null;
                item.SaleContainerType2UnitPrice = null;
                item.CostUnitPrice2InSaleCurrency = null;
                item.ContainerType2MarkUpValue = 0;
                item.ContainerType2MarkUpTypeCode = "F";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.PackageType3Id)) {
                item.CostContainerType3UnitPrice = null;
                item.SaleContainerType3UnitPrice = null;
                item.CostUnitPrice3InSaleCurrency = null;
                item.ContainerType3MarkUpValue = 0;
                item.ContainerType3MarkUpTypeCode = "F";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.PackageType4Id)) {
                item.CostContainerType4UnitPrice = null;
                item.SaleContainerType4UnitPrice = null;
                item.CostUnitPrice4InSaleCurrency = null;
                item.ContainerType4MarkUpValue = 0;
                item.ContainerType4MarkUpTypeCode = "F";
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.PackageType5Id)) {
                item.CostContainerType5UnitPrice = null;
                item.SaleContainerType5UnitPrice = null;
                item.CostUnitPrice5InSaleCurrency = null;
                item.ContainerType5MarkUpValue = 0;
                item.ContainerType5MarkUpTypeCode = "F";
            }
            _this.ComputeCostAmounts(item);
            _this.ComputeSaleAmounts(item);
        });
        this.ComputeChargesTotals();
    };
    PackagesTabComponent.prototype.ComputeCostAmounts = function (item) {
        if (item.CostMeasurementCode == "BCNT") {
            var myTotalAmount = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType1UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Quantity)) {
                var R1 = item.CostContainerType1UnitPrice * this.EntityPM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType2UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Quantity)) {
                var R2 = item.CostContainerType2UnitPrice * this.EntityPM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType3UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Quantity)) {
                var R3 = item.CostContainerType3UnitPrice * this.EntityPM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType4UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Quantity)) {
                var R4 = item.CostContainerType4UnitPrice * this.EntityPM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType5UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Quantity)) {
                var R5 = item.CostContainerType5UnitPrice * this.EntityPM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(myTotalAmount)) {
                item.CostTotalAmount = null;
                item.CostTotalAmountLocal = null;
                item.CostAmountInSaleCurrency = null;
            }
            else {
                item.CostTotalAmount = Tools_1.AppTool.Round(myTotalAmount, 2);
                if (Tools_1.AppTool.IsNullOrEmpty(item.CostExchangeRate)) {
                    item.CostTotalAmountLocal = null;
                }
                else {
                    item.CostTotalAmountLocal = Tools_1.AppTool.Round(myTotalAmount * item.CostExchangeRate, 2);
                }
                this.ComputeCostInSaleAmount(item);
            }
        }
    };
    PackagesTabComponent.prototype.ComputeSaleAmounts = function (item) {
        if (item.SaleMeasurementCode == "BCNT") {
            var myTotalAmount = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleContainerType1UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Quantity)) {
                var R1 = item.SaleContainerType1UnitPrice * this.EntityPM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleContainerType2UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Quantity)) {
                var R2 = item.SaleContainerType2UnitPrice * this.EntityPM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleContainerType3UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Quantity)) {
                var R3 = item.SaleContainerType3UnitPrice * this.EntityPM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleContainerType4UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Quantity)) {
                var R4 = item.SaleContainerType4UnitPrice * this.EntityPM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SaleContainerType5UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Quantity)) {
                var R5 = item.SaleContainerType5UnitPrice * this.EntityPM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }
            item.SaleTotalAmount = Tools_1.AppTool.Round(myTotalAmount, 2);
            item.SaleTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(myTotalAmount) ? null : Tools_1.AppTool.Round(myTotalAmount * item.SaleExchangeRate, 2);
        }
    };
    PackagesTabComponent.prototype.ComputeCostInSaleAmount = function (item) {
        var myResult = null;
        if (this.EntityPM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostTotalAmount)) {
                myResult = item.CostTotalAmount;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.CostTotalAmountLocal) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ExchangeRate)) {
                myResult = item.CostTotalAmountLocal / this.EntityPM.ExchangeRate;
            }
        }
        item.CostAmountInSaleCurrency = Tools_1.AppTool.Round(myResult, 2);
    };
    PackagesTabComponent.prototype.ComputeChargesTotals = function () {
        var myProfitAmount = 0;
        var myCostAmountLocal = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.QuoteCharges, "CostTotalAmountLocal"), 2);
        var mySaleAmountLocal = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(function (f) { return f.IsAllIN == false; }), "SaleTotalAmountLocal"), 2);
        var mySaleProfitLocal = Tools_1.AppTool.Round(mySaleAmountLocal - myCostAmountLocal, 2);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ExchangeRate)) {
            myProfitAmount = Tools_1.AppTool.Round(mySaleProfitLocal / this.EntityPM.ExchangeRate, 2);
        }
        if (this.EntityPM.EstimateProfit != myProfitAmount) {
            this.EntityPM.EstimateProfit = Tools_1.AppTool.Round(myProfitAmount, 2);
        }
        this.CurrentSession.FireEvent("FCLPackagesChanged");
    };
    PackagesTabComponent.prototype.DeleteFCLPackage = function (packageIndex) {
        switch (packageIndex) {
            case "1": {
                this.PackageType1Id = null;
                break;
            }
            case "2": {
                this.PackageType2Id = null;
                break;
            }
            case "3": {
                this.PackageType3Id = null;
                break;
            }
            case "4": {
                this.PackageType4Id = null;
                break;
            }
            case "5": {
                this.PackageType5Id = null;
                break;
            }
        }
        this.SetUIProperties_Expected_Details();
    };
    PackagesTabComponent.prototype.SetLabels = function () {
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Volume").replace("%VolumeCode", this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.GrossWeight").replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.ChargeableWeight").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.ChargeableWeightUnitCode");
        }
        else {
            this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.WtMsr.Short").replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.WtMsrUnitCode.Short");
        }
    };
    PackagesTabComponent.prototype.ComputeTotals = function () {
        if (this.EntityPM.QuotePackages.length == 0) {
            this.Volume = null;
            this.GrossWeight = null;
            this.ChargeableWeight = null;
            this.VolumetricWeight = null;
            this.NumberOfPackages = null;
            this.GrossWeightEdited = false;
            this.ChargeableWeightEdited = false;
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
            this.NumberOfPackages = myNumberOfPackages;
            this.Volume = myVolume;
            this.VolumetricWeight = Tools_1.AppTool.Round(myVolumetricWeight, 3);
            if (!this.GrossWeightEdited) {
                this.GrossWeight = Tools_1.AppTool.Round(myGrossWeight, 3);
            }
            if (!this.ChargeableWeightEdited) {
                this.ChargeableWeight = QuoteUtilities_1.QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
            }
        }
        this.ComputeGrossWeigh_Kg_Ton();
        this.ChargeableWeight_Kg();
        Tools_2.QuoteTool.OnQuoteQuantitiesChanged(this.EntityPM);
        this.SetUIProperties_Totals();
    };
    Object.defineProperty(PackagesTabComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = Tools_1.AppTool.Round(newValue, 3);
                if (this.EntityPM.QuotePackages.length == 0) {
                    this.EntityPM.ChargeableWeight = QuoteUtilities_1.QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
                }
                this.ComputeGrossWeigh_Kg_Ton();
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
                if (this.EntityPM.QuotePackages.length == 0) {
                    this.EntityPM.VolumetricWeight = QuoteUtilities_1.QuoteUtilities.ComputeVolumetricWeight(this.EntityPM);
                }
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
                if (this.EntityPM.QuotePackages.length == 0) {
                    this.EntityPM.ChargeableWeight = QuoteUtilities_1.QuoteUtilities.ComputeChargeableWeight(this.EntityPM);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "ChargeableWeight", {
        get: function () { return this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                var result = Tools_1.AppTool.Round(newValue, 2);
                this.EntityPM.ChargeableWeight = result;
                this.ChargeableWeight_Kg();
                if (this.EntityPM.QuotePackages.length == 0) {
                    if (this.GrossWeight == null && this.EntityPM.VolumetricWeight == null) {
                        this.EntityPM.VolumetricWeight = result;
                        this.EntityPM.GrossWeight = Tools_1.AppTool.GetWeightFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeightUnitCode, result);
                        this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.EntityPM.Ratio);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "NumberOfPackages", {
        get: function () { return this.EntityPM.NumberOfPackages; },
        set: function (newValue) {
            if (this.EntityPM.NumberOfPackages != newValue) {
                this.EntityPM.NumberOfPackages = newValue;
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
    PackagesTabComponent.prototype.GrossWeightLostFocus = function (input) {
        if (this.EntityPM.QuotePackages.length > 0) {
            var valueComputed = 0;
            var valueInserted = 0;
            this.EntityPM.QuotePackages.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    valueComputed += item.GrossWeight;
                }
            });
            if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
                input = Tools_1.AppTool.Replace(input, ",", "");
                valueInserted = Number(input);
            }
            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = valueInserted == 0 ? null : valueInserted;
            this.GrossWeightEdited = !(valueComputed == valueInserted);
            this.GrossWeight = valueInserted;
            this.ComputeTotals();
        }
    };
    PackagesTabComponent.prototype.ChargeableWeightLostFocus = function (input) {
        if (this.EntityPM.QuotePackages.length > 0) {
            var valueComputed = 0;
            var valueInserted = 0;
            valueComputed = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            if (!Tools_1.AppTool.IsNullOrEmpty(input)) {
                input = Tools_1.AppTool.Replace(input, ",", "");
                valueInserted = Number(input);
            }
            valueComputed = valueComputed == 0 ? null : valueComputed;
            valueInserted = valueInserted == 0 ? null : valueInserted;
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
    PackagesTabComponent.prototype.ChargeableWeight_Kg = function () {
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
    PackagesTabComponent.prototype.AddPackageClicked = function () {
        var itemPM = new QuotePackagePM_1.QuotePackagePM(null);
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.QuoteId = this.EntityPM.Id;
        var itemComponent = new QuotePackageItem(itemPM, this, true);
        this.RunAddEditPackage(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.Packages.AddPackage"));
    };
    PackagesTabComponent.prototype.EditPackageClicked = function (itemComponent) {
        this.RunAddEditPackage(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.Packages.EditPackage"));
    };
    PackagesTabComponent.prototype.RunAddEditPackage = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteTabs/Components/Packages/AddEditPackageComponent');
    };
    PackagesTabComponent.prototype.DeletePackageClicked = function (itemComponent) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.RemoveQuotePackagePM(itemComponent.EntityPM);
                _this.BuildItemsSource();
                _this.ComputeTotals();
            }
        });
    };
    PackagesTabComponent = __decorate([
        core_1.Component({
            selector: 'PackagesTabComponent',
            moduleId: module.id,
            templateUrl: './PackagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PackagesTabComponent);
    return PackagesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PackagesTabComponent = PackagesTabComponent;
var QuotePackageItem = /** @class */ (function (_super) {
    __extends(QuotePackageItem, _super);
    function QuotePackageItem(entity, fatherComponent, isNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "QuotePackage";
        _this.IsNewEntity = false;
        _this.CellReadOnlyBackground = "#E6E7E8";
        _this.IsQuoteEditEnabled = false;
        _this.IsVolumeEnabled = false;
        _this.EntityPM = entity;
        _this.QuotePM = fatherComponent.EntityPM;
        _this.TransportModeId = fatherComponent.EntityPM.TransportModeId;
        _this.IsNewEntity = isNew;
        _this.SetUIProperties();
        return _this;
    }
    QuotePackageItem.prototype.SetUIProperties = function () {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        this.IsQuoteEditEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.QuotePM);
        if (this.IsQuoteEditEnabled) {
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
        this.IsVolumeEnabled = isVolumeEnabled;
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);
        if (this.TransportModeId == "A") {
            this.UIProperties.SetVisibility("PackageTypeId", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetVisibility("PackageTypeId", this.ObjectTableName, true);
            if (Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
                this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, false);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.GrossWeight)) {
                this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, false);
            }
        }
    };
    Object.defineProperty(QuotePackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != newValue) {
                this.EntityPM.PackageTypeId = newValue;
                if (this.TransportModeId != "A") {
                    this.UIProperties.SetRequired('PackageTypeId', this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
                }
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.PackageTypeName = null;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingleFromCache(newValue).subscribe(function (myResponse) {
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
    Object.defineProperty(QuotePackageItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) {
            if (this.EntityPM.PackageTypeName != newValue) {
                this.EntityPM.PackageTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuotePackageItem.prototype, "Quantity", {
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
    Object.defineProperty(QuotePackageItem.prototype, "Length", {
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
    Object.defineProperty(QuotePackageItem.prototype, "Width", {
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
    Object.defineProperty(QuotePackageItem.prototype, "Height", {
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
    Object.defineProperty(QuotePackageItem.prototype, "Dimensions", {
        get: function () {
            var myDimensions = " - - ";
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
    Object.defineProperty(QuotePackageItem.prototype, "Volume", {
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
    Object.defineProperty(QuotePackageItem.prototype, "VolumetricWeight", {
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
    Object.defineProperty(QuotePackageItem.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 2);
            if (this.EntityPM.GrossWeight != myValue) {
                this.EntityPM.GrossWeight = myValue;
                this.SetUIProperties();
                if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                    this.fatherComponent.ResetTotalEditedValues();
                    this.fatherComponent.ComputeTotals();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    QuotePackageItem.prototype.OnGrossWeightLostFocus = function (input) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.QuotePM.ChargeableWeightUnitCode, this.QuotePM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.QuotePM.Ratio);
                this.SetUIProperties();
                if (this.fatherComponent.ItemsSource.Collection.indexOf(this) > -1) {
                    this.fatherComponent.ResetTotalEditedValues();
                    this.fatherComponent.ComputeTotals();
                }
            }
        }
    };
    QuotePackageItem.prototype.ComputeVolume = function () {
        if (this.fatherComponent.Ratio == null) {
            this.fatherComponent.Ratio = Tools_1.AppTool.GetRatio(this.QuotePM.DirectionId, this.QuotePM.TransportModeId, this.QuotePM.ShipmentTypeId, InfraSettings_1.InfraSettings.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.QuotePM.Ratio, this.QuotePM.DimensionsUnitCode, this.QuotePM.VolumeUnitCode, this.QuotePM.GrossWeightUnitCode);
    };
    QuotePackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.fatherComponent.Ratio == null) {
            this.fatherComponent.Ratio = Tools_1.AppTool.GetRatio(this.QuotePM.DirectionId, this.QuotePM.TransportModeId, this.QuotePM.ShipmentTypeId, InfraSettings_1.InfraSettings.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.QuotePM.Ratio, this.QuotePM.DimensionsUnitCode, this.QuotePM.VolumeUnitCode, this.QuotePM.GrossWeightUnitCode, this.QuotePM.ChargeableWeightUnitCode);
    };
    return QuotePackageItem;
}(BaseComponent_1.BaseComponent));
exports.QuotePackageItem = QuotePackageItem;
//# sourceMappingURL=PackagesTabComponent.js.map