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
var Tools_1 = require("../../Tools");
var LogCalendarComponent = /** @class */ (function () {
    function LogCalendarComponent() {
        this.monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"];
        this.dayNames = ["Sunday", "Monday", "Tuseday", "Wednesday", "Thursday", "Friday", "Saturday"];
        this.shortDayNames = ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"];
        this.SelectedDateChanged = new core_1.EventEmitter();
        this.daysOfMonth = new Array(42);
        this.weekRows = new Array(6);
        this.TodayDate = new Date();
    }
    Object.defineProperty(LogCalendarComponent.prototype, "SelectedDate", {
        get: function () { return this.selectedDate; },
        set: function (value) {
            if (typeof (value) == "string") {
                value = Tools_1.DateTool.GetDateParts(value).DateObject;
            }
            if (this.selectedDate != value) {
                this.selectedDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCalendarComponent.prototype, "ToggleOpened", {
        get: function () { return this.toggleOpened; },
        set: function (newValue) {
            this.toggleOpened = newValue;
            if (newValue) {
                if (!this.SelectedDate) {
                    this.SelectedDate = this.GetTodaysDate();
                    this.CalendarDate = new Date;
                }
                else {
                    if (typeof (this.SelectedDate) == "string") {
                        this.SelectedDate = Tools_1.DateTool.GetDateParts(this.SelectedDate).DateObject;
                    }
                    this.CalendarDate = this.SelectedDate;
                }
                this.InitializeCalendar();
            }
        },
        enumerable: true,
        configurable: true
    });
    LogCalendarComponent.prototype.ngOnInit = function () {
        if (!this.SelectedDate) {
            this.SelectedDate = this.GetTodaysDate();
            this.CalendarDate = new Date;
        }
        else {
            this.CalendarDate = this.SelectedDate;
        }
        this.InitializeCalendar();
    };
    LogCalendarComponent.prototype.InitializeCalendar = function () {
        this.SelectedMonthYear = this.monthNames[this.CalendarDate.getMonth()] + ' ' + this.CalendarDate.getFullYear();
        var today = new Date().getDate();
        var thisMonth = new Date().getMonth();
        var thisYear = new Date().getFullYear();
        var calendarDay = this.CalendarDate.getDate();
        var calendarYear = this.CalendarDate.getFullYear();
        var calendarMonth = this.CalendarDate.getMonth();
        var endDayNumber = new Date(this.CalendarDate.getFullYear(), this.CalendarDate.getMonth() + 1, 0).getDate();
        var firstDayWeekDayNum = new Date(this.CalendarDate.getFullYear(), this.CalendarDate.getMonth(), 1).getDay();
        var selectedDay = this.SelectedDate.getDate();
        var selectedYear = this.SelectedDate.getFullYear();
        var selectedMonth = this.SelectedDate.getMonth();
        for (var i = 0; i < 6; i++) {
            this.weekRows[i] = new Array();
        }
        var days = 1;
        for (var i = 0; i < 42; i++) {
            var strDay = new DayOfMonth();
            if (firstDayWeekDayNum <= i) {
                if (days <= endDayNumber) {
                    this.daysOfMonth.push(days.toString());
                    strDay.Day = days;
                    strDay.month = calendarMonth;
                    strDay.year = calendarYear;
                    if (days == today && calendarMonth == thisMonth && calendarYear == thisYear) {
                        strDay.IsToday = true;
                        strDay.ColorSelectedStyle = { 'background': '#fffa90' };
                    }
                    if (days == selectedDay && calendarMonth == selectedMonth && calendarYear == selectedYear && !strDay.IsToday) {
                        strDay.IsSelected = true;
                        strDay.ColorSelectedStyle = { 'background': '#3169cf' };
                    }
                    days++;
                }
                else {
                    strDay = null;
                }
            }
            else {
                strDay = null;
                this.daysOfMonth.push(' ');
            }
            if (i >= 0 && i <= 6) {
                this.weekRows[0].push(strDay);
            }
            if (i >= 7 && i <= 13) {
                this.weekRows[1].push(strDay);
            }
            if (i >= 14 && i <= 20) {
                this.weekRows[2].push(strDay);
            }
            if (i >= 21 && i <= 27) {
                this.weekRows[3].push(strDay);
            }
            if (i >= 28 && i <= 34) {
                this.weekRows[4].push(strDay);
            }
            if (i >= 35 && i <= 41) {
                if (strDay != null) {
                    this.weekRows[5].push(strDay);
                }
            }
        }
        if (this.weekRows[5].length == 0) {
            this.weekRows.pop();
        }
    };
    LogCalendarComponent.prototype.Previous = function () {
        var month = this.CalendarDate.getMonth() - 1;
        this.CalendarDate = new Date(this.CalendarDate.getFullYear(), month, 1);
        this.InitializeCalendar();
    };
    LogCalendarComponent.prototype.Next = function () {
        var month = this.CalendarDate.getMonth() + 1;
        this.CalendarDate = new Date(this.CalendarDate.getFullYear(), month, 1);
        this.InitializeCalendar();
    };
    LogCalendarComponent.prototype.OnDayClick = function (day) {
        var selectedHour = 0;
        var selectedMinute = 0;
        var selectedSecond = 0;
        if (this.SelectedDate) {
            selectedHour = this.SelectedDate.getUTCHours();
            selectedMinute = this.SelectedDate.getUTCMinutes();
            selectedSecond = this.SelectedDate.getUTCSeconds();
        }
        this.SelectedDate = this.GetDate(day.year, day.month, day.Day, selectedHour, selectedMinute, selectedSecond);
        this.SelectedDateChanged.emit({ SelectedDate: this.SelectedDate, Suffix: null });
    };
    LogCalendarComponent.prototype.TodayButtonClicked = function () {
        this.SelectedDate = this.GetTodaysDate(); //DateTool.GetCurrentDateAsUtc(); //DateTool.GetCurrentDateTimeAsUtc();
        this.SelectedDateChanged.emit({ SelectedDate: this.SelectedDate, Suffix: null });
    };
    LogCalendarComponent.prototype.GetDate = function (year, month, day, hour, minute, second) {
        var date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    };
    LogCalendarComponent.prototype.GetTodaysDate = function () {
        var today = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(0);
        today.setUTCMinutes(0);
        today.setUTCSeconds(0);
        return today;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogCalendarComponent.prototype, "SelectedDateChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean),
        __metadata("design:paramtypes", [Boolean])
    ], LogCalendarComponent.prototype, "ToggleOpened", null);
    LogCalendarComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogCalendar',
            templateUrl: './LogCalendarComponent.html',
            inputs: ['SelectedDate'],
        }),
        __metadata("design:paramtypes", [])
    ], LogCalendarComponent);
    return LogCalendarComponent;
}());
exports.LogCalendarComponent = LogCalendarComponent;
var DayOfMonth = /** @class */ (function () {
    function DayOfMonth() {
    }
    return DayOfMonth;
}());
exports.DayOfMonth = DayOfMonth;
//# sourceMappingURL=LogCalendarComponent.js.map