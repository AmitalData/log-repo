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
var Tools_2 = require("../../../../../Shipment/Tools");
var ShipmentPickUpDeliveryPackagePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var PackageTypeListService_1 = require("../../../../../Common/Services/StandardLists/PackageTypeListService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var DeliveryPackagesTabComponent = /** @class */ (function () {
    function DeliveryPackagesTabComponent() {
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.IsContainersFUVisible = false;
        this.IsShowCopyFromReleasesPackages = false;
        this.ObjectTableName = "ShipmentPickUpDelivery";
        this.ItemsSource = [];
        this.DataContext = this;
        this.TypeCode = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsConnectedToContainer = false;
        this.IsEditingEnabled = true;
        this.SelectedItem = null;
        this.PackageTypeColumnWidth = 80;
        this.SaveCompletedEvent = null;
    }
    DeliveryPackagesTabComponent.prototype.InitTab = function (myEntityPM, myShipmentPM, father) {
        var _this = this;
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        this.FatherComponent = father;
        this.DirectionId = this.ShipmentPM.DirectionId;
        this.TransportModeId = this.ShipmentPM.TransportModeId;
        this.TypeCode = this.EntityPM.PickUpDeliveryTypeCode;
        this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId);
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Area.ContainersFU")) {
            this.IsContainersFUVisible = true;
        }
        if (this.ShipmentPM) {
            if (this.ShipmentPM.ShipmentLevelCode == "D" || this.ShipmentPM.ShipmentLevelCode == "C") {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseRelease", "Module")) {
                    this.IsShowCopyFromReleasesPackages = this.ShipmentPM.DirectionId == "I" ? true : false;
                }
            }
        }
        if (this.FatherComponent.IsCreatingContainerDelivery) {
            this.IsConnectedToContainer = true;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        }
        else {
            if (this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.Id; }).length > 0) {
                this.IsConnectedToContainer = true;
            }
            //if (this.ShipmentPM.ShipmentPackages.filter(f => f.EmptyContainerReturnId == this.EntityPM.Id).length > 0) {
            //    this.IsConnectedToContainer = true;
            //}
        }
        this.SetLabels();
        this.SetUIProperties();
        this.BuildItemsSource();
    };
    DeliveryPackagesTabComponent.prototype.SetLabels = function () {
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.Volume').replace('%UnitCode', this.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = "Dimensions (L-W-H) (" + this.ShipmentPM.DimensionsUnitCode + ")";
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.GrossWeight').replace('%UnitCode', this.ShipmentPM.GrossWeightUnitCode);
    };
    DeliveryPackagesTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        if (this.IsEditingEnabled) {
            if (this.IsConnectedToContainer) {
                this.IsEditingEnabled = false;
            }
        }
    };
    DeliveryPackagesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myPackageTypeColumnWidth = 80;
        this.EntityPM.ShipmentPickUpDeliveryPackages.forEach(function (item) {
            var widthOfLabel = Tools_1.AppTool.GetTextWidth(item.PackageTypeName) + 10;
            if (widthOfLabel > myPackageTypeColumnWidth) {
                myPackageTypeColumnWidth = widthOfLabel;
            }
            _this.ItemsSource.push(new DeliveryPackageItem(item, _this));
        });
        if (myPackageTypeColumnWidth > 190) {
            myPackageTypeColumnWidth = 190;
        }
        this.PackageTypeColumnWidth = myPackageTypeColumnWidth;
    };
    DeliveryPackagesTabComponent.prototype.CopyfromShipmentPackagesButtonClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Copy from Shipment Packages"; //TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.ChooseDeliveryPackages");
        logWindow.WindowArgs = this;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesChooseComponent");
    };
    DeliveryPackagesTabComponent.prototype.AddButtonClicked = function () {
        var itemPM = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(null);
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.ShipmentPickUpDeliveryId = this.EntityPM.Id;
        var itemComponent = new DeliveryPackageItem(itemPM, this, true);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.AddDeliveryPackage");
        logWindow.DataContext = itemComponent;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesAddEditComponent");
    };
    DeliveryPackagesTabComponent.prototype.EditPackageClicked = function (itemComponent) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.O.EditDeliveryPackage");
        logWindow.DataContext = itemComponent;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesAddEditComponent");
    };
    DeliveryPackagesTabComponent.prototype.DeleteButtonClicked = function (itemComponent) {
        var _this = this;
        if (itemComponent) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPickUpDeliveryPackage.M.DeleteThisDeliveryPackage"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.EntityPM.RemovePackage(itemComponent.EntityPM);
                    _this.BuildItemsSource();
                }
            });
        }
    };
    DeliveryPackagesTabComponent.prototype.CopyFromReleasesPackages = function () {
        var windowArgs = {};
        windowArgs.ShipmentDeliveryPM = this.EntityPM;
        windowArgs.ShipmentPM = this.ShipmentPM;
        windowArgs.FatherComponent = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 600;
        logWindow.Title = "Warehouse Releases";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/CopyFromReleasesPackagesComponent");
    };
    DeliveryPackagesTabComponent.prototype.ConnectPackagesButtonClicked = function () {
        var _this = this;
        if (this.FatherComponent.IsNewEntity) {
            if (this.CurrentSession.CurrentEditComponent) {
                var isValid = this.FatherComponent.Validate();
                if (isValid) {
                    var SavedEntityId = this.EntityPM.Id;
                    var SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;
                    if (this.FatherComponent.IsNewEntity) {
                        this.ShipmentPM.AddDelivery(this.EntityPM);
                        this.FatherComponent.isEntityAdded = true;
                    }
                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                            _this.FatherComponent.OnSaveCompleted(isSaveSuccess, false, SavedEntityId, SavedEntityNumber);
                            Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                            _this.SaveCompletedEvent = null;
                            _this.ShowConnectWindow();
                        });
                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                    }
                }
            }
        }
        else {
            this.ShowConnectWindow();
        }
    };
    DeliveryPackagesTabComponent.prototype.ShowConnectWindow = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Connect Packages";
        logWindow.WindowArgs = this;
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesConnectComponent");
    };
    DeliveryPackagesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeliveryPackagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeliveryPackagesTabComponent);
    return DeliveryPackagesTabComponent;
}());
exports.DeliveryPackagesTabComponent = DeliveryPackagesTabComponent;
var DeliveryPackageItem = /** @class */ (function (_super) {
    __extends(DeliveryPackageItem, _super);
    function DeliveryPackageItem(item, fatherComponent, isNewEntity) {
        if (isNewEntity === void 0) { isNewEntity = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "ShipmentPickUpDeliveryPackage";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.IsAirShipment = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsVehicleDetails = false;
        _this.IsContainer = false;
        _this.IsEditingEnabled = true;
        _this.WarningErrorsList = [];
        _this.ContainerNumberWarning = null;
        _this.EntityPM = item;
        _this.IsNewEntity = isNewEntity;
        _this.IsLCLEntity = fatherComponent.IsLCLEntity;
        _this.IsFCLEntity = fatherComponent.IsFCLEntity;
        _this.IsAirShipment = fatherComponent.TransportModeId == "A" ? true : false;
        _this.SetUIProperties();
        if (_this.IsNewEntity) {
            _this.SetUIPropertiesOfCars(false);
            _this.IsVehicleDetails = false;
        }
        return _this;
    }
    DeliveryPackageItem.prototype.SetUIProperties = function () {
        var _this = this;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        if (this.IsEditingEnabled) {
            if (this.EntityPM.OriginalShipmentPackageId) {
                this.IsEditingEnabled = false;
            }
        }
        this.UIProperties.SetVisibility("Length", this.ObjectTableName, this.IsLCLEntity);
        this.UIProperties.SetVisibility("Width", this.ObjectTableName, this.IsLCLEntity);
        this.UIProperties.SetVisibility("Height", this.ObjectTableName, this.IsLCLEntity);
        this.SetUIProperties_IsContainer();
        this.SetUIProperties_Harmonize();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PackageTypeId)) {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            myService.getSingleFromCache(this.PackageTypeId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.IsContainer = list.IsContainer;
                        _this.SetUIProperties_IsContainer();
                        _this.SetUIPropertiesOfCars(_this.IsEditingEnabled && list.IsVehicle);
                        _this.IsVehicleDetails = list.IsVehicle;
                    }
                }
            });
        }
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ContainerNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ShipperSeal", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, this.IsEditingEnabled);
    };
    DeliveryPackageItem.prototype.SetUIPropertiesOfCars = function (isEnabled) {
        this.UIProperties.SetEnabled("Make", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Model", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Color", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ChassisNumber", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("RegistrationNumber", this.ObjectTableName, isEnabled);
    };
    DeliveryPackageItem.prototype.SetUIProperties_IsContainer = function () {
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.Quantity > 0) {
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
        this.UIProperties.SetVisibility("Length", this.ObjectTableName, !this.IsContainer);
        this.UIProperties.SetVisibility("Width", this.ObjectTableName, !this.IsContainer);
        this.UIProperties.SetVisibility("Height", this.ObjectTableName, !this.IsContainer);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, !this.IsContainer && this.IsEditingEnabled);
        this.UIProperties.SetVisibility("ShipperSeal", this.ObjectTableName, this.IsContainer);
        //this.UIProperties.SetVisibility("ContainerNumber", this.ObjectTableName, this.IsContainer);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        if (this.IsContainer) {
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, this.IsEditingEnabled);
        }
        else {
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        }
    };
    DeliveryPackageItem.prototype.SetUIProperties_Harmonize = function () {
        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            isFieldEnabled = true;
            if (this.IsMultiHarmonize == true) {
                isFieldEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Harmonize", this.ObjectTableName, isFieldEnabled);
    };
    Object.defineProperty(DeliveryPackageItem.prototype, "PackageTypeId", {
        get: function () { return this.EntityPM.PackageTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PackageTypeId != value) {
                this.EntityPM.PackageTypeId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.PackageTypeName = null;
                    this.PackageTypeTEU = null;
                    this.ContainerNumber = null;
                    this.Quantity = null;
                    this.ShipperSeal = null;
                    this.IsContainer = false;
                    this.SetUIProperties_IsContainer();
                    this.SetUIPropertiesOfCars(false);
                    this.IsVehicleDetails = false;
                }
                else {
                    var myService = new PackageTypeListService_1.PackageTypeListService();
                    myService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PackageTypeName = list.EnglishName;
                                _this.PackageTypeTEU = list.TEU;
                                _this.IsContainer = list.IsContainer;
                                if (_this.IsContainer) {
                                    _this.Quantity = 1;
                                }
                                else {
                                    _this.ContainerNumber = null;
                                    _this.Quantity = null;
                                    _this.ShipperSeal = null;
                                }
                                _this.SetUIProperties_IsContainer();
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
    Object.defineProperty(DeliveryPackageItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (value) {
            if (this.EntityPM.PackageTypeName != value) {
                this.EntityPM.PackageTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "PackageTypeTEU", {
        get: function () { return this.EntityPM.PackageTypeTEU; },
        set: function (value) {
            if (this.EntityPM.PackageTypeTEU != value) {
                this.EntityPM.PackageTypeTEU = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        set: function (value) {
            if (this.EntityPM.ContainerNumber != value) {
                this.EntityPM.ContainerNumber = value;
                this.ValidateContainerNumber(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "IsMultiHarmonize", {
        get: function () { return this.EntityPM.IsMultiHarmonize; },
        set: function (newValue) {
            if (this.EntityPM.IsMultiHarmonize != newValue) {
                this.EntityPM.IsMultiHarmonize = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    DeliveryPackageItem.prototype.ContainerNumberLostFocus = function (input) {
        this.ValidateContainerNumber(input);
    };
    DeliveryPackageItem.prototype.ValidateContainerNumber = function (input) {
        var warnings = [];
        var error = Tools_1.FormatTool.ValidateContainerNumber(input);
        if (!Tools_1.AppTool.IsNullOrEmpty(error)) {
            warnings.push(error);
        }
        this.WarningErrorsList = warnings;
        this.ContainerNumberWarning = error;
    };
    Object.defineProperty(DeliveryPackageItem.prototype, "ShipperSeal", {
        get: function () { return this.EntityPM.ShipperSeal; },
        set: function (value) {
            if (this.EntityPM.ShipperSeal != value) {
                this.EntityPM.ShipperSeal = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Harmonize", {
        get: function () { return this.EntityPM.Harmonize; },
        set: function (value) {
            if (this.EntityPM.Harmonize != value) {
                this.EntityPM.Harmonize = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (value) {
            if (this.EntityPM.Quantity != value) {
                this.EntityPM.Quantity = value;
                this.ComputeVolume();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (value) {
            if (this.EntityPM.Volume != value) {
                this.EntityPM.Volume = Tools_1.AppTool.Round(value, 3);
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (value) {
            if (this.EntityPM.Weight != value) {
                this.EntityPM.Weight = Tools_1.AppTool.Round(value, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Length", {
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
    Object.defineProperty(DeliveryPackageItem.prototype, "Width", {
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
    Object.defineProperty(DeliveryPackageItem.prototype, "Height", {
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
    Object.defineProperty(DeliveryPackageItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Dimensions", {
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
    Object.defineProperty(DeliveryPackageItem.prototype, "Make", {
        get: function () { return this.EntityPM.Make; },
        set: function (newValue) {
            if (this.EntityPM.Make != newValue) {
                this.EntityPM.Make = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Model", {
        get: function () { return this.EntityPM.Model; },
        set: function (newValue) {
            if (this.EntityPM.Model != newValue) {
                this.EntityPM.Model = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Year", {
        get: function () { return this.EntityPM.Year; },
        set: function (newValue) {
            if (this.EntityPM.Year != newValue) {
                this.EntityPM.Year = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "Color", {
        get: function () { return this.EntityPM.Color; },
        set: function (newValue) {
            if (this.EntityPM.Color != newValue) {
                this.EntityPM.Color = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "ChassisNumber", {
        get: function () { return this.EntityPM.ChassisNumber; },
        set: function (newValue) {
            if (this.EntityPM.ChassisNumber != newValue) {
                this.EntityPM.ChassisNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "RegistrationNumber", {
        get: function () { return this.EntityPM.RegistrationNumber; },
        set: function (newValue) {
            if (this.EntityPM.RegistrationNumber != newValue) {
                this.EntityPM.RegistrationNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackageItem.prototype, "CountryId", {
        get: function () { return this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    DeliveryPackageItem.prototype.ComputeVolume = function () {
        if (this.fatherComponent.ShipmentPM.Ratio == null) {
            this.fatherComponent.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.fatherComponent.ShipmentPM.DirectionId, this.fatherComponent.ShipmentPM.TransportModeId, this.fatherComponent.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.Volume = Tools_1.AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.fatherComponent.ShipmentPM.Ratio, this.fatherComponent.ShipmentPM.DimensionsUnitCode, this.fatherComponent.ShipmentPM.VolumeUnitCode, this.fatherComponent.ShipmentPM.GrossWeightUnitCode);
    };
    return DeliveryPackageItem;
}(BaseComponent_1.BaseComponent));
exports.DeliveryPackageItem = DeliveryPackageItem;
//# sourceMappingURL=DeliveryPackagesTabComponent.js.map