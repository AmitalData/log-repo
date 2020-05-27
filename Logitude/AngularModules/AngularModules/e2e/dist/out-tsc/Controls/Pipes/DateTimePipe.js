"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var DateTimePipe = /** @class */ (function () {
    function DateTimePipe() {
    }
    DateTimePipe.prototype.transform = function (value, myFormat) {
        if (myFormat === void 0) { myFormat = null; }
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            if (myFormat == "CustomField") {
                myResult = "" + value;
            }
            else {
                var myFormats = Tools_1.DateTool.GetDateFormats(value);
                switch (myFormat) {
                    case "D": {
                        // Today:Yesterday:Tomorrow:ShortDate
                        myResult = myFormats.DateString;
                        break;
                    }
                    case "SD": {
                        // ShortDate
                        myResult = myFormats.ShortDateString;
                        break;
                    }
                    case "T": {
                        // hh:mm
                        myResult = myFormats.ShortTimeString;
                        break;
                    }
                    case "DT": {
                        // Today:Yesterday:Tomorrow:ShortDate + hh:mm
                        myResult = myFormats.DateString + " " + myFormats.ShortTimeString;
                        break;
                    }
                    case "DTL12": {
                        // Today:Yesterday:Tomorrow:ShortDate + hh:mm:ss (AM/PM)
                        myResult = myFormats.DateString + " " + myFormats.LongTimeString12;
                        break;
                    }
                    case "DTL12NW": {
                        // ShortDate + hh:mm:ss (AM/PM)
                        myResult = myFormats.ShortDateString + " " + myFormats.ShortTimeString12;
                        break;
                    }
                    case "ST12": {
                        // hh:mm (AM/PM)
                        myResult = myFormats.ShortTimeString12;
                        break;
                    }
                    case "LT": {
                        // hh:mm:ss
                        // 13:30:22
                        myResult = myFormats.LongTimeString;
                        break;
                    }
                    case "LT12": {
                        // hh:mm:ss (AM/PM)
                        // 01:30:22 PM
                        myResult = myFormats.LongTimeString12;
                        break;
                    }
                    case "DDMM": {
                        // Ticket work
                        // Sat, 20 Feb 2016
                        myResult = myFormats.DayNameShort + ", " + Tools_1.AppTool.PadLeft(myFormats.DateParts.Day + "", 2, "0") + " " + myFormats.MonthNameShort + " " + myFormats.DateParts.Year;
                        break;
                    }
                    case "MSY": {
                        // Opportunities Work space in CRM
                        // 2y , 32 months ago ...
                        myResult = this.getMSYDATE(value);
                        break;
                    }
                    case "DTTA": {
                        myResult = this.DateTimeToTimeAmount(myFormats);
                        break;
                    }
                    case "DOW": { // day of the week 
                        var day = myFormats.DateParts.Day;
                        var month = myFormats.DateParts.DateObject.getMonth();
                        var currentYear = Tools_1.DateTool.GetCurrentDateAsUtc().getFullYear();
                        var newDate = new Date(currentYear, month, day);
                        var date = Tools_1.DateTool.GetDateFromDate(newDate);
                        myResult = Tools_1.DateTool.GetCurrentFullDayOfWeek(date.getDay());
                        break;
                    }
                    case "AGE": { // age pipe
                        myResult = (Tools_1.DateTool.GetCurrentDateAsUtc().getUTCFullYear() - myFormats.DateParts.Year) + "";
                        break;
                    }
                    case "TIMM": { // time management pipe 'day/month'
                        myResult = myFormats.DateParts.Day + "/" + myFormats.DateParts.Month;
                        break;
                    }
                    case "SHWH": {
                        myResult = Tools_1.AppTool.PadLeft(myFormats.DateParts.Day + "", 2, "0") + " " + myFormats.MonthNameShort + " " + myFormats.DateParts.Year;
                        break;
                    }
                    case "FIDATE": { //Filing Inbox
                        // Thu 22-Feb
                        myResult = myFormats.DayNameShort + " " + Tools_1.AppTool.PadLeft(myFormats.DateParts.Day + "", 2, "0") + "-" + myFormats.MonthNameShort;
                        break;
                    }
                    case "DTLL12": { //Scheduler
                        // Today:Yesterday:Tomorrow:ShortDate + hh:mm:ss (AM/PM)
                        myResult = myFormats.DateString + " " + myFormats.ShortTimeString12;
                        break;
                    }
                    case "SMD": { // time management pipe 'day/month'
                        myResult = Tools_1.AppTool.PadLeft(myFormats.DateParts.Day + "", 2, "0") + "." + Tools_1.AppTool.PadLeft(myFormats.DateParts.Month + "", 2, "0") + "." + myFormats.DateParts.Year.toString().substr(-2);
                        break;
                    }
                    default: {
                        // Today:Yesterday:Tomorrow:ShortDate Format
                        myResult = myFormats.DateString;
                        break;
                    }
                }
            }
        }
        return myResult;
    };
    DateTimePipe.prototype.getMSYDATE = function (value) {
        var myResult;
        var valueDateTime = Tools_1.DateTool.GetDateFromDate(value);
        ;
        var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var myDays = Tools_1.DateTool.GetDaysBetweenDates(todayDateTime, valueDateTime);
        var Numberseconds = Tools_1.DateTool.GetSecondsBetweenDates(todayDateTime, valueDateTime);
        var hours = Tools_1.DateTool.GetHoursBetweenDates(todayDateTime, valueDateTime);
        var minutes = Tools_1.DateTool.GetMinutesBetweenDates(todayDateTime, valueDateTime);
        var week = myDays / 7;
        var months = myDays / 31;
        var years = months / 12;
        if (myDays == 0) {
            if (minutes == 0 && years == 0 && months == 0 && hours == 0 && Numberseconds < 60) {
                if (Numberseconds > 0 && Numberseconds < 60) {
                    if (Numberseconds < 5) {
                        myResult = "1s";
                    }
                    else {
                        myResult = Numberseconds + "s";
                    }
                }
                else {
                    if (Numberseconds == 0) {
                        myResult = "1s";
                    }
                }
            }
            else if (minutes > 0 && minutes < 60 && years == 0 && months == 0 && hours == 0) {
                myResult = minutes + "m";
            }
            else if (hours > 0 && hours < 24 && years == 0 && months == 0) {
                myResult = hours + "h";
            }
        }
        else if (myDays > 0 && myDays < 31) {
            if (myDays == 1)
                myResult = "Today";
            else
                myResult = myDays + "d";
        }
        else if (myDays > 31 && myDays < 360) {
            var myTotalMonths = myDays / 31;
            var MonthName = valueDateTime.toLocaleString("en-us", { month: "long" });
            myResult = myDays / 7 + "w";
        }
        else if (myDays > 360) {
            var myTotalYears = myDays / 365;
            myResult = myTotalYears + "y";
        }
        return myResult;
    };
    DateTimePipe.prototype.DateTimeToTimeAmount = function (value) {
        var result = "";
        var valueDateTime = value.DateParts.DateObject;
        var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (valueDateTime.valueOf() > todayDateTime.valueOf()) {
            var myTotalDays = Tools_1.DateTool.GetDaysBetweenDates(valueDateTime, todayDateTime);
            if (myTotalDays == 0) {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Today");
            }
            else if (myTotalDays == 1) {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Tomorrow");
            }
            else if (myTotalDays > 1 && myTotalDays < 31) {
                result = "in " + myTotalDays + " days";
            }
            else if (myTotalDays > 31 && myTotalDays < 1095) {
                var myTotalMonths = Math.floor(myTotalDays / 31);
                if (myTotalMonths == 1) {
                    result = "in " + myTotalMonths + " month";
                }
                else {
                    result = "in " + myTotalMonths + " months";
                }
            }
            else if (myTotalDays > 1095) {
                var myTotalYears = Math.floor(myTotalDays / 365);
                if (myTotalYears == 1) {
                    result = "in " + myTotalYears + " year";
                }
                else {
                    result = "in " + myTotalYears + " years";
                }
            }
        }
        else {
            var myTotalDays = Tools_1.DateTool.GetDaysBetweenDates(todayDateTime, valueDateTime);
            if (myTotalDays == 0) {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Today");
            }
            else if (myTotalDays == 1) {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Yesterday");
            }
            else if (myTotalDays > 1 && myTotalDays < 31) {
                result = myTotalDays + " days ago";
            }
            else if (myTotalDays > 31 && myTotalDays < 1095) {
                var myTotalMonths = Math.floor(myTotalDays / 31);
                if (myTotalMonths == 1) {
                    result = myTotalMonths + " month ago";
                }
                else {
                    result = myTotalMonths + " months ago";
                }
            }
            else if (myTotalDays > 1095) {
                var myTotalYears = Math.floor(myTotalDays / 365);
                if (myTotalYears == 1) {
                    result = myTotalYears + " year ago";
                }
                else {
                    result = myTotalYears + " years ago";
                }
            }
        }
        return result;
    };
    DateTimePipe = __decorate([
        core_1.Pipe({ name: 'DateTimePipe' })
    ], DateTimePipe);
    return DateTimePipe;
}());
exports.DateTimePipe = DateTimePipe;
//# sourceMappingURL=DateTimePipe.js.map