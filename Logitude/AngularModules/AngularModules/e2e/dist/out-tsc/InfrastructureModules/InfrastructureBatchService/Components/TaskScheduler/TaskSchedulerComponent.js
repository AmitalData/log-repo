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
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TasksSchedulerPM_1 = require("../../../../Infrastructure/EntityPMs/TasksSchedulerPM");
var SchedulerDetails_1 = require("../../../../Infrastructure/DataContracts/SchedulerDetails");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var TaskSchedulerComponent = /** @class */ (function () {
    function TaskSchedulerComponent(_entityListService) {
        var _this = this;
        this._entityListService = _entityListService;
        this.ItemsSource = [];
        this.FixedItemsSource = [];
        this.HistoryItemsSource = [];
        this.loadedDataList = [];
        this.IsEnableAddButton = false;
        this.columns = null;
        this.CustomColumnsReady = new core_1.EventEmitter();
        this.MenuHeaderchangeevent = new core_1.EventEmitter();
        this.ShowUTCTimesLabel = "Show UTC Time";
        this.ShowUTCTimeEnabled = false;
        this.HasUTCFeature = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SchedulerType = "";
        this.IsHistoryGridVsisible = false;
        this.DataSource = {
            pageSize: 20,
            rowCount: null,
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this.filterTypeCode = "AC";
        this.infraDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "SHOWUTCBUTTON")) {
            this.HasUTCFeature = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "NEW"))
            this.IsEnableAddButton = true;
    }
    TaskSchedulerComponent.prototype.ngOnInit = function () {
        this.LoadTaskHistories();
    };
    TaskSchedulerComponent.prototype.LoadData = function (schedulerType) {
        this.SchedulerType = schedulerType;
        this.GetTasksSchedular();
    };
    TaskSchedulerComponent.prototype.GetTasksSchedular = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.IsHistoryGridVsisible = false;
        this.infraDomainService.GetAllTasksSchedulerPMs(this.SchedulerType).subscribe(function (myResult) {
            if (myResult == null) {
                _this.ItemsSource = [];
                _this.FixedItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.loadedDataList = myResponse.Result;
                    _this.BuildItemsSource();
                    _this.LoadTaskHistories();
                }
            }
        });
    };
    TaskSchedulerComponent.prototype.RefreshTasksSchedular = function (entityPM) {
        var index = this.loadedDataList.indexOf(entityPM);
        if (index > -1) {
            this.loadedDataList[index] = entityPM;
        }
        else
            this.loadedDataList.push(entityPM);
        this.BuildItemsSource();
    };
    TaskSchedulerComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.FixedItemsSource = [];
        this.loadedDataList.forEach(function (item) {
            _this.ItemsSource.push(new TaskSchedulerItemClass(item, _this));
            _this.FixedItemsSource.push(new TaskSchedulerItemClass(item, _this));
        });
        if (this.filterTypeCode) {
            if (this.filterTypeCode == "AL") {
                this.ItemsSource = this.FixedItemsSource;
            }
            else if (this.filterTypeCode == "IN") {
                this.ItemsSource = this.FixedItemsSource.filter(function (a) { return a.InActive == true; });
            }
            else {
                this.ItemsSource = this.FixedItemsSource.filter(function (a) { return a.InActive == false; });
                ;
            }
        }
        else {
            this.ItemsSource = this.FixedItemsSource.filter(function (a) { return a.InActive == false; });
        }
        this.CurrentSession.StopBusyIndicator();
    };
    TaskSchedulerComponent.prototype.Selecting = function (item) {
        this.SelectedRow = item;
        if (item == null) {
            this.IsHistoryGridVsisible = false;
        }
        else {
            //this.LoadHistoryList();
            this.LoadTaskHistories();
        }
    };
    TaskSchedulerComponent.prototype.LoadHistoryList = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.infraDomainService.GetTaskSchedulerHistory(this.SelectedRow.Id).subscribe(function (myResult) {
            if (myResult == null) {
                _this.HistoryItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.HistoryItemsSource = myResponse.Result;
                    _this.IsHistoryGridVsisible = true;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    TaskSchedulerComponent.prototype.NewTaskClicked = function () {
        var _this = this;
        var newItem = new TasksSchedulerPM_1.TasksSchedulerPM();
        newItem.CreatedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        newItem.UpdatedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        newItem.TriggerType = "O";
        newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newItem.Type = this.SchedulerType;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Height = (this.SchedulerType == "FTP" || this.SchedulerType == "SFTP") ? 820 : 750;
        logWindow.Width = 900;
        logWindow.Title = this.SchedulerType + " Scheduler Details";
        logWindow.DataContext = new TaskSchedulerItemClass(newItem, this, true);
        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/AddEditTaskSchedulerComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.RefreshButtonClicked();
            }
        });
    };
    TaskSchedulerComponent.prototype.EditClicked = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = this.SchedulerType + " Scheduler Details";
        logWindow.DataContext = item;
        logWindow.Height = (this.SchedulerType == "FTP" || this.SchedulerType == "SFTP") ? 820 : 750;
        logWindow.Width = 900;
        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/AddEditTaskSchedulerComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.RefreshButtonClicked();
            }
        });
    };
    TaskSchedulerComponent.prototype.RefreshButtonClicked = function () {
        this.GetTasksSchedular();
    };
    TaskSchedulerComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TaskSchedulerComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "Log",
            DataTypeCode: 'String',
            Display: 'Log',
            Styles: { width: '280px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        if (this.ShowUTCTimeEnabled == false) {
            this.columns.push({
                FieldName: "StartDateTime",
                DataTypeCode: 'String',
                Display: 'Start Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "StartDateTime"
            });
            this.columns.push({
                FieldName: "EndDateTime",
                DataTypeCode: 'String',
                Display: 'End Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "EndDateTime"
            });
        }
        else {
            this.columns.push({
                FieldName: "StartDateTimeUTC",
                DataTypeCode: 'String',
                Display: 'Start Date UTC',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "StartDateTime"
            });
            this.columns.push({
                FieldName: "EndDateTimeUTC",
                DataTypeCode: 'String',
                Display: 'End Date UTC',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "EndDateTime"
            });
        }
        this.columns.push({
            FieldName: "Duration",
            DataTypeCode: 'String',
            Display: 'Duration',
            Styles: { width: '200px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        this.columns.push({
            FieldName: "ViewLog",
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '100px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        /////
        //this.columns.push({
        //    FieldName: "RunResult",
        //    DataTypeCode: 'String',
        //    Display: 'Run Result',
        //    Styles: { width: '200px' },
        //    IsCustomTemplate: true,
        //    ServerSideSortable: false
        //});
        this.CustomColumnsReady.emit(this.columns);
    };
    TaskSchedulerComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        filters = new ApiQueryFilters_1.ApiQueryFilters();
        //filters.SortBy = "StatusDate";
        // filters.SortDirection = "Desc";
        sortingCol = "StartDateTimeUTC";
        sortingDir = "descending";
        if (!this.SelectedRow) {
            if (filters.AdditionalFilters.filter(function (a) { return a.FieldName == "TaskId"; }).length > 0) {
                filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "TaskId"; });
            }
            filters.addAdditionalFilter("TaskId", "0-0", null, null, "Equals", false, false, false, "String");
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
                if (filters.AdditionalFilters.filter(function (a) { return a.FieldName == "TaskId"; }).length > 0) {
                    filters.AdditionalFilters = filters.AdditionalFilters.filter(function (a) { return a.FieldName != "TaskId"; });
                }
                filters.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Equals", false, false, false, "String");
            }
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        if (sortingCol) {
            filters.SortBy = sortingCol;
        }
        if (sortingDir) {
            filters.SortDirection = sortingDir;
        }
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        return this._entityListService.getByFilters("TaskSchedulerHistory", filters);
    };
    TaskSchedulerComponent.prototype.LoadTaskHistories = function () {
        this.IsHistoryGridVsisible = true;
        this.BuildColumns();
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (!this.SelectedRow) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
            this.filterAgrs.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Contains", true, false, false, "String");
        }
        //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsCancelled').length > 0) {
        //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsCancelled');
        //}
        //this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        //}
        //this.filterAgrs.addAdditionalFilter("IsMissingDocument", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", false, null, null, "Equals", false, false, false, "Boolean");
        //if (this.SelectedTransportFilter != "All") {
        //    this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
        //}
        //else {
        //    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
        //        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
        //    }
        //}
        //if (this.SelectedArchiveFilter != "All") {
        //    this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedArchiveFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", this.SelectedArchiveFilter == "All" ? true : false);
        //}
        //else {
        //    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
        //        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
        //    }
        //}
        //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedTransportFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", true);
        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", "null", null, null, "Equals", false, true, false, "string", true);
        //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", true, null, null, "Equals", false, true, false, "string", true);
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    TaskSchedulerComponent.prototype.ShowUTCTimesClicked = function () {
        if (this.ShowUTCTimeEnabled == true) {
            this.ShowUTCTimeEnabled = false;
            this.ShowUTCTimesLabel = "Show UTC Time";
        }
        else {
            this.ShowUTCTimeEnabled = true;
            this.ShowUTCTimesLabel = "Hide UTC Time";
        }
        this.LoadTaskHistories();
    };
    Object.defineProperty(TaskSchedulerComponent.prototype, "FilterTypeCode", {
        get: function () { return this.filterTypeCode; },
        set: function (value) {
            if (this.filterTypeCode != value) {
                this.filterTypeCode = value;
                if (value == "AC") {
                    this.ItemsSource = this.FixedItemsSource.filter(function (a) { return a.InActive == false; });
                }
                else if (value == "IN") {
                    this.ItemsSource = this.FixedItemsSource.filter(function (a) { return a.InActive == true; });
                }
                else {
                    this.ItemsSource = this.FixedItemsSource;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TaskSchedulerComponent.prototype, "CustomColumnsReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TaskSchedulerComponent.prototype, "MenuHeaderchangeevent", void 0);
    TaskSchedulerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TaskSchedulerComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], TaskSchedulerComponent);
    return TaskSchedulerComponent;
}());
exports.TaskSchedulerComponent = TaskSchedulerComponent;
var TaskSchedulerItemClass = /** @class */ (function (_super) {
    __extends(TaskSchedulerItemClass, _super);
    function TaskSchedulerItemClass(item, fatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "TasksScheduler";
        _this.IsNew = false;
        _this.SchedulerDetailsData = new SchedulerDetails_1.SchedulerDetails();
        _this.EntityPM = item;
        _this.IsNew = isNew;
        return _this;
    }
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Id", {
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "NextRunTime", {
        get: function () { return this.EntityPM.NextRunTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "LastRunTime", {
        get: function () { return this.EntityPM.LastRunStartTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "NextRunTimeUTC", {
        get: function () { return this.EntityPM.NextRunTimeUTC; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "LastRunTimeUTC", {
        get: function () { return this.EntityPM.LastRunStartTimeUTC; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "LastRunResult", {
        get: function () { return this.EntityPM.LastRunResult; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "StartDate", {
        get: function () { return this.EntityPM.StartDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "StartDateUTC", {
        get: function () { return this.EntityPM.StartDateTimeUTC; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Status", {
        get: function () { return this.EntityPM.Status; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "UpdatedBy", {
        get: function () { return this.EntityPM.UpdatedBy; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "CreatedBy", {
        get: function () { return this.EntityPM.CreatedBy; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Duration", {
        get: function () { return this.EntityPM.Duration; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "LastRunEndTime", {
        get: function () { return this.EntityPM.LastRunEndTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "LastRunEndTimeUTC", {
        get: function () { return this.EntityPM.LastRunEndTimeUTC; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "ServiceClassName", {
        get: function () { return this.EntityPM.ServiceClassName; },
        set: function (newValue) {
            if (this.EntityPM.ServiceClassName != newValue) {
                this.EntityPM.ServiceClassName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "TriggerType", {
        get: function () { return this.EntityPM.TriggerType; },
        set: function (newValue) {
            if (this.EntityPM.TriggerType != newValue) {
                this.EntityPM.TriggerType = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "StartDateTime", {
        get: function () { return this.EntityPM.StartDateTime; },
        set: function (newValue) {
            if (this.EntityPM.StartDateTime != newValue) {
                this.EntityPM.StartDateTime = newValue;
                this.newValueinDateFormat = new Date(newValue);
                this.EntityPM.StartDateTimeUTC = new Date(this.newValueinDateFormat.getUTCFullYear(), this.newValueinDateFormat.getUTCMonth(), this.newValueinDateFormat.getUTCDate(), this.newValueinDateFormat.getUTCHours(), this.newValueinDateFormat.getUTCMinutes(), this.newValueinDateFormat.getUTCSeconds(), this.newValueinDateFormat.getUTCMilliseconds());
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "RepeatInMinutes", {
        get: function () { return this.EntityPM.RepeatInMinutes; },
        set: function (newValue) {
            if (this.EntityPM.RepeatInMinutes != newValue) {
                this.EntityPM.RepeatInMinutes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "MonthlyDay", {
        get: function () { return this.EntityPM.MonthlyDay; },
        set: function (newValue) {
            if (this.EntityPM.MonthlyDay != newValue) {
                this.EntityPM.MonthlyDay = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Satarday", {
        get: function () { return this.EntityPM.Satarday; },
        set: function (newValue) {
            if (this.EntityPM.Satarday != newValue) {
                this.EntityPM.Satarday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Sunday", {
        get: function () { return this.EntityPM.Sunday; },
        set: function (newValue) {
            if (this.EntityPM.Sunday != newValue) {
                this.EntityPM.Sunday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Monday", {
        get: function () { return this.EntityPM.Monday; },
        set: function (newValue) {
            if (this.EntityPM.Monday != newValue) {
                this.EntityPM.Monday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Tuesday", {
        get: function () { return this.EntityPM.Tuesday; },
        set: function (newValue) {
            if (this.EntityPM.Tuesday != newValue) {
                this.EntityPM.Tuesday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Wednesday", {
        get: function () { return this.EntityPM.Wednesday; },
        set: function (newValue) {
            if (this.EntityPM.Wednesday != newValue) {
                this.EntityPM.Wednesday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Thursday", {
        get: function () { return this.EntityPM.Thursday; },
        set: function (newValue) {
            if (this.EntityPM.Thursday != newValue) {
                this.EntityPM.Thursday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Friday", {
        get: function () { return this.EntityPM.Friday; },
        set: function (newValue) {
            if (this.EntityPM.Friday != newValue) {
                this.EntityPM.Friday = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Type", {
        get: function () {
            return this.EntityPM.Type ? this.EntityPM.Type : "";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Host", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Host : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Host != newValue) {
                this.FTPDetails.Host = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Folder", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Folder : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Folder != newValue) {
                this.FTPDetails.Folder = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "UserName", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.UserName : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.UserName != newValue) {
                this.FTPDetails.UserName = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Password", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Password : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Password != newValue) {
                this.FTPDetails.Password = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "From", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.From : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.From != newValue) {
                this.FTPDetails.From = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Subject", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Subject : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Subject != newValue) {
                this.FTPDetails.Subject = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Prefix", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Prefix : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Prefix != newValue) {
                this.FTPDetails.Prefix = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Suffix", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Suffix : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Suffix != newValue) {
                this.FTPDetails.Suffix = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "Extension", {
        get: function () {
            return this.FTPDetails ? this.FTPDetails.Extension : "";
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.Extension != newValue) {
                //if (!AppTool.IsNullOrEmpty(newValue) && newValue.startsWith("."))
                //    newValue = newValue.substring(1, newValue.length);
                this.FTPDetails.Extension = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaskSchedulerItemClass.prototype, "IsSFTP", {
        get: function () {
            return this.FTPDetails.IsSFTP;
        },
        set: function (newValue) {
            if (this.FTPDetails && this.FTPDetails.IsSFTP != newValue) {
                this.FTPDetails.IsSFTP = newValue;
                this.EntityPM.IsDirty = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    TaskSchedulerItemClass.prototype.SetSchedulerDetailsData = function (schedulerDetailsData) {
        this.SchedulerDetailsData = schedulerDetailsData;
        if (schedulerDetailsData) {
            if (this.EntityPM.Type == "FTP" || this.EntityPM.Type == "SFTP") {
                if (!schedulerDetailsData.FTPDetails) {
                    schedulerDetailsData.FTPDetails = new SchedulerDetails_1.FTPSchedulerDetails();
                    schedulerDetailsData.FTPDetails.IsSFTP = (this.EntityPM.Type == "SFTP" ? true : false);
                }
                this.FTPDetails = schedulerDetailsData.FTPDetails;
            }
        }
    };
    return TaskSchedulerItemClass;
}(BaseComponent_1.BaseComponent));
exports.TaskSchedulerItemClass = TaskSchedulerItemClass;
//# sourceMappingURL=TaskSchedulerComponent.js.map