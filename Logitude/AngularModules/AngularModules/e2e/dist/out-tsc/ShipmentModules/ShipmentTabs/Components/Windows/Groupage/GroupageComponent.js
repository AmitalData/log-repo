"use strict";
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
var ShipmentPackagePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentPackagePM");
var InsideShipmentPackagePM_1 = require("../../../../../Shipment/EntityPMs/InsideShipmentPackagePM");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ServiceLocator_1 = require("../../../../../Infrastructure/Locators/ServiceLocator");
var GroupageComponent = /** @class */ (function () {
    function GroupageComponent() {
        this.AllPackages = [];
        this.ShipmentsPackages = [];
        this.MyGroupagePackages = [];
        this.ToggleItems = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    GroupageComponent.prototype.SetWindowArgs = function (args) {
        this.AllPackages = args['AllPackages'];
        this.FatherComponent = args['FatherComponent'];
        this.EntityPM = this.FatherComponent.EntityPM;
        this.SetLabels();
        this.BuildMyGroupagePackages();
        this.BuildShipmentsPackages();
    };
    GroupageComponent.prototype.SetLabels = function () {
        this.VolumeColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
    };
    GroupageComponent.prototype.BuildShipmentsPackages = function () {
        var _this = this;
        this.ShipmentsPackages = [];
        this.AllPackages.forEach(function (item) {
            var isAlreadyAddedToContainer = false;
            _this.MyGroupagePackages.forEach(function (itemContainer) {
                if (!isAlreadyAddedToContainer) {
                    var itemInsideContainer = itemContainer.ItemsSource.filter(function (f) { return f.EntityPM.OriginalShipmentPackageId == item.Id; })[0];
                    if (itemInsideContainer) {
                        isAlreadyAddedToContainer = true;
                    }
                }
            });
            if (!isAlreadyAddedToContainer) {
                var itemComponent = new GroupageListItem(item, _this, false);
                _this.ShipmentsPackages.push(itemComponent);
                itemComponent.UpdateItem();
            }
        });
    };
    GroupageComponent.prototype.BuildMyGroupagePackages = function () {
        var _this = this;
        this.MyGroupagePackages = [];
        this.EntityPM.ShipmentPackages.forEach(function (item) {
            var itemComponent = new GroupageListItem(item, _this, true);
            _this.MyGroupagePackages.push(itemComponent);
            itemComponent.UpdateItem();
        });
        this.BuildToggleItems();
    };
    GroupageComponent.prototype.BuildToggleItems = function () {
        var _this = this;
        this.ToggleItems = [];
        this.MyGroupagePackages.forEach(function (item) {
            var index = _this.MyGroupagePackages.indexOf(item) + 1;
            var itemString = "Container #" + index;
            if (!Tools_1.AppTool.IsNullOrEmpty(item.ContainerNumber)) {
                itemString += ": " + item.ContainerNumber;
            }
            _this.ToggleItems.push(new ToggleItem(itemString, item.ContainerNumber));
        });
        this.ToggleItems.push(new ToggleItem("New Container", null));
    };
    GroupageComponent.prototype.AddButtonClicked = function (toggleItem, shipmentListItem) {
        if (toggleItem.Label == "New Container") {
            this.AddToNewContainer(toggleItem, shipmentListItem);
        }
        else {
            this.AddToExistingContainer(toggleItem, shipmentListItem);
        }
    };
    GroupageComponent.prototype.AddToNewContainer = function (toggleItem, shipmentListItem) {
        var shipmentPackagePM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
        shipmentPackagePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        shipmentPackagePM.ShipmentId = this.EntityPM.Id;
        shipmentPackagePM.ShipmentNumber = this.EntityPM.ShipmentNumber;
        shipmentPackagePM.IsContainer = true;
        shipmentPackagePM.Quantity = 1;
        shipmentPackagePM.ContainerNumber = shipmentListItem.ContainerNumber;
        shipmentPackagePM.Weight = 0;
        var insideShipmentPack = new InsideShipmentPackagePM_1.InsideShipmentPackagePM(shipmentPackagePM);
        insideShipmentPack.Quantity = shipmentListItem.EntityPM.Quantity;
        insideShipmentPack.Height = shipmentListItem.EntityPM.Height;
        insideShipmentPack.Length = shipmentListItem.EntityPM.Length;
        insideShipmentPack.Width = shipmentListItem.EntityPM.Width;
        insideShipmentPack.Weight = shipmentListItem.EntityPM.Weight;
        insideShipmentPack.PackageTypeId = shipmentListItem.EntityPM.PackageTypeId;
        insideShipmentPack.PackageTypeName = shipmentListItem.EntityPM.PackageTypeName;
        insideShipmentPack.Volume = shipmentListItem.EntityPM.Volume;
        insideShipmentPack.VolumetricWeight = shipmentListItem.EntityPM.VolumetricWeight;
        insideShipmentPack.Tenant = shipmentListItem.EntityPM.Tenant;
        insideShipmentPack.Description = shipmentListItem.EntityPM.Description;
        insideShipmentPack.OriginalShipmentPackageId = shipmentListItem.EntityPM.Id;
        insideShipmentPack.Reference1 = shipmentListItem.Reference1;
        insideShipmentPack.Reference2 = shipmentListItem.Reference2;
        insideShipmentPack.Reference3 = shipmentListItem.Reference3;
        insideShipmentPack.Reference4 = shipmentListItem.Reference4;
        insideShipmentPack.CommodityNumber = shipmentListItem.CommodityNumber;
        insideShipmentPack.CommodityName = shipmentListItem.CommodityName;
        shipmentPackagePM.AddInsideShipmentPackagePM(insideShipmentPack);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: shipmentPackagePM, ShipmentListItem: shipmentListItem, FatherComponent: this };
        logWindow.Title = "New Container";
        logWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Groupage/GroupageContainerComponent");
    };
    GroupageComponent.prototype.AddToExistingContainer = function (toggleItem, shipmentListItem) {
        var MasterListItem = this.MyGroupagePackages.filter(function (f) { return f.ContainerNumber == toggleItem.ContainerNumber; })[0];
        if (MasterListItem) {
            var insideShipmentPack = new InsideShipmentPackagePM_1.InsideShipmentPackagePM(null);
            insideShipmentPack.Quantity = shipmentListItem.EntityPM.Quantity;
            insideShipmentPack.Height = shipmentListItem.EntityPM.Height;
            insideShipmentPack.Length = shipmentListItem.EntityPM.Length;
            insideShipmentPack.Width = shipmentListItem.EntityPM.Width;
            insideShipmentPack.Weight = shipmentListItem.EntityPM.Weight;
            insideShipmentPack.PackageTypeId = shipmentListItem.EntityPM.PackageTypeId;
            insideShipmentPack.PackageTypeName = shipmentListItem.EntityPM.PackageTypeName;
            insideShipmentPack.Volume = shipmentListItem.EntityPM.Volume;
            insideShipmentPack.VolumetricWeight = shipmentListItem.EntityPM.VolumetricWeight;
            insideShipmentPack.Tenant = shipmentListItem.EntityPM.Tenant;
            insideShipmentPack.Description = shipmentListItem.EntityPM.Description;
            insideShipmentPack.OriginalShipmentPackageId = shipmentListItem.EntityPM.Id;
            insideShipmentPack.Reference1 = shipmentListItem.Reference1;
            insideShipmentPack.Reference2 = shipmentListItem.Reference2;
            insideShipmentPack.Reference3 = shipmentListItem.Reference3;
            insideShipmentPack.Reference4 = shipmentListItem.Reference4;
            insideShipmentPack.CommodityNumber = shipmentListItem.CommodityNumber;
            insideShipmentPack.CommodityName = shipmentListItem.CommodityName;
            MasterListItem.EntityPM.AddInsideShipmentPackagePM(insideShipmentPack);
            var indexOfItem = this.ShipmentsPackages.indexOf(shipmentListItem);
            if (indexOfItem > -1) {
                this.ShipmentsPackages.splice(indexOfItem, 1);
            }
            MasterListItem.BuildItems();
            MasterListItem.ComputeFromInsidePackages();
        }
    };
    GroupageComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    GroupageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.ShipmentPackages.length > 0 || this.MyGroupagePackages.length > 0) {
            this.EntityPM.ShipmentPackages.forEach(function (item) {
                _this.EntityPM.RemovePackage(item);
            });
            this.MyGroupagePackages.forEach(function (item) {
                var newPackage = new ShipmentPackagePM_1.ShipmentPackagePM(_this.EntityPM);
                newPackage.ShipmentId = _this.EntityPM.Id;
                newPackage.ShipmentNumber = _this.EntityPM.ShipmentNumber;
                newPackage.Tenant = _this.EntityPM.Tenant;
                newPackage.ClassNumber = item.EntityPM.ClassNumber;
                newPackage.ContainerNumber = item.EntityPM.ContainerNumber;
                newPackage.Description = item.EntityPM.Description;
                newPackage.FlashPoint = item.EntityPM.FlashPoint;
                newPackage.Harmonize = item.EntityPM.Harmonize;
                newPackage.Height = item.EntityPM.Height;
                newPackage.IMDGCode = item.EntityPM.IMDGCode;
                newPackage.IsContainer = item.EntityPM.IsContainer;
                newPackage.IsDangerous = item.EntityPM.IsDangerous;
                newPackage.Length = item.EntityPM.Length;
                newPackage.MarksAndNumbers = item.EntityPM.MarksAndNumbers;
                newPackage.MaterialDescription = item.EntityPM.MaterialDescription;
                newPackage.PackageTypeId = item.EntityPM.PackageTypeId;
                newPackage.PackageTypeName = item.EntityPM.PackageTypeName;
                newPackage.PackagingGroup = item.EntityPM.PackagingGroup;
                newPackage.Quantity = item.EntityPM.Quantity;
                newPackage.ShipperSeal = item.EntityPM.ShipperSeal;
                newPackage.CarrierSeal = item.EntityPM.CarrierSeal;
                newPackage.SOC = item.EntityPM.SOC;
                newPackage.Tare = item.EntityPM.Tare;
                newPackage.Temperature = item.EntityPM.Temperature;
                newPackage.UnNumber = item.EntityPM.UnNumber;
                newPackage.Ventilation = item.EntityPM.Ventilation;
                newPackage.Volume = item.EntityPM.Volume;
                newPackage.VolumetricWeight = item.EntityPM.VolumetricWeight;
                newPackage.Weight = item.EntityPM.Weight;
                newPackage.Width = item.EntityPM.Width;
                newPackage.OriginalShipmentPackageId = item.EntityPM.OriginalShipmentPackageId;
                newPackage.Reference1 = item.EntityPM.Reference1;
                newPackage.Reference2 = item.EntityPM.Reference2;
                newPackage.Reference3 = item.EntityPM.Reference3;
                newPackage.Reference4 = item.EntityPM.Reference4;
                newPackage.CommodityNumber = item.EntityPM.CommodityNumber;
                newPackage.CommodityName = item.EntityPM.CommodityName;
                _this.EntityPM.AddPackage(newPackage);
                item.EntityPM.InsideShipmentPackages.forEach(function (insideItem) {
                    var newInsidePackage = new InsideShipmentPackagePM_1.InsideShipmentPackagePM(newPackage);
                    newInsidePackage.ShipmentPackageId = newPackage.Id;
                    newInsidePackage.Quantity = insideItem.Quantity;
                    newInsidePackage.Height = insideItem.Height;
                    newInsidePackage.Length = insideItem.Length;
                    newInsidePackage.Width = insideItem.Width;
                    newInsidePackage.Weight = insideItem.Weight;
                    newInsidePackage.PackageTypeId = insideItem.PackageTypeId;
                    newInsidePackage.PackageTypeName = insideItem.PackageTypeName;
                    newInsidePackage.Volume = insideItem.Volume;
                    newInsidePackage.VolumetricWeight = insideItem.VolumetricWeight;
                    newInsidePackage.Tenant = insideItem.Tenant;
                    newInsidePackage.Description = insideItem.Description;
                    newInsidePackage.OriginalShipmentPackageId = insideItem.OriginalShipmentPackageId;
                    newInsidePackage.OriginalInsideShipmentPackageId = insideItem.OriginalInsideShipmentPackageId;
                    newInsidePackage.Reference1 = insideItem.Reference1;
                    newInsidePackage.Reference2 = insideItem.Reference2;
                    newInsidePackage.Reference3 = insideItem.Reference3;
                    newInsidePackage.Reference4 = insideItem.Reference4;
                    newInsidePackage.CommodityNumber = insideItem.CommodityNumber;
                    newInsidePackage.CommodityName = insideItem.CommodityName;
                    newPackage.AddInsideShipmentPackagePM(newInsidePackage);
                });
            });
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Master", "Building packages for ocean groupage");
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    GroupageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GroupageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GroupageComponent);
    return GroupageComponent;
}());
exports.GroupageComponent = GroupageComponent;
var GroupageListItem = /** @class */ (function () {
    function GroupageListItem(item, fatherComponent, isGroupage) {
        this.fatherComponent = fatherComponent;
        this.Index = null;
        this.IsGroupageItem = false;
        this.ShipmentPM = null;
        this.ItemsSource = [];
        this.InsideGridHeight = 70;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.PackageTypeImage = null;
        this.EntityPM = item;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsGroupageItem = isGroupage;
        this.BuildItems();
        this.UpdateItem();
    }
    GroupageListItem.prototype.BuildItems = function () {
        var _this = this;
        this.ItemsSource = [];
        this.EntityPM.InsideShipmentPackages.forEach(function (item) {
            var myRatio = _this.fatherComponent.EntityPM.Ratio;
            if (Tools_1.AppTool.IsNullOrZero(myRatio)) {
                myRatio = 1;
            }
            item.VolumetricWeight = (item.Volume * 1000) / myRatio;
            _this.ItemsSource.push(new GroupageInsideItem(item));
        });
        this.InsideGridHeight = (this.ItemsSource.length * 26) + 44;
    };
    GroupageListItem.prototype.UpdateItem = function () {
        if (this.IsGroupageItem) {
            this.Index = this.fatherComponent.MyGroupagePackages.indexOf(this) + 1;
            this.PackageTypeImage = "./Images/Icons/Container.png";
        }
        else {
            this.Index = this.fatherComponent.ShipmentsPackages.indexOf(this) + 1;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
                if (this.EntityPM.IsContainer) {
                    this.PackageTypeImage = "./Images/Icons/Container.png";
                }
                else {
                    this.PackageTypeImage = "./Images/Icons/Package.png";
                }
            }
        }
    };
    GroupageListItem.prototype.ComputeFromInsidePackages = function () {
        if (this.IsGroupageItem) {
            var myWeight = 0;
            var myVolume = 0;
            this.EntityPM.InsideShipmentPackages.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Weight)) {
                    myWeight += item.Weight;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }
            });
            this.Weight = Tools_1.AppTool.Round(myWeight, 3);
            this.Volume = Tools_1.AppTool.Round(myVolume, 3);
        }
    };
    Object.defineProperty(GroupageListItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        set: function (value) {
            if (this.EntityPM.Weight != value) {
                this.EntityPM.Weight = Tools_1.AppTool.Round(value, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Volume", {
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
    Object.defineProperty(GroupageListItem.prototype, "VolumetricWeight", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        set: function (value) {
            if (this.EntityPM.VolumetricWeight != value) {
                this.EntityPM.VolumetricWeight = Tools_1.AppTool.Round(value, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Reference1", {
        get: function () { return this.EntityPM.Reference1; },
        set: function (newValue) {
            if (this.EntityPM.Reference1 != newValue) {
                this.EntityPM.Reference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Reference2", {
        get: function () { return this.EntityPM.Reference2; },
        set: function (newValue) {
            if (this.EntityPM.Reference2 != newValue) {
                this.EntityPM.Reference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Reference3", {
        get: function () { return this.EntityPM.Reference3; },
        set: function (newValue) {
            if (this.EntityPM.Reference3 != newValue) {
                this.EntityPM.Reference3 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "Reference4", {
        get: function () { return this.EntityPM.Reference4; },
        set: function (newValue) {
            if (this.EntityPM.Reference4 != newValue) {
                this.EntityPM.Reference4 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "CommodityNumber", {
        get: function () { return this.EntityPM.CommodityNumber; },
        set: function (newValue) {
            if (this.EntityPM.CommodityNumber != newValue) {
                this.EntityPM.CommodityNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageListItem.prototype, "CommodityName", {
        get: function () { return this.EntityPM.CommodityName; },
        set: function (newValue) {
            if (this.EntityPM.CommodityName != newValue) {
                this.EntityPM.CommodityName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    GroupageListItem.prototype.ComputeVolumetricWeight = function () {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = Tools_1.AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator_1.SessionLocator.TenantPM.CountryCode);
        }
        this.VolumetricWeight = Tools_1.AppTool.ComputePackageVolumetricWeight(this.EntityPM.Quantity, this.EntityPM.Width, this.EntityPM.Height, this.EntityPM.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    };
    GroupageListItem.prototype.DeleteButtonClicked = function () {
        if (this.ItemsSource.length > 0) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = "Remove Container";
            messageWindow.Show("Cant remove this container while it contains inside items");
        }
        else {
            var indexOfItem = this.fatherComponent.MyGroupagePackages.indexOf(this);
            if (indexOfItem > -1) {
                this.fatherComponent.MyGroupagePackages.splice(indexOfItem, 1);
            }
            this.fatherComponent.MyGroupagePackages.forEach(function (item) {
                item.UpdateItem();
            });
            this.fatherComponent.BuildToggleItems();
        }
    };
    GroupageListItem.prototype.DeleteInsideButtonClicked = function (item) {
        if (item) {
            this.EntityPM.RemoveInsideShipmentPackagePM(item.EntityPM);
            var indexOfItem = this.ItemsSource.indexOf(item);
            if (indexOfItem > -1) {
                this.ItemsSource.splice(indexOfItem, 1);
            }
            this.ComputeFromInsidePackages();
            this.fatherComponent.BuildToggleItems();
            this.fatherComponent.BuildShipmentsPackages();
        }
        //if (trigger.shipmentPackagePM.InsideShipmentPackages.Contains(insidePackage)) {
        //    trigger.shipmentPackagePM.InsideShipmentPackages.Remove(insidePackage);
        //}
        //if (trigger.InsideObsList.Contains(this)) {
        //    trigger.InsideObsList.Remove(this);
        //}
        //ShipmentPackagePM pp = trigger.Trigger.OriginList.Where(d => d.Id == this.insidePackage.OriginalShipmentPackageId).FirstOrDefault();
        //if (pp != null) {
        //    trigger.Trigger.ShipmentsPackagesList.Add(new BuildPackagesListBoxItemViewModel(trigger.Trigger, pp));
        //    trigger.ComputeFromInsidePackages();
        //}
        //trigger.UpdateInsidePackages();   
    };
    return GroupageListItem;
}());
exports.GroupageListItem = GroupageListItem;
var GroupageInsideItem = /** @class */ (function () {
    function GroupageInsideItem(entity) {
        this.EntityPM = null;
        this.EntityPM = entity;
    }
    Object.defineProperty(GroupageInsideItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Weight", {
        get: function () { return this.EntityPM.Weight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "VolumeKG", {
        get: function () { return this.EntityPM.VolumetricWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Length", {
        get: function () { return this.EntityPM.Length; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Width", {
        get: function () { return this.EntityPM.Width; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Height", {
        get: function () { return this.EntityPM.Height; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupageInsideItem.prototype, "Dimensions", {
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
    return GroupageInsideItem;
}());
exports.GroupageInsideItem = GroupageInsideItem;
var ToggleItem = /** @class */ (function () {
    function ToggleItem(label, myContainerNumber) {
        this.Label = label;
        this.ContainerNumber = myContainerNumber;
    }
    return ToggleItem;
}());
//# sourceMappingURL=GroupageComponent.js.map