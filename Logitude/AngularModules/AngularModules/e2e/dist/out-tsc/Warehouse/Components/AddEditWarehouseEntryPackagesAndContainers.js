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
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var WarehouseEntryPackagePM_1 = require("../../Warehouse/EntityPMs/WarehouseEntryPackagePM");
var WarehouseEntryPackagePMExtendedService_1 = require("../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ClassLevelValidator_1 = require("../../Infrastructure/Validators/ClassLevelValidator");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var PackageTypeListService_1 = require("../../Common/Services/StandardLists/PackageTypeListService");
var AddEditWarehouseEntryPackagesAndContainers = /** @class */ (function () {
    function AddEditWarehouseEntryPackagesAndContainers(_warehouseEntryPackagePMExtendedService) {
        this._warehouseEntryPackagePMExtendedService = _warehouseEntryPackagePMExtendedService;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.WarningErrorsList = [];
        this.warehouseEntryPackagePM = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
        this.ObjectTableName = "WarehouseEntryPackage";
        this.ContainerNumberWarning = null;
        this.IsNewEntity = false;
        this.IsLoadPage = false;
        this.AllPackageTypes = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsParentDirty = false;
        this.IsChildDirty = false;
        this.IsEnable = false;
        this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        var table = window.ObjectTables.filter(function (d) { return d.Name == "WarehouseEntryPackage"; })[0];
        if (table) {
            this.ObjectTableId = table.Id;
        }
    }
    AddEditWarehouseEntryPackagesAndContainers.prototype.ngOnInit = function () {
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntryPackage").subscribe(function (response) {
            _this.Start(args);
        });
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.Start = function (args) {
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.warehouseEntryPackagePM = args.WarehouseEntryPackagePM;
        this.AllPackageTypes = args.AllPackageTypes;
        if (this.warehouseEntryPM && this.warehouseEntryPackagePM) {
            this.ViewModelTrigger = args.ViewModelTrigger;
            this.IsNewEntity = args.IsNewEntity;
            if (this.IsNewEntity) {
                this.warehouseEntryPackagePM.Instock = 0;
                this.warehouseEntryPackagePM.IsContainer = this.warehouseEntryPackagePM.IsContainer;
            }
            else {
                if (this.warehouseEntryPackagePM.IsContainer) {
                    if (this.warehouseEntryPackagePM.ContainerNumber) {
                        this.ValidateContainerNumber(this.warehouseEntryPackagePM.ContainerNumber);
                    }
                }
                if (this.warehouseEntryPackagePM) {
                    this.ContainerNumberLostFocus(this.warehouseEntryPackagePM.ContainerNumber);
                    this.IsParentDirty = this.warehouseEntryPM.IsDirty;
                    this.IsChildDirty = this.warehouseEntryPackagePM.IsDirty;
                    this.savedItem = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
                    this.savedItem.PackageTypeId = this.warehouseEntryPackagePM.PackageTypeId;
                    this.savedItem.ContainerNumber = this.warehouseEntryPackagePM.ContainerNumber;
                    this.savedItem.Length = this.warehouseEntryPackagePM.Length;
                    this.savedItem.Height = this.warehouseEntryPackagePM.Height;
                    this.savedItem.Width = this.warehouseEntryPackagePM.Width;
                    this.savedItem.Volume = this.warehouseEntryPackagePM.Volume;
                    this.savedItem.Weight = this.warehouseEntryPackagePM.Weight;
                    this.savedItem.Description = this.warehouseEntryPackagePM.Description;
                    this.savedItem.Seal = this.warehouseEntryPackagePM.Seal;
                    this.savedItem.Harmonize = this.warehouseEntryPackagePM.Harmonize;
                    this.savedItem.Location = this.warehouseEntryPackagePM.Location;
                    this.savedItem.Dimensions = this.warehouseEntryPackagePM.Dimensions;
                    this.savedItem.Instock = this.warehouseEntryPackagePM.Instock;
                    this.savedItem.Quantity = this.warehouseEntryPackagePM.Quantity;
                    this.savedItem.Make = this.warehouseEntryPackagePM.Make;
                    this.savedItem.Year = this.warehouseEntryPackagePM.Year;
                    this.savedItem.ChassisNumber = this.warehouseEntryPackagePM.ChassisNumber;
                    this.savedItem.RegistrationNumber = this.warehouseEntryPackagePM.RegistrationNumber;
                    this.savedItem.CountryId = this.warehouseEntryPackagePM.CountryId;
                    this.savedItem.Model = this.warehouseEntryPackagePM.Model;
                    this.savedItem.Color = this.warehouseEntryPackagePM.Color;
                    this.savedItem.ContainerNumberWarning = this.warehouseEntryPackagePM.ContainerNumberWarning;
                }
            }
            this.warehouseEntryPackageItem = new WarehouseEntryPackageItem(this.warehouseEntryPackagePM, this);
            this.IsLoadPage = true;
        }
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.ResetPackageItem = function () {
        if (!this.IsNewEntity) {
            if (this.warehouseEntryPackagePM && this.savedItem) {
                this.warehouseEntryPackagePM.PackageTypeId = this.savedItem.PackageTypeId;
                this.warehouseEntryPackagePM.ContainerNumber = this.savedItem.ContainerNumber;
                this.warehouseEntryPackagePM.Length = this.savedItem.Length;
                this.warehouseEntryPackagePM.Height = this.savedItem.Height;
                this.warehouseEntryPackagePM.Width = this.savedItem.Width;
                this.warehouseEntryPackagePM.Volume = this.savedItem.Volume;
                this.warehouseEntryPackagePM.Weight = this.savedItem.Weight;
                this.warehouseEntryPackagePM.Description = this.savedItem.Description;
                this.warehouseEntryPackagePM.Seal = this.savedItem.Seal;
                this.warehouseEntryPackagePM.Harmonize = this.savedItem.Harmonize;
                this.warehouseEntryPackagePM.Location = this.savedItem.Location;
                this.warehouseEntryPackagePM.Dimensions = this.savedItem.Dimensions;
                this.warehouseEntryPackagePM.Instock = this.savedItem.Instock;
                this.warehouseEntryPackagePM.Quantity = this.savedItem.Quantity;
                this.warehouseEntryPackagePM.ContainerNumberWarning = this.savedItem.ContainerNumberWarning;
                this.warehouseEntryPackagePM.Make = this.savedItem.Make;
                this.warehouseEntryPackagePM.Year = this.savedItem.Year;
                this.warehouseEntryPackagePM.ChassisNumber = this.savedItem.ChassisNumber;
                this.warehouseEntryPackagePM.RegistrationNumber = this.savedItem.RegistrationNumber;
                this.warehouseEntryPackagePM.CountryId = this.savedItem.CountryId;
                this.warehouseEntryPackagePM.Model = this.savedItem.Model;
                this.warehouseEntryPackagePM.Color = this.savedItem.Color;
            }
            if (this.warehouseEntryPM && !this.IsParentDirty && this.warehouseEntryPM.IsDirty) {
                this.warehouseEntryPM.IsDirty = this.IsParentDirty;
            }
            if (this.warehouseEntryPackagePM && !this.IsChildDirty && this.warehouseEntryPackagePM.IsDirty) {
                this.warehouseEntryPackagePM.IsDirty = this.IsChildDirty;
            }
        }
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.CanceluttonClicked = function () {
        this.ResetPackageItem();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("WarehouseEntryPackage", this.warehouseEntryPackagePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.warehouseEntryPackagePM.IsDirty) {
                this.ComplateSave();
            }
            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.ComplateSave = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        this.warehouseEntryPackagePM.Dimensions = this.warehouseEntryPackageItem ? this.warehouseEntryPackageItem.Dimensions : "";
        this.warehouseEntryPackagePM.PackageTypeName = "";
        this.warehouseEntryPackagePM.Instock = this.warehouseEntryPackagePM.Quantity;
        if (this.AllPackageTypes) {
            var packageTypeList = this.AllPackageTypes.filter(function (d) { return d.Id == _this.warehouseEntryPackagePM.PackageTypeId; })[0];
            if (packageTypeList)
                this.warehouseEntryPackagePM.PackageTypeName = packageTypeList.EnglishName;
        }
        if (this.IsNewEntity && this.ViewModelTrigger.WarehouseEntryPackagesLists) {
            this.ViewModelTrigger.WarehouseEntryPackagesLists.push(this.warehouseEntryPackagePM);
        }
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("Refresh");
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.ContainerNumberLostFocus = function (input) {
        this.ValidateContainerNumber(input);
    };
    AddEditWarehouseEntryPackagesAndContainers.prototype.ValidateContainerNumber = function (input) {
        var warnings = [];
        var error = Tools_1.FormatTool.ValidateContainerNumber(input);
        if (!Tools_1.AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);
            if (Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPackagePM.ContainerNumberWarning)) {
                this.warehouseEntryPackagePM.ContainerNumberWarning = error;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.warehouseEntryPackagePM.ContainerNumberWarning)) {
                this.warehouseEntryPackagePM.ContainerNumberWarning = null;
            }
        }
        this.WarningErrorsList = warnings;
    };
    AddEditWarehouseEntryPackagesAndContainers = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddEditWarehouseEntryPackagesAndContainers',
            templateUrl: './AddEditWarehouseEntryPackagesAndContainers.html',
            providers: [WarehouseEntryPackagePMExtendedService_1.WarehouseEntryPackagePMExtendedService],
        }),
        __metadata("design:paramtypes", [WarehouseEntryPackagePMExtendedService_1.WarehouseEntryPackagePMExtendedService])
    ], AddEditWarehouseEntryPackagesAndContainers);
    return AddEditWarehouseEntryPackagesAndContainers;
}());
exports.AddEditWarehouseEntryPackagesAndContainers = AddEditWarehouseEntryPackagesAndContainers;
var WarehouseEntryPackageItem = /** @class */ (function (_super) {
    __extends(WarehouseEntryPackageItem, _super);
    function WarehouseEntryPackageItem(entity, fatherComponent) {
        if (fatherComponent === void 0) { fatherComponent = null; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "WarehouseEntryPackage";
        _this.IsContainer = false;
        _this.IsVehicleDetails = false;
        _this.FatherComponent = fatherComponent;
        _this.EntityPM = entity;
        _this.WarehouseEntryPM = _this.FatherComponent.warehouseEntryPM;
        _this.VolumeUnitCode = _this.WarehouseEntryPM.VolumeUnitCode;
        _this.GrossWeightUnitCode = _this.WarehouseEntryPM.GrossWeightUnitCode;
        _this.ChargeableWeightUnitCode = _this.WarehouseEntryPM.ChargeableWeightUnitCode;
        _this.DimensionsUnitCode = _this.WarehouseEntryPM.DimensionsUnitCode;
        _this.IsDependencyFilter2Value = _this.IsContainer = _this.EntityPM.IsContainer;
        _this.SetLabel();
        _this.SetUIProperties();
        _this.SetUIPropertiesOfCars(false);
        _this.IsVehicleDetails = false;
        return _this;
    }
    WarehouseEntryPackageItem.prototype.SetLabel = function () {
        this.VolumeLabel = "Volume (" + this.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Weight (" + this.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dimensions(L-W-H) (" + this.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.ChargeableWeightUnitCode + ")";
    };
    WarehouseEntryPackageItem.prototype.SetUIProperties = function () {
        var _this = this;
        if (this.EntityPM.IsContainer) {
            if (this.FatherComponent.IsNewEntity) {
                this.EntityPM.Quantity = 1;
            }
            this.UIProperties.SetEnabled("Quantity", "WarehouseEntryPackage", false);
        }
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        var isGrossWeightEnabled = false;
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
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            myService.getSingle(this.PackageTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.SetUIPropertiesOfCars(list.IsVehicle);
                        _this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isGrossWeightEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
    };
    WarehouseEntryPackageItem.prototype.ComputeVolume = function () {
        this.Volume = Tools_1.AppTool.GetVolumeFromDimentions(this.DimensionsUnitCode, this.VolumeUnitCode, this.Width, this.Height, this.Length, this.Quantity);
    };
    WarehouseEntryPackageItem.prototype.ComputeVolumetricWeight = function () {
        var ratio = Tools_1.AppTool.GetRatio(this.WarehouseEntryPM.DirectionId, this.WarehouseEntryPM.TransportModeId, this.WarehouseEntryPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, ratio, this.DimensionsUnitCode, this.VolumeUnitCode, this.GrossWeightUnitCode, this.ChargeableWeightUnitCode);
    };
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Quantity", {
        // Dimensions
        get: function () { return this.EntityPM.Quantity; },
        set: function (value) {
            if (this.EntityPM.Quantity != value) {
                this.EntityPM.Quantity = Tools_1.AppTool.Round(value, 0);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Length", {
        get: function () { return this.EntityPM.Length; },
        set: function (value) {
            if (this.EntityPM.Length != value) {
                this.EntityPM.Length = Tools_1.AppTool.Round(value, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Width", {
        get: function () { return this.EntityPM.Width; },
        set: function (value) {
            if (this.EntityPM.Width != value) {
                this.EntityPM.Width = Tools_1.AppTool.Round(value, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Height", {
        get: function () { return this.EntityPM.Height; },
        set: function (value) {
            if (this.EntityPM.Height != value) {
                this.EntityPM.Height = Tools_1.AppTool.Round(value, 2);
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Dimensions", {
        get: function () {
            var myDimensions;
            if (!this.IsContainer) {
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
            }
            return myDimensions;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (value) {
            if (this.EntityPM.Volume != value) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(value, 3);
                this.ComputeVolumetricWeight();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (value) {
            if (this.EntityPM.VolumetricWeight != value) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(value, 3);
                // this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (value) {
            var myValue = Tools_1.AppTool.Round(value, 3);
            if (this.EntityPM.Weight != myValue) {
                this.EntityPM.Weight = myValue;
                //if (this.ShipmentPM.TransportModeId != "A") {
                //    this.UIProperties.SetRequired('Weight', this.ObjectTableName, AppTool.IsNullOrEmpty(this.Weight) ? true : false);
                //}
                //this.fatherComponent.ResetTotalEditedValues();
                //this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != value) {
                this.EntityPM.PackageTypeId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.SetUIPropertiesOfCars(false);
                    this.IsVehicleDetails = false;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.SetUIPropertiesOfCars(list.IsVehicle);
                                _this.IsVehicleDetails = list.IsVehicle;
                                if (!list.IsVehicle) {
                                    _this.Make = null;
                                    _this.Model = null;
                                    _this.Color = null;
                                    _this.Year = null;
                                    _this.CountryId = null;
                                    _this.ChassisNumber = null;
                                    _this.RegistrationNumber = null;
                                }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseEntryPackageItem.prototype.SetUIPropertiesOfCars = function (isEnabled) {
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
    };
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Location", {
        get: function () { return this.EntityPM.Location; },
        set: function (value) {
            if (this.EntityPM.Location != value) {
                this.EntityPM.Location = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Harmonize", {
        get: function () { return this.EntityPM.Harmonize; },
        set: function (value) {
            if (this.EntityPM.Harmonize != value) {
                this.EntityPM.Harmonize = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Seal", {
        get: function () { return this.EntityPM.Seal; },
        set: function (value) {
            if (this.EntityPM.Seal != value) {
                this.EntityPM.Seal = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "CommodityNumber", {
        get: function () { return this.EntityPM.CommodityNumber; },
        set: function (value) {
            if (this.EntityPM.CommodityNumber != value) {
                this.EntityPM.CommodityNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseEntryPackageItem.prototype.ChooseCommodityClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'CommodityNumber' };
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
    };
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Make", {
        get: function () { return this.EntityPM.Make; },
        set: function (value) {
            if (this.EntityPM.Make != value) {
                this.EntityPM.Make = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Model", {
        get: function () { return this.EntityPM.Model; },
        set: function (value) {
            if (this.EntityPM.Model != value) {
                this.EntityPM.Model = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Year", {
        get: function () { return this.EntityPM.Year; },
        set: function (value) {
            if (this.EntityPM.Year != value) {
                this.EntityPM.Year = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "Color", {
        get: function () { return this.EntityPM.Color; },
        set: function (value) {
            if (this.EntityPM.Color != value) {
                this.EntityPM.Color = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "ChassisNumber", {
        get: function () { return this.EntityPM.ChassisNumber; },
        set: function (value) {
            if (this.EntityPM.ChassisNumber != value) {
                this.EntityPM.ChassisNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "RegistrationNumber", {
        get: function () { return this.EntityPM.RegistrationNumber; },
        set: function (value) {
            if (this.EntityPM.RegistrationNumber != value) {
                this.EntityPM.RegistrationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageItem.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (value) {
            if (this.EntityPM.CountryId != value) {
                this.EntityPM.CountryId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return WarehouseEntryPackageItem;
}(BaseComponent_1.BaseComponent));
exports.WarehouseEntryPackageItem = WarehouseEntryPackageItem;
//# sourceMappingURL=AddEditWarehouseEntryPackagesAndContainers.js.map