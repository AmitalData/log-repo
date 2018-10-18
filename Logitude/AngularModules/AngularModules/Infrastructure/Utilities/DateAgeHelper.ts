
export class DateAgeHelper {

    Years: number;
    Months: number;
    Week: number;
    Days: number;
    Hours: number;
    Minutes: number;
    Seconds: number;

    constructor(date: Date) {
        if (date) {
            var myDate: Date = this.GetDateFromString(date.toString());
            var todayDate: Date = this.GetTodaysDate();
            var x = todayDate.getTime() / 1000;
            var y = myDate.getTime() / 1000;
            var seconds = null;
            if (x >= y) {
                seconds = (todayDate.getTime() - myDate.getTime()) / 1000;
            }

            else {
                seconds = (myDate.getTime() - todayDate.getTime()) / 1000;
            }

            this.Seconds = seconds;
            this.Minutes = Math.floor(Number(this.Seconds / 60));
            this.Hours = Math.floor(Number(this.Minutes / 60));
            this.Days = Math.floor(Number(this.Hours / 24));
            this.Months = Math.floor(Number(this.Days / 31));
            this.Years = Math.floor(Number(this.Months / 12));
        }
    }

    GetTodaysDate() {
        var today: Date = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(today.getHours());
        today.setUTCMinutes(today.getMinutes());
        today.setUTCSeconds(today.getSeconds());

        return today;
    }

    public GetDateFromString(datestring: string) {
        //2016/08/14 05:00:00
        //2016-08-14T05:00:00
        //2016/08/14 05:00:00 PM
        var dateAndTime: string[];
        var suffix: string;
        if (datestring.indexOf('T') > -1) {
            dateAndTime = datestring.split('T');
        }
        else {
            dateAndTime = datestring.split(' ');
        }
        var dateArray: string[];
        if (dateAndTime[0].indexOf('/') > -1) {
            dateArray = dateAndTime[0].split('/');
        }
        else if (dateAndTime[0].indexOf('-') > -1) {
            dateArray = dateAndTime[0].split('-');
        }

        if (dateAndTime.length > 2) {
            suffix = dateAndTime[2];
        }

        var timeArray: string[];
        var hour: number = 0;
        var minute: number = 0;
        var second: number = 0;
        if (dateAndTime.length >= 2) {
            if (dateAndTime[1].indexOf('.') > -1) {
                timeArray = dateAndTime[1].split('.')[0].split(':');
            }
            else {
                timeArray = dateAndTime[1].split(':');
            }
            var hour: number = this.GetTimeFor24Mode(Number(timeArray[0]), suffix);
            var minute: number = Number(timeArray[1]);
            var second: number = Number(timeArray[2]);
        }

        var year: number = Number(dateArray[0]);
        var month: number = Number(dateArray[1]) - 1;
        var day: number = Number(dateArray[2]);

        var date: Date = this.GetDate(year, month, day, hour, minute, second);
        return date;
    }

    GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
        var date: Date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);

        return date;
    }

    GetTimeFor24Mode(hours: number, suffix: string) {
        if (suffix) {
            var convHour;
            if (suffix.toLowerCase() == 'am') {
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
            if (suffix.toLowerCase() == 'pm') {
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
}