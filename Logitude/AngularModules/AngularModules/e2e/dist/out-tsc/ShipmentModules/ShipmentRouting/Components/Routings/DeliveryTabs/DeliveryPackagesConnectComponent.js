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
var ShipmentPickUpDeliveryPackagePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var PickUpDeliveryPackageHarmonizePM_1 = require("../../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM");
var DeliveryPackagesConnectComponent = /** @class */ (function () {
    function DeliveryPackagesConnectComponent() {
        this.DataContext = this;
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.ItemsSource = [];
        this.IsOkButtonEnabled = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SelectedItem = null;
        this.PackageTypeColumnWidth = 80;
    }
    DeliveryPackagesConnectComponent.prototype.SetWindowArgs = function (args) {
        this.fatherComponent = args;
        this.EntityPM = args.EntityPM;
        this.ShipmentPM = args.ShipmentPM;
        this.IsLCLEntity = args.IsLCLEntity;
        this.IsFCLEntity = args.IsFCLEntity;
        this.TransportModeId = args.TransportModeId;
        this.SetLabels();
        this.BuildItemsSource();
    };
    DeliveryPackagesConnectComponent.prototype.SetLabels = function () {
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.Volume').replace('%UnitCode', this.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = "Dimensions (L-W-H) (" + this.ShipmentPM.DimensionsUnitCode + ")";
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPickUpDeliveryPackage.O.Packages.GrossWeight').replace('%UnitCode', this.ShipmentPM.GrossWeightUnitCode);
    };
    DeliveryPackagesConnectComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myPackageTypeColumnWidth = 80;
        this.ShipmentPM.ShipmentPackages.filter(function (f) { return Tools_1.AppTool.IsNullOrEmpty(f.DeliveryId); }).forEach(function (item) {
            var widthOfLabel = Tools_1.AppTool.GetTextWidth(item.PackageTypeName);
            if (widthOfLabel > myPackageTypeColumnWidth) {
                myPackageTypeColumnWidth = widthOfLabel;
            }
            _this.ItemsSource.push(new DeliveryPackagesConnectItem(item, null, _this));
            //item.InsideShipmentPackages.forEach(insideItem => {
            //    this.ItemsSource.push(new DeliveryPackagesConnectItem(item, insideItem, this));
            //});
        });
        if (myPackageTypeColumnWidth > 190) {
            myPackageTypeColumnWidth = 190;
        }
        this.PackageTypeColumnWidth = myPackageTypeColumnWidth;
        this.OnItemsChecked();
    };
    DeliveryPackagesConnectComponent.prototype.OnItemsChecked = function () {
        var isChecked = false;
        if (this.ItemsSource) {
            if (this.ItemsSource.filter(function (f) { return f.IsChecked; })[0]) {
                isChecked = true;
            }
        }
        this.IsOkButtonEnabled = isChecked;
    };
    DeliveryPackagesConnectComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DeliveryPackagesConnectComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ItemsSource.filter(function (f) { return f.IsChecked; }).forEach(function (item) {
            _this.EntityPM.AllConnectedPackagesId.push(item.EntityPM.Id);
            var newPickUpPackPM = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(_this.EntityPM);
            newPickUpPackPM.Tenant = _this.EntityPM.Tenant;
            newPickUpPackPM.ContainerNumber = item.ContainerNumber;
            newPickUpPackPM.Description = item.Description;
            newPickUpPackPM.PackageTypeId = item.PackageTypeId;
            newPickUpPackPM.PackageTypeName = item.PackageTypeName;
            newPickUpPackPM.Quantity = item.Quantity;
            newPickUpPackPM.Volume = item.Volume;
            newPickUpPackPM.Weight = item.Weight;
            newPickUpPackPM.ShipperSeal = item.ShipperSeal;
            newPickUpPackPM.Width = item.Width;
            newPickUpPackPM.Height = item.Height;
            newPickUpPackPM.Length = item.Length;
            newPickUpPackPM.ShipmentPickUpDeliveryId = _this.EntityPM.Id;
            newPickUpPackPM.OriginalShipmentPackageId = item.EntityPM.Id;
            newPickUpPackPM.IsMultiHarmonize = item.IsMultiHarmonize;
            item.EntityPM.ShipmentPackageHarmonizes.forEach(function (harmonizeItem) {
                var harmonize = new PickUpDeliveryPackageHarmonizePM_1.PickUpDeliveryPackageHarmonizePM(newPickUpPackPM);
                harmonize.Harmonize = harmonizeItem.Harmonize;
                harmonize.Tenant = harmonizeItem.Tenant;
                newPickUpPackPM.AddPickUpDeliveryPackageHarmonizePM(harmonize);
            });
            _this.EntityPM.AddPackage(newPickUpPackPM);
        });
        this.fatherComponent.BuildItemsSource();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    DeliveryPackagesConnectComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeliveryPackagesConnectComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeliveryPackagesConnectComponent);
    return DeliveryPackagesConnectComponent;
}());
exports.DeliveryPackagesConnectComponent = DeliveryPackagesConnectComponent;
var DeliveryPackagesConnectItem = /** @class */ (function () {
    function DeliveryPackagesConnectItem(entity, InsideEntityPM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.IsContainer = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isChecked = false;
        this.EntityPM = entity;
        this.InsideEntityPM = InsideEntityPM;
        if (this.InsideEntityPM != null && !Tools_1.AppTool.IsNullOrEmpty(this.InsideEntityPM.PackageTypeId)) {
            this.IsContainer = this.InsideEntityPM.IsContainer;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
            this.IsContainer = this.EntityPM.IsContainer;
        }
    }
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                this.fatherComponent.OnItemsChecked();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "PackageTypeId", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.PackageTypeId : this.EntityPM.PackageTypeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "PackageTypeName", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.PackageTypeName : this.EntityPM.PackageTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Description", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Description : this.EntityPM.Description; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Quantity", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Quantity : this.EntityPM.Quantity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Weight", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Weight : this.EntityPM.Weight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Volume", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Volume : this.EntityPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Length", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Length : this.EntityPM.Length; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Width", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Width : this.EntityPM.Width; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Height", {
        get: function () { return this.InsideEntityPM != null ? this.InsideEntityPM.Height : this.EntityPM.Height; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "ShipperSeal", {
        get: function () { return this.EntityPM.ShipperSeal; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "IsMultiHarmonize", {
        get: function () { return this.EntityPM.IsMultiHarmonize; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeliveryPackagesConnectItem.prototype, "Dimensions", {
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
    return DeliveryPackagesConnectItem;
}());
exports.DeliveryPackagesConnectItem = DeliveryPackagesConnectItem;
//# sourceMappingURL=DeliveryPackagesConnectComponent.js.map