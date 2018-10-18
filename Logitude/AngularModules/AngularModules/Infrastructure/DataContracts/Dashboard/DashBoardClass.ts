export class DashBoardClass {

    //public static counter: number = 0;
    //private class;
    constructor() {

        //this.linePrimary = DashBoardClass.counter+=1;
    }
    public linePrimary: number;
    public day: number;
    public month: number;
    public year: number;
    public directionID: string;
    public transportModeID: string;
    public countryCode: string;
    public countryName: string;
    public country: string;
    public shipmentTypeId: string;
    public total: number;
    public sumChargeableWeight: number;
    public sumGrossWeight: number;
    public totalLastMonth: number;
    public sumChargeableWeightLastMonth: number;
    public sumGrossWeightLastMonth: number;
    public customerID: string;
    public customerName: string;
    public xField: string;
    public yField: number;
    public date: Date;
    public isExport: boolean;
    public transportModeName: string;
    public directionName: string;
    public startOfTheWeek: Date;
    public endOfTheWeek: Date;
    public dateRange: string;
    public totalProfitInLocalCurrency: number;
    public totalProfitInProfitCurrency: number;
    public receivablesInLocalCurrency: number;
    public receivablesInProfitCurrency: number;
    public generalTotal: number;

    public get LinePrimary() { return this.linePrimary; }
    public set LinePrimary(newValue: number) { this.linePrimary = newValue; }

    public get Day() { return this.day; }
    public set Day(newValue: number) { this.day = newValue; }

    public get Month() { return this.month; }
    public set Month(newValue: number) { this.month = newValue; }


    public get Year() { return this.year; }
    public set Year(newValue: number) { this.year = newValue; }


    public get DirectionID() { return this.directionID; }
    public set DirectionID(newValue: string) { this.directionID = newValue; }


    public get TransportModeID() { return this.transportModeID; }
    public set TransportModeID(newValue: string) { this.transportModeID = newValue; }


    public get DountryCode() { return this.countryCode; }
    public set DountryCode(newValue: string) { this.countryCode = newValue; }


    public get CountryName() { return this.countryName; }
    public set CountryName(newValue: string) { this.countryName = newValue; }


    public get Country() { return this.country; }
    public set Country(newValue: string) { this.country = newValue; }


    public get ShipmentTypeId() { return this.shipmentTypeId; }
    public set ShipmentTypeId(newValue: string) { this.shipmentTypeId = newValue; }


    public get Total() { return this.total; }
    public set Total(newValue: number) { this.total = newValue; }


    public get SumChargeableWeight() { return this.sumChargeableWeight; }
    public set SumChargeableWeight(newValue: number) { this.sumChargeableWeight = newValue; }


    public get SumGrossWeight() { return this.sumGrossWeight; }
    public set SumGrossWeight(newValue: number) { this.sumGrossWeight = newValue; }


    public get TotalLastMonth() { return this.totalLastMonth; }
    public set TotalLastMonth(newValue: number) { this.totalLastMonth = newValue; }


    public get SumChargeableWeightLastMonth() { return this.sumChargeableWeightLastMonth; }
    public set SumChargeableWeightLastMonth(newValue: number) { this.sumChargeableWeightLastMonth = newValue; }

    public get SumGrossWeightLastMonth() { return this.sumGrossWeightLastMonth; }
    public set SumGrossWeightLastMonth(newValue: number) { this.sumGrossWeightLastMonth = newValue; }

    public get CustomerID() { return this.customerID; }
    public set CustomerID(newValue: string) { this.customerID = newValue; }

    public get CustomerName() { return this.customerName; }
    public set CustomerName(newValue: string) { this.customerName = newValue; }

    public get XField() { return this.xField; }
    public set XField(newValue: string) { this.xField = newValue; }

    public get YField() { return this.yField; }
    public set YField(newValue: number) { this.yField = newValue; }

    public get Date() { return this.date; }
    public set Date(newValue: Date) { this.date = newValue; }

    public get IsExport() { return this.isExport; }
    public set IsExport(newValue: boolean) { this.isExport = newValue; }

    public get TransportModeName() { return this.transportModeName; }
    public set TransportModeName(newValue: string) { this.transportModeName = newValue; }

    public get DirectionName() { return this.directionName; }
    public set DirectionName(newValue: string) { this.directionName = newValue; }

    public get StartOfTheWeek() { return this.startOfTheWeek; }
    public set StartOfTheWeek(newValue: Date) { this.startOfTheWeek = newValue; }

    public get EndOfTheWeek() { return this.endOfTheWeek; }
    public set EndOfTheWeek(newValue: Date) { this.endOfTheWeek = newValue; }

    public get DateRange() { return this.dateRange; }
    public set DateRange(newValue: string) { this.dateRange = newValue; }

    public get TotalProfitInLocalCurrency() { return this.totalProfitInLocalCurrency; }
    public set TotalProfitInLocalCurrency(newValue: number) { this.totalProfitInLocalCurrency = newValue; }

    public get TotalProfitInProfitCurrency() { return this.totalProfitInProfitCurrency; }
    public set TotalProfitInProfitCurrency(newValue: number) { this.totalProfitInProfitCurrency = newValue; }

    public get ReceivablesInLocalCurrency() { return this.receivablesInLocalCurrency; }
    public set ReceivablesInLocalCurrency(newValue: number) { this.receivablesInLocalCurrency = newValue; }

    public get ReceivablesInProfitCurrency() { return this.receivablesInProfitCurrency; }
    public set ReceivablesInProfitCurrency(newValue: number) { this.receivablesInProfitCurrency = newValue; }

    public get GeneralTotal() { return this.generalTotal; }
    public set GeneralTotal(newValue: number) { this.generalTotal = newValue; }
}