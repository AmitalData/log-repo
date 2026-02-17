import {EventEmitter, Output, OnInit, OnChanges, Component} from '@angular/core';
import * as Rx from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ViewResponse} from '../../../DataContracts/ViewResponse';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LogEvents} from '../../../../Infrastructure/Utilities/LogEvents';

interface IRow {
    pageIndex: number;
    rowIndex: number;
}

export class EditGridVirtualRowController implements OnInit, OnChanges {
    dataSource: any;
    private cachedData: { [key: string]: any[] };
    private pageSize: number = 15; // = 15;
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
    getRow(rowIndex: number, sortingCol: string, sortingDir: string, getCount: boolean, searchfields: string, returnCached: boolean, Filters: ApiQueryFilters, reload: boolean = false) {
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
        var rowInfo: IRow = { pageIndex: 0, rowIndex: rowIndex };
        var pageIndex = Math.floor(rowIndex / this.pageSize);
        ////console.log("pageIndex", pageIndex);
        ////console.log(this.dataSource.getRows(0, 20))
        var exists = this.cachedData[rowIndex];
        ////console.log(exists);

        if (!exists) {
            var rowsRequestedPage = this.rowsRequested.filter(x => x.pageIndex == pageIndex)[0];
            if (!rowsRequestedPage) {
                this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                var result: any[] = [];
                this.dataSource.getRows(pageIndex * this.pageSize, this.pageSize, sortingCol, sortingDir, getCount, searchfields, Filters).then(res=> {
                    res.subscribe((viewResponse: ViewResponse) => {
                        
                        ////console.log("getRows from server pageIndex", pageIndex);
                        //console.log(viewResponse);
                        var jsonlist = viewResponse.Data; //.json();

                        //this.getCount(viewResponse.length);
                        //this.cachedData[0] = jsonlist;
                        for (var i = 0; i < jsonlist.length; i++) {

                            this.cachedData[i + (pageIndex * this.pageSize)] = jsonlist[i];

                        }
                        ////var rowInfo: IRow = { pageIndex: 0, rowIndex: rowIndex };
                        //this.rowsRequested[this.pageIndex] = jsonlist;
                        
                        
                        ////console.log("rowIndex", rowIndex);
                        //this.requestedPageReady.emit('data');
                        var requestedRows = this.rowsRequested.filter(x => x.pageIndex == pageIndex);
                        requestedRows.forEach((value: IRow, index: number) => {
                            if (jsonlist[index] == undefined) {
                                //console.log(index);
                                //console.log(requestedRows);
                                //console.log(jsonlist);
                                //console.log(jsonlist[index]);
                            }
                            result.push({ RowIndex: value.rowIndex, RowData: jsonlist[index] });
                        });

                        if (this.cacheBuffer.length>0) {
                            //console.log("emit cached data in server call=-=-=-=-=-=-", this.cacheBuffer);
                            this.cacheBuffer.forEach((cacheValue, index: number) => {
                                result.push({ RowIndex: cacheValue.RowIndex, RowData: cacheValue.RowData });
                            });
                            this.cacheBuffer = [];
                        }

                        //this.rowsRequested = [];
                        if (!getCount) {
                            //for (var i = 0; i < result.length; i++) {
                            //    console.log("emit from server ------------>", result[i].RowIndex);
                            //}
                            this.requestedRowsReady.emit(result);
                        }
                        else {
                           // console.log("emit the count------------------------------------88888");
                            this.requestedRowCount.emit(viewResponse.Count);
                        }
                        //return result;
                    });
                });

                //Rx.Observable.defer(() => {
                   
                //});
            }
            else {
                if (!this.rowsRequested.filter(e => e.pageIndex == pageIndex && e.rowIndex == rowIndex)[0]) {
                    this.rowsRequested.push({ pageIndex: pageIndex, rowIndex: rowIndex });
                }
                
                //return Rx.Observable.fromArray(() => {
                //    return ["In Progress"];
                //});
            }
        }
        else {
            
            // return Rx.Observable.fromArray(this.cachedData).filter(x => x == this.cachedData[rowIndex]);
            var cacheResult: any = { RowIndex: rowIndex, RowData: this.cachedData[rowIndex] };
            var resExists = this.cacheBuffer.filter(d=> d.RowIndex === cacheResult.RowIndex)[0];
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
        
    }

    getCount(rowcount: number) {
        //return Rx.Observable.defer(() => { return this.dataSource.getRows(0, 20); });

        this.requestedRowCount.emit(rowcount);
    }

    setDataSource(dataSource: any) {
        this.dataSource = dataSource;
    }

    Clear() {
        this.cachedData = {};
        this.cacheBuffer = [];
        this.rowsRequested = [];
        
    }

}