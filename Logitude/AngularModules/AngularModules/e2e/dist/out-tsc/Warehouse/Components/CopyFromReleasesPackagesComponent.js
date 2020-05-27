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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var WarehouseReleaseListExtendedService_1 = require("../../Warehouse/Services/ExtendedLists/WarehouseReleaseListExtendedService");
var WarehouseReleasePackagePMExtendedService_1 = require("../../Warehouse/Services/ExtendedPMs/WarehouseReleasePackagePMExtendedService");
var Tools_1 = require("../../Infrastructure/Tools");
var ShipmentPickUpDeliveryPackagePM_1 = require("../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var CopyFromReleasesPackagesComponent = /** @class */ (function () {
    function CopyFromReleasesPackagesComponent(_warehouseReleaseListExtendedService, _warehouseReleasePackagePMExtendedService) {
        this._warehouseReleaseListExtendedService = _warehouseReleaseListExtendedService;
        this._warehouseReleasePackagePMExtendedService = _warehouseReleasePackagePMExtendedService;
        this.IsNoReleasePackage = false;
        this.LabelPackageRleaseArea = "Pressing on Copy Packages button will copy the release packages to your Delivery Packages ";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.WarehouseReleaseLists = [];
    }
    CopyFromReleasesPackagesComponent.prototype.ngOnInit = function () {
    };
    CopyFromReleasesPackagesComponent.prototype.SetWindowArgs = function (args) {
        this.ShipmentPM = args.ShipmentPM;
        this.ShipmentDeliveryPM = args.ShipmentDeliveryPM;
        this.FatherComponent = args.FatherComponent;
        this.LoadWarehouseReleasesPackages();
    };
    CopyFromReleasesPackagesComponent.prototype.LoadWarehouseReleasesPackages = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.WarehouseReleaseLists = [];
        if (this.ShipmentPM != null) {
            this._warehouseReleaseListExtendedService.getWarehouseReleaseListsByShipmentId(this.ShipmentPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                // IsNoReleasePackage
                if (!pmResponse.HasError) {
                    if (pmResponse.Result && pmResponse.Result.length > 0) {
                        pmResponse.Result.forEach(function (item) {
                            _this.WarehouseReleaseLists.push(new WarehouseReleaseClass(item));
                        });
                    }
                    else
                        _this.IsNoReleasePackage = true;
                }
            });
        }
    };
    CopyFromReleasesPackagesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CopyFromReleasesPackagesComponent.prototype.CopyPackagebuttonClick = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.YesButtonText = "confirm";
        confirmWindow.Title = "Confirmation Message";
        confirmWindow.Show("Please confirm copying packages from " + (item.ReleaseNumberLabel + item.ReleaseNumberValue));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentSession.CurrentWindow.StartBusyIndicator("Copy release Package...");
                if (!item.IsLoad) {
                    _this._warehouseReleasePackagePMExtendedService.GetWarehouseReleasePackagePMListsByWarehouseReleaseId(item.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            item.WarehouseReleasePackage = pmResponse.Result;
                            item.IsLoad = true;
                            _this.CopyReleasePackageToShipmentDeliveryPM(item);
                        }
                        else
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    });
                }
                else {
                    _this.CopyReleasePackageToShipmentDeliveryPM(item);
                }
            }
        });
    };
    CopyFromReleasesPackagesComponent.prototype.CopyReleasePackageToShipmentDeliveryPM = function (item) {
        var _this = this;
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        if (item != null) {
            if (item.WarehouseReleasePackage != null && item.WarehouseReleasePackage.length > 0) {
                item.WarehouseReleasePackage.forEach(function (item) {
                    var newPickUpPackPM = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(_this.ShipmentDeliveryPM);
                    newPickUpPackPM.Tenant = _this.ShipmentDeliveryPM.Tenant;
                    newPickUpPackPM.ContainerNumber = item.ContainerNumber;
                    newPickUpPackPM.Description = item.Description;
                    newPickUpPackPM.PackageTypeId = item.PackageTypeId;
                    newPickUpPackPM.PackageTypeName = item.PackageTypeName;
                    newPickUpPackPM.Quantity = item.Quantity;
                    newPickUpPackPM.Volume = item.Volume;
                    newPickUpPackPM.Weight = item.Weight;
                    newPickUpPackPM.ShipperSeal = item.Seal;
                    newPickUpPackPM.Width = item.Width;
                    newPickUpPackPM.Height = item.Height;
                    newPickUpPackPM.Length = item.Length;
                    newPickUpPackPM.Harmonize = item.Harmonize;
                    newPickUpPackPM.ShipmentPickUpDeliveryId = _this.ShipmentDeliveryPM.Id;
                    _this.ShipmentDeliveryPM.AddPackage(newPickUpPackPM);
                    _this.FatherComponent.BuildItemsSource();
                });
                this.CloseButtonClicked();
            }
        }
    };
    CopyFromReleasesPackagesComponent.prototype.ViewEntity = function (item) {
        var _this = this;
        var myBackButtonLabel = "Warehouse Releases";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.LoadWarehouseReleasesPackages();
            });
        });
    };
    CopyFromReleasesPackagesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CopyFromReleasesPackagesComponent',
            templateUrl: './CopyFromReleasesPackagesComponent.html',
            providers: [WarehouseReleaseListExtendedService_1.WarehouseReleaseListExtendedService, WarehouseReleasePackagePMExtendedService_1.WarehouseReleasePackagePMExtendedService],
        }),
        __metadata("design:paramtypes", [WarehouseReleaseListExtendedService_1.WarehouseReleaseListExtendedService, WarehouseReleasePackagePMExtendedService_1.WarehouseReleasePackagePMExtendedService])
    ], CopyFromReleasesPackagesComponent);
    return CopyFromReleasesPackagesComponent;
}());
exports.CopyFromReleasesPackagesComponent = CopyFromReleasesPackagesComponent;
var WarehouseReleaseClass = /** @class */ (function () {
    function WarehouseReleaseClass(warehouseReleaseLists) {
        this.IsLoad = false;
        this.ReleaseNumberValue = "";
        this.ReleaseNumberLabel = "";
        this.Id = "";
        this.WarehouseReleasePackage = [];
        this.StatusName = "";
        this.ReleaseNumberLabel = "Warehouse Release # ";
        this.ReleaseNumberValue = warehouseReleaseLists.ReleaseNumber;
        this.Id = warehouseReleaseLists.Id;
        this.ExpectedReleaseDate = warehouseReleaseLists.ExpectedReleaseDate;
        this.ActualReleaseDate = warehouseReleaseLists.ActualReleaseDate;
        this.ReleaseBy = warehouseReleaseLists.ReleaseBy;
        this.StatusName = warehouseReleaseLists.StatusName;
        this.ComputeReleaseDate(this);
    }
    WarehouseReleaseClass.prototype.ComputeReleaseDate = function (item) {
        if (item.ActualReleaseDate != null) {
            item.ReleaseDateBackgroudColor = Tools_1.FontTool.Green;
            item.WarehouseReleaseDateType = " (actual)";
            item.ReleaseDate = item.ActualReleaseDate;
        }
        else if (item.ExpectedReleaseDate != null) {
            item.ReleaseDateBackgroudColor = Tools_1.FontTool.Red;
            this.WarehouseReleaseDateType = " (expected)";
            item.ReleaseDate = item.ExpectedReleaseDate;
        }
    };
    return WarehouseReleaseClass;
}());
exports.WarehouseReleaseClass = WarehouseReleaseClass;
//# sourceMappingURL=CopyFromReleasesPackagesComponent.js.map