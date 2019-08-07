"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
require("rxjs/add/operator/map");
var EditGridVirtualRowController = /** @class */ (function () {
    function EditGridVirtualRowController() {
        this.pageSize = 15; // = 15;
        this.requestedRowsReady = new core_1.EventEmitter();
        this.requestedRowCount = new core_1.EventEmitter();
        this.firstRow = 0;
        this.firstTime = true;
        this.sortingDir = "Descending";
        this.sortingCol = "CreateDateTime";
        this.oldSearchFields = null;
        this.cacheBuffer = [];
        this.cachedData = {};
        this.rowsRequested = [];
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    }
    EditGridVirtualRowController.prototype.ngOnInit = function () {
        //this.pageSize = this.dataSource.pageSize;
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    };
    EditGridVirtualRowController.prototype.ngOnChanges = function () {
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    };
    EditGridVirtualRowController.prototype.calculatePageIndex = function () {
    };
    EditGridVirtualRowController.prototype.getRow = function (rowIndex, sortingCol, sortingDir, getCount, searchfields, returnCached, Filters, reload) {
        var _this = this;
        if (reload === void 0) { reload = false; }
        if (searchfields !== undefined) {
            if (this.oldSearchFields != searchfields) {
                this.oldSearchFields = searchfields;
                this.rowsRequested = [];
                this.cachedData = {};
            }
        }
        if (this.sortingCol != sortingCol || this.sortingDir != sortingDir) {
            this.sortingCol = sortingCol;
            this.sortingDir = sortingDir;
            this.cachedData = {};
            //this.firstTime = true;
            //this.requestedRowsReady = new EventEmitter();
            this.rowsRequested = [];
        }
        if (reload) {
            this.cachedData = {};
            this.rowsRequested = [];
        }
        var rowInfo = { pageIndex: 0, rowIndex: rowIndex };
        var pageIndex = Math.floor(rowIndex / this.pageSize);
        ////console.log("pageIndex", pageIndex);
        ////console.log(this.dataSource.getRows(0, 20))
        var exists = this.cachedData[rowIndex];
        ////console.log(exists);
        if (!exists) {
            var rowsRequestedPage = this.rowsRequested.filter(function (x) { return x.pageIndex == pageIndex; })[0];
            if (!rowsRequestedPage) {
                this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                var result = [];
                this.dataSource.getRows(pageIndex * this.pageSize, this.pageSize, sortingCol, sortingDir, getCount, searchfields, Filters).then(function (res) {
                    res.subscribe(function (viewResponse) {
                        ////console.log("getRows from server pageIndex", pageIndex);
                        //console.log(viewResponse);
                        var jsonlist = viewResponse.Data; //.json();
                        //this.getCount(viewResponse.length);
                        //this.cachedData[0] = jsonlist;
                        for (var i = 0; i < jsonlist.length; i++) {
                            _this.cachedData[i + (pageIndex * _this.pageSize)] = jsonlist[i];
                        }
                        ////var rowInfo: IRow = { pageIndex: 0, rowIndex: rowIndex };
                        //this.rowsRequested[this.pageIndex] = jsonlist;
                        ////console.log("rowIndex", rowIndex);
                        //this.requestedPageReady.emit('data');
                        var requestedRows = _this.rowsRequested.filter(function (x) { return x.pageIndex == pageIndex; });
                        requestedRows.forEach(function (value, index) {
                            if (jsonlist[index] == undefined) {
                                //console.log(index);
                                //console.log(requestedRows);
                                //console.log(jsonlist);
                                //console.log(jsonlist[index]);
                            }
                            result.push({ RowIndex: value.rowIndex, RowData: jsonlist[index] });
                        });
                        if (_this.cacheBuffer.length > 0) {
                            //console.log("emit cached data in server call=-=-=-=-=-=-", this.cacheBuffer);
                            _this.cacheBuffer.forEach(function (cacheValue, index) {
                                result.push({ RowIndex: cacheValue.RowIndex, RowData: cacheValue.RowData });
                            });
                            _this.cacheBuffer = [];
                        }
                        //this.rowsRequested = [];
                        if (!getCount) {
                            //for (var i = 0; i < result.length; i++) {
                            //    console.log("emit from server ------------>", result[i].RowIndex);
                            //}
                            _this.requestedRowsReady.emit(result);
                        }
                        else {
                            // console.log("emit the count------------------------------------88888");
                            _this.requestedRowCount.emit(viewResponse.Count);
                        }
                        //return result;
                    });
                });
                //Rx.Observable.defer(() => {
                //});
            }
            else {
                if (!this.rowsRequested.filter(function (e) { return e.pageIndex == pageIndex && e.rowIndex == rowIndex; })[0]) {
                    this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                }
                //return Rx.Observable.fromArray(() => {
                //    return ["In Progress"];
                //});
            }
        }
        else {
            // return Rx.Observable.fromArray(this.cachedData).filter(x => x == this.cachedData[rowIndex]);
            var cacheResult = { RowIndex: rowIndex, RowData: this.cachedData[rowIndex] };
            var resExists = this.cacheBuffer.filter(function (d) { return d.RowIndex === cacheResult.RowIndex; })[0];
            if (!resExists) {
                this.cacheBuffer.push(cacheResult);
            }
            if (returnCached) {
                //for (var i = 0; i < this.cacheBuffer.length; i++) {
                //    console.log("emit from cache ------------>", this.cacheBuffer[i].RowIndex);
                //}
                this.requestedRowsReady.emit(this.cacheBuffer);
                this.cacheBuffer = [];
            }
        }
    };
    EditGridVirtualRowController.prototype.getCount = function (rowcount) {
        //return Rx.Observable.defer(() => { return this.dataSource.getRows(0, 20); });
        this.requestedRowCount.emit(rowcount);
    };
    EditGridVirtualRowController.prototype.setDataSource = function (dataSource) {
        this.dataSource = dataSource;
    };
    EditGridVirtualRowController.prototype.Clear = function () {
        this.cachedData = {};
        this.cacheBuffer = [];
        this.rowsRequested = [];
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditGridVirtualRowController.prototype, "requestedRowsReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditGridVirtualRowController.prototype, "requestedRowCount", void 0);
    return EditGridVirtualRowController;
}());
exports.EditGridVirtualRowController = EditGridVirtualRowController;
//# sourceMappingURL=EditGridVirtualRowController.js.map