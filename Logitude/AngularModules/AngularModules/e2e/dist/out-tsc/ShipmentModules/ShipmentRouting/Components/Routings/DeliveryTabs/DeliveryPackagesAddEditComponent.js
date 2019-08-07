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
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeliveryPackagesAddEditComponent = /** @class */ (function (_super) {
    __extends(DeliveryPackagesAddEditComponent, _super);
    function DeliveryPackagesAddEditComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "ShipmentPickUpDeliveryPackage";
        _this.IsNewEntity = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    DeliveryPackagesAddEditComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNewEntity = dataContext.IsNewEntity;
        this.SetLabels();
        this.Clone();
    };
    DeliveryPackagesAddEditComponent.prototype.SetLabels = function () {
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('ShipmentPackage.F.Dimensions').replace('%UnitCode', this.DataContext.fatherComponent.ShipmentPM.DimensionsUnitCode);
    };
    DeliveryPackagesAddEditComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    DeliveryPackagesAddEditComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.IsNewEntity) {
                this.DataContext.fatherComponent.EntityPM.AddPackage(this.EntityPM);
                this.DataContext.fatherComponent.BuildItemsSource();
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    DeliveryPackagesAddEditComponent.prototype.MultiHarmonizeClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { PackagePM: this.EntityPM, IsEditingEnabled: this.DataContext.IsEditingEnabled };
        logWindow.Title = "Multi-Harmonize";
        logWindow.Show("./ShipmentModules/ShipmentRouting/Components/Routings/AddEditPackageHarmonizeComponent");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.DataContext.SetUIProperties_Harmonize();
            }
        });
    };
    DeliveryPackagesAddEditComponent.prototype.ChooseHarmonizeClicked = function () {
        if (this.DataContext) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("HarmonizeCode") + " Search";
            logitudeWindow.WindowArgs = { Entity: this.DataContext, FieldName: 'Harmonize' };
            logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Harmonizes/HarmonizesComponent");
            logitudeWindow.WindowClosed.subscribe(function (s) {
            });
        }
    };
    DeliveryPackagesAddEditComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('PackageTypeId');
        this.myCloner.AddField('ContainerNumber');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('Weight');
        this.myCloner.AddField('Seal');
        this.myCloner.AddField('Harmonize');
        this.myCloner.AddField('Length');
        this.myCloner.AddField('Width');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Make');
        this.myCloner.AddField('Model');
        this.myCloner.AddField('Year');
        this.myCloner.AddField('Color');
        this.myCloner.AddField('ChassisNumber');
        this.myCloner.AddField('RegistrationNumber');
        this.myCloner.AddField('CountryId');
        this.myCloner.AddEntity(this.EntityPM);
        if (this.DataContext.fatherComponent) {
            this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
            this.myCloner.AddEntity(this.DataContext.fatherComponent.ShipmentPM);
        }
    };
    DeliveryPackagesAddEditComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    DeliveryPackagesAddEditComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeliveryPackagesAddEditComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeliveryPackagesAddEditComponent);
    return DeliveryPackagesAddEditComponent;
}(BaseComponent_1.BaseComponent));
exports.DeliveryPackagesAddEditComponent = DeliveryPackagesAddEditComponent;
//# sourceMappingURL=DeliveryPackagesAddEditComponent.js.map