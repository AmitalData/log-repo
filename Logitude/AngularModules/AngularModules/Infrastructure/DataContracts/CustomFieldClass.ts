declare var window: any;
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../EntityPMs/ObjectTablePM';
import {AppTool, DateTool} from '../Tools';
export class CustomFieldClass {

    constructor(public Value: string, public FieldName: string, public TableName: string) {
    }

    private resolvedValue: any;
    public get ResolvedValue() {
        return this.GetFieldDataTypeValue(this.GetObjectField(this.FieldName, this.TableName), this.Value);                                 //this.resolvedValue;;
    }
    //public set ResolvedValue(newValue: any) {
    //    if (this.resolvedValue != newValue) {
    //        this.resolvedValue = newValue;
    //    }
    //}

    public GetObjectField(fieldName: string, objectTableName: string){
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        var field: ObjectFieldPM = window.ObjectFields.filter(d => d.FieldName == fieldName && d.ObjectTableId==table.Id)[0];
        return field;
    }

    public ConvertToDate(s: string) {
        //20120828000000
        var date: Date = null;
        if (!AppTool.IsNullOrEmpty(s)) {
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

    }

    public ConvertToString(date: Date) {
        var time: string = "";
        var month: string;
        var day: string;
        var minute: string;
        var second: string;
        var hour: string;
        var year: string;

        var monthNumber = date.getUTCMonth() + 1;
        var dayNumber = date.getUTCDate();
        var yearNumber = date.getUTCFullYear();
        var hourNumber = date.getUTCHours();
        var minuteNumber = date.getUTCMinutes();
        var secondNumber = date.getUTCSeconds();

        if (monthNumber < 10) {
            month = "0" + monthNumber;

        }
        else { month = monthNumber + ""; }

        if (dayNumber < 10) {
            day = "0" + dayNumber;

        }
        else { day = dayNumber + ""; }

        if (hourNumber < 10) {
            hour = "0" + hourNumber;

        }
        else { hour = hourNumber + ""; }

        if (minuteNumber < 10) {
            minute = "0" + minuteNumber;

        }
        else { minute = minuteNumber + ""; }

        if (secondNumber < 10) {
            second = "0" + secondNumber;
        }
        else { second = secondNumber + ""; }

        year = yearNumber + "";
        time = year + month + day + hour + minute + second;

        return time;
    }

    public ConvertIntToString(myNumber: number, signed: boolean) {
        let newNumber = myNumber;
        if (newNumber) {
            newNumber = Math.trunc(myNumber);


            //var fmt: string = "000000000000";
            if (newNumber > 0) {
                var paddedString = this.ApplyIntPadding(newNumber + "");
                var dString: string = paddedString;//d + "";
                if (signed) {
                    dString = "+" + paddedString;
                }
            }
            else {
                newNumber = (newNumber * -1);
                var paddedString = this.ApplyIntPadding(newNumber + "");
                var dString: string = paddedString;//d + "";
                if (signed) {
                    dString = "-" + paddedString;
                }
            }


            return dString;
        }
        else
            return null;

    }

    public ConvertDoubleOrDecimalToString(d: number, signed: boolean) {
        //string fmt = "000000000000.000";
        var dString: string = this.ApplyDoublePadding(d + ""); //d.ToString(fmt);
        var originalString: string = d + "";
        dString = dString.replace("+", "").replace("-", "").replace(".", "");

        if (signed) {
            dString = (originalString.indexOf("-") > -1 ? "-" + dString : "+" + dString);
        }

        return dString;
    }

    ApplyIntPadding(str: string) {
        var pad = "000000000000";
        var ans = pad.substring(0, pad.length - str.length) + str
        return ans;
    }

    ApplyDoublePadding(str: string) {

        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(str)) {
            var stringParts: string[] = str.split('.');

            myResult = AppTool.PadLeft(stringParts[0], 12, "0") + "." + AppTool.PadRight(stringParts[1], 3, "0");
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
    }


    public GetFieldDataTypeValue(field: ObjectFieldPM, customField: string): any {
        if (field != null) {
            var result: any = null;
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
                            var date: Date = this.ConvertToDate(customField);
                            result = date;
                            break;
                        }
                    case "UnsDecimal":
                    case "Decimal":
                    case "Double":
                    case "SigDouble":
                        {
                            var d: number = 0;
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
                            var i: number = 0;
                            i = Number(customField);
                            result = i;
                            break;
                        }
                    case "Boolean":
                        {
                            var b: boolean;
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
    }

    public SetFieldDataType(field: ObjectFieldPM, value: any){
        if (field != null && value != null) {
            if (AppTool.IsNullOrEmpty(value+"")) {
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
                    var date: Date = value;
                    var dateToStore : string = this.ConvertToString(date);
                    return dateToStore;
                }
            case "Double":
            case "SigDouble":
            case "UnsDecimal":
            case "Decimal":
                {
                    var d: number = 0;
                    d = value;
                    var decimalTostore: string = this.ConvertDoubleOrDecimalToString(d, true);
                    return decimalTostore;
                }
            case "UnsInteger":
            case "Integer":
                {
                    var d: number = 0;
                    d = value;
                    var intTostore : string  = this.ConvertIntToString(d, true);
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
    }

}
