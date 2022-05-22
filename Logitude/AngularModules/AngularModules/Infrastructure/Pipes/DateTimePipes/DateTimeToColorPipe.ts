import {Pipe} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../Tools';

@Pipe({ name: 'DateTimeToColorPipe' })

export class DateTimeToColorPipe {
    transform(value: Date, defaultColor: string = null, comparisonDate: Date = null): string {

        var myResult = FontTool.Black;

        switch (defaultColor) {
            case "O": {
                myResult = FontTool.Orange;
                break;
            }
            case "A": { // ActivityWork
                myResult = "rgb(110,113,114)";
                break;
            }
            default: {
                myResult = FontTool.Black;
                break;
            }
        }

        if (!AppTool.IsNullOrEmpty(value)) {
            var myDateTicks = DateTool.GetDateParts(value).DateTicks;

            var myDateFilterTicks: number;
            if (comparisonDate) {
                myDateFilterTicks = DateTool.GetDateParts(comparisonDate).DateTicks;
            }

            else {
                myDateFilterTicks = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateTicks;
            }
            
            if (myDateTicks < myDateFilterTicks) {
                myResult = FontTool.Red;
            }
        }

        return myResult;
    }
}
