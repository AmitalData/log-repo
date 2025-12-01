import {Pipe} from '@angular/core';
import {AppTool, DateTool} from '../Tools';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

@Pipe({ name: 'FollowUpDatePipe' })

export class FollowUpDatePipe {

    transform(value: Date, parameter: string): string {

        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(value)) {
            var todayDate = DateTool.GetCurrentDateAsUtc();
            
            if (parameter != null) {
                if (parameter == "F") {

                    myResult = "black";
                    if (value != null) {

                        if (value.valueOf() < todayDate.valueOf()) {
                            myResult = "red";
                        }
                    }

                }

            }

            else {
                myResult = "white";
                var myDate: Date = DateTool.GetDateFromDate(value, true);

                if (value != null) {
                    if (myDate.valueOf() < todayDate.valueOf()) {
                        myResult = "#F7E3E3";
                    }
                }

               
            }
        }

        return myResult;
    }
}