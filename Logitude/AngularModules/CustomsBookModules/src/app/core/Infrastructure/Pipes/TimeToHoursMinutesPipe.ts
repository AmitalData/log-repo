import {Pipe} from '@angular/core';
import {AppTool, FormatTool} from '../Tools';

@Pipe({ name: 'TimeToHoursMinutesPipe' })

export class TimeToHoursMinutesPipe {

    transform(value: number): string {

        var myResult: string = "";
        var result = "";
        var val = value;
        var h = val / 60 | 0,
            m = val % 60 | 0;
        var result = h + ":" + AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            myResult = "";
        }
        else {
            myResult = result;
        }

        return myResult;
    }
}
 