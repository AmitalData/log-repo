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
    styleUrls: ['./dashboard-shipments.component.css']
})
export class DashboardShipmentsComponent 
{


    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    searchForm;
    Shipments: CargoTrackingShipmentList[] = [];
    tenant;
    isLoading = false;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private searchService: CargoTrackingSearchService)
    {
        this.GetVariablesFromURI();
        // this.listenToRouterEvents();
        this.InitForm();

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
        this.Search();
    }
    Search()
    {
        if (this.tenant && this.SearchText) {
            // this.router.navigate([this.tenant,'search', this.SearchText]);
            this.LoadShipments();
        }

    }


    ItemClicked(item)
    {
        var SecurityKey = item.SecurityKey;

        this.router.navigate([this.tenant, 'search', 'shipment', SecurityKey]);

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


}
