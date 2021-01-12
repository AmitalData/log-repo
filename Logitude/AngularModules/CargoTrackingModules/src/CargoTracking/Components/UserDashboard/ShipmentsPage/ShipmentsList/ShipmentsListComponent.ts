import { Component, ViewChild, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from '../../../../Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from '../../../../EntityLists/CargoTrackingShipmentList';
import { SessionInfo } from '../../../../../Infrastructure/Utilities/SessionInfo';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { ShipmentDataSource } from '../../../../DataContracts/CargoTrackingShipmentDataSource';
import { CargoTrackingShipmentFilters } from '../../../../DataContracts/CargoTrackingShipmentFilters';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';

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

    isLoading: boolean = false;
    isFiltersSideBarOpened: boolean = false;
    isFilter1Expanded: boolean = false;
    isFilter2Expanded: boolean = false;
    isAbdullahCompanyChecked: boolean = true;
    showSortDetailsMenu: boolean = false;
    showShipmentDetailsMenu: boolean = false;
    InvitedCustomersIds: string[];
    InvitedCustomers: any[] = [];
    ShipmentsDataSource;
    @ViewChild(CdkVirtualScrollViewport) virtualScroll: CdkVirtualScrollViewport;
    @ViewChild('input') searchInput: ElementRef;


    get tenant(){
        return CargoTrackingBrandingData.Tenant;
    }

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
        this.GetPreservedToggleFiltersFromSessionInfo();
        this.GetCompanyLoginsFromCache();
    }

    private GetPreservedToggleFiltersFromSessionInfo()
    {
        if (SessionInfo.ShipmentsFilters) {
            this.SearchText = SessionInfo.ShipmentsFilters.SearchText;
            this.SelectToggleFilters(SessionInfo.ShipmentsFilters.TransportModeCodes);
            this.SelectToggleFilters(SessionInfo.ShipmentsFilters.DirectionCodes);
        }
    }

    private InitComponent()
    {

        this.InitForm();

    }

    private GetCompanyLoginsFromCache()
    {
        SessionInfo.LoggedUserCompanyLogins = JSON.parse(sessionStorage.getItem("LoggedUserCompanyLogins"));
        console.log("[LoggedUserCompanyLogins]", SessionInfo.LoggedUserCompanyLogins);
        this.GetInvitedCustomers();
    }

    private GetInvitedCustomers()
    {

        // this.AddDemoCustomersForTest();

        this.InvitedCustomersIds = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => d.CardId);

        this.InvitedCustomers = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => ({ IsSelected: false, ...d }));
            
 
        console.log("[Invited Customers]", this.InvitedCustomersIds);

        this.LoadScreenData();
    }

  

    private AddDemoCustomersForTest()
    {
        var demoCustomer1 = {
            CardType: 'CS',
            CardId: '1-711',
            Tenant: this.tenant,
            CompanyName: 'Customer 711'
        };
        var demoCustomer2 = {
            CardType: 'CS',
            CardId: '1-749',
            Tenant: this.tenant,
            CompanyName: 'Customer 749'
        };
        SessionInfo.LoggedUserCompanyLogins.push(demoCustomer1, demoCustomer2);
    }

    private InitForm()
    {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });

    }
 



    private _SearchText: string = '';
    public get SearchText(): string
    {
        return this._SearchText;
    }
    public set SearchText(v: string)
    {
        this._SearchText = v;

        // if (this.SearchText)
        //     this.LoadShipments();
    }

    Clear()
    {
        this.SearchText = '';
        this.LoadScreenData();
    }

    Search()
    {
        if (this.tenant!=null && this.SearchText) {
            this.Shipments = [];
            this.LoadScreenData();
        }

    }

    ItemClicked(item)
    {
        var SecurityKey = item.SecurityKey;
        SessionInfo.ShipmentsFilters = this.BuildShipmentFilters();

        this.router.navigate(['Cargo-Tracking', 'shipment', SecurityKey]);

    }

    ShipmentsCounter: CargoTrackingShipmentsCounter = new CargoTrackingShipmentsCounter();
    LoadScreenData()
    {
        if (this.tenant) 
        {
            var shipmentFilters = this.BuildShipmentFilters();
            this.LoadShipments(shipmentFilters);
            this.LoadShipmentsCounter(shipmentFilters);
        }
    }
    references: string[];
    isSortDescending: boolean = true;
    private LoadShipments(shipmentFilters: CargoTrackingShipmentFilters)
    {
        if (this.ShipmentsDataSource)
            this.ReloadShipments(shipmentFilters);

        else
            this.InitiateShipmentDataSource(shipmentFilters);
    }

    private LoadShipmentsCounter(shipmentFilters: CargoTrackingShipmentFilters)
    {
        this.searchService.GetUserShipmentsCounter(shipmentFilters).subscribe((counter: any) =>
        {
            console.log("[GetUserShipmentsCounter]", counter);
            this.ShipmentsCounter = counter;
            this.BuildToggleFilters();
        });
    }

    private InitiateShipmentDataSource(shipmentFilters: CargoTrackingShipmentFilters)
    {
        this.ShipmentsDataSource = new ShipmentDataSource(this.changeDetector, this.searchService, shipmentFilters, this);
    }

    private ReloadShipments(shipmentFilters: CargoTrackingShipmentFilters)
    {
        this.ShipmentsDataSource.ReloadData(shipmentFilters);
        this.ResetShipmentsScrollbarPosition();
    }

    private ResetShipmentsScrollbarPosition()
    {
        this.virtualScroll.scrollToIndex(0);
    }

    SortClicked(){
        this.isSortDescending = !this.isSortDescending;
        this.LoadScreenData();
    }
    private BuildShipmentFilters()
    {
        var shipmentFilters = new CargoTrackingShipmentFilters();
        shipmentFilters.Tenant = this.tenant;
        shipmentFilters.SearchText = this._SearchText ? this._SearchText.trim().toLowerCase() : '';

        
        shipmentFilters.SortDescending = this.isSortDescending;
        
        this.SetCustomersFilter(shipmentFilters);
        this.SetTransportModeFilters(shipmentFilters);
        this.SetDirectionFilters(shipmentFilters);
        return shipmentFilters;
    }

    private SetCustomersFilter(shipmentFilters: CargoTrackingShipmentFilters)
    {
        if(this.InvitedCustomers.filter(cs=>cs.IsSelected).length > 0){
            var str = this.InvitedCustomers.filter(cs=>cs.IsSelected).map(d => d.CardId)?.join(',');

        }else{
            var str = this.InvitedCustomers.map(d => d.CardId)?.join(',');
        }
        
        shipmentFilters.CustomersIds = this.InvitedCustomersIds;
        shipmentFilters.CustomersIdsString = str;
    }

    private SetDirectionFilters(shipmentFilters: CargoTrackingShipmentFilters)
    {
        var direction = "";
        if (this.SelectedFilters.length > 0){
            const directionsCodes = ['IM', 'EX'];
            direction = this.SelectedFilters.filter(d => directionsCodes.includes(d.Code)).map(d => d.Code).join(',');
        }
        shipmentFilters.DirectionCodes = direction;
    }

    private SetTransportModeFilters(shipmentFilters: CargoTrackingShipmentFilters)
    {
        var transportMode = "";
        if (this.SelectedFilters.length > 0){
            const transportModeCodes = ['A', 'I', 'O'];
            transportMode = this.SelectedFilters.filter(d => transportModeCodes.includes(d.Code)).map(d => d.Code).join(',');
        }
        shipmentFilters.TransportModeCodes = transportMode;
    }
    private SelectToggleFilters(toggleFilterCodes: string)
    {
        if(toggleFilterCodes){
            var splitted = toggleFilterCodes.split(',');
            splitted.forEach(filterCode=>{
                this.SelectFilterByCode(filterCode);
            });
        }
    }

    SplitReference(reference: string)
    {
        this.references = reference != null ? reference.split(',') : null;

    }

    GetModeIcon(mode: string)
    {
        var iconPath = "";
        const Air = 'A';
        const Ocean = 'O';
        const InLand = 'I';
        switch (mode) {
            case Air:
                iconPath = "./assets/images/misc/plane.svg";
                break;
            case Ocean:
                iconPath = "./assets/images/misc/ship.svg";
                break;
            case InLand:
                iconPath = "./assets/images/misc/Truck.svg";
                break;
            default:
                break;
        }
        return iconPath;
    }

    ToggleFilters: ToggleFilter[] = [
        new ToggleFilter('IM', 'Import'),
        new ToggleFilter('EX', 'Export'),
        new ToggleFilter('A', 'Air'),
        new ToggleFilter('I', 'Land'),
        new ToggleFilter('O', 'Sea'),
    ];

    BuildToggleFilters(){
        this.ToggleFilters = [
            new ToggleFilter('IM', 'Import',this.ShipmentsCounter.Import),
            new ToggleFilter('EX', 'Export',this.ShipmentsCounter.Export),
            new ToggleFilter('A', 'Air',this.ShipmentsCounter.Air),
            new ToggleFilter('I', 'Land',this.ShipmentsCounter.Land),
            new ToggleFilter('O', 'Sea',this.ShipmentsCounter.Sea),
        ];

        this.changeDetector.detectChanges();
    }

    SelectedFilters: ToggleFilter[] = [];
    SelectFilter(filter: ToggleFilter)
    {
        var item = this.SelectedFilters.find(d => d.Name == filter.Name);
        if (!item)
            this.SelectedFilters.push(filter);

        this.LoadScreenData();
    }
    SelectFilterByCode(filterCode: string)
    {
        var filter = this.ToggleFilters.find(d => d.Code == filterCode);
        this.SelectFilter(filter);
    }
    DeselectFilter(filter: ToggleFilter)
    {
        var index = this.SelectedFilters.findIndex(d => d.Name == filter.Name);
        this.SelectedFilters.splice(index, 1);

        this.LoadScreenData();

    }
    ClearFilters()
    {
        this.SelectedFilters = [];
        this.LoadScreenData();
    }

    ApplyFilterButtonClicked()
    {
        this.isFiltersSideBarOpened = false;
        this.LoadScreenData();
    }
    ClearAdvancedFilters(){
        this.isFiltersSideBarOpened = false;
        this.InvitedCustomers.forEach(d=>{d.IsSelected=false});
        this.LoadScreenData();
    }
    SortMenuClicked(buttonCode: string)
    {
        this.isSortDescending = buttonCode == "desc";
        this.LoadScreenData();
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

    ShipmentsLoadingError: string;

    FocusOnSearchInput(){
        this.searchInput.nativeElement.focus();
    }
}

export class ToggleFilter
{
    constructor(code: string, name: string, count: number = 0)
    {
        this.Code = code;
        this.Name = name;
        this.Count = count;
    }


    private name: string;
    public get Name(): string
    {
        return this.name;
    }
    public set Name(v: string)
    {
        this.name = v;
    }



    private count: number = 0;
    public get Count(): number
    {
        return this.count;
    }
    public set Count(v: number)
    {
        this.count = v;
    }


    private code: string;
    public get Code(): string
    {
        return this.code;
    }
    public set Code(v: string)
    {
        this.code = v;
    }



}

export class CargoTrackingShipmentsCounter{
    
    Import: number = 0;
    Export: number = 0;
    Air: number = 0;
    Land: number = 0;
    Sea: number = 0;
}
