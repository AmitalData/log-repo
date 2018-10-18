import {Pipe} from '@angular/core';

import {DateAgeHelper} from '../Utilities/DateAgeHelper';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

import {AppTool} from '../Tools';

@Pipe({ name: 'DateTimeToMSDYDatePipe' })

export class DateTimeToMSDYDatePipe {
    transform(value: Date): string {


        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(value)) {
         
            var helper: DateAgeHelper = new DateAgeHelper(value);
            if (helper) {
                if (helper.Years > 0) {
                    myResult = helper.Years + " y";
                }

                else if (helper.Months > 0) {
                
                    myResult = Math.floor(Number(helper.Days / 7 ))+ "w";
                   // myResult = helper.Months + " m ago";
                }
                else if (helper.Week > 0) {
                    myResult = helper.Week + " w";
                }

                else if (helper.Days > 0) {
                    myResult = helper.Days + " d";
                }
                else if (helper.Hours > 0) {
                    myResult = helper.Hours + " h";
                }
                else if (helper.Minutes > 0) {
                    myResult = helper.Minutes + " m";
                }
                else if (helper.Seconds > 0) {
                    myResult = helper.Seconds + " s";
                }
                else {
                    myResult ="1 s ";
                }
            }


        }

        return myResult;
    }



}

