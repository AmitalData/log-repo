import { Component,
     ViewChild, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { CargoTrackingShipmentWithMilestones, Milestone } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { CargoTrackingPortService } from '../../../../Services/Others/CargoTrackingPortService';
import { CargoTrackingShipmentService } from '../../../../Services/Others/CargoTrackingShipmentService';


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
    Shipment: CargoTrackingShipmentWithMilestones = null;
    public ShipmentWithMilestones: CargoTrackingShipmentWithMilestones;
    public toPortCode: string;
    public fromPortCode: string;
    isFromPortCodeFilled: boolean = false;
    SearchText: string = "";
    ShipmentReferences: string[] = [];
    CustomsBrokerReference: string;
    ShipmentPM: any;


    get tenant()
    {
        return CargoTrackingBrandingData.Tenant;
    }
    constructor(private router: Router,
        private route: ActivatedRoute,
        private searchService: CargoTrackingSearchService,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService
  )
    {

        this.GetIdFromURI();
        this.LoadShipment();

    }
    ngAfterViewInit(): void
    {
        setTimeout(() => {
            this.InitSlider();
            this.BuildSliderCards();

        }, 200);
        this.InitRoutes();

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
            console.log("[getShipment]", result);
            this.ShipmentWithMilestones = result;
            if (this.ShipmentWithMilestones) {
                this.Shipment = result;
                this.ShipmentReferences = result.ShipmentList.CustomerReference ? result.ShipmentList.CustomerReference.split(',') : null;
                this.SetRoutingVariables();
                this.GetShipmentPM();
            }

        });
    }

    SetRoutingVariables() {
       this.GetCargoTrackingPortById(this.Shipment.ShipmentList.FromPortId);
    }

    GetCargoTrackingPortById(id: string) {
        this.cargoTrackingPortService.get(id).subscribe((result: any) => {
            var code = result.Code;
            this.SetFromPortCodeORToPortCode(code);
        })
    }

    private SetFromPortCodeORToPortCode(code: any) {

        if (!this.isFromPortCodeFilled) {
            this.fromPortCode = code;
            this.isFromPortCodeFilled = true;
        }

        else
            this.toPortCode = code;
       this.GetCargoTrackingPortById(this.Shipment.ShipmentList.ToPortId);

    }


    GetShipmentPM() {
        this.cargoTrackingShipmentService.get(this.Shipment.ShipmentList.EntityId).subscribe((result: any) => {
            if (result) {
                this.ShipmentPM = result;
                this.CustomsBrokerReference = result.CustomFileNumber;
                this.isLoading = false;
            }
        });
    }


    private GetIdFromURI()
    {

        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;
        return _id;
    }
    SliderCards: MilestoneCard[] = [];

    // [
    //     { Code: "", Date: new Date(2020, 11, 2), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
    //     { Code: "US-NYC", Date: new Date(2020, 11, 14), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
    //     { Code: "", Date: new Date(2020, 11, 18), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
    //     { Code: "", Date: new Date(2020, 11, 20), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
    //     { Code: "US-BOS", Date: new Date(2020, 11, 24), Title: "Boat ETA - Qalqilya", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: false },
    //     { Code: "", Date: new Date(2020, 11, 26), Title: "Boat ETD - Haifa", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: true, IsDimmed: false },
    //     { Code: "", Date: new Date(2020, 11, 29), Title: "Shipping Certificate", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: true, HasWarning: true, IsDimmed: false },
    //     { Code: "", Date: new Date(2020, 11, 30), Title: "Order file printed", Description: "Lorem ipsum dolor sit amet, consectetur", IsActive: false, HasWarning: false, IsDimmed: true },
    // ];
    sliderMarginLeft: number = 0;
    sliderMarginCardCount: number = 0;
    sliderCardWidth: number = 200;
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;

    BuildSliderCards(){
        // this.Shipment.Milestones.forEach((milstone:Milestone) => {
        //     var newCard = new MilestoneCard();
        //     newCard.Code = milstone.Code;
        //     newCard.Date = milstone.EstimationDate || milstone.Date;
        //     newCard.Title = milstone.Name;
        //     newCard.Description = milstone.Notes;
        //     newCard.IsDimmed = milstone.IsEstimation;
        //     newCard.IsActive = milstone.Code == this.Shipment.ShipmentList.CurrentMilestoneCode;
        //     this.SliderCards.push(newCard);
        // });

        this.SliderCards = this.Shipment.Milestones
        .filter(milstone=>{
            var date = milstone.EstimationDate || milstone.Date;
            if(date)
                return true;
            return false;
        })
        .sort((a, b) => {
            if (a.Id > b.Id) return 1;
            if (a.Id < b.Id) return -1;
             return 0;
            })
        .map((milstone:Milestone) => {
            var newCard = new MilestoneCard();
            newCard.Date =  milstone.Done ? milstone.Date : (milstone.EstimationDate || milstone.Date);
            newCard.Code = 'No. '+milstone.Id;
            newCard.Title = milstone.Name;
            newCard.Description = milstone.Notes || 'This milestone does not have descriptions';
            newCard.IsDimmed = milstone.IsEstimation && !milstone.Done;
            newCard.IsActive = milstone.Id + '' == this.Shipment.ShipmentList.CurrentMilestoneCode;
            newCard.HasWarning = newCard.IsActive;
            return newCard;
        });
        // .sort((a, b) => {
        //     if (a.Date > b.Date) return 1;
        //     if (a.Date < b.Date) return -1;
        //      return 0;
        //     });
    }

    MoveSlider(dir)
    {

        if(dir == 'right' && this.sliderMarginLeft==0)
        return;

        if(dir == 'left' && ((this.sliderMarginCardCount+this.sliderVisibleCardsCount)>=this.SliderCards.length) || (this.sliderVisibleCardsCount >= this.SliderCards.length))
            return;

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
        switch (this.Shipment.ShipmentList.TransportModeId) {
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
        this.router.navigate(['cargo-tracking', 'shipments']);
    }

    ShipmentRouteSteps: RoutingStep[] = [];
    InitRoutes(){
        var step1 = new RoutingStep();
        step1.FromPortLabel = "US-BOS";
        step1.ToPortLabel = "US-NYC";
        step1.Description = "Via lorem ipsum co.";
        step1.TransportModeCode = "A";
        step1.Directions = [
            new RouteDirection(new Date(),"ATA","in"),
            new RouteDirection(new Date(),"ETD","out"),
        ];


        var step2 = new RoutingStep();
        step2.FromPortLabel = "US-QAL";
        step2.ToPortLabel = "US-NYC";
        step2.Description = "Via lorem ipsum co.";
        step2.TransportModeCode = "I";
        step2.Directions = [
            new RouteDirection(new Date(),"ATA","in"),
            new RouteDirection(new Date(),"ETD","out"),
        ];

        var step3 = new RoutingStep();
        step3.FromPortLabel = "US-QAL";
        step3.ToPortLabel = "US-NAB";
        step3.Description = "Rafedia main st.";
        step3.TransportModeCode = "O";
        step3.IsActive = true;
        step3.Directions = [
            new RouteDirection(new Date(),"ATA","in"),
            new RouteDirection(new Date(),"ATD","in"),
        ];

        this.ShipmentRouteSteps =  [step1,step2,step3];
    }
}


export class MilestoneCard
{
    Code: string;
    Date: Date;
    Title: string;
    Description: string;
    IsActive: boolean;
    HasWarning: boolean;
    IsDimmed: boolean;
}

export class RoutingStep
{
    IsActive: boolean;
    FromPortLabel;
    ToPortLabel;
    Description: string;
    TransportModeCode: 'A' | 'I' | 'O';
    Directions: RouteDirection[] = [];
}
export class RouteDirection{
    constructor(date: Date,label: string,direction: 'in' | 'out') {
        this.Date = date;
        this.Label = label;
        this.Direction = direction;
    }
    Date: Date;
    Label: string;
    Direction: 'in' | 'out' = 'in';
}
