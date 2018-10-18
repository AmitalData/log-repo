import {Pipe} from '@angular/core';
import {DateTool} from '../Tools';

@Pipe({ name: 'DateTimeToTimePipe' })

export class DateTimeToTimePipe {
    transform(value: Date, myFormat: string = null): string {
        var myResult = "";

        if (value) {
            switch (myFormat) {
                case "LT": {
                    myResult = DateTool.GetDateFormats(value).LongTimeString;
                    break;
                }
                case "L12": {
                    myResult = DateTool.GetDateFormats(value).LongTimeString12;
                    break;
                }

                case "S12": {
                    myResult = DateTool.GetDateFormats(value).ShortTimeString12;
                    break;
                }

                default: {
                    myResult = DateTool.GetDateFormats(value).ShortTimeString;
                    break;
                }
            }
        }

        return myResult;
    }

    static Pipe(value: Date, myFormat: string = null): string {
        var myResult = "";

        if (value) {
            switch (myFormat) {
                case "LT": {
                    myResult = DateTool.GetDateFormats(value).LongTimeString;
                    break;
                }
                case "L12": {
                    myResult = DateTool.GetDateFormats(value).LongTimeString12;
                    break;
                }

                case "S12": {
                    myResult = DateTool.GetDateFormats(value).ShortTimeString12;
                    break;
                }

                default: {
                    myResult = DateTool.GetDateFormats(value).ShortTimeString;
                    break;
                }
            }
        }

        return myResult;
    }
}