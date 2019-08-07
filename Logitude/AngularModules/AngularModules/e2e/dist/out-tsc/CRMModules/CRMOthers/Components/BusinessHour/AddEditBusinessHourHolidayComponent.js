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
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var AddEditBusinessHourHolidayComponent = /** @class */ (function (_super) {
    __extends(AddEditBusinessHourHolidayComponent, _super);
    function AddEditBusinessHourHolidayComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "BusinessHoursHoliday";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditBusinessHourHolidayComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.entityPM;
        this.Clone();
    };
    // Commands
    AddEditBusinessHourHolidayComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditBusinessHourHolidayComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.DataContext.CreateDatePicker == null) {
            if (this.DataContext.IsRecurring) {
                if (this.DataContext.Day == 0 && this.DataContext.Month == 0) {
                    errors.push("Holiday Date field is required");
                }
            }
            else {
                errors.push("Holiday Date field is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.isNew) {
                this.DataContext.isNew = false;
                if (this.DataContext.trigger.entityPM.BusinessHoursHolidays.indexOf(this.EntityPM) == -1) {
                    this.DataContext.trigger.entityPM.AddBusinessHoursHolidayPM(this.EntityPM);
                }
                if (this.DataContext.trigger.HolidaysDataList.indexOf(this.DataContext) == -1) {
                    this.DataContext.trigger.HolidaysDataList.push(this.DataContext);
                }
            }
            this.DataContext.trigger.fillHolidays();
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditBusinessHourHolidayComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('HolidayName');
        this.myCloner.AddField('IsRecurring');
        this.myCloner.AddField('CreateDatePicker');
        this.myCloner.AddField('Day');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Month');
        this.myCloner.AddField('Inactive');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.entityPM);
    };
    AddEditBusinessHourHolidayComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditBusinessHourHolidayComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditBusinessHourHolidayComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditBusinessHourHolidayComponent);
    return AddEditBusinessHourHolidayComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditBusinessHourHolidayComponent = AddEditBusinessHourHolidayComponent;
//# sourceMappingURL=AddEditBusinessHourHolidayComponent.js.map