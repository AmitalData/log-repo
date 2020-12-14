import { Component, ViewChild, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';


@Component({
    selector: 'ShipmentDetailsComponent',
    templateUrl: './ShipmentDetailsComponent.html',
    styleUrls: ['./ShipmentDetailsComponent.css']
})
export class ShipmentDetailsComponent implements AfterViewInit
{

    @ViewChild('SliderWrapper') SliderWrapperElement: ElementRef;



    isLoading: boolean = false;
    showMoreReferences: boolean = false;
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentList = null;
    SearchText: string = "";
    CustomersReferences = [
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
        '5689974987646132',
    ]
    get tenant()
    {
        return CargoTrackingBrandingData.Tenant;
    }
    constructor(private router: Router,
        private route: ActivatedRoute,
        private searchService: CargoTrackingSearchService)
    {

        this.GetIdFromURI();

    }
    ngAfterViewInit(): void
    {
        this.LoadShipment();

    }
    @HostListener('window:resize', ['$event'])
    onResize()
    {
        //event.target.innerWidth;
        this.InitSlider();
    }

    onMousewheel(event: WheelEvent)
    {
        event.preventDefault();
        if (event.deltaY > 0) {
            this.MoveSlider('left');
        }
        if (event.deltaY < 0) {
            this.MoveSlider('right');
        }
    }

    logPan(i)
    {
        console.log(i);

    }
    InitSlider()
    {

        var PAGERS_WIDTH = 200; // 100 * 2 pager 
        var screenwidth = window.innerWidth;

        var sliderWrapperWidth = this.SliderWrapperElement.nativeElement.offsetWidth;

        if (screenwidth > 470)
            var count = Math.floor((sliderWrapperWidth - PAGERS_WIDTH) / this.sliderCardWidth);


        this.sliderVisibleCardsCount = count;

        this.sliderVisibleCardsWidth = count * this.sliderCardWidth;
        this.sliderMarginCardCount = 0;
        this.sliderMarginLeft = screenwidth < 470 ? (this.sliderCardWidth + 55) * -1 : 0; // mobile: add 

    }
    LoadShipment()
    {
        this.isLoading = true;
        this.searchService.getShipment(this.SecurityKey, this.tenant).subscribe((result: any) =>
        {
            this.isLoading = false;
            console.log("[getShipment]", result);
            this.Shipment = result;

            setTimeout(() =>
            {
                this.InitSlider();
            }, 200);

        });
    }
    private GetIdFromURI()
    {

        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }
    SliderCards: any[] = [
        { Code: "", Date: new Date(2020, 11, 2), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
        { Code: "US-NYC", Date: new Date(2020, 11, 14), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
        { Code: "", Date: new Date(2020, 11, 18), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
        { Code: "", Date: new Date(2020, 11, 20), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
        { Code: "US-BOS", Date: new Date(2020, 11, 24), Title: "Boat ETA - Qalqilya", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
        { Code: "", Date: new Date(2020, 11, 26), Title: "Boat ETD - Haifa", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: true, IsDimmed: false },
        { Code: "", Date: new Date(2020, 11, 29), Title: "Shipping Certificate", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: true, HasWarning: true, IsDimmed: false },
        { Code: "", Date: new Date(2020, 11, 30), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: true },
    ];
    sliderMarginLeft: number = 0;
    sliderMarginCardCount: number = 0;
    sliderCardWidth: number = 200;
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;

    MoveSlider(dir)
    {
        var margin = this.sliderMarginLeft;

        // inc\dec
        if (dir == 'left') {
            margin -= this.sliderCardWidth;
            this.sliderMarginCardCount++;
        }
        else {
            margin += this.sliderCardWidth;
            this.sliderMarginCardCount--;
        }

        // limit boundary
        if (margin > 0)
            this.sliderMarginLeft = 0;
        else if (margin < this.sliderVisibleCardsWidth * -1)
            this.sliderMarginLeft = this.sliderVisibleCardsWidth;
        else
            this.sliderMarginLeft = margin;

        var screenwidth = window.innerWidth;
        if (screenwidth < 470)
            this.sliderMarginLeft - 55;

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



    selectedNavButton: string = "Overview";
    PanelsNavigatorClicked(panelName: string)
    {
        this.ScrollToPanel(panelName);
    }


    private ScrollToPanel(panelName: string)
    {
        this.selectedNavButton = panelName;
        var panelElement = document.getElementById(panelName) as HTMLElement;
        if (panelElement)
            panelElement.scrollIntoView();
    }

    BackLinkClicked()
    {
        this.router.navigate(['Cargo-Tracking', 'shipments']);
    }
}
