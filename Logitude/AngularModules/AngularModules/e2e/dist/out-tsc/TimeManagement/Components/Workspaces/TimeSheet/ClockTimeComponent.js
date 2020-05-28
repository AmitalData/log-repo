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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var TimeOfficeHourDomainService_1 = require("../../../Services/TimeOfficeHourDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ClockTimeComponent = /** @class */ (function (_super) {
    __extends(ClockTimeComponent, _super);
    function ClockTimeComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.ObjectTableName = "TMOfficeHour";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.employeeUserId = null;
        _this.ItemSourceCollection = new ObservableCollection_1.ObservableCollection([]);
        _this.myDomainService = new TimeOfficeHourDomainService_1.TimeOfficeHourDomainService();
        _this._entityResourceService.getEntityResourceByTableName("TMOfficeHour", 0).subscribe(function (response) {
        });
        return _this;
    }
    Object.defineProperty(ClockTimeComponent.prototype, "EmployeeUserId", {
        get: function () { return this.employeeUserId; },
        set: function (value) {
            if (this.employeeUserId != value) {
                this.employeeUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClockTimeComponent.prototype, "FromDate", {
        get: function () {
            return this.fromDate;
        },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClockTimeComponent.prototype, "ToDate", {
        get: function () {
            return this.toDate;
        },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ClockTimeComponent.prototype.AddNewClockHour = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("TMOfficeHour", 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "New Office Hour";
            var args = {};
            args.EmployeeUserId = _this.EmployeeUserId;
            logWindow.WindowArgs = args;
            logWindow.Show('./TimeManagement/Components/NewEntity/NewOfficeHourComponent');
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
        });
    };
    ClockTimeComponent.prototype.OnWindowClosed = function (event) {
        if (event == "OK") {
            this.LoadClockTimeSheet();
        }
    };
    Object.defineProperty(ClockTimeComponent.prototype, "HasChanged", {
        //DeleteLineClicked(item: ItemSourceItem) {
        //    var confirmWindow = new ConfirmWindow();
        //    confirmWindow.Show("Are you sure you want to delete this line ?");
        //    confirmWindow.WindowClosed.subscribe((event: any) => {
        //        if (confirmWindow.Yes) {
        //            this.CurrentSession.StartBusyIndicatorSaving();
        //            if (this.myDomainService == null) {
        //                this.myDomainService = new TimeManagementDomainService();
        //            }
        //            this.CurrentSession.StopBusyIndicator();
        //        }
        //    });
        //}
        get: function () {
            if (this.ItemSourceCollection.Collection.filter(function (p) { return p.IsDirty; })[0])
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    ClockTimeComponent.prototype.LoadClockTimeSheet = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.FromDate == null)
            this.ValidationErrorsList.push("From date field is required");
        if (this.ToDate == null)
            this.ValidationErrorsList.push("To date field is required");
        if (this.ToDate < this.FromDate && this.ValidationErrorsList.length == 0)
            this.ValidationErrorsList.push("From date field must be less than To date field");
        if (this.ValidationErrorsList.length == 0) {
            if (this.myDomainService == null) {
                this.myDomainService = new TimeOfficeHourDomainService_1.TimeOfficeHourDomainService();
            }
            this.CurrentSession.StartBusyIndicatorLoading();
            this.myDomainService.GetTimeOfficeClock(this.EmployeeUserId, this.FromDate, this.ToDate).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                _this.ItemSource = [];
                _this.ItemSourceCollection.Clear();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myCollection = [];
                    var index = 0;
                    myResponse.Result.sort(function (a, b) { return ((a.WorkDate === b.WorkDate) ? ((a.EntryTime === b.EntryTime) ? 0 : (a.EntryTime < b.EntryTime) ? -1 : 1) : (a.WorkDate < b.WorkDate ? -1 : 1)); });
                    myResponse.Result.forEach(function (item) {
                        index += 1;
                        _this.ItemSource.push(new ItemSourceItem(item, _this, index));
                    });
                    _this.ItemSourceCollection.InsertCollection(_this.ItemSource);
                }
                _this.ComputeTotals();
            });
        }
    };
    ClockTimeComponent.prototype.SaveSingleTimeOfficeHourRecord = function (item) {
        var _this = this;
        var items = [];
        if (item.IsDirty) {
            items.push(item);
        }
        if (items.length != 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myDomainService.UpdateOfficeHourList(items).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    item.IsDirty = false;
                }
                _this.ComputeTotals();
            });
        }
    };
    ClockTimeComponent.prototype.SaveTimeOfficeHour = function () {
        var _this = this;
        var items = [];
        this.ItemSourceCollection.Collection.filter(function (p) { return p.IsDirty; }).forEach(function (p) {
            items.push(p.entity);
        });
        if (items.length != 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myDomainService.UpdateOfficeHourList(items).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.ItemSourceCollection.Collection.forEach(function (p) {
                        p.EntityPM.IsDirty = false;
                    });
                }
                _this.ComputeTotals();
            });
        }
    };
    ClockTimeComponent.prototype.InitTab = function (arg) {
        this.employeeUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        var fromDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        fromDate.setDate(1);
        this.fromDate = fromDate;
        var toDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        toDate.setMonth(toDate.getUTCMonth() + 1);
        toDate.setDate(1);
        this.toDate = toDate;
        this.myDomainService = new TimeOfficeHourDomainService_1.TimeOfficeHourDomainService();
        this.LoadClockTimeSheet();
    };
    ClockTimeComponent.prototype.SearchButtonClicked = function () {
        if (this.ItemSourceCollection.Collection.filter(function (p) { return p.IsDirty; }).length > 0) {
            this.SaveTimeOfficeHour();
        }
        else {
            this.LoadClockTimeSheet();
        }
    };
    ClockTimeComponent.prototype.RefreshTab = function () {
    };
    ClockTimeComponent.prototype.PrintPreviewClicked = function () {
    };
    ClockTimeComponent.prototype.ComputeTotals = function () {
        this.TotalMinutes = Tools_1.ArrayTool.Sum(this.ItemSourceCollection.Collection.filter(function (f) { return f.Inactive == false; }), "Minutes");
    };
    ClockTimeComponent = __decorate([
        core_1.Component({
            selector: 'ClockTimeComponent',
            moduleId: module.id,
            templateUrl: './ClockTimeComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ClockTimeComponent);
    return ClockTimeComponent;
}(BaseComponent_1.BaseComponent));
exports.ClockTimeComponent = ClockTimeComponent;
var ItemSourceItem = /** @class */ (function (_super) {
    __extends(ItemSourceItem, _super);
    function ItemSourceItem(entity, father, index) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.father = father;
        _this.DataContext = _this;
        _this.IsCopy = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = entity;
        _this.Index = index;
        var idIndex = _this.CurrentSession.GetNewId("DIV");
        _this.DivId = "DIV_" + idIndex;
        return _this;
    }
    Object.defineProperty(ItemSourceItem.prototype, "Id", {
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "IsDirty", {
        get: function () { return this.EntityPM.IsDirty; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "WorkDate", {
        get: function () { return this.EntityPM.WorkDate; },
        set: function (value) {
            if (this.EntityPM.WorkDate != value) {
                this.EntityPM.WorkDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "EntryTime", {
        get: function () { return this.EntityPM.EntryTime; },
        set: function (value) {
            if (this.EntityPM.EntryTime != value) {
                var iResult = null;
                if (value) {
                    var iDateParts = Tools_1.DateTool.GetDateParts(value);
                    iResult = Tools_1.DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;
                    iResult.setUTCHours(iDateParts.Hours);
                    iResult.setUTCMinutes(iDateParts.Minutes);
                    if (this.ExitTime) {
                        if (Tools_1.DateTool.GetDateParts(iResult).DateTicks > Tools_1.DateTool.GetDateParts(this.ExitTime).DateTicks) {
                            iResult = this.EntryTime;
                        }
                    }
                }
                this.EntityPM.EntryTime = iResult;
                this.ComputeMinutes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "ExitTime", {
        get: function () { return this.EntityPM.ExitTime; },
        set: function (value) {
            if (this.EntityPM.ExitTime != value) {
                var iResult = null;
                if (value) {
                    var iDateParts = Tools_1.DateTool.GetDateParts(value);
                    iResult = Tools_1.DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;
                    iResult.setUTCHours(iDateParts.Hours);
                    iResult.setUTCMinutes(iDateParts.Minutes);
                    if (this.EntryTime) {
                        if (Tools_1.DateTool.GetDateParts(iResult).DateTicks < Tools_1.DateTool.GetDateParts(this.EntryTime).DateTicks) {
                            iResult = this.ExitTime;
                        }
                    }
                }
                this.EntityPM.ExitTime = iResult;
                this.ComputeMinutes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Minutes", {
        get: function () { return this.EntityPM.Minutes; },
        set: function (value) {
            if (this.EntityPM.Minutes != value) {
                this.EntityPM.Minutes = value;
                this.father.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "UpdatedByUserId", {
        get: function () { return this.EntityPM.UpdatedByUserId; },
        set: function (value) {
            if (this.EntityPM.UpdatedByUserId != value) {
                this.EntityPM.UpdatedByUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "UpdatedByUserName", {
        get: function () { return this.EntityPM.UpdatedByUserName; },
        set: function (value) {
            if (this.EntityPM.UpdatedByUserName != value) {
                this.EntityPM.UpdatedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "Inactive", {
        get: function () { return this.EntityPM.Inactive; },
        set: function (value) {
            if (this.EntityPM.Inactive != value) {
                this.EntityPM.Inactive = value;
                this.father.ComputeTotals();
                this.SaveSingleLine();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "RecordedEntryAddedManually", {
        get: function () {
            if (this.EntryTime != this.EntityPM.RecordedEntryTime || this.EntityPM.RecordedEntryTime == null)
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "RecordedExitAddedManually", {
        get: function () {
            if (this.ExitTime != this.EntityPM.RecordedExitTime || this.EntityPM.RecordedExitTime == null)
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "EditedManualyEntryTime", {
        get: function () {
            if (this.EntryTime != this.EntityPM.RecordedEntryTime && this.EntityPM.RecordedEntryTime != null) {
                var timeRecorded = "";
                var RecordedDate = Tools_1.DateTool.GetDateParts(this.EntityPM.RecordedEntryTime).DateObject;
                if (RecordedDate != null) {
                    timeRecorded = RecordedDate.getUTCHours() + ":" + (RecordedDate.getUTCMinutes() >= 10 ? RecordedDate.getUTCMinutes() : "0" + RecordedDate.getUTCMinutes());
                }
                var timeEntry = "";
                var RecordedEntry = Tools_1.DateTool.GetDateParts(this.EntryTime).DateObject;
                if (RecordedEntry != null) {
                    timeEntry = RecordedEntry.getUTCHours() + ":" + (RecordedEntry.getUTCMinutes() >= 10 ? RecordedEntry.getUTCMinutes() : "0" + RecordedEntry.getUTCMinutes());
                }
                return "The value edited by " + this.UpdatedByUserName + " from " + timeRecorded + " to " + timeEntry;
            }
            else if (this.EntityPM.RecordedEntryTime == null) {
                return "The value added manually by " + this.UpdatedByUserName;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemSourceItem.prototype, "EditedManualyExitTime", {
        get: function () {
            if (this.ExitTime != this.EntityPM.RecordedExitTime && this.EntityPM.RecordedExitTime != null) {
                var timeRecorded = "";
                var RecordedDate = Tools_1.DateTool.GetDateParts(this.EntityPM.RecordedExitTime).DateObject;
                if (RecordedDate != null) {
                    timeRecorded = RecordedDate.getUTCHours() + ":" + (RecordedDate.getUTCMinutes() >= 10 ? RecordedDate.getUTCMinutes() : "0" + RecordedDate.getUTCMinutes());
                }
                var timeExit = "";
                var RecordedExit = Tools_1.DateTool.GetDateParts(this.ExitTime).DateObject;
                if (RecordedExit != null) {
                    timeExit = RecordedExit.getUTCHours() + ":" + (RecordedExit.getUTCMinutes() >= 10 ? RecordedExit.getUTCMinutes() : "0" + RecordedExit.getUTCMinutes());
                }
                return "The value edited by " + this.UpdatedByUserName + " from " + timeRecorded + " to " + timeExit;
            }
            else if (this.EntityPM.RecordedExitTime == null) {
                return "The value added manually by " + this.UpdatedByUserName;
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemSourceItem.prototype.ComputeMinutes = function () {
        var iResult = 0;
        if (this.EntryTime && this.ExitTime) {
            var ExitTimeTotalMinutes = Tools_1.DateTool.GetDateParts(this.ExitTime).TotalMinutes;
            var EntryTimeTotalMinutes = Tools_1.DateTool.GetDateParts(this.EntryTime).TotalMinutes;
            iResult = ExitTimeTotalMinutes - EntryTimeTotalMinutes;
        }
        this.Minutes = iResult;
        this.SaveSingleLine();
    };
    ItemSourceItem.prototype.SaveSingleLine = function () {
        this.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        this.father.SaveSingleTimeOfficeHourRecord(this.EntityPM);
    };
    return ItemSourceItem;
}(BaseComponent_1.BaseComponent));
exports.ItemSourceItem = ItemSourceItem;
//# sourceMappingURL=ClockTimeComponent.js.map