import {Pipe} from '@angular/core';
import {AppTool, DateFormats, DateParts} from '../Tools';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {SessionLocator} from '../Utilities/SessionLocator';

@Pipe({ name: 'LogBoxStatusDatePipe' })

export class LogBoxStatusDatePipe {
    transform(value: Date, myFormat: string = null): string {
        var myResult = "";

        if (!AppTool.IsNullOrEmpty(value)) {

            var myFormats = this.GetDateFormats(value);
            myResult = myFormats.DateString + " " + myFormats.ShortTimeString;
        }

        return myResult;
    }

    private GetDateParts(input: Date) {
        var myDateParts = new DateParts();

        if (!AppTool.IsNullOrEmpty(input)) {
            // 2009-12-18 10:54:50.546
            // 2016-12-27T00:00:00
            // 2016-12-27T16:58:15.527
            // 2012-02-07CET00:00:00
            // 2009-04-19T16:11:05+02:00
            // 2009-04-19T16:11:05Z

            if (typeof (input) == "string") {
                var dateTimeString: string = (input + "").replace('Z', '');

                var splitBy = " ";

                if (dateTimeString.indexOf("CET") !== -1) {
                    splitBy = "CET";
                }

                else if (dateTimeString.indexOf("T") !== -1) {
                    splitBy = "T";
                }

                if (!AppTool.IsNullOrEmpty(dateTimeString)) {
                    var dateString: string = dateTimeString.split(splitBy)[0];
                    var timeString: string = dateTimeString.split(splitBy)[1];

                    if (!AppTool.IsNullOrEmpty(dateString)) {
                        var dateParts: string[] = dateString.split('-');

                        myDateParts.Year = +dateParts[0];
                        myDateParts.Month = +dateParts[1];
                        myDateParts.Day = +dateParts[2];
                    }

                    if (!AppTool.IsNullOrEmpty(timeString)) {
                        var timeParts: string[] = timeString.split(':');

                        myDateParts.Hours = +timeParts[0];
                        myDateParts.Minutes = +timeParts[1];
                        myDateParts.Seconds = +timeParts[2].split('.')[0];

                        if (!AppTool.IsNullOrEmpty(timeParts[2].split('.')[1])) {
                            myDateParts.Milliseconds = +timeParts[2].split('.')[1];
                        }
                    }
                }
            }

            else {
                myDateParts.Year = input.getFullYear();
                myDateParts.Month = input.getMonth() + 1;
                myDateParts.Day = input.getDate();
                myDateParts.Hours = input.getHours();
                myDateParts.Minutes = input.getMinutes();
                myDateParts.Seconds = input.getSeconds();
                myDateParts.Milliseconds = input.getMilliseconds();
            }

            // we need it by this sort (Year > Month > Day)
            // if current month is 6 (max day is 30/6)
            // and we used this method to get date of (31/5)
            // then it will be a problem
            // the new Date(); will init date as today date (month 6)
            // but then when assign the day (31) it will 31/6 which will be converted to 1/7
            // then when assign the month (5) it will become (1/5) instead of (31/5)

            var dateObject = new Date();
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);

            myDateParts.DateObject = dateObject;
            myDateParts.Hours12 = myDateParts.Hours > 12 ? (myDateParts.Hours - 12) : myDateParts.Hours;
            myDateParts.DateTicks = myDateParts.DateObject.valueOf();
        }

