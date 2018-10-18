
export class DateTimeZone {

    public static TimeZonesList: Array<TimeZoneInfoClass>;
    public static GetTimeZonesList() {

        if (DateTimeZone.TimeZonesList == null) {
            this.TimeZonesList = new Array<TimeZoneInfoClass>();
        }

        var zone1 = new TimeZoneInfoClass("(UTC)", 0);
        var zone2 = new TimeZoneInfoClass("(UTC+01:00)", 1);
        var zone3 = new TimeZoneInfoClass("(UTC+02:00)", 2);
        var zone4 = new TimeZoneInfoClass("(UTC+03:00)", 3);
        var zone5 = new TimeZoneInfoClass("(UTC+03:30)", 3.5);
        var zone6 = new TimeZoneInfoClass("(UTC+04:00)", 4);
        var zone7 = new TimeZoneInfoClass("(UTC+04:30)", 4.5);
        var zone8 = new TimeZoneInfoClass("(UTC+05:00)", 5);
        var zone9 = new TimeZoneInfoClass("(UTC+05:30)", 5.5);
        var zone10 = new TimeZoneInfoClass("(UTC+06:00)", 6);
        var zone11 = new TimeZoneInfoClass("(UTC+06:30)", 6.5);
        var zone12 = new TimeZoneInfoClass("(UTC+07:00)", 7);
        var zone13 = new TimeZoneInfoClass("(UTC+08:00)", 8);
        var zone14 = new TimeZoneInfoClass("(UTC+09:00)", 9);
        var zone15 = new TimeZoneInfoClass("(UTC+09:30)", 9.5);
        var zone16 = new TimeZoneInfoClass("(UTC+010:00)", 10);
        var zone17 = new TimeZoneInfoClass("(UTC+011:00)", 11);
        var zone18 = new TimeZoneInfoClass("(UTC+012:00)", 12);
        var zone19 = new TimeZoneInfoClass("(UTC+013:00)", 13);
        var zone20 = new TimeZoneInfoClass("(UTC-01:00)", -1);
        var zone21 = new TimeZoneInfoClass("(UTC-02:00)", -2);
        var zone22 = new TimeZoneInfoClass("(UTC-03:00)", -3);
        var zone23 = new TimeZoneInfoClass("(UTC-03:30)", -3.5);
        var zone24 = new TimeZoneInfoClass("(UTC-04:00)", -4);
        var zone25 = new TimeZoneInfoClass("(UTC-04:30)", -4.5);
        var zone26 = new TimeZoneInfoClass("(UTC-05:00)", -5);
        var zone27 = new TimeZoneInfoClass("(UTC-06:00)", -6);
        var zone28 = new TimeZoneInfoClass("(UTC-07:00)", -7);
        var zone29 = new TimeZoneInfoClass("(UTC-08:00)", -8);
        var zone30 = new TimeZoneInfoClass("(UTC-09:00)", -9);
        var zone31 = new TimeZoneInfoClass("(UTC-10:00)", -10);
        var zone32 = new TimeZoneInfoClass("(UTC-11:00)", -11);
        var zone33 = new TimeZoneInfoClass("(UTC-12:00)", -12);

        if (DateTimeZone.TimeZonesList.length <= 0) {
            this.TimeZonesList.push(zone1);
            this.TimeZonesList.push(zone2);
            this.TimeZonesList.push(zone3);
            this.TimeZonesList.push(zone4);
            this.TimeZonesList.push(zone5);
            this.TimeZonesList.push(zone6);
            this.TimeZonesList.push(zone7);
            this.TimeZonesList.push(zone8);
            this.TimeZonesList.push(zone9);
            this.TimeZonesList.push(zone10);
            this.TimeZonesList.push(zone11);
            this.TimeZonesList.push(zone12);
            this.TimeZonesList.push(zone13);
            this.TimeZonesList.push(zone14);
            this.TimeZonesList.push(zone15);
            this.TimeZonesList.push(zone16);
            this.TimeZonesList.push(zone17);
            this.TimeZonesList.push(zone18);
            this.TimeZonesList.push(zone19);
            this.TimeZonesList.push(zone20);
            this.TimeZonesList.push(zone21);
            this.TimeZonesList.push(zone22);
            this.TimeZonesList.push(zone23);
            this.TimeZonesList.push(zone24);
            this.TimeZonesList.push(zone25);
            this.TimeZonesList.push(zone26);
            this.TimeZonesList.push(zone27);
            this.TimeZonesList.push(zone28);
            this.TimeZonesList.push(zone29);
            this.TimeZonesList.push(zone30);
            this.TimeZonesList.push(zone31);
            this.TimeZonesList.push(zone32);
            this.TimeZonesList.push(zone33);
        }

        return DateTimeZone.TimeZonesList;
    }

    public static GetDateTimeFormats() {
        var DateTimeFormatsList: Array<DateTimeFormat> = new Array<DateTimeFormat>();
        var dateTime1 = new DateTimeFormat("dd\\/MM\\/yyyy", "dd/MM/yyyy");
        var dateTime2 = new DateTimeFormat("MM\\/dd\\/yyyy", "MM/dd/yyyy");
        DateTimeFormatsList.push(dateTime1);
        DateTimeFormatsList.push(dateTime2);
        return DateTimeFormatsList;
    }
}

export class TimeZoneInfoClass {
    public BaseUtcOffset: number;
    public TimeZoneName: string;
    constructor(public timeZoneName: string, public baseUtcOffset: number) {
        this.BaseUtcOffset = baseUtcOffset;
        this.TimeZoneName = timeZoneName;
    }
}

export class DateTimeFormat {
    public Format: string;
    public DisplayName: string;
    constructor(public format: string, public displaname: string) {
        this.Format = format;
        this.DisplayName = displaname;
    }
}