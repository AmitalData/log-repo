import { Component, OnInit, Output, EventEmitter, Input} from '@angular/core';
import {AppTool, DateTool} from '../../Tools';

@Component({
    
    selector: 'LogCalendar',
    templateUrl: './LogCalendarComponent.html',
    inputs: ['SelectedDate','UseTimezoneOffSetHours'],
})

export class LogCalendarComponent implements OnInit {
    public SelectedMonthYear: string;
    monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
    dayNames = ["Sunday", "Monday", "Tuseday", "Wednesday", "Thursday", "Friday", "Saturday"];
    shortDayNames = ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"];
    daysOfMonth: string[];
    weekRows: Array<DayOfMonth[]>;
    @Output() SelectedDateChanged = new EventEmitter();

    CalendarDate: Date;
    TodayDate: Date;
    public UseTimezoneOffSetHours: boolean;

    private selectedDate: Date;
    public get SelectedDate() { return this.selectedDate; }
    public set SelectedDate(value: Date) {

        if (typeof (value) == "string") {
            value = DateTool.GetDateParts(value).DateObject;
        }

        if (this.selectedDate != value) {
            this.selectedDate = value;
        }
    }

    private toggleOpened: boolean;
    @Input() get ToggleOpened() { return this.toggleOpened; }
     set ToggleOpened(newValue: boolean) {
         this.toggleOpened = newValue;
         if (newValue) {

             if (!this.SelectedDate) {
                 this.SelectedDate = this.GetTodaysDate();
                 this.CalendarDate = new Date;
             }

             else {

                 if (typeof (this.SelectedDate) == "string") {
                     this.SelectedDate = DateTool.GetDateParts(this.SelectedDate).DateObject;
                 }

                 this.CalendarDate = this.SelectedDate;
             }

             this.InitializeCalendar();
         }
    }


    constructor() {
        this.daysOfMonth = new Array(42);
        this.weekRows = new Array(6);
        this.TodayDate = new Date();
    }

    ngOnInit() {
        if (!this.SelectedDate) {
            this.SelectedDate = this.GetTodaysDate();
            this.CalendarDate = new Date;
        }
        else {
            this.CalendarDate = this.SelectedDate;
        }
        this.InitializeCalendar();
    }

    public InitializeCalendar() {

        this.SelectedMonthYear = this.monthNames[this.CalendarDate.getMonth()] + ' ' + this.CalendarDate.getFullYear();
        var today = new Date().getDate();
        var thisMonth = new Date().getMonth();
        var thisYear = new Date().getFullYear();

        var calendarDay = this.CalendarDate.getDate();
        var calendarYear = this.CalendarDate.getFullYear();
        var calendarMonth: number = this.CalendarDate.getMonth();
        var endDayNumber: number = new Date(this.CalendarDate.getFullYear(), this.CalendarDate.getMonth() + 1, 0).getDate();
        var firstDayWeekDayNum: number = new Date(this.CalendarDate.getFullYear(), this.CalendarDate.getMonth(), 1).getDay();

        var selectedDay = this.SelectedDate.getDate();
        var selectedYear = this.SelectedDate.getFullYear();
        var selectedMonth: number = this.SelectedDate.getMonth();


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
    }

    public Previous() {
        var month = this.CalendarDate.getMonth() - 1;
        this.CalendarDate = new Date(this.CalendarDate.getFullYear(), month, 1);
        this.InitializeCalendar();
    }

    public Next() {
        var month = this.CalendarDate.getMonth() + 1;
        this.CalendarDate = new Date(this.CalendarDate.getFullYear(), month, 1);
        this.InitializeCalendar();
    }

    public OnDayClick(day: DayOfMonth) {

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
    }

    TodayButtonClicked() {
        this.SelectedDate = this.GetTodaysDate(); //DateTool.GetCurrentDateAsUtc(); //DateTool.GetCurrentDateTimeAsUtc();
        this.SelectedDateChanged.emit({ SelectedDate: this.SelectedDate, Suffix: null });
    }

    GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
        var date: Date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours((this.UseTimezoneOffSetHours && this.GetTimezoneOffsetHours() > 0 ? this.GetTimezoneOffsetHours() : hour));
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    }

    GetTodaysDate() {
        var today: Date = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours((this.UseTimezoneOffSetHours && this.GetTimezoneOffsetHours() > 0 ? this.GetTimezoneOffsetHours() : 0));
        today.setUTCMinutes(0);
        today.setUTCSeconds(0);

        return today;
    }


    GetTimezoneOffsetHours() {
        let timezoneOffsetHours = new Date().getTimezoneOffset() / 60;
        return timezoneOffsetHours;
    }
}

export class DayOfMonth {
    public Day: number;
    public month: number;
    public year: number;
    public IsSelected: boolean;
    public IsToday: boolean;
    public ColorSelectedStyle: any;
}
