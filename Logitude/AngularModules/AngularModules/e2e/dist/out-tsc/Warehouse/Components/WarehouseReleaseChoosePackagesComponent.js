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
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var WarehouseReleasePackagePM_1 = require("../../Warehouse/EntityPMs/WarehouseReleasePackagePM");
var Tools_1 = require("../../Infrastructure/Tools");
var WarehouseReleaseChoosePackagesComponent = /** @class */ (function (_super) {
    __extends(WarehouseReleaseChoosePackagesComponent, _super);
    function WarehouseReleaseChoosePackagesComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ObjectTableName = "WarehouseEntry";
        _this.WarehouseEntryPackagesLists = [];
        _this.AllWarehouseEntryPackagesLists = [];
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.IsLoadPage = false;
        _this.ShowTextBoxReleaseQTY = false;
        _this.IsContainerShipment = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsStartFilter = false;
        _this.IsShowMessageNoResult = false;
        _this.IsDisableFilter = false;
        _this.transportModeId = "All";
        _this.directionId = "All";
        return _this;
    }
    WarehouseReleaseChoosePackagesComponent.prototype.ngOnInit = function () {
    };
    WarehouseReleaseChoosePackagesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntryPackage").subscribe(function (response) {
            _this.Start(args);
        });
    };
    WarehouseReleaseChoosePackagesComponent.prototype.Start = function (args) {
        this.WarehouseEntryPackagesLists = [];
        this.warehouseReleasePM = args.WarehouseReleasePM;
        this.ViewModelTrigger = args.ViewModelTrigger;
        this.transportModeId = this.ViewModelTrigger.TransportModeId;
        this.DirectionId = this.ViewModelTrigger.DirectionId;
        this.CustomerId = this.ViewModelTrigger.CustomerId;
        this.FromPortId = this.ViewModelTrigger.FromPortId;
        this.ToPortId = this.ViewModelTrigger.ToPortId;
        this.PackageType = args.PackageType;
        this.AllWarehouseEntryPackagesLists = args.WarehouseEntryPackagesLists;
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
        this.IsStartFilter = true;
        this.FilterWarehouseEntryPackageList();
        this.SetValue();
        this.IsLoadPage = true;
    };
    WarehouseReleaseChoosePackagesComponent.prototype.SetValue = function () {
        this.VolumeLabel = this.ViewModelTrigger.VolumeLabel;
        this.GrossWeightLabel = this.ViewModelTrigger.GrossWeightLabel;
        this.DimensionsLabel = this.ViewModelTrigger.DimensionsLabel;
        this.VolumetricWeightLabel = this.ViewModelTrigger.ChargeableWeightLabel;
    };
    WarehouseReleaseChoosePackagesComponent.prototype.FilterWarehouseEntryPackageList = function () {
        var _this = this;
        if (this.IsStartFilter) {
            this.WarehouseEntryPackagesLists = [];
            if (this.PackageType == "Container") {
                this.IsContainerShipment = true;
                this.AllWarehouseEntryPackagesLists.filter(function (d) { return d.IsContainer; }).forEach(function (item) {
                    _this.WarehouseEntryPackagesLists.push(new WarehouseEntryPackageClass(item));
                });
            }
            else {
                this.AllWarehouseEntryPackagesLists.filter(function (d) { return !d.IsContainer; }).forEach(function (item) {
                    _this.WarehouseEntryPackagesLists.push(new WarehouseEntryPackageClass(item));
                });
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId) && this.TransportModeId != "All") {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(function (d) { return d.TransportModeId == _this.TransportModeId; });
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.DirectionId) && this.DirectionId != "All") {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(function (d) { return d.DirectionId == _this.DirectionId; });
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.FromPortId)) {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(function (d) { return d.FromPortId == _this.FromPortId; });
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ToPortId)) {
                this.WarehouseEntryPackagesLists = this.WarehouseEntryPackagesLists.filter(function (d) { return d.ToPortId == _this.ToPortId; });
            }
            if (this.WarehouseEntryPackagesLists.length == 0) {
                this.IsShowMessageNoResult = true;
            }
            else
                this.IsShowMessageNoResult = false;
            if (this.ViewModelTrigger && this.ViewModelTrigger.WarehouseReleasePackagesLists.length > 0) {
                this.WarehouseEntryPackagesLists.forEach(function (item) {
                    var temp = _this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(function (d) { return d.WarehouseReleaseId == item.EntityPM.Id; })[0];
                    if (temp)
                        item.IsSelected = true;
                });
            }
        }
    };
    WarehouseReleaseChoosePackagesComponent.prototype.CloseButtonClicked = function () {
        this.WarehouseEntryPackagesLists.forEach(function (item) {
            item.EntityPM.ReleaseQTY = item.OldReleaseQTY;
            item.EntityPM.IsSelected = item.OldIsSelected;
        });
        this.CurrentSession.CloseCurrentWindow();
    };
    WarehouseReleaseChoosePackagesComponent.prototype.SetEnableProp = function () {
        if (this.UIProperties && this.IsLoadPage) {
            if (this.WarehouseEntryPackagesLists.filter(function (d) { return d.IsSelected; })[0]) {
                this.UIProperties.SetEnabled("TransportModeId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("DirectionId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, false);
                this.IsDisableFilter = true;
            }
            else {
                this.UIProperties.SetEnabled("TransportModeId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("DirectionId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, true);
                this.IsDisableFilter = false;
            }
        }
    };
    WarehouseReleaseChoosePackagesComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var itemvalid = this.WarehouseEntryPackagesLists.filter(function (d) { return d.ReleaseQTY > d.Instock; })[0];
        if (itemvalid) {
            this.ValidationErrorsList.push("The release quantity must be less than or equal to in stock quantity");
        }
        if (this.ValidationErrorsList.length == 0) {
            //if (this.ViewModelTrigger.WarehouseReleasePackagesLists) {
            //    this.ViewModelTrigger.WarehouseReleasePackagesLists.forEach((item) => {
            //        var warehouseEntryPackages: any = this.WarehouseEntryPackagesLists.filter(d => d.EntityPM.Id == item.EntryPackageId)[0];
            //        if (!warehouseEntryPackages) {
            //            this.ViewModelTrigger.WarehouseReleasePackagesLists = this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(d => d.EntryPackageId != item.EntryPackageId);
            //        }
            //    });
            //}
            this.WarehouseEntryPackagesLists.forEach(function (item) {
                if (item.IsSelected) {
                    var newWarehouseReleasePackagePM = _this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(function (d) { return d.EntryPackageId == item.EntityPM.Id; })[0];
                    if (!newWarehouseReleasePackagePM)
                        var newWarehouseReleasePackagePM = new WarehouseReleasePackagePM_1.WarehouseReleasePackagePM(null);
                    newWarehouseReleasePackagePM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
                    newWarehouseReleasePackagePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    newWarehouseReleasePackagePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    newWarehouseReleasePackagePM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    newWarehouseReleasePackagePM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    newWarehouseReleasePackagePM.WarehouseReleaseId = _this.warehouseReleasePM.Id;
                    newWarehouseReleasePackagePM.Id = "1-1";
                    newWarehouseReleasePackagePM.Quantity = item.EntityPM.ReleaseQTY;
                    newWarehouseReleasePackagePM.ContainerNumber = item.ContainerNumber;
                    newWarehouseReleasePackagePM.Width = item.EntityPM.Width;
                    newWarehouseReleasePackagePM.Dimensions = (item.EntityPM.Length ? item.EntityPM.Length : "") + "-" + (item.EntityPM.Width ? item.EntityPM.Width : "") + "-" + (item.EntityPM.Height ? item.EntityPM.Height : "");
                    newWarehouseReleasePackagePM.Volume = item.EntityPM.Volume;
                    newWarehouseReleasePackagePM.Weight = item.EntityPM.Weight;
                    newWarehouseReleasePackagePM.Harmonize = item.EntityPM.Harmonize;
                    newWarehouseReleasePackagePM.Length = item.EntityPM.Length;
                    newWarehouseReleasePackagePM.Height = item.EntityPM.Height;
                    newWarehouseReleasePackagePM.IsContainer = item.EntityPM.IsContainer;
                    newWarehouseReleasePackagePM.PackageTypeName = item.PackageTypeName;
                    newWarehouseReleasePackagePM.Seal = item.EntityPM.Seal;
                    newWarehouseReleasePackagePM.Tenant = item.EntityPM.Tenant;
                    newWarehouseReleasePackagePM.PackageTypeId = item.EntityPM.PackageTypeId;
                    newWarehouseReleasePackagePM.EntryPackageId = item.EntityPM.Id;
                    newWarehouseReleasePackagePM.Description = item.EntityPM.Description;
                    newWarehouseReleasePackagePM.ContainerNumberWarning = item.EntityPM.ContainerNumberWarning;
                    newWarehouseReleasePackagePM.ActualReleaseDate = item.EntityPM.ActualEntryDate;
                    newWarehouseReleasePackagePM.VolumetricWeight = item.EntityPM.VolumetricWeight;
                    var existItem = _this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(function (d) { return d.EntryPackageId == newWarehouseReleasePackagePM.EntryPackageId; })[0];
                    if (!existItem) {
                        _this.ViewModelTrigger.WarehouseReleasePackagesLists.push(newWarehouseReleasePackagePM);
                    }
                }
                else {
                    var existItem = _this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(function (d) { return d.EntryPackageId == item.EntityPM.Id; })[0];
                    if (existItem) {
                        _this.ViewModelTrigger.WarehouseReleasePackagesLists = _this.ViewModelTrigger.WarehouseReleasePackagesLists.filter(function (d) { return d.EntryPackageId != item.EntityPM.Id; });
                    }
                    item.EntityPM.ReleaseQTY = 0;
                }
            });
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    Object.defineProperty(WarehouseReleaseChoosePackagesComponent.prototype, "TransportModeId", {
        get: function () {
            this.SetEnableProp();
            return this.transportModeId;
        },
        set: function (newValue) {
            if (this.transportModeId != newValue) {
                this.transportModeId = newValue;
                this.FilterWarehouseEntryPackageList();
                this.ViewModelTrigger.TransportModeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseReleaseChoosePackagesComponent.prototype, "DirectionId", {
        get: function () { return this.directionId; },
        set: function (newValue) {
            if (this.directionId != newValue) {
                this.directionId = newValue;
                this.ViewModelTrigger.DirectionId = newValue;
                this.FilterWarehouseEntryPackageList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseReleaseChoosePackagesComponent.prototype, "CustomerId", {
        get: function () {
            return this.customerId;
        },
        set: function (newValue) {
            if (this.customerId != newValue) {
                this.customerId = newValue;
                this.ViewModelTrigger.CustomerId = newValue;
                this.FilterWarehouseEntryPackageList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseReleaseChoosePackagesComponent.prototype, "FromPortId", {
        get: function () { return this.fromPortId; },
        set: function (newValue) {
            if (this.fromPortId != newValue) {
                this.fromPortId = newValue;
                this.ViewModelTrigger.FromPortId = newValue;
                this.FilterWarehouseEntryPackageList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseReleaseChoosePackagesComponent.prototype, "ToPortId", {
        get: function () { return this.toPortId; },
        set: function (newValue) {
            if (this.toPortId != newValue) {
                this.toPortId = newValue;
                this.ViewModelTrigger.ToPortId = newValue;
                this.FilterWarehouseEntryPackageList();
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseReleaseChoosePackagesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'WarehouseReleaseChoosePackagesComponent',
            templateUrl: './WarehouseReleaseChoosePackagesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WarehouseReleaseChoosePackagesComponent);
    return WarehouseReleaseChoosePackagesComponent;
}(BaseComponent_1.BaseComponent));
exports.WarehouseReleaseChoosePackagesComponent = WarehouseReleaseChoosePackagesComponent;
var WarehouseEntryPackageClass = /** @class */ (function (_super) {
    __extends(WarehouseEntryPackageClass, _super);
    function WarehouseEntryPackageClass(entityPM) {
        var _this = _super.call(this) || this;
        _this.IsConnectedToShipment = false;
        _this.IsSelectedKeyId = Guid_1.Guid.newGuid();
        _this.PackageTypeName = entityPM.PackageTypeName;
        _this.ContainerNumberWarning = entityPM.ContainerNumberWarning;
        _this.ContainerNumber = entityPM.ContainerNumber;
        _this.Dimensions = entityPM.Dimensions;
        _this.Quantity = entityPM.Quantity;
        _this.Volume = entityPM.Volume;
        _this.Weight = entityPM.Weight;
        _this.VolumetricWeight = entityPM.VolumetricWeight;
        _this.Description = entityPM.Description;
        _this.Instock = entityPM.Instock;
        _this.IsContainer = entityPM.IsContainer;
        _this.EntryPackageId = entityPM.Id;
        _this.EntityPM = entityPM;
        _this.OldReleaseQTY = entityPM.ReleaseQTY;
        _this.OldIsSelected = entityPM.IsSelected;
        _this.TransportModeId = entityPM.TransportModeId;
        _this.DirectionId = entityPM.DirectionId;
        _this.FromPortId = entityPM.FromPortId;
        _this.ToPortId = entityPM.ToPortId;
        _this.CustomerId = entityPM.CustomerId;
        _this.IsConnectedToShipment = entityPM.IsConnectedToShipment;
        return _this;
    }
    Object.defineProperty(WarehouseEntryPackageClass.prototype, "ReleaseQTY", {
        get: function () {
            var releaseQTY = 0;
            if (this.EntityPM) {
                releaseQTY = this.EntityPM.ReleaseQTY;
            }
            return releaseQTY;
        },
        set: function (newValue) {
            if (this.ReleaseQTY != newValue) {
                if (newValue) {
                    if (newValue > 0) {
                        this.EntityPM.ReleaseQTY = newValue;
                        this.ReleaseQTY = newValue;
                        if (!this.IsSelected) {
                            this.IsFullReleaseQTYAuto = true;
                            this.IsSelected = true;
                        }
                    }
                }
                else {
                    this.EntityPM.ReleaseQTY = newValue;
                    this.ReleaseQTY = newValue;
                    this.IsSelected = false;
                    this.IsFullReleaseQTYAuto = true;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WarehouseEntryPackageClass.prototype, "IsSelected", {
        get: function () {
            var iselected = false;
            if (this.EntityPM) {
                iselected = this.EntityPM.IsSelected;
                ;
            }
            return iselected;
        },
        set: function (newValue) {
            if (this.IsSelected != newValue) {
                this.EntityPM.IsSelected = newValue;
                this.IsSelected = newValue;
                if (newValue) {
                    if (!this.IsFullReleaseQTYAuto)
                        this.ReleaseQTY = this.Instock;
                    else
                        this.IsFullReleaseQTYAuto = false;
                }
                else {
                    if (!this.IsFullReleaseQTYAuto)
                        this.ReleaseQTY = 0;
                    else
                        this.IsFullReleaseQTYAuto = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseEntryPackageClass.prototype.ReleaseQTYLostFocusMethod = function (value) {
        this.ReleaseQTY = value;
    };
    return WarehouseEntryPackageClass;
}(BaseComponent_1.BaseComponent));
exports.WarehouseEntryPackageClass = WarehouseEntryPackageClass;
//# sourceMappingURL=WarehouseReleaseChoosePackagesComponent.js.map