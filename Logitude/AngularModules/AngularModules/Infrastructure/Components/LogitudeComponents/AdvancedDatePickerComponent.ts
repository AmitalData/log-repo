declare var window: any;
import { Input, Output, Component, OnInit, EventEmitter, OnDestroy, HostListener, ElementRef } from "@angular/core";
import { CustomFieldClass } from "../../DataContracts/CustomFieldClass";
import { ControlsIdCounter } from "../../Utilities/ControlsIdCounter";
import { CodeNameClass } from '../../DataContracts/CodeNameClass';
import { FieldValidator } from "../../Validators/FieldValidator";
import { SessionLocator } from "../../Utilities/SessionLocator";
import { ObjectsLocator } from "../../Locators/ObjectsLocator";
import { ObjectFieldPM } from "../../EntityPMs/ObjectFieldPM";
import { UIProperty, } from "./UIProperties";
import { AppTool } from "../../Tools";

@Component({
    selector: "AdvancedDatePicker",
    templateUrl: "./AdvancedDatePickerComponent.html",
    inputs: [
        "ObjectFieldName",
        "ObjectTableName",
        "DataContext",
        "InputType",
        "TimeMode",
        "IsDisabled",
        "ShowToolTip",
    ]
})

export class AdvancedDatePickerComponent implements OnInit, OnDestroy {
    public DateOptions: CodeNameClass[] = [];
    DropdownId: string;
    DivAdvancedDatePickerId: string;
    DisplayValue: string;
    LayoutDirection: string = "ltr";
    isRTL: boolean = false;
    show: boolean;
    public CopyValueSubs: any;
    public ObjectField: ObjectFieldPM;
    public ObjectFieldName: string = null;
    public ObjectTableName: string = null;
    public DataContext: any;
    public InputType: string;
    public TimeMode: string;
    public uiProperty: UIProperty;
    private isDisabled: boolean;
    public ShowToolTip: boolean;

    @Output() ValueChanged = new EventEmitter();
    @Input() NoObjectField: boolean = false;
    @Input() NoValidation: boolean = false;

    private selectedDateValue: any;
    @Input()
    public get SelectedDateValue() {
        return this.selectedDateValue;
    }

    public set SelectedDateValue(newValue: any) {
        var isOk = true;
        if (newValue) {
            isOk = false;
            if (newValue instanceof Date) {
                isOk = true;
            }
            else if (typeof (newValue) == "string") {
                var selectedItem: CodeNameClass = this.DateOptions.filter(date => date.Code == newValue)[0];
                if (selectedItem) {
                    this.SelectedItem = selectedItem;
                    this.SelectedItemObject = this.SelectedItem;
                    this.selectedDateValue = selectedItem.Name;
                    this.DisplayValue = selectedItem.Name; 
                }
                else {
                    selectedItem = this.DateOptions.filter(date => date.Code == "SPD")[0];
                    this.SelectedItem = selectedItem;
                    this.SelectedItemObject = this.SelectedItem;
                    this.selectedDateValue = newValue;
                    var myDisplayValue = this.SetDateValue(new Date(newValue));
                    this.DisplayValue = myDisplayValue?.split(" ")[0];;
                }
            }
        }

        if (isOk) {
            if (newValue === undefined) {
                newValue = null;
            }

            if (this.selectedDateValue === undefined) {
                this.selectedDateValue = null;
            }

            if (this.selectedDateValue != newValue) {
                var selectedItem = this.DateOptions.filter(date => date.Code == "SPD")[0];
                this.SelectedItem = selectedItem;
                var selectedCalendarDate = this.SetDateValue(newValue, null);
                this.selectedDateValue = selectedCalendarDate;
                this.DisplayValue = selectedCalendarDate?.split(" ")[0];
                this.SelectedItemObject = this.SelectedItem;
            }
        }
    }

    public get IsDisabled() {
        return this.isDisabled;
    }

    public set IsDisabled(newValue: boolean) {
        this.isDisabled = newValue;
    }

    public SelectedItem: any;
    @Input() SelectedItemObject: any;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();

    private selectedValue: any;
    public get SelectedValue() {
        return this.selectedValue;
    }

    @Input()
    public set SelectedValue(newValue: any) {
        if (this.selectedValue != newValue) {
            this.selectedValue = newValue;
            if (this.uiProperty != null) {
                this.uiProperty.UIPropertyChanged.emit("valuechanges");
            }
            this.ValueChanged.emit(this.selectedValue);
        }
    }