        return myDateParts;
    }
    private GetDateFormats(input: Date) {
        var myDateFormats = new DateFormats();

        if (!AppTool.IsNullOrEmpty(input)) {
            var myDateParts = this.GetDateParts(input);

            var stringOfYear = (AppTool.PadLeft("" + myDateParts.Year, 4, '0')).substring(2);
            var stringOfMonth = AppTool.PadLeft("" + myDateParts.Month, 2, '0');
            var stringOfDay = AppTool.PadLeft("" + myDateParts.Day, 2, '0');
            var stringOfHours = AppTool.PadLeft("" + myDateParts.Hours, 2, '0');
            var stringOfHours12 = AppTool.PadLeft("" + myDateParts.Hours12, 2, '0');
            var stringOfMinutes = AppTool.PadLeft("" + myDateParts.Minutes, 2, '0');
            var stringOfSeconds = AppTool.PadLeft("" + myDateParts.Seconds, 2, '0');
            var stringOfMilliseconds = AppTool.PadLeft("" + myDateParts.Milliseconds, 3, '0');

            // Build Formats
            myDateFormats.AMPM = myDateParts.Hours >= 12 ? 'PM' : 'AM';
            myDateFormats.LocalDateString = myDateParts.DateObject.toLocaleDateString();
            myDateFormats.LocalTimeString = myDateParts.DateObject.toLocaleTimeString();
            myDateFormats.LocalDateTimeString = myDateParts.DateObject.toLocaleString();

            myDateFormats.ShortTimeString = stringOfHours + ":" + stringOfMinutes;
            myDateFormats.LongTimeString = stringOfHours + ":" + stringOfMinutes + ":" + stringOfSeconds;
            myDateFormats.ShortTimeString12 = stringOfHours12 + ":" + stringOfMinutes + " " + myDateFormats.AMPM;
            myDateFormats.LongTimeString12 = stringOfHours12 + ":" + stringOfMinutes + ":" + stringOfSeconds + " " + myDateFormats.AMPM;

            // ShortDateString
            if (!AppTool.IsNullOrEmpty(SessionLocator.TenantPM.DateTimeFormat)) {

                var myDateTimeFormatPrefix = SessionLocator.TenantPM.DateTimeFormat.toLowerCase().substring(0, 2);

                if (myDateTimeFormatPrefix == "mm") {
                    myDateFormats.ShortDateString = stringOfMonth + "/" + stringOfDay + "/" + stringOfYear;
                }

                else {
                    myDateFormats.ShortDateString = stringOfDay + "/" + stringOfMonth + "/" + stringOfYear;
                }
            }

            else {
                myDateFormats.ShortDateString = myDateFormats.LocalDateString;
            }

            // DateString
            var todayDateParts = this.GetDateParts(this.GetCurrentDateAsUtc());

            if (todayDateParts.Year == myDateParts.Year && todayDateParts.Month == myDateParts.Month && todayDateParts.Day == myDateParts.Day) {
                myDateFormats.DateString = TextCodeTranslator.Translate("General.O.Today");
            }

            else if (todayDateParts.Year == myDateParts.Year && todayDateParts.Month == myDateParts.Month && (todayDateParts.Day + 1) == myDateParts.Day) {
                myDateFormats.DateString = TextCodeTranslator.Translate("General.O.Tomorrow");
            }

            else if (todayDateParts.Year == myDateParts.Year && todayDateParts.Month == myDateParts.Month && (todayDateParts.Day - 1) == myDateParts.Day) {
                myDateFormats.DateString = TextCodeTranslator.Translate("General.O.Yesterday");
            }

            else {
                myDateFormats.DateString = myDateFormats.ShortDateString;
            }

            myDateFormats.DateParts = myDateParts;
            this.GetPartsNames(myDateFormats);

        }

        return myDateFormats;
    }
    private GetPartsNames(dateFormats: DateFormats) {
        if (dateFormats) {

            var dayIndex = dateFormats.DateParts.DateObject.getDay();
            var monthIndex = dateFormats.DateParts.DateObject.getMonth();

            switch (dayIndex) {
                case 0: { dateFormats.DayName = "Sunday", dateFormats.DayNameShort = "Sun"; break; }
                case 1: { dateFormats.DayName = "Monday", dateFormats.DayNameShort = "Mon"; break; }
                case 2: { dateFormats.DayName = "Tuesday", dateFormats.DayNameShort = "Tue"; break; }
                case 3: { dateFormats.DayName = "Wednesday", dateFormats.DayNameShort = "Wed"; break; }
                case 4: { dateFormats.DayName = "Thursday", dateFormats.DayNameShort = "Thu"; break; }
                case 5: { dateFormats.DayName = "Friday", dateFormats.DayNameShort = "Fri"; break; }
                case 6: { dateFormats.DayName = "Saturday", dateFormats.DayNameShort = "Sat"; break; }
            }

            switch (monthIndex) {
                case 0: { dateFormats.MonthName = "January", dateFormats.MonthNameShort = "Jan"; break; }
                case 1: { dateFormats.MonthName = "February", dateFormats.MonthNameShort = "Feb"; break; }
                case 2: { dateFormats.MonthName = "March", dateFormats.MonthNameShort = "Mar"; break; }
                case 3: { dateFormats.MonthName = "April", dateFormats.MonthNameShort = "Apr"; break; }
                case 4: { dateFormats.MonthName = "May", dateFormats.MonthNameShort = "May"; break; }
                case 5: { dateFormats.MonthName = "June", dateFormats.MonthNameShort = "Jun"; break; }
                case 6: { dateFormats.MonthName = "July", dateFormats.MonthNameShort = "Jul"; break; }
                case 7: { dateFormats.MonthName = "August", dateFormats.MonthNameShort = "Aug"; break; }
                case 8: { dateFormats.MonthName = "September", dateFormats.MonthNameShort = "Sep"; break; }
                case 9: { dateFormats.MonthName = "October", dateFormats.MonthNameShort = "Oct"; break; }
                case 10: { dateFormats.MonthName = "November", dateFormats.MonthNameShort = "Nov"; break; }
                case 11: { dateFormats.MonthName = "December", dateFormats.MonthNameShort = "Dec"; break; }
            }
        }
    }

    private GetCurrentDateAsUtc() {
        var myResult: Date = new Date();
        myResult.setUTCFullYear(myResult.getFullYear());
        myResult.setUTCMonth(myResult.getMonth());
        myResult.setUTCDate(myResult.getDate());
        myResult.setUTCHours(0);
        myResult.setUTCMinutes(0);
        myResult.setUTCSeconds(0);
        myResult.setUTCMilliseconds(0);
        return myResult;
    }
}