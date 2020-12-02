import { Component, ViewChild, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
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
    ShipmentsDS;
    @ViewChild(CdkVirtualScrollViewport) virtualScroll: CdkVirtualScrollViewport;

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
        this.GetCompanyLoginsFromCache();
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
        this.InvitedCustomersIds = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => d.CardId);
        console.log("[Invited Customers]", this.InvitedCustomersIds);

        this.LoadShipments();
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
        this.LoadShipments();
    }

    Search()
    {
        if (this.tenant!=null && this.SearchText) {
            this.Shipments = [];
            this.LoadShipments();
        }

    }




    ItemClicked(item)
    {
        var SecurityKey = item.SecurityKey;

        this.router.navigate(['dashboard', 'shipment', SecurityKey]);

    }
    LoadShipments()
    {
        if (this.tenant) {
            var shipmentFilters = this.BuildShipmentFilters();
            if (this.ShipmentsDS) {
                this.ShipmentsDS.ReloadData(shipmentFilters);
                this.virtualScroll.scrollToIndex(0);
            } else {
                this.ShipmentsDS = new ShipmentDataSource(this.changeDetector, this.searchService, shipmentFilters, this);
                this.changeDetector.detectChanges();
            }
        }
    }
    references: string[];
    isSortDescending: boolean = true;
    SortClicked(){
        this.isSortDescending = !this.isSortDescending;
        this.LoadShipments();
    }
    private BuildShipmentFilters()
    {
        var shipmentFilters = new CargoTrackingShipmentFilters();
        shipmentFilters.Tenant = this.tenant;
        shipmentFilters.SearchText = this._SearchText ? this._SearchText.trim().toLowerCase() : '';
        shipmentFilters.CustomersIds = this.InvitedCustomersIds;
        shipmentFilters.CustomersIdsString = this.InvitedCustomersIds?.join(',');
        shipmentFilters.SortDescending = this.isSortDescending;

        this.SetTransportModeFilters(shipmentFilters);
        this.SetDirectionFilters(shipmentFilters);
        return shipmentFilters;
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

    SelectedFilters: ToggleFilter[] = [];
    SelectFilter(filter: ToggleFilter)
    {
        var item = this.SelectedFilters.find(d => d.Name == filter.Name);
        if (!item)
            this.SelectedFilters.push(filter);

        this.LoadShipments();
    }
    DeselectFilter(filter: ToggleFilter)
    {
        var index = this.SelectedFilters.findIndex(d => d.Name == filter.Name);
        this.SelectedFilters.splice(index, 1);

        this.LoadShipments();

    }
    ClearFilters()
    {
        this.SelectedFilters = [];
        this.LoadShipments();
    }

    ApplyFilterButtonClicked()
    {
        this.isFiltersSideBarOpened = false;
    }
    SortMenuClicked(buttonCode: string)
    {
        this.isSortDescending = buttonCode == "desc";
        this.LoadShipments();
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
}

export class ToggleFilter
{
    constructor(code: string, name: string)
    {
        this.Code = code;
        this.Name = name;
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
