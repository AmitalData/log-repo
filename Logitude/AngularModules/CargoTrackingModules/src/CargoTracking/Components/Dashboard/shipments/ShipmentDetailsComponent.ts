import { Component, ViewChild, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized } from '@angular/router';
import { fromEvent } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';


@Component({
    selector: 'ShipmentDetailsComponent',
    templateUrl: './ShipmentDetailsComponent.html',
    styleUrls: ['./ShipmentDetailsComponent.css']
})
export class ShipmentDetailsComponent implements AfterViewInit
{

    @ViewChild('SliderWrapper') SliderWrapperElement : ElementRef;


    tenant;
    isLoading: boolean  = false;
    showMoreReferences: boolean  = false;
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentList = null;
    CustomersReferences = [
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
    ]

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private searchService: CargoTrackingSearchService)
    {
        this.GetVariablesFromURI();
        this.GetIdFromURI();
        this.LoadShipment();

    }
    ngAfterViewInit(): void
    {
    }
    @HostListener('window:resize', ['$event'])
    onResize(event) {
        //event.target.innerWidth;
        this.InitSlider();
    }

    InitSlider()
    {

        var sliderWrapperWidth = this.SliderWrapperElement.nativeElement.offsetWidth;
        var count = Math.floor((sliderWrapperWidth - 200)/this.sliderCardWidth);

        this.sliderVisibleCardsCount = count;

        this.sliderVisibleCardsWidth = count * this.sliderCardWidth;
        this.sliderMarginCardCount = 0;
        this.sliderMarginLeft = 0;

    }
    LoadShipment(){
        this.isLoading = true;
        this.searchService.getShipment(this.SecurityKey, this.tenant).subscribe((result: any) =>
        {
            this.isLoading = false;
            console.log("[getShipment]", result);
            this.Shipment = result;


            this.InitSlider();                       

        });
    }
    private GetIdFromURI()
    {      
        
        // var tenant = this.route.snapshot.paramMap.get('SecurityKey');
        if(this.tenant==null){
            this.tenant = Number(this.route.snapshot.parent.paramMap.get('Tenant'));
        }
        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }
    SliderCards: any[] = [
        {Code: "", Date: new Date(2020,11,2), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false},
        {Code: "US-NYC", Date: new Date(2020,11,14), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false},
        {Code: "", Date: new Date(2020,11,18), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false},
        {Code: "", Date: new Date(2020,11,20), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false},
        {Code: "US-BOS", Date: new Date(2020,11,24), Title: "Boat ETA - Qalqilya", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false},
        {Code: "", Date: new Date(2020,11,26), Title: "Boat ETD - Haifa", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: true, IsDimmed: false},
        {Code: "", Date: new Date(2020,11,29), Title: "Shipping Certificate", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: true, HasWarning: true, IsDimmed: false},
        {Code: "", Date: new Date(2020,11,30), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: true},
    ];
    sliderMarginLeft: number = 0;
    sliderMarginCardCount: number = 0;
    sliderCardWidth: number = 200;
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;

    MoveSlider(dir){
        var margin = this.sliderMarginLeft;

        // inc\dec
        if(dir == 'left')
        {
            margin -= this.sliderCardWidth;
            this.sliderMarginCardCount++;
        }
        else{
            margin += this.sliderCardWidth;
            this.sliderMarginCardCount--;
        }

        // limit boundary
        if(margin > 0)
            this.sliderMarginLeft = 0;
        else if(margin < this.sliderVisibleCardsWidth * -1)
            this.sliderMarginLeft = this.sliderVisibleCardsWidth;
        else
            this.sliderMarginLeft = margin;
        
    }
    GetModeIcon()
    {
        var iconPath = "";
        switch (this.Shipment.TransportModeId) {
            case 'A':
                iconPath = "./assets/images/misc/plane.svg";
                break;

            case 'O':
                iconPath = "./assets/images/misc/ship.svg";
            break;

            default:
                case 'L':
                iconPath = "./assets/images/misc/Truck.svg";
                break;

        }

        return iconPath;
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
    
    ItemClicked(item)
    {   var selection = window.getSelection();
        if(selection.toString().length === 0) {
            var SecurityKey = item.SecurityKey;

        this.router.navigate([this.tenant,'dashboard','shipment', SecurityKey]);
        }
    }

}
