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
var Tools_1 = require("../../../../Infrastructure/Tools");
var AddEditPackageComponent = /** @class */ (function () {
    function AddEditPackageComponent() {
        this.ObjectTableName = "QuotePackage";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditPackageComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.SetLabels();
        this.Clone();
    };
    AddEditPackageComponent.prototype.SetLabels = function () {
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('QuotePackage.F.Dimensions').replace('%UnitCode', this.DataContext.QuotePM.DimensionsUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('QuotePackage.F.Volume').replace('%VolumeCode', this.DataContext.QuotePM.VolumeUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('QuotePackage.F.VolumetricWeight').replace('%WeightCode', this.DataContext.QuotePM.ChargeableWeightUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('QuotePackage.F.GrossWeight').replace('%WeightCode', this.DataContext.QuotePM.GrossWeightUnitCode);
    };
    AddEditPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.DataContext.QuotePM.TransportModeId != "A") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.PackageTypeId)) {
                errors.push("Package Type Field is Required");
            }
            if (this.DataContext.GrossWeight == null) {
                errors.push("Gross Weight Field is Required");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                this.DataContext.QuotePM.AddQuotePackagePM(this.EntityPM);
                this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                this.DataContext.fatherComponent.BuildItemsSource();
            }
            this.DataContext.IsNewEntity = false;
            this.DataContext.fatherComponent.ResetTotalEditedValues();
            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuotePM);
    };
    AddEditPackageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPackageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPackageComponent);
    return AddEditPackageComponent;
}());
exports.AddEditPackageComponent = AddEditPackageComponent;
//# sourceMappingURL=AddEditPackageComponent.js.map