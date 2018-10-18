import {isBlank, isNumber, NumberWrapper} from 'angular2/src/facade/lang';
import {NumberFormatter, NumberFormatStyle} from 'angular2/src/facade/intl';

export class DecimalFormatter {

    public static format(value: number, maxDigits): string {

        var myResult: string = "";

        if (value != null) {
            if (!isBlank(value)) {
                if (isNumber(value)) {

                    var fractionDigits = 3;
                    var defaultLocale: string = 'en-US';
                    var formatStyle: NumberFormatStyle = NumberFormatStyle.Decimal;

                    if (!isBlank(maxDigits)) {
                        fractionDigits = maxDigits;
                    }

                    //myResult = NumberWrapper.toFixed(value, 3);

                    myResult = NumberFormatter.format(value, defaultLocale, formatStyle, { minimumIntegerDigits: 1, minimumFractionDigits: 2, maximumFractionDigits: 3, currency: null, currencyAsSymbol: false });
                }
            }
        }

        return myResult;
    }
}