    @HostListener('document:click', ['$event'])
    clickout(event) {
        if (!this.eRef.nativeElement.contains(event.target)) {
            this.IsDropDownVisible = false;
            this.IsOpen = false;
        }
    }

    constructor(private eRef: ElementRef) {
        this.show = false;
        if (ObjectsLocator.GlobalSetting) {
            this.LayoutDirection = ObjectsLocator.GlobalSetting.LayoutDirection;
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
        }
        else this.LayoutDirection = "ltr";

        this.SetDateOptions();
    }

    SetDateOptions() {
        this.DateOptions.push(new CodeNameClass("SPD", "Specific Date"));
        this.DateOptions.push(new CodeNameClass("TOD", "Today"));
        this.DateOptions.push(new CodeNameClass("YES", "Yesterday"));
        this.DateOptions.push(new CodeNameClass("BTM", "Begin of this month"));
        this.DateOptions.push(new CodeNameClass("BLM", "Begin of last month"));
        this.DateOptions.push(new CodeNameClass("BTQ", "Begin of this quarter"));
        this.DateOptions.push(new CodeNameClass("BLQ", "Begin of last quarter"));
        this.DateOptions.push(new CodeNameClass("BTY", "Begin of this year"));
        this.DateOptions.push(new CodeNameClass("BLY", "Begin of last year"));
        this.DateOptions.push(new CodeNameClass("ETM", "End of this month"));
        this.DateOptions.push(new CodeNameClass("ELM", "End of last month"));
        this.DateOptions.push(new CodeNameClass("ETQ", "End of this quarter"));
        this.DateOptions.push(new CodeNameClass("ELQ", "End of last quarter"));
        this.DateOptions.push(new CodeNameClass("ETY", "End of this year"));
        this.DateOptions.push(new CodeNameClass("ELY", "End of last year"));
    }

    ngOnInit() {
        this.InitializeControl();
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.IsDisabled = this.uiProperty != null ? !this.uiProperty.IsEnabled : false;
    }

    counterId: number;
    InitializeControl() {
        this.counterId = ControlsIdCounter.GetNextIdCounter();
        this.DivAdvancedDatePickerId = 'AdvancedDatePicker-' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.DropdownId = 'AdvancedDatePickerDropDown-' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.ErrorPopUpId = 'advanceddatepickerererrorpop-' + this.counterId;
        this.LogCalendarId = 'LogCalendar-' + this.ObjectFieldName + '-' + this.counterId.toString();
    }


    ngOnDestroy() {
        console.log("advanceddatepicker:ngOnDestroy");
    }

    //ErrorPopUp
    ErrorPopUpId: string;
    ShowErrorPopup: boolean = false;

    SetValidity(validValue: boolean, errorMessage) {
        if (this.uiProperty != null) {
            this.uiProperty.ValidValue = validValue;
            this.uiProperty.ValidationError = errorMessage;
        }
        if (!validValue) {
            if (this.show) {
                this.ShowErrorPopup = true;
            }
        }
        else {
            this.ShowErrorPopup = false;
        }
    }

    //MainTable
    DeleteButtonNgStyle: any;
    Detach: boolean;
    ComponentMouseInArea: boolean;

