import {Pipe} from '@angular/core';

@Pipe({ name: 'CustomNumbersPipe' })

export class CustomNumbersPipe {
    transform(myNumber: number, myFractionDigits: number): string {

        var myResult: string = "";

        if (myNumber != null && !isNaN(myNumber)) {
        
            if (myFractionDigits == 0) {
                //myResult = myNumber.toLocaleString('en-US'); (not working with Safari)
                myResult = myNumber.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }

            //else if (myFormat.length == 0) {
            //    //myResult = myNumber.toLocaleString('en-US'); (not working with Safari)
            //    myResult = myNumber.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            //}

            else {

                //var myFractionDigits = 0;

                //switch (myFormat.toString().toLowerCase()) {
                //    case "n1": { myFractionDigits = 1; break; }
                //    case "n2": { myFractionDigits = 2; break; }
                //    case "n3": { myFractionDigits = 3; break; }
                //    case "n4": { myFractionDigits = 4; break; }
                //    case "n5": { myFractionDigits = 5; break; }
                //}

                myResult = myNumber.toFixed(myFractionDigits).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

                //(not working with Safari)
                //myResult = myNumber.toLocaleString('en-US', { minimumFractionDigits: myMinFractionDigits, maximumFractionDigits: myMaxFractionDigits });
                //myResult = parseFloat((Math.round(myNumber * 100) / 100).toString()).toFixed(myMinFractionDigits).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }

        return myResult;
    }
}
