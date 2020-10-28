import { CargoTrackingShipmentList } from './../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from './../../Services/Others/CargoTrackingSearchService';
import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized } from '@angular/router';
import { fromEvent } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';
import { db } from '../../../app/mem.data';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';
import { RootContext } from 'src/CargoTracking/Utilities/RootContext';


@Component({
    selector: 'search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.css']
})
export class SearchComponent implements AfterViewInit
{

    @ViewChild('input') input: ElementRef;
    isLoading: boolean = false;
    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    searchForm;
    Shipments: CargoTrackingShipmentList[] = [];
    _Tenant:number;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private searchService: CargoTrackingSearchService)
    {
        this.GetVariablesFromURI();
        this.GetSearchTextFromURI();
        this.listenToRouterEvents();
       

        if (this.SearchText) {
            this.Search();
        }

        this.InitForm();
    }


    private GetSearchTextFromURI()
    {   
        let searchKey = this.route.snapshot.paramMap.get('searchKey');
        this.SearchText = searchKey;
 
    }


    private GetVariablesFromURI()
    {   
        let searchKey = this.route.snapshot.paramMap.get('searchKey');
      

        var tenant = this.route.snapshot.parent.paramMap.get('Tenant');
        if(tenant!=null && tenant!=""){
           this._Tenant = Number(tenant);
         }
         else{
            //  if(searchKey!=null && searchKey!=""){
            //     this.router.navigate([1,'search',searchKey]);
            //  }
            //  else{
            //     this.router.navigate([1,'search']);
            //  }
            
         }
    }
    private InitForm()
    {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });
    }

    ngAfterViewInit()
    { 
     //   document.documentElement.style.setProperty('--MainColor', CargoTrackingBrandingData.MainColor);
    }

    SubscribeInputTextChanges()
    {
        // server-side search // after view init
        // fromEvent(this.input.nativeElement,'keyup')
        //     .pipe(
        //         // get value
        //         map((event: any) =>
        //         {
        //             this.isLoading = true;
        //             console.log("Setting keey!");

        //             return event.target.value;
        //         })
        //         // if character length greater then 2
        //         , filter((res:string) =>
        //         {
        //             return true;
        //             if (res.length > 2) {
        //                 console.log("I'm BIG!");
        //                 return true;
        //             }
        //             else {
        //                 this.FilterItems();
        //                 console.log("I'm little");
        //                 return false;
        //             }
        //         }),

        //         // filter(Boolean),
        //         debounceTime(400),
        //         distinctUntilChanged(),
        //         tap((obj) =>
        //         {
        //             this.isLoading = true;
        //             var text = this.input.nativeElement.value;
        //             console.log(this.input.nativeElement.value);

        //             this.router.navigate(['/search', text]);
        //             this.FilterItems();
        //         })
        //     )
        //     .subscribe();
    }


    private listenToRouterEvents()
    {
        this.router.events.subscribe((event: Event) =>
        {
            if (event instanceof RoutesRecognized) {

                var url = event.urlAfterRedirects;
                if (url == "/"+this._Tenant+"/search/") {
                    this._SearchText = '';
                    this.FilterItems();
                }

            }

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
        this.Search();
    }
    Search()
    {
        if(this._Tenant && this.SearchText){
            this.router.navigate([this._Tenant,'search', this.SearchText]);
            // this.FilterItems();
            this.LoadShipments();
        }
            
    }
    FilterItems()
    {
        this.noResult = false;
        var searchText = this._SearchText.toLowerCase();
        this.FilteredItems = [];
        if (searchText) {
            var items = db.Shipments;
            // var items =  this.Items;
            items.forEach((item) =>
            {

                Object.keys(item).forEach(k =>
                {
                    var itemProperty = item[k].toString().toLowerCase();
                    if (itemProperty.includes(searchText)) {
                        if (this.FilteredItems.indexOf(item) < 0)
                            this.FilteredItems.push(item);
                    }
                });
            });
            this.noResult = this.FilteredItems.length == 0 && !!this.SearchText;
            this.isLoading = false;
        }
    }

    ItemClicked(item)
    {   var selection = window.getSelection();
        if(selection.toString().length === 0) {
            var SecurityKey = item.SecurityKey;

        this.router.navigate([this._Tenant,'search','shipment', SecurityKey]);
        }
    }
    LoadShipments()
    {

     

        this.noResult = false;
        var searchText = this._SearchText.trim().toLowerCase();
        if (searchText) {
            this.isLoading = true;
            RootContext.StartBusyIndicatorLoading();
            this.searchService.getShipments(searchText, this._Tenant).subscribe((result: any) =>
            {   RootContext.StopBusyIndicator();
                this.isLoading = false;
                console.log("[getShipments]", result);
                this.Shipments = result;
                this.noResult = this.Shipments.length == 0 && !!this.SearchText;

            });
        }else{
            this.Shipments = [];
        }

    }
    references: string[];
    SplitReference(reference: string){
        this.references = reference != null ? reference.split(',').slice(0, 6) : null;
  
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
