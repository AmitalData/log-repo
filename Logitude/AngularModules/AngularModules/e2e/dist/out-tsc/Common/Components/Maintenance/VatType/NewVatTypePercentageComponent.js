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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var NewVatTypePercentageComponent = /** @class */ (function (_super) {
    __extends(NewVatTypePercentageComponent, _super);
    function NewVatTypePercentageComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "VatTypePercentage";
        _this.ValidationErrorsList = [];
        _this.IsNewEntity = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    NewVatTypePercentageComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.VatTypePM = args['VatTypePM'];
        this.IsNewEntity = args['IsNewEntity'];
        this.Clone();
    };
    Object.defineProperty(NewVatTypePercentageComponent.prototype, "Percentage", {
        get: function () { return this.EntityPM.Percentage; },
        set: function (value) {
            if (this.EntityPM.Percentage != value) {
                this.EntityPM.Percentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVatTypePercentageComponent.prototype, "FromDate", {
        get: function () { return this.EntityPM.FromDate; },
        set: function (value) {
            if (this.EntityPM.FromDate != value) {
                this.EntityPM.FromDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewVatTypePercentageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    NewVatTypePercentageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNewEntity) {
                this.VatTypePM.AddVatTypePercentagePM(this.EntityPM);
            }
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    NewVatTypePercentageComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Percentage');
        this.myCloner.AddField('FromDate');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.VatTypePM);
    };
    NewVatTypePercentageComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    NewVatTypePercentageComponent = __decorate([
        core_1.Component({
            selector: 'NewChargesTypeComponent',
            moduleId: module.id,
            templateUrl: './NewVatTypePercentageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewVatTypePercentageComponent);
    return NewVatTypePercentageComponent;
}(BaseComponent_1.BaseComponent));
exports.NewVatTypePercentageComponent = NewVatTypePercentageComponent;
//# sourceMappingURL=NewVatTypePercentageComponent.js.map