"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var List_1 = require("./List");
var DashBoardClass_1 = require("./DashBoardClass");
var GroupByClass_1 = require("./GroupByClass");
var Tools_1 = require("../../Tools");
var ChartingDataClass_1 = require("./ChartingDataClass");
var FunctionsCRM = /** @class */ (function () {
    function FunctionsCRM() {
    }
    FunctionsCRM.getDirectionFilteredList = function (dtf, dataList) {
        var FilteredData = new List_1.List();
        var transportId = dtf.FilterTransportID;
        var directionId = dtf.FilterDirectionID;
        if ((transportId == null || transportId == "") && (directionId == null || directionId == "")) {
            FilteredData = dataList;
        }
        else if (!(transportId == null || transportId == "") && (directionId == null || directionId == "")) {
            dataList.getAll();
            FilteredData.Assign(dataList.getAll().filter(function (s) { return s.transportModeID == transportId; }));
        }
        else if ((transportId == null || transportId == "") && !(directionId == null || directionId == "")) {
            FilteredData.Assign(dataList.getAll().filter(function (s) { return s.DirectionID == directionId; }));
        }
        else {
            FilteredData.Assign(dataList.getAll().filter(function (s) { return s.DirectionID == directionId && s.TransportModeID == transportId; }));
        }
        return FilteredData;
    };
    FunctionsCRM.getMeasurmentFilterList = function (measurment, dataList, monthFilterIndex, customerid) {
        var SeconGroup = [];
        var resultList = new List_1.List();
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        // Region Customer is null 
        if (customerid == "" || customerid == null) {
            // Region By Weeks
            if (monthFilterIndex == 1 || monthFilterIndex == 2) {
                switch (parseInt(measurment + "")) {
                    case 0:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    existsedItem.YField = item.total;
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    existsedItem.YField += item.total;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 1:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.sumChargeableWeight == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.sumChargeableWeight + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumChargeableWeight == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.sumChargeableWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 2:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.sumGrossWeight == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.sumGrossWeight + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumGrossWeight == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.sumGrossWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 3:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.totalProfitInLocalCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.totalProfitInLocalCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInLocalCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.totalProfitInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 4:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.totalProfitInProfitCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.totalProfitInProfitCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInProfitCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.totalProfitInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 5:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.ReceivablesInLocalCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.ReceivablesInLocalCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.ReceivablesInLocalCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.ReceivablesInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 6:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.ReceivablesInProfitCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.ReceivablesInProfitCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.ReceivablesInProfitCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.ReceivablesInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    default: break;
                }
            }
            // // End Region By Weeks
            ////Region By Months
            if (monthFilterIndex == 3) {
                switch (parseInt(measurment + "")) {
                    case 0:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "/" + item.year;
                                    var total = 0;
                                    existsedItem.YField = item.total;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    existsedItem.YField += item.total;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 1:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    var total = 0;
                                    if (item.sumChargeableWeight == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.sumChargeableWeight + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumChargeableWeight == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.sumChargeableWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 2:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    var total = 0;
                                    if (item.sumGrossWeight == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.sumGrossWeight + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumGrossWeight == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.sumGrossWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 3:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    var total = 0;
                                    if (item.TotalProfitInLocalCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.TotalProfitInLocalCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.TotalProfitInLocalCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.TotalProfitInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 4:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    var total = 0;
                                    if (item.totalProfitInProfitCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.totalProfitInProfitCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInProfitCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.totalProfitInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 5:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    var total = 0;
                                    if (item.receivablesInLocalCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.receivablesInLocalCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInLocalCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.receivablesInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 6:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    var total = 0;
                                    if (item.receivablesInProfitCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = parseFloat(item.receivablesInProfitCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInProfitCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += parseFloat(item.receivablesInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    default: break;
                }
            }
            // // End Region By Months
            //     //Region By Days 
            if (monthFilterIndex == 0) {
                switch (parseInt(measurment + "")) {
                    case 0:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    existsedItem.YField = item.total;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    existsedItem.YField += item.total;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 1:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    if (item.sumChargeableWeight == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = item.sumChargeableWeight;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumChargeableWeight == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += item.sumChargeableWeight;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 2:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    if (item.sumGrossWeight == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = item.sumGrossWeight;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumGrossWeight == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += item.sumGrossWeight;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 3:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    if (item.totalProfitInLocalCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = item.totalProfitInLocalCurrency;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInLocalCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += item.totalProfitInLocalCurrency;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 4:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    if (item.totalProfitInProfitCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = item.totalProfitInProfitCurrency;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInProfitCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += item.totalProfitInProfitCurrency;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 5:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    if (item.ReceivablesInLocalCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = item.ReceivablesInLocalCurrency;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.ReceivablesInLocalCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += item.ReceivablesInLocalCurrency;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 6:
                        {
                            var groupedData = [];
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.day == item.Day && f.month == item.Month && f.year == item.Year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    existsedItem.Month = item.Month;
                                    if (item.ReceivablesInProfitCurrency == null)
                                        existsedItem.YField = 0;
                                    else
                                        existsedItem.YField = item.ReceivablesInProfitCurrency;
                                    existsedItem.Year = item.Year;
                                    existsedItem.Day = item.Day;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.ReceivablesInProfitCurrency == null)
                                        existsedItem.YField += 0;
                                    else
                                        existsedItem.YField += item.ReceivablesInProfitCurrency;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    default: break;
                }
            }
        }
        // Region Customer is not null
        else {
            //By Months
            if (parseInt(monthFilterIndex + "") == 1) {
                switch (parseInt(measurment + "")) {
                    case 0:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.total == null)
                                        item.total = 0;
                                    existsedItem.yField = item.total;
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.total == null)
                                        item.total = 0;
                                    existsedItem.yField += item.total;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 1:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.sumChargeableWeight == null)
                                        item.sumChargeableWeight = 0;
                                    existsedItem.yField = parseFloat(item.sumChargeableWeight + "");
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumChargeableWeight == null)
                                        item.sumChargeableWeight = 0;
                                    existsedItem.yField += parseFloat(item.sumChargeableWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 2:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.sumGrossWeight == null)
                                        item.sumGrossWeight = 0;
                                    existsedItem.yField = parseFloat(item.sumGrossWeight + "");
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumGrossWeight == null)
                                        item.sumGrossWeight = 0;
                                    existsedItem.yField += parseFloat(item.sumGrossWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 3:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.totalProfitInLocalCurrency == null)
                                        item.totalProfitInLocalCurrency = 0;
                                    existsedItem.yField = parseFloat(item.totalProfitInLocalCurrency + "");
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInLocalCurrency == null)
                                        item.totalProfitInLocalCurrency = 0;
                                    existsedItem.yField += parseFloat(item.totalProfitInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 4:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.totalProfitInProfitCurrency == null)
                                        item.totalProfitInProfitCurrency = 0;
                                    existsedItem.yField = parseFloat(item.totalProfitInProfitCurrency + "");
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInProfitCurrency == null)
                                        item.totalProfitInProfitCurrency = 0;
                                    existsedItem.yField += parseFloat(item.totalProfitInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 5:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.receivablesInLocalCurrency == null)
                                        item.receivablesInLocalCurrency = 0;
                                    existsedItem.yField = parseFloat(item.receivablesInLocalCurrency + "");
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInLocalCurrency == null)
                                        item.receivablesInLocalCurrency = 0;
                                    existsedItem.yField += parseFloat(item.receivablesInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 6:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.Day + "-" + item.Month;
                                    if (item.receivablesInProfitCurrency == null)
                                        item.receivablesInProfitCurrency = 0;
                                    existsedItem.yField = parseFloat(item.receivablesInProfitCurrency + "");
                                    existsedItem.Day = item.Day;
                                    existsedItem.Month = item.Month;
                                    existsedItem.Year = item.Year;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInProfitCurrency == null)
                                        item.receivablesInProfitCurrency = 0;
                                    existsedItem.yField += parseFloat(item.receivablesInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                }
            }
            // By Weeks
            if (parseInt(monthFilterIndex + "") == 0) {
                switch (parseInt(measurment + "")) {
                    case 0:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.total == null)
                                        item.total = 0;
                                    existsedItem.yField = item.total;
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.total == null)
                                        item.total = 0;
                                    existsedItem.yField += item.total;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 1:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.sumChargeableWeight == null)
                                        item.sumChargeableWeight = 0;
                                    existsedItem.yField = parseFloat(item.sumChargeableWeight + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumChargeableWeight == null)
                                        item.sumChargeableWeight = 0;
                                    existsedItem.yField += parseFloat(item.sumChargeableWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 2:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.sumGrossWeight == null)
                                        item.sumGrossWeight = 0;
                                    existsedItem.yField = parseFloat(item.sumGrossWeight + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumGrossWeight == null)
                                        item.sumGrossWeight = 0;
                                    existsedItem.yField += parseFloat(item.sumGrossWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 3:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.totalProfitInLocalCurrency == null)
                                        item.totalProfitInLocalCurrency = 0;
                                    existsedItem.yField = parseFloat(item.totalProfitInLocalCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInLocalCurrency == null)
                                        item.totalProfitInLocalCurrency = 0;
                                    existsedItem.yField += parseFloat(item.totalProfitInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 4:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.totalProfitInProfitCurrency == null)
                                        item.totalProfitInProfitCurrency = 0;
                                    existsedItem.yField = parseFloat(item.totalProfitInProfitCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInProfitCurrency == null)
                                        item.totalProfitInProfitCurrency = 0;
                                    existsedItem.yField += parseFloat(item.totalProfitInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 5:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.receivablesInLocalCurrency == null)
                                        item.receivablesInLocalCurrency = 0;
                                    existsedItem.yField = parseFloat(item.receivablesInLocalCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInLocalCurrency == null)
                                        item.receivablesInLocalCurrency = 0;
                                    existsedItem.yField += parseFloat(item.receivablesInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 6:
                        {
                            dataList = this.FillEmptyDates(monthFilterIndex, dataList);
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.XField == item.DateRange && f.startOfTheWeek.getDate() == item.StartOfTheWeek.getDate() && f.startOfTheWeek.getMonth() == item.StartOfTheWeek.getMonth() && f.startOfTheWeek.getFullYear() == item.StartOfTheWeek.getFullYear(); })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.XField = item.DateRange;
                                    existsedItem.Date = item.Date;
                                    if (item.receivablesInProfitCurrency == null)
                                        item.receivablesInProfitCurrency = 0;
                                    existsedItem.yField = parseFloat(item.receivablesInProfitCurrency + "");
                                    existsedItem.startOfTheWeek = item.StartOfTheWeek;
                                    existsedItem.dateRange = item.DateRange;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInProfitCurrency == null)
                                        item.receivablesInProfitCurrency = 0;
                                    existsedItem.yField += parseFloat(item.receivablesInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                }
            }
            // By Years
            else {
                switch (parseInt(measurment + "")) {
                    case 0:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.total == null)
                                        item.total = 0;
                                    existsedItem.yField = item.total;
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.total == null)
                                        item.total = 0;
                                    existsedItem.yField += item.total;
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 1:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.sumChargeableWeight == null)
                                        item.sumChargeableWeight = 0;
                                    existsedItem.yField = parseFloat(item.sumChargeableWeight + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumChargeableWeight == null)
                                        item.sumChargeableWeight = 0;
                                    existsedItem.yField += parseFloat(item.sumChargeableWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 2:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.sumGrossWeight == null)
                                        item.sumGrossWeight = 0;
                                    existsedItem.yField = parseFloat(item.sumGrossWeight + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.sumGrossWeight == null)
                                        item.sumGrossWeight = 0;
                                    existsedItem.yField += parseFloat(item.sumGrossWeight + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 3:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.totalProfitInLocalCurrency == null)
                                        item.totalProfitInLocalCurrency = 0;
                                    existsedItem.yField = parseFloat(item.totalProfitInLocalCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInLocalCurrency == null)
                                        item.totalProfitInLocalCurrency = 0;
                                    existsedItem.yField += parseFloat(item.totalProfitInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 4:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.totalProfitInProfitCurrency == null)
                                        item.totalProfitInProfitCurrency = 0;
                                    existsedItem.yField = parseFloat(item.totalProfitInProfitCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.totalProfitInProfitCurrency == null)
                                        item.totalProfitInProfitCurrency = 0;
                                    existsedItem.yField += parseFloat(item.totalProfitInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 5:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.receivablesInLocalCurrency == null)
                                        item.receivablesInLocalCurrency = 0;
                                    existsedItem.yField = parseFloat(item.receivablesInLocalCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInLocalCurrency == null)
                                        item.receivablesInLocalCurrency = 0;
                                    existsedItem.yField += parseFloat(item.receivablesInLocalCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                    case 6:
                        {
                            var groupedData = [];
                            dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                            dataList.getAll().forEach(function (item) {
                                var existsedItem = groupedData.filter(function (f) { return f.month == item.month && f.year == item.year; })[0];
                                if (existsedItem == null) {
                                    existsedItem = new GroupByClass_1.GroupByClass();
                                    existsedItem.Month = item.month;
                                    existsedItem.Year = item.year;
                                    existsedItem.XField = item.month + "-" + item.year;
                                    if (item.receivablesInProfitCurrency == null)
                                        item.receivablesInProfitCurrency = 0;
                                    existsedItem.yField = parseFloat(item.receivablesInProfitCurrency + "");
                                    groupedData.push(existsedItem);
                                }
                                else {
                                    if (item.receivablesInProfitCurrency == null)
                                        item.receivablesInProfitCurrency = 0;
                                    existsedItem.yField += parseFloat(item.receivablesInProfitCurrency + "");
                                }
                            });
                            resultList.Assign(groupedData);
                            break;
                        }
                }
            }
        }
        return resultList;
    };
    FunctionsCRM.getCustomersFilterdList = function (dataList, showSelectedIndex) {
        var resultList = new List_1.List();
        var measurment = showSelectedIndex;
        switch (parseInt(measurment + "")) {
            case 0:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.Total == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.Total;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.Total == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.Total;
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            case 1:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.SumChargeableWeight == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.SumChargeableWeight;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.SumChargeableWeight == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += parseFloat(item.SumChargeableWeight + "");
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            case 2:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.SumGrossWeight == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.SumGrossWeight;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.SumGrossWeight == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.SumGrossWeight;
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            case 3:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.TotalProfitInLocalCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.TotalProfitInLocalCurrency;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.TotalProfitInLocalCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.TotalProfitInLocalCurrency;
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            case 4:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.TotalProfitInProfitCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.TotalProfitInProfitCurrency;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.TotalProfitInProfitCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.TotalProfitInProfitCurrency;
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            case 5:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.ReceivablesInLocalCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.ReceivablesInLocalCurrency;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.ReceivablesInLocalCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.ReceivablesInLocalCurrency;
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            case 6:
                {
                    var groupedData = [];
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedData.filter(function (f) { return f.CustomerName == item.CustomerName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.ReceivablesInProfitCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CustomerName;
                            }
                            else {
                                existsedItem.YField = item.ReceivablesInProfitCurrency;
                                existsedItem.XField = item.CustomerName;
                            }
                            existsedItem.CustomerName = item.CustomerName;
                            groupedData.push(existsedItem);
                        }
                        else {
                            if (item.ReceivablesInProfitCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.ReceivablesInProfitCurrency;
                        }
                    });
                    resultList.Assign(groupedData);
                    break;
                }
            default: break;
        }
        return resultList;
    };
    FunctionsCRM.getCountriesFilterdList = function (dataList, ShowSelectedIndex, TopSelected, IncludeOthers) {
        var Rlist1 = new List_1.List();
        var RlistOthers = new List_1.List();
        var rlist = new List_1.List();
        switch (parseInt(ShowSelectedIndex + "")) {
            case 0:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.Total == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.Total;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.Total == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.Total;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.Total == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.Total;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.Total == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.Total;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                    break;
                }
            case 1:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.SumChargeableWeight == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.SumChargeableWeight;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.SumChargeableWeight == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.SumChargeableWeight;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.SumChargeableWeight == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.SumChargeableWeight;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.SumChargeableWeight == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.SumChargeableWeight;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                }
            case 2:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.SumGrossWeight == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.SumGrossWeight;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.SumGrossWeight == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.SumGrossWeight;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.SumGrossWeight == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.SumGrossWeight;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.SumGrossWeight == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.SumGrossWeight;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                }
            case 3:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.TotalProfitInLocalCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.TotalProfitInLocalCurrency;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.TotalProfitInLocalCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.TotalProfitInLocalCurrency;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.TotalProfitInLocalCurrency == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.TotalProfitInLocalCurrency;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.TotalProfitInLocalCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.TotalProfitInLocalCurrency;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                }
            case 4:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.TotalProfitInProfitCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.TotalProfitInProfitCurrency;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.TotalProfitInProfitCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.TotalProfitInProfitCurrency;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.TotalProfitInProfitCurrency == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.TotalProfitInProfitCurrency;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.TotalProfitInProfitCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.TotalProfitInProfitCurrency;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                }
            case 5:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.ReceivablesInLocalCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.ReceivablesInLocalCurrency;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.ReceivablesInLocalCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.ReceivablesInLocalCurrency;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.ReceivablesInLocalCurrency == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.ReceivablesInLocalCurrency;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.ReceivablesInLocalCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.ReceivablesInLocalCurrency;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                }
            case 6:
                {
                    var list = dataList.getAll().filter(function (f) { return f.CountryName != "Others"; });
                    var FilterdList = new List_1.List();
                    FilterdList.Assign(list);
                    FilterdList.getAll().forEach(function (item) {
                        var existsedItem = Rlist1.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            if (item.ReceivablesInProfitCurrency == null) {
                                existsedItem.YField = 0;
                                existsedItem.XField = item.CountryName;
                            }
                            else {
                                existsedItem.YField = item.ReceivablesInProfitCurrency;
                                existsedItem.XField = item.CountryName;
                            }
                            existsedItem.CountryName = item.CountryName;
                            Rlist1.add(existsedItem);
                        }
                        else {
                            if (item.ReceivablesInProfitCurrency == null)
                                existsedItem.YField += 0;
                            else
                                existsedItem.YField += item.ReceivablesInProfitCurrency;
                        }
                    });
                    Rlist1.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                    rlist.Assign(Rlist1.getAll());
                    if (IncludeOthers) {
                        list = dataList.getAll().filter(function (f) { return f.CountryName == "Others"; });
                        FilterdList.Assign(list);
                        FilterdList.getAll().forEach(function (item) {
                            var existsedItem = RlistOthers.getAll().filter(function (f) { return f.CountryName == item.CountryName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.ReceivablesInProfitCurrency == null) {
                                    existsedItem.YField = 0;
                                    existsedItem.XField = item.CountryName;
                                }
                                else {
                                    existsedItem.YField = item.ReceivablesInProfitCurrency;
                                    existsedItem.XField = item.CountryName;
                                }
                                existsedItem.CountryName = item.CountryName;
                                RlistOthers.add(existsedItem);
                            }
                            else {
                                if (item.ReceivablesInProfitCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.ReceivablesInProfitCurrency;
                            }
                        });
                        RlistOthers.getAll().sort(function (a, b) { return (a.YField === b.YField) ? 0 : (a.YField > b.YField) ? -1 : 1; });
                        rlist.Assign(Rlist1.getAll().concat(RlistOthers.getAll()));
                    }
                }
            default: break;
        }
        return rlist;
    };
    FunctionsCRM.getMeasurmentFilterListForDirectionAndTransmode = function (measurment, dataList, monthFilterIndex, customerid) {
        var SeconGroup = [];
        var resultList = new List_1.List();
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        // if the selected month filter was last month.
        if (monthFilterIndex == 0 && (customerid == "" || customerid == null)) {
            switch (parseInt(measurment + "")) {
                case 0:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.total == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = item.total;
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.total == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.total;
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 1:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.sumChargeableWeight == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.sumChargeableWeight + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.sumChargeableWeight == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.sumChargeableWeight + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 2:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.sumGrossWeight == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.sumGrossWeight + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.sumChargeableWeight == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.sumGrossWeight + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 3:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.totalProfitInLocalCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.totalProfitInLocalCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.totalProfitInLocalCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.totalProfitInLocalCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 4:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.totalProfitInProfitCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.totalProfitInProfitCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.totalProfitInProfitCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.totalProfitInProfitCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 5:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.ReceivablesInLocalCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.ReceivablesInLocalCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.ReceivablesInLocalCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.ReceivablesInLocalCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 6:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.ReceivablesInProfitCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.ReceivablesInProfitCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.ReceivablesInProfitCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.ReceivablesInProfitCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                default: break;
            }
        }
        //not last month. or customerid is not null
        else {
            switch (parseInt(measurment + "")) {
                case 0:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.total == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = item.total;
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.total == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += item.total;
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 1:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.sumChargeableWeight == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.sumChargeableWeight + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.sumChargeableWeight == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.sumChargeableWeight + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 2:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.sumGrossWeight == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.sumGrossWeight + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.sumGrossWeight == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.sumGrossWeight + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 3:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.totalProfitInLocalCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.totalProfitInLocalCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.totalProfitInLocalCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.totalProfitInLocalCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 4:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.totalProfitInProfitCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.totalProfitInProfitCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.totalProfitInProfitCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.totalProfitInProfitCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 5:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.ReceivablesInLocalCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.ReceivablesInLocalCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.ReceivablesInLocalCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.ReceivablesInLocalCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                case 6:
                    {
                        var groupedData = [];
                        dataList.getAll().forEach(function (item) {
                            var existsedItem = groupedData.filter(function (f) { return f.transportModeID == item.transportModeID && f.directionID == item.directionID && f.directionName == item.directionName && f.transportModeName == item.transportModeName; })[0];
                            if (existsedItem == null) {
                                existsedItem = new GroupByClass_1.GroupByClass();
                                if (item.ReceivablesInProfitCurrency == null)
                                    existsedItem.YField = 0;
                                else
                                    existsedItem.YField = parseFloat(item.ReceivablesInProfitCurrency + "");
                                existsedItem.XField = item.DirectionName + "/" + item.TransportModeName;
                                existsedItem.transportModeID = item.transportModeID;
                                existsedItem.directionID = item.directionID;
                                existsedItem.transportModeName = item.transportModeName;
                                existsedItem.directionName = item.directionName;
                                groupedData.push(existsedItem);
                            }
                            else {
                                if (item.ReceivablesInProfitCurrency == null)
                                    existsedItem.YField += 0;
                                else
                                    existsedItem.YField += parseFloat(item.ReceivablesInProfitCurrency + "");
                            }
                        });
                        resultList.Assign(groupedData);
                        break;
                    }
                default: break;
            }
        }
        return resultList;
    };
    FunctionsCRM.getmonthQuartersList = function (selected, dataList, customerid) {
        var groupedbyList = new List_1.List();
        if (customerid == "" || customerid == null) {
            switch (parseInt(selected + "")) {
                case 1:
                case 2:
                    {
                        groupedbyList = dataList;
                        var newList = new List_1.List();
                        newList = groupedbyList;
                        newList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                        groupedbyList = newList;
                        break;
                    }
                case 3:
                    {
                        groupedbyList = dataList;
                        if (groupedbyList.size() < 12) {
                            groupedbyList = this.getFilledListByMonths(groupedbyList, -11);
                        }
                        var newList = new List_1.List();
                        newList = groupedbyList;
                        newList.getAll().sort(function (a, b) { return ((a.year === b.year) ? ((a.month === b.month) ? 0 : (a.month < b.month) ? -1 : 1) : (a.year < b.year ? -1 : 1)); });
                        groupedbyList = newList;
                        break;
                    }
                case 0:
                    {
                        groupedbyList = dataList;
                        if (groupedbyList.size() < 7) {
                            groupedbyList = this.getFilledListByMonths(groupedbyList, -6);
                        }
                        var newList = new List_1.List();
                        newList = groupedbyList;
                        newList.getAll().sort(function (a, b) { return ((a.year === b.year) ? ((a.month === b.month) ? ((a.day === b.day) ? 0 : (a.day < b.day) ? -1 : 1) : (a.month < b.month) ? -1 : 1) : (a.year < b.year) ? -1 : 1); });
                        groupedbyList = newList;
                        break;
                    }
                default: break;
            }
        }
        else {
            switch (parseInt(selected + "")) {
                case 0:
                    {
                        groupedbyList = dataList;
                        var newList = new List_1.List();
                        newList = groupedbyList;
                        newList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
                        groupedbyList = newList;
                        break;
                    }
                case 1: {
                    groupedbyList = dataList;
                    if (groupedbyList.size() < 12) {
                        groupedbyList = this.getFilledListByMonths(groupedbyList, -11);
                    }
                    var newList = new List_1.List();
                    newList = groupedbyList;
                    newList.getAll().sort(function (a, b) { return ((a.year === b.year) ? ((a.month === b.month) ? 0 : (a.month < b.month) ? -1 : 1) : (a.year < b.year ? -1 : 1)); });
                    groupedbyList = newList;
                    break;
                }
                case 2: {
                    dataList.getAll().forEach(function (item) {
                        var existsedItem = groupedbyList.getAll().filter(function (f) { return f.year == item.year; })[0];
                        if (existsedItem == null) {
                            existsedItem = new GroupByClass_1.GroupByClass();
                            existsedItem.XField = item.Year + "";
                            if (item.yField == null)
                                existsedItem.yField = 0;
                            else
                                existsedItem.yField = item.yField;
                            existsedItem.Year = item.Year;
                            groupedbyList.add(existsedItem);
                        }
                        else {
                            if (item.yField != null)
                                existsedItem.yField += item.yField;
                        }
                    });
                    if (groupedbyList.size() < 3) {
                        var currentYear = new Date().getFullYear();
                        var startYear = currentYear - 2;
                        for (var i = startYear; i <= currentYear; i++) {
                            var exists = false;
                            groupedbyList.getAll().forEach(function (g) {
                                if (i + "" == g.XField)
                                    exists = true;
                            });
                            if (exists == false) {
                                var newEntry = new GroupByClass_1.GroupByClass();
                                newEntry.XField = i + "";
                                newEntry.yField = 0;
                                groupedbyList.add(newEntry);
                                groupedbyList.getAll().sort(function (a, b) { return (a.XField === b.XField) ? 0 : (a.XField < b.XField) ? -1 : 1; });
                            }
                        }
                    }
                    break;
                }
            }
        }
        return groupedbyList;
    };
    FunctionsCRM.getFilledListByMonths = function (list, days) {
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var startDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        startDate.setDate(startDate.getDate() + days);
        if (days == -1) {
            while (startDate <= todayDate) {
                var monthShipments = list.getAll().filter(function (f) { return f.month == startDate.getMonth() + 1 && f.year == startDate.getFullYear() && f.day == startDate.getDate(); });
                if (monthShipments.length == 0) {
                    var newEntry = new GroupByClass_1.GroupByClass();
                    newEntry.Day = startDate.getDate();
                    newEntry.Year = startDate.getFullYear();
                    newEntry.Month = startDate.getMonth();
                    newEntry.XField = startDate.getDate().toString() + "-" + startDate.getMonth().toString() + "-" + startDate.getFullYear().toString();
                    newEntry.YField = 0;
                    list.add(newEntry);
                }
                startDate.setDate(startDate.getDate() + 1);
            }
        }
        else if (days == -11) {
            startDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            startDate.setMonth(todayDate.getMonth() + days);
            todayDate.setMonth(todayDate.getMonth() - 1 + 1);
            while (startDate <= todayDate) {
                var monthShipments = list.getAll().filter(function (p) { return p.month == startDate.getMonth() + 1 && p.year == startDate.getFullYear(); });
                if (monthShipments.length == 0) {
                    var newEntry = new GroupByClass_1.GroupByClass();
                    newEntry.Day = startDate.getDate();
                    newEntry.Year = startDate.getFullYear();
                    // if (startDate.getMonth()==0)
                    newEntry.Month = startDate.getMonth() + 1;
                    //  else
                    //     newEntry.Month = startDate.getMonth();
                    newEntry.XField = newEntry.Month.toString() + "-" + startDate.getFullYear();
                    newEntry.YField = 0;
                    list.add(newEntry);
                }
                startDate.setMonth(startDate.getMonth() + 1);
            }
        }
        else {
            var i = 0;
            while (startDate <= todayDate) {
                var monthShipments = list.getAll().filter(function (p) { return p.month == startDate.getMonth() + 1 && p.year == startDate.getFullYear() && p.day == startDate.getDate(); });
                if (monthShipments.length == 0) {
                    var newEntry = new GroupByClass_1.GroupByClass();
                    newEntry.day = startDate.getDate();
                    newEntry.year = startDate.getFullYear();
                    newEntry.month = startDate.getMonth() + 1;
                    newEntry.XField = startDate.getDate().toString() + "-" + (startDate.getMonth() + 1);
                    newEntry.YField = 0;
                    list.add(newEntry);
                }
                startDate.setDate(startDate.getDate() + 1);
            }
        }
        return list;
    };
    FunctionsCRM.FillEmptyDates = function (monthFilterIndex, dataList) {
        var i = 0;
        dataList.getAll().forEach(function (p) {
            var item = dataList.get(i);
            item.StartOfTheWeek = new Date(dataList.get(i).StartOfTheWeek + "");
            item.EndOfTheWeek = new Date(dataList.get(i).EndOfTheWeek + "");
            item.Date = new Date(dataList.get(i).Date + "");
            dataList.set(i, item);
            i++;
        });
        dataList.getAll().sort(function (a, b) { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1; });
        //var lastDate = DateTool.GetStartOfTheWeek(DateTool.GetCurrentDateAsUtc());
        //var firstDate = new Date(lastDate.toString());
        //firstDate.setMonth(firstDate.getMonth() + -3);
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var startDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        startDate.setMonth(todayDate.getMonth() - 1);
        if (monthFilterIndex == 2) {
            startDate.setMonth(todayDate.getMonth() - 3);
        }
        while (startDate <= todayDate) {
            var monthShipments = dataList.getAll().filter(function (s) { return s.month == startDate.getMonth() + 1 && s.year == startDate.getFullYear() && s.day == startDate.getDate(); });
            if (monthShipments.length == 0) {
                var newEntry = new DashBoardClass_1.DashBoardClass();
                newEntry.day = startDate.getDate();
                newEntry.month = startDate.getMonth() + 1;
                newEntry.year = startDate.getFullYear();
                newEntry.total = 0;
                newEntry.sumChargeableWeight = 0;
                newEntry.sumGrossWeight = 0;
                newEntry.Date = new Date(newEntry.year, newEntry.month - 1, newEntry.day);
                var day = newEntry.Date.getDay();
                var startOfWeek = new Date(newEntry.year, newEntry.month - 1, newEntry.day);
                startOfWeek.setDate(startOfWeek.getDate() + (-1 * day));
                var endOfWeek = new Date(newEntry.year, newEntry.month - 1, newEntry.day);
                endOfWeek.setDate(endOfWeek.getDate() + (6 - day));
                newEntry.startOfTheWeek = startOfWeek;
                newEntry.endOfTheWeek = endOfWeek;
                newEntry.dateRange = startOfWeek.getDate() + "/" + (startOfWeek.getMonth() + 1);
                dataList.add(newEntry);
            }
            startDate.setDate(startDate.getDate() + 1);
        }
        return dataList;
    };
    FunctionsCRM.ManageChartLabels = function (list) {
        if (list != null) {
            if (list.size() > 0) {
                var groupedData = [];
                list.getAll().forEach(function (item) {
                    var existsedItem = groupedData.filter(function (f) { return f.StringProperty == item.StringProperty; })[0];
                    if (existsedItem == null) {
                        existsedItem = new ChartingDataClass_1.ChartingDataClass();
                        existsedItem.StringProperty = item.StringProperty;
                        groupedData.push(existsedItem);
                    }
                });
                var groupedData2 = [];
                groupedData.forEach(function (item) {
                    var existsedItem = groupedData2.filter(function (f) { return f.StringProperty == item.StringProperty && f.OwnerId == item.OwnerId; })[0];
                    if (existsedItem == null) {
                        existsedItem = new ChartingDataClass_1.ChartingDataClass();
                        existsedItem.StringProperty = item.StringProperty;
                        existsedItem.OwnerId = item.OwnerId;
                        groupedData2.push(existsedItem);
                    }
                });
                //foreach(var item1 in group1.Where(d => d.Count > 1))
                //{
                //    var group2 = (from d in list
                //    where d.StringProperty == item1.StringProperty
                //    group d by new { d.OwnerId } into g
                //    select new
                //        {
                //            OwnerId = g.Key.OwnerId,
                //        });
                //    if (group2.Count() > 1) {
                //        int index = 1;
                //        foreach(var item2 in group2)
                //        {
                //            if (index > 1) {
                //                foreach(ChartingDataClass dataItem in list.Where(d => d.OwnerId == item2.OwnerId))
                //                {
                //                    dataItem.StringProperty += " [" + index + "]";
                //                }
                //            }
                //            index++;
                //        }
                //    }
                //}
            }
        }
        return list;
    };
    return FunctionsCRM;
}());
exports.FunctionsCRM = FunctionsCRM;
//# sourceMappingURL=FunctionsCRM.js.map