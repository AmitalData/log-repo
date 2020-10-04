import {EventEmitter, Output, OnInit, OnChanges, Component} from '@angular/core';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

interface IRow {
    pageIndex: number;
    rowIndex: number;
}

export class VirtualRowController implements OnInit, OnChanges {
    dataSource: any;
    public cachedData: { [key: string]: any };
    private pageSize: number = 30; // = 15;
    private rowsRequested: IRow[];
    @Output() requestedRowsReady = new EventEmitter();
    @Output() requestedRowCount = new EventEmitter();
    firstRow: number = 0;


    constructor() {

        this.cachedData = {};
        this.rowsRequested = [];
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }

    }

    ngOnInit() {
        //this.pageSize = this.dataSource.pageSize;
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    }

    ngOnChanges() {
        if (this.dataSource) {
            this.pageSize = this.dataSource.pageSize;
            this.sortingDir = this.dataSource.sortingDir;
            this.sortingCol = this.dataSource.sortingCol;
        }
    }

    firstTime: boolean = true;
    calculatePageIndex() {

    }
    sortingDir: string = "Descending";
    sortingCol: string = "CreateDateTime";
    oldSearchFields: string = null;
    cacheBuffer: any[] = [];
    OldFilters: ApiQueryFilters;
    ReloadData: boolean;
    RecievedDataCount: number = 0;
    MyCallTime: Date;
    getRow(rowIndex: number, sortingCol: string, sortingDir: string, getCount: boolean, searchfields: string, returnCached: boolean, Filters: ApiQueryFilters, reload: boolean = false, IgnorerowsRequestedPage: boolean = false, PSize: number = this.pageSize, SearchFieldChanged: boolean = false) {
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
        var rowInfo: IRow = { pageIndex: 0, rowIndex: rowIndex };
        var pageIndex = Math.floor(rowIndex / PSize);
        var exists = this.cachedData[rowIndex];

        if (!exists) {
            var rowsRequestedPage = this.rowsRequested.filter(x => x.pageIndex == pageIndex)[0];
            if (!rowsRequestedPage || IgnorerowsRequestedPage == true) {
                this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                var result: any[] = [];
                console.log("this.MyCallTime before" + this.MyCallTime);
                this.dataSource.getRows(pageIndex * PSize, PSize, sortingCol, sortingDir, getCount, searchfields, Filters).then(res => {
                    res.subscribe((viewResponse: ServiceResponse) => {
                        if (!viewResponse.HasError) {
                            if (this.MyCallTime == null || viewResponse.CallTime > this.MyCallTime) {
                                this.MyCallTime = viewResponse.CallTime;
                                console.log("this.MyCallTime " + this.MyCallTime);
                                this.RecievedDataCount = viewResponse.Result.length;
                                var jsonlist = viewResponse.Result;
                                for (var i = 0; i < jsonlist.length; i++) {
                                    this.cachedData[i + (pageIndex * PSize)] = jsonlist[i];
                                }
                                var requestedRows = this.rowsRequested.filter(x => x.pageIndex == pageIndex);
                                requestedRows.forEach((value: IRow, index: number) => {
                                    if (jsonlist[index] != undefined) {
                                    }
                                    result.push({ RowIndex: value.rowIndex, RowData: jsonlist[index] });
                                });

                                if (this.cacheBuffer.length > 0) {

                                    this.cacheBuffer.forEach((cacheValue, index: number) => {
                                        result.push({ RowIndex: cacheValue.RowIndex, RowData: cacheValue.RowData });
                                    });
                                    this.cacheBuffer = [];
                                }


                                if (!getCount) {
                                    this.requestedRowsReady.emit(result);
                                }
                                else {
                                    this.requestedRowCount.emit(viewResponse.Count);
                                }
                            }
                        }
                    });
                });
            }
            else {
                if (!this.rowsRequested.filter(e => e.pageIndex == pageIndex && e.rowIndex == rowIndex)[0]) {
                    this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                }
            }
        }
        else {

            var cacheResult: any = { RowIndex: rowIndex, RowData: this.cachedData[rowIndex] };
            var resExists = this.cacheBuffer.filter(d => d.RowIndex === cacheResult.RowIndex)[0];
            if (!resExists) {
                this.cacheBuffer.push(cacheResult);
            }
            if (returnCached) {
                this.requestedRowsReady.emit(this.cacheBuffer);
                this.cacheBuffer = [];
            }

        }

    }
    getCount(rowcount: number) {
        this.requestedRowCount.emit(rowcount);
    }

    setDataSource(dataSource: any) {
        this.dataSource = dataSource;
        this.pageSize = dataSource.pageSize;
    }

    Clear() {
        this.cachedData = {};
        this.cacheBuffer = [];
        this.rowsRequested = [];

    }

    public clone(jsonPM: any) {

        var entityPM: any;

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

    }

    public ClearCache() {
        this.cachedData = {};
        this.rowsRequested = [];
    }

}