    OnComponentMouseOver() {
        this.ComponentMouseInArea = true;
        if (!this.SelectedItem) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        } else {
            this.DeleteButtonNgStyle = null;
        }
        this.DetectChanges();
    }

    OnComponentMouseOut() {
        this.ComponentMouseInArea = false;
        this.DetectChanges();
    }

    DetectChanges() {
        return;
    }

    //DropDownList
    public IsDropDownVisible: boolean;
    public DropDownMouseInArea: boolean;
    public IsOpen: boolean;
    public ItemsSource: any[];

    OnDropDownMouseOver() {
        this.DropDownMouseInArea = true;
    }

    OnDropDownMouseOut() {
        this.DropDownMouseInArea = false;
    }

    OnLiMouseOver($event) {
        this.DropDownMouseInArea = true;
        var active = document.getElementsByClassName("highlighted");
        if (active[0]) {
            active[0].classList.remove("highlighted");
        }
        if ($event.target.tagName != "DIV") {
            $event.target.classList.add("highlighted");
        }
        else {
            $event.target.parentElement.parentElement.classList.add("highlighted");
        }
        if ($event.target.innerText == "Specific Date") {
            this.ToggleCalendar(true);
        }
        else {
            this.ToggleCalendar(false);
        }
    }

    OnLiMouseLeave($event) {
        this.DropDownMouseInArea = false;
        $event.target.classList.remove("highlighted");
    }

    OnDropDownSelected(item: any, newValue = null) {
        if (item.Code == "SPD" && AppTool.IsNullOrEmpty(newValue)) return;
        this.SelectedItem = item;
        var selectedValue = this.SelectedItem.Name;
        if (!AppTool.IsNullOrEmpty(newValue)) selectedValue = newValue;
        this.DataContext[this.ObjectFieldName] = this.SelectedItem.Code == "SPD" ? selectedValue : this.SelectedItem.Code;
        this.SelectedItemObject = this.SelectedItem;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.ValidateField();
        this.ToggleOpenDropDown();
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
        this.DetectChanges();
    }

    OnDropDownClicked() {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }
    }

    ToggleOpenDropDown() {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        this.DetectChanges();
    }

    Populate(searchText: string) {
        if (searchText) {
            this.ItemsSource = this.DateOptions.filter(date => date.Name.toLowerCase().indexOf(searchText.toLowerCase()) != -1);
        }
        else {
            this.ItemsSource = this.DateOptions;
        }
        this.DetectChanges();
    }

    OnDropDownFocus() {
        //var inputElement = document.getElementById(this.DropdownId);
        //if (inputElement) inputElement.focus();
    }

    OnDropDownBlur() {
        if (!this.IsCalendarOpen || (this.IsCalendarOpen && !this.CalendarMouseInArea)) {
            this.IsDropDownVisible = false;
            this.IsOpen = false;
            this.ToggleCalendar(false);
        }
    }

    OnDeleteValue() {
        this.SelectedItem = null;
        this.DataContext[this.ObjectFieldName] = null;
        this.SelectedItemObject = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DetectChanges();
        this.DisplayValue = null;
        this.ValidateField();
        this.ValueChanged.emit(null);
    }

    //LogCalendar
    IsCalendarOpen: boolean;
    IsCalendarDateDropDownOpen: boolean;
    LogCalendarId: string;
    CalendarMouseInArea: boolean;
    InputValue: string;
    DateValue: string;
    TimeValue: string;

    private selectedCalendarDate: Date;
    public get SelecteCalendardDate() {
        return this.selectedCalendarDate;
    }
    public set SelectedCalendarDate(newValue: Date) {
        if (this.selectedCalendarDate != newValue) {
            this.selectedCalendarDate = newValue;
            this.ValueChanged.emit(this.selectedCalendarDate);
        }
    }

    OnCalendarBlur() {
        if (!this.DropDownMouseInArea && !this.CalendarMouseInArea) {
            this.IsDropDownVisible = false;
        }
    }

    OnSelectedCalendarDateChanged(selectedDateObj: any) {
        var selectedCalendarDate = selectedDateObj.SelectedDate;
        this.OnDropDownSelected(this.DateOptions.filter(d => d.Code == "SPD")[0], selectedCalendarDate);
        this.ToggleCalendar(false);
    }

    ToggleCalendar(open: boolean) {
        this.IsCalendarDateDropDownOpen = open;
        this.IsCalendarOpen = open;
    }

    OnCalendarMouseOver() {
        this.CalendarMouseInArea = true;
        var inputElement = document.getElementById(this.LogCalendarId);
        if (inputElement) inputElement.focus();
    }

    OnCalendarMouseOut() {
        this.CalendarMouseInArea = false;
        var inputElement = document.getElementById(this.LogCalendarId);
        if (inputElement) inputElement.focus();
    }

    SetDateValue(date: Date, timeSuffix: string = null) {
        if (date) {
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

            this.DateValue =
                year +
                "/" +
                this.ApplyPadding(month.toString()) +
                "/" +
                this.ApplyPadding(day.toString());
            this.TimeValue =
                this.ApplyPadding(hour.toString()) +
                ":" +
                this.ApplyPadding(minute.toString()) +
                ":" +
                this.ApplyPadding(second.toString());

            var timearr = this.TimeValue.split(":");
            var hourRes = this.GetTimeModeHours(
                Number(timearr[0]),
                timeSuffix
            );
            var hourResArr = hourRes.split(",");
            if (this.TimeMode == "12") {
                var tSuffix = hourResArr[1];
                if (timeSuffix) {
                    tSuffix = timeSuffix;
                }
                this.TimeValue =
                    this.ApplyPadding(hourResArr[0]) +
                    ":" +
                    timearr[1] +
                    ":" +
                    timearr[2] +
                    " " +
                    tSuffix;
            } else {
                this.TimeValue =
                    this.ApplyPadding(hourResArr[0]) +
                    ":" +
                    timearr[1] +
                    ":" +
                    timearr[2];
            }

            var dateValue;

            if (this.InputType == "date") {
                if (
                    !AppTool.IsNullOrEmpty(
                        SessionLocator.TenantPM.DateTimeFormat
                    )
                ) {
                    var myDateTimeFormatPrefix = SessionLocator.TenantPM.DateTimeFormat.toLowerCase().substring(
                        0,
                        2
                    );

                    if (myDateTimeFormatPrefix == "mm") {
                        dateValue = this.ApplyPadding(month.toString()) + "/" + this.ApplyPadding(day.toString()) + "/" + year;
                    } else {
                        dateValue = this.ApplyPadding(day.toString()) + "/" + this.ApplyPadding(month.toString()) + "/" + year;
                    }
                } else {
                    dateValue = this.ApplyPadding(day.toString()) + "/" + this.ApplyPadding(month.toString()) + "/" + year;
                }
                return dateValue + " " + this.TimeValue;
            }
            else {
                var timeArray = this.TimeValue.split(":");
                if (
                    this.TimeValue.indexOf("AM") > -1 ||
                    this.TimeValue.indexOf("PM") > -1
                ) {
                    var secondsWithsuffix = timeArray[2].split(" ");
                    var suffix = secondsWithsuffix[1];

                    dateValue = this.ApplyPadding(timeArray[0]) + ":" + timeArray[1] + " " + suffix;
                } else {
                    dateValue = this.ApplyPadding(timeArray[0]) + ":" + timeArray[1];
                }
                return dateValue + " " + this.TimeValue;
            }
        }
        else {
            this.SelectedCalendarDate = null;
            this.InputValue = null;
            this.TimeValue = null;
            if (this.ObjectField && this.ObjectField.IsCustom) {
                var customFieldClass: CustomFieldClass = this.DataContext[
                    this.ObjectFieldName
                ];
                if (
                    customFieldClass != null &&
                    customFieldClass != undefined
                ) {
                    customFieldClass.Value = null;
                    this.DataContext[
                        this.ObjectFieldName
                    ] = customFieldClass;
                } else {
                    console.warn(
                        "Custom Fields are not implemented in: " +
                        this.ObjectTableName
                    );
                }
            } else {
                this.DataContext[this.ObjectFieldName] = null;
            }

            this.ValidateField();
            var dateUiProp = this.DataContext.UIProperties.GetUIProperty(
                this.ObjectFieldName,
                this.ObjectTableName,
                this.DataContext
            );
            var timeUiProp = this.DataContext.UIProperties.GetUIProperty(
                this.ObjectFieldName + "_timepicker",
                this.ObjectTableName,
                this.DataContext
            );
            if (timeUiProp != null && timeUiProp != undefined) {
                timeUiProp.UIPropertyChanged.emit("datevaluechanges");
            }
            dateUiProp.UIPropertyChanged.emit("datevaluechanges");
        }
    }

    ValidateField(emitPropertyChanged: boolean = true) {
        if (!this.NoValidation) {
            var errors = null;
            var table;
            if (this.uiProperty != null) {
                table = window.ObjectTables.filter(
                    d => d.Name === this.uiProperty.ObjectTableName
                )[0];
            }
            if (table) {
                var field: ObjectFieldPM = window.ObjectFields.filter(
                    d =>
                        d.ObjectTableId === table.Id &&
                        d.FieldName === this.uiProperty.FieldName
                )[0];
                var fieldValidator: FieldValidator = new FieldValidator();
                errors = fieldValidator.Validate(
                    this.ObjectFieldName,
                    this.ObjectTableName,
                    this.DataContext
                );
            }
            if (this.uiProperty != null && this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            } else if (errors) {
                if (errors.length > 0) {
                    this.SetValidity(false, errors[0]);
                }
                else {
                    this.SetValidity(true, null);
                }
            }
            else {
                this.SetValidity(true, null);
            }
            if (emitPropertyChanged && this.uiProperty != null) {
                this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
            }
        }
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

    ApplyPadding(str: string) {
        var pad = "00";
        var ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    }

    GetTimeModeHours(hours: number, suffix: string = null) {
        if (isNaN(hours)) {
            hours = 0;
        }
        if (this.TimeMode == "12") {
            if (hours <= 11) {
                if (hours == 0) {
                    hours = 12;
                }
                return hours.toString() + "," + "AM";
            } else {
                var convHour;
                switch (hours) {
                    case 12: {
                        convHour = 12;
                        break;
                    }
                    case 13: {
                        convHour = 1;
                        break;
                    }
                    case 14: {
                        convHour = 2;
                        break;
                    }
                    case 15: {
                        convHour = 3;
                        break;
                    }
                    case 16: {
                        convHour = 4;
                        break;
                    }
                    case 17: {
                        convHour = 5;
                        break;
                    }
                    case 18: {
                        convHour = 6;
                        break;
                    }
                    case 19: {
                        convHour = 7;
                        break;
                    }
                    case 20: {
                        convHour = 8;
                        break;
                    }
                    case 21: {
                        convHour = 9;
                        break;
                    }
                    case 22: {
                        convHour = 10;
                        break;
                    }
                    case 23: {
                        convHour = 11;
                        break;
                    }
                }
                return convHour.toString() + "," + "PM";
            }
        } else {
            hours = this.GetTimeFor24Mode(hours, suffix);
        }

        return hours.toString();
    }

    GetDateFromString(datestring: string) {
        var dateAndTime: string[];
        var suffix: string;
        if (datestring.indexOf("T") > -1) {
            dateAndTime = datestring.split("T");
        } else {
            dateAndTime = datestring.split(" ");
        }
        var dateArray: string[];
        if (dateAndTime[0].indexOf("/") > -1) {
            dateArray = dateAndTime[0].split("/");
        } else if (dateAndTime[0].indexOf("-") > -1) {
            dateArray = dateAndTime[0].split("-");
        } else if (dateAndTime[0].indexOf(".") > -1) {
            dateArray = dateAndTime[0].split(".");
        }

        if (dateAndTime.length > 2) {
            suffix = dateAndTime[2];
        }

        var timeArray: string[];
        var hour: number = 0;
        var minute: number = 0;
        var second: number = 0;
        if (dateAndTime.length >= 2) {
            if (dateAndTime[1].indexOf(".") > -1) {
                timeArray = dateAndTime[1].split(".")[0].split(":");
            } else {
                timeArray = dateAndTime[1].split(":");
            }
            var hour: number = this.GetTimeFor24Mode(
                Number(timeArray[0]),
                suffix
            );
            var minute: number = Number(timeArray[1]);
            var second: number = Number(timeArray[2].substring(0, 2));
        }

        var year: number = Number(dateArray[0]);
        var month: number = Number(dateArray[1]) - 1;
        var day: number = Number(dateArray[2]);

        var date: Date = this.GetDate(year, month, day, hour, minute, second);
        return date;
    }

    GetTimeFor24Mode(hours: number, suffix: string) {
        if (suffix) {
            var convHour;
            if (suffix.toLowerCase() == "am") {
                if (hours >= 12) {
                    switch (hours) {
                        case 12: {
                            convHour = 0;
                            break;
                        }
                        case 13: {
                            convHour = 1;
                            break;
                        }
                        case 14: {
                            convHour = 2;
                            break;
                        }
                        case 15: {
                            convHour = 3;
                            break;
                        }
                        case 16: {
                            convHour = 4;
                            break;
                        }
                        case 17: {
                            convHour = 5;
                            break;
                        }
                        case 18: {
                            convHour = 6;
                            break;
                        }
                        case 19: {
                            convHour = 7;
                            break;
                        }
                        case 20: {
                            convHour = 8;
                            break;
                        }
                        case 21: {
                            convHour = 9;
                            break;
                        }
                        case 22: {
                            convHour = 10;
                            break;
                        }
                        case 23: {
                            convHour = 11;
                            break;
                        }
                    }
                    return convHour.toString();
                }
                return hours.toString();
            }
            if (suffix.toLowerCase() == "pm") {
                if (hours < 12) {
                    switch (hours) {
                        case 0: {
                            convHour = 12;
                            break;
                        }
                        case 1: {
                            convHour = 13;
                            break;
                        }
                        case 2: {
                            convHour = 14;
                            break;
                        }
                        case 3: {
                            convHour = 15;
                            break;
                        }
                        case 4: {
                            convHour = 16;
                            break;
                        }
                        case 5: {
                            convHour = 17;
                            break;
                        }
                        case 6: {
                            convHour = 18;
                            break;
                        }
                        case 7: {
                            convHour = 19;
                            break;
                        }
                        case 8: {
                            convHour = 20;
                            break;
                        }
                        case 9: {
                            convHour = 21;
                            break;
                        }
                        case 10: {
                            convHour = 22;
                            break;
                        }
                        case 11: {
                            convHour = 23;
                            break;
                        }
                    }
                    return convHour.toString();
                }
                return hours.toString();
            }
        }
        return hours;
    }

    GetDate(
        year: number,
        month: number,
        day: number,
        hour: number,
        minute: number,
        second: number
    ) {
        var date: Date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    }
}
