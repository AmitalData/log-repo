"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DashBoardClass = /** @class */ (function () {
    //public static counter: number = 0;
    //private class;
    function DashBoardClass() {
        //this.linePrimary = DashBoardClass.counter+=1;
    }
    Object.defineProperty(DashBoardClass.prototype, "LinePrimary", {
        get: function () { return this.linePrimary; },
        set: function (newValue) { this.linePrimary = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "Day", {
        get: function () { return this.day; },
        set: function (newValue) { this.day = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "Month", {
        get: function () { return this.month; },
        set: function (newValue) { this.month = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "Year", {
        get: function () { return this.year; },
        set: function (newValue) { this.year = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "DirectionID", {
        get: function () { return this.directionID; },
        set: function (newValue) { this.directionID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "TransportModeID", {
        get: function () { return this.transportModeID; },
        set: function (newValue) { this.transportModeID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "DountryCode", {
        get: function () { return this.countryCode; },
        set: function (newValue) { this.countryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "CountryName", {
        get: function () { return this.countryName; },
        set: function (newValue) { this.countryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "Country", {
        get: function () { return this.country; },
        set: function (newValue) { this.country = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "ShipmentTypeId", {
        get: function () { return this.shipmentTypeId; },
        set: function (newValue) { this.shipmentTypeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "Total", {
        get: function () { return this.total; },
        set: function (newValue) { this.total = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "SumChargeableWeight", {
        get: function () { return this.sumChargeableWeight; },
        set: function (newValue) { this.sumChargeableWeight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "SumGrossWeight", {
        get: function () { return this.sumGrossWeight; },
        set: function (newValue) { this.sumGrossWeight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "TotalLastMonth", {
        get: function () { return this.totalLastMonth; },
        set: function (newValue) { this.totalLastMonth = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "SumChargeableWeightLastMonth", {
        get: function () { return this.sumChargeableWeightLastMonth; },
        set: function (newValue) { this.sumChargeableWeightLastMonth = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "SumGrossWeightLastMonth", {
        get: function () { return this.sumGrossWeightLastMonth; },
        set: function (newValue) { this.sumGrossWeightLastMonth = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "CustomerID", {
        get: function () { return this.customerID; },
        set: function (newValue) { this.customerID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "CustomerName", {
        get: function () { return this.customerName; },
        set: function (newValue) { this.customerName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "XField", {
        get: function () { return this.xField; },
        set: function (newValue) { this.xField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "YField", {
        get: function () { return this.yField; },
        set: function (newValue) { this.yField = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "Date", {
        get: function () { return this.date; },
        set: function (newValue) { this.date = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "IsExport", {
        get: function () { return this.isExport; },
        set: function (newValue) { this.isExport = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "TransportModeName", {
        get: function () { return this.transportModeName; },
        set: function (newValue) { this.transportModeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "DirectionName", {
        get: function () { return this.directionName; },
        set: function (newValue) { this.directionName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "StartOfTheWeek", {
        get: function () { return this.startOfTheWeek; },
        set: function (newValue) { this.startOfTheWeek = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "EndOfTheWeek", {
        get: function () { return this.endOfTheWeek; },
        set: function (newValue) { this.endOfTheWeek = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "DateRange", {
        get: function () { return this.dateRange; },
        set: function (newValue) { this.dateRange = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "TotalProfitInLocalCurrency", {
        get: function () { return this.totalProfitInLocalCurrency; },
        set: function (newValue) { this.totalProfitInLocalCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "TotalProfitInProfitCurrency", {
        get: function () { return this.totalProfitInProfitCurrency; },
        set: function (newValue) { this.totalProfitInProfitCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "ReceivablesInLocalCurrency", {
        get: function () { return this.receivablesInLocalCurrency; },
        set: function (newValue) { this.receivablesInLocalCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "ReceivablesInProfitCurrency", {
        get: function () { return this.receivablesInProfitCurrency; },
        set: function (newValue) { this.receivablesInProfitCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardClass.prototype, "GeneralTotal", {
        get: function () { return this.generalTotal; },
        set: function (newValue) { this.generalTotal = newValue; },
        enumerable: true,
        configurable: true
    });
    return DashBoardClass;
}());
exports.DashBoardClass = DashBoardClass;
//# sourceMappingURL=DashBoardClass.js.map