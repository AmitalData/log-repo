import {Pipe} from '@angular/core';
import { AppTool, FormatTool} from '../../Infrastructure/Tools';

@Pipe({ name: 'MinutesToTimePipe' })

export class MinutesToTimePipe {
    transform(value: number): string {
        var myResult: string = "";

        if (!AppTool.IsNullOrZero(value)) {
            if (FormatTool.IsNumeric(value + "")) {
                var num = value;
                var hours = (num / 60);
                var rhours = Math.floor(hours);
                var minutes = (hours - rhours) * 60;
                var rminutes = Math.round(minutes);
                myResult = AppTool.PadLeft(rhours + "", 2, "0") + ":" + AppTool.PadLeft(rminutes + "", 2, "0");
            }
        }

        return myResult;
    }
}
