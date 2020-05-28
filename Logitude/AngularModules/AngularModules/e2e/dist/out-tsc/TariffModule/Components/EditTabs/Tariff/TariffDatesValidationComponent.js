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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var TariffDatesValidationComponent = /** @class */ (function (_super) {
    __extends(TariffDatesValidationComponent, _super);
    function TariffDatesValidationComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tariff";
        _this.ValidationErrorsList = [];
        return _this;
    }
    TariffDatesValidationComponent.prototype.SetWindowArgs = function (arg) {
        this.EntityPM = arg;
        this.Clone();
    };
    Object.defineProperty(TariffDatesValidationComponent.prototype, "StartDate", {
        get: function () {
            return this.EntityPM.StartDate;
        },
        set: function (value) {
            if (this.EntityPM.StartDate != value) {
                this.EntityPM.StartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffDatesValidationComponent.prototype, "ExpirationDate", {
        get: function () {
            return this.EntityPM.ExpirationDate;
        },
        set: function (value) {
            if (this.EntityPM.ExpirationDate != value) {
                this.EntityPM.ExpirationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    TariffDatesValidationComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    TariffDatesValidationComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (this.StartDate == null) {
            this.ValidationErrorsList.push("Satrt date must be less than start date");
        }
        if (this.ExpirationDate == null) {
            this.ValidationErrorsList.push("Expiration date must be less than start date");
        }
        if (this.ExpirationDate != null && Tools_1.DateTool.GetDateParts(this.ExpirationDate).DateTicks < Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
            this.ValidationErrorsList.push("Can't set Expiration date Field to past date");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    TariffDatesValidationComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('ExpirationDate');
        this.myCloner.AddEntity(this.EntityPM);
    };
    TariffDatesValidationComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    TariffDatesValidationComponent = __decorate([
        core_1.Component({
            selector: 'TariffDatesValidationComponent',
            moduleId: module.id,
            templateUrl: './TariffDatesValidationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TariffDatesValidationComponent);
    return TariffDatesValidationComponent;
}(BaseComponent_1.BaseComponent));
exports.TariffDatesValidationComponent = TariffDatesValidationComponent;
//# sourceMappingURL=TariffDatesValidationComponent.js.map