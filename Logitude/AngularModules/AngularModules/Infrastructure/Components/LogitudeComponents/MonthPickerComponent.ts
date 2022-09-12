import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import * as moment from 'moment';

@Component({
    selector: 'monthpicker',
    templateUrl: './MonthpickerComponent.html'
})
export class MonthpickerComponent implements OnInit {
    LayoutDirection: string = "ltr";
    isRTL: boolean = false;
    IsDateDropDownOpen: boolean = false;
    DropDownId: string;
    DatePickerInputId: string;
    model: MonthPickerModel;
    Date: string;

    @Output() OnChange = new EventEmitter();
    @Input() SelectedDate: string;

    constructor(private cd: ChangeDetectorRef) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator.GlobalSetting) this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
        this.DatePickerInputId = this.MakeRandomid();
        this.DropDownId = this.MakeRandomid();
    }

    ngOnInit(): void {
        moment.locale('en');
        this.SetDefaultDate();

    }


    private SetDefaultDate() {
        this.model = new MonthPickerModel();
        if (!this.SelectedDate) {
            this.SubmitChanges();
            return;
        }

        this.model.selectedYearMoment = moment(this.SelectedDate, 'MM/YYYY');
        this.model.selectedMonthMoment = moment(this.SelectedDate, 'MM/YYYY');
        this.model.selectedMonthIndex = this.model.selectedMonthMoment.month();
        this.model.selectedMonthYear = this.model.selectedYearMoment.year();
        this.SubmitChanges();
    }

    ToggleCalendar() {
        this.IsDateDropDownOpen = !this.IsDateDropDownOpen;
    }

    MakeRandomid() {
        let possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,./;'[]\=-)(*&^%$#@!~`";
        let text = "";
        for (let i = 0; i < 10; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }
        return text;
    }

    isSelectedMonth(monthIndex: number) {
        return this.model.selectedMonthIndex == monthIndex && this.model.selectedMonthYear == this.model.selectedYearMoment.year();
    }

    Previous() {
        this.model.DecrementYear();
    }


    Next() {
        this.model.IncrementYear();
    }


    SelectMonth(index: number) {
        this.model.SelectMonth(index);
        this.SubmitChanges();
    }

    ThisMonthButtonClicked() {
        this.model = new MonthPickerModel();
        this.SubmitChanges();
    }

    SubmitChanges() {
        this.IsDateDropDownOpen = false;
        this.Date = (("0" + (this.model.selectedMonthIndex + 1)).slice(-2)) + "/" + this.model.selectedMonthYear;
        this.OnChange.emit(this.Date);
    }
}

export class MonthPickerModel {
    selectedYearMoment: moment.Moment;
    selectedYearText: string;
    selectedMonthMoment: moment.Moment;
    selectedMonthIndex: number;
    selectedMonthYear: number;
    months: Array<string> = [];

    constructor() {
        this.selectedYearMoment = moment();
        this.UpdateYearText();
        this.selectedMonthMoment = moment();

        this.months = moment.months('MMM');
        for (let index = 0; index < this.months.length; index++) {
            this.months[index] = this.months[index].substring(0, 3);
        }
        this.selectedMonthIndex = this.selectedMonthMoment.month();
        this.selectedMonthYear = this.selectedYearMoment.year();
    }

    UpdateYearText() {
        this.selectedYearText = moment(this.selectedYearMoment).format('YYYY');
    }

    SelectMonth(index: number) {
        this.selectedMonthMoment = moment().month(index);
        this.selectedMonthIndex = this.selectedMonthMoment.month();
        this.selectedMonthYear = this.selectedYearMoment.year();
    }

    IncrementYear() {
        this.selectedYearMoment = this.selectedYearMoment.add(1, "year")
        this.UpdateYearText();
    }

    DecrementYear() {
        this.selectedYearMoment = this.selectedYearMoment.subtract(1, "year")
        this.UpdateYearText();
    }
}