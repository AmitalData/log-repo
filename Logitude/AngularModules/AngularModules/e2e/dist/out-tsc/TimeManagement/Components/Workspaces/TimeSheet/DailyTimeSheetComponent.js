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
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TMProjectListService_1 = require("../../../Services/StandardLists/TMProjectListService");
var TMLocationListService_1 = require("../../../Services/StandardLists/TMLocationListService");
var SprintListService_1 = require("../../../Services/StandardLists/SprintListService");
var TimeManagementDomainService_1 = require("../../../Services/TimeManagementDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DateTimePipe_1 = require("../../../../Controls/Pipes/DateTimePipe");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var DailyTimeSheetComponent = /** @class */ (function (_super) {
    __extends(DailyTimeSheetComponent, _super);
    function DailyTimeSheetComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.HasChanges = false;
        _this.TotalFromClock = "";
        _this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.LoggedUserName = "";
        _this.isLoaderReady = false;
        _this.Retries = 0;
        _this.TotalDayHours = 0;
        _this.TotalDayClockHours = null;
        _this.employeeUserId = null;
        _this.endDate = null;
        // Filters
        _this.mySelectedLocationFilter = "O";
        _this.mySelectedDateFilter = "T";
        // Commands
        _this.SelectedRow = null;
        _this.IsValid = true;
        return _this;
    }
    DailyTimeSheetComponent.prototype.ComputeLoggedUserName = function () {
        var pipe = new DateTimePipe_1.DateTimePipe();
        this.LoggedUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName + " " + pipe.transform(Tools_1.DateTool.GetCurrentDateAsUtc(), "SD");
    };
    DailyTimeSheetComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.Initialize();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    DailyTimeSheetComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    DailyTimeSheetComponent.prototype.InitTab = function (arg) {
        this.ItemSource = new ObservableCollection_1.ObservableCollection([]);
        this.employeeUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.locationCodeFilter = this.SelectedLocationFilter;
        this.startDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.endDate = this.startDate;
        this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        this.ComputeLoggedUserName();
        this.Initialize();
    };
    DailyTimeSheetComponent.prototype.Initialize = function () {
        //if (this.isLoaderReady) {
        this.LoadDailyTimeSheetList();
        //}
    };
    DailyTimeSheetComponent.prototype.RefreshTab = function () {
        this.ComputeLoggedUserName();
        this.LoadDailyTimeSheetList();
    };
    DailyTimeSheetComponent.prototype.LoadDailyTimeSheetList = function () {
        var _this = this;
        var itemSource = [];
        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        }
        this.myDomainService.GetPeriodTimeSheetList(this.EmployeeUserId, this.LocationCodeFilter, this.StartDate, this.EndDate).subscribe(function (myResponse) {
            // this.ItemSource = [];
            _this.ItemSource.Clear();
            if (myResponse.HasError) {
                _this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                var index = 0;
                var myCollection = [];
                myResponse.Result.ItemsPM.forEach(function (item) {
                    index += 1;
                    itemSource.push(new ItemSourceItem(item, _this, index));
                });
                _this.ItemSource.InsertCollection(itemSource);
                setTimeout(function () { return _this.SetTotalDatesOfList(); }, 2);
                _this.TotalFromClock = myResponse.Result.TotalFromClock;
            }
        });
    };
    DailyTimeSheetComponent.prototype.ShowMessage = function (msg) {
        var myMessageWindow = new MessageWindow_1.MessageWindow();
        myMessageWindow.Show(msg);
    };
    DailyTimeSheetComponent.prototype.SetTotalDatesOfList = function () {
        var _this = this;
        this.TotalDayHours = 0;
        this.ItemSource.Collection.forEach(function (item) {
            _this.TotalDayHours += item.TimeInMinutes;
        });
    };
    Object.defineProperty(DailyTimeSheetComponent.prototype, "EmployeeUserId", {
        get: function () { return this.employeeUserId; },
        set: function (value) {
            if (this.employeeUserId != value) {
                this.employeeUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DailyTimeSheetComponent.prototype, "LocationCodeFilter", {
        get: function () {
            return this.locationCodeFilter;
        },
        set: function (value) {
            if (this.locationCodeFilter != value) {
                this.locationCodeFilter = value;
                this.SaveChanges();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DailyTimeSheetComponent.prototype, "StartDate", {
        get: function () {
            return this.startDate;
        },
        set: function (value) {
            if (this.startDate != value) {
                this.startDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DailyTimeSheetComponent.prototype, "EndDate", {
        get: function () {
            return this.endDate;
        },
        set: function (value) {
            if (this.endDate != value) {
                this.endDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DailyTimeSheetComponent.prototype, "SelectedLocationFilter", {
        get: function () { return this.mySelectedLocationFilter; },
        set: function (value) {
            if (this.mySelectedLocationFilter != value) {
                this.mySelectedLocationFilter = value;
                this.LocationCodeFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DailyTimeSheetComponent.prototype, "SelectedDateFilter", {
        get: function () { return this.mySelectedDateFilter; },
        set: function (value) {
            if (this.mySelectedDateFilter != value) {
                this.mySelectedDateFilter = value;
                this.SetPeriodDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    DailyTimeSheetComponent.prototype.SetPeriodDates = function () {
        if (this.SelectedDateFilter == "T") {
            this.StartDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this.EndDate = this.StartDate;
        }
        else if (this.SelectedDateFilter == "Y") {
            this.StartDate = Tools_1.DateTool.NextDay(Tools_1.DateTool.GetCurrentDateTimeAsUtc(), -1);
            this.EndDate = this.StartDate;
        }
        else {
            this.StartDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this.EndDate = Tools_1.DateTool.NextDay(Tools_1.DateTool.GetCurrentDateTimeAsUtc(), 7);
        }
        this.SaveChanges();
    };
    DailyTimeSheetComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    DailyTimeSheetComponent.prototype.PrintPreviewClicked = function () {
    };
    DailyTimeSheetComponent.prototype.AddLineClicked = function () {
        this.SaveClicked(null, "Add");
    };
    DailyTimeSheetComponent.prototype.ShowAddScreen = function () {
        var _this = this;
        var args = {};
        args.IsNew = true;
        args.LocationCode = this.LocationCodeFilter != "A" ? this.LocationCodeFilter : "O";
        args.EmployeeUserId = this.EmployeeUserId;
        args.Father = this;
        if (this.SelectedDateFilter != "P") {
            var date = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (this.SelectedDateFilter == "Y") {
                date = Tools_1.DateTool.NextDay(Tools_1.DateTool.GetCurrentDateTimeAsUtc(), -1);
            }
            args.DateOfWork = date;
        }
        args.WINumber = null;
        args.ProjectId = null;
        args.Description = null;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Line";
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
    };
    DailyTimeSheetComponent.prototype.CopyLineClicked = function (item) {
        this.SaveClicked(item, "Copy");
    };
    DailyTimeSheetComponent.prototype.ShowCopyScreen = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Copy Line";
        var args = {};
        args.LocationCode = item.LocationCode;
        args.EmployeeUserId = this.EmployeeUserId;
        var date = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (this.SelectedDateFilter == "Y") {
            date = Tools_1.DateTool.NextDay(Tools_1.DateTool.GetCurrentDateTimeAsUtc(), -1);
        }
        args.DateOfWork = date;
        args.Father = this;
        args.IsNew = true;
        args.WINumber = item.WINumber;
        args.ProjectId = item.ProjectId;
        args.SprintId = item.SprintId;
        args.Description = item.Description;
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
    };
    DailyTimeSheetComponent.prototype.EditLineClicked = function (item) {
        this.SaveClicked(item, "Edit");
    };
    DailyTimeSheetComponent.prototype.ShowEditScreen = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Line";
        var args = {};
        args.EntityPM = item.entityPM;
        args.Father = this;
        args.IsNew = false;
        args.LocationCode = item.LocationCode;
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
    };
    DailyTimeSheetComponent.prototype.OnWindowClosed = function (arg) {
        if (arg == 'OK') {
            this.LoadDailyTimeSheetList();
        }
    };
    DailyTimeSheetComponent.prototype.SearchButtonClicked = function () {
        if (this.StartDate == null || this.EndDate == null) {
            this.ShowMessage("Please enter both Start and End dates");
        }
        else {
            this.SaveChanges();
        }
    };
    DailyTimeSheetComponent.prototype.SaveClicked = function (item, params) {
        var _this = this;
        if (item === void 0) { item = null; }
        if (params === void 0) { params = null; }
        this.IsValid = true;
        var items = this.ItemSource.Collection;
        var itemsChanges = this.ItemSource.Collection.filter(function (f) { return f.HasChanges == true; });
        if (itemsChanges.length > 0) {
            var msg = "";
            var requiredSprints = items.filter(function (f) { return f.SprintId == null; }).length;
            var requiredDescriptions = items.filter(function (f) { return Tools_1.AppTool.IsNullOrEmpty(f.Description); }).length;
            var dayOffValidation = items.filter(function (f) { return (f.Project != null && !Tools_1.AppTool.IsNullOrEmpty(f.Project.DayOffTypeCode) && f.LocationCode != "D") || (f.LocationCode == "D" && f.Project != null && Tools_1.AppTool.IsNullOrEmpty(f.Project.DayOffTypeCode)); }).length;
            var dayOffProjectValidation = items.filter(function (f) { return Tools_1.AppTool.IsNullOrEmpty(f.ProjectId) && f.LocationCode == "D"; }).length;
            if (requiredSprints > 0 && requiredDescriptions) {
                msg = "Sprint and Description fields are required for each line.";
            }
            else {
                if (requiredSprints > 0) {
                    msg = "Sprint field is required for each line.";
                }
                if (requiredDescriptions > 0) {
                    msg = "Description field is required for each line.";
                }
            }
            if (dayOffValidation > 0) {
                msg = "Project with a Day Off type requires a Day off Location for each line.";
            }
            if (dayOffProjectValidation > 0) {
                msg = "Project is required for Day Off location";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(msg)) {
                this.IsValid = false;
                this.ShowMessage(msg);
            }
            if (this.IsValid) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.HasChanges = false;
                var myServiceHelper = new TimeManagementDomainService_1.TimeManagementAPIHelper();
                myServiceHelper.Id = SessionLocator_1.SessionLocator.Tenant;
                myServiceHelper.EmployeeUserId = this.EmployeeUserId;
                myServiceHelper.LocationCode = this.LocationCodeFilter;
                myServiceHelper.StartDate = this.StartDate;
                myServiceHelper.EndDate = this.EndDate;
                itemsChanges.forEach(function (item) {
                    myServiceHelper.ItemsPM.push(item.entity);
                });
                if (this.myDomainService == null) {
                    this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
                }
                this.myDomainService.UpdateTimeSheetList(myServiceHelper).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        _this.OnDataLoaded(myResponse.Result);
                        if (!myResponse.HasError) {
                            _this.LoadDailyTimeSheetList();
                            if (params != null) {
                                switch (params) {
                                    case "Edit":
                                        _this.ShowEditScreen(item);
                                        break;
                                    case "Add":
                                        _this.ShowAddScreen();
                                        break;
                                    case "Copy":
                                        _this.ShowCopyScreen(item);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                });
            }
        }
        else {
            if (params != null) {
                switch (params) {
                    case "Edit":
                        this.ShowEditScreen(item);
                        break;
                    case "Add":
                        this.ShowAddScreen();
                        break;
                    case "Copy":
                        this.ShowCopyScreen(item);
                        break;
                    default:
                        break;
                }
            }
        }
    };
    DailyTimeSheetComponent.prototype.SaveChanges = function () {
        var _this = this;
        var items = this.ItemSource.Collection;
        var itemsChanges = this.ItemSource.Collection.filter(function (f) { return f.HasChanges == true; });
        if (itemsChanges.length > 0) {
            if (this.IsValid) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.HasChanges = false;
                var myServiceHelper = new TimeManagementDomainService_1.TimeManagementAPIHelper();
                myServiceHelper.Id = SessionLocator_1.SessionLocator.Tenant;
                myServiceHelper.EmployeeUserId = this.EmployeeUserId;
                myServiceHelper.LocationCode = this.LocationCodeFilter;
                myServiceHelper.StartDate = this.StartDate;
                myServiceHelper.EndDate = this.EndDate;
                itemsChanges.forEach(function (item) {
                    myServiceHelper.ItemsPM.push(item.entity);
                });
                if (this.myDomainService == null) {
                    this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
                }
                this.myDomainService.UpdateTimeSheetList(myServiceHelper).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        }
        else {
            this.LoadDailyTimeSheetList();
        }
    };
    DailyTimeSheetComponent.prototype.OnDataLoaded = function (myResultHelper) {
        var _this = this;
        //this.ItemSource = [];
        this.ItemSource.Clear();
        if (myResultHelper) {
            var myCollection = [];
            var index = 0;
            myResultHelper.ItemsPM.forEach(function (item) {
                index += 1;
                myCollection.push(new ItemSourceItem(item, _this, index));
            });
            this.ItemSource.InsertCollection(myCollection);
            setTimeout(function () { return _this.SetTotalDatesOfList(); }, 2);
            this.TotalFromClock = myResultHelper.TotalFromClock;
        }
    };
    DailyTimeSheetComponent.prototype.DeleteLineClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this line?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (item != null) {
                    // var isValid = true;
                    //if (this.ItemSource.Collection.filter(f => AppTool.IsNullOrEmpty(f.ProjectId) || AppTool.IsNullOrEmpty(f.Description)).length > 0) {
                    //    isValid = false;
                    //    this.ShowMessage("Project and Description fields are required for each line");
                    //}
                    // if (isValid) {
                    _this.CurrentSession.StartBusyIndicatorSaving();
                    _this.HasChanges = false;
                    if (_this.myDomainService == null) {
                        _this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
                    }
                    _this.myDomainService.DeleteTimeSheetItem(item.Id, item.EmployeeUserId, item.LocationCode, _this.StartDate, _this.EndDate).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            _this.OnDataLoaded(myResponse.Result);
                        }
                    });
                    //}
                }
            }
        });
    };
    DailyTimeSheetComponent.prototype.RefreshButtonClicked = function () {
        this.RefreshTab();
    };
    DailyTimeSheetComponent.prototype.ProrateButtonClicked = function () {
        var iService = new TimeManagementDomainService_1.TimeManagementDomainService();
        iService.Prorate(this.EmployeeUserId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
            }
        });
    };
    DailyTimeSheetComponent.prototype.CalculationButtonClicked = function () {
        var _this = this;
        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        }
        this.myDomainService.GetCalculationCompleteWork().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    DailyTimeSheetComponent.prototype.btnExcelCLicked = function () {
        this.DownloadExcel();
    };
    // Download Excel 
    DailyTimeSheetComponent.prototype.DownloadExcel = function () {
        this.myDomainService.DownloadEmployeesTimesToExcel(this.EmployeeUserId, this.LocationCodeFilter, this.StartDate, this.EndDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + "Tariffs" + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007";
                {
                    window.open(url);
                }
            }
        });
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], DailyTimeSheetComponent.prototype, "AllLocations", void 0);
    DailyTimeSheetComponent = __decorate([
        core_1.Component({
            selector: 'DailyTimeSheetComponent',
            moduleId: module.id,
            templateUrl: './DailyTimeSheetComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DailyTimeSheetComponent);
    return DailyTimeSheetComponent;
}(BaseComponent_1.BaseComponent));
exports.DailyTimeSheetComponent = DailyTimeSheetComponent;
var ItemSourceItem = /** @class */ (function (_super) {
    __extends(ItemSourceItem, _super);
    function ItemSourceItem(entity, father, index) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.father = father;
        _this.DataContext = _this;
        _this.IsCopy = false;
        _this.hasChanges = false;
        _this.showText = true;
        _this.entityPM = entity;
        _this.TMProjectListService = new TMProjectListService_1.TMProjectListService();
        _this.SprintListService = new SprintListService_1.SprintListService();
        _this.Index = index;
        _this.DayDateFormat = _this.ApplyTimeFormat(_this.TimeInMinutes);
        return _this;
    }
    //public DayDateFormat = "";
    ItemSourceItem.prototype.ApplyTimeFormat = function (minutes) {
        var formattedMinutes = "";
        var val = minutes;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            formattedMinutes = "";
        }
        else {
            formattedMinutes = result;
        }
        return formattedMinutes;
    };
    Object.defineProperty(ItemSourceItem.prototype, "HasChanges", {
        get: function () { return this.hasChanges; },
        set: function (value) {
            if (this.hasChanges != value) {
                this.hasChanges = value;
                this.father.HasChanges = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Id", {
        get: function () { return this.entity.Id; },
        set: function (value) {
            if (this.entity.Id != value) {
                this.entity.Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "ProjectId", {
        get: function () { return this.entity.ProjectId; },
        set: function (value) {
            if (this.entity.ProjectId != value) {
                this.entity.ProjectId = value;
                this.HasChanges = true;
                this.getProjectName(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Project", {
        get: function () { return this.project; },
        set: function (value) {
            if (this.project != value) {
                this.project = value;
                if (this.project != null && !Tools_1.AppTool.IsNullOrEmpty(this.project.DayOffTypeCode)) {
                    this.LocationCode = "D";
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemSourceItem.prototype.getProjectName = function (value) {
        var _this = this;
        if (this.TMProjectListService == null) {
            this.TMProjectListService = new TMProjectListService_1.TMProjectListService();
        }
        this.TMProjectListService.getSingle(value).subscribe(function (myResult) {
            var project = myResult.Result;
            if (project != null) {
                _this.ProjectName = project.Name;
                if (_this.project != null && !Tools_1.AppTool.IsNullOrEmpty(_this.project.DayOffTypeCode)) {
                    _this.LocationCode = "D";
                }
            }
            else {
                _this.ProjectName = null;
            }
        });
    };
    ItemSourceItem.prototype.getLocationName = function (value) {
        var _this = this;
        if (this.TMLocationListService == null) {
            this.TMLocationListService = new TMLocationListService_1.TMLocationListService();
        }
        if (this.TMProjectListService == null) {
            this.TMProjectListService = new TMProjectListService_1.TMProjectListService();
        }
        this.TMLocationListService.getSingle(value).subscribe(function (myResult) {
            var location = myResult.Result;
            if (location != null) {
                _this.LocationName = location.Name;
            }
            else {
                _this.LocationName = null;
            }
        });
        this.TMProjectListService.getSingle(this.ProjectId).subscribe(function (myResult) {
            var project = myResult.Result;
            if (project != null) {
                _this.project = project;
            }
            else {
                _this.project = null;
            }
        });
    };
    Object.defineProperty(ItemSourceItem.prototype, "ProjectName", {
        get: function () { return this.entity.ProjectName; },
        set: function (value) {
            if (this.entity.ProjectName != value) {
                this.entity.ProjectName = value;
                this.HasChanges = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "SprintId", {
        get: function () { return this.entity.SprintId; },
        set: function (value) {
            if (this.entity.SprintId != value) {
                this.entity.SprintId = value;
                this.HasChanges = true;
                this.getSprintName(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemSourceItem.prototype.getSprintName = function (value) {
        var _this = this;
        if (this.SprintListService == null) {
            this.SprintListService = new SprintListService_1.SprintListService();
        }
        this.SprintListService.getSingle(value).subscribe(function (myResult) {
            var sprint = myResult.Result;
            if (sprint != null) {
                _this.SprintName = sprint.Name;
            }
            else {
                _this.SprintName = null;
            }
        });
    };
    Object.defineProperty(ItemSourceItem.prototype, "SprintName", {
        get: function () { return this.entity.SprintName; },
        set: function (value) {
            if (this.entity.SprintName != value) {
                this.entity.SprintName = value;
                this.HasChanges = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "LocationName", {
        get: function () { return this.entity.LocationName; },
        set: function (value) {
            if (this.entity.LocationName != value) {
                this.entity.LocationName = value;
                this.HasChanges = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Description", {
        get: function () { return this.entity.Description; },
        set: function (value) {
            if (this.entity.Description != value) {
                this.entity.Description = value;
                this.HasChanges = true;
                //this.entity.IsHeaderUpdated = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "WINumber", {
        get: function () { return this.entity.WINumber; },
        set: function (value) {
            if (this.entity.WINumber != value) {
                this.entity.WINumber = value;
                this.HasChanges = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "EmployeeUserId", {
        get: function () { return this.entity.EmployeeUserId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "LocationCode", {
        get: function () { return this.entity.LocationCode; },
        set: function (value) {
            if (this.entity.LocationCode != value) {
                this.entity.LocationCode = value;
                this.HasChanges = true;
                this.getLocationName(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "ShowText", {
        get: function () {
            return this.showText;
        },
        set: function (value) {
            if (this.showText != value) {
                this.showText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemSourceItem.prototype.Ondblclicked = function () {
        this.ShowText = false;
    };
    ItemSourceItem.prototype.OnDivBlur = function () {
        this.ShowText = true;
    };
    Object.defineProperty(ItemSourceItem.prototype, "TimeInMinutes", {
        get: function () { return this.entity.TimeInMinutes; },
        set: function (value) {
            if (this.entity.TimeInMinutes != value) {
                this.entity.TimeInMinutes = value;
                this.HasChanges = true;
                this.DayDateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "DayDateFormat", {
        get: function () { return this.dayDateFormat; },
        set: function (value) {
            if (this.dayDateFormat != value) {
                this.dayDateFormat = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "DateOfWork", {
        get: function () {
            return this.entity.DateOfWork;
        },
        set: function (value) {
            if (this.entity.DateOfWork != value) {
                this.entity.DateOfWork = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemSourceItem.prototype.ViewWI = function (wiNumber) {
        var url = "https://logitudeteam.visualstudio.com/DefaultCollection/LogitudeWorld/_workitems/edit/" + wiNumber;
        window.open(url);
    };
    return ItemSourceItem;
}(BaseComponent_1.BaseComponent));
exports.ItemSourceItem = ItemSourceItem;
//# sourceMappingURL=DailyTimeSheetComponent.js.map