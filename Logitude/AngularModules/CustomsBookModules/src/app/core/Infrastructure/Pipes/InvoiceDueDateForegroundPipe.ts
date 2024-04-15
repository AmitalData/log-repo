import {Pipe} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../Tools';

@Pipe({ name: 'InvoiceDueDateForegroundPipe' })

export class InvoiceDueDateForegroundPipe {
    transform(dateField: Date, isClosed: boolean): string {

        var myResult = FontTool.Black;

        if (!AppTool.IsNullOrEmpty(dateField) && !isClosed) {
            var myDateTicks = DateTool.GetDateParts(dateField).DateTicks;
            var todayDateTicks = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateTicks;
            if (myDateTicks < todayDateTicks) {
                myResult = FontTool.Red;
            }
            //var todayDate: Date = DateTool.GetCurrentDateAsUtc();
            ////todayDate = DateTool.TruncateTime(todayDate);
            //var entityDate: Date = DateTool.GetDateParts(dateField).DateObject;
            ////entityDate = DateTool.TruncateTime(entityDate);
            //if (entityDate.valueOf() < todayDate.valueOf()) {
            //    myResult = FontTool.Red;
            //}
        }
        return myResult;
    }
}