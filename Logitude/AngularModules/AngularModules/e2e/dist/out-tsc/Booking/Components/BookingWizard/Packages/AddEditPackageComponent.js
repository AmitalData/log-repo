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
var core_1 = require("@angular/core");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditPackageComponent = /** @class */ (function (_super) {
    __extends(AddEditPackageComponent, _super);
    function AddEditPackageComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditPackageComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.SetLabels();
        this.Clone();
    };
    AddEditPackageComponent.prototype.SetLabels = function () {
        this.TareLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('BookingPackage.S.Packages.Tare').replace('%WeightCode', this.DataContext.BookingPM.GrossWeightUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('BookingPackage.S.Packages.Volume').replace('%VolumeCode', this.DataContext.BookingPM.VolumeUnitCode);
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('BookingPackage.S.Packages.Dimensions').replace('%UnitCode', this.DataContext.BookingPM.DimensionsUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('BookingPackage.S.Packages.Weight').replace('%WeightCode', this.DataContext.BookingPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('BookingPackage.S.Packages.VolumetricWeight').replace('%WeightCode', this.DataContext.BookingPM.ChargeableWeightUnitCode);
    };
    AddEditPackageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPackageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                this.DataContext.IsNewEntity = false;
                if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                }
                if (this.DataContext.BookingPM.BookingPackages.indexOf(this.EntityPM) == -1) {
                    this.DataContext.BookingPM.AddBookingPackage(this.EntityPM);
                }
            }
            this.DataContext.fatherComponent.BuildData();
            this.DataContext.fatherComponent.ComputeTotals();
            //this.DataContext.SetUIProperties();
            this.CurrentSession.CloseCurrentWindow();
            this.DataContext.IsWindowMode = false;
        }
    };
    AddEditPackageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('VolumetricWeight');
        this.myCloner.AddField('Weight');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.BookingPM);
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
}(BaseComponent_1.BaseComponent));
exports.AddEditPackageComponent = AddEditPackageComponent;
//# sourceMappingURL=AddEditPackageComponent.js.map