import {DatePipe} from '@angular/common';
import {DateTool} from '../Tools';

export class LastFilter {
    public lastTitle: string;
    public Lastmonths: number;
    public LastDays: number;
    public static myList(): Array<LastFilter> {
        var datePipe: DatePipe = new DatePipe("en-US");
        var list: Array<LastFilter> = new Array<LastFilter>();
        var from: string = null;
        var to: string = datePipe.transform(DateTool.GetCurrentDateAsUtc(), 'dd MMM yy');
        to = to.substr(0, 7) + "'" + to.substr(7);
        var fromdate: Date = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 29);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter0: LastFilter = new LastFilter();
        lastFilter0.lastTitle = "Last month" + " (" + from + " - " + to + ")";
        lastFilter0.LastDays = -30;
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 6);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter1: LastFilter = new LastFilter();
        lastFilter1.lastTitle = "Last 7 days" + " (" + from + " - " + to + ")"
        lastFilter1.LastDays = -7;
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 89);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter2: LastFilter = new LastFilter();
        lastFilter2.lastTitle = "Last 3 months" + " (" + from + " - " + to + ")";
        lastFilter2.LastDays = -90;
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 365);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);

        var lastFilter5: LastFilter = new LastFilter();
        lastFilter5.lastTitle = "Last year" + " (" + from + " - " + to + ")";
        lastFilter5.LastDays = -365;

        list.push(lastFilter1);
        list.push(lastFilter0);
        list.push(lastFilter2);
        list.push(lastFilter5);
        return list;
    }
    public static ActivitymyList(): Array<LastFilter> {
        var datePipe: DatePipe = new DatePipe("en-US");
        var list: Array<LastFilter> = new Array<LastFilter>();
        var from: string = null;
        var to: string = datePipe.transform(DateTool.GetCurrentDateAsUtc(), 'dd MMM yy');
        to = to.substr(0, 7) + "'" + to.substr(7);
        var fromdate: Date = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 29);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter0: LastFilter = new LastFilter();
        lastFilter0.lastTitle = "Last month";
        lastFilter0.LastDays = -30;
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 6);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter1: LastFilter = new LastFilter();
        lastFilter1.lastTitle = "Last 7 days";
        lastFilter1.LastDays = -7;
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 89);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter2: LastFilter = new LastFilter();
        lastFilter2.lastTitle = "Last 3 months";
        lastFilter2.LastDays = -90;
        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 365);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);

        var lastFilter5: LastFilter = new LastFilter();
        lastFilter5.lastTitle = "Last year";
        lastFilter5.LastDays = -365;



        var lastFilter6: LastFilter = new LastFilter();
        lastFilter6.lastTitle = "Custom";
        lastFilter6.LastDays = 0;
        lastFilter6.Lastmonths = 0;

        list.push(lastFilter1);
        list.push(lastFilter0);
        list.push(lastFilter2);
        list.push(lastFilter5);
        list.push(lastFilter6);

        return list;
    }
    public static MyCRMList(): Array<LastFilter> {

        var datePipe: DatePipe = new DatePipe("en-US");
        var list: Array<LastFilter> = new Array<LastFilter>();
        var from: string = null;
        var to: string = datePipe.transform(DateTool.GetCurrentDateAsUtc(), 'dd MMM yy');
        to = to.substr(0, 7) + "'" + to.substr(7);

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 89);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter2: LastFilter = new LastFilter();
        lastFilter2.lastTitle = "Last 3 months" + " (" + from + " - " + to + ")";
        lastFilter2.LastDays = -90;
        lastFilter2.Lastmonths = 0;

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 365);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);

        var lastFilter5: LastFilter = new LastFilter();
        lastFilter5.lastTitle = "Last year" + " (" + from + " - " + to + ")";
        lastFilter5.LastDays = -365;
        lastFilter5.Lastmonths = 0;


        var fromdate: Date = DateTool.GetCurrentDateAsUtc();

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 1095);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter1: LastFilter = new LastFilter();
        lastFilter1.lastTitle = "Last 3 years" + " (" + from + " - " + to + ")"
        lastFilter1.LastDays = 0;
        lastFilter1.Lastmonths = -36;

        list.push(lastFilter2);
        list.push(lastFilter5);
        list.push(lastFilter1);

        return list;



    }
    public static MyCRMListDefault(): Array<LastFilter> {

        var datePipe: DatePipe = new DatePipe("en-US");
        var list: Array<LastFilter> = new Array<LastFilter>();
        var from: string = null;
        var to: string = datePipe.transform(DateTool.GetCurrentDateAsUtc(), 'dd MMM yy');
        to = to.substr(0, 7) + "'" + to.substr(7);

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 89);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter2: LastFilter = new LastFilter();
        lastFilter2.lastTitle = "Last 3 months";
        lastFilter2.LastDays = -90;
        lastFilter2.Lastmonths = 0;

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 365);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);

        var lastFilter5: LastFilter = new LastFilter();
        lastFilter5.lastTitle = "Last year";
        lastFilter5.LastDays = -365;
        lastFilter5.Lastmonths = 0;


        var fromdate: Date = DateTool.GetCurrentDateAsUtc();

        fromdate = DateTool.GetCurrentDateAsUtc();
        fromdate.setDate(fromdate.getDate() - 1095);
        from = datePipe.transform(fromdate, 'dd MMM yy');
        from = from.substr(0, 7) + "'" + from.substr(7);
        var lastFilter1: LastFilter = new LastFilter();
        lastFilter1.lastTitle = "Last 3 years";
        lastFilter1.LastDays = 0;
        lastFilter1.Lastmonths = -36;


        var lastFilter4: LastFilter = new LastFilter();
        lastFilter4.lastTitle = "Custom";
        




        list.push(lastFilter2);
        list.push(lastFilter5);
        list.push(lastFilter1);
        list.push(lastFilter4);

        return list;



    }
}