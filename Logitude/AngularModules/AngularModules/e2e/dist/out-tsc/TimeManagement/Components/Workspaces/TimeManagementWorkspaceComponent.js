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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var TimeManagementWorkspaceComponent = /** @class */ (function () {
    function TimeManagementWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_TIMESHEET = null;
        this.Page_REPORTS = null;
        this.Page_SETTINGS = null;
        this.RunComponent();
    }
    TimeManagementWorkspaceComponent.prototype.RunComponent = function () {
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
    TimeManagementWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    TimeManagementWorkspaceComponent.prototype.SetSelectedItem = function () {
        this.SelectedItem = "TIMESHEET";
    };
    Object.defineProperty(TimeManagementWorkspaceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    TimeManagementWorkspaceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation != null) {
                    switch (this.SelectedItem) {
                        case "TIMESHEET": {
                            if (this.Page_TIMESHEET == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/TimeSheetWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_TIMESHEET = cmpRef.instance;
                                    _this.Page_TIMESHEET.InitComponent();
                                });
                            }
                            break;
                        }
                        case "SETTINGS": {
                            if (this.Page_SETTINGS == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/SettingsWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_SETTINGS = cmpRef.instance;
                                });
                            }
                            break;
                        }
                        case "REPORTS": {
                            if (this.Page_REPORTS == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/ReportsWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_REPORTS = cmpRef.instance;
                                    _this.Page_REPORTS.InitComponent();
                                });
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
    ], TimeManagementWorkspaceComponent.prototype, "AllLocations", void 0);
    TimeManagementWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'TimeManagementWorkspaceComponent',
            moduleId: module.id,
            templateUrl: './TimeManagementWorkspaceComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TimeManagementWorkspaceComponent);
    return TimeManagementWorkspaceComponent;
}());
exports.TimeManagementWorkspaceComponent = TimeManagementWorkspaceComponent;
//# sourceMappingURL=TimeManagementWorkspaceComponent.js.map