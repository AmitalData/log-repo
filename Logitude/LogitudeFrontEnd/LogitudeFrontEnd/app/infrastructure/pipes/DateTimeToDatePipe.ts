import {Pipe} from 'angular2/core';
import {DateFormatter} from 'angular2/src/facade/intl';

@Pipe({ name: 'DateTimeToDatePipe' })

export class DateTimeToDatePipe {

    transform(value: Date): string {

        var myResult: string = "";

        if (value != null) {

            var defaultLocale: string = 'en-US';
            var myDate: Date = new Date(value.toString());
            
            myResult = DateFormatter.format(myDate, defaultLocale, `'dd'`) + "/" + DateFormatter.format(myDate, defaultLocale, "MM") + "/" + DateFormatter.format(myDate, defaultLocale, "yy");            
        }

        return myResult;
    }
}