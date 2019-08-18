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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var BaseComponent_1 = require("../../Components/LogitudeComponents/BaseComponent");
var TimeInput = /** @class */ (function (_super) {
    __extends(TimeInput, _super);
    function TimeInput() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TMEmployeeTime";
        _this.ControlId = null;
        _this.IsShowText = true;
        _this.IsNew = false;
        _this.FormattedMinutes = "";
        _this.Disabled = false;
        _this.TextAlign = "center";
        _this.FocusOnMe = false;
        _this.Changed = new core_1.EventEmitter();
        _this.DisplayChanged = new core_1.EventEmitter();
        _this.TotalMinutesChanged = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.inputValue = "";
        _this.IsMouseOverMe = false;
        _this.isKeyDown = false;
        _this.isCtrlKeyDown = false;
        _this.isTextChanged = false;
        var idIndex = _this.CurrentSession.GetNewId("TimeInput");
        _this.ControlId = "TimeInputControl_" + idIndex;
        return _this;
    }
    Object.defineProperty(TimeInput.prototype, "InputValue", {
        get: function () {
            return this.inputValue;
        },
        set: function (value) {
            if (this.inputValue != value) {
                this.inputValue = value;
                this.Changed.emit(value);
                this.TotalMinutes = this.GetTotalMinutesFromString(value);
                this.TotalMinutesChanged.emit(this.TotalMinutes);
            }
        },
        enumerable: true,
        configurable: true
    });
    TimeInput.prototype.GetTotalMinutesFromString = function (value) {
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
    };
    Object.defineProperty(TimeInput.prototype, "DisplayValue", {
        get: function () {
            return this.displayValue;
        },
        set: function (value) {
            if (this.displayValue != value) {
                this.displayValue = value;
                this.DisplayChanged.emit(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    TimeInput.prototype.GetTimeValue = function () {
        var errorMessage = null;
        if (this.InputValue) {
            var date;
            date = this.GetTodaysDate();
            var dateparts = this.GetDateParts(date);
            var day;
            var month;
            var year;
            day = dateparts[2];
            month = dateparts[1] + 1;
            year = dateparts[0];
            var invalidText = false;
            if ((this.InputValue != "." && this.InputValue.indexOf(".") > -1) || (this.InputValue != "/" && this.InputValue.indexOf("/") > -1)
                || (this.InputValue != "-" && this.InputValue.indexOf("-") > -1) || (this.InputValue != ":" && this.InputValue.indexOf(":") > -1)) {
                var dateStrings;
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
                    var datetime = this.GetDate(dateparts[0], dateparts[1], dateparts[2], hour, minute, second);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, second);
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
                            var datetime = this.GetDate(year, month - 1, day, hour, minute, 0);
                            this.SetDateValue(datetime);
                        }
                    }
                    else if (this.InputValue.length == 1) {
                        hourString = this.InputValue;
                        hour = Number(hourString);
                        if (!invalidText) {
                            var datetime = this.GetDate(year, month - 1, day, hour, 0, 0);
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
            errorMessage = ""; //'Invalid Input';
        }
    };
    TimeInput.prototype.GetTodaysDate = function () {
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
    TimeInput.prototype.GetDateParts = function (date) {
        var dateParts;
        dateParts = [];
        var year;
        var month;
        var day;
        var hour;
        var minute;
        var second;
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
    TimeInput.prototype.GetDate = function (year, month, day, hour, minute, second) {
        var date = new Date();
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        var endDayNumber = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
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
    };
    TimeInput.prototype.SetDateValue = function (date) {
        var day;
        var month;
        var year;
        var hour;
        var minute;
        var second;
        var dateparts = this.GetDateParts(date);
        day = dateparts[2];
        month = dateparts[1] + 1;
        year = dateparts[0];
        hour = dateparts[3];
        minute = dateparts[4];
        second = dateparts[5];
        this.InputValue = this.ApplyPadding(hour.toString()) + ':' + this.ApplyPadding(minute.toString()) /* + ':' + this.ApplyPadding(second.toString())*/;
    };
    TimeInput.prototype.ApplyPadding = function (str) {
        var pad = "00";
        var ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    };
    TimeInput.prototype.OnLostFocus = function () {
        if (!this.IsMouseOverMe) {
            if (this.IsNew && Tools_1.AppTool.IsNullOrEmpty(this.DisplayValue)) {
                this.IsShowText = false;
            }
            else {
                this.IsShowText = true;
            }
            this.GetTimeValue();
        }
    };
    TimeInput.prototype.TextBoxLostFocus = function () {
        if (this.IsNew && Tools_1.AppTool.IsNullOrEmpty(this.DisplayValue)) {
            this.IsShowText = false;
        }
        else {
            this.IsShowText = true;
        }
    };
    TimeInput.prototype.OnFocus = function () {
        if (this.Disabled)
            this.IsShowText = true;
        else
            this.IsShowText = false;
    };
    TimeInput.prototype.onkeydown = function (event) {
        this.isKeyDown = true;
        var key = event.keyCode;
        var keyChar = event.key;
        var success;
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
    };
    TimeInput.prototype.onButtonClick = function (e) {
        e.preventDefault();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], TimeInput.prototype, "Changed", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], TimeInput.prototype, "DisplayChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], TimeInput.prototype, "TotalMinutesChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [String])
    ], TimeInput.prototype, "InputValue", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], TimeInput.prototype, "DisplayValue", null);
    TimeInput = __decorate([
        core_1.Component({
            selector: "TimeInput",
            inputs: ["TextAlign", "IsShowText", "IsNew", "Disabled", "TotalMinutes", "FocusOnMe"],
            moduleId: module.id,
            templateUrl: './TimeInput.html',
        }),
        __metadata("design:paramtypes", [])
    ], TimeInput);
    return TimeInput;
}(BaseComponent_1.BaseComponent));
exports.TimeInput = TimeInput;
//# sourceMappingURL=TimeInput.js.map