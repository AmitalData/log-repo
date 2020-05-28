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
var VirtualRowController = /** @class */ (function () {
    function VirtualRowController() {
        this.pageSize = 30; // = 15;
        this.requestedRowsReady = new core_1.EventEmitter();
        this.requestedRowCount = new core_1.EventEmitter();
        this.firstRow = 0;
        this.firstTime = true;
        this.sortingDir = "Descending";
        this.sortingCol = "CreateDateTime";
        this.oldSearchFields = null;
        this.cacheBuffer = [];
        this.RecievedDataCount = 0;
        this.cachedData = {};
        this.rowsRequested = [];
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    }
    VirtualRowController.prototype.ngOnInit = function () {
        //this.pageSize = this.dataSource.pageSize;
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    };
    VirtualRowController.prototype.ngOnChanges = function () {
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    };
    VirtualRowController.prototype.calculatePageIndex = function () {
    };
    VirtualRowController.prototype.getRow = function (rowIndex, sortingCol, sortingDir, getCount, searchfields, returnCached, Filters, reload, IgnorerowsRequestedPage, PSize, SearchFieldChanged) {
        var _this = this;
        if (reload === void 0) { reload = false; }
        if (IgnorerowsRequestedPage === void 0) { IgnorerowsRequestedPage = false; }
        if (PSize === void 0) { PSize = this.pageSize; }
        if (SearchFieldChanged === void 0) { SearchFieldChanged = false; }
        this.ReloadData = reload;
        if (searchfields != undefined && searchfields != null) {
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
            this.rowsRequested = [];
        }
        var rowInfo = { pageIndex: 0, rowIndex: rowIndex };
        var pageIndex = Math.floor(rowIndex / PSize);
        var exists = this.cachedData[rowIndex];
        if (!exists) {
            var rowsRequestedPage = this.rowsRequested.filter(function (x) { return x.pageIndex == pageIndex; })[0];
            if (!rowsRequestedPage || IgnorerowsRequestedPage == true) {
                this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                var result = [];
                console.log("this.MyCallTime before" + this.MyCallTime);
                this.dataSource.getRows(pageIndex * PSize, PSize, sortingCol, sortingDir, getCount, searchfields, Filters).then(function (res) {
                    res.subscribe(function (viewResponse) {
                        if (!viewResponse.HasError) {
                            if (_this.MyCallTime == null || viewResponse.CallTime > _this.MyCallTime || SearchFieldChanged == false) {
                                _this.MyCallTime = viewResponse.CallTime;
                                console.log("this.MyCallTime " + _this.MyCallTime);
                                _this.RecievedDataCount = viewResponse.Result.length;
                                var jsonlist = viewResponse.Result;
                                for (var i = 0; i < jsonlist.length; i++) {
                                    _this.cachedData[i + (pageIndex * PSize)] = jsonlist[i];
                                }
                                var requestedRows = _this.rowsRequested.filter(function (x) { return x.pageIndex == pageIndex; });
                                requestedRows.forEach(function (value, index) {
                                    if (jsonlist[index] != undefined) {
                                    }
                                    result.push({ RowIndex: value.rowIndex, RowData: jsonlist[index] });
                                });
                                if (_this.cacheBuffer.length > 0) {
                                    _this.cacheBuffer.forEach(function (cacheValue, index) {
                                        result.push({ RowIndex: cacheValue.RowIndex, RowData: cacheValue.RowData });
                                    });
                                    _this.cacheBuffer = [];
                                }
                                if (!getCount) {
                                    _this.requestedRowsReady.emit(result);
                                }
                                else {
                                    _this.requestedRowCount.emit(viewResponse.Count);
                                }
                            }
                        }
                    });
                });
            }
            else {
                if (!this.rowsRequested.filter(function (e) { return e.pageIndex == pageIndex && e.rowIndex == rowIndex; })[0]) {
                    this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                }
            }
        }
        else {
            var cacheResult = { RowIndex: rowIndex, RowData: this.cachedData[rowIndex] };
            var resExists = this.cacheBuffer.filter(function (d) { return d.RowIndex === cacheResult.RowIndex; })[0];
            if (!resExists) {
                this.cacheBuffer.push(cacheResult);
            }
            if (returnCached) {
                this.requestedRowsReady.emit(this.cacheBuffer);
                this.cacheBuffer = [];
            }
        }
    };
    VirtualRowController.prototype.getCount = function (rowcount) {
        this.requestedRowCount.emit(rowcount);
    };
    VirtualRowController.prototype.setDataSource = function (dataSource) {
        this.dataSource = dataSource;
        this.pageSize = dataSource.pageSize;
    };
    VirtualRowController.prototype.Clear = function () {
        this.cachedData = {};
        this.cacheBuffer = [];
        this.rowsRequested = [];
    };
    VirtualRowController.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    VirtualRowController.prototype.ClearCache = function () {
        this.cachedData = {};
        this.rowsRequested = [];
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], VirtualRowController.prototype, "requestedRowsReady", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], VirtualRowController.prototype, "requestedRowCount", void 0);
    return VirtualRowController;
}());
exports.VirtualRowController = VirtualRowController;
//# sourceMappingURL=VirtualRowController.js.map