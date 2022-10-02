import { Component, OnInit, Input, Output, EventEmitter, HostListener, ElementRef } from '@angular/core';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import * as moment from 'moment';
import { FieldValueResolver } from 'Infrastructure/Utilities/FieldValueResolver';

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
    DateText: string;
    HideCalendar: boolean;
    Quarters: string[] = ['Q1', 'Q2', 'Q3', 'Q4'];
    IsQuarterCalendar: boolean;

    @Output() OnChange = new EventEmitter();
    @Input() SelectedDate: Date;
    @Input() SelectedQuarter: string;

    pickerType: string;
    get PickerType(): string {
        return this.pickerType;
    }
    @Input() set PickerType(value: string) {
        if (value == this.pickerType) return;
        this.pickerType = value;
        this.ManagePickerType();
        this.DateText = null;
        this.model = new MonthPickerModel();
    }

    constructor(private eRef: ElementRef) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator.GlobalSetting) this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
        this.DatePickerInputId = this.MakeRandomid();
        this.DropDownId = this.MakeRandomid();
    }

    ngOnInit(): void {
        moment.locale('en');
        this.SetDefaultDate();
        this.ManagePickerType();
    }

    @HostListener('document:click', ['$event'])
    clickout(event) {
        if (this.eRef.nativeElement != event.target && !this.eRef.nativeElement.contains(event.target)) {
            this.IsDateDropDownOpen = false;
        }
    }

    ManagePickerType() {
        if (!this.PickerType) this.PickerType = "Month";
        if (this.PickerType == "Month") this.ManageMonthPickerType();
        else if (this.PickerType == "Year") this.ManageYearPickerType();
        else if (this.PickerType == "Quarter") this.ManageQuarterPickerType();
    }

    ManageMonthPickerType() {
        this.HideCalendar = false;
        this.IsQuarterCalendar = false;
    }

    ManageQuarterPickerType() {
        this.HideCalendar = true;
        this.IsQuarterCalendar = true;
    }

    ManageYearPickerType() {
        this.HideCalendar = true;
        this.IsQuarterCalendar = false;
    }


    private SetDefaultDate() {
        this.model = new MonthPickerModel();
        if (!this.SelectedDate) {
            if (this.PickerType == "Quarter" && !this.SelectedQuarter) this.SelectedQuarter = "Q1";
            return;
        }
        this.model.selectedYearMoment = moment(this.SelectedDate);
        this.model.selectedMonthMoment = moment(this.SelectedDate);
        this.model.selectedMonthIndex = this.model.selectedMonthMoment.month();
        this.model.selectedMonthYear = this.model.selectedYearMoment.year();
        this.model.UpdateYearText();
        this.SetDateText();
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

    SelectQuarter(quarter: string) {
        this.SelectedQuarter = quarter;
        this.SubmitChanges();
    }

    SelectMonth(index: number) {
        this.model.SelectMonth(index);
        this.SubmitChanges();
    }

    Selectyear() {
        if (this.PickerType != "Year") return;
        this.SubmitChanges();
    }

    ThisMonthButtonClicked() {
        this.model = new MonthPickerModel();
        this.SubmitChanges();
    }

    SubmitChanges() {
        this.IsDateDropDownOpen = false;
        var dateString = this.SetDateText();
        const [day, month, year] = dateString.split('/');
        var date = FieldValueResolver.GetDate(Number(year), Number(month) - 1, Number(day), 0, 0, 0);
        this.OnChange.emit({ date: date, quarter: this.SelectedQuarter });
    }

    private SetDateText() {
        var dateString = "";
        switch (this.PickerType) {
            case "Month":
                this.DateText = (("0" + (this.model.selectedMonthIndex + 1)).slice(-2)) + "/" + this.model.selectedYearText;
                dateString = "01/" + this.DateText;
                break;
            case "Year":
                this.DateText = this.model.selectedYearText;
                dateString = "01/01/" + this.model.selectedYearText;
                break;
            case "Quarter":
                this.DateText = this.SelectedQuarter + " - " + this.model.selectedYearText;
                var startQuarterMonth = this.GetStartQuarterMonth();
                dateString = `01/${startQuarterMonth}/${this.model.selectedYearText}`;
                break;
            default:
                break;
        }
        return dateString;
    }
    private GetStartQuarterMonth() {
        var quarter = +this.SelectedQuarter[1];
        return quarter * 3 - 2;
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