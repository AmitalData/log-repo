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
var AddEditInsidePackageComponent = /** @class */ (function () {
    function AddEditInsidePackageComponent() {
        this.ObjectTableName = "InsideShipmentPackage";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditInsidePackageComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.SetLabels();
        this.Clone();
    };
    AddEditInsidePackageComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    };
    AddEditInsidePackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditInsidePackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                this.DataContext.ShipmentPackagePM.AddInsideShipmentPackagePM(this.EntityPM);
                this.DataContext.fatherComponent.BuildInsideItemsSource();
                this.DataContext.fatherComponent.ComputeFromInsidePackages();
                if (this.DataContext.fatherComponent.Row) {
                    var isExpandaple = false;
                    if (this.DataContext.fatherComponent.InsideItemsSource.length > 0) {
                        isExpandaple = true;
                    }
                    this.DataContext.fatherComponent.Row.SetExpandaple(isExpandaple);
                }
                if (this.DataContext.fatherComponent.InsideItemsSource.length == 0) {
                    this.DataContext.fatherComponent.fatherComponent.BuildItemsSource();
                }
            }
            this.DataContext.IsNewEntity = false;
            this.DataContext.fatherComponent.ComputeFromInsidePackages();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditInsidePackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('PackageTypeId');
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
        this.myCloner.AddEntity(this.DataContext.ShipmentPackagePM);
    };
    AddEditInsidePackageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditInsidePackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditInsidePackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditInsidePackageComponent);
    return AddEditInsidePackageComponent;
}());
exports.AddEditInsidePackageComponent = AddEditInsidePackageComponent;
//# sourceMappingURL=AddEditInsidePackageComponent.js.map