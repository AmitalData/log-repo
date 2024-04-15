import {Pipe} from '@angular/core';
import {DateAgeHelper} from '../Utilities/DateAgeHelper';

import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

import {AppTool} from '../Tools';

@Pipe({ name: 'ExchangeRateDatePipe' })

export class ExchangeRateDatePipe {
    transform(value: Date): string {


        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(value)) {
         
            var helper: DateAgeHelper = new DateAgeHelper(value);

            if (helper) {
                if (helper.Years > 0) {
                    myResult = helper.Years == 1 ? helper.Years + " year ago" : helper.Years + " years ago";
                }

                else if (helper.Months > 0) {
                    myResult = helper.Months == 1 ? helper.Months + " month ago" : helper.Months + " months ago";
                }

                else {
                    if (helper.Days == 0) {
                        myResult = TextCodeTranslator.Translate("General.O.Today");
                    }

                    else if (helper.Days == 1) {
                        myResult = TextCodeTranslator.Translate("General.O.Yesterday");
                    }

                    else {
                        myResult = helper.Days + " days ago";
                    }
                }
            }


        }

        return myResult;
    }
}

