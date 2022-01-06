import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { ShipmentsListComponent } from '../Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';
import { CargoTrackingShipmentSearchInput } from './CargoTrackingShipmentFilters';

export class ShipmentDataSource extends DataSource<any | undefined>  {
    private pageSize = 50;
    private cachedShipments = Array.from<any>({ length: this.ShipmentsCount });
    private fetchedPages = new Set<number>();
    private dataStream = new BehaviorSubject<(any | undefined)[]>(this.cachedShipments);
    private subscription = new Subscription();
    timer = null;
    constructor(
        public ChangeDetector: ChangeDetectorRef,
        public ShipmentSearchService: CargoTrackingSearchService,
        public ShipmentsFilters: CargoTrackingShipmentSearchInput,
        private parent: ShipmentsListComponent,
        public ShipmentsCount = 1
    )
    {
        super();
        this.InitComponent();

    }

    private InitComponent()
    {
        this.parent.ShipmentsCount = 0;
        this.parent.noResult = false;
        this.cachedShipments = Array.from<any>({ length: this.ShipmentsCount || 1 });
        this.fetchedPages = new Set<number>();
        this.FetchPage(0);


        this.ChangeDetector.detectChanges();
    }

    ReloadData(filters)
    {
        this.ShipmentsFilters = filters;
        this.InitComponent();
        this.FetchPage(0);

        this.ChangeDetector.detectChanges();
        this.dataStream.next(this.cachedShipments);
    }
    connect(collectionViewer: CollectionViewer): Observable<(any | undefined)[]>
    {
        this.subscription.add(collectionViewer.viewChange.subscribe(range =>
        {
            // the following 4 lines of code added by Rabaia in order to inhance the performance of the CargoTracking. if you have Problem with it please talk to me --Rabaia 
            if (this.timer) {
                clearTimeout(this.timer);
            }
            this.timer = setTimeout(() => this.HandleRange(range), 400); 
            
        }));
        return this.dataStream;
    }

    HandleRange(range:any): void {
        const startPage = this.GetPageForIndex(range.start);
        const endPage = this.GetPageForIndex(range.end - 1);
        for (let i = startPage; i <= endPage; i++) {
            this.FetchPage(i);
        }
    }

    disconnect(): void
    {
        this.subscription.unsubscribe();
    }

    private GetPageForIndex(index: number)
    {
        return Math.floor(index / this.pageSize);
    }

    private FetchPage(pageNumber: number)
    {
        if (!this.fetchedPages.has(pageNumber)) {
            this.fetchedPages.add(pageNumber);
            this.GetShipmentsPage(pageNumber);
        }
    }

    private GetShipmentsPage(page: number)
    {
        this.ShipmentsFilters.PageIndex = page;
        this.ShipmentsFilters.PageSize = this.pageSize;
        this.ShipmentSearchService.GetUserShipments( this.ShipmentsFilters)
            .subscribe((shipmentsResponse: any) =>
            {
                this.parent.ShipmentsLoadingError = "";
                this.HandleShipmentsResponse(page, shipmentsResponse);
            }, error =>
            {
                this.parent.ShipmentsLoadingError = error.statusText;
                console.error(error);
                this.ChangeDetector.detectChanges();

            });
    }

    private HandleShipmentsResponse(page: number, shipmentsResponse: any)
    {
        if (page == 0)
            this.ResetDataSourceVariablesForFirstPage(shipmentsResponse);

        this.CacheShipments(page, shipmentsResponse.Shipments);

        this.ChangeDetector.detectChanges();
        this.dataStream.next(this.cachedShipments);
    }

    private ResetDataSourceVariablesForFirstPage(shipmentsResponse: any)
    {
        if(this.parent.ShipmentsCount != shipmentsResponse.ShipmentsCount)
            this.parent.ShipmentsCount = shipmentsResponse.ShipmentsCount

        if (shipmentsResponse.ShipmentsCount != this.ShipmentsCount) {
            this.SetShipmentsCount(shipmentsResponse.ShipmentsCount);
            this.ResetCachedShipmentsArray(shipmentsResponse.ShipmentsCount);
        }

        this.SetNoResultToggle();
    }

    private ResetCachedShipmentsArray(count: number)
    {
        this.cachedShipments = Array.from<any>({ length: count });
    }

    private SetShipmentsCount(count: any)
    {
        this.ShipmentsCount = count;
        this.parent.ShipmentsCount = count;
    }

    private SetNoResultToggle()
    {
        this.parent.noResult = (this.ShipmentsCount == 0);
    }

    private CacheShipments(pageNumber: number, shipments: any)
    {
        this.cachedShipments.splice(
            pageNumber * this.pageSize,
            this.pageSize,
            ...shipments);
    }
}

