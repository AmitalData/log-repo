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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var AddEditPriceStepComponent = /** @class */ (function (_super) {
    __extends(AddEditPriceStepComponent, _super);
    function AddEditPriceStepComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "QuotePriceSteps";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditPriceStepComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.QuotePM = dataContext.fatherComponent.QuotePM;
        this.Clone();
    };
    AddEditPriceStepComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPriceStepComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.DataContext.fatherComponent.EntityPM.QuoteChargePriceSteps.filter(function (d) { return d.Step == _this.DataContext.Step; }).forEach(function (item) {
            if (item != _this.EntityPM) {
                errors.push("Price Steps list already contains Step: " + Tools_1.AppTool.Round(_this.DataContext.Step, 2));
            }
        });
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.DataContext.IsNew) {
                this.DataContext.fatherComponent.EntityPM.AddQuotePriceStepsPM(this.EntityPM);
            }
            this.DataContext.fatherComponent.BuildStepItemsSource();
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditPriceStepComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Step');
        this.myCloner.AddField('CostUnitPrice');
        this.myCloner.AddField('MarkupValue');
        this.myCloner.AddField('SaleUnitPrice');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuoteChargePM);
        this.myCloner.AddEntity(this.QuotePM);
    };
    AddEditPriceStepComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditPriceStepComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPriceStepComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPriceStepComponent);
    return AddEditPriceStepComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPriceStepComponent = AddEditPriceStepComponent;
//# sourceMappingURL=AddEditPriceStepComponent.js.map