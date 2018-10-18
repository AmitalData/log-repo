import {Pipe} from '@angular/core';
import {DateTool} from '../../Infrastructure/Tools';

@Pipe({ name: 'DateTimeToDatePipe' })

export class DateTimeToDatePipe {
    transform(value: Date): string {
        return DateTool.GetDateFormats(value).DateString;
    }

    static Pipe(value: Date): string {
        var myResult = "";

        if (value) {
            myResult = DateTool.GetDateFormats(value).DateString;
        }

        return myResult;
    }
}