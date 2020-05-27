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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var BookingPackagePM_1 = require("../../../EntityPMs/BookingPackagePM");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Tools_2 = require("../../../Tools");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var PackagesTabComponent = /** @class */ (function (_super) {
    __extends(PackagesTabComponent, _super);
    function PackagesTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.TabSummaryAreaHeight = 100;
        _this.IsVisible = false;
        _this.firstDigit = ",";
        _this.secondDigit = ".";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.IsTotalsFieldEnabled = true;
        _this.IsChooseDescriptionOfGoodsVisible = false;
        _this.IsChooseDescriptionOfGoodsEnabled = false;
        _this.IsTemperatureSensitiveHelpVisible = false;
        // Validate
        _this.ShowWarning_GrossWeight = false;
        _this.ShowWarning_DescriptionOfGoods = false;
        _this.ChargeableWeightPasted = false;
        _this.GrossWeightPasted = false;
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.setDigits();
        return _this;
    }
    PackagesTabComponent.prototype.InitTab = function (wizard) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("BookingPackage", 0).subscribe(function (response1) {
            _this.IsVisible = true;
            _this.Wizard = wizard;
            _this.EntityPM = _this.Wizard.EntityPM;
            _this.ObjectTableName = _this.Wizard.ObjectTableName;
            _this.SetLabels();
            _this.BuildData();
            _this.SetUIProperties();
            _this.Listen();
            _this.Validate();
        });
    };
    PackagesTabComponent.prototype.RefreshTab = function () {
        this.SetUIProperties();
        this.Validate();
        this.BuildData();
    };
    PackagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
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
    PackagesTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.BookingPackages.length > 0) {
                isFieldEnabled = true;
            }
        }
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
        this.IsTotalsFieldEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isFieldEnabled);
        // Ayman: Task 28676
        //this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
        if (this.EntityPM.ZeroIsDescOfGoodsFromList) {
            this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("DescriptionOfGoodsService", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("DescriptionOfGoodsService", this.ObjectTableName, false);
            this.IsChooseDescriptionOfGoodsEnabled = isFieldEnabled;
            this.IsChooseDescriptionOfGoodsVisible = true;
        }
        else {
            this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetVisibility("DescriptionOfGoodsService", this.ObjectTableName, false);
            this.IsChooseDescriptionOfGoodsEnabled = false;
            this.IsChooseDescriptionOfGoodsVisible = false;
        }
        if (this.EntityPM.IsTemperatureSensitive) {
            this.IsTemperatureSensitiveHelpVisible = true;
        }
        else {
            this.IsTemperatureSensitiveHelpVisible = false;
        }
    };
    PackagesTabComponent.prototype.FireWizardEvent = function () {
        this.Validate();
        this.Wizard.ValidateScreen_PAC();
    };
    PackagesTabComponent.prototype.Validate = function () {
        var isShowWarning_GrossWeight = false;
        var isShowWarning_DescriptionOfGoods = false;
        if (Tools_1.AppTool.IsNullOrZero(this.GrossWeight)) {
            isShowWarning_GrossWeight = true;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
            isShowWarning_DescriptionOfGoods = true;
        }
        if (!isShowWarning_DescriptionOfGoods) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "DescriptionOfGoods"; })[0];
            if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.DescriptionOfGoods)) {
                isShowWarning_DescriptionOfGoods = true;
            }
        }
        this.ShowWarning_GrossWeight = isShowWarning_GrossWeight;
        this.ShowWarning_DescriptionOfGoods = isShowWarning_DescriptionOfGoods;
    };
    Object.defineProperty(PackagesTabComponent.prototype, "DescriptionOfGoods", {
        // Properties    
        get: function () { return this.EntityPM.DescriptionOfGoods; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoods != newValue) {
                this.EntityPM.DescriptionOfGoods = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "DescriptionOfGoodsService", {
        get: function () { return this.EntityPM.DescriptionOfGoodsService; },
        set: function (newValue) {
            if (this.EntityPM.DescriptionOfGoodsService != newValue) {
                this.EntityPM.DescriptionOfGoodsService = newValue;
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
                if (!newValue) {
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
    Object.defineProperty(PackagesTabComponent.prototype, "Volume", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "VolumetricWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.VolumetricWeight) ? 0 : this.EntityPM.VolumetricWeight; },
        set: function (newValue) {
            if (this.EntityPM.VolumetricWeight != newValue) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "NumberOfPackages", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.NumberOfPackages) ? 0 : this.EntityPM.NumberOfPackages; },
        set: function (newVaule) {
            this.EntityPM.NumberOfPackages = newVaule;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "GrossWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = Tools_1.AppTool.Round(newValue, 3);
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PackagesTabComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                this.EntityPM.ChargeableWeight = Tools_1.AppTool.Round(newValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    PackagesTabComponent.prototype.SetLabels = function () {
        this.VolumeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.Volume").replace('%UnitCode', this.EntityPM.VolumeUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.VolWeight").replace('%UnitCode', this.EntityPM.ChargeableWeightUnitCode);
    };
    // BuildData
    PackagesTabComponent.prototype.BuildData = function () {
        var _this = this;
        this.ItemsSource = [];
        var list = new Array();
        this.EntityPM.BookingPackages.forEach(function (item) {
            list.push(item);
        });
        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {
                var item = new BookingPackagePM_1.BookingPackagePM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.BookingId = this.EntityPM.Id;
                list.push(item);
            }
        }
        list.sort(function (a, b) { return (a === b) ? 0 : a ? -1 : 1; }).forEach(function (item) {
            var itemViewModel = new BookingWizardPackageItem(item, false, _this);
            _this.ItemsSource.push(itemViewModel);
            itemViewModel.SetUIProperties();
        });
    };
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
        this.EntityPM.BookingPackages.forEach(function (item) {
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
    PackagesTabComponent.prototype.ChargeableWeightPaste = function ($event) {
        this.ChargeableWeightPasted = true;
    };
    PackagesTabComponent.prototype.GrossWeightPaste = function ($event) {
        this.GrossWeightPasted = true;
    };
    PackagesTabComponent.prototype.ChargeableWeightLostFocus = function (input) {
        var valueComputed = 0;
        var valueInserted = 0;
        valueComputed = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode);
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
        this.ChargeableWeight = Tools_1.AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode);
        this.ComputeTotals();
    };
    PackagesTabComponent.prototype.ResetGrossWeightEdited = function () {
        this.EntityPM.GrossWeightEdited = false;
        this.ComputeTotals();
    };
    PackagesTabComponent.prototype.ResetChargeableWeightEdited = function () {
        this.EntityPM.ChargeableWeightEdited = false;
        this.ComputeTotals();
    };
    PackagesTabComponent.prototype.ResetTotalEditedValues = function () {
        this.EntityPM.GrossWeightEdited = false;
        this.EntityPM.ChargeableWeightEdited = false;
    };
    PackagesTabComponent.prototype.ComputeTotals = function () {
        this.SetUIProperties();
        if (this.EntityPM.BookingPackages.length == 0) {
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
            this.EntityPM.BookingPackages.forEach(function (item) {
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
            this.EntityPM.ChargeableWeight = Tools_1.AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode);
        }
        this.SetUIProperties();
        this.FireWizardEvent();
    };
    PackagesTabComponent.prototype.AddPackage = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("BookingPackage", 0).subscribe(function (response) {
            var itemPM = new BookingPackagePM_1.BookingPackagePM(null);
            itemPM.BookingId = _this.EntityPM.Id;
            itemPM.Tenant = _this.EntityPM.Tenant;
            var itemViewModel = new BookingWizardPackageItem(itemPM, true, _this);
            _this.RunPackageWindow(itemViewModel, "Add Package line");
        });
    };
    ;
    PackagesTabComponent.prototype.EditPackage = function (itemViewModel) {
        this.RunPackageWindow(itemViewModel, "Edit Package line");
    };
    PackagesTabComponent.prototype.DeletePackage = function (itemViewModel) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var itemIndex = _this.ItemsSource.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    _this.ItemsSource.splice(itemIndex, 1);
                }
                var index = _this.EntityPM.BookingPackages.indexOf(itemViewModel.EntityPM);
                if (index > -1) {
                    _this.EntityPM.RemoveBookingPackage(itemViewModel.EntityPM);
                }
                _this.ComputeTotals();
                _this.SetUIProperties();
                _this.FireWizardEvent();
            }
        });
    };
    PackagesTabComponent.prototype.RunPackageWindow = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./Booking/Components/BookingWizard/Packages/AddEditPackageComponent');
    };
    PackagesTabComponent.prototype.EditDangerouse = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.O.Packages.EditDangerousGoods");
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnLogitudeWindowClosed($event); });
        logitudeWindow.Show('./Booking/Components/BookingWizard/Packages/DangerousPackageComponent');
    };
    PackagesTabComponent.prototype.OnLogitudeWindowClosed = function (message) {
        if (message == "ok") {
            this.IsDangerous = this.EntityPM.IsDangerous;
        }
    };
    PackagesTabComponent.prototype.ChooseDescriptionOfGoods = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("AWBDescriptionOfGoods", 0).subscribe(function (response1) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = "AWB Description of Goods Search";
            logitudeWindow.WindowArgs = _this.EntityPM;
            logitudeWindow.Show('./Booking/Components/BookingWizard/Packages/ChooseDescriptionOfGoodsComponent');
            logitudeWindow.WindowClosed.subscribe(function ($event) {
                _this.SetUIProperties();
                _this.Validate();
                _this.Wizard.ValidateScreen_GEN();
                if (_this.Wizard.PageChild_GEN != null) {
                    _this.Wizard.PageChild_GEN.Validate_AWBSpecialHandlingCodes();
                }
            });
        });
    };
    PackagesTabComponent = __decorate([
        core_1.Component({
            selector: 'PackagesTabComponent',
            moduleId: module.id,
            templateUrl: './PackagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PackagesTabComponent);
    return PackagesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PackagesTabComponent = PackagesTabComponent;
var BookingWizardPackageItem = /** @class */ (function (_super) {
    __extends(BookingWizardPackageItem, _super);
    function BookingWizardPackageItem(entityPM, isNew, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "BookingPackage";
        _this.ShowWarning_Dimensions = false;
        _this.ShowWarning_GrossWeight = false;
        _this.EntityPM = entityPM;
        _this.BookingPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.SetUIProperties();
        return _this;
    }
    BookingWizardPackageItem.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.BookingTool.IsEditingFieldsEnabled_Others(this.BookingPM);
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        if (isEditingEnabled) {
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
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);
        this.Validate();
    };
    BookingWizardPackageItem.prototype.HasValue = function (hasValue) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    };
    BookingWizardPackageItem.prototype.Validate = function () {
        this.ShowWarning_Dimensions = false;
        this.ShowWarning_GrossWeight = false;
        if (!Tools_1.AppTool.IsNullOrZero(this.Quantity)) {
            if (Tools_1.AppTool.IsNullOrZero(this.Weight)) {
                this.ShowWarning_GrossWeight = true;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.Volume)) {
                if (Tools_1.AppTool.IsNullOrZero(this.Height) || Tools_1.AppTool.IsNullOrZero(this.Width) || Tools_1.AppTool.IsNullOrZero(this.Length)) {
                    this.ShowWarning_Dimensions = true;
                }
            }
        }
    };
    Object.defineProperty(BookingWizardPackageItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newValue) {
            if (this.EntityPM.Quantity != newValue) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(newValue, 0);
                var itemIndex = this.BookingPM.BookingPackages.indexOf(this.EntityPM);
                if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                    this.Height = null;
                    this.Length = null;
                    this.Width = null;
                    this.Volume = null;
                    this.VolumetricWeight = null;
                    this.Weight = null;
                    if (!this.IsWindowMode) {
                        if (itemIndex > -1) {
                            this.BookingPM.RemoveBookingPackage(this.EntityPM);
                        }
                    }
                }
                else {
                    if (!this.IsWindowMode) {
                        if (itemIndex == -1) {
                            this.BookingPM.AddBookingPackage(this.EntityPM);
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "Length", {
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "Width", {
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "Height", {
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "Dimensions", {
        get: function () {
            var myDimensions;
            if (this.Length == null && this.Width == null && this.Height == null) {
                myDimensions = "";
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "Volume", {
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "VolumetricWeight", {
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
    Object.defineProperty(BookingWizardPackageItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (newValue) {
            var myValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.Weight != myValue) {
                this.EntityPM.Weight = myValue;
                this.SetUIProperties();
                this.fatherComponent.ResetTotalEditedValues();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingWizardPackageItem.prototype.OnGrossWeightLostFocus = function (input) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.BookingPM.TransportModeCode == "A") {
                if (this.Width == null || this.Height == null || this.Length == null) {
                    this.EntityPM.VolumetricWeight = Tools_1.AppTool.GetWeightFromWeight(this.BookingPM.GrossWeightUnitCode, this.BookingPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                    this.EntityPM.Volume = Tools_1.AppTool.GetVolumeFromWeight(this.BookingPM.ChargeableWeightUnitCode, this.BookingPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.BookingPM.Ratio);
                    this.SetUIProperties();
                    this.fatherComponent.ResetTotalEditedValues();
                    this.fatherComponent.ComputeTotals();
                }
            }
        }
    };
    BookingWizardPackageItem.prototype.ComputeVolume = function () {
        if (this.BookingPM.Ratio == null) {
            this.BookingPM.Ratio = Tools_1.AppTool.GetRatio(this.BookingPM.DirectionCode, this.BookingPM.TransportModeCode, null, this.fatherComponent.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.BookingPM.Ratio, this.BookingPM.DimensionsUnitCode, this.BookingPM.VolumeUnitCode, this.BookingPM.GrossWeightUnitCode);
    };
    BookingWizardPackageItem.prototype.ComputeVolumetricWeight = function () {
        if (this.BookingPM.Ratio == null) {
            this.BookingPM.Ratio = Tools_1.AppTool.GetRatio(this.BookingPM.DirectionCode, this.BookingPM.TransportModeCode, null, this.fatherComponent.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.BookingPM.Ratio, this.BookingPM.DimensionsUnitCode, this.BookingPM.VolumeUnitCode, this.BookingPM.GrossWeightUnitCode, this.BookingPM.ChargeableWeightUnitCode);
    };
    return BookingWizardPackageItem;
}(BaseComponent_1.BaseComponent));
exports.BookingWizardPackageItem = BookingWizardPackageItem;
//# sourceMappingURL=PackagesTabComponent.js.map