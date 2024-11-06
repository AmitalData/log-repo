import { EventEmitter, Output, OnInit, OnChanges, Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

interface IRow {
    pageIndex: number;
    rowIndex: number;
}

//interface IRowMetaData {
//  rowIndex: number;
//  rowData: any;
//  styles?: any;
//  Detailsstyles?: any;
//  ShowDetails?: boolean;
//  DetailsIcon: string;
//  IsArchived?: boolean;
//}

export class VirtualRowMetaData {
    sortingCol: any;
    sortingDir: string;
    searchFields: string;
    Filters: ApiQueryFilters;
    rowsCount: number;
    IsSpotLight: boolean;
    SpotlightDataTemplate: string;
    DetailsIcon: string;
    cd: ChangeDetectorRef;
    DontApplyVirtualization: boolean;
}
export class VirtualRowControllerV2 extends DataSource<any | undefined> implements OnInit, OnChanges {
    dataSource: any;
    //public cachedData: { [key: string]: any };
    //private pageSize: number = 30; // = 15;
    private rowsRequested: IRow[];
    @Output() requestedRowsReady = new EventEmitter();
    @Output() requestedRowCount = new EventEmitter();
    firstRow: number = 0;
    myMetaData: VirtualRowMetaData;
    myCollectionViewer: CollectionViewer
    constructor(myMetaData: VirtualRowMetaData) {
        super();
        this.myMetaData = myMetaData;
        //this.cachedData = {};
        this.length = myMetaData.rowsCount;
        this.cachedData = Array.from<any>({ length: this.length });
        this.mycachedData = Array.from<any>({ length: this.length });
        this.dataStream = new BehaviorSubject<(any | undefined)[]>(this.cachedData);
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
    public length = 44;
    public pageSize = 23;
    cachedData: any[] = [];//Array.from<any>({ length: this.length });
    mycachedData: any[] = [];//Array.from<any>({ length: this.length });

    private fetchedPages = new Set<number>();
    private dataStream = new BehaviorSubject<(any | undefined)[]>(this.cachedData);
    private subscription = new Subscription();
    mySub: any;
    //getCount: boolean = false;
    searchfields: string = "";
    Filters: ApiQueryFilters = new ApiQueryFilters();

    connect(collectionViewer: CollectionViewer): Observable<(any | undefined)[]> {
        
            this.myCollectionViewer = collectionViewer;
            this.mySub = collectionViewer.viewChange.subscribe(range => {
                if (this.timer) {
                    clearTimeout(this.timer);
                }
                this.timer = setTimeout(() => this.HandleRange(range), 400);
            }); 
        return this.dataStream;
    }
    HandleRange(range: any) {
        if (this.myMetaData.DontApplyVirtualization) {
            //this.fetchedPages.delete(0);
            this._fetchPage(0);
        }
        else {
            const startPage = this._getPageForIndex(range.start);
            const endPage = this._getPageForIndex(range.end - 1);
            for (let i = startPage; i <= endPage; i++) {
                this._fetchPage(i);
            }
        }
        //if (this.myMetaData.cd) {
        //    this.myMetaData.cd.detectChanges();
        //}
    }

    disconnect(): void {
        //this.dataStream.complete();
        this.mySub.unsubscribe();
        //this.fetchedPages = null;
        //this.cachedData = null;
    }

    private _getPageForIndex(index: number): number {
        var page = Math.floor(index / this.pageSize);
        //if (page > 0) {
        //    page = page - 1;
        //}
        return page;
    }
    timer = null;
    private _fetchPage(page: number) {
        if (this.fetchedPages.has(page) && !this.myMetaData.DontApplyVirtualization) {
            if (!this.fetchedPages.has(page + 1)) {
                this._fetchPage(page + 1);
                //if (!this.fetchedPages.has(page + 2)) {  
                //    this._fetchPage(page + 2);
                //} 
            }
            //else {
            //this.dataStream.next(this.cachedData);
            //if (this.myMetaData.cd) {
            //    this.myMetaData.cd.detectChanges();
            //}
            //this.dataStream.next(this.cachedData);
            return;
            //}
        }
        else {
            if (!this.fetchedPages.has(page)) {
                this.getPageData(page)
            }
        }
    }


    private getPageData(page: number) {
        this.fetchedPages.add(page);
        //this.mycachedData = [];
        //this.pageSize = 17;
        this.dataSource.getRows(page * this.pageSize, this.pageSize, this.myMetaData.sortingCol, this.myMetaData.sortingDir, false, this.myMetaData.searchFields, this.myMetaData.Filters).then(res => {
            res.subscribe((viewResponse: ServiceResponse) => {
                if (!viewResponse.HasError) {
                    //if (this.MyCallTime == null || viewResponse.CallTime > this.MyCallTime) {
                    this.MyCallTime = viewResponse.CallTime;
                    console.log("this.MyCallTime " + this.MyCallTime);
                    this.RecievedDataCount = viewResponse.Result.length;
                    var jsonlist = viewResponse.Result;
                    if (jsonlist.length < this.pageSize && page == 0) {
                        this.mycachedData = Array.from<any>({ length: jsonlist.length });
                        this.cachedData = Array.from<any>({ length: jsonlist.length });
                    }
                    else if (this.cachedData.length < this.pageSize && page == 0) {
                        this.mycachedData = Array.from<any>({ length: this.length });
                        this.cachedData = Array.from<any>({ length: this.length });
                    }
                    this.dataStream.next(this.cachedData);
                    for (var i = 0; i < jsonlist.length; i++) {
                        //this.mycachedData[i + (page * this.pageSize)] = { rowData: jsonlist[i], rowIndex: (i + (page * this.pageSize)), DetailsIcon: "./Images/SpotLightPlusIcon.png" };//jsonlist[i];
                        var item = { rowData: jsonlist[i], rowIndex: (i + (page * this.pageSize)), DetailsIcon: "./Images/SpotLightPlusIcon.png" };
                        item.rowData['$id'] = (i + (page * this.pageSize)).toString();
                        var myIndex = i + (page * this.pageSize);
                        this.mycachedData.splice(i + (page * this.pageSize), 1, item);
                            //...Array.from({ length: jsonlist.length })
                               // .map((_, i) => item));
                    }
                    this.cachedData = this.mycachedData;//[...this.mycachedData]
                    this.dataStream.next(this.cachedData);
                    this.requestedRowCount.emit(viewResponse.Count);  

                    if (this.myMetaData.cd) {
                        this.myMetaData.cd.detectChanges();
                    }
                 
                }
               
            });
        });
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
    getRow(rowIndex: number, sortingCol: string, sortingDir: string, getCount: boolean, searchfields: string, returnCached: boolean, Filters: ApiQueryFilters, reload: boolean = false, IgnorerowsRequestedPage: boolean = false, PSize: number = this.pageSize, SearchFieldChanged: boolean = false ) {
        this.ReloadData = reload;
        if (searchfields != undefined && searchfields != null) {
            if (this.oldSearchFields != searchfields) {
                this.oldSearchFields = searchfields;
                this.rowsRequested = [];
                //this.cachedData = {};
            }
        }
        if (this.sortingCol != sortingCol || this.sortingDir != sortingDir) {
            this.sortingCol = sortingCol;
            this.sortingDir = sortingDir;
            //this.cachedData = {};
            this.rowsRequested = [];
        }
        var rowInfo: IRow = { pageIndex: 0, rowIndex: rowIndex };
        var pageIndex = Math.floor(rowIndex / PSize); 
         console.log("this.MyCallTime before" + this.MyCallTime);
        this.dataSource.getRows(pageIndex * PSize, PSize, sortingCol, sortingDir, getCount, searchfields, Filters).then(res => {
            res.subscribe((viewResponse: ServiceResponse) => {
                if (!viewResponse.HasError) { 
                    this.requestedRowCount.emit(viewResponse.Count);  
                }
            });
        });


    }
    getCount(rowcount: number) {
        this.requestedRowCount.emit(rowcount);
    }

    setDataSource(dataSource: any) {
        this.dataSource = dataSource;
        this.pageSize = dataSource.pageSize;
    }

    Clear() {
        //this.cachedData = {};
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
        //this.cachedData = Array.from<any>({ length: this.length });
        //this.dataStream = new BehaviorSubject<(any | undefined)[]>(this.cachedData);
        //this.rowsRequested = [];
    }

    public UpdateRecord(Row) {
        //this.dataStream.complete();
        var mycachedData = this.cachedData;//.splice(Row.rowIndex, 1, Row);
        mycachedData.splice(Row.rowIndex, 1, Row);
        this.cachedData = [...mycachedData]
        this.dataStream.next(this.cachedData);
        if (this.myMetaData.cd) {
            this.myMetaData.cd.detectChanges();
        }
        //this.dataStream.complete();
        //this.dataStream = new BehaviorSubject<(any | undefined)[]>(this.cachedData);
        //this.dataStream.next(this.cachedData);
    }
    public ReloadDataSource(Count:number) {
        //this.fetchedPages = new Set<number>();
        //this.cachedData = Array.from<any>({ length: Count });
        this._fetchPage(0);
    }

}
