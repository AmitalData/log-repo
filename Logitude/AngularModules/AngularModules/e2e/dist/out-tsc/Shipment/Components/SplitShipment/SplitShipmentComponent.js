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
var Tools_1 = require("../../../Infrastructure/Tools");
var ShipmentPackagePM_1 = require("../../EntityPMs/ShipmentPackagePM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SplitShipmentService_1 = require("../../Services/SplitShipmentService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var SplitShipmentComponent = /** @class */ (function () {
    function SplitShipmentComponent() {
        this.ObjectTableName = "Shipment";
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.DataSource = [];
        this.ShipmentPackages = [];
        this.NewShipmentPackages = [];
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.PackageTypeLabel = null;
    }
    SplitShipmentComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.SetLabels();
        this.EntityPM.ShipmentPackages.forEach(function (item) {
            _this.DataSource.push(new SplitShipmentItem(item, _this));
        });
        this.BuildItemsSource();
    };
    SplitShipmentComponent.prototype.SetLabels = function () {
        this.PackageTypeLabel = this.IsLCLEntity ? "Package Type" : "Container Type";
    };
    SplitShipmentComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ShipmentPackages = [];
        this.NewShipmentPackages = [];
        this.DataSource.forEach(function (item) {
            if (item.IsSplit || item.IsPartialSplit) {
                _this.NewShipmentPackages.push(item);
            }
            else {
                _this.ShipmentPackages.push(item);
            }
        });
    };
    SplitShipmentComponent.prototype.CancelButtonClicked = function () {
        // Clone All Packages
        //this.EntityPM.ShipmentPackages.forEach(item => {
        //    item.IsDirty = false;
        //});
        //this.EntityPM.IsDirty = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    SplitShipmentComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.NewShipmentPackages.length == 0) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Please Choose Packages");
        }
        else {
            this.CurrentSession.StartBusyIndicatorSaving();
            var helper = new SplitShipmentService_1.SplitShipmentHelper();
            helper.OldShipmentId = this.EntityPM.Id;
            helper.Shipment = this.EntityPM;
            helper.SplitPackages = [];
            this.NewShipmentPackages.forEach(function (item) {
                var newItem = new SplitShipmentService_1.SplitPackage();
                newItem.Id = item.Id;
                newItem.IsSplit = item.IsSplit;
                newItem.IsPartialSplit = item.IsPartialSplit;
                newItem.ParentId = item.PartialSplitParentId;
                newItem.Quantity = item.Quantity;
                newItem.Weight = item.Weight;
                newItem.Volume = item.Volume;
                helper.SplitPackages.push(newItem);
            });
            var myService = new SplitShipmentService_1.SplitShipmentService();
            myService.Split(helper).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var updatedHelper = myResponse.Result;
                    _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: updatedHelper.NewShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber });
                    });
                }
            });
        }
    };
    SplitShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SplitShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SplitShipmentComponent);
    return SplitShipmentComponent;
}());
exports.SplitShipmentComponent = SplitShipmentComponent;
var SplitShipmentItem = /** @class */ (function () {
    function SplitShipmentItem(itemPM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.IsSplit = false;
        this.IsPartialSplit = false;
        this.IsPartialSplitParent = false;
        this.PartialSplitParentId = null;
        this._quantity = null;
        this._volume = null;
        this._weight = null;
        this.PackageTypeImage = null;
        this.EntityPM = itemPM;
        this.Quantity = this.EntityPM.Quantity;
        this.Volume = this.EntityPM.Volume;
        this.Weight = this.EntityPM.Weight;
        this.GetImageSource();
        this.EntityPM.CommodityId;
    }
    Object.defineProperty(SplitShipmentItem.prototype, "Id", {
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "Commodity", {
        get: function () {
            var myResult = "";
            if (this.EntityPM.CommodityNumber) {
                myResult = this.EntityPM.CommodityNumber;
                if (this.EntityPM.CommodityName) {
                    myResult += "," + this.EntityPM.CommodityName;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "Quantity", {
        get: function () { return this._quantity; },
        set: function (value) {
            if (this._quantity != value) {
                this._quantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "Volume", {
        get: function () { return this._volume; },
        set: function (value) {
            if (this._volume != value) {
                this._volume = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SplitShipmentItem.prototype, "Weight", {
        get: function () { return this._weight; },
        set: function (value) {
            if (this._weight != value) {
                this._weight = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SplitShipmentItem.prototype.GetImageSource = function () {
        if (this.EntityPM.IsContainer) {
            this.PackageTypeImage = "./Images/CellIcons/Container.png";
        }
        else {
            this.PackageTypeImage = "./Images/CellIcons/Package.png";
        }
    };
    SplitShipmentItem.prototype.SplitClicked = function () {
        this.IsSplit = true;
        this.fatherComponent.BuildItemsSource();
    };
    SplitShipmentItem.prototype.CancelClicked = function () {
        var _this = this;
        this.IsSplit = false;
        if (this.IsPartialSplit) {
            var index = this.fatherComponent.DataSource.indexOf(this);
            if (index > -1) {
                this.fatherComponent.DataSource.splice(index, 1);
            }
            var ParentDataItem = this.fatherComponent.DataSource.filter(function (f) { return f.Id == _this.PartialSplitParentId; })[0];
            if (ParentDataItem) {
                if (ParentDataItem.Quantity && this.Quantity) {
                    ParentDataItem.Quantity += this.Quantity;
                }
                if (ParentDataItem.Volume && this.Volume) {
                    ParentDataItem.Volume += this.Volume;
                }
                if (ParentDataItem.Weight && this.Weight) {
                    ParentDataItem.Weight += this.Weight;
                }
                ParentDataItem.IsPartialSplitParent = this.fatherComponent.DataSource.filter(function (f) { return f.IsPartialSplit && f.PartialSplitParentId == _this.PartialSplitParentId; }).length > 0 ? true : false;
            }
        }
        this.fatherComponent.BuildItemsSource();
    };
    SplitShipmentItem.prototype.PartialSplitClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Partial Split";
        logWindow.WindowArgs = { Item: this };
        logWindow.Show('./Shipment/Components/SplitShipment/SplitPartialPackageComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.IsPartialSplitParent = true;
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.Quantity) && !Tools_1.AppTool.IsNullOrEmpty(comp.Quantity)) {
                        _this.Quantity = _this.Quantity - comp.Quantity;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.Volume) && !Tools_1.AppTool.IsNullOrEmpty(comp.Volume)) {
                        _this.Volume = _this.Volume - comp.Volume;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.Weight) && !Tools_1.AppTool.IsNullOrEmpty(comp.Weight)) {
                        _this.Weight = _this.Weight - comp.Weight;
                    }
                    var newPackagePM = new ShipmentPackagePM_1.ShipmentPackagePM(null);
                    newPackagePM.PackageTypeId = _this.EntityPM.PackageTypeId;
                    newPackagePM.PackageTypeCode = _this.EntityPM.PackageTypeCode;
                    newPackagePM.CommodityId = _this.EntityPM.CommodityId;
                    newPackagePM.CommodityName = _this.EntityPM.CommodityName;
                    newPackagePM.CommodityNumber = _this.EntityPM.CommodityNumber;
                    newPackagePM.Quantity = comp.Quantity;
                    newPackagePM.Volume = comp.Volume;
                    newPackagePM.Weight = comp.Weight;
                    var dataSourceItem = new SplitShipmentItem(newPackagePM, _this.fatherComponent);
                    dataSourceItem.IsPartialSplit = true;
                    dataSourceItem.PartialSplitParentId = _this.EntityPM.Id;
                    _this.fatherComponent.DataSource.push(dataSourceItem);
                    _this.fatherComponent.BuildItemsSource();
                }
            });
        });
    };
    return SplitShipmentItem;
}());
exports.SplitShipmentItem = SplitShipmentItem;
//# sourceMappingURL=SplitShipmentComponent.js.map