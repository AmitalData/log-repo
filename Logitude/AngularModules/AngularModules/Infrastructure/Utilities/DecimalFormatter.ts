//import {isBlank, isNumber} from '@angular/common/src/facade/lang';
import {AppTool, FormatTool} from '../Tools';

export class DecimalFormatter {

    public static format(value: number, maxDigits): string {

        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(value)) {
            if (FormatTool.IsDecimal(value.toString())){
                var myMinFractionDigits = 0;
                var myMaxFractionDigits = 0;

                if (!AppTool.IsNullOrEmpty(maxDigits)) {
                    myMinFractionDigits = myMaxFractionDigits = maxDigits;
                }

                myResult = FormatTool.FormatNumber(value, "N" + myMinFractionDigits);/* value.toLocaleString('en-US', { minimumFractionDigits: myMinFractionDigits, maximumFractionDigits: myMaxFractionDigits });*/
            }
        }

        return myResult;
    }
}
