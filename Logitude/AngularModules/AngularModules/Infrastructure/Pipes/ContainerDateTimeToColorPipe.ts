import {Pipe} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../Tools';

@Pipe({ name: 'ContainerDateTimeToColorPipe' })

export class ContainerDateTimeToColorPipe {      
    transform(value: Date, comparisonDate: Date = null): string {

        var myResult = FontTool.Black;
        if (!AppTool.IsNullOrEmpty(value)) {
            var myDateTicks = DateTool.GetDateParts(value).DateTicks;

            var myDateFilterTicks: number;

            if (comparisonDate) {
                myDateFilterTicks = DateTool.GetDateParts(comparisonDate).DateTicks;
            }

            else {
                myDateFilterTicks = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateTicks;

                if (myDateTicks < myDateFilterTicks) {
                    myResult = FontTool.Red;
                }
                else if (myDateTicks == myDateFilterTicks) {
                    myResult = FontTool.Orange;
                }
            }
            
           
        
        }
        return myResult;
    }
}
