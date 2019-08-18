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
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AddEditAddOnComponent = /** @class */ (function () {
    function AddEditAddOnComponent() {
        this.ObjectTableName = "TenantAddOn";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditAddOnComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNewEntity = dataContext.IsNew;
        this.Clone();
    };
    AddEditAddOnComponent.prototype.CancelButtonClicked = function () {
        this.DataContext.ResetOldData();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAddOnComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.IsNewEntity) {
            if (this.DataContext.fatherComponent.EntityPM.AddOns.filter(function (d) { return d.PackageCode == _this.DataContext.PackageCode; })[0]) {
                errors.push("Same package already exists");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.IsNewEntity) {
                this.IsNewEntity = false;
                this.DataContext.fatherComponent.EntityPM.AddTenantAddOnPM(this.DataContext.EntityPM);
                if (this.DataContext.fatherComponent.AddOnsList.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.AddOnsList.push(this.DataContext);
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditAddOnComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PackageCode');
        this.myCloner.AddField('NumberOfUsers');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.TenantManagementPM);
    };
    AddEditAddOnComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditAddOnComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAddOnComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAddOnComponent);
    return AddEditAddOnComponent;
}());
exports.AddEditAddOnComponent = AddEditAddOnComponent;
//# sourceMappingURL=AddEditAddOnComponent.js.map