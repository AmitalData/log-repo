import { Component, ViewChild, ElementRef, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized } from '@angular/router';
import { BehaviorSubject, fromEvent, Observable, Subscription } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';


@Component({
    selector: 'ShipmentsListComponent',
    templateUrl: './ShipmentsListComponent.html',
    styleUrls: [
        './ShipmentsListComponent.css',
        './ShipmentsListComponent.mobile.css',
        './ShipmentsListComponent.tablet.css'
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,

})
export class ShipmentsListComponent implements AfterViewInit
{


    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    searchForm;
    Shipments: CargoTrackingShipmentList[] = [];
    tenant;
    isLoading: boolean = false;
    isFiltersSideBarOpened: boolean = false;
    isFilter1Expanded: boolean = false;
    isFilter2Expanded: boolean = false;
    isAbdullahCompanyChecked: boolean = true;
    showSortDetailsMenu: boolean = false;
    showShipmentDetailsMenu: boolean = false;
    InvitedCustomersIds: string[];
    ShipmentsDS;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private changeDetector: ChangeDetectorRef,
        private searchService: CargoTrackingSearchService)
    {

        this.InitComponent();

    }
    ngAfterViewInit(): void
    {
        this.GetCompanyLoginsFromCache();
    }

    private InitComponent()
    {
        this.InitForm();
        this.GetTenantFromURL();


    }

    private GetCompanyLoginsFromCache()
    {
        SessionInfo.LoggedUserCompanyLogins = JSON.parse(sessionStorage.getItem("LoggedUserCompanyLogins"));
        console.log("[LoggedUserCompanyLogins]", SessionInfo.LoggedUserCompanyLogins);
        this.GetInvitedCustomers();
    }

    private GetInvitedCustomers()
    {
        this.InvitedCustomersIds = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => d.CardId);
            console.log("[Invited Customers]",this.InvitedCustomersIds);     
            
        this.Search();
    }

    private GetTenantFromURL()
    {
        var tenant = Number(this.route.snapshot.parent.paramMap.get('Tenant'));
        if (tenant) 
            this.tenant = tenant;
        else
            console.error("Tenant not provided in URL");            
    }

    private InitForm()
    {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });
    }

    

    private _SearchText: string;
    public get SearchText(): string
    {
        return this._SearchText;
    }
    public set SearchText(v: string)
    {
        this._SearchText = v;
        if (!this.SearchText)
            this.Search();
    }

    Clear()
    {
        this.SearchText = '';
        this.LoadShipments();
    }
    Search()
    {
        if (this.tenant) {
            this.Shipments = [];
            this.ShipmentsDS = null;
            this.changeDetector.detectChanges();
            this.LoadShipments();
        }

    }


    ItemClicked(item)
    {
        var SecurityKey = item.SecurityKey;

        this.router.navigate([this.tenant, 'dashboard', 'shipment', SecurityKey]);

    }
    LoadShipments()
    {

        this.noResult = false;
        var searchText = this._SearchText ? this._SearchText.trim().toLowerCase() : '';
            var shipmentFilters = new CargoTrackingShipmentFilters();
            shipmentFilters.Tenant = this.tenant;
            shipmentFilters.SearchText = searchText;
            shipmentFilters.CustomersIds = this.InvitedCustomersIds;
            shipmentFilters.CustomersIdsString = this.InvitedCustomersIds?.join(',');
            
            this.ShipmentsDS = new ShipmentDataSource(this.changeDetector, this.searchService,shipmentFilters, this);
            this.changeDetector.detectChanges();
    }
    references: string[];
    SplitReference(reference: string)
    {
        this.references = reference != null ? reference.split(',') : null;

    }

    GetModeIcon(mode: string)
    {
        var iconPath = "";
        switch (mode) {
            case 'A':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'O':
                iconPath = "./assets/images/misc/ship.svg";
                break;

            case 'I':
                iconPath = "./assets/images/misc/Truck.svg";
                break;

            default:
                break;
        }

        return iconPath;
    }

    SearchFilters: SearchFilter[] = [
        new SearchFilter('Import'),
        new SearchFilter('Export'),
        new SearchFilter('Air'),
        new SearchFilter('Land'),
        new SearchFilter('Sea'),
    ];

    SelectedFilters: SearchFilter[] = [];
    SelectFilter(filter: SearchFilter)
    {
        var item = this.SelectedFilters.find(d => d.Name == filter.Name);
        if (!item)
            this.SelectedFilters.push(filter);
    }
    DeselectFilter(filter: SearchFilter)
    {
        var index = this.SelectedFilters.findIndex(d => d.Name == filter.Name);
        this.SelectedFilters.splice(index, 1);
    }
    ClearFilters()
    {
        this.SelectedFilters = [];
    }

    ApplyFilterButtonClicked()
    {
        this.isFiltersSideBarOpened = false;
    }
    SortMenuClicked(buttonCode: string)
    {
        console.log("sort by clicked, ", buttonCode);
    }

    lastClickedShipment: any;
    ShipmentMoreButtonClicked(shipment: any, event: any)
    {
        event.preventDefault();
        event.stopPropagation();

        this.lastClickedShipment = shipment;
        this.showShipmentDetailsMenu = true;
    }
    ShipmentDetailsMenuClicked(buttonCode: string)
    {
        console.log("shipment more details", buttonCode, this.lastClickedShipment);

        if (buttonCode == "set")
            this.lastClickedShipment.IsFavorite = true;

    }

    ShipmentsCount: number = 0;

    error: string;
}

