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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var WizardAddEditDimensionsComponent = /** @class */ (function () {
    function WizardAddEditDimensionsComponent() {
        this.ObjectTableName = "ShipmentOrderPackage";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    WizardAddEditDimensionsComponent.prototype.SetWindowArgs = function (item) {
        this.DataContext = item;
        this.EntityPM = item.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.DimensionsLabel = item.fatherComponent.DimensionsColumnHeader;
        this.VolumeLabel = item.fatherComponent.VolumeColumnHeader;
        this.VolumetricWeightLabel = item.fatherComponent.VolumetricWeightColumnHeader;
        this.GrossWeightLabel = item.fatherComponent.WeightColumnHeader;
    };
    WizardAddEditDimensionsComponent.prototype.CancelButtonClicked = function () {
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    WizardAddEditDimensionsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.DataContext.IsPackageTypeVisible) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageTypeId)) {
                errors.push("Package Type is required");
            }
            if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight)) {
                errors.push("Gross Weight is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.DataContext.fatherComponent.EntityPM.AddOrderPackage(this.DataContext.EntityPM);
            if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
            }
            this.DataContext.IsWindowMode = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    WizardAddEditDimensionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WizardAddEditDimensionsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WizardAddEditDimensionsComponent);
    return WizardAddEditDimensionsComponent;
}());
exports.WizardAddEditDimensionsComponent = WizardAddEditDimensionsComponent;
//# sourceMappingURL=WizardAddEditDimensionsComponent.js.map