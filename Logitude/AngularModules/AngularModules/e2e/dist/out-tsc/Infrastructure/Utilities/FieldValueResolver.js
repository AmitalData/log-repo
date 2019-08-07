"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var FieldValueResolver = /** @class */ (function () {
    function FieldValueResolver() {
    }
    FieldValueResolver.GetFieldDataValue = function (field, value) {
        if (field != null && value != null) {
            switch (field.DataTypeCode.trim()) {
                case "Text":
                case "nText":
                case "LookUp":
                    {
                        return value.toString();
                    }
                case "DateTime":
                    {
                        var date = FieldValueResolver.ConvertToDate(value);
                        return date;
                    }
                case "Date":
                    {
                        var date = FieldValueResolver.ConvertToDate(value);
                        return date;
                    }
                case "Decimal":
                case "Integer":
                case "Double":
                    {
                        var d = Number(value);
                        return d;
                    }
                case "Boolean":
                    {
                        var b = Boolean(value);
                        return b;
                    }
                default:
                    {
                        return value != null ? value.toString() : null;
                    }
            }
        }
        return null;
    };
    //private static ConvertToDate(s: string): Date {
    //    var date: Date = null;
    //    if (!string.IsNullOrEmpty(s)) {
    //        date = new Date(System.Convert.ToInt32(s.Substring(0, 4)), System.Convert.ToInt32(s.Substring(4, 2)), System.Convert.ToInt32(s.Substring(6, 2)), System.Convert.ToInt32(s.Substring(8, 2)), System.Convert.ToInt32(s.Substring(10, 2)), System.Convert.ToInt32(s.Substring(12, 2)));
    //    }
    //    return date;
    //}
    FieldValueResolver.ConvertToDate = function (date, type) {
        if (type === void 0) { type = null; }
        var result = null;
        if (date) {
            var year = Number(date.substring(0, 4));
            var month = Number(date.substring(4, 6));
            var day = Number(date.substring(6, 8));
            var hour = Number(date.substring(8, 10));
            var minute = Number(date.substring(10, 12));
            var seconds = Number(date.substring(12, 14));
            result = new Date();
            result.setUTCFullYear(year);
            if (type == "Automation")
                result.setUTCMonth(month - 1);
            else
                result.setUTCMonth(month);
            result.setUTCDate(day);
            result.setUTCHours(hour);
            result.setUTCMinutes(minute);
            result.setUTCSeconds(seconds);
        }
        return result;
    };
    FieldValueResolver.GetFieldStringValue = function (field, value) {
        if (field != null && value != null) {
            if (value.toString() == "") {
                return null;
            }
            switch (field.DataTypeCode.trim()) {
                case "Text":
                case "nText":
                case "Boolean":
                    {
                        return value.toString();
                    }
                case "DateTime":
                case "Date":
                    {
                        //var date: Date = Convert.ToDateTime(value);
                        var dateToStore = FieldValueResolver.ConvertDateToString(value);
                        return dateToStore;
                    }
                case "Decimal":
                case "Integer":
                case "Double":
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(value.toString())) {
                            return null;
                        }
                        //var d: number = Convert.ToDecimal(value);
                        var decimalTostore = FieldValueResolver.ConvertNumberToString(value);
                        return decimalTostore;
                    }
                //case "Integer":
                //    {
                //        var d: number = Convert.ToInt32(value);
                //        var intTostore: string = FieldValueResolver.ConvertToString(d);
                //        return intTostore;
                //    }
                //case "Double":
                //    {
                //        var d: number = Convert.ToDouble(value);
                //        var doubleTostore: string = FieldValueResolver.ConvertToString(d);
                //        return doubleTostore;
                //    }
                default:
                    {
                        return value != null ? value.toString() : null;
                    }
            }
        }
        return null;
    };
    FieldValueResolver.ConvertDateToString = function (date) {
        var time = "";
        var month, day, minuit, second, hour;
        if (date && date instanceof Date) {
            if (date.getMonth() < 10) {
                month = "0" + date.getMonth().toString();
            }
            else {
                month = date.getMonth().toString();
            }
            if (date.getDay() < 10) {
                day = "0" + date.getDay().toString();
            }
            else {
                day = date.getDay().toString();
            }
            if (date.getHours() < 10) {
                hour = "0" + date.getHours().toString();
            }
            else {
                hour = date.getHours().toString();
            }
            if (date.getMinutes() < 10) {
                minuit = "0" + date.getMinutes().toString();
            }
            else {
                minuit = date.getMinutes().toString();
            }
            if (date.getSeconds() < 10) {
                second = "0" + date.getSeconds().toString();
            }
            else {
                second = date.getSeconds().toString();
            }
            time = date.getFullYear().toString() + month + day + hour + minuit + second;
        }
        return time;
    };
    FieldValueResolver.ConvertUTCDateToString = function (date, type) {
        if (type === void 0) { type = null; }
        var time = "";
        var month, day, minuit, second, hour;
        if (date.getUTCMonth() < 10) {
            month = "0" + date.getUTCMonth().toString();
        }
        else {
            month = date.getUTCMonth().toString();
        }
        if (date.getUTCDate() < 10) {
            day = "0" + date.getUTCDate().toString();
        }
        else {
            day = date.getUTCDate().toString();
        }
        if (date.getUTCHours() < 10) {
            hour = "0" + date.getUTCHours().toString();
        }
        else {
            hour = date.getUTCHours().toString();
        }
        if (date.getUTCMinutes() < 10) {
            minuit = "0" + date.getUTCMinutes().toString();
        }
        else {
            minuit = date.getUTCMinutes().toString();
        }
        if (date.getUTCSeconds() < 10) {
            second = "0" + date.getUTCSeconds().toString();
        }
        else {
            second = date.getUTCSeconds().toString();
        }
        if (type == "Automation") {
            var newmonth = Number(month) + 1;
            if (newmonth < 10)
                month = "0" + newmonth.toString();
            else
                month = newmonth.toString();
        }
        time = date.getUTCFullYear().toString() + month + day + hour + minuit + second;
        return time;
    };
    FieldValueResolver.ConvertNumberToString = function (d) {
        var dstring = d.toString();
        var s = "";
        if (dstring.length == 1) {
            s = "0000000" + dstring;
        }
        if (dstring.length == 2) {
            s = "000000" + dstring;
        }
        if (dstring.length == 3) {
            s = "00000" + dstring;
        }
        if (dstring.length == 4) {
            s = "0000" + dstring;
        }
        if (dstring.length == 5) {
            s = "000" + dstring;
        }
        if (dstring.length == 6) {
            s = "00" + dstring;
        }
        if (dstring.length == 7) {
            s = "0" + dstring;
        }
        if (dstring.length == 8) {
            s = dstring;
        }
        return s;
    };
    return FieldValueResolver;
}());
exports.FieldValueResolver = FieldValueResolver;
//# sourceMappingURL=FieldValueResolver.js.map