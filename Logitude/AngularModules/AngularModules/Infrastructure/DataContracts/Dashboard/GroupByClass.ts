export class GroupByClass {
    public day: number;
    public month: number;
    public year: number;
    public xField: string;
    public yField: number;
    public directionID: string;
    public transportID: string;
    public date: Date;
    public quarter: number;
    public dateRange: string;
    public startOfTheWeek: Date;
    public transportModeID: string;
    public directionName: string;
    public transportModeName: string;
    public countryName: string;
    public customerName: string;
    public dataType: string;
    public get Day() { return this.day; }
    public set Day(newValue: number) { this.day = newValue; }

    public get DataType() { return this.dataType; }
    public set DataType(newValue: string) { this.dataType = newValue; }


    public get Month() { return this.month; }
    public set Month(newValue: number) { this.month = newValue; }


    public get Year() { return this.year; }
    public set Year(newValue: number) { this.year = newValue; }


    public get XField() { return this.xField; }
    public set XField(newValue: string) { this.xField = newValue; }

    public get YField() { return this.yField; }
    public set YField(newValue: number) { this.yField = newValue; }

    public get DirectionID() { return this.directionID; }
    public set DirectionID(newValue: string) { this.directionID = newValue; }


    public get TransportID() { return this.transportID; }
    public set TransportID(newValue: string) { this.transportID = newValue; }

    public get Date() { return this.date; }
    public set Date(newValue: Date) { this.date = newValue; }


    public get Quarter() { return this.quarter; }
    public set Quarter(newValue: number) { this.quarter = newValue; }

    public get DateRange() { return this.dateRange; }
    public set DateRange(newValue: string) { this.dateRange = newValue; }

    public get StartOfTheWeek() { return this.startOfTheWeek; }
    public set StartOfTheWeek(newValue: Date) { this.startOfTheWeek = newValue; }

    public get TransportModeID() { return this.transportModeID; }
    public set TransportModeID(newValue: string) { this.transportModeID = newValue; }

    public get DirectionName() { return this.directionName; }
    public set DirectionName(newValue: string) { this.directionName = newValue; }

    public get TransportModeName() { return this.transportModeName; }
    public set TransportModeName(newValue: string) { this.transportModeName = newValue; }

    public get CountryName() { return this.countryName; }
    public set CountryName(newValue: string) { this.countryName = newValue; }

    public get CustomerName() { return this.customerName; }
    public set CustomerName(newValue: string) { this.customerName = newValue; }

}