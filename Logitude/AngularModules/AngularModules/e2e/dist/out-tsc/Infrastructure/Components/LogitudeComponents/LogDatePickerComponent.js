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
var UIProperties_1 = require("./UIProperties");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var FieldValidator_1 = require("../../Validators/FieldValidator");
var forms_1 = require("@angular/forms");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var LogDatePickerComponent = /** @class */ (function () {
    function LogDatePickerComponent(cd) {
        this.cd = cd;
        this.ShowHelp = false;
        this.ObjectFieldName = null;
        this.ObjectFieldHelp = null;
        this.ObjectTableName = null;
        this.HideColumns = false;
        this.HideLastColumn = false;
        this.IsFreeValue = false;
        this.ForceSubscribe = false;
        this.FocusOnMe = false;
        this.ValueChanged = new core_1.EventEmitter();
        this.Click = new core_1.EventEmitter();
        this.NoObjectField = false;
        this.NoValidation = false;
        this.DatePickerInputDivStyle = {};
        this.ShowErrorPopup = false;
        this.isFirstTime = true;
        this.isSelectedFromPicker = false;
        this.isKeyDown = false;
        this.OnBlurEvent = new core_1.EventEmitter();
        this.isCtrlKeyDown = false;
        this.isTextChanged = false;
        this.LayoutDirection = "ltr";
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.initialized = false;
        this.LostFocus = new core_1.EventEmitter();
        this.show = false;
        this.LayoutDirection =
            ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined
                ? "ltr"
                : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    }
    Object.defineProperty(LogDatePickerComponent.prototype, "RefreshMe", {
        get: function () {
            return this.refreshMe;
        },
        set: function (newValue) {
            if (this.refreshMe != newValue) {
                this.refreshMe = newValue;
                if (this.refreshMe) {
                    this.GetParsedDate(this.DataContext[this.ObjectFieldName]);
                }
                this.refreshMe = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogDatePickerComponent.prototype, "IsDisabled", {
        get: function () {
            return this.isDisabled;
        },
        set: function (newValue) {
            this.isDisabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogDatePickerComponent.prototype, "ForceFocus", {
        get: function () {
            return this.forceFocus;
        },
        set: function (newValue) {
            if (newValue) {
                var element = document.getElementById(this.DatePickerInputId);
                if (element) {
                    element.focus();
                }
            }
            this.forceFocus = false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogDatePickerComponent.prototype, "SelectedDate", {
        get: function () {
            return this.selectedDate;
        },
        set: function (newValue) {
            if (this.selectedDate != newValue) {
                this.selectedDate = newValue;
                this.ValueChanged.emit(this.selectedDate);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogDatePickerComponent.prototype, "SelectedDateValue", {
        get: function () {
            return this.selectedDateValue;
        },
        set: function (newValue) {
            var isOk = true;
            if (newValue) {
                isOk = false;
                if (newValue instanceof Date) {
                    isOk = true;
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
                    this.selectedDateValue = newValue;
                    if ((this.isKeyDown == false &&
                        this.isSelectedFromPicker == false) ||
                        this.IsFreeValue) {
                        this.SetDateValue(this.selectedDateValue, null, false);
                    }
                }
                this.isSelectedFromPicker = false;
                this.isKeyDown = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    LogDatePickerComponent.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    LogDatePickerComponent.prototype.SetControlIds = function (baseIdCombination) {
        this.DatePickerInputId = baseIdCombination;
        this.DropDownId = "dropdown_" + baseIdCombination;
        this.InputDivId = "datepickerinputdiv_" + baseIdCombination;
        this.ErrorPopUpId = "datepickererrorpop_" + baseIdCombination;
        if (this.InputType == "date") {
            this.CalendarButtonId = "calendarbutton_" + baseIdCombination;
        }
        else {
            this.CalendarButtonId = "timebutton_" + baseIdCombination;
        }
    };
    LogDatePickerComponent.prototype.InitializeEnability = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.uiProperty != null && this.uiProperty != undefined) {
            if (this.uiProperty.IsVisible) {
                if (this.IsDisabled) {
                    this.SetDisabled();
                }
                else {
                    this.SetEnabled();
                }
            }
        }
    };
    LogDatePickerComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.timerToken = setTimeout(function () { return _this.InitializeEnability(); }, 1);
        if (this.FocusOnMe) {
            var element = document.getElementById(this.DatePickerInputId);
            element.focus();
            this.CurrentSession.SessionEvent.emit({
                IsCell: true,
                Id: element.id,
                OnBlurEvent: this.OnBlurEvent
            });
            this.timerToken = setTimeout(function () {
                SelectingElement(element);
            }, 1);
            //this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id });
        }
    };
    LogDatePickerComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (!this.InputType) {
            this.InputType = "date";
        }
        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination =
                this.InputType +
                    "_" +
                    this.ObjectTableName +
                    "_" +
                    this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }
        if (this.counterId != null) {
            baseIdCombination =
                baseIdCombination + "_" + this.counterId.toString();
        }
        this.SetControlIds(baseIdCombination);
        if (this.FocusOnMe) {
            // it means it is inside a grid.
            this.CopyValueSubs = this.CurrentSession.CopyCellIntoMemory.subscribe(function (id) {
                if (id == _this.DatePickerInputId) {
                    //this.CurrentSession.CopiedCell = this.DataContext[this.ObjectFieldName];
                    _this.DataContext[_this.ObjectFieldName] =
                        _this.CurrentSession.CopiedCell;
                    _this.CurrentSession.CopiedCell = null;
                }
            });
            if (this.CurrentSession.CopiedCell) {
                //this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
                //this.CurrentSession.CopiedCell = null;
            }
        }
        this.CalendarButtonStyle = { border: "1px solid transparent" };
        var objectFieldAvailable = true;
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (table) {
            this.ObjectField = window.ObjectFields.filter(function (d) {
                return d.ObjectTableId === table.Id &&
                    d.FieldName === _this.ObjectFieldName;
            })[0];
            if (!this.ObjectField) {
                objectFieldAvailable = false;
            }
            else if (this.ObjectField.HelpTextCodeId != null) {
                this.ObjectFieldHelp = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectField.HelpTextTextCodeCode);
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                    if (this.ObjectFieldHelp.length > 1) {
                        this.ShowHelp = true;
                    }
                }
            }
        }
        else {
            objectFieldAvailable = false;
        }
        if (!this.TimeMode) {
            this.TimeMode = "12";
        }
        if (this.InputType == "date") {
            this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            this.DatePickerPlaceHolder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.EnterDate");
        }
        else {
            var dateUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName + "_timepicker", this.ObjectTableName);
            this.uiProperty.IsEnabled = dateUiProp.IsEnabled;
            this.uiProperty.IsVisible = dateUiProp.IsVisible;
            this.DatePickerPlaceHolder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.EnterTime");
        }
        this.IsDisabled = !this.uiProperty.IsEnabled;
        //this.ctrl = new FormControl(this.DataContext[this.ObjectFieldName]);
        //this.LogitudeForm.addControl(this.ObjectFieldName, this.ctrl);
        //if (objectFieldAvailable) {
        if (objectFieldAvailable || this.ForceSubscribe) {
            //this.ctrl.valueChanges.subscribe(res=> {
            //    this.uiProperty.UIPropertyChanged.emit("valuechanges");
            //    this.ValueChanged.emit(res);
            this.uiProperty.UIPropertyChanged.subscribe(function (value) {
                if (value == "datevaluechanges") {
                    if (_this.ObjectField && _this.ObjectField.IsCustom) {
                        var customFieldClass = _this
                            .DataContext[_this.ObjectFieldName];
                        if (customFieldClass != null &&
                            customFieldClass != undefined) {
                            var valueDate = customFieldClass.GetFieldDataTypeValue(_this.ObjectField, customFieldClass.Value);
                        }
                        else {
                            console.warn("Custom Fields are not implemented in: " +
                                _this.ObjectTableName);
                        }
                        _this.GetParsedDate(valueDate);
                    }
                    else {
                        _this.GetParsedDate(_this.DataContext[_this.ObjectFieldName]);
                    }
                    return;
                }
                if (value instanceof UIProperties_1.UIPropertyArgs) {
                    var uiPropertyArgs = value;
                    var uiProperty = uiPropertyArgs.uiProperty;
                    if (uiProperty.FieldName == _this.ObjectFieldName &&
                        uiProperty.ObjectTableName == _this.ObjectTableName) {
                        if (uiPropertyArgs.property == "IsEnabled") {
                            var isEnabled = uiPropertyArgs.newValue;
                            _this.IsDisabled = !isEnabled;
                            _this.uiProperty.IsEnabled = isEnabled;
                            if (_this.IsDisabled) {
                                _this.SetDisabled();
                            }
                            else {
                                _this.SetEnabled();
                            }
                        }
                        else if (uiPropertyArgs.property == "IsRequired" ||
                            uiPropertyArgs.property == "IsValid") {
                            if (!_this.isFirstTime) {
                                _this.ValidateField(false);
                            }
                            else {
                                _this.isFirstTime = false;
                            }
                        }
                        if (_this.InputType == "date") {
                            var timeUIProperty = _this.DataContext.UIProperties.GetUIProperty(_this.ObjectFieldName + "_timepicker", _this.ObjectTableName, _this.DataContext);
                            if (timeUIProperty != null &&
                                timeUIProperty != undefined) {
                                timeUIProperty.UIPropertyChanged.emit(value);
                            }
                        }
                    }
                    else if (uiProperty.FieldName.indexOf("_timepicker") > -1) {
                        if (uiPropertyArgs.property == "IsEnabled") {
                            var isEnabled = uiPropertyArgs.newValue;
                            _this.IsDisabled = !isEnabled;
                            _this.uiProperty.IsEnabled = isEnabled;
                            if (_this.IsDisabled) {
                                _this.SetDisabled();
                            }
                            else {
                                _this.SetEnabled();
                            }
                        }
                        else if (uiPropertyArgs.property == "IsRequired" ||
                            uiPropertyArgs.property == "IsValid") {
                            if (!_this.isFirstTime) {
                                _this.ValidateField(false);
                            }
                            else {
                                _this.isFirstTime = false;
                            }
                        }
                    }
                }
            });
        }
        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " DATEPICKER has no object field!");
        }
        var valueDate = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                valueDate = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);
            }
            else {
                console.warn("Custom Fields are not implemented in: " +
                    this.ObjectTableName);
            }
        }
        this.selectedDateValue = valueDate; //this.DataContext[this.ObjectFieldName];
        this.GetParsedDate(valueDate); //(this.DataContext[this.ObjectFieldName]);
        this.initialized = true;
    };
    LogDatePickerComponent.prototype.SetControlPropertiesAndValidations = function (uiProperty, ctrl) {
        var _this = this;
        this.uiProperty.IsRequired = uiProperty.IsRequired;
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        var field = window.ObjectFields.filter(function (d) {
            return d.ObjectTableId === table.Id &&
                d.FieldName === _this.ObjectFieldName;
        })[0];
        var minlength = field.MinLength;
        var maxlenght = field.MaxLength;
        var hasminmax;
        hasminmax = false;
        if (field.DataTypeCode.toLowerCase() == "text" ||
            field.DataTypeCode.toLowerCase() == "ntext") {
            if (maxlenght != 0) {
                hasminmax = true;
            }
        }
        if (this.uiProperty.IsRequired) {
            var valueDate = this.DataContext[this.ObjectFieldName];
            if (this.ObjectField && this.ObjectField.IsCustom) {
                var customFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    valueDate = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value);
                }
                else {
                    console.warn("Custom Fields are not implemented in: " +
                        this.ObjectTableName);
                }
            }
            if (valueDate == null || valueDate == "") {
                this.ctrl.setErrors({ required: true });
            }
            if (hasminmax) {
                this.ctrl.validator = forms_1.Validators.compose([
                    forms_1.Validators.required,
                    forms_1.Validators.minLength(minlength),
                    forms_1.Validators.maxLength(maxlenght)
                ]);
            }
            else {
                this.ctrl.validator = forms_1.Validators.required;
            }
        }
        else if (!this.uiProperty.ValidValue) {
            this.ctrl.setErrors({ error: this.uiProperty.ValidationError });
            this.DatePickerInputDivStyle = { border: "1px solid #ff0000" };
        }
        else {
            if (this.show) {
                this.DatePickerInputDivStyle = { border: "1px solid #3BB3E2" };
            }
            else {
                this.DatePickerInputDivStyle = null;
            }
            this.ctrl.setErrors(null);
            if (hasminmax) {
                this.ctrl.validator = forms_1.Validators.compose([
                    forms_1.Validators.minLength(minlength),
                    forms_1.Validators.maxLength(maxlenght)
                ]);
            }
            else {
                this.ctrl.validator = null;
            }
        }
    };
    LogDatePickerComponent.prototype.onFocus = function () {
        this.show = true;
        if (this.uiProperty.ValidValue) {
            this.DatePickerInputDivStyle = { border: "1px solid #3BB3E2" };
            this.ShowErrorPopup = false;
        }
        else {
            this.DatePickerInputDivStyle = { border: "1px solid #ff0000" };
            this.ShowErrorPopup = true;
        }
        this.Click.emit("");
        if (!this.IsDateDropDownOpen) {
            var input = document.getElementById(this.DatePickerInputId);
            //input.select();
            SelectingElement(input);
        }
    };
    LogDatePickerComponent.prototype.OnBtnFocus = function () {
        this.Click.emit("");
    };
    LogDatePickerComponent.prototype.onBlur = function () {
        this.ShowErrorPopup = false;
        this.show = false;
        if (this.uiProperty.ValidValue) {
            this.DatePickerInputDivStyle = null;
        }
        if (!this.MouseInArea) {
            if (this.IsCalendarOpen) {
                this.ToggleCalendar();
            }
            if (this.InputType == "date" && this.isTextChanged) {
                this.GetDateValue();
            }
            else if (this.isTextChanged) {
                this.GetTimeValue();
            }
            this.OnBlurEvent.emit({ Id: this.DatePickerInputId });
            this.LostFocus.emit(true);
        }
        this.isTextChanged = false;
    };
    LogDatePickerComponent.prototype.GetParsedDate = function (value) {
        if (value) {
            var arr;
            var datev;
            var hour = 0;
            var minute = 0;
            var second = 0;
            var day;
            var month;
            var year;
            if (typeof value == "string") {
                var stringValue = value;
                datev = this.GetDateFromString(stringValue);
                var dateparts = this.GetDateParts(datev);
                day = dateparts[2];
                month = dateparts[1] + 1;
                year = dateparts[0];
                hour = dateparts[3];
                minute = dateparts[4];
                second = dateparts[5];
                this.SelectedDate = datev;
                this.TimeValue =
                    this.ApplyPadding(hour.toString()) +
                        ":" +
                        this.ApplyPadding(minute.toString()) +
                        ":" +
                        this.ApplyPadding(second.toString());
            }
            else {
                datev = value;
                this.SelectedDate = value;
                var dateparts = this.GetDateParts(datev);
                day = dateparts[2];
                month = dateparts[1] + 1;
                year = dateparts[0];
                hour = dateparts[3];
                minute = dateparts[4];
                second = dateparts[5];
                this.TimeValue =
                    this.ApplyPadding(hour.toString()) +
                        ":" +
                        this.ApplyPadding(minute.toString()) +
                        ":" +
                        this.ApplyPadding(second.toString());
            }
            this.DateValue =
                year +
                    "/" +
                    this.ApplyPadding(month.toString()) +
                    "/" +
                    this.ApplyPadding(day.toString());
            if (this.InputType == "date") {
                // ShortDateString
                if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.DateTimeFormat)) {
                    var myDateTimeFormatPrefix = SessionLocator_1.SessionLocator.TenantPM.DateTimeFormat.toLowerCase().substring(0, 2);
                    if (myDateTimeFormatPrefix == "mm") {
                        this.InputValue =
                            this.ApplyPadding(month.toString()) +
                                "/" +
                                this.ApplyPadding(day.toString()) +
                                "/" +
                                year;
                    }
                    else {
                        this.InputValue =
                            this.ApplyPadding(day.toString()) +
                                "/" +
                                this.ApplyPadding(month.toString()) +
                                "/" +
                                year;
                    }
                }
                else {
                    this.InputValue =
                        this.ApplyPadding(day.toString()) +
                            "/" +
                            this.ApplyPadding(month.toString()) +
                            "/" +
                            year;
                }
            }
            else {
                if (this.TimeMode == "12" &&
                    (this.TimeValue.indexOf("A") <= -1 &&
                        this.TimeValue.indexOf("P") <= -1)) {
                    var timearr = this.TimeValue.split(":");
                    var hourRes = this.GetTimeModeHours(Number(timearr[0]));
                    var hourResArr = hourRes.split(",");
                    this.TimeValue =
                        hourResArr[0] +
                            ":" +
                            timearr[1] +
                            ":" +
                            timearr[2] +
                            " " +
                            hourResArr[1];
                }
                var timeArray = this.TimeValue.split(":");
                if (this.TimeValue.indexOf("AM") > -1 ||
                    this.TimeValue.indexOf("PM") > -1) {
                    var secondsWithsuffix = timeArray[2].split(" ");
                    var suffix = secondsWithsuffix[1];
                    this.InputValue =
                        this.ApplyPadding(timeArray[0]) +
                            ":" +
                            timeArray[1] +
                            " " +
                            suffix;
                }
                else {
                    this.InputValue =
                        this.ApplyPadding(timeArray[0]) + ":" + timeArray[1]; //this.TimeValue;
                }
                // this.InputValue = this.TimeValue;
            }
        }
        else {
            if (this.InputType == "time") {
                this.DateValue = null;
                this.InputValue = null;
                this.SelectedDate = null;
                this.TimeValue = null;
            }
            else if (this.InputType == "date") {
                if (this.SelectedDate) {
                    var dateparts = this.GetDateParts(this.SelectedDate);
                    var y = dateparts[0]; //this.SelectedDate.getFullYear();
                    var m = dateparts[1]; //this.SelectedDate.getMonth();
                    var d = dateparts[2]; //this.SelectedDate.getDate();
                    this.SelectedDate = this.GetDate(y, m, d, 0, 0, 0);
                }
            }
        }
    };
    LogDatePickerComponent.prototype.onkeydown = function (event) {
        this.isKeyDown = true;
        var key = event.keyCode;
        var keyChar = event.key;
        var success;
        var uipr = this.uiProperty;
        if (key == 17) {
            this.isCtrlKeyDown = true;
        }
        if (key != 13 && key != 9 && key != 17) {
            this.isTextChanged = true;
        }
        if (this.IsCalendarOpen) {
            this.ToggleCalendar();
        }
        if (key == 9) {
            this.MouseInArea = false;
        }
        switch (this.InputType) {
            case "date": {
                if ((key >= 48 && key <= 57) ||
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
                    key == 17) {
                    success = true;
                }
                else {
                    success = false;
                }
                break;
            }
            case "time": {
                if ((key >= 48 && key <= 57) ||
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
                    keyChar == ":") {
                    success = true;
                }
                else {
                    success = false;
                }
                break;
            }
        }
        if (key == 13 || key == 9) {
            if (this.InputType == "date" && this.isTextChanged) {
                this.GetDateValue();
            }
            else if (this.isTextChanged) {
                this.GetTimeValue();
            }
            //if (this.IsCalendarOpen)
            //{
            //    this.ToggleCalendar();
            //}
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
    };
    LogDatePickerComponent.prototype.onkeyup = function (event) {
        if (event.keyCode == 17) {
            this.isCtrlKeyDown = false;
        }
        if (event.keyCode == 8) {
            if (this.InputValue == "" ||
                this.InputValue == undefined ||
                this.InputValue == null) {
                if (this.InputType == "date") {
                    this.SetDateValue(null);
                }
                else {
                    if (this.SelectedDate != null &&
                        this.SelectedDate != undefined) {
                        var dateCombination = "date" +
                            "_" +
                            this.ObjectTableName +
                            "_" +
                            this.ObjectFieldName;
                        var dateelement = document.getElementById(dateCombination);
                        if (dateelement) {
                            this.SelectedDate.setUTCHours(0);
                            this.SelectedDate.setUTCMinutes(0);
                            this.SelectedDate.setUTCSeconds(0);
                            this.SelectedDate.setUTCMilliseconds(0);
                            this.SetDateValue(this.SelectedDate);
                        }
                        else {
                            this.SetDateValue(null);
                        }
                    }
                }
            }
        }
    };
    LogDatePickerComponent.prototype.SetDateValue = function (date, timeSuffix, setDataContext) {
        if (timeSuffix === void 0) { timeSuffix = null; }
        if (setDataContext === void 0) { setDataContext = true; }
        if (this.initialized || this.IsFreeValue) {
            var invalidDate;
            if (date) {
                var day;
                var month;
                var year;
                var hour;
                var minute;
                var second;
                var dateparts = this.GetDateParts(date);
                day = dateparts[2]; //date.getDate();
                month = dateparts[1] + 1; //date.getMonth() + 1;
                year = dateparts[0]; //date.getFullYear();
                hour = dateparts[3]; //date.getHours();
                minute = dateparts[4]; //date.getMinutes();
                second = dateparts[5]; //date.getSeconds();
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
                var hourRes = this.GetTimeModeHours(Number(timearr[0]), timeSuffix);
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
                }
                else {
                    this.TimeValue =
                        this.ApplyPadding(hourResArr[0]) +
                            ":" +
                            timearr[1] +
                            ":" +
                            timearr[2];
                }
                var datestring = this.DateValue + " " + this.TimeValue;
                if (this.InputType == "date") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.DateTimeFormat)) {
                        var myDateTimeFormatPrefix = SessionLocator_1.SessionLocator.TenantPM.DateTimeFormat.toLowerCase().substring(0, 2);
                        if (myDateTimeFormatPrefix == "mm") {
                            this.InputValue =
                                this.ApplyPadding(month.toString()) +
                                    "/" +
                                    this.ApplyPadding(day.toString()) +
                                    "/" +
                                    year;
                        }
                        else {
                            this.InputValue =
                                this.ApplyPadding(day.toString()) +
                                    "/" +
                                    this.ApplyPadding(month.toString()) +
                                    "/" +
                                    year;
                        }
                    }
                    else {
                        this.InputValue =
                            this.ApplyPadding(day.toString()) +
                                "/" +
                                this.ApplyPadding(month.toString()) +
                                "/" +
                                year;
                    }
                }
                else {
                    var timeArray = this.TimeValue.split(":");
                    if (this.TimeValue.indexOf("AM") > -1 ||
                        this.TimeValue.indexOf("PM") > -1) {
                        var secondsWithsuffix = timeArray[2].split(" ");
                        var suffix = secondsWithsuffix[1];
                        this.InputValue =
                            this.ApplyPadding(timeArray[0]) +
                                ":" +
                                timeArray[1] +
                                " " +
                                suffix;
                    }
                    else {
                        this.InputValue =
                            this.ApplyPadding(timeArray[0]) +
                                ":" +
                                timeArray[1]; //this.TimeValue;
                    }
                }
                var dateval = this.GetDateFromString(datestring);
                var dateparts = this.GetDateParts(dateval);
                this.SelectedDate = this.GetDate(dateparts[0], dateparts[1], dateparts[2], dateparts[3], dateparts[4], dateparts[5]);
                if (setDataContext == true) {
                    if (this.ObjectField && this.ObjectField.IsCustom) {
                        var customFieldClass = this
                            .DataContext[this.ObjectFieldName];
                        if (customFieldClass != null &&
                            customFieldClass != undefined) {
                            customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.SelectedDate);
                        }
                        else {
                            console.warn("Custom Fields are not implemented in: " +
                                this.ObjectTableName);
                        }
                        this.DataContext[this.ObjectFieldName] = customFieldClass;
                    }
                    else {
                        this.DataContext[this.ObjectFieldName] = this.SelectedDate;
                    }
                }
                var dateUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
                var timeUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName + "_timepicker", this.ObjectTableName, this.DataContext);
                timeUiProp.UIPropertyChanged.emit("datevaluechanges");
                dateUiProp.UIPropertyChanged.emit("datevaluechanges");
                if (!this.IsFreeValue) {
                    this.SetValidity(true, null);
                    this.ValidateField();
                }
            }
            else {
                this.SelectedDate = null;
                this.InputValue = null;
                this.TimeValue = null;
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null &&
                        customFieldClass != undefined) {
                        customFieldClass.Value = null;
                        this.DataContext[this.ObjectFieldName] = customFieldClass;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " +
                            this.ObjectTableName);
                    }
                }
                else {
                    this.DataContext[this.ObjectFieldName] = null;
                }
                this.ValidateField();
                var dateUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
                var timeUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName + "_timepicker", this.ObjectTableName, this.DataContext);
                if (timeUiProp != null && timeUiProp != undefined) {
                    timeUiProp.UIPropertyChanged.emit("datevaluechanges");
                }
                dateUiProp.UIPropertyChanged.emit("datevaluechanges");
            }
        }
    };
    LogDatePickerComponent.prototype.SetTimeValue = function (date, timeSuffix) {
        if (timeSuffix === void 0) { timeSuffix = null; }
        if (this.initialized) {
            var invalidDate;
            if (date) {
                var day;
                var month;
                var year;
                var hour;
                var minute;
                var second;
                var dateparts = this.GetDateParts(date);
                //day = this.DateValue
                //month = dateparts[1] + 1; //date.getMonth() + 1;
                //year = dateparts[0];//date.getFullYear();
                hour = date.getHours();
                minute = date.getMinutes();
                second = 0;
                //this.DateValue = year + '/' + this.ApplyPadding(month.toString()) + '/' + this.ApplyPadding(day.toString());
                this.TimeValue =
                    this.ApplyPadding(hour.toString()) +
                        ":" +
                        this.ApplyPadding(minute.toString()) +
                        ":" +
                        this.ApplyPadding(second.toString());
                var timearr = this.TimeValue.split(":");
                var hourRes = this.GetTimeModeHours(Number(timearr[0]), timeSuffix);
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
                }
                else {
                    this.TimeValue =
                        this.ApplyPadding(hourResArr[0]) +
                            ":" +
                            timearr[1] +
                            ":" +
                            timearr[2];
                }
                var datestring = this.DateValue + " " + this.TimeValue;
                //console.log("DATETIME: ", datestring);
                var timeArray = this.TimeValue.split(":");
                if (this.TimeValue.indexOf("AM") > -1 ||
                    this.TimeValue.indexOf("PM") > -1) {
                    var secondsWithsuffix = timeArray[2].split(" ");
                    var suffix = secondsWithsuffix[1];
                    this.InputValue =
                        this.ApplyPadding(timeArray[0]) +
                            ":" +
                            timeArray[1] +
                            " " +
                            suffix;
                }
                else {
                    this.InputValue =
                        this.ApplyPadding(timeArray[0]) + ":" + timeArray[1]; //this.TimeValue;
                }
                // this.InputValue = this.TimeValue;
                var dateval = this.GetDateFromString(datestring);
                var dateparts = this.GetDateParts(dateval);
                this.SelectedDate = this.GetDate(dateparts[0], dateparts[1], dateparts[2], dateparts[3], dateparts[4], dateparts[5]);
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null &&
                        customFieldClass != undefined) {
                        customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.SelectedDate);
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " +
                            this.ObjectTableName);
                    }
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    this.DataContext[this.ObjectFieldName] = this.SelectedDate;
                }
                this.SetValidity(true, null);
                this.ValidateField();
                var timeUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName + "_timepicker", this.ObjectTableName, this.DataContext);
                timeUiProp.UIPropertyChanged.emit("datevaluechanges");
                this.uiProperty.UIPropertyChanged.emit("datevaluechanges");
            }
            else {
                this.SelectedDate = null;
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null &&
                        customFieldClass != undefined) {
                        customFieldClass.Value = null;
                        this.DataContext[this.ObjectFieldName] = customFieldClass;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " +
                            this.ObjectTableName);
                    }
                }
                else {
                    this.DataContext[this.ObjectFieldName] = null;
                }
                this.ValidateField();
                var timeUiProp = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName + "_timepicker", this.ObjectTableName, this.DataContext);
                timeUiProp.UIPropertyChanged.emit("datevaluechanges");
                this.uiProperty.UIPropertyChanged.emit("datevaluechanges");
            }
        }
    };
    LogDatePickerComponent.prototype.GetDateValue = function () {
        if (this.InputValue && this.InputValue != "") {
            if (!this.TimeValue) {
                this.TimeValue = "00:00:00";
            }
            var suffix = null;
            if (this.TimeValue.indexOf("AM") > -1 ||
                this.TimeValue.indexOf("PM") > -1) {
                suffix = this.TimeValue.split(" ")[1];
                this.TimeValue = this.TimeValue.split(" ")[0];
            }
            var timearr = this.TimeValue.split(":");
            var hour = 0; // Number(timearr[0]);
            if (suffix != null) {
                hour = this.GetTimeFor24Mode(Number(timearr[0]), suffix);
            }
            else {
                hour = Number(timearr[0]);
            }
            var minute = Number(timearr[1]);
            var second = Number(timearr[2]);
            var todayDateTime = this.GetTodaysDate();
            var dateparts = this.GetDateParts(todayDateTime);
            var date = this.GetDate(dateparts[0], dateparts[1], dateparts[2], hour, minute, second);
            var invalidText = false;
            var invalidDate = false;
            var errorMessage = "";
            var val = this.InputValue.trim();
            if (val.length > 10) {
                invalidText = true;
            }
            if (!invalidText) {
                if (this.InputValue.trim() == ".") {
                    this.SetDateValue(date);
                }
                else if (this.InputValue.indexOf("+") == 0 ||
                    this.InputValue.indexOf("-") == 0) {
                    if (this.InputValue.length > 1) {
                        var days = 0;
                        var sign = this.InputValue.substring(0, 1);
                        var daysString = this.InputValue.substring(1, this.InputValue.length);
                        if (sign == "+") {
                            days = Number(daysString);
                            if (days) {
                                date.setDate(date.getDate() + days);
                            }
                            else {
                                invalidText = true;
                            }
                        }
                        else if (sign == "-") {
                            days = Number(daysString);
                            if (days) {
                                date.setDate(date.getDate() - days);
                            }
                            else {
                                invalidText = true;
                            }
                        }
                    }
                    if (!invalidText) {
                        this.SetDateValue(date);
                    }
                }
                else if ((this.InputValue != "." &&
                    this.InputValue.indexOf(".") > -1) ||
                    (this.InputValue != "/" &&
                        this.InputValue.indexOf("/") > -1) ||
                    (this.InputValue != "-" &&
                        this.InputValue.indexOf("-") > -1)) {
                    var nowDate = this.GetTodaysDate();
                    var dateStrings;
                    if (this.InputValue.indexOf(".") > -1) {
                        dateStrings = this.InputValue.split(".");
                    }
                    else if (this.InputValue.indexOf("-") > -1) {
                        dateStrings = this.InputValue.split("-");
                    }
                    else {
                        dateStrings = this.InputValue.split("/");
                    }
                    var nowdateparts = this.GetDateParts(nowDate);
                    var day;
                    var month;
                    var year;
                    var currentYear = nowdateparts[0]; //nowDate.getFullYear();
                    var currentMonth = nowdateparts[1] + 1; //nowDate.getMonth() + 1;
                    var currentYearMillinium = currentYear
                        .toString()
                        .substring(0, 1);
                    currentYearMillinium = currentYearMillinium + "000";
                    var currentMillinium = Number(currentYearMillinium);
                    if (dateStrings.length == 3) {
                        if (dateStrings[0].length > 2 &&
                            dateStrings[2].length <= 2) {
                            year = Number(dateStrings[0]);
                            month = Number(dateStrings[1]);
                            day = Number(dateStrings[2]);
                        }
                        else if (dateStrings[2].length >= 2 &&
                            dateStrings[0].length <= 2) {
                            day = Number(dateStrings[0]);
                            month = Number(dateStrings[1]);
                            year = Number(dateStrings[2]);
                        }
                        else {
                            invalidText = true;
                        }
                        //calculating year
                        if (year == 0) {
                            year = currentYear;
                        }
                        if (year < 1000) {
                            year = year + currentMillinium;
                        }
                        if (month > 12 || day > 31) {
                            invalidText = true;
                        }
                        if (!invalidText) {
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (dateStrings.length == 2) {
                        day = Number(dateStrings[0]);
                        month = Number(dateStrings[1]);
                        if (month == 0) {
                            month = currentMonth;
                        }
                        if (month > 12 || day > 31) {
                            invalidText = true;
                        }
                        if (!invalidText) {
                            var datetime = this.GetDate(currentYear, month - 1, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (dateStrings.length == 1) {
                        day = Number(dateStrings[0]);
                        if (day > 31) {
                            invalidText = true;
                        }
                        if (!invalidText) {
                            var datetime = this.GetDate(currentYear, currentMonth, day, hour, minute, second);
                            this.SetDateValue(datetime);
                        }
                    }
                }
                else {
                    var valid = Number(this.InputValue);
                    if (valid) {
                        var nowdateparts = this.GetDateParts(date);
                        var day;
                        var month;
                        var year;
                        var dayString;
                        var monthString;
                        var yearString;
                        var currentYear = nowdateparts[0]; //date.getFullYear();
                        var currentMonth = nowdateparts[1] + 1; //date.getMonth() + 1;
                        var currentYearMillinium = currentYear
                            .toString()
                            .substring(0, 1);
                        currentYearMillinium = currentYearMillinium + "000";
                        var currentMillinium = Number(currentYearMillinium);
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
                                errorMessage =
                                    "Invalid day, day must be between 01 and 31";
                            }
                            if (month > 12) {
                                invalidText = true;
                                errorMessage =
                                    "Invalid month, month must be between 01 and 12";
                            }
                            //calculating year
                            if (year == 0) {
                                year = currentYear;
                            }
                            if (year < 1000) {
                                year = year + currentMillinium;
                            }
                            if (year < currentYear - 100 ||
                                year > currentYear + 100) {
                                invalidText = true;
                                errorMessage =
                                    "Please Enter More Suitable Year";
                            }
                            if (!invalidText) {
                                var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                                this.SetDateValue(datetime);
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
                                errorMessage =
                                    "Invalid month, month must be between 01 and 12";
                            }
                            if (year < currentYear - 100 ||
                                year > currentYear + 100) {
                                invalidText = true;
                                errorMessage =
                                    "Please Enter More Suitable Year";
                            }
                            if (!invalidText) {
                                var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                                this.SetDateValue(datetime);
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
                                errorMessage =
                                    "Invalid day, day must be between 01 and 31";
                            }
                            if (month > 12) {
                                invalidText = true;
                                errorMessage =
                                    "Invalid month, month must be between 01 and 12";
                            }
                            if (!invalidText) {
                                var datetime = this.GetDate(currentYear, month - 1, day, hour, minute, second);
                                this.SetDateValue(datetime);
                            }
                        }
                        else if (this.InputValue.length == 2) {
                            var dayDigits = 2;
                            dayString = this.InputValue.substring(0, 2);
                            day = Number(dayString);
                            if (day > 31) {
                                invalidText = true;
                                errorMessage =
                                    "Invalid day, day must be between 01 and 31";
                            }
                            if (!invalidText) {
                                var datetime = this.GetDate(currentYear, currentMonth - 1, day, hour, minute, second);
                                this.SetDateValue(datetime);
                            }
                        }
                        else if (this.InputValue.length == 1) {
                            dayString = this.InputValue;
                            day = Number(dayString);
                            if (day != 0) {
                                if (!invalidText) {
                                    var datetime = this.GetDate(currentYear, currentMonth - 1, day, hour, minute, second);
                                    this.SetDateValue(datetime);
                                }
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
        }
        else {
            this.SetDateValue(null);
        }
        if (!invalidText) {
            if (this.SelectedDate) {
                if (this.SelectedDate.getFullYear() <
                    new Date().getFullYear() - 100) {
                    invalidDate = true;
                    errorMessage = "Date time is too way in the past!"; //yet to be translated.
                }
            }
        }
        if (invalidText || invalidDate) {
            if (errorMessage == "") {
                //errorMessage = 'Invalid Input';//yet to be translated.
                errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput");
            }
            this.SetValidity(false, errorMessage);
        }
        else {
            this.SetValidity(true, null);
        }
        this.isTextChanged = false;
        this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
    };
    LogDatePickerComponent.prototype.GetTimeValue = function () {
        var errorMessage = null;
        if (this.InputValue) {
            var date;
            if (this.DateValue) {
                date = this.GetDateFromString(this.DateValue);
            }
            else {
                date = this.GetTodaysDate();
            }
            var dateparts = this.GetDateParts(date);
            var day;
            var month;
            var year;
            day = dateparts[2]; //date.getDate();
            month = dateparts[1] + 1; //date.getMonth() + 1;
            year = dateparts[0]; // date.getFullYear();
            var invalidText = false;
            var suffix;
            if (this.InputValue.toLowerCase().indexOf("m") > -1) {
                if (this.InputValue.toLowerCase().indexOf("p") > -1) {
                    suffix = "PM";
                }
                else {
                    suffix = "AM";
                }
                this.InputValue = this.InputValue.substring(0, this.InputValue.toLowerCase().indexOf("m") - 1);
            }
            if (this.InputValue.indexOf("+") == 0 ||
                this.InputValue.indexOf("-") == 0) {
                if (this.InputValue.length > 1) {
                    var hours = 0;
                    var sign = this.InputValue.substring(0, 1);
                    var hoursString = this.InputValue.substring(1, this.InputValue.length);
                    if (sign == "+") {
                        hours = Number(hoursString);
                        if (hours) {
                            var haha = date.getTime();
                            date.setTime(date.getTime() + hours * 60 * 60 * 1000);
                        }
                        else {
                            invalidText = true;
                        }
                    }
                    else if (sign == "-") {
                        hours = Number(hoursString);
                        if (hours) {
                            date.setTime(date.getTime() - hours * 60 * 60 * 1000);
                        }
                        else {
                            invalidText = true;
                        }
                    }
                }
                this.SetDateValue(date, suffix);
            }
            else if ((this.InputValue != "." && this.InputValue.indexOf(".") > -1) ||
                (this.InputValue != "/" && this.InputValue.indexOf("/") > -1) ||
                (this.InputValue != "-" && this.InputValue.indexOf("-") > -1) ||
                (this.InputValue != ":" && this.InputValue.indexOf(":") > -1)) {
                var dateStrings;
                if (this.InputValue.indexOf(".") > -1) {
                    dateStrings = this.InputValue.split(".");
                }
                else if (this.InputValue.indexOf("-") > -1) {
                    dateStrings = this.InputValue.split("-");
                }
                else if (this.InputValue.indexOf(":") > -1) {
                    dateStrings = this.InputValue.split(":");
                }
                else {
                    dateStrings = this.InputValue.split("/");
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
                    var datetime = this.GetDate(dateparts[0], dateparts[1], dateparts[2], hour, minute, second);
                    if (datetime.toString() == "Invalid Date") {
                        invalidText = true;
                    }
                    else {
                        this.SetDateValue(datetime, suffix);
                    }
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                            if (datetime.toString() == "Invalid Date") {
                                invalidText = true;
                            }
                            else {
                                this.SetDateValue(datetime, suffix);
                            }
                            //this.SetDateValue(datetime, suffix);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                            if (datetime.toString() == "Invalid Date") {
                                invalidText = true;
                            }
                            else {
                                this.SetDateValue(datetime, suffix);
                            }
                            //this.SetDateValue(datetime, suffix);
                        }
                    }
                    else if (this.InputValue.length == 4) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = this.InputValue.substring(2, 4);
                        secondString = "0";
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                            if (datetime.toString() == "Invalid Date") {
                                invalidText = true;
                            }
                            else {
                                this.SetDateValue(datetime, suffix);
                            }
                            //this.SetDateValue(datetime, suffix);
                        }
                    }
                    else if (this.InputValue.length == 3) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = this.InputValue.substring(2, 3);
                        secondString = "0"; //this.InputValue.substring(4, 5);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
                            if (datetime.toString() == "Invalid Date") {
                                invalidText = true;
                            }
                            else {
                                this.SetDateValue(datetime, suffix);
                            }
                            //this.SetDateValue(datetime, suffix);
                        }
                    }
                    else if (this.InputValue.length == 2) {
                        hourString = this.InputValue.substring(0, 2);
                        minuteString = "0";
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, 0);
                            if (datetime.toString() == "Invalid Date") {
                                invalidText = true;
                            }
                            else {
                                this.SetDateValue(datetime, suffix);
                            }
                            //this.SetDateValue(datetime, suffix);
                        }
                    }
                    else if (this.InputValue.length == 1) {
                        hourString = this.InputValue;
                        hour = Number(hourString);
                        if (!invalidText) {
                            var datetime = this.GetDate(year, month - 1, day, hour, 0, 0);
                            if (datetime.toString() == "Invalid Date") {
                                invalidText = true;
                            }
                            else {
                                this.SetDateValue(datetime, suffix);
                            }
                            //this.SetDateValue(datetime, suffix);
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
            errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput"); //'Invalid Input';
            this.SetValidity(false, errorMessage);
        }
        else {
            this.SetValidity(true, null);
        }
    };
    LogDatePickerComponent.prototype.ApplyPadding = function (str) {
        var pad = "00";
        var ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    };
    LogDatePickerComponent.prototype.GetTimeModeHours = function (hours, suffix) {
        if (suffix === void 0) { suffix = null; }
        if (isNaN(hours)) {
            hours = 0;
        }
        if (this.TimeMode == "12") {
            if (hours <= 11) {
                if (hours == 0) {
                    hours = 12;
                }
                return hours.toString() + "," + "AM";
            }
            else {
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
        }
        else {
            hours = this.GetTimeFor24Mode(hours, suffix);
        }
        return hours.toString();
    };
    LogDatePickerComponent.prototype.GetTimeFor24Mode = function (hours, suffix) {
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
    };
    LogDatePickerComponent.prototype.ToggleCalendar = function () {
        if (!this.IsCalendarOpen) {
            if (this.InputType == "date") {
                this.IsDateDropDownOpen = false;
                this.IsDateDropDownOpen = true;
            }
            else if (this.InputType == "time") {
                this.IsTimeDropDownOpen = true;
            }
            this.IsCalendarOpen = true;
            //this.DatePickerCalendarStyle = {
            //    'visibility': 'visible'
            //};
            this.CalendarButtonStyle = { background: "#808080" };
            var inputElement = document.getElementById(this.DatePickerInputId);
            inputElement.focus();
        }
        else {
            this.IsDateDropDownOpen = false;
            this.IsTimeDropDownOpen = false;
            this.IsCalendarOpen = false;
            //this.DatePickerCalendarStyle = {
            //    'visibility': 'hidden'
            //};
            this.CalendarButtonStyle = null;
            var elem = document.getElementById(this.DatePickerInputId);
            elem.focus();
        }
    };
    LogDatePickerComponent.prototype.OnMouseOver = function () {
        this.MouseInArea = true;
    };
    LogDatePickerComponent.prototype.OnMouseOut = function () {
        this.MouseInArea = false;
    };
    LogDatePickerComponent.prototype.OnCalendarMouseOver = function () {
        this.MouseInArea = true;
        var inputElement = document.getElementById(this.DatePickerInputId);
        inputElement.focus();
    };
    LogDatePickerComponent.prototype.OnCalendarMouseOut = function () {
        this.MouseInArea = false;
        var inputElement = document.getElementById(this.DatePickerInputId);
        inputElement.focus();
    };
    LogDatePickerComponent.prototype.OnSelectedDateChanged = function (selectedDateObj) {
        this.isSelectedFromPicker = true;
        var selectedDate = selectedDateObj.SelectedDate;
        this.SetDateValue(selectedDate, selectedDateObj.Suffix);
        this.ToggleCalendar();
    };
    LogDatePickerComponent.prototype.OnSelectedTimeChanged = function (selectedDateObj) {
        this.isSelectedFromPicker = true;
        var selectedDate = selectedDateObj.SelectedTime;
        this.SetDateValue(selectedDate, selectedDateObj.Suffix);
        this.ToggleCalendar();
    };
    LogDatePickerComponent.prototype.OnMouseWeel = function () {
        if (this.IsCalendarOpen)
            this.ToggleCalendar();
    };
    LogDatePickerComponent.prototype.OnChange = function (event) {
        //console.log(event+"afasdfasdfasfasdfsdaf");
    };
    LogDatePickerComponent.prototype.SetDisabled = function () {
        var inputDiv = document.getElementById(this.InputDivId); //("DatePickerInputDiv");
        if (inputDiv != null && inputDiv != undefined) {
            inputDiv.classList.add("DatePickerInputDivDisabled");
        }
    };
    LogDatePickerComponent.prototype.SetEnabled = function () {
        var inputDiv = document.getElementById(this.InputDivId); //("DatePickerInputDiv");
        if (inputDiv != null && inputDiv != undefined) {
            inputDiv.classList.remove("DatePickerInputDivDisabled");
        }
    };
    LogDatePickerComponent.prototype.GetDate = function (year, month, day, hour, minute, second) {
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
    LogDatePickerComponent.prototype.GetDateFromString = function (datestring) {
        //2016/08/14 05:00:00
        //2016-08-14T05:00:00
        //2016/08/14 05:00:00 PM
        //console.log("this is the date string that arrived " + datestring);
        var dateAndTime;
        var suffix;
        if (datestring.indexOf("T") > -1) {
            dateAndTime = datestring.split("T");
        }
        else {
            dateAndTime = datestring.split(" ");
        }
        var dateArray;
        if (dateAndTime[0].indexOf("/") > -1) {
            dateArray = dateAndTime[0].split("/");
        }
        else if (dateAndTime[0].indexOf("-") > -1) {
            dateArray = dateAndTime[0].split("-");
        }
        else if (dateAndTime[0].indexOf(".") > -1) {
            dateArray = dateAndTime[0].split(".");
        }
        if (dateAndTime.length > 2) {
            suffix = dateAndTime[2];
        }
        var timeArray;
        var hour = 0;
        var minute = 0;
        var second = 0;
        if (dateAndTime.length >= 2) {
            if (dateAndTime[1].indexOf(".") > -1) {
                timeArray = dateAndTime[1].split(".")[0].split(":");
            }
            else {
                timeArray = dateAndTime[1].split(":");
            }
            var hour = this.GetTimeFor24Mode(Number(timeArray[0]), suffix);
            var minute = Number(timeArray[1]);
            var second = Number(timeArray[2].substring(0, 2));
        }
        var year = Number(dateArray[0]);
        var month = Number(dateArray[1]) - 1;
        var day = Number(dateArray[2]);
        var date = this.GetDate(year, month, day, hour, minute, second);
        return date;
    };
    LogDatePickerComponent.prototype.GetTodaysDate = function () {
        var today = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(today.getHours());
        today.setUTCMinutes(today.getMinutes());
        today.setUTCSeconds(today.getSeconds());
        today.setUTCMilliseconds(0);
        return today;
    };
    LogDatePickerComponent.prototype.GetDateParts = function (date) {
        var dateParts;
        dateParts = [];
        var year;
        var month;
        var day;
        var hour;
        var minute;
        var second;
        //year = date.getFullYear();
        //month = date.getMonth();
        //day = date.getDate();
        //hour = date.getHours();
        //minute = date.getMinutes();
        //second = date.getSeconds();
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
    };
    LogDatePickerComponent.prototype.SetValidity = function (validValue, errorMessage) {
        if (!this.IsFreeValue) {
            var siblingUIProperty = null;
            if (this.uiProperty) {
                this.uiProperty.ValidValue = validValue;
                this.uiProperty.ValidationError = errorMessage;
                if (!validValue) {
                    this.DatePickerInputDivStyle = {
                        border: "1px solid #ff0000"
                    };
                    if (this.show) {
                        this.ShowErrorPopup = true;
                    }
                }
                else {
                    this.ShowErrorPopup = false;
                    if (this.show) {
                        this.DatePickerInputDivStyle = {
                            border: "1px solid #3BB3E2"
                        };
                    }
                    else {
                        this.DatePickerInputDivStyle = null;
                    }
                }
            }
        }
    };
    LogDatePickerComponent.prototype.OnClick = function () {
        //alert("Come on man !!");
        this.Click.emit("");
    };
    LogDatePickerComponent.prototype.ValidateField = function (emitPropertyChanged) {
        var _this = this;
        if (emitPropertyChanged === void 0) { emitPropertyChanged = true; }
        if (!this.NoValidation) {
            var errors = null;
            var table = window.ObjectTables.filter(function (d) { return d.Name === _this.uiProperty.ObjectTableName; })[0];
            if (table) {
                var field = window.ObjectFields.filter(function (d) {
                    return d.ObjectTableId === table.Id &&
                        d.FieldName === _this.uiProperty.FieldName;
                })[0];
                var fieldValidator = new FieldValidator_1.FieldValidator();
                errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }
            if (this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            }
            else if (errors) {
                if (errors.length > 0) {
                    this.SetValidity(false, errors[0]);
                }
                //else if (this.uiProperty.IsValidManually == false) {
                //    this.SetValidity(false, this.uiProperty.ManualValidationError);
                //}
                else {
                    this.SetValidity(true, null);
                }
            }
            //else if (this.uiProperty.IsValidManually == false) {
            //    this.SetValidity(false, this.uiProperty.ManualValidationError);
            //}
            else {
                this.SetValidity(true, null);
            }
            if (emitPropertyChanged) {
                this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
            }
        }
    };
    LogDatePickerComponent.prototype.ngOnDestroy = function () {
        console.log("datepicker:ngOnDestroy");
        this.cd = null;
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], LogDatePickerComponent.prototype, "ForceFocus", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", forms_1.FormGroup)
    ], LogDatePickerComponent.prototype, "LogitudeForm", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogDatePickerComponent.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogDatePickerComponent.prototype, "Click", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogDatePickerComponent.prototype, "NoObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogDatePickerComponent.prototype, "NoValidation", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Date),
        __metadata("design:paramtypes", [Date])
    ], LogDatePickerComponent.prototype, "SelectedDateValue", null);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogDatePickerComponent.prototype, "OnBlurEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogDatePickerComponent.prototype, "LostFocus", void 0);
    LogDatePickerComponent = __decorate([
        core_1.Component({
            selector: "LogDatePicker",
            moduleId: module.id,
            templateUrl: "./LogDatePickerComponent.html",
            //directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, HelpIcon, LogCalendarComponent, TimeSelectComponent, FixedPositionDirective],
            inputs: [
                "ObjectFieldName",
                "ObjectTableName",
                "DataContext",
                "HideColumns",
                "HideLastColumn",
                "InputType",
                "TimeMode",
                "FocusOnMe",
                "IsFreeValue",
                "ForceSubscribe",
                "RefreshMe"
            ]
            //changeDetection: ChangeDetectionStrategy.OnPush,
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], LogDatePickerComponent);
    return LogDatePickerComponent;
}());
exports.LogDatePickerComponent = LogDatePickerComponent;
//# sourceMappingURL=LogDatePickerComponent.js.map