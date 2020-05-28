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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TimeSheetWorkspaceComponent = /** @class */ (function () {
    function TimeSheetWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.PageChild_Daily = null;
        this.PageChild_Weekly = null;
        this.PageChild_Monthly = null;
        this.PageChild_ClockTime = null;
        this.PageChild_Vacations = null;
    }
    TimeSheetWorkspaceComponent.prototype.InitComponent = function () {
        this.RunComponent();
    };
    TimeSheetWorkspaceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    TimeSheetWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    TimeSheetWorkspaceComponent.prototype.SetSelectedItem = function () {
        this.SelectedTabCode = "Daily";
    };
    Object.defineProperty(TimeSheetWorkspaceComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    TimeSheetWorkspaceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedTabCode != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedTabCode) {
                        case "Daily": {
                            if (this.PageChild_Daily == null) {
                                this._entityResourceService.getEntityResourceByTableName("TMEmployeeTime", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(function (response) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/DailyTimeSheetComponent', myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.PageChild_Daily = cmpRef.instance;
                                            _this.PageChild_Daily.InitTab(_this);
                                        });
                                    });
                                });
                            }
                            else {
                                this.PageChild_Daily.RefreshTab();
                            }
                            break;
                        }
                        case "Weekly": {
                            if (this.PageChild_Weekly == null) {
                                this._entityResourceService.getEntityResourceByTableName("TMEmployeeTime", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(function (response) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/WeeklyTimeSheetComponent', myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.PageChild_Weekly = cmpRef.instance;
                                            _this.PageChild_Weekly.InitTab(_this);
                                        });
                                    });
                                });
                            }
                            else {
                                this.PageChild_Weekly.RefreshTab();
                            }
                            break;
                        }
                        case 'Monthly': {
                            if (this.PageChild_Monthly == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/MonthlyTimeSheetComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_Monthly = cmpRef.instance;
                                    _this.PageChild_Monthly.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_Monthly.RefreshTab();
                            }
                            break;
                        }
                        case 'ClockTime': {
                            if (this.PageChild_ClockTime == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/ClockTimeComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_ClockTime = cmpRef.instance;
                                    _this.PageChild_ClockTime.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_ClockTime.RefreshTab();
                            }
                            break;
                        }
                        case "Vacations": {
                            if (this.PageChild_Vacations == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/VacationsComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_Vacations = cmpRef.instance;
                                    _this.PageChild_Vacations.InitTab();
                                });
                            }
                            else {
                                this.PageChild_Vacations.LoadAllScreenData();
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], TimeSheetWorkspaceComponent.prototype, "AllLocations", void 0);
    TimeSheetWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'TimeSheetWorkspaceComponent',
            moduleId: module.id,
            templateUrl: './TimeSheetWorkspaceComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TimeSheetWorkspaceComponent);
    return TimeSheetWorkspaceComponent;
}());
exports.TimeSheetWorkspaceComponent = TimeSheetWorkspaceComponent;
//# sourceMappingURL=TimeSheetWorkspaceComponent.js.map