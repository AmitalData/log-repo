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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TMOfficeHourPM_1 = require("../../EntityPMs/TMOfficeHourPM");
var TMOfficeHourPMService_1 = require("../../Services/StandardPMs/TMOfficeHourPMService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var TimeManagementDomainService_1 = require("../../Services/TimeManagementDomainService");
var NewOfficeHourComponent = /** @class */ (function (_super) {
    __extends(NewOfficeHourComponent, _super);
    function NewOfficeHourComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TMOfficeHour";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TMOfficeHourPMService = new TMOfficeHourPMService_1.TMOfficeHourPMService();
        _this.EntityPM = new TMOfficeHourPM_1.TMOfficeHourPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.CreateDate = todayDate;
        _this.EntityPM.UpdateDate = todayDate;
        _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        return _this;
    }
    NewOfficeHourComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.EntityPM.UserId = args.EmployeeUserId;
        }
    };
    Object.defineProperty(NewOfficeHourComponent.prototype, "WorkDate", {
        get: function () { return this.EntityPM.WorkDate; },
        set: function (value) {
            if (this.EntityPM.WorkDate != value) {
                this.EntityPM.WorkDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOfficeHourComponent.prototype, "EntryTime", {
        get: function () {
            return this.EntityPM.EntryTime;
        },
        set: function (value) {
            this.EntityPM.EntryTime = value;
        },
        enumerable: true,
        configurable: true
    });
    NewOfficeHourComponent.prototype.SetEntryDateTime = function (value) {
        if (value) {
            var hours = value.split(':')[0];
            var minutes = value.split(':')[1];
            if (this.EntityPM.WorkDate != null) {
                this.EntityPM.EntryTime = Tools_1.DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;
                this.EntityPM.EntryTime.setUTCHours(+hours);
                this.EntityPM.EntryTime.setUTCMinutes(+minutes);
            }
            else {
                this.EntityPM.EntryTime = Tools_1.DateTool.GetCurrentDateAsUtc();
                this.EntityPM.EntryTime.setUTCHours(+hours);
                this.EntityPM.EntryTime.setUTCMinutes(+minutes);
            }
        }
    };
    Object.defineProperty(NewOfficeHourComponent.prototype, "ExitTime", {
        get: function () {
            return this.EntityPM.ExitTime;
        },
        set: function (value) {
            this.EntityPM.ExitTime = value;
        },
        enumerable: true,
        configurable: true
    });
    NewOfficeHourComponent.prototype.SetExistDateTime = function (value) {
        if (value) {
            var hours = value.split(':')[0];
            var minutes = value.split(':')[1];
            if (this.EntityPM.WorkDate != null) {
                this.EntityPM.ExitTime = Tools_1.DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;
                this.EntityPM.ExitTime.setUTCHours(+hours);
                this.EntityPM.ExitTime.setUTCMinutes(+minutes);
            }
            else {
                this.EntityPM.ExitTime = Tools_1.DateTool.GetCurrentDateAsUtc();
                this.EntityPM.ExitTime.setUTCHours(+hours);
                this.EntityPM.ExitTime.setUTCMinutes(+minutes);
            }
        }
    };
    Object.defineProperty(NewOfficeHourComponent.prototype, "Description", {
        get: function () {
            return this.EntityPM.Description;
        },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewOfficeHourComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewOfficeHourComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (this.EntityPM.WorkDate == null)
            this.ValidationErrorsList.push("Work Date Field is required");
        if (this.EntityPM.ExitTime != null && this.EntityPM.EntryTime != null) {
            if (this.EntityPM.ExitTime <= this.EntityPM.EntryTime) {
                this.ValidationErrorsList.push("Exist time Field must be greater than Entry time Field");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.ExitTime != null) {
                this.SetExistDateTime(this.EntityPM.ExitTime.getUTCHours() + ":" + this.EntityPM.ExitTime.getUTCMinutes());
            }
            if (this.EntityPM.EntryTime != null) {
                this.SetEntryDateTime(this.EntityPM.EntryTime.getUTCHours() + ":" + this.EntityPM.EntryTime.getUTCMinutes());
            }
            this.InsertTMOfficeHour();
        }
    };
    NewOfficeHourComponent.prototype.InsertTMOfficeHour = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.CurrentSession.StartBusyIndicatorSaving();
        var myServiceHelper = new TimeManagementDomainService_1.TimeManagementAPIHelper();
        myServiceHelper.Id = SessionLocator_1.SessionLocator.Tenant;
        if (this.TMOfficeHourPMService == null) {
            this.TMOfficeHourPMService = new TMOfficeHourPMService_1.TMOfficeHourPMService();
        }
        this.TMOfficeHourPMService.insert(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewOfficeHourComponent = __decorate([
        core_1.Component({
            selector: 'NewOfficeHourComponent',
            moduleId: module.id,
            templateUrl: './NewOfficeHourComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewOfficeHourComponent);
    return NewOfficeHourComponent;
}(BaseComponent_1.BaseComponent));
exports.NewOfficeHourComponent = NewOfficeHourComponent;
//# sourceMappingURL=NewOfficeHourComponent.js.map