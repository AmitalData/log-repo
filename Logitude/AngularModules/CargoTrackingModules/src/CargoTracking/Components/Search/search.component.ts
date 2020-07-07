import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized } from '@angular/router';
import { fromEvent } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';
import { db } from '../../../app/mem.data';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';


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
    

    constructor(private router: Router, private route: ActivatedRoute, private formBuilder: FormBuilder)
    {
        this.listenToRouterEvents();

        this.GetSearchTextFromURI();

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

    private InitForm()
    {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });
    }

    ngAfterViewInit()
    {
        document.documentElement.style.setProperty('--MianColor', CargoTrackingBrandingData.MainColor);
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
                if (url == "/search/") {
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
        this.router.navigate(['/search', this.SearchText]);
        this.FilterItems();
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
    {
        var id = item.Id;

        this.router.navigate(['/shipment', id]);

    }

    GetModeIcon(mode: string)
    {
        var iconPath = "";
        switch (mode) {
            case 'Air':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'Ocean':
                iconPath = "./assets/images/misc/ship.svg";
                break;

            case 'Land':
                iconPath = "./assets/images/misc/Truck.svg";
                break;

            default:
                break;
        }

        return iconPath;
    }
}
