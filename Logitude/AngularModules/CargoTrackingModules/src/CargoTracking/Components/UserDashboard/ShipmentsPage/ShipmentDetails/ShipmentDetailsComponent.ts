import {Component,ViewChild, ElementRef, AfterViewInit, HostListener, Input, EventEmitter, OnInit, Inject} from '@angular/core';
import { Router, ActivatedRoute, Data } from '@angular/router';

import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from 'src/CargoTracking/Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from 'src/CargoTracking/EntityLists/CargoTrackingShipmentList';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { CargoTrackingShipmentWithMilestones, Milestone } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { CargoTrackingPortService } from '../../../../Services/Others/CargoTrackingPortService';
import { CargoTrackingShipmentService } from '../../../../Services/Others/CargoTrackingShipmentService';
import { CargoTrackingShipmentCustomsData } from "../../../../DataContracts/CargoTrackingShipmentCustomsData";
import { DocumentDownloadService } from '../../../../Services/Others/DocumentDownloadService';
import { MessageWindowComponent } from '../../../../../Infrastructure/Components/MessageWindow/MessageWindowComponent';
import { CargoTrackingShipmentOrderService } from '../../../../Services/Others/CargoTrackingShipmentOrderService';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { CargoTrackingShipmentExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingShipmentExtendedService';
import { CargoTrackingShipmentMappedPM } from 'src/CargoTracking/DataContracts/CargoTrackingShipmentMappedPM';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';

const mobileScreenMaxWidth = 470;
@Component({
    selector: 'ShipmentDetailsComponent',
    templateUrl: './ShipmentDetailsComponent.html',
    styleUrls: ['./ShipmentDetailsComponent.css',
                '../../UserDashboardComponent.css']
})
export class ShipmentDetailsComponent implements OnInit,AfterViewInit
{

    @ViewChild('SliderWrapper') SliderWrapperElement: ElementRef;
    @ViewChild('slider') SliderElement: ElementRef;
    @ViewChild('RoutingSliderWrapper') RoutingSliderWrapperElement: ElementRef;


    @Input() DetailsSectionToggleEvent: EventEmitter<any> = new EventEmitter();

    cargoTrackingShipmentPM: CargoTrackingShipmentMappedPM = new CargoTrackingShipmentMappedPM();

    public isLoading: boolean = true;
    showMoreReferences: boolean = false;
    SecurityKey: string = "";
    Shipment: CargoTrackingShipmentWithMilestones = null;
    public ShipmentWithMilestones: CargoTrackingShipmentWithMilestones;
    public toPortCode: string;
    public fromPortCode: string;
    ShipmentReferences: string[] = [];
    SearchText: string = "";
    ShipmentPM: any ;
    ShipmentOrder: any ;
    HasReferences: boolean = false;
    HasContainersDetails: boolean = false;
    InlandTransportMode = 'I';
    WarehouseTransportMode ='W'
    OceanTransportMode = 'O';
    AirTransportMode = 'A';
    ContainersNumbers: string[] = [];
    ShowDetailsSection: boolean = false;
    TitleOfCustomsOrForwarder: string = "";
    ValueOfCustomsOrForwarder: string = "";
    CustomsEntityType: string = "C";
    ForwardingEntityType: string = "F";
    OrderEntityType: string = "O";
    RoutingPagersWidth : number = 100;
    RoutingMobilePagersWidth : number = 150;
    MaxWidthForMobileScreenForRouting: number = 470;
    RoutingMobileMarginLeft: number = 55;

    NoTaxDetails: boolean = false;
    public OverviewPanelTitle: string;
    public TypeTitle: string;
    isSharedLink: boolean = false;
    noShipmentFound: boolean = false;
    PartnerCardTypesOfShipmentTransportMode = {
        'A': "AIRLINES",
        'I': "TRUCKER",
        'O': "SHIPPING LINES"
    }
    EntityType_Customs = "C";
    _tenant;
    focusOnPanel;
    baseURL;
    isBrandingDataLoaded;

    get tenant()
    {
        return this._tenant || CargoTrackingBrandingData.Tenant;
    }
    constructor(private router: Router,
        private route: ActivatedRoute,
        private searchService: CargoTrackingSearchService,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService,
        private cargoTrackingShipmentOrderService: CargoTrackingShipmentOrderService,
        private cargoTrackingShipmentExtendedService: CargoTrackingShipmentExtendedService,
        private brandingService: CargoTrackingBrandingDataExtendedService,
        private documentDownloadService: DocumentDownloadService,
        public dialog: MatDialog,
        private datePipe: DatePipe,
        @Inject('BASE_URL') baseUrl: string)
    {
        this.baseURL = baseUrl;

        this.GetIdFromURI();

    }
    ngOnInit(): void
    {

    }
    ngAfterViewInit(): void
    {
        this.LoadCargoShipmentPM();
    }


    private GetIdFromURI()
    {

        let _id = this.route.snapshot.paramMap.get('SecurityKey');
        this.SecurityKey = _id;

        this.route.queryParams.subscribe(params => {
            this.SecurityKey = params['SecurityKey'] || this.SecurityKey;
            this._tenant = params['Tenant'];
            this.focusOnPanel = params['Panel'];
        });

        const data: Data = this.route.snapshot.data;
        this.isSharedLink = data?.isSharedLink;
        if(this.isSharedLink){
            this.GetBrandingData();
        }
    }


    LoadCargoShipmentPM()
    {
        if(this.isSharedLink){
            this.GetMainShipmentByShipmentSecurityKey();
        }else{
            this.GetShipmentBySecurityKey();
        }

    }

    private GetShipmentBySecurityKey()
    {
        this.cargoTrackingShipmentExtendedService.GetCargoShipmentPMBySecurityKey(this.SecurityKey, this.tenant)
            .subscribe((result: any) =>
            {
                this.isLoading = false;

                console.log("GetCargoShipmentPMBySecurityKey", this.ShipmentPM);
                if (result) {
                    this.InitializeComponent(result);
                } else {
                    this.noShipmentFound = true;
                }
            }, (error) =>
            {
                this.noShipmentFound = true;
                this.isLoading = false;

            });
    }

    private GetMainShipmentByShipmentSecurityKey()
    {
        this.cargoTrackingShipmentExtendedService.GetMainCargoShipmentPMBySecurityKey(this.SecurityKey, this.tenant)
            .subscribe((result: any) =>
            {
                this.isLoading = false;

                console.log("GetMainCargoShipmentPMBySecurityKey", this.ShipmentPM);
                if (result) {
                    this.InitializeComponent(result);
                } else {
                    this.noShipmentFound = true;
                }
            }, (error) =>
            {
                this.noShipmentFound = true;
                this.isLoading = false;

            });
    }

    private InitializeComponent(result: any)
    {
        this.cargoTrackingShipmentPM = result;
        this.ShipmentReferences = this.cargoTrackingShipmentPM.CustomerReference ? this.cargoTrackingShipmentPM.CustomerReference.split(',') : null;


        this.SetCustomsOrForwarderFields();
        this.SetOverviewPanelTitle();
        this.SetTypeTitle();

        this.SetContainersNumbers(result);
        this.SetHasContainersDetails();

        this.NoTaxDetails = this.cargoTrackingShipmentPM?.CustomsData?.TaxDetails?.length == 0 ? true : false;


        setTimeout(() =>
        {
            this.InitSlider();
            this.InitRoutingSlider();
            this.BuildSliderCards();

            if (this.isSharedLink && this.focusOnPanel)
                this.ScrollToPanel(this.focusOnPanel);

        }, 200);
    }

    private GetBrandingData()
    {
        this.brandingService
            .GetUserDashboardBrandingData(ServiceHelper.GetcargoTrackingDataRequest(this.baseURL))
            .subscribe((response: ServiceResponse) =>
            {
                if (response.Result) {
                    if (response?.Result?.ForceHttps)
                        this.RedirectAppToHttps();

                    ServiceHelper.SetCargoTrackingDate(response.Result, this.baseURL);

                    this.isBrandingDataLoaded = true;
                }
            });
    }
    RedirectAppToHttps(){
        const isLocally = window.location.origin.indexOf('localhost') > -1;

        if (!isLocally && location.protocol === 'http:') {
            window.location.href = location.href.replace('http', 'https');
        }
    }
    private SetContainersNumbers(result: any) {
        this.ContainersNumbers = [];
        this.ContainersNumbers = result.ContainersNumbers ? result.ContainersNumbers.split(',') : null;
    }
    get CompanyLogo()
    {
        return CargoTrackingBrandingData.ComapnylogoURL;
    }
    get BrowserIcon()
    {
        return CargoTrackingBrandingData.BrowserIconURL;
    }
    get BackGroundImg()
    {
        return CargoTrackingBrandingData.BackgroundURL;
    }
    get ShipmentHeaderImage(){
        return CargoTrackingBrandingData.ShipmentHeaderURL;
    }

    public get InvertedLogoURL()
    {
        return CargoTrackingBrandingData.InvertedLogoURL;
    }


    private SetOverviewPanelTitle() {
        if (this.cargoTrackingShipmentPM.EntityType == this.OrderEntityType) {
            this.OverviewPanelTitle = "Order Overview";

        }
        else {
            this.OverviewPanelTitle = "Overview";
        }
    }
    @HostListener('window:resize', ['$event'])
    onResize()
    {
        //event.target.innerWidth;
        this.InitSlider();
        this.InitRoutingSlider();

    }

    onMousewheelOnMilestonesSlider(event: WheelEvent)
    {
        event.preventDefault();
        if (event.deltaY > 0) {
            this.MoveSlider('left');
        }
        if (event.deltaY < 0) {
            this.MoveSlider('right');
        }
    }

    onMousewheelOnRoutingSlider(event: WheelEvent)
    {
        event.preventDefault();
        if (event.deltaY > 0) {
            this.MoveRoutingSlider('left');
        }
        if (event.deltaY < 0) {
            this.MoveRoutingSlider('right');
        }
    }

    logPan(i)
    {
        console.log(i);

    }
    InitSlider()
    {
        if(!this.SliderWrapperElement)
            return;

        this.SetSliderVisibleCardsWrapperWidth();

        this.sliderMarginCardCount = 0;
        this.sliderMarginLeft = 0;
    }

    private SetSliderVisibleCardsWrapperWidth()
    {
        const webPagersWidth = 200;
        const mobilePagersWidth = 60;

        const sliderWrapperWidth = this.SliderWrapperElement.nativeElement.offsetWidth;

        const extraOffset = 20;
        if (this.IsMobileView)
            var mobileCount = Math.floor((sliderWrapperWidth - mobilePagersWidth - extraOffset) / this.sliderMobileCardWidth);

        else
            var webCount = Math.floor((sliderWrapperWidth - webPagersWidth) / this.sliderCardWidth);

        this.sliderVisibleCardsCount = mobileCount || webCount;

        if(this.IsMobileView){
            const extraOffset = 10;
            this.sliderVisibleCardsWidth = this.sliderMobileCardWidth * this.sliderVisibleCardsCount + extraOffset;
        }else{
            this.sliderVisibleCardsWidth = this.sliderVisibleCardsCount * this.sliderCardWidth;
        }
    }

    InitRoutingSlider() {
        var screenwidth = window.innerWidth;
        var sliderWrapperWidth = this.RoutingSliderWrapperElement.nativeElement.offsetWidth;
        var count = this.CalculateRoutingSliderVisibleCardsWidth(screenwidth, sliderWrapperWidth);

        this.routingSliderVisibleCardsWidth = count * this.routingSliderCardWidth;
        this.routingSliderMarginCardCount = 0;
        this.routingSliderMarginLeft =  0;

    }

    private CalculateRoutingSliderVisibleCardsWidth(screenwidth: number, sliderWrapperWidth: any) {
        if (screenwidth > this.MaxWidthForMobileScreenForRouting)
            var count = this.CalculateVisibleSliderCardsCountForWeb(sliderWrapperWidth);

        if (screenwidth <= this.MaxWidthForMobileScreenForRouting)
            var mobileCount = this.CalculateVisibleSliderCardsCountForMobile(sliderWrapperWidth, this.RoutingMobilePagersWidth, this.routingSliderMobileCardWidth);
        this.routingSliderVisibleCardsCount = count == undefined ? mobileCount : count;
        return count;
    }

    private CalculateVisibleSliderCardsCountForWeb(sliderWrapperWidth: any) {
        return Math.floor((sliderWrapperWidth - this.RoutingPagersWidth) / this.routingSliderCardWidth);
    }

    private CalculateVisibleSliderCardsCountForMobile(sliderWrapperWidth: number, mobilePagersWidth: number, sliderMobileCardWidth: number ) {
        return Math.floor((sliderWrapperWidth - mobilePagersWidth) / sliderMobileCardWidth);
    }


    SetTitleForSupplierOrClient(shipment: any) {
        var title;
        switch (shipment.DirectionId) {
            case ShipmentDirections.Import:{
                title = "SHIPPER"
                break;
            }

            case ShipmentDirections.Export: {
                title = "CLIENT"
                break;
            }
        }

        if (shipment.EntityType == this.EntityType_Customs) {
            title = "SHIPPER"
        }

        return title;
    }

    SetSupplierOrCleintValueByDirection(shipmentList) {
        if(shipmentList.DirectionId == ShipmentDirections.Import || shipmentList.EntityType == ShipmentDirections.Customs)
        {
            return shipmentList.ShipperName;
        } else if(shipmentList.DirectionId == ShipmentDirections.Export) {
            return shipmentList.ConsigneeName;
        }
        return '';
    }
    SetTypeTitle() {
        if (this.cargoTrackingShipmentPM.TransportModeId == "A") {
            this.TypeTitle = "PACKAGE TYPE";

        }
         if (this.cargoTrackingShipmentPM.TransportModeId != "A") {
            this.TypeTitle = "CONTAINER TYPE";
        }
         if (this.cargoTrackingShipmentPM.EntityType == 'O') {
            this.TypeTitle = "SHIPMENT TYPE";
        }
    }
    SetHasContainersDetails() {
        return this.cargoTrackingShipmentPM.Packages.length==0  ? false : true;
    }


    SetCustomsOrForwarderFields() {
        this.SetTitleOfCustomsOrForwarder();
        this.SetValueOfCustomsOrForwarder();
    }

    SetTitleOfCustomsOrForwarder() {
        if (this.cargoTrackingShipmentPM.EntityType == this.CustomsEntityType) {
            this.TitleOfCustomsOrForwarder = "Customs Broker References";
        }
        if (this.cargoTrackingShipmentPM.EntityType == this.ForwardingEntityType) {
            this.TitleOfCustomsOrForwarder = "Forwarder Reference";
        }
        if (this.cargoTrackingShipmentPM.EntityType == this.OrderEntityType) {
            this.TitleOfCustomsOrForwarder = "Order References";
        }
    }

    SetValueOfCustomsOrForwarder() {
        if (this.cargoTrackingShipmentPM.EntityType == this.ForwardingEntityType) {
            this.ValueOfCustomsOrForwarder = this.cargoTrackingShipmentPM.ShipmentNumber;
        }

        if (this.cargoTrackingShipmentPM.EntityType == this.CustomsEntityType || this.cargoTrackingShipmentPM.EntityType == this.OrderEntityType) {
            var ForwardingShipmentNumber = this.cargoTrackingShipmentPM.ForwardingShipmentHeaderId != null ? this.cargoTrackingShipmentPM.ForwardingShipmentNumber != null ? "\n" + this.cargoTrackingShipmentPM.ForwardingShipmentNumber : "": "";
            this.ValueOfCustomsOrForwarder = this.cargoTrackingShipmentPM.ShipmentNumber + ForwardingShipmentNumber;
        }

    }

    //#region Slider
    SliderCards: MilestoneCard[] = [];
    sliderMarginLeft: number = 0;
    sliderMarginCardCount: number = 0;
    sliderCardWidth: number = 164;
    sliderMobileCardWidth: number = 154;
    sliderVisibleCardsCount: number = 5;
    sliderVisibleCardsWidth: number = 0;
    NoMilstonesFound: boolean = false;
    BuildSliderCards()
    {
        if (!this.cargoTrackingShipmentPM.Milestones)
            return;
        this.SliderCards = this.cargoTrackingShipmentPM.Milestones
            .filter(milstone =>
            {
                var date = milstone.EstimationDate || milstone.Date;
                if (date)
                    return true;
                return false;
            })
            .sort((a, b) =>
            {
                if (a.Id > b.Id) return 1;
                if (a.Id < b.Id) return -1;
                return 0;
            })
            .map((milstone: Milestone) =>
            {
                var newCard = new MilestoneCard();
                var CurrentMilestoneExceptions= this.cargoTrackingShipmentPM.CurrentMilestoneExceptions;
                newCard.Date = milstone.Done ? milstone.Date : (milstone.EstimationDate || milstone.Date);
                newCard.Code = 'No. ' + milstone.Id;
                newCard.Title = milstone.Name;
                newCard.Description = milstone.Notes;
                newCard.IsDimmed = milstone.IsEstimation && !milstone.Done;
                newCard.IsActive = milstone.Id + '' == this.cargoTrackingShipmentPM.CurrentMilestoneCode;
                newCard.HasWarning = milstone.IsCurrent && CurrentMilestoneExceptions != null;
                newCard.WarningMessage = newCard.HasWarning ? CurrentMilestoneExceptions.substring(CurrentMilestoneExceptions.indexOf(',')+1,) : null;
                newCard.WarningDate = newCard.HasWarning ? this.datePipe.transform(CurrentMilestoneExceptions?.split(',')[0], 'dd/MM/yyyy, HH:mm'): null;
                return newCard;
            });
        this.SetNoMilstonesFound();

        this.ScrollIntoLastSliderCard();


    }
    ScrollIntoLastSliderCard()
    {
        const hiddenCardsCount = this.SliderCards.length - this.sliderVisibleCardsCount;
        const width = this.IsMobileView ? this.sliderMobileCardWidth : this.sliderCardWidth;

        this.sliderMarginLeft = hiddenCardsCount * width * -1;
        this.sliderMarginCardCount = hiddenCardsCount;

    }
    SetNoMilstonesFound() {
        if (this.SliderCards.length == 0) this.NoMilstonesFound = true;
    }
    MoveSlider(dir)
    {

        if (dir == 'right' && this.sliderMarginLeft == 0)
            return;

        if (dir == 'left' && ((this.sliderMarginCardCount + this.sliderVisibleCardsCount) >= this.SliderCards.length) || (this.sliderVisibleCardsCount >= this.SliderCards.length))
            return;

        let margin = this.sliderMarginLeft;
        const cardWidth = this.IsMobileView ? this.sliderMobileCardWidth : this.sliderCardWidth

        // inc\dec
        if (dir == 'left') {
            margin -= cardWidth;
            this.sliderMarginCardCount++;
        }
        else {
            margin += cardWidth;
            this.sliderMarginCardCount--;
        }

        // limit boundary
        if (margin > 0)
            this.sliderMarginLeft = 0;
        else
            this.sliderMarginLeft = margin;

        console.log("sliderMarginCardCount:sliderMarginLeft === ",this.sliderMarginCardCount , '\t' , this.sliderMarginLeft);



    }

    get IsMobileView(){ return window.innerWidth <= mobileScreenMaxWidth; }
    get IsNotMobileView(){ return window.innerWidth > mobileScreenMaxWidth; }

    //#endregion

    //#region Routing Slider
    routingSliderMarginLeft: number = 0;
    routingSliderMarginCardCount: number = 0;
    routingSliderCardWidth: number = 200;
    routingSliderMobileCardWidth: number = 320;
    routingSliderVisibleCardsCount: number = 1;
    routingSliderVisibleCardsWidth: number = 0;


    MoveRoutingSlider(direction) {

        if (direction == 'right' && this.routingSliderMarginLeft == 0)
            return;

        if (direction == 'left' && ((this.routingSliderMarginCardCount + this.routingSliderVisibleCardsCount) >= this.cargoTrackingShipmentPM.RoutingSteps.length) || (this.routingSliderVisibleCardsCount >= this.cargoTrackingShipmentPM.RoutingSteps.length))
            return;

        var margin = this.SetRoutingSliderMarginBasedOnDirection(direction);

        this.SetRoutingSliderMarginLeft(margin);

        var screenwidth = window.innerWidth;
        if (screenwidth < this.MaxWidthForMobileScreenForRouting)
            this.routingSliderMarginLeft - this.RoutingMobileMarginLeft;


    }
    private SetRoutingSliderMarginLeft(margin: number) {
        if (margin > 0)
            this.routingSliderMarginLeft = 0;

        else
            this.routingSliderMarginLeft = margin;
    }

    private SetRoutingSliderMarginBasedOnDirection(direction: any) {
        var margin = this.routingSliderMarginLeft;

        if (direction == 'left') {
            margin -= this.routingSliderCardWidth;
            this.routingSliderMarginCardCount++;
        }
        else {
            margin += this.routingSliderCardWidth;
            this.routingSliderMarginCardCount--;
        }
        return margin;
    }

    //#endregion

    GetModeIcon()
    {
        var iconPath = "";
        switch (this.cargoTrackingShipmentPM.TransportModeId) {
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
        if (panelElement){
            panelElement.scrollIntoView();
            document.getElementsByTagName('html')[0].scrollTop -= 103;

        }


    }

    BackLinkClicked()
    {
        this.router.navigate(['cargo-tracking', 'shipments']);
    }
    ExternalDownloadAllClick(securityKey: string) {
        this.documentDownloadService.ExternalDownloadAllDocuments(securityKey,this.tenant);
    }
    ExternalDownloadDocument(securityId: string) {
        this.documentDownloadService.ExternalDownloadPage(securityId,this.tenant);
    }

    DownloadDocument(documentId: string, documentTypeName: string) {
        if (documentId)
            this.documentDownloadService.DownloadPage(documentId, this.cargoTrackingShipmentPM.ShipmentNumber + '-' + documentTypeName);
    }

    DownloadAllClick(entityId: string, securityKey: string) {
        this.documentDownloadService.DownloadAllPages(entityId, securityKey);
    }

    ShowMoreLinkClicked() {
        this.ShowDetailsSection = !this.ShowDetailsSection;
        this.DetailsSectionToggleEvent.emit();
    }

    OpenMessageWindow(messageDescription, messageDate) {
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'Exception',
                date: messageDate,
                description: messageDescription,
            }
        });
    }

    OpenReferencesWindow(references) {
        references = references.map(x => x.trim());
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'References',
                description: references.join("\n"),
            }
        });
    }

    ShowMoreWindow(text) {
        console.log(text)
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: '',
                description: text,
            }
        });
    }

    ChangeShowDetailsSectionValue(newValue) {
        this.ShowDetailsSection = newValue;
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
    WarningMessage: string;
    WarningDate: string;
}

export enum ShipmentDirections {
    Import = "I",
    Export = "E",
    Customs = "C"
}
