import {Pipe} from '@angular/core';

@Pipe({ name: 'RatesNumbersPipe' })

export class RatesNumbersPipe {
    transform(myNumber: number, myFormat: string): string {

        var myResult: string = "";

        if (myNumber != null && !isNaN(myNumber)) {

            if (myFormat == null) {
                myResult = myNumber.toString();
            }

            else if (myFormat.length == 0) {
                myResult = myNumber.toString();
            }

            else {

                var myFractionDigits = 0;

                switch (myFormat.toString().toLowerCase()) {
                    case "n1": { myFractionDigits = 1; break; }
                    case "n2": { myFractionDigits = 2; break; }
                    case "n3": { myFractionDigits = 3; break; }
                    case "n4": { myFractionDigits = 4; break; }
                    case "n5": { myFractionDigits = 5; break; }
                }

                myResult = myNumber.toFixed(myFractionDigits).toString();

            }
        }

        return myResult;
    }
}
