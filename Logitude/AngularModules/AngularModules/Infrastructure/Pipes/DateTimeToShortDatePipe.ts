import {Pipe} from '@angular/core';
import {AppTool, DateTool} from '../Tools';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';
import {SessionLocator} from '../Utilities/SessionLocator';

@Pipe({ name: 'DateTimeToShortDatePipe' })

export class DateTimeToShortDatePipe {


    transform(value: Date): string {

        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(value)) {

            var myDate: Date = DateTool.GetDateFromDate(value, true);
            var myCurrentDateTime: Date = DateTool.GetCurrentDateAsUtc();
            var plus: Date = DateTool.GetDateByDay(1);
            var minus: Date = DateTool.GetDateByDay(-1);
            var yyyy = myDate.getFullYear().toString();
            var mm = (myDate.getMonth() + 1).toString();
            var dd = myDate.getDate().toString();

            if (dd.length == 1) {
                dd = "0" + dd;
            }

            if (mm.length == 1) {
                mm = "0" + mm;
            }

            var myResult = "";

            var tenantFormat = SessionLocator.TenantPM.DateTimeFormat;
            var compare = tenantFormat.localeCompare("MM\/dd\/yyyy");
            if (compare != -1) {
                myResult = mm + "/" + dd + "/" + yyyy;
            }
            else {
                myResult = dd + "/" + mm + "/" + yyyy;
            }
            
        }

        return myResult;
    }

}