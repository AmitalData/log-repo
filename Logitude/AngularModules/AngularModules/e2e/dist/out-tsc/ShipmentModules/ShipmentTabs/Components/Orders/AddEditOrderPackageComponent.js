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
var AddEditOrderPackageComponent = /** @class */ (function () {
    function AddEditOrderPackageComponent() {
        this.ObjectTableName = "ShipmentOrderPackage";
        this.ValidationErrorsList = [];
        this.TransportModeId = null;
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditOrderPackageComponent.prototype.SetDataContext = function (args) {
        this.DataContext = args;
        this.EntityPM = args.EntityPM;
        this.TransportModeId = this.DataContext.fatherComponent.TransportModeId;
        this.IsLCLEntity = this.DataContext.fatherComponent.IsLCLEntity;
        this.IsFCLEntity = this.DataContext.fatherComponent.IsFCLEntity;
        this.SetLabels();
        this.Clone();
    };
    AddEditOrderPackageComponent.prototype.SetLabels = function () {
        if (this.IsLCLEntity) {
            this.TypeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentOrderPackage.F.PackageTypeId");
        }
        else {
            this.TypeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentOrderPackage.F.ContainerTypeId");
        }
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentOrderPackage.F.Volume').replace('%VolumeCode', this.DataContext.ShipmentPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentOrderPackage.F.Dimensions').replace('%UnitCode', this.DataContext.ShipmentPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentOrderPackage.F.GrossWeight').replace('%WeightCode', this.DataContext.ShipmentPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentOrderPackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.ShipmentPM.ChargeableWeightUnitCode);
    };
    AddEditOrderPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditOrderPackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.TransportModeId != "A") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
                errors.push(this.TypeLabel + " is required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.GrossWeight)) {
                errors.push("Gross Weight is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                this.DataContext.IsNewEntity = false;
                if (this.DataContext.ShipmentPM.ShipmentOrderPackages.indexOf(this.EntityPM) == -1) {
                    this.DataContext.ShipmentPM.AddOrderPackage(this.EntityPM);
                    this.DataContext.fatherComponent.BuildItemsSource();
                    this.DataContext.fatherComponent.ComputeTotals();
                }
            }
            this.DataContext.fatherComponent.ResetTotalEditedValues();
            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditOrderPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('GrossWeight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    AddEditOrderPackageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditOrderPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditOrderPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditOrderPackageComponent);
    return AddEditOrderPackageComponent;
}());
exports.AddEditOrderPackageComponent = AddEditOrderPackageComponent;
//# sourceMappingURL=AddEditOrderPackageComponent.js.map