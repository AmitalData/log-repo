"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var CustomFieldClass = /** @class */ (function () {
    function CustomFieldClass(Value, FieldName, TableName) {
        this.Value = Value;
        this.FieldName = FieldName;
        this.TableName = TableName;
    }
    Object.defineProperty(CustomFieldClass.prototype, "ResolvedValue", {
        get: function () {
            return this.GetFieldDataTypeValue(this.GetObjectField(this.FieldName, this.TableName), this.Value); //this.resolvedValue;;
        },
        enumerable: true,
        configurable: true
    });
    //public set ResolvedValue(newValue: any) {
    //    if (this.resolvedValue != newValue) {
    //        this.resolvedValue = newValue;
    //    }
    //}
    CustomFieldClass.prototype.GetObjectField = function (fieldName, objectTableName) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        var field = window.ObjectFields.filter(function (d) { return d.FieldName == fieldName && d.ObjectTableId == table.Id; })[0];
        return field;
    };
    CustomFieldClass.prototype.ConvertToDate = function (s) {
        //20120828000000
        var date = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(s)) {
            date = new Date();
            // date.setUTCFullYear(Number(s.substr(0, 4)));
            // date.setUTCMonth(Number(s.substr(4, 2)) - 1);
            // date.setUTCDate(Number(s.substr(6, 2)));
            // date.setUTCHours(Number(s.substr(8, 2)));
            // date.setUTCMinutes(Number(s.substr(10, 2)));
            // date.setUTCSeconds(Number(s.substr(12, 2)));
            date.setUTCDate(1);
            date.setUTCFullYear(Number(s.substr(0, 4)));
            date.setUTCMonth(Number(s.substr(4, 2)) - 1);
            date.setUTCDate(Number(s.substr(6, 2)));
            date.setUTCHours(Number(s.substr(8, 2)));
            date.setUTCMinutes(Number(s.substr(10, 2)));
            date.setUTCSeconds(Number(s.substr(12, 2)));
            date.setUTCMilliseconds(0);
        }
        return date;
    };
    CustomFieldClass.prototype.ConvertToString = function (date) {
        var time = "";
        var month;
        var day;
        var minute;
        var second;
        var hour;
        var year;
        var monthNumber = date.getUTCMonth() + 1;
        var dayNumber = date.getUTCDate();
        var yearNumber = date.getUTCFullYear();
        var hourNumber = date.getUTCHours();
        var minuteNumber = date.getUTCMinutes();
        var secondNumber = date.getUTCSeconds();
        if (monthNumber < 10) {
            month = "0" + monthNumber;
        }
        else {
            month = monthNumber + "";
        }
        if (dayNumber < 10) {
            day = "0" + dayNumber;
        }
        else {
            day = dayNumber + "";
        }
        if (hourNumber < 10) {
            hour = "0" + hourNumber;
        }
        else {
            hour = hourNumber + "";
        }
        if (minuteNumber < 10) {
            minute = "0" + minuteNumber;
        }
        else {
            minute = minuteNumber + "";
        }
        if (secondNumber < 10) {
            second = "0" + secondNumber;
        }
        else {
            second = secondNumber + "";
        }
        year = yearNumber + "";
        time = year + month + day + hour + minute + second;
        return time;
    };
    CustomFieldClass.prototype.ConvertIntToString = function (d, signed) {
        //var fmt: string = "000000000000";
        if (d > 0) {
            var paddedString = this.ApplyIntPadding(d + "");
            var dString = paddedString; //d + "";
            if (signed) {
                dString = "+" + paddedString;
            }
        }
        else {
            d = (d * -1);
            var paddedString = this.ApplyIntPadding(d + "");
            var dString = paddedString; //d + "";
            if (signed) {
                dString = "-" + paddedString;
            }
        }
        return dString;
    };
    CustomFieldClass.prototype.ConvertDoubleOrDecimalToString = function (d, signed) {
        //string fmt = "000000000000.000";
        var dString = this.ApplyDoublePadding(d + ""); //d.ToString(fmt);
        var originalString = d + "";
        dString = dString.replace("+", "").replace("-", "").replace(".", "");
        if (signed) {
            dString = (originalString.indexOf("-") > -1 ? "-" + dString : "+" + dString);
        }
        return dString;
    };
    CustomFieldClass.prototype.ApplyIntPadding = function (str) {
        var pad = "000000000000";
        var ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    };
    CustomFieldClass.prototype.ApplyDoublePadding = function (str) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(str)) {
            var stringParts = str.split('.');
            myResult = Tools_1.AppTool.PadLeft(stringParts[0], 12, "0") + "." + Tools_1.AppTool.PadRight(stringParts[1], 3, "0");
        }
        return myResult;
        //var strArr:string[] = str.split('.');
        //var leftPad: string = "000000000000";
        //var rightPad: string = "000";
        //var zerrrr: string = leftPad.substr(0, leftPad.length - strArr[0].length);
        //var leftAns: string = zerrrr + str[0];
        //var rightAns: string = "00";
        //if (strArr.length > 1) {
        //    rightAns = strArr[1] + rightPad.substring(0, rightPad.length - strArr[1].length);
        //}
        //var ans: string = leftAns + '.' + rightAns;
        //return ans;
    };
    CustomFieldClass.prototype.GetFieldDataTypeValue = function (field, customField) {
        if (field != null) {
            var result = null;
            if (customField != null) {
                switch (field.DataTypeCode.trim()) {
                    case "Text":
                    case "nText":
                    case "LookUp":
                    case "PickList":
                        {
                            result = customField + "";
                            break;
                        }
                    case "Date":
                    case "DateTime":
                        {
                            var date = this.ConvertToDate(customField);
                            result = date;
                            break;
                        }
                    case "UnsDecimal":
                    case "Decimal":
                    case "Double":
                    case "SigDouble":
                        {
                            var d = 0;
                            if (customField.length >= 15) {
                                //customField = customField.Insert(customField.Length - 3, ".");
                                var sign = customField.substr(0, 1);
                                var first = customField.substr(1, 12);
                                var second = customField.substr(13, 3);
                                customField = sign + first + '.' + second;
                                //if (sign == '-') {
                                //    customField = sign + first + '.' + second;
                                //}
                                //else {
                                //    customField = first + '.' + second;
                                //}
                            }
                            //decimal.TryParse(customField, out d);
                            d = Number(customField);
                            result = d;
                            break;
                        }
                    case "Integer":
                    case "UnsInteger":
                        {
                            var i = 0;
                            i = Number(customField);
                            result = i;
                            break;
                        }
                    case "Boolean":
                        {
                            var b;
                            if (customField.toLowerCase() == 'false') {
                                b = false;
                            }
                            else if (customField.toLowerCase() == 'true') {
                                b = true;
                            }
                            result = b;
                            break;
                        }
                    default:
                        {
                            result = null;
                            break;
                        }
                }
            }
            else if (field.DataTypeCode.trim() == "Boolean") {
                result = false;
            }
            //this.ResolvedValue = result;
            return result;
        }
        return null;
    };
    CustomFieldClass.prototype.SetFieldDataType = function (field, value) {
        if (field != null && value != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(value + "")) {
                return null;
            }
            switch (field.DataTypeCode.trim()) {
                case "Text":
                case "nText":
                case "LookUp":
                case "PickList":
                    {
                        return value + "";
                    }
                case "DateTime":
                case "Date":
                    {
                        var date = value;
                        var dateToStore = this.ConvertToString(date);
                        return dateToStore;
                    }
                case "Double":
                case "SigDouble":
                case "UnsDecimal":
                case "Decimal":
                    {
                        var d = 0;
                        d = value;
                        var decimalTostore = this.ConvertDoubleOrDecimalToString(d, true);
                        return decimalTostore;
                    }
                case "UnsInteger":
                case "Integer":
                    {
                        var d = 0;
                        d = value;
                        var intTostore = this.ConvertIntToString(d, true);
                        return intTostore;
                    }
                case "Boolean":
                    {
                        return value + "";
                    }
                default:
                    {
                        return (value != null ? value + "" : null);
                    }
            }
        }
        return null;
    };
    return CustomFieldClass;
}());
exports.CustomFieldClass = CustomFieldClass;
//# sourceMappingURL=CustomFieldClass.js.map