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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var BatchTaskExecutionListService_1 = require("../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService");
var TimeManagementDomainService_1 = require("../../Services/TimeManagementDomainService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var NewGetProjectComponent = /** @class */ (function (_super) {
    __extends(NewGetProjectComponent, _super);
    function NewGetProjectComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsResponseProgressVisible = false;
        // Timer
        _this.timerSeconds = 10;
        _this.Retries = 0;
        _this.SetUIProperties();
        _this.myService = new TimeManagementDomainService_1.TimeManagementDomainService();
        return _this;
    }
    NewGetProjectComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("FromDate", null, this.FromDate == null);
        this.UIProperties.SetRequired("ToDate", null, this.ToDate == null);
    };
    Object.defineProperty(NewGetProjectComponent.prototype, "UserId", {
        get: function () {
            return this.userId;
        },
        set: function (value) {
            if (this.userId != value) {
                this.userId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGetProjectComponent.prototype, "FromDate", {
        get: function () {
            return this.fromDate;
        },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewGetProjectComponent.prototype, "ToDate", {
        get: function () {
            return this.toDate;
        },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewGetProjectComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewGetProjectComponent.prototype.GetProjectsButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (this.FromDate == null) {
            errors.push("From Date field is required");
        }
        if (this.ToDate == null) {
            errors.push("To Date field is required");
        }
        if (this.FromDate > this.ToDate) {
            errors.push("From Date cannot be greater than To Date");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.Retries = 0;
            this.myService.GetTMProjectsByBatchTask(this.UserId, this.FromDate, this.ToDate).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.StopTimer();
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.batchEntity = myResponse.Result;
                    if (_this.batchEntity != null) {
                        _this.IsResponseProgressVisible = true;
                        _this.timer = setInterval(function () { return _this.RunTimerFunction(); }, _this.timerSeconds * 1000);
                    }
                }
            });
        }
    };
    NewGetProjectComponent.prototype.IncreaseTimer = function () {
        var _this = this;
        clearTimeout(this.timer);
        this.timer = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
    };
    NewGetProjectComponent.prototype.AdjustTimerSpeed = function () {
        if (this.Retries <= 60) {
            if (this.timerSeconds != 1) {
                this.timerSeconds = 1;
                this.IncreaseTimer();
            }
        }
        else if (this.Retries <= 120) {
            if (this.timerSeconds != 5) {
                this.timerSeconds = 5;
                this.IncreaseTimer();
            }
        }
        else if (this.Retries <= 180) {
            if (this.timerSeconds != 60) {
                this.timerSeconds = 60;
                this.IncreaseTimer();
            }
        }
        else {
            this.StopTimer();
        }
    };
    NewGetProjectComponent.prototype.RunTimerFunction = function () {
        this.Retries++;
        this.GetBTE();
    };
    NewGetProjectComponent.prototype.StopTimer = function () {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.IsResponseProgressVisible = false;
    };
    NewGetProjectComponent.prototype.ngOnDestroy = function () {
        this.StopTimer();
    };
    NewGetProjectComponent.prototype.GetBTE = function () {
        var _this = this;
        var batchTaskExecutionListService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
        batchTaskExecutionListService.getSingle(this.batchEntity.Id).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.StopTimer();
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.bteList = myResponse.Result;
                if (_this.bteList.StatusCode == "D") // D- Done
                 {
                    _this.StopTimer();
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("Get TM Projects Completed Succesfully");
                }
                else if (_this.bteList.StatusCode == "F") // F- Failed
                 {
                    _this.StopTimer();
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("Faild: " + _this.bteList.ErrorLog);
                }
                _this.AdjustTimerSpeed();
            }
        });
    };
    NewGetProjectComponent.prototype.CloseResponseProgressClicked = function () {
        this.StopTimer();
    };
    NewGetProjectComponent = __decorate([
        core_1.Component({
            selector: 'NewGetProjectComponent',
            moduleId: module.id,
            templateUrl: './NewGetProjectComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewGetProjectComponent);
    return NewGetProjectComponent;
}(BaseComponent_1.BaseComponent));
exports.NewGetProjectComponent = NewGetProjectComponent;
//# sourceMappingURL=NewGetProjectComponent.js.map