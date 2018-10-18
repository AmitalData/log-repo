import {CodeNameClass} from '../Infrastructure/DataContracts/CodeNameClass';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../Infrastructure/Tools';
import {DatePipe} from '@angular/common';
export class CRMUtilities {




    public static GetDateFilterList() {
        var datePipe: DatePipe = new DatePipe("en-US");
        var list: Array<CodeNameClass> = new Array<CodeNameClass>();
        var from: string = null;
        var to: string = datePipe.transform(DateTool.GetCurrentDateAsUtc(), 'dd MMM yy');
        to = to.substr(0, 7) + "'" + to.substr(7);
        var fromdate: Date = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 29);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter0: CodeNameClass = new CodeNameClass();
        lastFilter0.Name = "Last 30 days";
        lastFilter0.Code = "-30";     
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 6);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter1: CodeNameClass = new CodeNameClass();
        lastFilter1.Name = "Last 7 days"
        lastFilter1.Code = "-7";

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 1);
        var yesrarday: string = datePipe.transform(fromdate, 'dd MMM yy');
        yesrarday = yesrarday.substr(0, 7) + "'" + yesrarday.substr(7);

        var lastFilterYestarday: CodeNameClass = new CodeNameClass();
        lastFilterYestarday.Name = "Yesterday"
        lastFilterYestarday.Code = "-1";

        var lastFilterToday: CodeNameClass = new CodeNameClass();

        lastFilterToday.Name = "Today"
        lastFilterToday.Code = "0";

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 89);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter2: CodeNameClass = new CodeNameClass();
        lastFilter2.Name = "Last 90 days";
        lastFilter2.Code = "-90";
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 365);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);

        var lastFilter5: CodeNameClass = new CodeNameClass();
        lastFilter5.Name = "Last Year";
        lastFilter5.Code = "-365";

        var lastFilter6: CodeNameClass = new CodeNameClass();
        lastFilter6.Name = "Custom";
        lastFilter6.Code = "-2";


        list.push(lastFilterToday);
        list.push(lastFilterYestarday);
        list.push(lastFilter1);
        list.push(lastFilter0);
        list.push(lastFilter2);
        list.push(lastFilter5);
        list.push(lastFilter6);

        return list;

    }

   private static getDaysInMonth(m, y) {
    return m === 2 ? y & 3 || !(y % 25) && y & 15 ? 28 : 29 : 30 + (m + (m >> 3) & 1);
}

    public static GetClosingDateFilterList() {
        var datePipe: DatePipe = new DatePipe("en-US");
        var list: Array<CodeNameClass> = new Array<CodeNameClass>();
        var from: string = null;
        var to: string = datePipe.transform(DateTool.GetCurrentDateAsUtc(), 'dd MMM yy');
        to = to.substr(0, 7) + "'" + to.substr(7);
        var fromdate: Date = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 29);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter0: CodeNameClass = new CodeNameClass();
        lastFilter0.Name = "Last 30 days";
        lastFilter0.Code = "-30";
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 6);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter1: CodeNameClass = new CodeNameClass();
        lastFilter1.Name = "Last 7 days";
        lastFilter1.Code = "-7";

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 1);
        var yesrarday: string = datePipe.transform(fromdate, 'dd MMM yy');
        yesrarday = yesrarday.substr(0, 7) + "'" + yesrarday.substr(7);

        var lastFilterYestarday: CodeNameClass = new CodeNameClass();
        lastFilterYestarday.Name = "Yesterday";
        lastFilterYestarday.Code = "-1";
     
        var lastFilterToday: CodeNameClass = new CodeNameClass();

        lastFilterToday.Name = "Today";
        lastFilterToday.Code = "0";
       
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 89);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter2: CodeNameClass = new CodeNameClass();
        lastFilter2.Name = "Last 90 days";
        lastFilter2.Code = "-90";
       
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 365);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);

        var lastFilter5: CodeNameClass = new CodeNameClass();
        lastFilter5.Name = "Last Year";
        lastFilter5.Code = "-365";
        


        var date1: Date = DateTool.GetCurrentDateAsUtc();
        var date2: Date = DateTool.GetCurrentDateAsUtc();

        date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear() - 1, 0, 1);
        date2.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear() - 1, 11, 31);

        var date1String = datePipe.transform(date1, 'dd MMM yy');
        date1String = date1String.substr(0, 7) + "'" + date1String.substr(7);

        var date2String = datePipe.transform(date2, 'dd MMM yy');
        date2String = date2String.substr(0, 7) + "'" + date2String.substr(7);

        var lastFilter6: CodeNameClass = new CodeNameClass();
        lastFilter6.Name = "Last year only";
        lastFilter6.Code = "-365_1";
        lastFilter6.FromDate = date1;
        lastFilter6.ToDate = date2;


        var date1: Date = DateTool.GetCurrentDateAsUtc();
        var date2: Date = DateTool.GetCurrentDateAsUtc();

        if (DateTool.GetCurrentDateAsUtc().getUTCMonth() == 0) {
            date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear() - 1, 11, 1);
        }
        else {
            date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), DateTool.GetCurrentDateAsUtc().getUTCMonth()-1, 1);
        }

        date2.setFullYear(date1.getUTCFullYear(), date1.getUTCMonth(),this.getDaysInMonth(date1.getUTCMonth()+1, date1.getUTCFullYear()));

        var date1String = datePipe.transform(date1, 'dd MMM yy');
        date1String = date1String.substr(0, 7) + "'" + date1String.substr(7);

        var date2String = datePipe.transform(date2, 'dd MMM yy');
        date2String = date2String.substr(0, 7) + "'" + date2String.substr(7);

        var lastFilter7: CodeNameClass = new CodeNameClass();
        lastFilter7.Name = "Last month";
        lastFilter7.Code = "-30_1";
        lastFilter7.FromDate = date1;
        lastFilter7.ToDate = date2;

        var date1: Date = DateTool.GetCurrentDateAsUtc();
        var date2: Date = DateTool.GetCurrentDateAsUtc();

        switch (DateTool.GetCurrentDateAsUtc().getUTCMonth()) {
            case 1:
            case 2:
            case 3:
                {
                    date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear() - 1, 9, 1);
                    date2.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear() - 1, 11, 31);
                    break;
                }

            case 4:
            case 5:
            case 6:
                {
                    date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear() , 0, 1);
                    date2.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), 2, 31);
                    break;
                }

            case 7:
            case 8:
            case 9:
                {
                    date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), 3, 1);
                    date2.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), 5, 30);
                    break;
                }

            case 10:
            case 11:
            case 12:
                {
                    date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), 6, 1);
                    date2.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), 8, 30);
                    break;
                }
        }

        var date1String = datePipe.transform(date1, 'dd MMM yy');
        date1String = date1String.substr(0, 7) + "'" + date1String.substr(7);

        var date2String = datePipe.transform(date2, 'dd MMM yy');
        date2String = date2String.substr(0, 7) + "'" + date2String.substr(7);

        var lastFilter8: CodeNameClass = new CodeNameClass();
        lastFilter8.Name = "Last quarter";
        lastFilter8.Code = "-90_1";
        lastFilter8.FromDate = date1;
        lastFilter8.ToDate = date2;


        var date1: Date = DateTool.GetCurrentDateAsUtc();
        var date2: Date = DateTool.GetCurrentDateAsUtc();

        date1.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), DateTool.GetCurrentDateAsUtc().getUTCMonth(), 1);
        date2.setFullYear(DateTool.GetCurrentDateAsUtc().getFullYear(), DateTool.GetCurrentDateAsUtc().getUTCMonth(), this.getDaysInMonth(DateTool.GetCurrentDateAsUtc().getUTCMonth()+1,DateTool.GetCurrentDateAsUtc().getUTCFullYear()));

        var date1String = datePipe.transform(date1, 'dd MMM yy');
        date1String = date1String.substr(0, 7) + "'" + date1String.substr(7);

        var date2String = datePipe.transform(date2, 'dd MMM yy');
        date2String = date2String.substr(0, 7) + "'" + date2String.substr(7);

        var lastFilter9: CodeNameClass = new CodeNameClass();
        lastFilter9.Name = "Current month";
        lastFilter9.Code = "0_1";
        lastFilter9.FromDate = date1;
        lastFilter9.ToDate = date2;

        var lastFilter10: CodeNameClass = new CodeNameClass();
        lastFilter10.Name = "Custom";
        lastFilter10.Code = "-1_-1";


        list.push(lastFilterToday);
        list.push(lastFilterYestarday);
        list.push(lastFilter1);
        list.push(lastFilter0);
        list.push(lastFilter2);
        list.push(lastFilter5);
        list.push(lastFilter6);
        list.push(lastFilter7);
        list.push(lastFilter8);
        list.push(lastFilter9);
        list.push(lastFilter10);

        return list;


    }
  

}