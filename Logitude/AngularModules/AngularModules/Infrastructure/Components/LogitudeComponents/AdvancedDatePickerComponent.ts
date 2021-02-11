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
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';

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
    AdvancedDatePickerInputId: string;
    LayoutDirection: string = "ltr";
    isRTL: boolean = false;
    private showErrorOnHover: boolean;
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

    public AdvancedDatePickerPlaceHolder: string;

    @Output() OnInputBlurEvent: EventEmitter<any> = new EventEmitter();
    @Output() InputLostFocus: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() ValueChanged = new EventEmitter();
    @Input() NoObjectField: boolean = false;
    @Input() NoValidation: boolean = false;

    private selectedDateValue: any;
    @Input()
    public get SelectedDateValue() {
        return this.selectedDateValue;
    }

    public set SelectedDateValue(newValue: any) {
        let isOk = true;
        if (newValue) {
            isOk = false;
            if (newValue instanceof Date) {
                isOk = true;
            }
            else if (typeof (newValue) == "string") {
                const selectedItem: CodeNameClass = this.DateOptions.filter(date => date.Code == newValue)[0];
                if (selectedItem) {
                    this.SelectedItem = selectedItem;
                    this.SelectedItemObject = this.SelectedItem;
                    this.selectedDateValue = selectedItem.Name;
                    this.InputValue = selectedItem.Name; 
                }
                else {
                    this.selectedDateValue = newValue;
                    this.DateValueChanged(new Date(newValue));
                }
            }
        }

        if (isOk) {
            if (newValue === undefined) newValue = null;
            if (this.selectedDateValue === undefined) this.selectedDateValue = null;
            if (this.selectedDateValue != newValue && !AppTool.IsNullOrEmpty(newValue)) this.DateValueChanged(newValue);
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
        this.showErrorOnHover = false;
        if (ObjectsLocator.GlobalSetting) {
            this.LayoutDirection = ObjectsLocator.GlobalSetting.LayoutDirection;
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
        }
        else this.LayoutDirection = "ltr";

        this.setDateOptions();
    }

    private setDateOptions() {
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
        if (this.InputType == "date") {
            this.AdvancedDatePickerPlaceHolder = TextCodeTranslator.Translate(
                "General.O.EnterDate"
            );
        }
    }

    counterId: number;
    InitializeControl() {
        this.counterId = ControlsIdCounter.GetNextIdCounter();
        this.AdvancedDatePickerInputId = 'AdvancedDatePickerInputId-' + this.ObjectFieldName + '-' + this.counterId.toString();
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
    public AdvancedDatePickerInputDivStyle;
    SetValidity(validValue: boolean, errorMessage) {
        if (this.uiProperty != null) {
            this.uiProperty.ValidValue = validValue;
            this.uiProperty.ValidationError = errorMessage;
        }
        this.ShowErrorPopup = !validValue ? this.showErrorOnHover : false;
        this.AdvancedDatePickerInputDivStyle = !validValue ? { border: "1px solid #ff0000" } : "" ;
    }

    //MainTable
    public DeleteButtonNgStyle: any;

    public OnComponentMouseOver() {
        this.DeleteButtonNgStyle = this.SelectedItem ? null : { 'visibility': 'hidden' };
        this.DetectChanges();
    }

    public OnComponentMouseOut() {
        this.DetectChanges();
    }

    private DetectChanges() {
        return;
    }

    //DropDownList
    public IsDropDownVisible: boolean;
    public DropDownMouseInArea: boolean;
    public IsOpen: boolean;
    public ItemsSource: any[];

    public OnDropDownMouseOver() {
        this.DropDownMouseInArea = true;
    }

    public OnDropDownMouseOut() {
        this.DropDownMouseInArea = false;
    }

    public OnLiMouseOver($event) {
        this.DropDownMouseInArea = true;
        let active = document.getElementsByClassName("highlighted");
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

    public OnLiMouseLeave($event) {
        this.DropDownMouseInArea = false;
        $event.target.classList.remove("highlighted");
    }

    public OnDropDownSelected(item: any, newValue = null, toggleDropDown = true) {
        if (item.Code == "SPD" && AppTool.IsNullOrEmpty(newValue)) return;
        this.SelectedItem = item;
        let selectedValue = this.SelectedItem.Name;
        if (!AppTool.IsNullOrEmpty(newValue)) selectedValue = newValue;
        this.DataContext[this.ObjectFieldName] = this.SelectedItem.Code == "SPD" ? selectedValue : this.SelectedItem.Code;
        this.SelectedItemObject = this.SelectedItem;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.ValidateField();
        if (toggleDropDown) this.ToggleOpenDropDown();
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
        this.DetectChanges();
    }

    public OnDropDownClicked() {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.ItemsSource = this.DateOptions;
            this.DetectChanges();
        }
    }

    private ToggleOpenDropDown() {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        this.DetectChanges();
    }

    //OnDropDownBlur() {
    //    if (!this.IsCalendarOpen || (this.IsCalendarOpen && !this.CalendarMouseInArea)) {
    //        this.IsDropDownVisible = false;
    //        this.IsOpen = false;
    //        this.ToggleCalendar(false);
    //    }
    //}

    public OnDeleteValue() {
        this.SelectedItem = null;
        this.DataContext[this.ObjectFieldName] = null;
        this.SelectedItemObject = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DetectChanges();
        this.InputValue = null;
        this.ValidateField();
        this.ValueChanged.emit(null);
    }

    //LogCalendar
    public IsCalendarOpen: boolean;
    public IsCalendarDateDropDownOpen: boolean;
    public LogCalendarId: string;
    private CalendarMouseInArea: boolean;
    public InputValue: string;
    private DateValue: string;
    private TimeValue: string;

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

    public OnCalendarBlur() {
        if (!this.DropDownMouseInArea && !this.CalendarMouseInArea) {
            this.IsDropDownVisible = false;
        }
    }

    public OnSelectedCalendarDateChanged(selectedDateObj: any) {
        let selectedCalendarDate = selectedDateObj.SelectedDate;
        this.OnDropDownSelected(this.DateOptions.filter(d => d.Code == "SPD")[0], selectedCalendarDate);
        this.ToggleCalendar(false);
    }

    private ToggleCalendar(open: boolean) {
        this.IsCalendarDateDropDownOpen = open;
        this.IsCalendarOpen = open;
    }

    public OnCalendarMouseOver() {
        this.CalendarMouseInArea = true;
        let inputElement = document.getElementById(this.LogCalendarId);
        if (inputElement) inputElement.focus();
    }

    public OnCalendarMouseOut() {
        this.CalendarMouseInArea = false;
        let inputElement = document.getElementById(this.LogCalendarId);
        if (inputElement) inputElement.focus();
    }

    private SetDateValue(date: Date, timeSuffix: string = null) {
        if (date) {
            const dateParts = this.GetDateParts(date);
            const day: number = dateParts[2];
            const month: number = dateParts[1] + 1;
            const year: number = dateParts[0];
            const hour: number = dateParts[3];
            const minute: number = dateParts[4];
            const second: number = dateParts[5];

            this.DateValue = year + "/" + this.ApplyPadding(month.toString()) + "/" + this.ApplyPadding(day.toString());
            this.TimeValue = this.ApplyPadding(hour.toString()) + ":" + this.ApplyPadding(minute.toString()) + ":" + this.ApplyPadding(second.toString());

            let timearr = this.TimeValue.split(":");
            let hourRes = this.GetTimeModeHours(Number(timearr[0]),timeSuffix);
            let hourResArr = hourRes.split(",");
            if (this.TimeMode == "12") {
                let tSuffix = hourResArr[1];
                if (timeSuffix) 
                    tSuffix = timeSuffix;
                
                this.TimeValue = this.ApplyPadding(hourResArr[0]) + ":" + timearr[1] + ":" + timearr[2] + " " + tSuffix;
            }
            else
                this.TimeValue = this.ApplyPadding(hourResArr[0]) + ":" + timearr[1] + ":" + timearr[2];

            if (this.InputType == "date") {
                if (!AppTool.IsNullOrEmpty(SessionLocator.TenantPM.DateTimeFormat)) {
                    const myDateTimeFormatPrefix = SessionLocator.TenantPM.DateTimeFormat.toLowerCase().substring(0,2);

                    if (myDateTimeFormatPrefix == "mm") 
                        this.DateValue = this.ApplyPadding(month.toString()) + "/" + this.ApplyPadding(day.toString()) + "/" + year;
                    else
                        this.DateValue = this.ApplyPadding(day.toString()) + "/" + this.ApplyPadding(month.toString()) + "/" + year;
                    
                }
                else
                    this.DateValue = this.ApplyPadding(day.toString()) + "/" + this.ApplyPadding(month.toString()) + "/" + year;
            }
            else {
                let timeArray = this.TimeValue.split(":");
                if (this.TimeValue.indexOf("AM") > -1 || this.TimeValue.indexOf("PM") > -1) {
                    let suffix = timeArray[2].split(" ")[1];
                    this.DateValue = this.ApplyPadding(timeArray[0]) + ":" + timeArray[1] + " " + suffix;
                }
                else 
                    this.DateValue = this.ApplyPadding(timeArray[0]) + ":" + timeArray[1];
             }
        }
        else {
            this.SelectedCalendarDate = null;
            this.InputValue = null;
            this.TimeValue = null;
            if (this.ObjectField && this.ObjectField.IsCustom) {
                let customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    customFieldClass.Value = null;
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);     
            }
            else 
                this.DataContext[this.ObjectFieldName] = null;
            
            this.ValidateField();
            let dateUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName,this.ObjectTableName,this.DataContext);
            let timeUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName + "_timepicker",this.ObjectTableName,this.DataContext);
            if (timeUiProp != null && timeUiProp != undefined) {
                timeUiProp.UIPropertyChanged.emit("datevaluechanges");
            }
            dateUiProp.UIPropertyChanged.emit("datevaluechanges");
        }
    }

    private ValidateField(emitPropertyChanged: boolean = true) {
        if (!this.NoValidation) {
            let errors = null;
            let table;
            if (this.uiProperty != null)
                table = window.ObjectTables.filter(d => d.Name === this.uiProperty.ObjectTableName)[0];
            
            if (table) {
                let fieldValidator: FieldValidator = new FieldValidator();
                errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }

            if (this.uiProperty != null && this.uiProperty.IsValidManually == false)
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            else if (errors) {
                if (errors.length > 0) this.SetValidity(false, errors[0]);
                else this.SetValidity(true, null);
            }
            else this.SetValidity(true, null);
            
            if (emitPropertyChanged && this.uiProperty != null) 
                this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
        }
    }

    private GetDateParts(date: Date) {
        let dateParts: number[] = [];
        let year: number = date.getUTCFullYear();
        let month: number = date.getUTCMonth();
        let day: number = date.getUTCDate();
        let hour: number = date.getUTCHours();
        let minute: number = date.getUTCMinutes();
        let second: number = date.getUTCSeconds();

        dateParts.push(year);
        dateParts.push(month);
        dateParts.push(day);
        dateParts.push(hour);
        dateParts.push(minute);
        dateParts.push(second);
        return dateParts;
    }

    private ApplyPadding(str: string) {
        let pad = "00";
        let ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    }

    private GetTimeModeHours(hours: number, suffix: string = null) {
        if (isNaN(hours)) {
            hours = 0;
        }
        if (this.TimeMode == "12") {
            if (hours <= 11) {
                hours = hours == 0 ? 12 : hours;
                return hours.toString() + "," + "AM";
            }
            else {
                let convHour;
                switch (hours) {
                    case 12: {
                        convHour = 12;
                        break;
                    }
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23: {
                        convHour = hours - 12;
                        break;
                    }
                }
                return convHour.toString() + "," + "PM";
            }
        }
        else hours = this.GetTimeFor24Mode(hours, suffix);
        
        return hours.toString();
    }

    private GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
        let date: Date = new Date();
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

    //Input DatePicker
    private SelectedDate: Date;
    public OnInputFocus() {
        this.showErrorOnHover = true;
        this.ShowErrorPopup = !this.uiProperty.ValidValue;
        if (this.IsDropDownVisible) this.IsDropDownVisible = false;
    }

    public OnInputBlur() {
        this.ShowErrorPopup = false;
        this.showErrorOnHover = false;

        if (!this.CalendarMouseInArea) {
            if (this.InputType == "date" && this.isTextChanged) this.GetInputDateValue();
            //else if (this.isTextChanged) this.GetTimeValue(); //DateTime

            this.OnInputBlurEvent.emit({ Id: this.AdvancedDatePickerInputId });
            this.InputLostFocus.emit(true);
        }
        this.isTextChanged = false;
    }

    private isCtrlKeyDown: boolean = false;
    private isTextChanged: boolean = false;
    public Onkeydown(event) {
        const key = event.keyCode;
        const keyChar = event.key;
        let success: boolean;

        if (key == 17) this.isCtrlKeyDown = true;
        if (key != 13 && key != 9 && key != 17) this.isTextChanged = true; //9: Tab, 13: Enter, 17: Ctrl
        if (this.IsCalendarOpen) this.ToggleCalendar(false);
        if (key == 9) this.CalendarMouseInArea = false;
        
        success = this.IsAllowedKey(key, keyChar);

        if (key == 13 || key == 9) {
            if (this.InputType == "date" && this.isTextChanged) {
                this.GetInputDateValue();
            }
            //else if (this.isTextChanged) {//DateTime
            //    this.GetTimeValue();
            //}
        }

        if (key == 67 || key == 65 || key == 86 || key == 88) { //Select All & Copy & Past & Cut
            if (this.isCtrlKeyDown) {
                success = true;
            }
        }

        return success ? key : false;
    }

    private IsAllowedKey(key, keyChar) {
        let success: boolean = false;
        switch (this.InputType) {
            case "date": {
                success = this.IsAllowedDateKey(key);
                break;
            }
            case "time": {
                success = this.IsAllowedTimeKey(key, keyChar);
                break;
            }
        }
        return success;
    }

    private IsAllowedDateKey(key) {
        return (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105) ||
            key == 8 ||
            key == 45 ||
            key == 47 ||
            key == 46 ||
            key == 43 ||
            key == 9 ||
            key == 190 ||
            key == 110 ||
            key == 107 ||
            key == 109 ||
            key == 189 ||
            key == 16 ||
            key == 111 ||
            key == 35 ||
            key == 36 ||
            key == 16 ||
            key == 187 ||
            key == 37 ||
            key == 38 ||
            key == 39 ||
            key == 40 ||
            key == 190 ||
            key == 191 ||
            key == 17;
    }

    private IsAllowedTimeKey(key, keyChar) {
        return (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105) ||
            key == 190 ||
            key == 110 ||
            key == 8 ||
            key == 58 ||
            key == 45 ||
            key == 47 ||
            key == 46 ||
            key == 43 ||
            key == 112 ||
            key == 109 ||
            key == 97 ||
            key == 9 ||
            key == 189 ||
            key == 35 ||
            key == 36 ||
            key == 16 ||
            key == 187 ||
            key == 37 ||
            key == 38 ||
            key == 39 ||
            key == 40 ||
            key == 190 ||
            key == 191 ||
            key == 111 ||
            key == 17 ||
            keyChar == ":";
    }

    public Onkeyup(event) {
        if (event.keyCode == 17) this.isCtrlKeyDown = false;

        if (event.keyCode == 8 && this.InputType == "date" && this.IsEmptyInputValue()) { //8: BackSpace Key
            this.SetDateValue(null);
            this.DateValueChanged(null);
        }
    }

    private IsEmptyInputValue() {
        return this.InputValue == "" || this.InputValue == undefined || this.InputValue == null;
    }

    private GetInputDateValue() {
        if (this.InputValue && this.InputValue != "") {
            if (!this.TimeValue) this.TimeValue = "00:00:00";
            let suffix = null;
            if (this.TimeValue.indexOf("AM") > -1 ||this.TimeValue.indexOf("PM") > -1) {
                suffix = this.TimeValue.split(" ")[1];
                this.TimeValue = this.TimeValue.split(" ")[0];
            }
            const timearr = this.TimeValue.split(":");
            let hour = 0;
            if (suffix != null) hour = this.GetTimeFor24Mode(Number(timearr[0]), suffix);
            else hour = Number(timearr[0]);
            
            const minute = Number(timearr[1]);
            const second = Number(timearr[2]);
            const todayDateTime: Date = this.GetTodaysDate();
            const dateparts = this.GetDateParts(todayDateTime);
            const date: Date = this.GetDate(dateparts[0],dateparts[1],dateparts[2],hour,minute,second);
            var invalidText: boolean = false;
            var invalidDate: boolean = false;
            var errorMessage: string = "";
            const val = this.InputValue.trim();
            if (val.length > 10) invalidText = true;
            

            if (!invalidText && !invalidDate) {
                if (this.InputValue.trim() == ".") this.DateValueChanged(date);
                else if (this.InputValue.indexOf("+") == 0 || this.InputValue.indexOf("-") == 0) {
                    if (this.InputValue.length > 1) {
                        let days = 0;
                        const sign = this.InputValue.substring(0, 1);
                        const daysString = this.InputValue.substring(1,this.InputValue.length);

                        if (sign == "+") {
                            days = Number(daysString);
                            if (days) date.setDate(date.getDate() + days);
                            else invalidText = true;
                        }
                        else if (sign == "-") {
                            days = Number(daysString);
                            if (days) date.setDate(date.getDate() - days);
                            else invalidText = true;
                        }
                    }
                    if (!invalidText && this.IsOldDate(date)) {
                        invalidDate = true;
                        errorMessage = "Date time is too way in the past!";
                    }
                    else if (date && date.toString() == "Invalid Date") {
                        invalidDate = true;
                        errorMessage = "Invalid Date";
                    }
                    if (!invalidText && !invalidDate) this.DateValueChanged(date);
                }
                else if ((this.InputValue != "." && this.InputValue.indexOf(".") > -1) ||
                         (this.InputValue != "/" && this.InputValue.indexOf("/") > -1) ||
                         (this.InputValue != "-" && this.InputValue.indexOf("-") > -1)) {
                    const nowDate: Date = this.GetTodaysDate();
                    let dateStrings: string[];
                    if (this.InputValue.indexOf(".") > -1) dateStrings = this.InputValue.split(".");
                    else if (this.InputValue.indexOf("-") > -1) dateStrings = this.InputValue.split("-");
                    else dateStrings = this.InputValue.split("/");
                    
                    const nowdateparts = this.GetDateParts(nowDate);
                    let day;
                    let month;
                    let year;
                    const currentYear = nowdateparts[0];
                    const currentMonth = nowdateparts[1] + 1;
                    const currentYearMillinium = currentYear.toString().substring(0, 1) + "000";
                    const currentMillinium = Number(currentYearMillinium);

                    if (dateStrings.length == 3) {
                        if (dateStrings[0].length > 2 && dateStrings[2].length <= 2) {
                            year = Number(dateStrings[0]);
                            month = Number(dateStrings[1]);
                            day = Number(dateStrings[2]);
                        }
                        else if (dateStrings[2].length >= 2 && dateStrings[0].length <= 2) {
                            day = Number(dateStrings[0]);
                            month = Number(dateStrings[1]);
                            year = Number(dateStrings[2]);
                        }
                        else invalidText = true;
                        
                        //calculating year
                        if (year == 0) year = currentYear;
                        if (year < 1000) year = year + currentMillinium;
                        if (month > 12 || day > 31) invalidText = true;

                        const dateTime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                        if (!invalidText && this.IsOldDate(dateTime)) {
                            invalidDate = true;
                            errorMessage = "Date time is too way in the past!";
                        }
                        if (!invalidText && !invalidDate) this.DateValueChanged(dateTime);
                    }
                    else if (dateStrings.length == 2) {
                        day = Number(dateStrings[0]);
                        month = Number(dateStrings[1]);

                        if (month == 0) month = currentMonth;
                        if (month > 12 || day > 31) invalidText = true;
                        if (!invalidText) {
                            const datetime: Date = this.GetDate(currentYear,month - 1,day,hour,minute,second);
                            this.DateValueChanged(datetime);
                        }
                    }
                    else if (dateStrings.length == 1) {
                        day = Number(dateStrings[0]);
                        if (day > 31) invalidText = true;
                        if (!invalidText) {
                            const datetime: Date = this.GetDate(currentYear,currentMonth,day,hour,minute,second);
                            this.DateValueChanged(datetime);
                        }
                    }
                }
                else {
                    const valid = Number(this.InputValue);
                    if (valid) {
                        const nowdateparts = this.GetDateParts(date);
                        let day;
                        let month;
                        let year;
                        let dayString;
                        let monthString;
                        let yearString;
                        const currentYear = nowdateparts[0]; //date.getFullYear();
                        const currentMonth = nowdateparts[1] + 1; //date.getMonth() + 1;
                        const currentYearMillinium = currentYear.toString().substring(0, 1) + "000";
                        const currentMillinium = Number(currentYearMillinium);

                        if (this.InputValue.length == 8) {
                            //01082016
                            dayString = this.InputValue.substring(0, 2);
                            monthString = this.InputValue.substring(2, 4);
                            yearString = this.InputValue.substring(4, 8);
                            day = Number(dayString);
                            month = Number(monthString);
                            year = Number(yearString);

                            if (day > 31) {
                                invalidText = true;
                                errorMessage = "Invalid day, day must be between 01 and 31";
                            }
                            if (month > 12) {
                                invalidText = true;
                                errorMessage = "Invalid month, month must be between 01 and 12";
                            }

                            //calculating year
                            if (year == 0) {
                                year = currentYear;
                            }
                            if (year < 1000) {
                                year = year + currentMillinium;
                            }

                            if (year < currentYear - 100 || year > currentYear + 100) {
                                invalidText = true;
                                errorMessage = "Please Enter More Suitable Year";
                            }

                            if (!invalidText) {
                                const datetime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                                this.DateValueChanged(datetime);
                            }
                        }
                        else if (this.InputValue.length == 6) {
                            //010816

                            dayString = this.InputValue.substring(0, 2);
                            monthString = this.InputValue.substring(2, 4);
                            yearString = this.InputValue.substring(4, 6);
                            day = Number(dayString);
                            month = Number(monthString);
                            year = Number(yearString);

                            //calculating year
                            if (year == 0) {
                                year = currentYear;
                            }
                            if (year < 1000) {
                                year = year + currentMillinium;
                            }

                            if (day > 31) {
                                invalidText = true;
                                errorMessage = "Invalid day, day must be between 01 and 31";
                            }
                            if (month > 12) {
                                invalidText = true;
                                errorMessage = "Invalid month, month must be between 01 and 12";
                            }

                            if (year < currentYear - 100 || year > currentYear + 100) {
                                invalidText = true;
                                errorMessage = "Please Enter More Suitable Year";
                            }

                            if (!invalidText) {
                                const datetime: Date = this.GetDate(year, month - 1, day, hour, minute, second);
                                this.DateValueChanged(datetime);
                            }
                        }
                        else if (this.InputValue.length == 4) {
                            //0206
                            dayString = this.InputValue.substring(0, 2);
                            monthString = this.InputValue.substring(2, 4);
                            day = Number(dayString);
                            month = Number(monthString);

                            if (day > 31) {
                                invalidText = true;
                                errorMessage = "Invalid day, day must be between 01 and 31";
                            }
                            if (month > 12) {
                                invalidText = true;
                                errorMessage = "Invalid month, month must be between 01 and 12";
                            }

                            if (!invalidText) {
                                const datetime: Date = this.GetDate(currentYear, month - 1, day, hour, minute, second);
                                this.DateValueChanged(datetime);
                            }
                        }
                        else if (this.InputValue.length == 2) {
                            dayString = this.InputValue.substring(0, 2);
                            day = Number(dayString);
                            if (day > 31) {
                                invalidText = true;
                                errorMessage = "Invalid day, day must be between 01 and 31";
                            }

                            if (!invalidText) {
                                const datetime: Date = this.GetDate(currentYear, currentMonth - 1, day, hour, minute, second);
                                this.DateValueChanged(datetime);
                            }
                        }
                        else if (this.InputValue.length == 1) {
                            dayString = this.InputValue;
                            day = Number(dayString);
                            if (day != 0) {
                                if (!invalidText) {
                                    const datetime: Date = this.GetDate(currentYear,currentMonth - 1,day,hour,minute,second);
                                    this.DateValueChanged(datetime);
                                }
                            }
                        }
                        else invalidText = true;
                    }
                    else invalidText = true;
                }
            }
        }
        else 
            this.DateValueChanged(null);

        if (invalidText || invalidDate) {
            if (errorMessage == "") errorMessage = TextCodeTranslator.Translate("General.O.InvalidInput");
            this.SetValidity(false, errorMessage);
            this.DataContext[this.ObjectFieldName] = null;
        }
        else 
            this.SetValidity(true, null);
        this.isTextChanged = false;

        this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
    }

    private GetTimeFor24Mode(hours: number, suffix: string) {
        if (suffix) {
            let convHour;
            if (suffix.toLowerCase() == "am") {
                if (hours >= 12) {
                    switch (hours) {
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23: {
                            convHour = hours - 12;
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
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11: {
                            convHour = hours + 12;
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

    private IsOldDate(date) {
        if (date && date.getFullYear() < new Date().getFullYear() - 100)
            return true;
        return false;
    }

    private GetTodaysDate() {
        let today: Date = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(today.getHours());
        today.setUTCMinutes(today.getMinutes());
        today.setUTCSeconds(today.getSeconds());
        today.setUTCMilliseconds(0);
        return today;
    }

    private DateValueChanged(date) {
        this.SelectedDate = date;
        this.SetDateValue(date);
        if (date) {
            this.InputValue = this.DateValue;
            this.selectedDateValue = this.DateValue + " " + this.TimeValue;
        }
        this.OnDropDownSelected(this.DateOptions.filter(d => d.Code == "SPD")[0], date, false);
    }
}
