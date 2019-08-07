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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PickUpDeliveryPackageHarmonizePM_1 = require("../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM");
var AddEditOceanPackageComponent = /** @class */ (function () {
    function AddEditOceanPackageComponent() {
        this.ObjectTableName = "ShipmentPackage";
        this.SelectedTabCode = "0";
        this.IsFCLEntity = false;
        this.IsLCLEntity = false;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditOceanPackageComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.FillMethodsList();
        this.IsFCLEntity = dataContext.IsFCLEntity;
        this.IsLCLEntity = dataContext.IsLCLEntity;
        this.SetLabels();
        this.Clone();
    };
    AddEditOceanPackageComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    };
    AddEditOceanPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditOceanPackageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var isValid = this.Validate();
        if (isValid) {
            this.DataContext.PackageItemsList.Collection.forEach(function (item) {
                if (item != null) {
                    if (item.IsNewEntity) {
                        if (_this.DataContext.EntityPM.ShipmentPackageItems.indexOf(item.EntityPM) == -1) {
                            item.IsNewEntity = false;
                            _this.DataContext.EntityPM.AddShipmentPackageItemPM(item.EntityPM);
                        }
                    }
                }
            });
            if (this.DataContext.IsNewEntity) {
                this.DataContext.ShipmentPM.AddPackage(this.EntityPM);
                this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                this.DataContext.fatherComponent.SetGenerateData();
                this.DataContext.fatherComponent.ComputeTotals();
            }
            this.DataContext.IsNewEntity = false;
            this.UpdateDeliveryPackage();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditOceanPackageComponent.prototype.Validate = function () {
        var myResult = true;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.DataContext.PackageItemsList.Collection.forEach(function (item) {
            if (item != null) {
                Validator_1.Validator.TryValidateObject(item, "ShipmentPackageItem", errors);
            }
        });
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
            if (this.DataContext.IsLCLEntity) {
                errors.push("Package Type is required");
            }
            else {
                errors.push("Container Type is required");
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Weight)) {
            errors.push("Gross Weight is required");
        }
        this.ValidationErrorsList = errors;
        if (errors.length > 0) {
            myResult = false;
        }
        return myResult;
    };
    AddEditOceanPackageComponent.prototype.UpdateDeliveryPackage = function () {
        var _this = this;
        if (this.EntityPM.DeliveryId) {
            var Delivery = this.DataContext.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.Id == _this.EntityPM.DeliveryId; })[0];
            if (Delivery) {
                var DeliveryPackagePM = Delivery.ShipmentPickUpDeliveryPackages.filter(function (f) { return f.OriginalShipmentPackageId == _this.EntityPM.Id; })[0];
                if (DeliveryPackagePM) {
                    if (DeliveryPackagePM.ContainerNumber != this.EntityPM.ContainerNumber) {
                        DeliveryPackagePM.ContainerNumber = this.EntityPM.ContainerNumber;
                    }
                    if (DeliveryPackagePM.Description != this.EntityPM.Description) {
                        DeliveryPackagePM.Description = this.EntityPM.Description;
                    }
                    if (DeliveryPackagePM.PackageTypeId != this.EntityPM.PackageTypeId) {
                        DeliveryPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
                    }
                    if (DeliveryPackagePM.PackageTypeName != this.EntityPM.PackageTypeName) {
                        DeliveryPackagePM.PackageTypeName = this.EntityPM.PackageTypeName;
                    }
                    if (DeliveryPackagePM.Quantity != this.EntityPM.Quantity) {
                        DeliveryPackagePM.Quantity = this.EntityPM.Quantity;
                    }
                    if (DeliveryPackagePM.Volume != this.EntityPM.Volume) {
                        DeliveryPackagePM.Volume = this.EntityPM.Volume;
                    }
                    if (DeliveryPackagePM.Weight != this.EntityPM.Weight) {
                        DeliveryPackagePM.Weight = this.EntityPM.Weight;
                    }
                    if (DeliveryPackagePM.ShipperSeal != this.EntityPM.ShipperSeal) {
                        DeliveryPackagePM.ShipperSeal = this.EntityPM.ShipperSeal;
                    }
                    if (DeliveryPackagePM.Width != this.EntityPM.Width) {
                        DeliveryPackagePM.Width = this.EntityPM.Width;
                    }
                    if (DeliveryPackagePM.Height != this.EntityPM.Height) {
                        DeliveryPackagePM.Height = this.EntityPM.Height;
                    }
                    if (DeliveryPackagePM.Length != this.EntityPM.Length) {
                        DeliveryPackagePM.Length = this.EntityPM.Length;
                    }
                    if (DeliveryPackagePM.Harmonize != this.EntityPM.Harmonize) {
                        DeliveryPackagePM.Harmonize = this.EntityPM.Harmonize;
                    }
                    if (DeliveryPackagePM.IsMultiHarmonize != this.EntityPM.IsMultiHarmonize) {
                        DeliveryPackagePM.IsMultiHarmonize = this.EntityPM.IsMultiHarmonize;
                    }
                    if (DeliveryPackagePM.PickUpDeliveryPackageHarmonizes != null && DeliveryPackagePM.PickUpDeliveryPackageHarmonizes.length > 0) {
                        for (var i = DeliveryPackagePM.PickUpDeliveryPackageHarmonizes.length - 1; i >= 0; i--) {
                            var item = DeliveryPackagePM.PickUpDeliveryPackageHarmonizes[i];
                            DeliveryPackagePM.RemovePickUpDeliveryPackageHarmonizePM(item);
                        }
                    }
                    if (this.EntityPM.ShipmentPackageHarmonizes.length > 0) {
                        this.EntityPM.ShipmentPackageHarmonizes.forEach(function (harmonizeItem) {
                            if (harmonizeItem != null) {
                                var harmonize = new PickUpDeliveryPackageHarmonizePM_1.PickUpDeliveryPackageHarmonizePM(null);
                                harmonize.Harmonize = harmonizeItem.Harmonize;
                                harmonize.Tenant = harmonizeItem.Tenant;
                                DeliveryPackagePM.AddPickUpDeliveryPackageHarmonizePM(harmonize);
                            }
                        });
                    }
                }
            }
        }
    };
    AddEditOceanPackageComponent.prototype.MultiHarmonizeClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { PackagePM: this.EntityPM, ShipmentPM: this.DataContext.ShipmentPM, IsEditingEnabled: this.DataContext.IsEditingEnabled };
        logWindow.Title = "Container Multi-Harmonize";
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditPackageHarmonizeComponent");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.DataContext.SetUIProperties_Harmonize();
            }
        });
    };
    AddEditOceanPackageComponent.prototype.ChooseHarmonizeClicked = function () {
        if (this.DataContext) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("HarmonizeCode") + " Search";
            logitudeWindow.WindowArgs = { Entity: this.DataContext, FieldName: 'Harmonize' };
            logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Harmonizes/HarmonizesComponent");
            logitudeWindow.WindowClosed.subscribe(function (s) {
            });
        }
    };
    AddEditOceanPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('ContainerNumber');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('Tare');
        this.myCloner.AddField('ShipperSeal');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('IsDangerous');
        this.myCloner.AddField('ClassNumber');
        this.myCloner.AddField('UnNumber');
        this.myCloner.AddField('PackagingGroup');
        this.myCloner.AddField('IMDGCode');
        this.myCloner.AddField('FlashPoint');
        this.myCloner.AddField('MaterialDescription');
        this.myCloner.AddField('Harmonize');
        this.myCloner.AddField('CarrierSeal');
        this.myCloner.AddField('Temperature');
        this.myCloner.AddField('Ventilation');
        this.myCloner.AddField('MarksAndNumbers');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('SOC');
        this.myCloner.AddField('VGM');
        this.myCloner.AddField('MethodUsed');
        this.myCloner.AddField('CommodityNumber');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Reference3');
        this.myCloner.AddField('Reference4');
        this.myCloner.AddField('Make');
        this.myCloner.AddField('Model');
        this.myCloner.AddField('Year');
        this.myCloner.AddField('Color');
        this.myCloner.AddField('ChassisNumber');
        this.myCloner.AddField('RegistrationNumber');
        this.myCloner.AddField('CountryId');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    AddEditOceanPackageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditOceanPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditOceanPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditOceanPackageComponent);
    return AddEditOceanPackageComponent;
}());
exports.AddEditOceanPackageComponent = AddEditOceanPackageComponent;
//# sourceMappingURL=AddEditOceanPackageComponent.js.map