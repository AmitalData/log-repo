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
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var AWBAddEditPackageComponent = /** @class */ (function () {
    function AWBAddEditPackageComponent() {
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AWBAddEditPackageComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.SetLabels();
        this.Clone();
    };
    AWBAddEditPackageComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Tare').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Weight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    };
    AWBAddEditPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    AWBAddEditPackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity || this.EntityPM.IsAWBWizardDefault) {
                this.DataContext.ShipmentPM.AddPackage(this.EntityPM);
                if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                }
                this.DataContext.fatherComponent.SetRebuildButton();
                this.DataContext.fatherComponent.SetGenerateButton();
                this.DataContext.fatherComponent.ComputeTotals();
            }
            this.DataContext.IsNewEntity = false;
            this.DataContext.IsWindowMode = false;
            this.EntityPM.IsAWBWizardDefault = false;
            this.DataContext.SetUIProperties();
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AWBAddEditPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    AWBAddEditPackageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AWBAddEditPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AWBAddEditPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBAddEditPackageComponent);
    return AWBAddEditPackageComponent;
}());
exports.AWBAddEditPackageComponent = AWBAddEditPackageComponent;
//# sourceMappingURL=AWBAddEditPackageComponent.js.map