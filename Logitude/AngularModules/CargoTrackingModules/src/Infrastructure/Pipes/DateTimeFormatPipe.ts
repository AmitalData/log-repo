import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dateTimeFormat'
})
export class DateTimeFormatPipe implements PipeTransform {

    transform(date: string): string {
        if(date) {
        var time = date.split(',')[1];
        var dateWithoutTime = date.split(',')[0];

        if (dateWithoutTime?.startsWith("00:00"))
            return '';
        if (time?.startsWith(" 00:00"))
            return dateWithoutTime;
        else
            return date.toString();
        }
    }
}
