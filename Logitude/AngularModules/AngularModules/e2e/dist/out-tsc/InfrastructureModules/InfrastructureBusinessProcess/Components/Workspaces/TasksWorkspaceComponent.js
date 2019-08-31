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
var BusinessProcessDomainService_1 = require("../../../../Infrastructure/Services/BusinessProcessDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TasksWorkspaceComponent = /** @class */ (function () {
    function TasksWorkspaceComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.QueuesItemsSource = [];
        this.NoQueuesVisibility = false;
        this.selectedFilterValue = "";
        this.Retries = 0;
        this.businessProcessDomainService = new BusinessProcessDomainService_1.BusinessProcessDomainService();
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.GetTeamsForLoggedUser();
        this.InitListArgs();
        this.SelectedFilterValue = "My";
        this.RunComponent();
    }
    TasksWorkspaceComponent.prototype.GetTeamsForLoggedUser = function () {
        var _this = this;
        this.businessProcessDomainService.GetTeamsForLoggedUser(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.teamsIdsList = myResponse.Result;
            }
        });
    };
    TasksWorkspaceComponent.prototype.InitListArgs = function () {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.listArgs = new Args_1.ListComponentArgs();
        this.listArgs.QueryCode = "All Activities";
        this.listArgs.ObjectTableName = "Activity";
        this.listArgs.IsTasksMenuClicked = true;
    };
    TasksWorkspaceComponent.prototype.LoadQueues = function (myFilter) {
        var _this = this;
        this.QueuesItemsSource = [];
        this.businessProcessDomainService.GetQueuesWithCounts(myFilter).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    myData.forEach(function (item) {
                        _this.QueuesItemsSource.push(new QueueItem(item, _this));
                    });
                    if (_this.QueuesItemsSource.length == 0) {
                        _this.NoQueuesVisibility = true;
                    }
                    else {
                        _this.selectedQueue = _this.QueuesItemsSource[0];
                        _this.SelectQueue(_this.selectedQueue);
                    }
                }
            }
        });
    };
    Object.defineProperty(TasksWorkspaceComponent.prototype, "SelectedQueue", {
        get: function () { return this.selectedQueue; },
        set: function (value) {
            if (this.selectedQueue != value) {
                this.selectedQueue = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TasksWorkspaceComponent.prototype.SelectQueue = function (entity) {
        var _this = this;
        this.SelectedQueue = entity;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("ActivityTypeCode", "TX", null, null, "Equals", false, false, false, "String");
        if (this.SelectedFilterValue == "My") {
            this.filterAgrs.addAdditionalFilter("OwnerId", SessionLocator_1.SessionLocator.LoggedUserId, null, null, "Equals", false, false, false, "String");
        }
        else if (this.SelectedFilterValue == "Team") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.teamsIdsList)) {
                this.filterAgrs.addAdditionalFilter("TeamId", this.teamsIdsList, null, null, "InList", false, true, false, "string");
            }
            else {
                this.filterAgrs.addAdditionalFilter("TeamId", "XXX", null, null, "InList", false, true, false, "string");
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedQueue.QueueId)) {
            this.filterAgrs.addAdditionalFilter("BusinessProcessQueueId", this.SelectedQueue.QueueId, null, null, "Equals", false, false, false, "String");
        }
        this.listArgs.Filters = this.filterAgrs;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(_this.listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    Object.defineProperty(TasksWorkspaceComponent.prototype, "SelectedFilterValue", {
        get: function () { return this.selectedFilterValue; },
        set: function (value) {
            if (this.selectedFilterValue != value) {
                this.selectedFilterValue = value;
                this.LoadQueues(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    TasksWorkspaceComponent.prototype.FilterItemClicked = function (myArgs) {
        this.SelectedFilterValue = myArgs;
    };
    TasksWorkspaceComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    TasksWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    TasksWorkspaceComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        this.listArgs.Filters = this.filterAgrs;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(_this.listArgs);
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], TasksWorkspaceComponent.prototype, "viewContainerRef", void 0);
    TasksWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TasksWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TasksWorkspaceComponent);
    return TasksWorkspaceComponent;
}());
exports.TasksWorkspaceComponent = TasksWorkspaceComponent;
var QueueItem = /** @class */ (function () {
    function QueueItem(entity, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.MyEntity = entity;
    }
    Object.defineProperty(QueueItem.prototype, "QueueId", {
        get: function () { return this.MyEntity.QueueId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueItem.prototype, "QueueName", {
        get: function () { return this.MyEntity.QueueName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueItem.prototype, "Count", {
        get: function () { return this.MyEntity.Count; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueItem.prototype, "Title", {
        get: function () { return this.MyEntity.QueueName + " (" + this.MyEntity.Count + ")"; },
        enumerable: true,
        configurable: true
    });
    return QueueItem;
}());
exports.QueueItem = QueueItem;
//# sourceMappingURL=TasksWorkspaceComponent.js.map