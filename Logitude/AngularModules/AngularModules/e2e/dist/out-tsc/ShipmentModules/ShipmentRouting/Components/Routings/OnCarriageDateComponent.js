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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var OnCarriageDateComponent = /** @class */ (function (_super) {
    __extends(OnCarriageDateComponent, _super);
    function OnCarriageDateComponent() {
        var _this = _super.call(this) || this;
        _this.IsOpened = false;
        _this.ObjectTableName = null;
        _this.State = "Departure";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.PopupClosed = new core_1.EventEmitter();
        return _this;
    }
    OnCarriageDateComponent.prototype.ngOnInit = function () {
        this.Clone();
    };
    Object.defineProperty(OnCarriageDateComponent.prototype, "ExpectedDate", {
        get: function () {
            if (this.State == "Departure") {
                return this.EntityPM.OnCarriageETD;
            }
            else {
                return this.EntityPM.OnCarriageETA;
            }
        },
        set: function (newValue) {
            if (this.State == "Departure") {
                this.EntityPM.OnCarriageETD = newValue;
            }
            else {
                this.EntityPM.OnCarriageETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OnCarriageDateComponent.prototype, "ActualDate", {
        get: function () {
            if (this.State == "Departure") {
                return this.EntityPM.OnCarriageATD;
            }
            else {
                return this.EntityPM.OnCarriageATA;
            }
        },
        set: function (newValue) {
            if (this.State == "Departure") {
                this.EntityPM.OnCarriageATD = newValue;
            }
            else {
                this.EntityPM.OnCarriageATA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    OnCarriageDateComponent.prototype.SetActualDateClicked = function () {
        this.ActualDate = Tools_1.DateTool.GetDateParts(this.ExpectedDate).DateObject;
    };
    OnCarriageDateComponent.prototype.OnSelectedDate_ActualChanged = function (date) {
        this.ActualDate = date.SelectedDate;
    };
    OnCarriageDateComponent.prototype.OnSelectedDate_ExpectedChanged = function (date) {
        this.ExpectedDate = date.SelectedDate;
    };
    OnCarriageDateComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (!Tools_1.DateTool.IsActualDateValid(this.ActualDate)) {
            this.ValidationErrorsList.push("Can't set actual date to future date");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.Clone();
            this.IsOpened = false;
            this.PopupClosed.emit(this);
        }
    };
    OnCarriageDateComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.ValidationErrorsList = [];
        this.IsOpened = false;
    };
    OnCarriageDateComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this);
        this.myCloner.AddField('ExpectedDate');
        this.myCloner.AddField('ActualDate');
        this.myCloner.AddEntity(this.EntityPM);
    };
    OnCarriageDateComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], OnCarriageDateComponent.prototype, "PopupClosed", void 0);
    OnCarriageDateComponent = __decorate([
        core_1.Component({
            selector: 'OnCarriageDate',
            moduleId: module.id,
            templateUrl: './OnCarriageDateComponent.html',
            inputs: ['EntityPM', 'State'],
        }),
        __metadata("design:paramtypes", [])
    ], OnCarriageDateComponent);
    return OnCarriageDateComponent;
}(BaseComponent_1.BaseComponent));
exports.OnCarriageDateComponent = OnCarriageDateComponent;
//# sourceMappingURL=OnCarriageDateComponent.js.map