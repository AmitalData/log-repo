import {Pipe} from '@angular/core';
import {AppTool, DateTool} from '../Tools';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

@Pipe({ name: 'StageAgePipe' })

export class StageAgePipe {

    transform(value: Date, sizeCode: string = null): string {

        var myResult: string = "";

        if (value != null) {
            var myDate: Date = new Date(value.toString());
            var helper: DateAgeHelper = new DateAgeHelper(myDate);

            if (helper.Years > 0) {
                if (sizeCode == "1") {
                    myResult = helper.Years == 1 ? helper.Years + "y" : helper.Years + "y";
                }
                else if (sizeCode == "2") {
                    myResult = helper.Years == 1 ? helper.Years + " y" : helper.Years + " y";
                }
                else {
                    myResult = helper.Years == 1 ? helper.Years + " year" : helper.Years + " years";
                }
            }

            else if (helper.Months > 0) {
                if (sizeCode == "1") {
                    myResult = helper.Months == 1 ? helper.Months + "m" : helper.Months + "m";
                }
                else if (sizeCode == "2") {
                    myResult = helper.Months == 1 ? helper.Months + " m" : helper.Months + " m";
                }
                else {
                    myResult = helper.Months == 1 ? helper.Months + " month" : helper.Months + " months";
                }
            }

            else if (helper.Days > 0) {
                if (sizeCode == "1") {
                    myResult = helper.Days == 1 ? helper.Days + "d" : helper.Days + "d";
                    if (myResult == "0d")
                        myResult = "Today";
                }
                else if (sizeCode == "2") {
                    myResult = helper.Days == 1 ? helper.Days + " d" : helper.Days + " d";
                }
                else {
                    myResult = helper.Days == 1 ? helper.Days + " day" : helper.Days + " days";
                }
            }

            else {
                myResult = "Today";
            }
        }

        return myResult;
    }
}

export class DateAgeHelper {
    public Years: number;
    public Months: number;
    public Days: number;
    public Hours: number;
    public Minutes: number;
    public Seconds: number;

    constructor(myDate: Date) {
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
        
        if (myDate != null && todayDate != null) {
            if (myDate != undefined && todayDate != undefined) {

                var d1 = new Date(myDate.toString());
                var d2 = new Date(todayDate.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());

                var Secndiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                var Mintdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                var Hourdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600 * 24))-1;
                                
                this.Seconds = +Secndiff.toString().split('.')[0];
                this.Minutes = +Mintdiff.toString().split('.')[0];
                this.Hours = +Hourdiff.toString().split('.')[0];
                this.Days = +Daysdiff.toString().split('.')[0];
                this.Months = +(Daysdiff / 31).toString().split('.')[0];
                this.Years = +(Daysdiff / 365).toString().split('.')[0];
            }
        }
        
        //this.Seconds = myTimeSpan.Seconds;
        //this.Minutes = myTimeSpan.Minutes;
        //this.Hours = myTimeSpan.Hours;
        //this.Days = myTimeSpan.Days;
        //this.Months = myTimeSpan.Days / 31;
        //this.Years = myTimeSpan.Days / 365; 
    }
}