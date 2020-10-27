import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized } from '@angular/router';
import { fromEvent } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';


@Component({
    selector: 'dashboard-shipments',
    templateUrl: './dashboard-shipments.component.html',
    styleUrls: ['./dashboard-shipments.component.min.css']
})
export class DashboardShipmentsComponent 
{


    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    searchForm;
    Shipments: CargoTrackingShipmentList[] = [];
    tenant;
    isLoading: boolean  = false;
    isFiltersSideBarOpened: boolean  = false;
    isFilter1Expanded: boolean  = false;
    isFilter2Expanded: boolean  = false;
    isAbdullahCompanyChecked: boolean  = true;
    showSortDetailsMenu: boolean  = false;
    showShipmentDetailsMenu: boolean  = false;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private searchService: CargoTrackingSearchService)
    {
        this.GetVariablesFromURI();
        // this.listenToRouterEvents();
        this.InitForm();
        this.SearchText = 'abed';
        this.Search();

    }
    private InitForm()
    {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });
    }

    private GetVariablesFromURI()
    {
        // let searchKey = this.route.snapshot.paramMap.get('searchKey');


        var tenant = this.route.snapshot.parent.paramMap.get('Tenant');
        if (tenant != null && tenant != "") {
            this.tenant = Number(tenant);
        }
        else {
            //  if(searchKey!=null && searchKey!=""){
            //     this.router.navigate([1,'search',searchKey]);
            //  }
            //  else{
            //     this.router.navigate([1,'search']);
            //  }

        }
    }

    private _SearchText: string = '';
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
        if (this.tenant && this.SearchText) {
            this.Shipments = [];
            // this.router.navigate([this.tenant,'search', this.SearchText]);
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
        var searchText = this._SearchText.trim().toLowerCase();
        if (searchText) {
            this.isLoading = true;
            this.searchService.getShipments(searchText, this.tenant).subscribe((result: any) =>
            {
                this.isLoading = false;
                console.log("[getShipments]", result);
                this.Shipments = result;
                this.noResult = this.Shipments.length == 0 && !!this.SearchText;

            });
        } else {
            this.Shipments = [];
        }

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
    SelectFilter(filter:SearchFilter ){
        var item = this.SelectedFilters.find(d=>d.Name == filter.Name);
        if(!item)
            this.SelectedFilters.push(filter);
    }
    DeselectFilter(filter:SearchFilter ){
        var index = this.SelectedFilters.findIndex(d=>d.Name == filter.Name);
        this.SelectedFilters.splice(index,1);
    }
    ClearFilters(){
        this.SelectedFilters = [];
    }

    ApplyFilterButtonClicked() {
        this.isFiltersSideBarOpened = false;
    }
    SortMenuClicked(buttonCode: string){
        console.log("sort by clicked, ",buttonCode);
    }

    lastClickedShipment: any;
    ShipmentMoreButtonClicked(shipment: any , event: any){
        event.preventDefault();
        event.stopPropagation();

        this.lastClickedShipment = shipment;
        this.showShipmentDetailsMenu = true;
    }
    ShipmentDetailsMenuClicked(buttonCode: string){
        console.log("shipment more details", buttonCode, this.lastClickedShipment);

        if(buttonCode == "set")
            this.lastClickedShipment.IsFavorite = true;
        
    }

}

export class SearchFilter {
    constructor(name: string) {
        this.Name = name;
    }
    

    private _Name : string;
    public get Name() : string {
        return this._Name;
    }
    public set Name(v : string) {
        this._Name = v;
    }


    
    private _Count : number = 0;
    public get Count() : number {
        return this._Count;
    }
    public set Count(v : number) {
        this._Count = v;
    }
    
    
}