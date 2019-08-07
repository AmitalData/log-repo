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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var AddEditOCIComponent = /** @class */ (function (_super) {
    __extends(AddEditOCIComponent, _super);
    function AddEditOCIComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditOCIComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.DataContext.SetUIProperties();
        this.ObjectTableName = dataContext.ObjectTableName;
        this.Clone();
    };
    AddEditOCIComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditOCIComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.CountryId) && Tools_1.AppTool.IsNullOrEmpty(this.DataContext.AWBCustomsInformationCode) && Tools_1.AppTool.IsNullOrEmpty(this.DataContext.AWBInformationCode)) {
            errors.push("You must fill one of the fields (Country or Information or CustomsInformation)");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity || this.EntityPM.IsAWBWizardDefault) {
                this.DataContext.IsNewEntity = false;
                this.EntityPM.IsAWBWizardDefault = false;
                if (this.DataContext.ShipmentPM.AWBOCIPMs.indexOf(this.EntityPM) == -1) {
                    this.DataContext.ShipmentPM.AddOCI(this.EntityPM);
                    this.DataContext.fatherComponent.BuildData();
                }
            }
            this.DataContext.IsWindowMode = false;
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditOCIComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('CountryId');
        this.myCloner.AddField('AWBInformationCode');
        this.myCloner.AddField('AWBCustomsInformationCode');
        this.myCloner.AddField('SupplementaryCustomsInfo');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    AddEditOCIComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditOCIComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditOCIComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditOCIComponent);
    return AddEditOCIComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditOCIComponent = AddEditOCIComponent;
//# sourceMappingURL=AddEditOCIComponent.js.map