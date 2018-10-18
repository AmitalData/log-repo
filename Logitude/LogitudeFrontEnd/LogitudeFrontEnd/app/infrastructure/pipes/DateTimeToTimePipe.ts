import {Pipe} from 'angular2/core';
import {DateFormatter} from 'angular2/src/facade/intl';

@Pipe({ name: 'DateTimeToTimePipe' })

export class DateTimeToTimePipe {

    transform(value: Date): string {

        var myResult: string = "";
               
        if (value != null && value !== undefined) {

            console.log("Origin: " + value);            
            //Origin: 2016-01-04T00:00:00
            
            var myDate: Date = new Date(value.toString());
            console.log("Parsed: " + myDate);
            //Parsed: Tue Jan 19 2016 02:00:00 GMT+ 0200(Jerusalem Standard Time)

            var defaultLocale: string = 'en-US';
            myResult = DateFormatter.format(myDate, defaultLocale, "HH:mm");
        }

        return myResult;
    }
}