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
var TimeManagementDomainService_1 = require("../../Services/TimeManagementDomainService");
var NewMoveHBProjectsComponent = /** @class */ (function (_super) {
    __extends(NewMoveHBProjectsComponent, _super);
    function NewMoveHBProjectsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //  private batchEntity: BatchTaskExecutionPM;
        _this.IsResponseProgressVisible = false;
        // Timer
        _this.timerSeconds = 10;
        _this.Retries = 0;
        _this.SetUIProperties();
        _this.myService = new TimeManagementDomainService_1.TimeManagementDomainService();
        return _this;
    }
    NewMoveHBProjectsComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("FromProject", null, this.FromProject == null);
        this.UIProperties.SetRequired("ToProject", null, this.ToProject == null);
    };
    Object.defineProperty(NewMoveHBProjectsComponent.prototype, "UserId", {
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
    Object.defineProperty(NewMoveHBProjectsComponent.prototype, "FromProject", {
        get: function () {
            return this.fromProject;
        },
        set: function (value) {
            if (this.fromProject != value) {
                this.fromProject = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMoveHBProjectsComponent.prototype, "ToProject", {
        get: function () {
            return this.toProject;
        },
        set: function (value) {
            if (this.toProject != value) {
                this.toProject = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewMoveHBProjectsComponent.prototype, "FromDate", {
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
    Object.defineProperty(NewMoveHBProjectsComponent.prototype, "ToDate", {
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
    NewMoveHBProjectsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewMoveHBProjectsComponent.prototype.MoveHoursClicked = function () {
        var _this = this;
        var errors = [];
        if (this.FromProject == null) {
            errors.push("From Project field is required");
        }
        if (this.ToProject == null) {
            errors.push("To Project field is required");
        }
        if (this.FromProject == this.ToProject) {
            errors.push("Can't Move Hours: From Project and To Project Are The Same");
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
                    //  this.batchEntity = myResponse.Result;
                    //if (this.batchEntity != null) {
                    //    this.IsResponseProgressVisible = true;
                    //    this.timer = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
                    //}
                }
            });
        }
    };
    NewMoveHBProjectsComponent.prototype.IncreaseTimer = function () {
        var _this = this;
        clearTimeout(this.timer);
        this.timer = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
    };
    NewMoveHBProjectsComponent.prototype.AdjustTimerSpeed = function () {
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
    NewMoveHBProjectsComponent.prototype.RunTimerFunction = function () {
        this.Retries++;
        //this.MoveHours();
    };
    NewMoveHBProjectsComponent.prototype.StopTimer = function () {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.IsResponseProgressVisible = false;
    };
    NewMoveHBProjectsComponent.prototype.ngOnDestroy = function () {
        this.StopTimer();
    };
    //private bteList: BatchTaskExecutionList;
    //MoveHours() {
    //    var batchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    //    batchTaskExecutionListService.getSingle(this.batchEntity.Id).subscribe((myResponse: ServiceResponse) => {
    //        if (myResponse.HasError) {
    //            this.StopTimer();
    //            this.ValidationErrorsList = myResponse.ErrorsArray;
    //        }
    //        else {
    //            this.bteList = myResponse.Result;
    //            if (this.bteList.StatusCode == "D") // D- Done
    //            {
    //                this.StopTimer();
    //                var window: MessageWindow = new MessageWindow();
    //                window.Show("Moving Hours Between Projects Completed Succesfully");
    //            }
    //            else if (this.bteList.StatusCode == "F") // F- Failed
    //            {
    //                this.StopTimer();
    //                var window: MessageWindow = new MessageWindow();
    //                window.Show("Faild: " + this.bteList.ErrorLog);
    //            }
    //            this.AdjustTimerSpeed();
    //        }
    //    });
    //}
    NewMoveHBProjectsComponent.prototype.CloseResponseProgressClicked = function () {
        this.StopTimer();
    };
    NewMoveHBProjectsComponent = __decorate([
        core_1.Component({
            selector: 'NewMoveHBProjectsComponent',
            moduleId: module.id,
            templateUrl: './NewMoveHBProjectsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewMoveHBProjectsComponent);
    return NewMoveHBProjectsComponent;
}(BaseComponent_1.BaseComponent));
exports.NewMoveHBProjectsComponent = NewMoveHBProjectsComponent;
//# sourceMappingURL=NewMoveHBProjectsComponent.js.map