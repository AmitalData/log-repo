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
var Tools_1 = require("../../Infrastructure/Tools");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var WarehouseEntryPackagePM_1 = require("../../Warehouse/EntityPMs/WarehouseEntryPackagePM");
var PackageTypeListService_1 = require("../../Common/Services/StandardLists/PackageTypeListService");
var WarehouseEntryPackagesDetailsComponent = /** @class */ (function (_super) {
    __extends(WarehouseEntryPackagesDetailsComponent, _super);
    function WarehouseEntryPackagesDetailsComponent() {
        var _this = _super.call(this) || this;
        _this.AllPackageTypes = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.IsLoadPage = false;
        _this.IsLCLEntity = false;
        _this.WarehouseEntryPackagesLists = [];
        _this.ObjectTableName = "WarehouseEntryPM";
        _this.IsEditMode = false;
        _this.IsFromFullWarehouseEntryComponent = false;
        _this.savedItems = [];
        _this.ShowAddPackageButton = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Measurments
        _this.MeasurmentsButtonToolTip = "Measurement Settings";
        _this.IsMeasurmentsHidden = true;
        _this.DimensionsDependencyProperty1 = null;
        _this.DimensionsDependencyProperty1IsList = false;
        _this.ChargeableWeightLabel = null;
        _this.myPackageTypeService = new PackageTypeListService_1.PackageTypeListService();
        return _this;
    }
    WarehouseEntryPackagesDetailsComponent.prototype.ngOnInit = function () {
    };
    WarehouseEntryPackagesDetailsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.IsFromFullWarehouseEntryComponent = args.IsFromFullWarehouseEntryComponent;
        this.IsEditMode = args.IsEditMode;
        this.ShowPackageSummary = args.ShowPackageSummary;
        this.ShowAddPackageButton = args.ShowAddPackageButton;
        if (!this.IsEditMode) {
            this.ShowAddPackageButton = true;
        }
        this.myPackageTypeService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                _this.AllPackageTypes = resp.Result;
            }
            _this.Start(args);
        });
    };
    WarehouseEntryPackagesDetailsComponent.prototype.MeasurmentsSettingsClicked = function () {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;
        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = "Hide Measurement Settings";
        }
        else {
            this.MeasurmentsButtonToolTip = "Measurement Settings";
        }
    };
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "VolumeUnitCode", {
        get: function () {
            var volumeUnitCode = null;
            if (this.warehouseEntryPM)
                volumeUnitCode = this.warehouseEntryPM.VolumeUnitCode;
            return volumeUnitCode;
        },
        set: function (newValue) {
            if (this.warehouseEntryPM.VolumeUnitCode != newValue) {
                this.warehouseEntryPM.VolumeUnitCode = newValue;
                this.warehouseEntryPM.DimensionsUnitCode = Tools_1.AppTool.GetDimentionsCodeFromVolumeCode(newValue);
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.SetUIProperties_DimensionsUnitCode();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "DimensionsUnitCode", {
        get: function () {
            var dimensionsUnitCode = null;
            if (this.warehouseEntryPM)
                dimensionsUnitCode = this.warehouseEntryPM.DimensionsUnitCode;
            return dimensionsUnitCode;
        },
        set: function (newValue) {
            if (this.warehouseEntryPM.DimensionsUnitCode != newValue) {
                this.warehouseEntryPM.DimensionsUnitCode = newValue;
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "GrossWeightUnitCode", {
        get: function () {
            var grossWeightUnitCode = null;
            if (this.warehouseEntryPM)
                grossWeightUnitCode = this.warehouseEntryPM.GrossWeightUnitCode;
            return grossWeightUnitCode;
        },
        set: function (newValue) {
            if (this.warehouseEntryPM.GrossWeightUnitCode != newValue) {
                this.warehouseEntryPM.GrossWeightUnitCode = newValue;
                this.OnMeasurmentsSettingsChanged();
                this.ComputeGrossWeigh_Kg_Ton();
            }
        },
        enumerable: true,
        configurable: true
    });
    //get ChargeableWeightUnitCode() { return this.warehouseEntryPM.ChargeableWeightUnitCode; }
    //set ChargeableWeightUnitCode(newValue: string) {
    //    if (this.warehouseEntryPM.ChargeableWeightUnitCode != newValue) {
    //        this.warehouseEntryPM.ChargeableWeightUnitCode = newValue;
    //        this.ComputeDimFactor();
    //        this.OnMeasurmentsSettingsChanged();
    //    }
    //}
    //get Ratio() { return this.warehouseEntryPM.Ratio; }
    //set Ratio(newValue: number) {
    //    if (this.warehouseEntryPM.Ratio != newValue) {
    //        this.warehouseEntryPM.Ratio = newValue;
    //        this.ComputeDimFactor();
    //        ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
    //    }
    //}
    WarehouseEntryPackagesDetailsComponent.prototype.ComputeDimFactor = function () {
        //  this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    };
    WarehouseEntryPackagesDetailsComponent.prototype.OnMeasurmentsSettingsChanged = function () {
        this.SetAttachedLabels();
        this.RecalculateShipmentFields(this.warehouseEntryPM);
    };
    WarehouseEntryPackagesDetailsComponent.prototype.SetUIProperties_DimFactor = function () {
        var isDimFactorVisibile = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }
        //this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    };
    WarehouseEntryPackagesDetailsComponent.prototype.SetUIProperties_DimensionsUnitCode = function () {
        var isFieldEnabled = false;
        if (this.VolumeUnitCode == "CBF") {
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
    WarehouseEntryPackagesDetailsComponent.prototype.SetAttachedLabels = function () {
        this.VolumeLabel = "Volume (" + this.warehouseEntryPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + this.warehouseEntryPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + this.warehouseEntryPM.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseEntryPM.ChargeableWeightUnitCode + ")";
    };
    WarehouseEntryPackagesDetailsComponent.prototype.ComputeGrossWeigh_Kg_Ton = function () {
        //var weigh_Kg: number = null;
        //var weigh_Ton: number = null;
        //if (this.GrossWeight != null) {
        //    var factorOfConvert: number = 1;
        //    if (!AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
        //        switch (this.GrossWeightUnitCode.toUpperCase()) {
        //            case "KG": { factorOfConvert = 1; break; }
        //            case "LB": { factorOfConvert = 0.45359237; break; }
        //            case "MT": { factorOfConvert = 1000; break; }
        //        }
        //    }
        //    weigh_Kg = this.GrossWeight * factorOfConvert;
        //}
        //if (weigh_Kg != null) {
        //    weigh_Kg = AppTool.Round(weigh_Kg, 3);
        //    weigh_Ton = weigh_Kg / 1000;
        //}
        //if (weigh_Ton != null) {
        //    weigh_Ton = AppTool.Round(weigh_Ton, 3);
        //}
        //this.EntityPM.GrossWeightInKG = weigh_Kg;
        //this.EntityPM.GrossWeightPerTon = weigh_Ton;
    };
    WarehouseEntryPackagesDetailsComponent.prototype.RecalculateShipmentFields = function (warehouseEntryPM) {
        if (warehouseEntryPM != null) {
            //if (warehouseEntryPM.Ratio == null) {
            //    warehouseEntryPM.Ratio = AppTool.GetRatio(warehouseEntryPM.DirectionId, warehouseEntryPM.TransportModeId, warehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            //}
            if (warehouseEntryPM.WarehouseEntryPackages.length == 0) {
                warehouseEntryPM.TotalPieces = null;
                warehouseEntryPM.TotalGrossWeight = null;
                warehouseEntryPM.TotalVolume = null;
                warehouseEntryPM.TotalVolumetricWeight = null;
                // warehouseEntryPM.ChargeableWeight = null;
                // warehouseEntryPM.AWBCommodityItemNumber = null;
                // warehouseEntryPM.GrossWeightEdited = false;
                // warehouseEntryPM.ChargeableWeightEdited = false;
            }
            else {
                var myQuantity = 0;
                var myVolume = 0;
                var myGrossWeight = 0;
                var myVolumetricWeight = 0;
                warehouseEntryPM.WarehouseEntryPackages.forEach(function (item) {
                    //item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, warehouseEntryPM.Ratio, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode);
                    item.Volume = Tools_1.AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, null, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode);
                    //item.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.Weight, warehouseEntryPM.Ratio, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode, warehouseEntryPM.ChargeableWeightUnitCode);
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
                });
                warehouseEntryPM.TotalPieces = myQuantity;
                warehouseEntryPM.TotalVolume = Tools_1.AppTool.Round(myVolume, 3);
                warehouseEntryPM.TotalVolumetricWeight = Tools_1.AppTool.Round(myVolumetricWeight, 3);
            }
        }
    };
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "TotalVolume", {
        get: function () {
            var totalVolume = 0;
            if (this.warehouseEntryPM && this.warehouseEntryPM.TotalVolume)
                totalVolume = this.warehouseEntryPM.TotalVolume;
            return totalVolume;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "TotalGrossWeight", {
        get: function () {
            var totalGrossWeight = 0;
            if (this.warehouseEntryPM && this.warehouseEntryPM.TotalGrossWeight)
                totalGrossWeight = this.warehouseEntryPM.TotalGrossWeight;
            return totalGrossWeight;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "TotalPieces", {
        get: function () {
            var totalPieces = 0;
            if (this.warehouseEntryPM && this.warehouseEntryPM.TotalPieces)
                totalPieces = this.warehouseEntryPM.TotalPieces;
            return totalPieces;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "TotalVolumetricWeight", {
        get: function () {
            var iResult = 0;
            if (this.warehouseEntryPM && this.warehouseEntryPM.TotalVolumetricWeight) {
                iResult = this.warehouseEntryPM.TotalVolumetricWeight;
            }
            return iResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackagesDetailsComponent.prototype, "QuantityLabel", {
        get: function () {
            var quantityLabel = "";
            if (this.IsLCLEntity) {
                quantityLabel = "Number of Packages";
            }
            else
                quantityLabel = "Number of Containers";
            return quantityLabel;
        },
        enumerable: true,
        configurable: true
    });
    WarehouseEntryPackagesDetailsComponent.prototype.Start = function (args) {
        var _this = this;
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.ViewModelTrigger = args.ViewModelTrigger;
        if (this.warehouseEntryPM) {
            this.VolumeLabel = "Volume (" + this.warehouseEntryPM.VolumeUnitCode + ")";
            this.GrossWeightLabel = "Gross Weight (" + this.warehouseEntryPM.GrossWeightUnitCode + ")";
            this.DimensionsLabel = "Dim(L-W-H) (" + this.warehouseEntryPM.DimensionsUnitCode + ")";
            this.SetAttachedLabels();
            this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
            //this.TotalPieces = this.warehouseEntryPM.TotalPieces ? this.warehouseEntryPM.TotalPieces : 0;
            //this.TotalGrossWeight = this.warehouseEntryPM.TotalGrossWeight ? this.warehouseEntryPM.TotalGrossWeight : 0;
            //this.TotalVolume = this.warehouseEntryPM.TotalVolume ? this.warehouseEntryPM.TotalVolume : 0;
            this.warehouseEntryPM.WarehouseEntryPackages.forEach(function (item) {
                var savedItem = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
                savedItem.PackageTypeId = item.PackageTypeId;
                savedItem.ContainerNumber = item.ContainerNumber;
                savedItem.Length = item.Length;
                savedItem.Height = item.Height;
                savedItem.Width = item.Width;
                savedItem.Volume = item.Volume;
                savedItem.Weight = item.Weight;
                savedItem.Description = item.Description;
                savedItem.Seal = item.Seal;
                savedItem.Harmonize = item.Harmonize;
                savedItem.Location = item.Location;
                savedItem.Dimensions = item.Dimensions;
                savedItem.Instock = item.Instock;
                savedItem.Quantity = item.Quantity;
                savedItem.ContainerNumberWarning = item.ContainerNumberWarning;
                savedItem.Id = item.Id;
                savedItem.CreateDate = item.CreateDate;
                savedItem.UpdateDate = item.UpdateDate;
                savedItem.CreatedByUserId = item.CreatedByUserId;
                savedItem.UpdatedByUserId = item.UpdatedByUserId;
                savedItem.WarehouseEntryId = item.WarehouseEntryId;
                savedItem.PackageTypeName = item.PackageTypeName;
                savedItem.IsContainer = item.IsContainer;
                savedItem.Make = item.Make;
                savedItem.Year = item.Year;
                savedItem.ChassisNumber = item.ChassisNumber;
                savedItem.RegistrationNumber = item.RegistrationNumber;
                savedItem.CountryId = item.CountryId;
                savedItem.Model = item.Model;
                savedItem.Color = item.Color;
                _this.savedItems.push(savedItem);
                _this.WarehouseEntryPackagesLists.push(item);
            });
        }
        this.ComputeAndFullTotalPackage(true);
    };
    WarehouseEntryPackagesDetailsComponent.prototype.AddPackage = function (isContainer) {
        var newWarehouseEntryPackagePM = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
        newWarehouseEntryPackagePM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        newWarehouseEntryPackagePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newWarehouseEntryPackagePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newWarehouseEntryPackagePM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newWarehouseEntryPackagePM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newWarehouseEntryPackagePM.WarehouseEntryId = this.warehouseEntryPM.Id;
        newWarehouseEntryPackagePM.Id = "1-1";
        newWarehouseEntryPackagePM.IsContainer = isContainer;
        var title = isContainer ? "Add Container" : "Add Package";
        this.ShowAddPackageWindow(newWarehouseEntryPackagePM, true, title);
    };
    WarehouseEntryPackagesDetailsComponent.prototype.ShowAddPackageWindow = function (warehouseEntryPackagePM, isNewEntity, title) {
        var _this = this;
        var windowArgs = {};
        windowArgs.IsNewEntity = isNewEntity;
        windowArgs.WarehouseEntryPackagePM = warehouseEntryPackagePM;
        windowArgs.WarehouseEntryPM = this.warehouseEntryPM;
        windowArgs.ViewModelTrigger = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 830;
        logWindow.Height = 550;
        logWindow.Title = title;
        windowArgs.AllPackageTypes = this.AllPackageTypes;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/AddEditWarehouseEntryPackagesAndContainers");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.ComputeAndFullTotalPackage();
            }
        });
    };
    WarehouseEntryPackagesDetailsComponent.prototype.EditPackage = function (warehouseEntryPackagePM) {
        if (warehouseEntryPackagePM.Quantity == warehouseEntryPackagePM.Instock) {
            var title = !warehouseEntryPackagePM.IsContainer ? "Edit Package" : "Edit Container";
            this.ShowAddPackageWindow(warehouseEntryPackagePM, false, title);
        }
    };
    WarehouseEntryPackagesDetailsComponent.prototype.DeletePackage = function (item) {
        var _this = this;
        if (item.Quantity == item.Instock) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Delete this package");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    var index = _this.WarehouseEntryPackagesLists.indexOf(item);
                    if (index != -1)
                        _this.WarehouseEntryPackagesLists.splice(index, 1);
                    _this.ComputeAndFullTotalPackage();
                }
            });
        }
    };
    WarehouseEntryPackagesDetailsComponent.prototype.ComputeAndFullTotalPackage = function (firstTime) {
        if (firstTime === void 0) { firstTime = false; }
        var totalPieces = 0;
        var totalVolume = 0;
        var totalGrossWeight = 0;
        var totalVolumetricWeight = 0;
        if (this.WarehouseEntryPackagesLists && this.WarehouseEntryPackagesLists.length > 0) {
            this.WarehouseEntryPackagesLists.forEach(function (item) {
                if (item.Quantity)
                    totalPieces += item.Quantity;
                if (item.Volume)
                    totalVolume += item.Volume;
                if (item.Weight)
                    totalGrossWeight += item.Weight;
                if (item.VolumetricWeight)
                    totalVolumetricWeight += item.VolumetricWeight;
            });
        }
        this.warehouseEntryPM.WarehouseEntryPackages = this.WarehouseEntryPackagesLists;
        this.warehouseEntryPM.TotalPieces = totalPieces;
        this.warehouseEntryPM.TotalVolume = totalVolume;
        this.warehouseEntryPM.TotalGrossWeight = totalGrossWeight;
        this.warehouseEntryPM.TotalVolumetricWeight = totalVolumetricWeight;
        if (firstTime && this.warehouseEntryPM.IsDirty)
            this.warehouseEntryPM.IsDirty = false;
    };
    WarehouseEntryPackagesDetailsComponent.prototype.CancelButtonClicked = function () {
        this.ResetPackage();
        this.CloseButtonClicked();
    };
    WarehouseEntryPackagesDetailsComponent.prototype.ResetPackage = function () {
        this.warehouseEntryPM.WarehouseEntryPackages = this.savedItems;
    };
    WarehouseEntryPackagesDetailsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    WarehouseEntryPackagesDetailsComponent.prototype.SaveButtonClicked = function () {
        this.SaveOnWarewarehouseEntryPM();
        this.CurrentSession.CurrentWindow.Close("Refresh");
    };
    WarehouseEntryPackagesDetailsComponent.prototype.SaveOnWarewarehouseEntryPM = function () {
        if (this.warehouseEntryPM != null) {
            this.warehouseEntryPM.WarehouseEntryPackages = this.WarehouseEntryPackagesLists;
            if (this.WarehouseEntryPackagesLists.length == 0) {
                this.warehouseEntryPM.TotalVolume = 0;
                this.warehouseEntryPM.TotalGrossWeight = 0;
                this.warehouseEntryPM.TotalPieces = 0;
                this.warehouseEntryPM.TotalVolumetricWeight = 0;
            }
            if (this.warehouseEntryPM.TotalPieces == 0 || !this.warehouseEntryPM.TotalPieces) {
                this.warehouseEntryPM.TotalVolume = 0;
                this.warehouseEntryPM.TotalPieces = 0;
                this.warehouseEntryPM.TotalVolumetricWeight = 0;
            }
            if (this.ViewModelTrigger != null) {
                //  this.ViewModelTrigger.SetPackagesDetailsEnable();
            }
        }
    };
    WarehouseEntryPackagesDetailsComponent.prototype.ReloadComponent = function (args) {
        this.WarehouseEntryPackagesLists = [];
        this.SelectedWarehouseEntryPackage = null;
        this.Start(args);
    };
    WarehouseEntryPackagesDetailsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'WarehouseEntryPackagesDetailsComponent',
            templateUrl: './WarehouseEntryPackagesDetailsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WarehouseEntryPackagesDetailsComponent);
    return WarehouseEntryPackagesDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.WarehouseEntryPackagesDetailsComponent = WarehouseEntryPackagesDetailsComponent;
//# sourceMappingURL=WarehouseEntryPackagesDetailsComponent.js.map