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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditAirPackageComponent = /** @class */ (function () {
    function AddEditAirPackageComponent() {
        this.ObjectTableName = "ShipmentPackage";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TabKeypressed = false;
        if (this.CurrentSession == null) {
            this.OkBtnId = "OkBtn_-1_-1";
        }
        else {
            this.OkBtnId = "OkBtn_" + this.CurrentSession.GetNewId("OkBtn");
        }
    }
    AddEditAirPackageComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.SetLabels();
        this.Clone();
    };
    AddEditAirPackageComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    };
    AddEditAirPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAirPackageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //this.EntityPM.ShipmentPackageItems.forEach(packageItem => {
        //    Validator.TryValidateObject(packageItem, this.ObjectTableName, errors);
        //});
        this.DataContext.PackageItemsList.Collection.forEach(function (item) {
            if (item != null) {
                Validator_1.Validator.TryValidateObject(item, "ShipmentPackageItem", errors);
            }
        });
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
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
                this.DataContext.IsNewEntity = false;
                this.DataContext.ShipmentPM.AddPackage(this.EntityPM);
                this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                //if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                //    this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                //}
                //this.DataContext.fatherComponent.SetGenerateData();
                //this.DataContext.fatherComponent.ComputeTotals();
            }
            this.DataContext.fatherComponent.SetGenerateData();
            this.DataContext.fatherComponent.ResetTotalEditedValues();
            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditAirPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('CommodityNumber');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Reference3');
        this.myCloner.AddField('Reference4');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    AddEditAirPackageComponent.prototype.RejectChanges = function () {
        this.DataContext.ResetPackageItems();
        this.myCloner.RejectChanges();
    };
    AddEditAirPackageComponent.prototype.onWeightLostFocus = function ($event) {
        if (this.TabKeypressed == true) {
            var OkBtnElement = document.getElementById(this.OkBtnId);
            if (OkBtnElement) {
                OkBtnElement.focus();
            }
            this.TabKeypressed = false;
        }
    };
    AddEditAirPackageComponent.prototype.onWeightkeyDown = function ($event) {
        if ($event.keyCode == 9) {
            this.TabKeypressed = true;
        }
    };
    AddEditAirPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAirPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAirPackageComponent);
    return AddEditAirPackageComponent;
}());
exports.AddEditAirPackageComponent = AddEditAirPackageComponent;
//# sourceMappingURL=AddEditAirPackageComponent.js.map