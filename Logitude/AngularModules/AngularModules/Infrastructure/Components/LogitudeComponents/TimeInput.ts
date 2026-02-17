import {Component, Output, EventEmitter, ChangeDetectionStrategy, Input} from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool, ImageTool} from  '../../Tools';
import {BaseComponent} from '../../Components/LogitudeComponents/BaseComponent';

@Component({
    selector: "TimeInput",
    inputs: ["TextAlign", "IsShowText", "IsNew", "Disabled", "TotalMinutes", "FocusOnMe"],
    moduleId: module.id,
    templateUrl: './TimeInput.html',
})

export class TimeInput extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMEmployeeTime";
    public ControlId: string = null;
    public IsShowText = true;
    public IsNew = false;
    public FormattedMinutes: string = "";
    public Disabled = false;
    public TextAlign: string = "center";
    public TotalMinutes; number;
    public FocusOnMe: boolean = false;
    @Output() Changed: EventEmitter<string> = new EventEmitter<string>();
    @Output() DisplayChanged: EventEmitter<any> = new EventEmitter<any>();
    @Output() TotalMinutesChanged: EventEmitter<any> = new EventEmitter<any>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        var idIndex = this.CurrentSession.GetNewId("TimeInput");
        this.ControlId = "TimeInputControl_" + idIndex;
    }

    private inputValue: string = "";
    @Input() public get InputValue() {
        return this.inputValue;
    }
    public set InputValue(value: string) {
        if (this.inputValue != value) {
            this.inputValue = value;
            this.Changed.emit(value);
            this.TotalMinutes = this.GetTotalMinutesFromString(value);
            this.TotalMinutesChanged.emit(this.TotalMinutes);
        }
    }
    GetTotalMinutesFromString(value: string) {
        var minutes = 0;
        if (value) {
            if (value.indexOf(':') != -1) {
                var list = value.split(':');
                if (list != null && list.length > 0) {
                    var h = list[0];
                    var m = list[1];
                    minutes = +m + (+h * 60);
                }
            }
            else if (value.indexOf('.') != -1) {
                var list = value.split('.');
                if (list != null && list.length > 0) {
                    var h = list[0];
                    var m = list[1];
                    minutes = +m + (+h * 60);
                }
            }
            else {
                minutes = +value;
            }
        }
        return minutes;
    }

    private displayValue: number;
    @Input() public get DisplayValue() {
        return this.displayValue;
    }
    public set DisplayValue(value: any) {
        if (this.displayValue != value) {
            this.displayValue = value;
            this.DisplayChanged.emit(value);
        }
    }

    GetTimeValue() {
        var errorMessage = null;
        if (this.InputValue) {
            var date: Date;

            date = this.GetTodaysDate();
            var dateparts = this.GetDateParts(date);
            var day: number;
            var month: number;
            var year: number;

            day = dateparts[2];
            month = dateparts[1] + 1;
            year = dateparts[0];

            var invalidText: boolean = false;

            if ((this.InputValue != "." && this.InputValue.indexOf(".") > -1) || (this.InputValue != "/" && this.InputValue.indexOf("/") > -1)
                || (this.InputValue != "-" && this.InputValue.indexOf("-") > -1) || (this.InputValue != ":" && this.InputValue.indexOf(":") > -1)) {

                var dateStrings: string[];

                if (this.InputValue.indexOf(".") > -1) {
                    dateStrings = this.InputValue.split('.');
                }
                else if (this.InputValue.indexOf("-") > -1) {
                    dateStrings = this.InputValue.split('-');
                }
                else if (this.InputValue.indexOf(":") > -1) {
                    dateStrings = this.InputValue.split(':');
                }
                else {
                    dateStrings = this.InputValue.split('/');
                }

                var hour;
                var minute;
                var second;

                hour = 0;
                minute = 0;
                second = 0;

                if (dateStrings.length == 3) {
                    hour = Number(dateStrings[0]);
                    minute = Number(dateStrings[1]);
                    second = Number(dateStrings[2]);
                }

                else if (dateStrings.length == 2) {
                    hour = Number(dateStrings[0]);
                    minute = Number(dateStrings[1]);
                }

                else if (dateStrings.length == 1) {
                    hour = Number(dateStrings[0]);
                }

                if (hour > 23 || minute > 59 || second > 59) {
                    invalidText = true;
                }

                if (!invalidText) {
                    var dateparts = this.GetDateParts(date);
                    var datetime: Date = this.GetDate(dateparts[0], dateparts[1], dateparts[2], hour, minute, second);
                    this.SetDateValue(datetime);
                }
            }

            else {
                var valid = Number(this.InputValue);
                if (valid) {
                    var hour;
                    var minute;
                    var second;
                    var hourString;
                    var minuteString;
                    var secondString;

                    if (this.InputValue.length == 6) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = this.InputValue.substring(2, 4);
                        secondString = this.InputValue.substring(4, 6);
                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);

                        if (hour > 23 || minute > 59 || second > 59) {
                            invalidText = true;
                        }

                        if (!invalidText) {
                            var datetime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }

                    else if (this.InputValue.length == 5) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = this.InputValue.substring(2, 4);
                        secondString = this.InputValue.substring(4, 5);
                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);
                        if (hour > 23) {
                            hourString = this.InputValue.substring(0, 1);
                            minuteString = this.InputValue.substring(1, 3);
                            secondString = this.InputValue.substring(3, 5);
                        }

                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);

                        if (minute > 59 || second > 59) {
                            invalidText = true;
                        }

                        if (!invalidText) {
                            var datetime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (this.InputValue.length == 4) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = this.InputValue.substring(2, 4);
                        secondString = '0';
                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);

                        var hourDigitCount = 2;
                        if (hour > 23) {
                            hourDigitCount = 1;
                            hourString = this.InputValue.substring(0, 1);
                            minuteString = this.InputValue.substring(1, 3);
                            secondString = this.InputValue.substring(3, 4);
                            hour = Number(hourString);
                            minute = Number(minuteString);
                            second = Number(secondString);
                        }

                        if (minute > 59) {
                            if (hourDigitCount == 1) {
                                minuteString = this.InputValue.substring(1, 2);
                                secondString = this.InputValue.substring(2, 4);
                            }
                            else {
                                minuteString = this.InputValue.substring(2, 3);
                                secondString = this.InputValue.substring(3, 4);
                            }
                        }

                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);

                        if (second > 59) {
                            invalidText = true;
                        }

                        if (!invalidText) {
                            var datetime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (this.InputValue.length == 3) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = this.InputValue.substring(2, 3);
                        secondString = '0';

                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);

                        var hourDigitCount = 2;
                        if (hour > 23) {
                            hourDigitCount = 1;
                            hourString = this.InputValue.substring(0, 1);
                            minuteString = this.InputValue.substring(1, 3);
                        }

                        if (minute > 59) {
                            minuteString = this.InputValue.substring(1, 2);
                            secondString = this.InputValue.substring(2, 3);
                        }

                        hour = Number(hourString);
                        minute = Number(minuteString);
                        second = Number(secondString);

                        if (!invalidText) {
                            var datetime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (this.InputValue.length == 2) {

                        hourString = this.InputValue.substring(0, 2);
                        minuteString = '0';
                        hour = Number(hourString);
                        minute = Number(minuteString);
                        var hourDigitCount = 2;
                        if (hour > 23) {
                            hourDigitCount = 1;
                            hourString = this.InputValue.substring(0, 1);
                            minuteString = this.InputValue.substring(1, 2);
                        }
                        hour = Number(hourString);
                        minute = Number(minuteString);
                        if (!invalidText) {
                            var datetime: Date = this.GetDate(year, month - 1, day, hour, minute, 0);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (this.InputValue.length == 1) {

                        hourString = this.InputValue;
                        hour = Number(hourString);

                        if (!invalidText) {
                            var datetime: Date = this.GetDate(year, month - 1, day, hour, 0, 0);
                            this.SetDateValue(datetime);
                        }
                    }
                    else {
                        invalidText = true;
                    }
                }
                else {
                    invalidText = true;
                }
            }
        }
        if (invalidText) {
            errorMessage = "";//'Invalid Input';
        }
    }
    GetTodaysDate() {
        var today: Date = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(today.getHours());
        today.setUTCMinutes(today.getMinutes());
        today.setUTCSeconds(today.getSeconds());
        today.setUTCMilliseconds(0);
        return today;
    }
    GetDateParts(date: Date) {
        var dateParts: number[];
        dateParts = [];
        var year: number;
        var month: number;
        var day: number;
        var hour: number;
        var minute: number;
        var second: number;

        year = date.getUTCFullYear();
        month = date.getUTCMonth();
        day = date.getUTCDate();
        hour = date.getUTCHours();
        minute = date.getUTCMinutes();
        second = date.getUTCSeconds();

        dateParts.push(year);
        dateParts.push(month);
        dateParts.push(day);
        dateParts.push(hour);
        dateParts.push(minute);
        dateParts.push(second);
        return dateParts;
    }
    GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
        var date: Date = new Date();
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        var endDayNumber: number = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
        if (endDayNumber == 31) {
            date.setUTCDate(day);
            date.setUTCMonth(month);
        }
        else {
            date.setUTCMonth(month);
            date.setUTCDate(day);
        }
        date.setUTCFullYear(year);
        return date;
    }
    SetDateValue(date: Date) {
        var day: number;
        var month: number;
        var year: number;
        var hour: number;
        var minute: number;
        var second: number;
        var dateparts = this.GetDateParts(date);
        day = dateparts[2];
        month = dateparts[1] + 1; 
        year = dateparts[0];

        hour = dateparts[3];
        minute = dateparts[4];
        second = dateparts[5];

        this.InputValue = this.ApplyPadding(hour.toString()) + ':' + this.ApplyPadding(minute.toString())/* + ':' + this.ApplyPadding(second.toString())*/;
    }
    ApplyPadding(str: string) {
        var pad = "00"
        var ans = pad.substring(0, pad.length - str.length) + str
        return ans;
    }
    

    public IsMouseOverMe: boolean = false;
    OnLostFocus() {
        if (!this.IsMouseOverMe) {
            if (this.IsNew && AppTool.IsNullOrEmpty(this.DisplayValue)) {
                this.IsShowText = false;
            }
            else {
                this.IsShowText = true;
            }

            this.GetTimeValue();
        }
    }
    TextBoxLostFocus() {
        if (this.IsNew && AppTool.IsNullOrEmpty(this.DisplayValue)) {
            this.IsShowText = false;
        }
        else {
            this.IsShowText = true;
        }
    }
    OnFocus() {
        if (this.Disabled)
            this.IsShowText = true;
        else
            this.IsShowText = false;
    }

    isKeyDown: boolean = false;
    isCtrlKeyDown: boolean = false;
    isTextChanged: boolean = false;
    onkeydown(event) {
        this.isKeyDown = true;
        var key = event.keyCode;
        var keyChar = event.key;
        var success: boolean;

        if (key == 17) {
            this.isCtrlKeyDown = true;
        }

        if (key != 13 && key != 9 && key != 17) {
            this.isTextChanged = true;
        }

        if (key == 9) {
            if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == 190 || key == 110
                || key == 8 || key == 58 || key == 45 || key == 47 || key == 46 || key == 43 || key == 112
                || key == 109 || key == 97 || key == 9 || key == 189 || key == 35 || key == 36 || key == 16 || key == 187 || key == 37 ||
                key == 38 || key == 39 || key == 40 || key == 190 || key == 191 || key == 111 || key == 17 || keyChar == ":" || keyChar == ".:") {
                success = true;
            }
            else {
                success = false;
            }

            if (key == 13 || key == 9) {
                this.GetTimeValue();
            }

            if (key == 67 || key == 65 || key == 86 || key == 88) {
                if (this.isCtrlKeyDown) {
                    success = true;
                }
            }

            if (success) {
                return key;
            }
            else {
                return false;
            }

        }
    }

    onButtonClick(e) {
        e.preventDefault();
    }
}
