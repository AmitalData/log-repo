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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var core_1 = require("@angular/core");
var MainSchedulerComponent = /** @class */ (function () {
    function MainSchedulerComponent() {
        this.IsShowTabUpdate = true;
        this.PageChild_STASK = null;
        this.PageChild_SFTP = null;
        this.PageChild_SSFTP = null;
        this.IsShowTaskScheduler = false;
        this.IsShowTabFTBScheduler = false;
        this.IsShowTabSFTBScheduler = false;
        this.IsShowComponentWithTabs = false;
        this.IsShowComponentWithOutTabs = false;
        this.IsShowPackageNotIncludeMessage = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isLoaderReady = false;
        this.Retries = 0;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "TASK"))
            this.IsShowTaskScheduler = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "FTP"))
            this.IsShowTabFTBScheduler = true;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "SFTP"))
            this.IsShowTabSFTBScheduler = true;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "READ"))
            this.IsShowPackageNotIncludeMessage = true;
        if (this.IsShowTaskScheduler && (this.IsShowTabFTBScheduler || this.IsShowTabSFTBScheduler))
            this.IsShowComponentWithTabs = true;
        else if (this.IsShowTaskScheduler || this.IsShowTabFTBScheduler || this.IsShowTabSFTBScheduler)
            this.IsShowComponentWithOutTabs = true;
        else
            this.IsShowPackageNotIncludeMessage = true;
        if ((this.IsShowTaskScheduler || this.IsShowTabFTBScheduler || this.IsShowTabSFTBScheduler) && !this.IsShowPackageNotIncludeMessage) {
            this.RunComponent();
        }
    }
    MainSchedulerComponent.prototype.ngOnInit = function () {
    };
    MainSchedulerComponent.prototype.SetSelectedItem = function (tabCode) {
        this.SelectedTabCode = tabCode;
    };
    MainSchedulerComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                var tabCode = this.IsShowComponentWithTabs || this.IsShowTaskScheduler ? "STASK" : (this.IsShowTabFTBScheduler ? "SFTP" : "SSFTP");
                this.SetSelectedItem(tabCode);
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    MainSchedulerComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    MainSchedulerComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(MainSchedulerComponent.prototype, "SelectedTabCode", {
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
    MainSchedulerComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedTabCode != null) {
                var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                if (myLocation != null) {
                    switch (this.SelectedTabCode) {
                        //Task
                        case "STASK": {
                            if (this.PageChild_STASK == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_STASK = cmpRef.instance;
                                    _this.PageChild_STASK.LoadData("Task");
                                });
                            }
                            break;
                        }
                        //SFTP
                        case "SFTP": {
                            if (this.PageChild_SFTP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_SFTP = cmpRef.instance;
                                    _this.PageChild_SFTP.LoadData("FTP");
                                });
                            }
                            break;
                        }
                        case "SSFTP": {
                            if (this.PageChild_SSFTP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_SSFTP = cmpRef.instance;
                                    _this.PageChild_SSFTP.LoadData("SFTP");
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
    ], MainSchedulerComponent.prototype, "AllLocations", void 0);
    MainSchedulerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MainSchedulerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MainSchedulerComponent);
    return MainSchedulerComponent;
}());
exports.MainSchedulerComponent = MainSchedulerComponent;
//# sourceMappingURL=MainSchedulerComponent.js.map