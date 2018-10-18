import {Pipe} from '@angular/core';
import {AppTool, DateTool} from '../Tools';

@Pipe({ name: 'DateTimeToBackgroundPipe' })

export class DateTimeToBackgroundPipe {
    transform(value: Date): string {
        var myResult = "white";

        if (!AppTool.IsNullOrEmpty(value)) {
            var myDate: Date = DateTool.GetDateFromDate(value, true);
            var myCurrentDateTime: Date = DateTool.GetCurrentDateAsUtc();
           
            if (myDate.valueOf() < myCurrentDateTime.valueOf()) {
                myResult = "#F7E3E3";
            }
        }

        return myResult;
    }
}