export class SearchFilter
{
    constructor(name: string)
    {
        this.Name = name;
    }


    private _Name: string;
    public get Name(): string
    {
        return this._Name;
    }
    public set Name(v: string)
    {
        this._Name = v;
    }



    private _Count: number = 0;
    public get Count(): number
    {
        return this._Count;
    }
    public set Count(v: number)
    {
        this._Count = v;
    }


}
export class CargoTrackingShipmentFilters{
    public Tenant: number;
    public SearchText: string;
    public CustomersIds: string[] = [];
    public CustomersIdsString: string;

}
export class ShipmentDataSource extends DataSource<any | undefined> {
    private PAGE_SIZE = 50;
    private CachedShipments = Array.from<any>({ length: this.ShipmentsCount });
    private FetchedPages = new Set<number>();
    private _dataStream = new BehaviorSubject<(any | undefined)[]>(this.CachedShipments);
    private _subscription = new Subscription();

    constructor(
        private ChangeDetector: ChangeDetectorRef,
        private ShipmentSearchService: CargoTrackingSearchService,
        private ShipmentsFilters: CargoTrackingShipmentFilters,
        private parent: ShipmentsListComponent,
        private ShipmentsCount = 1
    )
    {
        super();

        this.InitComponent();

    }

    private InitComponent()
    {
        this.CachedShipments = Array.from<any>({ length: this.ShipmentsCount });
    }

    connect(collectionViewer: CollectionViewer): Observable<(any | undefined)[]>
    {
        this._subscription.add(collectionViewer.viewChange.subscribe(range =>
        {
            const startPage = this.GetPageForIndex(range.start);
            const endPage = this.GetPageForIndex(range.end - 1);
            for (let i = startPage; i <= endPage; i++) {
                this.FetchPage(i);
            }
        }));
        return this._dataStream;
    }

    disconnect(): void
    {
        this._subscription.unsubscribe();
    }

    private GetPageForIndex(index: number)
    {
        return Math.floor(index / this.PAGE_SIZE);
    }

    private FetchPage(pageNumber: number)
    {
        if (!this.FetchedPages.has(pageNumber)) {
            this.FetchedPages.add(pageNumber);
            this.GetShipmentsPage(pageNumber);
        }
    }

    private GetShipmentsPage(page: number)
    {
        this.ShipmentSearchService.GetUserShipments(page, this.PAGE_SIZE, this.ShipmentsFilters)
        .subscribe((result: any) =>
        {
            this.parent.error = "";

            var fetchedShipments = result.Shipments;

            if(result.Count != this.ShipmentsCount){
                this.ShipmentsCount = result.Count;
                this.parent.ShipmentsCount = result.Count;
                this.FetchedPages = new Set<number>();
                this.CachedShipments = Array.from<any>({ length: result.Count });
            }

            this.CacheShipments(page, fetchedShipments);

            this.ChangeDetector.detectChanges();

            this._dataStream.next(this.CachedShipments);
        }, error=>{
            this.parent.error = error.statusText;
            console.error(error);
            
            this.ChangeDetector.detectChanges();

        });
    }

    private CacheShipments(pageNumber: number, shipments: any)
    {
        this.CachedShipments.splice(
            pageNumber * this.PAGE_SIZE,
            this.PAGE_SIZE,
            ...shipments);
    }
}

