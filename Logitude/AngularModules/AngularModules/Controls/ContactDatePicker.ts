import {Component, OnInit, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'ContactDatePicker',
    moduleId: module.id,
    templateUrl: './ContactDatePicker.html',
    inputs:['Date'],
})

export class ContactDatePicker implements OnInit {
    public Days: number[];
    public Months: number[];
    public Years: number[];
    @Output() DateChanged: EventEmitter<Date> = new EventEmitter<Date>();
    constructor() {
        this.Days = [];
        this.Months = [];
        this.Years = [];

        for (var i = 1; i <= 12; i++)
        {
            this.Months.push(i);
        }

        for (var i = new Date().getFullYear(); i >= 1950; i--)
        {
            this.Years.push(i);
        }
    }

    ngOnInit() {
        if (this.Date != null) {
            var myDate: Date = new Date(this.Date.toString());

            this.year = myDate.getFullYear();
            this.month = myDate.getMonth() + 1;
            this.FillDays();
            this.day = myDate.getDate();
        }

        else {
            this.FillDays();
        }
    }

    private date: Date = null;
    get Date() { return this.date; }
    set Date(newValue: Date) {
        if (this.date != newValue) {
            this.date = newValue;
            this.DateChanged.emit(this.date);

            if (!this.date) {
                this.year = null;
                this.month = null;
                this.day = null;
            }

        }
    }

    private day: number = null;
    get Day() { return this.day; }
    set Day(newValue: number) {
        if (this.day != newValue) {
            this.day = newValue;
            this.BuildDateObject();
        }
    }

    private month: number = null;
    get Month() { return this.month; }
    set Month(newValue: number) {
        if (this.month != newValue) {
            this.month = newValue;
            this.FillDays();
            this.BuildDateObject();
        }
    }

    private year: number = null;
    get Year() { return this.year; }
    set Year(newValue: number) {
        if (this.year != newValue) {
            this.year = newValue;
            this.FillDays();
            this.BuildDateObject();
        }
    }

    private FillDays() {
        this.Days = [];
        var myMonth = this.Month == null ? new Date().getMonth() + 1 : this.Month;
        var myYear = this.Year == null ? new Date().getFullYear() : this.Year;
        var days = new Date(myYear, myMonth, 0).getDate();
        for (var i = 1; i <= days; i++) {
            this.Days.push(i);
        }
    }
    private BuildDateObject() {
        if (this.Day == null || this.Month == null || this.Year == null) {
            this.Date = null;
        }

        else {            
            this.Date = new Date();            
            this.Date.setUTCFullYear(this.Year);
            this.Date.setUTCMonth(this.Month - 1);
            this.Date.setUTCDate(this.Day);
            this.Date.setUTCHours(0);
            this.Date.setUTCMinutes(0);
            this.Date.setUTCSeconds(0);
            this.Date.setUTCMilliseconds(0);
        }
    }
}