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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TMProjectListService_1 = require("../../../Services/StandardLists/TMProjectListService");
var TimeManagementDomainService_1 = require("../../../Services/TimeManagementDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var WeeklyTimeSheetComponent = /** @class */ (function (_super) {
    __extends(WeeklyTimeSheetComponent, _super);
    function WeeklyTimeSheetComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.HasChanges = false;
        _this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TotalSunDayHours = 0;
        _this.TotalMonDayHours = 0;
        _this.TotalTueDayHours = 0;
        _this.TotalWedDayHours = 0;
        _this.TotalThuDayHours = 0;
        _this.TotalSatDayHours = 0;
        _this.TotalFriDayHours = 0;
        _this.TotalWeekHours = 0;
        _this.TotalSunDayClockHours = null;
        _this.TotalMonDayClockHours = null;
        _this.TotalTueDayClockHours = null;
        _this.TotalWedDayClockHours = null;
        _this.TotalThuDayClockHours = null;
        _this.TotalSatDayClockHours = null;
        _this.TotalFriDayClockHours = null;
        _this.TotalWeekClockHours = 0;
        _this.employeeUserId = null;
        // Filters 
        _this.mySelectedLocationFilter = "O";
        // Commands
        _this.SelectedRow = null;
        return _this;
    }
    WeeklyTimeSheetComponent.prototype.InitTab = function (arg) {
        this.employeeUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.locationCode = this.SelectedLocationFilter;
        this.periodStartDate = Tools_1.DateTool.GetStartOfTheWeek(Tools_1.DateTool.GetCurrentDateTimeAsUtc());
        this.SetDates();
        //this.ItemSource = new ObservableCollection([]);
        this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        this.LoadWeeklyTimeSheetList();
    };
    WeeklyTimeSheetComponent.prototype.RefreshTab = function () {
        this.LoadWeeklyTimeSheetList();
    };
    WeeklyTimeSheetComponent.prototype.LoadWeeklyTimeSheetList = function () {
        var _this = this;
        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        }
        this.myDomainService.GetWeeklyTimeSheetList(this.EmployeeUserId, this.LocationCode, this.PeriodStartDate).subscribe(function (myResponse) {
            // this.ItemSource.Clear();
            _this.ItemSource = [];
            if (myResponse.HasError) {
                _this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                var index = 0;
                var myCollection = [];
                myResponse.Result.Items.forEach(function (item) {
                    index += 1;
                    _this.ItemSource.push(new ItemSourceItem(item, _this, index));
                });
                setTimeout(function () { return _this.SetTotalDatesOfList(); }, 2);
                setTimeout(function () { return _this.SetTotalDatesFromClockOfList(myResponse.Result.OfficeClockDays); }, 2);
            }
        });
    };
    WeeklyTimeSheetComponent.prototype.ShowMessage = function (msg) {
        var myMessageWindow = new MessageWindow_1.MessageWindow();
        myMessageWindow.Show(msg);
    };
    WeeklyTimeSheetComponent.prototype.SetDates = function () {
        if (this.periodStartDate != null) {
            this.MonDate = Tools_1.DateTool.NextDay(this.periodStartDate, 1);
            this.TueDate = Tools_1.DateTool.NextDay(this.periodStartDate, 2);
            this.WedDate = Tools_1.DateTool.NextDay(this.periodStartDate, 3);
            this.ThuDate = Tools_1.DateTool.NextDay(this.periodStartDate, 4);
            this.FriDate = Tools_1.DateTool.NextDay(this.periodStartDate, 5);
            this.SatDate = Tools_1.DateTool.NextDay(this.periodStartDate, 6);
        }
    };
    WeeklyTimeSheetComponent.prototype.SetTotalDatesOfList = function () {
        var _this = this;
        this.TotalSunDayHours = 0;
        this.TotalMonDayHours = 0;
        this.TotalTueDayHours = 0;
        this.TotalWedDayHours = 0;
        this.TotalThuDayHours = 0;
        this.TotalSatDayHours = 0;
        this.TotalFriDayHours = 0;
        this.TotalWeekHours = 0;
        this.ItemSource.forEach(function (item) {
            _this.TotalSunDayHours += item.Day1Minutes;
            _this.TotalMonDayHours += item.Day2Minutes;
            _this.TotalTueDayHours += item.Day3Minutes;
            _this.TotalWedDayHours += item.Day4Minutes;
            _this.TotalThuDayHours += item.Day5Minutes;
            _this.TotalFriDayHours += item.Day6Minutes;
            _this.TotalSatDayHours += item.Day7Minutes;
            _this.TotalWeekHours += item.TotalMinutes;
        });
    };
    WeeklyTimeSheetComponent.prototype.SetTotalDatesFromClockOfList = function (items) {
        var totalSunDayClockHoursString = items[0].TotalFromClockString;
        var totalMonDayClockHoursString = items[1].TotalFromClockString;
        var totalTueDayClockHoursString = items[2].TotalFromClockString;
        var totalWedDayClockHoursString = items[3].TotalFromClockString;
        var totalThuDayClockHoursString = items[4].TotalFromClockString;
        var totalSatDayClockHoursString = items[5].TotalFromClockString;
        var totalFriDayClockHoursString = items[6].TotalFromClockString;
        var totalSunDayClockHours = items[0].TotalFromClock;
        var totalMonDayClockHours = items[1].TotalFromClock;
        var totalTueDayClockHours = items[2].TotalFromClock;
        var totalWedDayClockHours = items[3].TotalFromClock;
        var totalThuDayClockHours = items[4].TotalFromClock;
        var totalSatDayClockHours = items[5].TotalFromClock;
        var totalFriDayClockHours = items[6].TotalFromClock;
        this.TotalSunDayClockHours = totalSunDayClockHoursString;
        this.TotalMonDayClockHours = totalMonDayClockHoursString;
        this.TotalTueDayClockHours = totalTueDayClockHoursString;
        this.TotalWedDayClockHours = totalWedDayClockHoursString;
        this.TotalThuDayClockHours = totalThuDayClockHoursString;
        this.TotalSatDayClockHours = totalSatDayClockHoursString;
        this.TotalFriDayClockHours = totalFriDayClockHoursString;
        this.TotalWeekClockHours = (totalSunDayClockHours + totalMonDayClockHours + totalTueDayClockHours +
            totalWedDayClockHours + totalThuDayClockHours + totalSatDayClockHours + totalFriDayClockHours) * 60;
    };
    Object.defineProperty(WeeklyTimeSheetComponent.prototype, "EmployeeUserId", {
        get: function () {
            return this.employeeUserId;
        },
        set: function (value) {
            if (this.employeeUserId != value) {
                this.employeeUserId = value;
                this.LoadWeeklyTimeSheetList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WeeklyTimeSheetComponent.prototype, "LocationCode", {
        get: function () {
            return this.locationCode;
        },
        set: function (value) {
            if (this.locationCode != value) {
                this.locationCode = value;
                this.LoadWeeklyTimeSheetList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WeeklyTimeSheetComponent.prototype, "PeriodStartDate", {
        get: function () {
            return this.periodStartDate;
        },
        set: function (value) {
            if (this.periodStartDate != value) {
                this.periodStartDate = value;
                this.SetDates();
                this.LoadWeeklyTimeSheetList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WeeklyTimeSheetComponent.prototype, "SelectedLocationFilter", {
        get: function () { return this.mySelectedLocationFilter; },
        set: function (value) {
            if (this.mySelectedLocationFilter != value) {
                this.mySelectedLocationFilter = value;
                this.LocationCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WeeklyTimeSheetComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    WeeklyTimeSheetComponent.prototype.PrintPreviewClicked = function () {
    };
    WeeklyTimeSheetComponent.prototype.AddLineClicked = function () {
        var _this = this;
        //this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Line";
        var args = {};
        args.LocationCode = this.LocationCode;
        args.EmployeeUserId = this.EmployeeUserId;
        args.SunDate = this.PeriodStartDate;
        args.MonDate = this.MonDate;
        args.TueDate = this.TueDate;
        args.WedDate = this.WedDate;
        args.ThuDate = this.ThuDate;
        args.SatDate = this.SatDate;
        args.FriDate = this.FriDate;
        args.Father = this;
        args.WINumber = "";
        args.ProjectId = "";
        args.Description = "";
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
        //});
    };
    WeeklyTimeSheetComponent.prototype.CopyLineClicked = function (item) {
        var _this = this;
        //this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Copy Line";
        var args = {};
        args.LocationCode = this.LocationCode;
        args.EmployeeUserId = this.EmployeeUserId;
        args.SunDate = this.PeriodStartDate;
        args.MonDate = this.MonDate;
        args.TueDate = this.TueDate;
        args.WedDate = this.WedDate;
        args.ThuDate = this.ThuDate;
        args.SatDate = this.SatDate;
        args.FriDate = this.FriDate;
        args.Father = this;
        args.WINumber = item.WINumber;
        args.ProjectId = item.ProjectId;
        args.Description = item.Description;
        logWindow.WindowArgs = args;
        logWindow.Show('./TimeManagement/Components/NewEntity/NewLineComponent');
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
        //});
    };
    WeeklyTimeSheetComponent.prototype.OnWindowClosed = function (arg) {
        if (arg == 'OK') {
            this.LoadWeeklyTimeSheetList();
        }
    };
    WeeklyTimeSheetComponent.prototype.SaveWeeklyClicked = function () {
        var _this = this;
        var items = this.ItemSource;
        var itemsChanges = this.ItemSource.filter(function (f) { return f.HasChanges == true; });
        if (itemsChanges.length > 0) {
            var isValid = true;
            if (items.filter(function (f) { return Tools_1.AppTool.IsNullOrEmpty(f.ProjectId) || Tools_1.AppTool.IsNullOrEmpty(f.Description); }).length > 0) {
                isValid = false;
                this.ShowMessage("Project and Description fields are required for each line");
            }
            //else if (items.filter(f => AppTool.IsNullOrEmpty(f.Description)).length > 0) {
            //    isValid = false;
            //    this.ShowMessage("Description field is required for each line");
            //}
            if (isValid) {
                var groupeditems = [];
                items.forEach(function (item) {
                    if (groupeditems.filter(function (f) { return f.ProjectId == item.ProjectId && f.Description == item.Description && f.WINumber == item.WINumber; }).length > 0) {
                        isValid = false;
                    }
                    else {
                        groupeditems.push(item);
                    }
                });
                if (!isValid) {
                    this.ShowMessage("Can't add more than one line with the same Project, Description, WINumber");
                }
                else {
                    this.CurrentSession.StartBusyIndicatorSaving();
                    this.HasChanges = false;
                    var myServiceHelper = new TimeManagementDomainService_1.TimeManagementAPIHelper();
                    myServiceHelper.Id = SessionLocator_1.SessionLocator.Tenant;
                    myServiceHelper.EmployeeUserId = this.EmployeeUserId;
                    myServiceHelper.LocationCode = this.LocationCode;
                    myServiceHelper.StartDate = this.PeriodStartDate;
                    itemsChanges.forEach(function (item) {
                        myServiceHelper.Items.push(item.entity);
                    });
                    if (this.myDomainService == null) {
                        this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
                    }
                    this.myDomainService.UpdateTimeSheetList(myServiceHelper).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            _this.OnDataLoaded(myResponse.Result);
                        }
                    });
                }
            }
        }
    };
    WeeklyTimeSheetComponent.prototype.OnDataLoaded = function (myResultHelper) {
        var _this = this;
        this.ItemSource = [];
        if (myResultHelper) {
            var myCollection = [];
            var index = 0;
            myResultHelper.Items.forEach(function (item) {
                //myCollection.push(new ItemSourceItem(item, this));
                index += 1;
                _this.ItemSource.push(new ItemSourceItem(item, _this, index));
            });
            //this.ItemSource.InsertCollection(myCollection);
            setTimeout(function () { return _this.SetTotalDatesOfList(); }, 2);
            setTimeout(function () { return _this.SetTotalDatesFromClockOfList(myResultHelper.OfficeClockDays); }, 2);
        }
    };
    WeeklyTimeSheetComponent.prototype.GetProjectsClicked = function () {
        var _this = this;
        var employee = this.EmployeeUserId;
        var location = this.LocationCode;
        var periodDate = this.PeriodStartDate;
        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetTMProjects(this.EmployeeUserId, this.LocationCode, this.PeriodStartDate).subscribe(function (myResponse) {
            _this.ItemSource = [];
            if (myResponse.HasError) {
                _this.ShowMessage(myResponse.ErrorsArray[0]);
            }
            else {
                var index = 0;
                var myCollection = [];
                myResponse.Result.Items.forEach(function (item) {
                    index += 1;
                    _this.ItemSource.push(new ItemSourceItem(item, _this, index));
                });
                setTimeout(function () { return _this.SetTotalDatesOfList(); }, 2);
                setTimeout(function () { return _this.SetTotalDatesFromClockOfList(myResponse.Result.OfficeClockDays); }, 2);
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    WeeklyTimeSheetComponent = __decorate([
        core_1.Component({
            selector: 'WeeklyTimeSheetComponent',
            moduleId: module.id,
            templateUrl: './WeeklyTimeSheetComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], WeeklyTimeSheetComponent);
    return WeeklyTimeSheetComponent;
}(BaseComponent_1.BaseComponent));
exports.WeeklyTimeSheetComponent = WeeklyTimeSheetComponent;
var ItemSourceItem = /** @class */ (function (_super) {
    __extends(ItemSourceItem, _super);
    function ItemSourceItem(entity, father, index) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.father = father;
        _this.DataContext = _this;
        _this.IsCopy = false;
        _this.Day1DateFormat = "";
        _this.Day2DateFormat = "";
        _this.Day3DateFormat = "";
        _this.Day4DateFormat = "";
        _this.Day5DateFormat = "";
        _this.Day6DateFormat = "";
        _this.Day7DateFormat = "";
        _this.hasChanges = false;
        _this.showText = true;
        _this.TMProjectListService = new TMProjectListService_1.TMProjectListService();
        _this.Index = index;
        _this.Day1Date = entity.Days.filter(function (d) { return d.Index == 0; })[0];
        _this.Day2Date = entity.Days.filter(function (d) { return d.Index == 1; })[0];
        _this.Day3Date = entity.Days.filter(function (d) { return d.Index == 2; })[0];
        _this.Day4Date = entity.Days.filter(function (d) { return d.Index == 3; })[0];
        _this.Day5Date = entity.Days.filter(function (d) { return d.Index == 4; })[0];
        _this.Day6Date = entity.Days.filter(function (d) { return d.Index == 5; })[0];
        _this.Day7Date = entity.Days.filter(function (d) { return d.Index == 6; })[0];
        _this.Day1DateFormat = _this.ApplyTimeFormat(_this.Day1Minutes);
        _this.Day2DateFormat = _this.ApplyTimeFormat(_this.Day2Minutes);
        _this.Day3DateFormat = _this.ApplyTimeFormat(_this.Day3Minutes);
        _this.Day4DateFormat = _this.ApplyTimeFormat(_this.Day4Minutes);
        _this.Day5DateFormat = _this.ApplyTimeFormat(_this.Day5Minutes);
        _this.Day6DateFormat = _this.ApplyTimeFormat(_this.Day6Minutes);
        _this.Day7DateFormat = _this.ApplyTimeFormat(_this.Day7Minutes);
        return _this;
    }
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
    Object.defineProperty(ItemSourceItem.prototype, "ProjectId", {
        get: function () { return this.entity.ProjectId; },
        set: function (value) {
            if (this.entity.ProjectId != value) {
                this.entity.ProjectId = value;
                this.HasChanges = true;
                this.entity.IsHeaderUpdated = true;
                this.getProjectName(value);
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
            }
            else {
                _this.ProjectName = null;
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
    Object.defineProperty(ItemSourceItem.prototype, "Description", {
        get: function () { return this.entity.Description; },
        set: function (value) {
            if (this.entity.Description != value) {
                this.entity.Description = value;
                this.HasChanges = true;
                this.entity.IsHeaderUpdated = true;
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
    Object.defineProperty(ItemSourceItem.prototype, "Day1Minutes", {
        get: function () {
            if (this.Day1Date != null) {
                return this.Day1Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day1Date.Minuts != value) {
                this.Day1Date.Minuts = value;
                this.HasChanges = true;
                this.Day1DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Day2Minutes", {
        get: function () {
            if (this.Day2Date != null) {
                return this.Day2Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day2Date.Minuts != value) {
                this.Day2Date.Minuts = value;
                this.HasChanges = true;
                this.Day2DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Day3Minutes", {
        get: function () {
            if (this.Day3Date != null) {
                return this.Day3Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day3Date.Minuts != value) {
                this.Day3Date.Minuts = value;
                this.HasChanges = true;
                this.Day3DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Day4Minutes", {
        get: function () {
            if (this.Day4Date != null) {
                return this.Day4Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day4Date.Minuts != value) {
                this.Day4Date.Minuts = value;
                this.HasChanges = true;
                this.Day4DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Day5Minutes", {
        get: function () {
            if (this.Day5Date != null) {
                return this.Day5Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day5Date.Minuts != value) {
                this.Day5Date.Minuts = value;
                this.HasChanges = true;
                this.Day5DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Day6Minutes", {
        get: function () {
            if (this.Day6Date != null) {
                return this.Day6Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day6Date.Minuts != value) {
                this.Day6Date.Minuts = value;
                this.HasChanges = true;
                this.Day6DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Day7Minutes", {
        get: function () {
            if (this.Day7Date != null) {
                return this.Day7Date.Minuts;
            }
        },
        set: function (value) {
            if (this.Day7Date.Minuts != value) {
                this.Day7Date.Minuts = value;
                this.HasChanges = true;
                this.Day7DateFormat = this.ApplyTimeFormat(value);
                this.father.SetTotalDatesOfList();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "TotalMinutes", {
        get: function () { return this.entity.TotalMinutes; },
        set: function (value) {
            if (this.entity.TotalMinutes != value) {
                this.entity.TotalMinutes = value;
                this.HasChanges = true;
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
//# sourceMappingURL=WeeklyTimeSheetComponent.js.map