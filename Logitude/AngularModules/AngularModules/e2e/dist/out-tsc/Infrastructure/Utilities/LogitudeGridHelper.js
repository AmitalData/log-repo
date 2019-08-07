"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LogitudeGridHelper = /** @class */ (function () {
    function LogitudeGridHelper(sessionIndex) {
        this.LogGridIndexId = null;
        this.LogGridColumnsIndexId = null;
        this.LogGridRowsIndexId = null;
        this.EditableLogGridIndexId = null;
        this.EditableLogGridColumnsIndexId = null;
        this.EditableLogGridRowsIndexId = null;
        this.ColumnId = null;
        this.EditCellIndex = null;
        this.ColumnsCount = null;
        this.RowIndex = null;
        this.DetailsDivIndex = null;
        this.GridRowIndex = null;
        this.GridColumnsCount = [];
        this.SessionIndex = sessionIndex;
    }
    LogitudeGridHelper.prototype.GetGridRowIndex = function (reset) {
        if (reset === void 0) { reset = false; }
        if (reset == false) {
            if (this.GridRowIndex == null) {
                this.GridRowIndex = 0;
            }
            else {
                this.GridRowIndex += 1;
            }
        }
        else {
            this.GridRowIndex = null;
        }
        return this.GridRowIndex;
    };
    LogitudeGridHelper.prototype.GetEditCellIndex = function () {
        if (this.EditCellIndex == null) {
            this.EditCellIndex = 0;
        }
        else {
            this.EditCellIndex += 1;
        }
        return this.EditCellIndex;
    };
    LogitudeGridHelper.prototype.SetRowIndex = function (GridId) {
        if (this.GridsRowIndex == null) {
            this.GridsRowIndex = [];
        }
        if (this.GridsRowIndex.filter(function (a) { return a.Id == GridId; }).length == 0) {
            this.GridsRowIndex.push({ Index: 0, Id: GridId });
        }
        else {
            this.GridsRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index += 1;
        }
        //if (this.RowIndex == null) {
        //    this.RowIndex = 0;
        //}
        //else {
        //    this.RowIndex += 1;
        //} 
        if (this.GridsRowIndex.filter(function (a) { return a.Id == GridId; }).length > 0) {
            return this.GridsRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index;
        }
        //return this.RowIndex;
    };
    LogitudeGridHelper.prototype.GetRowIndex = function (GridId) {
        //return this.RowIndex;
        if (this.GridsRowIndex.filter(function (a) { return a.Id == GridId; }).length > 0) {
            return this.GridsRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index;
        }
        else {
            this.GridsRowIndex.push({ Index: 0, Id: GridId });
            return 0;
        }
    };
    LogitudeGridHelper.prototype.ResetRowIndex = function (GridId) {
        if (this.GridsRowIndex == null) {
            this.GridsRowIndex = [];
        }
        if (this.GridsRowIndex.filter(function (a) { return a.Id == GridId; }).length > 0) {
            this.GridsRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index = -1;
        }
        //this.RowIndex = 0;
    };
    LogitudeGridHelper.prototype.ResetNextRowIndex = function (GridId) {
        if (this.GridsNextRowIndex == null) {
            this.GridsNextRowIndex = [];
        }
        if (this.GridsNextRowIndex.filter(function (a) { return a.Id == GridId; }).length > 0) {
            this.GridsNextRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index = 0;
        }
        //this.RowIndex = 0;
    };
    LogitudeGridHelper.prototype.SetNextRowIndex = function (GridId) {
        if (this.GridsNextRowIndex == null) {
            this.GridsNextRowIndex = [];
        }
        if (this.GridsNextRowIndex.filter(function (a) { return a.Id == GridId; }).length == 0) {
            this.GridsNextRowIndex.push({ Index: 0, Id: GridId });
        }
        else {
            this.GridsNextRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index += 1;
        }
        //if (this.GridsNextRowIndex.filter(a => a.Id == GridId).length > 0) {
        //    return this.GridsNextRowIndex.filter(a => a.Id == GridId)[0].Index;
        //}
        //return this.RowIndex;
    };
    LogitudeGridHelper.prototype.GetNextRowIndex = function (GridId) {
        //return this.RowIndex;
        if (this.GridsNextRowIndex == null) {
            this.GridsNextRowIndex = [];
            this.GridsNextRowIndex.push({ Index: 0, Id: GridId });
            return 0;
        }
        if (this.GridsNextRowIndex.filter(function (a) { return a.Id == GridId; }).length > 0) {
            return this.GridsNextRowIndex.filter(function (a) { return a.Id == GridId; })[0].Index;
        }
        else {
            this.GridsNextRowIndex.push({ Index: 0, Id: GridId });
            return 0;
        }
    };
    LogitudeGridHelper.prototype.ResetEditCellIndex = function () {
        this.EditCellIndex = null;
    };
    LogitudeGridHelper.prototype.SetColumnsCount = function (reset, GridId) {
        if (this.GridColumnsCount == null) {
            this.GridColumnsCount = [];
        }
        if (reset == true) {
            if (this.GridColumnsCount.filter(function (a) { return a.Id == GridId; }).length == 0) {
                this.GridColumnsCount.push({ Count: 0, Id: GridId });
            }
            else {
                this.GridColumnsCount.filter(function (a) { return a.Id == GridId; })[0].Count = 0;
            }
            //this.ColumnsCount = 0;
        }
        else {
            if (this.GridColumnsCount.filter(function (a) { return a.Id == GridId; }).length == 0) {
                this.GridColumnsCount.push({ Count: 0, Id: GridId });
            }
            else {
                this.GridColumnsCount.filter(function (a) { return a.Id == GridId; })[0].Count += 1;
            }
            //if (this.ColumnsCount == null) {
            //    this.ColumnsCount = 0;
            //}
            //else {
            //    this.ColumnsCount += 1;
            //}
        }
        //return this.ColumnsCount;
    };
    LogitudeGridHelper.prototype.getColumnsCount = function (GridId) {
        if (this.GridColumnsCount.filter(function (a) { return a.Id == GridId; }).length > 0) {
            return this.GridColumnsCount.filter(function (a) { return a.Id == GridId; })[0].Count;
        }
        //return this.ColumnsCount;
    };
    LogitudeGridHelper.prototype.GetLogGridIndexId = function () {
        if (this.LogGridIndexId == null) {
            this.LogGridIndexId = 0;
        }
        else {
            this.LogGridIndexId += 1;
        }
        return this.SessionIndex + "_" + this.LogGridIndexId;
    };
    LogitudeGridHelper.prototype.GetLogGridColumnsIndexId = function () {
        if (this.LogGridColumnsIndexId == null) {
            this.LogGridColumnsIndexId = 0;
        }
        else {
            this.LogGridColumnsIndexId += 1;
        }
        return this.SessionIndex + "_" + this.LogGridColumnsIndexId;
    };
    LogitudeGridHelper.prototype.GetDetailsDivId = function () {
        if (this.DetailsDivIndex == null) {
            this.DetailsDivIndex = 0;
        }
        else {
            this.DetailsDivIndex += 1;
        }
        return this.SessionIndex + "_" + this.DetailsDivIndex;
    };
    LogitudeGridHelper.prototype.GetLogGridRowsIndexId = function () {
        if (this.LogGridRowsIndexId == null) {
            this.LogGridRowsIndexId = 0;
        }
        else {
            this.LogGridRowsIndexId += 1;
        }
        return this.SessionIndex + "_" + this.LogGridRowsIndexId;
    };
    LogitudeGridHelper.prototype.GetEditableLogGridIndexId = function () {
        if (this.EditableLogGridIndexId == null) {
            this.EditableLogGridIndexId = 0;
        }
        else {
            this.EditableLogGridIndexId += 1;
        }
        return this.SessionIndex + "_" + this.EditableLogGridIndexId;
    };
    LogitudeGridHelper.prototype.GetEditableLogGridColumnsIndexId = function () {
        if (this.EditableLogGridColumnsIndexId == null) {
            this.EditableLogGridColumnsIndexId = 0;
        }
        else {
            this.EditableLogGridColumnsIndexId += 1;
        }
        return this.SessionIndex + "_" + this.EditableLogGridColumnsIndexId;
    };
    LogitudeGridHelper.prototype.GetEditableLogGridRowsIndexId = function () {
        if (this.EditableLogGridRowsIndexId == null) {
            this.EditableLogGridRowsIndexId = 0;
        }
        else {
            this.EditableLogGridRowsIndexId += 1;
        }
        return this.SessionIndex + "_" + this.EditableLogGridRowsIndexId;
    };
    LogitudeGridHelper.prototype.GetColumnId = function () {
        if (this.ColumnId == null) {
            this.ColumnId = 0;
        }
        else {
            this.ColumnId += 1;
        }
        //if (this.CurrentWindow) {
        //    return this.SessionIndex + "_" + this.ColumnId + this.CurrentWindow.WindowIndex;
        //}
        //else {
        return this.SessionIndex + "_" + this.ColumnId;
        //}
    };
    return LogitudeGridHelper;
}());
exports.LogitudeGridHelper = LogitudeGridHelper;
//# sourceMappingURL=LogitudeGridHelper.js.map