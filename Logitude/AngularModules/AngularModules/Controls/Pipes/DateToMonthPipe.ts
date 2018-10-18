import { Pipe } from '@angular/core';
import { AppTool } from '../../Infrastructure/Tools';
import { DateTool } from '../../Infrastructure/Tools';

@Pipe({ name: 'DateToMonthPipe' })


	export class DateToMonthPipe
{


  transform(value: any): string {
    var myResult: string = "";
    var month: string = null;
    var year: string = null;
    if (value) {
   
      if (typeof (value) == "string") {

     var chars: string[] = value.split("-");
     month = chars[1];
     year = chars[0];
      }
      else {
        month = value.getUTCMonth();
        year = value.getFullYear();
    
      }
    }
    myResult = month + "." + year;
    return myResult;
  }
  static Pipe(value: Date): Date
  {
   var  myResult:Date =null;

    if (value) {
      myResult = DateTool.GetDateFromDate(value);
    }

    return myResult;
  }


}
